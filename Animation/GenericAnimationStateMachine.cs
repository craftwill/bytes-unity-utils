using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bytes
{
    public class GenericAnimationStateMachine : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        private BaseAnimState state;
        private Animate currentPlayOnceAnim;

        public void Initialize()
        {
            animator = GetComponentInChildren<Animator>();
        }

        public void SetLoopedState(BaseAnimState newState, string prefix = "", bool force = true)
        {
            if (state == newState) { return; }

            state = newState;
            PlayStateLoopedAnimation(BuildClipName(prefix, newState.ClipName));
        }

        // Only use variation suffix if nbVariation is defined
        public void PlayAnimOnce(BaseAnimState animState, string prefix, string speedParamName = "", float speedMult = 1f)
        {
            CancelCurrentPlayOnceAnim();

            if (speedParamName != "")
            {
                Debug.Log("set " + speedParamName + " : " + speedMult);
                animator.SetFloat(speedParamName, speedMult);
            }

            currentPlayOnceAnim = Utils.PlayAnimatorClip(animator, BuildClipName(prefix, animState.ClipName, animState.NbVariations), ()=> {
                currentPlayOnceAnim = null;
                PlayStateLoopedAnimation(BuildClipName(prefix, state.ClipName));
            });
        }

        // Only use variation suffix if nbVariation is defined
        public void PlayAnimOnceCustom(string animName, string prefix)
        {
            CancelCurrentPlayOnceAnim();

            currentPlayOnceAnim = Utils.PlayAnimatorClip(animator, BuildClipName(prefix, animName, -1), () => {
                currentPlayOnceAnim = null;
                PlayStateLoopedAnimation(BuildClipName(prefix, state.ClipName));
            });
        }

        public void CancelCurrentPlayOnceAnim()
        {
            if (currentPlayOnceAnim != null)
            {
                currentPlayOnceAnim.Stop(callEndFunction: true);
            }
        }

        private void PlayStateLoopedAnimation(string clipName, bool force = false)
        {
            if (force != true && currentPlayOnceAnim != null || enabled == false || animator == null) { return; }
            animator?.Play(clipName, -1, 0);
        }

        private string BuildClipName(string prefix, string clipName, int nbVariation = -1)
        {
            if (clipName == "empty") { return clipName; }

            string variationSuffix; ;
            if (nbVariation == -1) { variationSuffix = ""; }
            else { variationSuffix = Random.Range(1, nbVariation + 1).ToString(); }

            string f = prefix + "_" + clipName + variationSuffix;

            return f;
        }

    }

    public class BaseAnimState
    {
        static public readonly BaseAnimState Nothing = new BaseAnimState("empty", 1f);

        private readonly string _clipName = "";
        private readonly float _animSpeed = 1f;
        private readonly int _nbVariations = -1;

        public BaseAnimState(string pClipName, float pAnimSpeed = 1f, int pNbVariations = -1)
        {
            _clipName = pClipName;
            _animSpeed = pAnimSpeed;
            _nbVariations = pNbVariations;
        }

        public string ClipName { get { return _clipName; } }
        public float AnimSpeed { get { return _animSpeed; } }
        public int NbVariations{ get { return _nbVariations; } }
    }
}
