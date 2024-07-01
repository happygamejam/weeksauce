using UnityEngine;
using UnityEngine.UIElements;

public class EndScreenController : MonoBehaviour
{
	// UI Elements
	private Button returnButton;

	void OnEnable()
	{
		var uiDocument = GetComponent<UIDocument>();

		returnButton = uiDocument.rootVisualElement.Q<Button>( "return-button" );

		returnButton.clicked += DungeonManager.Quit;
	}
}
