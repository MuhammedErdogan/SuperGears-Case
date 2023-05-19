using Others;
using UnityEngine;

[DefaultExecutionOrder(-101)]
public class GameManager : MonoBehaviour
{
    private void Start()
    {
        EventManager.TriggerEvent(EventManager.GameStarted);
    }

    private void OnEnable()
    {
        EventManager.StartListening(EventManager.OnClick, ClickHandler);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventManager.OnClick, ClickHandler);
    }

    private void ClickHandler(ClickableType clickable)
    {
        switch (clickable)
        {
            case ClickableType.StartButton:
                //start the game
                break;
            case ClickableType.PauseButton:
                // Pause the game
                break;
            case ClickableType.ResumeButton:
                // Resume the game
                break;
            case ClickableType.RestartButton:
                EventManager.TriggerEvent(EventManager.OnTourRestart);
                break;
        }
    }
}
