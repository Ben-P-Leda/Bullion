using UnityEngine;
using Assets.Scripts.Configuration;
using Assets.Scripts.Event_Handling;
using Assets.Scripts.Gameplay.Avatar;

namespace Assets.Scripts.Gameplay.Player
{
    public class PlayerAttack : MonoBehaviour, IConfigurable, IAnimated
    {
        private Transform _transform;
        private Animator _animator;
        private GameObject _damageCollider;
        private PlayerInput _input;
        private PlayerMovement _movement;

        private int _comboStepCount;
        private int _lastStrikingComboIndex;

        public CharacterConfiguration Configuration { private get; set; }
        public bool CanAttack { private get; set; }

        private void Start()
        {
            _transform = transform;
            _damageCollider = _transform.FindChild("Damage Collider").gameObject;

            _input = GetComponent<PlayerInput>();
            _movement = GetComponent<PlayerMovement>();

            _comboStepCount = 0;
            _lastStrikingComboIndex = 0;
        }

        public void WireUpAnimators(Animator aliveModelAnimator, Animator deadModelAnimator)
        {
            _animator = aliveModelAnimator;
            _animator.GetBehaviour<AvatarRestingAnimationStateChange>().AddStateEntryHandler(EndComboSequence);
        }

        private void EndComboSequence()
        {
            _animator.SetBool("IsAttacking", false);
            _comboStepCount = 0;
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
            if ((target == _damageCollider.transform) && (message == PlayerTakeDamage.Event_Register_Damage))
            {
                EventDispatcher.FireEvent(_transform, originator, Event_Inflict_Damage, Configuration.ComboStepDamage[_lastStrikingComboIndex]);
            }

            if (target == _transform)
            {
                if (message == AvatarAnimationEventHandler.Event_Start_Strike) { SetStrikingState(true); }
                else if (message == AvatarAnimationEventHandler.Event_End_Strike) { SetStrikingState(false); }
            }
        }

        private void SetStrikingState(bool strikeInProgress)
        {
            _damageCollider.SetActive(strikeInProgress);

            if (strikeInProgress)
            {
                _animator.SetBool("IsAttacking", false);
                _lastStrikingComboIndex = _comboStepCount;
                _comboStepCount += 1;
            }
        }

        private void Update()
        {
            if ((CanAttack) && (_comboStepCount < Configuration.ComboStepCount) && (_input.Attack))
            {
                _movement.CanMove = false;
                _animator.SetBool("IsAttacking", true);
            }
        }

        public const string Event_Inflict_Damage = "InflictDamage";
    }
}