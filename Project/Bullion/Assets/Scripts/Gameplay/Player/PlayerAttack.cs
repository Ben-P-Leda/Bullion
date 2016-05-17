using UnityEngine;
using Assets.Scripts.Configuration;
using Assets.Scripts.EventHandling;
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
            EventDispatcher.MessageEventHandler += MessageEventHandler;
            EventDispatcher.BoolEventHandler += BoolEventHandler;
        }

        private void OnDisable()
        {
            EventDispatcher.MessageEventHandler -= MessageEventHandler;
            EventDispatcher.BoolEventHandler -= BoolEventHandler;
        }

        private void MessageEventHandler(Transform originator, Transform target, string message)
        {
            if ((target == _damageCollider.transform) && (message == EventMessage.Register_Damage))
            {
                EventDispatcher.FireEvent(_transform, originator, EventMessage.Inflict_Damage, Configuration.ComboStepDamage[_lastStrikingComboIndex]);
            }
        }

        private void BoolEventHandler(Transform originator, Transform target, string message, bool value)
        {
            if (target == _transform)
            {
                if (message == EventMessage.Change_Strike_State)
                {
                    SetStrikingState(value);
                }
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
                EventDispatcher.FireEvent(_transform, _transform, EventMessage.Block_Movement_Attack);
                _animator.SetBool("IsAttacking", true);
            }
        }
    }
}