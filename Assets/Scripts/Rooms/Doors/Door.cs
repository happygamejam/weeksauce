using UnityEngine;

public class Door : MonoBehaviour
{
	[SerializeField] SoundManager soundManager;
	[SerializeField] Sound doorSound;
	public bool isOpen { get; private set; }

	private void Awake()
	{
		soundManager = FindObjectOfType<SoundManager>();
	}

	public void OpenDoor()
	{
		soundManager.PlaySoundEffect( doorSound );
		isOpen = true;
	}





}
