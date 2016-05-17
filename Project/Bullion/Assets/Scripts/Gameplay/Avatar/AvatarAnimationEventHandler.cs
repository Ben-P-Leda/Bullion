using UnityEngine;
using Assets.Scripts.Event_Handling;

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
            EventDispatcher.FireEvent(_transform, _parentTransform, Event_Start_Strike, 0.0f);
        }

        
        public void EndAttackEffect()
        {
            EventDispatcher.FireEvent(_transform, _parentTransform, Event_End_Strike, 0.0f);
        }

        public void SwitchToDeadMode()
        {
            EventDispatcher.FireEvent(_transform, _parentTransform, Event_End_Death_Sequence, 0.0f);
        }

        public const string Event_Start_Strike = "AttackStrikeStart";
        public const string Event_End_Strike = "AttackStrikeEnd";
        public const string Event_End_Death_Sequence = "EndDeathSequence";
    }
}