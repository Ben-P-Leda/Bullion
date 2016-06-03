using UnityEngine;

namespace Assets.Scripts.Gameplay.Treasure.Helpers
{
    public class ChestClusterCoordinator
    {
        private PlacementGridReference _direction;
        private float _revealDelay;
        private GameObject[] _chests;

        public PlacementGridReference NextChestGridPosition { get; private set; }

        public ChestClusterCoordinator()
        {
            _direction = new PlacementGridReference();
            NextChestGridPosition = new PlacementGridReference();

            _chests = new GameObject[Cluster_Size];
        }
        
        public void SetForPlacement(PlacementGridReference clusterCenter)
        {
            int seed = Random.Range(-1, 3);
            _direction.x = seed > -1 ? 1 : 0;
            _direction.z = Mathf.Max(-1, seed - 1);

            bool invert = Random.value > 0.5f;
            if (invert)
            {
                _direction.x = -_direction.x;
                _direction.z = -_direction.z;
            }

            NextChestGridPosition.x = clusterCenter.x - _direction.x;
            NextChestGridPosition.z = clusterCenter.z - _direction.z;
            _revealDelay = Reveal_Delay_Step;
        }

        public void PlaceChest(GameObject chest, Vector3 worldPosition, float hitPoints)
        {
            ChestEntry entryControlScript = chest.GetComponent<ChestEntry>();
            entryControlScript.StartPosition = new Vector3(worldPosition.x, worldPosition.y + Hidden_Chest_Vertical_Offset, worldPosition.z);
            entryControlScript.SecondsToLaunch = _revealDelay;

            chest.GetComponent<ChestCollisions>().HitPoints = hitPoints;
            chest.SetActive(true);

            NextChestGridPosition.x += _direction.x;
            NextChestGridPosition.z += _direction.z;
            _revealDelay += Reveal_Delay_Step;
        }

        private const float Hidden_Chest_Vertical_Offset = -2.0f;
        private const float Reveal_Delay_Step = 0.35f;

        public const int Cluster_Size = 3;
    }
}
