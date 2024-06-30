using UnityEngine;

public class SoundTester : MonoBehaviour
{
	[SerializeField] Sound sound;
	[SerializeField] Sound song;


	private void Start()
	{
		SoundManager.instance.PlaySong(song);
	}

	private void Update()
	{
		if (Input.GetKey(KeyCode.R))
		{
			SoundManager.instance.PlaySoundEffect( sound );
		}
	}
}
