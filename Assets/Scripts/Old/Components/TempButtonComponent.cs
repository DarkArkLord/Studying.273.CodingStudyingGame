using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TempButtonComponent : MonoBehaviour, IPointerClickHandler
{
    public UnityEvent OnClick = new UnityEvent();

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick.Invoke();
    }
}
