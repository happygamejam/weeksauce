using System;
using UnityEngine.UIElements;

public class TimerController
{
    private double time;
    private bool isRunning;

    // UI Elements
    private Label timerLabel;

    public event Action OnTimerEnd;

    public void Initialize(VisualElement root)
    {
        timerLabel = root.Q<Label>("timer");
        timerLabel.text = "99:99";
    }

    // @duration: duration of the timer in milliseconds
    public void Start(double duration) {
        time = duration;
        isRunning = true;
        timerLabel.text = formatTime(time);
    }

    public void Stop() {
        isRunning = false;
        time = 0;
        timerLabel.text = formatTime(time);
        OnTimerEnd?.Invoke();
    }

    public void Tick(double deltaTime) {
        if (!isRunning) {
            return;
        }

        time -= deltaTime;
        if (time <= 0) {
            Stop();
            return;
        }

        timerLabel.text = formatTime(time);
    }

    private string formatTime(double time) {
        var t = System.TimeSpan.FromMilliseconds(time);
        int minutes = t.Minutes;
        int seconds = t.Seconds;
        return string.Format("{0:D2}:{1:D2}", minutes, seconds);
    }

}
