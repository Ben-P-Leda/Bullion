using UnityEngine;
using Assets.Scripts.Generic;
using Assets.Scripts.EventHandling;
using Assets.Scripts.Gameplay.Environment;
using Assets.Scripts.Gameplay.Chests.Helpers;

namespace Assets.Scripts.Gameplay.Chests
{
    public class ChestFactory : MonoBehaviour
    {
        private PlacementGrid _placementGrid;
        private ChestClusterCoordinator _clusterCoordinator;
        private ObjectPool _chestPool;
        private Transform[] _playerTransforms;
        private bool _roundInProgress;
        private float _timeToNextSpawn;

        public GameObject ChestPrefab;
        public float ChestHitPoints;
        public int LowPointMargin;
        public int ObstructionMargin;
        public int StartPointMargin;
        public int CharacterMargin;
        public float MinimumTimeBetweenSpawns;
        public float MaximumTimeBetweenSpawns;

        public void AddPlayerReference(int playerIndex, GameObject player)
        {
            if (_playerTransforms == null)
            {
                _playerTransforms = new Transform[Constants.Player_Count];
            }

            _playerTransforms[playerIndex] = player.transform;
        }

        private void Start()
        {
            _clusterCoordinator = new ChestClusterCoordinator();
            _chestPool = new ObjectPool(transform, Chest_Pool_Capacity, CreateChestForPool, ActivateChest);

            _roundInProgress = false;

            _timeToNextSpawn = 2.5f;

            InitialisePlacementGrid();
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
            if (message == EventMessage.Start_Round) { _roundInProgress = true; }
            if (message == EventMessage.End_Round) { _roundInProgress = false; }
        }

        private GameObject CreateChestForPool()
        {
            GameObject chest = (GameObject)Instantiate(ChestPrefab);
            chest.GetComponent<ChestEntry>().InitializeComponents();

            return chest;
        }

        private void ActivateChest(GameObject chestToActivate)
        {
            PlacementGridReference nextChestGridPosition = _clusterCoordinator.NextChestGridPosition;
            Vector3 chestWorldPosition = _placementGrid.GetChestStartPosition(nextChestGridPosition);
            _clusterCoordinator.PlaceChest(chestToActivate, chestWorldPosition, ChestHitPoints);
        }

        private void InitialisePlacementGrid()
        {
            ILandDataProvider landData = GameObject.Find("Land").GetComponent<ILandDataProvider>();
            Transform[] obstructionPositions = GetObstructionPositions();

            _placementGrid = new PlacementGrid(landData, Grid_Cell_Size, LowPointMargin);
            _placementGrid.BlockCellsPermanently(obstructionPositions, ObstructionMargin);
            _placementGrid.BlockCellsPermanently(_playerTransforms, StartPointMargin);
            _placementGrid.BlockClusterEdgeCells();
        }

        private Transform[] GetObstructionPositions()
        {
            GameObject obstructionContainer = GameObject.Find("Obstructions");
            int obstructionCount = obstructionContainer != null ? obstructionContainer.transform.childCount : 0;
            Transform[] obstructions = new Transform[obstructionCount];

            for (int i = 0; i < obstructionCount; i++)
            {
                obstructions[i] = obstructionContainer.transform.GetChild(i);
            }

            return obstructions;
        }

        private void Update()
        {
            if (_roundInProgress)
            {
                _timeToNextSpawn -= Time.deltaTime;
                if (_timeToNextSpawn <= 0.0f)
                {
                    UpdatePlacementGridBlockedCells();

                    if (_chestPool.GetAvailableObjectCount() >= ChestClusterCoordinator.Cluster_Size)
                    {
                        PlaceCluster();
                    }

                    _timeToNextSpawn = Random.Range(MinimumTimeBetweenSpawns, MaximumTimeBetweenSpawns);
                }
            }
        }

        private void UpdatePlacementGridBlockedCells()
        {
            _placementGrid.ClearTemporaryCellBlockages();
            _placementGrid.BlockCellsTemporarily(_playerTransforms, CharacterMargin);

            _chestPool.ApplyActionToActivePoolObjects(_placementGrid.BlockCellsAroundChest);
        }

        private void PlaceCluster()
        {
            PlacementGridReference clusterCenter = _placementGrid.GetClusterCenter();
            if (clusterCenter != null)
            {
                _clusterCoordinator.SetForPlacement(clusterCenter);
                _chestPool.AttemptMultipleActivation(ChestClusterCoordinator.Cluster_Size);
            }
        }

        private const float Grid_Cell_Size = 1.0f;
        private const int Chest_Pool_Capacity = 12;
    }
}