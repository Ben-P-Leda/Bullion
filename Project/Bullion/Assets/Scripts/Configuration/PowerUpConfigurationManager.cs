using System.Collections.Generic;

namespace Assets.Scripts.Configuration
{
    public class PowerUpConfigurationManager
    {
        private static PowerUpConfigurationManager _instance = null;

        public static void Initialize()
        {
            _instance = new PowerUpConfigurationManager();
        }

        public static PowerUpConfiguration GetPowerUpConfiguration(PowerUpEffect powerUpEffect)
        {
            if (_instance == null)
            {
                throw new System.Exception("PowerupFactory should call PowerUpConfigurationManager.Initialize on stage startup");
            }

            return _instance._powerUps.ContainsKey(powerUpEffect) ? _instance._powerUps[powerUpEffect] : null;
        }

        private Dictionary<PowerUpEffect, PowerUpConfiguration> _powerUps = null;

        public PowerUpConfigurationManager()
        {
            CreatePowerUpConfigurations();
        }

        private void CreatePowerUpConfigurations()
        {
            _powerUps = new Dictionary<PowerUpEffect, PowerUpConfiguration>();

            _powerUps.Add(PowerUpEffect.HealthRestore, new PowerUpConfiguration { Value = 20.0f, Duration = 0.0f, PercentageEffect = true });
            _powerUps.Add(PowerUpEffect.ComboDamageBoost, new PowerUpConfiguration { Value = 15.0f, Duration = 3.0f, PercentageEffect = true });
            _powerUps.Add(PowerUpEffect.SpeedBoost, new PowerUpConfiguration { Value = 10.0f, Duration = 6.0f, PercentageEffect = true });
        }
    }

    public enum PowerUpEffect
    {
        HealthRestore = 1,
        SpeedBoost = 2,
        ComboDamageBoost = 3
    }
}