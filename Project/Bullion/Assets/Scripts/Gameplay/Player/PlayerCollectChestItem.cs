using UnityEngine;
using Assets.Scripts.Configuration;
using Assets.Scripts.EventHandling;
using Assets.Scripts.Gameplay.Player.Interfaces;
using Assets.Scripts.Gameplay.Player.Support;

namespace Assets.Scripts.Gameplay.Player.ChestItemCollection
{
    public class PlayerCollectChestItem : MonoBehaviour, IConfigurable, IModifiable
    {
        private Transform _transform;
        private Transform _rushColliderTransform;

        public CharacterConfiguration Configuration { private get; set; }
        public CharacterConfigurationModifier ConfigurationModifier { private get; set; }

        private void Start()
        {
            _transform = transform;
            _rushColliderTransform = _transform.FindChild("Rush Collider");
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
            if (ItemCollectedByMe(target))
            {
                if (message == EventMessage.Collect_Treasure)
                {
                    Configuration.TreasureValue += value;
                    EventDispatcher.FireEvent(_transform, _transform, EventMessage.Update_Treasure, Configuration.TreasureValue);
                }
                else if (message.StartsWith(EventMessage.Apply_Power_Up_Prefix))
                {
                    ApplyPowerUp(message, value);
                }
            }
        }

        private bool ItemCollectedByMe(Transform collectedBy)
        {
            if (collectedBy == _transform) { return true; }
            if (collectedBy == _rushColliderTransform) { return true; }

            return false;
        }

        private void ApplyPowerUp(string eventMessage, float powerUpValue)
        {
            ConfigurationModifier.SetPowerUpModifier(eventMessage, powerUpValue);
        }
    }
}
