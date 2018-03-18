using System;
using System.Reflection;
using Harmony;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Oculus.Newtonsoft.Json;

namespace SeamothStorageAccess
{
    public class Main
    {
        public static KeyCode UpgradeKey;

        public static void Patch()
        {
            LoadConfig();

            var harmony = HarmonyInstance.Create("com.ahk1221.seamothstorageaccess");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            var slotTypes = typeof(CraftData).GetField("slotTypes", BindingFlags.NonPublic | BindingFlags.Static)
                            .GetValue(null) as Dictionary<TechType, QuickSlotType>;

            slotTypes[TechType.VehicleStorageModule] = QuickSlotType.Instant;

            Console.WriteLine("[SeamothStorageAccess] Successfully patched.");
        }

        public static void LoadConfig()
        {
            var path = @"./QMods/SeamothStorageAccess/config.json";

            if (!File.Exists(path))
            {
                CreateConfig(path);
                Console.WriteLine("[SeamothStorageAccess] Config doesn't exist! Creating..");

                return;
            }

            var rawJson = File.ReadAllText(path);
            var config = JsonConvert.DeserializeObject<Config>(rawJson);

            var key = (KeyCode)Enum.Parse(typeof(KeyCode), config.UpgradeKey);
            UpgradeKey = key;

            Console.WriteLine("[SeamothStorageAccess] Successfully loaded config! Upgrade Key now: " + config.UpgradeKey);
        }

        public static void CreateConfig(string path)
        {
            var config = new Config() { UpgradeKey = "U" };
            var json = JsonConvert.SerializeObject(config);
            File.WriteAllText(path, json);

            UpgradeKey = KeyCode.U;
        }
    }
}
