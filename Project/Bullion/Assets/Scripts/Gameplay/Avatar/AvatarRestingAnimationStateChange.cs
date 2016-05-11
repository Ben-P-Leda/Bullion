﻿using UnityEngine;

namespace Assets.Scripts.Gameplay.Avatar
{
    public class AvatarRestingAnimationStateChange : StateMachineBehaviour
    {
        public delegate void EnableMovementHandler();
        public EnableMovementHandler EnableMovementCallback;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Debug.Log("State change");

            if (EnableMovementCallback != null)
            {
                Debug.Log("EnableMove fired");
                EnableMovementCallback();
            }
        }
    }
}