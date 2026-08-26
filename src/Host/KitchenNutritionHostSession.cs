using System;
using System.Collections.Generic;
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Host session for KitchenNutritionSystem.
    /// Wraps the Core kitchen pipeline (StartPrepJob → TickDay → ServeMeals)
    /// and forwards StateChanged for host wiring. Engine-agnostic Core authority.
    /// </summary>
    public sealed class KitchenNutritionHostSession
    : HostSessionBase{
        public KitchenNutritionSystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;
        public KitchenNutritionHostSession(
            KitchenNutritionSystem system,
            Inventory inventory,
            NeedsSystem needs)
        {
            System = system
                ?? new KitchenNutritionSystem(new SeededRng(1986), inventory, needs, new GodotLog());

            System.OnJobCompleted += _ => RaiseStateChanged();
            System.OnMealServed += serving =>
            {
                LastEvent = $"Meal served: {serving.recipeId} to {serving.survivorId} (+{serving.moraleBonus} morale)";
                RaiseStateChanged();
            };
            System.OnKitchenChanged += () => RaiseStateChanged();
        }

        public ActionResult StartPrepJob(string recipeId, string assignedCookId, Dictionary<string, int> inputRequirements)
        {
            var res = System.StartPrepJob(recipeId, assignedCookId, inputRequirements);
            if (res.IsSuccess)
            {
                LastEvent = $"Prep started: {recipeId} by {assignedCookId}";
                RaiseStateChanged();
            }
            return res;
        }

        public ActionResult ServeMeal(string survivorId, string recipeId)
        {
            var res = System.ServeMeal(survivorId, recipeId);
            if (res.IsSuccess)
            {
                LastEvent = $"Meal served: {recipeId} to {survivorId}";
                RaiseStateChanged();
            }
            return res;
        }

        public void TickDay(int day)
        {
            System.TickDay(day);
            RaiseStateChanged();
        }
    }

    [Serializable]
    public sealed class KitchenNutritionHostSave
    {
        public string SchemaVersion { get; set; } = "1.0";
        public KitchenNutritionState State { get; set; }
        public string Checksum { get; set; } = string.Empty;
    }

    public static class KitchenNutritionSaveStore
    {
        public const string FileName = "kitchen_nutrition_save.json";
        private static readonly FileSystemIO s_files = new FileSystemIO();
        private static readonly SystemTextJsonSerializer s_json = new SystemTextJsonSerializer();

        public static string SavePath =>
            Path.Combine(ProjectSettings.GlobalizePath("user://"), FileName);
        public static bool Exists => s_files.FileExists(SavePath);

        public static bool TrySave(KitchenNutritionState state)
        {
            try
            {
                if (state == null) return false;
                var envelope = new KitchenNutritionHostSave { State = state };
                envelope.Checksum = SaveChecksum.Compute(envelope);
                string path = SavePath;
                string? dir = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                File.WriteAllText(path, s_json.Serialize(envelope));
                return true;
            }
            catch (Exception e)
            {
                GD.PrintErr("[Kitchen] save failed: " + e.Message);
                return false;
            }
        }

        public static KitchenNutritionState? TryLoad()
        {
            try
            {
                string path = SavePath;
                if (!s_files.FileExists(path)) return null;
                string raw = s_files.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(raw)) return null;
                var envelope = s_json.Deserialize<KitchenNutritionHostSave>(raw);
                if (envelope != null && envelope.State != null)
                {
                    if (string.IsNullOrEmpty(envelope.Checksum)) return null;
                    return envelope.State;
                }
                return s_json.Deserialize<KitchenNutritionState>(raw);
            }
            catch (Exception e)
            {
                GD.PrintErr("[Kitchen] load failed: " + e.Message);
                return null;
            }
        }
    }
}
