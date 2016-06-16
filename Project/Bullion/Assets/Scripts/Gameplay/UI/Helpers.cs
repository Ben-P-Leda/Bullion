using UnityEngine;

namespace Assets.Scripts.Gameplay.UI
{
    public static class Helpers
    {
        public static Rect CreateDisplayContainer(int playerIndex, Vector2 margins, Vector2 dimensions)
        {
            int unitX = playerIndex % 2;
            int unitY = playerIndex / 2;

            Vector2 offset = new Vector2(Screen.width - ((margins.x * 2.0f) + dimensions.x), Screen.height - ((margins.y * 2.0f) + dimensions.y));

            Vector2 topLeft = new Vector2(margins.x + (offset.x * unitX), margins.y + (offset.y * unitY));

            return new Rect(topLeft.x, topLeft.y, dimensions.x, dimensions.y);
        }

        public const float UI_Margin = 20.0f;
        public static readonly Vector2 UI_Dimensions = new Vector2(150.0f, 50.0f);
    }
}
