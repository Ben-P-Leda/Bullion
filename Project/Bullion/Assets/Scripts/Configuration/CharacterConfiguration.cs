using UnityEngine;
using System.Collections.Generic;

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

        private Dictionary<PowerUpEffect, float> _powerUpStatModifiers;
        private List<PowerUpEffect> _powerUpStatModifierKeys;

        public CharacterConfiguration()
        {
            _powerUpStatModifiers = new Dictionary<PowerUpEffect, float>();
            _powerUpStatModifierKeys = new List<PowerUpEffect>();
        }

        public float GetPowerUpModifier(PowerUpEffect powerUp)
        {
            AddKeyIfNotPresent(powerUp);
            return 1.0f + _powerUpStatModifiers[powerUp];
        }

        private void AddKeyIfNotPresent(PowerUpEffect powerUp)
        {
            if (!_powerUpStatModifiers.ContainsKey(powerUp))
            {
                _powerUpStatModifiers.Add(powerUp, 0.0f);
            }

            if (!_powerUpStatModifierKeys.Contains(powerUp))
            {
                _powerUpStatModifierKeys.Add(powerUp);
            }
        }

        public void SetPowerUpModifier(PowerUpEffect powerUp, float effectValue)
        {
            for (int i = 0; i < _powerUpStatModifierKeys.Count; i++)
            {
                _powerUpStatModifiers[_powerUpStatModifierKeys[i]] = 0.0f;
            }

            AddKeyIfNotPresent(powerUp);
            _powerUpStatModifiers[powerUp] = effectValue;
        }
    }
}