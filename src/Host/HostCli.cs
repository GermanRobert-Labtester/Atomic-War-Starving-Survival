using Godot;
using Ashfall.Core;
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
        JournalSelfTest,
        JournalUiTest,
        BridgeSelfTest,
        DutyRosterSelfTest,
        StandingRecordSelfTest,
        CrossingSelfTest,
        ArbitrationSelfTest,
        LedgerDebtSelfTest,
        GreenhouseSelfTest,
        ExpansionsSelfTest
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
            if (Has(args, "--journal-selftest"))
                return HostCliAction.JournalSelfTest;
            if (Has(args, "--journal-uitest"))
                return HostCliAction.JournalUiTest;
            if (Has(args, "--bridge-selftest"))
                return HostCliAction.BridgeSelfTest;
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
            GD.Print("  --holdfast-briefing      Print location count and every Holdfast quest briefing");
            GD.Print("  --journal-selftest       Journal domain + save roundtrip");
            GD.Print("  --journal-uitest         Build ledger UI, cycle tabs, quit");
            GD.Print("  --bridge-selftest        UnityEngine shim failure policy (semantic throws, cosmetic quiet)");
            GD.Print("  --host-help              This list");
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

        public static int RunCoreSelfTest(string dataDirectory)
        {
            int ice = RunIceRoadSelfTest(dataDirectory);
            int census = RunCensusSelfTest();
            return ice != 0 ? ice : census;
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
    }
}
