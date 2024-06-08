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

    private List<GameObject> roomInstances = new List<GameObject>();

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
            GameObject room = result.room.Generate(parameters);
            Vector3 origin = room.transform.Find("Points/StartPoint").position;
            room.transform.position = cumulativeOffset;
            Debug.Log("Placing room at " + room.transform.position);

            Vector3 attach = room.transform.Find("Points/EndPoint").position;
            cumulativeOffset = attach - origin;
            Debug.Log("Next offset is " + cumulativeOffset);

            roomInstances.Add(room);
        }
    }
}
