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
    
    private List<VersusWeapon> getVersusWeapons()
    {
        bool flag = GameSettings.Instance == null;
        List<VersusWeapon> result;
        if (flag)
        {
            result = null;
        }
        else
        {
            List<VersusWeapon> list = GameSettings.Instance.AvailableVersusWeapons();
            this.runOnce = false;
            result = list;
        }
        return result;
    }
    
    private void spawnWeapon(int selectedWeapon)
    {
        GameObject gameObject = GameObject.FindGameObjectWithTag("PlayerRigidbody");
        Vector2 a = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        bool flag = !Physics2D.Raycast(a + new Vector2(0f, 10f), gameObject.transform.up, 0.1f, GameController.instance.worldLayers);
        bool flag2 = flag;
        if (flag2)
        {
            UnityEngine.Object.Instantiate<GameObject>(this.getVersusWeapons()[selectedWeapon].weapon, gameObject.transform.position, gameObject.transform.rotation);
        }
    }
    
}
