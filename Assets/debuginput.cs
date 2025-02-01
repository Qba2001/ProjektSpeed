using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debuginput : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A)) Debug.Log("input A");
    }
}
