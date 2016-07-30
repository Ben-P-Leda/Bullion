using UnityEngine;

namespace Assets.Scripts.Gameplay.Environment
{
    public class TerrainDataProvider : MonoBehaviour, ILandDataProvider
    {
        private Terrain _terrain = null;

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

            return _terrain.SampleHeight(position);
        }
    }
}