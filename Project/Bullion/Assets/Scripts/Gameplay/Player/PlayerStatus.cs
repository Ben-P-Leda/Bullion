﻿using UnityEngine;
using Assets.Scripts.Configuration;
using Assets.Scripts.EventHandling;

namespace Assets.Scripts.Gameplay.Player
{
    public class PlayerStatus : MonoBehaviour, IConfigurable, IAnimated
    {
        private Transform _transform;
        private GameObject _aliveModel;
        private GameObject _deadModel;
        private Animator _aliveModelAnimator;

        private PlayerMovement _movement;
        private PlayerAttack _attack;

        private float _remainingHealth;
        private Rect _nameDisplayContainer;
        private Rect _healthDisplayContainer;

        public CharacterConfiguration Configuration { private get; set; }
        public int PlayerIndex { set { SetGuiArea(value % 2, value / 2); } }

        private void SetGuiArea(int unitX, int unitY)
        {
            Vector2 offset = new Vector2(
                Screen.width - ((UI_Margin * 2.0f) + UI_Dimensions.x),
                Screen.height - ((UI_Margin * 2.0f) + UI_Dimensions.y));

            Vector2 topLeft = new Vector2(UI_Margin + (offset.x * unitX), UI_Margin + (offset.y * unitY));

            _nameDisplayContainer = new Rect(topLeft.x, topLeft.y, UI_Dimensions.x, UI_Margin);
            _healthDisplayContainer = new Rect(_nameDisplayContainer.x, _nameDisplayContainer.y + _nameDisplayContainer.height, UI_Dimensions.x, UI_Margin);
        }

        private void Start()
        {
            _transform = transform;
            _movement = GetComponent<PlayerMovement>();
            _attack = GetComponent<PlayerAttack>();

            _remainingHealth = Configuration.MaximumHealth;
        }

        public void WireUpModels(GameObject aliveModel, GameObject deadModel)
        {
            _aliveModel = aliveModel;
            _deadModel = deadModel;
        }

        public void WireUpAnimators(Animator aliveModelAnimator, Animator deadModelAnimator)
        {
            _aliveModelAnimator = aliveModelAnimator;
        }

        private void OnEnable()
        {
            EventDispatcher.MessageEventHandler += MessageEventHandler;
            EventDispatcher.FloatEventHandler += FloatEventHandler;
        }

        private void OnDisable()
        {
            EventDispatcher.MessageEventHandler -= MessageEventHandler;
            EventDispatcher.FloatEventHandler -= FloatEventHandler;
        }

        private void MessageEventHandler(Transform originator, Transform target, string message)
        {
            if ((target == _transform) && (message == EventMessage.Enter_Dead_Mode))
            {
                _deadModel.SetActive(true);
                _aliveModel.SetActive(false);
            }
        }

        private void FloatEventHandler(Transform originator, Transform target, string message, float value)
        {
            if ((target == _transform) && (message == EventMessage.Inflict_Damage))
            {
                HandleDamageTaken(value);
            }
        }

        private void HandleDamageTaken(float damageInflicted)
        {
            if (_remainingHealth > 0.0f)
            {
                _remainingHealth -= damageInflicted;
                if (_remainingHealth > 0.0f)
                {
                    _aliveModelAnimator.SetBool("DamageTaken", true);
                }
                else
                {
                    _aliveModelAnimator.CrossFade("dead", 0.5f);
                    EventDispatcher.FireEvent(_transform, _transform, EventMessage.Has_Died);
                }
            }
        }

        private void OnGUI()
        {
            GUI.Label(_nameDisplayContainer, Configuration.Name);

            float healthPercentage = Mathf.Round((_remainingHealth / Configuration.MaximumHealth) * 100.0f);
            GUI.Label(_healthDisplayContainer, "Health:" + Mathf.Max(0.0f, healthPercentage) + "%");
        }

        private const float UI_Margin = 20.0f;
        public static readonly Vector2 UI_Dimensions = new Vector2(100.0f, 50.0f);
    }
}
