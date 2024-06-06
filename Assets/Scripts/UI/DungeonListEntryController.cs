using UnityEngine.UIElements;

public class DungeonListEntryController
{
    private Label name;
    
    public void SetVisualElement(VisualElement visualElement)
    {
        name = visualElement.Q<Label>("dungeon-name");
    }
    
    public void SetDungeonParameters(DungeonParameters parameters)
    {
        name.text = parameters.dungeonName;
    }    
}
