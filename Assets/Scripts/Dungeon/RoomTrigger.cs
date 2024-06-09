using UnityEngine;

public class RoomTrigger : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Player"))
        {
            return;
        }

        transform.parent.GetComponent<Room>().Enter();
    }
}
