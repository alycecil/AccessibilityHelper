using System;
using System.Drawing;

namespace runner
{
    static internal class MouseManagerHelper
    {
        public static string GetButtonTxt(MouseButton button)
        {
            string buttonTxt;
            switch (button)
            {
                case MouseButton.LEFT:
                    buttonTxt = "LEFT";
                    break;
                case MouseButton.RIGHT:
                    buttonTxt = "RIGHT";
                    break;
                case MouseButton.MIDDLE:
                    buttonTxt = "MIDDLE";
                    break;
                default:
                    throw new NotImplementedException();
            }

            return buttonTxt;
        }

        public static void Scale(IntPtr baseHandle, int x, int y, out int scaledX, out int scaledY)
        {
            WindowHandleInfo.GetScale(baseHandle, out float sX, out float sY);

            scaledX = (int) (x * sX);
            scaledY = (int) (y * sY);
        }

        public static void Offset(IntPtr baseHandle,
            int scaledX,  int scaledY, out int offsetX, out int offsetY)
        {
            //todo rect from bounds adds
            WindowHandleInfo.GetBounds(baseHandle, out Rectangle rectangle);

            rectangle.X += scaledX;
            rectangle.Y += scaledY;

            offsetX = rectangle.X;
            offsetY = rectangle.Y;
        }
    }
}