using UnityEngine;
using UnityEngine.UIElements;

public class HUD : MonoBehaviour
{
    private TimerController timer;

    void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
    
        timer = new TimerController();
        timer.Initialize(uiDocument.rootVisualElement);
        timer.OnTimerEnd += OnTimerEnd;
        
        timer.Start(300000);
        
    }

    void Update()
    {
        timer.Tick(Time.deltaTime * 1000);
    }

    private void OnTimerEnd() {
        DungeonManager.GameOver();
    }
}
