#pragma once

#include "UObject/ObjectMacros.h"
#include "DungeonParameters.generated.h"


USTRUCT(BlueprintType)
struct FDungeonParameters
{
	GENERATED_BODY()

	UPROPERTY()
	int32 RoomCount{0};
};
