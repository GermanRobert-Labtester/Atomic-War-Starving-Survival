// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core
{
    /// <summary>
    /// Declarative enum of all executable host actions dispatched via CLI.
    /// </summary>
    public enum HostCliAction
    {
        Interactive,
        Help,
        Version,
        SelfTestManifest,
        ListSelfTests,
        UserDataDirConfig,
        LogDirConfig,

        // Core & System Gates
        SevenDayDeterministicSmokeSelfTest,
        AssetCoverageReport,
        AssetRegistrySelfTest,
        BridgeSelfTest,
        CoreSelfTest,
        DataIntegritySelfTest,
        DifficultySelfTest,
        ResearchCatalogSelfTest,
        RadioCatalogSelfTest,
        PanelBindLifecycleSelfTest,
        SaveLoadUiFailureSelfTest,
        SaveStoreChecksumSelfTest,
        RuntimeScaleSelfTest,
        StandaloneSystemsSelfTest,
        Plans139To141SelfTest,
        Plans122to125SelfTest,
        LateTechMobilitySelfTest,
        Plans122to125BalanceSoak,
        SkyDefenseSelfTest,
        VehicleGarageSelfTest,
        PortContractSelfTest,

        // Expansions & Campaign Modules
        ArbitrationSelfTest,
        BlackFlotillaSelfTest,
        BrineSelfTest,
        CensusSelfTest,
        ClusterSelfTest,
        CombatSelfTest,
        CrossingSelfTest,
        DeepCoastHostSelfTest,
        DeepCoastSelfTest,
        DiseaseSelfTest,
        DutyRosterSelfTest,
        EndingsSelfTest,
        ExpansionsSelfTest,
        GreenhouseSelfTest,
        AgricultureSelfTest,
        DefenseSelfTest,
        PsychologySelfTest,
        WildlifeSelfTest,
        PrecisionMetrologySelfTest,
        DirectionFindingSelfTest,
        AquaponicsSelfTest,
        CombatBreachingSelfTest,
        HoldfastBriefing,
        HoldfastSelfTest,
        IceRoadSelfTest,
        IceRoadTickDemo,
        LedgerDebtSelfTest,
        MoralChoiceSelfTest,
        EvolvingWorldSelfTest,
        WorldPlaytestSelfTest,
        SyntheticLubricantSelfTest,
        UvCoronaSelfTest,
        CarbonCompositeSelfTest,
        GprCartographySelfTest,
        AdvancedIndustrialReconSelfTest,
        MusterSelfTest,
        FactionEcologySelfTest,
        Phase0SelfTest,
        SilentFoundrySelfTest,
        StandingRecordSelfTest,
        VerdictSelfTest,
        WarlordHostSelfTest,
        WarlordSelfTest,
        CommitmentsSelfTest,
        SessionDurabilitySelfTest,
        PlayMetricsSelfTest,
        SurvivorVoiceSelfTest,
        ContentCertificationSelfTest,
        HoldfastPresentationSelfTest,
        ScarcityAudioSelfTest,
        SliceScenarioSelfTest,
        RetentionSelfTest,
        OutpostSettlementSelfTest,
        WeatherCascadeSelfTest,
        StandingGatesSelfTest,
        TerritoryControlSelfTest,
        CookingSelfTest,
        NeedsPerformanceSelfTest,
        CampaignLegacySelfTest,
        ResearchUnlockSelfTest,
        UnifiedEndingSelfTest,
        NpcMemorySelfTest,
        IdeologicalFrictionSelfTest,
        RomanceFamilySelfTest,
        VehicleCustomizationSelfTest,
        BackstorySelfTest,
        MetaProgressionSelfTest,
        OrphanSealWave1SelfTest,
        WarlordUiSelfTest,
        FactionCommuniqueBoardSelfTest,

        // Host Domains & Save Stores
        AudioSelfTest,
        CaravanSelfTest,
        ChemicalDependencySaveSelfTest,
        ContrabandStashSelfTest,
        DoseLedgerSelfTest,
        DutyRosterSaveSelfTest,
        EconomySelfTest,
        ExpansionHubSaveSelfTest,
        ExpeditionEncounterBridgeSelfTest,
        ExpeditionSelfTest,
        ExpeditionPlaytestSelfTest,
        PatrolEncounterSelfTest,
        HoldfastSaveSelfTest,
        HoldfastTradeSaveSelfTest,
        InventorySaveSelfTest,
        JournalSaveSelfTest,
        JournalSelfTest,
        JournalWeatherPanelSelfTest,
        MedicalSelfTest,
        MedicalWardSaveSelfTest,
        NarrativeSelfTest,
        NpcArcSelfTest,
        RadioSelfTest,
        SettingsSelfTest,
        SurvivorsSelfTest,
        HiddenAgendaSelfTest,
        ShelterReputationSelfTest,
        UtilityAiSelfTest,
        WeatherSaveSelfTest,
        WorldSelfTest,
        YearOfAshSaveSelfTest,

        // UI Tests, Layout & Gameplay Smoke
        DashboardUiTest,
        Day1PlayableSelfTest,
        Day1ToDay2MilestoneSelfTest,
        DoseUiTest,
        DutyRosterUiTest,
        EconomyUiTest,
        ExpeditionPanelUiTest,
        HoldfastRuntimeUiTest,
        InventoryUiTest,
        JournalUiTest,
        MusterUiTest,
        Phase0UiTest,
        PlayableShellSelfTest,
        PlayerPanelsUiTest,
        ShelterHazardLoopSelfTest,
        ShelterOperationsSelfTest,
        ShelterDecorSelfTest,
        ShelterPhysicsSelfTest,
        ShelterAtmosphereSelfTest,
        SilentFoundryUiTest,
        SurvivorsUiTest,
        UiLayoutSelfTest,
        UiAccessibilitySelfTest,
        UiSnapshotRegenerate,
        UiSnapshotSelfTest,
        UtilityAiUiTest,
        VerdictUiTest,
        OnboardingJourneySelfTest,
        RealCampaignJourneySelfTest,
        EbPvdCoatingUiTest,
        MicrofluidicDiagnosticUiTest,
        MineFlailUiTest,
        RailGrindingUiTest,
        PropagandaSelfTest,
        RumorNetworkSelfTest,
        ShelterSecuritySelfTest,
        PersonalQuestSelfTest,
        TimeCapsuleSelfTest,
        DeathLegacySelfTest,
        RelationshipDecaySelfTest
    }

    /// <summary>
    /// Metadata descriptor for a single registered host-CLI command or verb.
    /// </summary>
    public sealed class HostCliActionDescriptor
    {
        public HostCliAction Action { get; }
        public string Category { get; }
        public string PrimaryFlag { get; }
        public IReadOnlyList<string> Aliases { get; }
        public string Description { get; }
        public string ValuePlaceholder { get; }
        public IReadOnlyList<string> AllFlags { get; }
        public bool IsSelfTest { get; }
        public bool IsTest { get; }
        public bool HeadlessCompatible { get; }
        public string TestId { get; }

        public HostCliActionDescriptor(
            HostCliAction action,
            string category,
            string primaryFlag,
            string[]? aliases,
            string description,
            string valuePlaceholder = "")
        {
            Action = action;
            Category = category;
            PrimaryFlag = primaryFlag;
            Aliases = aliases ?? Array.Empty<string>();
            Description = description;
            ValuePlaceholder = valuePlaceholder;

            var all = new List<string>(1 + Aliases.Count) { primaryFlag };
            all.AddRange(Aliases);
            AllFlags = all.AsReadOnly();

            IsSelfTest = AllFlags.Any(f => f.EndsWith("-selftest", StringComparison.OrdinalIgnoreCase));
            IsTest = IsSelfTest ||
                     category == "UI Tests, Layout & Gameplay Smoke" ||
                     primaryFlag.EndsWith("-report", StringComparison.OrdinalIgnoreCase) ||
                     primaryFlag.EndsWith("-briefing", StringComparison.OrdinalIgnoreCase) ||
                     primaryFlag.EndsWith("-demo", StringComparison.OrdinalIgnoreCase) ||
                     primaryFlag.EndsWith("-uitest", StringComparison.OrdinalIgnoreCase);

            HeadlessCompatible = action != HostCliAction.UiSnapshotRegenerate && action != HostCliAction.UiSnapshotSelfTest;
            TestId = HostTestSummary.NormalizeTestName(primaryFlag);
        }

        public string FormatHelpLine()
        {
            var sb = new StringBuilder();
            sb.Append(PrimaryFlag);
            foreach (var alias in Aliases)
            {
                sb.Append(" / ").Append(alias);
            }
            if (!string.IsNullOrEmpty(ValuePlaceholder))
            {
                sb.Append(' ').Append(ValuePlaceholder);
            }

            string flagsPart = sb.ToString();
            if (flagsPart.Length < 25)
            {
                return $"  {flagsPart.PadRight(25)}{Description}";
            }
            return $"  {flagsPart} {Description}";
        }
    }

    /// <summary>
    /// Declarative, authoritative registry of all host CLI actions.
    /// Drives argument parsing, --host-help generation, CI verification gates, and markdown documentation.
    /// </summary>
    public static class HostCliRegistry
    {
        public static readonly IReadOnlyList<string> Categories = new ReadOnlyCollection<string>(new[]
        {
            "Core & System Gates",
            "Expansions & Campaign Modules",
            "Host Domains & Save Stores",
            "UI Tests, Layout & Gameplay Smoke",
            "User Data & Log Configuration",
            "General & Information"
        });

        private static readonly HostCliActionDescriptor[] _coreDescriptors = new[]
        {
                new HostCliActionDescriptor(
                    HostCliAction.SevenDayDeterministicSmokeSelfTest,
                    "Core & System Gates",
                    "--7-day-smoke-selftest",
                    new[] { "--seven-day-smoke-selftest", "--deterministic-smoke-selftest" },
                    "7-day deterministic smoke run: map discovery + weather rolls + survivor needs drift + mid-run save/reload round-trip across 10 verification gates"),
                new HostCliActionDescriptor(
                    HostCliAction.AssetCoverageReport,
                    "Core & System Gates",
                    "--asset-coverage-report",
                    null,
                    "Full non-gating sweep of every catalog id (core + expansions) vs loadable art; prints per-category coverage and the missing list"),
                new HostCliActionDescriptor(
                    HostCliAction.AssetRegistrySelfTest,
                    "Core & System Gates",
                    "--asset-registry-selftest",
                    null,
                    "Verify that catalog IDs (items/survivors/locations) resolve to actual texture assets under assets/"),
                new HostCliActionDescriptor(
                    HostCliAction.BridgeSelfTest,
                    "Core & System Gates",
                    "--bridge-selftest",
                    null,
                    "Report UnityEngine shim removal (shim is gone; always exits 0)"),
                new HostCliActionDescriptor(
                    HostCliAction.CoreSelfTest,
                    "Core & System Gates",
                    "--core-selftest",
                    null,
                    "Ice road + census headless demos"),
                new HostCliActionDescriptor(
                    HostCliAction.DataIntegritySelfTest,
                    "Core & System Gates",
                    "--data-integrity-selftest",
                    null,
                    "Cross-reference every id in the 129 StreamingAssets catalogs (recipe→item, quest→location, events, door encounters, survivors, factions, ranges, duplicates)"),
                new HostCliActionDescriptor(
                    HostCliAction.DifficultySelfTest,
                    "Core & System Gates",
                    "--difficulty-selftest",
                    null,
                    "XP-01 difficulty catalog, scalar consumers, starting bonuses, fail-closed selection, and save checksum binding"),
                new HostCliActionDescriptor(
                    HostCliAction.ResearchCatalogSelfTest,
                    "Core & System Gates",
                    "--research-catalog-selftest",
                    null,
                    "Research knowledge catalog gate (Plan 34): load count, DAG validity, original 15 save-contract nodes, and cross-catalog unlock references (breakthrough items, relic research unlocks, manual/autopsy knowledge grants)"),
                new HostCliActionDescriptor(
                    HostCliAction.RadioCatalogSelfTest,
                    "Core & System Gates",
                    "--radio-catalog-selftest",
                    null,
                    "Radio station catalog gate (AF-B1 / Plan 60): JSON authority, schedules, signal degradation model, and overrides"),
                new HostCliActionDescriptor(
                    HostCliAction.PanelBindLifecycleSelfTest,
                    "Core & System Gates",
                    "--panel-bind-lifecycle-selftest",
                    new[] { "--panel-bind-selftest", "--panel-lifecycle-selftest" },
                    "Real Godot-node callback tests for panel bind → unbind → rebind, event propagation, and session-switch"),
                new HostCliActionDescriptor(
                    HostCliAction.SaveLoadUiFailureSelfTest,
                    "Core & System Gates",
                    "--save-load-ui-failure-selftest",
                    new[] { "--save-load-failure-selftest", "--save-load-failure-uitest", "--save-load-selftest" },
                    "Save/load UI failure-path smoke test: missing, corrupt, and checksum-invalid saves show recoverable user messages and leave live session intact"),
                new HostCliActionDescriptor(
                    HostCliAction.SaveStoreChecksumSelfTest,
                    "Core & System Gates",
                    "--save-store-checksum-selftest",
                    new[] { "--save-store-checksums-selftest", "--checksum-sweep-selftest" },
                    "Source-scan all SaveStore files for checksum coverage + 5 in-memory round-trip probes (Weather, Map, Survivors, SaveChecksum stability, null-field guard)"),
                new HostCliActionDescriptor(
                    HostCliAction.RuntimeScaleSelfTest,
                    "Core & System Gates",
                    "--runtime-scale-selftest",
                    new[] { "--runtime-scale", "--performance-selftest", "--perf-selftest" },
                    "Performance budget validation: 30/180/360-day campaign workloads, day-advance latency, save/load/checksum, allocations, retained memory, and lifecycle leak tests; writes artifacts/runtime-scale-results.json"),
                new HostCliActionDescriptor(
                    HostCliAction.StandaloneSystemsSelfTest,
                    "Core & System Gates",
                    "--standalone-selftest",
                    null,
                    "SkyLayerArmor, VigilStateMachine, GenerationalSuccession, EpilogueMatrix, DiveInstance")
                ,
                new HostCliActionDescriptor(
                    HostCliAction.Plans139To141SelfTest,
                    "Core & System Gates",
                    "--plans-139-141-selftest",
                    new[] { "--insar-selftest", "--hydraulic-extrusion-selftest", "--runflat-tire-selftest" },
                    "Flagship Plans 139–141: InSAR repeat-pass classification, extrusion quality/defect/tool-wear, run-flat hazard/heat/rolling-resistance"),
                new HostCliActionDescriptor(
                    HostCliAction.SkyDefenseSelfTest,
                    "Core & System Gates",
                    "--sky-defense-selftest",
                    null,
                    "Flagship Task 7: kinetic sky-layer counter-battery — telemetry track intake, magazine logistics, deterministic volley, heat/hydraulics service, crew claim, save round-trip, and player-panel construction"),
                new HostCliActionDescriptor(
                    HostCliAction.VehicleGarageSelfTest,
                    "Core & System Gates",
                    "--vehicle-garage-selftest",
                    null,
                    "Flagship Plan 50: overland vehicle customization & maintenance — modification install/uninstall, component wear, service, immobilization gate, recovery mission completion, and expedition-profile decoration"),
                new HostCliActionDescriptor(
                    HostCliAction.Plans122to125SelfTest,
                    "Core & System Gates",
                    "--plans-122-125-selftest",
                    new[] { "--sofc-power-selftest", "--sound-ranging-selftest", "--cvd-diamond-selftest", "--amphibious-draisine-selftest" },
                    "Flagship Plans 122-125: SOFC grid/CHP wiring, CVD consumer wear registry, defensive threat projection, amphibious route capability"),
                new HostCliActionDescriptor(
                    HostCliAction.LateTechMobilitySelfTest,
                    "Core & System Gates",
                    "--late-tech-mobility-selftest",
                    null,
                    "Flagship Plans 122-125 Phase 10: deterministic 75-day combined scenario — SOFC commissioning/baseload, acoustic threat narrowing, diamond tool economy, amphibious retrofit/crossing, day-71 save + replay parity via state hashes"),
                new HostCliActionDescriptor(
                    HostCliAction.Plans122to125BalanceSoak,
                    "Core & System Gates",
                    "--plans-122-125-balance-soak",
                    null,
                    "Flagship Plans 122-125 Phase 11: long-horizon balance soaks — SOFC 180-day characterization, acoustic fixed-seed event matrix, diamond 120-day tool economy, amphibious 54-cell route matrix; prints [SOAK] data rows for the balance reports"),
                new HostCliActionDescriptor(
                    HostCliAction.PortContractSelfTest,
                    "Core & System Gates",
                    "--port-contract-selftest",
                    null,
                    "Plan 36: Port Contracts & Host Wiring — validates all Core integration seams against docs/ci/port_contract_policy.json and runtime host collaborator/port wiring")
        };

        private static readonly HostCliActionDescriptor[] _expansionDescriptors = new[]
        {
                new HostCliActionDescriptor(
                    HostCliAction.ArbitrationSelfTest,
                    "Expansions & Campaign Modules",
                    "--arbitration-selftest",
                    null,
                    "CrossingArbitrationHeadlessDemo"),
                new HostCliActionDescriptor(
                    HostCliAction.BlackFlotillaSelfTest,
                    "Expansions & Campaign Modules",
                    "--black-flotilla-selftest",
                    new[] { "--maritime-selftest", "--expansion-09-selftest" },
                    "The Black Flotilla (Exp 09): catalog load, deterministic scavenge, dive rooms/air/noise, contamination, visit state, save round-trip"),
                new HostCliActionDescriptor(
                    HostCliAction.BrineSelfTest,
                    "Expansions & Campaign Modules",
                    "--brine-selftest",
                    new[] { "--salt-steam-selftest" },
                    "BrineWaterHeadlessDemo (S2 salt & steam)"),
                new HostCliActionDescriptor(
                    HostCliAction.CensusSelfTest,
                    "Expansions & Campaign Modules",
                    "--census-selftest",
                    null,
                    "CensusHeadlessDemo"),
                new HostCliActionDescriptor(
                    HostCliAction.ClusterSelfTest,
                    "Expansions & Campaign Modules",
                    "--cluster-selftest",
                    new[] { "--order-12c-selftest" },
                    "Cluster12CHeadlessDemo (S3 order 12-C + quest snapshot)"),
                new HostCliActionDescriptor(
                    HostCliAction.CombatSelfTest,
                    "Expansions & Campaign Modules",
                    "--combat-selftest",
                    null,
                    "Combat Expansion: catalog (JSON), ballistics, weapon condition, determinism, save round-trip"),
                new HostCliActionDescriptor(
                    HostCliAction.CrossingSelfTest,
                    "Expansions & Campaign Modules",
                    "--crossing-selftest",
                    null,
                    "CrossingHeadlessDemo (Exp 04)"),
                new HostCliActionDescriptor(
                    HostCliAction.DeepCoastHostSelfTest,
                    "Expansions & Campaign Modules",
                    "--deep-coast-host-selftest",
                    new[] { "--deep-coast-playthrough" },
                    "Deep-coast host playthrough: survey → decision → dive → scavenge → save/restore"),
                new HostCliActionDescriptor(
                    HostCliAction.DeepCoastSelfTest,
                    "Expansions & Campaign Modules",
                    "--deep-coast-selftest",
                    new[] { "--deep-coast-route-selftest" },
                    "District 8 deep-coast route: stages, decisions, Ice Road gating, dive handoff, v5 save"),
                new HostCliActionDescriptor(
                    HostCliAction.DiseaseSelfTest,
                    "Expansions & Campaign Modules",
                    "--disease-selftest",
                    new[] { "--disease-expansion-selftest" },
                    "Disease Expansion: catalog, quarantine, protocols, determinism, save round-trip"),
                new HostCliActionDescriptor(
                    HostCliAction.DutyRosterSelfTest,
                    "Expansions & Campaign Modules",
                    "--duty-roster-selftest",
                    null,
                    "DutyRosterHeadlessDemo (Exp 02)"),
                new HostCliActionDescriptor(
                    HostCliAction.EndingsSelfTest,
                    "Expansions & Campaign Modules",
                    "--endings-selftest",
                    new[] { "--shelf-selftest" },
                    "EndingsHeadlessDemo (S4 endings exclusive + roundtrip)"),
                new HostCliActionDescriptor(
                    HostCliAction.ExpansionsSelfTest,
                    "Expansions & Campaign Modules",
                    "--expansions-selftest",
                    new[] { "--all-expansions-selftest" },
                    "Run full 7-expansion verification suite (Holdfast, Duty Roster, Standing Record, Crossing, Arbitration, LedgerDebt, Glass Orchard)"),
                new HostCliActionDescriptor(
                    HostCliAction.GreenhouseSelfTest,
                    "Expansions & Campaign Modules",
                    "--greenhouse-selftest",
                    new[] { "--glass-orchard-selftest" },
                    "GreenhouseHeadlessDemo (Exp 05)"),
                new HostCliActionDescriptor(
                    HostCliAction.AgricultureSelfTest,
                    "Expansions & Campaign Modules",
                    "--agriculture-selftest",
                    null,
                    "Plans 162-165: crop strain catalog, growth composition, mutation isolation, compost, nutrition diversity"),
                new HostCliActionDescriptor(
                    HostCliAction.DefenseSelfTest,
                    "Expansions & Campaign Modules",
                    "--defense-selftest",
                    null,
                    "Plans 162-165: trap catalog, perimeter composition, pre-combat raid resolution, capture handoff"),
                new HostCliActionDescriptor(
                    HostCliAction.PsychologySelfTest,
                    "Expansions & Campaign Modules",
                    "--psychology-selftest",
                    null,
                    "Plans 162-165: arc catalog, sustained triggers, conditional behaviors, stash conservation, catharsis bounds"),
                new HostCliActionDescriptor(
                    HostCliAction.WildlifeSelfTest,
                    "Expansions & Campaign Modules",
                    "--wildlife-selftest",
                    null,
                    "Plans 162-165: species catalog, single-population ecology, extinction/recolonization, apex, taming transfer"),
                new HostCliActionDescriptor(
                    HostCliAction.PrecisionMetrologySelfTest,
                    "Expansions & Campaign Modules",
                    "--precision-metrology-selftest",
                    null,
                    "Plan B89: metrology catalog, registered-consumer calibration, workshop projection, disturbance determinism, save round-trip"),
                new HostCliActionDescriptor(
                    HostCliAction.DirectionFindingSelfTest,
                    "Expansions & Campaign Modules",
                    "--direction-finding-selftest",
                    null,
                    "Plan B88: DF catalog, baselines, skywave uncertainty, fingerprint identity, triangulation save nest, RadioSave V3"),
                new HostCliActionDescriptor(
                    HostCliAction.AquaponicsSelfTest,
                    "Expansions & Campaign Modules",
                    "--aquaponics-selftest",
                    null,
                    "Plan B87: aquaponics catalog, growth determinism, power-loss DO crash, harvest, nutrient export, save round-trip"),
                new HostCliActionDescriptor(
                    HostCliAction.CombatBreachingSelfTest,
                    "Expansions & Campaign Modules",
                    "--combat-breaching-selftest",
                    null,
                    "Plan B86: breaching catalog, quiet cut vs loud breach, vehicle gate, mid-breach save, barrier clone fields"),
                new HostCliActionDescriptor(
                    HostCliAction.HoldfastBriefing,
                    "Expansions & Campaign Modules",
                    "--holdfast-briefing",
                    null,
                    "Print location count and every Holdfast quest briefing"),
                new HostCliActionDescriptor(
                    HostCliAction.HoldfastSelfTest,
                    "Expansions & Campaign Modules",
                    "--holdfast-selftest",
                    null,
                    "Holdfast S1 survival loop, ice road, and trade verification"),
                new HostCliActionDescriptor(
                    HostCliAction.IceRoadSelfTest,
                    "Expansions & Campaign Modules",
                    "--ice-road-selftest",
                    null,
                    "IceRoadHeadlessDemo (Exp 01)"),
                new HostCliActionDescriptor(
                    HostCliAction.IceRoadTickDemo,
                    "Expansions & Campaign Modules",
                    "--ice-road-tick-demo",
                    null,
                    "Unlock, clerk, 30 day ticks, print catalog + briefing"),
                new HostCliActionDescriptor(
                    HostCliAction.LedgerDebtSelfTest,
                    "Expansions & Campaign Modules",
                    "--ledger-debt-selftest",
                    null,
                    "LedgerDebtHeadlessDemo"),
                new HostCliActionDescriptor(
                    HostCliAction.MoralChoiceSelfTest,
                    "Expansions & Campaign Modules",
                    "--moral-choice-selftest",
                    null,
                    "Moral choice: catalog + scripted arc + bands + reconcile events + journal hook + save/tamper checks"),
                new HostCliActionDescriptor(
                    HostCliAction.MusterSelfTest,
                    "Expansions & Campaign Modules",
                    "--muster-selftest",
                    new[] { "--expansion-06-selftest" },
                    "MusterHeadlessDemo (Exp 06 the Muster)"),
                new HostCliActionDescriptor(
                    HostCliAction.FactionEcologySelfTest,
                    "Expansions & Campaign Modules",
                    "--faction-ecology-selftest",
                    null,
                    "Plan 25 faction ecology vertical slice: faction action board, E-P1 escalation chain, claimant witness, camp arrivals, muster path"),
                new HostCliActionDescriptor(
                    HostCliAction.Phase0SelfTest,
                    "Expansions & Campaign Modules",
                    "--phase0-selftest",
                    null,
                    "Phase-0 effects: phantom work-eff/refusal, flashbacks, trade specialty, final-wish buff, respiratory stamina + save roundtrip"),
                new HostCliActionDescriptor(
                    HostCliAction.SilentFoundrySelfTest,
                    "Expansions & Campaign Modules",
                    "--silent-foundry-selftest",
                    null,
                    "Silent Foundry (Exp 10): trade stance, trust momentum, recipes, and save round-trip"),
                new HostCliActionDescriptor(
                    HostCliAction.StandingRecordSelfTest,
                    "Expansions & Campaign Modules",
                    "--standing-record-selftest",
                    null,
                    "StandingRecordHeadlessDemo (Exp 03)"),
                new HostCliActionDescriptor(
                    HostCliAction.CommitmentsSelfTest,
                    "Expansions & Campaign Modules",
                    "--commitments-selftest",
                    null,
                    "Plan 38 commitments & deadlines: authored catalog, warning ladder, exactly-once miss + consequence routing, save round-trip"),
                new HostCliActionDescriptor(
                    HostCliAction.SessionDurabilitySelfTest,
                    "Expansions & Campaign Modules",
                    "--session-durability-selftest",
                    null,
                    "Plan 39 session durability: slot capacity/isolation, interrupted-write + backup recovery audit, soak stability verdicts, capture round-trip"),
                new HostCliActionDescriptor(
                    HostCliAction.PlayMetricsSelfTest,
                    "Expansions & Campaign Modules",
                    "--playable-metrics-selftest",
                    null,
                    "Plan 46 playable metrics: bounded session stream, first-hour funnel, aggregation grades, capture round-trip"),
                new HostCliActionDescriptor(
                    HostCliAction.SurvivorVoiceSelfTest,
                    "Expansions & Campaign Modules",
                    "--survivor-voice-selftest",
                    null,
                    "Plan 42 survivor voice: authored catalog selection, day cooldowns, dispatch arbitration, capture round-trip"),
                new HostCliActionDescriptor(
                    HostCliAction.ContentCertificationSelfTest,
                    "Expansions & Campaign Modules",
                    "--content-certification-selftest",
                    null,
                    "Plan 49 content orphan certification: family manifest, live-evidence rows, clean/dormant/orphan verdicts"),
                new HostCliActionDescriptor(
                    HostCliAction.HoldfastPresentationSelfTest,
                    "Expansions & Campaign Modules",
                    "--holdfast-presentation-selftest",
                    null,
                    "Plan 51 holdfast presentation slate: room/actor/map projections, hazard and crisis bands, motion profile"),
                new HostCliActionDescriptor(
                    HostCliAction.ScarcityAudioSelfTest,
                    "Expansions & Campaign Modules",
                    "--scarcity-audio-selftest",
                    null,
                    "Plan 52 scarcity audio: weather-to-bed/cue authority mapping, absolute silence, alert ducking, geiger rate bands"),
                new HostCliActionDescriptor(
                    HostCliAction.SliceScenarioSelfTest,
                    "Expansions & Campaign Modules",
                    "--seven-day-slice-selftest",
                    null,
                    "Plan 54 seven-day slice playtest: authored beats, frozen scenario hash, beat verification and scorecard"),
                new HostCliActionDescriptor(
                    HostCliAction.RetentionSelfTest,
                    "Expansions & Campaign Modules",
                    "--retention-selftest",
                    null,
                    "Plan 55 retention & save budgeting: authored policy overlay, bounded canonical collections, protected obligations, capture round-trip"),
                new HostCliActionDescriptor(
                    HostCliAction.OutpostSettlementSelfTest,
                    "Expansions & Campaign Modules",
                    "--outpost-settlement-selftest",
                    null,
                    "Plan 58 outposts & second holdfast: authored catalog, establish/garrison/supply lifecycle, daily consume, capture round-trip"),
                new HostCliActionDescriptor(
                    HostCliAction.WeatherCascadeSelfTest,
                    "Expansions & Campaign Modules",
                    "--weather-cascade-selftest",
                    null,
                    "Plan 135 weather→gameplay cascade: authored template table, canonical severity, owner routing, expiry withdrawal, capture round-trip"),
                new HostCliActionDescriptor(
                    HostCliAction.StandingGatesSelfTest,
                    "Governance",
                    "--standing-gates-selftest",
                    null,
                    "Plan 59 retrospective standing gates: 22-row register, enforcement-bound verdicts, unbound/non-critical exposure"),
                new HostCliActionDescriptor(
                    HostCliAction.TerritoryControlSelfTest,
                    "Expansions & Campaign Modules",
                    "--territory-control-selftest",
                    new[] { "--territory-selftest" },
                    "Plan 134 faction territory & supply line control: contested nodes, fortification, garrison, supply line status, capture round-trip"),
                new HostCliActionDescriptor(
                    HostCliAction.CookingSelfTest,
                    "Expansions & Campaign Modules",
                    "--cooking-selftest",
                    new[] { "--cooking-test" },
                    "Plan 136 wildlife trapping food pipeline & cooking system: recipe loading, ingredient consumption, decontamination, skill progression, capture/restore"),
                new HostCliActionDescriptor(
                    HostCliAction.NeedsPerformanceSelfTest,
                    "Expansions & Campaign Modules",
                    "--needs-performance-selftest",
                    new[] { "--needs-perf-selftest" },
                    "Plan 137 needs to performance cascade: hunger, thirst, fatigue, cold, and morale modifiers on combat, work speed, and expeditions"),
                new HostCliActionDescriptor(
                    HostCliAction.CampaignLegacySelfTest,
                    "Expansions & Campaign Modules",
                    "--campaign-legacy-selftest",
                    new[] { "--legacy-selftest" },
                    "Plan 140 generational legacy & campaign inheritance: catalog load, campaign archiving, shelter persistence, trait inheritance, and New Game+ context"),
                new HostCliActionDescriptor(
                    HostCliAction.ResearchUnlockSelfTest,
                    "Expansions & Campaign Modules",
                    "--research-unlock-selftest",
                    new[] { "--research-unlocks-selftest" },
                    "Plan 141 research downstream unlocks bridge: breakthrough item grant, crafting recipes, shelter, expedition, combat, and medical capabilities"),
                new HostCliActionDescriptor(
                    HostCliAction.UnifiedEndingSelfTest,
                    "Expansions & Campaign Modules",
                    "--unified-ending-selftest",
                    new[] { "--epilogue-selftest" },
                    "Plan 145 unified ending resolution & epilogue personalization: political, social, moral, personal, and expedition resolution"),
                new HostCliActionDescriptor(
                    HostCliAction.NpcMemorySelfTest,
                    "Expansions & Campaign Modules",
                    "--npc-memory-selftest",
                    new[] { "--npc-memory-test" },
                    "Plan 147 per-NPC memory and relationship depth: trust, grudge, favors owed, forgiveness, and trade multipliers"),
                new HostCliActionDescriptor(
                    HostCliAction.IdeologicalFrictionSelfTest,
                    "Expansions & Campaign Modules",
                    "--ideological-friction-selftest",
                    new[] { "--ideology-selftest" },
                    "Plan 148 ideological friction events and quests: confrontations, conversions, bunker factions, and mediation"),
                new HostCliActionDescriptor(
                    HostCliAction.RomanceFamilySelfTest,
                    "Expansions & Campaign Modules",
                    "--romance-family-selftest",
                    new[] { "--romance-selftest" },
                    "Plan 150 romance & family dynamics: courtship stages, partnership, bonded pairs, family units, and adoption"),
                new HostCliActionDescriptor(
                    HostCliAction.VehicleCustomizationSelfTest,
                    "Expansions & Campaign Modules",
                    "--vehicle-customization-selftest",
                    new[] { "--vehicle-modules-selftest" },
                    "Plan 152 vehicle customization & mobile base: module slots, effective stats, bunk capacity, and base camps"),
                new HostCliActionDescriptor(
                    HostCliAction.BackstorySelfTest,
                    "Expansions & Campaign Modules",
                    "--backstory-selftest",
                    new[] { "--backstories-selftest" },
                    "Plan 174 procedural survivor backstories & origin mechanics: occupations, experiences, and secrets"),
                new HostCliActionDescriptor(
                    HostCliAction.MetaProgressionSelfTest,
                    "Expansions & Campaign Modules",
                    "--meta-progression-selftest",
                    new[] { "--meta-selftest" },
                    "Plan 175 meta progression & cross-run profile store: prestige scoring, crests, and NG+ boons"),
                new HostCliActionDescriptor(
                    HostCliAction.OrphanSealWave1SelfTest,
                    "Expansions & Campaign Modules",
                    "--orphan-seal-wave1-selftest",
                    null,
                    "ORPHAN-SEAL-PRIORITY-W1: ten priority orphan authorities — catalog bind, command, and state round-trip"),
                new HostCliActionDescriptor(
                    HostCliAction.VerdictSelfTest,
                    "Expansions & Campaign Modules",
                    "--verdict-selftest",
                    new[] { "--expansion-08-selftest" },
                    "The Verdict (Exp 08): machine log, reckoning phases, evidence, census, save"),
                new HostCliActionDescriptor(
                    HostCliAction.WarlordHostSelfTest,
                    "Expansions & Campaign Modules",
                    "--warlord-host-selftest",
                    null,
                    "Warlord host playthrough: YearOfAsh wiring, standing, v3 save/tamper"),
                new HostCliActionDescriptor(
                    HostCliAction.WarlordSelfTest,
                    "Expansions & Campaign Modules",
                    "--warlord-selftest",
                    new[] { "--warlord-ai-selftest" },
                    "Adaptive warlord AI: doctrines, territory, tribute, determinism, v3 save"),
                new HostCliActionDescriptor(
                    HostCliAction.WarlordUiSelfTest,
                    "Expansions & Campaign Modules",
                    "--warlord-ui-selftest",
                    null,
                    "Warlord tribute payment loop + collector voice + FactionsPanel card"),
                new HostCliActionDescriptor(
                    HostCliAction.FactionCommuniqueBoardSelfTest,
                    "Expansions & Campaign Modules",
                    "--faction-communique-board-selftest",
                    new[] { "--communique-board-selftest" },
                    "Faction Communiqué Board: day-gated rendering, empty states, attribution, refresh idempotence")
        };

        private static readonly HostCliActionDescriptor[] _hostDomainDescriptors = new[]
        {
                new HostCliActionDescriptor(
                    HostCliAction.AudioSelfTest,
                    "Host Domains & Save Stores",
                    "--audio-selftest",
                    new[] { "--audio-test" },
                    "Audio cue catalog, AudioManager wiring, and sound event verification"),
                new HostCliActionDescriptor(
                    HostCliAction.CaravanSelfTest,
                    "Host Domains & Save Stores",
                    "--caravan-selftest",
                    new[] { "--traveling-caravan-selftest" },
                    "Traveling caravan economy, inventory generation, and barter ticks"),
                new HostCliActionDescriptor(
                    HostCliAction.ChemicalDependencySaveSelfTest,
                    "Host Domains & Save Stores",
                    "--chemical-dependency-save-selftest",
                    null,
                    "Chemical dependency system save store round-trip, tolerance, and withdrawal states"),
                new HostCliActionDescriptor(
                    HostCliAction.ContrabandStashSelfTest,
                    "Host Domains & Save Stores",
                    "--contraband-stash-selftest",
                    new[] { "--contraband-selftest" },
                    "Plan 147 contraband stash discovery: day gate, once-only claim, canonical grant, checksummed save round-trip"),
                new HostCliActionDescriptor(
                    HostCliAction.EvolvingWorldSelfTest,
                    "Host Domains & Save Stores",
                    "--evolving-world-selftest",
                    null,
                    "Evolving-world activation: seeds, live weather-fed ticks, migration, expedition consequences, scarcity, save envelope, 360-day scenario"),
                new HostCliActionDescriptor(
                    HostCliAction.WorldPlaytestSelfTest,
                    "Host Domains & Save Stores",
                    "--world-playtest-selftest",
                    null,
                    "Fixed-seed 30-day evolving-world campaign proof: stable snapshots, downstream consumers, bounds, determinism, and midpoint save/load parity"),
                new HostCliActionDescriptor(
                    HostCliAction.SyntheticLubricantSelfTest,
                    "Host Domains & Save Stores",
                    "--synthetic-lubricant-selftest",
                    null,
                    "Plan 118: deterministic catalog-backed synthesis, catalyst, product routing, consumer registration, and save-state proof"),
                new HostCliActionDescriptor(
                    HostCliAction.UvCoronaSelfTest,
                    "Host Domains & Save Stores",
                    "--uv-corona-selftest",
                    null,
                    "Plan 119: bounded seeded electrical-fault observations, environmental limits, and detector state proof"),
                new HostCliActionDescriptor(
                    HostCliAction.CarbonCompositeSelfTest,
                    "Host Domains & Save Stores",
                    "--carbon-composite-selftest",
                    null,
                    "Plan 120: deterministic composite cure quality, explicit component projections, and active-job proof"),
                new HostCliActionDescriptor(
                    HostCliAction.GprCartographySelfTest,
                    "Host Domains & Save Stores",
                    "--gpr-cartography-selftest",
                    null,
                    "Plan 121: uncertain terrain-aware GPR observations, staged leads, and active-survey proof"),
                new HostCliActionDescriptor(
                    HostCliAction.AdvancedIndustrialReconSelfTest,
                    "Host Domains & Save Stores",
                    "--advanced-industrial-recon-selftest",
                    null,
                    "Plans 118-121: deterministic advanced industrial/reconnaissance catalog and 60-day Core proof"),
                new HostCliActionDescriptor(
                    HostCliAction.DoseLedgerSelfTest,
                    "Host Domains & Save Stores",
                    "--dose-ledger-selftest",
                    null,
                    "Dose Ledger save write → reload → restore → checksum/tamper checks"),
                new HostCliActionDescriptor(
                    HostCliAction.DutyRosterSaveSelfTest,
                    "Host Domains & Save Stores",
                    "--duty-roster-save-selftest",
                    null,
                    "Duty Roster save write → reload → restore → checksum/tamper checks"),
                new HostCliActionDescriptor(
                    HostCliAction.EconomySelfTest,
                    "Host Domains & Save Stores",
                    "--economy-selftest",
                    null,
                    "Run the engine-agnostic economy headless demo (goods load, market ticks, barter, save/load round-trip)"),
                new HostCliActionDescriptor(
                    HostCliAction.ExpansionHubSaveSelfTest,
                    "Host Domains & Save Stores",
                    "--expansion-hub-save-selftest",
                    null,
                    "Expansion hub save write → reload → restore → checksum/tamper checks"),
                new HostCliActionDescriptor(
                    HostCliAction.ExpeditionEncounterBridgeSelfTest,
                    "Host Domains & Save Stores",
                    "--expedition-encounter-bridge-selftest",
                    null,
                    "ExpeditionEncounterBridge bare-notice + resolved surface smoke test"),
                new HostCliActionDescriptor(
                    HostCliAction.ExpeditionSelfTest,
                    "Host Domains & Save Stores",
                    "--expedition-selftest",
                    null,
                    "Expedition domain: sorties, encounter resolution, loot drops, and save round-trip"),
                new HostCliActionDescriptor(
                    HostCliAction.ExpeditionPlaytestSelfTest,
                    "Host Domains & Save Stores",
                    "--expedition-playtest-selftest",
                    null,
                    "Plans 51: deterministic 30-day expedition campaign, vehicle balance ledger, breakdown, wear, and save/resume proof"),
                new HostCliActionDescriptor(
                    HostCliAction.PatrolEncounterSelfTest,
                    "Host Domains & Save Stores",
                    "--patrol-encounter-selftest",
                    new[] { "--travel-encounter-selftest" },
                    "Patrol catalog, cooldown, recognition, resolution, and save/restore lifecycle"),
                new HostCliActionDescriptor(
                    HostCliAction.HoldfastSaveSelfTest,
                    "Host Domains & Save Stores",
                    "--holdfast-save-selftest",
                    null,
                    "S1 save write → reload → restore → checksum/tamper checks"),
                new HostCliActionDescriptor(
                    HostCliAction.HoldfastTradeSaveSelfTest,
                    "Host Domains & Save Stores",
                    "--holdfast-trade-save-selftest",
                    null,
                    "Holdfast trade ledger and save store round-trip and tamper checks"),
                new HostCliActionDescriptor(
                    HostCliAction.InventorySaveSelfTest,
                    "Host Domains & Save Stores",
                    "--inventory-save-selftest",
                    null,
                    "Inventory system save store round-trip, item serialization, and checksum verification"),
                new HostCliActionDescriptor(
                    HostCliAction.JournalSaveSelfTest,
                    "Host Domains & Save Stores",
                    "--journal-save-selftest",
                    null,
                    "Journal system save store round-trip, entry ordering, and tamper checks"),
                new HostCliActionDescriptor(
                    HostCliAction.JournalSelfTest,
                    "Host Domains & Save Stores",
                    "--journal-selftest",
                    null,
                    "Journal domain + save roundtrip"),
                new HostCliActionDescriptor(
                    HostCliAction.JournalWeatherPanelSelfTest,
                    "Host Domains & Save Stores",
                    "--journal-weather-panel-selftest",
                    null,
                    "Journal and Weather forecast panel integration and live data binding"),
                new HostCliActionDescriptor(
                    HostCliAction.MedicalSelfTest,
                    "Host Domains & Save Stores",
                    "--medical-selftest",
                    null,
                    "Medical domain: patient triage, treatment protocols, affliction progression, and save round-trip"),
                new HostCliActionDescriptor(
                    HostCliAction.MedicalWardSaveSelfTest,
                    "Host Domains & Save Stores",
                    "--medical-ward-save-selftest",
                    null,
                    "Medical ward save store round-trip, bed allocation, and affliction persistence"),
                new HostCliActionDescriptor(
                    HostCliAction.NarrativeSelfTest,
                    "Host Domains & Save Stores",
                    "--narrative-selftest",
                    null,
                    "Narrative domain: dialog trees, echoes, flags, and story event resolution"),
                new HostCliActionDescriptor(
                    HostCliAction.NpcArcSelfTest,
                    "Host Domains & Save Stores",
                    "--npc-arc-selftest",
                    null,
                    "Plan 52 recurring NPC arcs: resolution precedence, encounter-to-quest memory, save round-trip, distress suppression"),
                new HostCliActionDescriptor(
                    HostCliAction.RadioSelfTest,
                    "Host Domains & Save Stores",
                    "--radio-selftest",
                    null,
                    "Radio persistence: history/frequency/played-dedup survive save/load; tamper rejected"),
                new HostCliActionDescriptor(
                    HostCliAction.SettingsSelfTest,
                    "Host Domains & Save Stores",
                    "--settings-selftest",
                    new[] { "--settings-test" },
                    "SettingsManager state, resolution, audio buses, and keybindings save/load"),
                new HostCliActionDescriptor(
                    HostCliAction.SurvivorsSelfTest,
                    "Host Domains & Save Stores",
                    "--survivors-selftest",
                    null,
                    "Survivors domain: needs decay, skill progression, trauma, and morale"),
                new HostCliActionDescriptor(
                    HostCliAction.HiddenAgendaSelfTest,
                    "Host Domains & Save Stores",
                    "--hidden-agenda-selftest",
                    new[] { "--hidden-agendas-selftest" },
                    "Survivor hidden agendas, multi-day investigation, confrontation branches, persistence round-trip, and UI binding"),
                new HostCliActionDescriptor(
                    HostCliAction.ShelterReputationSelfTest,
                    "Host Domains & Save Stores",
                    "--shelter-reputation-selftest",
                    new[] { "--reputation-selftest" },
                    "Plan 207: Shelter reputation, external perception, notoriety, public tags, persistence round-trip, and UI binding"),
                new HostCliActionDescriptor(
                    HostCliAction.UtilityAiSelfTest,
                    "Host Domains & Save Stores",
                    "--utility-ai-selftest",
                    null,
                    "Utility AI decision scoring, survivor behaviors, and action selection"),
                new HostCliActionDescriptor(
                    HostCliAction.WeatherSaveSelfTest,
                    "Host Domains & Save Stores",
                    "--weather-save-selftest",
                    null,
                    "Weather system save store round-trip, forecast queue, and atmospheric condition persistence"),
                new HostCliActionDescriptor(
                    HostCliAction.WorldSelfTest,
                    "Host Domains & Save Stores",
                    "--world-selftest",
                    null,
                    "World domain: map nodes, sector navigation, hazard regions, and landmark states"),
                new HostCliActionDescriptor(
                    HostCliAction.YearOfAshSaveSelfTest,
                    "Host Domains & Save Stores",
                    "--year-of-ash-save-selftest",
                    null,
                    "Year of Ash save write → reload → restore → checksum/tamper checks"),
                new HostCliActionDescriptor(
                    HostCliAction.PropagandaSelfTest,
                    "Host Domains & Save Stores",
                    "--propaganda-selftest",
                    new[] { "--propaganda-campaign-selftest" },
                    "Plan 168: Propaganda and morale warfare system, campaigns, broadcasts, save persistence, and UI binding"),
                new HostCliActionDescriptor(
                    HostCliAction.RumorNetworkSelfTest,
                    "Host Domains & Save Stores",
                    "--rumor-network-selftest",
                    new[] { "--rumors-selftest" },
                    "Plan 203: Wasteland information flow, rumors, intelligence gathering, save persistence, and UI binding"),
                new HostCliActionDescriptor(
                    HostCliAction.ShelterSecuritySelfTest,
                    "Host Domains & Save Stores",
                    "--shelter-security-selftest",
                    new[] { "--security-selftest" },
                    "Plan 138: Shelter defense, security clearance levels, breach alerts, save persistence, and UI binding"),
                new HostCliActionDescriptor(
                    HostCliAction.PersonalQuestSelfTest,
                    "Host Domains & Save Stores",
                    "--personal-quests-selftest",
                    new[] { "--personal-quest-selftest" },
                    "Plan 200: Survivor personal quests, character arcs, stage progression, save persistence, and UI binding"),
                new HostCliActionDescriptor(
                    HostCliAction.TimeCapsuleSelfTest,
                    "Host Domains & Save Stores",
                    "--time-capsule-selftest",
                    new[] { "--time-capsules-selftest" },
                    "Plan 212: Time capsule & legacy messages system, scheduled opening, save persistence, and UI binding"),
                new HostCliActionDescriptor(
                    HostCliAction.DeathLegacySelfTest,
                    "Host Domains & Save Stores",
                    "--death-legacy-selftest",
                    new[] { "--wills-selftest", "--survivor-death-selftest" },
                    "Plan 206: Survivor death records, wills, estate inheritance, disputes, save persistence, and UI binding"),
                new HostCliActionDescriptor(
                    HostCliAction.RelationshipDecaySelfTest,
                    "Host Domains & Save Stores",
                    "--relationship-decay-selftest",
                    new[] { "--social-drift-selftest" },
                    "Plan 182: Relationship decay, social drift, bond maintenance, save persistence, and UI binding")
        };

        private static readonly HostCliActionDescriptor[] _uiDescriptors = new[]
        {
                new HostCliActionDescriptor(
                    HostCliAction.DashboardUiTest,
                    "UI Tests, Layout & Gameplay Smoke",
                    "--dashboard-uitest",
                    null,
                    "Game Dashboard panel UI construction, HUD binding, and metrics display"),
                new HostCliActionDescriptor(
                    HostCliAction.Day1PlayableSelfTest,
                    "UI Tests, Layout & Gameplay Smoke",
                    "--day1-selftest",
                    new[] { "--day-1-selftest", "--day1-playable-selftest" },
                    "Day 1 onboarding, needs depletion, and shelter survival verification"),
                new HostCliActionDescriptor(
                    HostCliAction.Day1ToDay2MilestoneSelfTest,
                    "UI Tests, Layout & Gameplay Smoke",
                    "--day1-to-day2-selftest",
                    new[] { "--day1-to-day2", "--day1-to-day2-milestone-selftest" },
                    "Day 1 to Day 2 transition, overnight triage, and milestone progression"),
                new HostCliActionDescriptor(
                    HostCliAction.DoseUiTest,
                    "UI Tests, Layout & Gameplay Smoke",
                    "--dose-uitest",
                    null,
                    "Dose Ledger panel UI construction, radiation tiers, and dose history"),
                new HostCliActionDescriptor(
                    HostCliAction.DutyRosterUiTest,
                    "UI Tests, Layout & Gameplay Smoke",
                    "--duty-roster-uitest",
                    null,
                    "Duty Roster panel UI construction, role assignments, and shift scheduling"),
                new HostCliActionDescriptor(
                    HostCliAction.EconomyUiTest,
                    "UI Tests, Layout & Gameplay Smoke",
                    "--economy-uitest",
                    null,
                    "Economy market panel UI construction, price shock display, and barter grid"),
                new HostCliActionDescriptor(
                    HostCliAction.ExpeditionPanelUiTest,
                    "UI Tests, Layout & Gameplay Smoke",
                    "--expedition-panel-uitest",
                    new[] { "--expedition-panel-lifecycle" },
                    "Expedition panel encounter-notice lifecycle: open→surface→close→reopen→surface"),
                new HostCliActionDescriptor(
                    HostCliAction.HoldfastRuntimeUiTest,
                    "UI Tests, Layout & Gameplay Smoke",
                    "--holdfast-runtime-uitest",
                    new[] { "--holdfast-runtime-ui-test", "--holdfast-runtime-selftest" },
                    "Godot Holdfast terminal browse → trade → failed trade → save → reload"),
                new HostCliActionDescriptor(
                    HostCliAction.InventoryUiTest,
                    "UI Tests, Layout & Gameplay Smoke",
                    "--inventory-uitest",
                    new[] { "--inventory-selftest" },
                    "Inventory panel UI construction, item grid, and slot binding"),
                new HostCliActionDescriptor(
                    HostCliAction.JournalUiTest,
                    "UI Tests, Layout & Gameplay Smoke",
                    "--journal-uitest",
                    null,
                    "Build ledger UI, cycle tabs, quit"),
                new HostCliActionDescriptor(
                    HostCliAction.MusterUiTest,
                    "UI Tests, Layout & Gameplay Smoke",
                    "--muster-uitest",
                    null,
                    "The Muster panel UI construction, faction stance cards, and vote tally"),
                new HostCliActionDescriptor(
                    HostCliAction.Phase0UiTest,
                    "UI Tests, Layout & Gameplay Smoke",
                    "--phase0-uitest",
                    null,
                    "Phase 0 expansion UI preview and workstation panels"),
                new HostCliActionDescriptor(
                    HostCliAction.PlayableShellSelfTest,
                    "UI Tests, Layout & Gameplay Smoke",
                    "--playable-shell-selftest",
                    new[] { "--shell-selftest", "--playable-loop-selftest" },
                    "Playable shell game loop, scene transitions, and day advancement"),
                new HostCliActionDescriptor(
                    HostCliAction.PlayerPanelsUiTest,
                    "UI Tests, Layout & Gameplay Smoke",
                    "--player-panels-uitest",
                    new[] { "--player-panels-ui-test" },
                    "Bind and render Survivors, Medical, Weather, Radio, Shelter panels"),
                new HostCliActionDescriptor(
                    HostCliAction.ShelterHazardLoopSelfTest,
                    "UI Tests, Layout & Gameplay Smoke",
                    "--shelter-hazard-loop-selftest",
                    new[] { "--shelter-hazard-selftest", "--duty-roster-loop-selftest" },
                    "Shelter hazard loop and duty roster assignment verification"),
                new HostCliActionDescriptor(
                    HostCliAction.ShelterOperationsSelfTest,
                    "UI Tests, Layout & Gameplay Smoke",
                    "--shelter-operations-selftest",
                    new[] { "--shelter-ops-selftest", "--operations-selftest" },
                    "Medical triage, expedition sorties, radio network, crafting, and respiratory affliction verification"),
                new HostCliActionDescriptor(
                    HostCliAction.ShelterDecorSelfTest,
                    "UI Tests, Layout & Gameplay Smoke",
                    "--shelter-decor-selftest",
                    new[] { "--shelter-interior-selftest", "--memorial-wall-selftest" },
                    "Live items.json decor, inventory mount/remove, NeedsSystem morale, memorial-wall projection, save, and panel verification"),
                new HostCliActionDescriptor(
                    HostCliAction.ShelterPhysicsSelfTest,
                    "UI Tests, Layout & Gameplay Smoke",
                    "--shelter-physics-selftest",
                    new[] { "--shelter-actor-physics-selftest" },
                    "Physics base: CharacterBody2D gravity/floor collision, accelerated horizontal seek to room anchors, and blockout character sheet animation"),
                new HostCliActionDescriptor(
                    HostCliAction.ShelterAtmosphereSelfTest,
                    "UI Tests, Layout & Gameplay Smoke",
                    "--shelter-atmosphere-selftest",
                    new[] { "--atmosphere-selftest", "--shelter-noise-selftest" },
                    "Plan 220 & 205: Shelter atmosphere environmental facets, noise discipline, quiet hours, and acoustic management verification"),
                new HostCliActionDescriptor(
                    HostCliAction.SilentFoundryUiTest,
                    "UI Tests, Layout & Gameplay Smoke",
                    "--silent-foundry-uitest",
                    null,
                    "Silent Foundry trade panel UI construction, binding, and trade loop"),
                new HostCliActionDescriptor(
                    HostCliAction.SurvivorsUiTest,
                    "UI Tests, Layout & Gameplay Smoke",
                    "--survivors-uitest",
                    null,
                    "Survivors panel UI construction, roster cards, and affliction badges"),
                new HostCliActionDescriptor(
                    HostCliAction.UiAccessibilitySelfTest,
                    "UI Tests, Layout & Gameplay Smoke",
                    "--ui-accessibility-selftest",
                    new[] { "--accessibility-selftest", "--ui-a11y-selftest" },
                    "Verify focus order, keyboard close action, readable labels, and modal dismissal paths"),
                new HostCliActionDescriptor(
                    HostCliAction.UiLayoutSelfTest,
                    "UI Tests, Layout & Gameplay Smoke",
                    "--ui-layout-selftest",
                    new[] { "--layout-selftest" },
                    "Verify fixed 1920x1080 UI layout bounds, responsive containers, and panel alignments"),
                new HostCliActionDescriptor(
                    HostCliAction.UiSnapshotRegenerate,
                    "UI Tests, Layout & Gameplay Smoke",
                    "--ui-snapshot-regenerate",
                    new[] { "--ui-snapshots-regen" },
                    "Recapture all snapshot targets and OVERWRITE snapshots/ goldens (needs real display)"),
                new HostCliActionDescriptor(
                    HostCliAction.UiSnapshotSelfTest,
                    "UI Tests, Layout & Gameplay Smoke",
                    "--ui-snapshot-uitest",
                    new[] { "--ui-snapshots" },
                    "Capture all snapshot targets, DIFF against snapshots/ goldens (needs real display, not --headless)"),
                new HostCliActionDescriptor(
                    HostCliAction.UtilityAiUiTest,
                    "UI Tests, Layout & Gameplay Smoke",
                    "--utility-ai-uitest",
                    null,
                    "Utility AI debug view, consideration curves, and behavior trees"),
                new HostCliActionDescriptor(
                    HostCliAction.VerdictUiTest,
                    "UI Tests, Layout & Gameplay Smoke",
                    "--verdict-uitest",
                    null,
                    "Build THE MACHINE'S REGISTER panel; assert 13 transmissions render + leak-free"),
                new HostCliActionDescriptor(
                    HostCliAction.OnboardingJourneySelfTest,
                    "UI Tests, Layout & Gameplay Smoke",
                    "--onboarding-journey-selftest",
                    new[] { "--onboarding-selftest" },
                    "First-hour onboarding journey: water→power→food→research→expedition, with save/load resume and state-true signals"),
                new HostCliActionDescriptor(
                    HostCliAction.RealCampaignJourneySelfTest,
                    "UI Tests, Layout & Gameplay Smoke",
                    "--real-campaign-journey-selftest",
                    new[] { "--campaign-journey-selftest", "--real-main-journey-selftest" },
                    "Real Main-composed player journey (Plans #5/#7/#8/#9): New Game -> ComposeCampaign() -> typed gameplay action -> real day advance -> SaveAll -> reset -> Continue -> restored state -> post-load action; combat auto-spawn via expedition encounter trigger -> victory loot & weapon-condition write-back (Plan #9); Holdfast trade against the shared inventory -> day advance -> save/reload (Plan #7); radiation exposure -> treatment -> save/reload (Plan #8)"),
                new HostCliActionDescriptor(
                    HostCliAction.EbPvdCoatingUiTest,
                    "UI Tests, Layout & Gameplay Smoke",
                    "--ebpvd-coating-uitest",
                    new[] { "--ebpvd-coating-selftest" },
                    "EB-PVD thermal barrier coating UI panel construction, status rail, and job controls"),
                new HostCliActionDescriptor(
                    HostCliAction.MicrofluidicDiagnosticUiTest,
                    "UI Tests, Layout & Gameplay Smoke",
                    "--microfluidic-diagnostic-uitest",
                    new[] { "--microfluidic-diagnostic-selftest" },
                    "Microfluidic diagnostic analyzer UI panel construction, cartridge fab, and assay telemetry"),
                new HostCliActionDescriptor(
                    HostCliAction.MineFlailUiTest,
                    "UI Tests, Layout & Gameplay Smoke",
                    "--mine-flail-uitest",
                    new[] { "--mine-flail-selftest" },
                    "Mine-clearing flail UI panel construction, breach telemetry, and hardware maintenance"),
                new HostCliActionDescriptor(
                    HostCliAction.RailGrindingUiTest,
                    "UI Tests, Layout & Gameplay Smoke",
                    "--rail-grinding-uitest",
                    new[] { "--rail-grinding-selftest" },
                    "Rail grinding train UI panel construction, reprofiling telemetry, and stone maintenance")
        };

        private static readonly HostCliActionDescriptor[] _configDescriptors = new[]
        {
                new HostCliActionDescriptor(
                    HostCliAction.LogDirConfig,
                    "User Data & Log Configuration",
                    "--log-dir",
                    null,
                    "Override user://logs output directory",
                    "<dir>"),
                new HostCliActionDescriptor(
                    HostCliAction.UserDataDirConfig,
                    "User Data & Log Configuration",
                    "--user-data-dir",
                    null,
                    "Override user:// data directory for saves, logs, and cache",
                    "<dir>")
        };

        private static readonly HostCliActionDescriptor[] _infoDescriptors = new[]
        {
                new HostCliActionDescriptor(
                    HostCliAction.Help,
                    "General & Information",
                    "--host-help",
                    new[] { "--help" },
                    "This list"),
                new HostCliActionDescriptor(
                    HostCliAction.ListSelfTests,
                    "General & Information",
                    "--list-selftests",
                    new[] { "--list-tests", "--selftests" },
                    "Enumerate all registered host self-tests with stable test IDs and descriptions"),
                new HostCliActionDescriptor(
                    HostCliAction.SelfTestManifest,
                    "General & Information",
                    "--selftest-manifest",
                    new[] { "--test-manifest" },
                    "Export machine-readable JSON self-test manifest"),
                new HostCliActionDescriptor(
                    HostCliAction.Version,
                    "General & Information",
                    "--version",
                    new[] { "-v" },
                    "Show build, data schema, and save schema versions")
        };

        private static readonly List<HostCliActionDescriptor> _descriptors;
        private static readonly Dictionary<string, HostCliActionDescriptor> _flagMap;

        public static IReadOnlyList<HostCliActionDescriptor> AllDescriptors => _descriptors;
        public static IReadOnlyDictionary<string, HostCliActionDescriptor> FlagMap => _flagMap;

        public static IReadOnlyList<HostCliActionDescriptor> CoreDescriptors => _coreDescriptors;
        public static IReadOnlyList<HostCliActionDescriptor> ExpansionDescriptors => _expansionDescriptors;
        public static IReadOnlyList<HostCliActionDescriptor> HostDomainDescriptors => _hostDomainDescriptors;
        public static IReadOnlyList<HostCliActionDescriptor> UiDescriptors => _uiDescriptors;
        public static IReadOnlyList<HostCliActionDescriptor> ConfigDescriptors => _configDescriptors;
        public static IReadOnlyList<HostCliActionDescriptor> InfoDescriptors => _infoDescriptors;

        static HostCliRegistry()
        {
            var list = new List<HostCliActionDescriptor>(
                _coreDescriptors.Length +
                _expansionDescriptors.Length +
                _hostDomainDescriptors.Length +
                _uiDescriptors.Length +
                _configDescriptors.Length +
                _infoDescriptors.Length);

            list.AddRange(_coreDescriptors);
            list.AddRange(_expansionDescriptors);
            list.AddRange(_hostDomainDescriptors);
            list.AddRange(_uiDescriptors);
            list.AddRange(_configDescriptors);
            list.AddRange(_infoDescriptors);

            _descriptors = list;
            _flagMap = (Dictionary<string, HostCliActionDescriptor>)ValidateDescriptors(_descriptors);
        }

        /// <summary>
        /// Validates that all registered primary flags and aliases across all descriptors are strictly unique.
        /// Throws <see cref="InvalidOperationException"/> on duplicate primary flags or aliases.
        /// </summary>
        public static IReadOnlyDictionary<string, HostCliActionDescriptor> ValidateFlagRegistry()
        {
            return ValidateDescriptors(_descriptors);
        }

        /// <summary>
        /// Validates an arbitrary collection of descriptors for duplicate primary flags or aliases.
        /// </summary>
        public static IReadOnlyDictionary<string, HostCliActionDescriptor> ValidateDescriptors(IEnumerable<HostCliActionDescriptor> descriptors)
        {
            if (descriptors == null) throw new ArgumentNullException(nameof(descriptors));

            var flagMap = new Dictionary<string, HostCliActionDescriptor>(StringComparer.OrdinalIgnoreCase);
            foreach (var desc in descriptors)
            {
                if (string.IsNullOrWhiteSpace(desc.PrimaryFlag))
                {
                    throw new InvalidOperationException($"HostCliAction '{desc.Action}' has an empty or null primary flag.");
                }

                foreach (var flag in desc.AllFlags)
                {
                    if (string.IsNullOrWhiteSpace(flag))
                    {
                        throw new InvalidOperationException($"HostCliAction '{desc.Action}' has an empty or whitespace flag string.");
                    }

                    if (flagMap.TryGetValue(flag, out var existing))
                    {
                        throw new InvalidOperationException(
                            $"Duplicate CLI flag '{flag}' detected on action '{desc.Action}'. It conflicts with existing action '{existing.Action}' (primary flag: '{existing.PrimaryFlag}').");
                    }
                    flagMap[flag] = desc;
                }
            }
            return flagMap;
        }

        public static HostCliAction Resolve(string[]? args)
        {
            ValidateFlagRegistry();
            if (args == null || args.Length == 0) return HostCliAction.Interactive;

            for (int i = 0; i < args.Length; i++)
            {
                string flag = args[i];
                int eqIdx = flag.IndexOf('=');
                if (eqIdx >= 0)
                {
                    flag = flag.Substring(0, eqIdx);
                }

                if (_flagMap.TryGetValue(flag, out var desc))
                {
                    if (desc.Action == HostCliAction.UserDataDirConfig || desc.Action == HostCliAction.LogDirConfig)
                    {
                        if (eqIdx < 0 && i + 1 < args.Length && !args[i + 1].StartsWith("-"))
                        {
                            i++;
                        }
                        continue;
                    }

                    return desc.Action;
                }
            }

            return HostCliAction.Interactive;
        }

        public static void PrintHelp(Action<string> print)
        {
            ValidateFlagRegistry();
            print("ASHFALL Godot host flags (after --):");
            foreach (var category in Categories)
            {
                print($"\n--- {category} ---");
                foreach (var desc in _descriptors.Where(d => d.Category == category))
                {
                    print(desc.FormatHelpLine());
                }
            }
        }

        public static void PrintSelfTests(Action<string> print)
        {
            ValidateFlagRegistry();
            var testDescriptors = _descriptors
                .Where(d => d.IsTest)
                .OrderBy(d => d.Category)
                .ThenBy(d => d.TestId)
                .ToList();

            print($"ASHFALL Registered Self-Tests ({testDescriptors.Count} total):");
            string currentCategory = null;
            foreach (var test in testDescriptors)
            {
                if (test.Category != currentCategory)
                {
                    currentCategory = test.Category;
                    print($"\n--- {currentCategory} ---");
                }
                string flags = test.Aliases.Count > 0
                    ? $"{test.PrimaryFlag} ({string.Join(", ", test.Aliases)})"
                    : test.PrimaryFlag;
                print($"  [{test.TestId}] {flags}");
                print($"      {test.Description}");
            }
        }

        public static HostSelfTestManifest CreateSelfTestManifest()
        {
            ValidateFlagRegistry();
            var testItems = _descriptors
                .Where(d => d.IsTest)
                .Select(d => new HostSelfTestItem
                {
                    TestId = d.TestId,
                    Action = d.Action.ToString(),
                    Category = d.Category,
                    PrimaryFlag = d.PrimaryFlag,
                    Aliases = d.Aliases.ToArray(),
                    Description = d.Description,
                    HeadlessCompatible = d.HeadlessCompatible,
                    ExpectedSummaryId = d.TestId,
                    TimeoutSeconds = d.PrimaryFlag.Contains("smoke") ? 60 : 30
                })
                .ToList();

            return new HostSelfTestManifest
            {
                SchemaVersion = "1.0.0",
                Description = "Machine-readable manifest of all registered self-tests, UI tests, and diagnostic gates in ASHFALL",
                TotalTests = testItems.Count,
                HeadlessTestCount = testItems.Count(t => t.HeadlessCompatible),
                Tests = testItems
            };
        }

        public static string GenerateJsonManifest()
        {
            var manifest = CreateSelfTestManifest();
            return JsonSerializer.Serialize(manifest, new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });
        }

        public static string GenerateMarkdownCatalog(string verifiedDate)
        {
            ValidateFlagRegistry();
            var sb = new StringBuilder();
            sb.AppendLine("# ASHFALL — Host CLI Command Catalog");
            sb.AppendLine();
            sb.AppendLine($"**Last Verified:** {verifiedDate}<br>");
            int totalTokens = _descriptors.Sum(d => 1 + d.Aliases.Count);
            sb.AppendLine($"**Total Registered Actions:** {_descriptors.Count} entries / {totalTokens} flag tokens (aliases included)");
            sb.AppendLine();
            sb.AppendLine("> **GENERATED FILE — do not edit by hand.**");
            sb.AppendLine("> Source of truth: the live `godot --headless --path . -- --host-help`");
            sb.AppendLine("> output (`HostCli.PrintHelp` in `src/Host/HostCli.cs` and its partials).");
            sb.AppendLine("> Owning runner code for each verb lives under `src/` (grep the flag name).");
            sb.AppendLine("> Regenerate: `bash scripts/ci/generate-cli-catalog.sh`");
            sb.AppendLine("> Drift gate: `bash scripts/ci/generate-cli-catalog.sh --check` (fails on drift)");
            sb.AppendLine("> Exit Codes & Output Protocol: [`HOST_TEST_EXIT_CODES.md`](HOST_TEST_EXIT_CODES.md)");
            sb.AppendLine();
            sb.AppendLine("| Primary Flag | Aliases | Description |");
            sb.AppendLine("|---|---|---|");

            var sortedDescriptors = _descriptors.OrderBy(d => d.PrimaryFlag, StringComparer.OrdinalIgnoreCase).ToList();
            foreach (var desc in sortedDescriptors)
            {
                string primary = string.IsNullOrEmpty(desc.ValuePlaceholder)
                    ? $"`{desc.PrimaryFlag}`"
                    : $"`{desc.PrimaryFlag}` `{desc.ValuePlaceholder}`";
                string aliases = desc.Aliases.Count > 0
                    ? string.Join(", ", desc.Aliases.Select(a => $"`{a}`"))
                    : "—";
                sb.AppendLine($"| {primary} | {aliases} | {desc.Description} |");
            }

            return sb.ToString();
        }
    }

    public sealed class HostSelfTestManifest
    {
        [JsonPropertyName("schema_version")]
        public string SchemaVersion { get; set; } = "1.0.0";

        [JsonPropertyName("description")]
        public string Description { get; set; } = "";

        [JsonPropertyName("total_tests")]
        public int TotalTests { get; set; }

        [JsonPropertyName("headless_test_count")]
        public int HeadlessTestCount { get; set; }

        [JsonPropertyName("tests")]
        public List<HostSelfTestItem> Tests { get; set; } = new List<HostSelfTestItem>();
    }

    public sealed class HostSelfTestItem
    {
        [JsonPropertyName("test_id")]
        public string TestId { get; set; } = "";

        [JsonPropertyName("action")]
        public string Action { get; set; } = "";

        [JsonPropertyName("category")]
        public string Category { get; set; } = "";

        [JsonPropertyName("primary_flag")]
        public string PrimaryFlag { get; set; } = "";

        [JsonPropertyName("aliases")]
        public string[] Aliases { get; set; } = Array.Empty<string>();

        [JsonPropertyName("description")]
        public string Description { get; set; } = "";

        [JsonPropertyName("headless_compatible")]
        public bool HeadlessCompatible { get; set; }

        [JsonPropertyName("expected_summary_id")]
        public string ExpectedSummaryId { get; set; } = "";

        [JsonPropertyName("timeout_seconds")]
        public int TimeoutSeconds { get; set; } = 30;
    }
}
