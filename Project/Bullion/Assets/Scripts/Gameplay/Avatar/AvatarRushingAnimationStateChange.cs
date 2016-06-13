using UnityEngine;

namespace Assets.Scripts.Gameplay.Avatar
{
    public class AvatarRushingAnimationStateChange : StateMachineBehaviour
    {
        public delegate void StateEntryHandler();
        public StateEntryHandler StateEntryCallback { private get; set; }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (StateEntryCallback != null)
            {
                StateEntryCallback();
            }
        }
    }
}