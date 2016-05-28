using UnityEngine;
using Assets.Scripts.Gameplay.Player;

namespace Assets.Scripts.Gameplay.Treasure
{
    public class ChestFactory : MonoBehaviour
    {
        private ChestPlacementGrid _placementGrid;
        private GameObject[] _chestPool;

        public GameObject ChestPrefab;
        public float ChestHitPoints;
        public int LowPointMargin;
        public int ObstructionMargin;
        public int StartPointMargin;

        private void Start()
        {
            InitialisePlacementGrid();
            InitialiseChestPool();

            AttemptClusterSpawn();
        }

        private void InitialisePlacementGrid()
        {
            _placementGrid = new ChestPlacementGrid(Terrain.activeTerrain, Grid_Cell_Size, LowPointMargin);

            Vector3[] obstructionPositions = GetObstructionPositions();
            _placementGrid.MakeCellBlocksUnavailable(obstructionPositions, ObstructionMargin);

            Vector3[] playerStartPoints = GetPlayerStartPoints();
            _placementGrid.MakeCellBlocksUnavailable(playerStartPoints, StartPointMargin);

            _placementGrid.BlockClusterEdgeCells();
        }

        private Vector3[] GetObstructionPositions()
        {
            GameObject obstructionContainer = GameObject.Find("Obstructions");
            int obstructionCount = obstructionContainer != null ? obstructionContainer.transform.childCount : 0;
            Vector3[] obstructions = new Vector3[obstructionCount];

            for (int i = 0; i < obstructionCount; i++)
            {
                obstructions[i] = obstructionContainer.transform.GetChild(i).position;
            }

            return obstructions;
        }

        private Vector3[] GetPlayerStartPoints()
        {
            Vector3[] playerStartPoints = new Vector3[Constants.Player_Count];
            PlayerFactory playerFactory = GameObject.Find("Player Factory").GetComponent<PlayerFactory>();

            for (int i = 0; i < Constants.Player_Count; i++)
            {
                playerStartPoints[i] = playerFactory.PlayerStartPoints.Length > i
                    ? new Vector3(playerFactory.PlayerStartPoints[i].x, 0.0f, playerFactory.PlayerStartPoints[i].z)
                    : playerStartPoints[i] = Vector3.zero;
            }

            return playerStartPoints;
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


        private void AttemptClusterSpawn()
        {
            if (SufficientChestsAvailableInPool())
            {
                Vector3 clusterCenterCell = _placementGrid.GetClusterCenter();
                Vector3 offset = GetClusterDirection();
            }
        }

        private bool SufficientChestsAvailableInPool()
        {
            int availableChests = 0;
            for (int i = 0; (i < Chest_Pool_Capacity) && (availableChests < Chests_In_Cluster); i++)
            {
                if (!_chestPool[i].activeInHierarchy)
                {
                    availableChests++;
                }
            }

            return availableChests >= Chests_In_Cluster;
        }

        private Vector3 GetClusterDirection()
        {
            return Vector3.up;
        }

        private const float Hidden_Chest_Vertical_Offset = -2.0f;
        private const float Grid_Cell_Size = 1.0f;
        private const int Chest_Pool_Capacity = 21;
        private const int Chests_In_Cluster = 3;
    }
}