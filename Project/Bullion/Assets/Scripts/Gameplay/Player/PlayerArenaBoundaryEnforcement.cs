using UnityEngine;
using Assets.Scripts.Generic;
using Assets.Scripts.EventHandling;

namespace Assets.Scripts.Gameplay.Player
{
    public class PlayerArenaBoundaryEnforcement : MonoBehaviour
    {
        private Transform _transform;


        private bool _isInDeadMode;
        private float _arenaRadius;
        private float _arenaRadiusSquared;
        private float _ghostFadeRadiusSquared;

        public Vector3 ArenaCenter { private get; set; }
        public float ArenaBoundaryRadius
        {
            set
            {
                _arenaRadius = value;
                _arenaRadiusSquared = value * value;
                _ghostFadeRadiusSquared = (value - Ghost_Fade_Zone_Width) * (value - Ghost_Fade_Zone_Width);
            }
        }

        private void Start()
        {
            _transform = transform;

            _isInDeadMode = false;
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
                    case EventMessage.Enter_Dead_Mode: _isInDeadMode = true; break;
                    case EventMessage.Respawn: _isInDeadMode = false; break;
                }
            }
        }

        private void Update()
        {
            EnforceBoundaryLimitations();

            if (_isInDeadMode)
            {
                // TODO...
            }
        }

        private void EnforceBoundaryLimitations()
        {
            float _distanceFromCenterSquared = Utilities.DistanceOverGroundSquared(ArenaCenter, _transform.position);
            if (_distanceFromCenterSquared > _arenaRadiusSquared)
            {
                Vector3 limitVector = Vector3.Normalize(_transform.position - ArenaCenter) * _arenaRadius;
                _transform.position = new Vector3(ArenaCenter.x + limitVector.x, _transform.position.y, ArenaCenter.z + limitVector.z);
            }
        }

        private const float Ghost_Fade_Zone_Width = 2.0f;
    }
}