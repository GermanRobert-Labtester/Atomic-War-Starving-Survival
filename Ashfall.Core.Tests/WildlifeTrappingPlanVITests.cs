// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Ashfall.Core;
using Ashfall.Core.Crafting;
using Ashfall.Core.Economy;
using Ashfall.Core.Inventory;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Plan VI closure tests: complete bycatch state, miss-only narrative
    /// incidents, continuous RNG save positions, and live vendor matching.
    /// </summary>
    public sealed class WildlifeTrappingPlanVITests
    {
        private static string FindDataDir()
        {
            string dir = Directory.GetCurrentDirectory();
            for (int i = 0; i < 10; i++)
            {
                string candidate = Path.Combine(dir, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(candidate)) return candidate;
                dir = Path.GetDirectoryName(dir) ?? dir;
            }
            return "Assets/StreamingAssets/Data";
        }

        private static PreyDefinition Prey(
            string id, float yield, float toxicChance, float diseaseRisk = 0f,
            string diseaseId = "", float contaminationRisk = 0f, float dose = 0f)
        {
            return new PreyDefinition
            {
                speciesId = id,
                displayName = id,
                description = "Test prey.",
                baseYieldKg = yield,
                toxicChance = toxicChance,
                preferredTrapType = "snare",
                diseaseRisk = diseaseRisk,
                diseaseId = diseaseId,
                contaminationRisk = contaminationRisk,
                contaminationDose = dose
            };
        }

        private static void RegisterPrey(WildlifeTrappingSystem system, PreyDefinition definition)
        {
            system.RegisterPreyDefinition(definition);
            system.RegisterQuarry(new QuarrySpecies
            {
                speciesId = definition.speciesId,
                displayName = definition.displayName,
                baseYieldKg = definition.baseYieldKg,
                toxicChance = definition.toxicChance,
                preferredTrapType = definition.preferredTrapType
            });
        }

        private static TrapDefinition Trap(
            string id, string primaryId, float bycatchChance = 0f,
            float incidentChance = 0f, string[]? incidentIds = null)
        {
            return new TrapDefinition
            {
                trap_id = id,
                displayName = id,
                description = "Test trap.",
                trapType = "snare",
                checkIntervalDays = 1,
                durabilityChecks = 10,
                compatiblePrey = new List<string> { primaryId },
                bycatchChance = bycatchChance,
                narrativeIncidentChance = incidentChance,
                narrativeIncidentIds = incidentIds == null
                    ? new List<string>()
                    : new List<string>(incidentIds)
            };
        }

        private sealed class ScriptedRng : ISeededRng
        {
            private readonly Queue<double> _values;
            private readonly double _fallback;

            public ScriptedRng(int seed, IEnumerable<double> values, double fallback = 0.99)
            {
                Seed = seed;
                _values = new Queue<double>(values);
                _fallback = fallback;
            }

            public int Seed { get; }

            public int DrawCount { get; private set; }

            public int Next(int minInclusive, int maxExclusive)
            {
                if (minInclusive >= maxExclusive)
                    throw new ArgumentOutOfRangeException(nameof(maxExclusive));
                DrawCount++;
                return minInclusive;
            }

            public float NextFloat() => (float)NextDouble();

            public double NextDouble()
            {
                DrawCount++;
                return _values.Count > 0 ? _values.Dequeue() : _fallback;
            }
        }

        [Fact]
        public void Bycatch_CommitsIndependentState_AndEmitsTypedEventOnce()
        {
            // Semantic primary/bycatch order:
            // success, primary selection, primary yield, primary toxicity,
            // bycatch chance, bycatch selection, bycatch yield, bycatch
            // toxicity, bycatch disease, bycatch contamination. Primary
            // disease/contamination are zero-risk in this fixture and consume
            // no draw.
            var rng = new ScriptedRng(71, new[]
            {
                0.0, 0.0, 0.5, 1.0, 0.0, 0.0, 0.25, 0.0, 0.0, 0.0
            });
            var system = new WildlifeTrappingSystem(rng);
            var primary = Prey("test_primary", 2f, 0f);
            var bycatch = Prey("test_secondary", 4f, 1f, 1f, "disease_secondary", 1f, 7f);
            RegisterPrey(system, primary);
            RegisterPrey(system, bycatch);
            var trap = Trap("trap_test_bycatch", primary.speciesId, 1f);
            trap.bycatchSpecies.Add(new BycatchCandidate { speciesId = bycatch.speciesId, weight = 1f });
            system.RegisterTrapDefinition(trap);

            int legacyEvents = 0;
            int typedEvents = 0;
            BycatchOccurredEvent? observed = null;
            system.OnBycatchOccurred += (_, _, _, _, _, _) => legacyEvents++;
            system.OnBycatchResolved += value =>
            {
                typedEvents++;
                observed = value;
            };

            Assert.True(system.SetTrap("site_bycatch", "", "hunter", "snare", trap.trap_id, 1, 10).IsSuccess);
            system.TickDay(2);

            var site = Assert.Single(system.State.trapSites);
            Assert.True(site.hasCatch);
            Assert.Equal(primary.speciesId, site.catchSpecies);
            Assert.Equal(bycatch.speciesId, site.bycatchSpecies);
            Assert.Equal(4f * (0.7f + 0.25f * 0.6f), site.bycatchYield, 4);
            Assert.True(site.bycatchToxic);
            Assert.Equal("disease_secondary", site.bycatchDiseaseId);
            Assert.Equal(7f, site.bycatchContaminationDose, 3);
            Assert.Equal(1, legacyEvents);
            Assert.Equal(1, typedEvents);
            Assert.NotNull(observed);
            Assert.Equal("site_bycatch", observed!.siteId);
            Assert.Equal(primary.speciesId, observed.primarySpeciesId);
            Assert.Equal(bycatch.speciesId, observed.bycatchSpeciesId);
            Assert.Equal(site.bycatchYield, observed.bycatchYield, 4);
            Assert.True(observed.bycatchToxic);
            Assert.Equal(10, rng.DrawCount);

            // The catch remains pending until an explicit butcher/clear path;
            // another check cannot emit the same bycatch again.
            system.TickDay(3);
            Assert.Equal(1, legacyEvents);
            Assert.Equal(1, typedEvents);
        }

        [Fact]
        public void Bycatch_ButcheryIncludesSecondaryYield_AndNoSecondMoraleEvent()
        {
            var system = new WildlifeTrappingSystem(new ScriptedRng(72, new[] { 0d, 0d, 0.5d, 1d, 0d, 0d, 0.5d, 1d }));
            var primary = Prey("test_primary", 2f, 0f);
            var bycatch = Prey("test_secondary", 4f, 1f);
            RegisterPrey(system, primary);
            RegisterPrey(system, bycatch);
            var trap = Trap("trap_test_butcher", primary.speciesId, 1f);
            trap.bycatchSpecies.Add(new BycatchCandidate { speciesId = bycatch.speciesId, weight = 1f });
            system.RegisterTrapDefinition(trap);
            Assert.True(system.SetTrap("site_butcher", "", "hunter", "snare", trap.trap_id, 1, 10).IsSuccess);
            system.TickDay(2);

            var result = system.Butcher("site_butcher", "survivor_1");
            Assert.True(result.IsSuccess);
            Assert.Equal(2f + (4f * 1.0f), (float)result.Deltas["yield"], 3);
            Assert.Equal(4f, (float)result.Deltas["bycatchYield"], 3);
            Assert.Equal(0, system.CountPendingEvents(WildlifeTrappingEventKinds.MoralConsequence));
        }

        [Fact]
        public void Bycatch_ChanceMissBrokenAndLegacyPathsRemainSilent()
        {
            var missSystem = new WildlifeTrappingSystem(new ScriptedRng(73, new[] { 0d, 0d, 0.5d, 1d, 0.99d }));
            var primary = Prey("test_primary", 1f, 0f);
            var secondary = Prey("test_secondary", 1f, 0f);
            RegisterPrey(missSystem, primary);
            RegisterPrey(missSystem, secondary);
            var missTrap = Trap("trap_test_miss", primary.speciesId, 0f);
            missTrap.bycatchSpecies.Add(new BycatchCandidate { speciesId = secondary.speciesId, weight = 1f });
            missSystem.RegisterTrapDefinition(missTrap);
            int events = 0;
            missSystem.OnBycatchResolved += _ => events++;
            missSystem.SetTrap("site_miss", "", "hunter", "snare", missTrap.trap_id, 1, 10);
            missSystem.TickDay(2);
            Assert.Equal(0, events);

            var brokenSystem = new WildlifeTrappingSystem(new ScriptedRng(74, Array.Empty<double>()));
            brokenSystem.RegisterTrapDefinition(Trap("trap_test_broken", primary.speciesId, 1f));
            brokenSystem.State.trapSites.Add(new TrapSite
            {
                siteId = "site_broken",
                trapId = "trap_test_broken",
                trapType = "snare",
                setDay = 1,
                checkDay = 1,
                isBroken = true,
                remainingDurability = 0
            });
            brokenSystem.OnBycatchResolved += _ => events++;
            brokenSystem.TickDay(1);
            Assert.Equal(0, events);

            var legacySystem = new WildlifeTrappingSystem(new ScriptedRng(75, new[] { 0.99d }));
            legacySystem.RegisterQuarry(new QuarrySpecies { speciesId = primary.speciesId, preferredTrapType = "snare" });
            legacySystem.OnBycatchResolved += _ => events++;
            legacySystem.State.trapSites.Add(new TrapSite
            {
                siteId = "site_legacy",
                trapId = "",
                trapType = "snare",
                setDay = 1,
                checkDay = 1,
                remainingDurability = -1
            });
            legacySystem.TickDay(1);
            Assert.Equal(0, events);
        }

        [Fact]
        public void Incident_IsMissOnlyPendingAndExactlyOnceAcrossRestore()
        {
            var incidentIds = new[] { TrapNarrativeIncidentIds.BaitStolen };
            var system = new WildlifeTrappingSystem(new ScriptedRng(76, new[] { 0.99d }));
            var primary = Prey("test_primary", 1f, 0f);
            RegisterPrey(system, primary);
            var trap = Trap("trap_test_incident", primary.speciesId, 0f, 1f, incidentIds);
            system.RegisterTrapDefinition(trap);
            system.SetTrap("site_incident", "", "hunter", "snare", trap.trap_id, 1, 10);
            system.TickDay(2);

            var site = Assert.Single(system.State.trapSites);
            Assert.False(site.hasCatch);
            Assert.Equal(TrapNarrativeIncidentIds.BaitStolen, site.pendingNarrativeEvent);
            var pending = Assert.Single(system.GetPendingEvents());
            Assert.Equal(WildlifeTrappingEventKinds.NarrativeIncident, pending.kind);
            Assert.Equal(TrapNarrativeIncidentIds.BaitStolen, pending.payloadId);

            // The next due check cannot overwrite an incident waiting for the
            // host/event authority, even though it misses again.
            system.TickDay(3);
            Assert.Equal(TrapNarrativeIncidentIds.BaitStolen, site.pendingNarrativeEvent);
            Assert.Single(system.GetPendingEvents());

            var serializer = new SystemTextJsonSerializer();
            var restoredState = serializer.Deserialize<WildlifeTrappingState>(serializer.Serialize(system.CaptureState()));
            Assert.NotNull(restoredState);
            var restored = new WildlifeTrappingSystem(new Ashfall.Core.SeededRng(999));
            restored.RegisterPreyDefinition(primary);
            restored.RegisterQuarry(new QuarrySpecies
            {
                speciesId = primary.speciesId,
                preferredTrapType = "snare",
                baseYieldKg = primary.baseYieldKg,
                toxicChance = primary.toxicChance
            });
            restored.RegisterTrapDefinition(trap);
            restored.RestoreState(restoredState!);

            var restoredPending = Assert.Single(restored.GetPendingEvents());
            Assert.Equal(pending.eventId, restoredPending.eventId);
            Assert.True(restored.MarkEventDelivered(restoredPending.eventId));
            Assert.False(restored.MarkEventDelivered(restoredPending.eventId));
            Assert.Empty(restored.GetPendingEvents());
            Assert.Empty(restored.State.trapSites[0].pendingNarrativeEvent);
        }

        [Fact]
        public void Incident_SuccessfulCatchAndBrokenTrapDoNotCreateFoodOrIncident()
        {
            var system = new WildlifeTrappingSystem(new ScriptedRng(77, new[] { 0d, 0d, 0.5d, 1d }));
            var primary = Prey("test_primary", 1f, 0f);
            RegisterPrey(system, primary);
            var trap = Trap("trap_test_success", primary.speciesId, 0f, 1f,
                new[] { TrapNarrativeIncidentIds.HumanBootprints });
            system.RegisterTrapDefinition(trap);
            system.SetTrap("site_success", "", "hunter", "snare", trap.trap_id, 1, 10);
            system.TickDay(2);
            Assert.True(system.State.trapSites[0].hasCatch);
            Assert.Empty(system.State.trapSites[0].pendingNarrativeEvent);
            Assert.DoesNotContain(system.GetPendingEvents(), e => e.kind == WildlifeTrappingEventKinds.NarrativeIncident);
        }

        [Fact]
        public void SaveRestore_PreservesAllWildlifeRngPositions()
        {
            var system = new WildlifeTrappingSystem(new Ashfall.Core.SeededRng(78));
            var primary = Prey("test_primary", 1f, 0f);
            RegisterPrey(system, primary);
            var trap = Trap("trap_test_rng", primary.speciesId);
            system.RegisterTrapDefinition(trap);
            system.SetTrap("site_rng", "", "hunter", "snare", trap.trap_id, 1, 10);
            system.TickDay(2);

            var captured = system.CaptureState();
            Assert.NotEqual(0UL, captured.primaryRngState);
            Assert.NotEqual(0UL, captured.encounterRngState);
            Assert.NotEqual(0UL, captured.incidentRngState);

            var serializer = new SystemTextJsonSerializer();
            var roundTripped = serializer.Deserialize<WildlifeTrappingState>(serializer.Serialize(captured));
            Assert.NotNull(roundTripped);
            var restored = new WildlifeTrappingSystem(new Ashfall.Core.SeededRng(captured.rngSeed));
            restored.RegisterPreyDefinition(primary);
            restored.RegisterQuarry(new QuarrySpecies
            {
                speciesId = primary.speciesId,
                preferredTrapType = "snare",
                baseYieldKg = primary.baseYieldKg,
                toxicChance = primary.toxicChance
            });
            restored.RegisterTrapDefinition(trap);
            restored.RestoreState(roundTripped!);
            var recaptured = restored.CaptureState();

            Assert.Equal(captured.rngSeed, recaptured.rngSeed);
            Assert.Equal(captured.primaryRngState, recaptured.primaryRngState);
            Assert.Equal(captured.encounterRngState, recaptured.encounterRngState);
            Assert.Equal(captured.incidentRngState, recaptured.incidentRngState);
        }

        [Fact]
        public void LiveVendorMatcher_ReachabilityAndBodyGripExclusionAreProven()
        {
            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var loaded = GoodsCatalogLoader.Load(FindDataDir(), io, json);
            Assert.False(loaded.HasErrors, string.Join("; ", loaded.Errors));
            var catalog = GoodsCatalogLoader.ToCatalog(loaded);

            Assert.True(RegionalSupplyRouter.IsAcceptedSupplyTag("general"));
            Assert.True(RegionalSupplyRouter.IsAcceptedSupplyTag("settlement"));
            Assert.True(RegionalSupplyRouter.IsAcceptedSupplyTag("coastal"));
            Assert.True(RegionalSupplyRouter.ProducesGood(catalog, "ash_flats", "trap_improvised_wire"));
            Assert.True(RegionalSupplyRouter.ProducesGood(catalog, "settlement", "trap_box"));
            Assert.True(RegionalSupplyRouter.ProducesGood(catalog, "deep_coast", "trap_fish"));
            Assert.False(RegionalSupplyRouter.ProducesGood(catalog, "industrial_belt", "trap_fish"));
            Assert.Null(catalog.Find("trap_body_grip"));
            Assert.DoesNotContain(
                RegionalSupplyRouter.SpecialtyCargoForOrigin(catalog, "settlement", 100),
                entry => entry.GoodId == "trap_body_grip");
        }

        [Fact]
        public void GoodsLoader_RejectsRegionalSupplyOutsideLiveMatcherVocabulary()
        {
            const string raw = "{\"schema_version\":1,\"goods\":[{\"id\":\"trap_test\",\"displayName\":\"Test Trap\",\"category\":\"tools\",\"basePrice\":10,\"regionalSupply\":\"invented_region\"}]}";
            var result = GoodsCatalogLoader.Load("/tmp", new SingleFileIO(raw), new SystemTextJsonSerializer());
            Assert.True(result.HasErrors);
            Assert.Contains(result.Errors, error => error.Contains("regionalSupply", StringComparison.Ordinal));
        }

        [Fact]
        public void TrapTradePrices_ExceedCraftInputValueUnderConveniencePremiumPolicy()
        {
            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var items = ItemCatalogLoader.LoadCatalog(FindDataDir(), io, json);
            var goodsLoad = GoodsCatalogLoader.Load(FindDataDir(), io, json);
            Assert.False(goodsLoad.HasErrors, string.Join("; ", goodsLoad.Errors));
            var goods = GoodsCatalogLoader.ToCatalog(goodsLoad);
            var recipes = RecipeCatalogLoader.Load(FindDataDir(), io, json, items);

            foreach (var recipeId in new[] { "craft_trap_improvised_wire", "craft_trap_box", "craft_trap_fish" })
            {
                var recipe = Assert.Single(recipes, value => value.id == recipeId);
                Assert.NotNull(recipe.result);
                var good = goods.Find(recipe.result.id);
                Assert.NotNull(good);
                float craftInputValue = recipe.ingredients.Sum(ingredient => ingredient.item.tradeValue * ingredient.amount);
                const float conveniencePremiumFloor = 1.1f;
                Assert.True(good!.basePrice >= craftInputValue * conveniencePremiumFloor,
                    $"{recipe.result.id}: purchase {good.basePrice} must be >= craft input {craftInputValue} × {conveniencePremiumFloor}");
            }
        }

        [Fact]
        public void NarrativeIncidentCatalog_ContainsThreeResolvedAtmosphericEvents()
        {
            using var doc = JsonDocument.Parse(File.ReadAllText(Path.Combine(FindDataDir(), "events.json")));
            var ids = new HashSet<string>(StringComparer.Ordinal);
            foreach (var eventEl in doc.RootElement.GetProperty("events").EnumerateArray())
            {
                string id = eventEl.GetProperty("id").GetString() ?? string.Empty;
                if (Array.IndexOf(TrapNarrativeIncidentIds.Ordered, id) < 0) continue;
                ids.Add(id);
                Assert.False(string.IsNullOrWhiteSpace(eventEl.GetProperty("title").GetString()));
                Assert.False(string.IsNullOrWhiteSpace(eventEl.GetProperty("bodyText").GetString()));
            }
            Assert.Equal(3, ids.Count);
        }

        private sealed class SingleFileIO : IFileIO
        {
            private readonly string _content;
            public SingleFileIO(string content) { _content = content; }
            public bool DirectoryExists(string path) => true;
            public bool FileExists(string path) => true;
            public string ReadAllText(string path) => _content;
            public void WriteAllText(string path, string contents) { }
            public string Combine(params string[] parts) => string.Join("/", parts);
        }
    }
}
