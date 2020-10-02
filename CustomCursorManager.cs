using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bytes
{
    //Singleton<CustomCursorManager>
    public class CustomCursorManager : MonoBehaviour
    {
        #region Singleton
        static private CustomCursorManager Instance;
        #endregion

        [SerializeField] private SpriteRenderer customCursor;
        [SerializeField] private float cursorScale = 1f;
        [SerializeField] private Vector3 cursorRotation = new Vector3(0, 0, 36f);
        [SerializeField] private Vector3 cursorOffset = new Vector3(0, 0, 0);
        [SerializeField] private Sprite[] cursorSprites;
        Transform CustomCursor;
        Camera mainCam;
        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            mainCam = Camera.main;
            Cursor.visible = false;

            GameObject cursorGameObject = new GameObject("Bytes_CustomCursor");
            customCursor = cursorGameObject.AddComponent<SpriteRenderer>();
            customCursor.transform.localScale = new Vector3(cursorScale, cursorScale, cursorScale);
            customCursor.sortingOrder = 3000;
            customCursor.transform.rotation = Quaternion.Euler(cursorRotation);
            SetCursorState(0);
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Cursor.visible = false;
            }

            Vector3 targetPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            targetPos.z = 0f;
            customCursor.transform.position = targetPos + cursorOffset;
        }
        static public void SetCursorState(int newCursorSpriteIndex)
        {
            if (newCursorSpriteIndex < 0 || newCursorSpriteIndex >= Instance.cursorSprites.Length)
            {
                Debug.LogWarning("You did not set sprite for your custom cursor at index: " + newCursorSpriteIndex);
                return;
            }
            Instance.customCursor.sprite = Instance.cursorSprites[newCursorSpriteIndex];
        }
    }
}