using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bytes
{
    public abstract class ListenEvent : MonoBehaviour
    {
        [SerializeField] private string eventName;
        [SerializeField] private bool removeAfterDispatch = false;
        private void Start()
        {
            EventManager.AddEventListener(eventName, HandleEventDispatch);
        }
        protected virtual void HandleEventDispatch(Data data)
        {
            if (removeAfterDispatch) { EventManager.RemoveEventListener(eventName, HandleEventDispatch); }
        }
    }
}