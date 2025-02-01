using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keybinds : MonoBehaviour
{
    [SerializeField] private KeyCode upKey;
    [SerializeField] private KeyCode leftKey;
    [SerializeField] private KeyCode downKey;
    [SerializeField] private KeyCode rightKey;
    [SerializeField] private KeyCode jumpKey;

    void Start()
    {
        if (PlayerPrefs.HasKey("up"))
        {
            PlayerPrefs.SetString("up", "W");
            string upstring = PlayerPrefs.GetString("up");
            upKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), upstring);
        }
        if (PlayerPrefs.HasKey("down"))
        {
            PlayerPrefs.SetString("down", "S");
            string downstring = PlayerPrefs.GetString("down");
            downKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), downstring);
        }
        if (PlayerPrefs.HasKey("left"))
        {
            PlayerPrefs.SetString("left", "A");
            string leftstring = PlayerPrefs.GetString("left");
            leftKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), leftstring);
        }
        if (PlayerPrefs.HasKey("right"))
        {
            PlayerPrefs.SetString("right", "D");
            string rightstring = PlayerPrefs.GetString("right");
            rightKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), rightstring);
        }
        if (PlayerPrefs.HasKey("jump"))
        {
            PlayerPrefs.SetString("jump", "Space");
            string jumpstring = PlayerPrefs.GetString("jump");
            jumpKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), jumpstring);
        }
    }
    void Update()
    {
        // Przyk³adowe dzia³anie z keybindami
        if (Input.GetKeyDown(upKey))
        {
            Debug.Log("W zosta³o naciœniête.");
        }

        if (Input.GetKeyDown(leftKey))
        {
            Debug.Log("A zosta³o naciœniête.");
        }

        if (Input.GetKeyDown(downKey))
        {
            Debug.Log("S zosta³o naciœniête.");
        }

        if (Input.GetKeyDown(rightKey))
        {
            Debug.Log("D zosta³o naciœniête.");
        }

        if (Input.GetKeyDown(jumpKey))
        {
            Debug.Log("Spacja zosta³a naciœniêta.");
        }
    }

}
