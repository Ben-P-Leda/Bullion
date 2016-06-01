using UnityEngine;
using Assets.Scripts.Gameplay.Treasure.Helpers;

namespace Assets.Scripts.Gameplay.Treasure
{
    public class ChestFactory : MonoBehaviour
    {
        private PlacementGrid _placementGrid;
        private GameObject[] _chestPool;
        private Transform[] _playerTransforms;
        private float _timeToNextSpawn;
        private ClusterContainer _clusterContainer;

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
            InitialisePlacementGrid();
            InitialiseChestPool();

            _clusterContainer = new ClusterContainer() { ChestHitPoints = this.ChestHitPoints };
            _timeToNextSpawn = 2.5f;
        }

        private void InitialisePlacementGrid()
        {
            Transform[] obstructionPositions = GetObstructionPositions();

            _placementGrid = new PlacementGrid(Terrain.activeTerrain, Grid_Cell_Size, LowPointMargin);
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

        private void InitialiseChestPool()
        {
            _chestPool = new GameObject[Chest_Pool_Capacity];
            for (int i = 0; i < Chest_Pool_Capacity; i++)
            {
                _chestPool[i] = (GameObject)Instantiate(ChestPrefab);
                _chestPool[i].transform.parent = transform;
            }
        }

        private void Update()
        {
            _timeToNextSpawn -= Time.deltaTime;
            if (_timeToNextSpawn <= 0.0f)
            {
                UpdatePlacementGridBlockedCells();

                if (ChestsAllocatedToClusterContainer())
                {
                    PlaceCluster();
                }

                _timeToNextSpawn = Random.Range(MinimumTimeBetweenSpawns, MaximumTimeBetweenSpawns);
            }
        }

        private void UpdatePlacementGridBlockedCells()
        {
            _placementGrid.ClearTemporaryCellBlockages();
            _placementGrid.BlockCellsTemporarily(_playerTransforms, CharacterMargin);

            for (int i = 0; i < Chest_Pool_Capacity; i++)
            {
                if (_chestPool[i].activeInHierarchy)
                {
                    _placementGrid.BlockCellsAroundChest(_chestPool[i]);
                }
            }
        }

        private bool ChestsAllocatedToClusterContainer()
        {
            _clusterContainer.AllocateChestsFromPool(_chestPool);
            return _clusterContainer.SufficientChestsAllocated;
        }

        private void PlaceCluster()
        {
            _clusterContainer.SetForPlacement(_placementGrid.GetClusterCenter());
      
            while (!_clusterContainer.PlacementComplete)
            {
                Vector3 chestWorldPosition = _placementGrid.GetChestStartPosition(_clusterContainer.NextChestCellPosition);
                _clusterContainer.PlaceNextChest(chestWorldPosition);
            }
        }

        // TESTING STUFF
        private Rect _displayArea = new Rect(300, 0, 300, 100);
        private void OnGUI()
        {
            GUI.Label(_displayArea, "Update in " + _timeToNextSpawn);
        }

        private const float Grid_Cell_Size = 1.0f;
        private const int Chest_Pool_Capacity = 10;
    }
}