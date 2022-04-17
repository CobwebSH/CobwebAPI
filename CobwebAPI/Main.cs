using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace CobwebAPI
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    public class Main : BaseUnityPlugin
    {
        public const string ModName = "CobwebAPI";
        public const string ModAuthor  = "CobwebSH";
        public const string ModGUID = "com.cobwebsh.cobwebapi";
        public const string ModVersion = "1.0.0";
        internal static ManualLogSource Log;
        internal Harmony Harmony;
        internal void Awake()
        {
            Harmony = new Harmony(ModGUID);
            Log = Logger;

            Harmony.PatchAll();
            Logger.LogInfo($"{ModName} successfully loaded! Made by {ModAuthor}");
        }
    }
}
