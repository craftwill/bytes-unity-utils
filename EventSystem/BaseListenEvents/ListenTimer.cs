using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

namespace Bytes
{
    public class ListenTimer : ListenEvent
    {
        [SerializeField] private Text timeLeftText;
        [SerializeField] private Image fillingImage;

        [SerializeField] private float timerMax;
        [SerializeField] private float timeLeft;

        private Animate currentAnim;

        protected override void HandleEventDispatch(Data data)
        {
            TimerData timerData = (TimerData)data;

            if (currentAnim == null)
            {
                // Animate the skill cooldown
                timerMax = timerData.TimerMax;
                timeLeft = timerData.TimerMax;
                currentAnim = Animate.LerpSomething(timeLeft, GetStepHandler(), HandleAnimEnd);
            }
            else
            {
                /* We update the timer if it already existed */

                // We stop the Animation in case it would glitch out when changing the timeLeft variable
                currentAnim.Stop(callEndFunction: false);

                // We only reset timerMax if: ResetTimer is true
                if (timerData.ResetTimer) { timerMax = timerData.TimerMax; }

                // Reseting time left
                timeLeft = timerData.TimerMax;

                // Restarting animation
                currentAnim = Animate.LerpSomething(timeLeft, GetStepHandler(), HandleAnimEnd);
            }

            // Base stuff, don't worry about it
            base.HandleEventDispatch(data);
        }

        private System.Action<float> GetStepHandler()
        {
            if (timeLeftText != null) { return HandleAnimStep; }
            else { return HandleAnimStepNoText; }
        }

        private void HandleAnimStep(float step)
        {
            timeLeftText.text = timeLeft.ToString("0.0");
            fillingImage.fillAmount = 1 - (timeLeft / timerMax);
            timeLeft -= Time.deltaTime;
        }

        private void HandleAnimStepNoText(float step)
        {
            fillingImage.fillAmount = 1 - (timeLeft / timerMax);
            timeLeft -= Time.deltaTime;
        }

        private void HandleAnimEnd()
        {
            timeLeft = 0;
            timerMax = 0;
            fillingImage.fillAmount = 1;
            currentAnim.Stop(false);
            currentAnim = null;
        }

        public class TimerData : Data
        {
            public TimerData(float timerMax, bool resetTimer = false) { TimerMax = timerMax; ResetTimer = resetTimer; }
            public float TimerMax { get; private set; }
            public bool ResetTimer { get; private set; }
        }
    }
}