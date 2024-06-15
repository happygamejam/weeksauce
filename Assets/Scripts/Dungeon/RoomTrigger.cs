using UnityEngine;

public class RoomTrigger : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Player"))
        {
            return;
        }

        Debug.Log("Player hit exit trigger");
    }
}
