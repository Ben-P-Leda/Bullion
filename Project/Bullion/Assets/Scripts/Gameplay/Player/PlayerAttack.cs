using UnityEngine;
using Assets.Scripts.Configuration;
using Assets.Scripts.Gameplay.Avatar;

namespace Assets.Scripts.Gameplay.Player
{
    public class PlayerAttack : MonoBehaviour, IConfigurable, IAnimated
    {
        private Animator _animator;
        private PlayerInput _input;
        private PlayerMovement _movement;

        private int _comboStepCount;

        public CharacterConfiguration Configuration { private get; set; }
        public bool CanAttack { private get; set; }

        public Animator Animator
        {
            set
            {
                _animator = value;
                _animator.transform.GetComponent<AvatarAttackAnimationEventHandler>().StrikeCallback = SetStrikingState;
                _animator.GetBehaviour<AvatarRestingAnimationStateChange>().AddStateEntryHandler(EndComboSequence);
            }
        }

        private void SetStrikingState(bool strikeInProgress)
        {
            if (strikeInProgress)
            {
                _animator.SetBool("IsAttacking", false);
                _comboStepCount += 1;
            }
        }

        private void EndComboSequence()
        {
            _animator.SetBool("IsAttacking", false);
            _comboStepCount = 0;
        }

        private void Start()
        {
            _input = GetComponent<PlayerInput>();
            _movement = GetComponent<PlayerMovement>();

            _comboStepCount = 0;
        }

        private void Update()
        {
            if ((CanAttack) && (_comboStepCount < Configuration.ComboStepCount) && (_input.Attack))
            {
                _movement.CanMove = false;
                _animator.SetBool("IsAttacking", true);
            }
        }
    }
}