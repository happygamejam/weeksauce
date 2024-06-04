using UnityEngine;

public abstract class Room : MonoBehaviour
{
   // Offset for the next room.
   // This is the size of the room in the axis of the doors
   private float offset;
   public float Offset => offset;

   public abstract GameObject Generate();

   protected Room(float offset) {
      this.offset = offset;
   }
}

[System.Serializable]
public struct WeightedRoom : IWeighted {
    public Room room;
    public float Weight;

    float IWeighted.Weight => Weight;
}
