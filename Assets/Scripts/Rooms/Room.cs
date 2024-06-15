using UnityEngine;

public abstract class Room : MonoBehaviour
{
   private PlayerSpawner playerSpawner;

   public abstract Room Generate(DungeonParameters parameters);

   public void SetPlayerSpawner(PlayerSpawner playerSpawner)
   {
       this.playerSpawner = playerSpawner;
   }

   public void Enter() {
   }

    public void SetupCamera(GameObject player)
    {
        Debug.Log("Setting up camera for room ");
        var movement = player.GetComponent<PlayerMovement>();
        Debug.Assert(player != null, "Player does not have a PlayerMovement component.");

        var camera = GetComponentInChildren<Camera>();
        Debug.Assert(camera != null, "Room does not have a camera.");

        camera.enabled = true;
        movement.SetCamera(camera);
    }

    public void DisableCamera()
    {
        var camera = GetComponentInChildren<Camera>();
        camera.enabled = false;
    }
}

[System.Serializable]
public struct WeightedRoom : IWeighted {
    public Room room;
    public float Weight;

    float IWeighted.Weight => Weight;
}
