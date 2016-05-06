using UnityEngine;

namespace Assets.Scripts.Gameplay.Player
{
    public class PlayerFactory : MonoBehaviour
    {
        public GameObject _playerPrefab;
        public GameObject[] _avatarPrefabs;

        private GameObject[] _playerGameObjects;

        private void Start()
        {
            if (_playerGameObjects == null)
            {
                _playerGameObjects = new GameObject[Definitions.Player_Count];
            }

            for (int i=0; i<Definitions.Player_Count; i++)
            {
                if (_playerGameObjects[i] == null)
                {
                    _playerGameObjects[i] = InitializePlayer(i);
                }
            }
        }

        private GameObject InitializePlayer(int playerIndex)
        {
            GameObject newPlayer = CreateNewPlayer(playerIndex);
            ConnectPlayerToModel(newPlayer, playerIndex);
            ConnectPlayerToCamera(newPlayer);
            SetPlayerMetrics(newPlayer, playerIndex);

            return newPlayer;
        }

        private GameObject CreateNewPlayer(int playerIndex)
        {
            GameObject newPlayer = (GameObject)Instantiate(_playerPrefab);
            newPlayer.transform.parent = transform.parent;

            ((PlayerInput)newPlayer.GetComponent<PlayerInput>()).AxisPrefix = "P" + (playerIndex + 1);

            return newPlayer;
        }

        private void ConnectPlayerToModel(GameObject player, int playerIndex)
        {
            GameObject model = (GameObject)Instantiate(_avatarPrefabs[playerIndex]);
            model.transform.SetParent(player.transform);
        }

        private void ConnectPlayerToCamera(GameObject player)
        {
            ((CameraMovement)Camera.main.GetComponent<CameraMovement>()).Avatars.Add(player);
        }

        private void SetPlayerMetrics(GameObject player, int playerIndex)
        {
            player.transform.position = new Vector3((playerIndex + 1) * 10.0f, 3.0f, 15.0f);
        }
    }
}