using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    [SerializeField]
    private Room room;

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Player"))
        {
            return;
        }

        Debug.Log("Player hit exit trigger");
        if (room == null)
        {
            return;
        }
        room.SetupCamera(other.gameObject);
    }
}
