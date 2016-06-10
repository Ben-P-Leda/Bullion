using UnityEngine;
using Assets.Scripts.Generic;
using Assets.Scripts.EventHandling;

namespace Assets.Scripts.Gameplay.UI.GameControl
{
    public class RoundIntroDisplay : MonoBehaviour
    {
        private Rect _displayContainer;
        private GUIStyle _style;
        private string _text;
        private bool _roundStarted;
        private float _crossFadeValue;

        private void Start()
        {
            _displayContainer = new Rect(0, 0, Screen.width, Screen.height);

            _style = new GUIStyle();
            _style.fontSize = (int)(Base_Font_Size * Utilities.Scale);
            _style.normal.textColor = Color.Lerp(Color.red, Color.yellow, 0.5f);
            _style.alignment = TextAnchor.MiddleCenter;

            _text = "Ready?";
            _crossFadeValue = 1.0f;
        }

        private void OnEnable()
        {
            EventDispatcher.FloatEventHandler += FloatEventHandler;
        }

        private void OnDisable()
        {
            EventDispatcher.FloatEventHandler -= FloatEventHandler;
        }

        private void FloatEventHandler(Transform originator, Transform target, string message, float value)
        {
            if (message == EventMessage.Set_Round_Timer)
            {
                UpdateForRoundStart();
            }
        }

        private void UpdateForRoundStart()
        {
            _roundStarted = true;
            _text = "Go!";
        }

        private void FixedUpdate()
        {
            if (_roundStarted)
            {
                _crossFadeValue = Mathf.Max(0.0f, _crossFadeValue - Time.deltaTime);
                _style.normal.textColor = Color.Lerp(Color.clear, Color.green, _crossFadeValue);
            }
        }

        private void OnGUI()
        {
            if (_crossFadeValue > 0.0f)
            {
                GUI.Label(_displayContainer, _text, _style);
            }
        }

        private const float Base_Font_Size = 72.0f;
        private const float Fade_Step = 0.01f;
    }
}
