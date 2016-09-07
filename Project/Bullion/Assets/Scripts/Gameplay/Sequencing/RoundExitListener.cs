using UnityEngine;
using Assets.Scripts.EventHandling;

namespace Assets.Scripts.Gameplay.Sequencing
{
    public class RoundExitListener : MonoBehaviour
    {
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
            if (message == EventMessage.Exit_Gameplay)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
            }
        }
    }
}