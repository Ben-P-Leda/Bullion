using System.Collections.Generic;
using Assets.Scripts.EventHandling;

namespace Assets.Scripts.Gameplay.Player.Support
{
    public class CharacterConfigurationModifier
    {
        private Dictionary<string, float> _powerUpStatModifiers;
        private List<string> _powerUpStatModifierKeys;

        public CharacterConfigurationModifier()
        {
            _powerUpStatModifiers = new Dictionary<string, float>();
            _powerUpStatModifierKeys = new List<string>();
        }

        public float GetPowerUpModifier(PowerUpEffect powerUp)
        {
            string eventMessageKey = EventMessage.Apply_Power_Up_Prefix + powerUp.ToString();
            AddKeyIfNotPresent(eventMessageKey);
            return 1.0f + _powerUpStatModifiers[eventMessageKey];
        }

        private void AddKeyIfNotPresent(string powerUpEventMessage)
        {
            if (!_powerUpStatModifiers.ContainsKey(powerUpEventMessage))
            {
                _powerUpStatModifiers.Add(powerUpEventMessage, 0.0f);
            }

            if (!_powerUpStatModifierKeys.Contains(powerUpEventMessage))
            {
                _powerUpStatModifierKeys.Add(powerUpEventMessage);
            }
        }

        public void SetPowerUpModifier(string powerUpEventMessage, float effectValue)
        {
            ResetPowerUpEffects();
            AddKeyIfNotPresent(powerUpEventMessage);
            _powerUpStatModifiers[powerUpEventMessage] = effectValue;
        }

        public void ResetPowerUpEffects()
        {
            for (int i = 0; i < _powerUpStatModifierKeys.Count; i++)
            {
                _powerUpStatModifiers[_powerUpStatModifierKeys[i]] = 0.0f;
            }
        }
    }
}