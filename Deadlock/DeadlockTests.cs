using Xunit.Abstractions;

namespace Deadlock
{
    /// <summary>
    /// Simple example of deadlock.
    /// Test will not end.
    /// </summary>
    public class DeadlockTests
    {
        private object lock1 = new();
        private object lock2 = new();

        private readonly ITestOutputHelper output;

        public DeadlockTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void DeadlockOccuredTest()
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

        private void AcquireLock2()
        {
            var threadId = Thread.CurrentThread.ManagedThreadId;

            lock (lock2)
            {
                output.WriteLine($"Thread {threadId} acquired lock 2.");
                Thread.Sleep(1000);
                output.WriteLine($"Thread {threadId} attempted to acquire lock 1.");
                lock (lock1)
                {
                    output.WriteLine($"Thread {threadId} acquired lock 1.");
                }
            }
        }

        //    [Fact]
        //    public void TimeoutOccuredWhenDeadlockTest()
        //    {
        //        output.WriteLine("Main Thread Started");
        //        Wallet wallet1 = new Wallet(100, 5000);
        //        Wallet wallet2 = new Wallet(200, 3000);
        //        Bank bank1 = new AliorBank(wallet1, wallet2, 300);
        //        Thread thread1 = new Thread(bank1.TransferMoney)
        //        {
        //            Name = "Thread1"
        //        };

        //        Bank bank2 = new AliorBank(wallet2, wallet1, 500);
        //        Thread thread2 = new Thread(bank2.TransferMoney)
        //        {
        //            Name = "Thread2"
        //        };

        //        thread1.Start();
        //        thread2.Start();
        //        var timeout1 = thread1.Join(5000);
        //        if (!timeout1)
        //        {
        //            throw new Exception("Deadlock appeared");
        //        }

        //        var timeout2 = thread2.Join(5000);

        //        if (!timeout2)
        //        {
        //            throw new Exception("Deadlock appeared");
        //        }
        //    }
    }
}