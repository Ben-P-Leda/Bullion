using UnityEngine;
using Assets.Scripts.EventHandling;

namespace Assets.Scripts.Gameplay.Player
{
    public class PlayerRespawnBlast : MonoBehaviour
    {
        private Transform _transform;

        private void Start()
        {
            _transform = transform;
        }

        private void OnEnable()
        {
            EventDispatcher.MessageEventHandler += MessageEventHandler;
        }

        private void OnDisable()
        {
            EventDispatcher.MessageEventHandler -= MessageEventHandler;
        }

        private void MessageEventHandler(Transform originator, Transform target, string message)
        {
            if ((message == EventMessage.Respawn) && (target != _transform))
            {
                float distance = Vector3.Distance(originator.position, _transform.position);
                if (distance < Blast_Radius)
                {
                    EventDispatcher.FireEvent(originator, _transform, EventMessage.Respawn_Blast);
                    EventDispatcher.FireEvent(originator, _transform, EventMessage.Inflict_Damage, (Blast_Radius - distance) * Unit_Blast_Damage);
                }
            }
        }

        public const float Blast_Radius = 5.0f;

        private const float Unit_Blast_Damage = 4.0f;
    }
}