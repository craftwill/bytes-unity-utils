using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

using Bytes;

namespace Bytes
{
    public class DebugManager : MonoBehaviour
    {
        static public DebugManager instance;

        public bool active = false;
        private Text debugText;
        private Canvas debugManagerCanvas;
        private CanvasGroup canvasGroup;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            if (debugManagerCanvas == null)
            {
                debugManagerCanvas = Utils.CreateCanvas(this.transform, "debugManagerCanvas");
                debugManagerCanvas.sortingOrder = System.Int16.MaxValue - 100;

                canvasGroup = debugManagerCanvas.gameObject.AddComponent<CanvasGroup>();

                var g = new GameObject("debugText");
                g.transform.SetParent(debugManagerCanvas.transform, true);
                debugText = g.AddComponent<Text>();
                debugText.raycastTarget = false;
                debugText.gameObject.SetActive(false);
                debugText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                debugText.fontSize = 21;
                debugText.color = Color.white;

                debugText.rectTransform.pivot = new Vector2(0, 1);
                debugText.rectTransform.anchorMin = new Vector2(0, 1);
                debugText.rectTransform.anchorMax = new Vector2(0, 1);
                debugText.rectTransform.sizeDelta = new Vector2(900, 900);
                debugText.rectTransform.anchoredPosition = new Vector2(15, -15);
         
                SetDebugText("testing");
            }
        }

        static public void SetDebugText(string text)
        {
            if (!instance.active) { return; }

            instance.SetDebugText_Private(text);
        }
        private void SetDebugText_Private(string text)
        {
            if (!debugText.gameObject.activeInHierarchy)
            {
                debugText.gameObject.SetActive(true);
                Animate.FadeCanvasGroup(canvasGroup, 2f, 0, 1);
            }
            debugText.text = text;
            Debug.Log(text);
        }

        static public void AddDebugText(string text)
        {
            if (!instance.active) { return; }

            instance.AddDebugText_Private(text);
        }
        public void AddDebugText_Private(string text)
        {
            debugText.text += "\n" + text;
            Debug.Log(text);
        }

    }
}