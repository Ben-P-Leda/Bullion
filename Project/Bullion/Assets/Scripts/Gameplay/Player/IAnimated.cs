using UnityEngine;

namespace Assets.Scripts.Gameplay.Player
{
    public interface IAnimated
    {
        Animator AliveModelAnimator { set; }
        Animator DeadModelAnimator { set; }
    }
}
