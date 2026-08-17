using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ashfall.Core.Combat;
using Ashfall.Core.Disease;
using Ashfall.Core.Foundry;
using Ashfall.Core.Narrative;
using Ashfall.Core.Warlords;

namespace Ashfall.Core
{
    /// <summary>
    /// Master expansion orchestrator for ASHFALL.
    /// Unifies and coordinates all 4 game expansion subsystems:
    /// - Expansion 01: The Holdfast (District 8, Ice Road, Census Claims, Brine Water, Waystations)
    /// - Expansion 02: The Duty Roster (Allocation 12 Interior, Chart State, Morale Marks, Labour Shifts)
    /// - Expansion 03: The Standing Record (Ground Layouts, Room Hierarchies, Site Stencils)
    /// - Expansion 04: Nobody's Charter (The Crossing Viaduct, Vouch Access, The Scale Bloc)
    /// plus the silent-foundry production hub (Expansion 10) and the
    /// Disease Expansion (contagion, quarantine, outbreak ward).
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
        public SilentFoundrySystem SilentFoundry { get; }
        public SilentFoundryCatalog FoundryData { get; }
        public DiseaseSystem Disease { get; }
        public DiseaseCatalog DiseaseData { get; }

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
            ILog log = null,
            SilentFoundrySystem silentFoundry = null,
            SilentFoundryCatalog foundryData = null,
            DiseaseSystem disease = null,
            DiseaseCatalog diseaseData = null)
        {
            Holdfast = holdfast ?? throw new ArgumentNullException(nameof(holdfast));
            DutyRoster = dutyRoster ?? throw new ArgumentNullException(nameof(dutyRoster));
            DutyRosterData = dutyRosterData ?? throw new ArgumentNullException(nameof(dutyRosterData));
            StandingRecord = standingRecord ?? throw new ArgumentNullException(nameof(standingRecord));
            Crossing = crossing ?? throw new ArgumentNullException(nameof(crossing));
            Clock = clock ?? throw new ArgumentNullException(nameof(clock));
            Log = log ?? NullLog.Instance;
            SilentFoundry = silentFoundry ?? new SilentFoundrySystem(log: log);
            FoundryData = foundryData ?? new SilentFoundryCatalog();
            Disease = disease ?? new DiseaseSystem(log: log);
            DiseaseData = diseaseData ?? new DiseaseCatalog();
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

            // Expansion 10: The Silent Foundry — static catalogs + blueprint/treaty anchors.
            var foundryData = new SilentFoundryCatalog();
            foundryData.Load(
                SilentFoundryCatalogLoader.LoadProduction(dataDirectory, files, json),
                SilentFoundryCatalogLoader.LoadFaction(dataDirectory, files, json));
            var foundry = new SilentFoundrySystem(log: log);
            var consequencePolicy = new SilentFoundryConsequencePolicyCatalog();
            consequencePolicy.Load(SilentFoundryConsequenceCatalogLoader.Load(dataDirectory, files, json));
            foundry.BindConsequencePolicy(consequencePolicy);
            int maintenanceCycle = 4;
            var blueprints = new BunkerBlueprintCatalog();
            string bpJson = files.FileExists(Path.Combine(dataDirectory, "narrative", "bunker_blueprints_codex.json"))
                ? files.ReadAllText(Path.Combine(dataDirectory, "narrative", "bunker_blueprints_codex.json"))
                : string.Empty;
            if (!string.IsNullOrEmpty(bpJson))
            {
                blueprints.Load(bpJson, json);
                var bp = blueprints.GetById(SilentFoundryIds.BlueprintRoomId);
                if (bp != null && bp.maintenance_cycle_days > 0) maintenanceCycle = bp.maintenance_cycle_days;
            }
            // District 8 accords (foundry_accords.json) drive the treaty clock —
            // campaign-reachable days, Sector 4 / District 8 canon.
            var ratificationDays = SilentFoundryCatalogLoader.LoadAccordRatificationDays(dataDirectory, files, json);
            if (ratificationDays.Count > 0)
                foundry.BindTreaties(ratificationDays);
            foundry.BindCatalog(foundryData, maintenanceCycle);

            // Disease Expansion: static catalog + deterministic contagion engine.
            // Always active once bound; outbreaks threaten every day of the campaign.
            var diseaseData = DiseaseCatalogLoader.Load(dataDirectory, files, json);
            var disease = new DiseaseSystem(log: log);
            disease.BindCatalog(diseaseData);

            return new ExpansionMasterSession(holdfast, dutySys, dutyData, layoutSys, crossing, clock, log,
                foundry, foundryData, disease, diseaseData);
        }

        /// <summary>
        /// Daily advance simulation across all 4 active expansion subsystems.
        /// </summary>
        public void TickDaily(WeatherKind weather, float outdoorTemp, List<DutyRosterOccupant> homeOccupants = null,
            IReadOnlyList<string> diseaseCandidates = null)
        {
            Clock.AdvanceDays(1);
            int day = Clock.Day;

            // 1. Tick Holdfast Ice Road & Census
            Holdfast.IceRoad.TickDaily(day, weather, outdoorTemp);
            Holdfast.Census.TickDaily(day);
            Holdfast.Brine.TickDaily(day, weather, outdoorTemp, false);
            Holdfast.DeepCoast.TickDaily(day, weather);

            // 2. Tick Duty Roster morning occupancy
            if (homeOccupants != null && homeOccupants.Count > 0)
            {
                DutyRoster.TickMorning(day, homeOccupants);
            }

            // 3. Tick The Silent Foundry (Exp 10)
            SilentFoundry.TickDaily(day);

            // 4. Tick the Disease Expansion (contagion / quarantine ward). The
            // host supplies the survivor pool; without candidates the ward only
            // advances existing patients (no autonomous spread).
            Disease.TickDaily(day, diseaseCandidates);

            Log.Info($"[ExpansionMasterSession] Ticked day {day} across all 4 expansions + Silent Foundry + Disease Expansion.");
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
                ("Glass Orchard (Exp 05)", GreenhouseHeadlessDemo.Run(log)),
                ("Deep Coast (District 8)", DeepCoastHeadlessDemo.Run(dataDirectory, log)),
                ("Warlord AI (Exp 05 sibling)", WarlordHeadlessDemo.Run(dataDirectory, log)),
                ("Silent Foundry (Exp 10)", SilentFoundryHeadlessDemo.Run(dataDirectory, log)),
                ("Disease Expansion", DiseaseHeadlessDemo.Run(dataDirectory, log)),
                ("Combat Expansion", CombatHeadlessDemo.Run(log))
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
