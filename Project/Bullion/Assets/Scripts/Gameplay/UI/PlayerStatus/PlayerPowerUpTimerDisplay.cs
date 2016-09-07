using UnityEngine;
using Assets.Scripts.EventHandling;

namespace Assets.Scripts.Gameplay.UI.PlayerStatus
{
    public class PlayerPowerUpTimerDisplay : MonoBehaviour
    {
        private Transform _linkedPlayerTransform;
        private Rect _displayContainer;
        private string _displayName;
        private float _remainingDuration;

        public void Initialize(Transform linkedPlayerTransform, int playerIndex)
        {
            _linkedPlayerTransform = linkedPlayerTransform;
            _displayContainer = Helpers.CreateDisplayContainer(playerIndex, new Vector2(Helpers.UI_Margin + Helpers.UI_Dimensions.x, Helpers.UI_Margin), Helpers.UI_Dimensions);
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
            if ((message.StartsWith(EventMessage.Start_Power_Up_Timer_Prefix)) &&  (target == _linkedPlayerTransform))
            {
                _displayName = message.Replace(EventMessage.Start_Power_Up_Timer_Prefix, "");
                _remainingDuration = value;
            }
        }

        private void FixedUpdate()
        {
            _remainingDuration = Mathf.Max(_remainingDuration - Time.deltaTime, 0.0f);
        }

        private void OnGUI()
        {
            if (_remainingDuration > 0.0f)
            {
                GUI.Label(_displayContainer, _displayName + ": " + Mathf.RoundToInt(_remainingDuration));
            }
            else
            {
                GUI.Label(_displayContainer, "No power up");
            }
        }
    }
}