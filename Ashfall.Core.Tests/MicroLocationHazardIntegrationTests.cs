using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.Disease;
using Ashfall.Core.Flags;
using Ashfall.Core.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// F17 flagship integration — micro-location hazards reach the canonical
    /// disease authority. Proves the full chain from the plan:
    ///
    ///   micro-location choice → NarrativeEncounterSystem.TryResolve (payload)
    ///     → flag ledger commits → MicroLocationHazardRegistry routes
    ///     → DiseaseSystem.Infect (deterministic, exactly once) → save/reload parity.
    ///
    /// Authoritative mapping (data-authored, not invented): the disease
    /// catalog's own source_note binds "dead livestock" scavenging to
    /// disease_zoonotic_flu. The shell crater and collapsed bridge carry NO
    /// contamination flag in the catalog — asserted below so they can never
    /// silently acquire one. No new contamination subsystem is introduced:
    /// this suite exercises EncounterChoiceEffectDispatcher,
    /// MicroLocationHazardRegistry, and DiseaseSystem only.
    /// </summary>
    public class MicroLocationHazardIntegrationTests
    {
        private const string DeadLivestockId = "micro_dead_livestock";
        private const string ScavengeLivestockChoiceId = "scavenge_livestock";
        private const string InspectTagsChoiceId = "inspect_livestock_tags";
        private const string AvoidLivestockChoiceId = "avoid_livestock";
        private const string ShellCraterId = "micro_shell_crater";
        private const string InspectCraterChoiceId = "inspect_crater";
        private const string CollapsedBridgeId = "micro_collapsed_bridge";
        private const string SearchBridgeVehicleChoiceId = "search_bridge_vehicle";
        private const string ContaminationFlag = MicroLocationHazardRegistry.ContaminationExposureFlag;
        private const string ZoonoticFluId = MicroLocationHazardRegistry.DeadLivestockDiseaseId;
        private const string SurvivorId = "surv_scavenger";

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

        private static DiseaseSystem CreateDiseaseSystem(int seed = 4242)
        {
            var catalog = DiseaseCatalogLoader.Load(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.False(catalog.HasErrors, string.Join("; ", catalog.Errors));
            var sys = new DiseaseSystem(rng: new SeededRng(seed));
            sys.BindCatalog(catalog);
            return sys;
        }

        /// <summary>Host-faithful single-shot application: resolve, apply flag,
        /// route hazard with the pre-resolution flag verdict.</summary>
        private static MicroLocationHazardRegistry.HazardApplicationResult ApplyProductionFlow(
            NarrativeEncounterSystem sys,
            CampaignConsequenceLedger ledger,
            DiseaseSystem disease,
            string encounterId,
            string choiceId,
            int day,
            string survivorId)
        {
            bool alreadyBefore = ledger.IsSet(ContaminationFlag);
            var result = sys.TryResolve(encounterId, choiceId, "loc_suburban_ruins", day);
            Assert.NotNull(result);

            if (!string.IsNullOrEmpty(result!.SetWorldFlagId))
                EncounterChoiceEffectDispatcher.ApplyWorldFlag(result, ledger);

            return MicroLocationHazardRegistry.ApplyFlagHazard(
                result.SetWorldFlagId,
                flagWasAlreadySet: alreadyBefore && result.SetWorldFlagId == ContaminationFlag,
                survivorId,
                day,
                (sid, did, d) => disease.Infect(sid, did, d));
        }

        // ── Positive hazard path ───────────────────────────────────────

        [Fact]
        public void F17_01_DeadLivestock_Scavenge_ContaminationReachesDiseaseAuthority_ExactlyOnce()
        {
            var sys = CreateProductionNarrativeSystem();
            var ledger = new CampaignConsequenceLedger();
            var disease = CreateDiseaseSystem();

            var hazard = ApplyProductionFlow(sys, ledger, disease, DeadLivestockId, ScavengeLivestockChoiceId, 5, SurvivorId);

            Assert.Equal(MicroLocationHazardRegistry.HazardStatus.Applied, hazard.Status);
            Assert.Equal(ZoonoticFluId, hazard.DiseaseId);
            Assert.Equal(SurvivorId, hazard.SurvivorId);
            Assert.True(ledger.IsSet(ContaminationFlag));
            Assert.True(disease.IsInfected(SurvivorId, ZoonoticFluId));

            // Exactly one infection record for the survivor, with the authored day.
            var entry = disease.State.diseases.Find(e => e.disease_id == ZoonoticFluId);
            Assert.NotNull(entry);
            Assert.Equal(1, entry!.infected.Count);
            Assert.Equal(SurvivorId, entry.infected[0].survivor_id);
            Assert.Equal(5, entry.infected[0].infected_day);
        }

        [Fact]
        public void F17_02_DeadLivestock_LootStillGranted_WithContamination()
        {
            var sys = CreateProductionNarrativeSystem();
            var res = sys.TryResolve(DeadLivestockId, ScavengeLivestockChoiceId, "loc_suburban_ruins", 5);
            Assert.NotNull(res);
            Assert.Equal("cloth", res!.GrantItemId);
            Assert.Equal(2, res.GrantItemQuantity);
            Assert.Equal(ContaminationFlag, res.SetWorldFlagId);
            Assert.True(res.DepletesEncounter);
        }

        [Fact]
        public void F17_03_ReprocessedFlag_CannotReinfect_FlagReplayExploit()
        {
            var sys = CreateProductionNarrativeSystem();
            var ledger = new CampaignConsequenceLedger();
            var disease = CreateDiseaseSystem();

            ApplyProductionFlow(sys, ledger, disease, DeadLivestockId, ScavengeLivestockChoiceId, 5, SurvivorId);
            var entry = disease.State.diseases.Find(e => e.disease_id == ZoonoticFluId);
            Assert.Equal(1, entry!.infected.Count);

            // Flag replay: same flag routed again with the ledger verdict
            // "already set" — the host's save/reload / revisit path.
            var replay = MicroLocationHazardRegistry.ApplyFlagHazard(
                ContaminationFlag, flagWasAlreadySet: true, SurvivorId, 6,
                (sid, did, d) => disease.Infect(sid, did, d));
            Assert.Equal(MicroLocationHazardRegistry.HazardStatus.AlreadyKnown, replay.Status);
            Assert.Equal(1, entry.infected.Count);

            // Double protection: even a raw duplicate Infect call is a no-op
            // (canonical authority stacking policy — no additive dose).
            disease.Infect(SurvivorId, ZoonoticFluId, 6);
            Assert.Equal(1, entry.infected.Count);
        }

        // ── Avoidance / non-hazard paths ───────────────────────────────

        [Fact]
        public void F17_04_DeadLivestock_AvoidChoice_NoContamination_NoHazard()
        {
            var sys = CreateProductionNarrativeSystem();
            var ledger = new CampaignConsequenceLedger();
            var disease = CreateDiseaseSystem();

            var hazard = ApplyProductionFlow(sys, ledger, disease, DeadLivestockId, AvoidLivestockChoiceId, 5, SurvivorId);

            Assert.Equal(MicroLocationHazardRegistry.HazardStatus.NotApplicable, hazard.Status);
            Assert.False(ledger.IsSet(ContaminationFlag));
            Assert.False(disease.IsInfected(SurvivorId, ZoonoticFluId));
            var entry = disease.State.diseases.Find(e => e.disease_id == ZoonoticFluId);
            Assert.NotNull(entry);
            Assert.Equal(0, entry!.infected.Count);
        }

        [Fact]
        public void F17_05_DeadLivestock_TagInspection_JournalOnly_NoContamination()
        {
            var sys = CreateProductionNarrativeSystem();
            var res = sys.TryResolve(DeadLivestockId, InspectTagsChoiceId, "loc_suburban_ruins", 5);
            Assert.NotNull(res);
            Assert.Equal("micro_dead_livestock_tags", res!.JournalUnlockId);
            Assert.True(string.IsNullOrEmpty(res.SetWorldFlagId));
            Assert.True(string.IsNullOrEmpty(res.GrantItemId));
        }

        [Fact]
        public void F17_06_ShellCrater_InspectCrater_NoBiologicalContamination()
        {
            var sys = CreateProductionNarrativeSystem();
            var ledger = new CampaignConsequenceLedger();
            var disease = CreateDiseaseSystem();

            var hazard = ApplyProductionFlow(sys, ledger, disease, ShellCraterId, InspectCraterChoiceId, 3, SurvivorId);

            // Structural salvage site: loot only, no flag, no contamination.
            Assert.Equal(MicroLocationHazardRegistry.HazardStatus.NotApplicable, hazard.Status);
            Assert.False(ledger.IsSet(ContaminationFlag));
            Assert.False(disease.IsInfected(SurvivorId, ZoonoticFluId));

            // One-shot holds at the level the production UI uses: the site is
            // depleted, so the selector can never surface it again.
            Assert.True(sys.IsDepleted(ShellCraterId));
            for (int seed = 0; seed < 64; seed++)
            {
                var picked = sys.SelectEncounter("Cautious", 0f, "loc_bombed_street", new SeededRng(seed));
                Assert.NotEqual(ShellCraterId, picked?.id);
            }
        }

        [Fact]
        public void F17_07_ShellCraterAndBridge_NeverAuthorContaminationFlag()
        {
            // Data scan: the contamination flag must be authored by exactly one
            // producer in the whole catalog — the dead-livestock scavenge choice.
            var defs = NarrativeEncounterCatalogLoader.Load(
                DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            var producers = new List<string>();
            foreach (var def in defs)
            {
                if (def?.choices == null) continue;
                foreach (var choice in def.choices)
                {
                    if (choice != null && choice.setWorldFlag == ContaminationFlag)
                        producers.Add($"{def.id}/{choice.choiceId}");
                }
            }
            Assert.Equal(new[] { $"{DeadLivestockId}/{ScavengeLivestockChoiceId}" }, producers);

            // And neither hazard-sibling site carries it on any choice.
            foreach (var def in defs)
            {
                if (def == null || def.choices == null) continue;
                if (def.id != ShellCraterId && def.id != CollapsedBridgeId) continue;
                foreach (var choice in def.choices)
                    Assert.True(string.IsNullOrEmpty(choice?.setWorldFlag),
                        $"{def.id}/{choice?.choiceId} must not author a contamination flag");
            }
        }

        [Fact]
        public void F17_08_CollapsedBridge_AndShellCrater_StructuralLoot_Unchanged()
        {
            var sys = CreateProductionNarrativeSystem();

            var bridge = sys.TryResolve(CollapsedBridgeId, SearchBridgeVehicleChoiceId, "loc_river_crossing", 2);
            Assert.NotNull(bridge);
            Assert.Equal("fuel", bridge!.GrantItemId);
            Assert.Equal(2, bridge.GrantItemQuantity);
            Assert.True(string.IsNullOrEmpty(bridge.SetWorldFlagId));

            var crater = sys.TryResolve(ShellCraterId, InspectCraterChoiceId, "loc_bombed_street", 3);
            Assert.NotNull(crater);
            Assert.Equal("scrap_metal", crater!.GrantItemId);
            Assert.Equal(2, crater.GrantItemQuantity);
            Assert.True(string.IsNullOrEmpty(crater.SetWorldFlagId));
        }

        // ── Registry contract ──────────────────────────────────────────

        [Fact]
        public void F17_09_Registry_MapsOnlyRegisteredFlags_UnknownFlagsPassThrough()
        {
            Assert.Equal(ZoonoticFluId, MicroLocationHazardRegistry.TryGetFlagDiseaseId(ContaminationFlag));
            Assert.Null(MicroLocationHazardRegistry.TryGetFlagDiseaseId("micro_generator_marked"));
            Assert.Null(MicroLocationHazardRegistry.TryGetFlagDiseaseId(null));
            Assert.Null(MicroLocationHazardRegistry.TryGetFlagDiseaseId(string.Empty));
        }

        [Fact]
        public void F17_10_Registry_NoSurvivorOrNoAuthority_DegradesHonestly()
        {
            var noSurvivor = MicroLocationHazardRegistry.ApplyFlagHazard(
                ContaminationFlag, flagWasAlreadySet: false, null, 5, (s, d, day) => { });
            Assert.Equal(MicroLocationHazardRegistry.HazardStatus.SkippedNoSurvivor, noSurvivor.Status);

            var noAuthority = MicroLocationHazardRegistry.ApplyFlagHazard(
                ContaminationFlag, flagWasAlreadySet: false, SurvivorId, 5, null);
            Assert.Equal(MicroLocationHazardRegistry.HazardStatus.SkippedNoAuthority, noAuthority.Status);

            // Unknown disease id is rejected by the canonical authority itself.
            var disease = CreateDiseaseSystem();
            var recorded = 0;
            disease.Infect(SurvivorId, "disease_not_in_catalog", 5);
            foreach (var e in disease.State.diseases) recorded += e.infected.Count;
            Assert.Equal(0, recorded);
        }

        // ── Persistence ────────────────────────────────────────────────

        [Fact]
        public void F17_11_SaveReload_ContaminationStateParity_NoDuplicateExposure()
        {
            var sys = CreateProductionNarrativeSystem();
            var ledger = new CampaignConsequenceLedger();
            var disease = CreateDiseaseSystem();

            ApplyProductionFlow(sys, ledger, disease, DeadLivestockId, ScavengeLivestockChoiceId, 5, SurvivorId);

            // Save: capture every authoritative subsystem through its own contract.
            var json = new SystemTextJsonSerializer();
            string diseaseJson = json.Serialize(disease.CaptureState());
            string ledgerJson = json.Serialize(ledger.CaptureState());
            string narrativeJson = json.Serialize(sys.CaptureState());

            // Restore into fresh fixtures.
            var disease2 = CreateDiseaseSystem();
            disease2.RestoreState(json.Deserialize<DiseaseSystemState>(diseaseJson)!);
            var ledger2 = new CampaignConsequenceLedger();
            ledger2.RestoreState(json.Deserialize<CampaignConsequenceSaveState>(ledgerJson)!);
            var sys2 = CreateProductionNarrativeSystem();
            sys2.RestoreState(json.Deserialize<NarrativeEncounterState>(narrativeJson)!);

            // Contamination state identical.
            Assert.True(disease2.IsInfected(SurvivorId, ZoonoticFluId));
            var entryA = disease.State.diseases.Find(e => e.disease_id == ZoonoticFluId)!;
            var entryB = disease2.State.diseases.Find(e => e.disease_id == ZoonoticFluId)!;
            Assert.Equal(entryA.infected.Count, entryB.infected.Count);
            Assert.Equal(entryA.infected[0].infected_day, entryB.infected[0].infected_day);

            // First post-load tick/hazard pass: the flag is already set, so the
            // exposure must NOT fire again.
            bool flagAlreadySet = ledger2.IsSet(ContaminationFlag);
            Assert.True(flagAlreadySet);
            var post = MicroLocationHazardRegistry.ApplyFlagHazard(
                ContaminationFlag, flagWasAlreadySet: flagAlreadySet, SurvivorId, 6,
                (sid, did, d) => disease2.Infect(sid, did, d));
            Assert.Equal(MicroLocationHazardRegistry.HazardStatus.AlreadyKnown, post.Status);
            Assert.Equal(entryB.infected.Count, disease2.State.diseases.Find(e => e.disease_id == ZoonoticFluId)!.infected.Count);

            // The depleted site cannot re-surface through the production selector.
            for (int seed = 0; seed < 64; seed++)
            {
                var picked = sys2.SelectEncounter("Cautious", 0f, "loc_suburban_ruins", new SeededRng(seed));
                Assert.NotEqual(DeadLivestockId, picked?.id);
            }
        }

        // ── Determinism ────────────────────────────────────────────────

        [Fact]
        public void F17_12_Deterministic_SameSeedSameChoice_IdenticalContaminationState()
        {
            for (int seed = 0; seed < 8; seed++)
            {
                var (jsonA, _) = RunSeededPass(seed, day: 7);
                var (jsonB, _) = RunSeededPass(seed, day: 7);
                Assert.Equal(jsonA, jsonB);
            }
        }

        private static (string json, DiseaseSystem system) RunSeededPass(int seed, int day)
        {
            var sys = CreateProductionNarrativeSystem();
            var ledger = new CampaignConsequenceLedger();
            var disease = CreateDiseaseSystem(seed);
            ApplyProductionFlow(sys, ledger, disease, DeadLivestockId, ScavengeLivestockChoiceId, day, SurvivorId);
            var entry = disease.State.diseases.Find(e => e.disease_id == ZoonoticFluId);
            var json = new SystemTextJsonSerializer();
            return (json.Serialize(disease.CaptureState()) + "|" + json.Serialize(entry), disease);
        }

        // ── Cross-catalog authority (§16.4 — no orphan flag) ───────────

        [Fact]
        public void F17_13_ContaminationFlagDisease_IsRegisteredInProductionCatalog()
        {
            // §16.2/§16.4 — the mapping target must be a real catalog entry.
            var catalog = DiseaseCatalogLoader.Load(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.NotNull(catalog.GetDefinition(ZoonoticFluId));
            Assert.NotNull(MicroLocationHazardRegistry.TryGetFlagDiseaseId(ContaminationFlag));
        }
    }
}
