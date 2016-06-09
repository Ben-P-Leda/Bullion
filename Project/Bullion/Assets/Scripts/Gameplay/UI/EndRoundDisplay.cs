using UnityEngine;
using Assets.Scripts.Generic;
using Assets.Scripts.EventHandling;
using Assets.Scripts.Configuration;

namespace Assets.Scripts.Gameplay.UI
{
    public class EndRoundDisplay : MonoBehaviour
    {
        private Transform _transform;
        private bool _displaying;

        private CharacterConfiguration[] _characterConfigurations;

        private Rect _displayContainer;

        public void AddPlayerConfiguration(int playerIndex, CharacterConfiguration characterConfiguration)
        {
            if (_characterConfigurations == null)
            {
                _characterConfigurations = new CharacterConfiguration[Constants.Player_Count];
            }

            _characterConfigurations[playerIndex] = characterConfiguration;
        }

        private void Start()
        {
            _transform = transform;
            _displaying = false;

            float margin = Margin * Utilities.Scale;
            _displayContainer = new Rect(margin, margin, Screen.width - (margin * 2.0f), Screen.height - (margin * 2.0f));
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
            if (message == EventMessage.End_Round)
            {
                _displaying = true;
            }
        }

        private void OnGUI()
        {
            if (_displaying)
            {
                GUI.Window(0, _displayContainer, WindowCallback, MakeWindowContent());
            }
        }

        private void WindowCallback(int windowId)
        {
        }

        private string MakeWindowContent()
        {
            string content = "";
            string result = "";
            float currentBest = 0;
            for (int i = 0; i < Constants.Player_Count; i++)
            {
                content += _characterConfigurations[i].Name + ": " + _characterConfigurations[i].TreasureValue + "\n";

                if (_characterConfigurations[i].TreasureValue > currentBest)
                {
                    currentBest = _characterConfigurations[i].TreasureValue;
                    result = _characterConfigurations[i].Name + " wins!";
                }
                else if (_characterConfigurations[i].TreasureValue == currentBest)
                {
                    result = "Draw!";
                }
            }

            return content + result;
        }

        private float Margin = 300.0f;
    }
}