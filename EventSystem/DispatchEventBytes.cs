using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Bytes
{
    public class DispatchEventBytes : MonoBehaviour
    {
        public string eventName;
        public void DispatchEvent()
        {
            EventManager.Dispatch(eventName, null);
        }
    }
}
