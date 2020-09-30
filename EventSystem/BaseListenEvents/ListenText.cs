using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

namespace Bytes
{
    public class ListenText : ListenEvent
    {
        [SerializeField] private Text textToChange;
        protected override void HandleEventDispatch(Data data)
        {
            TextChangeData textData = (TextChangeData)data;
            textToChange.text = textData.Text;
            base.HandleEventDispatch(data);
        }

        public class TextChangeData : Data
        {
            public TextChangeData(string text) { Text = text; }
            public string Text { get; private set; }
        }
    }
}
