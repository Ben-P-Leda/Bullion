using UnityEngine;

namespace Assets.Scripts.Gameplay.Player
{
    public class PlayerInput : MonoBehaviour
    {
        public string AxisPrefix;

        public float Horizontal { get; private set; }
        public float Vertical { get; private set; }
        public bool Attack { get; private set; }
        public bool Rush { get; set; }

        private void Update()
        {
            Horizontal = Input.GetAxis(AxisPrefix + "-Horizontal");
            Vertical = Input.GetAxis(AxisPrefix + "-Vertical");
            Attack = Input.GetButtonDown(AxisPrefix + "-Attack");
            Rush = Input.GetButtonDown(AxisPrefix + "-Rush");
        }
    }
}