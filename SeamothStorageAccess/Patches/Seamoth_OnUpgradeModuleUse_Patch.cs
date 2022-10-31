using System;
using Harmony;
using UnityEngine;

namespace SeamothStorageAccess.Patches {
    [HarmonyPatch(typeof(SeaMoth))]
    [HarmonyPatch("OnUpgradeModuleUse")]
    public class Seamoth_OnUpgradeModuleUse_Patch {
        static bool Prefix(Vehicle __instance, TechType techType, int slotID) {
            if (techType != TechType.VehicleStorageModule) return true;

            ItemsContainer storageContainer = __instance.GetStorageInSlot(slotID, techType);
            if (storageContainer != null) {
                PDA pda = Player.main.GetPDA();
                Inventory.main.SetUsedStorage(storageContainer, false);
                pda.Open(PDATab.Inventory);

                return false;
            }
            
            return true;
        }
    }

    [HarmonyPatch(typeof(SeaMoth))]
    [HarmonyPatch("Update")]
    public class Seamoth_Update_Patch {
        static void Prefix(SeaMoth __instance) {
            if (!Player.main.GetPDA().isOpen && Player.main.GetVehicle() == __instance) {
                // Seamoth updates handling
                if (Input.GetKeyDown(Main.UpgradeKey)) {
                    __instance.upgradesInput.OpenFromExternal();
                }
                // Seamoth torpedoes storage handling
                if (Input.GetKeyDown(Main.TorpedoesKey) && __instance.GetActiveSlotID() > -1) {
                    ItemsContainer torpedoesContainer = __instance.GetStorageInSlot(__instance.GetActiveSlotID(), TechType.SeamothTorpedoModule);
                    if (torpedoesContainer != null) {
                        PDA pda = Player.main.GetPDA();
                        Inventory.main.SetUsedStorage(torpedoesContainer, false);
                        pda.Open(PDATab.Inventory);
                    }
                }
            }
        }
    }

    [HarmonyPatch(typeof(Exosuit))]
    [HarmonyPatch("Update")]
    public class Exosuit_Update_Patch {
        static void Prefix(Exosuit __instance) {
            if (!Player.main.GetPDA().isOpen && Player.main.GetVehicle() == __instance) {
                // Prawn updates handling
                if (Input.GetKeyDown(Main.UpgradeKey)) {
                    __instance.upgradesInput.OpenFromExternal();
                }
                // Prawn storage handling
                if (Input.GetKeyDown(Main.StorageKey)) {
                    __instance.storageContainer.Open();
                }
                // Prawn left arm torpedoes storage handling
                if (Input.GetKeyDown(Main.LeftPrawnTorpedoes) && __instance.leftArmType == TechType.ExosuitTorpedoArmModule) {
                    ItemsContainer leftArmTorpedoesInventory = __instance.GetStorageInSlot(__instance.GetSlotIndex("ExosuitArmLeft"), TechType.ExosuitTorpedoArmModule);
                    if (leftArmTorpedoesInventory != null) {
                        PDA pda = Player.main.GetPDA();
                        Inventory.main.SetUsedStorage(leftArmTorpedoesInventory, false);
                        pda.Open(PDATab.Inventory);
                    }
                }
                // Prawn right arm torpedoes storage handling
                if (Input.GetKeyDown(Main.RighPrawnTorpedoes) && __instance.leftArmType == TechType.ExosuitTorpedoArmModule) {
                    ItemsContainer rightArmTorpedoesInventory = __instance.GetStorageInSlot(__instance.GetSlotIndex("ExosuitArmRight"), TechType.ExosuitTorpedoArmModule);
                    if (rightArmTorpedoesInventory != null) {
                        PDA pda = Player.main.GetPDA();
                        Inventory.main.SetUsedStorage(rightArmTorpedoesInventory, false);
                        pda.Open(PDATab.Inventory);
                    }
                }
            }
        }
    }
}
