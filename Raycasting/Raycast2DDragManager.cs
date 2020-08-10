using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Bytes
{
    [RequireComponent(typeof(Raycast2DManager))]
    public class Raycast2DDragManager : MonoBehaviour
    {
        // Automaticly assigned
        private Raycast2DManager raycast2DManager;

        [Header("Variables")]
        public bool interactable = true;            // Can the player drag an object
        public bool returnToPosition = false;       // Should the object dragged return to it's initial position
        public bool centerObject = false;           // Should the object dragged be centered
        public float dragZValue = 0;               // Z position given to dragged object
        public bool setAsLastSibbling = true;       // Should the object be set as last sibling (to get in front of other objects on canvas mode)

        private Transform mDragged = null;
        private Vector3 mDraggedInitialPos;         // Initial pos of dragged object for offset
        private Vector3 mMouseInitialPos;           // Initial pos of mouse for offset
        private int mIndexAsChildBuffer;            // Store index as child before SetAsLastSibling() is called so that we can restore the dragged index

        [Header("Events")]
        public TransformEvent OnDragStart;
        public TransformEvent OnDragEnd;

        private void Start()
        {
            Initialize();
        }

        // Called when this component is attached in editor
        private void Initialize()
        {
            raycast2DManager = GetComponent<Raycast2DManager>();
            // Start drag
            raycast2DManager.OnRaycast2DMouseDown_left.AddListener((RaycastHit2D pHit) => {
                if (pHit.transform == null || !interactable) { return; }



                // pHit.transform exists, so we start dragging it
                if (raycast2DManager.Logs) { Debug.Log("Started dragging: " + pHit.transform.name); }
                mDragged = pHit.transform;
                mIndexAsChildBuffer = mDragged.GetSiblingIndex();
                mDragged.transform.SetAsLastSibling();
                mDraggedInitialPos = mDragged.transform.position;
                mMouseInitialPos = raycast2DManager.raycastCamera.ScreenToWorldPoint(Input.mousePosition);
                OnDragStart.Invoke(mDragged);
            });
        }

        // Drag update and release detection
        void Update()
        {
            if (interactable == false) { mDragged = null; return; }
            if (mDragged != null)
            {
                // Mouse position
                Vector3 lDraggedPos = raycast2DManager.raycastCamera.ScreenToWorldPoint(Input.mousePosition);
                lDraggedPos.z = dragZValue;

                // Mouse Offset
                Vector3 lMouseOffset = Vector3.zero;
                if (!centerObject) { lMouseOffset = mDraggedInitialPos - mMouseInitialPos; }

                // Final assignation
                mDragged.transform.position = lDraggedPos + lMouseOffset;

                // Detect if the mouse was released and end dragging if true
                if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    if (returnToPosition) { mDragged.transform.position = mDraggedInitialPos; }
                    if (setAsLastSibbling) { mDragged.SetSiblingIndex(mIndexAsChildBuffer); }
                    OnDragEnd.Invoke(mDragged);
                    if (raycast2DManager.Logs) { Debug.Log("Stopped dragging: " + mDragged.name); }
                    mDragged = null;
                }
            }
        }
    }
}