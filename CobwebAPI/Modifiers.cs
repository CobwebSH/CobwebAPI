using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace CobwebAPI
{
    public class Modifiers
    {
        public static void Add(int maxLevel, string name, string id, bool wavesModifer, bool versusModifier, Sprite icon = default)
        {
            var modifierData = ScriptableObject.CreateInstance<ModifierData>();

            modifierData.maxLevel = maxLevel;
            modifierData.icon = icon;
            modifierData.title = name;
            modifierData.versus = versusModifier;
            modifierData.waves = wavesModifer;
            modifierData.key = id;
            var modifier = new Modifier(modifierData);

            ModifierManagerGetNonMaxedWaveModsPatch.result.Add(modifier);
        }

        public static Modifier Get(string id)
        {
            return ModifierManagerGetNonMaxedWaveModsPatch.result.Where(m => m != null && m.data.key == id).First();
        }
    }

    [HarmonyPatch(typeof(ModifierManager), "GetNonMaxedWaveMods")]
    public static class ModifierManagerGetNonMaxedWaveModsPatch
    {
        public static List<Modifier> result = new();
        public static bool Prefix(ModifierManager __instance, ref List<Modifier> __result)
        {
            var templist = (from m in Traverse.Create(__instance).Field<List<Modifier>>("_modifiers").Value
                        where m.levelInWaves < m.data.maxLevel && m.data.waves
                        select m).ToList();

            if (result.Count > 0)
            {
                foreach(Modifier mod in result)
                {
                    if (mod != null && templist.Contains(mod))
                        continue;
                    templist.Add(mod);
                }
            }

            __result = templist;
            return false;
        }
        public static void Postfix(ref List<Modifier> __result)
        {
            result = __result;
        }
    }
}