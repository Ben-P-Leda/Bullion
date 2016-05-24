using UnityEngine;
using Assets.Scripts.EventHandling;

namespace Assets.Scripts.Gameplay.Treasure
{
    public class ChestCollisions : MonoBehaviour
    {
        private Transform _transform;

        // TODO: Should be a property with private getter
        public float HitPoints;

        private void Start()
        {
            _transform = transform;
        }

        private void OnEnable()
        {
            EventDispatcher.FloatEventHandler += FloatEventHandler;
        }

        private void OnDisable()
        {
            EventDispatcher.FloatEventHandler -= FloatEventHandler;
        }

        private void FloatEventHandler(Transform originator, Transform target, string message, float value)
        {
            if ((target == _transform) && (message == EventMessage.Inflict_Damage))
            {
                HandleDamageTaken(value);
            }
        }

        private void HandleDamageTaken(float damageTaken)
        {
            HitPoints -= damageTaken;
            if (HitPoints <= 0.0f)
            {
                // TODO: Pop the chest
                Debug.Log("POP! Treasure everywhere...");
            }
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.tag == Constants.Rush_Collider_Tag)
            {
                HandleDamageTaken(HitPoints);
            }
            else
            {
                EventDispatcher.FireEvent(_transform, collider.transform, EventMessage.Hit_Trigger_Collider);
            }
        }

        private void Update()
        {
            Debug.Log(GetComponent<Rigidbody>().velocity.y);
        }
    }
}