using UnityEngine;

public abstract class Room : MonoBehaviour
{
   protected GameObject room;
   public GameObject GameObject => room;
   private PlayerSpawner playerSpawner;

   public abstract GameObject Generate(DungeonParameters parameters);

   public void SetPlayerSpawner(PlayerSpawner playerSpawner)
   {
       this.playerSpawner = playerSpawner;
   }

   public void Enter() {
      playerSpawner.SetupCamera(this);
   }
}

[System.Serializable]
public struct WeightedRoom : IWeighted {
    public Room room;
    public float Weight;

    float IWeighted.Weight => Weight;
}
