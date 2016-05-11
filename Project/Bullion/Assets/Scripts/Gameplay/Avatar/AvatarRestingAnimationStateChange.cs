using UnityEngine;

namespace Assets.Scripts.Gameplay.Avatar
{
    public class AvatarRestingAnimationStateChange : StateMachineBehaviour
    {
        public delegate void EnableMovementHandler();
        public EnableMovementHandler EnableMovementCallback;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (EnableMovementCallback != null)
            {
                EnableMovementCallback();
            }
        }
    }
}