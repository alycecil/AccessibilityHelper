using System;
using System.Drawing;
using System.Threading;
using AutoIt;
using IO.Swagger.Model;
using runner.Cursor;

namespace runner.ActionWorkers
{
    enum Direction
    {
        UP,
        DOWN,
        LEFT,
        RIGHT,
        DOOR,
        TRAPDOOR,
        EXIT
    }


    public class ActionMove : AbstractActionWorker
    {
        public static bool handle(IntPtr baseHandle, Event currentEvent)
        {
            if (currentEvent.Targets.Count != 1)
            {
                throw new NotImplementedException();
            }


            var direction = currentEvent.Targets[0].Name;


            if (Direction.TryParse(direction, true, out Direction directionE))
            {
                Span span;
                int constantSide, start, end;
                Bitmap desired;
                Console.WriteLine("Move requested [{0}]", directionE);
                switch (directionE)
                {
                    case Direction.UP:
                        span = Span.Hori;
                        constantSide = 25;
                        start = 4;
                        end = 640;
                        desired = CursorUtil.up;
                        break;
                    case Direction.DOWN:
                        span = Span.Hori;
                        constantSide = 310;
                        start = 310;
                        end = 640;
                        desired = CursorUtil.down;
                        break;
                    case Direction.LEFT:
                        span = Span.Vert;
                        constantSide = 4;
                        start = 110;
                        end = 300;
                        desired = CursorUtil.left;
                        break;
                    case Direction.RIGHT:
                        constantSide = 640;
                        start = 110;
                        end = 280;
                        span = Span.Vert;
                        desired = CursorUtil.right;
                        break;
                    default:
                        throw new NotImplementedException();
                }
                Thread.Sleep(TimeSpan.FromSeconds(1));
                return findPlaceToClickAndClick(baseHandle, desired, span, constantSide, start, end);
            }

            return false;
        }

        enum Span
        {
            Vert,
            Hori
        }

        static bool findPlaceToClickAndClick(IntPtr baseHandle, Bitmap desired, Span spanType, int constantSide, int start, int end)
        {
            for (int slide = start; slide < end; slide += (end - start) / 20)
            {
                int x, y;
                switch (spanType)
                {
                    case Span.Vert:
                        x = constantSide;
                        y = slide;
                        break;
                    case Span.Hori:
                        x = slide;
                        y = constantSide;
                        break;
                    default:
                        throw new NotImplementedException();
                }

                MouseManager.MouseMoveUnScaled(baseHandle, x, y, 1);
                if (!CursorUtil.isCursor(desired, out int clickx, out int clickY))
                {
                    Thread.Sleep(TimeSpan.FromMilliseconds(100)); 
                    continue;
                }
                MouseManager.MouseClick(baseHandle, x, y);
                Thread.Sleep(TimeSpan.FromSeconds(1));
                return true;
            }


            //ScreenCapturer.ImageSave("Cursor_%NOW%", ImageFormat.Tiff, cursor);


            return false;
        }
    }
}