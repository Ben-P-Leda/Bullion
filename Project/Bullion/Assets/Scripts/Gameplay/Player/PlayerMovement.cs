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
        private bool _hasBeenLaunched;
        private bool _rushInProgress;

        private Vector3 _rushVelocity;
        private float _rushDurationRemaining;

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
            _hasBeenLaunched = false;

            _rushVelocity = Vector3.zero;
            _rushDurationRemaining = 0.0f;

            EnterRestState();
        }

        public void WireUpAnimators(Animator aliveModelAnimator, Animator deadModelAnimator)
        {
            _aliveModelAnimator = aliveModelAnimator;
            _aliveModelAnimator.GetBehaviour<AvatarRestingAnimationStateChange>().AddStateEntryHandler(EnterRestState);
            _aliveModelAnimator.GetBehaviour<AvatarRushingAnimationStateChange>().StateEntryCallback = EnterRushState;

            _deadModelAnimator = deadModelAnimator;

            _activeAnimator = _aliveModelAnimator;
        }

        private void EnterRestState()
        {
            _attackInProgress = false;
            _rushInProgress = false;
            SetLifeEventRunning(false);
        }

        private void EnterRushState()
        {
            _rushVelocity = Vector3.Normalize(new Vector3(_transform.forward.x, 0.0f, _transform.forward.z)) * Configuration.RushMovementSpeed;
            _rushDurationRemaining = Configuration.RushDuration;
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
                    case EventMessage.Enter_Dead_Mode: SetDeadModeState(true); break;
                    case EventMessage.Respawn: SetDeadModeState(false); break;
                    case EventMessage.Respawn_Blast: AttemptLaunch(originator.position); break;
                    case EventMessage.Begin_Rush_Sequence: _rushInProgress = true; break;
                }
            }
        }

        private void SetLifeEventRunning(bool isRunning)
        {
            _lifecycleEventInProgress = isRunning;
            _rigidBody.constraints = isRunning ? RigidbodyConstraints.FreezeAll : RigidbodyConstraints.FreezeRotation;
        }

        private void SetDeadModeState(bool enterDeadMode)
        {
            SetLifeEventRunning(!enterDeadMode);

            _isInDeadMode = enterDeadMode;
            _activeAnimator = enterDeadMode ? _deadModelAnimator : _aliveModelAnimator;
        }

        private void AttemptLaunch(Vector3 respawnPointPosition)
        {
            if (CanMove())
            {
                Vector3 unitDirection = Vector3.Normalize(_transform.position - respawnPointPosition);
                Vector3 launchVelocityBase = new Vector3(unitDirection.x, Respawn_Blast_Launch_Vertical_Speed, unitDirection.z);

                _rigidBody.velocity = launchVelocityBase * Respawn_Blast_Launch_Speed_Multiplier;
                _transform.LookAt(new Vector3(respawnPointPosition.x, _transform.position.y, respawnPointPosition.z));
                _hasBeenLaunched = true;
            }
        }

        private void Update()
        {
            float floor = GetFloorAtPosition(_transform.position);
            _transform.position = new Vector3(_transform.position.x, Mathf.Max(_transform.position.y, floor), _transform.position.z);

            if (CanMove())
            {
                UpdateControlledMotion();
            }
            else if (_rushInProgress)
            {
                UpdateRushMotion(floor);
            }

            if (!_isInDeadMode)
            {
                UpdateSwimmingState();
            }
           
            if (_hasBeenLaunched)
            {
                CheckForLaunchReset(floor);
            }
        }

        public float GetFloorAtPosition(Vector3 position)
        {
            float groundHeight = _terrain.SampleHeight(position);
            float lowestVerticalPosition = _isInDeadMode ? _seaEntryHeight : _swimHeight;
            return Mathf.Max(groundHeight, lowestVerticalPosition);
        }

        private void UpdateControlledMotion()
        {
            float speed = GetMovementSpeed();
            Vector3 inputVelocity = new Vector3(_input.Horizontal, 0.0f, _input.Vertical).normalized * speed;
            bool isMoving = inputVelocity != Vector3.zero;

            if (isMoving)
            {
                _transform.LookAt(_transform.position + inputVelocity);
            }

            _rigidBody.velocity = new Vector3(inputVelocity.x, _rigidBody.velocity.y, inputVelocity.z);
            _activeAnimator.SetBool("IsMoving", isMoving);
        }

        private bool CanMove()
        {
            return (!_attackInProgress)
                && (!_lifecycleEventInProgress)
                && (!_hasBeenLaunched)
                && (!_rushInProgress);
        }

        private float GetMovementSpeed()
        {
            float speed = _isInDeadMode ? Configuration.DeadMovementSpeed : Configuration.AliveMovementSpeed;

            if ((!_isInDeadMode) && (_transform.position.y < _seaEntryHeight))
            {
                float depthOffset = Mathf.Clamp((_seaEntryHeight - _transform.position.y) / _wadeHeightRange, 0.0f, 1.0f);
                speed *=  (1.0f - (Swim_Speed_Modifier * depthOffset));
            }

            return speed;
        }

        private void UpdateRushMotion(float hipFloor)
        {
            Vector3 headPosition = _transform.position + (Vector3.Normalize(_rushVelocity) * Configuration.HipShoulderDistance);
            float heightDifference = GetFloorAtPosition(headPosition) - hipFloor;
            Vector3 lookAtOffset = new Vector3(_rushVelocity.x, heightDifference, _rushVelocity.z);
            _transform.LookAt(_transform.position + lookAtOffset);

            _rigidBody.velocity = new Vector3(_rushVelocity.x, _rigidBody.velocity.y, _rushVelocity.z);

            if (_rushVelocity != Vector3.zero)
            {
                _rushDurationRemaining -= Time.deltaTime;
                if (_rushDurationRemaining <= 0.0f)
                {
                    EndRush();
                }
            }
        }

        private void EndRush()
        {
            _rushVelocity = Vector3.zero;
            _aliveModelAnimator.SetBool("IsRushing", false);
        }

        private void UpdateSwimmingState()
        {
            bool isSwimming = _transform.position.y <= _swimHeight;
            _aliveModelAnimator.SetBool("IsSwimming", isSwimming);
            if (isSwimming != _wasSwimming)
            {
                EventDispatcher.FireEvent(_transform, _transform, EventMessage.Block_Attack_Swimming, isSwimming);
                _wasSwimming = isSwimming;

                if (_rushInProgress)
                {
                    EndRush();
                }
            }
        }

        private void CheckForLaunchReset(float floor)
        {
            if ((_rigidBody.velocity.y < Launch_Reset_Speed_Threshold) && (_transform.position.y - Launch_Reset_Floor_Tolerance <= floor))
            {
                _hasBeenLaunched = false;
                EventDispatcher.FireEvent(_transform, _transform, EventMessage.End_Launch_Effect);
            }
        }

        private const float Sea_Entry_Height_Modifier = 0.5f;
        private const float Swim_Height_Modifier = 0.05f;
        private const float Swim_Speed_Modifier = 0.4f;

        private const float Respawn_Blast_Launch_Vertical_Speed = 3.0f;
        private const float Respawn_Blast_Launch_Speed_Multiplier = 3.0f;
        private const float Launch_Reset_Speed_Threshold = 1.0f;
        private const float Launch_Reset_Floor_Tolerance = 0.5f;
    }
}