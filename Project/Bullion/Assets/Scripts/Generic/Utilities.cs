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

        public static int ScaledFontSize(float fontSize)
        {
            return (int)(fontSize * Scale);
        }

        public static float DistanceOverGroundSquared(Vector3 center, Vector3 target)
        {
            float xDistance = target.x - center.x;
            float zDistance = target.z - center.z;

            return ((xDistance * xDistance) + (zDistance * zDistance));
        }
    }
}