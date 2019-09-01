using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bytes.Mouse
{
    public class MouseManager : MonoBehaviour
    {
        public enum MouseState { LOCKED, UNLOCKED }
        static public void SetMouse(MouseState state)
        {
            if (state == MouseState.LOCKED)
            {

                var lockMode = CursorLockMode.Locked;
                Cursor.lockState = lockMode;
                Cursor.visible = false;
            }
            else if (state == MouseState.UNLOCKED)
            {
                var lockMode = CursorLockMode.None;
                Cursor.lockState = lockMode;
                Cursor.visible = true;
            }
        }
    }
}
