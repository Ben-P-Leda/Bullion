using UnityEngine;
using Assets.Scripts.Configuration;
using Assets.Scripts.EventHandling;
using Assets.Scripts.Generic.ParameterManagement;

namespace Assets.Scripts.GameSetup.AvatarSelection
{
    public class SelectorDisplay : MonoBehaviour
    {
        public GameObject[] AvatarPrefabs;
        public string DefaultAvatarName;

        private Transform _transform;
        private GameObject[] _avatars;
        private string[] _avatarNames;
        private TextMesh _statusText;
        private int _defaultAvatarIndex;
        private int _activeAvatarIndex;
        private bool _isActive;
        private bool _selectionConfirmed;

        private void Start()
        {
            ParameterRepository.SetItem(Parameter.Game_Started_By_Player_Index, 1);

            _transform = transform;
            _statusText = transform.FindChild("Status Text").GetComponent<TextMesh>();

            InitializeDisplayAvatars();

            _selectionConfirmed = false;
            if (WasActivatedFromTitleScreen())
            {
                EnableSelection();
            }
        }

        private void InitializeDisplayAvatars()
        {
            _avatars = new GameObject[AvatarPrefabs.Length];
            _avatarNames = new string[AvatarPrefabs.Length];

            for (int i = 0; i < AvatarPrefabs.Length; i++)
            {
                string configKey = AvatarPrefabs[i].name.Split('-')[0];
                _avatarNames[i] = CharacterConfigurationManager.GetCharacterConfiguration(configKey).Name;

                _avatars[i] = (GameObject)Instantiate(AvatarPrefabs[i]);
                _avatars[i].transform.parent = _transform;
                _avatars[i].transform.localPosition = Vector3.zero;
                _avatars[i].transform.LookAt(Camera.main.transform);

                _avatars[i].SetActive(false);

                if (AvatarPrefabs[i].name.ToLower().StartsWith(DefaultAvatarName.ToLower()))
                {
                    _defaultAvatarIndex = i;
                }
            }
        }

        private bool WasActivatedFromTitleScreen()
        {
            string prefix = string.Format("P{0}", ParameterRepository.GetItem<int>(Parameter.Game_Started_By_Player_Index) + 1);
            return _transform.name.StartsWith(prefix);
        }

        private void OnEnable()
        {
            EventDispatcher.MessageEventHandler += MessageEventHandler;
            EventDispatcher.FloatEventHandler += FloatEventHandler;
        }

        private void OnDisable()
        {
            EventDispatcher.MessageEventHandler -= MessageEventHandler;
            EventDispatcher.FloatEventHandler -= FloatEventHandler;
        }

        private void MessageEventHandler(Transform originator, Transform target, string message)
        {
            if ((target == transform) && (message == EventMessage.Avatar_Selector_Confirm))
            {
                if (!_isActive)
                {
                    EnableSelection();
                }
                else
                {
                    ConfirmSelection();
                }
            }
        }

        private void FloatEventHandler(Transform originator, Transform target, string message, float value)
        {
            if ((_isActive) && (target == transform) && (message == EventMessage.Avatar_Selection_Step))
            {
                StepToNextAvatar((int)Mathf.Sign(value));
            }
        }

        private void EnableSelection()
        {
            _statusText.text = "";

            _avatars[_defaultAvatarIndex].SetActive(true);
            _activeAvatarIndex = _defaultAvatarIndex;

            _isActive = true;
        }

        private void StepToNextAvatar(int stepDirection)
        {
            _avatars[_activeAvatarIndex].SetActive(false);
            _activeAvatarIndex = (_activeAvatarIndex + stepDirection + _avatars.Length) % _avatars.Length;
            _avatars[_activeAvatarIndex].SetActive(true);

            _selectionConfirmed = false;
            _statusText.text = "";
        }

        private void ConfirmSelection()
        {
            _selectionConfirmed = true;
            EventDispatcher.FireEvent(_transform, _transform, EventMessage.Avatar_Selected, _activeAvatarIndex);
            _avatars[_activeAvatarIndex].GetComponent<Animator>().Play("salute");
            _statusText.text = _avatarNames[_activeAvatarIndex];
        }
    }
}
