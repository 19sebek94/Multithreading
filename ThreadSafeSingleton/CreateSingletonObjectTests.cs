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
        public void CreateNotThreadSafeSingletonObjectTest()
        {
            Thread thread1 = new Thread(() => NotThreadSafeSingleton.GetInstance());
            Thread thread2 = new Thread(() => NotThreadSafeSingleton.GetInstance());

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();

            output.WriteLine($"Number of objects: {NotThreadSafeSingleton.ObjectCounter}");
            Assert.Equal(1, NotThreadSafeSingleton.ObjectCounter);
        }

        [Fact]
        public void CreateLockedSingletonObjectTest()
        {
            Thread thread1 = new Thread(() => LockedSingleton.GetInstance());
            Thread thread2 = new Thread(() => LockedSingleton.GetInstance());

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();

            output.WriteLine($"Number of objects: {LockedSingleton.ObjectCounter}");
            Assert.Equal(1, LockedSingleton.ObjectCounter);
        }

        [Fact]
        public void CreateLazySingletonObjectTest()
        {
            Thread thread1 = new Thread(() => LazySingleton.GetInstance());
            Thread thread2 = new Thread(() => LazySingleton.GetInstance());

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();

            output.WriteLine($"Number of objects: {LazySingleton.ObjectCounter}");
            Assert.Equal(1, LazySingleton.ObjectCounter);
        }
    }
}