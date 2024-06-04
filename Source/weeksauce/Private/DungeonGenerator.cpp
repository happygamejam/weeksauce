// Fill out your copyright notice in the Description page of Project Settings.


#include "DungeonGenerator.h"
#include "Dungeon.h"
#include "DungeonParameters.h"

UDungeon* UDungeonGenerator::generate(FDungeonParameters* params) {
    UDungeon* dungeon = NewObject<UDungeon>();
    
    return dungeon;
}

