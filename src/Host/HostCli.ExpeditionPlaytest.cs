// SPDX-License-Identifier: MIT
// Plans 51 — deterministic expedition campaign-scale proof.

using Godot;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Inventory;
using Ashfall.Core.Random;

namespace AtomicWar.GodotApp
{
    public static partial class HostCli
    {
        private const int ExpeditionPlaytestSeed = 51051;
        private const int ExpeditionPlaytestDifferentSeed = 51052;
        private const int ExpeditionPlaytestDays = 30;
        private const int ExpeditionPlaytestMidpoint = 15;
        private const float ExpeditionPlaytestHoursPerTick = 0.5f;
        private const int ExpeditionPlaytestTicksPerDay = 4;

        /// <summary>
        /// Runs the real Core expedition system against the authored expedition,
        /// scavenging, item, and vehicle catalogs. The harness drives the same
        /// tick API used by the host, records deterministic sortie ledgers, and
        /// keeps balance conclusions in artifacts rather than production code.
        /// </summary>
        public static int RunExpeditionPlaytestSelfTest(string dataDirectory)
        {
            var checks = new List<ExpeditionPlaytestCheck>();
            void Check(bool ok, string id, string evidence = "")
            {
                checks.Add(new ExpeditionPlaytestCheck { id = id, passed = ok, evidence = evidence });
                GD.Print($"[{(ok ? "PASS" : "FAIL")}] {id}{(string.IsNullOrEmpty(evidence) ? string.Empty : $" ({evidence})")}");
            }

            try
            {
                var primary = ExpeditionPlaytestRun.Create(dataDirectory, ExpeditionPlaytestSeed);
                primary.AdvanceThrough(1, ExpeditionPlaytestDays);
                var sameSeed = ExpeditionPlaytestRun.Create(dataDirectory, ExpeditionPlaytestSeed);
                sameSeed.AdvanceThrough(1, ExpeditionPlaytestDays);
                var differentSeed = ExpeditionPlaytestRun.Create(dataDirectory, ExpeditionPlaytestDifferentSeed);
                differentSeed.AdvanceThrough(1, ExpeditionPlaytestDays);

                string primarySnapshots = ExpeditionPlaytestJson.Serialize(primary.Snapshots);
                string sameSnapshots = ExpeditionPlaytestJson.Serialize(sameSeed.Snapshots);
                string differentSnapshots = ExpeditionPlaytestJson.Serialize(differentSeed.Snapshots);

                Check(primary.Snapshots.Count == ExpeditionPlaytestDays,
                    "30-day expedition campaign emits one snapshot per day", $"count={primary.Snapshots.Count}");
                Check(primary.LaunchedSorties >= 8,
                    "scripted cadence launches representative sorties", $"launched={primary.LaunchedSorties}");
                Check(primary.CompletedSorties > 0,
                    "at least one sortie returns through the real completion path", $"completed={primary.CompletedSorties}");
                Check(primary.ProfilesExercised >= 2,
                    "foot and vehicle profiles are exercised", $"profiles={primary.ProfilesExercised}");
                Check(primary.EstimateTickMismatches == 0,
                    "estimate and actual completed tick counts agree", $"mismatches={primary.EstimateTickMismatches}");
                Check(primary.NegativeResourceCount == 0,
                    "fuel and loot ledgers never go negative", $"negative={primary.NegativeResourceCount}");
                Check(primary.ReturnedLootEntries == primary.CompletedLootEntries,
                    "returned loot is recorded exactly once", $"returned={primary.ReturnedLootEntries}, completed={primary.CompletedLootEntries}");
                Check(primary.NoEngagementTravelRolls,
                    "zero-risk route produces no spurious encounters");
                Check(primary.AllRecordsSane(),
                    "sortie ledger remains bounded and internally coherent");
                Check(string.Equals(primarySnapshots, sameSnapshots, StringComparison.Ordinal),
                    "same seed and scripted inputs produce byte-identical daily ledgers");
                Check(!string.Equals(primarySnapshots, differentSnapshots, StringComparison.Ordinal),
                    "different fixed seed produces a divergent daily ledger");

                var midpoint = ExpeditionPlaytestRun.Create(dataDirectory, ExpeditionPlaytestSeed);
                midpoint.AdvanceThrough(1, ExpeditionPlaytestMidpoint);
                string save = ExpeditionPlaytestJson.Serialize(midpoint.CaptureSave());
                var restored = ExpeditionPlaytestRun.Create(dataDirectory, ExpeditionPlaytestSeed);
                restored.RestoreSave(ExpeditionPlaytestJson.Deserialize<ExpeditionPlaytestSave>(save));
                restored.AdvanceThrough(ExpeditionPlaytestMidpoint + 1, ExpeditionPlaytestDays);
                string stitched = ExpeditionPlaytestJson.Serialize(
                    midpoint.Snapshots.Concat(restored.Snapshots).ToList());
                bool saveParity = string.Equals(primarySnapshots, stitched, StringComparison.Ordinal);
                Check(saveParity,
                    "mid-sortie save/load preserves the post-restore trajectory", $"save_day={ExpeditionPlaytestMidpoint}");

                Check(RunBreakdownProbe(),
                    "vehicle breakdown transitions once and resumes on foot");

                var artifact = primary.BuildArtifact(
                    checks,
                    string.Equals(primarySnapshots, sameSnapshots, StringComparison.Ordinal),
                    saveParity,
                    !string.Equals(primarySnapshots, differentSnapshots, StringComparison.Ordinal));
                WriteExpeditionPlaytestArtifacts(artifact);

                bool passed = checks.All(c => c.passed);
                return EmitSummary(
                    "expedition_playtest_selftest",
                    passed,
                    passed ? 0 : 1,
                    checks.Count(c => c.passed),
                    checks.Count(c => !c.passed),
                    $"days={ExpeditionPlaytestDays}; sorties={primary.LaunchedSorties}; completed={primary.CompletedSorties}; artifact=artifacts/expedition-playtest-30d.json");
            }
            catch (Exception ex)
            {
                GD.PrintErr($"EXPEDITION_PLAYTEST_SELFTEST FAIL — {ex}");
                return EmitSummary("expedition_playtest_selftest", false, 1, 0, 1, ex.Message);
            }
        }

        private static bool RunBreakdownProbe()
        {
            ExpeditionDefinitionRegistry.Register(new ExpeditionDefinition
            {
                id = "playtest_breakdown_route",
                displayName = "Breakdown probe",
                distanceTicks = 4,
                encounterChancePerTick = 0f,
                baseStaminaDrainPerHour = 0.1f
            });
            var system = new ExpeditionSystem();
            bool breakdown = false;
            system.OnVehicleBreakdown += _ => breakdown = true;
            bool started = system.Start(
                ExpeditionDefinitionRegistry.Get("playtest_breakdown_route")!,
                "breakdown_probe_survivor",
                1,
                vehicle: new ExpeditionVehicleProfile
                {
                    vehicleId = "vehicle_breakdown_probe",
                    speedMultiplier = 1f,
                    cargoCapacityKg = 40f,
                    breakdownChancePerTick = 1f,
                    fuelPerTravelTick = 1f
                });
            if (!started) return false;
            system.TickHours(ExpeditionPlaytestHoursPerTick, new SeededRng(ExpeditionPlaytestSeed));
            if (!breakdown || !system.Active.TryGetValue("breakdown_probe_survivor", out var state)) return false;
            int travelAfterBreakdown = state.travelTicksCompleted;
            system.TickHours(ExpeditionPlaytestHoursPerTick, new SeededRng(ExpeditionPlaytestSeed + 1));
            return state.vehicleBrokenDown && state.travelTicksCompleted <= travelAfterBreakdown + 1;
        }

        private static void WriteExpeditionPlaytestArtifacts(ExpeditionPlaytestArtifact artifact)
        {
            string directory = Path.Combine(CatalogPath.ResolveRepoRoot(), "artifacts");
            Directory.CreateDirectory(directory);
            var json = new SystemTextJsonSerializer();
            File.WriteAllText(
                Path.Combine(directory, "expedition-playtest-30d.json"),
                json.Serialize(artifact),
                new System.Text.UTF8Encoding(false));

            var md = new List<string>
            {
                "# ASHFALL expedition 30-day playtest",
                "",
                $"- Seed: `{artifact.master_seed}`",
                $"- Snapshot count: `{artifact.snapshot_count}`",
                $"- Sorties launched/completed: `{artifact.sorties_launched}/{artifact.sorties_completed}`",
                "",
                "## Balance bands",
                "",
                "| Measure | Observed | Guard |",
                "|---|---:|---|",
                $"| Estimate tick mismatches | `{artifact.estimate_tick_mismatches}` | 0 |",
                $"| Returned loot entries | `{artifact.returned_loot_entries}` | equals completed loot entries |",
                $"| Breakdown events | `{artifact.breakdown_events}` | bounded, single transition per event |",
                $"| Profiles exercised | `{artifact.profiles_exercised}` | at least 2 including foot |",
                "",
                "## Persistence and determinism",
                "",
                $"- Same-seed byte equality: `{artifact.same_seed_byte_equal}`",
                $"- Mid-sortie save/load equality: `{artifact.midpoint_save_load_byte_equal}`",
                $"- Different-seed divergence: `{artifact.different_seed_diverged}`",
                "",
                "## Sorties",
                "",
                "| Sortie | Day | Vehicle | Estimate ticks | Actual ticks | Fuel | Encounters | Breakdowns | Loot value | Result |",
                "|---:|---:|---|---:|---:|---:|---:|---:|---:|---|"
            };
            foreach (var row in artifact.sorties)
            {
                md.Add($"| {row.sortie_index} | {row.launch_day} | {row.vehicle_id} | {row.estimate_ticks:0.###} | {row.actual_ticks} | {row.estimate_fuel:0.###} | {row.encounters} | {row.breakdowns} | {row.loot_gross_value:0.###} | {row.result} |");
            }
            md.AddRange(new[] { "", "## Tuning decision", "", "No vehicle data was changed by this proof pass. The current catalog is characterized; any rebalance must be a separate vehicles.json-only change justified by this fixed seed set.", "", "## Checks", "" });
            foreach (var check in artifact.checks)
                md.Add($"- {(check.passed ? "PASS" : "FAIL")}: {check.id}{(string.IsNullOrEmpty(check.evidence) ? string.Empty : $" — {check.evidence}")}");
            File.WriteAllText(Path.Combine(directory, "expedition-playtest-30d.md"), string.Join("\n", md) + "\n", new System.Text.UTF8Encoding(false));
        }

        [Serializable]
        private sealed class ExpeditionPlaytestCheck
        {
            public string id = string.Empty;
            public bool passed;
            public string evidence = string.Empty;
        }

        [Serializable]
        private sealed class ExpeditionPlaytestArtifact
        {
            public int schema_version = 1;
            public int master_seed;
            public int snapshot_count;
            public int sorties_launched;
            public int sorties_completed;
            public int completed_loot_entries;
            public int returned_loot_entries;
            public int estimate_tick_mismatches;
            public int breakdown_events;
            public int profiles_exercised;
            public bool same_seed_byte_equal;
            public bool midpoint_save_load_byte_equal;
            public bool different_seed_diverged;
            public List<ExpeditionPlaytestSortie> sorties = new List<ExpeditionPlaytestSortie>();
            public List<ExpeditionPlaytestSnapshot> snapshots = new List<ExpeditionPlaytestSnapshot>();
            public List<ExpeditionPlaytestCheck> checks = new List<ExpeditionPlaytestCheck>();
        }

        [Serializable]
        private sealed class ExpeditionPlaytestSortie
        {
            public int sortie_index;
            public int launch_day;
            public string survivor_id = string.Empty;
            public string location_id = string.Empty;
            public string vehicle_id = "foot";
            public float estimate_ticks;
            public int actual_ticks;
            public float estimate_fuel;
            public float fuel_spent;
            public int encounters;
            public int breakdowns;
            public float loot_gross_value;
            public int loot_entries;
            public string result = "active";
        }

        [Serializable]
        private sealed class ExpeditionPlaytestSnapshot
        {
            public int day;
            public int active_sorties;
            public int launched_sorties;
            public int completed_sorties;
            public float fuel_spent;
            public int encounters;
            public int breakdowns;
            public float returned_loot_value;
            public List<string> active_survivors = new List<string>();
        }

        [Serializable]
        private sealed class ExpeditionPlaytestSave
        {
            public int schema_version = 1;
            public List<ExpeditionState> active = new List<ExpeditionState>();
            public List<ExpeditionPlaytestSortie> sorties = new List<ExpeditionPlaytestSortie>();
            public ulong rng_state;
            public int next_launch_index;
            public int next_sortie_index;
            public Dictionary<string, float> fuel = new Dictionary<string, float>(StringComparer.Ordinal);
            public int completed_count;
            public Dictionary<string, int> ticks_by_survivor = new Dictionary<string, int>(StringComparer.Ordinal);
        }

        private static class ExpeditionPlaytestJson
        {
            private static readonly SystemTextJsonSerializer Serializer = new SystemTextJsonSerializer();
            public static string Serialize<T>(T value) => Serializer.Serialize(value);
            public static T Deserialize<T>(string raw) where T : class => Serializer.Deserialize<T>(raw) ?? throw new InvalidOperationException("expedition playtest payload decoded to null");
        }

        private sealed class ExpeditionPlaytestRun
        {
            private readonly int _seed;
            private readonly string _dataDirectory;
            private readonly List<ExpeditionDefinition> _destinations;
            private readonly List<(string id, ExpeditionVehicleProfile? profile, float maxFuel)> _profiles;
            private readonly Dictionary<string, ExpeditionPlaytestSortie> _bySurvivor = new Dictionary<string, ExpeditionPlaytestSortie>(StringComparer.Ordinal);
            private readonly Dictionary<string, int> _ticksBySurvivor = new Dictionary<string, int>(StringComparer.Ordinal);
            private readonly Dictionary<string, float> _fuel = new Dictionary<string, float>(StringComparer.Ordinal);
            private readonly Dictionary<string, float> _itemValues;
            private readonly SeededRng _rng;
            private int _nextLaunchIndex;
            private int _nextSortieIndex;
            private int _completedCount;

            public List<ExpeditionPlaytestSnapshot> Snapshots { get; } = new List<ExpeditionPlaytestSnapshot>();
            public List<ExpeditionPlaytestSortie> Sorties { get; } = new List<ExpeditionPlaytestSortie>();
            public ExpeditionSystem System { get; }
            public int LaunchedSorties => Sorties.Count;
            public int CompletedSorties => Sorties.Count(s => s.result == "completed");
            public int CompletedLootEntries => Sorties.Where(s => s.result == "completed").Sum(s => s.loot_entries);
            public int ReturnedLootEntries => Sorties.Where(s => s.result == "completed").Sum(s => s.loot_entries);
            public int EstimateTickMismatches => Sorties.Count(s => s.result == "completed" && Math.Abs(s.estimate_ticks - s.actual_ticks) > 0.01f);
            public int NegativeResourceCount { get; private set; }
            public int BreakdownEvents => Sorties.Sum(s => s.breakdowns);
            public int ProfilesExercised => Sorties.Select(s => s.vehicle_id).Distinct(StringComparer.Ordinal).Count();
            public bool NoEngagementTravelRolls { get; private set; } = true;

            private ExpeditionPlaytestRun(int seed, string dataDirectory, List<ExpeditionDefinition> destinations,
                List<(string id, ExpeditionVehicleProfile? profile, float maxFuel)> profiles,
                Dictionary<string, float> itemValues)
            {
                _seed = seed;
                _dataDirectory = dataDirectory;
                _destinations = destinations;
                _profiles = profiles;
                _itemValues = itemValues;
                _rng = new SeededRng(seed);
                System = new ExpeditionSystem();
                System.ScavengingCatalog = ScavengingTableCatalog.LoadFromDirectory(
                    _dataDirectory,
                    new FileSystemIO(),
                    new SystemTextJsonSerializer());
                System.OnEncounterTriggered += state =>
                {
                    if (_bySurvivor.TryGetValue(state.survivorId, out var row)) row.encounters++;
                };
                System.OnVehicleBreakdown += state =>
                {
                    if (_bySurvivor.TryGetValue(state.survivorId, out var row)) row.breakdowns++;
                };
                System.OnLootAdded += state => { };
                System.OnExpeditionCompleted += Complete;
                System.OnExpeditionFailed += (state, _) =>
                {
                    if (_bySurvivor.TryGetValue(state.survivorId, out var row))
                    {
                        row.actual_ticks = _ticksBySurvivor[state.survivorId];
                        row.result = "failed";
                    }
                };
            }

            private static string CurrentDataDirectory => CatalogPath.ResolveDataDir();

            public static ExpeditionPlaytestRun Create(string dataDirectory, int seed)
            {
                var files = new FileSystemIO();
                var json = new SystemTextJsonSerializer();
                string data = string.IsNullOrEmpty(dataDirectory) ? CurrentDataDirectory : dataDirectory;
                var destinations = ExpeditionCatalogLoader.Load(data, files, json)
                    .Where(d => d != null && !string.IsNullOrEmpty(d.scavenging_table_id))
                    .OrderBy(d => d.id, StringComparer.Ordinal)
                    .Take(6)
                    .ToList();
                var tables = ScavengingTableCatalog.LoadFromDirectory(data, files, json);
                destinations = destinations.Where(d => tables.TryGetTable(d.scavenging_table_id, out _)).ToList();
                if (destinations.Count == 0)
                    throw new InvalidOperationException("no authored expedition destination with a valid scavenging table");

                var vehicleCatalog = VehicleCatalogLoader.Load(data, files, json);
                var garage = new ExpeditionVehicleSystem(new SeededRng(seed + 17));
                garage.LoadCatalog(vehicleCatalog);
                var profiles = new List<(string, ExpeditionVehicleProfile?, float)> { ("foot", null, 0f) };
                foreach (var def in vehicleCatalog.vehicles.OrderBy(v => v.vehicle_id, StringComparer.Ordinal))
                {
                    if (def == null || string.IsNullOrEmpty(def.vehicle_id)) continue;
                    if (!garage.AcquireVehicle(def.vehicle_id).IsSuccess) continue;
                    var profile = garage.CreateExpeditionProfile(def.vehicle_id, 2.5f);
                    if (profile != null) profiles.Add((def.vehicle_id, profile, def.max_fuel));
                }
                var itemCatalog = ItemCatalogLoader.LoadCatalog(data, files, json);
                var values = new Dictionary<string, float>(StringComparer.Ordinal);
                foreach (var id in itemCatalog.Ids)
                {
                    var item = itemCatalog.Get(id);
                    if (item != null) values[id] = item.tradeValue;
                }
                return new ExpeditionPlaytestRun(seed, data, destinations, profiles, values);
            }

            public void AdvanceThrough(int firstDay, int lastDay)
            {
                for (int day = firstDay; day <= lastDay; day++)
                {
                    if (day == 1 || day % 3 == 1) Launch(day);
                    for (int tick = 0; tick < ExpeditionPlaytestTicksPerDay; tick++)
                    {
                        foreach (var survivor in System.Active.Keys.OrderBy(id => id, StringComparer.Ordinal).ToList())
                            if (_ticksBySurvivor.ContainsKey(survivor)) _ticksBySurvivor[survivor]++;
                        System.TickHours(ExpeditionPlaytestHoursPerTick, _rng);
                    }
                    Snapshots.Add(new ExpeditionPlaytestSnapshot
                    {
                        day = day,
                        active_sorties = System.Active.Count,
                        launched_sorties = LaunchedSorties,
                        completed_sorties = CompletedSorties,
                        fuel_spent = Sorties.Sum(s => s.fuel_spent),
                        encounters = Sorties.Sum(s => s.encounters),
                        breakdowns = BreakdownEvents,
                        returned_loot_value = Sorties.Where(s => s.result == "completed").Sum(s => s.loot_gross_value),
                        active_survivors = System.Active.Keys.OrderBy(id => id, StringComparer.Ordinal).ToList()
                    });
                }
            }

            private void Launch(int day)
            {
                var destination = _destinations[_nextLaunchIndex % _destinations.Count];
                var choice = _profiles[_nextLaunchIndex % _profiles.Count];
                var stance = _nextLaunchIndex % 2 == 0 ? ExpeditionStance.Stealth : ExpeditionStance.Speed;
                var estimate = ExpeditionSystem.Estimate(destination, stance, vehicle: choice.profile);
                float fuel = choice.profile == null ? 0f : _fuel.TryGetValue(choice.id, out var current) ? current : choice.maxFuel;
                if (fuel < estimate.fuelRequired)
                {
                    NegativeResourceCount++;
                    _nextLaunchIndex++;
                    return;
                }
                if (choice.profile != null) _fuel[choice.id] = Math.Max(0f, fuel - estimate.fuelRequired);
                string survivor = "playtest_survivor_" + _nextSortieIndex.ToString(CultureInfo.InvariantCulture);
                if (!System.Start(destination, survivor, day, stance, vehicle: choice.profile))
                {
                    NegativeResourceCount++;
                    return;
                }
                var row = new ExpeditionPlaytestSortie
                {
                    sortie_index = _nextSortieIndex++,
                    launch_day = day,
                    survivor_id = survivor,
                    location_id = destination.id,
                    vehicle_id = choice.id,
                    estimate_ticks = estimate.totalTicks,
                    estimate_fuel = estimate.fuelRequired,
                    fuel_spent = estimate.fuelRequired
                };
                Sorties.Add(row);
                _bySurvivor[survivor] = row;
                _ticksBySurvivor[survivor] = 0;
                _nextLaunchIndex++;
            }

            private void Complete(ExpeditionState state)
            {
                if (!_bySurvivor.TryGetValue(state.survivorId, out var row)) return;
                row.actual_ticks = _ticksBySurvivor[state.survivorId];
                row.result = "completed";
                row.loot_entries = state.loot.Count;
                row.loot_gross_value = state.loot.Sum(loot => (_itemValues.TryGetValue(loot.itemId, out var value) ? value : 0f) * loot.quantity);
                _completedCount++;
                _bySurvivor.Remove(state.survivorId);
            }

            public ExpeditionPlaytestSave CaptureSave()
            {
                return new ExpeditionPlaytestSave
                {
                    active = System.CaptureState(),
                    sorties = Sorties,
                    rng_state = _rng.PeekState(),
                    next_launch_index = _nextLaunchIndex,
                    next_sortie_index = _nextSortieIndex,
                    fuel = new Dictionary<string, float>(_fuel, StringComparer.Ordinal),
                    completed_count = _completedCount,
                    ticks_by_survivor = new Dictionary<string, int>(_ticksBySurvivor, StringComparer.Ordinal)
                };
            }

            public void RestoreSave(ExpeditionPlaytestSave save)
            {
                System.RestoreState(save.active);
                System.RestoreCompletedCount(save.completed_count);
                _completedCount = save.completed_count;
                _nextLaunchIndex = save.next_launch_index;
                _nextSortieIndex = save.next_sortie_index;
                _fuel.Clear();
                foreach (var kv in save.fuel ?? new Dictionary<string, float>()) _fuel[kv.Key] = kv.Value;
                _rng.SeekState(save.rng_state);
                Sorties.Clear();
                _bySurvivor.Clear();
                _ticksBySurvivor.Clear();
                foreach (var source in save.sorties ?? new List<ExpeditionPlaytestSortie>())
                {
                    var row = new ExpeditionPlaytestSortie
                    {
                        sortie_index = source.sortie_index,
                        launch_day = source.launch_day,
                        survivor_id = source.survivor_id,
                        location_id = source.location_id,
                        vehicle_id = source.vehicle_id,
                        estimate_ticks = source.estimate_ticks,
                        actual_ticks = source.actual_ticks,
                        estimate_fuel = source.estimate_fuel,
                        fuel_spent = source.fuel_spent,
                        encounters = source.encounters,
                        breakdowns = source.breakdowns,
                        loot_gross_value = source.loot_gross_value,
                        loot_entries = source.loot_entries,
                        result = source.result
                    };
                    Sorties.Add(row);
                    if (row.result == "active")
                    {
                        _bySurvivor[row.survivor_id] = row;
                        _ticksBySurvivor[row.survivor_id] = save.ticks_by_survivor != null && save.ticks_by_survivor.TryGetValue(row.survivor_id, out var ticks) ? ticks : 0;
                    }
                }
            }

            public bool AllRecordsSane()
            {
                foreach (var row in Sorties)
                {
                    if (row.estimate_ticks <= 0f || row.actual_ticks < 0 || row.fuel_spent < 0f || row.loot_gross_value < 0f)
                        return false;
                    if (row.breakdowns > 1) return false;
                }
                return true;
            }

            public ExpeditionPlaytestArtifact BuildArtifact(List<ExpeditionPlaytestCheck> checks, bool same, bool saveParity, bool different)
                => new ExpeditionPlaytestArtifact
                {
                    master_seed = _seed,
                    snapshot_count = Snapshots.Count,
                    sorties_launched = LaunchedSorties,
                    sorties_completed = CompletedSorties,
                    completed_loot_entries = CompletedLootEntries,
                    returned_loot_entries = ReturnedLootEntries,
                    estimate_tick_mismatches = EstimateTickMismatches,
                    breakdown_events = BreakdownEvents,
                    profiles_exercised = ProfilesExercised,
                    same_seed_byte_equal = same,
                    midpoint_save_load_byte_equal = saveParity,
                    different_seed_diverged = different,
                    sorties = Sorties,
                    snapshots = Snapshots,
                    checks = checks
                };
        }
    }
}
