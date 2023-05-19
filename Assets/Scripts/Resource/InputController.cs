using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InputController
{
    public void Update()
    {

    }

    public void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
        }
        else if (Input.GetKey(KeyCode.Space))
        {
        }
    }
}

public enum InputKey
{
    Keyboard,
    Mouse,
    Gamepad
}