using UnityEngine;
using Assets.Scripts.Generic;

namespace Assets.Scripts.TitleScene
{
    public class PlaceholderTitleManager : MonoBehaviour
    {
        private bool _gameStarted;
        private Rect _displayContainer;
        private GUIStyle _style;
        private string _content;

        private void Start()
        {
            _gameStarted = false;
            _displayContainer = new Rect(0, 0, Screen.width, Screen.height);

            _content =
                "<color=#ffffffff>" +
                "<size=" + Utilities.ScaledFontSize(72) + ">Bullion</size>\n" +
                "<size=" + Utilities.ScaledFontSize(36) + ">The Curse of the Cut-Throat Cattle</size>\n\n\n\n\n" +
                "<size=" + Utilities.ScaledFontSize(44) + ">Press Start</size>" +
                "</color>";

            _style = new GUIStyle();
            _style.richText = true;
            _style.alignment = TextAnchor.MiddleCenter;
        }

        private void Update()
        {
            if (!_gameStarted)
            {
                CheckForGameStart();
            }
        }

        private void CheckForGameStart()
        {
            for (int i = 0; ((i < InputFormats.Length) && (!_gameStarted)); i++)
            {
                for (int j = 0; ((j < 4) && (!_gameStarted)); j++)
                {
                    string button = string.Format(InputFormats[i], j + 1);
                    if (Input.GetButtonDown(button))
                    {
                        _gameStarted = true;
                        Debug.Log("GAME START! By " + button);
                    }
                }
            }
        }

        private void OnGUI()
        {
            GUI.Label(_displayContainer, _content, _style);
        }

        private readonly string[] InputFormats = { "P{0}-Attack", "P{0}-Rush" };
    }
}