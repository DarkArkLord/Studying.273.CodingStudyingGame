using Unity.VisualScripting;
using UnityEngine;

public class BootstrapperController : MonoBehaviour
{
    [SerializeField, Header("123")]
    private Light _light;
    private Camera _camera;

    [SerializeField, Header("123")]
    private GameObject _playerPrefab;

    private void Awake()
    {
        _camera = Camera.main;

        _light.transform.position = _camera.transform.position;
        _light.transform.rotation = _camera.transform.rotation;

        var movableCamera = _camera.AddComponent<SmoothMoveController>();
        var movableLight = _light.AddComponent<SmoothMoveController>();

        //
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Add keys down to event bus )0)
    }
}
