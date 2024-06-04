#pragma once

#include "Containers/Array.h"
#include "Room.h"
#include "Templates/UniquePtr.h"
#include "Dungeon.generated.h"

UCLASS()
class UDungeon: public UObject
{
	GENERATED_BODY()
private:
	TArray<TUniquePtr<URoom>> rooms;
public:
	void Add(TUniquePtr<URoom> room);
	UDungeon() = default;
	~UDungeon() = default;
};
