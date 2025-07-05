using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace IdDumper
{
    [BepInPlugin("jas.Dinkum.IdDumper", "ID Dumper", "1.0.0")]
    public class IdDumper : BaseUnityPlugin
    {
        public static new ManualLogSource Logger;
        internal static Harmony harmony = new Harmony("jas.Dinkum.IdDumper");

        public void Awake()
        {
            Logger = base.Logger;
            Logger.LogInfo("Mod jas.Dinkum.IdDumper loaded!");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }

    [HarmonyPatch(typeof(Inventory))]
    public class InventoryPatch
    {
        [HarmonyPatch(typeof(Inventory), "Awake")]
        public static void Postfix(Inventory __instance)
        {
            if (__instance == null)
            {
                IdDumper.Logger.LogWarning("Inventory.Instance is null");
                return;
            }

            #region Items
            List<string> csvBuilder = new List<string> { "ID,Name,Tag" };
            foreach (InventoryItem item in __instance.allItems)
            {
                csvBuilder.Add(string.Join(",", item.getItemId().ToString(), item.getInvItemName(), item.tag));
            }

            string outputFilePath = Path.Combine(Paths.BepInExRootPath, "DumpedItemIds.csv");
            if (File.Exists(outputFilePath)) File.Delete(outputFilePath);
            File.WriteAllText(outputFilePath, string.Join("\r\n", csvBuilder));

            IdDumper.Logger.LogMessage("Item IDs successfully dumped to: " + outputFilePath);
            #endregion
        }
    }

    [HarmonyPatch(typeof(AnimalManager))]
    public class AnimalManagerPatch
    {
        [HarmonyPatch(typeof(AnimalManager), "Awake")]
        public static void Postfix(AnimalManager __instance)
        {
            if (__instance == null)
            {
                IdDumper.Logger.LogWarning("AnimalManager.manage is null");
                return;
            }

            #region Animals
            List<string> csvBuilder = new List<string> { "ID,Name" };
            foreach (AnimalAI animal in __instance.allAnimals)
            {
                csvBuilder.Add(string.Join(",", animal.animalId.ToString(), animal.GetAnimalName()));
            }

            string outputFilePath = Path.Combine(Paths.BepInExRootPath, "DumpedAnimalIds.csv");
            if (File.Exists(outputFilePath)) File.Delete(outputFilePath);
            File.WriteAllText(outputFilePath, string.Join("\r\n", csvBuilder));

            IdDumper.Logger.LogMessage("Animal IDs successfully dumped to: " + outputFilePath);
            #endregion
        }
    }

    [HarmonyPatch(typeof(WorldManager))]
    public class WorldManagerPatch
    {
        [HarmonyPatch(typeof(WorldManager), "Awake")]
        public static void Postfix(WorldManager __instance)
        {
            if (__instance == null)
            {
                IdDumper.Logger.LogWarning("WorldManager.Instance is null");
                return;
            }

            #region TileObjects
            List<string> csvBuilder = new List<string> { "ID,Name" };
            foreach (TileObject tileObject in __instance.allObjects)
            {
                csvBuilder.Add(string.Join(",", tileObject.name.Split(' ')));
            }

            string outputFilePath = Path.Combine(Paths.BepInExRootPath, "DumpedTileObjectIds.csv");
            if (File.Exists(outputFilePath)) File.Delete(outputFilePath);
            File.WriteAllText(outputFilePath, string.Join("\r\n", csvBuilder));

            IdDumper.Logger.LogMessage("TileObject IDs successfully dumped to: " + outputFilePath);
            #endregion
        }
    }

    [HarmonyPatch(typeof(SaveLoad))]
    public class SaveLoadPatch
    {
        [HarmonyPatch(typeof(SaveLoad), "Awake")]
        public static void Postfix(SaveLoad __instance)
        {
            if (__instance == null)
            {
                IdDumper.Logger.LogWarning("SaveLoad.saveOrLoad is null");
            }

            #region Carriables
            List<string> csvBuilder = new List<string> { "ID,Name" };
            foreach (GameObject carryable in __instance.carryablePrefabs)
            {
                csvBuilder.Add(string.Join(",", carryable.name.Split(' ')));
                // csvBuilder.Add(string.Join(",", pickUpAndCarry.prefabId.ToString(), pickUpAndCarry.GetName(), pickUpAndCarry.canBePickedUp.ToString(), isFragile.GetValue(pickUpAndCarry).ToString(), pickUpAndCarry.investigationItem.ToString(), pickUpAndCarry.photoRequestable.ToString()));
            }

            string outputFilePath = Path.Combine(Paths.BepInExRootPath, "DumpedCarriableIds.csv");
            if (File.Exists(outputFilePath)) File.Delete(outputFilePath);
            File.WriteAllText(outputFilePath, string.Join("\r\n", csvBuilder));

            IdDumper.Logger.LogMessage("Carriable IDs successfully dumped to: " + outputFilePath);
            #endregion

            #region Vehicles
            csvBuilder = new List<string> { "ID,Name" };
            foreach (GameObject vehicle in __instance.vehiclePrefabs)
            {
                csvBuilder.Add(string.Join(",", vehicle.name.Split(' ')));
            }

            outputFilePath = Path.Combine(Paths.BepInExRootPath, "DumpedVehicleIds.csv");
            if (File.Exists(outputFilePath)) File.Delete(outputFilePath);
            File.WriteAllText(outputFilePath, string.Join("\r\n", csvBuilder));

            IdDumper.Logger.LogMessage("Vehicle IDs successfully dumped to: " + outputFilePath);
            #endregion
        }
    }
}
