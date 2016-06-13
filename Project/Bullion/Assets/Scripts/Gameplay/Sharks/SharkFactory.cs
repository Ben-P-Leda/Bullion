using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Generic;
using Assets.Scripts.EventHandling;

namespace Assets.Scripts.Gameplay.Sharks
{
    public class SharkFactory : MonoBehaviour
    {
        private ObjectPool _sharkPool;
        private Terrain _terrain;
        private Vector3[] _startOffsets;

        public GameObject SharkPrefab;

        private void Start()
        {
            _sharkPool = new ObjectPool(transform, Shark_Pool_Size, CreateSharkForPool, null);
            _terrain = Terrain.activeTerrain;

            CreateStartOffsets();
        }

        private GameObject CreateSharkForPool()
        {
            GameObject shark = (GameObject)Instantiate(SharkPrefab);
            shark.GetComponent<SharkMovement>().InitializeComponents();

            return shark;
        }

        private void CreateStartOffsets()
        {
            List<Vector3> startOffsets = new List<Vector3>();
            float step = 360.0f / Start_Offset_Count;
            float angle = 0.0f;
            while (angle < 360.0f)
            {
                startOffsets.Add(Quaternion.Euler(0.0f, angle, 0.0f) * Vector3.right * Constants.Shark_Maximum_Trigger_Distance);
                angle += step;
            }
            _startOffsets = startOffsets.ToArray();
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
            GameObject attackingShark = _sharkPool.GetFirstAvailableObject();
            Vector3 startPosition = GetSharkStartPosition(target.position);

            if (startPosition != Vector3.zero)
            {
                attackingShark.SetActive(true);
                attackingShark.GetComponent<SharkMovement>().SetForAttack(target, startPosition);
            }
        }

        private Vector3 GetSharkStartPosition(Vector3 targetPosition)
        {
            Vector3 startPosition = Vector3.zero;

            int offset = Random.Range(0, _startOffsets.Length - 1);
            for (int i = 0; ((i < _startOffsets.Length) && (startPosition == Vector3.zero)); i++)
            {
                Vector3 position = targetPosition + _startOffsets[(offset + i) % _startOffsets.Length];
                if (_terrain.SampleHeight(position) <= Minimum_Shark_Activation_Depth)
                {
                    startPosition = new Vector3(position.x, Constants.Shark_Swim_Depth, position.z);
                }
            }

            return startPosition;
        }

        private const float Start_Offset_Count = 18.0f;
        private const float Minimum_Shark_Activation_Depth = 0.0f;
        private const int Shark_Pool_Size = 20;
    }
}