using UnityEngine;

namespace Assets.Scripts.Gameplay.Environment
{
    public class LandModelDataProvider : MonoBehaviour, ILandDataProvider
    {
        private MeshCollider _primaryLandMassMeshCollider = null;
        private float _left;
        private float _front;
        private float _width;
        private float _depth;

        public GameObject PrimaryLandMass;

        private MeshCollider PrimaryLandMassMeshCollider
        {
            get
            {
                EnsureDataReady();
                return _primaryLandMassMeshCollider;
            }
        }

        public float Left
        {
            get
            {
                EnsureDataReady();
                return _left;
            }
        }

        public float Front
        {
            get
            {
                EnsureDataReady();
                return _front;
            }
        }

        public float Width
        {
            get
            {
                EnsureDataReady();
                return _width;
            }
        }

        public float Depth
        {
            get
            {
                EnsureDataReady();
                return _depth;
            }
        }

        public float HeightAtPosition(Vector3 position)
        {
            RaycastHit rayHitData;
            Ray ray = new Ray(new Vector3(position.x, Ray_Ceiling, position.z), Vector3.down);

            if (PrimaryLandMassMeshCollider.Raycast(ray, out rayHitData, Ray_Ceiling * 2.0f))
            {
                return rayHitData.point.y;
            }
            else
            {
                return -999.999f;
            } 
        }

        private void EnsureDataReady()
        {
            if (_primaryLandMassMeshCollider == null)
            {
                _left = 0;
                _front = 0;

                float right = 0;
                float back = 0;

                _primaryLandMassMeshCollider = PrimaryLandMass.GetComponent<MeshCollider>();

                for (int i = 0; i < _primaryLandMassMeshCollider.sharedMesh.vertexCount; i++)
                {
                    _left = Mathf.Min(_primaryLandMassMeshCollider.sharedMesh.vertices[i].x, _left);
                    _front = Mathf.Min(_primaryLandMassMeshCollider.sharedMesh.vertices[i].z, _front);

                    right = Mathf.Max(_primaryLandMassMeshCollider.sharedMesh.vertices[i].x, right);
                    back = Mathf.Max(_primaryLandMassMeshCollider.sharedMesh.vertices[i].z, back);
                }

                _width = right - _left;
                _depth = back - _front;
            }
        }

        private const float Ray_Ceiling = 10.0f;
    }
}