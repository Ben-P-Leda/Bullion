using UnityEngine;
using Assets.Scripts.EventHandling;

namespace Assets.Scripts.GameSetup.AvatarSelection
{
    public class SelectorInput : MonoBehaviour
    {
        private Transform _transform;
        private float _previousStep;

        public string AxisPrefix;

        private void Start()
        {
            _transform = transform;
            _previousStep = 0.0f;
        }

        private void Update()
        {
            if (Input.GetButtonDown(AxisPrefix + "-Attack"))
            {
                EventDispatcher.FireEvent(_transform, _transform, EventMessage.Avatar_Selector_Confirm);
            }

            float step = Input.GetAxis(AxisPrefix + "-Horizontal");
            if (ShouldStep(step))
            {
                EventDispatcher.FireEvent(_transform, _transform, EventMessage.Avatar_Selection_Step, step);
            }
            _previousStep = step;
        }

        private bool ShouldStep(float step)
        {
            if (step != 0.0f)
            {
                if (_previousStep == 0.0f) { return true; }
                if ((step < 0.0f) && (_previousStep >= 0.0f)) { return true; }
                if ((step > 0.0f) && (_previousStep <= 0.0f)) { return true; }
            }

            return false;
        }
    }
}
