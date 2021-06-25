using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Bytes
{
    public class DispatchEventBytes : MonoBehaviour
    {
        public string eventName;

        [Header("Only use one...")]
        public bool useStringParam = false;
        public string stringParam = "";

        [Header("Only use one...")]
        public bool useIntParam = false;
        public int intParam = -1;

        [Header("Only use one...")]
        public bool useBoolParam = false;
        public bool boolParam = false;

        public void DispatchEvent()
        {
            Bytes.Data data = null;
            if (useStringParam)    { data = new StringDataBytes(stringParam); }
            else if (useIntParam)  { data = new IntDataBytes(intParam); }
            else if (useBoolParam) { data = new BoolDataBytes(boolParam); }

            EventManager.Dispatch(eventName, data);
        }
    }
}
