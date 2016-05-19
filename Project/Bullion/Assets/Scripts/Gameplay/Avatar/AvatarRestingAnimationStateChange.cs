using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Gameplay.Avatar
{
    public class AvatarRestingAnimationStateChange : StateMachineBehaviour
    {
        public delegate void StateEntryHandler();

        private List<StateEntryHandler> _stateEntryCallbacks;

        public AvatarRestingAnimationStateChange() 
            : base()
        {
            _stateEntryCallbacks = new List<StateEntryHandler>();
        }

        public void AddStateEntryHandler(StateEntryHandler stateEntryHandler)
        {
            if (!_stateEntryCallbacks.Contains(stateEntryHandler))
            {
                _stateEntryCallbacks.Add(stateEntryHandler);
            }
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_stateEntryCallbacks != null)
            {
                for (int i=0; i<_stateEntryCallbacks.Count; i++)
                {
                    _stateEntryCallbacks[i]();
                }
            }
        }
    }
}