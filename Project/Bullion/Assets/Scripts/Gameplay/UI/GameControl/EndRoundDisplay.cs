using UnityEngine;
using Assets.Scripts.Generic;
using Assets.Scripts.EventHandling;
using Assets.Scripts.Configuration;

namespace Assets.Scripts.Gameplay.UI.GameControl
{
    public class EndRoundDisplay : MonoBehaviour
    {
        private Rect _displayContainer;
        private GUIStyle _style;
        private string _windowText;

        private CharacterConfiguration[] _characterConfigurations;

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
            float margin = Margin * Utilities.Scale;
            _displayContainer = new Rect(margin, margin, Screen.width - (margin * 2.0f), Screen.height - (margin * 2.0f));

            _style = new GUIStyle();
            _style.fontSize = (int)(Base_Font_Size * Utilities.Scale);
            _style.richText = true;
            _style.alignment = TextAnchor.MiddleCenter;

            _windowText = "";
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
                MakeWindowContent();
            }
        }

        private void MakeWindowContent()
        {
            string content = "<size=" + Utilities.ScaledFontSize(36) + ">Results</size>\n\n";
            string result = "";
            float currentBest = 0;
            for (int i = 0; i < Constants.Player_Count; i++)
            {
                content +=
                    "<size=" + Utilities.ScaledFontSize(18) + ">" +
                    "<color=#dbdbdbff>" + _characterConfigurations[i].Name + ": " + _characterConfigurations[i].TreasureValue +
                    "</color>" +
                    "</size>" + "\n";

                if (_characterConfigurations[i].TreasureValue > currentBest)
                {
                    currentBest = _characterConfigurations[i].TreasureValue;
                    result = "<color=red>" + _characterConfigurations[i].Name + " wins!</color>";
                }
                else if (_characterConfigurations[i].TreasureValue == currentBest)
                {
                    result = "<color=orange>Draw!</color>";
                }
            }

            _windowText = content + "\n\n<size=" + Utilities.ScaledFontSize(44) + ">" + result + "</size>";
        }

        private void OnGUI()
        {
            if (!string.IsNullOrEmpty(_windowText))
            {
                GUI.Window(0, _displayContainer, WindowCallback, _windowText);
            }
        }

        private void WindowCallback(int windowId)
        {
        }

        private float Margin = 300.0f;
        private const float Base_Font_Size = 24.0f;
    }
}