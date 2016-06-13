using UnityEngine;

namespace Assets.Scripts.Gameplay.Sharks
{
    public class SharkExitAnimationStateChange : StateMachineBehaviour
    {
        public delegate void StateExitCallback();

        public StateExitCallback StateExitHandler { private get; set; }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool("IsExiting", false);
            StateExitHandler();
        }
    }
}