using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Dictionaries
{
    /// <summary>
    /// This is one of possible solutions to resolve problem with race condition.
    /// We simply lock dictionary before checking if given key exists in dictionary.
    /// Now our results are preductible and data is consistent.
    /// </summary>
    public class AddKeyToLockedDictionaryTests
    {
        private Dictionary<int, string> dictionary = new Dictionary<int, string>();
        private const int keyToAdd = 5;

        private readonly ITestOutputHelper output;

        public AddKeyToLockedDictionaryTests(ITestOutputHelper output)
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
            output.WriteLine("Main thread completed");
        }

        private void AddKeyToDictionary()
        {
            var threadId = Thread.CurrentThread.ManagedThreadId;
            lock (this.dictionary)
            {
                if (!this.dictionary.ContainsKey(keyToAdd))
                {
                    try
                    {
                        Thread.Sleep(5000);
                        output.WriteLine($"Thread {threadId} attempted to add key:{keyToAdd}");
                        this.dictionary.Add(keyToAdd, "someValue");
                        output.WriteLine($"Thread {threadId} added key:{keyToAdd} to dictionary.");
                    }
                    catch (Exception ex)
                    {
                        output.WriteLine(ex.ToString());
                    }
                }
            }
        }
    }
}
