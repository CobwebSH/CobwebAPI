using System.Reflection;
using BepInEx;
using BepInEx.Logging;

namespace CobwebAPI.Utilities;

public static class Logger<T> where T : BaseUnityPlugin
{
    public static ManualLogSource Instance =>
        _instance ??= (ManualLogSource) PluginSingleton<T>.Instance
            .GetType()
            .GetProperty("Logger", BindingFlags.NonPublic | BindingFlags.Instance)!
            .GetValue(PluginSingleton<T>.Instance);

    /// <inheritdoc cref="ManualLogSource.LogDebug(object)" />
    public static void Debug(object data) => Instance.LogDebug(data);
    
    /// <inheritdoc cref="ManualLogSource.LogInfo(object)" />
    public static void Info(object data) => Instance.LogInfo(data);
    
    /// <inheritdoc cref="ManualLogSource.LogMessage(object)" />
    public static void Message(object data) => Instance.LogMessage(data);
    
    /// <inheritdoc cref="ManualLogSource.LogWarning(object)" />
    public static void Warning(object data) => Instance.LogWarning(data);
    
    /// <inheritdoc cref="ManualLogSource.LogError(object)" />
    public static void Error(object data) => Instance.LogError(data);
    
    /// <inheritdoc cref="ManualLogSource.LogFatal(object)" />
    public static void Fatal(object data) => Instance.LogFatal(data);
    
    // ReSharper disable once StaticMemberInGenericType
    private static ManualLogSource? _instance;
}