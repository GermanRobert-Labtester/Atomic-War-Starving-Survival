using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Greenhouse;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Thin Godot host session for GreenhouseSystem (The Glass Orchard / Expansion 05).
    /// Connects the pure C# cultivation engine to host inventory, shelter power, and save persistence.
    /// </summary>
    public sealed class GreenhouseHostSession
    : HostSessionBase{
        private const int DefaultPlanterBoxCount = 4;
        private const int DefaultSeed = 1986;

        public GreenhouseSystem System { get; }
        public ApicultureSystem Apiculture { get; }
        public InventoryHostSession? InventoryHost { get; set; }
        public string LastEvent { get; private set; } = string.Empty;
        public GreenhouseHostSession(GreenhouseSystem system, InventoryHostSession? inventoryHost = null, ApicultureSystem? apiculture = null)
        {
            System = system ?? new GreenhouseSystem(DefaultSeed);
            Apiculture = apiculture ?? new ApicultureSystem();
            InventoryHost = inventoryHost;

            System.EnsurePlots(DefaultPlanterBoxCount);

            System.OnCropPlanted += (plot, seed, day) =>
            {
                LastEvent = $"Plot {plot + 1}: Seed {seed} planted on Day {day}.";
                RaiseStateChanged();
            };
            System.OnCropMatured += (plot, seed) =>
            {
                LastEvent = $"Plot {plot + 1}: Crop {seed} is fully mature and ready for harvest.";
                RaiseStateChanged();
            };
            System.OnCropHarvested += harvest =>
            {
                LastEvent = harvest.success
                    ? $"Plot {harvest.plotIndex + 1}: Harvested {harvest.amount}x {harvest.yieldItemId} (Contaminated: {harvest.contaminated})."
                    : $"Plot {harvest.plotIndex + 1}: Harvest failed.";
                RaiseStateChanged();
            };
            System.OnBlightOutbreak += plot =>
            {
                LastEvent = $"Plot {plot + 1}: WARNING — Blight outbreak detected in soil bed!";
                RaiseStateChanged();
            };
            System.OnPlotDriedOut += plot =>
            {
                LastEvent = $"Plot {plot + 1}: Soil moisture depleted! Risk of crop failure.";
                RaiseStateChanged();
            };
            System.OnCropFailed += plot =>
            {
                LastEvent = $"Plot {plot + 1}: Crop has failed due to drought or severe blight.";
                RaiseStateChanged();
            };

            // Apiculture events
            Apiculture.OnStateChanged += _ => RaiseStateChanged();
            Apiculture.OnHiveInstalled += id => { LastEvent = $"Hive {id} installed."; RaiseStateChanged(); };
            Apiculture.OnColonyDied += id => { LastEvent = $"Hive {id}: Colony has died!"; RaiseStateChanged(); };
            Apiculture.OnColonySwarming += id => { LastEvent = $"Hive {id}: Colony is swarming!"; RaiseStateChanged(); };
        }

        public static GreenhouseHostSession Create(InventoryHostSession? inventoryHost = null)
        {
            var session = new GreenhouseHostSession(new GreenhouseSystem(DefaultSeed), inventoryHost);
            var save = GreenhouseSaveStore.TryLoad();
            if (save != null)
            {
                session.System.RestoreState(save);
                if (save.apiculture != null)
                    session.Apiculture.RestoreState(save.apiculture);
            }
            else
            {
                session.System.EnsurePlots(DefaultPlanterBoxCount);
                // Install default apiary hive
                session.Apiculture.InstallHive("hive_01", "bay_orchard", 1);
                session.Apiculture.LinkPlots("hive_01", new List<string> { "plot_0", "plot_1", "plot_2", "plot_3" });
            }

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
                RaiseStateChanged();
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
            RaiseStateChanged();
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
                    RaiseStateChanged();
                    return true;
                }

                LastEvent = "Cannot treat blight: no blight treatment or iodine pills available.";
                return false;
            }

            if (System.TreatBlight(plotIndex, out var consumed))
            {
                InventoryHost?.Remove(consumed, 1);
                LastEvent = $"Plot {plotIndex + 1}: Blight eradicated using {consumed}.";
                RaiseStateChanged();
                return true;
            }

            return false;
        }

        public bool Harvest(int plotIndex)
        {
            var harvest = System.Harvest(plotIndex);
            if (harvest.success && !string.IsNullOrEmpty(harvest.yieldItemId))
            {
                float bonus = Apiculture.GetPollinationBonus($"plot_{plotIndex}");
                float multiplier = 1f + bonus;
                int totalAmount = (int)Math.Max(1, Math.Round(harvest.amount * multiplier));
                InventoryHost?.Add(harvest.yieldItemId, totalAmount);
                LastEvent = $"Plot {plotIndex + 1}: Harvested {totalAmount}x {harvest.yieldItemId} (Pollination: +{bonus:P0}).";
                RaiseStateChanged();
                return true;
            }

            return false;
        }

        public bool Clear(int plotIndex)
        {
            if (System.Clear(plotIndex))
            {
                LastEvent = $"Plot {plotIndex + 1}: Cleared soil bed to fallow.";
                RaiseStateChanged();
                return true;
            }
            return false;
        }

        // ── Apiculture Actions ──────────────────────────────────────────

        public bool InstallHive(string hiveId, string bayId, int currentDay)
        {
            bool ok = Apiculture.InstallHive(hiveId, bayId, currentDay);
            if (ok)
            {
                var linked = new List<string>();
                for (int i = 0; i < System.PlotCount; i++) linked.Add($"plot_{i}");
                Apiculture.LinkPlots(hiveId, linked);
                LastEvent = $"Apiary: Installed new beehive '{hiveId}'.";
                RaiseStateChanged();
            }
            return ok;
        }

        public bool InspectHive(string hiveId, int currentDay)
        {
            var state = Apiculture.InspectHive(hiveId, currentDay);
            if (state != null)
            {
                LastEvent = $"Apiary: Inspected hive '{hiveId}' (Pop: {state.colonyPopulation:P0}, Queen: {state.queenVitality:P0}).";
                RaiseStateChanged();
                return true;
            }
            return false;
        }

        public bool FeedHive(string hiveId, float amount = 0.5f)
        {
            if (InventoryHost != null && InventoryHost.Inventory.CountById("clean_water") >= 1)
            {
                InventoryHost.Remove("clean_water", 1);
                Apiculture.RefillFeed(hiveId, amount);
                Apiculture.RefillWater(hiveId, amount);
                LastEvent = $"Apiary: Fed and watered hive '{hiveId}'.";
                RaiseStateChanged();
                return true;
            }
            LastEvent = "Apiary: Insufficient clean water in inventory to feed hive.";
            return false;
        }

        public bool HarvestHoney(string hiveId)
        {
            var (honey, wax) = Apiculture.Harvest(hiveId);
            if (honey > 0f || wax > 0f)
            {
                int foodUnits = (int)Math.Max(1, Math.Round(honey * 10f));
                int waxUnits = (int)Math.Max(1, Math.Round(wax * 10f));
                if (honey > 0f) InventoryHost?.Add("food_rations", foodUnits);
                if (wax > 0f) InventoryHost?.Add("crafting_parts", waxUnits);
                LastEvent = $"Apiary: Harvested {honey:F2}kg honey ({foodUnits} rations), {wax:F2}kg wax ({waxUnits} parts).";
                RaiseStateChanged();
                return true;
            }
            LastEvent = "Apiary: No honey or wax ready for harvest.";
            return false;
        }

        public void TickDay(int currentDay, float growLightHours = 6f, float ashContaminationRate = 0.05f)
        {
            System.TickDay(currentDay, growLightHours, ashContaminationRate);
            Apiculture.TickDaily(
                day: currentDay,
                greenhouseTemperatureC: 22f,
                greenhouseContamination: ashContaminationRate,
                radiationLevel: 2f,
                rng: new SeededRng(DefaultSeed + currentDay));
            RaiseStateChanged();
        }

        public GreenhouseState CaptureSave()
        {
            var state = System.CaptureState();
            state.apiculture = Apiculture.CaptureState();
            return state;
        }
    }

    /// <summary>
    /// Persistent on-disk JSON store for GreenhouseState at user://greenhouse_save.json.
    /// Thin façade over the Core SaveStore&lt;T&gt; service (via SaveStoreHub,
    /// codec flavor): this section's on-disk shape (indented System.Text.Json,
    /// property envelope <c>{ State, Checksum }</c>) is preserved verbatim by
    /// local encode/decode delegates; path resolution, atomic write, and
    /// error handling live in the service.
    /// </summary>
    public static class GreenhouseSaveStore
    {
        public const string FileName = "greenhouse_save.json";
        public const string SectionName = "greenhouse";

        private static readonly JsonSerializerOptions JsonOpts = new JsonSerializerOptions
        {
            WriteIndented = true,
            IncludeFields = true
        };

        private static readonly SaveStore<GreenhouseState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(GreenhouseSaveStore),
            EncodeSave,
            DecodeSave);

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        public static bool TrySave(GreenhouseState state) => s_store.TrySave(state);

        public static GreenhouseState? TryLoad() => s_store.TryLoad();

        /// <summary>Capture the exact persisted bytes for the campaign envelope without writing to disk.</summary>
        public static string TryCapturePersisted(GreenhouseState state) => s_store.CapturePersisted(state);

        private static string EncodeSave(GreenhouseState state, IJsonSerializer json)
        {
            var envelope = new GreenhouseSaveEnvelope { State = state };
            envelope.Checksum = SaveChecksum.Compute(envelope);
            return JsonSerializer.Serialize(envelope, JsonOpts);
        }

        private static GreenhouseState? DecodeSave(string raw, IJsonSerializer json)
        {
            // Attempt envelope decode first
            try
            {
                var envelope = JsonSerializer.Deserialize<GreenhouseSaveEnvelope>(raw, JsonOpts);
                if (envelope?.State != null)
                {
                    if (string.IsNullOrEmpty(envelope.Checksum))
                        throw new InvalidOperationException("save envelope missing checksum (corrupt save)");
                    if (!string.Equals(envelope.Checksum, SaveChecksum.Compute(envelope), StringComparison.Ordinal))
                        throw new InvalidOperationException("checksum mismatch — possible tampering");
                    return envelope.State;
                }
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch
            {
                // Fall back to legacy bare state decode
            }

            return JsonSerializer.Deserialize<GreenhouseState>(raw, JsonOpts);
        }
    }

    [Serializable]
    public sealed class GreenhouseSaveEnvelope
    {
        public GreenhouseState? State { get; set; }
        public string? Checksum { get; set; }
    }
}
