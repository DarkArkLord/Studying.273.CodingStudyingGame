using UnityEngine;
using UnityEngine.EventSystems;

public class TempBattle1Conponent : MonoBehaviour
{
    public TempButtonComponent WinButton;
    public TempButtonComponent LoseButton;

    public bool? IsWin { get; private set; } = null;

    public void Init()
    {
        WinButton.OnClick.AddListener(SetWin);
        LoseButton.OnClick.AddListener(SetLose);
    }

    public void SetWin()
    {
        this.IsWin = true;
    }

    public void SetLose()
    {
        this.IsWin = false;
    }

    public void SetNoResult()
    {
        this.IsWin = null;
    }

    public void SetActivity(bool active)
    {
        gameObject.SetActive(active);
    }
}
