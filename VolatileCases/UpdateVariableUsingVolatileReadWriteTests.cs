namespace VolatileCases
{
    /// <summary>
    /// In the first test we have one check of shared variable outside lock, one check inside lock and two writes.
    /// Due to out of order execution made by processor and compilator to optimize as much as possible, 
    /// order of reading initialized value and writing to initialized value can be different and the result of x variable can be undeterministic.
    /// The solution for that is explicitly use Volatile.Read where reading initialized value and Volatile.Write when writing to sharedBoolValue.
    /// This will cause that operations on this variable will not be optimized or reorderd by processor/compiler.
    /// Additionaly writing sharedIntValue value must be before writing initialized value and Volatile.Write
    /// makes sure that memory barrier occured 
    /// 
    /// Of course using this kind of solution is not without cost. 
    /// As mentioned before, processor/compiler will not optimize multithreaded functionality.
    /// Volatile operations may require cache synchronization or flushing, which can introduce additional latency when accessing shared variables
    /// </summary>
    public class UpdateVariableUsingVolatileReadWriteTests
    {
        private bool initialized = false;
        private int sharedIntValue = 0;
        private object lockInstance = new object();

        [Fact]
        public void WrongExampleWithoutVolatileWriteReadTest()
        {
            if (initialized)
            {
                lock (lockInstance)
                {
                    if (initialized)
                    {
                        initialized = true;
                        sharedIntValue = 10;
                    }
                }
            }

            var x = sharedIntValue;
        }

        [Fact]
        public void CorrectCodeWithVolatileWriteReadTest()
        {
            if (Volatile.Read(ref initialized))
            {
                lock (lockInstance)
                {
                    if (initialized)
                    {
                        sharedIntValue = 10;
                        //Memory barrier
                        Volatile.Write(ref initialized, true);
                    }
                }
            }

            var x = sharedIntValue;
        }
    }
}
