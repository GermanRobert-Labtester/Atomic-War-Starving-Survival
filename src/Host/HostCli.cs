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
        InventorySaveSelfTest,
        MedicalWardSaveSelfTest,
        ChemicalDependencySaveSelfTest,
        WeatherSaveSelfTest,
        UiSnapshotSelfTest,
        UiSnapshotRegenerate
    }

    /// <summary>
    /// User-args after Godot's `--`. Extra flags sit beside --ice-road-selftest;
    /// they call existing Ashfall.Core APIs and verify all 4 expansions.
    /// </summary>
    public static partial class HostCli
    {
        public static HostCliAction Parse(string[] args)
        {
            if (args == null || args.Length == 0)
                return HostCliAction.Interactive;

            if (Has(args, "--host-help") || Has(args, "--help"))
                return HostCliAction.Help;
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
            if (Has(args, "--ui-snapshot-regenerate") || Has(args, "--ui-snapshots-regen"))
                return HostCliAction.UiSnapshotRegenerate;
            if (Has(args, "--ui-snapshot-uitest") || Has(args, "--ui-snapshots"))
                return HostCliAction.UiSnapshotSelfTest;
            if (Has(args, "--journal-save-selftest"))
                return HostCliAction.JournalSaveSelfTest;
            if (Has(args, "--journal-weather-panel-selftest"))
                return HostCliAction.JournalWeatherPanelSelfTest;
            if (Has(args, "--moral-choice-selftest"))
                return HostCliAction.MoralChoiceSelfTest;
            if (Has(args, "--inventory-save-selftest"))
                return HostCliAction.InventorySaveSelfTest;
            if (Has(args, "--medical-ward-save-selftest"))
                return HostCliAction.MedicalWardSaveSelfTest;
            if (Has(args, "--chemical-dependency-save-selftest"))
                return HostCliAction.ChemicalDependencySaveSelfTest;
            if (Has(args, "--weather-save-selftest"))
                return HostCliAction.WeatherSaveSelfTest;
            return HostCliAction.Interactive;
        }

        public static void PrintHelp()
        {
            GD.Print("ASHFALL Godot host flags (after --):");
            GD.Print("  --expansions-selftest    Run full 7-expansion verification suite (Holdfast, Duty Roster, Standing Record, Crossing, Arbitration, LedgerDebt, Glass Orchard)");
            GD.Print("  --duty-roster-selftest   DutyRosterHeadlessDemo (Exp 02)");
            GD.Print("  --standing-record-selftest StandingRecordHeadlessDemo (Exp 03)");
            GD.Print("  --crossing-selftest      CrossingHeadlessDemo (Exp 04)");
            GD.Print("  --arbitration-selftest   CrossingArbitrationHeadlessDemo");
            GD.Print("  --ledger-debt-selftest   LedgerDebtHeadlessDemo");
            GD.Print("  --greenhouse-selftest    GreenhouseHeadlessDemo (Exp 05)");
            GD.Print("  --ice-road-selftest      IceRoadHeadlessDemo (Exp 01)");
            GD.Print("  --census-selftest        CensusHeadlessDemo");
            GD.Print("  --core-selftest          Ice road + census headless demos");
            GD.Print("  --ice-road-tick-demo     Unlock, clerk, 30 day ticks, print catalog + briefing");
            GD.Print("  --holdfast-save-selftest S1 save write → reload → restore → checksum/tamper checks");
            GD.Print("  --holdfast-runtime-uitest        Godot Holdfast terminal browse → trade → failed trade → save → reload\n" +
                      "  --holdfast-runtime-ui-test        alias for --holdfast-runtime-uitest\n" +
                      "  --holdfast-runtime-selftest        alias for --holdfast-runtime-uitest");
            GD.Print("  --brine-selftest         BrineWaterHeadlessDemo (S2 salt & steam)");
            GD.Print("  --muster-selftest        MusterHeadlessDemo (Exp 06 the Muster)");
            GD.Print("  --cluster-selftest       Cluster12CHeadlessDemo (S3 order 12-C + quest snapshot)");
            GD.Print("  --endings-selftest       EndingsHeadlessDemo (S4 endings exclusive + roundtrip)");
            GD.Print("  --holdfast-briefing      Print location count and every Holdfast quest briefing");
            GD.Print("  --journal-selftest       Journal domain + save roundtrip");
            GD.Print("  --moral-choice-selftest  Moral choice: catalog + scripted arc + bands + reconcile events + journal hook + save/tamper checks");
            GD.Print("  --journal-uitest         Build ledger UI, cycle tabs, quit");
            GD.Print("  --player-panels-uitest  Bind and render Survivors, Medical, Weather, Radio, Shelter panels");
            GD.Print("  --ui-snapshot-uitest     Capture all snapshot targets, DIFF against snapshots/ goldens (needs real display, not --headless)");
            GD.Print("  --ui-snapshot-regenerate Recapture all snapshot targets and OVERWRITE snapshots/ goldens (needs real display)");
            GD.Print("  --bridge-selftest        Report UnityEngine shim removal (shim is gone; always exits 0)");
            GD.Print("  --expedition-encounter-bridge-selftest  ExpeditionEncounterBridge bare-notice + resolved surface smoke test");
            GD.Print("  --year-of-ash-save-selftest Year of Ash save write → reload → restore → checksum/tamper checks");
            GD.Print("  --verdict-selftest         The Verdict (Exp 08): machine log, reckoning phases, evidence, census, save");
            GD.Print("  --verdict-uitest          Build THE MACHINE'S REGISTER panel; assert 13 transmissions render + leak-free");
            GD.Print("  --duty-roster-save-selftest Duty Roster save write → reload → restore → checksum/tamper checks");
            GD.Print("  --expansion-hub-save-selftest Expansion hub save write → reload → restore → checksum/tamper checks");
            GD.Print("  --dose-ledger-selftest       Dose Ledger save write → reload → restore → checksum/tamper checks");
            GD.Print("  --data-integrity-selftest  Cross-reference every id in the 55 StreamingAssets catalogs (recipe→item, quest→location, events, door encounters, survivors, factions, ranges, duplicates)");
            GD.Print("  --asset-registry-selftest  Verify that catalog IDs (items/survivors/locations) resolve to actual texture assets under assets/");
            GD.Print("  --asset-coverage-report    Full non-gating sweep of every catalog id (core + expansions) vs loadable art; prints per-category coverage and the missing list");
            GD.Print("  --standalone-selftest     SkyLayerArmor, VigilStateMachine, GenerationalSuccession, EpilogueMatrix, DiveInstance");
            GD.Print("  --deep-coast-selftest    District 8 deep-coast route: stages, decisions, Ice Road gating, dive handoff, v5 save");
            GD.Print("  --deep-coast-host-selftest Deep-coast host playthrough: survey → decision → dive → scavenge → save/restore");
            GD.Print("  --warlord-selftest       Adaptive warlord AI: doctrines, territory, tribute, determinism, v3 save");
            GD.Print("  --warlord-host-selftest  Warlord host playthrough: YearOfAsh wiring, standing, v3 save/tamper");
            GD.Print("  --warlord-ui-selftest    Warlord tribute payment loop + collector voice + FactionsPanel card");
            GD.Print("  --black-flotilla-selftest The Black Flotilla (Exp 09): catalog load, deterministic scavenge, dive rooms/air/noise, contamination, visit state, save round-trip");
            GD.Print("  --radio-selftest         Radio persistence: history/frequency/played-dedup survive save/load; tamper rejected");
            GD.Print("  --expedition-panel-uitest Expedition panel encounter-notice lifecycle: open→surface→close→reopen→surface (no double-subscribe, no stale handler)");
            GD.Print("  --phase0-selftest         Phase-0 effects: phantom work-eff/refusal, flashbacks, trade specialty, final-wish buff, respiratory stamina + save roundtrip");
            GD.Print("  --disease-selftest       Disease Expansion: catalog, quarantine, protocols, determinism, save round-trip");
            GD.Print("  --combat-selftest        Combat Expansion: catalog (JSON), ballistics, weapon condition, determinism, save round-trip");
            GD.Print("  --economy-selftest        Run the engine-agnostic economy headless demo (goods load, market ticks, barter, save/load round-trip)");
            GD.Print("  --host-help              This list");
        }
    }
}

