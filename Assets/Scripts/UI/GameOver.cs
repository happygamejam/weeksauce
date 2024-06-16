using UnityEngine;
using UnityEngine.UIElements;

public class GameOverController : MonoBehaviour
{
	// UI Elements
	private Button retryButton;
	private Button returnButton;

	void OnEnable()
	{
		var uiDocument = GetComponent<UIDocument>();

		retryButton = uiDocument.rootVisualElement.Q<Button>( "retry-button" );
		returnButton = uiDocument.rootVisualElement.Q<Button>( "return-button" );

		retryButton.clicked += DungeonManager.Restart;
		returnButton.clicked += DungeonManager.Quit;
	}
}
