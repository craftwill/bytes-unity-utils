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

        public void SetLoopedState(BaseAnimState newState, string prefix = "")
        {
            if (state == newState) { return; }

            state = newState;
            PlayStateLoopedAnimation(BuildClipName(prefix, newState.ClipName));
        }

        public void PlayAnimOnce(BaseAnimState animState, string prefix)
        {
            CancelCurrentPlayOnceAnim();

            currentPlayOnceAnim = Utils.PlayAnimatorClip(animator, BuildClipName(prefix, animState.ClipName), ()=> {
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

        private void PlayStateLoopedAnimation(string clipName)
        {
            if (currentPlayOnceAnim != null) { return; }
            animator.Play(clipName, -1, 0);
        }

        private string BuildClipName(string prefix, string clipName)
        {
            if (clipName == "empty") { return clipName; }
            return prefix + "_" + clipName;
        }

    }

    public class BaseAnimState
    {
        static public readonly BaseAnimState Nothing = new BaseAnimState("empty", 1f);

        private readonly string _clipName = "";
        private readonly float _animSpeed = 1f;

        public BaseAnimState(string pClipName, float pAnimSpeed = 1f)
        {
            _clipName = pClipName;
            _animSpeed = pAnimSpeed;
        }

        public string ClipName { get { return _clipName; } }
        public float AnimSpeed { get { return _animSpeed; } }
    }
}
