using System.Collections.Generic;
using System.Text;

namespace Ashfall.Core
{
    /// <summary>
    /// Headless verification smoke for Expansion 02: The Duty Roster.
    /// Tests: catalog loading, chart initial state, occupant morning registration,
    /// pencil allowance, ink commitment, burn action, morale marks, and state serialization.
    /// </summary>
    public static class DutyRosterHeadlessDemo
    {
        public static HeadlessReport Run(string dataDirectory = null, ILog log = null)
        {
            CatalogLocator.UseInvariantCulture();
            log = log ?? NullLog.Instance;
            var report = new HeadlessReport();

            void Check(bool condition, string name)
            {
                report.Checks.Add(new HeadlessCheck { Name = name, Passed = condition });
                if (condition) report.PassedCount++;
                else
                {
                    report.FailedCount++;
                    log.Error("[FAIL] " + name);
                }
                if (condition) log.Info("[PASS] " + name);
            }

            log.Info("[DutyRosterHeadlessDemo] begin");

            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var loader = new DutyRosterCatalogLoader(files, json, log);
            var catalog = loader.Load(dataDirectory);

            report.LocationCount = catalog.Locations.Count;
            report.QuestCount = catalog.Quests.Count;

            Check(catalog.Locations.Count >= 4, "duty roster locations loaded (>=4)");
            Check(catalog.Quests.Count >= 2, "duty roster quests loaded (>=2)");
            Check(catalog.Marks.Count >= 5, "duty roster marks loaded (>=5)");

            var wall = catalog.GetLocation(DutyRosterSystem.LocStackRosterWall);
            Check(wall != null, "loc_stack_roster_wall present in catalog");

            var mark = catalog.GetMark("mark_ration_protocol");
            Check(mark != null, "mark_ration_protocol present in catalog");

            // System runtime test
            var roster = new DutyRosterSystem();
            roster.Unlock(1);
            Check(roster.State.chartScript == DutyRosterSystem.ScriptBlank, "initial chart is blank");
            Check(!roster.State.wallInspected, "wall not inspected initially");

            roster.NotifyWallInspected();
            Check(roster.State.wallInspected, "wall inspected after action");

            var occupants = new List<DutyRosterOccupant>
            {
                new DutyRosterOccupant { survivorId = "sv_kess", displayName = "Kess", occupationObserved = "Mechanic", sleptHere = true },
                new DutyRosterOccupant { survivorId = "sv_ianov", displayName = "Dr. Ianov", occupationObserved = "Surgeon", sleptHere = true },
                new DutyRosterOccupant { survivorId = "sv_elena", displayName = "Elena", occupationObserved = "Scout", sleptHere = false }
            };

            roster.ResolveChartChoice(DutyRosterSystem.ChoiceWritePencil, 1);
            Check(roster.State.kessPencilAllowed, "pencil allowed for Kess");
            Check(roster.State.chartScript == DutyRosterSystem.ScriptPencil, "chart script transitioned to pencil");

            roster.TickMorning(1, occupants);
            Check(roster.State.rows.Count == 2, "morning tick registered 2 slept survivors");
            Check(roster.GetRow("sv_kess") != null, "Kess found in roster rows");

            roster.WriteName("sv_elena", "Elena", "Scout", DutyRosterSystem.ScriptInk, 2, false);
            Check(roster.GetRow("sv_elena") != null, "Elena committed in ink");

            // Save / Load roundtrip
            string blob = json.Serialize(roster.CaptureState());
            var restored = new DutyRosterSystem();
            restored.RestoreState(json.Deserialize<DutyRosterSystemState>(blob));
            Check(restored.State.rows.Count == 3, "save roundtrip preserved 3 rows");
            Check(restored.GetRow("sv_elena") != null, "Elena found after restore");

            // Burn action
            roster.BurnChart(10);
            Check(roster.State.chartScript == DutyRosterSystem.ScriptBurned, "chart script is burned after BurnChart");
            Check(roster.State.rows.Count == 0, "rows cleared after BurnChart");

            report.Passed = report.FailedCount == 0;
            var sb = new StringBuilder();
            sb.Append("DutyRosterHeadlessDemo ");
            sb.Append(report.Passed ? "PASS" : "FAIL");
            sb.Append(" ").Append(report.PassedCount).Append("/").Append(report.PassedCount + report.FailedCount);
            sb.Append(" locations=").Append(report.LocationCount).Append(" quests=").Append(report.QuestCount);
            report.Summary = sb.ToString();
            log.Info(report.Summary);
            return report;
        }
    }
}
