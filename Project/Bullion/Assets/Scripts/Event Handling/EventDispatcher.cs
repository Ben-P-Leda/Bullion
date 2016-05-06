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
            if (_instance == null)
            {
                _instance = new EventDispatcher();
            }
            _instance.Dispatch(originator, target, message, value);
        }        

        public EventDispatcher()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                throw new System.Exception("Only one EventDispatcher instance can be created.");
            }
        }

        private void Dispatch(Transform originator, Transform target, string message, float value)
        {
            EventHandler(originator, target, message, value);
        }
    }
}