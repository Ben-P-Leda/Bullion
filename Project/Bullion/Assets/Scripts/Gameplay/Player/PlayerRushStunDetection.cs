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
            if (collider.tag == Constants.Obstruction_Tag)
            {
                EventDispatcher.FireEvent(_transform, _parentPlayerTransform, EventMessage.Rush_Stun_Impact);
            }

            if (collider.tag == Constants.Rush_Collider_Tag)
            {
                EventDispatcher.FireEvent(collider.transform.parent, _parentPlayerTransform, EventMessage.Rush_Head_On_Collision);
            }
        }
    }
}