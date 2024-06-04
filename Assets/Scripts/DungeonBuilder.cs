using System.Collections.Generic;
using UnityEngine;


public class DungeonBuilder : MonoBehaviour
{
    [SerializeField]
    private DungeonParameters dungeonParameters;
    [SerializeField]
    private List<WeightedRoom> rooms;

    void Start()
    {
        Debug.Assert(dungeonParameters != null, "DungeonParameters is not set in DungeonBuilder");
        Generate(dungeonParameters);
    }

    private void Generate(DungeonParameters parameters)
    {
        var weightedRooms = new WeightedList<WeightedRoom>(rooms, parameters.seed);
        float cumulativeOffset = 0;
        for (int i = 0; i < parameters.roomCount; i++)
        {
            WeightedRoom result = weightedRooms.Pick();
            GameObject room = result.room.Generate();
            room.transform.position = new Vector3(cumulativeOffset, 0, 0);
            cumulativeOffset += result.room.Offset;
        }
    }
}
