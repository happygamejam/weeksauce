#pragma once

#include "Dungeon.h"
#include "DungeonParameters.h"


/**
 * 
 */
class UDungeonGenerator
{
public:
	UDungeon generate(FDungeonParameters* params);
	UDungeonGenerator() = default;
	~UDungeonGenerator() = default;
private:
	URoom* last;
};
