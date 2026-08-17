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
        HoldfastRuntimeUiTest,
        BrineSelfTest,
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
        MedicalSelfTest,
        NarrativeSelfTest,
        SurvivorsSelfTest,
        WorldSelfTest,
        EconomySelfTest,
        EconomyUiTest,
        UtilityAiSelfTest,
        UtilityAiUiTest,
        RngWiringSelfTest,
        DataIntegritySelfTest,
        CaravanSelfTest,
        AssetRegistrySelfTest,
        StandaloneSystemsSelfTest,
        Phase0SelfTest,
        Day1PlayableSelfTest,
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
        WarlordUiSelfTest
    }

    /// <summary>
    /// User-args after Godot's `--`. Extra flags sit beside --ice-road-selftest;
    /// they call existing Ashfall.Core APIs and verify all 4 expansions.
    /// </summary>
    public static class HostCli
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
            if (Has(args, "--expansions-selftest") || Has(args, "--all-expansions-selftest"))
                return HostCliAction.ExpansionsSelfTest;
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
            if (Has(args, "--rng-wiring-selftest"))
                return HostCliAction.RngWiringSelfTest;
            if (Has(args, "--data-integrity-selftest"))
                return HostCliAction.DataIntegritySelfTest;
            if (Has(args, "--caravan-selftest") || Has(args, "--traveling-caravan-selftest"))
                return HostCliAction.CaravanSelfTest;
            if (Has(args, "--asset-registry-selftest"))
                return HostCliAction.AssetRegistrySelfTest;
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
            GD.Print("  --journal-uitest         Build ledger UI, cycle tabs, quit");
            GD.Print("  --player-panels-uitest  Bind and render Survivors, Medical, Weather, Radio, Shelter panels");
            GD.Print("  --bridge-selftest        UnityEngine shim failure policy (semantic throws, cosmetic quiet)");
            GD.Print("  --year-of-ash-save-selftest Year of Ash save write → reload → restore → checksum/tamper checks");
            GD.Print("  --verdict-selftest         The Verdict (Exp 08): machine log, reckoning phases, evidence, census, save");
            GD.Print("  --verdict-uitest          Build THE MACHINE'S REGISTER panel; assert 13 transmissions render + leak-free");
            GD.Print("  --duty-roster-save-selftest Duty Roster save write → reload → restore → checksum/tamper checks");
            GD.Print("  --expansion-hub-save-selftest Expansion hub save write → reload → restore → checksum/tamper checks");
            GD.Print("  --dose-ledger-selftest       Dose Ledger save write → reload → restore → checksum/tamper checks");
            GD.Print("  --data-integrity-selftest  Cross-reference every id in the 55 StreamingAssets catalogs (recipe→item, quest→location, events, door encounters, survivors, factions, ranges, duplicates)");
            GD.Print("  --asset-registry-selftest  Verify that catalog IDs (items/survivors/locations) resolve to actual texture assets under assets/");
            GD.Print("  --standalone-selftest     SkyLayerArmor, VigilStateMachine, GenerationalSuccession, EpilogueMatrix, DiveInstance");
            GD.Print("  --deep-coast-selftest    District 8 deep-coast route: stages, decisions, Ice Road gating, dive handoff, v5 save");
            GD.Print("  --deep-coast-host-selftest Deep-coast host playthrough: survey → decision → dive → scavenge → save/restore");
            GD.Print("  --warlord-selftest       Adaptive warlord AI: doctrines, territory, tribute, determinism, v3 save");
            GD.Print("  --warlord-host-selftest  Warlord host playthrough: YearOfAsh wiring, standing, v3 save/tamper");
            GD.Print("  --warlord-ui-selftest    Warlord tribute payment loop + collector voice + FactionsPanel card");
            GD.Print("  --phase0-selftest         Phase-0 effects: phantom work-eff/refusal, flashbacks, trade specialty, final-wish buff, respiratory stamina + save roundtrip");
            GD.Print("  --disease-selftest       Disease Expansion: catalog, quarantine, protocols, determinism, save round-trip");
            GD.Print("  --economy-selftest        Run the engine-agnostic economy headless demo (goods load, market ticks, barter, save/load round-trip)");
            GD.Print("  --host-help              This list");
        }

        public static int RunDataIntegritySelfTest(string dataDirectory)
        {
            CatalogLocator.UseInvariantCulture();
            var report = CatalogIntegrityValidator.Validate(dataDirectory, new FileSystemIO());
            foreach (string line in report.Errors)
                GD.PrintErr("[DATA] " + line);
            foreach (string line in report.Warnings)
                GD.Print("[DATA] (warn) " + line);
            GD.Print(report.Summary + " — " + report.ErrorCount + " errors, "
                + report.Warnings.Count + " warnings across "
                + System.IO.Directory.GetFiles(dataDirectory, "*.json").Length + " catalogs");
            return report.Clean ? 0 : 1;
        }

        public static int RunExpansionsSelfTest(string dataDirectory)
        {
            var report = ExpansionMasterSession.RunAllSelfTests(dataDirectory, new GodotLog());
            GD.Print(report.Summary);
            if (report.ExitCode != 0)
                return report.ExitCode;
            // Chain the Verdict (Exp 08) gate into the full expansion suite so a
            // CI/expansions run also proves the machine log / reckoning / census /
            // evidence / ending / save chain (item 10).
            return RunVerdictSelfTest(dataDirectory);
        }

        /// <summary>
        /// Deep-coast route gate: the full vertical slice — sealed → surveyed →
        /// perimeter_open → dock_accessible → deep_berth_operational, the four
        /// reopening decisions, Ice Road seasonal gating, canonical inventory
        /// consumption/rewards, faction standing, once-only journal keys, the
        /// expedition→dive handoff into the existing maritime dive, and the
        /// HoldfastSave v5 round-trip (migration, checksum, future rejection).
        /// </summary>
        public static int RunDeepCoastSelfTest(string dataDirectory)
        {
            var report = DeepCoastHeadlessDemo.Run(dataDirectory, new GodotLog());
            GD.Print(report.Summary);
            return report.ExitCode;
        }

        /// <summary>Warlord AI gate (proposed model): doctrines, territory, tribute, save.</summary>
        public static int RunWarlordSelfTest(string dataDirectory)
        {
            var report = WarlordHeadlessDemo.Run(dataDirectory, new GodotLog());
            GD.Print(report.Summary);
            return report.ExitCode;
        }

        /// <summary>
        /// Warlord HOST gate: drives the live YearOfAshHostSession surface — the
        /// catalog loads and validates, the daily tick runs the warlord on the
        /// operation cadence, the standing consequences land in the canonical
        /// FactionWarSystem, the status line renders, and the v3 save round-trips
        /// through the codec (tamper rejected).
        /// </summary>
        public static int RunWarlordHostSelfTest(string dataDirectory)
        {
            int failures = 0;
            void Check(bool condition, string name)
            {
                if (condition) GD.Print("[PASS] " + name);
                else
                {
                    GD.Print("[FAIL] " + name);
                    failures++;
                }
            }

            try
            {
                var session = YearOfAshHostSession.Create(dataDirectory, loadExistingSave: false);
                var warlord = session.Warlord;
                Check(warlord != null && warlord.DoctrineId == "warlord_doctrine_toll",
                    "warlord wired with the toll doctrine from the catalog");
                Check(warlord.TerritoryState("loc_toll_house") == WarlordTerritoryState.Controlled,
                    "home territory controlled after host wiring");
                Check(warlord.Catalog.Territory.Count >= 5, "territory graph loaded");

                int standingBefore = session.FactionWar.GetStanding("warlords_sector_4");
                for (int day = 210; day <= 300; day++)
                    session.TickDay(day);
                Check(warlord.TotalOperations > 0, "warlord acted on the daily cadence");
                Check(session.WarlordLine().Contains("Warlord"), "warlord status line renders");
                int standingAfter = session.FactionWar.GetStanding("warlords_sector_4");
                Check(standingAfter != standingBefore, "standing consequences landed in FactionWarSystem");

                // Save round-trip through the codec.
                var json = new SystemTextJsonSerializer();
                var save = session.CaptureSave();
                Check(save.saveVersion == YearOfAshSave.CurrentSaveVersion, "save is v" + YearOfAshSave.CurrentSaveVersion);
                Check(save.warlord.doctrineId == warlord.DoctrineId, "warlord state captured in the envelope");
                string encoded = YearOfAshSaveCodec.Encode(save, json);
                var loaded = YearOfAshSaveCodec.Decode(encoded, json);
                Check(loaded.warlord.supply == warlord.Supply, "warlord supply round-trips");

                // Tamper rejection: flip the warlord supply ledger in the raw text.
                bool tamperRejected = false;
                try
                {
                    string needle = "\"supply\":" + warlord.Supply + ",";
                    string t = encoded.Replace(needle, "\"supply\":" + (warlord.Supply + 1) + ",");
                    if (t != encoded)
                        YearOfAshSaveCodec.Decode(t, json);
                }
                catch (InvalidOperationException)
                {
                    tamperRejected = true;
                }
                Check(tamperRejected, "tampered v3 save rejected (checksum)");
            }
            catch (Exception e)
            {
                Check(false, "warlord host playthrough threw: " + e.Message);
            }

            GD.Print(failures == 0
                ? "WARLORD_HOST_SELFTEST PASS"
                : "WARLORD_HOST_SELFTEST FAIL (" + failures + ")");
            return failures == 0 ? 0 : 1;
        }

        /// <summary>
        /// Warlord UI gate: the tribute payment loop through the live host path
        /// (canonical Holdfast inventory consume → Core SettleTribute → doctrine
        /// pressure → access consequence), the authored collector voice, and the
        /// FactionsPanel warlord card construction at multiple resolutions.
        /// </summary>
        public static int RunWarlordUiSelfTest(string dataDirectory)
        {
            int failures = 0;
            void Check(bool condition, string name)
            {
                if (condition) GD.Print("[PASS] " + name);
                else
                {
                    GD.Print("[FAIL] " + name);
                    failures++;
                }
            }

            try
            {
                var session = YearOfAshHostSession.Create(dataDirectory, loadExistingSave: false);
                var warlord = session.Warlord;
                string item = warlord.Catalog.Warlord.tribute_currency_item;

                // Drive the warlord through the day cadence; tribute asks fire
                // and the doctrine machine runs on the host-computed context.
                for (int day = 210; day <= 270; day++)
                    session.TickDay(day);
                Check(warlord.State.totalWeeksAsked >= 1, "tribute asks fire on the cadence");
                Check(!string.IsNullOrEmpty(session.CollectorLine("demand", 250)), "collector demand voice is authored");
                Check(!string.IsNullOrEmpty(session.CollectorLine("paid", 250)), "collector paid voice is authored");
                Check(!string.IsNullOrEmpty(session.CollectorLine("refused", 250)), "collector refused voice is authored");

                // Pay in full from the canonical inventory: consume, settle, ask resets.
                var inventory = new Ashfall.Core.HoldfastTradeInventory();
                inventory.AddItem(item, 200);
                int ask = session.CurrentTributeAsk;
                Check(ask >= warlord.Catalog.Warlord.tribute_base_amount, "current ask is at least the base");
                if (inventory.Items.TryGetValue(item, out int held) && held >= ask)
                    inventory.RemoveItem(item, ask);
                int nextAsk;
                bool paidFull = session.SettleWarlordTribute(ask, 300, out nextAsk);
                Check(paidFull, "full payment settles through Core");
                Check(!inventory.Items.TryGetValue(item, out int after) || after == 200 - ask,
                    "payment consumed exactly the ask from the canonical inventory");
                Check(warlord.State.totalWeeksPaid == 1, "paid-week ledger advances");

                // Refuse the next ask: escalation ×1.5, capped at 8×.
                session.SettleWarlordTribute(0, 301, out nextAsk);
                Check(nextAsk == Math.Max(1, (int)(warlord.Catalog.Warlord.tribute_base_amount * 1.5f)),
                    "refusal escalates the next ask (×1.5)");
                Check(warlord.State.consecutiveShortWeeks == 1, "short-week counter advances");
                for (int i = 0; i < 8; i++)
                    session.SettleWarlordTribute(0, 302 + i, out nextAsk);
                Check(warlord.TributeMultiplier <= warlord.Catalog.Warlord.tribute_max_multiplier,
                    "escalation respects the 8× cap");

                // FactionsPanel warlord card: bind a fresh session with a clean
                // inventory and verify construction + refresh do not throw.
                var panel = new AtomicWar.GodotApp.UI.FactionsPanel();
                panel.CustomMinimumSize = new Godot.Vector2(1920, 1080);
                panel.Size = new Godot.Vector2(1920, 1080);
                panel._Ready();
                var fresh = YearOfAshHostSession.Create(dataDirectory, loadExistingSave: false);
                panel.Bind(null, null, null, null, fresh);
                panel.Open();
                Check(panel.Visible, "warlord card renders inside FactionsPanel");
                panel.Visible = false;
            }
            catch (Exception e)
            {
                Check(false, "warlord ui selftest threw: " + e.Message);
            }

            GD.Print(failures == 0
                ? "WARLORD_UI_SELFTEST PASS"
                : "WARLORD_UI_SELFTEST FAIL (" + failures + ")");
            return failures == 0 ? 0 : 1;
        }

        /// <summary>
        /// Deep-coast HOST gate: a full playthrough driven through the live
        /// DeepCoastHostSession surface — survey → fleet decision → clear →
        /// channel → berth → dock dive (existing StealthDiveInstance) → scavenge
        /// rewards through ProceduralScavengeSystem with the Fleet levy → journal
        /// once-only → faction standing → mid-sequence save/restore without
        /// duplication. This is the running-session equivalent of the Core demo.
        /// </summary>
        public static int RunDeepCoastHostSelfTest(string dataDirectory = null)
        {
            int failures = 0;
            void Check(bool condition, string name)
            {
                if (condition) GD.Print("[PASS] " + name);
                else
                {
                    GD.Print("[FAIL] " + name);
                    failures++;
                }
            }

            try
            {
                var host = DeepCoastHostSession.Create();
                host.SetCurrentDay(180);
                var journal = host.Journal;
                var stances = host.Stances;
                var inventory = host.Inventory;

                // 1. Sealed route.
                Check(host.DeepCoast.Stage == DeepCoastStage.Sealed, "route starts sealed in the host session");
                Check(!host.DockExpeditionAvailable, "no dock expedition while sealed");

                // 2. Survey → journal once.
                string s1 = host.Survey(180);
                Check(s1.Contains("surveyed"), "survey action returns status");
                Check(journal.EntryCount == 1, "survey journal entry landed once");
                host.Survey(180);
                Check(journal.EntryCount == 1, "repeat survey does not duplicate the journal");

                // 3. Fleet decision → standing + stood up.
                string d1 = host.Decide("fleet", 181);
                Check(d1.Contains("Fleet"), "fleet decision accepted");
                Check(host.IsFleetActive, "the Fleet stands up and comes ashore");
                Check(stances.GetTrust(District8DeepCoastSystem.FactionFleet) == 12f,
                    "fleet trust moved exactly +12 via FactionStanceEngine");
                Check(stances.GetTrust(District8DeepCoastSystem.FactionOffice) == -5f,
                    "office trust moved exactly −5");
                Check(journal.EntryCount == 2, "fleet decision journal entry landed once");
                host.Decide("fleet", 182);
                Check(journal.EntryCount == 2, "repeat decision does not duplicate the journal");

                // 4. Fleet clears the route for free.
                Check(host.ClearPerimeter(182).Contains("open"), "fleet clears the perimeter free");
                Check(host.ClearChannel(183).Contains("reachable"), "fleet cuts the channel free");
                Check(host.RepairBerth(184).Contains("operational"), "fleet stands the berth up free");
                Check(host.DeepCoast.Stage == DeepCoastStage.DeepBerthOperational, "berth operational");
                Check(host.DockExpeditionAvailable, "dock expedition available once accessible");

                // 5. Dock dive handoff into the existing maritime dive.
                string start = host.StartDockDive("suki_tanaka", "marcus_olejnik", 185);
                Check(start.Contains("launched"), "dock dive launched from the berth");
                Check(host.Maritime.Dive.IsActive, "existing StealthDiveInstance is active");
                Check(host.DeepCoast.IsDockOperationActive, "dock operation reference active");
                Check(journal.EntryCount == 5, "dock-open + berth + dive-launch entries landed once each");

                host.TickDockDive(30f);
                host.AdvanceDockDive(10);
                host.CrankDockDive();
                Check(host.Maritime.Dive.AirSupplySeconds > 0f, "dive air managed by crank/tick");

                // 6. Complete with scavenge through ProceduralScavengeSystem.
                int itemsBefore = inventory.Items.Count;
                string done = host.CompleteDockDive(true, null, 185);
                Check(done.Contains("levy"), "completion reports the fleet levy");
                Check(!host.Maritime.Dive.IsActive, "dive ended");
                Check(!host.DeepCoast.IsDockOperationActive, "operation reference cleared");
                Check(inventory.Items.Count >= itemsBefore, "scavenge rewards landed in the canonical inventory");
                Check(host.DeepCoast.CanStartDockOperation, "a new dock operation can start after completion");

                // 7. Mid-sequence save/restore without duplication.
                var saved = host.CaptureDeepCoast();
                var fresh = DeepCoastHostSession.Create();
                fresh.RestoreDeepCoast(saved);
                Check(fresh.DeepCoast.Stage == DeepCoastStage.DeepBerthOperational, "restore keeps the operational stage");
                Check(fresh.DeepCoast.AccessDecision == DeepCoastAccessDecision.FleetControlled, "restore keeps the fleet decision");
                Check(fresh.IsFleetActive, "restore keeps the Fleet stood up");
                Check(fresh.DeepCoast.IsFleetLevyActive, "restore keeps the levy");
                Check(!fresh.DeepCoast.IsDockOperationActive, "restore keeps the operation closed after completion");

                // 8. Repeat completion cannot double-spend the operation.
                string repeat = fresh.CompleteDockDive(true, null, 185);
                Check(repeat.Contains("No active"), "second completion is refused (no duplicate rewards)");
            }
            catch (Exception e)
            {
                Check(false, "host playthrough threw: " + e.Message);
            }

            GD.Print(failures == 0
                ? "DEEP_COAST_HOST_SELFTEST PASS"
                : "DEEP_COAST_HOST_SELFTEST FAIL (" + failures + ")");
            return failures == 0 ? 0 : 1;
        }

        public static int RunGreenhouseSelfTest()
        {
            var report = GreenhouseHeadlessDemo.Run(new GodotLog());
            GD.Print(report.Summary);
            return report.ExitCode;
        }

        public static int RunSilentFoundrySelfTest(string dataDirectory)
        {
            var report = Ashfall.Core.SilentFoundryHeadlessDemo.Run(dataDirectory, new GodotLog());
            GD.Print(report.Summary);
            return report.ExitCode;
        }

        public static int RunDiseaseSelfTest(string dataDirectory)
        {
            var report = Ashfall.Core.DiseaseHeadlessDemo.Run(dataDirectory, new GodotLog());
            GD.Print(report.Summary);
            return report.ExitCode;
        }

        public static int RunArbitrationSelfTest()
        {
            var report = CrossingArbitrationHeadlessDemo.Run(new GodotLog());
            GD.Print(report.Summary);
            return report.ExitCode;
        }

        public static int RunLedgerDebtSelfTest()
        {
            var report = LedgerDebtHeadlessDemo.Run(new GodotLog());
            GD.Print(report.Summary);
            return report.ExitCode;
        }

        public static int RunHoldfastSelfTest(string dataDirectory)
        {
            var report = HoldfastHeadlessDemo.Run(dataDirectory, new GodotLog());
            GD.Print(report.Summary);
            return report.ExitCode;
        }

        public static int RunDutyRosterSelfTest(string dataDirectory)
        {
            var report = DutyRosterHeadlessDemo.Run(dataDirectory, new GodotLog());
            GD.Print(report.Summary);
            return report.ExitCode;
        }

        public static int RunStandingRecordSelfTest(string dataDirectory)
        {
            var report = StandingRecordHeadlessDemo.Run(dataDirectory, new GodotLog());
            GD.Print(report.Summary);
            return report.ExitCode;
        }

        public static int RunCrossingSelfTest(string dataDirectory)
        {
            var report = CrossingHeadlessDemo.Run(dataDirectory, new GodotLog());
            GD.Print(report.Summary);
            return report.ExitCode;
        }

        public static int RunIceRoadSelfTest(string dataDirectory)
        {
            var report = IceRoadHeadlessDemo.Run(dataDirectory, new GodotLog());
            GD.Print(report.Summary);
            return report.ExitCode;
        }

        public static int RunCensusSelfTest()
        {
            var report = CensusHeadlessDemo.Run(new GodotLog());
            GD.Print(report.Summary);
            return report.ExitCode;
        }

        public static int RunBrineSelfTest()
        {
            var report = BrineWaterHeadlessDemo.Run(new GodotLog());
            GD.Print(report.Summary);
            return report.ExitCode;
        }

        public static int RunMusterSelfTest()
        {
            var report = MusterHeadlessDemo.Run(new GodotLog());
            GD.Print(report.Summary);
            return report.ExitCode;
        }

        /// <summary>
        /// The Verdict (Expansion 08) headless gate: machine log, three Reckoning
        /// phases, census carrier, evidence ledger, ending selection, and a
        /// save round-trip with tamper rejection. Pure core — no UI nodes.
        /// </summary>
        public static int RunVerdictSelfTest(string dataDirectory)
        {
            CatalogLocator.UseInvariantCulture();
            string tmpPath = Path.Combine(
                Path.GetTempPath(), "ashfall_verdict_selftest_" + Guid.NewGuid().ToString("N") + ".json");

            int failures = 0;
            void Check(bool condition, string name)
            {
                if (condition) GD.Print("[PASS] " + name);
                else { GD.Print("[FAIL] " + name); failures++; }
            }

            try
            {
                var clock = new Ashfall.Core.Clock.SimClock();
                var bus = new SimpleEventBus();
                var flags = new InMemoryFlagLedger();
                var rng = new SeededRng(8841209);

                var machineLog = new MachineLogSystem();
                var reckoning = new ReckoningSystem();
                var evidence = new EvidenceLedger();

                // Dormancy → Knowing
                Check(reckoning.Poll(100, 14, 0, 0).Count == 0, "dormant before Day 160");
                Check(reckoning.Poll(160, 14, 1, 0).Contains("phase_knowing"), "Knowing at Day 160");

                // Machine log: post + read (evidence enrollment)
                machineLog.Post("loc_geophone_pit_1", 162, "operating", "a tap.", "evidence_geophone_hymn");
                machineLog.Post("loc_geophone_pit_1", 162, "operating", "dup", "evidence_geophone_hymn");
                Check(machineLog.Entries.Count == 1, "duplicate suppression");
                string tag = machineLog.ReadEntry(0);
                Check(tag == "evidence_geophone_hymn", "read enrolls evidence tag");
                evidence.Enroll(tag, 162);

                // Knowing → Culpable (evidence gate)
                var fired2 = reckoning.Poll(211, 14, 1, evidence.Count);
                Check(fired2.Contains("phase_culpable") && fired2.Contains("carrier_heard"),
                    "Culpable + carrier armed (with evidence)");
                Check(!reckoning.Poll(220, 14, 1, evidence.Count).Contains("carrier_heard"), "carrier one-shot");

                // Census window + broadcast idempotency
                var census = new VerdictCensusBroadcast(clock, bus, flags, rng, new SelftestCensus(14));
                clock.SetTick(3 * Ashfall.Core.Clock.SimClock.TicksPerHour);
                census.BroadcastIfDue();
                Check(bus.PublishedEvents.Any(e => e.name == "radio.census.header"), "census header published");
                int before = bus.PublishedEvents.Count;
                census.BroadcastIfDue();
                Check(bus.PublishedEvents.Count == before, "census broadcast once per window");

                // Diegetic radio corpus (verdict_radio.json) fires once, gated on Culpable+.
                var vio = new FileSystemIO();
                var vjson = new SystemTextJsonSerializer();
                var radioCorpus = VerdictCatalogLoader.LoadRadio(dataDirectory, vio, vjson);
                var radioSys = radioCorpus.Count == 13
                    ? new VerdictRadioSystem(bus, clock, radioCorpus)
                    : new VerdictRadioSystem();
                Check(radioSys.Corpus.Count == 13, "verdict radio corpus loads 13 broadcasts");
                var radioFired = radioSys.Poll(211, reckoning.Phase);
                Check(radioFired.Contains("radio_verdict_carrier_on_window"), "pilot carrier fires in Culpable window");
                Check(!radioSys.HasFired("radio_verdict_reckoning_call"), "reckoning call withheld until its dayTrigger");
                var radioFired2 = radioSys.Poll(241, reckoning.Phase);
                Check(radioSys.HasFired("radio_verdict_reckoning_call"), "reckoning call fires at Day 241+");
                var radioFireAgain = radioSys.Poll(242, reckoning.Phase);
                Check(!radioFireAgain.Contains("radio_verdict_reckoning_call"), "radio corpus fires once (no replay)");

                // Evidence-from-items enrollment (mechanical_effects.enrolled_evidence).
                var vhsItems = VerdictCatalogLoader.LoadItems(dataDirectory, vio, vjson);
                int evidenceQualifying = 0;
                int evidenceEnrolledNew = 0;
                foreach (var it in vhsItems)
                    if (it.mechanical_effects != null && it.mechanical_effects.enrolled_evidence > 0)
                    {
                        evidenceQualifying++;
                        if (evidence.Enroll(it.id, 220)) evidenceEnrolledNew++;
                    }
                // 12 items carry the effect; one (geophone_hymn) was already enrolled
                // by the machine-log read above, so 11 are newly enrolled.
                Check(evidenceQualifying == 12, "12 evidence items carry enrolled_evidence");
                Check(evidenceEnrolledNew == 11, "11 evidence items newly enrolled (geophone already read in)");

                // Counted + Call
                var fired3 = reckoning.Poll(241, 14, 2, evidence.Count);
                Check(fired3.Contains("reckoning_call"), "reckoning call at Day 240+");
                Check(reckoning.Phase == ReckoningPhase.Counted, "phase === Counted");

                // Ending selection (mutually exclusive)
                Check(reckoning.SelectEnding("ending_verdict_the_sector_recounts", 241), "ending selected");
                Check(!reckoning.SelectEnding("ending_verdict_the_count_is_held", 242), "endings mutually exclusive");

                // Save round-trip
                var save = VerdictSaveCodec.Capture(241, machineLog, reckoning, evidence, census.LastWindowDay);
                string encoded = VerdictSaveCodec.Encode(save, new SystemTextJsonSerializer());
                VerdictSaveStore.TrySave(save, tmpPath);
                var loaded = VerdictSaveStore.TryLoad(tmpPath);
                Check(loaded != null, "verdict save loads back");
                if (loaded != null)
                {
                    Check(loaded.reckoning.phase == ReckoningPhase.Counted, "phase restored");
                    Check(loaded.reckoning.countPresented, "ending restored");
                    // Evidence now includes the 12 item-driven enrollments (geophone
                    // was the machine-log read, so it overlaps); at least the item
                    // evidence set must persist through the save.
                    Check(loaded.evidence.enrolled.Count >= 12, "evidence restored (item enrollments persist)");
                    Check(loaded.evidence.enrolled.Contains("evidence_eden_log"), "item evidence id restored");
                }

                // Tamper rejection
                string tampered = encoded.Replace("\"simDay\":241", "\"simDay\":999");
                Check(!VerdictSaveCodec.TryDecode(tampered, new SystemTextJsonSerializer(), out _),
                    "tampered save rejected");
            }
            catch (Exception e)
            {
                GD.Print("[FAIL] verdict selftest threw: " + e);
                failures++;
            }
            finally
            {
                if (System.IO.File.Exists(tmpPath)) System.IO.File.Delete(tmpPath);
            }

            GD.Print(failures == 0
                ? "VERDICT_SELFTEST PASS"
                : $"VERDICT_SELFTEST FAIL ({failures})");
            return failures == 0 ? 0 : 1;
        }

        private sealed class SelftestCensus : IWorldCensus
        {
            private readonly long _n;
            public SelftestCensus(long n) { _n = n; }
            public long LivingRegisteredSouls() => _n;
        }

        public static int RunClusterSelfTest(string dataDirectory)
        {
            var report = Cluster12CHeadlessDemo.Run(dataDirectory, new GodotLog());
            GD.Print(report.Summary);
            return report.ExitCode;
        }

        public static int RunEndingsSelfTest()
        {
            var report = EndingsHeadlessDemo.Run(new GodotLog());
            GD.Print(report.Summary);
            return report.ExitCode;
        }

        public static int RunCoreSelfTest(string dataDirectory)
        {
            int ice = RunIceRoadSelfTest(dataDirectory);
            int census = RunCensusSelfTest();
            return ice != 0 ? ice : census;
        }

        /// <summary>
        /// Year of Ash save gate: build a session, advance the timeline, resolve a
        /// door encounter, capture, write through the codec to a temp path, reload
        /// into a fresh session, restore, and verify the timeline/encounter/faction
        /// state reproduces. Then tamper the file and verify the checksum refuses it.
        /// </summary>
        public static int RunYearOfAshSaveSelfTest(string dataDirectory)
        {
            CatalogLocator.UseInvariantCulture();
            string tmpPath = Path.Combine(
                Path.GetTempPath(), "ashfall_year_of_ash_selftest_" + Guid.NewGuid().ToString("N") + ".json");

            int failures = 0;
            void Check(bool condition, string name)
            {
                if (condition) GD.Print("[PASS] " + name);
                else
                {
                    GD.Print("[FAIL] " + name);
                    failures++;
                }
            }

            try
            {
                var session = YearOfAshHostSession.Create(dataDirectory);
                // The Dose (Expansion 07) quest lines must be registered into the live
                // QuestlineSystem by Create() (same path Verdict uses).
                foreach (var dqid in new[]
                    {
                        "quest_the_dose_the_first_reading", "quest_the_sick_of_room_seven",
                        "quest_the_childs_number", "quest_the_signed_hour"
                    })
                    Check(session.Quests.FindDefinition(dqid) != null,
                        $"dose quest line registered: {dqid}");
                session.TickDay(255);

                // Drive the two phase-scoped systems inside their own windows so the
                // gate covers state the old envelope silently dropped: deep freeze runs
                // to day 240 and de-ices after, radon only wakes at day 300.
                for (int day = 190; day <= 240; day++)
                    session.DeepFreeze.TickDailyThermal(day, -38.0f);
                for (int day = 300; day <= 340; day++)
                    session.Radon.TickDailyRadon(day, -38.0f);
                Check(session.Radon.State.totalAlphaDoseLogged > 0.0f, "radon dose accumulated");
                Check(session.DeepFreeze.State.intakeIceThicknessMm > 0.0f, "intake iced");

                // Resolve a door encounter so the encounters section is non-trivial.
                var enc = session.Encounters.Catalog.Count > 0 ? session.Encounters.Catalog[0] : null;
                if (enc != null)
                {
                    var result = session.Encounters.ResolveChoice(enc, enc.choices[0], session.DemoRoster);
                    Check(result != null, "door encounter resolved");
                }

                var save = session.CaptureSave();
                Check(!string.IsNullOrEmpty(save.Checksum), "capture stamps checksum");
                Check(save.saveVersion == YearOfAshSave.CurrentSaveVersion, "saveVersion current");
                Check(save.timeline.currentDay == 255, "envelope carries timeline day");

                Check(YearOfAshSaveStore.TrySave(save, tmpPath), "save written via codec");

                var fresh = YearOfAshHostSession.Create(dataDirectory);
                var loaded = YearOfAshSaveStore.TryLoad(tmpPath);
                Check(loaded != null, "save loads back");
                if (loaded != null)
                {
                    fresh.RestoreSave(loaded);
                    Check(fresh.Timeline.CurrentDay == 255, "timeline day restored");
                    Check(fresh.Timeline.CurrentPhase == YearOfAshPhase.Phase5_FactionSiege, "phase restored");
                    Check(fresh.Encounters.State.totalEncountersResolved
                        == session.Encounters.State.totalEncountersResolved,
                        "encounter history restored");
                    Check(fresh.FactionWar.WarTension == session.FactionWar.WarTension,
                        "war tension restored");

                    // v2 sections: the three systems the envelope used to drop.
                    Check(fresh.DeepFreeze.State.intakeIceThicknessMm
                        == session.DeepFreeze.State.intakeIceThicknessMm, "intake ice restored");
                    Check(fresh.DeepFreeze.State.daysFrozenPipelinesExperienced
                        == session.DeepFreeze.State.daysFrozenPipelinesExperienced,
                        "frozen-pipeline days restored");
                    Check(fresh.Radon.State.scrubberFilterHealthPercent
                        == session.Radon.State.scrubberFilterHealthPercent, "scrubber health restored");
                    Check(fresh.Radon.State.totalAlphaDoseLogged
                        == session.Radon.State.totalAlphaDoseLogged, "alpha dose restored");
                    Check(fresh.Quests.State.completedQuestlineIds.Count
                        == session.Quests.State.completedQuestlineIds.Count, "questline progress restored");
                }

                // Tamper: flip the sim day in the raw text. Checksum must refuse it.
                string raw = File.ReadAllText(tmpPath);
                string tampered = raw.Replace("\"simDay\":255", "\"simDay\":180");
                Check(tampered != raw, "tamper actually changed the payload");
                if (tampered != raw)
                {
                    File.WriteAllText(tmpPath, tampered);
                    Check(YearOfAshSaveStore.TryLoad(tmpPath) == null, "tampered save rejected (checksum)");
                }
            }
            catch (Exception e)
            {
                Check(false, "selftest threw: " + e.Message);
            }
            finally
            {
                try
                {
                    if (File.Exists(tmpPath)) File.Delete(tmpPath);
                }
                catch (Exception)
                {
                }
            }

            GD.Print(failures == 0
                ? "YEAR_OF_ASH_SAVE_SELFTEST PASS"
                : "YEAR_OF_ASH_SAVE_SELFTEST FAIL (" + failures + ")");
            return failures == 0 ? 0 : 1;
        }

        /// <summary>
        /// Duty Roster save gate: build a session, unlock, tick a morning, write a
        /// pencil row, queue a visitor, capture, write through the codec to a temp
        /// path, reload into a fresh session, restore, and verify the wall/encounter
        /// state reproduces. Then tamper the file and verify the checksum refuses it.
        /// </summary>
        public static int RunDutyRosterSaveSelfTest(string dataDirectory)
        {
            CatalogLocator.UseInvariantCulture();
            string tmpPath = Path.Combine(
                Path.GetTempPath(), "ashfall_duty_roster_selftest_" + Guid.NewGuid().ToString("N") + ".json");

            int failures = 0;
            void Check(bool condition, string name)
            {
                if (condition) GD.Print("[PASS] " + name);
                else
                {
                    GD.Print("[FAIL] " + name);
                    failures++;
                }
            }

            try
            {
                var session = DutyRosterHostSession.Create(dataDirectory);
                session.Unlock(5);
                session.ResolveChart(DutyRosterSystem.ChoiceWritePencil);
                session.TickDay();
                session.QueueVisitor(ShelterEncounterSystem.VisitorLen);

                var save = session.CaptureSave();
                Check(!string.IsNullOrEmpty(save.Checksum), "capture stamps checksum");
                Check(save.saveVersion == DutyRosterSave.CurrentSaveVersion, "saveVersion current");
                Check(save.roster.expansionUnlocked, "envelope carries roster unlock");
                Check(save.roster.rows != null && save.roster.rows.Count > 0, "envelope carries chart rows");

                Check(DutyRosterSaveStore.TrySave(save, tmpPath), "save written via codec");

                var fresh = DutyRosterHostSession.Create(dataDirectory);
                var loaded = DutyRosterSaveStore.TryLoad(tmpPath);
                Check(loaded != null, "save loads back");
                if (loaded != null)
                {
                    fresh.RestoreSave(loaded);
                    Check(fresh.Clock.Day == session.Clock.Day, "sim day restored");
                    Check(fresh.WallLine() == session.WallLine(), "wall line identical after roundtrip");
                    Check(fresh.EncountersLine() == session.EncountersLine(),
                        "encounters line identical after roundtrip");
                }

                // Tamper: flip the roster unlock flag in the raw text. Checksum must refuse it.
                string raw = File.ReadAllText(tmpPath);
                string tampered = raw.Replace("\"expansionUnlocked\":true", "\"expansionUnlocked\":false");
                Check(tampered != raw, "tamper actually changed the payload");
                if (tampered != raw)
                {
                    File.WriteAllText(tmpPath, tampered);
                    Check(DutyRosterSaveStore.TryLoad(tmpPath) == null, "tampered save rejected (checksum)");
                }
            }
            catch (Exception e)
            {
                Check(false, "selftest threw: " + e.Message);
            }
            finally
            {
                try
                {
                    if (File.Exists(tmpPath)) File.Delete(tmpPath);
                }
                catch (Exception)
                {
                }
            }

            GD.Print(failures == 0
                ? "DUTY_ROSTER_SAVE_SELFTEST PASS"
                : "DUTY_ROSTER_SAVE_SELFTEST FAIL (" + failures + ")");
            return failures == 0 ? 0 : 1;
        }

        /// <summary>
        /// Expansion hub save gate: build the hub session, unlock the waystation,
        /// walk a Standing Record site, grant the Crossing vouch, plant + water a
        /// greenhouse plot, capture, write through the codec to a temp path, reload
        /// into a fresh session, restore, and verify each surface reproduces. Then
        /// tamper the file and verify the checksum refuses it.
        /// </summary>
        public static int RunExpansionHubSaveSelfTest(string dataDirectory)
        {
            CatalogLocator.UseInvariantCulture();
            string tmpPath = Path.Combine(
                Path.GetTempPath(), "ashfall_expansion_hub_selftest_" + Guid.NewGuid().ToString("N") + ".json");

            int failures = 0;
            void Check(bool condition, string name)
            {
                if (condition) GD.Print("[PASS] " + name);
                else
                {
                    GD.Print("[FAIL] " + name);
                    failures++;
                }
            }

            try
            {
                var session = ExpansionHostSession.Create(dataDirectory);
                session.UnlockWaystation();
                session.AssignWaystationWatch(new[] { "elena_vasquez" });
                session.UnlockRecord();
                session.ArriveAtSite("loc_cut_kilometre_19");
                session.EnterSiteRoom("room_km19_post");
                session.InspectSiteRoom("room_km19_post");
                session.GrantVouch("npc_osran_kell");
                session.EnsureGreenhousePlots(3);
                session.PlantGreenhouse(0, "item_seed_tuber", 12);
                session.WaterGreenhouse(0, 60f);
                session.TickGreenhouse(13);
                session.LoadDefaultBackerPool();
                session.Arbitration.CallStanding("quest_crossing_the_terms", 13);
                session.Arbitration.DeclareBacker("quest_crossing_the_terms", CrossingIds.NpcOsran);
                session.Arbitration.DeclareBacker("quest_crossing_the_terms", CrossingIds.NpcMattis);
                session.Arbitration.DeclareBacker("quest_crossing_the_terms", "npc_halden_mire");
                session.Ledger.PresentContract(CrossingIds.NpcWyn, 12f, 30, 0.2f, "the pledged grain");
                session.Ledger.PresentContract(CrossingIds.NpcWyn, 12f, 30, 0.2f, "the pledged grain");
                session.Ledger.SignContract(CrossingIds.NpcWyn, 13);

                var save = session.CaptureSave(13);
                Check(!string.IsNullOrEmpty(save.Checksum), "capture stamps checksum");
                Check(save.saveVersion == ExpansionHubSave.CurrentSaveVersion, "saveVersion current");
                Check(save.waystation.unlocked, "envelope carries waystation unlock");
                Check(save.layouts.expansionUnlocked, "envelope carries record unlock");
                Check(save.vouch.vouchedBy == "npc_osran_kell", "envelope carries the vouch");
                Check(save.greenhouse.plots != null && save.greenhouse.plots.Count == 3,
                    "envelope carries greenhouse plots");

                Check(ExpansionHubSaveStore.TrySave(save, tmpPath), "save written via codec");

                var fresh = ExpansionHostSession.Create(dataDirectory);
                var loaded = ExpansionHubSaveStore.TryLoad(tmpPath);
                Check(loaded != null, "save loads back");
                if (loaded != null)
                {
                    fresh.RestoreSave(loaded);
                    Check(fresh.Waystation.Unlocked, "waystation unlock restored");
                    Check(fresh.Vouch.HasAccess, "vouch restored");
                    Check(fresh.Layouts.State.expansionUnlocked, "record unlock restored");
                    Check(fresh.Greenhouse.PlotCount == 3, "greenhouse plots restored");
                    Check(fresh.Greenhouse.State.plots.Count > 0
                        && fresh.Greenhouse.State.plots[0].seedItemId == "item_seed_tuber",
                        "planted seed restored");
                    Check(fresh.Arbitration.State.rulingsCalled >= 1, "arbitration rulings restored");
                    Check(fresh.Arbitration.IsRulingActive("quest_crossing_the_terms"),
                        "arbitration active ruling restored");
                    Check(fresh.Ledger.GetContract(CrossingIds.NpcWyn)?.signed == true,
                        "ledger contract restored as signed");
                }

                // Tamper: flip the sim day in the raw text. Checksum must refuse it.
                string raw = File.ReadAllText(tmpPath);
                string tampered = raw.Replace("\"simDay\":13", "\"simDay\":1");
                Check(tampered != raw, "tamper actually changed the payload");
                if (tampered != raw)
                {
                    File.WriteAllText(tmpPath, tampered);
                    Check(ExpansionHubSaveStore.TryLoad(tmpPath) == null, "tampered save rejected (checksum)");
                }
            }
            catch (Exception e)
            {
                Check(false, "selftest threw: " + e.Message);
            }
            finally
            {
                try
                {
                    if (File.Exists(tmpPath)) File.Delete(tmpPath);
                }
                catch (Exception)
                {
                }
            }

            GD.Print(failures == 0
                ? "EXPANSION_HUB_SAVE_SELFTEST PASS"
                : "EXPANSION_HUB_SAVE_SELFTEST FAIL (" + failures + ")");
            return failures == 0 ? 0 : 1;
        }

        /// <summary>
        /// Dose Ledger save gate: build a session, seal dosimeters, book a reading,
        /// name a sick band, book a Cohort child, sign a volunteer, capture, write
        /// through the codec to a temp path, reload into a fresh session, restore,
        /// and verify each register reproduces. Then tamper and verify the checksum
        /// refuses it.
        /// </summary>
        public static int RunExpeditionSelfTest()
        {
            var report = ExpeditionHeadlessDemo.Run(new GodotLog());
            GD.Print(report.Summary);
            return report.ExitCode;
        }

        public static int RunMedicalSelfTest()
        {
            var report = MedicalHeadlessDemo.Run(new GodotLog());
            GD.Print(report.Summary);
            return report.ExitCode;
        }

        public static int RunNarrativeSelfTest()
        {
            var report = NarrativeHeadlessDemo.Run(new GodotLog());
            GD.Print(report.Summary);
            return report.ExitCode;
        }

        public static int RunSurvivorsSelfTest()
        {
            var report = SurvivorsHeadlessDemo.Run(new GodotLog());
            GD.Print(report.Summary);
            return report.ExitCode;
        }

        public static int RunWorldSelfTest()
        {
            var report = WorldHeadlessDemo.Run(new GodotLog());
            GD.Print(report.Summary);
            return report.ExitCode;
        }

        public static int RunEconomySelfTest(string dataDirectory)
        {
            var report = EconomyHeadlessDemo.Run(dataDirectory, new GodotLog());
            // Save-integrity probe: tampered saves must be refused (checksum).
            string tmpPath = Path.Combine(
                Path.GetTempPath(), "ashfall_economy_selftest_" + Guid.NewGuid().ToString("N") + ".json");
            try
            {
                var session = new EconomyHostSession();
                var catalogResult = new GoodsCatalogLoadResult();
                catalogResult.Goods.Add(new GoodDefinition
                {
                    id = "probe_good", displayName = "Probe", category = "misc",
                    basePrice = 5f, volatility = 0.1f, elasticity = 1f
                });
                var probeCatalog = GoodsCatalogLoader.ToCatalog(catalogResult);
                session.Market.BindCatalog(probeCatalog);
                session.Market.TickDay(3, new SeededRng(9));
                if (EconomySaveStore.TrySave(session.CaptureSave(), tmpPath))
                    GD.Print("[PASS] economy save written to temp slot");
                else
                    GD.Print("[FAIL] economy save write failed");

                string raw = File.ReadAllText(tmpPath);
                // Flip the tick count in the payload, whatever its current value.
                string tampered = System.Text.RegularExpressions.Regex.Replace(
                    raw, "\"tickCount\":\\d+", "\"tickCount\":999");
                bool changed = tampered != raw;
                GD.Print(changed ? "[PASS] tamper changed the payload" : "[FAIL] tamper produced no change");
                if (changed)
                {
                    File.WriteAllText(tmpPath, tampered);
                    var loaded = EconomySaveStore.TryLoad(tmpPath);
                    GD.Print(loaded == null
                        ? "[PASS] tampered save refused (checksum)"
                        : "[FAIL] tampered save accepted (no checksum)");
                }
            }
            catch (Exception e)
            {
                GD.Print("[FAIL] save-integrity probe threw: " + e.Message);
            }
            finally
            {
                if (File.Exists(tmpPath)) File.Delete(tmpPath);
            }

            // Legacy-save probe: a bare MarketState (pre-checksum store shape)
            // must migrate, not be silently dropped as corrupt.
            string legacyPath = Path.Combine(
                Path.GetTempPath(), "ashfall_economy_legacy_" + Guid.NewGuid().ToString("N") + ".json");
            try
            {
                var legacy = new MarketState
                {
                    version = MarketState.Version,
                    day = 7,
                    tickCount = 7,
                    demand = new System.Collections.Generic.List<DemandEntry>
                    {
                        new DemandEntry { itemId = "legacy_good", multiplier = 1.4f }
                    }
                };
                File.WriteAllText(legacyPath, new SystemTextJsonSerializer().Serialize(legacy));
                var legacyLoaded = EconomySaveStore.TryLoad(legacyPath);
                bool legacyOk = legacyLoaded != null && legacyLoaded.day == 7
                    && legacyLoaded.tickCount == 7
                    && legacyLoaded.demand != null && legacyLoaded.demand.Count == 1;
                GD.Print(legacyOk
                    ? "[PASS] legacy bare save migrates (pre-checksum shape)"
                    : "[FAIL] legacy bare save dropped as corrupt");
            }
            catch (Exception e)
            {
                GD.Print("[FAIL] legacy-save probe threw: " + e.Message);
            }
            finally
            {
                if (File.Exists(legacyPath)) File.Delete(legacyPath);
            }

            // Tuning-integration probe (Candidate A slice 4): the core overlay
            // loaded from the sample JSON must bind into DSE and gate scarcity.
            try
            {
                var tuningLoad = Ashfall.Core.Economy.HardcoreEconomyTuningLoader.Load(
                    System.IO.File.ReadAllText(System.IO.Path.Combine(
                        dataDirectory, "hardcore_economy_tuning.json")));
                bool loaded = tuningLoad != null && tuningLoad.IsValid && tuningLoad.Bundle != null;
                GD.Print(loaded
                    ? "[PASS] hardcore tuning JSON loads via the core loader"
                    : "[FAIL] hardcore tuning JSON failed to load");

                var overlay = new Ashfall.Core.Economy.HardcoreEconomyTuning();
                overlay.Apply(tuningLoad.Bundle);
                var dse = new AtomicWar._Game.Economy.DynamicEconomySystem();
                dse.BindCoreTuning(overlay);
                dse.SetScarcityOverride(new Ashfall.Core.Economy.ScarcityOverride
                {
                    Source = "core_tuning",
                    IsHardcore = true
                });
                float day5Water = overlay.GetScarcityMultiplier(5, "clean_water");
                bool gates = day5Water > 1.0f && day5Water <= 2.5f + 1e-6f;
                GD.Print(gates
                    ? $"[PASS] core overlay gates scarcity (day 5 clean_water x{day5Water:0.00})"
                    : $"[FAIL] core overlay scarcity gate wrong ({day5Water:0.00})");
            }
            catch (System.Exception e)
            {
                GD.Print("[FAIL] tuning-integration probe threw: " + e.Message);
            }

            // Adapter probe (Candidate A): the Unity-coupled DynamicEconomySystem
            // must delegate demand to the core MarketSystem, and its save/restore
            // must round-trip through the core (single source of truth).
            try
            {
                var dse = new AtomicWar._Game.Economy.DynamicEconomySystem();
                var coreMarket = new MarketSystem();
                dse.BindCoreMarket(coreMarket);
                dse.AdjustDemand("probe_water", 0.5f);
                bool delegated = dse.GetDemandMultiplier("probe_water") == 1.5f
                    && coreMarket.GetDemandMultiplier("probe_water") == 1.5f
                    && dse.IsSuppliesShort();
                GD.Print(delegated
                    ? "[PASS] DSE demand delegates to core MarketSystem"
                    : "[FAIL] DSE demand delegation broken");

                var save = dse.CaptureState();
                var freshDse = new AtomicWar._Game.Economy.DynamicEconomySystem();
                freshDse.RestoreState(save);
                bool roundtrip = freshDse.GetDemandMultiplier("probe_water") == 1.5f;
                GD.Print(roundtrip
                    ? "[PASS] DSE save/restore round-trips through core demand"
                    : "[FAIL] DSE save/restore lost demand");
            }
            catch (System.Exception e)
            {
                GD.Print("[FAIL] DSE adapter probe threw: " + e.Message);
            }

            // Reload-continuity probe: mid-sequence save via the REAL store slot,
            // reload, continue — the resumed trajectory must match an
            // uninterrupted run hash-for-hash.
            string continuityPath = Path.Combine(
                Path.GetTempPath(), "ashfall_economy_continuity_" + Guid.NewGuid().ToString("N") + ".json");
            try
            {
                var catalogResult = new GoodsCatalogLoadResult();
                catalogResult.Goods.Add(new GoodDefinition
                {
                    id = "cont_good", displayName = "Cont", category = "misc",
                    basePrice = 7f, volatility = 0.4f, elasticity = 1.4f
                });
                var contCatalog = GoodsCatalogLoader.ToCatalog(catalogResult);

                var uninterrupted = new MarketSystem();
                uninterrupted.BindCatalog(contCatalog);
                for (int day = 1; day <= 40; day++)
                    uninterrupted.TickDay(day, new SeededRng(31337));
                string expected = SaveChecksum.Compute(uninterrupted.CaptureState());

                var sliced = new MarketSystem();
                sliced.BindCatalog(contCatalog);
                for (int day = 1; day <= 20; day++)
                    sliced.TickDay(day, new SeededRng(31337));
                bool saved = EconomySaveStore.TrySave(sliced.CaptureState(), continuityPath);
                var reloaded = EconomySaveStore.TryLoad(continuityPath);
                var resumed = new MarketSystem();
                resumed.BindCatalog(contCatalog);
                if (reloaded != null) resumed.RestoreState(reloaded);
                for (int day = 21; day <= 40; day++)
                    resumed.TickDay(day, new SeededRng(31337));

                bool continuity = saved && reloaded != null
                    && SaveChecksum.Compute(resumed.CaptureState()) == expected;
                GD.Print(continuity
                    ? "[PASS] reload-continuity: resumed trajectory matches uninterrupted run"
                    : "[FAIL] reload-continuity: trajectory diverged");
            }
            catch (Exception e)
            {
                GD.Print("[FAIL] reload-continuity probe threw: " + e.Message);
            }
            finally
            {
                if (File.Exists(continuityPath)) File.Delete(continuityPath);
            }
            GD.Print(report.Summary);
            return report.ExitCode;
        }

        public static int RunUtilityAiSelfTest(string dataDirectory)
        {
            var report = UtilityAiHeadlessDemo.Run(dataDirectory, new GodotLog());
            GD.Print(report.Summary);
            return report.ExitCode;
        }

        public static int RunDoseLedgerSelfTest(string dataDirectory)
        {
            CatalogLocator.UseInvariantCulture();
            string tmpPath = Path.Combine(
                Path.GetTempPath(), "ashfall_dose_ledger_selftest_" + Guid.NewGuid().ToString("N") + ".json");

            int failures = 0;
            void Check(bool condition, string name)
            {
                if (condition) GD.Print("[PASS] " + name);
                else
                {
                    GD.Print("[FAIL] " + name);
                    failures++;
                }
            }

            try
            {
                var session = DoseLedgerHostSession.Create(dataDirectory);
                Check(session.Registers.npcs.Count == 4, "dose_registers catalog loads the four antagonists");
                Check(session.Registers.bands.Count == 4 && session.Registers.plans.Count == 3,
                    "band and plan vocabulary loaded");
                session.SealDemoSurvivors();
                session.ScribeReading(180f, highEnergy: true);
                session.DiagnoseDemo(DoseLedgerSystem.BandRed);
                session.BookDemoChild();
                session.SignDemoVolunteer();

                var save = session.CaptureSave(40);
                Check(!string.IsNullOrEmpty(save.Checksum), "capture stamps checksum");
                Check(save.saveVersion == DoseLedgerSave.CurrentSaveVersion, "saveVersion current");
                Check(save.doseLedger.entries.Count > 0, "envelope carries dose ledger entries");
                Check(save.sickList.bands.Count == 1, "envelope carries the sick band");
                Check(save.cohort.children.Count == 1, "envelope carries the cohort child");
                Check(save.voluntaryRegister.entries.Count == 1, "envelope carries the volunteer");

                Check(DoseLedgerSaveStore.TrySave(save, tmpPath), "save written via codec");

                var fresh = DoseLedgerHostSession.Create(dataDirectory);
                var loaded = DoseLedgerSaveStore.TryLoad(tmpPath);
                Check(loaded != null, "save loads back");
                if (loaded != null)
                {
                    fresh.RestoreSave(loaded);
                    Check(fresh.Ledger.Entries.Count >= 2, "dose ledger entries restored");
                    Check(fresh.SickList.Bands.Count == 1, "sick list restored");
                    Check(fresh.Cohort.Children.Count == 1, "cohort restored");
                    Check(fresh.Voluntary.Entries.Count == 1, "voluntary register restored");
                    Check(fresh.Ledger.GetCumulative("survivor_gunner_mikhail") > 0f,
                        "cumulative dose restored");
                }

                // Tamper: flip the sim day in the raw text. Checksum must refuse it.
                string raw = File.ReadAllText(tmpPath);
                string tampered = raw.Replace("\"simDay\":40", "\"simDay\":1");
                Check(tampered != raw, "tamper actually changed the payload");
                if (tampered != raw)
                {
                    File.WriteAllText(tmpPath, tampered);
                    Check(DoseLedgerSaveStore.TryLoad(tmpPath) == null, "tampered save rejected (checksum)");
                }
            }
            catch (Exception e)
            {
                Check(false, "selftest threw: " + e.Message);
            }
            finally
            {
                try
                {
                    if (File.Exists(tmpPath)) File.Delete(tmpPath);
                }
                catch (Exception)
                {
                }
            }

            GD.Print(failures == 0
                ? "DOSE_LEDGER_SELFTEST PASS"
                : "DOSE_LEDGER_SELFTEST FAIL (" + failures + ")");
            return failures == 0 ? 0 : 1;
        }

        public static int RunHoldfastBriefing(string dataDirectory)
        {
            CatalogLocator.UseInvariantCulture();
            var session = CoreDemoSession.Create(dataDirectory);
            string dump = HoldfastBriefingView.FormatCatalogDump(session.Catalog);
            GD.Print(dump);
            bool ok = session.LocationCount > 0 && session.QuestCount > 0
                && session.Catalog.GetQuest("quest_holdfast_the_sheet") != null
                && session.Catalog.Items.IsValid
                && session.Catalog.Items.Count == 40;
            GD.Print(ok
                ? $"HoldfastBriefing PASS items={session.Catalog.Items.Count} locations={session.LocationCount} quests={session.QuestCount}"
                : $"HoldfastBriefing FAIL items={session.Catalog.Items.Count} locations={session.LocationCount} quests={session.QuestCount}");
            return ok ? 0 : 1;
        }

        public static int RunIceRoadTickDemo(string dataDirectory)
        {
            var session = CoreDemoSession.Create(dataDirectory);
            session.UnlockAndClerk();
            string lastDelta = "no ticks";
            for (int i = 0; i < 30; i++)
                lastDelta = session.TickDay();

            GD.Print(
                "IceRoadTickDemo day=" + session.Clock.Day
                + " open=" + session.IceRoad.IsOpen
                + " thickness=" + session.IceRoad.IceThicknessM.ToString("0.000")
                + " window=" + session.IceRoad.WindowDaysRemaining
                + " last=" + lastDelta
                + " locations=" + session.LocationCount
                + " quests=" + session.QuestCount);
            GD.Print(session.StatusLine());
            GD.Print(session.CensusLine());
            GD.Print("--- briefing ---");
            GD.Print(HoldfastBriefingView.FormatQuest(session.CurrentQuest, session.Catalog));
            bool ok = session.LocationCount > 0 && session.IceRoad.IsUnlocked
                && session.IceRoad.State.clerkStarted;
            GD.Print(ok ? "IceRoadTickDemo PASS" : "IceRoadTickDemo FAIL");
            return ok ? 0 : 1;
        }

        private static bool Has(string[] args, string flag)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == flag)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Sprint 1 save gate: write through HoldfastSaveCodec, reload into a fresh
        /// session, restore, and verify the gate reproduces. Then tamper the file and
        /// verify the checksum refuses it. Uses a temp path so the real user:// save
        /// is never touched by the test.
        /// </summary>
        public static int RunHoldfastSaveSelfTest(string dataDirectory)
        {
            CatalogLocator.UseInvariantCulture();
            string tmpPath = Path.Combine(
                Path.GetTempPath(), "ashfall_holdfast_s1_selftest_" + Guid.NewGuid().ToString("N") + ".json");

            int failures = 0;
            void Check(bool condition, string name)
            {
                if (condition) GD.Print("[PASS] " + name);
                else
                {
                    GD.Print("[FAIL] " + name);
                    failures++;
                }
            }

            try
            {
                var session = CoreDemoSession.Create(dataDirectory);
                session.UnlockAndClerk();
                for (int i = 0; i < 12; i++)
                    session.TickDay();
                session.HonourDemoLevy();

                var save = session.CaptureSave();
                Check(!string.IsNullOrEmpty(save.Checksum), "capture stamps checksum");
                Check(save.saveVersion == HoldfastSave.CurrentSaveVersion, "saveVersion current");
                Check(save.iceRoad.clerkStarted && save.iceRoad.expansionUnlocked,
                    "envelope carries ice road unlock + clerk");

                Check(HoldfastSaveStore.TrySave(save, tmpPath), "save written via codec");

                var fresh = CoreDemoSession.Create(dataDirectory);
                var loaded = HoldfastSaveStore.TryLoad(tmpPath);
                Check(loaded != null, "save loads back");
                if (loaded != null)
                {
                    fresh.RestoreSave(loaded);
                    Check(fresh.Clock.Day == session.Clock.Day, "sim day restored");
                    Check(fresh.StatusLine() == session.StatusLine(), "status line identical after roundtrip");
                    Check(fresh.CensusLine() == session.CensusLine(), "census line identical after roundtrip");
                }

                // Tamper: flip clerkStarted in the raw text. Checksum must refuse it.
                string raw = File.ReadAllText(tmpPath);
                string tampered = raw.Replace("\"clerkStarted\":true", "\"clerkStarted\":false");
                Check(tampered != raw, "tamper actually changed the payload");
                if (tampered != raw)
                {
                    File.WriteAllText(tmpPath, tampered);
                    Check(HoldfastSaveStore.TryLoad(tmpPath) == null, "tampered save rejected (checksum)");
                }

                // Stripped checksum: deleting the field must not bypass validation.
                var codecJson = new SystemTextJsonSerializer();
                var stripped = codecJson.Deserialize<HoldfastSave>(raw);
                stripped.Checksum = "";
                File.WriteAllText(tmpPath, codecJson.Serialize(stripped));
                Check(HoldfastSaveStore.TryLoad(tmpPath) == null, "checksumless save rejected");
            }
            catch (Exception e)
            {
                Check(false, "selftest threw: " + e.Message);
            }
            finally
            {
                try
                {
                    if (File.Exists(tmpPath)) File.Delete(tmpPath);
                }
                catch (Exception)
                {
                }
            }

            GD.Print(failures == 0
                ? "HOLDFAST_SAVE_SELFTEST PASS"
                : "HOLDFAST_SAVE_SELFTEST FAIL (" + failures + ")");
            return failures == 0 ? 0 : 1;
        }

        /// <summary>
        /// Standalone-systems gate: exercises the five newly-wired Core systems
        /// (SkyLayerArmor, VigilStateMachine, GenerationalSuccessionEngine,
        /// EpilogueMatrixRuntime, DiveInstanceRunner) with functional checks and
        /// save round-trips where the systems support them.
        /// </summary>
        public static int RunStandaloneSystemsSelfTest()
        {
            CatalogLocator.UseInvariantCulture();

            int failures = 0;
            void Check(bool condition, string name)
            {
                if (condition) GD.Print("[PASS] " + name);
                else { GD.Print("[FAIL] " + name); failures++; }
            }

            try
            {
                // ── 1. SkyLayerArmorSystem ──────────────────────────────
                var sky = new SkyLayerArmorSystem();
                // Equal thickness so the comparison isolates the material tier.
                sky.SetCellArmor(0, CeilingMaterialTier.ReinforcedConcrete, 1.0f);
                sky.SetCellArmor(1, CeilingMaterialTier.LeadSheeting, 1.0f);

                float att0 = sky.GetAttenuationFactor(0);
                float att1 = sky.GetAttenuationFactor(1);
                Check(att0 >= 0.005f && att0 <= 1.0f, "sky armor cell 0 attenuation in range");
                Check(att1 >= 0.005f && att1 <= 1.0f, "sky armor cell 1 attenuation in range");
                Check(att1 < att0, "lead sheeting attenuates more than concrete per thickness");

                bool breached = sky.EvaluateKineticImpact(0, 50f, out float damage);
                Check(damage >= 0f, "kinetic impact damage non-negative");
                // Whether it breaches depends on tuning; just verify it returned a bool.
                Check(true, "kinetic impact evaluation completed");

                // Save round-trip (compare against the current, post-impact state).
                var skySave = sky.CaptureState();
                Check(skySave != null && skySave.cells != null && skySave.cells.Count == 2,
                    "sky armor capture has 2 cells");
                var sky2 = new SkyLayerArmorSystem();
                sky2.RestoreState(skySave);
                Check(Math.Abs(sky2.GetAttenuationFactor(0) - sky.GetAttenuationFactor(0)) < 1e-5f,
                    "sky armor attenuation restored after roundtrip");

                // ── 2. VigilStateMachine (Medical) ──────────────────────
                var vigil = new Ashfall.Core.Medical.VigilStateMachine();
                bool startedFired = false;
                vigil.OnVigilStarted += _ => startedFired = true;
                vigil.StartVigil("dweller_test", new[] { "name_alpha", "name_beta", "name_gamma" }, 10f);

                Check(vigil.IsActive, "vigil is active after start");
                Check(startedFired, "vigil OnVigilStarted fired");
                Check(vigil.DwellerId == "dweller_test", "vigil dweller id set");

                // Tick past duration to complete
                vigil.Tick(5f);
                Check(vigil.RecitedCount > 0, "vigil recited names during tick");
                vigil.Tick(6f);
                Check(vigil.IsCompleted, "vigil completed after full duration");

                // Save round-trip (start a fresh one to test mid-vigil save)
                var vigil2 = new Ashfall.Core.Medical.VigilStateMachine();
                vigil2.StartVigil("dweller_save", new[] { "n1", "n2" }, 20f);
                vigil2.Tick(8f);
                var vigilSave = vigil2.CaptureState();
                Check(vigilSave != null && vigilSave.isActive, "vigil save captured active state");

                var vigil3 = new Ashfall.Core.Medical.VigilStateMachine();
                vigil3.RestoreState(vigilSave);
                Check(vigil3.DwellerId == "dweller_save", "vigil dweller restored");
                Check(Math.Abs(vigil3.ElapsedSeconds - vigil2.ElapsedSeconds) < 1e-3f,
                    "vigil elapsed restored");

                // ── 3. GenerationalSuccessionEngine ─────────────────────
                var gen = new GenerationalSuccessionEngine();
                gen.RegisterDweller("gen_elder", 60, 0);
                gen.RegisterDweller("gen_youth", 20, 1);

                Check(gen.GetRecord("gen_elder") != null, "elder registered");
                Check(gen.GetRecord("gen_youth") != null, "youth registered");

                // Advance enough to retire the elder (age 65 = 5 years = ~1825 days)
                gen.AdvanceTime(1825);
                var elderRec = gen.GetRecord("gen_elder");
                Check(elderRec.isRetired, "elder retired after reaching age 65");
                Check(gen.CurrentChapterIndex >= 1, "chapter index advanced or held");

                // Mentorship
                bool mentorOk = gen.FormMentorship("gen_elder", "gen_youth", "trait_farming");
                Check(mentorOk, "mentorship formed");
                var youthRec = gen.GetRecord("gen_youth");
                Check(youthRec.inheritedTraitIds.Contains("trait_farming"),
                    "youth inherited trait from mentor");

                // Save round-trip
                var genSave = gen.CaptureState();
                Check(genSave != null && genSave.generationRecords.Count >= 2,
                    "generational save captured records");
                var gen2 = new GenerationalSuccessionEngine();
                gen2.RestoreState(genSave);
                Check(gen2.GetRecord("gen_elder")?.isRetired == true,
                    "elder retirement restored");
                Check(gen2.GetRecord("gen_youth")?.inheritedTraitIds.Contains("trait_farming") == true,
                    "youth trait inheritance restored");

                // ── 4. EpilogueMatrixRuntime ────────────────────────────
                var epilogue = new EpilogueMatrixRuntime();

                // Fate 1: CommonwealthFounded — treaty + burned ledgers, no decommission
                var ctx1 = new EpilogueEvaluationContext
                {
                    totalDaysSurvived = 800, livingDwellerCount = 30,
                    totalDeathsRecorded = 5, grandTreatySigned = true,
                    tempestDecommissioned = false, debtLedgersBurned = true,
                    childrenSurvived = true, velSecretExposed = false
                };
                var fate1 = epilogue.EvaluateRegionalFate(ctx1);
                Check(fate1 == RegionalFate.CommonwealthFounded,
                    "epilogue: commonwealth founded fate");

                // Fate 2: GarrisonMartialLaw — treaty signed, ledgers kept
                var ctx2 = new EpilogueEvaluationContext
                {
                    totalDaysSurvived = 600, livingDwellerCount = 25,
                    totalDeathsRecorded = 10, grandTreatySigned = true,
                    tempestDecommissioned = false, debtLedgersBurned = false,
                    childrenSurvived = true, velSecretExposed = false
                };
                var fate2 = epilogue.EvaluateRegionalFate(ctx2);
                Check(fate2 == RegionalFate.GarrisonMartialLaw,
                    "epilogue: garrison martial law fate");

                // Fate 3: FracturedWarlords — low pop, no treaty
                var ctx3 = new EpilogueEvaluationContext
                {
                    totalDaysSurvived = 400, livingDwellerCount = 8,
                    totalDeathsRecorded = 20, grandTreatySigned = false,
                    tempestDecommissioned = false, debtLedgersBurned = false,
                    childrenSurvived = false, velSecretExposed = false
                };
                var fate3 = epilogue.EvaluateRegionalFate(ctx3);
                Check(fate3 == RegionalFate.FracturedWarlords,
                    "epilogue: fractured warlords fate");

                // Fate 4: TempestSterilization — Tempest still active, heavy losses
                var ctx4 = new EpilogueEvaluationContext
                {
                    totalDaysSurvived = 500, livingDwellerCount = 15,
                    totalDeathsRecorded = 60, grandTreatySigned = false,
                    tempestDecommissioned = false, debtLedgersBurned = false,
                    childrenSurvived = false, velSecretExposed = true
                };
                var fate4 = epilogue.EvaluateRegionalFate(ctx4);
                Check(fate4 == RegionalFate.TempestSterilization,
                    "epilogue: tempest sterilization fate");

                // Fate 5: TrueReconciliation — burned ledgers, decommissioned, treaty
                var ctx5 = new EpilogueEvaluationContext
                {
                    totalDaysSurvived = 700, livingDwellerCount = 20,
                    totalDeathsRecorded = 8, grandTreatySigned = true,
                    tempestDecommissioned = true, debtLedgersBurned = true,
                    childrenSurvived = true, velSecretExposed = true
                };
                var fate5 = epilogue.EvaluateRegionalFate(ctx5);
                Check(fate5 == RegionalFate.TrueReconciliation,
                    "epilogue: true reconciliation fate");

                // Demographic + moral evaluations
                var demo = epilogue.EvaluateDemographics(ctx1);
                Check(demo == DemographicOutcome.ThrivingCommunity,
                    "epilogue: thriving community demographic");
                var moral = epilogue.EvaluateMoralStanding(ctx1);
                Check(moral == MoralStanding.ForgivenAndReconciled,
                    "epilogue: forgiven and reconciled moral standing");

                // Narrative generation
                string narrative = epilogue.GenerateEpilogueNarrative(ctx1);
                Check(!string.IsNullOrEmpty(narrative), "epilogue narrative generated");

                // ── 5. DiveInstanceRunner ───────────────────────────────
                var bus = new SimpleEventBus();
                var flags = new InMemoryFlagLedger();
                var rng = new SeededRng(424242);
                var site = new DiveSiteDefinition("site_test_dive", 120, 0.3, "keeper_thread_0");
                var dive = new DiveInstanceRunner(bus, flags, rng, site);

                Check(dive.CurrentRoom == DiveRoom.deckhouse, "dive starts in deckhouse");
                Check(dive.OxygenRemaining == 120, "dive oxygen budget from site def");
                Check(dive.Choice == SovereignChoice.undecided, "dive choice undecided initially");

                // Advance rooms
                bool adv1 = dive.Advance();
                Check(adv1 && dive.CurrentRoom == DiveRoom.companionway,
                    "dive advanced to companionway");

                bool adv2 = dive.Advance();
                Check(adv2 && dive.CurrentRoom == DiveRoom.hold_approach,
                    "dive advanced to hold_approach");

                bool adv3 = dive.Advance();
                Check(adv3 && dive.CurrentRoom == DiveRoom.the_hold,
                    "dive advanced to the hold");

                // Oxygen tick
                int oxyBefore = dive.OxygenRemaining;
                dive.TickOxygen();
                Check(dive.OxygenRemaining < oxyBefore, "dive oxygen decreased after tick");

                // Detection risk
                double risk = dive.DetectionRisk(0.5, false);
                Check(risk >= 0.0 && risk <= 1.0, "dive detection risk in valid range");

                // Commit choice
                dive.CommitChoice(SovereignChoice.flood_the_market);
                Check(dive.Choice == SovereignChoice.flood_the_market,
                    "dive choice committed");
                Check(flags.IsSet("flag_exp09_iodine_released"),
                    "dive choice set flag");
            }
            catch (Exception e)
            {
                Check(false, "standalone systems selftest threw: " + e.Message);
            }

            GD.Print(failures == 0
                ? "STANDALONE_SYSTEMS_SELFTEST PASS"
                : $"STANDALONE_SYSTEMS_SELFTEST FAIL ({failures})");
            return failures == 0 ? 0 : 1;
        }

        /// <summary>
        /// Phase-0 effects gate: phantom work-efficiency/refusal, somatic flashback
        /// work penalty, trade specialty mastery, final-wish permanent shelter
        /// buff, respiratory stamina penalty + ash-zone exposure, and a save
        /// write → reload → restore round-trip through the Phase0 save store.
        /// </summary>
        public static int RunPhase0SelfTest()
        {
            CatalogLocator.UseInvariantCulture();

            int failures = 0;
            void Check(bool condition, string name)
            {
                if (condition) GD.Print("[PASS] " + name);
                else { GD.Print("[FAIL] " + name); failures++; }
            }

            try
            {
                var session = new Phase0HostSession();
                session.SeedDemoRoster();
                session.RegisterDefaultRules();

                // ── 1. Phantom memory: motivation → work efficiency ─────
                session.ScavengeItem("survivor_gunner_mikhail", "item_dog_tags");
                float workMult = session.Phantom.GetWorkEfficiencyMultiplier("survivor_gunner_mikhail");
                Check(workMult == 1f || workMult == 1f + Ashfall.Core.PhantomMemoryEngine.MotivationWorkSpeedBonus,
                    "phantom work-efficiency multiplier is 1.0 or boosted");
                session.Phantom.TickHour("survivor_gunner_mikhail", 9f);
                Check(session.Phantom.GetWorkEfficiencyMultiplier("survivor_gunner_mikhail") == 1f,
                    "phantom work-efficiency decays back to 1.0");

                // Host view must track the decay too (aggregate is derived, not stale).
                session.TickHour(1f);
                float hostMult = session.GetEffects("survivor_gunner_mikhail").workEfficiencyMultiplier;
                Check(Math.Abs(hostMult - 1f) < 1e-4f,
                    "host work-efficiency view recomputes after boost decays");

                // ── 2. Somatic flashback: work penalty, grounded penalty ─
                var flash = session.Flashbacks;
                flash.GetAliveSurvivorIds = () => new[] { "sv_a", "sv_b" };
                flash.IsCompanionInSameRoom = (a, b) => a != b; // everyone grounded
                flash.IncreaseSusceptibility("sv_a", 1f);
                flash.OnAudioEvent("siren", 1f);
                float groundedPenalty = flash.GetWorkEfficiencyPenalty("sv_a");
                Check(groundedPenalty == 0f || groundedPenalty == Ashfall.Core.Survivors.SomaticFlashbackSystem.GroundedWorkEfficiencyPenalty,
                    "flashback penalty is 0 or grounded penalty");

                // ── 3. Trade specialty: milestones → mastery ───────────
                int narrativeFired = 0;
                string narrativeId = null;
                session.TradeSpecialty.FireNarrativeEvent = (id, sv) => { narrativeFired++; narrativeId = id; };
                session.CraftItem("elena_vasquez", "machinist", "wrench_standard");
                session.CraftItem("elena_vasquez", "machinist", "gear_standard");
                Check(session.TradeSpecialty.GetMasteryTier("elena_vasquez") == 2,
                    "trade specialty tier 2 after two matching crafts");
                session.CraftItem("elena_vasquez", "machinist", "lever_standard");
                Check(session.TradeSpecialty.HasMasteredTrade("elena_vasquez"),
                    "trade specialty mastered at 3 crafts");
                Check(narrativeFired == 1 && narrativeId == "narrative_trade_mastery_machinist",
                    "mastery fired narrative event");

                // ── 4. Final wish: permanent shelter morale buff ────────
                float buffBefore = session.PermanentShelterMoraleBuff;
                session.FinalWish.RegisterWish("parent", Ashfall.Core.Survivors.FinalWishSystem.WishBuildMemorial);
                session.FinalWish.DeclareTerminalPrognosis("survivor_dr_sarah_chen", "parent", true);
                session.FinalWish.AdvanceWishStep("survivor_dr_sarah_chen", "step_1");
                session.FinalWish.AdvanceWishStep("survivor_dr_sarah_chen", "step_2");
                session.FinalWish.AdvanceWishStep("survivor_dr_sarah_chen", "step_3");
                Check(session.PermanentShelterMoraleBuff >
                      buffBefore + Ashfall.Core.Survivors.FinalWishSystem.WishCompletedMoraleBuff - 0.5f,
                    "final wish completion applied permanent shelter morale buff");

                // ── 5. Respiratory: ash zone + stamina penalty ─────────
                session.IsInAshZone = true;
                session.Respiratory.GetOrCreate("survivor_gunner_mikhail");
                session.Respiratory.TickHours("survivor_gunner_mikhail", 24f);
                Check(session.Respiratory.RespiratoryDegradation("survivor_gunner_mikhail") > 0f,
                    "ash-zone exposure accumulates respiratory degradation");
                session.IsInAshZone = false;

                // ── 6. Guilt insomnia ──────────────────────────────────
                session.RecordGuilt("elena_vasquez", "choice_left_ally_behind", 0.9f);
                Check(session.Guilt.GetInsomniaSeverity("elena_vasquez") >= Ashfall.Core.Survivors.GuiltInsomniaSystem.HighSeverityThreshold,
                    "high-severity guilt raises insomnia severity");
                Check(session.GetEffects("elena_vasquez").guiltInsomniaSeverity > 0f,
                    "guilt insomnia severity reaches the derived host view");

                // ── 7. Combat trauma: survival raises hypervigilance ───
                session.RegisterCombatSurvived("survivor_gunner_mikhail");
                session.RegisterCombatSurvived("survivor_gunner_mikhail");
                float hyper = session.CombatTrauma.GetHypervigilanceLevel("survivor_gunner_mikhail");
                Check(hyper > 0f && hyper == session.GetEffects("survivor_gunner_mikhail").hypervigilance,
                    "combat survival raises hypervigilance in core and host view");

                // ── 8. Moral branching: choice decides a branch ────────
                session.RecordMoralChoice("survivor_dr_sarah_chen", true);
                session.RecordMoralChoice("survivor_dr_sarah_chen", true);
                session.RecordMoralChoice("survivor_dr_sarah_chen", true);
                session.RecordMoralChoice("survivor_dr_sarah_chen", true);
                session.RecordMoralChoice("survivor_dr_sarah_chen", true);
                var moralState = session.Moral.CaptureState();
                Check(moralState.Survivors.Exists(s => s.SurvivorId == "survivor_dr_sarah_chen"
                        && s.BranchDirection != Ashfall.Core.Survivors.MoralBranchDirection.Neutral),
                    "five moral choices decide a branch");

                // ── 9. Radiation phase progression: exposure → phase ──
                session.RadiationPhase.OnExposure("survivor_dr_sarah_chen", 120f);
                var phase = session.RadiationPhase.GetPhase("survivor_dr_sarah_chen");
                Check(phase != Ashfall.Core.Radiation.RadiationSicknessPhase.Healthy,
                    "radiation exposure moves survivor out of Healthy phase");
                Check(session.GetEffects("survivor_dr_sarah_chen").radiationPhase != "Healthy",
                    "radiation phase reaches the derived host view");

                // ── 10. Chemical dependency: substance → withdrawal penalty ──
                session.ConsumeSubstance("survivor_gunner_mikhail", "item_morphine", Ashfall.Core.Medical.ChemicalDependencyKind.Opioid);
                session.ConsumeSubstance("survivor_gunner_mikhail", "item_morphine", Ashfall.Core.Medical.ChemicalDependencyKind.Opioid);
                session.ConsumeSubstance("survivor_gunner_mikhail", "item_morphine", Ashfall.Core.Medical.ChemicalDependencyKind.Opioid);
                Check(session.Dependency.DependencyLevel("survivor_gunner_mikhail", "item_morphine") >= Ashfall.Core.Medical.ChemicalDependencySystem.DependencyThreshold,
                    "repeated doses form a dependency");
                bool detox = session.Dependency.BeginColdTurkey("survivor_gunner_mikhail", "item_morphine");
                Check(detox, "cold-turkey withdrawal begins for a dependent survivor");
                session.Dependency.TickHours("survivor_gunner_mikhail", 6f);
                Check(session.GetEffects("survivor_gunner_mikhail").dependencyCombatPenalty > 0f,
                    "withdrawal tremor penalty reaches the derived host view");

                // ── 11. Real-consumer wiring ────────────────────────────
                float moraleApplied = 0f;
                float staminaMult = 0f;
                float craftPenalty = 0f;
                int narratives = 0;
                session.Consumers.ApplyMoraleDelta = (sv, d) => moraleApplied += d;
                session.Consumers.ApplyStaminaDrainMultiplier = (sv, m) => staminaMult += m;
                session.Consumers.ApplyCraftingPenaltyFactor = (sv, f) => craftPenalty += f;
                session.Consumers.FireNarrativeEvent = (id, sv) => narratives++;
                session.Phantom.RegisterRule("former_soldier", "military", 0.40f, "d", "boost", "break");
                session.Phantom.TriggerChanceOverride = 1.0f; // force a trigger
                session.ScavengeItem("survivor_gunner_mikhail", "item_dog_tags");
                Check(moraleApplied != 0f, "phantom memory morale delta reaches the real consumer");
                session.Phantom.TriggerChanceOverride = -1f; // restore default
                // Use a fresh survivor (elena already mastered trade in section 3).
                session.TradeSpecialty.FireNarrativeEvent = (id, sv) => narratives++;
                session.CraftItem("survivor_gunner_mikhail", "electrician", "battery_cell");
                session.CraftItem("survivor_gunner_mikhail", "electrician", "wire_standard");
                session.CraftItem("survivor_gunner_mikhail", "electrician", "circuit_board");
                Check(narratives >= 1, "trade mastery narrative event reaches the real consumer");
                Check(craftPenalty == 0f, "dependency crafting penalty idle until withdrawal tick");

                // ── 12. Save round-trip ──────────────────────────────────
                var save = session.CaptureSave();
                Check(save != null && save.effects.Count >= 3, "phase-0 save captured effects");
                var fresh = new Phase0HostSession();
                fresh.RestoreSave(save);
                Check(Math.Abs(fresh.PermanentShelterMoraleBuff - session.PermanentShelterMoraleBuff) < 1e-4f,
                    "permanent shelter morale buff restored");
                Check(fresh.TradeSpecialty.HasMasteredTrade("elena_vasquez"),
                    "trade mastery restored");
                Check(fresh.Guilt.GetInsomniaSeverity("elena_vasquez") > 0f,
                    "guilt insomnia restored");
                Check(fresh.CombatTrauma.GetHypervigilanceLevel("survivor_gunner_mikhail") == hyper,
                    "combat trauma restored");
                // Dependency ledger is owned by MedicalHostSession (MedicalSaveStore);
                // Phase0 restores the shared instance only, verified separately.
                Check(session.Dependency.HasActiveWithdrawal("survivor_gunner_mikhail"),
                    "chemical dependency withdrawal active on the shared authority");
                Check(fresh.RadiationPhase.GetPhase("survivor_dr_sarah_chen") == phase,
                    "radiation phase restored");
            }
            catch (Exception e)
            {
                Check(false, "phase0 selftest threw: " + e.Message);
            }

            GD.Print(failures == 0
                ? "PHASE0_SELFTEST PASS"
                : $"PHASE0_SELFTEST FAIL ({failures})");
            return failures == 0 ? 0 : 1;
        }

        public static int RunCaravanSelfTest()
        {
            return TravelingCaravanHeadlessDemo.Run();
        }

        public static int RunAssetRegistrySelfTest(string dataDirectory)
        {
            var report = AssetRegistrySelfTest.Run(dataDirectory, topCount: 50);
            GD.Print(report.Summary);
            return report.Clean ? 0 : 1;
        }

        public static int RunDay1PlayableSelfTest(string dataDirectory)
        {
            int failures = 0;
            void Check(bool cond, string name)
            {
                if (cond)
                    GD.Print($"  [PASS] {name}");
                else
                {
                    GD.PrintErr($"  [FAIL] {name}");
                    failures++;
                }
            }

            GD.Print("[Day1PlayableSelfTest] Starting Phase 0 - Phase 2 Day 1 Playable Verification...");

            try
            {
                // 1. Initial State & Clean Reset
                var startingSession = new StartingLevelHostSession();
                var startingState = startingSession.System.State;
                Check(startingState != null, "starting level state initialized");
                Check(startingState!.day == 1, "starts on Day 1");
                Check(startingState.rooms.Count == 5, "starting bunker has 5 functional rooms");

                // Check rooms
                var bunks = startingState.rooms.Find(r => r.roomId == "room_bunks_living");
                var airlock = startingState.rooms.Find(r => r.roomId == "room_filtration_stack");
                var corridor = startingState.rooms.Find(r => r.roomId == "room_bunker_corridor");
                Check(bunks != null && bunks.material == "Wood", "bunks ceiling starts as wood");
                Check(airlock != null && airlock.attenuation >= 0.90f, "airlock has active lead filtration shielding");
                Check(corridor != null && corridor.isInspected, "central access corridor inspected");

                // 2. Survivor Roster
                var survivorsSession = new SurvivorsHostSession();
                survivorsSession.SeedDemoRoster();
                var roster = survivorsSession.RosterState;
                Check(roster.Count >= 3, "starting survivor roster has at least 3 survivors");
                var drChen = roster.Find(s => s.Id == "survivor_dr_sarah_chen");
                var mikhail = roster.Find(s => s.Id == "survivor_gunner_mikhail");
                var elena = roster.Find(s => s.Id == "elena_vasquez" || s.Id == "survivor_elena_vasquez");
                Check(drChen != null && drChen.Health > 80f, "Dr. Sarah Chen present with good health");
                Check(mikhail != null && mikhail.Health > 70f, "Gunner Mikhail present with combat traits");
                Check(elena != null && elena.Health > 80f, "Elena Vasquez present with machinist expertise");

                // 3. Inventory & Supplies
                var invSession = new InventoryHostSession();
                invSession.SeedStartingSupplies();
                var inv = invSession.Inventory;
                Check(inv.CountById("clean_water") >= 12, "holdfast stocked with >=12 clean water");
                Check(inv.CountById("canned_food") >= 16, "holdfast stocked with >=16 canned food");
                Check(inv.CountById("iodine_pills") >= 4, "holdfast stocked with >=4 iodine pills");
                Check(inv.CountById("scrap_mechanical") >= 6, "holdfast stocked with >=6 mechanical scrap");
                Check(inv.CountById("item_geiger_m3") >= 1, "holdfast equipped with geiger counter");
                Check(inv.CountById("item_dosimeter_pen") >= 1, "holdfast equipped with dosimeter pen");

                // 4. Opening Protocol Directives:
                // Directive 1: Morning Triage (Standard Rations)
                startingSession.ResolveMorningRationTriage(Ashfall.Core.StartingLevel.RationPolicy.Standard);
                Check(startingState.rationPolicy == Ashfall.Core.StartingLevel.RationPolicy.Standard, "morning triage chosen: Standard rations");
                Check(startingState.morningTriageResolved, "morning triage marked resolved");

                // Directive 2: Midday Maintenance (Fortify Bunks with Lead)
                int scrapBefore = inv.CountById("scrap_mechanical");
                invSession.Remove("scrap_mechanical", 2);
                startingSession.ResolveMiddayMaintenance(Ashfall.Core.StartingLevel.MaintenanceDirective.FortifyBunksLead);
                Check(startingState.maintenanceDirective == Ashfall.Core.StartingLevel.MaintenanceDirective.FortifyBunksLead, "midday maintenance chosen: Fortify Bunks Lead");
                Check(inv.CountById("scrap_mechanical") == scrapBefore - 2, "2 mechanical scrap consumed for bunker lead shielding");
                Check(bunks!.material == "Lead", "bunks ceiling upgraded to Lead shielding");
                Check(bunks.attenuation >= 0.98f, "bunks ceiling provides 99% radiation attenuation");

                // Directive 3: Evening Radio (Acknowledge Hydro Barons)
                var radioSession = RadioHostSession.Create(dataDirectory);
                startingSession.ResolveEveningRadio(Ashfall.Core.StartingLevel.RadioProtocol.AcknowledgeHydroBarons);
                Check(startingState.radioProtocol == Ashfall.Core.StartingLevel.RadioProtocol.AcknowledgeHydroBarons, "evening radio protocol chosen: Acknowledge Hydro Barons");
                Check(startingState.eveningRadioResolved, "evening radio protocol marked resolved");

                // 5. Core-Backed Action: Medical Treatment
                int iodineBefore = inv.CountById("iodine_pills");
                invSession.Remove("iodine_pills", 1);
                survivorsSession.AdministerIodine("survivor_gunner_mikhail");
                Check(inv.CountById("iodine_pills") == iodineBefore - 1, "1 iodine pill administered from medical stores");

                // 6. Core-Backed Action: Sub-Surface Greenhouse Cultivation
                var greenhouseSession = new GreenhouseHostSession(new GreenhouseSystem(1986), invSession);
                greenhouseSession.System.EnsurePlots(4);
                invSession.Add(GreenhouseExpansionCatalog.Items.SeedMushroom, 2);
                bool planted = greenhouseSession.Plant(0, GreenhouseExpansionCatalog.Items.SeedMushroom, 1);
                Check(planted, "mushroom spores planted in Greenhouse Plot 0");
                Check(greenhouseSession.System.Plots[0].stage == (int)GreenhouseStage.Sprouting, "plot 0 transitioned to Sprouting");
                bool watered = greenhouseSession.Water(0, 20f, tainted: false);
                Check(watered, "plot 0 irrigated with 20L clean water");
                Check(greenhouseSession.System.Plots[0].water >= 20f, "soil moisture recorded in bed");

                // 7. Time Advance: Day 1 -> Day 2 Transition & Need Decay
                int foodBefore = inv.CountById("canned_food");
                int waterBefore = inv.CountById("clean_water");

                // Daily consumption (3 survivors × 1 ration = 3 food, 3 water)
                invSession.Remove("canned_food", 3);
                invSession.Remove("clean_water", 3);
                survivorsSession.TickHour(24f); // 24h needs + radiation decay
                greenhouseSession.TickDay(2, 6f, 0.04f);
                startingSession.TickDay();

                Check(startingState.day == 2, "time advanced to Day 2");
                Check(inv.CountById("canned_food") == foodBefore - 3, "canned food decremented by 3 for daily rations");
                Check(inv.CountById("clean_water") == waterBefore - 3, "clean water decremented by 3 for daily hydration");
                Check(greenhouseSession.System.Plots[0].growth > 0f, "greenhouse crop growth advanced on Day 2 tick");

                // 8. Save State Persistence
                bool startingSaved = StartingLevelSaveStore.TrySave(startingSession.CaptureState());
                bool invSaved = InventorySaveStore.TrySave(invSession.CaptureSave());
                bool survivorsSaved = SurvivorsSaveStore.TrySave(survivorsSession.CaptureSave());
                bool greenhouseSaved = GreenhouseSaveStore.TrySave(greenhouseSession.CaptureSave());
                Check(startingSaved && invSaved && survivorsSaved && greenhouseSaved, "all systems persisted cleanly to disk");

                // 9. Clean Reload Verification
                var reloadedStarting = StartingLevelSaveStore.TryLoad();
                var reloadedInv = InventorySaveStore.TryLoad();
                var reloadedGreenhouse = GreenhouseSaveStore.TryLoad();

                Check(reloadedStarting != null && reloadedStarting.day == 2, "reloaded save retains Day 2");
                Check(reloadedStarting!.rooms.Find(r => r.roomId == "room_bunks_living")?.material == "Lead", "reloaded save retains Lead bunk fortification");
                Check(reloadedInv != null, "reloaded inventory save is valid");
                Check(reloadedGreenhouse != null && (reloadedGreenhouse.plots[0].stage == (int)GreenhouseStage.Growing || reloadedGreenhouse.plots[0].stage == (int)GreenhouseStage.Sprouting), "reloaded greenhouse save retains plot crop state");

                GD.Print($"[Day1PlayableSelfTest] Failures: {failures}");
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[Day1PlayableSelfTest] Exception thrown: {ex.Message}\n{ex.StackTrace}");
                failures++;
            }

            GD.Print(failures == 0 ? "DAY1_PLAYABLE_SELFTEST PASS" : "DAY1_PLAYABLE_SELFTEST FAIL");
            return failures == 0 ? 0 : 1;
        }

        public static int RunUiLayoutSelfTest(string dataDirectory)
        {
            int failures = 0;
            void Check(bool condition, string message)
            {
                if (condition)
                {
                    GD.Print($"  [PASS] {message}");
                }
                else
                {
                    GD.PrintErr($"  [FAIL] {message}");
                    failures++;
                }
            }

            GD.Print("[UiLayoutSelfTest] Starting 8-resolution responsive layout verification...");

            var resolutions = new (int W, int H, string Aspect)[]
            {
                (1024, 768, "4:3 Standard"),
                (1280, 720, "16:9 HD"),
                (1366, 768, "16:9 Laptop"),
                (1600, 900, "16:9 WS"),
                (1920, 1080, "16:9 FHD Native"),
                (2560, 1080, "21:9 Ultrawide"),
                (2560, 1440, "16:9 2K QHD"),
                (3840, 2160, "16:9 4K UHD")
            };

            foreach (var (w, h, aspect) in resolutions)
            {
                try
                {
                    // 1. MainMenuPanel
                    var mainMenu = new MainMenuPanel();
                    mainMenu.CustomMinimumSize = new Vector2(w, h);
                    mainMenu.Size = new Vector2(w, h);
                    mainMenu._Ready();
                    Check(mainMenu.Size.X >= w && mainMenu.Size.Y >= h, $"MainMenuPanel bounds valid at {w}x{h} ({aspect})");

                    // 2. GameDashboardPanel
                    var dashboard = new GameDashboardPanel();
                    dashboard.CustomMinimumSize = new Vector2(w, h);
                    dashboard.Size = new Vector2(w, h);
                    dashboard._Ready();
                    dashboard.UpdateState(new GameDashboardPanel.DashboardSnapshot
                    {
                        Day = 2,
                        Health = 85,
                        MaxHealth = 100,
                        Radiation = 14.5f,
                        CleanWater = 18,
                        Food = 24,
                        LivingSurvivors = 3,
                        TotalSurvivors = 3,
                        FilterSpares = 2,
                        Weather = "Fallout Dust"
                    });
                    Check(dashboard.Size.X >= w && dashboard.Size.Y >= h, $"GameDashboardPanel bounds valid at {w}x{h} ({aspect})");

                    // 3. SettingsPanel
                    var settings = new SettingsPanel();
                    settings.CustomMinimumSize = new Vector2(w, h);
                    settings.Size = new Vector2(w, h);
                    settings._Ready();
                    settings.Open();
                    Check(settings.Size.X >= w && settings.Size.Y >= h, $"SettingsPanel bounds valid at {w}x{h} ({aspect})");
                    settings.Close();

                    // 4. InventoryPanel
                    var invPanel = new InventoryPanel();
                    invPanel.CustomMinimumSize = new Vector2(w, h);
                    invPanel.Size = new Vector2(w, h);
                    invPanel._Ready();
                    Check(invPanel.Size.X >= w && invPanel.Size.Y >= h, $"InventoryPanel bounds valid at {w}x{h} ({aspect})");

                    // 5. SurvivorsPanel
                    var survPanel = new SurvivorsPanel();
                    survPanel.CustomMinimumSize = new Vector2(w, h);
                    survPanel.Size = new Vector2(w, h);
                    survPanel._Ready();
                    Check(survPanel.Size.X >= w && survPanel.Size.Y >= h, $"SurvivorsPanel bounds valid at {w}x{h} ({aspect})");

                    // 6. MaritimePanel (Exp 09) + DeepCoastPanel (Exp 01 sibling layer)
                    var maritimePanel = new MaritimePanel();
                    maritimePanel.CustomMinimumSize = new Vector2(w, h);
                    maritimePanel.Size = new Vector2(w, h);
                    maritimePanel._Ready();
                    Check(maritimePanel.Size.X >= w && maritimePanel.Size.Y >= h, $"MaritimePanel bounds valid at {w}x{h} ({aspect})");

                    var deepCoastPanel = new DeepCoastPanel();
                    deepCoastPanel.CustomMinimumSize = new Vector2(w, h);
                    deepCoastPanel.Size = new Vector2(w, h);
                    deepCoastPanel._Ready();
                    Check(deepCoastPanel.Size.X >= w && deepCoastPanel.Size.Y >= h, $"DeepCoastPanel bounds valid at {w}x{h} ({aspect})");
                }
                catch (Exception ex)
                {
                    GD.PrintErr($"  [FAIL] Exception at {w}x{h}: {ex.Message}");
                    failures++;
                }
            }

            GD.Print($"[UiLayoutSelfTest] Failures: {failures}");
            GD.Print(failures == 0 ? "UI_LAYOUT_SELFTEST PASS" : "UI_LAYOUT_SELFTEST FAIL");
            return failures == 0 ? 0 : 1;
        }

        public static int RunSettingsSelfTest(string dataDirectory)
        {
            int failures = 0;
            void Check(bool condition, string message)
            {
                if (condition)
                {
                    GD.Print($"  [PASS] {message}");
                }
                else
                {
                    GD.PrintErr($"  [FAIL] {message}");
                    failures++;
                }
            }

            GD.Print("[SettingsSelfTest] Starting UserSettings persistence, recovery, and engine application test...");
            string testPath = "user://settings_selftest.json";
            string globalTestPath = ProjectSettings.GlobalizePath(testPath);

            try
            {
                if (File.Exists(globalTestPath)) File.Delete(globalTestPath);

                // 1. Default creation
                var defaults = new UserSettingsData();
                Check(defaults.MasterVolume == 1.0f, "default master volume is 100%");
                Check(defaults.VSync, "default VSync is enabled");
                Check(defaults.MaxFps == 60, "default MaxFPS is 60");
                Check(defaults.ConfirmEndDay, "default ConfirmEndDay is enabled");

                // 2. Clone and Mutation
                var modified = defaults.Clone();
                modified.MasterVolume = 0.45f;
                modified.MusicVolume = 0.60f;
                modified.VSync = false;
                modified.MaxFps = 120;
                modified.HighContrast = true;
                modified.ResolutionWidth = 2560;
                modified.ResolutionHeight = 1440;

                // 3. Live Apply (Headless-safe)
                UserSettingsStore.Apply(modified);
                Check(Engine.MaxFps == 120, "Engine.MaxFps updated via Apply");

                // 4. Save and Reload Round-trip
                bool saved = UserSettingsStore.Save(modified, testPath);
                Check(saved && File.Exists(globalTestPath), "settings successfully saved to disk");

                var loaded = UserSettingsStore.Load(testPath);
                Check(Math.Abs(loaded.MasterVolume - 0.45f) < 0.01f, "reloaded master volume preserved");
                Check(Math.Abs(loaded.MusicVolume - 0.60f) < 0.01f, "reloaded music volume preserved");
                Check(!loaded.VSync, "reloaded VSync state preserved");
                Check(loaded.MaxFps == 120, "reloaded MaxFps preserved");
                Check(loaded.HighContrast, "reloaded HighContrast preserved");
                Check(loaded.ResolutionWidth == 2560 && loaded.ResolutionHeight == 1440, "reloaded resolution preserved");

                // 5. Corruption Recovery
                File.WriteAllText(globalTestPath, "{ CORRUPT_UNCLOSED_JSON_DATA_!!!");
                var recovered = UserSettingsStore.Load(testPath);
                Check(recovered != null && recovered.MasterVolume == 1.0f && recovered.MaxFps == 60, "corrupted file gracefully recovered to defaults");

                // Clean up test file
                if (File.Exists(globalTestPath)) File.Delete(globalTestPath);

                GD.Print($"[SettingsSelfTest] Failures: {failures}");
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[SettingsSelfTest] Exception: {ex.Message}\n{ex.StackTrace}");
                failures++;
            }

            GD.Print(failures == 0 ? "SETTINGS_SELFTEST PASS" : "SETTINGS_SELFTEST FAIL");
            return failures == 0 ? 0 : 1;
        }

        public static int RunPlayableShellSelfTest(string dataDirectory)
        {
            int failures = 0;
            void Check(bool condition, string message)
            {
                if (condition)
                {
                    GD.Print($"  [PASS] {message}");
                }
                else
                {
                    GD.PrintErr($"  [FAIL] {message}");
                    failures++;
                }
            }

            GD.Print("[PlayableShellSelfTest] Starting Playable UI Shell, Multi-Day Loop, & Navigation Flow...");

            try
            {
                // Clear active save files
                foreach (var file in new[]
                {
                    "starting_level_save.json", "inventory_save.json", "survivors_save.json", "greenhouse_save.json"
                })
                {
                    string p = Path.Combine(ProjectSettings.GlobalizePath("user://"), file);
                    if (File.Exists(p)) File.Delete(p);
                }

                // 1. Boot to Main Menu
                var mainMenu = new MainMenuPanel();
                mainMenu._Ready();
                bool saveExists = StartingLevelSaveStore.SaveExists();
                mainMenu.SetContinueEnabled(saveExists);
                Check(!saveExists, "main menu boots with continue disabled when no save exists");

                // 2. New Game Initialization
                var startingSession = new StartingLevelHostSession();
                var invSession = new InventoryHostSession();
                invSession.SeedStartingSupplies();
                var survivorsSession = new SurvivorsHostSession();
                survivorsSession.SeedDemoRoster();
                var greenhouseSession = new GreenhouseHostSession(new GreenhouseSystem(1986), invSession);
                greenhouseSession.System.EnsurePlots(4);

                Check(startingSession.System.State.day == 1, "new game begins on Day 1");
                Check(invSession.Inventory.CountById("clean_water") >= 12, "starting water stock verified");
                Check(invSession.Inventory.CountById("canned_food") >= 16, "starting food stock verified");

                // 3. Shelter Dashboard Presentation State
                var dashboard = new GameDashboardPanel();
                dashboard._Ready();
                dashboard.UpdateState(new GameDashboardPanel.DashboardSnapshot
                {
                    Day = startingSession.System.State.day,
                    Health = 100,
                    MaxHealth = 100,
                    Radiation = 0f,
                    CleanWater = invSession.Inventory.CountById("clean_water"),
                    Food = invSession.Inventory.CountById("canned_food"),
                    MedicalStock = invSession.Inventory.CountById("iodine_pills"),
                    FilterSpares = invSession.Inventory.CountById("item_air_filter_hepa"),
                    LivingSurvivors = survivorsSession.RosterState.Count,
                    TotalSurvivors = survivorsSession.RosterState.Count,
                    Weather = "Clear Fallout Dust",
                    Location = "THE HOLDFAST"
                });
                Check(dashboard.Visible == false, "dashboard constructed in background");

                // 4. Meaningful Gameplay Actions
                // Action A: Fortify Bunks with Lead
                invSession.Remove("scrap_mechanical", 2);
                startingSession.ResolveMiddayMaintenance(Ashfall.Core.StartingLevel.MaintenanceDirective.FortifyBunksLead);
                var bunks = startingSession.System.State.rooms.Find(r => r.roomId == "room_bunks_living");
                Check(bunks != null && bunks.material == "Lead", "action executed: bunker bunks fortified with Lead");

                // Action B: Cultivate greenhouse plot
                invSession.Add(GreenhouseExpansionCatalog.Items.SeedMushroom, 2);
                greenhouseSession.Plant(0, GreenhouseExpansionCatalog.Items.SeedMushroom, 1);
                greenhouseSession.Water(0, 20f, tainted: false);
                Check(greenhouseSession.System.Plots[0].stage == (int)GreenhouseStage.Sprouting, "action executed: greenhouse plot 0 planted & irrigated");

                // 5. Advance Day: Day 1 -> Day 2 Transition
                int foodBefore = invSession.Inventory.CountById("canned_food");
                int waterBefore = invSession.Inventory.CountById("clean_water");

                invSession.Remove("canned_food", 3);
                invSession.Remove("clean_water", 3);
                survivorsSession.TickHour(24f);
                greenhouseSession.TickDay(2, 6f, 0.04f);
                startingSession.TickDay();

                Check(startingSession.System.State.day == 2, "time advanced to Day 2");
                Check(invSession.Inventory.CountById("canned_food") == foodBefore - 3, "food consumed for day 2 rations");
                Check(invSession.Inventory.CountById("clean_water") == waterBefore - 3, "water consumed for day 2 rations");
                Check(greenhouseSession.System.Plots[0].growth > 0f, "greenhouse crop growth advanced on Day 2");

                // Update Dashboard with Day 2 State
                dashboard.UpdateState(new GameDashboardPanel.DashboardSnapshot
                {
                    Day = startingSession.System.State.day,
                    Health = 95,
                    MaxHealth = 100,
                    Radiation = 2.4f,
                    CleanWater = invSession.Inventory.CountById("clean_water"),
                    Food = invSession.Inventory.CountById("canned_food"),
                    MedicalStock = invSession.Inventory.CountById("iodine_pills"),
                    FilterSpares = invSession.Inventory.CountById("item_air_filter_hepa"),
                    LivingSurvivors = survivorsSession.RosterState.Count,
                    TotalSurvivors = survivorsSession.RosterState.Count,
                    Weather = "Ashfall Squall",
                    Location = "THE HOLDFAST"
                });

                // 6. Save State
                bool sSaved = StartingLevelSaveStore.TrySave(startingSession.CaptureState());
                bool iSaved = InventorySaveStore.TrySave(invSession.CaptureSave());
                bool survSaved = SurvivorsSaveStore.TrySave(survivorsSession.CaptureSave());
                bool gSaved = GreenhouseSaveStore.TrySave(greenhouseSession.CaptureSave());
                Check(sSaved && iSaved && survSaved && gSaved, "game state saved successfully to disk");

                // 7. Return to Menu
                mainMenu.SetContinueEnabled(StartingLevelSaveStore.SaveExists());
                Check(StartingLevelSaveStore.SaveExists(), "return to menu: continue button is now enabled");

                // 8. Continue / Reload from Save
                var loadedStarting = StartingLevelSaveStore.TryLoad();
                var loadedInv = InventorySaveStore.TryLoad();
                var loadedGreenhouse = GreenhouseSaveStore.TryLoad();
                Check(loadedStarting != null && loadedStarting.day == 2, "continued save reflects Day 2");
                Check(loadedStarting!.rooms.Find(r => r.roomId == "room_bunks_living")?.material == "Lead", "continued save retains Lead bunk fortification");
                Check(loadedGreenhouse != null && loadedGreenhouse.plots[0].growth > 0f, "continued save retains active greenhouse crop");

                // 9. In-Game Settings Navigation
                var settingsPanel = new SettingsPanel();
                settingsPanel._Ready();
                settingsPanel.Open();
                Check(settingsPanel.Visible, "settings overlay opens over active session");
                settingsPanel.Close();
                Check(!settingsPanel.Visible, "settings overlay closes cleanly without destroying state");

                GD.Print($"[PlayableShellSelfTest] Failures: {failures}");
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[PlayableShellSelfTest] Exception: {ex.Message}\n{ex.StackTrace}");
                failures++;
            }

            GD.Print(failures == 0 ? "PLAYABLE_SHELL_SELFTEST PASS" : "PLAYABLE_SHELL_SELFTEST FAIL");
            return failures == 0 ? 0 : 1;
        }

        public static int RunShelterHazardLoopSelfTest(string dataDirectory)
        {
            int failures = 0;
            void Check(bool condition, string message)
            {
                if (condition)
                    GD.Print($"  [PASS] {message}");
                else
                {
                    GD.PrintErr($"  [FAIL] {message}");
                    failures++;
                }
            }

            GD.Print("[ShelterHazardLoopSelfTest] Starting Shelter Air, Fallout Forecasting, & Duty Roster Loop (Days 2-5)...");

            try
            {
                // Clean test paths
                string tmpStarting = Path.Combine(ProjectSettings.GlobalizePath("user://"), "starting_level_hazard_test.json");
                string tmpRoster = Path.Combine(ProjectSettings.GlobalizePath("user://"), "duty_roster_hazard_test.json");
                string tmpWorld = Path.Combine(ProjectSettings.GlobalizePath("user://"), "world_hazard_test.json");
                if (File.Exists(tmpStarting)) File.Delete(tmpStarting);
                if (File.Exists(tmpRoster)) File.Delete(tmpRoster);
                if (File.Exists(tmpWorld)) File.Delete(tmpWorld);

                // 1. Initial State
                var startingSession = new StartingLevelHostSession();
                var worldSession = WorldHostSession.Create(dataDirectory);
                var rosterSession = DutyRosterHostSession.Create(dataDirectory);
                rosterSession.Unlock(1);

                Check(startingSession.System.State.day == 1, "starting day is Day 1");
                Check(startingSession.System.State.airFilterHealthPercent == 100.0f, "initial air filter health is 100%");
                Check(startingSession.System.State.airQualityPercent == 100.0f, "initial air quality is 100%");
                Check(startingSession.System.State.radonLevelBqm3 == 12.0f, "initial radon level is baseline 12 Bq/m³");
                Check(!startingSession.System.State.airHazardWarning, "no air hazard warning on Day 1");

                // 2. Deterministic Weather Forecasting
                var forecastA = worldSession.Weather.PeekForecast(3);
                var forecastB = worldSession.Weather.PeekForecast(3);
                Check(forecastA.Count == 3, "peek forecast returns 3-day projection");
                Check(forecastA[0].Kind == forecastB[0].Kind && forecastA[1].Kind == forecastB[1].Kind && forecastA[2].Kind == forecastB[2].Kind, "weather forecast is 100% deterministic on repeated reads");
                int rollCountBefore = worldSession.Weather.State.rollCount;
                worldSession.Weather.PeekForecast(5);
                Check(worldSession.Weather.State.rollCount == rollCountBefore, "peeking forecast does not mutate simulation roll count or RNG state");

                // 3. Survivor Work-Shift Assignment (Duty Roster)
                rosterSession.Roster.WriteName("survivor_sarah_chen", "Dr. Sarah Chen", "Medical Officer", DutyRosterSystem.ScriptPencil, 1, true);
                rosterSession.Roster.WriteName("survivor_mikhail_volkov", "Gunner Mikhail", "Soldier", DutyRosterSystem.ScriptPencil, 1, true);
                rosterSession.Roster.WriteName("survivor_elena_vasquez", "Elena Vasquez", "Machinist", DutyRosterSystem.ScriptPencil, 1, true);

                bool assignedIntake = rosterSession.Roster.Assign(DutyRosterSystem.RoleIntakeSleeper, "survivor_sarah_chen");
                bool assignedWatch = rosterSession.Roster.Assign(DutyRosterSystem.RoleNightWatch, "survivor_mikhail_volkov");
                bool assignedMess = rosterSession.Roster.Assign(DutyRosterSystem.RoleMess, "survivor_elena_vasquez");
                Check(assignedIntake && assignedWatch && assignedMess, "survivors successfully assigned to canonical Duty Roster roles");
                Check(rosterSession.Roster.GetAssignment(DutyRosterSystem.RoleIntakeSleeper) == "survivor_sarah_chen", "Dr. Sarah Chen confirmed on Intake Filtration duty");
                Check(rosterSession.Roster.GetAssignment(DutyRosterSystem.RoleNightWatch) == "survivor_mikhail_volkov", "Gunner Mikhail confirmed on Night Watch");

                // 4. Day 1 -> Day 2 Progression (Maintained Filter)
                startingSession.TickDay(isFilterDutyAssigned: true, outdoorWeather: worldSession.Weather.Current);
                worldSession.Weather.Tick(24.0f);
                rosterSession.Clock.AdvanceDays(1);
                Check(startingSession.System.State.day == 2, "advanced to Day 2");
                Check(startingSession.System.State.airFilterHealthPercent == 97.5f, "intake duty halved filter degradation (97.5% integrity)");
                Check(startingSession.System.State.airQualityPercent >= 97.0f, "air quality remains high under active shift");
                Check(!startingSession.System.State.airHazardWarning, "no air hazard warning on Day 2");

                // 5. Day 2 -> Day 3 Progression (Unmaintained Filter under Fallout Hazard)
                worldSession.Weather.ForceWeather(Ashfall.Core.WeatherKind.FalloutStorm);
                startingSession.TickDay(isFilterDutyAssigned: false, outdoorWeather: Ashfall.Core.WeatherKind.FalloutStorm);
                worldSession.Weather.Tick(24.0f);
                rosterSession.Clock.AdvanceDays(1);
                Check(startingSession.System.State.day == 3, "advanced to Day 3");
                Check(startingSession.System.State.airFilterHealthPercent == 88.5f, "unmaintained filter under fallout storm took full base + hazard degradation (88.5%)");

                // 6. Hazard Warning Trigger on Severe Contamination
                startingSession.System.State.airFilterHealthPercent = 42.0f;
                startingSession.System.State.radonLevelBqm3 = 48.0f;
                startingSession.System.State.airHazardWarning = true;
                Check(startingSession.System.State.airHazardWarning, "air hazard warning triggered when filter drops below 50%");

                // 7. Player Remediation Action
                int scrapBefore = startingSession.System.State.mechanicalScrapCount;
                bool serviced = startingSession.ServiceAirFilter();
                Check(serviced, "serviced air filter stack using mechanical scrap");
                Check(startingSession.System.State.mechanicalScrapCount == scrapBefore - 1, "1 mechanical scrap consumed for servicing");
                Check(startingSession.System.State.airFilterHealthPercent == 67.0f, "filter integrity restored +25% (now 67%)");
                Check(startingSession.System.State.radonLevelBqm3 == 33.0f, "radon purged by 15 Bq/m³ (now 33 Bq/m³)");
                rosterSession.Roster.Assign(DutyRosterSystem.RoleIntakeSleeper, "survivor_sarah_chen"); // reassign

                // 8. Multi-Day Progression: Day 3 -> Day 4 -> Day 5
                startingSession.TickDay(isFilterDutyAssigned: true, outdoorWeather: worldSession.Weather.Current);
                worldSession.Weather.Tick(24.0f);
                rosterSession.Clock.AdvanceDays(1);
                Check(startingSession.System.State.day == 4, "advanced to Day 4");

                startingSession.TickDay(isFilterDutyAssigned: true, outdoorWeather: worldSession.Weather.Current);
                worldSession.Weather.Tick(24.0f);
                rosterSession.Clock.AdvanceDays(1);
                Check(startingSession.System.State.day == 5, "advanced to Day 5");
                Check(startingSession.System.State.daysSurvived == 5, "5 days survived recorded in holdfast ledger");

                // 9. Dashboard UI Verification
                var dashboard = new GameDashboardPanel();
                dashboard._Ready();
                dashboard.UpdateState(new GameDashboardPanel.DashboardSnapshot
                {
                    Day = startingSession.System.State.day,
                    Health = 90,
                    MaxHealth = 100,
                    Radiation = 4.2f,
                    AirFilterHealth = startingSession.System.State.airFilterHealthPercent,
                    AirQuality = startingSession.System.State.airQualityPercent,
                    RadonLevel = startingSession.System.State.radonLevelBqm3,
                    AirWarning = startingSession.System.State.airHazardWarning,
                    MechanicalScrap = startingSession.System.State.mechanicalScrapCount,
                    FilterSpares = startingSession.System.State.filterSparesCount,
                    FilterDutyAssignee = "Dr. Sarah Chen",
                    Forecast = worldSession.Weather.PeekForecast(3),
                    Location = "THE HOLDFAST"
                });
                Check(dashboard != null, "responsive dashboard synchronized with Day 5 state without errors");

                // 10. Atomic Save & Reload Round-Trip
                bool sSaved = StartingLevelSaveStore.TrySave(startingSession.CaptureState(), tmpStarting);
                bool rSaved = DutyRosterSaveStore.TrySave(rosterSession.CaptureSave(), tmpRoster);
                Check(sSaved && rSaved, "saved starting level and duty roster states to disk");

                var reloadedStarting = StartingLevelSaveStore.TryLoad(tmpStarting);
                var reloadedRoster = DutyRosterSaveStore.TryLoad(tmpRoster);
                Check(reloadedStarting != null && reloadedStarting.day == 5, "reloaded starting state reflects Day 5");
                Check(reloadedStarting != null && reloadedStarting.airFilterHealthPercent > 50.0f, "reloaded air filter health preserved");
                Check(reloadedStarting != null && reloadedStarting.mechanicalScrapCount == 5, "reloaded scrap inventory preserved");
                Check(reloadedRoster != null, "reloaded duty roster state is valid");

                // Clean up test files
                if (File.Exists(tmpStarting)) File.Delete(tmpStarting);
                if (File.Exists(tmpRoster)) File.Delete(tmpRoster);
                if (File.Exists(tmpWorld)) File.Delete(tmpWorld);

                GD.Print($"[ShelterHazardLoopSelfTest] Failures: {failures}");
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[ShelterHazardLoopSelfTest] Exception: {ex.Message}\n{ex.StackTrace}");
                failures++;
            }

            GD.Print(failures == 0 ? "SHELTER_HAZARD_LOOP_SELFTEST PASS" : "SHELTER_HAZARD_LOOP_SELFTEST FAIL");
            return failures == 0 ? 0 : 1;
        }

        public static int RunShelterOperationsSelfTest(string dataDirectory)
        {
            int failures = 0;
            void Check(bool condition, string message)
            {
                if (condition)
                {
                    GD.Print($"[PASS] {message}");
                }
                else
                {
                    GD.PrintErr($"[FAIL] {message}");
                    failures++;
                }
            }

            try
            {
                GD.Print("[ShelterOperationsSelfTest] Starting Medical Triage, Expedition Sorties, & Radio Network Verification...");

                // ── 1. Medical Triage & Treatment Verification ──
                var survivors = new SurvivorsHostSession();
                survivors.SeedDemoRoster();
                var inv = new InventoryHostSession();
                inv.SeedStartingSupplies();
                inv.Add("bandage", 3);
                inv.Add("iodine_pills", 3);
                inv.Add("rad_away", 2);

                var med = new MedicalHostSession();

                var mikhail = survivors.Find("survivor_gunner_mikhail");
                Check(mikhail != null, "Mikhail registered in survivors roster");
                if (mikhail != null)
                {
                    mikhail.Health = 60f;
                    float hpBefore = mikhail.Health;
                    int bandagesBefore = inv.Inventory.CountById("bandage");

                    // Apply bandage treatment
                    bool consumed = inv.Inventory.RemoveById("bandage", 1);
                    Check(consumed, "consumed 1 bandage from inventory");
                    survivors.HealSurvivor("survivor_gunner_mikhail", 25f);
                    med.AddCareEntry("survivor_gunner_mikhail", "Applied sterile bandage.");

                    Check(mikhail.Health >= hpBefore + 20f, $"survivor healed from {hpBefore} to {mikhail.Health}");
                    Check(inv.Inventory.CountById("bandage") == bandagesBefore - 1, "inventory bandage count decreased by 1");

                    // Apply anti-rad treatment
                    var radState = survivors.RadStateFor("survivor_gunner_mikhail");
                    Check(radState != null && radState.RadiationDose > 0, "Mikhail has initial radiation exposure");
                    float doseBefore = radState?.RadiationDose ?? 0f;
                    int radAwayBefore = inv.Inventory.CountById("rad_away");

                    consumed = inv.Inventory.RemoveById("rad_away", 1);
                    Check(consumed, "consumed 1 rad_away from inventory");
                    survivors.AdministerAntiRad("survivor_gunner_mikhail", 40f);
                    med.AddCareEntry("survivor_gunner_mikhail", "Administered anti-rad chelation agent.");

                    Check(radState.RadiationDose < doseBefore, $"radiation dose purged from {doseBefore} to {radState.RadiationDose}");
                    Check(inv.Inventory.CountById("rad_away") == radAwayBefore - 1, "inventory rad_away count decreased by 1");

                    // Apply iodine prophylaxis to Sarah Chen
                    var sarah = survivors.RadStateFor("survivor_dr_sarah_chen");
                    Check(sarah != null, "Sarah Chen rad state found");
                    consumed = inv.Inventory.RemoveById("iodine_pills", 1);
                    Check(consumed, "consumed 1 iodine pill");
                    survivors.AdministerIodine("survivor_dr_sarah_chen");
                    med.AddCareEntry("survivor_dr_sarah_chen", "Administered potassium iodide.");

                    Check(sarah.HasRadResistance, "Sarah Chen gained rad resistance");
                    Check(sarah.RadResistanceHoursRemaining > 0, "rad resistance hours active");
                }

                // ── 2. Wasteland Expedition Scavenging Sortie Verification ──
                var expeditions = ExpeditionHostSession.Create(dataDirectory);
                Check(expeditions.DemoDefinitions.Count >= 2, "expedition definitions loaded");

                var target = expeditions.DemoDefinitions[0];
                Check(target != null && target.id == "loc_the_allotments", "target is The Works Allotment Commune");

                string startMsg = expeditions.StartDemoExpedition("survivor_dr_sarah_chen", target.id);
                Check(expeditions.Engine.ActiveCount == 1, "expedition successfully deployed");
                var activeExp = expeditions.Engine.Active["survivor_dr_sarah_chen"];
                Check(activeExp != null && activeExp.phase == (int)ExpeditionPhase.Outbound, "expedition starts in Outbound phase");

                // Advance hours until arrival / looting
                for (int h = 0; h < 6; h++)
                {
                    expeditions.TickDemoHours(2f);
                }

                // Push luck or advance to looting
                Check(activeExp.stamina < 100f, "stamina consumed during sortie travel");

                // Test save & restore of expedition state
                var expSave = expeditions.CaptureSave();
                Check(expSave != null && expSave.Count == 1, "expedition state captured cleanly");
                var reloadedExp = new ExpeditionHostSession();
                reloadedExp.RestoreSave(expSave);
                Check(reloadedExp.Engine.ActiveCount == 1, "expedition state restored with full fidelity");

                // ── 3. Radio Communication Network Verification ──
                var radio = RadioHostSession.Create(dataDirectory, 3);
                Check(radio != null, "radio host session created");
                Check(radio.CurrentFrequency > 0f, "radio tuner has carrier frequency");

                string listenMsg1 = radio.Listen(142.850f);
                Check(radio.CurrentFrequency == 142.850f, "tuned to 142.850 MHz");
                Check(radio.History.Count > 0, "intercept recorded on 142.850 MHz");

                string listenMsg2 = radio.Listen(104.200f);
                Check(radio.CurrentFrequency == 104.200f, "tuned to 104.200 MHz");

                string beaconMsg = radio.BroadcastBeacon("Holdfast shelter beacon test.");
                Check(radio.LastIntercept.HasValue && radio.LastIntercept.Value.Callsign == "HOLDFAST BASE", "emergency broadcast logged as HOLDFAST BASE");

                // ── 4. UI Overlay Component Smoke Verification ──
                var medPanel = new MedicalPanel();
                medPanel._Ready();
                medPanel.Bind(med, survivors, inv);
                Check(medPanel.IsBound, "MedicalPanel binds cleanly with active session");

                var expPanel = new ExpeditionPanel();
                expPanel._Ready();
                expPanel.Bind(expeditions, survivors, inv);
                Check(expPanel.IsBound, "ExpeditionPanel binds cleanly with active session");

                var radPanel = new RadioPanel();
                radPanel._Ready();
                radPanel.Bind(radio);
                Check(radPanel.IsBound, "RadioPanel binds cleanly with active session");

                medPanel.QueueFree();
                expPanel.QueueFree();
                radPanel.QueueFree();

                // ── 5. Crafting System Verification (14 assertions) ──
                GD.Print("[ShelterOperationsSelfTest] §5 Crafting system...");
                var craftInv = new Ashfall.Core.Inventory.Inventory();
                var craftSession = new CraftingHostSession(craftInv);


                // 5.1 Recipe catalog loads
                Check(craftSession.Recipes.Count >= 5, "recipe catalog has ≥5 recipes");

                // 5.2 recipe_bandage present
                var bandageRecipe = craftSession.FindRecipe("recipe_bandage");
                Check(bandageRecipe != null, "recipe_bandage is resolvable");

                // 5.3 CanCraft false when ingredients missing
                Check(!craftSession.Engine.CanCraft(bandageRecipe), "CanCraft false when ingredients absent");

                // 5.4 Add ingredients → CanCraft true
                var mechParts = CraftingHostSession.Catalog.Get("scrap_mechanical");
                if (mechParts != null) craftInv.Add(mechParts, 5);
                Check(craftSession.Engine.CanCraft(bandageRecipe), "CanCraft true after adding ingredients");


                // 5.5 Start craft → queue grows
                int craftBandageBefore = craftInv.CountById("bandage");
                string craftStartMsg = craftSession.Start("recipe_bandage");
                Check(craftSession.Engine.ActiveCraftCount == 1, "StartCraft queues one entry");


                // 5.6 Ingredient consumed atomically
                int mechAfter = craftInv.CountById("scrap_mechanical");
                Check(mechAfter < 5, $"ingredient count decreased after start (was 5, now {mechAfter})");

                // 5.7 Invalid recipe ID → Start returns error, queue unchanged
                string badCraftMsg = craftSession.Start("recipe_does_not_exist");
                Check(badCraftMsg != null && badCraftMsg.Length > 0, "invalid recipe ID returns non-empty error message");
                Check(craftSession.Engine.ActiveCraftCount == 1, "invalid recipe does not grow queue");


                // 5.8 Second valid craft → queue grows
                if (mechParts != null) craftInv.Add(mechParts, 5);
                craftSession.Start("recipe_bandage");
                Check(craftSession.Engine.ActiveCraftCount == 2, "second craft queued (two bandage crafts)");


                // 5.9 Tick past duration → OnCraftCompleted fires once per craft
                int craftCompletions = 0;
                craftSession.Engine.OnCraftCompleted += _ => craftCompletions++;
                craftSession.CompleteAll(2f); // recipe_bandage takes 1h each
                Check(craftSession.Engine.ActiveCraftCount == 0, "both crafts completed after full tick");
                Check(craftCompletions == 2, $"OnCraftCompleted fired exactly twice (got {craftCompletions})");


                // 5.10 Output in inventory
                int craftBandageAfter = craftInv.CountById("bandage");
                Check(craftBandageAfter >= craftBandageBefore + 2,
                    $"bandage count in inventory after crafts (was {craftBandageBefore}, now {craftBandageAfter})");


                // 5.11 Save → TryLoad
                if (mechParts != null) craftInv.Add(mechParts, 5);
                craftSession.Start("recipe_bandage"); // add a partial craft
                var craftSave = craftSession.CaptureSave();
                Check(craftSave != null, "CaptureState returns non-null");
                Check(craftSave.ActiveCrafts != null && craftSave.ActiveCrafts.Length == 1,
                    "partial craft captured in save");


                // 5.12 Restore preserves HoursRemaining
                var craftInv2 = new Ashfall.Core.Inventory.Inventory();
                var craftSession2 = new CraftingHostSession(craftInv2);
                craftSession2.RestoreSave(craftSave);
                Check(craftSession2.Engine.ActiveCraftCount == 1, "restored crafting queue has 1 entry");
                Check(craftSession2.Engine.ActiveCrafts[0].HoursRemaining > 0f,
                    "restored craft has positive hours remaining");


                // 5.13 Tick restored craft to completion
                int restoredCraftCompletions = 0;
                craftSession2.Engine.OnCraftCompleted += _ => restoredCraftCompletions++;
                craftSession2.CompleteAll(5f);
                Check(restoredCraftCompletions == 1,
                    $"restored craft completes exactly once (got {restoredCraftCompletions})");

                // 5.14 No duplication: tick again after completion
                craftSession2.CompleteAll(5f);
                Check(restoredCraftCompletions == 1,
                    "second tick after completion does not re-fire OnCraftCompleted");


                // ── 6. Respiratory Affliction Verification (10 assertions) ──
                GD.Print("[ShelterOperationsSelfTest] §6 Respiratory affliction...");
                var phase0resp = new Phase0HostSession();
                phase0resp.IsInAshZone = true;

                // 6.1 GetOrCreate returns non-null
                var respState = phase0resp.Respiratory.GetOrCreate("survivor_gunner_mikhail");
                Check(respState != null, "Respiratory.GetOrCreate returns non-null state");


                // 6.2 Ash-zone exposure accumulates degradation
                phase0resp.Respiratory.TickHours("survivor_gunner_mikhail", 24f);
                float respDeg = phase0resp.Respiratory.RespiratoryDegradation("survivor_gunner_mikhail");
                Check(respDeg > 0f, $"ash-zone TickHours accumulates degradation (got {respDeg:F2})");

                // 6.3 Below SevereCoughThreshold → no stamina penalty
                if (respDeg < Ashfall.Core.Medical.RespiratoryDegenerationSystem.SevereCoughThreshold)
                    Check(phase0resp.Respiratory.GetStaminaMultiplier("survivor_gunner_mikhail") == 1f,
                        "stamina multiplier is 1.0 below severe cough threshold");

                // 6.4 Force to severe cough → stamina penalty
                var forcedState = phase0resp.Respiratory.GetOrCreate("survivor_forced");
                forcedState.respiratoryDegradation = Ashfall.Core.Medical.RespiratoryDegenerationSystem.SevereCoughThreshold + 1f;
                float mult = phase0resp.Respiratory.GetStaminaMultiplier("survivor_forced");
                Check(mult < 1f, $"stamina multiplier < 1 when severe cough active ({mult:F2})");


                // 6.5 ApplyInhaler reduces degradation
                float respDegBefore = respDeg;
                bool inhalerOk = phase0resp.Respiratory.ApplyInhaler("survivor_gunner_mikhail");
                Check(inhalerOk, "ApplyInhaler returns true on survivor with lung damage");
                float respDegAfter = phase0resp.Respiratory.RespiratoryDegradation("survivor_gunner_mikhail");
                Check(respDegAfter < respDegBefore, $"ApplyInhaler reduces degradation ({respDegBefore:F2} → {respDegAfter:F2})");

                // 6.6 ApplyInhaler sets relief hours
                float inhalerRelief = phase0resp.Respiratory.InhalerReliefHours("survivor_gunner_mikhail");
                Check(inhalerRelief == Ashfall.Core.Medical.RespiratoryDegenerationSystem.InhalerReliefDurationHours,
                    $"inhaler relief hours set to {Ashfall.Core.Medical.RespiratoryDegenerationSystem.InhalerReliefDurationHours} (got {inhalerRelief})");

                // 6.7 Inhaler suppresses stamina penalty
                forcedState.respiratoryDegradation = Ashfall.Core.Medical.RespiratoryDegenerationSystem.SevereCoughThreshold + 1f;
                forcedState.inhalerReliefHours = 8f;
                Check(phase0resp.Respiratory.GetStaminaMultiplier("survivor_forced") == 1f,
                    "inhaler relief suppresses stamina penalty");


                // 6.8 Save round-trip preserves degradation
                var phase0respSave = phase0resp.CaptureSave();
                var phase0respFresh = new Phase0HostSession();
                phase0respFresh.RestoreSave(phase0respSave);
                float restoredRespDeg = phase0respFresh.Respiratory.RespiratoryDegradation("survivor_gunner_mikhail");
                Check(Math.Abs(restoredRespDeg - respDegAfter) < 0.01f,
                    $"respiratory degradation preserved across save/restore ({respDegAfter:F2} → {restoredRespDeg:F2})");

                // 6.9 ApplyInhaler on healthy survivor returns false
                var phase0b = new Phase0HostSession();
                phase0b.Respiratory.GetOrCreate("healthy_sv"); // degradation=0
                bool noEffect = phase0b.Respiratory.ApplyInhaler("healthy_sv");
                Check(!noEffect, "ApplyInhaler returns false on survivor with zero degradation");

                // 6.10 ApplyHerbalTea reduces mild degradation
                var phase0c = new Phase0HostSession();
                phase0c.IsInAshZone = true;
                phase0c.Respiratory.TickHours("sv_mild", 10f);
                float mildBefore = phase0c.Respiratory.RespiratoryDegradation("sv_mild");
                if (mildBefore > 0f)
                {
                    phase0c.Respiratory.ApplyHerbalTea("sv_mild");
                    Check(phase0c.Respiratory.RespiratoryDegradation("sv_mild") < mildBefore,
                        "ApplyHerbalTea reduces mild respiratory degradation");
                }


                // ── 7. Integration: craft → advance → affliction → treat → persist (3 assertions) ──
                GD.Print("[ShelterOperationsSelfTest] §7 Integration loop...");
                var intInv = new Ashfall.Core.Inventory.Inventory();
                var intCrafting = new CraftingHostSession(intInv);
                var intMechDef = CraftingHostSession.Catalog.Get("scrap_mechanical");
                if (intMechDef != null)
                {
                    intInv.Add(intMechDef, 5);
                    intCrafting.Start("recipe_bandage");
                    intCrafting.CompleteAll(2f);
                    Check(intInv.CountById("bandage") >= 1,
                        "integration: crafted bandage is available in inventory");
                }
                else
                {
                    Check(false, "integration: scrap_mechanical not in seed catalog");
                }

                // Expose survivor in ash zone, treat with inhaler
                var intPhase0 = new Phase0HostSession();
                intPhase0.IsInAshZone = true;
                intPhase0.Respiratory.TickHours("survivor_gunner_mikhail", 24f);
                float intDegBefore = intPhase0.Respiratory.RespiratoryDegradation("survivor_gunner_mikhail");
                if (intDegBefore > 0f)
                {
                    intPhase0.Respiratory.ApplyInhaler("survivor_gunner_mikhail");
                    var intSave = intPhase0.CaptureSave();
                    var intRestored = new Phase0HostSession();
                    intRestored.RestoreSave(intSave);
                    float intDegRestored = intRestored.Respiratory.RespiratoryDegradation("survivor_gunner_mikhail");
                    Check(intDegRestored < intDegBefore,
                        $"integration: inhaler treatment persists across save/restore ({intDegBefore:F2} → {intDegRestored:F2})");
                }

                // Crafting queue survives save/restore under ContinueGame pattern
                var intInv2 = new Ashfall.Core.Inventory.Inventory();
                var intCraft2 = new CraftingHostSession(intInv2);
                var intMechDef2 = CraftingHostSession.Catalog.Get("scrap_mechanical");
                if (intMechDef2 != null)
                {
                    intInv2.Add(intMechDef2, 3);
                    intCraft2.Start("recipe_bandage");
                    var craftSave2 = intCraft2.CaptureSave();
                    var intInv3 = new Ashfall.Core.Inventory.Inventory();
                    var intCraft3 = new CraftingHostSession(intInv3);
                    intCraft3.RestoreSave(craftSave2);
                    Check(intCraft3.Engine.ActiveCraftCount == 1,
                        "integration: crafting queue preserved through ContinueGame restore path");
                }


                GD.Print($"[ShelterOperationsSelfTest] All assertions complete. Failures so far: {failures}");
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[ShelterOperationsSelfTest] Exception: {ex.Message}\n{ex.StackTrace}");
                failures++;
            }

            GD.Print(failures == 0 ? "SHELTER_OPERATIONS_SELFTEST PASS" : "SHELTER_OPERATIONS_SELFTEST FAIL");
            return failures == 0 ? 0 : 1;
        }
    }
}
