namespace ThreadSafeSingleton
{
    /// <summary>
    /// Here manager class has implemented lock while checking if manager is null.
    /// Therefore this class is thread safe.
    /// There is additionaly implemented double check locking. It is kind of improvement of performance
    /// related to avoid unnecessary locks. However, it is also risky, because in multithreaded environment
    /// there is a chance that object will not be properly initialized by one thread, which will lead to 
    /// unpredictible behavior of another thread. This is why it is often considered as anti-pattern.
    /// The solution is to keep it simple with single check in lock or changing MemoryBarrier which will be
    /// shown in VolatileCases project
    /// </summary>
    public class LockedSingleton
    {
        private static LockedSingleton instance = null;
        public static volatile int ObjectCounter = 0;

        private static readonly object lockObject = new object();
        private LockedSingleton()
        {
            ObjectCounter++;
        }

        public static LockedSingleton GetInstance()
        {
            if (instance == null)
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        Thread.Sleep(1000);
                        instance = new LockedSingleton();
                    }
                    return instance;
                }
            }
            else
            {
                return instance;
            }
        }
    }
}
