using System.Collections.Generic;

namespace Assets.Scripts.Configuration
{
    public class ConfigurationManager
    {
        private static ConfigurationManager _instance = null;

        public static CharacterConfiguration GetCharacterConfiguration(string characterName)
        {
            _instance = _instance == null ? new ConfigurationManager() : _instance;
            return _instance._characters[characterName];
        }

        private Dictionary<string, CharacterConfiguration> _characters = null;

        public ConfigurationManager()
        {
            CreateCharacterConfigurations();
        }

        private void CreateCharacterConfigurations()
        {
            _characters = new Dictionary<string, CharacterConfiguration>();

            _characters.Add("Red", new CharacterConfiguration()
            {
                Name = "Red Player",
                MovementSpeed = 5.0f,
                ComboStepCount = 3,
                ComboStepDamage = new float[] {10, 10, 10},
                MaximumHealth = 100.0f
            });

            _characters.Add("Green", new CharacterConfiguration()
            {
                Name = "Green Player",
                MovementSpeed = 5.5f,
                ComboStepCount = 2,
                ComboStepDamage = new float[] { 20, 20 },
                MaximumHealth = 85.0f
            });

            _characters.Add("Blue", new CharacterConfiguration()
            {
                Name = "Blue Player",
                MovementSpeed = 5.25f,
                ComboStepCount = 3,
                ComboStepDamage = new float[] { 10, 15, 20 },
                MaximumHealth = 75.0f
            });

            _characters.Add("Purple", new CharacterConfiguration()
            {
                Name = "Purple Player",
                MovementSpeed = 4.5f,
                ComboStepCount = 2,
                ComboStepDamage = new float[] { 12.5f, 12.5f },
                MaximumHealth = 115.0f
            });
        }
    }
}