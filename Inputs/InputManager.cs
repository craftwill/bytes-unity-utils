using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Bytes
{
    [System.Serializable]
    [SerializeField]
    public class Keybind
    {
        public string name;
        public KeyCode[] keycodes;
        public UnityEvent OnKeyDown;
        public UnityEvent OnKeyUp;
        public bool[] keysPressed;
        public bool pressedLastFrame;
        public bool allKeysAtOnce;
    }
    public class InputManager : MonoBehaviour
    {
        #region Singleton
        static public InputManager instance;
        private void Awake()
        {
            instance = this;
        }
        #endregion

        public List<Keybind> keybinds;
        private void Update()
        {
            foreach (Keybind keybind in keybinds)
            {
                if (keybind.allKeysAtOnce)
                {
                    for (int i = 0; i < keybind.keycodes.Length; i++)
                    {
                        if (Input.GetKeyDown(keybind.keycodes[i]))
                        {
                            keybind.keysPressed[i] = true;
                        }
                        else if (Input.GetKeyUp(keybind.keycodes[i]))
                        {
                            keybind.keysPressed[i] = false;
                            keybind.pressedLastFrame = false;
                            keybind.OnKeyUp?.Invoke();
                        }
                    }
                    bool allKeysPressed = GetKeybindKeysArePressed(keybind);
                    // We check the first key arbitrarily because we don't want the 
                    //  event to fire each frame
                    if (allKeysPressed && !keybind.pressedLastFrame)
                    {
                        keybind.pressedLastFrame = true;
                        keybind.OnKeyDown?.Invoke();
                    }
                }
                else
                {
                    for (int i = 0; i < keybind.keycodes.Length; i++)
                    {
                        if (Input.GetKeyDown(keybind.keycodes[i]))
                        {
                            keybind.OnKeyDown?.Invoke();
                            keybind.keysPressed[i] = true;
                        }
                        else if (Input.GetKeyUp(keybind.keycodes[i]))
                        {
                            keybind.OnKeyUp?.Invoke();
                            keybind.keysPressed[i] = false;
                        }
                    }
                }
            }
        }

        static public void SubscribeToKeydown(string keybindName, UnityAction callback)
        {
            Keybind keybind = GetKeybindByName(keybindName);
            keybind.OnKeyDown.AddListener(callback);
        }
        static public void UnsubscribeFromKeydown(string keybindName, UnityAction callback)
        {
            Keybind keybind = GetKeybindByName(keybindName);
            keybind.OnKeyDown.RemoveListener(callback);
        }

        static public void SubscribeToKeyup(string keybindName, UnityAction callback)
        {
            Keybind keybind = GetKeybindByName(keybindName);
            keybind.OnKeyUp.AddListener(callback);
        }
        static public void UnsubscribeFromKeyup(string keybindName, UnityAction callback)
        {
            Keybind keybind = GetKeybindByName(keybindName);
            keybind.OnKeyUp.RemoveListener(callback);
        }

        static public Keybind GetKeybindByName(string keybindName)
        {
            return instance.keybinds.Find(e => e.name == keybindName);
        }
        static public bool GetKeybindKeysArePressed(string keybindName)
        {
            return GetKeybindKeysArePressed(GetKeybindByName(keybindName));
        }
        static public bool GetKeybindKeysArePressed(Keybind pKeybind)
        {
            bool yes = false;
            bool allPressed = true;
            for (int i = 0; i < pKeybind.keycodes.Length; i++)
            {
                if (pKeybind.keysPressed[i]) { yes = true; }
                else { allPressed = false; }
            }
            return yes && ((pKeybind.allKeysAtOnce) ? allPressed : true);
        }
    }
}
