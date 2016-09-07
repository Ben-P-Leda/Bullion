using UnityEngine;

namespace Assets.Scripts.Gameplay.Sharks
{
    public class SharkAnimationEventHandler : MonoBehaviour
    {
        private GameObject _parentGameObject;

        private void Start()
        {
            _parentGameObject = transform.parent.gameObject;
        }

        public void CompleteExit()
        {
            _parentGameObject.SetActive(false);
        }
    }
}