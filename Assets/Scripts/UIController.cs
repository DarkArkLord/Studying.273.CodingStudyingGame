using UnityEngine;

public class UIController : MonoBehaviour
{
    public ObjectPoolManager ObjectPool;

    private UIButtonController[] buttons = new UIButtonController[9];
    const float buttonOffset = 0.333f;
    private Vector3 buttonLocalScale;

    public bool IsActive { get; private set; } = false;
    public bool IsComplete { get; private set; } = true;

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
            buttons[i].Value.text = i.ToString();
        }
        SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetActive(bool active)
    {
        IsActive = active;
        IsComplete = !active;
        gameObject.SetActive(active);
        foreach (var button in buttons)
        {
            button?.gameObject.SetActive(active);
        }
    }
}
