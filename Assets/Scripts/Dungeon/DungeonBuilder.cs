using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class DungeonBuilder : MonoBehaviour
{
	[SerializeField]
	private DungeonParameters dungeonParameters;
	[SerializeField]
	private List<WeightedRoom> rooms;
	[SerializeField]
	private PlayerSpawner playerSpawner;

	private readonly List<Room> roomInstances = new();
	private int roomIndex = -1;

	private void OnEnable()
	{
		var dungeonParameters = DungeonManager.ActiveDungeon;
		if ( dungeonParameters == null )
		{
			dungeonParameters = this.dungeonParameters;
		}

		Debug.Assert( dungeonParameters != null, "DungeonParameters is not set in DungeonBuilder" );
		Generate( dungeonParameters );

		Debug.Assert( playerSpawner != null, "PlayerSpawner is not set in DungeonBuilder" );
		playerSpawner.SpawnPlayer( roomInstances[0] );
	}

	private void Generate(DungeonParameters parameters)
	{
		var weightedRooms = new WeightedList<WeightedRoom>( rooms, parameters.seed );
		Vector3 cumulativeOffset = Vector3.zero;
		for ( int i = 0; i < parameters.roomCount; i++ )
		{
			WeightedRoom result = weightedRooms.Pick();
			var room = result.room.Generate( parameters );
			room.name = "Room " + i;

			Vector3 origin = room.transform.Find( "Points/StartPoint" ).position;
			room.transform.position = cumulativeOffset;
			Debug.Log( "Placing room at " + room.transform.position );

			Vector3 attach = room.transform.Find( "Points/EndPoint" ).position;
			cumulativeOffset = attach - origin;
			Debug.Log( "Next offset is " + cumulativeOffset );

			// Delete the entrance to to be able to chain rooms togther.
			// Except for the first room because we need it.
			if ( i != 0 )
			{
				foreach ( GameObject obj in FindChildrenWithTag( room.gameObject, "ChainDelete" ) )
				{
					Destroy( obj );
				}
			}
			if ( i != parameters.roomCount - 1 )
			{
				foreach ( GameObject obj in FindChildrenWithTag( room.gameObject, "ChainDeletePre" ) )
				{
					Destroy( obj );
				}
			}

			room.SetPlayerSpawner( playerSpawner );
			room.DisableCamera();

			roomInstances.Add( room );
		}
	}

	// TODO: This should probably be done recursively so it doesn't have to be a direct child.
	private List<GameObject> FindChildrenWithTag(GameObject parent, string tag)
	{
		List<GameObject> children = new();

		foreach ( Transform transform in parent.transform )
		{
			if ( !transform.CompareTag( tag ) )
			{
				continue;
			}
			children.Add( transform.gameObject );
		}

		return children;
	}
}


