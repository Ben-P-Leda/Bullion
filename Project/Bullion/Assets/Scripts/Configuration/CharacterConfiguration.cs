using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Gameplay;
using Assets.Scripts.EventHandling;

namespace Assets.Scripts.Configuration
{
    public class CharacterConfiguration
    {
        public string Name { get; set; }
        public float HipShoulderDistance { get; set; }
        public float AliveMovementSpeed { get; set; }
        public float DeadMovementSpeed { get; set; }
        public float RushMovementSpeed { get; set; }
        public float RushDuration { get; set; }
        public float RushDamage { get; set; }
        public int ComboStepCount { get; set; }
        public float[] ComboStepDamage { get; set; }
        public float MaximumHealth { get; set; }
        public float RushRechargeSpeed { get; set; }
        public Color RespawnPointColour { get; set; }

        public float TreasureValue { get; set; }

        private Dictionary<string, float> _powerUpStatModifiers;
        private List<string> _powerUpStatModifierKeys;

        public CharacterConfiguration()
        {
            TreasureValue = 0.0f;

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
            for (int i = 0; i < _powerUpStatModifierKeys.Count; i++)
            {
                _powerUpStatModifiers[_powerUpStatModifierKeys[i]] = 0.0f;
            }

            AddKeyIfNotPresent(powerUpEventMessage);
            _powerUpStatModifiers[powerUpEventMessage] = effectValue;
        }
    }
}