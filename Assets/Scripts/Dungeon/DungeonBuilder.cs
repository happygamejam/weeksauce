using System.Collections.Generic;
using UnityEngine;


public class DungeonBuilder : MonoBehaviour
{
    [SerializeField]
    private DungeonParameters dungeonParameters;
    [SerializeField]
    private List<WeightedRoom> rooms;
    [SerializeField]
    private PlayerSpawner playerSpawner;

    private List<Room> roomInstances = new List<Room>();
    private int roomIndex = -1;

    private void OnEnable() {
        var dungeonParameters = DungeonManager.ActiveDungeon;
        if (dungeonParameters == null) {
            dungeonParameters = this.dungeonParameters;
        }

        Debug.Assert(dungeonParameters != null, "DungeonParameters is not set in DungeonBuilder");
        Generate(dungeonParameters);

        Debug.Assert(playerSpawner != null, "PlayerSpawner is not set in DungeonBuilder");
        playerSpawner.SpawnPlayer(roomInstances[0]);
    }

    private void Generate(DungeonParameters parameters)
    {
        var weightedRooms = new WeightedList<WeightedRoom>(rooms, parameters.seed);
        Vector3 cumulativeOffset = Vector3.zero;
        for (int i = 0; i < parameters.roomCount; i++)
        {
            WeightedRoom result = weightedRooms.Pick();
            var room = result.room;
            room.SetPlayerSpawner(playerSpawner);

            room.Generate(parameters);
            Vector3 origin = room.GameObject.transform.Find("Points/StartPoint").position;
            room.GameObject.transform.position = cumulativeOffset;
            Debug.Log("Placing room at " + room.GameObject.transform.position);

            Vector3 attach = room.GameObject.transform.Find("Points/EndPoint").position;
            cumulativeOffset = attach - origin;
            Debug.Log("Next offset is " + cumulativeOffset);

            // If not the last room, we need to delete some elements to allow for seamless chaining
            if (i < parameters.roomCount - 1) {
                var objs = new List<GameObject>();
                GameObject.FindGameObjectsWithTag("ChainDelete", objs);
                foreach (GameObject obj in objs) {
                    Destroy(obj);
                }
            }

            roomInstances.Add(room);
        }
    }
}
