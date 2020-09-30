using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

namespace Bytes
{
    public class ListenStatFillBar : ListenEvent
    {
        [SerializeField] private Text hpText;
        [SerializeField] private Image fillingImage;
        protected override void HandleEventDispatch(Data data)
        {
            FillingBarChangeData statData = (FillingBarChangeData)data;

            hpText.text = (int)statData.Stat + " / " + (int)statData.MaxStat;
            fillingImage.fillAmount = statData.Stat / statData.MaxStat;

            base.HandleEventDispatch(data);
        }

        public class FillingBarChangeData : Data
        {
            public FillingBarChangeData(float stat, float maxStat) { Stat = stat; MaxStat = maxStat; }
            public float Stat    { get; private set; }
            public float MaxStat { get; private set; }
        }
    }
}
