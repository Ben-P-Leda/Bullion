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
            //Debug.Log("Complete Exit animation (handler)");
            //_parentGameObject.SetActive(false);
        }
    }
}