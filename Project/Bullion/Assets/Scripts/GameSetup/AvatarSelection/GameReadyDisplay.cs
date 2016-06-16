using UnityEngine;
using Assets.Scripts.Generic;
using Assets.Scripts.EventHandling;

namespace Assets.Scripts.GameSetup.AvatarSelection
{
    public class GameReadyDisplay : MonoBehaviour
    {
        private Rect _displayContainer;
        private GUIStyle _style;
        private string _text;

        private void Start()
        {
            _displayContainer = new Rect(0, 0, Screen.width, Screen.height);

            _style = new GUIStyle();
            _style.fontSize = (int)(Base_Font_Size * Utilities.Scale);
            _style.alignment = TextAnchor.LowerCenter;

            UpdateText(false);
        }

        private void UpdateText(bool readyToPlay)
        {
            _text = readyToPlay ? "Press start!" : "Waiting for players";
        }

        private void OnEnable()
        {
            EventDispatcher.BoolEventHandler += BoolEventHandler;
        }

        private void OnDisable()
        {
            EventDispatcher.BoolEventHandler -= BoolEventHandler;
        }

        private void BoolEventHandler(Transform originator, Transform target, string message, bool value)
        {
            if (message == EventMessage.Ready_To_Play)
            {
                UpdateText(value);
            }
        }

        private void OnGUI()
        {
            GUI.Label(_displayContainer, _text, _style);
        }

        private const float Base_Font_Size = 72.0f;
    }
}
