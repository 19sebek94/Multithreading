namespace ThreadSafeSingleton
{
    /// <summary>
    /// From C# 4.0 there is available Lazy keyword thanks to which we can create lazy objects. 
    /// Lazy object is created only when there such need. It is thread safe. 
    /// In compare to locked Singlton example it is simpler and safier. 
    /// In compare to eager loading where the object is created at the beginning of the application,
    /// lazy loading is more efficient.
    /// </summary>
    public sealed class LazySingleton
    {
        private static readonly Lazy<LazySingleton> instance =
       new Lazy<LazySingleton>(() => new LazySingleton());

        public static int ObjectCounter { get; private set; } = 0;

        private LazySingleton()
        {
            ObjectCounter++;
        }

        public static LazySingleton GetInstance()
        {
            Thread.Sleep(1000);
            return instance.Value;
        }
    }
}
