using UnityEngine;
using Assets.Scripts.Configuration;
using Assets.Scripts.EventHandling;
using Assets.Scripts.Gameplay.Avatar;

namespace Assets.Scripts.Gameplay.Player
{
    public class PlayerMovement : MonoBehaviour, IConfigurable, IAnimated
    {
        private Transform _transform;
        private Rigidbody _rigidBody;
        private Animator _aliveModelAnimator;
        private Animator _deadModelAnimator;
        private Animator _activeAnimator;
        private PlayerInput _input;
        private Terrain _terrain;

        private bool _wasSwimming;
        private float _swimHeight;
        private float _seaEntryHeight;
        private float _wadeHeightRange;

        private bool _lifecycleEventInProgress;
        private bool _attackInProgress;
        private bool _isInDeadMode;

        public CharacterConfiguration Configuration { private get; set; }

        private void Start()
        {
            _transform = transform;
            _rigidBody = GetComponent<Rigidbody>();
            _input = GetComponent<PlayerInput>();
            _terrain = Terrain.activeTerrain;

            _wasSwimming = false;
            _swimHeight = GetComponent<CapsuleCollider>().height * Swim_Height_Modifier;
            _seaEntryHeight = GetComponent<CapsuleCollider>().height * Sea_Entry_Height_Modifier;
            _wadeHeightRange = _seaEntryHeight - _swimHeight;

            _isInDeadMode = false;

            EnableMovement();
        }

        public void WireUpAnimators(Animator aliveModelAnimator, Animator deadModelAnimator)
        {
            _aliveModelAnimator = aliveModelAnimator;
            _aliveModelAnimator.GetBehaviour<AvatarRestingAnimationStateChange>().AddStateEntryHandler(EnableMovement);

            _deadModelAnimator = deadModelAnimator;

            _activeAnimator = _aliveModelAnimator;
        }

        private void EnableMovement()
        {
            _attackInProgress = false;
            _lifecycleEventInProgress = false;
        }

        private void OnEnable()
        {
            EventDispatcher.MessageEventHandler += MessageEventHandler;
        }

        private void OnDisable()
        {
            EventDispatcher.MessageEventHandler -= MessageEventHandler;
        }

        private void MessageEventHandler(Transform originator, Transform target, string message)
        {
            if (target == _transform)
            {
                switch (message)
                {
                    case EventMessage.Block_Movement_Attack: _attackInProgress = true; break;
                    case EventMessage.Has_Died: SetLifeEventRunning(true); break;
                    case EventMessage.Enter_Dead_Mode: EnterDeadMode(); break;
                }
            }
        }

        private void SetLifeEventRunning(bool isRunning)
        {
            _lifecycleEventInProgress = isRunning;
            _rigidBody.constraints = isRunning ? RigidbodyConstraints.FreezeAll : RigidbodyConstraints.FreezeRotation;
        }

        private void EnterDeadMode()
        {
            SetLifeEventRunning(false);

            _activeAnimator = _deadModelAnimator;
            _isInDeadMode = true;
        }

        private void Update()
        {
            if (CanMove())
            {
                float speed = GetMovementSpeed();
                Vector3 inputVelocity = new Vector3(_input.Horizontal, 0.0f, _input.Vertical).normalized * speed;
                bool isMoving = inputVelocity != Vector3.zero;

                if (isMoving)
                {
                    _rigidBody.velocity = new Vector3(inputVelocity.x, _rigidBody.velocity.y, inputVelocity.z);
                    _transform.LookAt(_transform.position + inputVelocity);
                }
                _activeAnimator.SetBool("IsMoving", isMoving);
            }

            float groundHeight = _terrain.SampleHeight(_transform.position);
            float lowestVerticalPosition = _isInDeadMode ? _seaEntryHeight : _swimHeight;
            float floor = Mathf.Max(groundHeight, lowestVerticalPosition);

            if (!_isInDeadMode)
            {
                UpdateSwimmingState();
            }

            _transform.position = new Vector3(_transform.position.x, Mathf.Max(_transform.position.y, floor), _transform.position.z);
        }

        private void UpdateSwimmingState()
        {
            bool isSwimming = _transform.position.y <= _swimHeight;
            _aliveModelAnimator.SetBool("IsSwimming", isSwimming);
            if (isSwimming != _wasSwimming)
            {
                EventDispatcher.FireEvent(_transform, _transform, EventMessage.Block_Attack_Swimming, isSwimming);
                _wasSwimming = isSwimming;
            }

        }

        private bool CanMove()
        {
            return (!_attackInProgress)
                && (!_lifecycleEventInProgress);
        }

        private float GetMovementSpeed()
        {
            float speed = Configuration.AliveMovementSpeed;

            if (_transform.position.y < _seaEntryHeight)
            {
                float depthOffset = Mathf.Clamp((_seaEntryHeight - _transform.position.y) / _wadeHeightRange, 0.0f, 1.0f);
                speed *=  (1.0f - (Swim_Speed_Modifier * depthOffset));
            }

            return speed;
        }

        private const float Sea_Entry_Height_Modifier = 0.5f;
        private const float Swim_Height_Modifier = 0.05f;
        private const float Swim_Speed_Modifier = 0.4f;
    }
}