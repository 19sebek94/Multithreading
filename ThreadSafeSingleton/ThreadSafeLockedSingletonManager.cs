namespace ThreadSafeSingleton
{
    /// <summary>
    /// Here manager class has implemented lock while checking if manager is null.
    /// Therefore this class is thread safe.
    /// There is additionaly duplicated check condition in order to improve 
    /// performance and avoide creating unnecessary locks
    /// </summary>
    public class ThreadSafeLockedSingletonManager
    {
        private static ThreadSafeLockedSingletonManager manager = null;
        public static volatile int ObjectCounter = 0;

        private static readonly object lockObject = new object();
        private ThreadSafeLockedSingletonManager()
        {
            ObjectCounter++;
        }

        public static ThreadSafeLockedSingletonManager GetInstance()
        {
            if (manager == null)
            {
                lock (lockObject)
                {
                    if (manager == null)
                    {
                        Thread.Sleep(1000);
                        manager = new ThreadSafeLockedSingletonManager();
                    }
                    return manager;
                }
            }
            else
            {
                return manager;
            }
        }
    }
}
