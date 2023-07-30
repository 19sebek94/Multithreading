using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace VolatileCases
{
    public class UpdateVariableUsingVolatileReadWriteTests
    {
        private readonly ITestOutputHelper output;
        private int sharedValue = 0;

        public UpdateVariableUsingVolatileReadWriteTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void UpdateVariableUsingVolatileTest()
        {
            Thread writerThread = new Thread(() =>
            {
                for (int i = 1; i <= 15; i++)
                {
                    var threadId = Thread.CurrentThread.ManagedThreadId;
                    Volatile.Write(ref sharedValue, i);
                    output.WriteLine($"{DateTime.Now.ToString("HH:mm:ss.fff",CultureInfo.InvariantCulture)}: Thread {threadId} saved {i} into shared variable.");
                    Thread.Sleep(1000);
                }
            });

            Thread readerThread = new Thread(() =>
            {
                for (int i = 0; i < 15; i++)
                {
                    var threadId = Thread.CurrentThread.ManagedThreadId;
                    //int value = Volatile.Read(ref sharedValue);
                    output.WriteLine($"{DateTime.Now.ToString("HH:mm:ss.fff",CultureInfo.InvariantCulture)}: Thread {threadId} read {Volatile.Read(ref sharedValue)} from shared value.");
                    Thread.Sleep(1000);
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
