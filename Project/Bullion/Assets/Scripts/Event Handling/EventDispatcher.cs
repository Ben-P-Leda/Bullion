using UnityEngine;

namespace Assets.Scripts.Event_Handling
{
    public class EventDispatcher
    {
        public delegate void EventCallback(Transform originator, Transform target, string message, float value);

        private static EventDispatcher _instance = null;
        public static event EventCallback EventHandler;
        
        public static void FireEvent(Transform originator, Transform target, string message, float value)
        {
            _instance = _instance == null ? new EventDispatcher() : _instance;
            _instance.Dispatch(originator, target, message, value);
        }

        private void Dispatch(Transform originator, Transform target, string message, float value)
        {
            EventHandler(originator, target, message, value);
        }
    }
}