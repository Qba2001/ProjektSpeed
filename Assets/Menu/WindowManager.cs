using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    public static WindowManager Instance { get; private set; }

    private GameObject activeObject;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public bool ActivateCanvas(GameObject obiekt)
    {
        if (activeObject != null)
        {
            Debug.Log("There is already an active canvas.");
            return false;
        }

        activeObject = obiekt;
        activeObject.gameObject.SetActive(true);
        return true;
    }

    public void DeactivateCanvas(GameObject obiekt)
    {
        if (activeObject == obiekt)
        {
            obiekt.gameObject.SetActive(false);
            activeObject = null;
        }
    }

    public bool IsAnyCanvasActive()
    {
        return activeObject != null;
    }
}
