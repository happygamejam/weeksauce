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
	private bool doorIsOpen = false;
	private SoundManager _soundManager;
	[SerializeField] private Sound deathSoundEffect;
		
	public void OnEnable()
	{
		_cube = transform.GetChild( 0 ).gameObject;
		_soundManager = FindObjectOfType<SoundManager>();
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
			other.gameObject.GetComponent<PlayerInput>().enabled = false;
			other.gameObject.GetComponent<CharacterController>().enabled = false;
			other.transform.position = new Vector3( transform.position.x, transform.position.y + 2, transform.position.z );
			other.gameObject.GetComponent<CharacterController>().enabled = true;

			/* Destroy( _theDuckingFloor ); */
			_cube.GetComponent<MeshRenderer>().enabled = false;
			_cube.GetComponent<BoxCollider>().enabled = false;
			GetComponent<BoxCollider>().enabled = false;
			other.gameObject.GetComponent<Animator>().Play( "Falling Idle" );

			StartCoroutine( KillChar() );

			return;
		}

		if ( _level == _checkpointLevel )
		{
			_master.UpdateTileLevel( _checkpointLevel + 1 );
		}

		if ( _level == _maxLevel )
		{
			if (!_door.GetComponent<Door>().isOpen)
			{
				_door.gameObject.GetComponent<Animator>().SetTrigger("Open");
				_door.GetComponent<Door>().OpenDoor();
			}
		}
	}

	IEnumerator KillChar()
	{
		_soundManager.StopSong();
		_soundManager.PlaySoundEffect( deathSoundEffect );
		
		yield return new WaitForSeconds( 2 );
		SceneManager.LoadScene( "GameOver" );
	}
}
