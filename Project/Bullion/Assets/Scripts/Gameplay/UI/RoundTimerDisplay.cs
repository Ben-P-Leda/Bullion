using UnityEngine;
using Assets.Scripts.EventHandling;

namespace Assets.Scripts.Gameplay.UI
{
    public class RoundTimerDisplay : MonoBehaviour
    {
        private Rect _displayContainer;

        private bool _timerRunning;
        private float _timeRemaining;

        private void Start()
        {
            _displayContainer = new Rect(300, 0, 300, 100);

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
            GUI.Label(_displayContainer, (_timerRunning ? "Time: " + _timeRemaining : "Ready..."));
        }
    }
}
