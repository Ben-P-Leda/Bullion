using UnityEngine;
using Assets.Scripts.Gameplay.Avatar;

namespace Assets.Scripts.Gameplay.Player
{
    public class PlayerAttack : MonoBehaviour, IAnimated
    {
        private Animator _animator;
        private PlayerInput _input;
        private PlayerMovement _movement;

        public bool CanAttack { private get; set; }

        public Animator Animator
        {
            set
            {
                _animator = value;
                _animator.transform.GetComponent<AvatarAttackAnimationEventHandler>().StrikeCallback = SetStrikingState;
            }
        }

        private void SetStrikingState(bool strikeInProgress)
        {
            if (strikeInProgress)
            {
                _animator.SetBool("IsAttacking", false);
            }
        }

        private void Start()
        {
            _input = GetComponent<PlayerInput>();
            _movement = GetComponent<PlayerMovement>();
        }

        private void Update()
        {
            if ((CanAttack) && (_input.Attack))
            {
                _movement.CanMove = false;
                _animator.SetBool("IsAttacking", true);
            }
        }
    }
}