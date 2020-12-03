using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bytes.Language
{
    public class LangText : MonoBehaviour
    {
        public int textId;
        public int speficicFile = -1;
        Text textComponent;
        public void UpdateText()
        {
            if (!LangManager.GetIsReady()) { return; }

            if (speficicFile == -1) { textComponent.text = LangManager.GetText(textId); }
            else { textComponent.text = LangManager.GetText(textId, speficicFile); }
        }
        private void Awake()
        {
            if (textComponent == null) { textComponent = GetComponent<Text>(); }
        }
        private void OnValidate()
        {
            if (textComponent == null) { textComponent = GetComponent<Text>(); }
            UpdateText();
        }
    }
}
