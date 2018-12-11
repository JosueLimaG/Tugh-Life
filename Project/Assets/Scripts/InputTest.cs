using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class InputTest : MonoBehaviour
{
    public Text system;
    public Text input;
    public Text axis;
    public Text buttons;
   
    private void Start()
    {
        print(SystemInfo.operatingSystemFamily);
        system.text = SystemInfo.operatingSystem;
        foreach (string x in Input.GetJoystickNames()) 
        {
            system.text += "\n" + x;
        }
    }

    public void Update()
    {
        axis.text = "";
        buttons.text = "";
        for (int i = 0; i < 28; i++)
        {
            axis.text += "\nAxis" + (i + 1) + ":\t";
            float value = CrossPlatformInputManager.GetAxisRaw("Axis" + (i + 1));
            value = (float)Math.Round(value * 100f) / 100f;
            axis.text += value;
        }

        if (DetectPressedKeyOrButton() != null)
            input.text = DetectPressedKeyOrButton();

        for (int i = 0; i < 13; i++)
        {
            buttons.text += "\nButton" + (i + 1) + ":\t";
            bool value2 = CrossPlatformInputManager.GetButton("Button" + (1 + i));
            buttons.text += value2;
        }

    }

    public string DetectPressedKeyOrButton()
    {
        foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(kcode))
                return kcode.ToString();
        }
        return null;
    }
}
