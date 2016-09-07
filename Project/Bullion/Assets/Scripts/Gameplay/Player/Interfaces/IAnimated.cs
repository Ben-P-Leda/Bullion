using UnityEngine;

namespace Assets.Scripts.Gameplay.Player.Interfaces
{
    public interface IAnimated
    {
        void WireUpAnimators(Animator aliveModelAnimator, Animator deadModelAnimator);
    }
}
