using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerInput : MonoBehaviour
    {
        public string AxisPrefix;

        public float Horizontal { get; private set; }
        public float Vertical { get; private set; }

        private void FixedUpdate()
        {
            Horizontal = Input.GetAxis(AxisPrefix + "-Horizontal");
            Vertical = Input.GetAxis(AxisPrefix + "-Vertical");
        }
    }
}