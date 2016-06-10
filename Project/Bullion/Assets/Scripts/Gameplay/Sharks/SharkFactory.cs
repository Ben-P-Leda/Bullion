using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Generic;
using Assets.Scripts.EventHandling;

namespace Assets.Scripts.Gameplay.Sharks
{
    public class SharkFactory : MonoBehaviour
    {
        private ObjectPool _sharkPool;
        private Dictionary<Transform, GameObject> _attackingSharkLookup;

        public GameObject SharkPrefab;

        private void Start()
        {
            _sharkPool = new ObjectPool(transform, Constants.Player_Count, CreateSharkForPool, null);
            _attackingSharkLookup = new Dictionary<Transform, GameObject>();
        }

        private GameObject CreateSharkForPool()
        {
            GameObject shark = (GameObject)Instantiate(SharkPrefab);
            // Call any initialization here...

            return shark;
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
            if (message == EventMessage.Entered_Deep_Water)
            {
                StartSharkAttackRun(originator);
                Debug.Log(originator.name + " entered deep water - dispatching shark!");
            }
            else if (message == EventMessage.Left_Deep_Water)
            {
                Debug.Log(originator.name + " left deep water & escaped!");
            }
        }

        private void StartSharkAttackRun(Transform target)
        {
            GameObject attackingShark = GetAttackingShark(target);
        }

        private GameObject GetAttackingShark(Transform target)
        {
            if (!_attackingSharkLookup.ContainsKey(target))
            {
                _attackingSharkLookup.Add(target, _sharkPool.GetFirstAvailableObject());
            }

            if (!_attackingSharkLookup[transform].activeInHierarchy)
            {
                // set position and enable
            }

            return _attackingSharkLookup[transform];
        }
    }
}