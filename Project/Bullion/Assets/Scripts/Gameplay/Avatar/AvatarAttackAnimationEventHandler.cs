using UnityEngine;

namespace Assets.Scripts.Gameplay.Avatar
{
    public class AvatarAttackAnimationEventHandler : MonoBehaviour
    {
        public delegate void EnableAttackHandler(bool attackEnabled);
        public EnableAttackHandler EnableAttackCallback;

        public void StartAttackEffect()
        {
            if (EnableAttackCallback != null)
            {
                EnableAttackCallback(true);
            }
        }

        public void EndAttackEffect()
        {
            if (EnableAttackCallback != null)
            {
                EnableAttackCallback(false);
            }
        }
    }
}