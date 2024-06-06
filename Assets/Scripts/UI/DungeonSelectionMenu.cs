using UnityEngine;
using UnityEngine.UIElements;

public class DungeonSelectionMenu : MonoBehaviour
{
    [SerializeField]
    VisualTreeAsset dungeonEntryTemplate;
    
    void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
    
        Debug.Log("Setting up Duingeon List Controller");
        var controller = new DungeonListController();
        controller.Initialize(uiDocument.rootVisualElement, dungeonEntryTemplate);
    }
}
