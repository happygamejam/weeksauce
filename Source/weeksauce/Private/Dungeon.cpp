#include "Dungeon.h"
#include "Room.h"
#include "Templates/UniquePtr.h"

void UDungeon::Add(TUniquePtr<URoom> room) {
    rooms.Add(MoveTemp(room));
}
