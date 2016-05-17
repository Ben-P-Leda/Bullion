using UnityEngine;
using Assets.Scripts.EventHandling;

namespace Assets.Scripts.Gameplay.Player
{
    public class PlayerTakeDamage : MonoBehaviour
    {
        private Transform _transform;

        private void Start()
        {
            _transform = transform;
        }

        private void OnTriggerEnter(Collider collider)
        {
            EventDispatcher.FireEvent(_transform, collider.transform, EventMessage.Register_Damage);
        }
    }
}