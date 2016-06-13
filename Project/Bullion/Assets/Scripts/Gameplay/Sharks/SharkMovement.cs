using UnityEngine;
using Assets.Scripts.EventHandling;

namespace Assets.Scripts.Gameplay.Sharks
{
    public class SharkMovement : MonoBehaviour
    {
        private Transform _transform;
        private Rigidbody _rigidBody;
        private Animator _animator;
        private Vector3 _terrainCenter;
        private Transform _target;

        public void InitializeComponents()
        {
            _transform = transform;
            _rigidBody = GetComponent<Rigidbody>();
            _animator = _transform.FindChild("Shark Model").GetComponent<Animator>();

            _terrainCenter = new Vector3(
                Terrain.activeTerrain.terrainData.size.x / 2.0f,
                Constants.Shark_Swim_Depth,
                Terrain.activeTerrain.terrainData.size.z / 2.0f);
        }

        private void HandleExitComplete()
        {
            gameObject.SetActive(false);
        }

        public void SetForAttack(Transform target, Vector3 startPosition)
        {
            _transform.position = startPosition;
            _target = target;

            _animator.SetBool("IsAttacking", false);
            _animator.SetBool("IsExiting", false);
            _animator.Play("Enter");
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
            if ((message == EventMessage.Left_Deep_Water) && (originator == _target))
            {
                SetForExit();
            }
        }

        private void SetForExit()
        {
            _target = null;
            _rigidBody.velocity = Vector3.zero;
            _animator.SetBool("IsExiting", true);
            _animator.SetBool("IsAttacking", false);
            _animator.GetBehaviour<SharkExitAnimationStateChange>().StateExitHandler = HandleExitComplete;
        }

        private void Update()
        {
            if (_target != null)
            {
                UpdateForAttack();
            }
        }

        private void UpdateForAttack()
        {
            Vector3 targetPlanePosition = new Vector3(_target.position.x, _transform.position.y, _target.position.z);
            bool inAttackRange = Vector3.Distance(_transform.position, targetPlanePosition) < Attack_Distance;

            _animator.SetBool("IsAttacking", inAttackRange);
            _transform.LookAt(targetPlanePosition);
            _rigidBody.velocity = _transform.forward * (inAttackRange ? Attack_Move_Speed : Basic_Move_Speed);
        }

        private const float Attack_Distance = 5.0f;
        private const float Basic_Move_Speed = 6.0f;
        private const float Attack_Move_Speed = 8.0f;
        private const float Exit_Distance = 10.0f;
    }
}
