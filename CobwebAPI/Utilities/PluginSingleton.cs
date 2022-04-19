using System.Reflection;
using BepInEx;
using CobwebAPI.Bootstrap;

namespace CobwebAPI.Utilities;

public static class PluginSingleton<T> where T : BaseUnityPlugin
{
    public static T Instance => _instance ??= ChainloaderHooks.Plugins.OfType<T>().Single();
    
    private static T? _instance;

    internal static void Initialize()
    {
        ChainloaderHooks.OnPluginLoad += plugin =>
        {
            typeof(PluginSingleton<>).MakeGenericType(plugin.GetType())
                .GetField(nameof(_instance), BindingFlags.NonPublic | BindingFlags.Static)!
                .SetValue(null, plugin);
        };
    }
}