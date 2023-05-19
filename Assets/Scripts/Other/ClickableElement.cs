using UnityEngine;
using UnityEngine.EventSystems;

namespace Others
{
    public enum ClickableType : byte
    {
        StartButton,
        PauseButton,
        ResumeButton,
        RestartButton,
    }

    public class ClickableElement : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private ClickableType _clickableType;

        public void OnPointerClick(PointerEventData eventData)
        {
            EventManager.TriggerEvent(EventManager.OnClick, _clickableType);
        }
    }
}
