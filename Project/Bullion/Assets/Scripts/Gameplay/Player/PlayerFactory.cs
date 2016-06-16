using UnityEngine;
using Assets.Scripts.Configuration;
using Assets.Scripts.Generic.ParameterManagement;
using Assets.Scripts.Gameplay.Chests;
using Assets.Scripts.Gameplay.UI.GameControl;
using Assets.Scripts.Gameplay.UI.PlayerStatus;
using Assets.Scripts.Gameplay.Player.Interfaces;
using Assets.Scripts.Gameplay.Player.Support;

namespace Assets.Scripts.Gameplay.Player
{
    public class PlayerFactory : MonoBehaviour
    {
        public GameObject PlayerPrefab;
        public GameObject RespawnPointPrefab;
        public GameObject UIDisplayPrefab;
        public GameObject[] AvatarPrefabs;
        public Vector3[] PlayerStartPoints;

        private GameObject[] _playerGameObjects;

        private void Start()
        {
            Terrain terrain = Terrain.activeTerrain;
            ChestFactory chestFactory = FindObjectOfType<ChestFactory>();
            EndRoundDisplay endRoundDisplay = FindObjectOfType<EndRoundDisplay>();

            int activePlayerCount = 0;
            for (int i = 0; i < Constants.Player_Count; i++)
            {
                string playerKey = string.Format("P{0}", i + 1);
                string avatarKey = ParameterRepository.GetItem<string>(Parameter.Selected_Avatar_Prefix + playerKey);

                if (!string.IsNullOrEmpty(avatarKey))
                {
                    CharacterConfiguration characterConfiguration = CharacterConfigurationManager.GetCharacterConfiguration(avatarKey);
                    Vector3 startPosition = GetStartPosition(terrain, i);

                    GameObject playerGameObject = InitializePlayer(playerKey, avatarKey, characterConfiguration, startPosition);
                    InitializeRespawnPoint(playerGameObject, characterConfiguration, startPosition);
                    InitializePlayerUI(playerGameObject, characterConfiguration, activePlayerCount);

                    chestFactory.AddPlayerReference(activePlayerCount, playerGameObject);
                    endRoundDisplay.AddPlayerConfiguration(characterConfiguration);

                    activePlayerCount++;
                }
            }
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

        private GameObject InitializePlayer(string playerHandle, string modelHandle, CharacterConfiguration characterConfiguration, Vector3 startPosition)
        {
            GameObject newPlayer = CreateNewPlayer(playerHandle);
            newPlayer.name = modelHandle;
            ConnectPlayerToModels(newPlayer, modelHandle);
            ConnectPlayerToCamera(newPlayer);
            SetPlayerConfiguration(newPlayer, characterConfiguration);

            newPlayer.transform.position = startPosition;

            return newPlayer;
        }

        private GameObject CreateNewPlayer(string playerHandle)
        {
            GameObject newPlayer = (GameObject)Instantiate(PlayerPrefab);
            newPlayer.transform.parent = transform.parent;

            ((PlayerInput)newPlayer.GetComponent<PlayerInput>()).AxisPrefix = playerHandle;

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

            CharacterConfigurationModifier configurationModifier = new CharacterConfigurationModifier();

            IModifiable[] modifiables = player.GetComponents<IModifiable>();
            for (int i = 0; i < modifiables.Length; i++)
            {
                modifiables[i].ConfigurationModifier = configurationModifier;
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

        private void InitializePlayerUI(GameObject player, CharacterConfiguration characterConfiguration, int uiIndex)
        {
            GameObject newPlayerUI = (GameObject)Instantiate(UIDisplayPrefab);
            newPlayerUI.transform.parent = transform.parent;
            newPlayerUI.GetComponent<PlayerStatusDisplay>().Initialize(player.transform, characterConfiguration, uiIndex);
            newPlayerUI.GetComponent<PlayerPowerUpTimerDisplay>().Initialize(player.transform, uiIndex);
        }
    }
}