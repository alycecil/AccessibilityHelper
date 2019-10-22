using System;

namespace runner
{
    public interface ITickable
    {
        void tickCommon(long tick, IntPtr baseHandle);
    }
}