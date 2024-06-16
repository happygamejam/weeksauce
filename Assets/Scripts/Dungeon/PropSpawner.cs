using System;
using System.Collections.Generic;
using UnityEngine;

public class PropSpawner : MonoBehaviour
{
	[SerializeField]
	private List<GameObject> props;
	[SerializeField]
	private List<Transform> spawnPoints;

	private int _propVariationValue;
	private System.Random random;

	public int GetPropVariationValue()
	{
		return _propVariationValue;
	}

	void Start()
	{
		if ( props.Count == 0 || spawnPoints.Count == 0 )
		{
			Debug.LogError( "Props or spawn points are not set." );
			return;
		}

		var provider = GetComponentInParent<DungeonParametersProvider>();
		if ( provider == null )
		{
			Debug.LogError( "DungeonParametersProvider not found. Room prefabs must have a DungeonParametersProvider component." );
			return;
		}

		var parameters = provider.Parameters;
		if ( parameters == null )
		{
			Debug.LogError( "DungeonsParameters were not set." );
			return;
		}

		if ( parameters.seed < 0 )
		{
			random = new System.Random();
		}
		else
		{
			random = new System.Random( parameters.seed );
		}

		int index = (int)Math.Round( random.NextDouble() * (props.Count - 1) );
		_propVariationValue = index;
		Debug.Log( "Spawning prop: " + index );
		GameObject prop = props[index];

		for ( int i = 0; i < spawnPoints.Count; i++ )
		{
			Transform spawnPoint = spawnPoints[i];
			Instantiate( prop, spawnPoint.position, spawnPoint.rotation );
		}
	}
}
