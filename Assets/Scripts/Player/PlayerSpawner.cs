using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
	[SerializeField]
	private GameObject playerPrefab;

	private GameObject player;

	public void SpawnPlayer(Room room)
	{
		if ( playerPrefab == null )
		{
			Debug.LogError( "Player prefab is not set in PlayerSpawner." );
			return;
		}

		player = Instantiate( playerPrefab, transform.position, transform.rotation );

		//Camera.main.enabled = false;
		room.SetupCamera( player );
	}

}
