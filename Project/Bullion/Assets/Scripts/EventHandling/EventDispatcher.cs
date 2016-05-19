using UnityEngine;

namespace Assets.Scripts.EventHandling
{
    public class EventDispatcher
    {
        public delegate void MessageEventCallback(Transform originator, Transform target, string message);
        public delegate void BoolEventCallback(Transform originator, Transform target, string message, bool value);
        public delegate void FloatEventCallback(Transform originator, Transform target, string message, float value);

        private static EventDispatcher _instance = null;

        public static event MessageEventCallback MessageEventHandler;
        public static event BoolEventCallback BoolEventHandler;
        public static event FloatEventCallback FloatEventHandler;

        public static void FireEvent(Transform originator, Transform target, string message)
        {
            _instance = _instance == null ? new EventDispatcher() : _instance;
            _instance.Dispatch(originator, target, message);
        }

        public static void FireEvent(Transform originator, Transform target, string message, bool value)
        {
            _instance = _instance == null ? new EventDispatcher() : _instance;
            _instance.Dispatch(originator, target, message, value);
        }

        public static void FireEvent(Transform originator, Transform target, string message, float value)
        {
            _instance = _instance == null ? new EventDispatcher() : _instance;
            _instance.Dispatch(originator, target, message, value);
        }

        private void Dispatch(Transform originator, Transform target, string message)
        {
            MessageEventHandler(originator, target, message);
        }

        private void Dispatch(Transform originator, Transform target, string message, bool value)
        {
            BoolEventHandler(originator, target, message, value);
        }

        private void Dispatch(Transform originator, Transform target, string message, float value)
        {
            FloatEventHandler(originator, target, message, value);
        }
    }
}