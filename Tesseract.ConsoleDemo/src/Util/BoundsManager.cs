using System.Drawing;

static internal class BoundsManager
{
    public static Rectangle BuildBounds(Rectangle rect, int offset, int w, int height, int location)
    {
        return new Rectangle(rect.X + offset, rect.Y + location, w, height);
    }
}