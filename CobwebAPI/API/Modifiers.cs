using HarmonyLib;
using UnityEngine;

namespace CobwebAPI.API;

public class WaveModifiers
{
    public static Modifier Create(string Name, string Id, int MaxLevel, Sprite? Icon = default, string Description = "")
    {
        var modifierData = ScriptableObject.CreateInstance<ModifierData>();

        modifierData.title = Name;
        modifierData.key = Id;
        modifierData.maxLevel = MaxLevel;
        modifierData.icon = Icon;
        modifierData.description = Description;

        modifierData.waves = true;
        modifierData.versus = false;

        return new Modifier(modifierData);
    }

    public static bool Add(Modifier modifier)
    {
        if (!ModifierManagerGetNonMaxedWavesModsPatch.Mods.Contains(modifier))
        {
            ModifierManagerGetNonMaxedWavesModsPatch.Mods.Add(modifier);
            return true;
        }
        return false;
    }

    public static bool Remove(Modifier modifier)
    {
        if (ModifierManagerGetNonMaxedWavesModsPatch.Mods.Contains(modifier))
        {
            var mod = ModifierManagerGetNonMaxedWavesModsPatch.Mods.Where(m => m.data.key == modifier.data.key).FirstOrDefault();
            return ModifierManagerGetNonMaxedWavesModsPatch.Mods.Remove(mod);
        }
        return false;
    }

        public static Modifier Get(string Id)
        {
            return ModifierManagerGetNonMaxedWavesModsPatch.Mods.Where(m => m != null && m.data.key == Id).First();
        }
        public static void Give(string id, int level)
        {
            var mod = Get(id);
            mod.levelInWaves = level;
        }

    [HarmonyPatch(typeof(ModifierManager), "GetNonMaxedWavesMods")]
    internal class ModifierManagerGetNonMaxedWavesModsPatch
    {
        internal static List<Modifier> Mods { get; private set; } = new();
        internal static bool Prefix(ModifierManager __instance, ref List<Modifier> __result)
        {
            var templist = (from m in Traverse.Create(__instance).Field<List<Modifier>>("_modifiers").Value
                where m.levelInWaves < m.data.maxLevel && m.data.waves
                select m).ToList();
            if(Mods.Count > 0)
            {
                foreach (Modifier mod in Mods)
                {
                    if (mod == null || templist.Contains(mod))
                        continue;

                    templist.Add(mod);
                }
            }

            __result = templist;
            return false;
        }

        internal static void Postfix(ref List<Modifier> __result)
        {
            Mods = __result;
        }
    }
}