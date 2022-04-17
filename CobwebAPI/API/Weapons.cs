using System.Collections.Generic;
using System.Runtime.CompilerServices;
using HarmonyLib;

namespace CobwebAPI.API;

public class Weapons
{
    
    [HarmonyPatch(typeof(WeaponManager))]
    public class WeaponManagerPatch
    {
        public static Weapon EquippedWeapon { get; internal set; }
        [HarmonyPatch("EquipWeapon")]
        [HarmonyPostfix]
        internal static void EquipWeaponPostfix(Weapon weapon)
        {
            EquippedWeapon = weapon;
        }
        [HarmonyPatch("UnEquipWeapon")]
        [HarmonyPostfix]
        internal static void UnEquipWeaponPostfix()
        {
            EquippedWeapon = null;
        }
    }
    public static Weapon GetEquippedWeapon()
    {
        return WeaponManagerPatch.EquippedWeapon;
    }
    public static void SetEquippedWeapon(Weapon weapon)
    {
        WeaponManagerPatch.EquippedWeapon = weapon;
    }
    // Add weapon
    public static void AddWeapon(int ammo, List<Weapon.WeaponType> weaponType, string label)
    {
        // Create new weapon
        Weapon weapon = new Weapon();
        weapon.maxAmmo = ammo;
        weapon.ammo = ammo;
        weapon.label = label;
        weapon.type = weaponType;
    }
    
}