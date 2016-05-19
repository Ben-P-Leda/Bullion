using UnityEngine;
using Assets.Scripts.Event_Handling;

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
            EventDispatcher.FireEvent(_transform, collider.transform, Event_Register_Damage, 0);
        }

        public const string Event_Register_Damage = "RegisterDamage";
    }
}