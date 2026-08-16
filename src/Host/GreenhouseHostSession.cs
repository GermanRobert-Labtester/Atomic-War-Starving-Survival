using System;
using System.IO;
using System.Text.Json;
using Godot;
using Ashfall.Core;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Thin Godot host session for GreenhouseSystem (The Glass Orchard / Expansion 05).
    /// Connects the pure C# cultivation engine to host inventory, shelter power, and save persistence.
    /// </summary>
    public sealed class GreenhouseHostSession
    {
        private const int DefaultPlanterBoxCount = 4;
        private const int DefaultSeed = 1986;

        public GreenhouseSystem System { get; }
        public InventoryHostSession? InventoryHost { get; set; }
        public string LastEvent { get; private set; } = string.Empty;

        public event Action? StateChanged;

        public GreenhouseHostSession(GreenhouseSystem system, InventoryHostSession? inventoryHost = null)
        {
            System = system ?? new GreenhouseSystem(DefaultSeed);
            InventoryHost = inventoryHost;

            System.EnsurePlots(DefaultPlanterBoxCount);

            System.OnCropPlanted += (plot, seed, day) =>
            {
                LastEvent = $"Plot {plot + 1}: Seed {seed} planted on Day {day}.";
                StateChanged?.Invoke();
            };
            System.OnCropMatured += (plot, seed) =>
            {
                LastEvent = $"Plot {plot + 1}: Crop {seed} is fully mature and ready for harvest.";
                StateChanged?.Invoke();
            };
            System.OnCropHarvested += harvest =>
            {
                LastEvent = harvest.success
                    ? $"Plot {harvest.plotIndex + 1}: Harvested {harvest.amount}x {harvest.yieldItemId} (Contaminated: {harvest.contaminated})."
                    : $"Plot {harvest.plotIndex + 1}: Harvest failed.";
                StateChanged?.Invoke();
            };
            System.OnBlightOutbreak += plot =>
            {
                LastEvent = $"Plot {plot + 1}: WARNING — Blight outbreak detected in soil bed!";
                StateChanged?.Invoke();
            };
            System.OnPlotDriedOut += plot =>
            {
                LastEvent = $"Plot {plot + 1}: Soil moisture depleted! Risk of crop failure.";
                StateChanged?.Invoke();
            };
            System.OnCropFailed += plot =>
            {
                LastEvent = $"Plot {plot + 1}: Crop has failed due to drought or severe blight.";
                StateChanged?.Invoke();
            };
        }

        public static GreenhouseHostSession Create(InventoryHostSession? inventoryHost = null)
        {
            var session = new GreenhouseHostSession(new GreenhouseSystem(DefaultSeed), inventoryHost);
            var save = GreenhouseSaveStore.TryLoad();
            if (save != null)
                session.System.RestoreState(save);
            else
                session.System.EnsurePlots(DefaultPlanterBoxCount);

            return session;
        }

        public bool Plant(int plotIndex, string seedItemId, int currentDay)
        {
            if (InventoryHost != null && InventoryHost.Inventory.CountById(seedItemId) < 1)
            {
                LastEvent = $"Cannot plant: insufficient {seedItemId} in inventory.";
                return false;
            }

            if (System.Plant(plotIndex, seedItemId, currentDay, out var consumed))
            {
                InventoryHost?.Remove(consumed, 1);
                StateChanged?.Invoke();
                return true;
            }

            return false;
        }

        public bool Water(int plotIndex, float waterUnits, bool tainted)
        {
            string waterItemId = tainted ? "irradiated_water" : "clean_water";
            int requiredUnits = (int)Math.Ceiling(waterUnits / 10f);

            if (InventoryHost != null && InventoryHost.Inventory.CountById(waterItemId) < requiredUnits)
            {
                LastEvent = $"Cannot water: insufficient {waterItemId} (needs {requiredUnits}).";
                return false;
            }

            InventoryHost?.Remove(waterItemId, requiredUnits);
            System.Water(plotIndex, waterUnits, tainted);
            LastEvent = $"Plot {plotIndex + 1}: Irrigated with {waterUnits:0} units of {(tainted ? "irradiated" : "clean")} water.";
            StateChanged?.Invoke();
            return true;
        }

        public bool TreatBlight(int plotIndex)
        {
            string treatmentId = GreenhouseExpansionCatalog.Items.BlightTreatment;
            if (InventoryHost != null && InventoryHost.Inventory.CountById(treatmentId) < 1)
            {
                // Fallback to chemical wash / iodine if available
                if (InventoryHost.Inventory.CountById("iodine_pills") >= 1)
                {
                    InventoryHost.Remove("iodine_pills", 1);
                    var p = plotIndex >= 0 && plotIndex < System.Plots.Count ? System.Plots[plotIndex] : null;
                    if (p != null) p.blight = Math.Max(0f, p.blight - 0.5f);
                    LastEvent = $"Plot {plotIndex + 1}: Applied iodine chemical wash. Blight reduced.";
                    StateChanged?.Invoke();
                    return true;
                }

                LastEvent = "Cannot treat blight: no blight treatment or iodine pills available.";
                return false;
            }

            if (System.TreatBlight(plotIndex, out var consumed))
            {
                InventoryHost?.Remove(consumed, 1);
                LastEvent = $"Plot {plotIndex + 1}: Blight eradicated using {consumed}.";
                StateChanged?.Invoke();
                return true;
            }

            return false;
        }

        public bool Harvest(int plotIndex)
        {
            var harvest = System.Harvest(plotIndex);
            if (harvest.success && !string.IsNullOrEmpty(harvest.yieldItemId))
            {
                InventoryHost?.Add(harvest.yieldItemId, harvest.amount);
                StateChanged?.Invoke();
                return true;
            }

            return false;
        }

        public bool Clear(int plotIndex)
        {
            if (System.Clear(plotIndex))
            {
                LastEvent = $"Plot {plotIndex + 1}: Cleared soil bed to fallow.";
                StateChanged?.Invoke();
                return true;
            }
            return false;
        }

        public void TickDay(int currentDay, float growLightHours = 6f, float ashContaminationRate = 0.05f)
        {
            System.TickDay(currentDay, growLightHours, ashContaminationRate);
            StateChanged?.Invoke();
        }

        public GreenhouseState CaptureSave() => System.CaptureState();
    }

    /// <summary>
    /// Persistent on-disk JSON store for GreenhouseState at user://greenhouse_save.json.
    /// </summary>
    public static class GreenhouseSaveStore
    {
        private static readonly JsonSerializerOptions JsonOpts = new JsonSerializerOptions
        {
            WriteIndented = true,
            IncludeFields = true
        };

        public static string SavePath =>
            Path.Combine(ProjectSettings.GlobalizePath("user://"), "greenhouse_save.json");

        public static bool Exists => File.Exists(SavePath);

        public static bool TrySave(GreenhouseState state)
        {
            if (state == null) return false;
            try
            {
                string dir = Path.GetDirectoryName(SavePath)!;
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                string json = JsonSerializer.Serialize(state, JsonOpts);
                File.WriteAllText(SavePath, json);
                return true;
            }
            catch (Exception ex)
            {
                GD.PushError($"[GreenhouseSaveStore] Save failed: {ex.Message}");
                return false;
            }
        }

        public static GreenhouseState? TryLoad()
        {
            if (!Exists) return null;
            try
            {
                string json = File.ReadAllText(SavePath);
                return JsonSerializer.Deserialize<GreenhouseState>(json, JsonOpts);
            }
            catch (Exception ex)
            {
                GD.PushWarning($"[GreenhouseSaveStore] Load failed: {ex.Message}");
                return null;
            }
        }
    }
}
