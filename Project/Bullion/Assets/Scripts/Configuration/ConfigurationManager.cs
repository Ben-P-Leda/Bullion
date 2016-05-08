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
                MovementSpeed = 5.0f
            });

            _characters.Add("Green", new CharacterConfiguration()
            {
                MovementSpeed = 5.5f
            });

            _characters.Add("Blue", new CharacterConfiguration()
            {
                MovementSpeed = 5.25f
            });

            _characters.Add("Purple", new CharacterConfiguration()
            {
                MovementSpeed = 4.5f
            });
        }
    }
}