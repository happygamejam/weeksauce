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
        
        timer.Start(300000);
    }

    void Update()
    {
        timer.Tick(Time.deltaTime * 1000);
    }
}
