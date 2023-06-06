using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public ObjectPoolManager ObjectPool;
    public TextMesh ResultText;

    private UIButtonController[] buttons = new UIButtonController[9];
    const float buttonOffset = 0.333f;
    private Vector3 buttonLocalScale;

    public bool IsActive { get; private set; } = false;
    public bool IsComplete { get; private set; } = true;
    public bool IsWin { get; private set; } = true;

    public Stack<UIButtonController> PressedButtons = new Stack<UIButtonController>();

    // Start is called before the first frame update
    void Start()
    {
        buttonLocalScale = new Vector3(0.2f, 0.2f, 1f);
        for (int i = 0; i < buttons.Length; i++)
        {
            var obj = ObjectPool.GetObject();
            var x = i / 3 - 1;
            var y = i % 3 - 1;
            obj.transform.localPosition = new Vector3(x * buttonOffset, y * buttonOffset, -2);
            obj.transform.localScale = buttonLocalScale;

            buttons[i] = obj.GetComponent<UIButtonController>();
            buttons[i].MainController = this;
            buttons[i].Value.text = (i + 1).ToString();
        }
        SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsActive && !IsComplete && buttons.Length == PressedButtons.Count)
        {
            IsComplete = true;
            IsWin = IsCorrectOrder();
            if (IsWin)
            {
                ResultText.text = "Верно";
            }
            else
            {
                ResultText.text = "Неверно";
            }
            ShowResult();
        }
    }

    void OnMouseUp()
    {
        if (IsComplete)
        {
            SetActive(false);
            foreach (var button in buttons)
            {
                button?.SetStartValue();
            }
            PressedButtons.Clear();
        }
    }

    public void SetActive(bool active)
    {
        IsActive = active;
        IsComplete = false;
        ResultText.gameObject.SetActive(false);
        gameObject.SetActive(active);
        foreach (var button in buttons)
        {
            button?.gameObject.SetActive(active);
        }
    }

    bool IsCorrectOrder()
    {
        var arr = PressedButtons.Select(value => value.Value.text).Select(int.Parse).ToArray();
        for (int i = 1; i < arr.Length; i++)
        {
            if (arr[i - 1] < arr[i])
            {
                return false;
            }
        }
        return true;
    }

    void ShowResult()
    {
        ResultText.gameObject.SetActive(true);
        foreach (var button in buttons)
        {
            button?.gameObject.SetActive(false);
        }
    }
}
