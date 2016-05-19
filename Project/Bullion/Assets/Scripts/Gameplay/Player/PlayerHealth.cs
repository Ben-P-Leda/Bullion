using UnityEngine;
using Assets.Scripts.Configuration;
using Assets.Scripts.EventHandling;

namespace Assets.Scripts.Gameplay.Player
{
    public class PlayerHealth : MonoBehaviour, IConfigurable
    {
        private Transform _transform;
        private float _remainingHealth;

        public CharacterConfiguration Configuration { private get; set; }
        public GameObject RespawnPoint { private get; set; }

        private void Start()
        {
            _transform = transform;

            _remainingHealth = Configuration.MaximumHealth;
        }

        private void OnEnable()
        {
            EventDispatcher.MessageEventHandler += MessageEventHandler;
            EventDispatcher.FloatEventHandler += FloatEventHandler;
        }

        private void OnDisable()
        {
            EventDispatcher.MessageEventHandler -= MessageEventHandler;
            EventDispatcher.FloatEventHandler -= FloatEventHandler;
        }

        private void MessageEventHandler(Transform originator, Transform target, string message)
        {
            if ((target == _transform) && (message == EventMessage.Respawn))
            {
                _remainingHealth = Configuration.MaximumHealth;
                EventDispatcher.FireEvent(_transform, _transform, EventMessage.Update_Health, _remainingHealth);
            }
        }

        private void FloatEventHandler(Transform originator, Transform target, string message, float value)
        {
            if ((target == _transform) && (message == EventMessage.Inflict_Damage))
            {
                HandleDamageTaken(value);
            }
        }

        private void HandleDamageTaken(float damageInflicted)
        {
            if (_remainingHealth > 0.0f)
            {
                _remainingHealth -= damageInflicted;
                if (_remainingHealth <= 0.0f)
                {
                    EventDispatcher.FireEvent(_transform, _transform, EventMessage.Has_Died);
                }
                EventDispatcher.FireEvent(_transform, _transform, EventMessage.Update_Health, _remainingHealth);
            }
        }
    }
}
