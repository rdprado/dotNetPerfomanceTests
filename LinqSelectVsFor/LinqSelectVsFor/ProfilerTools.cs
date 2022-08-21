using System;
using System.Diagnostics;
using System.Threading;

namespace LinqSelectVsFor
{
    public class ProfilerTools
    {
        static public void PrepareCPU()
        {
            //Run at highest priority to minimize fluctuations caused by other processes/threads
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
        }

        static public void CleanUp()
        {
            // clean up
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}
