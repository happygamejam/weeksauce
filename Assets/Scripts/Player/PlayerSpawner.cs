using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject playerPrefab;

    void OnEnable()
    {
        if(playerPrefab == null)
        {
            Debug.LogError("Player prefab is not set in PlayerSpawner.");
            return;
        }

        var _ = Instantiate(playerPrefab, transform.position, transform.rotation);
    }
}
