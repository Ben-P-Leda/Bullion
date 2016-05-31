using UnityEngine;

namespace Assets.Scripts.Gameplay.Treasure.Helpers
{
    public class PlacementGridReference
    {
        public int x { get; set; }
        public int z { get; set; }

        public PlacementGridReference()
        {
            x = 0;
            z = 0;
        }

        public PlacementGridReference(int x, int z)
        {
            this.x = x;
            this.z = z;
        }

        public static PlacementGridReference FromWorld(Vector3 worldPosition, float cellSize)
        {
            return new PlacementGridReference((int)(worldPosition.x / cellSize), (int)(worldPosition.z / cellSize));
        }

        public static Vector3 ToWorld(PlacementGridReference gridReference, float cellSize)
        {
            return ToWorld(gridReference.x, gridReference.z, cellSize);
        }

        public static Vector3 ToWorld(int gridX, int gridZ, float cellSize)
        {
            return new Vector3(gridX + (cellSize * 0.5f), 0.0f, gridZ + (cellSize * 0.5f)) * cellSize;
        }
    }
}
