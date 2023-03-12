using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCube : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        const float speed = 5f;
        var offset = Vector3.zero;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            offset += new Vector3(-speed, 0f);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            offset += new Vector3(speed, 0f);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            offset += new Vector3(0f, speed);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            offset += new Vector3(0f, -speed);
        }
        gameObject.transform.position += offset * Time.deltaTime;
    }
}
