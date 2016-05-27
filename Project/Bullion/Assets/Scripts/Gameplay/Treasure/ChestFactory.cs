using UnityEngine;
using Assets.Scripts.Gameplay;
using Assets.Scripts.Gameplay.Player;

namespace Assets.Scripts.Gameplay.Treasure
{
    public class ChestFactory : MonoBehaviour
    {
        private ChestPlacementGrid _placementGrid;

        public GameObject ChestPrefab;
        public float ChestHitPoints;
        public int LowPointMargin;
        public int ObstructionMargin;
        public int StartPointMargin;

        private void Start()
        {
            _placementGrid = new ChestPlacementGrid(Terrain.activeTerrain, 1.0f, LowPointMargin);

            Vector3[] obstructionPositions = GetObstructionPositions();
            _placementGrid.MakeCellBlocksUnavailable(obstructionPositions, ObstructionMargin);

            Vector3[] playerStartPoints = GetPlayerStartPoints();
            _placementGrid.MakeCellBlocksUnavailable(playerStartPoints, StartPointMargin);




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
                if (playerFactory.PlayerStartPoints.Length > 0)
                {
                    playerStartPoints[i] = new Vector3(playerFactory.PlayerStartPoints[i].x, 0.0f, playerFactory.PlayerStartPoints[i].z);
                }
                else
                {
                    playerStartPoints[i] = Vector3.zero;
                }
            }

            return playerStartPoints;
        }

        private const float Hidden_Chest_Vertical_Offset = -2.0f;
    }
}