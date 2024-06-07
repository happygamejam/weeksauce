using UnityEngine;
using UnityEngine.UIElements;

public class HUD : MonoBehaviour
{
    private TimerController timer;

    void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
    
        Debug.Log("Initializing HUD");

        timer = new TimerController();
        timer.Initialize(uiDocument.rootVisualElement);
        timer.OnTimerEnd += OnTimerEnd;
        
        timer.Start(30000 / 4);
        
    }

    void Update()
    {
        timer.Tick(Time.deltaTime * 1000);
        Debug.Log("Active Dungeon: " + DungeonManager.ActiveDungeon);
    }

    private void OnTimerEnd() {
        DungeonManager.GameOver();
    }
}
