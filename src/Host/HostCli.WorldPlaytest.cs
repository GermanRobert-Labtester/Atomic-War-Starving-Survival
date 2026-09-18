// SPDX-License-Identifier: MIT
// ASHFALL flagship Tasks 5-8: deterministic 30-day evolving-world proof.

using Godot;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Economy;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Random;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    public static partial class HostCli
    {
        private const int WorldPlaytestSeed = 424242;
        private const int WorldPlaytestDifferentSeed = 2026;
        private const int WorldPlaytestDays = 30;
        private const int WorldPlaytestMidpoint = 15;

        /// <summary>
        /// Runs the real seeded Core world systems through the campaign day
        /// coordinator. This is a validation harness, not a second gameplay
        /// authority: production rules remain in the Core systems and the
        /// production owner remains Main.CampaignOwners.EvolvingWorldDayOwner.
        /// </summary>
        public static int RunWorldPlaytestSelfTest(string dataDirectory, Main? productionMain = null)
        {
            CatalogLocator.UseInvariantCulture();
            var checks = new List<WorldPlaytestCheck>();
            void Check(bool ok, string label, string evidence = "")
            {
                checks.Add(new WorldPlaytestCheck { id = label, passed = ok, evidence = evidence });
                GD.Print($"[{(ok ? "PASS" : "FAIL")}] {label}{(string.IsNullOrEmpty(evidence) ? string.Empty : $" ({evidence})")}");
            }

            try
            {
                if (productionMain != null)
                {
                    Check(productionMain.RunWorldPlaytestProductionOwnerProbe(WorldPlaytestDays),
                        "production EvolvingWorldDayOwner advances the fixed 30-day window");
                }

                var primary = WorldPlaytestRun.Create(dataDirectory, WorldPlaytestSeed);
                primary.AdvanceThrough(1, WorldPlaytestDays);

                var sameSeed = WorldPlaytestRun.Create(dataDirectory, WorldPlaytestSeed);
                sameSeed.AdvanceThrough(1, WorldPlaytestDays);

                var differentSeed = WorldPlaytestRun.Create(dataDirectory, WorldPlaytestDifferentSeed);
                differentSeed.AdvanceThrough(1, WorldPlaytestDays);

                string primarySnapshots = WorldPlaytestJson.Serialize(primary.Snapshots);
                string sameSeedSnapshots = WorldPlaytestJson.Serialize(sameSeed.Snapshots);
                string differentSeedSnapshots = WorldPlaytestJson.Serialize(differentSeed.Snapshots);

                Check(primary.Snapshots.Count == WorldPlaytestDays,
                    "30 daily snapshots emitted", $"count={primary.Snapshots.Count}");
                Check(primary.Migrations > 0,
                    "wildlife migration is visible", $"events={primary.Migrations}");
                Check(primary.DegradationTransitions > 0,
                    "location degradation tier changes", $"transitions={primary.DegradationTransitions}");
                Check(primary.DensityDeltaMagnitude >= 1f,
                    "wildlife density moves meaningfully", $"delta={primary.DensityDeltaMagnitude.ToString("0.###", CultureInfo.InvariantCulture)}");
                Check(primary.EncounterMultiplierMovement >= 0.01f,
                    "encounter multiplier moves meaningfully",
                    $"movement={primary.EncounterMultiplierMovement.ToString("0.###", CultureInfo.InvariantCulture)}");
                Check(primary.ScarcityMovement >= 0.001f,
                    "scarcity proxy moves", $"movement={primary.ScarcityMovement.ToString("0.###", CultureInfo.InvariantCulture)}");
                Check(primary.SurfacedBriefingEvents > 0 && primary.SurfacedJournalEvents > 0 && primary.SurfacedRadioEvents > 0,
                    "world changes surface through briefing/journal/radio",
                    $"briefing={primary.SurfacedBriefingEvents}, journal={primary.SurfacedJournalEvents}, radio={primary.SurfacedRadioEvents}");

                Check(primary.AllSnapshotsSane(), "world trajectory stays inside documented bands");
                Check(primary.RuinedStatesAreSticky(), "ruined location states remain monotonic");
                Check(primary.NoDuplicateSnapshotRecords(), "daily snapshots contain no duplicate location or wildlife records");
                Check(primary.TrappingDayTicks == WorldPlaytestDays
                      && primary.TrappingDensityReads == WorldPlaytestDays,
                    "trapping density is consumed once per campaign day",
                    $"ticks={primary.TrappingDayTicks}, reads={primary.TrappingDensityReads}");
                Check(primary.EncounterMultiplierReads == primary.EncounterEvaluationTicks,
                    "expedition encounter multiplier is consumed once per evaluation",
                    $"reads={primary.EncounterMultiplierReads}, evaluations={primary.EncounterEvaluationTicks}");
                Check(primary.DeadSeedCount == 0,
                    "every world-evolution seed participates in the live tick/read pipeline",
                    $"dead={primary.DeadSeedCount}");

                Check(string.Equals(primarySnapshots, sameSeedSnapshots, StringComparison.Ordinal),
                    "same-seed snapshot artifacts are byte-identical",
                    $"seed={WorldPlaytestSeed}");
                Check(!string.Equals(primarySnapshots, differentSeedSnapshots, StringComparison.Ordinal),
                    "different seed produces a divergent trajectory",
                    $"seed={WorldPlaytestDifferentSeed}");

                var midpoint = WorldPlaytestRun.Create(dataDirectory, WorldPlaytestSeed);
                midpoint.AdvanceThrough(1, WorldPlaytestMidpoint);
                string midpointSave = WorldPlaytestJson.Serialize(midpoint.CaptureSave());
                var restored = WorldPlaytestRun.Create(dataDirectory, WorldPlaytestSeed);
                var restoredSave = WorldPlaytestJson.Deserialize<WorldPlaytestSave>(midpointSave);
                restored.RestoreSave(restoredSave);
                restored.AdvanceThrough(WorldPlaytestMidpoint + 1, WorldPlaytestDays);

                var stitched = primary.Snapshots.Take(WorldPlaytestMidpoint)
                    .Concat(restored.Snapshots)
                    .ToList();
                string restoredSnapshots = WorldPlaytestJson.Serialize(stitched);
                Check(string.Equals(primarySnapshots, restoredSnapshots, StringComparison.Ordinal),
                    "midpoint save/load preserves the full post-restore trajectory",
                    $"save_day={WorldPlaytestMidpoint}");
                Check(restored.DeadSeedCount == 0 && restored.NoDuplicateNarrativeEvents,
                    "restored world keeps seed state and narrative de-duplication",
                    $"dead={restored.DeadSeedCount}, duplicate_events={!restored.NoDuplicateNarrativeEvents}");

                var artifact = primary.BuildArtifact(
                    dataDirectory,
                    checks,
                    string.Equals(primarySnapshots, sameSeedSnapshots, StringComparison.Ordinal),
                    string.Equals(primarySnapshots, restoredSnapshots, StringComparison.Ordinal),
                    !string.Equals(primarySnapshots, differentSeedSnapshots, StringComparison.Ordinal));
                WriteWorldPlaytestArtifacts(artifact);

                bool pass = checks.All(c => c.passed);
                GD.Print(pass
                    ? "WORLD_PLAYTEST_SELFTEST PASS"
                    : $"WORLD_PLAYTEST_SELFTEST FAIL — {checks.Count(c => !c.passed)} failing check(s)");
                return pass ? 0 : 1;
            }
            catch (Exception ex)
            {
                GD.PrintErr($"WORLD_PLAYTEST_SELFTEST FAIL — {ex.GetType().Name}: {ex.Message}");
                return 1;
            }
        }

        private static void WriteWorldPlaytestArtifacts(WorldPlaytestArtifact artifact)
        {
            string artifactDir = Path.Combine(CatalogPath.ResolveRepoRoot(), "artifacts");
            Directory.CreateDirectory(artifactDir);
            var json = new SystemTextJsonSerializer();
            File.WriteAllText(
                Path.Combine(artifactDir, "world-playtest-30d.json"),
                json.Serialize(artifact),
                new System.Text.UTF8Encoding(false));

            var md = new List<string>
            {
                "# ASHFALL evolving-world 30-day proof",
                "",
                $"- Source commit: `{artifact.source_commit}`",
                $"- Master seed: `{artifact.master_seed}`",
                $"- Snapshot count: `{artifact.snapshot_count}`",
                $"- Day owner: `{artifact.day_owner}`",
                "",
                "## Target bands",
                "",
                "| Measure | Band / rule |",
                "|---|---|"
            };
            foreach (var band in artifact.target_bands)
                md.Add($"| {band.measure} | {band.band} |");

            md.AddRange(new[]
            {
                "",
                "## Trajectory summary",
                "",
                $"- First migration day: `{artifact.first_migration_day}`",
                $"- First degradation transition: `{artifact.first_degradation_transition_day}`",
                $"- Wildlife density min/max: `{F(artifact.wildlife_density_min)} / {F(artifact.wildlife_density_max)}`",
                $"- Notable density delta: `{F(artifact.wildlife_density_delta)}`",
                $"- Encounter multiplier min/max: `{F(artifact.encounter_multiplier_min)} / {F(artifact.encounter_multiplier_max)}`",
                $"- Canned-food scarcity min/max: `{F(artifact.canned_food_scarcity_min)} / {F(artifact.canned_food_scarcity_max)}`",
                $"- Surfaced events: briefing `{artifact.briefing_event_count}`, journal `{artifact.journal_event_count}`, radio `{artifact.radio_event_count}`",
                $"- Trapping density reads: `{artifact.trapping_density_reads}`; encounter multiplier reads: `{artifact.encounter_multiplier_reads}`",
                $"- Dead seeds: `{artifact.dead_seed_count}`",
                "",
                "## Determinism and persistence",
                "",
                $"- Same-seed byte equality: `{artifact.same_seed_byte_equal}`",
                $"- Midpoint save/load byte equality: `{artifact.midpoint_save_load_byte_equal}`",
                $"- Different-seed divergence: `{artifact.different_seed_diverged}`",
                $"- Duplicate narrative events after restore: `{artifact.duplicate_narrative_events_after_restore}`",
                "",
                "## Influence map",
                "",
                "| Producer | Value | Consumer | Read cadence | Player-visible consequence |",
                "|---|---|---|---|---|"
            });
            foreach (var row in artifact.influence_map)
                md.Add($"| {row.producer} | {row.value} | {row.consumer} | {row.read_cadence} | {row.player_visible_consequence} |");

            md.AddRange(new[] { "", "## Checks", "" });
            foreach (var check in artifact.checks)
                md.Add($"- {(check.passed ? "PASS" : "FAIL")}: {check.id}{(string.IsNullOrEmpty(check.evidence) ? string.Empty : $" — {check.evidence}")}");
            md.AddRange(new[] { "", "## Tuning decision", "", artifact.tuning_decision });

            File.WriteAllText(
                Path.Combine(artifactDir, "world-playtest-30d.md"),
                string.Join("\n", md) + "\n",
                new System.Text.UTF8Encoding(false));
        }

        private static string F(float value) => value.ToString("0.###", CultureInfo.InvariantCulture);

        [Serializable]
        private sealed class WorldPlaytestCheck
        {
            public string id = string.Empty;
            public bool passed;
            public string evidence = string.Empty;
        }

        [Serializable]
        private sealed class WorldPlaytestArtifact
        {
            public int schema_version = 1;
            public string source_commit = string.Empty;
            public string day_owner = "world_evolution";
            public int master_seed;
            public int snapshot_count;
            public List<WorldPlaytestBand> target_bands = new List<WorldPlaytestBand>();
            public int first_migration_day = -1;
            public int first_degradation_transition_day = -1;
            public float wildlife_density_min;
            public float wildlife_density_max;
            public float wildlife_density_delta;
            public float encounter_multiplier_min;
            public float encounter_multiplier_max;
            public float canned_food_scarcity_min;
            public float canned_food_scarcity_max;
            public int briefing_event_count;
            public int journal_event_count;
            public int radio_event_count;
            public int trapping_density_reads;
            public int encounter_multiplier_reads;
            public int dead_seed_count;
            public bool same_seed_byte_equal;
            public bool midpoint_save_load_byte_equal;
            public bool different_seed_diverged;
            public bool duplicate_narrative_events_after_restore;
            public List<WorldPlaytestInfluenceRow> influence_map = new List<WorldPlaytestInfluenceRow>();
            public List<WorldPlaytestCheck> checks = new List<WorldPlaytestCheck>();
            public List<WorldPlaytestSnapshot> snapshots = new List<WorldPlaytestSnapshot>();
            public string tuning_decision = string.Empty;
        }

        [Serializable]
        private sealed class WorldPlaytestBand
        {
            public string measure = string.Empty;
            public string band = string.Empty;
        }

        [Serializable]
        private sealed class WorldPlaytestInfluenceRow
        {
            public string producer = string.Empty;
            public string value = string.Empty;
            public string consumer = string.Empty;
            public string read_cadence = string.Empty;
            public string player_visible_consequence = string.Empty;
        }

        [Serializable]
        private sealed class WorldPlaytestSnapshot
        {
            public int day;
            public float global_wildlife_ratio;
            public float total_wildlife_density;
            public List<WorldPlaytestLocation> locations = new List<WorldPlaytestLocation>();
            public List<WorldPlaytestWildlife> wildlife = new List<WorldPlaytestWildlife>();
            public WorldPlaytestEconomy economy = new WorldPlaytestEconomy();
            public List<WorldPlaytestSurfaceEvent> surfaced_events = new List<WorldPlaytestSurfaceEvent>();
        }

        [Serializable]
        private sealed class WorldPlaytestLocation
        {
            public string location_id = string.Empty;
            public string owner = string.Empty;
            public string degradation_tier = string.Empty;
            public float contamination;
            public float loot_depletion;
            public float encounter_multiplier;
        }

        [Serializable]
        private sealed class WorldPlaytestWildlife
        {
            public string pack_id = string.Empty;
            public string species_id = string.Empty;
            public string region_id = string.Empty;
            public int density;
            public float starvation;
            public bool rabid;
        }

        [Serializable]
        private sealed class WorldPlaytestEconomy
        {
            public float canned_food_scarcity_proxy;
            public List<WorldPlaytestGood> scarcity_goods = new List<WorldPlaytestGood>();
        }

        [Serializable]
        private sealed class WorldPlaytestGood
        {
            public string item_id = string.Empty;
            public float demand_multiplier;
        }

        [Serializable]
        private sealed class WorldPlaytestSurfaceEvent
        {
            public string channel = string.Empty;
            public string kind = string.Empty;
            public string primary_id = string.Empty;
            public string secondary_id = string.Empty;
            public float numeric;
        }

        private sealed class WorldPlaytestSortieOutcome
        {
            public string expedition_id = string.Empty;
            public string location_id = string.Empty;
            public int phase;
        }

        [Serializable]
        private sealed class WorldPlaytestSave
        {
            public int schema_version = 1;
            public WorldWeatherState weather = new WorldWeatherState();
            public LocationEvolutionSaveState locations = new LocationEvolutionSaveState();
            public WildlifeSaveState wildlife = new WildlifeSaveState();
            public LandmarkSaveState landmarks = new LandmarkSaveState();
            public MarketState market = new MarketState();
            public WildlifeTrappingState trapping = new WildlifeTrappingState();
            public List<ExpeditionState> expeditions = new List<ExpeditionState>();
            public int expedition_completed_count;
            public CampaignDaySave coordinator = new CampaignDaySave();
            public List<string> previous_sectors = new List<string>();
            public List<string> surfaced_event_keys = new List<string>();
            public int last_trapping_catch;
        }

        private static class WorldPlaytestJson
        {
            private static readonly SystemTextJsonSerializer Serializer = new SystemTextJsonSerializer();
            public static string Serialize<T>(T value) => Serializer.Serialize(value);
            public static T Deserialize<T>(string json) where T : class => Serializer.Deserialize<T>(json)
                ?? throw new InvalidOperationException("world playtest save decoded to null");
        }

        private sealed class WorldPlaytestRun
        {
            private readonly int _seed;
            private readonly EvolvingWorldSeedContainer _seeds;
            private readonly Dictionary<string, string> _previousSectors = new Dictionary<string, string>(StringComparer.Ordinal);
            private readonly HashSet<string> _surfacedEventKeys = new HashSet<string>(StringComparer.Ordinal);
            private readonly Dictionary<string, string> _initialLocationTiers = new Dictionary<string, string>(StringComparer.Ordinal);
            private readonly Dictionary<string, string> _initialLandmarkTiers = new Dictionary<string, string>(StringComparer.Ordinal);
            private readonly List<WorldPlaytestSortieOutcome> _sortieOutcomes = new List<WorldPlaytestSortieOutcome>();
            private int _lastTrappingCatch;

            public WeatherSystem Weather { get; }
            public LocationEvolutionSystem Locations { get; }
            public WildlifeMigrationSystem Wildlife { get; }
            public LandmarkDegradationSystem Landmarks { get; }
            public MarketSystem Market { get; }
            public WildlifeTrappingSystem Trapping { get; }
            public ExpeditionSystem Expeditions { get; }
            public CampaignDayCoordinator Coordinator { get; }
            public List<WorldPlaytestSnapshot> Snapshots { get; } = new List<WorldPlaytestSnapshot>();
            public int Migrations { get; private set; }
            public int DegradationTransitions { get; private set; }
            public int TrappingDayTicks { get; private set; }
            public int TrappingDensityReads { get; private set; }
            public int EncounterMultiplierReads { get; private set; }
            public int EncounterEvaluationTicks { get; private set; }
            public int SurfacedBriefingEvents { get; private set; }
            public int SurfacedJournalEvents { get; private set; }
            public int SurfacedRadioEvents { get; private set; }
            public bool NoDuplicateNarrativeEvents { get; private set; } = true;
            public int DeadSeedCount { get; private set; }

            private WorldPlaytestRun(string dataDirectory, int seed, EvolvingWorldSeedContainer seeds,
                WeatherSystem weather, LocationEvolutionSystem locations, WildlifeMigrationSystem wildlife,
                LandmarkDegradationSystem landmarks, MarketSystem market, WildlifeTrappingSystem trapping,
                ExpeditionSystem expeditions)
            {
                _seed = seed;
                _seeds = seeds;
                Weather = weather;
                Locations = locations;
                Wildlife = wildlife;
                Landmarks = landmarks;
                Market = market;
                Trapping = trapping;
                Expeditions = expeditions;
                Coordinator = new CampaignDayCoordinator(
                    new CampaignCalendar(1),
                    new CampaignRngManager(seed));

                foreach (var record in Locations.State.mutations.Where(r => r != null).OrderBy(r => r.locationId, StringComparer.Ordinal))
                    _initialLocationTiers[record.locationId] = LocationTier(record);
                foreach (var landmark in Landmarks.State.landmarks.Where(l => l != null).OrderBy(l => l.landmarkId, StringComparer.Ordinal))
                    _initialLandmarkTiers[landmark.landmarkId] = LandmarkTier(landmark);

                Expeditions.SetEncounterChanceMultiplier(locationId =>
                {
                    EncounterMultiplierReads++;
                    return EncounterMultiplierFor(locationId);
                });
                Expeditions.OnExpeditionCompleted += state => _sortieOutcomes.Add(new WorldPlaytestSortieOutcome
                {
                    expedition_id = state.expeditionId,
                    location_id = state.locationId,
                    phase = (int)ExpeditionPhase.Completed
                });
                Expeditions.OnExpeditionFailed += (state, _) => _sortieOutcomes.Add(new WorldPlaytestSortieOutcome
                {
                    expedition_id = state.expeditionId,
                    location_id = state.locationId,
                    phase = (int)ExpeditionPhase.Failed
                });

                Coordinator.Register("weather", new WeatherOwner(this), phase: 1);
                Coordinator.Register("wildlife_trapping", new TrappingOwner(this), phase: 2);
                Coordinator.Register("expeditions_probe", new ExpeditionOwner(this), phase: 4);
                Coordinator.Register("world_evolution", new WorldOwner(this), phase: 4);
                Coordinator.OnDayAdvanced += OnDayAdvanced;
            }

            public static WorldPlaytestRun Create(string dataDirectory, int seed)
            {
                var fileIO = new FileSystemIO();
                var json = new SystemTextJsonSerializer();
                var seeds = EvolvingWorldCatalogLoader.Load(dataDirectory, fileIO, json)
                    ?? throw new InvalidOperationException("world_evolution_seeds.json could not be loaded");
                var profile = WeatherProfileLoader.Load(dataDirectory, fileIO, json)
                    ?? throw new InvalidOperationException("weather_seasons.json could not be loaded");

                var weather = new WeatherSystem();
                weather.BindProfile(profile, seed);
                var locations = new LocationEvolutionSystem(new SeededRng(DeriveSeed(seed, 0)));
                var wildlife = new WildlifeMigrationSystem(new SeededRng(DeriveSeed(seed, 1)));
                wildlife.BindSeasonProfile(profile);
                var landmarks = new LandmarkDegradationSystem(new SeededRng(DeriveSeed(seed, 2)));
                EvolvingWorldSeeder.Seed(locations, wildlife, landmarks, seeds);

                var market = new MarketSystem();
                var goodsLoad = GoodsCatalogLoader.Load(dataDirectory, fileIO, json);
                if (goodsLoad.Errors.Count == 0)
                    market.BindCatalog(GoodsCatalogLoader.ToCatalog(goodsLoad));

                var trapping = new WildlifeTrappingSystem(new SeededRng(DeriveSeed(seed, 3)));
                trapping.SetHunterSkill(65f);
                trapping.SetTrap("shelter_line", "bait_scrap_meat", "hunter_a");

                var expeditions = new ExpeditionSystem();
                string expeditionTarget = seeds.location_seeds
                    .Where(s => s != null && !string.IsNullOrEmpty(s.location_id))
                    .OrderBy(s => s.location_id, StringComparer.Ordinal)
                    .First().location_id;
                expeditions.Start(new ExpeditionDefinition
                {
                    id = expeditionTarget,
                    displayName = "30-day world probe",
                    distanceTicks = 8,
                    dangerLevel = 2,
                    encounterChancePerTick = 0.2f,
                    baseStaminaDrainPerHour = 1f
                }, "world_probe_survivor", 1, ExpeditionStance.Speed, startingStamina: 100f);

                return new WorldPlaytestRun(dataDirectory, seed, seeds, weather, locations, wildlife,
                    landmarks, market, trapping, expeditions);
            }

            public void AdvanceThrough(int firstDay, int lastDay)
            {
                for (int day = firstDay; day <= lastDay; day++)
                {
                    var result = Coordinator.Advance(day);
                    if (result == null || result.HasFailures)
                    {
                        string detail = result == null
                            ? "coordinator rejected day"
                            : string.Join("; ", result.FailedReports.Select(r => r.FailureMessage));
                        throw new InvalidOperationException($"world playtest day {day} failed: {detail}");
                    }
                }
            }

            public WorldPlaytestSave CaptureSave()
            {
                return new WorldPlaytestSave
                {
                    weather = Weather.CaptureState(),
                    locations = Locations.CaptureState(),
                    wildlife = Wildlife.CaptureState(),
                    landmarks = Landmarks.CaptureState(),
                    market = Market.CaptureState(),
                    trapping = Trapping.CaptureState(),
                    expeditions = Expeditions.CaptureState(),
                    expedition_completed_count = Expeditions.CompletedCount,
                    coordinator = Coordinator.CaptureState(),
                    previous_sectors = _previousSectors.OrderBy(p => p.Key, StringComparer.Ordinal).Select(p => p.Key + "\u001f" + p.Value).ToList(),
                    surfaced_event_keys = _surfacedEventKeys.OrderBy(k => k, StringComparer.Ordinal).ToList(),
                    last_trapping_catch = _lastTrappingCatch
                };
            }

            public void RestoreSave(WorldPlaytestSave save)
            {
                if (save == null) throw new ArgumentNullException(nameof(save));
                Weather.RestoreState(save.weather);
                Locations.RestoreState(save.locations);
                Wildlife.RestoreState(save.wildlife);
                Landmarks.RestoreState(save.landmarks);
                Market.RestoreState(save.market);
                Trapping.RestoreState(save.trapping);
                Expeditions.RestoreState(save.expeditions);
                Expeditions.RestoreCompletedCount(save.expedition_completed_count);
                Coordinator.RestoreState(save.coordinator);
                _previousSectors.Clear();
                foreach (string entry in save.previous_sectors ?? new List<string>())
                {
                    int split = entry.IndexOf('\u001f');
                    if (split > 0) _previousSectors[entry.Substring(0, split)] = entry.Substring(split + 1);
                }
                _surfacedEventKeys.Clear();
                foreach (string key in save.surfaced_event_keys ?? new List<string>())
                    if (!string.IsNullOrEmpty(key)) _surfacedEventKeys.Add(key);
                _lastTrappingCatch = save.last_trapping_catch;
                ValidateDeadSeeds();
            }

            public float DensityDeltaMagnitude
            {
                get
                {
                    if (Snapshots.Count < 2) return 0f;
                    return Math.Abs(Snapshots[Snapshots.Count - 1].total_wildlife_density - Snapshots[0].total_wildlife_density);
                }
            }

            public float EncounterMultiplierMovement => Snapshots.Count == 0
                ? 0f
                : Snapshots.SelectMany(s => s.locations).Select(l => l.encounter_multiplier).DefaultIfEmpty(1f).Max()
                  - Snapshots.SelectMany(s => s.locations).Select(l => l.encounter_multiplier).DefaultIfEmpty(1f).Min();

            public float ScarcityMovement => Snapshots.Count < 2
                ? 0f
                : Math.Abs(Snapshots[Snapshots.Count - 1].economy.canned_food_scarcity_proxy
                    - Snapshots[0].economy.canned_food_scarcity_proxy);

            public bool AllSnapshotsSane()
            {
                foreach (var snapshot in Snapshots)
                {
                    if (snapshot.global_wildlife_ratio < 0f || snapshot.global_wildlife_ratio > 2f
                        || snapshot.total_wildlife_density < 0f
                        || !Finite(snapshot.global_wildlife_ratio) || !Finite(snapshot.total_wildlife_density)
                        || snapshot.economy.canned_food_scarcity_proxy < MarketSystem.MinDemandMult
                        || snapshot.economy.canned_food_scarcity_proxy > MarketSystem.MaxDemandMult)
                        return false;
                    foreach (var location in snapshot.locations)
                    {
                        if (!Finite(location.contamination) || location.contamination < 0f || location.contamination > 1f
                            || !Finite(location.loot_depletion) || location.loot_depletion < 0f || location.loot_depletion > 1f
                            || !Finite(location.encounter_multiplier) || location.encounter_multiplier < 0.5f
                            || location.encounter_multiplier > 2f)
                            return false;
                    }
                    foreach (var wildlife in snapshot.wildlife)
                        if (wildlife.density < 0 || !Finite(wildlife.starvation) || wildlife.starvation < 0f || wildlife.starvation > 1f)
                            return false;
                }
                return true;
            }

            public bool NoDuplicateSnapshotRecords()
            {
                return Snapshots.All(s => s.locations.Select(l => l.location_id).Distinct(StringComparer.Ordinal).Count() == s.locations.Count
                    && s.wildlife.Select(w => w.pack_id).Distinct(StringComparer.Ordinal).Count() == s.wildlife.Count);
            }

            public bool RuinedStatesAreSticky()
            {
                var ruined = new HashSet<string>(StringComparer.Ordinal);
                foreach (var snapshot in Snapshots.OrderBy(s => s.day))
                {
                    foreach (var location in snapshot.locations)
                    {
                        if (ruined.Contains(location.location_id) && location.degradation_tier != "ruined")
                            return false;
                        if (location.degradation_tier == "ruined")
                            ruined.Add(location.location_id);
                    }
                }
                return true;
            }

            public WorldPlaytestArtifact BuildArtifact(string dataDirectory, List<WorldPlaytestCheck> checks,
                bool sameSeedEqual, bool midpointEqual, bool differentSeedDiverged)
            {
                var allLocations = Snapshots.SelectMany(s => s.locations).ToList();
                var allWildlife = Snapshots.SelectMany(s => s.wildlife).ToList();
                var allEvents = Snapshots.SelectMany(s => s.surfaced_events).ToList();
                int firstMigration = Snapshots.Where(s => s.surfaced_events.Any(e => e.channel == "radio"))
                    .Select(s => s.day).DefaultIfEmpty(-1).First();
                int firstDegradation = Snapshots.Where(s => s.locations.Any(l =>
                        _initialLocationTiers.TryGetValue(l.location_id, out var initial) && initial != l.degradation_tier))
                    .Select(s => s.day).DefaultIfEmpty(-1).First();
                var artifact = new WorldPlaytestArtifact
                {
                    source_commit = SourceCommit(),
                    master_seed = _seed,
                    snapshot_count = Snapshots.Count,
                    first_migration_day = firstMigration,
                    first_degradation_transition_day = firstDegradation,
                    wildlife_density_min = allWildlife.Count == 0 ? 0f : allWildlife.Min(w => w.density),
                    wildlife_density_max = allWildlife.Count == 0 ? 0f : allWildlife.Max(w => w.density),
                    wildlife_density_delta = DensityDeltaMagnitude,
                    encounter_multiplier_min = allLocations.Count == 0 ? 1f : allLocations.Min(l => l.encounter_multiplier),
                    encounter_multiplier_max = allLocations.Count == 0 ? 1f : allLocations.Max(l => l.encounter_multiplier),
                    canned_food_scarcity_min = Snapshots.Count == 0 ? 1f : Snapshots.Min(s => s.economy.canned_food_scarcity_proxy),
                    canned_food_scarcity_max = Snapshots.Count == 0 ? 1f : Snapshots.Max(s => s.economy.canned_food_scarcity_proxy),
                    briefing_event_count = allEvents.Count(e => e.channel == "briefing"),
                    journal_event_count = allEvents.Count(e => e.channel == "journal"),
                    radio_event_count = allEvents.Count(e => e.channel == "radio"),
                    trapping_density_reads = TrappingDensityReads,
                    encounter_multiplier_reads = EncounterMultiplierReads,
                    dead_seed_count = DeadSeedCount,
                    same_seed_byte_equal = sameSeedEqual,
                    midpoint_save_load_byte_equal = midpointEqual,
                    different_seed_diverged = differentSeedDiverged,
                    duplicate_narrative_events_after_restore = !NoDuplicateNarrativeEvents,
                    checks = new List<WorldPlaytestCheck>(checks),
                    snapshots = new List<WorldPlaytestSnapshot>(Snapshots),
                    tuning_decision = "No tuning applied. The playtest exercises the named AshfallMmFor / ContaminationHazardGain knobs as-is; change either only in a separate trajectory-pinning commit if future evidence shows inert or runaway behavior."
                };
                artifact.target_bands.Add(new WorldPlaytestBand { measure = "global wildlife ratio", band = "0.00 to 2.00; day-30 must remain above 0 (not silent)" });
                artifact.target_bands.Add(new WorldPlaytestBand { measure = "location contamination / loot depletion", band = "0.00 to 1.00; no NaN/Infinity or negative values" });
                artifact.target_bands.Add(new WorldPlaytestBand { measure = "expedition encounter multiplier", band = "0.50 to 2.00; production component remains bounded" });
                artifact.target_bands.Add(new WorldPlaytestBand { measure = "market scarcity proxy", band = "0.25 to 4.00; canonical MarketSystem clamp" });
                artifact.influence_map.Add(new WorldPlaytestInfluenceRow { producer = "wildlife migration", value = "sector pack population / global ratio", consumer = "wildlife trapping", read_cadence = "once per day, phase 2", player_visible_consequence = "prey availability and harvest pressure" });
                artifact.influence_map.Add(new WorldPlaytestInfluenceRow { producer = "location evolution", value = "threats, contamination, degradation tier", consumer = "ExpeditionSystem encounter multiplier", read_cadence = "once per active expedition tick", player_visible_consequence = "route pressure" });
                artifact.influence_map.Add(new WorldPlaytestInfluenceRow { producer = "wildlife ratio", value = "ratio-based demand delta", consumer = "MarketSystem", read_cadence = "once after world tick", player_visible_consequence = "canned-food scarcity" });
                artifact.influence_map.Add(new WorldPlaytestInfluenceRow { producer = "world evolution events", value = "migration / expedition / degradation events", consumer = "briefing, journal, radio projection", read_cadence = "daily snapshot", player_visible_consequence = "surface feedback" });
                return artifact;
            }

            private void OnDayAdvanced(DayAdvancedEventArgs args)
            {
                var events = args.OwnerReports.SelectMany(r => r.Events ?? Array.Empty<DayStateChangeEvent>())
                    .Where(e => e != null)
                    .Select(ToSurfaceEvent)
                    .Where(e => e != null)
                    .Select(e => e!)
                    .OrderBy(e => e.channel, StringComparer.Ordinal)
                    .ThenBy(e => e.kind, StringComparer.Ordinal)
                    .ThenBy(e => e.primary_id, StringComparer.Ordinal)
                    .ThenBy(e => e.secondary_id, StringComparer.Ordinal)
                    .ToList();
                foreach (var eventRow in events)
                {
                    if (eventRow.channel == "briefing") SurfacedBriefingEvents++;
                    else if (eventRow.channel == "journal") SurfacedJournalEvents++;
                    else if (eventRow.channel == "radio") SurfacedRadioEvents++;
                }
                Snapshots.Add(BuildSnapshot(args.Day, events));
            }

            private WorldPlaytestSnapshot BuildSnapshot(int day, List<WorldPlaytestSurfaceEvent> events)
            {
                var snapshot = new WorldPlaytestSnapshot
                {
                    day = day,
                    global_wildlife_ratio = Wildlife.GetGlobalPopulationRatio(),
                    total_wildlife_density = Wildlife.State.packs.Where(p => p != null).Sum(p => p.population),
                    surfaced_events = events
                };
                foreach (var record in Locations.State.mutations.Where(r => r != null).OrderBy(r => r.locationId, StringComparer.Ordinal))
                {
                    snapshot.locations.Add(new WorldPlaytestLocation
                    {
                        location_id = record.locationId,
                        owner = record.currentOwner,
                        degradation_tier = LocationTier(record),
                        contamination = record.contaminationLevel,
                        loot_depletion = record.lootDepletionFactor,
                        encounter_multiplier = EncounterMultiplierFor(record.locationId)
                    });
                }
                foreach (var pack in Wildlife.State.packs.Where(p => p != null).OrderBy(p => p.packId, StringComparer.Ordinal))
                {
                    snapshot.wildlife.Add(new WorldPlaytestWildlife
                    {
                        pack_id = pack.packId,
                        species_id = pack.speciesId,
                        region_id = pack.currentSectorId,
                        density = pack.population,
                        starvation = pack.starvationLevel,
                        rabid = pack.isRabid
                    });
                }
                snapshot.economy.canned_food_scarcity_proxy = Market.GetDemandMultiplier("canned_food");
                foreach (string itemId in EvolvingWorldSeeder.ScarcityGoods(_seeds).OrderBy(id => id, StringComparer.Ordinal))
                    snapshot.economy.scarcity_goods.Add(new WorldPlaytestGood
                    {
                        item_id = itemId,
                        demand_multiplier = Market.GetDemandMultiplier(itemId)
                    });
                return snapshot;
            }

            private void ValidateDeadSeeds()
            {
                var knownSectors = new HashSet<string>(_seeds.sectors.Where(s => s != null).Select(s => s.sector_id), StringComparer.Ordinal);
                var packIds = new HashSet<string>(Wildlife.State.packs.Where(p => p != null).Select(p => p.packId), StringComparer.Ordinal);
                var landmarkIds = new HashSet<string>(Landmarks.State.landmarks.Where(l => l != null).Select(l => l.landmarkId), StringComparer.Ordinal);
                var observedLocationIds = new HashSet<string>(
                    Snapshots.SelectMany(s => s.locations).Select(l => l.location_id), StringComparer.Ordinal);
                var observedPackIds = new HashSet<string>(
                    Snapshots.SelectMany(s => s.wildlife).Select(w => w.pack_id), StringComparer.Ordinal);
                DeadSeedCount = 0;
                foreach (var sector in _seeds.sectors.Where(s => s != null))
                    if (!knownSectors.Contains(sector.sector_id) || !Wildlife.TryGetNeighbors(sector.sector_id, out _)) DeadSeedCount++;
                foreach (var pack in _seeds.packs.Where(p => p != null))
                    if (!packIds.Contains(pack.pack_id) || !knownSectors.Contains(pack.sector_id)
                        || (Snapshots.Count > 0 && !observedPackIds.Contains(pack.pack_id))) DeadSeedCount++;
                foreach (var landmark in _seeds.landmarks.Where(l => l != null))
                    if (!landmarkIds.Contains(landmark.landmark_id)) DeadSeedCount++;
                foreach (var location in _seeds.location_seeds.Where(l => l != null))
                {
                    var record = Locations.TryGetRecord(location.location_id);
                    if (record == null || Locations.State.lastEvolutionDay < 0
                        || (Snapshots.Count > 0 && !observedLocationIds.Contains(location.location_id))) DeadSeedCount++;
                }
                if (Wildlife.State.lastMigrationDay < 0 || Landmarks.State.lastDegradationDay < 0)
                    DeadSeedCount++;
            }

            private static WorldPlaytestSurfaceEvent? ToSurfaceEvent(DayStateChangeEvent source)
            {
                string channel = source.Kind switch
                {
                    "radio_intercept" => "radio",
                    "expedition_milestone" => "journal",
                    "hazard_warning" => "journal",
                    _ => "briefing"
                };
                return new WorldPlaytestSurfaceEvent
                {
                    channel = channel,
                    kind = source.Kind ?? string.Empty,
                    primary_id = source.PrimaryId ?? string.Empty,
                    secondary_id = source.SecondaryId ?? string.Empty,
                    numeric = source.Numeric
                };
            }

            private sealed class WeatherOwner : IDayAdvanceOwner
            {
                private readonly WorldPlaytestRun _run;
                public WeatherOwner(WorldPlaytestRun run) => _run = run;
                public void CapturePreDaySnapshot(int day) { }
                public void TickDay(int day, List<DayStateChangeEvent> events)
                {
                    _run.Weather.Tick(24f);
                    events.Add(new DayStateChangeEvent("weather_ticked", "weather", _run.Weather.Current.ToString(), null, _run.Weather.OutdoorRadModifier));
                }
            }

            private sealed class TrappingOwner : IDayAdvanceOwner
            {
                private readonly WorldPlaytestRun _run;
                public TrappingOwner(WorldPlaytestRun run) => _run = run;
                public void CapturePreDaySnapshot(int day) { }
                public void TickDay(int day, List<DayStateChangeEvent> events)
                {
                    string sector = EvolvingWorldSeeder.ShelterSectorId(_run._seeds);
                    int population = _run.Wildlife.GetSectorPackPopulation(sector);
                    float density = Math.Clamp(0.5f + population * 0.1f, 0.4f, 1.5f);
                    _run.TrappingDensityReads++;
                    int caughtBefore = _run.Trapping.State.totalCatch;
                    _run.Trapping.TickDay(day, density);
                    _run.TrappingDayTicks++;
                    int caught = _run.Trapping.State.totalCatch - caughtBefore;
                    if (caught > 0)
                    {
                        int removed = _run.Wildlife.ApplyHarvestPressure(sector, caught);
                        events.Add(new DayStateChangeEvent("trapping_harvest", "wildlife_trapping", sector, null, removed));
                    }
                    if (_run.Trapping.State.trapSites.Any(s => s != null && s.hasCatch) && day < WorldPlaytestDays)
                        _run.Trapping.SetTrap("shelter_line", "bait_scrap_meat", "hunter_a");
                    _run._lastTrappingCatch = _run.Trapping.State.totalCatch;
                    events.Add(new DayStateChangeEvent("trapping_ticked", "wildlife_trapping", sector, null, density));
                }
            }

            private sealed class ExpeditionOwner : IDayAdvanceOwner
            {
                private readonly WorldPlaytestRun _run;
                public ExpeditionOwner(WorldPlaytestRun run) => _run = run;
                public void CapturePreDaySnapshot(int day) { }
                public void TickDay(int day, List<DayStateChangeEvent> events)
                {
                    int activeBefore = _run.Expeditions.ActiveCount;
                    _run.EncounterEvaluationTicks += activeBefore;
                    _run.Expeditions.TickHours(1f, Fork(_run._seed, day, 3));
                    events.Add(new DayStateChangeEvent("expedition_ticked", "expeditions_probe", null, null, activeBefore));
                }
            }

            private sealed class WorldOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
            {
                private readonly WorldPlaytestRun _run;
                private LocationEvolutionSaveState? _locations;
                private WildlifeSaveState? _wildlife;
                private LandmarkSaveState? _landmarks;
                public WorldOwner(WorldPlaytestRun run) => _run = run;
                public void CapturePreDaySnapshot(int day)
                {
                    _locations = _run.Locations.CaptureState();
                    _wildlife = _run.Wildlife.CaptureState();
                    _landmarks = _run.Landmarks.CaptureState();
                }
                public void RestorePreDaySnapshot(int day)
                {
                    if (_locations != null) _run.Locations.RestoreState(_locations);
                    if (_wildlife != null) _run.Wildlife.RestoreState(_wildlife);
                    if (_landmarks != null) _run.Landmarks.RestoreState(_landmarks);
                }
                public void TickDay(int day, List<DayStateChangeEvent> events)
                {
                    var beforeSectors = _run.Wildlife.State.packs.Where(p => p != null)
                        .ToDictionary(p => p.packId, p => p.currentSectorId, StringComparer.Ordinal);
                    var beforeTiers = _run.Locations.State.mutations.Where(r => r != null)
                        .ToDictionary(r => r.locationId, LocationTier, StringComparer.Ordinal);
                    _run.Landmarks.TickDay(day, AshfallMmFor(_run.Weather.Current));
                    bool hazard = _run.Weather.Current == WeatherKind.FalloutStorm || _run.Weather.Current == WeatherKind.BlackRain;
                    _run.Locations.TickDay(day, new LocationEvolutionInputs(_run.Weather.OutdoorRadModifier, hazard), Fork(_run._seed, day, 0));
                    _run.Wildlife.TickDay(day, Fork(_run._seed, day, 1));

                    foreach (var pack in _run.Wildlife.State.packs.Where(p => p != null).OrderBy(p => p.packId, StringComparer.Ordinal))
                    {
                        if (!beforeSectors.TryGetValue(pack.packId, out var before) || before == pack.currentSectorId) continue;
                        string key = $"migration:{pack.packId}:{day}";
                        if (_run._surfacedEventKeys.Add(key))
                        {
                            _run.Migrations++;
                            events.Add(new DayStateChangeEvent("radio_intercept", "world_evolution", pack.packId,
                                before + "->" + pack.currentSectorId, pack.population));
                        }
                    }

                    foreach (var outcome in _run._sortieOutcomes
                        .OrderBy(o => o.expedition_id, StringComparer.Ordinal)
                        .ThenBy(o => o.phase)
                        .ToList())
                    {
                        string key = $"expedition:{outcome.expedition_id}:{outcome.phase}";
                        if (!_run._surfacedEventKeys.Add(key))
                        {
                            _run.NoDuplicateNarrativeEvents = false;
                            continue;
                        }
                        if (outcome.phase == (int)ExpeditionPhase.Completed)
                        {
                            _run.Locations.MarkCleared(outcome.location_id, day);
                            events.Add(new DayStateChangeEvent("expedition_milestone", "world_evolution", outcome.location_id, "cleared", 1f));
                        }
                        else
                        {
                            _run.Locations.MarkVisited(outcome.location_id, day);
                            _run.Locations.AddThreat(outcome.location_id, LocationEvolutionSystem.ThreatSquatters);
                            events.Add(new DayStateChangeEvent("hazard_warning", "world_evolution", outcome.location_id, "failed", 1f));
                        }
                    }
                    _run._sortieOutcomes.Clear();

                    foreach (var record in _run.Locations.State.mutations.Where(r => r != null).OrderBy(r => r.locationId, StringComparer.Ordinal))
                    {
                        string currentTier = LocationTier(record);
                        if (!beforeTiers.TryGetValue(record.locationId, out var beforeTier) || currentTier == beforeTier) continue;
                        _run.DegradationTransitions++;
                        events.Add(new DayStateChangeEvent("hazard_warning", "world_evolution", record.locationId,
                            beforeTier + "->" + currentTier, record.lootDepletionFactor));
                    }

                    float ratio = _run.Wildlife.GetGlobalPopulationRatio();
                    foreach (string itemId in EvolvingWorldSeeder.ScarcityGoods(_run._seeds).OrderBy(id => id, StringComparer.Ordinal))
                    {
                        float delta = ratio < 0.6f ? 0.02f : ratio < 0.85f ? 0.005f : ratio > 1.2f ? -0.005f : 0f;
                        if (Math.Abs(delta) > 0f) _run.Market.AdjustDemand(itemId, delta);
                    }
                    _run.Market.TickDay(day, Fork(_run._seed, day, 4));

                    foreach (var landmark in _run.Landmarks.State.landmarks.Where(l => l != null).OrderBy(l => l.landmarkId, StringComparer.Ordinal))
                    {
                        string currentTier = LandmarkTier(landmark);
                        if (_run._initialLandmarkTiers.TryGetValue(landmark.landmarkId, out var initial) && currentTier != initial
                            && landmark.structuralIntegrity <= 0.5f)
                            events.Add(new DayStateChangeEvent("hazard_warning", "world_evolution", landmark.landmarkId, initial + "->" + currentTier, landmark.structuralIntegrity));
                    }
                    _run._previousSectors.Clear();
                    foreach (var pack in _run.Wildlife.State.packs.Where(p => p != null).OrderBy(p => p.packId, StringComparer.Ordinal))
                        _run._previousSectors[pack.packId] = pack.currentSectorId;
                    events.Add(new DayStateChangeEvent("world_evolution_ticked", "world_evolution", null, null, day));
                    _run.ValidateDeadSeeds();
                }
            }

            private float EncounterMultiplierFor(string locationId)
            {
                float multiplier = 1f;
                float ratio = Wildlife.GetGlobalPopulationRatio();
                if (ratio < 0.4f) multiplier *= 1.15f;
                else if (ratio > 1.2f) multiplier *= 0.95f;
                var record = Locations.TryGetRecord(locationId);
                if (record != null)
                {
                    multiplier *= 1f + Math.Min(0.45f, record.activeThreats.Count * 0.15f);
                    if (record.contaminationLevel > 0.6f) multiplier *= 1.1f;
                }
                return multiplier;
            }

            private static string LocationTier(LocationMutationRecord record)
            {
                if (record.isRuined) return "ruined";
                if (record.lootDepletionFactor >= 0.25f) return "depleted";
                if (record.lootDepletionFactor > 0f) return "pressured";
                return "stable";
            }

            private static string LandmarkTier(LandmarkStatusRecord landmark)
            {
                if (landmark.isCollapsed) return "collapsed";
                if (landmark.structuralIntegrity < 40f) return "failing";
                if (landmark.structuralIntegrity < 70f) return "compromised";
                if (landmark.structuralIntegrity < 90f) return "weathered";
                return "intact";
            }

            private static bool Finite(float value) => !float.IsNaN(value) && !float.IsInfinity(value);

            private static int DeriveSeed(int master, int stream)
            {
                return CampaignRngStream.DeriveSeed(master, CampaignStreamIds.WorldEvolution, 1, 0, stream);
            }

            private static ISeededRng Fork(int master, int day, int action)
            {
                int seed = CampaignRngStream.DeriveSeed(master, CampaignStreamIds.WorldEvolution, 1, day, action);
                return new SeededRng(seed == 0 ? 1986 : seed);
            }

        }

        private static string SourceCommit()
        {
            try
            {
                string git = Path.Combine(CatalogPath.ResolveRepoRoot(), ".git");
                string headPath = Directory.Exists(git) ? Path.Combine(git, "HEAD") : git;
                string head = File.ReadAllText(headPath).Trim();
                if (head.StartsWith("ref: ", StringComparison.Ordinal))
                {
                    string refPath = Path.Combine(git, head.Substring(5).Replace('/', Path.DirectorySeparatorChar));
                    if (File.Exists(refPath)) return File.ReadAllText(refPath).Trim();
                }
                return head;
            }
            catch
            {
                return "unknown";
            }
        }

        private static float AshfallMmFor(WeatherKind kind) => kind switch
        {
            WeatherKind.Ashfall => 6f,
            WeatherKind.FalloutStorm => 10f,
            WeatherKind.BlackRain => 14f,
            WeatherKind.BlackSnow => 4f,
            WeatherKind.BloodRain => 8f,
            WeatherKind.GlassStorm => 5f,
            WeatherKind.RadHail => 3f,
            _ => 0f
        };

        private static ISeededRng Fork(int master, int day, int action)
        {
            int seed = CampaignRngStream.DeriveSeed(master, CampaignStreamIds.WorldEvolution, 1, day, action);
            return new SeededRng(seed == 0 ? 1986 : seed);
        }
    }
}
