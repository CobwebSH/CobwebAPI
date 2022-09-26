using HarmonyLib;
using UnityEngine;

namespace CobwebAPI.API;

public class WaveModifiers
{
    public static Modifier Create(string Name, string Id, bool versus, bool survival, int MaxLevel,
        Sprite? Icon = default, string Description = "")
    {
        var modifierData = ScriptableObject.CreateInstance<ModifierData>();

        modifierData.title = Name;
        modifierData.key = Id;
        modifierData.maxLevel = MaxLevel;
        modifierData.icon = Icon;
        modifierData.description = Description;

        modifierData.survival = survival;
        modifierData.versus = versus;

        return new Modifier(modifierData);
    }

    public static bool Add(Modifier modifier)
    {
        if (!ModifierManagerGetNonMaxedSurvivalModsPatch.Mods.Contains(modifier))
        {
            ModifierManagerGetNonMaxedSurvivalModsPatch.Mods.Add(modifier);
            return true;
        }

        return false;
    }

    public static bool Remove(Modifier modifier)
    {
        if (ModifierManagerGetNonMaxedSurvivalModsPatch.Mods.Contains(modifier))
        {
            var mod = ModifierManagerGetNonMaxedSurvivalModsPatch.Mods.Where(m => m.data.key == modifier.data.key)
                .FirstOrDefault();
            return ModifierManagerGetNonMaxedSurvivalModsPatch.Mods.Remove(mod);
        }

        return false;
    }

    public static Modifier Get(string Id)
    {
        return ModifierManagerGetNonMaxedSurvivalModsPatch.Mods.Where(m => m != null && m.data.key == Id).First();
    }

    public static void Give(string id, int level, bool survival, bool versus)
    {
        var mod = Get(id);
        if (survival)
            mod.levelInSurvival = level;
        if (versus)
            mod.levelInVersus = level;
    }

    [HarmonyPatch(typeof(ModifierManager), "GetNonMaxedSurvivalMods")]
    internal class ModifierManagerGetNonMaxedSurvivalModsPatch
    {
        internal static List<Modifier> Mods { get; private set; } = new();

        internal static bool Prefix(ModifierManager __instance, ref List<Modifier> __result)
        {
            var templist = (from m in Traverse.Create(__instance).Field<List<Modifier>>("_modifiers").Value
                where m.levelInSurvival < m.data.maxLevel && m.data.survival
                select m).ToList();
            if (Mods.Count > 0)
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