using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Gameplay.Treasure
{
    public class ChestFactory : MonoBehaviour
    {
        private ChestPlacementGrid _placementGrid;
        private GameObject[] _chestPool;
        private List<Transform> _playerTransforms = new List<Transform>();

        public GameObject ChestPrefab;
        public float ChestHitPoints;
        public int LowPointMargin;
        public int ObstructionMargin;
        public int StartPointMargin;
        public int CharacterMargin;

        public List<Transform> Players { get { return _playerTransforms; } }

        private void Start()
        {
            InitialisePlacementGrid();

            CreateTestingDisplay();


            //InitialiseChestPool();

            //AttemptClusterSpawn();
        }

        private void InitialisePlacementGrid()
        {
            _placementGrid = new ChestPlacementGrid(Terrain.activeTerrain, Grid_Cell_Size, LowPointMargin);

            Transform[] obstructionPositions = GetObstructionPositions();
            _placementGrid.MakeCellBlocksUnavailable(obstructionPositions, ObstructionMargin);

            _placementGrid.MakeCellBlocksUnavailable(_playerTransforms.ToArray(), StartPointMargin);

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


        // FUNCTIONS FOR TESTING PLACEMENT CENTERS

        private GameObject[][] _boxes;
        private void CreateTestingDisplay()
        {
            _boxes = new GameObject[_placementGrid.Width][];

            for (int x = 0; x < _placementGrid.Width; x++)
            {
                _boxes[x] = new GameObject[_placementGrid.Depth];

                for (int z = 0; z < _placementGrid.Depth; z++)
                {
                    if (_placementGrid.CellCanBeCenter(x, z))
                    {
                        GameObject box = (GameObject)Instantiate(ChestPrefab);
                        box.transform.position = new Vector3(x + 0.5f, 4.0f, z + 0.5f);
                        box.SetActive(false);

                        _boxes[x][z] = box;
                    }
                }
            }
        }

        private float _timeToNextUpdate = 5;
        private Rect _displayArea = new Rect(300, 0, 300, 100);
        private int _selectedX = 22;
        private int _selectedZ = 20;
        private float _angle = 0.0f;
        private void Update()
        {
            _timeToNextUpdate -= Time.deltaTime;
            if (_timeToNextUpdate < 0)
            {
                _timeToNextUpdate = 10.0f;

                _placementGrid.ClearTemporaryBlocks();
                for (int i = 0; i < _playerTransforms.Count; i++)
                {
                    _placementGrid.UpdateTemporaryBlocking(_playerTransforms[i], CharacterMargin);
                }

                _angle = 0.0f;
                UpdateSelectedBox();

                Vector3 newBox = _placementGrid.GetClusterCenter();
                _selectedX = (int)newBox.x;
                _selectedZ = (int)newBox.z;

                UpdateTestingDisplay();
            }

            _angle += 1.0f;
            UpdateSelectedBox();
        }

        private void UpdateSelectedBox()
        {
            if (_boxes[_selectedX][_selectedZ] != null)
            {
                _boxes[_selectedX][_selectedZ].transform.Rotate(0, _angle, 0);
                _boxes[_selectedX][_selectedZ].transform.position = new Vector3(
                    _boxes[_selectedX][_selectedZ].transform.position.x,
                    4.0f + Mathf.Sin(_angle/ 10.0f),
                    _boxes[_selectedX][_selectedZ].transform.position.z);
            }
        }

        private void UpdateTestingDisplay()
        {
            for (int x = 0; x < _placementGrid.Width; x++)
            {
                for (int z = 0; z < _placementGrid.Depth; z++)
                {
                    if (_boxes[x][z] != null)
                    {
                        _boxes[x][z].SetActive(_placementGrid.CellCanBeCenter(x, z));
                    }
                }
            }
        }

        private void OnGUI()
        {
            GUI.Label(_displayArea, "Update in " + _timeToNextUpdate + "\nRotation: " + _angle);
        }


        // ACTUAL CHEST PLACEMENT ON HOLD WHILE TESTING TEMPORARY CELL BLOCKING

        //private void InitialiseChestPool()
        //{
        //    _chestPool = new GameObject[Chest_Pool_Capacity];
        //    for (int i = 0; i < Chest_Pool_Capacity; i++)
        //    {
        //        _chestPool[i] = (GameObject)Instantiate(ChestPrefab);
        //        _chestPool[i].transform.parent = transform;
        //    }
        //}

        //private void AttemptClusterSpawn()
        //{
        //    if (SufficientChestsAvailableInPool())
        //    {
        //        Vector3 clusterCenterCell = _placementGrid.GetClusterCenter();
        //        Vector3 offset = GetClusterDirection();
        //    }
        //}

        //private bool SufficientChestsAvailableInPool()
        //{
        //    int availableChests = 0;
        //    for (int i = 0; (i < Chest_Pool_Capacity) && (availableChests < Chests_In_Cluster); i++)
        //    {
        //        if (!_chestPool[i].activeInHierarchy)
        //        {
        //            availableChests++;
        //        }
        //    }

        //    return availableChests >= Chests_In_Cluster;
        //}

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