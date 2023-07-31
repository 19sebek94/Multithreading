namespace VolatileCases
{
    /// <summary>
    /// In the first test we have one check of shared variable outside lock, one check inside lock and two writes.
    /// Due to out of order execution made by processor and compilator to optimize as much as possible 
    /// order of reading sharedBoolValue and writing to sharedBoolValue can be different and the result of x variable can be undeterministic.
    /// The solution for that is explicitly use Volatile.Read where reading sharedBoolValue and Volatile.Write when writing to sharedBoolValue.
    /// This will cause that operations on this variable will not be optimized or reorderd by processor/compiler.
    /// Additionally we can ensure that processor executes logic in correct order by adding Thread.MemoryBarrier();
    /// 
    /// Of course using this kind of solution is not without cost. 
    /// As mentioned before, processor/compiler will not optimize multithreaded functionality.
    /// Volatile operations may require cache synchronization or flushing, which can introduce additional latency when accessing shared variables
    /// </summary>
    public class UpdateVariableUsingVolatileReadWriteTests
    {
        private bool sharedBoolValue = false;
        private int sharedIntValue = 0;
        private object lockInstance = new object();

        [Fact]
        public void RunCodeWithoutVolatileWriteReadTest()
        {
            if (sharedBoolValue)
            {
                lock (lockInstance)
                {
                    if (sharedBoolValue)
                    {
                        sharedBoolValue = true;
                        sharedIntValue = 10;
                    }
                }
            }

            var x = sharedIntValue;
        }

        [Fact]
        public void RunCodeWithVolatileWriteReadTest()
        {
            if (Volatile.Read(ref sharedBoolValue))
            {
                lock (lockInstance)
                {
                    if (sharedBoolValue)
                    {
                        Thread.MemoryBarrier();
                        Volatile.Write(ref sharedBoolValue, true);
                        Thread.MemoryBarrier();
                        sharedIntValue = 10;
                    }
                }
            }

            var x = sharedIntValue;
        }
    }
}
