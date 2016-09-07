using UnityEngine;
using Assets.Scripts.Configuration;
using Assets.Scripts.EventHandling;

namespace Assets.Scripts.Gameplay.UI.PlayerStatus
{
    public class PlayerStatusDisplay : MonoBehaviour
    {
        private Transform _linkedPlayerTransform;
        private CharacterConfiguration _configuration;
        private Rect _nameDisplayContainer;
        private Rect _healthDisplayContainer;
        private Rect _rushDisplayContainer;
        private Rect _treasureDisplayContainer;

        private float _targetRemainingHealth;
        private float _displayedRemainingHealth;
        private float _displayedRushCharge;
        private float _displayedTreasureValue;

        public void Initialize(Transform linkedPlayerTransform, CharacterConfiguration characterConfiguration, int playerIndex)
        {
            _linkedPlayerTransform = linkedPlayerTransform;
            _configuration = characterConfiguration;

            _nameDisplayContainer = Helpers.CreateDisplayContainer(playerIndex, new Vector2(Helpers.UI_Margin, Helpers.UI_Margin), Helpers.UI_Dimensions);
            _healthDisplayContainer = Helpers.CreateDisplayContainer(playerIndex, new Vector2(Helpers.UI_Margin, Helpers.UI_Margin * 2.0f), Helpers.UI_Dimensions);
            _rushDisplayContainer = Helpers.CreateDisplayContainer(playerIndex, new Vector2(Helpers.UI_Margin, Helpers.UI_Margin * 3.0f), Helpers.UI_Dimensions);
            _treasureDisplayContainer = Helpers.CreateDisplayContainer(playerIndex, new Vector2(Helpers.UI_Margin, Helpers.UI_Margin * 4.0f), Helpers.UI_Dimensions);

            _targetRemainingHealth = _configuration.MaximumHealth;
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
            if (target == _linkedPlayerTransform)
            {
                switch (message)
                {
                    case EventMessage.Update_Health: _targetRemainingHealth = Mathf.Clamp(value, 0.0f, 100.0f); break;
                    case EventMessage.Update_Rush_Charge: _displayedRushCharge = value; break;
                    case EventMessage.Update_Treasure: _displayedTreasureValue = value; break;
                }
            }
        }

        private void FixedUpdate()
        {
            UpdateHealthDisplay();
        }

        private void UpdateHealthDisplay()
        {
            float difference = Mathf.Abs(_targetRemainingHealth - _displayedRemainingHealth);
            if (difference > Health_Step)
            {
                _displayedRemainingHealth += Health_Step * (Mathf.Sign(_targetRemainingHealth - _displayedRemainingHealth));
            }
            else
            {
                _displayedRemainingHealth = _targetRemainingHealth;
            }
        }

        private void OnGUI()
        {
            GUI.Label(_nameDisplayContainer, _configuration.Name);

            float healthPercentage = Mathf.Round((_displayedRemainingHealth / _configuration.MaximumHealth) * 100.0f);
            GUI.Label(_healthDisplayContainer, "Health:" + Mathf.Max(0.0f, healthPercentage) + "%");

            GUI.Label(_rushDisplayContainer, "Rush Charge:" + Mathf.Round(_displayedRushCharge) + "%");
            GUI.Label(_treasureDisplayContainer, "Treasure Value:" + _displayedTreasureValue);
        }

        private const float Health_Step = 2.5f;
    }
}