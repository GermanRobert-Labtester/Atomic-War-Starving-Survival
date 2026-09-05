using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.Expeditions;
using Ashfall.Core.IO;
using Xunit;

namespace Ashfall.Core.Tests.Expeditions
{
    /// <summary>
    /// Plan 76.2 — seeded balance simulation over the fully-bound expedition
    /// destination catalog (53 destinations, 49 scavenging tables).
    ///
    /// Replicates the ExpeditionSystem runtime order 1:1 (TickHours →
    /// ApplyStaminaDrain → collapse → RollEncounter → phase advance with
    /// PerformLootRoll during Looting) for a baseline stealth, on-foot,
    /// no-vehicle, day sortie at 0.5 h/tick, 3 looting ticks auto-retreat.
    /// ISeededRng only. No production data is modified; the sweep writes a
    /// deterministic artifact to artifacts/balance-sim-expeditions.json and
    /// asserts sanity bounds. Findings:
    /// docs/balance/BALANCE_SIM_EXPEDITION_DESTINATIONS.md.
    ///
    /// Seed manifest: one SeededRng per sortie, seed =
    /// 761_000_000 + destinationIndex * 1000 + runIndex, where
    /// destinationIndex is the record order in expeditions.json.
    /// </summary>
    public sealed class Plan76BalanceSimulationTests
    {
        private const int RunsPerDestination = 200;
        private const int SeedBase = 761_000_000;
        private const float HoursPerTick = 0.5f;
        private const float EncumberPenaltyPerTickMax = 15f;
        private const float MaxStamina = 100f;
        private const float FootCapacityKg = 40f;
        private const int AutoRetreatAfterLootTicks = 3;

        private sealed class ItemValueDto
        {
            public string id { get; set; } = string.Empty;
            public float tradeValue { get; set; }
        }

        private sealed class ItemCatalogDto
        {
            public List<ItemValueDto> items { get; set; } = new List<ItemValueDto>();
        }

        private sealed class DestinationDto
        {
            public string id { get; set; } = string.Empty;
            public string displayName { get; set; } = string.Empty;
            public int distanceTicks { get; set; }
            public int dangerLevel { get; set; }
            public float encounterChancePerTick { get; set; }
            public float baseStaminaDrainPerHour { get; set; }
            public string scavenging_table_id { get; set; } = string.Empty;
        }

        private sealed class SortieAggregate
        {
            public double MeanValue;
            public double MeanEncounters;
            public double MeanTicks;
            public double MeanStaminaSpent;
            public double CollapseP;
            public double ValuePerStamina;
        }

        private readonly string _dataDir;

        public Plan76BalanceSimulationTests()
        {
            _dataDir = Path.Combine(AppContext.BaseDirectory, "../../../..", "Assets/StreamingAssets/Data");
            if (!Directory.Exists(_dataDir))
            {
                _dataDir = Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data");
            }
        }

        private Dictionary<string, float> LoadItemValues(out List<string> catalogsLoaded)
        {
            var values = new Dictionary<string, float>(StringComparer.Ordinal);
            catalogsLoaded = new List<string>();
            var serializer = new SystemTextJsonSerializer();
            foreach (var file in Directory.GetFiles(_dataDir, "*items*.json"))
            {
                try
                {
                    var catalog = serializer.Deserialize<ItemCatalogDto>(File.ReadAllText(file));
                    if (catalog?.items == null) continue;
                    bool any = false;
                    foreach (var item in catalog.items)
                    {
                        if (!string.IsNullOrEmpty(item?.id) && !values.ContainsKey(item.id))
                        {
                            values[item.id] = item.tradeValue;
                            any = true;
                        }
                    }

                    if (any) catalogsLoaded.Add(Path.GetFileName(file));
                }
                catch
                {
                    // not an item-list shape — skip (every *items*.json in the
                    // authority parses today)
                }
            }

            return values;
        }

        private List<DestinationDto> LoadAuthoredDestinations()
        {
            var raw = File.ReadAllText(Path.Combine(_dataDir, "expeditions.json"));
            var dtos = CatalogLocator.LoadWrappedList<DestinationDto>(raw, SystemTextJsonSerializer.Options);
            return dtos.Where(d => d != null && !string.IsNullOrEmpty(d.id)).ToList();
        }

        /// <summary>Deterministic sweep. One SeededRng per sortie — same
        /// inputs and seeds always produce identical aggregates.</summary>
        private (List<Dictionary<string, object>> rows, HashSet<string> unknownValueItems) RunSweep(
            List<DestinationDto> dests, ScavengingTableCatalog catalog, Dictionary<string, float> itemValues)
        {
            var unknownValueItems = new HashSet<string>(StringComparer.Ordinal);
            var rows = new List<Dictionary<string, object>>();

            for (int di = 0; di < dests.Count; di++)
            {
                var d = dests[di];
                string tableId = d.scavenging_table_id;

                double sumValue = 0, sumEnc = 0, sumTicks = 0, sumStamina = 0;
                int completed = 0;

                for (int run = 0; run < RunsPerDestination; run++)
                {
                    var rng = new SeededRng(SeedBase + di * 1000 + run);
                    float stamina = MaxStamina, carriedKg = 0f;
                    int travelDone = 0, lootTicks = 0, ticks = 0, encounters = 0, phase = 0;
                    var loot = new Dictionary<string, int>();
                    bool sortieCompleted = false;

                    // TickHours order: stamina drain → collapse → encounter →
                    // phase advance (loot roll during Looting).
                    while (phase != 3 && phase != 4 && ticks < 200)
                    {
                        ticks++;
                        float drain = d.baseStaminaDrainPerHour * HoursPerTick
                                      + Math.Clamp(carriedKg / FootCapacityKg, 0f, 1f) * EncumberPenaltyPerTickMax * HoursPerTick;
                        stamina = Math.Clamp(stamina - drain, 0f, MaxStamina);
                        if (stamina <= 0f) { phase = 4; break; }

                        if (rng.NextDouble() < Math.Clamp(d.encounterChancePerTick, 0f, 1f) * 0.5f) encounters++;

                        switch (phase)
                        {
                            case 0:
                                travelDone++;
                                if (travelDone >= d.distanceTicks) phase = 1;
                                break;
                            case 1:
                                lootTicks++;
                                if (rng.NextDouble() < 0.5f + d.dangerLevel * 0.05f)
                                {
                                    var roll = catalog.RollLoot(tableId, rng);
                                    if (roll != null && !string.IsNullOrEmpty(roll.ItemId))
                                    {
                                        float w = 1.0f * roll.Quantity;
                                        if (carriedKg + w <= FootCapacityKg)
                                        {
                                            loot.TryGetValue(roll.ItemId, out var q);
                                            loot[roll.ItemId] = q + roll.Quantity;
                                            carriedKg += w;
                                        }
                                    }
                                }

                                if (lootTicks >= AutoRetreatAfterLootTicks) phase = 2;
                                break;
                            case 2:
                                travelDone--;
                                if (travelDone <= 0) { phase = 3; sortieCompleted = true; }
                                break;
                        }
                    }

                    float value = 0f;
                    foreach (var kv in loot)
                    {
                        if (itemValues.TryGetValue(kv.Key, out var v)) value += v * kv.Value;
                        else unknownValueItems.Add(kv.Key);
                    }

                    if (sortieCompleted) { completed++; sumValue += value; }
                    sumEnc += encounters;
                    sumTicks += ticks;
                    sumStamina += MaxStamina - stamina;
                }

                double meanValue = sumValue / RunsPerDestination;
                double meanStaminaSpent = sumStamina / RunsPerDestination;

                rows.Add(new Dictionary<string, object>
                {
                    ["id"] = d.id,
                    ["dangerLevel"] = d.dangerLevel,
                    ["distanceTicks"] = d.distanceTicks,
                    ["table"] = tableId,
                    ["meanValue"] = Math.Round(meanValue, 2),
                    ["meanEncounters"] = Math.Round(sumEnc / RunsPerDestination, 3),
                    ["meanTicks"] = Math.Round(sumTicks / RunsPerDestination, 2),
                    ["meanStaminaSpent"] = Math.Round(meanStaminaSpent, 2),
                    ["collapseP"] = Math.Round(1.0 - (double)completed / RunsPerDestination, 4),
                    ["valuePerStamina"] = Math.Round(meanStaminaSpent > 0 ? meanValue / meanStaminaSpent : 0, 4),
                    ["completedRate"] = Math.Round((double)completed / RunsPerDestination, 4)
                });
            }

            return (rows, unknownValueItems);
        }

        [Fact]
        public void BalanceSweep_Deterministic_WithSanityBounds()
        {
            var fileIO = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();
            var catalog = ScavengingTableCatalog.LoadFromJson(
                fileIO.ReadAllText(Path.Combine(_dataDir, "scavenging_tables.json")), serializer);
            var dests = LoadAuthoredDestinations();
            Assert.Equal(53, dests.Count);

            var itemValues = LoadItemValues(out var catalogsLoaded);
            Assert.True(itemValues.Count > 300, $"item value merge too small: {itemValues.Count}");

            // Pass 1 and pass 2 must agree byte-for-byte (determinism proof).
            var (rows1, unknown1) = RunSweep(dests, catalog, itemValues);
            var (rows2, _) = RunSweep(dests, catalog, itemValues);
            var json1 = System.Text.Json.JsonSerializer.Serialize(rows1);
            var json2 = System.Text.Json.JsonSerializer.Serialize(rows2);
            Assert.Equal(json1, json2);

            // sanity bounds
            foreach (var r in rows1)
            {
                Assert.True((double)r["meanValue"] >= 0, $"{r["id"]}: negative mean value");
                Assert.True((double)r["collapseP"] <= 1.0, $"{r["id"]}: collapse probability out of range");
                Assert.True((double)r["completedRate"] > 0.0, $"{r["id"]}: never completes in 200 runs — dead destination");
            }

            // write artifact to the repo-root artifacts/ dir
            var artifactDir = _dataDir;
            while (artifactDir is not null && !File.Exists(Path.Combine(artifactDir, "project.godot")))
                artifactDir = Path.GetDirectoryName(artifactDir);
            Assert.False(artifactDir is null, "could not locate repo root from data dir");
            artifactDir = Path.Combine(artifactDir!, "artifacts");
            Directory.CreateDirectory(artifactDir);
            var payload = new Dictionary<string, object>
            {
                ["schema_version"] = 1,
                ["seedBase"] = SeedBase,
                ["runsPerDestination"] = RunsPerDestination,
                ["model"] = "stealth, on foot, no vehicle, day sortie, 0.5h ticks, 3 looting ticks auto-retreat",
                ["itemValueCatalogs"] = catalogsLoaded,
                ["unknownValueItems"] = unknown1.OrderBy(x => x).ToList(),
                ["destinations"] = rows1
            };
            File.WriteAllText(
                Path.Combine(artifactDir, "balance-sim-expeditions.json"),
                System.Text.Json.JsonSerializer.Serialize(payload, new System.Text.Json.JsonSerializerOptions { WriteIndented = true }));
        }
    }
}
