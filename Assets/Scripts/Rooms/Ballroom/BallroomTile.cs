using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class BallroomTile : MonoBehaviour
{
	private TileGenerator _master;
	private int _level = 0;
	private int _maxLevel = 0;

	private List<int> _validLevels = new List<int>();
	private int _checkpointLevel = -1;
	private GameObject _theDuckingFloor;

	private GameObject _cube;
	private GameObject _door;


	public void OnEnable()
	{
		_cube = transform.GetChild( 0 ).gameObject;
	}

	public void Setup(TileGenerator newMaster, GameObject theDuckingFloor, GameObject door, int newCheckpointLevel, int maxLevel)
	{
		_master = newMaster;
		newMaster.UpdateTileEvent += UpdateLevel;
		_checkpointLevel = newCheckpointLevel;
		_theDuckingFloor = theDuckingFloor;
		_maxLevel = maxLevel;
		_door = door;
	}

	public void AddValidLevel(int newValidLevel)
	{
		_validLevels.Add( newValidLevel );
	}

	public void UpdateLevel(int newLevel)
	{
		_level = newLevel;
	}



	private void OnTriggerEnter(Collider other)
	{
		Debug.Log( _level );
		Debug.Log( _maxLevel );

		if ( !_validLevels.Contains( _level ) )
		{
			Debug.Log( "entered" );


			other.gameObject.GetComponent<PlayerInput>().enabled = false;
			other.gameObject.GetComponent<CharacterController>().enabled = false;
			other.transform.position = new Vector3( transform.position.x, transform.position.y + 2, transform.position.z );
			other.gameObject.GetComponent<CharacterController>().enabled = true;




			Destroy( _theDuckingFloor );
			_cube.GetComponent<MeshRenderer>().enabled = false;
			_cube.GetComponent<BoxCollider>().enabled = false;
			GetComponent<BoxCollider>().enabled = false;
			other.gameObject.GetComponent<Animator>().Play( "Falling Idle" );

			StartCoroutine( KillChar() );
			// Destroy(gameObject);

			return;
			//TODO : KILL
		}

		if ( _level == _checkpointLevel )
		{
			_master.UpdateTileLevel( _checkpointLevel + 1 );
		}

		if ( _level == _maxLevel )
		{
			Debug.Log( "Boom?" );
			print( _door );
			Destroy( _door.gameObject );
		}
	}

	IEnumerator KillChar()
	{
		print( "What??" );
		yield return new WaitForSeconds( 2 );
		print( "What" );
		SceneManager.LoadScene( "GameOver" );
	}
}
