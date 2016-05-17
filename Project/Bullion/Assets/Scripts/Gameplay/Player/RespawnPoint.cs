using UnityEngine;
using Assets.Scripts.EventHandling;

namespace Assets.Scripts.Gameplay.Player
{
    public class RespawnPoint : MonoBehaviour
    {
        private ParticleSystem _particles;
        private CapsuleCollider _collider;

        public Transform Player { private get; set; }

        private void Start()
        {
            _particles = transform.GetComponent<ParticleSystem>();
            _collider = transform.GetComponent<CapsuleCollider>();
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
            }
        }
    }
}