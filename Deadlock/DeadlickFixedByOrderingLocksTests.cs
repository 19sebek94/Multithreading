using Xunit.Abstractions;

namespace Deadlock
{
    /// <summary>
    /// The best and the simplest solution for deadlocks. 
    /// Ordering locks in the same order in every part of application
    /// prevents application from deadlock when locks are nested.
    /// </summary>
    public class DeadlickFixedByOrderingLocksTests
    {
        private object lock1 = new();
        private object lock2 = new();

        private readonly ITestOutputHelper output;

        public DeadlickFixedByOrderingLocksTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void DeadlockFixedWithCorrectLockingOrderTest()
        {
            Thread thread1 = new Thread(AcquireLock1);
            Thread thread2 = new Thread(AcquireLock2WithFixedLockOrder);

            thread1.Start();
            thread2.Start();
            thread1.Join();
            thread2.Join();
        }

        private void AcquireLock1()
        {
            var threadId = Thread.CurrentThread.ManagedThreadId;

            lock (lock1)
            {
                output.WriteLine($"Thread {threadId} acquired lock 1.");
                Thread.Sleep(1000);
                output.WriteLine($"Thread {threadId} attempted to acquire lock 2.");
                lock (lock2)
                {
                    output.WriteLine($"Thread {threadId} acquired lock 2.");
                }
            }
        }

        private void AcquireLock2WithFixedLockOrder()
        {
            var threadId = Thread.CurrentThread.ManagedThreadId;

            lock (lock1)
            {
                output.WriteLine($"Thread {threadId} acquired lock 2.");
                Thread.Sleep(1000);
                output.WriteLine($"Thread {threadId} attempted to acquire lock 2.");
                lock (lock2)
                {
                    output.WriteLine($"Thread {threadId} acquired lock 2.");
                }
            }
        }
    }
}
