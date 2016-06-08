using UnityEngine;
using Assets.Scripts.EventHandling;

namespace Assets.Scripts.Gameplay.Player.Support
{
    public class RespawnPoint : MonoBehaviour
    {
        private Transform _transform;
        private ParticleSystem _particles;
        private CapsuleCollider _collider;

        public Transform Player { private get; set; }

        private void Start()
        {
            _transform = transform;
            _particles = _transform.GetComponent<ParticleSystem>();
            _collider = _transform.GetComponent<CapsuleCollider>();
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
            if ((target == Player) && (message == EventMessage.Enter_Dead_Mode))
            {
                _particles.Play();
                _collider.enabled = true;
            }

            if ((target == _transform) && (originator == Player) && (message == EventMessage.Hit_Trigger_Collider))
            {
                EventDispatcher.FireEvent(_transform, Player, EventMessage.Respawn);
                _particles.Stop();
                _collider.enabled = false;
            }
        }
    }
}