using UnityEngine;
using Assets.Scripts.EventHandling;

namespace Assets.Scripts.Gameplay.Chests
{
    public class ChestDestruction : MonoBehaviour
    {
        public float PowerUpPercentChance;

        private void OnEnable()
        {
            EventDispatcher.MessageEventHandler += MessageEventHandler;
        }

        private void OnDisable()
        {
            EventDispatcher.MessageEventHandler -= MessageEventHandler;
        }

        private void MessageEventHandler(Transform originator, Transform target, string message)
        {
            if (message == EventMessage.Chest_Destroyed)
            {
                string itemSpawnMessage = (Random.Range(0.0f, 100.0f) <= PowerUpPercentChance)
                    ? EventMessage.Spawn_Power_Up
                    : EventMessage.Spawn_Treasure;

                EventDispatcher.FireEvent(originator, target, itemSpawnMessage);
            }
        }
    }
}