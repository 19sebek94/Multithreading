using Xunit.Abstractions;

namespace Deadlock
{
    /// <summary>
    /// Workaround rather than solution. 
    /// While loop is just an example how to reach the resource in nested locks.
    /// However, this is not efficient since we do not now when the resource will be released. 
    /// Depending on the timout that we set, the retry can meet lock non stop.
    /// This is a roulette when and how fast we will reach the nested resource.
    /// </summary>
    public class DeadlockFixedByUsingTimeoutsAndRetrysTests
    {
        private object lock1 = new();
        private object lock2 = new();

        private readonly ITestOutputHelper output;

        public DeadlockFixedByUsingTimeoutsAndRetrysTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void DeadlockFixedByUsingTimeoutsAndRetrysTest()
        {
            Thread thread1 = new Thread(AcquireLock1);
            Thread thread2 = new Thread(AcquireLock2);

            thread1.Start();
            thread2.Start();
            thread1.Join();
            thread2.Join();
        }

        private void AcquireLock1()
        {
            var threadId = Thread.CurrentThread.ManagedThreadId;

            while (true)
            {
                if (Monitor.TryEnter(lock1, 1500))
                {
                    try
                    {
                        output.WriteLine($"Thread {threadId} acquired lock 1.");
                        Thread.Sleep(1000);
                        output.WriteLine($"Thread {threadId} attempted to acquire lock 2.");
                        if (Monitor.TryEnter(lock2, 1500))
                        {
                            try
                            {
                                output.WriteLine($"Thread {threadId} acquired lock 2.");
                                break;
                            }
                            finally
                            {
                                Monitor.Exit(lock2);
                            }
                        }
                    }
                    finally
                    {
                        Monitor.Exit(lock1);
                    }
                }
            }
        }

        private void AcquireLock2()
        {
            var threadId = Thread.CurrentThread.ManagedThreadId;

            while (true)
            {
                if (Monitor.TryEnter(lock2, 1000))
                {
                    try
                    {
                        output.WriteLine($"Thread {threadId} acquired lock 2.");
                        Thread.Sleep(1000);
                        output.WriteLine($"Thread {threadId} attempted to acquire lock 1.");
                        if (Monitor.TryEnter(lock1, 1000))
                        {
                            try
                            {
                                output.WriteLine($"Thread {threadId} acquired lock 1.");
                                break;
                            }
                            finally
                            {
                                Monitor.Exit(lock1);
                            }
                        }
                    }
                    finally
                    {
                        Monitor.Exit(lock2);
                    }
                }
            }
        }
    }
}
