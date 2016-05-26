using UnityEngine;

namespace Assets.Scripts.Gameplay.Treasure
{
    public class ChestFactory : MonoBehaviour
    {
        private ChestPlacementGrid _placementGrid;

        public GameObject ChestPrefab;
        public float ChestHitPoints;

        private void Start()
        {
            _placementGrid = new ChestPlacementGrid(Terrain.activeTerrain, 1.0f, 2);

            Transform[] fixedObstructions = GetFixedObstructions();
            _placementGrid.BlockOutFixedObstructions(fixedObstructions, 2);
            

            for (int x = 0; x<_placementGrid.Width; x++)
            {
                for (int z = 0; z<_placementGrid.Depth; z++)
                {
                    if (_placementGrid.GetCellState(x, z) == ChestPlacementGrid.CellState.Available)
                    {
                        GameObject box = (GameObject)Instantiate(ChestPrefab);
                        box.transform.position = new Vector3(x + 0.5f, 3.0f, z + 0.5f);
                    }
                }
            }
        }

        private Transform[] GetFixedObstructions()
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

        private const float Hidden_Chest_Vertical_Offset = -2.0f;
    }
}