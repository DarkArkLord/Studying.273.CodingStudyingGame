using System;
using UnityEngine;

public class UIButtonController : MonoBehaviour
{
    public TextMesh Value;
    public TextMesh Number;
    public UIController MainController;

    private int index = -1;

    // Start is called before the first frame update
    void Start()
    {
        SetStartValue();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseUp()
    {
        if (index < 0)
        {
            index = MainController.PressedButtons.Count;
            Number.text = index.ToString();
            Value.color = Color.yellow;
            Number.color = Color.yellow;
            MainController.PressedButtons.Push(this);
        }
        else if (index + 1 == MainController.PressedButtons.Count)
        {
            SetStartValue();
            MainController.PressedButtons.Pop();
        }
    }

    public void SetStartValue()
    {
        index = -1;
        Number.text = index.ToString();
        Value.color = Color.white;
        Number.color = Color.gray;
    }
}
