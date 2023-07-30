using Xunit.Abstractions;

namespace ThreadSafeSingleton
{
    /// <summary>
    /// First test: CreateSingletonDirectorObjectTest should throw assertion error
    /// since we managed to create 2 singleton objects by two threads.
    /// 
    /// Rest of tests should pass since applied singleton classes are thread safe.
    /// </summary>
    public class CreateSingletonObjectTests
    {
        private readonly ITestOutputHelper output;

        public CreateSingletonObjectTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void CreateSingletonDirectorObjectTest()
        {
            Thread thread1 = new Thread(() => NotThreadSafeSingletonDirector.GetInstance());
            Thread thread2 = new Thread(() => NotThreadSafeSingletonDirector.GetInstance());

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();

            output.WriteLine($"Number of objects: {NotThreadSafeSingletonDirector.ObjectCounter}");
            Assert.Equal(1, NotThreadSafeSingletonDirector.ObjectCounter);
        }

        [Fact]
        public void CreateSingletonManagerObjectTest()
        {
            Thread thread1 = new Thread(() => ThreadSafeLockedSingletonManager.GetInstance());
            Thread thread2 = new Thread(() => ThreadSafeLockedSingletonManager.GetInstance());

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();

            output.WriteLine($"Number of objects: {ThreadSafeLockedSingletonManager.ObjectCounter}");
            Assert.Equal(1, ThreadSafeLockedSingletonManager.ObjectCounter);
        }
    }
}