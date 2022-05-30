using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PlayerCamera : MonoBehaviour
{
    public static bool frozen = false;

    private Camera cam;

    private float currentSpeed = 1;

    private float camMinSize = 1;
    private float camMaxSize = LevelData.size * 0.5f;

    private void Start()
    {
        transform.position = new Vector3(LevelData.size / 2, 10, LevelData.size / 2);
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        if (frozen) return;

        Vector3 transPos = transform.position;

        currentSpeed = InputSettings.cameraSpeed * ( cam.orthographicSize / camMaxSize + 0.6f);

        float x = Mathf.Clamp(transPos.x + getHorizontalSpeed() * Time.deltaTime * currentSpeed, 0, LevelData.size);
        float z = Mathf.Clamp(transPos.z + getVerticalSpeed() * Time.deltaTime * currentSpeed, 0, LevelData.size);

        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize + -Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * InputSettings.zoomSpeed, camMinSize, camMaxSize);

        transform.position = new Vector3(x, 10, z);
    }

    private int getHorizontalSpeed() //left: -1; right: 1; neutral: 0
    {
        float mousePositionX = Input.mousePosition.x;
        if (/*mousePositionX < Screen.width * 0.02f || */Input.GetKey(InputSettings.left))
        {
            return -1; //left
        }
        else if (/*mousePositionX > Screen.width * 0.98f || */Input.GetKey(InputSettings.right))
        {
            return 1; //right
        }
        return 0; //idle
    }

    private int getVerticalSpeed() //left: -1; right: 1; neutral: 0
    {
        float mousePositionY = Input.mousePosition.y;
        if (/*mousePositionY < Screen.height * 0.02f || */Input.GetKey(InputSettings.down))
        {
            return -1; //down
        }
        else if (/*mousePositionY > Screen.height * 0.98f || */Input.GetKey(InputSettings.up))
        {
            return 1; //up
        }
        return 0; //idle
    }
}
