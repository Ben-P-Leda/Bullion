using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.EventHandling;

namespace Assets.Scripts.GameSetup.AvatarSelection
{
    public class SelectionListener : MonoBehaviour
    {
        private Transform _transform;
        private Dictionary<Transform, string> _selectionStates;

        private void Start()
        {
            _transform = transform;
            _selectionStates = new Dictionary<Transform, string>();
        }

        private void OnEnable()
        {
            EventDispatcher.MessageEventHandler += MessageEventHandler;
        }

        private void OnDisable()
        {
            EventDispatcher.MessageEventHandler -= MessageEventHandler;
        }

        private void MessageEventHandler(Transform originator, Transform target, string message)
        {
            if (target = _transform)
            {
                if (message.StartsWith(EventMessage.Player_Avatar_Selection_Prefix))
                {
                    string avatarName = message.Replace(EventMessage.Player_Avatar_Selection_Prefix, "");
                    UpdateSelectionStates(originator, avatarName);
                }
                else if ((message == EventMessage.Attempt_Game_Start) && (ReadyToStart()))
                {
                    StartGame();
                }
            }
        }

        private void UpdateSelectionStates(Transform avatarSelector, string avatarName)
        {
            if (string.IsNullOrEmpty(avatarName))
            {
                if (_selectionStates.ContainsKey(avatarSelector))
                {
                    _selectionStates.Remove(avatarSelector);
                }
            }
            else
            {
                if (!_selectionStates.ContainsKey(avatarSelector))
                {
                    _selectionStates.Add(avatarSelector, "");
                }
                _selectionStates[avatarSelector] = avatarName;
            }

            EventDispatcher.FireEvent(_transform, _transform, EventMessage.Ready_To_Play, ReadyToStart());
        }

        private bool ReadyToStart()
        {
            return _selectionStates.Count >= Minimum_Required_Players;
        }

        private void StartGame()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Island 1");
        }

        private const float Minimum_Required_Players = 2;
    }
}
