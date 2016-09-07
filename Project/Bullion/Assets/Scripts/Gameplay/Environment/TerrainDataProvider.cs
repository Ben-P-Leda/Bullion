using UnityEngine;

namespace Assets.Scripts.Gameplay.Environment
{
    public class TerrainDataProvider : MonoBehaviour, ILandDataProvider
    {
        private Terrain _terrain = null;

        public float Left { get { return 0.0f; } }
        public float Front { get { return 0.0f; } }

        public float Width
        {
            get
            {
                if (_terrain == null)
                {
                    _terrain = Terrain.activeTerrain;
                }

                return _terrain.terrainData.size.x;
            }
        }

        public float Depth
        {
            get
            {
                if (_terrain == null)
                {
                    _terrain = Terrain.activeTerrain;
                }

                return _terrain.terrainData.size.z;
            }
        }

        public float HeightAtPosition(Vector3 position)
        {
            if (_terrain == null)
            {
                _terrain = Terrain.activeTerrain;
            }

            return _terrain.SampleHeight(position) - 1.0f;
        }
    }
}