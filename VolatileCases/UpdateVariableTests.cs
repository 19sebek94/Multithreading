using Xunit.Abstractions;

namespace VolatileCases
{
    /// <summary>
    /// This is example of updating shared variable by multiple threads. 
    /// Each test has two test cases. One testcase should update variable 100 times per thread.
    /// The other 1000 times per thread. 
    /// We can see that sometimes when variable is not marked as volatile 
    /// There is error in assertion e.g. Should be 2000, but got 1998.
    /// This is why volatile is so important. Without it we can get 
    /// wrong state of variable which can lead to unpredictible application behavior.
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
        private int sharedValue = 0;
        private volatile int sharedVolatileValue = 0;

        public UpdateVariableTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Theory]
        [InlineData(100, 200)]
        [InlineData(1000, 2000)]
        public void UpdateVariableRaceConditionTest(int iterates, int expectedResult)
        {
            Thread writer1Thread = new Thread(() =>
            {
                for (int i = 1; i <= iterates; i++)
                {
                    sharedValue++;
                    Thread.Sleep(100);
                }
            });

            Thread writer2Thread = new Thread(() =>
            {
                for (int i = 0; i < iterates; i++)
                {
                    sharedValue++;
                    Thread.Sleep(100);
                }
            });

            writer1Thread.Start();
            writer2Thread.Start();

            writer1Thread.Join();
            writer2Thread.Join();

            output.WriteLine("Program completed.");
            output.WriteLine($"Value result: {sharedValue}.");
            Assert.Equal(expectedResult, sharedValue);
        }

        [Theory]
        [InlineData(100, 200)]
        [InlineData(1000, 2000)]
        public void UpdateVolatileVariableTest(int iterates, int expectedResult)
        {
            Thread writer1Thread = new Thread(() =>
            {
                for (int i = 1; i <= iterates; i++)
                {
                    sharedVolatileValue++;
                    Thread.Sleep(100);
                }
            });

            Thread writer2Thread = new Thread(() =>
            {
                for (int i = 0; i < iterates; i++)
                {
                    sharedVolatileValue++;
                    Thread.Sleep(100);
                }
            });

            writer1Thread.Start();
            writer2Thread.Start();

            writer1Thread.Join();
            writer2Thread.Join();

            output.WriteLine("Program completed.");
            output.WriteLine($"Value result: {sharedVolatileValue}.");
            Assert.Equal(expectedResult, sharedVolatileValue);
        }
    }
}