using System;
using System.Collections.Generic;
using System.Text;

namespace Ashfall.Core
{
    /// <summary>
    /// Master expansion orchestrator for ASHFALL.
    /// Unifies and coordinates all 4 game expansion subsystems:
    /// - Expansion 01: The Holdfast (District 8, Ice Road, Census Claims, Brine Water, Waystations)
    /// - Expansion 02: The Duty Roster (Allocation 12 Interior, Chart State, Morale Marks, Labour Shifts)
    /// - Expansion 03: The Standing Record (Ground Layouts, Room Hierarchies, Site Stencils)
    /// - Expansion 04: Nobody's Charter (The Crossing Viaduct, Vouch Access, The Scale Bloc)
    /// </summary>
    public sealed class ExpansionMasterSession
    {
        public HoldfastSession Holdfast { get; }
        public DutyRosterSystem DutyRoster { get; }
        public DutyRosterCatalog DutyRosterData { get; }
        public LocationLayoutSystem StandingRecord { get; }
        public CrossingSession Crossing { get; }
        public SimClock Clock { get; }
        public ILog Log { get; }

        public bool AllExpansionsActive =>
            Holdfast.IceRoad.IsUnlocked &&
            DutyRoster.State.expansionUnlocked &&
            StandingRecord.State.expansionUnlocked &&
            Crossing.Vouch.HasAccess;

        public ExpansionMasterSession(
            HoldfastSession holdfast,
            DutyRosterSystem dutyRoster,
            DutyRosterCatalog dutyRosterData,
            LocationLayoutSystem standingRecord,
            CrossingSession crossing,
            SimClock clock,
            ILog log = null)
        {
            Holdfast = holdfast ?? throw new ArgumentNullException(nameof(holdfast));
            DutyRoster = dutyRoster ?? throw new ArgumentNullException(nameof(dutyRoster));
            DutyRosterData = dutyRosterData ?? throw new ArgumentNullException(nameof(dutyRosterData));
            StandingRecord = standingRecord ?? throw new ArgumentNullException(nameof(standingRecord));
            Crossing = crossing ?? throw new ArgumentNullException(nameof(crossing));
            Clock = clock ?? throw new ArgumentNullException(nameof(clock));
            Log = log ?? NullLog.Instance;
        }

        public static ExpansionMasterSession Load(string dataDirectory, int seed = 808, ILog log = null)
        {
            CatalogLocator.UseInvariantCulture();
            log = log ?? NullLog.Instance;

            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var clock = new SimClock(90);

            // Expansion 1: Holdfast
            var holdfast = HoldfastSession.Load(dataDirectory, seed, expansionUnlocked: true, log);

            // Expansion 2: Duty Roster
            var dutyLoader = new DutyRosterCatalogLoader(files, json, log);
            var dutyData = dutyLoader.Load(dataDirectory);
            var dutySys = new DutyRosterSystem();
            dutySys.Unlock(clock.Day);

            // Expansion 3: Standing Record
            var layoutSys = new LocationLayoutSystem(files, json, log);
            layoutSys.Load(dataDirectory);
            layoutSys.Unlock();

            // Expansion 4: Nobody's Charter
            var crossing = CrossingSession.Load(dataDirectory, log);

            return new ExpansionMasterSession(holdfast, dutySys, dutyData, layoutSys, crossing, clock, log);
        }

        /// <summary>
        /// Daily advance simulation across all 4 active expansion subsystems.
        /// </summary>
        public void TickDaily(WeatherKind weather, float outdoorTemp, List<DutyRosterOccupant> homeOccupants = null)
        {
            Clock.AdvanceDays(1);
            int day = Clock.Day;

            // 1. Tick Holdfast Ice Road & Census
            Holdfast.IceRoad.TickDaily(day, weather, outdoorTemp);
            Holdfast.Census.TickDaily(day);
            Holdfast.Brine.TickDaily(day, weather, outdoorTemp, false);

            // 2. Tick Duty Roster morning occupancy
            if (homeOccupants != null && homeOccupants.Count > 0)
            {
                DutyRoster.TickMorning(day, homeOccupants);
            }

            Log.Info($"[ExpansionMasterSession] Ticked day {day} across all 4 expansions.");
        }

        /// <summary>
        /// Runs comprehensive validation across all 4 expansion packs.
        /// </summary>
        public static HeadlessReport RunAllSelfTests(string dataDirectory = null, ILog log = null)
        {
            CatalogLocator.UseInvariantCulture();
            log = log ?? NullLog.Instance;

            var masterReport = new HeadlessReport();
            log.Info("=== [ExpansionMasterSession] Running Full 4-Expansion Suite ===");

            var reports = new List<(string Name, HeadlessReport Report)>
            {
                ("Holdfast (Exp 01)", HoldfastHeadlessDemo.Run(dataDirectory, log)),
                ("Duty Roster (Exp 02)", DutyRosterHeadlessDemo.Run(dataDirectory, log)),
                ("Standing Record (Exp 03)", StandingRecordHeadlessDemo.Run(dataDirectory, log)),
                ("Nobody's Charter (Exp 04)", CrossingHeadlessDemo.Run(dataDirectory, log)),
                ("Crossing Arbitration", CrossingArbitrationHeadlessDemo.Run(log)),
                ("Ledger Debt", LedgerDebtHeadlessDemo.Run(log)),
                ("Glass Orchard (Exp 05)", GreenhouseHeadlessDemo.Run(log))
            };

            for (int i = 0; i < reports.Count; i++)
            {
                var r = reports[i];
                masterReport.PassedCount += r.Report.PassedCount;
                masterReport.FailedCount += r.Report.FailedCount;
                if (r.Report.Checks != null)
                {
                    for (int j = 0; j < r.Report.Checks.Count; j++)
                    {
                        masterReport.Checks.Add(r.Report.Checks[j]);
                    }
                }
            }

            masterReport.Passed = masterReport.FailedCount == 0;
            masterReport.Summary = $"[Expansions Suite] {masterReport.PassedCount}/{masterReport.PassedCount + masterReport.FailedCount} PASSED, {(masterReport.Passed ? "ALL EXPANSIONS GREEN" : "FAILED")}";
            log.Info(masterReport.Summary);
            return masterReport;
        }
    }
}
