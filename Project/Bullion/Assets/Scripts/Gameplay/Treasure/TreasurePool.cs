using UnityEngine;
using Assets.Scripts.EventHandling;
using Assets.Scripts.Generic;

namespace Assets.Scripts.Gameplay.Treasure
{
    public class TreasurePool : MonoBehaviour
    {
        private ObjectPool _treasurePool;
        private Vector3 _nextSpawnPoint;

        public GameObject TreasurePrefab;
        public int MinimumPerChest;
        public int MaximumPerChest;

        private void Start()
        {
            _treasurePool = new ObjectPool(transform, Treasure_Pool_Capacity, CreateTreasureForPool, LaunchTreasure);
        }

        private GameObject CreateTreasureForPool()
        {
            return (GameObject)Instantiate(TreasurePrefab);
        }

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
                _nextSpawnPoint = originator.position;
                _treasurePool.AttemptMultipleActivation(Random.Range(MinimumPerChest, MaximumPerChest));
            }
        }

        private void LaunchTreasure(GameObject treasure)
        {
            treasure.transform.position = _nextSpawnPoint;
        }

        private const int Treasure_Pool_Capacity = 50;
    }
}