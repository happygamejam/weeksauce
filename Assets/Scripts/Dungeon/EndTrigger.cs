using UnityEngine;
using UnityEngine.SceneManagement;

public class EndTrigger : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if ( !other.CompareTag( "Player" ) )
		{
			return;
		}

		Debug.Log( "Player hit done trigger" );
		SceneManager.LoadScene( "EndScreen" );
	}
}
