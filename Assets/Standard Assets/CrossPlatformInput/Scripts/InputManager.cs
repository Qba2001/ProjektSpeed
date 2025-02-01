using System.Collections.Generic;
using UnityEngine;

public static class InputManager
{
    private static Dictionary<string, bool> buttonStates = new Dictionary<string, bool>();
    private static Dictionary<string, float> axisStates = new Dictionary<string, float>();

    public static void SetButtonDown(string name)
    {
        buttonStates[name] = true;
    }

    public static void SetButtonUp(string name)
    {
        buttonStates[name] = false;
    }

    public static bool GetButtonDown(string name)
    {
        return buttonStates.ContainsKey(name) && buttonStates[name];
    }

    public static void SetAxisPositive(string name)
    {
        axisStates[name] = 1f;
    }

    public static void SetAxisNegative(string name)
    {
        axisStates[name] = -1f;
    }

    public static void SetAxisZero(string name)
    {
        axisStates[name] = 0f;
    }

    public static float GetAxis(string name)
    {
        return axisStates.ContainsKey(name) ? axisStates[name] : 0f;
    }
}