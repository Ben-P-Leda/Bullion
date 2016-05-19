using UnityEngine;

namespace Assets.Scripts.Gameplay.Avatar
{
    public class AvatarDamagedAnimationStateChange : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool("DamageTaken", false);
        }
    }
}