using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Assets.Scripts.CommonComponents
{
    public class ButtonComponent : MonoBehaviour, IPointerClickHandler
    {
        public UnityEvent OnClick = new UnityEvent();

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick.Invoke();
        }
    }
}
