using UnityEngine;
using Assets.Scripts.Configuration;
using Assets.Scripts.EventHandling;
using Assets.Scripts.Gameplay.Player.Interfaces;

namespace Assets.Scripts.Gameplay.Player.ChestItemCollection
{
    public class PlayerPowerUp : MonoBehaviour, IConfigurable
    {
        private Transform _transform;
        private PlayerChestItemCollectionDetector _collectionDetector;

        public CharacterConfiguration Configuration { private get; set; }

        private void OnEnable()
        {
            if (_collectionDetector == null)
            {
                _transform = transform;
                _collectionDetector = new PlayerChestItemCollectionDetector(
                    _transform, _transform.FindChild("Rush Collider"), EventMessage.Collect_Power_Up, HandlePowerUpCollection);
            }

            _collectionDetector.WireUpEventHandlers();
        }

        private void OnDisable()
        {
            _collectionDetector.UnhookEventHandlers();
        }

        private void HandlePowerUpCollection(float powerUpEffectAsFloat)
        {
            PowerUpEffect powerUpEffect = (PowerUpEffect)powerUpEffectAsFloat;
            
            if (powerUpEffect != PowerUpEffect.HealthRestore)
            {
                PowerUpConfiguration config = PowerUpConfigurationManager.GetPowerUpConfiguration(powerUpEffect);
                Configuration.SetPowerUpModifier(powerUpEffect, config.Value);

                // TODO: Start the clock, any effects we want
            }
        }
    }
}
