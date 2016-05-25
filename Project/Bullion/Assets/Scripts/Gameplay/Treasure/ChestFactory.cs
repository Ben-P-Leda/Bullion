using UnityEngine;

namespace Assets.Scripts.Gameplay.Treasure
{
    public class ChestFactory : MonoBehaviour
    {
        private Terrain _terrain;

        public GameObject ChestPrefab;
        public float ChestHitPoints;

        private void Start()
        {
            _terrain = Terrain.activeTerrain;

            Vector3 xzPosition = new Vector3(22.0f, 0.0f, 20.0f);
            Vector3 startPosition = new Vector3(xzPosition.x, _terrain.SampleHeight(xzPosition) + Hidden_Chest_Vertical_Offset, xzPosition.z);
            GameObject chest = (GameObject)Instantiate(ChestPrefab);
            chest.SetActive(true);

            chest.GetComponent<ChestEntry>().SecondsToLaunch = 10.0f;
            chest.GetComponent<ChestEntry>().StartPosition = startPosition;
            chest.GetComponent<ChestCollisions>().HitPoints = ChestHitPoints;
        }

        private const float Hidden_Chest_Vertical_Offset = -2.0f;
    }
}