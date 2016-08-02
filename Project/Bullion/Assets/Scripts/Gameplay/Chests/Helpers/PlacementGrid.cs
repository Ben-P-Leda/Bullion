using UnityEngine;
using Assets.Scripts.Gameplay.Environment;

namespace Assets.Scripts.Gameplay.Chests.Helpers
{
    public class PlacementGrid
    {
        private PlacementGridCell[][] _placementGrid;
        private float _gridWorldLeft;
        private float _gridWorldFront;
        private float _cellSize;

        public int Width { get; private set; }
        public int Depth { get; private set; }

        public PlacementGrid(ILandDataProvider landData, float cellSize, int neighboursToExclude)
        {
            _gridWorldLeft = landData.Left;
            _gridWorldFront = landData.Front;
            _cellSize = cellSize;

            SetGridDimensions(landData.Width, landData.Depth);
            CreateGridContainer(landData);
            MarkCellsUnavailableByGroundHeight(neighboursToExclude);
        }

        private void SetGridDimensions(float landWidth, float landDepth)
        {
            Width = (int)((landWidth - _gridWorldLeft) / _cellSize);
            Depth = (int)((landDepth - _gridWorldLeft) / _cellSize);
        }

        private void CreateGridContainer(ILandDataProvider landData)
        {
            _placementGrid = new PlacementGridCell[Width][];

            Vector3 positionOffset = new Vector3(landData.Left, 0.0f, landData.Front);
            for (int x = 0; x < Width; x++)
            {
                _placementGrid[x] = new PlacementGridCell[Depth];
                for (int z = 0; z < Depth; z++)
                {
                    _placementGrid[x][z] = new PlacementGridCell();

                    Vector3 terrainPosition = new Vector3(x + 0.5f, 0.0f, z + 0.5f) * _cellSize;

                    _placementGrid[x][z].Center = new Vector3(
                        terrainPosition.x + positionOffset.x, 
                        landData.HeightAtPosition(terrainPosition + positionOffset), 
                        terrainPosition.z + positionOffset.z);
                }
            }
        }

        private void MarkCellsUnavailableByGroundHeight(int neightboursToExclude)
        {
            for (int z = 0; z < Depth; z++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (_placementGrid[x][z].Center.y < Assignable_Cell_Minimum_Height)
                    {
                        MakeCellBlockUnavailable(x, z, neightboursToExclude, false);
                    }
                }
            }
        }

        private void MakeCellBlockUnavailable(int centerGridX, int centerGridZ, int neighbourCount, bool temporaryBlock)
        {
            for (int neighbourX = -neighbourCount; neighbourX <= neighbourCount; neighbourX++)
            {
                for (int neighbourZ = -neighbourCount; neighbourZ <= neighbourCount; neighbourZ++)
                {
                    int gridX = Mathf.Clamp(centerGridX + neighbourX, 0, _placementGrid.Length - 1);
                    int gridZ = Mathf.Clamp(centerGridZ + neighbourZ, 0, _placementGrid[gridX].Length - 1);

                    if (!temporaryBlock)
                    {
                        _placementGrid[gridX][gridZ].PermanentlyUnavailable = true;
                    }
                    else
                    {
                        _placementGrid[gridX][gridZ].TemporarilyUnavailable = true;
                    }
                }
            }
        }

        public void BlockCellsPermanently(Transform[] centerPoints, int neighbourCount)
        {
            MakeCellBlocksUnavailable(centerPoints, neighbourCount, false);
        }

        public void BlockCellsTemporarily(Transform[] centerPoints, int neighbourCount)
        {
            MakeCellBlocksUnavailable(centerPoints, neighbourCount, true);
        }

        public void BlockCellsAroundChest(GameObject chest)
        {
            PlacementGridReference gridPosition = GridFromWorldPosition(chest.transform.position);
            MakeCellBlockUnavailable(gridPosition.x, gridPosition.z, Neighbours_To_Exclude, true);
        }

        private PlacementGridReference GridFromWorldPosition(Vector3 worldPosition)
        {
            return new PlacementGridReference(
                (int)((worldPosition.x - _gridWorldLeft) / _cellSize),
                (int)((worldPosition.z - _gridWorldFront) / _cellSize));
        }

        private void MakeCellBlocksUnavailable(Transform[] blockCenters, int neighbourCount, bool temporaryBlock)
        {
            for (int i = 0; i < blockCenters.Length; i++)
            {
                PlacementGridReference gridPosition = GridFromWorldPosition(blockCenters[i].transform.position);
                MakeCellBlockUnavailable(gridPosition.x, gridPosition.z, neighbourCount, temporaryBlock);
            }
        }

        public void BlockClusterEdgeCells()
        {
            for (int x = 1; x < Width - 1; x++)
            {
                for (int z = 1; z < Depth - 1; z++)
                {
                    if ((_placementGrid[x][z].Available) && (GetAvailableNeighbourCount(x, z) < Cluster_Center_Neighbour_Count))
                    {
                        _placementGrid[x][z].CannotBeClusterCenter = true;
                    }
                }
            }
        }

        private int GetAvailableNeighbourCount(int gridX, int gridZ)
        {
            int availableNeighbours = -1;
            for (int x = gridX - 1; x <= gridX + 1; x++)
            {
                for (int z = gridZ - 1; z <= gridZ + 1; z++)
                {
                    if (_placementGrid[x][z].Available)
                    {
                        availableNeighbours++;
                    }
                }
            }

            return availableNeighbours;
        }

        public bool CellCanBeCenter(int x, int z)
        {
            return (_placementGrid[x][z].Available) && (!_placementGrid[x][z].CannotBeClusterCenter);
        }

        public void ClearTemporaryCellBlockages()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int z = 0; z < Depth; z++)
                {
                    _placementGrid[x][z].TemporarilyUnavailable = false;
                }
            }
        }

        public PlacementGridReference GetClusterCenter()
        {
            PlacementGridReference clusterCenter = null;

            int offset = Random.Range(0, (Width * Depth) - 1);
            for (int i = 0; ((clusterCenter == null) && (i < Width * Depth)); i++)
            {
                int gridX = (offset + i) % Width;
                int gridZ = ((offset + i) / Width) % Depth;

                if (CellCanBeCenter(gridX, gridZ))
                {
                    clusterCenter = new PlacementGridReference(gridX, gridZ);
                }
            }

            return clusterCenter;
        }

        public Vector3 GetChestStartPosition(PlacementGridReference gridPosition)
        {
            return _placementGrid[gridPosition.x][gridPosition.z].Center;
        }

        private const float Assignable_Cell_Minimum_Height = 0.1f;
        private const int Neighbours_To_Exclude = 2;
        private const int Cluster_Center_Neighbour_Count = 5;
    }
}