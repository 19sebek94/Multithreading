using System.Threading;
using Xunit;
using Xunit.Abstractions;

namespace InterlockedSample
{
    /// <summary>
    /// Interlocked class is a thread safe class which provides 
    /// handling atomic operations like e.g. incrementing shared variable.
    /// Interlocked does not require explicit lock statement, but its static methods 
    /// are still being done under lock behind the scene
    /// </summary>
    public class InterlockedTests
    {
        private readonly ITestOutputHelper output;
        private int sharedValue = 0;

        public InterlockedTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Theory]
        [InlineData(100, 200)]
        [InlineData(1000, 2000)]
        public void InterlockedIncrementingSharedValueTest(int increment, int expectedResult)
        {
            Thread writer1Thread = new Thread(() =>
            {
                for (int i = 1; i <= increment; i++)
                {
                    Interlocked.Increment(ref sharedValue);
                    Thread.Sleep(100);
                }
            });

            Thread writer2Thread = new Thread(() =>
            {
                for (int i = 1; i <= increment; i++)
                {
                    Interlocked.Increment(ref sharedValue);
                    Thread.Sleep(100);
                }
            });

            writer1Thread.Start();
            writer2Thread.Start();

            writer1Thread.Join();
            writer2Thread.Join();

            Assert.Equal(expectedResult, sharedValue);

            output.WriteLine("Program completed.");
        }
    }
}