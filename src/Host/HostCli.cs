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
        DutyRosterSelfTest,
        StandingRecordSelfTest,
        CrossingSelfTest,
        ArbitrationSelfTest,
        LedgerDebtSelfTest,
        GreenhouseSelfTest,
        SilentFoundrySelfTest,
        SilentFoundryUiTest,
        DiseaseSelfTest,
        DutyRosterUiTest,
        ExpansionsSelfTest,
        YearOfAshSaveSelfTest,
        VerdictSelfTest,
        DutyRosterSaveSelfTest,
        ExpansionHubSaveSelfTest,
        DoseLedgerSelfTest,
        ExpeditionSelfTest,
        ExpeditionEncounterBridgeSelfTest,
        MedicalSelfTest,
        NarrativeSelfTest,
        SurvivorsSelfTest,
        WorldSelfTest,
        EconomySelfTest,
        EconomyUiTest,
        UtilityAiSelfTest,
        UtilityAiUiTest,
        DataIntegritySelfTest,
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
        AudioSelfTest,
        DeepCoastSelfTest,
        DeepCoastHostSelfTest,
        WarlordSelfTest,
        WarlordHostSelfTest,
        WarlordUiSelfTest,
        BlackFlotillaSelfTest,
        RadioSelfTest,
        ExpeditionPanelUiTest,
        JournalSaveSelfTest,
        JournalWeatherPanelSelfTest,
        MoralChoiceSelfTest,
        EvolvingWorldSelfTest,
        InventorySaveSelfTest,
        MedicalWardSaveSelfTest,
        ChemicalDependencySaveSelfTest,
        WeatherSaveSelfTest,
        SaveLoadUiFailureSelfTest,
        PanelBindLifecycleSelfTest,
        SaveStoreChecksumSelfTest,
        SevenDayDeterministicSmokeSelfTest,
        UiAccessibilitySelfTest,
        UiSnapshotSelfTest,
        UiSnapshotRegenerate,
        OnboardingJourneySelfTest,
        SelfTestManifest,
        ListSelfTests
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
            if (Has(args, "--shelter-operations-selftest") || Has(args, "--operations-selftest") || Has(args, "--shelter-ops-selftest"))
                return HostCliAction.ShelterOperationsSelfTest;
            if (Has(args, "--shelter-hazard-loop-selftest") || Has(args, "--shelter-hazard-selftest") || Has(args, "--duty-roster-loop-selftest"))
                return HostCliAction.ShelterHazardLoopSelfTest;
            if (Has(args, "--ui-layout-selftest") || Has(args, "--layout-selftest"))
                return HostCliAction.UiLayoutSelfTest;
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
            if (Has(args, "--silent-foundry-selftest"))
                return HostCliAction.SilentFoundrySelfTest;
            if (Has(args, "--disease-selftest") || Has(args, "--disease-expansion-selftest"))
                return HostCliAction.DiseaseSelfTest;
            if (Has(args, "--combat-selftest"))
                return HostCliAction.CombatSelfTest;
            if (Has(args, "--silent-foundry-uitest"))
                return HostCliAction.SilentFoundryUiTest;
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
            if (Has(args, "--expedition-encounter-bridge-selftest"))
                return HostCliAction.ExpeditionEncounterBridgeSelfTest;
            if (Has(args, "--medical-selftest"))
                return HostCliAction.MedicalSelfTest;
            if (Has(args, "--narrative-selftest"))
                return HostCliAction.NarrativeSelfTest;
            if (Has(args, "--survivors-selftest"))
                return HostCliAction.SurvivorsSelfTest;
            if (Has(args, "--world-selftest"))
                return HostCliAction.WorldSelfTest;
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
            if (Has(args, "--black-flotilla-selftest") || Has(args, "--maritime-selftest") || Has(args, "--expansion-09-selftest"))
                return HostCliAction.BlackFlotillaSelfTest;
            if (Has(args, "--radio-selftest"))
                return HostCliAction.RadioSelfTest;
            if (Has(args, "--expedition-panel-uitest") || Has(args, "--expedition-panel-lifecycle"))
                return HostCliAction.ExpeditionPanelUiTest;
            if (Has(args, "--onboarding-journey-selftest") || Has(args, "--onboarding-selftest"))
                return HostCliAction.OnboardingJourneySelfTest;
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
            if (Has(args, "--inventory-save-selftest"))
                return HostCliAction.InventorySaveSelfTest;
            if (Has(args, "--medical-ward-save-selftest"))
                return HostCliAction.MedicalWardSaveSelfTest;
            if (Has(args, "--chemical-dependency-save-selftest"))
                return HostCliAction.ChemicalDependencySaveSelfTest;
            if (Has(args, "--weather-save-selftest"))
                return HostCliAction.WeatherSaveSelfTest;
            if (Has(args, "--save-load-ui-failure-selftest") || Has(args, "--save-load-failure-selftest") || Has(args, "--save-load-failure-uitest") || Has(args, "--save-load-selftest"))
                return HostCliAction.SaveLoadUiFailureSelfTest;
            if (Has(args, "--panel-bind-lifecycle-selftest") || Has(args, "--panel-bind-selftest") || Has(args, "--panel-lifecycle-selftest"))
                return HostCliAction.PanelBindLifecycleSelfTest;
            if (Has(args, "--save-store-checksum-selftest") || Has(args, "--save-store-checksums-selftest") || Has(args, "--checksum-sweep-selftest"))
                return HostCliAction.SaveStoreChecksumSelfTest;
            if (Has(args, "--7-day-smoke-selftest") || Has(args, "--seven-day-smoke-selftest") || Has(args, "--deterministic-smoke-selftest") || Has(args, "--deterministic-smoke-run"))
                return HostCliAction.SevenDayDeterministicSmokeSelfTest;
            if (Has(args, "--ui-accessibility-selftest") || Has(args, "--ui-access-selftest") || Has(args, "--accessibility-selftest"))
                return HostCliAction.UiAccessibilitySelfTest;
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
            GD.Print("  --bridge-selftest        Report UnityEngine shim removal (shim is gone; always exits 0)");
            GD.Print("  --core-selftest          Ice road + census headless demos");
            GD.Print("  --data-integrity-selftest Cross-reference every id in the 129 StreamingAssets catalogs (recipe→item, quest→location, events, door encounters, survivors, factions, ranges, duplicates)");
            GD.Print("  --catalog-boot-preflight   Machine-readable preflight: checks all catalogs are present, well-formed, and reports classification (required/optional/dev-only) with any load errors");
            GD.Print("  --panel-bind-lifecycle-selftest / --panel-bind-selftest / --panel-lifecycle-selftest Real Godot-node callback tests for panel bind → unbind → rebind, event propagation, and session-switch");
            GD.Print("  --save-load-ui-failure-selftest / --save-load-failure-selftest / --save-load-failure-uitest / --save-load-selftest Save/load UI failure-path smoke test: missing, corrupt, and checksum-invalid saves show recoverable user messages and leave live session intact");
            GD.Print("  --save-store-checksum-selftest / --save-store-checksums-selftest / --checksum-sweep-selftest Source-scan all SaveStore files for checksum coverage + 5 in-memory round-trip probes (Weather, Map, Survivors, SaveChecksum stability, null-field guard)");
            GD.Print("  --standalone-selftest    SkyLayerArmor, VigilStateMachine, GenerationalSuccession, EpilogueMatrix, DiveInstance");

            GD.Print("\n--- Expansions & Campaign Modules ---");
            GD.Print("  --arbitration-selftest   CrossingArbitrationHeadlessDemo");
            GD.Print("  --black-flotilla-selftest / --maritime-selftest / --expansion-09-selftest The Black Flotilla (Exp 09): catalog load, deterministic scavenge, dive rooms/air/noise, contamination, visit state, save round-trip");
            GD.Print("  --brine-selftest / --salt-steam-selftest         BrineWaterHeadlessDemo (S2 salt & steam)");
            GD.Print("  --census-selftest        CensusHeadlessDemo");
            GD.Print("  --cluster-selftest / --order-12c-selftest       Cluster12CHeadlessDemo (S3 order 12-C + quest snapshot)");
            GD.Print("  --combat-selftest        Combat Expansion: catalog (JSON), ballistics, weapon condition, determinism, save round-trip");
            GD.Print("  --crossing-selftest      CrossingHeadlessDemo (Exp 04)");
            GD.Print("  --deep-coast-host-selftest / --deep-coast-playthrough Deep-coast host playthrough: survey → decision → dive → scavenge → save/restore");
            GD.Print("  --deep-coast-selftest / --deep-coast-route-selftest District 8 deep-coast route: stages, decisions, Ice Road gating, dive handoff, v5 save");
            GD.Print("  --disease-selftest / --disease-expansion-selftest Disease Expansion: catalog, quarantine, protocols, determinism, save round-trip");
            GD.Print("  --duty-roster-selftest   DutyRosterHeadlessDemo (Exp 02)");
            GD.Print("  --endings-selftest / --shelf-selftest       EndingsHeadlessDemo (S4 endings exclusive + roundtrip)");
            GD.Print("  --expansions-selftest / --all-expansions-selftest    Run full 7-expansion verification suite (Holdfast, Duty Roster, Standing Record, Crossing, Arbitration, LedgerDebt, Glass Orchard)");
            GD.Print("  --greenhouse-selftest / --glass-orchard-selftest    GreenhouseHeadlessDemo (Exp 05)");
            GD.Print("  --holdfast-briefing      Print location count and every Holdfast quest briefing");
            GD.Print("  --holdfast-selftest      Holdfast S1 survival loop, ice road, and trade verification");
            GD.Print("  --ice-road-selftest      IceRoadHeadlessDemo (Exp 01)");
            GD.Print("  --ice-road-tick-demo     Unlock, clerk, 30 day ticks, print catalog + briefing");
            GD.Print("  --ledger-debt-selftest   LedgerDebtHeadlessDemo");
            GD.Print("  --moral-choice-selftest  Moral choice: catalog + scripted arc + bands + reconcile events + journal hook + save/tamper checks");
            GD.Print("  --evolving-world-selftest  Evolving-world activation: seeds, live weather-fed ticks, migration, expedition consequences, scarcity, save envelope, 360-day scenario");
            GD.Print("  --selftest-manifest      Emit the machine-readable self-test manifest JSON (scripts/ci/generate-selftest-manifest.py)");
            GD.Print("  --test-manifest          Alias for --selftest-manifest");
            GD.Print("  --list-selftests         List every registered selftest and run its signature live (runtime/CLI parity audit)");
            GD.Print("  --list-tests             Alias for --list-selftests");
            GD.Print("  --selftests              Alias for --list-selftests");
            GD.Print("  --list-selftest          Alias for --list-selftests");
            GD.Print("  --muster-selftest / --expansion-06-selftest        MusterHeadlessDemo (Exp 06 the Muster)");
            GD.Print("  --phase0-selftest        Phase-0 effects: phantom work-eff/refusal, flashbacks, trade specialty, final-wish buff, respiratory stamina + save roundtrip");
            GD.Print("  --silent-foundry-selftest Silent Foundry (Exp 10): trade stance, trust momentum, recipes, and save round-trip");
            GD.Print("  --standing-record-selftest StandingRecordHeadlessDemo (Exp 03)");
            GD.Print("  --verdict-selftest / --expansion-08-selftest The Verdict (Exp 08): machine log, reckoning phases, evidence, census, save");
            GD.Print("  --warlord-host-selftest  Warlord host playthrough: YearOfAsh wiring, standing, v3 save/tamper");
            GD.Print("  --warlord-selftest / --warlord-ai-selftest Adaptive warlord AI: doctrines, territory, tribute, determinism, v3 save");
            GD.Print("  --warlord-ui-selftest    Warlord tribute payment loop + collector voice + FactionsPanel card");

            GD.Print("\n--- Host Domains & Save Stores ---");
            GD.Print("  --audio-selftest / --audio-test Audio cue catalog, AudioManager wiring, and sound event verification");
            GD.Print("  --caravan-selftest / --traveling-caravan-selftest Traveling caravan economy, inventory generation, and barter ticks");
            GD.Print("  --chemical-dependency-save-selftest Chemical dependency system save store round-trip, tolerance, and withdrawal states");
            GD.Print("  --dose-ledger-selftest   Dose Ledger save write → reload → restore → checksum/tamper checks");
            GD.Print("  --duty-roster-save-selftest Duty Roster save write → reload → restore → checksum/tamper checks");
            GD.Print("  --economy-selftest       Run the engine-agnostic economy headless demo (goods load, market ticks, barter, save/load round-trip)");
            GD.Print("  --expansion-hub-save-selftest Expansion hub save write → reload → restore → checksum/tamper checks");
            GD.Print("  --expedition-encounter-bridge-selftest  ExpeditionEncounterBridge bare-notice + resolved surface smoke test");
            GD.Print("  --expedition-selftest    Expedition domain: sorties, encounter resolution, loot drops, and save round-trip");
            GD.Print("  --holdfast-save-selftest S1 save write → reload → restore → checksum/tamper checks");
            GD.Print("  --holdfast-trade-save-selftest Holdfast trade ledger and save store round-trip and tamper checks");
            GD.Print("  --inventory-save-selftest Inventory system save store round-trip, item serialization, and checksum verification");
            GD.Print("  --journal-save-selftest  Journal system save store round-trip, entry ordering, and tamper checks");
            GD.Print("  --journal-selftest       Journal domain + save roundtrip");
            GD.Print("  --journal-weather-panel-selftest  Journal and Weather forecast panel integration and live data binding");
            GD.Print("  --medical-selftest       Medical domain: patient triage, treatment protocols, affliction progression, and save round-trip");
            GD.Print("  --medical-ward-save-selftest Medical ward save store round-trip, bed allocation, and affliction persistence");
            GD.Print("  --narrative-selftest     Narrative domain: dialog trees, echoes, flags, and story event resolution");
            GD.Print("  --radio-selftest         Radio persistence: history/frequency/played-dedup survive save/load; tamper rejected");
            GD.Print("  --settings-selftest / --settings-test SettingsManager state, resolution, audio buses, and keybindings save/load");
            GD.Print("  --survivors-selftest     Survivors domain: needs decay, skill progression, trauma, and morale");
            GD.Print("  --utility-ai-selftest    Utility AI decision scoring, survivor behaviors, and action selection");
            GD.Print("  --weather-save-selftest  Weather system save store round-trip, forecast queue, and atmospheric condition persistence");
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
            GD.Print("  --onboarding-journey-selftest / --onboarding-selftest First-hour onboarding journey: protocol → inspect → rationing → assignment → weather → inventory-use → day-advance, with resume after save/load and no-resource-fabrication");
            GD.Print("  --holdfast-runtime-uitest / --holdfast-runtime-ui-test / --holdfast-runtime-selftest  Godot Holdfast terminal browse → trade → failed trade → save → reload");
            GD.Print("  --inventory-uitest / --inventory-selftest       Inventory panel UI construction, item grid, and slot binding");
            GD.Print("  --journal-uitest         Build ledger UI, cycle tabs, quit");
            GD.Print("  --muster-uitest          The Muster panel UI construction, faction stance cards, and vote tally");
            GD.Print("  --phase0-uitest          Phase 0 expansion UI preview and workstation panels");
            GD.Print("  --playable-shell-selftest / --shell-selftest / --playable-loop-selftest Playable shell game loop, scene transitions, and day advancement");
            GD.Print("  --player-panels-uitest / --player-panels-ui-test  Bind and render Survivors, Medical, Weather, Radio, Shelter panels");
            GD.Print("  --shelter-hazard-loop-selftest / --shelter-hazard-selftest / --duty-roster-loop-selftest Shelter hazard loop and duty roster assignment verification");
            GD.Print("  --shelter-operations-selftest / --shelter-ops-selftest / --operations-selftest Medical triage, expedition sorties, radio network, crafting, and respiratory affliction verification");
            GD.Print("  --silent-foundry-uitest   Silent Foundry trade panel UI construction, binding, and trade loop");
            GD.Print("  --survivors-uitest       Survivors panel UI construction, roster cards, and affliction badges");
            GD.Print("  --ui-layout-selftest / --layout-selftest Verify fixed 1920x1080 UI layout bounds, responsive containers, and panel alignments");
            GD.Print("  --ui-snapshot-regenerate / --ui-snapshots-regen Recapture all snapshot targets and OVERWRITE snapshots/ goldens (needs real display)");
            GD.Print("  --ui-snapshot-uitest / --ui-snapshots Capture all snapshot targets, DIFF against snapshots/ goldens (needs real display, not --headless)");
            GD.Print("  --utility-ai-uitest      Utility AI debug view, consideration curves, and behavior trees");
            GD.Print("  --verdict-uitest         Build THE MACHINE'S REGISTER panel; assert 13 transmissions render + leak-free");

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
            string gameVersion = "unknown";
            var setting = ProjectSettings.GetSetting("application/config/version");
            if (setting.VariantType == Variant.Type.String)
                gameVersion = setting.AsString();
            if (string.IsNullOrEmpty(gameVersion))
                gameVersion = "unknown";
            GD.Print($"\n{VersionReport.Compose(gameVersion, dataDir)}");
        }
    }
}

