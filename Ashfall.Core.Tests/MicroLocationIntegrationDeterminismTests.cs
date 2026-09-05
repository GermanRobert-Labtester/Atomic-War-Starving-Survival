using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Ashfall.Core.Disease;
using Ashfall.Core.Flags;
using Ashfall.Core.IO;
using Ashfall.Core.Journal;
using Ashfall.Core.Narrative;
using Xunit;
namespace Ashfall.Core.Tests
{
    using Inventory = Ashfall.Core.Inventory.Inventory;
    using ItemCatalog = Ashfall.Core.Inventory.ItemCatalog;
    /// <summary>
    /// F17–F20 flagship hardening — shared content validation and the
    /// deterministic integration trace (flagship plan §11 + §13).
    ///
    /// §11 — cross-catalog validation: the four flagship reward items, the two
    /// journal keys, and the one hazard flag must all be real, consumed
    /// content ("valid but dead" fails here).
    ///
    /// §13 — determinism harness: one production-wiring fixture resolves the
    /// flagship choice of each location and records a canonical trace line
    /// (location | choice | items | flags | journal | hazard | discovery |
    /// final resolution state). Two independent passes must serialize
    /// byte-for-byte identically.
    /// </summary>
    public class MicroLocationIntegrationDeterminismTests
    {
        private const string ContaminationFlag = MicroLocationHazardRegistry.ContaminationExposureFlag;
        private const string SurvivorId = "surv_trace";

        private static string DataDir()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null && !File.Exists(Path.Combine(dir.FullName, "Ashfall.csproj")))
                dir = dir.Parent!;
            return Path.Combine(dir!.FullName, "Assets", "StreamingAssets", "Data");
        }

        private static NarrativeEncounterSystem CreateProductionNarrativeSystem()
        {
            var sys = new NarrativeEncounterSystem();
            var defs = NarrativeEncounterCatalogLoader.Load(
                DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            sys.RegisterRange(defs);
            return sys;
        }

        private static ItemCatalog LoadItemCatalog()
        {
            return Ashfall.Core.Inventory.ItemCatalogLoader.LoadCatalog(
                DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
        }

        // ── §13 — deterministic integration trace ──────────────────────

        private sealed class TraceBuilder
        {
            public readonly StringBuilder Sb = new StringBuilder();

            public void Entry(string locationId, string choiceId, NarrativeEncounterResolutionResult? r,
                string hazardEffect, NarrativeEncounterSystem sys)
            {
                Sb.Append("loc=").Append(locationId)
                  .Append("|choice=").Append(choiceId)
                  .Append("|item=").Append(r?.GrantItemId ?? "none").Append('x').Append(r?.GrantItemQuantity ?? 0)
                  .Append("|flag=").Append(string.IsNullOrEmpty(r?.SetWorldFlagId) ? "none" : r!.SetWorldFlagId)
                  .Append("|journal=").Append(string.IsNullOrEmpty(r?.JournalUnlockId) ? "none" : r!.JournalUnlockId)
                  .Append("|discovery=").Append(string.IsNullOrEmpty(r?.DiscoverLocationId) ? "none" : r!.DiscoverLocationId)
                  .Append("|hazard=").Append(hazardEffect)
                  .Append("|depleted=").Append(sys.IsDepleted(locationId) ? '1' : '0')
                  .Append("|totalResolved=").Append(sys.TotalResolved)
                  .Append('\n');
            }
        }

        /// <summary>One full production pass over all four flagship sites in a
        /// fixed order, with the same application order the host uses.</summary>
        private string RunIntegrationPass(int seed, int dayBase)
        {
            var sys = CreateProductionNarrativeSystem();
            var ledger = new CampaignConsequenceLedger();
            var diseaseCatalog = DiseaseCatalogLoader.Load(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            var disease = new DiseaseSystem(rng: new SeededRng(seed));
            disease.BindCatalog(diseaseCatalog);
            var inventory = new Inventory();
            var journal = new JournalSystem();
            var trace = new TraceBuilder();

            void ApplyFlagship(string encounterId, string choiceId, string locationId, int day)
            {
                bool flagWasAlreadySet = ledger.IsSet(ContaminationFlag);
                var res = sys.TryResolve(encounterId, choiceId, locationId, day);
                Assert.NotNull(res);

                // item (canonical AddById grant — the host's loot path shape)
                if (!string.IsNullOrEmpty(res!.GrantItemId) && res.GrantItemQuantity > 0)
                    inventory.AddById(res.GrantItemId, res.GrantItemQuantity);

                // journal (canonical dedup gate)
                string journalEffect = "none";
                if (!string.IsNullOrEmpty(res.JournalUnlockId))
                    journalEffect = journal.TryDiscoverKnowledge(res.JournalUnlockId, new TraceAuthor(), day) != null
                        ? "unlocked" : "dedup";

                // world flag + hazard routing (ledger verdict decides)
                string hazardEffect = "none";
                if (!string.IsNullOrEmpty(res.SetWorldFlagId))
                {
                    EncounterChoiceEffectDispatcher.ApplyWorldFlag(res, ledger);
                    var hazard = MicroLocationHazardRegistry.ApplyFlagHazard(
                        res.SetWorldFlagId,
                        flagWasAlreadySet: flagWasAlreadySet && res.SetWorldFlagId == ContaminationFlag,
                        SurvivorId, day,
                        (sid, did, d) => disease.Infect(sid, did, d));
                    hazardEffect = hazard.Status.ToString();
                }

                trace.Entry(encounterId, choiceId, res, hazardEffect, sys);
            }

            ApplyFlagship("micro_dead_livestock", "scavenge_livestock", "loc_suburban_ruins", dayBase + 0);
            ApplyFlagship("micro_ruined_greenhouse", "take_greenhouse_seeds", "loc_allotments", dayBase + 1);
            ApplyFlagship("micro_radio_tower", "open_radio_cabinet", "loc_radio_hill", dayBase + 2);
            ApplyFlagship("micro_water_source", "collect_water", "loc_old_farmstead", dayBase + 3);

            trace.Sb.Append("inv=").Append(inventory.CountById("cloth")).Append(',')
                      .Append(inventory.CountById("seed_packets")).Append(',')
                      .Append(inventory.CountById("antenna_coil")).Append(',')
                      .Append(inventory.CountById("clean_water"))
                      .Append("|disease=").Append(disease.IsInfected(SurvivorId, MicroLocationHazardRegistry.DeadLivestockDiseaseId))
                      .Append("|journalKeys=").Append(journal.Entries.Count)
                      .Append("|depleted=").Append(sys.DepletedCount)
                      .Append('\n');
            return trace.Sb.ToString();
        }

        private sealed class TraceAuthor : ISurvivorAuthor
        {
            public string Id => SurvivorId;
            public string DisplayName => "Trace";
            public RiskBiasTrait RiskBias => RiskBiasTrait.Realist;
        }

        [Fact]
        public void Trace_TwoIndependentPasses_ByteIdentical()
        {
            string passA = RunIntegrationPass(seed: 7071, dayBase: 10);
            string passB = RunIntegrationPass(seed: 7071, dayBase: 10);
            Assert.Equal(passA, passB, ignoreLineEndingDifferences: false, ignoreCase: false);
        }

        [Fact]
        public void Trace_CoversAllFourFlagshipSites_WithExpectedEffects()
        {
            string pass = RunIntegrationPass(seed: 7071, dayBase: 10);
            var lines = pass.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            Assert.Contains(lines, l => l.StartsWith("loc=micro_dead_livestock|choice=scavenge_livestock") && l.Contains("item=clothx2") && l.Contains("flag=micro_contamination_exposure") && l.Contains("hazard=Applied") && l.Contains("depleted=1"));
            Assert.Contains(lines, l => l.StartsWith("loc=micro_ruined_greenhouse|choice=take_greenhouse_seeds") && l.Contains("item=seed_packetsx2") && l.Contains("hazard=none"));
            Assert.Contains(lines, l => l.StartsWith("loc=micro_radio_tower|choice=open_radio_cabinet") && l.Contains("item=antenna_coilx1") && l.Contains("hazard=none"));
            Assert.Contains(lines, l => l.StartsWith("loc=micro_water_source|choice=collect_water") && l.Contains("item=clean_waterx3") && l.Contains("hazard=none"));
        }

        [Fact]
        public void Trace_SameSeedDifferentDayBase_StillDeterministicPerFixture()
        {
            // Different campaign days change the recorded day-dependent payload
            // shape but never the grant/flag/hazard outcomes.
            string pass1 = RunIntegrationPass(seed: 7071, dayBase: 10);
            string pass2 = RunIntegrationPass(seed: 7071, dayBase: 99);
            Assert.Equal(
                pass1.Split('\n').Count(l => l.Contains("hazard=Applied")),
                pass2.Split('\n').Count(l => l.Contains("hazard=Applied")));
        }

        // ── §11.1/§11.4 — no valid-but-dead flagship content ───────────

        [Fact]
        public void ContentValidation_FlagshipRewardItems_ResolveAndHaveDownstreamConsumers()
        {
            var catalog = LoadItemCatalog();

            // §11.1 — all four reward ids are real catalog entries.
            foreach (var id in new[] { "seed_packets", "crop_medicinal_herb", "antenna_coil", "clean_water" })
                Assert.True(catalog.Contains(id), $"flagship reward item '{id}' missing from the item catalog");

            // §11.4 — each has a live downstream consumer:
            // seed_packets → the canonical crop catalog (agriculture input).
            Assert.NotNull(GreenhouseExpansionCatalog.CropCatalog.Get("seed_packets"));
            // crop_medicinal_herb → authored clean yield of the herb seed line.
            Assert.Equal("crop_medicinal_herb", GreenhouseExpansionCatalog.CropCatalog
                .Get(GreenhouseExpansionCatalog.Items.SeedMedicinalHerb)!.YieldCleanId);
            // clean_water → canonical hydration consumption (thirstRestore > 0).
            Assert.True(catalog.Get("clean_water")!.thirstRestore > 0f);
            // antenna_coil → relic repair bill (asserted in depth by F19_03/05;
            // the file-level reference check here guards the data authority).
            string relicRaw = new FileSystemIO().ReadAllText(
                new FileSystemIO().Combine(DataDir(), "relic_recipes.json"));
            Assert.Contains("antenna_coil", relicRaw, StringComparison.Ordinal);
        }

        [Fact]
        public void ContentValidation_FlagshipJournalKeys_ResolveThroughCanonicalUnlock()
        {
            foreach (var key in new[] { "micro_radio_tower_log", "micro_dead_livestock_tags" })
            {
                Assert.StartsWith("micro_", key, StringComparison.Ordinal);
                var journal = new JournalSystem();
                var entry = journal.TryDiscoverKnowledge(key, new TraceAuthor(), 5);
                Assert.NotNull(entry);
                Assert.Null(journal.TryDiscoverKnowledge(key, new TraceAuthor(), 6)); // exactly once
            }
        }

        [Fact]
        public void ContentValidation_FlagshipJournalKeys_HaveAuthoredProse_NeverPlaceholder()
        {
            // F19 §9.6 / F17 — the flagship unlocks carry authored per-bias prose
            // (journal_voice_prose.json). The generic "Something changed."
            // fallback is a content bug for these keys.
            var catalog = JournalVoiceProseCatalogLoader.LoadDefault();
            Assert.True(catalog.Count > 0, "journal_voice_prose.json must load");

            foreach (var key in new[] { "micro_radio_tower_log", "micro_dead_livestock_tags" })
            {
                foreach (RiskBiasTrait bias in Enum.GetValues<RiskBiasTrait>())
                {
                    string text = catalog.GetProse(key, bias);
                    Assert.NotEqual("Something changed. I wrote it down so I would not forget.", text);
                    Assert.False(string.IsNullOrWhiteSpace(text));
                }
            }
        }

        [Fact]
        public void ContentValidation_EveryAuthoredMicroFlag_HasRegisteredConsumerOrInertContract()
        {
            // §11.3 — world-flag consumer integrity. Each flag authored by a
            // micro-location must either (a) have a registered hazard consumer
            // in MicroLocationHazardRegistry, or (b) be on the explicitly
            // reviewed inert list (world-state markers consumed by quests /
            // future integration — tracked here so nothing silently rots).
            var inertReviewed = new HashSet<string>(StringComparer.Ordinal)
            {
                "micro_generator_marked", // waystation generator marker — quest/condition marker (F5 suite)
            };

            var defs = NarrativeEncounterCatalogLoader.Load(
                DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            var authored = new SortedSet<string>(StringComparer.Ordinal);
            foreach (var def in defs)
            {
                if (def?.choices == null) continue;
                foreach (var choice in def.choices)
                {
                    if (choice == null || string.IsNullOrWhiteSpace(choice.setWorldFlag)) continue;
                    authored.Add(choice.setWorldFlag.Trim());

                    bool hasHazardConsumer = MicroLocationHazardRegistry.TryGetFlagDiseaseId(choice.setWorldFlag) != null;
                    Assert.True(hasHazardConsumer || inertReviewed.Contains(choice.setWorldFlag),
                        $"micro-location flag '{choice.setWorldFlag}' has no registered consumer and is not on the reviewed inert list");
                }
            }

            Assert.Contains(ContaminationFlag, authored);
            Assert.Equal(MicroLocationHazardRegistry.DeadLivestockDiseaseId,
                MicroLocationHazardRegistry.TryGetFlagDiseaseId(ContaminationFlag));
        }

        [Fact]
        public void ContentValidation_ProductionCatalogLoad_StampsMicroLocationMarker()
        {
            // F6 §6.3 seal — production loads micro_locations.json through the shared
            // NarrativeEncounterCatalogLoader.LoadFile, which must apply the same
            // isMicroLocation/sourceFile stamp the dedicated
            // MicroLocationEncounterLoader applies. Previously only the dedicated
            // (test-only) loader stamped; production definitions silently carried
            // isMicroLocation = false.
            var defs = CreateProductionNarrativeSystem().Catalog;

            var microDefs = defs.Where(d => d.id.StartsWith("micro_", StringComparison.Ordinal)).ToList();
            Assert.True(microDefs.Count >= 6, $"expected the authored micro-location set, found {microDefs.Count}");

            foreach (var def in microDefs)
            {
                Assert.True(def.isMicroLocation, $"production-loaded '{def.id}' must be stamped isMicroLocation");
                Assert.Equal("micro_locations.json", def.sourceFile);
            }

            // Non-micro files keep their defaults (no cross-file stamping).
            var coreDefs = defs.Where(d => !d.id.StartsWith("micro_", StringComparison.Ordinal)).ToList();
            Assert.True(coreDefs.Count > 0);
            Assert.All(coreDefs, d => Assert.False(d.isMicroLocation));
        }

        [Fact]
        public void ContentValidation_MicroLocations_CarrySchemaVersion()
        {
            // Data-authority hygiene (Invariant 6): the catalog the flagship
            // hooks resolve from must keep its schema_version.
            string raw = new FileSystemIO().ReadAllText(
                new FileSystemIO().Combine(DataDir(), "micro_locations.json"));
            Assert.Contains("\"schema_version\"", raw, StringComparison.Ordinal);
        }
    }
}
