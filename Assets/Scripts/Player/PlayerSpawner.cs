using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject playerPrefab;

    private GameObject player;

    public void SpawnPlayer(GameObject room) {
        if(playerPrefab == null)
        {
            Debug.LogError("Player prefab is not set in PlayerSpawner.");
            return;
        }

        player = Instantiate(playerPrefab, transform.position, transform.rotation);

        Camera.main.enabled = false;
        SetupCamera(room);
    }

    private void SetupCamera(GameObject room) {
        Debug.Log("Setting up camera for room ");
        var movement = player.GetComponent<PlayerMovement>();
        Debug.Assert(player != null, "Player does not have a PlayerMovement component.");

        var camera = room.GetComponentInChildren<Camera>();
        Debug.Assert(camera != null, "Room does not have a camera.");

        camera.enabled = true;
        movement.SetCamera(camera);
    }
}
