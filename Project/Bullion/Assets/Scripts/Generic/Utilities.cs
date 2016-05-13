using UnityEngine;

namespace Assets.Scripts.Generic
{
    public static class Utilities
    {
        private static float _scale = 0.0f;
        public static float Scale
        {
            get
            {
                if (_scale == 0.0f)
                {
                    _scale = Screen.height / Definitions.Unit_Screen_Dimensions.y;
                }

                return _scale;
            }
        }
    }
}