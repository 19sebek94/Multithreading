namespace ThreadSafeSingleton
{
    /// <summary>
    /// SingletonDirector class is missing lock while checking if
    /// director is null. Therefore it is possible to other thread to
    /// pass this condition in the same time.
    /// </summary>
    public sealed class NotThreadSafeSingleton
    {
        private static NotThreadSafeSingleton instance = null;
        public static int ObjectCounter = 0;

        private NotThreadSafeSingleton()
        {
            ObjectCounter++;
        }

        public static NotThreadSafeSingleton GetInstance()
        {
            if (instance == null)
            {
                Thread.Sleep(1000);
                instance = new NotThreadSafeSingleton();
            }
            return instance;
        }
    }
}
