using UnityEngine;
using Assets.Scripts.EventHandling;

namespace Assets.Scripts.Gameplay.Chests
{
    public class ChestCollisions : MonoBehaviour
    {
        private Transform _transform;
        private GameObject _gameObject;

        public float HitPoints { private get; set; }

        private void Start()
        {
            _transform = transform;
            _gameObject = _transform.gameObject;
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
                EventDispatcher.FireEvent(_transform, _transform, EventMessage.Chest_Destroyed);
                _gameObject.SetActive(false);
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
    }
}