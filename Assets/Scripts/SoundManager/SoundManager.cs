using UnityEngine;
using System.Collections.Generic;
public class SoundManager : MonoBehaviour
{
	public static SoundManager instance;
	
	[SerializeField] AudioSource sfxAudioSource;
	[SerializeField] AudioSource musicAudioSource;

	private void Awake()
	{
		instance = this;
	}
	public void PlaySoundEffect(Sound sound)
	{
		sfxAudioSource.clip = sound.clip;
		sfxAudioSource.volume = sound.volume;
		sfxAudioSource.loop = sound.loop;
		sfxAudioSource.Play();
	}

	public void PlaySong(Sound sound)
	{
		musicAudioSource.clip = sound.clip;
		musicAudioSource.volume = sound.volume;
		musicAudioSource.loop = sound.loop;
		musicAudioSource.Play();
	}
}
