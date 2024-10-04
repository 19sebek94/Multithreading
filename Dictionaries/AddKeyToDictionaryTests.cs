using Xunit.Abstractions;

namespace Dictionaries
{
    /// <summary>
    /// Here we can see simple example of race condition.
    /// Line which checks if given key exists in dictionary is not thread safe.
    /// Both threads managed to pass this condition before updating dictionary.
    /// In that case the system is unpredictable and unstable.
    /// In this test depending on the roulette we can either achieve 
    /// error or dictionary with rubish keys or any other inconsistent result
    /// </summary>
    public class AddKeyToDictionaryTests
    {
        private Dictionary<int, string> dictionary = new Dictionary<int, string>();
        private const int keyToAdd = 5;

        private readonly ITestOutputHelper output;

        public AddKeyToDictionaryTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void AddKeyToSimpleDictionaryWhenMultithreadingTest()
        {
            Thread thread1 = new Thread(AddKeyToDictionary);
            Thread thread2 = new Thread(AddKeyToDictionary);

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();

            var addedKeyAmount = this.dictionary.Keys.Where(x => x == keyToAdd).Count();
            if (addedKeyAmount != 1)
            {
                throw new Exception("Inconsistent data. Two the same keys in dictionary");
            }


            output.WriteLine($"Dictionary keys: {string.Join(", ", dictionary.Keys)}");
            output.WriteLine("Main thread completed");
        }

        private void AddKeyToDictionary()
        {
            var threadId = Thread.CurrentThread.ManagedThreadId;
            if (!this.dictionary.ContainsKey(keyToAdd))
            {
                try
                {
                    Thread.Sleep(5000);
                    output.WriteLine($"Thread {threadId} attempted to add key:{keyToAdd}");
                    this.dictionary.Add(keyToAdd, "someValue");
                    output.WriteLine($"Thread {threadId} added key:{keyToAdd} to dictionary");
                }
                catch (Exception ex)
                {
                    output.WriteLine(ex.ToString());
                }
            }
        }
    }
}