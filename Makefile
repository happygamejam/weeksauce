.PHONY: validate
check-env:
ifndef UNREAL_PATH
	$(error UNREAL_PATH must be set.)
endif

.PHONY: build-dev
build-dev: check-env
	$(UNREAL_PATH)/Engine/Build/BatchFiles/Linux/Build.sh weeksauceEditor Linux Development -project=$(PWD)/weeksauce.uproject

