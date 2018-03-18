using System;
using Harmony;
using UnityEngine;

namespace SeamothStorageAccess.Patches
{
    [HarmonyPatch(typeof(SeaMoth))]
    [HarmonyPatch("OnUpgradeModuleUse")]
    public class Seamoth_OnUpgradeModuleUse_Patch
    {
        static bool Prefix(Vehicle __instance, TechType techType, int slotID)
        {
            if (techType != TechType.VehicleStorageModule) return true;

            var storageContainer = __instance.GetStorageInSlot(slotID, techType);
            if(storageContainer != null)
            {
                var pda = Player.main.GetPDA();
                Inventory.main.SetUsedStorage(storageContainer, false);
                pda.Open(PDATab.Inventory);

                return false;
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(SeaMoth))]
    [HarmonyPatch("Update")]
    public class Seamoth_Update_Patch
    {
        static void Prefix(SeaMoth __instance)
        {
            var updateContainer = __instance.upgradesInput;
            if (Input.GetKeyDown(Main.UpgradeKey))
                updateContainer.OpenFromExternal();
        }
    }
}
