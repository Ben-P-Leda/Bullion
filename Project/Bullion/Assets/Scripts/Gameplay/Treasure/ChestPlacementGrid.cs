using UnityEngine;

namespace Assets.Scripts.Gameplay.Treasure
{
    public class ChestPlacementGrid
    {
        private CellState[][] _placementGrid;
        private float _cellSize;

        public int Width { get; private set; }
        public int Depth { get; private set; }

        public ChestPlacementGrid(Terrain terrain, float cellSize, int neighboursToExclude)
        {
            Width = (int)(terrain.terrainData.size.x / cellSize);
            Depth = (int)(terrain.terrainData.size.z / cellSize);

            _cellSize = cellSize;

            CreateGridContainer(Width, Depth);
            MarkCellsUnavailableByGroundHeight(Width, Depth, terrain, cellSize, neighboursToExclude);
        }

        private void CreateGridContainer(int width, int depth)
        {
            _placementGrid = new CellState[width][];
            for (int x = 0; x < width; x++)
            {
                _placementGrid[x] = new CellState[depth];
                for (int z = 0; z < depth; z++)
                {
                    _placementGrid[x][z] = CellState.Unassigned;
                }
            }
        }

        private void MarkCellsUnavailableByGroundHeight(int width, int depth, Terrain terrain, float cellSize, int neightboursToExclude)
        {
            for (int z = 0; z < depth; z++)
            {
                for (int x = 0; x < width; x++)
                {
                    Vector3 terrainPosition = new Vector3((x + (cellSize * 0.5f)) * cellSize, 0.0f, (z + (cellSize * 0.5f)) * cellSize);

                    if (Terrain.activeTerrain.SampleHeight(terrainPosition) >= Assignable_Cell_Minimum_Height)
                    {
                        SetGridCellStateIfNotPermanentlyUnavailable(x, z, CellState.Available);
                    }
                    else
                    {
                        MakeCellBlockUnavailable(x, z, neightboursToExclude);
                    }
                }
            }
        }

        private void SetGridCellStateIfNotPermanentlyUnavailable(int gridX, int gridZ, CellState gridCellState)
        {
            if (_placementGrid[gridX][gridZ] != CellState.PermanentlyUnavailable)
            {
                _placementGrid[gridX][gridZ] = gridCellState;
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
                    _placementGrid[gridX][gridZ] = CellState.PermanentlyUnavailable;
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

        public CellState GetCellState(int x, int z)
        {
            return _placementGrid[x][z];
        }

        public enum CellState
        {
            Unassigned = 0,
            TemporarilyUnavailable = 1,
            PermanentlyUnavailable = 2,
            Available = 3
        }

        private const float Assignable_Cell_Minimum_Height = 1.2f;
        private const int Neighbours_To_Exclude = 2;
    }
}