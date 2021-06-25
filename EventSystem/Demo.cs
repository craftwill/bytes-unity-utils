using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bytes
{
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
            //print(">>>>> Sup ma dude: data=" + ((HandlerData)data).Number);
            //print(data.GetData());
            //EventManager.RemoveEventListener("pressE", HandleE);
        }
    }

    public class HandlerData : Data
    {
        public int Number { get; private set; }
        public HandlerData(int number)
        {
            Number = number;
        }
    }
}