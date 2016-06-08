using UnityEngine;
using Assets.Scripts.EventHandling;

namespace Assets.Scripts.Gameplay.Chests.Contents.PowerUps
{
    public class PowerUpLifeCycle : ChestItemLifeCycle
    {
        private Transform _transform;

        public PowerUpEffect Effect;
        public float Value;
        public float Duration;

        public void InitializeComponents()
        {
            _transform = transform;

            InitializeComponents(HandleCollection);
        }

        private void HandleCollection(Transform collectedBy)
        {
            EventDispatcher.FireEvent(_transform, collectedBy, EventMessage.Apply_Power_Up_Prefix + Effect.ToString(), Value / 100.0f);

            if (Duration > 0)
            {
                EventDispatcher.FireEvent(_transform, collectedBy, EventMessage.Start_Power_Up_Timer_Prefix + Effect.ToString(), Duration);
            }
        }
    }
}