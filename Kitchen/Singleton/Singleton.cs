namespace Kitchen.Singleton
{
    public abstract class Singleton<T> where T : class, new()
    {
        private static T _instance;

        public static T Instance()
        {
            if (_instance == null)
                _instance = new T();
            return _instance;
        }
    }
}
