using UnityEngine;
using Assets.Scripts.Configuration;
using Assets.Scripts.Gameplay.UI;

namespace Assets.Scripts.Gameplay.Player
{
    public class PlayerFactory : MonoBehaviour
    {
        public GameObject PlayerPrefab;
        public GameObject RespawnPointPrefab;
        public GameObject StatusDisplayPrefab;
        public GameObject[] AvatarPrefabs;
        public Vector3[] PlayerStartPoints;

        private GameObject[] _playerGameObjects;

        private void Start()
        {
            Terrain terrain = Terrain.activeTerrain;
            string[] playerAvatars = GetPlayerAvatars();

            _playerGameObjects = new GameObject[playerAvatars.Length];

            for (int i = 0; i < Constants.Player_Count; i++)
            {
                if (_playerGameObjects[i] == null)
                {
                    CharacterConfiguration characterConfiguration = ConfigurationManager.GetCharacterConfiguration(playerAvatars[i]);
                    Vector3 startPosition = GetStartPosition(terrain, i);

                    _playerGameObjects[i] = InitializePlayer(i, playerAvatars[i], characterConfiguration, startPosition);
                    InitializeRespawnPoint(_playerGameObjects[i], characterConfiguration, startPosition);
                    InitializeStatusDisplay(_playerGameObjects[i], characterConfiguration, i);
                }
            }
        }

        private string[] GetPlayerAvatars()
        {
            return Avatar_Names.Split(',');
        }

        private Vector3 GetStartPosition(Terrain terrain, int playerIndex)
        {
            if (playerIndex >= PlayerStartPoints.Length)
            {
                throw new System.Exception("Start point for player " + playerIndex + " not set!");
            }
            else
            {
                return new Vector3(
                    PlayerStartPoints[playerIndex].x,
                    terrain.SampleHeight(PlayerStartPoints[playerIndex]),
                    PlayerStartPoints[playerIndex].z);
            }
        }

        private GameObject InitializePlayer(int playerIndex, string modelHandle, CharacterConfiguration characterConfiguration, Vector3 startPosition)
        {
            GameObject newPlayer = CreateNewPlayer(playerIndex);
            newPlayer.name = modelHandle;
            ConnectPlayerToModels(newPlayer, modelHandle);
            ConnectPlayerToCamera(newPlayer);
            SetPlayerConfiguration(newPlayer, characterConfiguration);

            newPlayer.transform.position = startPosition;

            return newPlayer;
        }

        private GameObject CreateNewPlayer(int playerIndex)
        {
            GameObject newPlayer = (GameObject)Instantiate(PlayerPrefab);
            newPlayer.transform.parent = transform.parent;

            ((PlayerInput)newPlayer.GetComponent<PlayerInput>()).AxisPrefix = "P" + (playerIndex + 1);

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
                player.GetComponent<PlayerLifeCycle>().WireUpModels(aliveModel, deadModel);
                WireUpAnimationControllers(player, aliveModel.GetComponent<Animator>(), deadModel.GetComponent<Animator>());
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
                animationControllers[i].WireUpAnimators(aliveModelAnimator, deadModelAnimator);
            }
        }

        private void ConnectPlayerToCamera(GameObject player)
        {
            ((CameraMovement)Camera.main.GetComponent<CameraMovement>()).Avatars.Add(player);
        }

        private void SetPlayerConfiguration(GameObject player, CharacterConfiguration characterConfiguration)
        {
            IConfigurable[] configurables = player.GetComponents<IConfigurable>();

            for (int i=0; i < configurables.Length; i++)
            {
                configurables[i].Configuration = characterConfiguration;
            }
        }

        private void InitializeRespawnPoint(GameObject player, CharacterConfiguration characterConfiguration, Vector3 position)
        {
            GameObject newRespawnPoint = (GameObject)Instantiate(RespawnPointPrefab);
            newRespawnPoint.transform.parent = transform.parent;
            newRespawnPoint.transform.position = position;
            newRespawnPoint.GetComponent<ParticleSystem>().startColor = characterConfiguration.RespawnPointColour;

            newRespawnPoint.GetComponent<RespawnPoint>().Player = player.transform;
        }

        private void InitializeStatusDisplay(GameObject player, CharacterConfiguration characterConfiguration, int playerIndex)
        {
            GameObject newRespawnPoint = (GameObject)Instantiate(StatusDisplayPrefab);
            newRespawnPoint.transform.parent = transform.parent;
            newRespawnPoint.GetComponent<PlayerStatusDisplay>().Initialize(player.transform, characterConfiguration, playerIndex);
        }

        private const string Avatar_Names = "Red,Green,Purple,Blue";
    }
}