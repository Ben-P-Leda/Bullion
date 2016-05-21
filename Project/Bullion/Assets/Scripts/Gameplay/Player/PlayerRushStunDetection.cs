using UnityEngine;
using Assets.Scripts.EventHandling;

namespace Assets.Scripts.Gameplay.Player
{
    public class PlayerRushStunDetection : MonoBehaviour
    {
        Transform _transform;
        Transform _parentPlayerTransform;

        private void Start()
        {
            _transform = transform;
            _parentPlayerTransform = _transform.parent;
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.tag == Obstruction_Tag)
            {
                EventDispatcher.FireEvent(_transform, _parentPlayerTransform, EventMessage.Rush_Stun_Impact);
            }
        }

        private const string Obstruction_Tag = "Obstruction";
    }
}