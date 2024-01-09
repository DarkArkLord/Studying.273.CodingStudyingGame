using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Assets.Scripts.CommonComponents
{
    public class ButtonComponent : MonoBehaviour, IPointerClickHandler
    {
        public UnityEvent OnClick = new UnityEvent();

        public bool IsButtonActive { get; protected set; } = true;

        public virtual void SetButtonActive(bool isActive)
        {
            IsButtonActive = isActive;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (IsButtonActive)
            {
                OnClick.Invoke();
            }
        }
    }
}
