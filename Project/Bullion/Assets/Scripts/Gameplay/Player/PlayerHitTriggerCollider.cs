using UnityEngine;
using Assets.Scripts.EventHandling;

namespace Assets.Scripts.Gameplay.Player
{
    public class PlayerHitTriggerCollider : MonoBehaviour
    {
        private Transform _transform;

        private void Start()
        {
            _transform = transform;
        }

        private void OnTriggerEnter(Collider collider)
        {
            EventDispatcher.FireEvent(_transform, collider.transform, EventMessage.Hit_Trigger_Collider);
        }
    }
}