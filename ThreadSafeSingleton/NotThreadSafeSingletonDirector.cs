namespace ThreadSafeSingleton
{
    /// <summary>
    /// SingletonDirector class is missing lock while checking if
    /// director is null. Therefore it is possible to other thread to
    /// pass this condition in the same time.
    /// </summary>
    public sealed class NotThreadSafeSingletonDirector
    {
        private static NotThreadSafeSingletonDirector director = null;
        public static int ObjectCounter = 0;

        private NotThreadSafeSingletonDirector()
        {
            ObjectCounter++;
        }

        public static NotThreadSafeSingletonDirector GetInstance()
        {
            if (director == null)
            {
                Thread.Sleep(1000);
                director = new NotThreadSafeSingletonDirector();
            }
            return director;
        }
    }
}
