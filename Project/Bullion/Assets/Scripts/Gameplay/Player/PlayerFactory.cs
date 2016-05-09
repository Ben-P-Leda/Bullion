using UnityEngine;
using Assets.Scripts.Configuration;

namespace Assets.Scripts.Gameplay.Player
{
    public class PlayerFactory : MonoBehaviour
    {
        public GameObject _playerPrefab;
        public GameObject[] _avatarPrefabs;

        private GameObject[] _playerGameObjects;

        private void Start()
        {
            string[] playerAvatars = GetPlayerAvatars();

            _playerGameObjects = new GameObject[playerAvatars.Length];

            for (int i=0; i<Definitions.Player_Count; i++)
            {
                if (_playerGameObjects[i] == null)
                {
                    _playerGameObjects[i] = InitializePlayer(i, playerAvatars[i]);
                }
            }
        }

        private string[] GetPlayerAvatars()
        {
            return Avatar_Names.Split(',');
        }

        private GameObject InitializePlayer(int playerIndex, string avatarName)
        {
            GameObject newPlayer = CreateNewPlayer(playerIndex);
            ConnectPlayerToModel(newPlayer, avatarName);
            ConnectPlayerToCamera(newPlayer);
            SetPlayerConfiguration(newPlayer, avatarName);

            newPlayer.transform.position = new Vector3((playerIndex + 1) * 10.0f, 3.0f, 15.0f);

            return newPlayer;
        }

        private GameObject CreateNewPlayer(int playerIndex)
        {
            GameObject newPlayer = (GameObject)Instantiate(_playerPrefab);
            newPlayer.transform.parent = transform.parent;

            ((PlayerInput)newPlayer.GetComponent<PlayerInput>()).AxisPrefix = "P" + (playerIndex + 1);

            return newPlayer;
        }

        private void ConnectPlayerToModel(GameObject player, string avatarName)
        {
            GameObject model = GetModel(avatarName);

            if (model != null)
            {
                model.transform.SetParent(player.transform);
                Animator animator = model.GetComponent<Animator>();
                WireUpAnimationControllers(player, animator);
            }
            else
            {
                throw new System.Exception("Avatar " + avatarName + " does not exist!");
            }
        }

        private GameObject GetModel(string avatarName)
        {
            GameObject model = null;

            for (int i = 0; i < _avatarPrefabs.Length; i++)
            {
                if (_avatarPrefabs[i].name == avatarName)
                {
                    model = (GameObject)Instantiate(_avatarPrefabs[i]);
                    break;
                }
            }

            return model;
        }

        private void WireUpAnimationControllers(GameObject player, Animator animator)
        {
            IAnimated[] animationControllers = player.GetComponents<IAnimated>();

            for (int i = 0; i < animationControllers.Length; i++)
            {
                animationControllers[i].Animator = animator;
            }
        }

        private void ConnectPlayerToCamera(GameObject player)
        {
            ((CameraMovement)Camera.main.GetComponent<CameraMovement>()).Avatars.Add(player);
        }

        private void SetPlayerConfiguration(GameObject player, string avatarName)
        {
            CharacterConfiguration config = ConfigurationManager.GetCharacterConfiguration(avatarName);
            IConfigurable[] configurables = player.GetComponents<IConfigurable>();

            for (int i=0; i<configurables.Length; i++)
            {
                configurables[i].Configuration = config;
            }
        }

        private const string Avatar_Names = "Red,Green,Purple,Blue";
    }
}