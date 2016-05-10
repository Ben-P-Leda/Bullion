using System;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Player
{
    public class PlayerAttack : MonoBehaviour, IAnimated
    {
        private PlayerInput _input;
        private PlayerMovement _movement;

        public Animator Animator { private get; set; }
        public bool CanAttack { private get; set; }

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
                Animator.SetBool("IsAttacking", true);
            }
        }
    }
}