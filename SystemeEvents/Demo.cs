using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class Demo : MonoBehaviour
{
    private void Start()
    {
        EventManager.AddEventListener("pressE", HandleE);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            EventManager.Dispatch("pressE", new HandlerData(3));
        }
    }

    private void HandleE(Data data)
    {
        print("Sup ma dude");
        print(data.GetData());
        EventManager.RemoveEventListener("pressE", HandleE);
    }
}

public class HandlerData : Data
{
    private readonly int _number;
    public HandlerData(int number)
    {
        _number = number;
    }
    
    public object GetData()
    {
        return _number;
    }
}
