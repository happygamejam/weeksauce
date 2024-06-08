using UnityEngine;

public abstract class Room : MonoBehaviour
{
   public abstract GameObject Generate(DungeonParameters parameters);
}

[System.Serializable]
public struct WeightedRoom : IWeighted {
    public Room room;
    public float Weight;

    float IWeighted.Weight => Weight;
}
