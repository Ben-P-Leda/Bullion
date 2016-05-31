using UnityEngine;

namespace Assets.Scripts.Gameplay.Treasure.Helpers
{
    public class ClusterContainer
    {
        private PlacementGridReference _direction;
        private float _revealDelay;
        private int _numberPlaced;
        private GameObject[] _chests;

        public bool SufficientChestsAllocated { get; private set; }
        public PlacementGridReference NextChestCellPosition { get; private set; }
        public bool PlacementComplete { get { return _numberPlaced >= Cluster_Size; } }

        public ClusterContainer()
        {
            _direction = new PlacementGridReference();
            NextChestCellPosition = new PlacementGridReference();

            _chests = new GameObject[Cluster_Size];
        }

        public void AllocateChestsFromPool(GameObject[] chestPool)
        {
            int chestsAllocated = 0;
            for (int i = 0; ((i < chestPool.Length) && (chestsAllocated < Cluster_Size)); i++)
            {
                if (!chestPool[i].activeInHierarchy)
                {
                    _chests[chestsAllocated] = chestPool[i];
                    chestsAllocated++;
                }
            }

            SufficientChestsAllocated = (chestsAllocated == Cluster_Size);
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

            NextChestCellPosition.x = clusterCenter.x - _direction.x;
            NextChestCellPosition.z = clusterCenter.z - _direction.z;
            _revealDelay = Reveal_Delay_Step;
            _numberPlaced = 0;
        }

        public void PlaceNextChest(Vector3 worldPosition)
        {
            ChestEntry entryControlScript = _chests[_numberPlaced].GetComponent<ChestEntry>();
            entryControlScript.StartPosition = new Vector3(worldPosition.x, worldPosition.y + Hidden_Chest_Vertical_Offset, worldPosition.z);
            entryControlScript.SecondsToLaunch = _revealDelay;

            _chests[_numberPlaced].SetActive(true);

            NextChestCellPosition.x += _direction.x;
            NextChestCellPosition.z += _direction.z;
            _revealDelay += Reveal_Delay_Step;
            _numberPlaced++;
        }

        private const int Cluster_Size = 3;
        private const float Hidden_Chest_Vertical_Offset = -2.0f;
        private const float Reveal_Delay_Step = 0.35f;
    }
}
