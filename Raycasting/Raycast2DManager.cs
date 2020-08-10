using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Bytes
{
    [SerializeField]
    [System.Serializable]
    public class HitEvent : UnityEvent<RaycastHit2D> { }
    [SerializeField]
    [System.Serializable]
    public class HitTouchEvent : UnityEvent<RaycastHit2D, Touch> { }

    // The reason why this manager is not a singleton is because it can be used for raycasts across multiple screens
    public class Raycast2DManager : MonoBehaviour
    {
        [Header("References")]
        public Camera raycastCamera;
        [Header("Settings")]
        public bool Logs;
        public bool MultiTouchEnabled = true;
        public bool RaycastOnMouseDown_left = true;
        public bool RaycastOnMouseUp_left = true;
        public bool RaycastOnMouseDown_right;
        public bool RaycastOnMouseUp_right;
        public Vector3 offset;
        public string[] possibleTags;
        [Header("Events - General")]
        // General events
        public HitEvent OnRaycast2DMouseHit;
        public HitEvent OnRaycast2DMouseDown;
        public HitEvent OnRaycast2DMouseUp;
        // Mouse button specifics
        [Header("Events - Specifics")]
        public HitEvent OnRaycast2DMouseDown_left;
        public HitEvent OnRaycast2DMouseUp_left;
        public HitEvent OnRaycast2DMouseDown_right;
        public HitEvent OnRaycast2DMouseUp_right;

        [Header("Events - Touches")]
        public HitTouchEvent OnTouch2DHit;
        public HitTouchEvent OnTouch2DHitEnd;

        private void Reset()
        {
            Initialize();
        }
        public void Initialize()
        {
            if (raycastCamera == null)
            {
                if (Logs) { Debug.LogWarning("RaycastManager: 'raycastCamera' was not initialized, so we took Main Camera instead."); }
                raycastCamera = Camera.main;
            }
            if (MultiTouchEnabled) { Input.multiTouchEnabled = true; }
        }
        private void Update()
        {
            if (MultiTouchEnabled)
            {

            }
            else
            {
                // Left mouse button
                if (RaycastOnMouseDown_left)
                {
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        RaycastHit2D? hit = DoMouseRaycast2D();
                        if (hit != null) { OnRaycast2DMouseDown_left.Invoke((RaycastHit2D)hit); OnRaycast2DMouseDown.Invoke((RaycastHit2D)hit); if (Logs) { Debug.Log("left mouse down: " + hit.Value.transform.name); } }
                        else { if (Logs) { Debug.Log("left mouse down: Found Nothing"); } }
                    }
                }
                if (RaycastOnMouseUp_left)
                {
                    if (Input.GetKeyUp(KeyCode.Mouse0))
                    {
                        RaycastHit2D? hit = DoMouseRaycast2D();
                        if (hit != null) { OnRaycast2DMouseUp_left.Invoke((RaycastHit2D)hit); OnRaycast2DMouseUp.Invoke((RaycastHit2D)hit); if (Logs) { Debug.Log("left mouse up: " + hit.Value.transform.name); } }
                        else { if (Logs) { Debug.Log("left mouse up: Found Nothing"); } }
                    }
                }
                // Right mouse button
                // Left mouse button
                if (RaycastOnMouseDown_right)
                {
                    if (Input.GetKeyDown(KeyCode.Mouse1))
                    {
                        RaycastHit2D? hit = DoMouseRaycast2D();
                        if (hit != null) { OnRaycast2DMouseDown_right.Invoke((RaycastHit2D)hit); OnRaycast2DMouseDown.Invoke((RaycastHit2D)hit); if (Logs) { Debug.Log("right mouse down: " + hit.Value.transform.name); } }
                        else { if (Logs) { Debug.Log("right mouse down: Found Nothing"); } }
                    }
                }
                if (RaycastOnMouseUp_right)
                {
                    if (Input.GetKeyUp(KeyCode.Mouse1))
                    {
                        RaycastHit2D? hit = DoMouseRaycast2D();
                        if (hit != null) { OnRaycast2DMouseUp_right.Invoke((RaycastHit2D)hit); OnRaycast2DMouseUp.Invoke((RaycastHit2D)hit); if (Logs) { Debug.Log("right mouse up: " + hit.Value.transform.name); } }
                        else { if (Logs) { Debug.Log("right mouse up: Found Nothing"); } }
                    }
                }
            }
        }
        // Instance version
        public RaycastHit2D? DoMouseRaycast2D(string[] costumTags = null)
        {
            // By default, we use 'possibleTags', else we use the parameter.
            if (costumTags == null) { costumTags = possibleTags; }
            return DoMouseRaycast2D(this, raycastCamera, possibleTags);
        }

        // Static version: This is where the calculation is done
        // Raycast that 
        static public RaycastHit2D? DoMouseRaycast2D(Raycast2DManager managerInstance, Camera pRaycastCamera, string[] tags)
        {

            //GameObject.Find("t1").GetComponent<UnityEngine.UI.Text>().text = Display.RelativeMouseAt(Input.mousePosition).x.ToString();

            Ray ray = pRaycastCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction);
            // Return first raycast
            foreach (RaycastHit2D hit in hits)
            {
                //Debug.DrawLine(pRaycastCamera.transform.position - new Vector3(0,0,10), hit.transform.position, Color.red);

                bool lHasOneTag = false;
                foreach (string tag in tags) { if (hit.transform.tag == tag) { lHasOneTag = true; } }
                if (lHasOneTag)
                {
                    managerInstance.OnRaycast2DMouseHit.Invoke(hit);
                    return hit;
                }
            }
            return null;
        }

    }
}