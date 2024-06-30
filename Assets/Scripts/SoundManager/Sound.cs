using UnityEngine;

[CreateAssetMenu(fileName = "Sound", menuName = "SoundManagement/Sound")]
public class Sound : ScriptableObject
{
    public AudioClip clip;
	public float volume;
	public float pitch;
	public bool loop;
	

}
