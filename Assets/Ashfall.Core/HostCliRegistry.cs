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
        YearTwoChapterSelfTest,
        NpcMemorySelfTest,
        IdeologicalFrictionSelfTest,
        RomanceFamilySelfTest,
        VehicleCustomizationSelfTest,
        BackstorySelfTest,
        MetaProgressionSelfTest,
        TradeRoutesSelfTest,
        HumanMigrationSelfTest,
        TunnelNetworkSelfTest,
        AudioAccessibilitySelfTest,
        ModSupportSelfTest,
        ShelterIdentitySelfTest,
        OriginMechanicsSelfTest,
        DynamicQuestSelfTest,
        ShelterGovernanceSelfTest,
        AgingSelfTest,
        DifficultySettingsSelfTest,
        RailTrackMaintenanceSelfTest,
        GlassworksSelfTest,
        BroadsheetPressSelfTest,
        WildlifeHarvestSelfTest,
        YoaIceRoadSelfTest,
        SubsidenceSelfTest,
        InformantNetworkSelfTest,
        TradeRouteRiskSelfTest,
        StormForecastSelfTest,
        KilnworksSelfTest,
        DependencyTaperWithdrawalSelfTest,
        AntenatalMaternalHealthSelfTest,
        ClinicalWardTriageSelfTest,
        ChemicalReagentSynthesisSelfTest,
        MechanicalDrivelineSelfTest,
        SleepAcousticRestSelfTest,
        ShelterArchiveSelfTest,
        DreamSystemSelfTest,
        AccessibilitySettingsSelfTest,
        ShelterMaintenanceSelfTest,
        SurvivorRoutinesSelfTest,
        OrphanSealWave1SelfTest,
        WarlordUiSelfTest,
        FactionCommuniqueBoardSelfTest,
        ResourceMassBalanceSelfTest,
        StoreCapabilitySelfTest,
        LedgerTruthGateSelfTest,
        BootstrapLifecycleGateSelfTest,
        DifficultyConsequenceSelfTest,
        LivingMapRouteSelfTest,
        UndergroundEconomyPressureSelfTest,
        RehabilitationSlateSelfTest,
        RescuedArcSelfTest,
        CompletionHistorySelfTest,
        StringFreezeSelfTest,
        RehabilitationProgressionSelfTest,
        RestockAllocationSelfTest,
        ProstheticConditionWearSelfTest,
        SurvivorBodyPresentationSelfTest,
        EpilogueChronicleSelfTest,

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
        WaterSourcesSelfTest,
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
        FailureRestartSelfTest,
        FoodLoopSelfTest,
        ReasonablePlayerSelfTest,
        EbPvdCoatingUiTest,
        MicrofluidicDiagnosticUiTest,
        MineFlailUiTest,
        RailGrindingUiTest,
        PropagandaSelfTest,
        RumorNetworkSelfTest,
        ShelterSecuritySelfTest,
        PersonalQuestSelfTest,
        TimeCapsuleSelfTest,
        InternalCommunicationSelfTest,
        DeathLegacySelfTest,
        RelationshipDecaySelfTest,
        VisitorIntegrationSelfTest,
        PersonalBelongingsSelfTest,
        MemoryDecaySelfTest,
        InterpersonalConflictSelfTest,
        ExerciseSelfTest,
        WorldIncidentsSelfTest,
        SurvivorRolesSelfTest,
        ShelterMuseumSelfTest,
        RationingSelfTest,
        GenealogySelfTest,
        SurgicalGraftSelfTest,
        PharmaceuticalTabletSelfTest,
        TradeTellSelfTest,
        PowerLoadSheddingSelfTest,
        SpiritualRitualSelfTest,
        TraumaBondSelfTest,
        MigrationConsequenceSelfTest,
        WarlordResponseSelfTest,
        PatrolRadioSelfTest,
        ModalTravelDispatchSelfTest,
        RationConflictSelfTest,
        VoluntaryRegisterSelfTest,
        WorldEvolutionSelfTest,
        CassettePlaybackSelfTest,
        GuiltSourcesSelfTest,
        BlackFlotillaStandingSelfTest,
        PatientRecordIntegritySelfTest,
        CombatDoctrineSelfTest,
        GraveEpitaphsSelfTest,
        PatrolEncounterIntegritySelfTest,
        PlayerSurfaceManifestSelfTest,
        ThermalStormSealSelfTest,
        GenealogyFamilyNamesSelfTest,
        RelationshipBandsSelfTest,
        CaravanItemValueSelfTest,
        EconomyFamilySelfTest,
        ExpeditionFamilySelfTest,
        AfflictionBridgeSelfTest,
        RadiationMutationSelfTest,
        RadioProgramProductionSelfTest,
        WorkingAnimalsSelfTest,
        BlackMarketSelfTest,
        CultureCreationSelfTest,
        PsychologicalProfileSelfTest,
        SkillCertificationSelfTest,
        ChildDevelopmentSelfTest,
        BestiarySelfTest,
        HealthHistorySelfTest,
        LeadershipSuccessionSelfTest,
        RecruitmentSelfTest,
        ClothingWarmthSelfTest,
        EmergencyAlertSelfTest,
        DiplomacySelfTest,
        RadiationEconomySelfTest,
        RadiationSocialSelfTest,
        TrophySelfTest,
        BarterSelfTest,
        PerimeterEarlyWarningSelfTest,
        SkillAtrophySelfTest,
        ProceduralEulogySelfTest,
        PalliativeCareSelfTest,
        WaterQualityProfileSelfTest,
        WeatherForecastReliabilitySelfTest,
        ApprenticeshipCurriculumSelfTest,
        KnockWhitelistSelfTest,
        SecondGenerationMilestonesSelfTest,
        JourneyDiagnosticsSelfTest,
        CloudSeedingSelfTest,
        ChemicalPlumeSelfTest,
        OilseedPressingSelfTest,
        VerdictAccusationSelfTest,
        LoanSharkSelfTest,
        CommonTableRationingSelfTest,
        EmergencyMusterReadinessSelfTest,
        SoilReclamationProfileSelfTest,
        CampaignActionLogSelfTest,
        ChronicConditionSelfTest,

        // DEBT-HOSTCLI-PROBE-MANIFEST-GAP: dispatched host probes with no prior Core descriptor.
        CampaignFuzzSelfTest,
        CompositionRootSelfTest,
        ContentUtilizationSelfTest,
        ExportParitySelfTest,
        ModSelfTest,
        NarrativeContinuitySelfTest,
        PowerGridCatalogSelfTest,
        StartingCohortLifecycleSelfTest,
        StartingSuppliesSelfTest,
        CartographySelfTest,
        DynamicWorldSelfTest,
        ExpansionDepthSelfTest,
        OralLoreSelfTest,
        TrappingHostSelfTest,
        WastelandInhabitantsSelfTest,
        WorldExplorationSelfTest,
        ChemicalReconUiTest,
        DeconAirlockUiTest,
        GeodeticSurveyUiTest,
        GeothermalAquiferSelfTest,
        KineticStorageUiTest,
        Plans198To201UiTest,
        ReconTelemetrySelfTest,
        WorkshopRelicUiTest,
        SceneBindingSelfTest
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
                    new[] { "--port-contracts-selftest" },
                    "Plan 36: Port Contracts & Host Wiring — validates all Core integration seams against docs/ci/port_contract_policy.json and runtime host collaborator/port wiring"),
                new HostCliActionDescriptor(
                    HostCliAction.CampaignFuzzSelfTest,
                    "Core & System Gates",
                    "--campaign-fuzz-selftest",
                    null,
                    "Host-layer CI gate wrapper for the Core campaign fuzz harness (fuzz assertions themselves live in Ashfall.Core.Tests; this verb exists so CI can gate the suite through the same headless verb used by every other gate)"),
                new HostCliActionDescriptor(
                    HostCliAction.CompositionRootSelfTest,
                    "Core & System Gates",
                    "--composition-root-selftest",
                    null,
                    "Architecture regression test for the campaign composition root: ComposeCampaign() constructs all expected services, is idempotent across repeated calls, and opening panels in shuffled order never constructs a service outside ComposeCampaign()"),
                new HostCliActionDescriptor(
                    HostCliAction.ContentUtilizationSelfTest,
                    "Core & System Gates",
                    "--content-utilization-selftest",
                    new[] { "--content-utilization" },
                    "Deterministic diagnostic mode that exercises representative runtime content wiring and generates the utilization manifest"),
                new HostCliActionDescriptor(
                    HostCliAction.ExportParitySelfTest,
                    "Core & System Gates",
                    "--export-parity-selftest",
                    null,
                    "Plan VIII Task 23 packaged-data parity gate: proves the exported Linux layout (exe + .pck + loose StreamingAssets data) carries the same authoritative catalogs as the repository data authority",
                    "[--parity-target <path>]"),
                new HostCliActionDescriptor(
                    HostCliAction.ModSelfTest,
                    "Core & System Gates",
                    "--mod-selftest",
                    new[] { "--mods-selftest" },
                    "Mod-loading self-test: builds a temp Data+Mods sandbox, loads a well-formed sample mod and a mod referencing an unsafe path-traversal target, and verifies the safe mod applies while the unsafe one is rejected"),
                new HostCliActionDescriptor(
                    HostCliAction.NarrativeContinuitySelfTest,
                    "Core & System Gates",
                    "--narrative-continuity-selftest",
                    null,
                    "Deterministic diagnostic mode that runs the Core narrative continuity engine over the corpus and writes artifacts"),
                new HostCliActionDescriptor(
                    HostCliAction.PowerGridCatalogSelfTest,
                    "Core & System Gates",
                    "--power-grid-catalog-selftest",
                    null,
                    "SHELTER_GRID_CATALOG_SEAL Phase 5: verifies power_grid.json is the runtime authority — catalog loads via the Core loader, the session carries every shipped room, canonical consumer room IDs resolve via IsRoomPowered, and fluid-network power derivation is nominal when all breakers are closed"),
                new HostCliActionDescriptor(
                    HostCliAction.StartingCohortLifecycleSelfTest,
                    "Core & System Gates",
                    "--starting-cohort-lifecycle-selftest",
                    new[] { "--cohort-lifecycle-selftest" },
                    "Plan 138 lifecycle proof: exercises the production New Game and Continue routes in an isolated save root without relying on the broader combat/trade journey"),
                new HostCliActionDescriptor(
                    HostCliAction.StartingSuppliesSelfTest,
                    "Core & System Gates",
                    "--starting-supplies-selftest",
                    new[] { "--starting-profile-selftest" },
                    "Verifies the starting supplies catalog loads and resolves against the item catalog for every starting profile")
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
                    new[] { "--outposts-selftest" },
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
                    HostCliAction.YearTwoChapterSelfTest,
                    "Expansions & Campaign Modules",
                    "--year-two-chapter-selftest",
                    new[] { "--play-on-selftest", "--chapter-selftest" },
                    "Year Two Package P2: Play On chapter continuation, dual verb reading, and non-terminal continuation gate"),
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
                    HostCliAction.TradeRoutesSelfTest,
                    "Expansions & Campaign Modules",
                    "--trade-routes-selftest",
                    new[] { "--trade-route-selftest" },
                    "Plan 192 scheduled trade route contracts: tariffs, reliability tiers, exclusive goods, and cancellation cooldown"),
                new HostCliActionDescriptor(
                    HostCliAction.HumanMigrationSelfTest,
                    "Expansions & Campaign Modules",
                    "--human-migration-selftest",
                    new[] { "--migration-selftest" },
                    "Plan 199 seasonal human migration engine: regional population weights, seasonal dwell hysteresis, and transition scheduling"),
                new HostCliActionDescriptor(
                    HostCliAction.TunnelNetworkSelfTest,
                    "Expansions & Campaign Modules",
                    "--tunnel-network-selftest",
                    new[] { "--tunnel-selftest" },
                    "Plan 167 underground tunnel network: authored topology, discovery, integrity/hazards, daily wear, and census"),
                new HostCliActionDescriptor(
                    HostCliAction.AudioAccessibilitySelfTest,
                    "Presentation & Accessibility",
                    "--audio-accessibility-selftest",
                    new[] { "--audio-access-selftest" },
                    "Plan 169 audio accessibility & mix legibility: visual cue mapping, side-chain ducking, alert coalescing, and mix presets"),
                new HostCliActionDescriptor(
                    HostCliAction.ModSupportSelfTest,
                    "Content & Modding",
                    "--mod-support-selftest",
                    new[] { "--mod-contract-selftest" },
                    "Plan 165 modding support & mod data contract: manifest specification, version ranges, dependency DAG, conflicts, and load order"),
                new HostCliActionDescriptor(
                    HostCliAction.ShelterIdentitySelfTest,
                    "Expansions & Campaign Modules",
                    "--shelter-identity-selftest",
                    new[] { "--shelter-naming-selftest" },
                    "Plan 166 shelter identity & naming: authored origins, name/motto/emblem validation, infamy, community legacy tags, and persistence"),
                new HostCliActionDescriptor(
                    HostCliAction.OriginMechanicsSelfTest,
                    "Survivors & Identity",
                    "--origin-mechanics-selftest",
                    new[] { "--mechanical-origin-selftest" },
                    "C3-174 mechanical origin effects seam: enriched survivor skill, trade specialty, and keepsake resolution and host application"),
                new HostCliActionDescriptor(
                    HostCliAction.DynamicQuestSelfTest,
                    "Quests & Narrative",
                    "--dynamic-quest-selftest",
                    new[] { "--dynamic-quests-selftest" },
                    "Plan 171 dynamic quest generation: authored templates, deterministic candidate generation, lifecycle, deadlines, and census"),
                new HostCliActionDescriptor(
                    HostCliAction.ShelterGovernanceSelfTest,
                    "Expansions & Campaign Modules",
                    "--shelter-governance-selftest",
                    new[] { "--governance-selftest" },
                    "Plan 159 shelter governance & political system: ideological blocs, policy consent, civil disputes, and shelter stability"),
                new HostCliActionDescriptor(
                    HostCliAction.AgingSelfTest,
                    "Expansions & Campaign Modules",
                    "--aging-selftest",
                    new[] { "--elderly-survivor-selftest" },
                    "Plan 176 aging & elderly survivor system: chronological age progression, life stages, retirement, elder mentorship, and milestones"),
                new HostCliActionDescriptor(
                    HostCliAction.DifficultySettingsSelfTest,
                    "Expansions & Campaign Modules",
                    "--difficulty-settings-selftest",
                    new[] { "--difficulty-sliders-selftest" },
                    "Plan 181 difficulty settings system: preset selection, custom slider lanes, clamp bounds, ironman lock enforcement, and save/restore"),
                new HostCliActionDescriptor(
                    HostCliAction.RailTrackMaintenanceSelfTest,
                    "Expansions & Campaign Modules",
                    "--rail-track-maintenance-selftest",
                    new[] { "--iron-road-selftest" },
                    "Expansion 25 Iron Road rail track maintenance: gauge stability, wear, bridge load feasibility, workgang repair, and the segment ledger"),
                new HostCliActionDescriptor(
                    HostCliAction.GlassworksSelfTest,
                    "Expansions & Campaign Modules",
                    "--glassworks-selftest",
                    new[] { "--the-glass-selftest" },
                    "Expansion 29 The Glass vitrification: batch annealing, purity tiers, corrective lens grinding, theodolite calibration, and vision prescriptions"),
                new HostCliActionDescriptor(
                    HostCliAction.BroadsheetPressSelfTest,
                    "Expansions & Campaign Modules",
                    "--broadsheet-press-selftest",
                    new[] { "--the-press-selftest" },
                    "Expansion 30 The Press: movable type wear and reset, ink/paper consumables, print runs by publication kind, reach and morale stabilization, rumor debunk correction, and the printed archive"),
                new HostCliActionDescriptor(
                    HostCliAction.KilnworksSelfTest,
                    "Expansions & Campaign Modules",
                    "--kilnworks-selftest",
                    new[] { "--the-kiln-selftest" },
                    "Expansion 31 The Kiln: batch firing stages, thermal shock, draw grades, lime calcination yield, refractory lining wear and reline, fuel reserve, and the fired-output tallies"),
                new HostCliActionDescriptor(
                    HostCliAction.WildlifeHarvestSelfTest,
                    "Expansions & Campaign Modules",
                    "--wildlife-harvest-selftest",
                    new[] { "--the-wild-selftest" },
                    "Expansion 32 The Wild: sustainable harvest quota from reproduction surplus, overhunt collapse risk, predator conflict posture, and taming readiness"),
                new HostCliActionDescriptor(
                    HostCliAction.TradeRouteRiskSelfTest,
                    "Expansions & Campaign Modules",
                    "--trade-route-risk-selftest",
                    Array.Empty<string>(),
                    "ORPHAN-SEAL A.04: trade-route transit risk binding — deterministic raid/disruption/attrition evaluation over committed contracts"),
                new HostCliActionDescriptor(
                    HostCliAction.InformantNetworkSelfTest,
                    "Expansions & Campaign Modules",
                    "--informant-network-selftest",
                    new[] { "--the-network-selftest" },
                    "Plan 146 batch-4 / A.83: informant tradecraft — recruitment, method exposure, deterministic ops, drift, interrogation doctrine, sweeps, save ride"),
                new HostCliActionDescriptor(
                    HostCliAction.SubsidenceSelfTest,
                    "Expansions & Campaign Modules",
                    "--subsidence-selftest",
                    new[] { "--the-underneath-selftest" },
                    "Plan 146 batch-4 / A.56: subterranean subsidence strata crosswalk, daily integrity decay, shoring mitigation, evacuation gate, and topology evaluation"),
                new HostCliActionDescriptor(
                    HostCliAction.YoaIceRoadSelfTest,
                    "Expansions & Campaign Modules",
                    "--yoa-ice-road-selftest",
                    Array.Empty<string>(),
                    "Plan 146 Year of Ash residual: ice-road open/close against the −20 °C threshold, blocking storm windows, trade multipliers, expedition exposure, and envelope ride-along"),
                new HostCliActionDescriptor(
                    HostCliAction.StormForecastSelfTest,
                    "Expansions & Campaign Modules",
                    "--storm-forecast-selftest",
                    new[] { "--the-weather-selftest" },
                    "Expansion 33 The Weather: forecast confidence vs lead time, warning issuance gate, seasonal readiness composite, and black-rain absorption"),
                new HostCliActionDescriptor(
                    HostCliAction.DependencyTaperWithdrawalSelfTest,
                    "Expansions & Campaign Modules",
                    "--dependency-taper-selftest",
                    new[] { "--the-habit-selftest" },
                    "Expansion 35 The Habit: chemical dependency taper schedules, withdrawal symptom bands, peer-support mitigation, and shelter care policy posture"),
                new HostCliActionDescriptor(
                    HostCliAction.AntenatalMaternalHealthSelfTest,
                    "Expansions & Campaign Modules",
                    "--antenatal-care-selftest",
                    new[] { "--the-quickening-selftest" },
                    "Expansion 37 The Quickening: antenatal trimester progression, maternal nutritional demand, clinic readiness, and neonatal delivery resolution"),
                new HostCliActionDescriptor(
                    HostCliAction.ClinicalWardTriageSelfTest,
                    "Expansions & Campaign Modules",
                    "--clinical-ward-selftest",
                    new[] { "--the-ward-selftest" },
                    "Expansion 38 The Ward: clinical triage priority, surgical suite readiness, sterile consumable supply consumption, and nosocomial infection risks"),
                new HostCliActionDescriptor(
                    HostCliAction.ChemicalReagentSynthesisSelfTest,
                    "Expansions & Campaign Modules",
                    "--chemical-reagent-selftest",
                    new[] { "--the-reagent-selftest" },
                    "Expansion 39 The Reagent: chemical synthesis reactor safety, catalyst purity, stoichiometric mass balance, and acidic effluent neutralization"),
                new HostCliActionDescriptor(
                    HostCliAction.MechanicalDrivelineSelfTest,
                    "Expansions & Campaign Modules",
                    "--mechanical-driveline-selftest",
                    new[] { "--the-wheel-selftest" },
                    "Expansion 40 The Wheel: mechanical power driveline line shafts, friction transmission, machine tool tolerance, and millwright maintenance"),
                new HostCliActionDescriptor(
                    HostCliAction.SleepAcousticRestSelfTest,
                    "Expansions & Campaign Modules",
                    "--sleep-acoustic-selftest",
                    new[] { "--the-quiet-selftest" },
                    "Expansion 41 The Quiet: sleep quality index, acoustic decibel attenuation, quiet hours compliance, and sensory relief kit deployment"),
                new HostCliActionDescriptor(
                    HostCliAction.ShelterArchiveSelfTest,
                    "Expansions & Campaign Modules",
                    "--shelter-archive-selftest",
                    new[] { "--archive-system-selftest" },
                    "Plan 162 shelter history & archive: institutional memory, governance decisions, historical milestones, casualty memorials, and search indexing"),
                new HostCliActionDescriptor(
                    HostCliAction.DreamSystemSelfTest,
                    "Expansions & Campaign Modules",
                    "--dream-system-selftest",
                    new[] { "--dreams-selftest" },
                    "Plan 177 survivor dream & sleep event system: dream templates catalog, nocturnal dream generation, nightmare compounding, and psychological interpretations"),
                new HostCliActionDescriptor(
                    HostCliAction.AccessibilitySettingsSelfTest,
                    "Expansions & Campaign Modules",
                    "--accessibility-settings-selftest",
                    new[] { "--accessibility-options-selftest" },
                    "Plan 184 accessibility options system: visual, hearing, motor, and cognitive profiles, high contrast, font scaling, and custom assists"),
                new HostCliActionDescriptor(
                    HostCliAction.ShelterMaintenanceSelfTest,
                    "Expansions & Campaign Modules",
                    "--shelter-maintenance-selftest",
                    new[] { "--maintenance-selftest" },
                    "Plan 186 shelter maintenance & degradation system: component catalog validation, daily wear/degradation ticks, condition tracking, preventive maintenance tasks, repair ledger, and save/restore"),
                new HostCliActionDescriptor(
                    HostCliAction.SurvivorRoutinesSelfTest,
                    "Expansions & Campaign Modules",
                    "--survivor-routines-selftest",
                    new[] { "--routines-selftest" },
                    "Plan 188 individual survivor daily routines system: template catalog validation, hourly schedule assignments, chronotypes, activity satisfaction, conflict resolution, and save/restore"),
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
                    "Faction Communiqué Board: day-gated rendering, empty states, attribution, refresh idempotence"),
                new HostCliActionDescriptor(
                    HostCliAction.ResourceMassBalanceSelfTest,
                    "Expansions & Campaign Modules",
                    "--mass-balance-selftest",
                    new[] { "--resource-mass-balance-selftest" },
                    "Release-craft balance gate: deterministic 30-day survival-loop mass balance (water/power/nutrition/trapping/greenhouse) with invariant checks"),
                new HostCliActionDescriptor(
                    HostCliAction.StoreCapabilitySelfTest,
                    "Expansions & Campaign Modules",
                    "--store-capability-selftest",
                    new[] { "--store-manifest-selftest" },
                    "Plan 57 / Plan 48 release craft: store capability claims cannot run ahead of shipped systems and passing verification gates"),
                new HostCliActionDescriptor(
                    HostCliAction.LedgerTruthGateSelfTest,
                    "Expansions & Campaign Modules",
                    "--ledger-truth-selftest",
                    Array.Empty<string>(),
                    "EN-08 ledger truth: decision-register terminal/deferred invariants and the zero-quarantine D21 truth"),
                new HostCliActionDescriptor(
                    HostCliAction.BootstrapLifecycleGateSelfTest,
                    "Expansions & Campaign Modules",
                    "--bootstrap-lifecycle-selftest",
                    Array.Empty<string>(),
                    "EN-06 one bootstrap path: all path modes reach Ready with zero deferred seams and no unreached required subsystem"),
                new HostCliActionDescriptor(
                    HostCliAction.DifficultyConsequenceSelfTest,
                    "Expansions & Campaign Modules",
                    "--difficulty-consequence-selftest",
                    Array.Empty<string>(),
                    "EN-01 difficulty-consequence weave: war severity, crisis deadline offset, shock weight, monotonicity over difficulty scalars"),
                new HostCliActionDescriptor(
                    HostCliAction.LivingMapRouteSelfTest,
                    "Expansions & Campaign Modules",
                    "--living-map-route-selftest",
                    new[] { "--map-route-projection-selftest" },
                    "EN-02 living map route projection: canonical PlanRoute to hops, distance, flooded/amphibious hazards and tags"),
                new HostCliActionDescriptor(
                    HostCliAction.UndergroundEconomyPressureSelfTest,
                    "Expansions & Campaign Modules",
                    "--underground-economy-selftest",
                    new[] { "--market-temperature-selftest" },
                    "EN-03 underground economy pressure: heat/trust/relocation to Calm/Raised/Hot/Relocated band, price and attention multipliers"),
                new HostCliActionDescriptor(
                    HostCliAction.RehabilitationSlateSelfTest,
                    "Expansions & Campaign Modules",
                    "--rehabilitation-slate-selftest",
                    new[] { "--prosthetics-slate-selftest" },
                    "EN-04 rehabilitation medicine slate: prosthetics count, rehab phase, quality ramp, next milestone, phantom pain"),
                new HostCliActionDescriptor(
                    HostCliAction.RescuedArcSelfTest,
                    "Expansions & Campaign Modules",
                    "--rescued-arc-selftest",
                    new[] { "--distress-rescue-arc-selftest" },
                    "EN-05 rescued survivor arc projection: distress rescue stage to None/EnRoute/Hospitalized/Integrated/Perished/Ambushed with recovery countdown"),
                new HostCliActionDescriptor(
                    HostCliAction.CompletionHistorySelfTest,
                    "Expansions & Campaign Modules",
                    "--completion-history-selftest",
                    new[] { "--chronicle-summary-selftest" },
                    "EN-07 completion history chronicle: append-only completion records to a pure per-run summary (days, milestones, endings, difficulty)"),
                new HostCliActionDescriptor(
                    HostCliAction.StringFreezeSelfTest,
                    "Expansions & Campaign Modules",
                    "--string-freeze-selftest",
                    new[] { "--localization-freeze-selftest" },
                    "D22 localization string freeze: frozen classes require structured keys; raw strings refused unless allowlisted as debt"),
                new HostCliActionDescriptor(
                    HostCliAction.RehabilitationProgressionSelfTest,
                    "Expansions & Campaign Modules",
                    "--rehabilitation-progression-selftest",
                    new[] { "--prosthetic-progression-selftest" },
                    "F14-E rehabilitation arc progression: deterministic fitting -> adaptation -> mastery permille ramp with resilience scaling"),
                new HostCliActionDescriptor(
                    HostCliAction.RestockAllocationSelfTest,
                    "Expansions & Campaign Modules",
                    "--restock-allocation-selftest",
                    new[] { "--restock-allocation-engine-selftest" },
                    "F13-C restock capacity allocation: effective weights, scarcity floors, largest-remainder rounding, and rational stock/target_par sort"),
                new HostCliActionDescriptor(
                    HostCliAction.ProstheticConditionWearSelfTest,
                    "Expansions & Campaign Modules",
                    "--prosthetic-wear-selftest",
                    new[] { "--prosthetic-condition-selftest" },
                    "F14-D prosthetic condition & wear: daily wear, complexity-tier efficiency caps, and failure risk permille"),
                new HostCliActionDescriptor(
                    HostCliAction.SurvivorBodyPresentationSelfTest,
                    "Expansions & Campaign Modules",
                    "--body-presentation-selftest",
                    new[] { "--limb-presentation-selftest" },
                    "F14-G survivor body presentation slate: accessible limb rows, grip capability, mobility permille, maintenance and phantom-pain alerts"),
                new HostCliActionDescriptor(
                    HostCliAction.EpilogueChronicleSelfTest,
                    "Expansions & Campaign Modules",
                    "--epilogue-chronicle-selftest",
                    new[] { "--epilogue-builder-selftest" },
                    "Epilogue chronicle builder: deterministic ordering of ending slides, survivor fate cards, and metrics with ending-title mapping"),
                new HostCliActionDescriptor(
                    HostCliAction.CartographySelfTest,
                    "Expansions & Campaign Modules",
                    "--cartography-selftest",
                    new[] { "--plan16-selftest" },
                    "Plan 16 physical & institutional geography: 60-node wasteland map graph, 6 macro-regions, 6 waystations, 4 caravan circuits, 12-treaty accord web, and damaged map zones"),
                new HostCliActionDescriptor(
                    HostCliAction.DynamicWorldSelfTest,
                    "Expansions & Campaign Modules",
                    "--dynamic-world-selftest",
                    new[] { "--plan19-selftest" },
                    "Plan 19 Dynamic World Systems: weather forecasting lookahead, weather station tiers, 6-phase seasonal calendar, 18+ seasonal events, Orbital Harrow strike templates, sky armor impact cascades, salvage/site reveals, and save/load persistence"),
                new HostCliActionDescriptor(
                    HostCliAction.ExpansionDepthSelfTest,
                    "Expansions & Campaign Modules",
                    "--expansion-depth-selftest",
                    new[] { "--plan18-selftest" },
                    "Plan 18 Expansion Deepening: Holdfast (24 quests), Standing Record (52 memories, 22 quests), Crossing (20 quests, 14 encounters), Verdict (16 questlines, 9 NPCs), cross-expansion evidence hooks, and save stability"),
                new HostCliActionDescriptor(
                    HostCliAction.OralLoreSelfTest,
                    "Expansions & Campaign Modules",
                    "--oral-lore-selftest",
                    null,
                    "Oral Lore Codex self-test: loads both catalog files via the host session, verifies entry count, and exercises query methods (by id, by tag, by genre)"),
                new HostCliActionDescriptor(
                    HostCliAction.TrappingHostSelfTest,
                    "Expansions & Campaign Modules",
                    "--trapping-selftest",
                    null,
                    "Flagship trapping tranche host-layer gates for the player-facing TrySetTrap path: item billing, broken-trap replacement, atomic failure on missing materials, and the trap_active no-charge block"),
                new HostCliActionDescriptor(
                    HostCliAction.WastelandInhabitantsSelfTest,
                    "Expansions & Campaign Modules",
                    "--wasteland-inhabitants-selftest",
                    new[] { "--plan20-selftest", "--inhabitants-selftest" },
                    "Plan 20 Wasteland Inhabitants: Field Guide (32 entries), Wasteland Settlements (6 settlements, 18 named NPCs, standing greetings, trade tells), repeatable side-work quest templates, and route-aware travel encounters (24 encounters + 4 multi-stage chains)"),
                new HostCliActionDescriptor(
                    HostCliAction.WorldExplorationSelfTest,
                    "Expansions & Campaign Modules",
                    "--world-exploration-selftest",
                    new[] { "--plan11-selftest" },
                    "Plan 11 deep-strata excavation catalogs, cipher decoding loops, living geography evolution triggers, route blockades, and location memory recasts")
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
                    HostCliAction.AfflictionBridgeSelfTest,
                    "Host Domains & Save Stores",
                    "--affliction-bridge-selftest",
                    new[] { "--affliction-bridges-selftest", "--affliction-quest-work-selftest" },
                    "Plan 143: Medical Afflictions → Quest & Work Bridge: catalog loading, work modifiers, duty exclusions, quest gates, and UI projection"),
                new HostCliActionDescriptor(
                    HostCliAction.TimeCapsuleSelfTest,
                    "Host Domains & Save Stores",
                    "--time-capsule-selftest",
                    new[] { "--time-capsules-selftest" },
                    "Plan 212: Time capsule & legacy messages system, scheduled opening, save persistence, and UI binding"),
                new HostCliActionDescriptor(
                    HostCliAction.InternalCommunicationSelfTest,
                    "Host Domains & Save Stores",
                    "--internal-communication-selftest",
                    new[] { "--shelter-communications-selftest" },
                    "Plan 211: Internal shelter notices, identity refusals, private-mail privacy, canonical expiry, save/restore, and Shelter Social UI binding"),
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
                    "Plan 182: Relationship decay, social drift, bond maintenance, save persistence, and UI binding"),
                new HostCliActionDescriptor(
                    HostCliAction.VisitorIntegrationSelfTest,
                    "Host Domains & Save Stores",
                    "--visitor-integration-selftest",
                    new[] { "--visitors-selftest" },
                    "Plan 214: Admitted visitor stays, temporary housing, processing requirements, recruitment handoff, save persistence, and UI binding"),
                new HostCliActionDescriptor(
                    HostCliAction.PersonalBelongingsSelfTest,
                    "Host Domains & Save Stores",
                    "--personal-belongings-selftest",
                    new[] { "--keepsakes-selftest" },
                    "Plan 210: Survivor keepsake claims, sentimental bonding, favorites, gifts, loss reporting, inheritance, and UI binding"),
                new HostCliActionDescriptor(
                    HostCliAction.MemoryDecaySelfTest,
                    "Host Domains & Save Stores",
                    "--memory-decay-selftest",
                    new[] { "--memory-system-selftest" },
                    "Plan 185: Survivor memory & knowledge decay, cognitive degradation, reinforcement, save persistence, and UI binding"),
                new HostCliActionDescriptor(
                    HostCliAction.InterpersonalConflictSelfTest,
                    "Host Domains & Save Stores",
                    "--interpersonal-conflict-selftest",
                    new[] { "--conflict-system-selftest" },
                    "Plan 202: Interpersonal conflict & grievance, dispute escalation, mediation resolution, save persistence, and UI binding"),
                new HostCliActionDescriptor(
                    HostCliAction.ExerciseSelfTest,
                    "Host Domains & Save Stores",
                    "--exercise-selftest",
                    new[] { "--physical-training-selftest" },
                    "Plan 216: Survivor exercise & physical training, athletic conditioning, workout routines, deconditioning, save persistence, and UI binding"),
                new HostCliActionDescriptor(
                    HostCliAction.WorldIncidentsSelfTest,
                    "Host Domains & Save Stores",
                    "--world-incidents-selftest",
                    new[] { "--events-picker-selftest" },
                    "World incidents (events.json): weighted runtime picker as the third fallback of the per-day decision stream, day/flag/weather gating, scheduled follow-ups, consequence port routing, and save persistence"),
                new HostCliActionDescriptor(
                    HostCliAction.SurvivorRolesSelfTest,
                    "Host Domains & Save Stores",
                    "--survivor-roles-selftest",
                    new[] { "--specialization-roles-selftest" },
                    "Plan 195: Survivor specialization roles, discipline-gated assignment, earned practice XP, level progression, save persistence, and UI binding"),
                new HostCliActionDescriptor(
                    HostCliAction.ShelterMuseumSelfTest,
                    "Host Domains & Save Stores",
                    "--shelter-museum-selftest",
                    new[] { "--museum-selftest" },
                    "Plan 218: Shelter museum & historical archive, artifact accession, curator, exhibitions, once-per-day visits, save persistence, and UI binding"),
                new HostCliActionDescriptor(
                    HostCliAction.RationingSelfTest,
                    "Host Domains & Save Stores",
                    "--rationing-selftest",
                    new[] { "--ration-selftest" },
                    "Plan 215: Crisis rationing overlay, authored protocol catalog, lawful protocol command, inventory authorization, restore parity, and panel readout"),
                new HostCliActionDescriptor(
                    HostCliAction.GenealogySelfTest,
                    "Host Domains & Save Stores",
                    "--genealogy-selftest",
                    new[] { "--family-tree-selftest" },
                    "Plan 217: Survivor genealogy, committed kinship facts, union/birth/adoption/death lineage records, restore parity, and read-only projection"),
                new HostCliActionDescriptor(
                    HostCliAction.EconomyFamilySelfTest,
                    "Host Domains & Save Stores",
                    "--economy-family-selftest",
                    new[] { "--trade-monopoly-selftest" },
                    "PLAN-ECONOMY-DATA-FAMILY-TRUTH-270: route monopoly, contraband quoting, chit purity assay, syndicate heat attention, and composite save round-trip"),
                new HostCliActionDescriptor(
                    HostCliAction.ExpeditionFamilySelfTest,
                    "Host Domains & Save Stores",
                    "--expedition-family-selftest",
                    new[] { "--aerial-recon-selftest" },
                    "PLAN-EXPEDITION-FAMILY-TRUTH-269: aerial recon flight-window evaluation and expedition loot reference resolution/validation"),
                new HostCliActionDescriptor(
                    HostCliAction.TradeTellSelfTest,
                    "Host Domains & Save Stores",
                    "--trade-tell-selftest",
                    new[] { "--market-tell-selftest" },
                    "PLAN-TRADE-TELL-TRUTH-248: authored tell corpus load, trust-band mapping, deterministic stance x band selection, and pool coverage"),
                new HostCliActionDescriptor(
                    HostCliAction.PowerLoadSheddingSelfTest,
                    "Host Domains & Save Stores",
                    "--power-load-shedding-selftest",
                    new[] { "--grid-shedding-selftest", "--brownout-selftest" },
                    "EXPANSION-21-THE-GRID: live subgrid demand vector, priority shedding order, brownout/cascade risk, and energy-poverty morale penalty"),
                new HostCliActionDescriptor(
                    HostCliAction.SpiritualRitualSelfTest,
                    "Expansions & Campaign Modules",
                    "--spiritual-ritual-selftest",
                    new[] { "--ritual-calendar-selftest" },
                    "EXPANSION-13-THE-FAITHFUL-AND-THE-FRACTURED: authored ritual cooldown ledger, low-morale comfort scaling, holy-day windows, and save round-trip"),
                new HostCliActionDescriptor(
                    HostCliAction.TraumaBondSelfTest,
                    "Expansions & Campaign Modules",
                    "--trauma-bond-selftest",
                    new[] { "--trauma-bonds-selftest" },
                    "Trauma bond authority: shared-hazard bond formation routed to the canonical affinity owner, daily decay, and co-shift efficiency bonus"),
                new HostCliActionDescriptor(
                    HostCliAction.MigrationConsequenceSelfTest,
                    "Expansions & Campaign Modules",
                    "--migration-consequence-selftest",
                    new[] { "--migration-consequences-selftest" },
                    "XP-08-F6: regional migration consequences — market demand multipliers, labour pool, territorial friction, and exactly-once phase application"),
                new HostCliActionDescriptor(
                    HostCliAction.WarlordResponseSelfTest,
                    "Expansions & Campaign Modules",
                    "--warlord-response-selftest",
                    new[] { "--warlord-tribute-response-selftest" },
                    "Warlord tribute responses: idempotent Pay/Contest/Submit per canonical tribute id, exactly-once currency settlement, and superseded-id pruning"),
                new HostCliActionDescriptor(
                    HostCliAction.PatrolRadioSelfTest,
                    "Expansions & Campaign Modules",
                    "--patrol-radio-selftest",
                    new[] { "--patrol-radio-hooks-selftest" },
                    "Patrol radio hooks: patrol-encounter choices queue one-shot faction broadcasts delivered through the canonical radio intercept log"),
                new HostCliActionDescriptor(
                    HostCliAction.CassettePlaybackSelfTest,
                    "Culture & Audio",
                    "--cassette-playback-selftest",
                    new[] { "--cassette-sets-selftest" },
                    "Cultural cassette sets: authored catalog, tape acquisition from the live inventory, once-only first-play morale, set completion, and hidden-cache reveal"),
                new HostCliActionDescriptor(
                    HostCliAction.GuiltSourcesSelfTest,
                    "Survivors",
                    "--guilt-sources-selftest",
                    new[] { "--guilt-source-catalog-selftest" },
                    "Authored guilt sources: choice pattern resolves to authored severity and templated description instead of call-site literals"),
                new HostCliActionDescriptor(
                    HostCliAction.BlackFlotillaStandingSelfTest,
                    "Factions",
                    "--black-flotilla-standing-selftest",
                    new[] { "--flotilla-standing-selftest" },
                    "Black Flotilla standing: authored thresholds and Plan-23 trust tiers registered on the live FactionStanceEngine"),
                new HostCliActionDescriptor(
                    HostCliAction.CombatDoctrineSelfTest,
                    "Researched doctrine capability projected onto the live TacticalCombatSystem (accuracy, mobility, recoil, barrier)",
                    "--combat-doctrine-selftest",
                    new[] { "--doctrine-capability-selftest" },
                    "Combat"),
                new HostCliActionDescriptor(
                    HostCliAction.GraveEpitaphsSelfTest,
                    "Authored grave epitaphs bound to MemorialSystem.EpitaphCatalog/EpitaphRng with deterministic selection",
                    "--grave-epitaphs-selftest",
                    new[] { "--epitaph-binding-selftest" },
                    "Culture & Audio"),
                new HostCliActionDescriptor(
                    HostCliAction.PatrolEncounterIntegritySelfTest,
                    "Travel/patrol encounter integrity: duplicate ids, dangling faction/item references, unwired choices",
                    "--patrol-encounter-integrity-selftest",
                    new[] { "--travel-encounter-integrity-selftest" },
                    "Narrative"),
                new HostCliActionDescriptor(
                    HostCliAction.ThermalStormSealSelfTest,
                    "Shelter",
                    "--thermal-storm-seal-selftest",
                    new[] { "--insulation-catalog-selftest" },
                    "Authored"),
                new HostCliActionDescriptor(
                    HostCliAction.GenealogyFamilyNamesSelfTest,
                    "Survivors",
                    "--genealogy-family-names-selftest",
                    new[] { "--family-name-catalog-selftest" },
                    "Authored"),
                new HostCliActionDescriptor(
                    HostCliAction.RelationshipBandsSelfTest,
                    "Survivors",
                    "--relationship-bands-selftest",
                    new[] { "--affinity-bands-selftest" },
                    "Authored"),
                new HostCliActionDescriptor(
                    HostCliAction.CaravanItemValueSelfTest,
                    "Economy",
                    "--caravan-item-value-selftest",
                    new[] { "--canonical-item-value-selftest" },
                    "Canonical"),
                new HostCliActionDescriptor(
                    HostCliAction.PlayerSurfaceManifestSelfTest,
                    "Player surface coverage manifest generated from the live panel registry",
                    "--player-surface-manifest-selftest",
                    new[] { "--surface-manifest-selftest" },
                    "UI & Accessibility"),
                new HostCliActionDescriptor(
                    HostCliAction.PatientRecordIntegritySelfTest,
                    "Medical",
                    "--patient-record-integrity-selftest",
                    new[] { "--clinical-record-integrity-selftest" },
                    "Clinical record integrity: dangling survivor, treatment, and item references reported across the live medical pipeline (read-only)"),
                new HostCliActionDescriptor(
                    HostCliAction.RationConflictSelfTest,
                    "Survivors",
                    "--ration-conflict-selftest",
                    new[] { "--ration-resentment-selftest" },
                    "Ration conflict: perceived fairness, resentment escalation to confrontation or theft, and morale/relationship consequences through the canonical owners"),
                new HostCliActionDescriptor(
                    HostCliAction.VoluntaryRegisterSelfTest,
                    "Survivors",
                    "--voluntary-register-selftest",
                    new[] { "--volunteers-selftest" },
                    "Voluntary register: high-dose surface-work signatures, banked incurred dose, and exactly-once task completion per survivor"),
                new HostCliActionDescriptor(
                    HostCliAction.WorldEvolutionSelfTest,
                    "World",
                    "--world-evolution-selftest",
                    new[] { "--evolution-events-selftest" },
                    "World evolution: authored day-threshold and flag-gated world-state events, exactly-once triggering, and exact captured-state restore"),
                new HostCliActionDescriptor(
                    HostCliAction.ModalTravelDispatchSelfTest,
                    "World & Map",
                    "--modal-travel-dispatch-selftest",
                    new[] { "--travel-modality-selftest" },
                    "Modal travel dispatch: pre-departure feasibility, duration, fuel, and attrition projection over the live wasteland map routes"),
                new HostCliActionDescriptor(
                    HostCliAction.PharmaceuticalTabletSelfTest,
                    "Host Domains & Save Stores",
                    "--pharmaceutical-tablet-selftest",
                    new[] { "--tablet-works-selftest" },
                    "PLAN-PHARMACEUTICAL-TRUTH-167: press construction, authored formulations, batch staging, deterministic production, canonical inventory binding, and save round-trip"),
                new HostCliActionDescriptor(
                    HostCliAction.SurgicalGraftSelfTest,
                    "Host Domains & Save Stores",
                    "--surgical-graft-selftest",
                    new[] { "--graft-selftest" },
                    "PLAN-SURGICAL-WARD-TRUTH-213: graft placement, integration progress, rejection risk, immunosuppressant levels, deterministic daily tick, and save round-trip"),
                new HostCliActionDescriptor(
                    HostCliAction.RadiationMutationSelfTest,
                    "Host Domains & Save Stores",
                    "--radiation-mutation-selftest",
                    new[] { "--mutation-system-selftest" },
                    "Plan 172: Radiation mutation, genetic instability, exposure thresholds, and mutation tree progression"),
                new HostCliActionDescriptor(
                    HostCliAction.RadioProgramProductionSelfTest,
                    "Host Domains & Save Stores",
                    "--radio-production-selftest",
                    new[] { "--radio-program-production-selftest" },
                    "Plan 173: Radio station content creation, audience response, broadcast delivery, and follow-ups"),
                new HostCliActionDescriptor(
                    HostCliAction.WorkingAnimalsSelfTest,
                    "Host Domains & Save Stores",
                    "--working-animals-selftest",
                    new[] { "--companion-animal-selftest" },
                    "Plan 151: Working animals and companion system: adoption, training, guard modifiers, and pack capacity"),
                new HostCliActionDescriptor(
                    HostCliAction.BlackMarketSelfTest,
                    "Host Domains & Save Stores",
                    "--black-market-selftest",
                    new[] { "--underworld-economy-selftest" },
                    "Plan 155: Black market and underground economy: contacts, syndicate stock, loan credit, and debt enforcement"),
                new HostCliActionDescriptor(
                    HostCliAction.CultureCreationSelfTest,
                    "Host Domains & Save Stores",
                    "--culture-creation-selftest",
                    new[] { "--art-culture-selftest" },
                    "Plan 178: Art & culture creation, artworks, masterworks, cultural identity, and display morale bonuses"),
                new HostCliActionDescriptor(
                    HostCliAction.PsychologicalProfileSelfTest,
                    "Host Domains & Save Stores",
                    "--psychological-profile-selftest",
                    new[] { "--phobia-system-selftest", "--unified-psychology-selftest" },
                    "Plan 179: Unified psychology & phobia system, trauma, coping mechanisms, therapy, and resilience"),
                new HostCliActionDescriptor(
                    HostCliAction.SkillCertificationSelfTest,
                    "Host Domains & Save Stores",
                    "--skill-certification-selftest",
                    new[] { "--skill-tier-selftest", "--certifications-selftest" },
                    "Plan 180: Skill certification & tier system, exams, qualifications, and specializations"),
                new HostCliActionDescriptor(
                    HostCliAction.ChildDevelopmentSelfTest,
                    "Host Domains & Save Stores",
                    "--child-development-selftest",
                    new[] { "--child-stages-selftest" },
                    "Plan 183: Child development stages: age brackets, chore capacity, education, milestones, and canonical projection"),
                new HostCliActionDescriptor(
                    HostCliAction.BestiarySelfTest,
                    "Host Domains & Save Stores",
                    "--bestiary-selftest",
                    new[] { "--creature-encounters-selftest", "--bestiary-ui-selftest" },
                    "Plan 187: Bestiary creature tracking, 24-fauna catalog, sightings, kill/butcher counts, and tiered lore unlocks"),
                new HostCliActionDescriptor(
                    HostCliAction.HealthHistorySelfTest,
                    "Host Domains & Save Stores",
                    "--health-history-selftest",
                    new[] { "--medical-records-selftest", "--vaccination-history-selftest" },
                    "Plan 198: Health history & medical records, templates, diagnostic events, vaccination decay, and health trends"),
                new HostCliActionDescriptor(
                    HostCliAction.LeadershipSuccessionSelfTest,
                    "Host Domains & Save Stores",
                    "--leadership-succession-selftest",
                    new[] { "--succession-selftest", "--leadership-challenges-selftest" },
                    "Plan 208: Leadership succession, deputy appointment, challenges, policy enactments, and leader death succession"),
                new HostCliActionDescriptor(
                    HostCliAction.RecruitmentSelfTest,
                    "Host Domains & Save Stores",
                    "--recruitment-selftest",
                    new[] { "--defection-selftest", "--survivor-recruitment-selftest" },
                    "Plan 204: Survivor recruitment & defection campaigns, templates, offers, admission, and save round-trip"),
                new HostCliActionDescriptor(
                    HostCliAction.ClothingWarmthSelfTest,
                    "Host Domains & Save Stores",
                    "--clothing-warmth-selftest",
                    new[] { "--thermal-clothing-selftest", "--insulation-layers-selftest" },
                    "Plan 142: Clothing & warmth gear layers, condition wear, wetness penalties, and NeedsSystem cold-loss mitigation"),
                new HostCliActionDescriptor(
                    HostCliAction.EmergencyAlertSelfTest,
                    "Host Domains & Save Stores",
                    "--emergency-alert-selftest",
                    new[] { "--alert-selftest", "--emergency-warning-selftest" },
                    "Plan 194: Emergency alert types, prioritization, response windows, evacuation protocols, and save round-trip"),
                new HostCliActionDescriptor(
                    HostCliAction.DiplomacySelfTest,
                    "Host Domains & Save Stores",
                    "--diplomacy-selftest",
                    new[] { "--treaty-selftest", "--faction-diplomacy-selftest" },
                    "Faction diplomacy: treaty templates, relations, active treaties, missions, and save round-trip"),
                new HostCliActionDescriptor(
                    HostCliAction.RadiationEconomySelfTest,
                    "Host Domains & Save Stores",
                    "--radiation-economy-selftest",
                    new[] { "--contaminated-trade-selftest" },
                    "Radiation economy bridge: contamination price multipliers, trade blocks, and evaluation ledger"),
                new HostCliActionDescriptor(
                    HostCliAction.RadiationSocialSelfTest,
                    "Host Domains & Save Stores",
                    "--radiation-social-selftest",
                    new[] { "--dose-bracket-selftest" },
                    "Radiation social bridge: dose brackets, social penalties, discrimination incidents, and save round-trip"),
                new HostCliActionDescriptor(
                    HostCliAction.TrophySelfTest,
                    "Host Domains & Save Stores",
                    "--trophy-selftest",
                    new[] { "--trophies-selftest", "--trophy-mount-selftest" },
                    "Trophy mount pipeline: catalog, exactly-once quarry awards, unlocked recipes, and save round-trip"),
                new HostCliActionDescriptor(
                    HostCliAction.BarterSelfTest,
                    "Host Domains & Save Stores",
                    "--barter-selftest",
                    new[] { "--survivor-barter-selftest", "--trade-reputation-selftest" },
                    "Plan 213: Survivor barter offers, trades, trade reputation, favors, disputes, and save round-trip"),
                new HostCliActionDescriptor(
                    HostCliAction.PerimeterEarlyWarningSelfTest,
                    "Host Domains & Save Stores",
                    "--perimeter-early-warning-selftest",
                    new[] { "--radar-sweep-selftest" },
                    "Perimeter radar: sensor calibration, contact classification, false alarms, and save round-trip"),
                new HostCliActionDescriptor(
                    HostCliAction.SkillAtrophySelfTest,
                    "Host Domains & Save Stores",
                    "--skill-atrophy-selftest",
                    new[] { "--atrophy-selftest" },
                    "Skill atrophy: practice-hour decay, exactly-once events, and save round-trip"),
                new HostCliActionDescriptor(
                    HostCliAction.ProceduralEulogySelfTest,
                    "Host Domains & Save Stores",
                    "--procedural-eulogy-selftest",
                    new[] { "--eulogy-selftest" },
                    "Procedural eulogies: dweller life summary composition, archival, and save round-trip"),
                new HostCliActionDescriptor(
                    HostCliAction.PalliativeCareSelfTest,
                    "Host Domains & Save Stores",
                    "--palliative-care-selftest",
                    new[] { "--long-goodbye-selftest" },
                    "Expansion 24: palliative care pain/lucidity/dignity, grief stages, final wishes, memorial echoes, and save round-trip"),
                new HostCliActionDescriptor(
                    HostCliAction.WaterQualityProfileSelfTest,
                    "Host Domains & Save Stores",
                    "--water-quality-profile-selftest",
                    new[] { "--water-purity-selftest" },
                    "Water source contaminant profiles, purity-tier derivation, filter wear, and save round-trip"),
                new HostCliActionDescriptor(
                    HostCliAction.WeatherForecastReliabilitySelfTest,
                    "Host Domains & Save Stores",
                    "--weather-forecast-reliability-selftest",
                    new[] { "--forecast-confidence-selftest" },
                    "Weather forecast reliability: confidence grades, lead time, dispatch safety, and save round-trip"),
                new HostCliActionDescriptor(
                    HostCliAction.ApprenticeshipCurriculumSelfTest,
                    "Host Domains & Save Stores",
                    "--apprenticeship-curriculum-selftest",
                    new[] { "--curriculum-selftest" },
                    "Apprenticeship curriculum: learner literacy, subject progress, teaching sessions, and save round-trip"),
                new HostCliActionDescriptor(
                    HostCliAction.KnockWhitelistSelfTest,
                    "Host Domains & Save Stores",
                    "--knock-whitelist-selftest",
                    new[] { "--orphan-knock-selftest" },
                    "PLAN-KNOCK-WHITELIST-TRUTH-155: authored orphan-knock whitelist load, gated validation, and refusal diagnostics"),
                new HostCliActionDescriptor(
                    HostCliAction.SecondGenerationMilestonesSelfTest,
                    "Host Domains & Save Stores",
                    "--second-generation-milestones-selftest",
                    new[] { "--lineage-milestone-selftest" },
                    "PLAN-GENERATIONAL-MILESTONE-TRUTH-160: second-generation milestone evaluation, once-only recording, and capture/restore"),
                new HostCliActionDescriptor(
                    HostCliAction.JourneyDiagnosticsSelfTest,
                    "Host Domains & Save Stores",
                    "--journey-diagnostics-selftest",
                    new[] { "--journey-context-selftest" },
                    "PLAN-JOURNEY-CONTEXT-TRUTH-156: travel-context route/day/action tracking and standardized failure diagnostics"),
                new HostCliActionDescriptor(
                    HostCliAction.CloudSeedingSelfTest,
                    "Host Domains & Save Stores",
                    "--cloud-seeding-selftest",
                    new[] { "--weather-seeding-selftest" },
                    "PLAN-WEATHER-ATMOSPHERE-28 (cloud-seeding package): install, preflight, deploy, cooldown, and save round-trip"),
                new HostCliActionDescriptor(
                    HostCliAction.ChemicalPlumeSelfTest,
                    "Host Domains & Save Stores",
                    "--chemical-plume-selftest",
                    new[] { "--plume-dispersion-selftest" },
                    "PLAN-CHEMICAL-RECON-TRUTH-183: plume dispersion, shelter air infiltration, respirator protection, and save round-trip"),
                new HostCliActionDescriptor(
                    HostCliAction.OilseedPressingSelfTest,
                    "Host Domains & Save Stores",
                    "--oilseed-pressing-selftest",
                    new[] { "--seed-press-selftest" },
                    "PLAN-PRESERVATION-TRUTH-118: oilseed press install, yield evaluation, canonical inventory consumption, and save round-trip"),
                new HostCliActionDescriptor(
                    HostCliAction.VerdictAccusationSelfTest,
                    "Host Domains & Save Stores",
                    "--verdict-accusation-selftest",
                    new[] { "--tribunal-accusation-selftest" },
                    "PLAN-INVESTIGATION-EVIDENCE-TRUTH-121: typed accusation eligibility, tribunal resolution, and save round-trip"),
                new HostCliActionDescriptor(
                    HostCliAction.LoanSharkSelfTest,
                    "Host Domains & Save Stores",
                    "--loan-shark-selftest",
                    new[] { "--enforcer-debt-selftest" },
                    "PLAN-ECONOMY-LEDGER-TRUTH-96: loan issuance, escalation, repayment, trade sanction, and save round-trip"),
                new HostCliActionDescriptor(
                    HostCliAction.CommonTableRationingSelfTest,
                    "Host Domains & Save Stores",
                    "--common-table-rationing-selftest",
                    new[] { "--nutrition-diversity-selftest" },
                    "Expansion 26: common-table dietary diversity, deficiency risk, rationing policy, and save round-trip"),
                new HostCliActionDescriptor(
                    HostCliAction.EmergencyMusterReadinessSelfTest,
                    "Host Domains & Save Stores",
                    "--emergency-muster-readiness-selftest",
                    new[] { "--the-alarm-selftest" },
                    "Expansion 23 The Alarm: muster readiness, drill recency, evacuation timing, and save round-trip"),
                new HostCliActionDescriptor(
                    HostCliAction.SoilReclamationProfileSelfTest,
                    "Host Domains & Save Stores",
                    "--soil-reclamation-profile-selftest",
                    new[] { "--the-deep-root-selftest" },
                    "Expansion 15 The Deep Root: soil amendment chemistry, fertility evaluation, and save round-trip"),
                new HostCliActionDescriptor(
                    HostCliAction.CampaignActionLogSelfTest,
                    "Host Domains & Save Stores",
                    "--campaign-action-log-selftest",
                    new[] { "--action-log-selftest" },
                    "Deterministic campaign action log: sequence-stable command records, filtering, and save round-trip"),
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
                    "Medical triage, expedition sorties, radio network, crafting, respiratory afflictions, and the routed shelter operations board"),
                new HostCliActionDescriptor(
                    HostCliAction.WaterSourcesSelfTest,
                    "Water & Infrastructure",
                    "--water-sources-selftest",
                    Array.Empty<string>(),
                    "Atomic well, condenser, and piezometer commands with existing save snapshots"),
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
                    new[] { "--accessibility-selftest", "--ui-access-selftest", "--ui-a11y-selftest" },
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
                    HostCliAction.FailureRestartSelfTest,
                    "UI Tests, Layout & Gameplay Smoke",
                    "--failure-restart-selftest",
                    new[] { "--restart-journey-selftest", "--gameover-restart-selftest" },
                    "Failure & restart path proof (Task 9): New Game -> survivor deaths through the fate pipeline -> ShowGameOver terminal seal -> ReturnToMenu -> second New Game with no stale state -> Continue after a simulated crash -> corrupt campaign.json fails closed with the live session intact -> verified backup recovery"),
                new HostCliActionDescriptor(
                    HostCliAction.FoodLoopSelfTest,
                    "UI Tests, Layout & Gameplay Smoke",
                    "--food-loop-selftest",
                    null,
                    "Food loop from the UI: fresh-game starter cooking recipes are known (discovery list regression gate), kitchen panel prep -> real day advance -> pantry portions -> serve-all reduces hunger, holdfast consume seam eats/drinks, Plan 136 cooking authority live in the composed game"),
                new HostCliActionDescriptor(
                    HostCliAction.ReasonablePlayerSelfTest,
                    "UI Tests, Layout & Gameplay Smoke",
                    "--reasonable-player-selftest",
                    new[] { "--reasonable-player-bot-selftest" },
                    "Reasonable player week-1 bot: deterministic ration/cook/plant/fortify policy over real Core systems and authored data, swept across seeds and difficulty presets against the no-action baseline"),
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
                    "Rail grinding train UI panel construction, reprofiling telemetry, and stone maintenance"),
                new HostCliActionDescriptor(
                    HostCliAction.ChemicalReconUiTest,
                    "UI Tests, Layout & Gameplay Smoke",
                    "--chemical-recon-uitest",
                    new[] { "--chemical-recon-selftest" },
                    "Chemical recon UI panel construction via its action route (OPEN)"),
                new HostCliActionDescriptor(
                    HostCliAction.DeconAirlockUiTest,
                    "UI Tests, Layout & Gameplay Smoke",
                    "--decon-airlock-uitest",
                    new[] { "--decon-airlock-selftest" },
                    "Decon airlock UI panel construction via its action route (OPEN)"),
                new HostCliActionDescriptor(
                    HostCliAction.GeodeticSurveyUiTest,
                    "UI Tests, Layout & Gameplay Smoke",
                    "--geodetic-survey-uitest",
                    new[] { "--geodetic-survey-selftest" },
                    "Geodetic survey UI panel construction via its action route (OPEN)"),
                new HostCliActionDescriptor(
                    HostCliAction.GeothermalAquiferSelfTest,
                    "UI Tests, Layout & Gameplay Smoke",
                    "--geothermal-aquifer-selftest",
                    new[] { "--geothermal-uitest" },
                    "Geothermal aquifer UI panel construction and visibility toggle"),
                new HostCliActionDescriptor(
                    HostCliAction.KineticStorageUiTest,
                    "UI Tests, Layout & Gameplay Smoke",
                    "--kinetic-storage-uitest",
                    new[] { "--kinetic-storage-selftest" },
                    "Kinetic storage UI panel construction via its action route (OPEN)"),
                new HostCliActionDescriptor(
                    HostCliAction.Plans198To201UiTest,
                    "UI Tests, Layout & Gameplay Smoke",
                    "--plans198-201-uitest",
                    new[] { "--plans198-201-selftest" },
                    "Plans 198-201 end-to-end UI contract: per panel, route (registry resolve) -> bind -> visible -> command -> Core state delta -> feedback strip; any engine exception fails the gate"),
                new HostCliActionDescriptor(
                    HostCliAction.ReconTelemetrySelfTest,
                    "UI Tests, Layout & Gameplay Smoke",
                    "--recon-telemetry-uitest",
                    new[] { "--recon-telemetry-selftest" },
                    "Recon telemetry UI panel construction via its action route (OPEN)"),
                new HostCliActionDescriptor(
                    HostCliAction.WorkshopRelicUiTest,
                    "UI Tests, Layout & Gameplay Smoke",
                    "--workshop-relic-uitest",
                    new[] { "--workshop-relic-selftest" },
                    "Plan 87 follow-up QA — workshop dual-bind relic restoration path: route -> bind (shelter crafting + relic catalog) -> render (both sections present) -> select -> command (component-gated start) -> tick to completion -> state deltas (morale + world flag, each exactly once) -> save/reload"),
                new HostCliActionDescriptor(
                    HostCliAction.SceneBindingSelfTest,
                    "UI Tests, Layout & Gameplay Smoke",
                    "--scene-binding-selftest",
                    new[] { "--scene-bindings-selftest" },
                    "Loads every production scene declared via PanelSceneLoader.Load<R>() for the migrated detail panels and resolves each scene's typed unique-name node contract via SceneBinder")
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
                    HostCliAction.ChronicConditionSelfTest,
                    "Host Domains & Save Stores",
                    "--chronic-condition-selftest",
                    new[] { "--chronic-conditions-selftest", "--accommodation-selftest" },
                    "Plan 193 chronic conditions: clinical attribution, capability projection, accommodations, save replay"),
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
