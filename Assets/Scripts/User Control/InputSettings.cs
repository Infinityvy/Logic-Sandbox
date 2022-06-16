using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputSettings
{
    public static float cameraSpeed = 20;
    public static float zoomSpeed = 3000;

    public static KeyCode up = KeyCode.W;
    public static KeyCode down = KeyCode.S;
    public static KeyCode left = KeyCode.A;
    public static KeyCode right = KeyCode.D;

    public static KeyCode place = KeyCode.Mouse0;
    public static KeyCode interact = KeyCode.Space;
    public static KeyCode remove = KeyCode.Mouse1;

    public static KeyCode[] numbers =
    {
        KeyCode.Alpha1,
        KeyCode.Alpha2,
        KeyCode.Alpha3,
        KeyCode.Alpha4,
        KeyCode.Alpha5,
        KeyCode.Alpha6,
        KeyCode.Alpha7,
        KeyCode.Alpha8,
        KeyCode.Alpha9,
        KeyCode.Alpha0
    };
}
