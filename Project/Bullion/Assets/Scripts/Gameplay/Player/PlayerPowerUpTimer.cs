using UnityEngine;
using Assets.Scripts.EventHandling;
using Assets.Scripts.Configuration;
using Assets.Scripts.Gameplay.Player.Interfaces;
using Assets.Scripts.Gameplay.Player.Support;

namespace Assets.Scripts.Gameplay.Player
{
    public class PlayerPowerUpTimer : MonoBehaviour, IModifiable
    {
        private Transform _transform;
        private Transform _rushColliderTransform;

        private float _powerUpRemainingDuration;

        public CharacterConfigurationModifier ConfigurationModifier { private get; set; }

        private void Start()
        {
            _transform = transform;
            _rushColliderTransform = _transform.FindChild("Rush Collider");

            _powerUpRemainingDuration = 0.0f;
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
            if (message.StartsWith(EventMessage.Start_Power_Up_Timer_Prefix))
            {
                if (target == _rushColliderTransform)
                {
                    EventDispatcher.FireEvent(originator, _transform, message, value);
                }
                else if (target == _transform)
                {
                    _powerUpRemainingDuration = value;
                }
            }
        }

        private void FixedUpdate()
        {
            if (_powerUpRemainingDuration > 0.0f)
            {
                _powerUpRemainingDuration -= Time.deltaTime;
                if (_powerUpRemainingDuration <= 0.0f)
                {
                    ConfigurationModifier.ResetPowerUpEffects();
                }
            }
        }
    }
}
