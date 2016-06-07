using UnityEngine;
using Assets.Scripts.Configuration;
using Assets.Scripts.EventHandling;
using Assets.Scripts.Gameplay.Avatar;
using Assets.Scripts.Gameplay.Player.Interfaces;

namespace Assets.Scripts.Gameplay.Player
{
    public class PlayerAttack : MonoBehaviour, IConfigurable, IAnimated
    {
        private Transform _transform;
        private Animator _animator;
        private GameObject _damageCollider;
        private PlayerInput _input;

        private int _comboStepCount;
        private int _lastStrikingComboIndex;

        private bool _lifeEventInProgress;
        private bool _isSwimming;
        private bool _hasBeenLaunched;
        private bool _rushInProgress;

        public CharacterConfiguration Configuration { private get; set; }

        private void Start()
        {
            _transform = transform;
            _damageCollider = _transform.FindChild("Damage Collider").gameObject;
            _input = GetComponent<PlayerInput>();

            _comboStepCount = 0;
            _lastStrikingComboIndex = 0;

            _lifeEventInProgress = false;
            _isSwimming = false;
            _hasBeenLaunched = false;
            _rushInProgress = false;
        }

        public void WireUpAnimators(Animator aliveModelAnimator, Animator deadModelAnimator)
        {
            _animator = aliveModelAnimator;
            _animator.GetBehaviour<AvatarRestingAnimationStateChange>().AddStateEntryHandler(EnterRestState);
        }

        private void EnterRestState()
        {
            _animator.SetBool("IsAttacking", false);
            _comboStepCount = 0;
            _lifeEventInProgress = false;
            _rushInProgress = false;
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
            if ((_damageCollider != null) && (target == _damageCollider.transform) && (message == EventMessage.Hit_Trigger_Collider))
            {
                EventDispatcher.FireEvent(_transform, originator, EventMessage.Inflict_Damage, Configuration.ComboStepDamage[_lastStrikingComboIndex]);
            }

            if (target == _transform)
            {
                switch (message)
                {
                    case EventMessage.Has_Died: _lifeEventInProgress = true; break;
                    case EventMessage.Respawn_Blast: _hasBeenLaunched = true; break;
                    case EventMessage.End_Launch_Effect: _hasBeenLaunched = false; break;
                    case EventMessage.Begin_Rush_Sequence: _rushInProgress = true; break;
                }
            }
        }

        private void BoolEventHandler(Transform originator, Transform target, string message, bool value)
        {
            if (target == _transform)
            {
                switch (message)
                {
                    case EventMessage.Change_Strike_State: SetStrikingState(value); break;
                    case EventMessage.Block_Attack_Swimming: _isSwimming = value; break;
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
            if ((CanAttack()) && (_comboStepCount < Configuration.ComboStepCount) && (_input.Attack))
            {
                EventDispatcher.FireEvent(_transform, _transform, EventMessage.Block_Movement_Attack);
                _animator.SetBool("IsAttacking", true);
            }
        }

        private bool CanAttack()
        {
            return (!_lifeEventInProgress)
                && (!_isSwimming)
                && (!_hasBeenLaunched)
                && (!_rushInProgress);
        }
    }
}