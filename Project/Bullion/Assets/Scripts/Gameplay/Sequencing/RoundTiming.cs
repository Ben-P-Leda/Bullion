using UnityEngine;
using Assets.Scripts.EventHandling;

namespace Assets.Scripts.Gameplay.Sequencing
{
    public class RoundTiming : MonoBehaviour
    {
        private Transform _transform;

        private float _timer;
        private bool _roundStarted;
        private bool _roundComplete;

        public float RoundDuration;

        private void Start()
        {
            _transform = transform;

            _timer = Round_Start_Delay;
            _roundStarted = false;
            _roundComplete = false;
        }

        private void FixedUpdate()
        {
            _timer = Mathf.Max(0.0f, _timer - Time.deltaTime);
            if ((!_roundComplete) && (_timer == 0.0f))
            {
                if (!_roundStarted)
                {
                    _timer = RoundDuration;
                    EventDispatcher.FireEvent(_transform, _transform, EventMessage.Set_Round_Timer, RoundDuration);
                    EventDispatcher.FireEvent(_transform, _transform, EventMessage.Start_Round);
                    _roundStarted = true;
                }
                else if (!_roundComplete)
                {
                    EventDispatcher.FireEvent(_transform, _transform, EventMessage.End_Round);
                    _roundComplete = true;
                }
            }
        }

        private const float Round_Start_Delay = 1.5f;
    }
}