using System.Collections.Generic;

namespace Signals
{
    public static class AssemblySignals
    {
        public struct StartAssemblyLoopSignal
        {
        }

        public struct StopAssemblyLoopSignal
        {
        }

        public struct AssemblyCompleteSignal
        {
        }

        public struct AssembledPartSignal
        {
        }
        
        
        public struct UpdateTimeSignal
        {
            public float time;
        }

        public struct UpdateRecordsSignal
        {
            public List<float> timeRecords;
        }

        public struct UpdateLoopsSignal
        {
            public int currentLoop;
            public int totalLoops;
        }
    }
}