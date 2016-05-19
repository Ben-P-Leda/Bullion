using UnityEngine;

namespace Assets.Scripts.Gameplay.Player
{
    public interface IAnimated
    {
        void WireUpAnimators(Animator aliveModelAnimator, Animator deadModelAnimator);
    }
}
