using Xunit.Abstractions;

namespace VolatileCases
{
    /// <summary>
    /// This is example of updating and reading value from shared variable by multiple threads. 
    /// One thread should update variable 100 and the other should read value 100 times.
    /// Thanks to volatile we have the latest state everytime reading the value.
    /// 
    /// However, volatile does not prevent from race condition. In that case we would have to 
    /// add additional locks while writing and reading shared value.
    /// 
    /// Value is shared in memory between threads everytime when changed. 
    /// It is not safe e.g. while incrementing value where read, modify, write happen one after another,
    /// because volatile does not prevent from out of order execution of processor which can 
    /// change the order of executing writes and reads in multithreaded environment
    /// 
    /// Note from MSDN:
    /// On a multiprocessor system, a volatile read operation does not 
    /// guarantee to obtain the latest value written to that memory 
    /// location by any processor.Similarly, a volatile write operation 
    /// does not guarantee that the value written would be immediately 
    /// visible to other processors.
    /// </summary>
    public class UpdateVariableTests
    {
        private readonly ITestOutputHelper output;
        private volatile int sharedVolatileValue = 0;
        private object lockInstance = new object();

        public UpdateVariableTests(ITestOutputHelper output)
        {
            this.output = output;
        }


        [Fact]
        public void UpdateVolatileVariableTest()
        {
            Thread writerThread = new Thread(() =>
            {
                for (int i = 1; i <= 100; i++)
                {
                    output.WriteLine($"{DateTime.Now.ToString("HH:mm:ss:fffffff")} save: {i}");
                    lock (lockInstance)
                    {
                        sharedVolatileValue = i;
                    }
                    output.WriteLine($"{DateTime.Now.ToString("HH:mm:ss:fffffff")} saving done: {i}");
                    Thread.Sleep(100);
                }
            });

            Thread readerThread = new Thread(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    lock (lockInstance)
                    {
                        output.WriteLine($"{DateTime.Now.ToString("HH:mm:ss:fffffff")} read: {sharedVolatileValue}");
                    }
                    Thread.Sleep(100);
                }
            });

            writerThread.Start();
            readerThread.Start();

            writerThread.Join();
            readerThread.Join();

            output.WriteLine("Program completed.");
        }
    }
}