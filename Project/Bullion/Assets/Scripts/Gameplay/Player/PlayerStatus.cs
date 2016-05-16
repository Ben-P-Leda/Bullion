using UnityEngine;
using Assets.Scripts.Configuration;
using Assets.Scripts.Event_Handling;

namespace Assets.Scripts.Gameplay.Player
{
    public class PlayerStatus : MonoBehaviour, IConfigurable, IAnimated
    {
        private Transform _transform;

        private float _remainingHealth;
        private Rect _nameDisplayContainer;
        private Rect _healthDisplayContainer;

        public CharacterConfiguration Configuration { private get; set; }
        public int PlayerIndex { set { SetGuiArea(value % 2, value / 2); } }

        public Animator AliveModelAnimator { private get; set; }
        public Animator DeadModelAnimator { private get; set; }

        private void SetGuiArea(int unitX, int unitY)
        {
            Vector2 offset = new Vector2(
                Screen.width - ((UI_Margin * 2.0f) + UI_Dimensions.x),
                Screen.height - ((UI_Margin * 2.0f) + UI_Dimensions.y));

            Vector2 topLeft = new Vector2(UI_Margin + (offset.x * unitX), UI_Margin + (offset.y * unitY));

            _nameDisplayContainer = new Rect(topLeft.x, topLeft.y, UI_Dimensions.x, UI_Margin);
            _healthDisplayContainer = new Rect(_nameDisplayContainer.x, _nameDisplayContainer.y + _nameDisplayContainer.height, UI_Dimensions.x, UI_Margin);
        }

        private void OnEnable()
        {
            EventDispatcher.EventHandler += EventHandler;
        }

        private void OnDisable()
        {
            EventDispatcher.EventHandler -= EventHandler;
        }

        private void EventHandler(Transform originator, Transform target, string message, float value)
        {
            if ((target == _transform) && (message == PlayerAttack.Event_Inflict_Damage))
            {
                HandleDamageTaken(value);
            }
        }

        private void HandleDamageTaken(float damageInflicted)
        {
            _remainingHealth -= damageInflicted;
            if (_remainingHealth > 0.0f)
            {
                AliveModelAnimator.SetBool("DamageTaken", true);
            }
            else
            {
                AliveModelAnimator.SetBool("IsDead", true);
            }
        }

        private void Start()
        {
            _transform = transform;

            _remainingHealth = Configuration.MaximumHealth;
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
