using System.Collections.Concurrent;
using Xunit.Abstractions;

namespace Dictionaries
{
    /// <summary>
    /// This is example of using concurrentDictionary in order to avoid race condition.
    /// Despite the fact that 'ContainsKey' in if statement is not thread safe,
    /// using TryAdd to add key to dictionary is safe. Therefore in dictionary there
    /// is only one value added, and the second add is skipped
    /// </summary>
    public class AddKeyToConcurrentDictionaryTests
    {
        private ConcurrentDictionary<int, string> concurrentDictionary = new ConcurrentDictionary<int, string>();
        private const int keyToAdd = 5;

        private readonly ITestOutputHelper output;

        public AddKeyToConcurrentDictionaryTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void AddKeyToConcurrentDictionaryWhenMultithreadingTest()
        {
            Thread thread1 = new Thread(AddKeyToDictionary);
            Thread thread2 = new Thread(AddKeyToDictionary);

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();

            var addedKeyAmount = this.concurrentDictionary.Keys.Where(x => x == keyToAdd).Count();
            if (addedKeyAmount != 1)
            {
                throw new Exception("Inconsistent data. Two the same keys in dictionary");
            }
            output.WriteLine("Main thread completed");
        }

        private void AddKeyToDictionary()
        {
            var threadId = Thread.CurrentThread.ManagedThreadId;
            // This condition is NOT thread safe
            if (!this.concurrentDictionary.ContainsKey(keyToAdd))
            {
                try
                {
                    Thread.Sleep(5000);
                    output.WriteLine($"Thread {threadId} attempted to add key:{keyToAdd}");
                    var valueAdded = this.concurrentDictionary.TryAdd(keyToAdd, "someValue");

                    if (valueAdded)
                    {
                        output.WriteLine($"Thread {threadId} added key:{keyToAdd} to concurrentDictionary");
                    }
                    else
                    {
                        output.WriteLine($"Thread {threadId} did NOT add key:{keyToAdd} to concurrentDictionary");

                    }
                }
                catch (Exception ex)
                {
                    output.WriteLine(ex.ToString());
                }
            }
        }
    }
}
