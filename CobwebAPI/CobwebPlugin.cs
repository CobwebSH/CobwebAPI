using BepInEx;
using CobwebAPI.Bootstrap;
using CobwebAPI.Utilities;
using HarmonyLib;

namespace CobwebAPI;

[BepInAutoPlugin("com.cobwebsh.cobwebapi", "CobwebAPI")]
public partial class CobwebPlugin : BaseUnityPlugin
{
    public const string Author  = "CobwebSH";

    internal Harmony Harmony { get; } = new(Id);

    public CobwebPlugin()
    {
        PluginSingleton<BaseUnityPlugin>.Initialize();
        
        ChainloaderHooks.InvokePluginLoad(this);
    }
    
    private void Awake()
    {
        Harmony.PatchAll();
        
        Logger<CobwebPlugin>.Info($"{Name} successfully loaded! Made by {Author}");
    }
}