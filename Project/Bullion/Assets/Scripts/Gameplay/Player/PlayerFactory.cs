using UnityEngine;
using Assets.Scripts.Configuration;

namespace Assets.Scripts.Gameplay.Player
{
    public class PlayerFactory : MonoBehaviour
    {
        public GameObject PlayerPrefab;
        public GameObject[] AvatarPrefabs;
        public Vector3[] PlayerStartPoints;

        private GameObject[] _playerGameObjects;
        private Terrain _terrain;

        private void Start()
        {
            _terrain = Terrain.activeTerrain;

            string[] playerAvatars = GetPlayerAvatars();

            _playerGameObjects = new GameObject[playerAvatars.Length];

            for (int i=0; i<Player_Count; i++)
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
            ConnectPlayerToModels(newPlayer, avatarName);
            ConnectPlayerToCamera(newPlayer);
            SetPlayerConfiguration(newPlayer, avatarName);

            newPlayer.transform.position = GetStartPosition(playerIndex);

            return newPlayer;
        }

        private GameObject CreateNewPlayer(int playerIndex)
        {
            GameObject newPlayer = (GameObject)Instantiate(PlayerPrefab);
            newPlayer.transform.parent = transform.parent;

            ((PlayerInput)newPlayer.GetComponent<PlayerInput>()).AxisPrefix = "P" + (playerIndex + 1);
            ((PlayerStatus)newPlayer.GetComponent<PlayerStatus>()).PlayerIndex = playerIndex;

            return newPlayer;
        }

        private void ConnectPlayerToModels(GameObject player, string avatarName)
        {
            GameObject aliveModel = GetModel(avatarName + "-alive");
            GameObject deadModel = GetModel(avatarName + "-dead");

            if ((aliveModel != null) && (deadModel != null))
            {
                aliveModel.transform.SetParent(player.transform);
                deadModel.transform.SetParent(player.transform);
                WireUpAnimationControllers(player, aliveModel.GetComponent<Animator>(), deadModel.GetComponent<Animator>());

                deadModel.SetActive(false);
            }
            else
            {
                throw new System.Exception("Avatar " + avatarName + " model missing - check prefabs set for both alive & dead models");
            }
        }

        private GameObject GetModel(string avatarName)
        {
            GameObject model = null;

            for (int i = 0; i < AvatarPrefabs.Length; i++)
            {
                if (AvatarPrefabs[i].name == avatarName)
                {
                    model = (GameObject)Instantiate(AvatarPrefabs[i]);
                    break;
                }
            }

            return model;
        }

        private void WireUpAnimationControllers(GameObject player, Animator aliveModelAnimator, Animator deadModelAnimator)
        {
            IAnimated[] animationControllers = player.GetComponents<IAnimated>();

            for (int i = 0; i < animationControllers.Length; i++)
            {
                animationControllers[i].AliveModelAnimator = aliveModelAnimator;
                animationControllers[i].DeadModelAnimator = deadModelAnimator;
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

        private Vector3 GetStartPosition(int playerIndex)
        {
            if (playerIndex >= PlayerStartPoints.Length)
            {
                throw new System.Exception("Start point for player " + playerIndex + " not set!");
            }
            else
            {
                return new Vector3(
                    PlayerStartPoints[playerIndex].x,
                    _terrain.SampleHeight(PlayerStartPoints[playerIndex]),
                    PlayerStartPoints[playerIndex].z);
            }
        }

        private const string Avatar_Names = "Red,Green,Purple,Blue";
        private const float Player_Count = 2;
    }
}