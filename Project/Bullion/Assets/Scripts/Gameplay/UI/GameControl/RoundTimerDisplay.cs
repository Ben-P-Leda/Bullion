using UnityEngine;
using Assets.Scripts.Generic;
using Assets.Scripts.EventHandling;

namespace Assets.Scripts.Gameplay.UI.GameControl
{
    public class RoundTimerDisplay : MonoBehaviour
    {
        private Rect _displayContainer;
        private GUIStyle _style;
        private bool _timerRunning;
        private float _timeRemaining;

        private void Start()
        {
            _displayContainer = new Rect(0, 20.0f, Screen.width, Screen.height);

            _style = new GUIStyle();
            _style.fontSize = (int)(Base_Font_Size * Utilities.Scale);
            _style.normal.textColor = Color.yellow;
            _style.alignment = TextAnchor.UpperCenter;
            _timerRunning = false;
            _timeRemaining = 0.0f;
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
                _timeRemaining = value;
                _timerRunning = true;
            }
        }

        private void FixedUpdate()
        {
            _timeRemaining = Mathf.Max(0.0f, _timeRemaining - Time.deltaTime);
        }

        private void OnGUI()
        {
            GUI.Label(_displayContainer, "Time: " + Mathf.RoundToInt(_timeRemaining), _style);
        }

        private const float Base_Font_Size = 24.0f;
    }
}
