using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bytes
{
    public class CustomCursorManager : MonoBehaviour
    {
        #region Singleton
        static private CustomCursorManager Instance;
        #endregion

        [SerializeField] private Image customCursor;
        [SerializeField] private float cursorScale = 1f;
        [SerializeField] private Vector3 cursorRotation = new Vector3(0, 0, 36f);
        [SerializeField] private Vector3 cursorOffset = new Vector3(0, 0, 0);
        [SerializeField] private Sprite[] cursorSprites;
        Transform CustomCursor;
        Camera mainCam;
        Canvas mouseCanvas;
        private void Awake()
        {
            Instance = this;

            if (mouseCanvas == null)
            {
                var g = new GameObject("mouseCanvas");
                mouseCanvas = g.AddComponent<Canvas>();
                mouseCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
                mouseCanvas.sortingOrder = System.Int16.MaxValue;

                var cavansScaler = g.AddComponent<CanvasScaler>();
                cavansScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                cavansScaler.referenceResolution = new Vector2(1920, 1080);

                g.transform.SetParent(this.transform);
            }
        }
        private void Start()
        {
            mainCam = Camera.main;
            Cursor.visible = false;

            GameObject cursorGameObject = new GameObject("Bytes_CustomCursor");
            cursorGameObject.transform.SetParent(mouseCanvas.transform);
            customCursor = cursorGameObject.AddComponent<Image>();
            customCursor.transform.localScale = new Vector3(cursorScale, cursorScale, cursorScale);
            customCursor.transform.rotation = Quaternion.Euler(cursorRotation);
            SetCursorState(0);

            customCursor.SetNativeSize();
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Cursor.visible = false;
            }

            //Vector3 targetPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            //targetPos.z = 0f;
            customCursor.transform.position = Input.mousePosition + cursorOffset;
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