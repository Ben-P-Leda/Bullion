using UnityEngine;
using Assets.Scripts.EventHandling;

namespace Assets.Scripts.Gameplay.Player
{
    public class PlayerInput : MonoBehaviour
    {
        private Transform _transform;
        private bool _readyForRoundExit;

        public string AxisPrefix { private get; set; }
        public float Horizontal { get; private set; }
        public float Vertical { get; private set; }
        public bool Attack { get; private set; }
        public bool Rush { get; set; }

        private void Start()
        {
            _transform = transform;
            _readyForRoundExit = false;
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
            if (message == EventMessage.End_Round)
            {
                _readyForRoundExit = true;
            }
        }

        private void Update()
        {
            Horizontal = Input.GetAxis(AxisPrefix + "-Horizontal");
            Vertical = Input.GetAxis(AxisPrefix + "-Vertical");
            Attack = Input.GetButtonDown(AxisPrefix + "-Attack");
            Rush = Input.GetButtonDown(AxisPrefix + "-Rush");

            if ((_readyForRoundExit) && (Attack))
            {
                EventDispatcher.FireEvent(_transform, _transform, EventMessage.Exit_Gameplay);
            }
        }
    }
}