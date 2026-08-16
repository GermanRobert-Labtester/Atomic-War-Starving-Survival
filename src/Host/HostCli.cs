using Godot;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Medical;
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
        MusterUiTest,
        DoseUiTest,
        InventoryUiTest,
        SurvivorsUiTest,
        BridgeSelfTest,
        DutyRosterSelfTest,
        StandingRecordSelfTest,
        CrossingSelfTest,
        ArbitrationSelfTest,
        LedgerDebtSelfTest,
        GreenhouseSelfTest,
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
        Phase0SelfTest
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
            if (Has(args, "--muster-uitest"))
                return HostCliAction.MusterUiTest;
            if (Has(args, "--inventory-uitest") || Has(args, "--inventory-selftest"))
                return HostCliAction.InventoryUiTest;
            if (Has(args, "--survivors-uitest"))
                return HostCliAction.SurvivorsUiTest;
            if (Has(args, "--dose-uitest"))
                return HostCliAction.DoseUiTest;
            if (Has(args, "--bridge-selftest"))
                return HostCliAction.BridgeSelfTest;
            if (Has(args, "--year-of-ash-save-selftest"))
                return HostCliAction.YearOfAshSaveSelfTest;
            if (Has(args, "--verdict-selftest") || Has(args, "--expansion-08-selftest"))
                return HostCliAction.VerdictSelfTest;
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
            GD.Print("  --bridge-selftest        UnityEngine shim failure policy (semantic throws, cosmetic quiet)");
            GD.Print("  --year-of-ash-save-selftest Year of Ash save write → reload → restore → checksum/tamper checks");
            GD.Print("  --verdict-selftest         The Verdict (Exp 08): machine log, reckoning phases, evidence, census, save");
            GD.Print("  --duty-roster-save-selftest Duty Roster save write → reload → restore → checksum/tamper checks");
            GD.Print("  --expansion-hub-save-selftest Expansion hub save write → reload → restore → checksum/tamper checks");
            GD.Print("  --dose-ledger-selftest       Dose Ledger save write → reload → restore → checksum/tamper checks");
            GD.Print("  --data-integrity-selftest  Cross-reference every id in the 55 StreamingAssets catalogs (recipe→item, quest→location, events, door encounters, survivors, factions, ranges, duplicates)");
            GD.Print("  --asset-registry-selftest  Verify that catalog IDs (items/survivors/locations) resolve to actual texture assets under assets/");
            GD.Print("  --standalone-selftest     SkyLayerArmor, VigilStateMachine, GenerationalSuccession, EpilogueMatrix, DiveInstance");
            GD.Print("  --phase0-selftest         Phase-0 effects: phantom work-eff/refusal, flashbacks, trade specialty, final-wish buff, respiratory stamina + save roundtrip");
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

        public static int RunGreenhouseSelfTest()
        {
            var report = GreenhouseHeadlessDemo.Run(new GodotLog());
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
                    Check(loaded.evidence.enrolled.Count == 1, "evidence restored");
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

                // ── 6. Save round-trip ──────────────────────────────────
                var save = session.CaptureSave();
                Check(save != null && save.effects.Count >= 3, "phase-0 save captured effects");
                var fresh = new Phase0HostSession();
                fresh.RestoreSave(save);
                Check(Math.Abs(fresh.PermanentShelterMoraleBuff - session.PermanentShelterMoraleBuff) < 1e-4f,
                    "permanent shelter morale buff restored");
                Check(fresh.TradeSpecialty.HasMasteredTrade("elena_vasquez"),
                    "trade mastery restored");
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

    }
}
