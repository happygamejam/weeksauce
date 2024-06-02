.PHONY: validate
check-env:
ifndef UNREAL_PATH
	$(error UNREAL_PATH must be set.)
endif

PLATFORM ?= Linux
ENVIRONMENT ?= Development

_BUILD_BASE=$(UNREAL_PATH)/Engine/Build/BatchFiles/Linux/Build.sh \
		-project=$(PWD)/weeksauce.uproject \
		-game -engine \
		weeksauceEditor $(PLATFORM) $(ENVIRONMENT)

.PHONY: build
build: check-env
	$(_BUILD_BASE)

.PHONY: gen-clangd
gen-clangd: check-env
	$(_BUILD_BASE) -mode=GenerateClangDatabase && \
	cp $(UNREAL_PATH)/compile_commands.json $(PWD)/compile_commands.json

.PHONY: lint
lint: gen-clangd
	find $(PWD)/Source -name "*.cpp" -or -name "*.h" | xargs -I {} clang-tidy -p=compile_commands.json -quiet {}

.PHONY: lint-ci
lint-ci:
	$(MAKE) lint 2>&1 | tee clang-tidy-output.log
