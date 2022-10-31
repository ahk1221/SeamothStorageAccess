using System;
using System.Reflection;
using Harmony;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Oculus.Newtonsoft.Json;

namespace SeamothStorageAccess {
    public class Main {
        public static KeyCode UpgradeKey;
        public static KeyCode StorageKey;
        public static KeyCode TorpedoesKey;
        public static KeyCode LeftPrawnTorpedoes;
        public static KeyCode RighPrawnTorpedoes;

        public static void Patch() {
            LoadConfig();

            HarmonyInstance harmony = HarmonyInstance.Create("com.ahk1221.seamothstorageaccess");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            Dictionary<TechType, QuickSlotType> slotTypes = typeof(CraftData).GetField("slotTypes", BindingFlags.NonPublic | BindingFlags.Static)
                            .GetValue(null) as Dictionary<TechType, QuickSlotType>;

            slotTypes[TechType.VehicleStorageModule] = QuickSlotType.Instant;

            Console.WriteLine("[SeamothStorageAccess] Successfully patched.");
        }

        public static void LoadConfig() {
            string path = @"./QMods/SeamothStorageAccess/config.json";

            if (!File.Exists(path)) {
                Console.WriteLine("[SeamothStorageAccess] Config doesn't exist! Creating..");
                CreateConfig(path);

                return;
            }

            string rawJson = File.ReadAllText(path);
            Config config = JsonConvert.DeserializeObject<Config>(rawJson);

            UpgradeKey = (KeyCode)Enum.Parse(typeof(KeyCode), config.UpgradeKey);
            StorageKey = (KeyCode)Enum.Parse(typeof(KeyCode), config.StorageKey);
            TorpedoesKey = (KeyCode)Enum.Parse(typeof(KeyCode), config.TorpedoesKey);
            LeftPrawnTorpedoes = (KeyCode)Enum.Parse(typeof(KeyCode), config.LeftPrawnTorpedoes);
            RighPrawnTorpedoes = (KeyCode)Enum.Parse(typeof(KeyCode), config.RighPrawnTorpedoes);

            Console.WriteLine(String.Format("[SeamothStorageAccess] Successfully loaded config! Upgrade Key: {0}, Storage Key: {1}, Torpedoes Key: {2}, Left PRAWN Torpedoes: {3}, Right PRAWN Torpedoes: {4}",
                config.UpgradeKey, config.StorageKey, config.TorpedoesKey, config.LeftPrawnTorpedoes, config.RighPrawnTorpedoes));
        }

        public static void CreateConfig(string path) {
            Config config = new Config() { 
                UpgradeKey = "U", 
                StorageKey = "I", 
                TorpedoesKey = "T", 
                LeftPrawnTorpedoes = "Alpha1", 
                RighPrawnTorpedoes = "Alpha2" 
            };
            string json = JsonConvert.SerializeObject(config);
            File.WriteAllText(path, json);

            UpgradeKey = KeyCode.U;
            StorageKey = KeyCode.I;
            TorpedoesKey = KeyCode.T;
            LeftPrawnTorpedoes = KeyCode.Alpha1;
            RighPrawnTorpedoes = KeyCode.Alpha2;
        }
    }
}
