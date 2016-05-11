using UnityEngine;

namespace Assets.Scripts.Gameplay.Avatar
{
    public class AvatarAttackAnimationEventHandler : MonoBehaviour
    {
        public delegate void StrikeHandler(bool attackEnabled);
        public StrikeHandler StrikeCallback;

        public void StartAttackEffect()
        {
            if (StrikeCallback != null)
            {
                StrikeCallback(true);
            }
        }

        public void EndAttackEffect()
        {
            if (StrikeCallback != null)
            {
                StrikeCallback(false);
            }
        }
    }
}