using UnityEngine;

public class UIController : MonoBehaviour
{
    public ObjectPoolManager ObjectPool;

    private UIButtonController[] buttons = new UIButtonController[9];
    const float buttonOffset = 0.333f;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            var obj = ObjectPool.GetObject();
            buttons[i] = obj.GetComponent<UIButtonController>();
            var x = i / 3 - 1;
            var y = i % 3 - 1;
            obj.transform.localPosition = new Vector3(x * buttonOffset, y * buttonOffset, -2);
        }
        SetVisibility(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetVisibility(bool visibility)
    {
        gameObject.SetActive(visibility);
        foreach (var button in buttons)
        {
            button?.gameObject.SetActive(visibility);
        }
    }
}
