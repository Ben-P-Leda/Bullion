using UnityEngine;
using Assets.Scripts.EventHandling;

namespace Assets.Scripts.Gameplay.Avatar
{
    public class AvatarAnimationEventHandler : MonoBehaviour
    {
        private Transform _transform;
        private Transform _parentTransform;

        private void Start()
        {
            _transform = transform;
            _parentTransform = _transform.parent;
        }

        public void StartAttackEffect()
        {
            EventDispatcher.FireEvent(_transform, _parentTransform, EventMessage.Change_Strike_State, true);
        }
        
        public void EndAttackEffect()
        {
            EventDispatcher.FireEvent(_transform, _parentTransform, EventMessage.Change_Strike_State, false);
        }

        public void SwitchToDeadMode()
        {
            EventDispatcher.FireEvent(_transform, _parentTransform, EventMessage.End_Death_Sequence);
        }
    }
}