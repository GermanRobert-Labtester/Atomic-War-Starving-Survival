// SPDX-License-Identifier: MIT
using Godot;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Medical;
using Ashfall.Core.Warlords;
using Ashfall.Core.Narrative;
using Ashfall.Core.Survivors;
using Ashfall.Core.World;
using Ashfall.Core.Economy;
using Ashfall.Core.UtilityAI;
using Ashfall.Core.Muster;
using Ashfall.Core.YearOfAsh;
using Ashfall.Core.Verdict;
using Ashfall.Core.Crafting;
using Ashfall.Core.Clock;
using Ashfall.Core.Events;
using Ashfall.Core.Flags;
using Ashfall.Core.Shelter;
using Ashfall.Core.Legacy;
using Ashfall.Core.Endgame;
using AtomicWar.GodotApp.YearOfAsh;
using AtomicWar.GodotApp.Settings;
using AtomicWar.GodotApp.UI;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace AtomicWar.GodotApp
{
    public enum HostCliAction
    {
        Interactive,
        Help,
        Version,
        ExportParitySelfTest,
        HoldfastSelfTest,
        IceRoadSelfTest,
        CensusSelfTest,
        CoreSelfTest,
        HoldfastBriefing,
        IceRoadTickDemo,
        HoldfastSaveSelfTest,
        HoldfastTradeSaveSelfTest,
        HoldfastRuntimeUiTest,
        BrineSelfTest,
        CombatSelfTest,
        MusterSelfTest,
        FactionEcologySelfTest,
        ClusterSelfTest,
        EndingsSelfTest,
        JournalSelfTest,
        JournalUiTest,
        DashboardUiTest,
        PlayerPanelsUiTest,
        MusterUiTest,
        DoseUiTest,
        VerdictUiTest,
        InventoryUiTest,
        SurvivorsUiTest,
        Phase0UiTest,
        BridgeSelfTest,
        PowerGridCatalogSelfTest,
        DutyRosterSelfTest,
        StandingRecordSelfTest,
        CrossingSelfTest,
        ArbitrationSelfTest,
        LedgerDebtSelfTest,
        GreenhouseSelfTest,
        AgricultureSelfTest,
        DefenseSelfTest,
        PsychologySelfTest,
        WildlifeSelfTest,
        TrappingHostSelfTest,
        PrecisionMetrologySelfTest,
        DirectionFindingSelfTest,
        AquaponicsSelfTest,
        CombatBreachingSelfTest,
        Plans139To141SelfTest,
        Plans122to125SelfTest,
        LateTechMobilitySelfTest,
        Plans122to125BalanceSoak,
        SkyDefenseSelfTest,
        VehicleGarageSelfTest,
        PortContractSelfTest,
        SilentFoundrySelfTest,
        SilentFoundryUiTest,
        DeconAirlockUiTest,
        WorkshopRelicUiTest,

        GeodeticSurveyUiTest,

        KineticStorageUiTest,

        ChemicalReconUiTest,
        Plans198To201UiTest,
        EbPvdCoatingUiTest,
        MicrofluidicDiagnosticUiTest,
        MineFlailUiTest,
        RailGrindingUiTest,
        GeothermalAquiferSelfTest,
        DiseaseSelfTest,
        DifficultySelfTest,
        DutyRosterUiTest,
        ExpansionsSelfTest,
        YearOfAshSaveSelfTest,
        VerdictSelfTest,
        DutyRosterSaveSelfTest,
        ExpansionHubSaveSelfTest,
        DoseLedgerSelfTest,
        ExpeditionSelfTest,
        ExpeditionPlaytestSelfTest,
        ExpeditionEncounterBridgeSelfTest,
        PatrolEncounterSelfTest,
        MedicalSelfTest,
        NarrativeSelfTest,
        NpcArcSelfTest,
        SurvivorsSelfTest,
        HiddenAgendaSelfTest,
        ShelterReputationSelfTest,
        WorldSelfTest,
        EconomySelfTest,
        EconomyUiTest,
        UtilityAiSelfTest,
        UtilityAiUiTest,
        DataIntegritySelfTest,
        ResearchCatalogSelfTest,
        RadioCatalogSelfTest,
        CatalogBootPreflight,
        CaravanSelfTest,
        AssetRegistrySelfTest,
        AssetCoverageReport,
        StandaloneSystemsSelfTest,
        Phase0SelfTest,
        Day1PlayableSelfTest,
        Day1ToDay2MilestoneSelfTest,
        UiLayoutSelfTest,
        SettingsSelfTest,
        PlayableShellSelfTest,
        ShelterHazardLoopSelfTest,
        ShelterOperationsSelfTest,
        ShelterDecorSelfTest,
        ShelterAtmosphereSelfTest,
        ShelterPhysicsSelfTest,
        AudioSelfTest,
        DeepCoastSelfTest,
        DeepCoastHostSelfTest,
        WarlordSelfTest,
        WarlordHostSelfTest,
        OrphanSealWave1SelfTest,
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
        WarlordUiSelfTest,
        FactionCommuniqueBoardSelfTest,
        BlackFlotillaSelfTest,
        RadioSelfTest,
        ExpeditionPanelUiTest,
        JournalSaveSelfTest,
        JournalWeatherPanelSelfTest,
        MoralChoiceSelfTest,
        EvolvingWorldSelfTest,
        WorldPlaytestSelfTest,
        SyntheticLubricantSelfTest,
        UvCoronaSelfTest,
        CarbonCompositeSelfTest,
        GprCartographySelfTest,
        AdvancedIndustrialReconSelfTest,
        InventorySaveSelfTest,
        StartingSuppliesSelfTest,
        MedicalWardSaveSelfTest,
        ChemicalDependencySaveSelfTest,
        ContrabandStashSelfTest,
        WeatherSaveSelfTest,
        SaveLoadUiFailureSelfTest,
        PanelBindLifecycleSelfTest,
        SaveStoreChecksumSelfTest,
        SevenDayDeterministicSmokeSelfTest,
        UiAccessibilitySelfTest,
        SceneBindingSelfTest,
        UiSnapshotSelfTest,
        UiSnapshotRegenerate,
        OnboardingJourneySelfTest,
        ModSelfTest,
        ContentUtilizationSelfTest,
        NarrativeContinuitySelfTest,
        SelfTestManifest,
        ListSelfTests,
        RuntimeScaleSelfTest,
        CampaignFuzzSelfTest,
        CompositionRootSelfTest,
        RealCampaignJourneySelfTest,
        StartingCohortLifecycleSelfTest,
        WorldExplorationSelfTest,
        CartographySelfTest,
        ExpansionDepthSelfTest,
        DynamicWorldSelfTest,
        WastelandInhabitantsSelfTest,
        OralLoreSelfTest,
        ReconTelemetrySelfTest,
        PropagandaSelfTest,
        RumorNetworkSelfTest,
        ShelterSecuritySelfTest,
        PersonalQuestSelfTest,
        TimeCapsuleSelfTest,
        DeathLegacySelfTest,
        RelationshipDecaySelfTest,
        ResearchUnlockSelfTest,
        UnifiedEndingSelfTest,
        NpcMemorySelfTest,
        IdeologicalFrictionSelfTest,
        RomanceFamilySelfTest,
        VehicleCustomizationSelfTest,
        BackstorySelfTest,
        MetaProgressionSelfTest,
        TradeRoutesSelfTest,
        HumanMigrationSelfTest,
        TunnelNetworkSelfTest,
        AudioAccessibilitySelfTest
    }

    /// <summary>
    /// User-args after Godot's `--`. Extra flags sit beside --ice-road-selftest;
    /// they call existing Ashfall.Core APIs and verify all 4 expansions.
    /// </summary>
    public static partial class HostCli
    {
        public static string? ExtractArgValue(string[]? args, string flag)
        {
            if (args == null || args.Length == 0) return null;
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].StartsWith(flag + "=", StringComparison.OrdinalIgnoreCase))
                {
                    return args[i].Substring(flag.Length + 1);
                }
                if (args[i].Equals(flag, StringComparison.OrdinalIgnoreCase) && i + 1 < args.Length)
                {
                    return args[i + 1];
                }
            }
            return null;
        }

        public static void ConfigureHostEnvironment(string[]? args)
        {
            string? userDir = ExtractArgValue(args, "--user-data-dir")
                ?? System.Environment.GetEnvironmentVariable("ASHFALL_USER_DIR");
            if (!string.IsNullOrWhiteSpace(userDir))
            {
                SaveSlotRoot.ConfigureUserDataDirectory(userDir);
            }

            string? logDir = ExtractArgValue(args, "--log-dir")
                ?? System.Environment.GetEnvironmentVariable("ASHFALL_LOG_DIR");
            if (!string.IsNullOrWhiteSpace(logDir))
            {
                GodotLog.ConfigureLogDirectory(logDir);
            }
        }

        public static HostCliAction Parse(string[] args)
        {
            ConfigureHostEnvironment(args);

            if (args == null || args.Length == 0)
                return HostCliAction.Interactive;

            if (Has(args, "--host-help") || Has(args, "--help"))
                return HostCliAction.Help;
            if (Has(args, "--version") || Has(args, "-v"))
                return HostCliAction.Version;
            if (Has(args, "--shelter-decor-selftest") || Has(args, "--shelter-interior-selftest") || Has(args, "--memorial-wall-selftest"))
                return HostCliAction.ShelterDecorSelfTest;
            if (Has(args, "--shelter-atmosphere-selftest") || Has(args, "--atmosphere-selftest") || Has(args, "--shelter-noise-selftest"))
                return HostCliAction.ShelterAtmosphereSelfTest;
            if (Has(args, "--hidden-agenda-selftest") || Has(args, "--hidden-agendas-selftest"))
                return HostCliAction.HiddenAgendaSelfTest;
            if (Has(args, "--shelter-reputation-selftest") || Has(args, "--reputation-selftest"))
                return HostCliAction.ShelterReputationSelfTest;
            if (Has(args, "--shelter-physics-selftest") || Has(args, "--shelter-actor-physics-selftest"))
                return HostCliAction.ShelterPhysicsSelfTest;
            if (Has(args, "--shelter-operations-selftest") || Has(args, "--operations-selftest") || Has(args, "--shelter-ops-selftest"))
                return HostCliAction.ShelterOperationsSelfTest;
            if (Has(args, "--shelter-hazard-loop-selftest") || Has(args, "--shelter-hazard-selftest") || Has(args, "--duty-roster-loop-selftest"))
                return HostCliAction.ShelterHazardLoopSelfTest;
            if (Has(args, "--ui-layout-selftest") || Has(args, "--layout-selftest"))
                return HostCliAction.UiLayoutSelfTest;
            if (Has(args, "--content-utilization-selftest") || Has(args, "--content-utilization"))
                return HostCliAction.ContentUtilizationSelfTest;
            if (Has(args, "--narrative-continuity-selftest"))
                return HostCliAction.NarrativeContinuitySelfTest;
            if (Has(args, "--settings-selftest") || Has(args, "--settings-test"))
                return HostCliAction.SettingsSelfTest;
            if (Has(args, "--playable-shell-selftest") || Has(args, "--shell-selftest") || Has(args, "--playable-loop-selftest"))
                return HostCliAction.PlayableShellSelfTest;
            if (Has(args, "--audio-selftest") || Has(args, "--audio-test"))
                return HostCliAction.AudioSelfTest;
            if (Has(args, "--day1-selftest") || Has(args, "--day-1-selftest") || Has(args, "--day1-playable-selftest"))
                return HostCliAction.Day1PlayableSelfTest;
            if (Has(args, "--day1-to-day2-selftest") || Has(args, "--day1-to-day2") || Has(args, "--day1-to-day2-milestone-selftest"))
                return HostCliAction.Day1ToDay2MilestoneSelfTest;
            if (Has(args, "--expansions-selftest") || Has(args, "--all-expansions-selftest"))
                return HostCliAction.ExpansionsSelfTest;
            if (Has(args, "--holdfast-trade-save-selftest"))
                return HostCliAction.HoldfastTradeSaveSelfTest;
            if (Has(args, "--holdfast-selftest"))
                return HostCliAction.HoldfastSelfTest;
            if (Has(args, "--duty-roster-selftest"))
                return HostCliAction.DutyRosterSelfTest;
            if (Has(args, "--standing-record-selftest"))
                return HostCliAction.StandingRecordSelfTest;
            if (Has(args, "--crossing-selftest"))
                return HostCliAction.CrossingSelfTest;
            if (Has(args, "--arbitration-selftest"))
                return HostCliAction.ArbitrationSelfTest;
            if (Has(args, "--ledger-debt-selftest"))
                return HostCliAction.LedgerDebtSelfTest;
            if (Has(args, "--greenhouse-selftest") || Has(args, "--glass-orchard-selftest"))
                return HostCliAction.GreenhouseSelfTest;
            if (Has(args, "--agriculture-selftest"))
                return HostCliAction.AgricultureSelfTest;
            if (Has(args, "--orphan-seal-wave1-selftest"))
                return HostCliAction.OrphanSealWave1SelfTest;
            if (Has(args, "--commitments-selftest"))
                return HostCliAction.CommitmentsSelfTest;
            if (Has(args, "--session-durability-selftest"))
                return HostCliAction.SessionDurabilitySelfTest;
            if (Has(args, "--playable-metrics-selftest"))
                return HostCliAction.PlayMetricsSelfTest;
            if (Has(args, "--survivor-voice-selftest"))
                return HostCliAction.SurvivorVoiceSelfTest;
            if (Has(args, "--content-certification-selftest"))
                return HostCliAction.ContentCertificationSelfTest;
            if (Has(args, "--holdfast-presentation-selftest"))
                return HostCliAction.HoldfastPresentationSelfTest;
            if (Has(args, "--scarcity-audio-selftest"))
                return HostCliAction.ScarcityAudioSelfTest;
            if (Has(args, "--seven-day-slice-selftest"))
                return HostCliAction.SliceScenarioSelfTest;
            if (Has(args, "--retention-selftest"))
                return HostCliAction.RetentionSelfTest;
            if (Has(args, "--outpost-settlement-selftest") || Has(args, "--outposts-selftest"))
                return HostCliAction.OutpostSettlementSelfTest;
            if (Has(args, "--weather-cascade-selftest"))
                return HostCliAction.WeatherCascadeSelfTest;
            if (Has(args, "--standing-gates-selftest"))
                return HostCliAction.StandingGatesSelfTest;
            if (Has(args, "--territory-control-selftest") || Has(args, "--territory-selftest"))
                return HostCliAction.TerritoryControlSelfTest;
            if (Has(args, "--cooking-selftest") || Has(args, "--cooking-test"))
                return HostCliAction.CookingSelfTest;
            if (Has(args, "--needs-performance-selftest") || Has(args, "--needs-perf-selftest"))
                return HostCliAction.NeedsPerformanceSelfTest;
            if (Has(args, "--campaign-legacy-selftest") || Has(args, "--legacy-selftest"))
                return HostCliAction.CampaignLegacySelfTest;
            if (Has(args, "--defense-selftest"))
                return HostCliAction.DefenseSelfTest;
            if (Has(args, "--psychology-selftest"))
                return HostCliAction.PsychologySelfTest;
            if (Has(args, "--wildlife-selftest"))
                return HostCliAction.WildlifeSelfTest;
            if (Has(args, "--trapping-selftest"))
                return HostCliAction.TrappingHostSelfTest;
            if (Has(args, "--precision-metrology-selftest"))
                return HostCliAction.PrecisionMetrologySelfTest;
            if (Has(args, "--direction-finding-selftest"))
                return HostCliAction.DirectionFindingSelfTest;
            if (Has(args, "--aquaponics-selftest"))
                return HostCliAction.AquaponicsSelfTest;
            if (Has(args, "--combat-breaching-selftest"))
                return HostCliAction.CombatBreachingSelfTest;
            if (Has(args, "--silent-foundry-selftest"))
                return HostCliAction.SilentFoundrySelfTest;
            if (Has(args, "--plans-122-125-balance-soak"))
                return HostCliAction.Plans122to125BalanceSoak;
            if (Has(args, "--late-tech-mobility-selftest"))
                return HostCliAction.LateTechMobilitySelfTest;
            if (Has(args, "--plans-122-125-selftest") || Has(args, "--sofc-power-selftest")
                || Has(args, "--sound-ranging-selftest") || Has(args, "--cvd-diamond-selftest")
                || Has(args, "--amphibious-draisine-selftest"))
                return HostCliAction.Plans122to125SelfTest;
            if (Has(args, "--plans-139-141-selftest") || Has(args, "--insar-selftest")
                || Has(args, "--hydraulic-extrusion-selftest") || Has(args, "--runflat-tire-selftest"))
                return HostCliAction.Plans139To141SelfTest;
            if (Has(args, "--sky-defense-selftest"))
                return HostCliAction.SkyDefenseSelfTest;
            if (Has(args, "--vehicle-garage-selftest"))
                return HostCliAction.VehicleGarageSelfTest;
            if (Has(args, "--disease-selftest") || Has(args, "--disease-expansion-selftest"))
                return HostCliAction.DiseaseSelfTest;
            if (Has(args, "--difficulty-selftest"))
                return HostCliAction.DifficultySelfTest;
            if (Has(args, "--combat-selftest"))
                return HostCliAction.CombatSelfTest;
            if (Has(args, "--silent-foundry-uitest"))
                return HostCliAction.SilentFoundryUiTest;
            if (Has(args, "--decon-airlock-selftest") || Has(args, "--decon-airlock-uitest"))

                return HostCliAction.DeconAirlockUiTest;

            if (Has(args, "--workshop-relic-uitest") || Has(args, "--workshop-relic-selftest"))

                return HostCliAction.WorkshopRelicUiTest;

            if (Has(args, "--geodetic-survey-selftest") || Has(args, "--geodetic-survey-uitest"))

                return HostCliAction.GeodeticSurveyUiTest;

            if (Has(args, "--kinetic-storage-selftest") || Has(args, "--kinetic-storage-uitest"))

                return HostCliAction.KineticStorageUiTest;

            if (Has(args, "--chemical-recon-selftest") || Has(args, "--chemical-recon-uitest"))
                return HostCliAction.ChemicalReconUiTest;
            if (Has(args, "--plans198-201-uitest") || Has(args, "--plans198-201-selftest"))
                return HostCliAction.Plans198To201UiTest;
            if (Has(args, "--ebpvd-coating-uitest") || Has(args, "--ebpvd-coating-selftest"))
                return HostCliAction.EbPvdCoatingUiTest;
            if (Has(args, "--microfluidic-diagnostic-uitest") || Has(args, "--microfluidic-diagnostic-selftest"))
                return HostCliAction.MicrofluidicDiagnosticUiTest;
            if (Has(args, "--mine-flail-uitest") || Has(args, "--mine-flail-selftest"))
                return HostCliAction.MineFlailUiTest;
            if (Has(args, "--rail-grinding-uitest") || Has(args, "--rail-grinding-selftest"))
                return HostCliAction.RailGrindingUiTest;
            if (Has(args, "--geothermal-aquifer-selftest") || Has(args, "--geothermal-uitest"))

                return HostCliAction.GeothermalAquiferSelfTest;
            if (Has(args, "--recon-telemetry-selftest") || Has(args, "--recon-telemetry-uitest"))

                return HostCliAction.ReconTelemetrySelfTest;
            if (Has(args, "--duty-roster-uitest"))
                return HostCliAction.DutyRosterUiTest;
            if (Has(args, "--core-selftest"))
                return HostCliAction.CoreSelfTest;
            if (Has(args, "--ice-road-selftest"))
                return HostCliAction.IceRoadSelfTest;
            if (Has(args, "--census-selftest"))
                return HostCliAction.CensusSelfTest;
            if (Has(args, "--holdfast-briefing"))
                return HostCliAction.HoldfastBriefing;
            if (Has(args, "--ice-road-tick-demo"))
                return HostCliAction.IceRoadTickDemo;
            if (Has(args, "--holdfast-save-selftest"))
                return HostCliAction.HoldfastSaveSelfTest;
            if (Has(args, "--holdfast-runtime-uitest") || Has(args, "--holdfast-runtime-ui-test") || Has(args, "--holdfast-runtime-selftest"))
                return HostCliAction.HoldfastRuntimeUiTest;
            if (Has(args, "--brine-selftest") || Has(args, "--salt-steam-selftest"))
                return HostCliAction.BrineSelfTest;
            if (Has(args, "--muster-selftest") || Has(args, "--expansion-06-selftest"))
                return HostCliAction.MusterSelfTest;
            if (Has(args, "--faction-ecology-selftest"))
                return HostCliAction.FactionEcologySelfTest;
            if (Has(args, "--cluster-selftest") || Has(args, "--order-12c-selftest"))
                return HostCliAction.ClusterSelfTest;
            if (Has(args, "--endings-selftest") || Has(args, "--shelf-selftest"))
                return HostCliAction.EndingsSelfTest;
            if (Has(args, "--journal-selftest"))
                return HostCliAction.JournalSelfTest;
            if (Has(args, "--journal-uitest"))
                return HostCliAction.JournalUiTest;
            if (Has(args, "--dashboard-uitest"))
                return HostCliAction.DashboardUiTest;
            if (Has(args, "--player-panels-uitest") || Has(args, "--player-panels-ui-test"))
                return HostCliAction.PlayerPanelsUiTest;
            if (Has(args, "--muster-uitest"))
                return HostCliAction.MusterUiTest;
            if (Has(args, "--inventory-uitest") || Has(args, "--inventory-selftest"))
                return HostCliAction.InventoryUiTest;
            if (Has(args, "--survivors-uitest"))
                return HostCliAction.SurvivorsUiTest;
            if (Has(args, "--phase0-uitest"))
                return HostCliAction.Phase0UiTest;
            if (Has(args, "--dose-uitest"))
                return HostCliAction.DoseUiTest;
            if (Has(args, "--bridge-selftest"))
                return HostCliAction.BridgeSelfTest;
            if (Has(args, "--power-grid-catalog-selftest"))
                return HostCliAction.PowerGridCatalogSelfTest;
            if (Has(args, "--year-of-ash-save-selftest"))
                return HostCliAction.YearOfAshSaveSelfTest;
            if (Has(args, "--verdict-selftest") || Has(args, "--expansion-08-selftest"))
                return HostCliAction.VerdictSelfTest;
            if (Has(args, "--verdict-uitest"))
                return HostCliAction.VerdictUiTest;
            if (Has(args, "--duty-roster-save-selftest"))
                return HostCliAction.DutyRosterSaveSelfTest;
            if (Has(args, "--expansion-hub-save-selftest"))
                return HostCliAction.ExpansionHubSaveSelfTest;
            if (Has(args, "--dose-ledger-selftest"))
                return HostCliAction.DoseLedgerSelfTest;
            if (Has(args, "--expedition-selftest"))
                return HostCliAction.ExpeditionSelfTest;
            if (Has(args, "--expedition-playtest-selftest"))
                return HostCliAction.ExpeditionPlaytestSelfTest;
            if (Has(args, "--expedition-encounter-bridge-selftest"))
                return HostCliAction.ExpeditionEncounterBridgeSelfTest;
            if (Has(args, "--patrol-encounter-selftest") || Has(args, "--travel-encounter-selftest"))
                return HostCliAction.PatrolEncounterSelfTest;
            if (Has(args, "--medical-selftest"))
                return HostCliAction.MedicalSelfTest;
            if (Has(args, "--narrative-selftest"))
                return HostCliAction.NarrativeSelfTest;
            if (Has(args, "--npc-arc-selftest"))
                return HostCliAction.NpcArcSelfTest;
            if (Has(args, "--survivors-selftest"))
                return HostCliAction.SurvivorsSelfTest;
            if (Has(args, "--world-selftest"))
                return HostCliAction.WorldSelfTest;
            if (Has(args, "--world-exploration-selftest") || Has(args, "--plan11-selftest"))
                return HostCliAction.WorldExplorationSelfTest;
            if (Has(args, "--cartography-selftest") || Has(args, "--plan16-selftest"))
                return HostCliAction.CartographySelfTest;
            if (Has(args, "--economy-selftest"))
                return HostCliAction.EconomySelfTest;
            if (Has(args, "--economy-uitest"))
                return HostCliAction.EconomyUiTest;
            if (Has(args, "--utility-ai-selftest"))
                return HostCliAction.UtilityAiSelfTest;
            if (Has(args, "--utility-ai-uitest"))
                return HostCliAction.UtilityAiUiTest;
            if (Has(args, "--data-integrity-selftest"))
                return HostCliAction.DataIntegritySelfTest;
            if (Has(args, "--export-parity-selftest"))
                return HostCliAction.ExportParitySelfTest;
            if (Has(args, "--research-catalog-selftest"))
                return HostCliAction.ResearchCatalogSelfTest;
            if (Has(args, "--radio-catalog-selftest"))
                return HostCliAction.RadioCatalogSelfTest;
            if (Has(args, "--catalog-boot-preflight"))
                return HostCliAction.CatalogBootPreflight;
            if (Has(args, "--caravan-selftest") || Has(args, "--traveling-caravan-selftest"))
                return HostCliAction.CaravanSelfTest;
            if (Has(args, "--asset-registry-selftest"))
                return HostCliAction.AssetRegistrySelfTest;
            if (Has(args, "--asset-coverage-report"))
                return HostCliAction.AssetCoverageReport;
            if (Has(args, "--standalone-selftest"))
                return HostCliAction.StandaloneSystemsSelfTest;
            if (Has(args, "--phase0-selftest"))
                return HostCliAction.Phase0SelfTest;
            if (Has(args, "--deep-coast-selftest") || Has(args, "--deep-coast-route-selftest"))
                return HostCliAction.DeepCoastSelfTest;
            if (Has(args, "--deep-coast-host-selftest") || Has(args, "--deep-coast-playthrough"))
                return HostCliAction.DeepCoastHostSelfTest;
            if (Has(args, "--warlord-selftest") || Has(args, "--warlord-ai-selftest"))
                return HostCliAction.WarlordSelfTest;
            if (Has(args, "--warlord-host-selftest"))
                return HostCliAction.WarlordHostSelfTest;
            if (Has(args, "--warlord-ui-selftest"))
                return HostCliAction.WarlordUiSelfTest;
            if (Has(args, "--faction-communique-board-selftest") || Has(args, "--communique-board-selftest"))
                return HostCliAction.FactionCommuniqueBoardSelfTest;
            if (Has(args, "--black-flotilla-selftest") || Has(args, "--maritime-selftest") || Has(args, "--expansion-09-selftest"))
                return HostCliAction.BlackFlotillaSelfTest;
            if (Has(args, "--radio-selftest"))
                return HostCliAction.RadioSelfTest;
            if (Has(args, "--expedition-panel-uitest") || Has(args, "--expedition-panel-lifecycle"))
                return HostCliAction.ExpeditionPanelUiTest;
            if (Has(args, "--onboarding-journey-selftest") || Has(args, "--onboarding-selftest"))
                return HostCliAction.OnboardingJourneySelfTest;
            if (Has(args, "--mod-selftest") || Has(args, "--mods-selftest"))
                return HostCliAction.ModSelfTest;
            if (Has(args, "--ui-snapshot-regenerate") || Has(args, "--ui-snapshots-regen"))
                return HostCliAction.UiSnapshotRegenerate;
            if (Has(args, "--ui-snapshot-uitest") || Has(args, "--ui-snapshots"))
                return HostCliAction.UiSnapshotSelfTest;
            if (Has(args, "--selftest-manifest") || Has(args, "--test-manifest"))
                return HostCliAction.SelfTestManifest;
            if (Has(args, "--list-selftests") || Has(args, "--list-tests") || Has(args, "--selftests"))
                return HostCliAction.ListSelfTests;
            if (Has(args, "--journal-save-selftest"))
                return HostCliAction.JournalSaveSelfTest;
            if (Has(args, "--journal-weather-panel-selftest"))
                return HostCliAction.JournalWeatherPanelSelfTest;
            if (Has(args, "--moral-choice-selftest"))
                return HostCliAction.MoralChoiceSelfTest;
            if (Has(args, "--evolving-world-selftest"))
                return HostCliAction.EvolvingWorldSelfTest;
            if (Has(args, "--world-playtest-selftest"))
                return HostCliAction.WorldPlaytestSelfTest;
            if (Has(args, "--synthetic-lubricant-selftest"))
                return HostCliAction.SyntheticLubricantSelfTest;
            if (Has(args, "--uv-corona-selftest"))
                return HostCliAction.UvCoronaSelfTest;
            if (Has(args, "--carbon-composite-selftest"))
                return HostCliAction.CarbonCompositeSelfTest;
            if (Has(args, "--gpr-cartography-selftest"))
                return HostCliAction.GprCartographySelfTest;
            if (Has(args, "--advanced-industrial-recon-selftest"))
                return HostCliAction.AdvancedIndustrialReconSelfTest;
            if (Has(args, "--inventory-save-selftest"))
                return HostCliAction.InventorySaveSelfTest;
            if (Has(args, "--starting-supplies-selftest") || Has(args, "--starting-profile-selftest"))
                return HostCliAction.StartingSuppliesSelfTest;
            if (Has(args, "--medical-ward-save-selftest"))
                return HostCliAction.MedicalWardSaveSelfTest;
            if (Has(args, "--chemical-dependency-save-selftest"))
                return HostCliAction.ChemicalDependencySaveSelfTest;
            if (Has(args, "--contraband-stash-selftest") || Has(args, "--contraband-selftest"))
                return HostCliAction.ContrabandStashSelfTest;
            if (Has(args, "--weather-save-selftest"))
                return HostCliAction.WeatherSaveSelfTest;
            if (Has(args, "--save-load-ui-failure-selftest") || Has(args, "--save-load-failure-selftest") || Has(args, "--save-load-failure-uitest") || Has(args, "--save-load-selftest"))
                return HostCliAction.SaveLoadUiFailureSelfTest;
            if (Has(args, "--panel-bind-lifecycle-selftest") || Has(args, "--panel-bind-selftest") || Has(args, "--panel-lifecycle-selftest"))
                return HostCliAction.PanelBindLifecycleSelfTest;
            if (Has(args, "--save-store-checksum-selftest") || Has(args, "--save-store-checksums-selftest") || Has(args, "--checksum-sweep-selftest"))
                return HostCliAction.SaveStoreChecksumSelfTest;
            if (Has(args, "--runtime-scale-selftest") || Has(args, "--runtime-scale") || Has(args, "--performance-selftest") || Has(args, "--perf-selftest"))
                return HostCliAction.RuntimeScaleSelfTest;
            if (Has(args, "--7-day-smoke-selftest") || Has(args, "--seven-day-smoke-selftest") || Has(args, "--deterministic-smoke-selftest") || Has(args, "--deterministic-smoke-run"))
                return HostCliAction.SevenDayDeterministicSmokeSelfTest;
            if (Has(args, "--ui-accessibility-selftest") || Has(args, "--ui-access-selftest") || Has(args, "--accessibility-selftest"))
                return HostCliAction.UiAccessibilitySelfTest;
            if (Has(args, "--scene-binding-selftest") || Has(args, "--scene-bindings-selftest"))
                return HostCliAction.SceneBindingSelfTest;
            if (Has(args, "--campaign-fuzz-selftest"))
                return HostCliAction.CampaignFuzzSelfTest;
            if (Has(args, "--composition-root-selftest"))
                return HostCliAction.CompositionRootSelfTest;
            if (Has(args, "--real-campaign-journey-selftest") || Has(args, "--campaign-journey-selftest") || Has(args, "--real-main-journey-selftest"))
                return HostCliAction.RealCampaignJourneySelfTest;
            if (Has(args, "--starting-cohort-lifecycle-selftest") || Has(args, "--cohort-lifecycle-selftest"))
                return HostCliAction.StartingCohortLifecycleSelfTest;
            if (Has(args, "--expansion-depth-selftest") || Has(args, "--plan18-selftest"))
                return HostCliAction.ExpansionDepthSelfTest;
            if (Has(args, "--dynamic-world-selftest") || Has(args, "--plan19-selftest"))
                return HostCliAction.DynamicWorldSelfTest;
            if (Has(args, "--wasteland-inhabitants-selftest") || Has(args, "--plan20-selftest") || Has(args, "--inhabitants-selftest"))
                return HostCliAction.WastelandInhabitantsSelfTest;
            if (Has(args, "--port-contract-selftest") || Has(args, "--port-contracts-selftest"))
                return HostCliAction.PortContractSelfTest;
            if (Has(args, "--oral-lore-selftest"))
                return HostCliAction.OralLoreSelfTest;
            if (Has(args, "--propaganda-selftest") || Has(args, "--propaganda-campaign-selftest"))
                return HostCliAction.PropagandaSelfTest;
            if (Has(args, "--rumor-network-selftest") || Has(args, "--rumors-selftest"))
                return HostCliAction.RumorNetworkSelfTest;
            if (Has(args, "--shelter-security-selftest") || Has(args, "--security-selftest"))
                return HostCliAction.ShelterSecuritySelfTest;
            if (Has(args, "--personal-quests-selftest") || Has(args, "--personal-quest-selftest"))
                return HostCliAction.PersonalQuestSelfTest;
            if (Has(args, "--time-capsule-selftest") || Has(args, "--time-capsules-selftest"))
                return HostCliAction.TimeCapsuleSelfTest;
            if (Has(args, "--death-legacy-selftest") || Has(args, "--wills-selftest") || Has(args, "--survivor-death-selftest"))
                return HostCliAction.DeathLegacySelfTest;
            if (Has(args, "--relationship-decay-selftest") || Has(args, "--social-drift-selftest"))
                return HostCliAction.RelationshipDecaySelfTest;
            if (Has(args, "--research-unlock-selftest") || Has(args, "--research-unlocks-selftest"))
                return HostCliAction.ResearchUnlockSelfTest;
            if (Has(args, "--unified-ending-selftest") || Has(args, "--epilogue-selftest"))
                return HostCliAction.UnifiedEndingSelfTest;
            if (Has(args, "--npc-memory-selftest") || Has(args, "--npc-memory-test"))
                return HostCliAction.NpcMemorySelfTest;
            if (Has(args, "--ideological-friction-selftest") || Has(args, "--ideology-selftest"))
                return HostCliAction.IdeologicalFrictionSelfTest;
            if (Has(args, "--romance-family-selftest") || Has(args, "--romance-selftest"))
                return HostCliAction.RomanceFamilySelfTest;
            if (Has(args, "--vehicle-customization-selftest") || Has(args, "--vehicle-modules-selftest"))
                return HostCliAction.VehicleCustomizationSelfTest;
            if (Has(args, "--backstory-selftest") || Has(args, "--backstories-selftest"))
                return HostCliAction.BackstorySelfTest;
            if (Has(args, "--meta-progression-selftest") || Has(args, "--meta-selftest"))
                return HostCliAction.MetaProgressionSelfTest;
            if (Has(args, "--trade-routes-selftest") || Has(args, "--trade-route-selftest"))
                return HostCliAction.TradeRoutesSelfTest;
            if (Has(args, "--human-migration-selftest") || Has(args, "--migration-selftest"))
                return HostCliAction.HumanMigrationSelfTest;
            if (Has(args, "--tunnel-network-selftest") || Has(args, "--tunnel-selftest"))
                return HostCliAction.TunnelNetworkSelfTest;
            if (Has(args, "--audio-accessibility-selftest") || Has(args, "--audio-access-selftest"))
                return HostCliAction.AudioAccessibilitySelfTest;
            return HostCliAction.Interactive;
        }

        public static void PrintHelp()
        {
            GD.Print("ASHFALL Godot host flags (after --):");

            GD.Print("\n--- Core & System Gates ---");
            GD.Print("  --7-day-smoke-selftest / --seven-day-smoke-selftest / --deterministic-smoke-selftest / --deterministic-smoke-run 7-day deterministic smoke run: map discovery + weather rolls + survivor needs drift + mid-run save/reload round-trip across 10 verification gates");
            GD.Print("  --accessibility-selftest / --ui-accessibility-selftest / --ui-access-selftest Verify focus order, non-empty labels, modal close handling, and accessibility compliance across UI panels");
            GD.Print("  --asset-coverage-report  Full non-gating sweep of every catalog id (core + expansions) vs loadable art; prints per-category coverage and the missing list");
            GD.Print("  --asset-registry-selftest Verify that catalog IDs (items/survivors/locations) resolve to actual texture assets under assets/");
            GD.Print("  --starting-cohort-lifecycle-selftest / --cohort-lifecycle-selftest  Plan 138 fresh-vs-restore lifecycle: preserve old slots, apply an alternate cohort, honor an empty saved roster, and reject failed restores without reseeding");
            GD.Print("  --starting-supplies-profile <id>  Preselect an authored starting-store profile in the New Game selector; invalid IDs fall back to Standard Holdfast");
            GD.Print("  --bridge-selftest        Report UnityEngine shim removal (shim is gone; always exits 0)");
            GD.Print("  --power-grid-catalog-selftest  Verify power_grid.json loads at runtime via the Core loader, canonical room IDs resolve (room_water_pump/room_workshop), and fluid power derivation is nominal");
            GD.Print("  --core-selftest          Ice road + census headless demos");
            GD.Print("  --data-integrity-selftest Cross-reference every id in the 129 StreamingAssets catalogs (recipe→item, quest→location, events, door encounters, survivors, factions, ranges, duplicates)");
            GD.Print("  --difficulty-selftest    XP-01 difficulty catalog, scalar consumers, starting bonuses, fail-closed selection, and save checksum binding");
            GD.Print("  --export-parity-selftest [--parity-target <dir>] Packaged-data parity: exported build's catalogs byte-identical + parseable vs the data authority, exact Linux casing, no LFS pointers, ELF exe + PCK present");
            GD.Print("  --catalog-boot-preflight   Machine-readable preflight: checks all catalogs are present, well-formed, and reports classification (required/optional/dev-only) with any load errors");
            GD.Print("  --panel-bind-lifecycle-selftest / --panel-bind-selftest / --panel-lifecycle-selftest Real Godot-node callback tests for panel bind → unbind → rebind, event propagation, and session-switch");
            GD.Print("  --port-contract-selftest / --port-contracts-selftest Validate all Core integration seams and host subsystem wiring contracts against port_contract_policy.json (Plan 36)");
            GD.Print("  --save-load-ui-failure-selftest / --save-load-failure-selftest / --save-load-failure-uitest / --save-load-selftest Save/load UI failure-path smoke test: missing, corrupt, and checksum-invalid saves show recoverable user messages and leave live session intact");
            GD.Print("  --save-store-checksum-selftest / --save-store-checksums-selftest / --checksum-sweep-selftest Source-scan all SaveStore files for checksum coverage + 5 in-memory round-trip probes (Weather, Map, Survivors, SaveChecksum stability, null-field guard)");
            GD.Print("  --runtime-scale-selftest / --runtime-scale / --performance-selftest / --perf-selftest Performance budget validation: 30/180/360-day campaign workloads, day-advance latency, save/load/checksum, allocations, retained memory, and lifecycle leak tests; writes artifacts/runtime-scale-results.json");
            GD.Print("  --scene-binding-selftest / --scene-bindings-selftest Headless-instantiate every registered production scene and validate each unique_name_in_owner binding contract (Ticket #125 scene-ownership gate); exits 0 when all required nodes resolve with the expected Godot types");
            GD.Print("  --content-utilization-selftest / --content-utilization Scan every JSON catalog under StreamingAssets/Data, classify each by reachable consumer (gameplay / UI / codex / orphan), write artifacts/content-utilization.{json,md}, and run the CI gate against artifacts/content-utilization-baseline.json (Ticket #127 content-runtime gate)");
            GD.Print("  --standalone-selftest    SkyLayerArmor, VigilStateMachine, GenerationalSuccession, EpilogueMatrix, DiveInstance");
            GD.Print("  --campaign-fuzz-selftest      Core-level campaign fuzz harness gate (Task #129); delegates to Ashfall.Core.Tests.CampaignFuzz suite");
            GD.Print("  --composition-root-selftest   Composition root architecture gate: verifies ComposeCampaign() is the single entry point (Task #131)");
            GD.Print("  --real-campaign-journey-selftest / --campaign-journey-selftest / --real-main-journey-selftest Real Main-composed player journey: New Game -> ComposeCampaign() -> real gameplay action -> real day advance through the coordinator -> SaveAll -> full in-memory reset -> Continue -> restored composed state (Plan #5)");

            GD.Print("\n--- Expansions & Campaign Modules ---");
            GD.Print("  --agriculture-selftest   Agriculture Expansion (Plan 162): crop strain catalog, greenhouse growth, mutation RNG, compost, nutrition");
            GD.Print("  --orphan-seal-wave1-selftest  ORPHAN-SEAL-PRIORITY-W1: ten priority orphan authorities — catalog, command, state round-trip");
            GD.Print("  --commitments-selftest   Plan 38 commitments & deadlines: catalog, warning ladder, exactly-once miss + consequence routing, met settlement, save round-trip");
            GD.Print("  --session-durability-selftest  Plan 39 session durability: slot capacity/isolation, interrupted-write + backup recovery audit, soak stability verdicts, capture round-trip");
            GD.Print("  --playable-metrics-selftest   Plan 46 playable metrics: bounded recorder stream, first-hour funnel, aggregation grades, capture round-trip");
            GD.Print("  --survivor-voice-selftest     Plan 42 survivor voice: catalog selection, cooldowns, dispatch arbitration, barrel history, capture round-trip");
            GD.Print("  --content-certification-selftest  Plan 49 content orphan certification: family manifest, live-evidence rows, clean/dormant/orphan verdicts");
            GD.Print("  --holdfast-presentation-selftest Plan 51 holdfast presentation slate: room/actor/map projections, hazard + crisis bands, motion profile");
            GD.Print("  --scarcity-audio-selftest     Plan 52 scarcity audio: weather->bed/cue authority mapping, silence states, ducking, geiger bands");
            GD.Print("  --seven-day-slice-selftest    Plan 54 seven-day slice: authored beats, frozen scenario hash, beat verification + scorecard");
            GD.Print("  --retention-selftest           Plan 55 retention & save budgeting: authored policy overlay, bounded canonical collections, protected obligations, capture round-trip");
            GD.Print("  --outpost-settlement-selftest / --outposts-selftest  Plan 58 outposts & second holdfast: authored catalog, establish/garrison/supply lifecycle, daily consume, capture round-trip");
            GD.Print("  --territory-control-selftest / --territory-selftest  Plan 134 faction territory & supply line control: contested nodes, fortification, garrison, supply line status, capture round-trip");
            GD.Print("  --cooking-selftest / --cooking-test  Plan 136 wildlife trapping food pipeline & cooking system: recipe loading, ingredient consumption, decontamination, skill progression, capture/restore");
            GD.Print("  --aquaponics-selftest    Plan B87 closed-loop aquaponics: catalog, growth, power/DO crash, harvest, nutrient export, save round-trip");
            GD.Print("  --arbitration-selftest   CrossingArbitrationHeadlessDemo");
            GD.Print("  --black-flotilla-selftest / --maritime-selftest / --expansion-09-selftest The Black Flotilla (Exp 09): catalog load, deterministic scavenge, dive rooms/air/noise, contamination, visit state, save round-trip");
            GD.Print("  --brine-selftest / --salt-steam-selftest         BrineWaterHeadlessDemo (S2 salt & steam)");
            GD.Print("  --census-selftest        CensusHeadlessDemo");
            GD.Print("  --cluster-selftest / --order-12c-selftest       Cluster12CHeadlessDemo (S3 order 12-C + quest snapshot)");
            GD.Print("  --combat-breaching-selftest Plan B86 combat breaching: catalog, quiet/loud clearance, vehicle gate, mid-breach save fields");
            GD.Print("  --combat-selftest        Combat Expansion: catalog (JSON), ballistics, weapon condition, determinism, save round-trip");
            GD.Print("  --crossing-selftest      CrossingHeadlessDemo (Exp 04)");
            GD.Print("  --deep-coast-host-selftest / --deep-coast-playthrough Deep-coast host playthrough: survey → decision → dive → scavenge → save/restore");
            GD.Print("  --deep-coast-selftest / --deep-coast-route-selftest District 8 deep-coast route: stages, decisions, Ice Road gating, dive handoff, v5 save");
            GD.Print("  --defense-selftest       Shelter Defense Expansion (Plan 163): trap catalog, installation, engagement, alarm, capture handoff");
            GD.Print("  --direction-finding-selftest Plan B88 HF/DF: catalog, baselines, skywave, fingerprint≠fix, RadioSave V3 triangulation nest");
            GD.Print("  --disease-selftest / --disease-expansion-selftest Disease Expansion: catalog, quarantine, protocols, determinism, save round-trip");
            GD.Print("  --duty-roster-selftest   DutyRosterHeadlessDemo (Exp 02)");
            GD.Print("  --endings-selftest / --shelf-selftest       EndingsHeadlessDemo (S4 endings exclusive + roundtrip)");
            GD.Print("  --expansions-selftest / --all-expansions-selftest    Run full 7-expansion verification suite (Holdfast, Duty Roster, Standing Record, Crossing, Arbitration, LedgerDebt, Glass Orchard)");
            GD.Print("  --greenhouse-selftest / --glass-orchard-selftest    GreenhouseHeadlessDemo (Exp 05)");
            GD.Print("  --psychology-selftest    Psychology Arc Expansion (Plan 164): breakdown arcs, sustained-stress triggers, catharsis, treatment");
            GD.Print("  --wildlife-selftest      Wildlife Ecosystem (Plan 165): fauna catalog, predation/radiation pressure, apex, taming, save round-trip");
            GD.Print("  --trapping-selftest      Wildlife trapping host path: TrySetTrap billing, broken-trap replacement, atomic failure, trap-recipe identity");
            GD.Print("  --holdfast-briefing      Print location count and every Holdfast quest briefing");
            GD.Print("  --holdfast-selftest      Holdfast S1 survival loop, ice road, and trade verification");
            GD.Print("  --ice-road-selftest      IceRoadHeadlessDemo (Exp 01)");
            GD.Print("  --ice-road-tick-demo     Unlock, clerk, 30 day ticks, print catalog + briefing");
            GD.Print("  --ledger-debt-selftest   LedgerDebtHeadlessDemo");
            GD.Print("  --moral-choice-selftest  Moral choice: catalog + scripted arc + bands + reconcile events + journal hook + save/tamper checks");
            GD.Print("  --evolving-world-selftest  Evolving-world activation: seeds, live weather-fed ticks, migration, expedition consequences, scarcity, save envelope, 360-day scenario");
            GD.Print("  --world-playtest-selftest  Fixed-seed 30-day evolving-world campaign proof: snapshots, downstream reads, bounds, determinism, and midpoint save/load parity");
            GD.Print("  --selftest-manifest      Emit the machine-readable self-test manifest JSON (scripts/ci/generate-selftest-manifest.py)");
            GD.Print("  --test-manifest          Alias for --selftest-manifest");
            GD.Print("  --list-selftests         List every registered selftest and run its signature live (runtime/CLI parity audit)");
            GD.Print("  --list-tests             Alias for --list-selftests");
            GD.Print("  --selftests              Alias for --list-selftests");
            GD.Print("  --list-selftest          Alias for --list-selftests");
            GD.Print("  --muster-selftest / --expansion-06-selftest        MusterHeadlessDemo (Exp 06 the Muster)");
            GD.Print("  --faction-ecology-selftest                      Plan 25 faction ecology vertical slice (action board, E-P1 chain, witness, camp scene, muster path)");
            GD.Print("  --phase0-selftest        Phase-0 effects: phantom work-eff/refusal, flashbacks, trade specialty, final-wish buff, respiratory stamina + save roundtrip");
            GD.Print("  --precision-metrology-selftest Plan B89 precision metrology: grades, registered consumers only, workshop projection, disturbance, save round-trip");
            GD.Print("  --silent-foundry-selftest Silent Foundry (Exp 10): trade stance, trust momentum, recipes, and save round-trip");
            GD.Print("  --standing-record-selftest StandingRecordHeadlessDemo (Exp 03)");
            GD.Print("  --verdict-selftest / --expansion-08-selftest The Verdict (Exp 08): machine log, reckoning phases, evidence, census, save");
            GD.Print("  --warlord-host-selftest  Warlord host playthrough: YearOfAsh wiring, standing, v3 save/tamper");
            GD.Print("  --warlord-selftest / --warlord-ai-selftest Adaptive warlord AI: doctrines, territory, tribute, determinism, v3 save");
            GD.Print("  --warlord-ui-selftest    Warlord tribute payment loop + collector voice + FactionsPanel card");
            GD.Print("  --world-exploration-selftest / --plan11-selftest World exploration: deep-strata excavation, cipher hunts, living geography evolution, and location memory");
            GD.Print("  --cartography-selftest / --plan16-selftest Cartography and infrastructure: 60-node wasteland map, 6 waystations, 4 caravan circuits, 12 accords, and damaged map zones");
            GD.Print("  --expansion-depth-selftest / --plan18-selftest Expansion deepening: Holdfast (24 quests), Standing Record (52 memories, 22 quests), Crossing (20 quests, 14 encounters), Verdict (16 questlines, 9 NPCs)");
            GD.Print("  --wasteland-inhabitants-selftest / --plan20-selftest / --inhabitants-selftest Wasteland inhabitants: NPC catalog, faction presence, encounter density, and settlement population verification");

            GD.Print("\n--- Host Domains & Save Stores ---");
            GD.Print("  --audio-selftest / --audio-test Audio cue catalog, AudioManager wiring, and sound event verification");
            GD.Print("  --caravan-selftest / --traveling-caravan-selftest Traveling caravan economy, inventory generation, and barter ticks");
            GD.Print("  --chemical-dependency-save-selftest Chemical dependency system save store round-trip, tolerance, and withdrawal states");
            GD.Print("  --contraband-stash-selftest / --contraband-selftest Plan 147 contraband stash discovery: day gate, once-only claim, canonical grant, checksummed save round-trip");
            GD.Print("  --dose-ledger-selftest   Dose Ledger save write → reload → restore → checksum/tamper checks");
            GD.Print("  --duty-roster-save-selftest Duty Roster save write → reload → restore → checksum/tamper checks");
            GD.Print("  --economy-selftest       Run the engine-agnostic economy headless demo (goods load, market ticks, barter, save/load round-trip)");
            GD.Print("  --expansion-hub-save-selftest Expansion hub save write → reload → restore → checksum/tamper checks");
            GD.Print("  --expedition-encounter-bridge-selftest  ExpeditionEncounterBridge bare-notice + resolved surface smoke test");
            GD.Print("  --expedition-selftest    Expedition domain: sorties, encounter resolution, loot drops, and save round-trip");
            GD.Print("  --expedition-playtest-selftest  Plans 51: deterministic 30-day expedition campaign, vehicle balance ledger, breakdown, wear, and save/resume proof");
            GD.Print("  --synthetic-lubricant-selftest  Plan 118: catalog-backed synthetic lubricant production, catalyst state, atomic feed/output, and consumer registration");
            GD.Print("  --uv-corona-selftest     Plan 119: bounded electrical-fault observations, seeded sensor noise, battery use, and capture/restore");
            GD.Print("  --carbon-composite-selftest  Plan 120: material aging, deterministic cure quality, explicit component projection, and capture/restore");
            GD.Print("  --gpr-cartography-selftest  Plan 121: terrain/mode survey trade-offs, uncertain buried observations, map-lead idempotence, and capture/restore");
            GD.Print("  --advanced-industrial-recon-selftest  Plans 118-121: deterministic 60-day Core industrial/reconnaissance integration with midpoint save/replay proof");
            GD.Print("  --patrol-encounter-selftest / --travel-encounter-selftest  Patrol catalog, cooldown, recognition, resolution, and save/restore lifecycle");
            GD.Print("  --research-catalog-selftest  Research knowledge catalog: load count, DAG validity, and cross-catalog unlock references (Plan 34)");
            GD.Print("  --radio-catalog-selftest     Radio station catalog: JSON authority, schedules, and signal model (AF-B1 / Plan 60)");
            GD.Print("  --holdfast-save-selftest S1 save write → reload → restore → checksum/tamper checks");
            GD.Print("  --holdfast-trade-save-selftest Holdfast trade ledger and save store round-trip and tamper checks");
            GD.Print("  --inventory-save-selftest Inventory system save store round-trip, item serialization, and checksum verification");
            GD.Print("  --starting-supplies-selftest / --starting-profile-selftest  Plan 134 six-profile fresh-inventory matrix, fallback, idempotence, and save bypass");
            GD.Print("  --journal-save-selftest  Journal system save store round-trip, entry ordering, and tamper checks");
            GD.Print("  --journal-selftest       Journal domain + save roundtrip");
            GD.Print("  --journal-weather-panel-selftest  Journal and Weather forecast panel integration and live data binding");
            GD.Print("  --medical-selftest       Medical domain: patient triage, treatment protocols, affliction progression, and save round-trip");
            GD.Print("  --medical-ward-save-selftest Medical ward save store round-trip, bed allocation, and affliction persistence");
            GD.Print("  --narrative-selftest     Narrative domain: dialog trees, echoes, flags, and story event resolution");
            GD.Print("  --narrative-continuity-selftest  Plan 50: normalize narrative graphs (questline stages, event chains, quest refs), lint dangling refs/reachability/flag set-vs-read/case discipline, write artifacts/narrative-continuity.{json,md}");
            GD.Print("  --npc-arc-selftest       Plan 52 recurring NPC arcs: resolution precedence, encounter→quest memory, save round-trip, distress suppression");
            GD.Print("  --oral-lore-selftest     Oral Lore Codex: load 16 songs/poems from narrative catalogs, verify query by id/tag/genre");
            GD.Print("  --radio-selftest         Radio persistence: history/frequency/played-dedup survive save/load; tamper rejected");
            GD.Print("  --settings-selftest / --settings-test SettingsManager state, resolution, audio buses, and keybindings save/load");
            GD.Print("  --survivors-selftest     Survivors domain: needs decay, skill progression, trauma, and morale");
            GD.Print("  --utility-ai-selftest    Utility AI decision scoring, survivor behaviors, and action selection");
            GD.Print("  --weather-save-selftest  Weather system save store round-trip, forecast queue, and atmospheric condition persistence");
            GD.Print("  --dynamic-world-selftest / --plan19-selftest Dynamic world systems: weather forecasting lookahead, station tiers, 6 seasonal phases, 18+ seasonal events, Orbital Harrow kinetic impact templates, sky armor cascades, salvage, and save persistence");
            GD.Print("  --wasteland-inhabitants-selftest / --plan20-selftest / --inhabitants-selftest Wasteland inhabitants: 32-entry field guide (20 fauna + 12 flora), 6 wasteland settlements, 18 named NPCs with standing-reactive greetings, 6 repeatable side-work quests, 24 route-aware travel encounters + 4 multi-stage chains with stance weighting and deterministic RNG");
            GD.Print("  --world-selftest         World domain: map nodes, sector navigation, hazard regions, and landmark states");
            GD.Print("  --year-of-ash-save-selftest Year of Ash save write → reload → restore → checksum/tamper checks");

            GD.Print("\n--- UI Tests, Layout & Gameplay Smoke ---");
            GD.Print("  --dashboard-uitest       Game Dashboard panel UI construction, HUD binding, and metrics display");
            GD.Print("  --day1-selftest / --day-1-selftest / --day1-playable-selftest Day 1 onboarding, needs depletion, and shelter survival verification");
            GD.Print("  --day1-to-day2-selftest / --day1-to-day2 / --day1-to-day2-milestone-selftest Day 1 to Day 2 transition, overnight triage, and milestone progression");
            GD.Print("  --dose-uitest            Dose Ledger panel UI construction, radiation tiers, and dose history");
            GD.Print("  --duty-roster-uitest     Duty Roster panel UI construction, role assignments, and shift scheduling");
            GD.Print("  --economy-uitest         Economy market panel UI construction, price shock display, and barter grid");
            GD.Print("  --expedition-panel-uitest / --expedition-panel-lifecycle Expedition panel encounter-notice lifecycle: open→surface→close→reopen→surface");
            GD.Print("  --onboarding-journey-selftest / --onboarding-selftest First-hour onboarding journey: water → power → food → research → expedition, with resume after save/load and state-true signals");
            GD.Print("  --mod-selftest / --mods-selftest Deterministic JSON mod manifest validation, whitelist enforcement, layering, failure isolation, and catalog-integrity staging");
            GD.Print("  --holdfast-runtime-uitest / --holdfast-runtime-ui-test / --holdfast-runtime-selftest  Godot Holdfast terminal browse → trade → failed trade → save → reload");
            GD.Print("  --inventory-uitest / --inventory-selftest       Inventory panel UI construction, item grid, and slot binding");
            GD.Print("  --journal-uitest         Build ledger UI, cycle tabs, quit");
            GD.Print("  --muster-uitest          The Muster panel UI construction, faction stance cards, and vote tally");
            GD.Print("  --phase0-uitest          Phase 0 expansion UI preview and workstation panels");
            GD.Print("  --playable-shell-selftest / --shell-selftest / --playable-loop-selftest Playable shell game loop, scene transitions, and day advancement");
            GD.Print("  --player-panels-uitest / --player-panels-ui-test  Bind and render Survivors, Medical, Weather, Radio, Shelter panels");
            GD.Print("  --shelter-hazard-loop-selftest / --shelter-hazard-selftest / --duty-roster-loop-selftest Shelter hazard loop and duty roster assignment verification");
            GD.Print("  --shelter-decor-selftest / --shelter-interior-selftest / --memorial-wall-selftest Live items.json decor, inventory mount/remove, NeedsSystem morale, memorial-wall projection, save, and panel verification");
            GD.Print("  --shelter-operations-selftest / --shelter-ops-selftest / --operations-selftest Medical triage, expedition sorties, radio network, crafting, and respiratory affliction verification");
            GD.Print("  --silent-foundry-uitest   Silent Foundry trade panel UI construction, binding, and trade loop");
            GD.Print("  --plans198-201-uitest / --plans198-201-selftest  CBRN/comms/ceremony/robotics panels: route, bind, command, state delta, feedback");
            GD.Print("  --decon-airlock-uitest     Decon Airlock UI data grid panel bindings");
            GD.Print("  --workshop-relic-uitest / --workshop-relic-selftest  Workshop dual-bind relic restoration smoke: render, select, repair, deltas, save/reload");
            GD.Print("  --decon-airlock-selftest   Decon Airlock UI data grid panel bindings");
            GD.Print("  --geodetic-survey-uitest   Geodetic Survey UI data grid panel bindings");
            GD.Print("  --geodetic-survey-selftest Geodetic Survey UI data grid panel bindings");
            GD.Print("  --kinetic-storage-uitest   Kinetic Storage UI data grid panel bindings");
            GD.Print("  --kinetic-storage-selftest Kinetic Storage UI data grid panel bindings");
            GD.Print("  --chemical-recon-uitest    Chemical Recon UI data grid panel bindings");
            GD.Print("  --chemical-recon-selftest  Chemical Recon UI data grid panel bindings");
            GD.Print("  --recon-telemetry-uitest / --recon-telemetry-selftest Recon Telemetry UI data grid panel bindings");
            GD.Print("  --geothermal-uitest        Geothermal Aquifer UI panel bindings");
            GD.Print("  --geothermal-aquifer-selftest Geothermal Aquifer UI panel bindings");
            GD.Print("  --ebpvd-coating-uitest     EB-PVD Thermal Barrier Coating UI panel bindings");
            GD.Print("  --ebpvd-coating-selftest   EB-PVD Thermal Barrier Coating UI panel bindings");
            GD.Print("  --microfluidic-diagnostic-uitest Microfluidic Diagnostics UI panel bindings");
            GD.Print("  --microfluidic-diagnostic-selftest Microfluidic Diagnostics UI panel bindings");
            GD.Print("  --mine-flail-uitest        Mine-Clearing Flail UI panel bindings");
            GD.Print("  --mine-flail-selftest      Mine-Clearing Flail UI panel bindings");
            GD.Print("  --rail-grinding-uitest     Rail Grinding Corridor UI panel bindings");
            GD.Print("  --rail-grinding-selftest   Rail Grinding Corridor UI panel bindings");
            GD.Print("  --survivors-uitest       Survivors panel UI construction, roster cards, and affliction badges");
            GD.Print("  --ui-layout-selftest / --layout-selftest Verify fixed 1920x1080 UI layout bounds, responsive containers, and panel alignments");
            GD.Print("  --ui-snapshot-regenerate / --ui-snapshots-regen Recapture all snapshot targets and OVERWRITE snapshots/ goldens (needs real display)");
            GD.Print("  --ui-snapshot-uitest / --ui-snapshots Capture all snapshot targets, DIFF against snapshots/ goldens (needs real display, not --headless)");
            GD.Print("  --utility-ai-uitest      Utility AI debug view, consideration curves, and behavior trees");
            GD.Print("  --verdict-uitest         Build THE MACHINE'S REGISTER panel; assert 13 transmissions render + leak-free");

            GD.Print("\n--- Late-Tech, Survey & Mobility Gates ---");
            GD.Print("  --sofc-power-selftest    Plan 122 solid-oxide fuel cell: catalog, electrochemistry engine, power/water gating, save round-trip");
            GD.Print("  --sound-ranging-selftest Plan 123 sound ranging: catalog, threat engine, bearing/registration math, determinism");
            GD.Print("  --cvd-diamond-selftest   Plan 124 CVD diamond synthesis: catalog, plasma-phase engine, batch lifecycle, save round-trip");
            GD.Print("  --amphibious-draisine-selftest Plan 125 amphibious draisine: catalog, crossing engine, cargo/load gates, water crossings");
            GD.Print("  --late-tech-mobility-selftest Combined Plans 122–125 harness: all four late-tech systems composed through one CLI world");
            GD.Print("  --plans-122-125-selftest Plans 122–125 aggregate: catalog + engine + wiring + persistence checks for SOFC/sound ranging/CVD diamond/amphibious draisine");
            GD.Print("  --plans-122-125-balance-soak  Plans 122–125 balance soaks: bounded multi-day soak over the four late-tech systems, writes the plan 122–125 balance reports");
            GD.Print("  --insar-selftest         Plan 139 InSAR geodesy: repeat passes, decorrelation, deformation classes, travel/excavation projections");
            GD.Print("  --hydraulic-extrusion-selftest  Plan 140 hydraulic extrusion: phases, defect rolls, tool wear, rejected/premium outcomes");
            GD.Print("  --runflat-tire-selftest  Plan 141 run-flat tires: install gating, hazard reduction, heat/fuel penalty, severe-failure paths");
            GD.Print("  --plans-139-141-selftest Plans 139–141 aggregate: InSAR + hydraulic extrusion + run-flat wiring, persistence, and panel reachability");
            GD.Print("  --sky-defense-selftest   Flagship Task 7 counter-battery: telemetry track intake, magazine logistics, deterministic volley, service, crew claim, save round-trip, and player-panel construction");
            GD.Print("  --vehicle-garage-selftest Plan 50 overland vehicle garage: modification install/uninstall, component wear, service, immobilization gate, recovery completion, and expedition-profile decoration");
            GD.Print("  --shelter-physics-selftest / --shelter-actor-physics-selftest  Shelter physics and actor movement selftests: interior traversal, hazard interaction");
            GD.Print("  --propaganda-selftest    Plan 168: Propaganda and morale warfare system, campaigns, broadcasts, save persistence, and UI binding");
            GD.Print("  --rumor-network-selftest Plan 203: Wasteland information flow, rumors, intelligence gathering, save persistence, and UI binding");
            GD.Print("  --shelter-security-selftest Plan 138: Shelter defense, security clearance levels, breach alerts, save persistence, and UI binding");
            GD.Print("  --personal-quests-selftest Plan 200: Survivor personal quests, character arcs, stage progression, save persistence, and UI binding");
            GD.Print("  --time-capsule-selftest  Plan 212: Time capsule & legacy messages system, scheduled opening, save persistence, and UI binding");
            GD.Print("  --death-legacy-selftest  Plan 206: Survivor death records, wills, estate inheritance, disputes, save persistence, and UI binding");
            GD.Print("  --relationship-decay-selftest Plan 182: Relationship decay, social drift, bond maintenance, save persistence, and UI binding");
            GD.Print("  --research-unlock-selftest / --research-unlocks-selftest Plan 141 research unlock bridge: catalog load, downstream unlock queries, capability grants, recipe unlocks, and inventory awards");
            GD.Print("  --unified-ending-selftest / --epilogue-selftest Plan 145 unified ending resolver: epilogue evaluation, personalized chronicle, survivor fates, legacy trait awards, and save round-trip");
            GD.Print("  --npc-memory-selftest / --npc-memory-test Plan 147 per-NPC memory: trust, grudge, favors owed, forgiveness, dialogue tone, and trade pricing modifiers");
            GD.Print("  --ideological-friction-selftest / --ideology-selftest Plan 148 ideological friction: confrontations, conversions, bunker factions, and mediation");
            GD.Print("  --romance-family-selftest / --romance-selftest     Plan 150 romance & family dynamics: courtship stages, partnership, bonded pairs, family units, adoption");
            GD.Print("  --vehicle-customization-selftest / --vehicle-modules-selftest Plan 152 vehicle module slots, effective stats, bunk capacity, and base camps");

            GD.Print("\n--- User Data & Log Configuration ---");
            GD.Print("  --user-data-dir <path>   Override user:// base directory for isolated test runs (or set ASHFALL_USER_DIR)");
            GD.Print("  --log-dir <path>         Configure log output directory for headless runs (or set ASHFALL_LOG_DIR)");

            GD.Print("\n--- General & Information ---");
            GD.Print("  --host-help / --help     This list");
            GD.Print("  --version / -v           Show build, data schema, and save schema versions");
        }

        /// <summary>
        /// Prints the `--version` report: game/build version (project
        /// settings), live data-authority schema summary, and save codec
        /// schema versions. Logic lives in Core (Ashfall.Core.VersionReport);
        /// this host method only supplies the engine-side version string.
        /// </summary>
        public static void PrintVersion(string dataDir)
        {
            string gameVersion = "INVALID (config/version missing or not semver)";
            var setting = ProjectSettings.GetSetting("application/config/version");
            if (setting.VariantType == Variant.Type.String)
            {
                string raw = setting.AsString();
                if (ReleaseVersion.TryParse(raw, out string normalized))
                {
                    gameVersion = normalized;
                }
            }

            GD.Print($"\n{VersionReport.Compose(gameVersion, dataDir)}");
            GD.Print($"data resolution: {CatalogPath.ResolveDataDir()} [source: {CatalogPath.LastResolutionSource}]");
        }
    }
}
