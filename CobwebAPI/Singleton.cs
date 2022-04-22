namespace CobwebAPI;

public abstract class Singleton<T> where T : class
{
    public static T Instance => _instance ??= Activator.CreateInstance<T>();
    
    private static T? _instance;
}