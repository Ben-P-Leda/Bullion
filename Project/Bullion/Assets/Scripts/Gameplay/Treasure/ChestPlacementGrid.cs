using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Gameplay.Treasure
{
    public class ChestPlacementGrid
    {
        private ChestPlacementGridCell[][] _placementGrid;
        private float _cellSize;

        public int Width { get; private set; }
        public int Depth { get; private set; }

        public ChestPlacementGrid(Terrain terrain, float cellSize, int neighboursToExclude)
        {
            Width = (int)(terrain.terrainData.size.x / cellSize);
            Depth = (int)(terrain.terrainData.size.z / cellSize);

            _cellSize = cellSize;

            CreateGridContainer();
            MarkCellsUnavailableByGroundHeight(terrain, neighboursToExclude);
        }

        private void CreateGridContainer()
        {
            _placementGrid = new ChestPlacementGridCell[Width][];
            for (int x = 0; x < Width; x++)
            {
                _placementGrid[x] = new ChestPlacementGridCell[Depth];
                for (int z = 0; z < Depth; z++)
                {
                    _placementGrid[x][z] = new ChestPlacementGridCell();
                }
            }
        }

        private void MarkCellsUnavailableByGroundHeight(Terrain terrain, int neightboursToExclude)
        {
            for (int z = 0; z < Depth; z++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Vector3 terrainPosition = new Vector3((x + (_cellSize * 0.5f)) * _cellSize, 0.0f, (z + (_cellSize * 0.5f)) * _cellSize);

                    if (Terrain.activeTerrain.SampleHeight(terrainPosition) < Assignable_Cell_Minimum_Height)
                    {
                        MakeCellBlockUnavailable(x, z, neightboursToExclude);
                    }
                }
            }
        }

        private void MakeCellBlockUnavailable(int centerGridX, int centerGridZ, int neighbourCount)
        {
            for (int neighbourX = -neighbourCount; neighbourX <= neighbourCount; neighbourX++)
            {
                for (int neighbourZ = -neighbourCount; neighbourZ <= neighbourCount; neighbourZ++)
                {
                    int gridX = (int)Mathf.Clamp(centerGridX + neighbourX, 0, _placementGrid.Length - 1);
                    int gridZ = (int)Mathf.Clamp(centerGridZ + neighbourZ, 0, _placementGrid[gridX].Length - 1);
                    _placementGrid[gridX][gridZ].PermanentlyUnavailable = true;
                }
            }
        }

        public void MakeCellBlocksUnavailable(Vector3[] blockCenters, int neighbourCount)
        {
            for (int i = 0; i < blockCenters.Length; i++)
            {
                int gridX = (int)(blockCenters[i].x / _cellSize);
                int gridZ = (int)(blockCenters[i].z / _cellSize);

                MakeCellBlockUnavailable(gridX, gridZ, neighbourCount);
            }
        }

        public void BlockClusterEdgeCells()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int z = 0; z < Depth; z++)
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

        public bool CellIsAvailable(int x, int z)
        {
            return _placementGrid[x][z].Available;
        }

        public bool CellCanBeCenter(int x, int z)
        {
            return (_placementGrid[x][z].Available) && (!_placementGrid[x][z].CannotBeClusterCenter);
        }

        public Vector3 GetClusterCenter()
        {
            return new Vector3(22, 0, 20);
        }

        private const float Assignable_Cell_Minimum_Height = 1.2f;
        private const int Neighbours_To_Exclude = 2;
        private const int Cluster_Center_Neighbour_Count = 5;
    }
}