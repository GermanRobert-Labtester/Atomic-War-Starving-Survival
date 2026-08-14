using Godot;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Medical;
using Ashfall.Core.Narrative;
using Ashfall.Core.Survivors;
using Ashfall.Core.Muster;
using Ashfall.Core.YearOfAsh;
using AtomicWar.GodotApp.YearOfAsh;
using System;
using System.IO;

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
        DutyRosterSaveSelfTest,
        ExpansionHubSaveSelfTest,
        DoseLedgerSelfTest,
        ExpeditionSelfTest,
        MedicalSelfTest,
        NarrativeSelfTest,
        SurvivorsSelfTest,
        DataIntegritySelfTest,
        CaravanSelfTest
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
            if (Has(args, "--data-integrity-selftest"))
                return HostCliAction.DataIntegritySelfTest;
            if (Has(args, "--caravan-selftest") || Has(args, "--traveling-caravan-selftest"))
                return HostCliAction.CaravanSelfTest;
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
            GD.Print("  --brine-selftest         BrineWaterHeadlessDemo (S2 salt & steam)");
            GD.Print("  --muster-selftest        MusterHeadlessDemo (Exp 06 the Muster)");
            GD.Print("  --cluster-selftest       Cluster12CHeadlessDemo (S3 order 12-C + quest snapshot)");
            GD.Print("  --endings-selftest       EndingsHeadlessDemo (S4 endings exclusive + roundtrip)");
            GD.Print("  --holdfast-briefing      Print location count and every Holdfast quest briefing");
            GD.Print("  --journal-selftest       Journal domain + save roundtrip");
            GD.Print("  --journal-uitest         Build ledger UI, cycle tabs, quit");
            GD.Print("  --bridge-selftest        UnityEngine shim failure policy (semantic throws, cosmetic quiet)");
            GD.Print("  --year-of-ash-save-selftest Year of Ash save write → reload → restore → checksum/tamper checks");
            GD.Print("  --duty-roster-save-selftest Duty Roster save write → reload → restore → checksum/tamper checks");
            GD.Print("  --expansion-hub-save-selftest Expansion hub save write → reload → restore → checksum/tamper checks");
            GD.Print("  --dose-ledger-selftest       Dose Ledger save write → reload → restore → checksum/tamper checks");
            GD.Print("  --data-integrity-selftest  Cross-reference every id in the 55 StreamingAssets catalogs (recipe→item, quest→location, events, door encounters, survivors, factions, ranges, duplicates)");
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
            return report.ExitCode;
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
                && session.Catalog.GetQuest("quest_holdfast_the_sheet") != null;
            GD.Print(ok
                ? $"HoldfastBriefing PASS locations={session.LocationCount} quests={session.QuestCount}"
                : $"HoldfastBriefing FAIL locations={session.LocationCount} quests={session.QuestCount}");
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

        public static int RunCaravanSelfTest()
        {
            return TravelingCaravanHeadlessDemo.Run();
        }
    }
}
