using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace CobwebAPI;

[BepInAutoPlugin("com.cobwebsh.cobwebapi", "CobwebAPI")]
public partial class Main : BaseUnityPlugin
{
    public const string Author  = "CobwebSH";

    internal static ManualLogSource Log = null!;
    
    internal Harmony Harmony { get; } = new(Id);
    
    internal void Awake()
    {
        Log = Logger;

        Harmony.PatchAll();
        Logger.LogInfo($"{Name} successfully loaded! Made by {Author}");
    }
}