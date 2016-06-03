using UnityEngine;
using Assets.Scripts.Configuration;
using Assets.Scripts.EventHandling;

namespace Assets.Scripts.Gameplay.Player
{
    public class PlayerTreasureCount : MonoBehaviour
    {
        private Transform _transform;
        private float _treasureValue;

        private void Start()
        {
            _transform = transform;

            _treasureValue = 0.0f;
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
            if ((target == _transform) && (message == EventMessage.Collect_Treasure))
            {
                _treasureValue += value;
                EventDispatcher.FireEvent(_transform, _transform, EventMessage.Update_Treasure, _treasureValue);
            }
        }
    }
}
