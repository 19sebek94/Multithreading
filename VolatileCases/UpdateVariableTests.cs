using Xunit.Abstractions;

namespace VolatileCases
{
    /// <summary>
    /// This is example of updating and reading value from shared variable by multiple threads. 
    /// One thread should update variable 100 and the other should read value 100 times.
    /// Thanks to volatile we have correct state everytime reading the value.
    /// 
    /// Value is shared in memory between threads everytime when changed. 
    /// Volatile is good when one thread only reads value and the othere only makes only writes.
    /// It is not safe e.g. while incrementing value where read and write happens one after another,
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
                    sharedVolatileValue = i;
                    output.WriteLine($"{DateTime.Now.ToString("HH:mm:ss:fffffff")} done: {i}");
                    Thread.Sleep(100);
                }
            });

            Thread readerThread = new Thread(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    output.WriteLine($"{DateTime.Now.ToString("HH:mm:ss:fffffff")} read: {sharedVolatileValue}");
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