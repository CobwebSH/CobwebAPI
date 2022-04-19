using BepInEx;
using HarmonyLib;

namespace CobwebAPI.Bootstrap;

public static class ChainloaderHooks
{
    public static List<BaseUnityPlugin> Plugins { get; } = new();
    
    public delegate void PluginLoadHandler(BaseUnityPlugin plugin);

    public static event PluginLoadHandler? OnPluginLoad;

    internal static void InvokePluginLoad(BaseUnityPlugin plugin)
    {
        if (!Plugins.Contains(plugin))
        {
            Plugins.Add(plugin);
        }
        
        OnPluginLoad?.Invoke(plugin);
    }

    [HarmonyPatch(typeof(BaseUnityPlugin), MethodType.Constructor)]
    public static class LoadPluginPatch
    {
        public static void Postfix(BaseUnityPlugin __instance)
        {
            InvokePluginLoad(__instance);
        }
    }
}