// SPDX-License-Identifier: MIT
// T10 action-result surfacing gate — prevents the "silent refusal" class from
// regrowing. Two contracts are pinned:
//
//   1. Host sessions that publish a `LastEvent` string and back a panel that
//      renders it must assign `LastEvent` on the failure branch too. Assigning
//      only inside `if (result.IsSuccess)` leaves the previous success sentence
//      on screen after a refusal — a silent no-op to the player.
//   2. Panels that bind a Core system directly (no host `LastEvent` seam) must
//      render refusals through the shared `ActionRefusalText` formatter.
//
// Naming pattern: UiLifecycleBypassGateTests. Docs:
// docs/ACTION_RESULT_SURFACING_MATRIX.md.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class ActionResultSurfacingGateTests
    {
        private static string RepoRoot()
        {
            string dir = new DirectoryInfo(AppContext.BaseDirectory).FullName;
            for (int i = 0; i < 8 && dir != null; i++)
            {
                if (Directory.Exists(Path.Combine(dir, "src")))
                    return dir;
                dir = Directory.GetParent(dir)?.FullName;
            }
            throw new DirectoryNotFoundException("repo root not found from test context");
        }

        // Swept host methods that back a panel rendering host `LastEvent`.
        private static readonly (string File, string Method, string Marker)[] HostMethods =
        {
            ("src/Host/ArchiveDeskHostSession.cs", "QueueTranscription", "LastEvent"),
            ("src/Host/ArchiveDeskHostSession.cs", "CancelJob", "LastEvent"),
            ("src/Host/ContractorRosterHostSession.cs", "GenerateOffer", "LastEvent"),
            ("src/Host/ContractorRosterHostSession.cs", "AcceptOffer", "LastEvent"),
            ("src/Host/ContractorRosterHostSession.cs", "Dismiss", "LastEvent"),
            ("src/Host/DecontaminationHostSession.cs", "Enqueue", "LastEvent"),
            ("src/Host/DecontaminationHostSession.cs", "ProcessQueue", "LastEvent"),
            ("src/Host/DecontaminationHostSession.cs", "CompleteCycle", "LastEvent"),
            ("src/Host/DefenseHostSession.cs", "ToggleSectorArm", "LastEvent"),
            ("src/Host/EquipmentConditionHostSession.cs", "StartMaintenance", "LastEvent"),
            ("src/Host/ShelterScheduleHostSession.cs", "SetCurfew", "LastEvent"),
            ("src/Host/ShelterScheduleHostSession.cs", "SetEmergencyOverride", "LastEvent"),
            ("src/Host/ShelterThermalHostSession.cs", "SetBoilerActive", "LastEvent"),
            ("src/Host/ShelterThermalHostSession.cs", "SetRadiatorValve", "LastEvent"),
            ("src/Host/ShelterThermalHostSession.cs", "RepairPipe", "LastEvent"),
            ("src/Host/SurvivorRelationsHostSession.cs", "Mediate", "LastEvent"),
            ("src/Host/VinylMoraleHostSession.cs", "PlayRecord", "LastEvent"),
            ("src/Host/VinylMoraleHostSession.cs", "StopPlayback", "LastEvent"),
            ("src/Host/SumpFloodingHostSession.cs", "InstallPump", "LastEvent"),
            ("src/Host/SumpFloodingHostSession.cs", "AssignStratum", "LastEvent"),
            ("src/Host/SumpFloodingHostSession.cs", "SetNodePower", "LastEvent"),
            ("src/Host/SumpFloodingHostSession.cs", "AddMitigation", "LastEvent"),
            ("src/Host/SumpFloodingHostSession.cs", "DrainNode", "LastEvent"),
            ("src/Host/WildlifeTrappingHostSession.cs", "SetTrap", "LastEvent"),
            ("src/Host/WildlifeTrappingHostSession.cs", "TrySetTrap", "Refuse("),
            ("src/Host/WildlifeTrappingHostSession.cs", "TryRepairTrap", "Refuse("),
            ("src/Host/WildlifeTrappingHostSession.cs", "PreserveHide", "LastEvent"),
            ("src/Host/WildlifeTrappingHostSession.cs", "RemoveTrap", "LastEvent"),
            ("src/Host/WildlifeTrappingHostSession.cs", "Butcher", "LastEvent"),
            ("src/Host/AirlockSecurityHostSession.cs", "CycleDoor", "LastEvent"),
            ("src/Host/AirlockSecurityHostSession.cs", "VisitorArrives", "LastEvent"),
            ("src/Host/AirlockSecurityHostSession.cs", "ResolveIncident", "LastEvent"),
            ("src/Host/AirlockSecurityHostSession.cs", "RepairDoor", "LastEvent"),
            ("src/Host/RadioProgramProductionHostSession.cs", "StartPrep", "LastEvent"),
            ("src/Host/RadioProgramProductionHostSession.cs", "CancelJob", "LastEvent"),
            ("src/Host/ApprenticeshipHostSession.cs", "RespondVocationalPair", "LastEvent"),
            ("src/Host/WildlifeTrappingHostSession.cs", "CheckTraps", "LastEvent"),
            ("src/Host/LowBackgroundMetrologyHostSession.cs", "Calibrate", "LastEvent"),
            ("src/Host/LowBackgroundMetrologyHostSession.cs", "StartBatch", "LastEvent"),
            ("src/Host/LowBackgroundMetrologyHostSession.cs", "CommitBatch", "LastEvent"),
            ("src/Host/ApprenticeshipHostSession.cs", "StartPair", "LastEvent"),
            ("src/Host/ApprenticeshipHostSession.cs", "CancelPair", "LastEvent"),
            ("src/Host/AutopsyHostSession.cs", "QueueCase", "LastEvent"),
            ("src/Host/AutopsyHostSession.cs", "BeginAutopsy", "LastEvent"),
            ("src/Host/ChemicalDependencyHostSession.cs", "BeginManagedDetox", "LastEvent"),
            ("src/Host/ChemicalDependencyHostSession.cs", "BeginColdTurkey", "LastEvent"),
            ("src/Host/DutyRosterHostSession.cs", "AssignDuty", "LastEvent"),
            ("src/Host/ExcavationHostSession.cs", "AddSite", "LastEvent"),
            ("src/Host/ExcavationHostSession.cs", "AssignWorkers", "LastEvent"),
            ("src/Host/ExcavationHostSession.cs", "ApplyShoring", "LastEvent"),
            ("src/Host/KitchenNutritionHostSession.cs", "StartPrepJob", "LastEvent"),
            ("src/Host/KitchenNutritionHostSession.cs", "ServeMeal", "LastEvent"),
            ("src/Host/KitchenNutritionHostSession.cs", "ServeAllMeals", "LastEvent"),
            ("src/Host/LibraryStudyHostSession.cs", "StartStudy", "LastEvent"),
            ("src/Host/MentalHealthCrisisHostSession.cs", "TriggerCrisis", "LastEvent"),
            ("src/Host/MentalHealthCrisisHostSession.cs", "BeginTreatment", "LastEvent"),
            ("src/Host/RegionalTreatyHostSession.cs", "ProposeTreaty", "LastEvent"),
            ("src/Host/RegionalTreatyHostSession.cs", "RatifyTreaty", "LastEvent"),
            ("src/Host/ShelterScheduleHostSession.cs", "AssignBed", "LastEvent"),
            ("src/Host/WildlifeTrappingHostSession.cs", "RemoveToxin", "LastEvent"),
            ("src/Host/EquipmentConditionHostSession.cs", "RegisterItem", "LastEvent"),
            ("src/Host/EquipmentConditionHostSession.cs", "UseItem", "LastEvent"),
            ("src/Host/SumpFloodingHostSession.cs", "AddNode", "LastEvent"),
            ("src/Host/WaterTreatmentHostSession.cs", "ReplaceFilter", "LastEvent"),
            ("src/Host/NightWatchHostSession.cs", "AssignWatchShift", "LastEvent"),
            ("src/Host/NightWatchHostSession.cs", "CompleteWatchShift", "LastEvent"),
            ("src/Host/NightWatchHostSession.cs", "WalkRoute", "LastEvent"),
            ("src/Host/NightWatchHostSession.cs", "RecordDebrief", "LastEvent"),
            ("src/Host/NightWatchHostSession.cs", "RunDrill", "LastEvent"),
            ("src/Host/NightWatchHostSession.cs", "SetPostActive", "LastEvent"),
            ("src/Host/NightWatchHostSession.cs", "RepairPost", "LastEvent"),
            ("src/Host/NightWatchHostSession.cs", "ToggleSectorAlarm", "LastEvent"),
            ("src/Host/SalvageHostSession.cs", "TryTeardown", "LastEvent"),
        };

        // Host-backed panels whose only feedback surface is host `LastEvent`; they
        // must render it, or the host's failure sentence never reaches the player.
        private static readonly string[] LastEventPanels =
        {
            "src/UI/AmphibiousDraisinePanel.cs",
            "src/UI/CvdDiamondPanel.cs",
            "src/UI/SolidOxideFuelCellPanel.cs",
            "src/UI/SoundRangingPanel.cs",
            "src/UI/LowBackgroundLeadPanel.cs",
        };

        // Direct-system panels must render refusals through the shared formatter.
        private static readonly string[] DirectPanels =
        {
            "src/UI/WorkshopPanel.cs",
            "src/UI/PharmaLabPanel.cs",
            "src/UI/WeatherForecastPanel.cs",
            "src/UI/ExpeditionPanel.cs",
        };

        [Fact]
        public void HostRefusalMethods_AssignLastEventOnFailure()
        {
            var failures = new List<string>();
            foreach (var (file, method, marker) in HostMethods)
            {
                string path = Path.Combine(RepoRoot(), file.Replace('/', Path.DirectorySeparatorChar));
                Assert.True(File.Exists(path), $"pinned host file missing: {file}");

                string src = File.ReadAllText(path);
                var bodies = ExtractMethodBodies(src, method);
                if (bodies.Count == 0)
                {
                    failures.Add($"{file}::{method} — method body not found (rename?)");
                    continue;
                }

                foreach (string body in bodies)
                {
                    bool hasMarker = body.Contains(marker, StringComparison.Ordinal);
                    bool hasFailure =
                        body.Contains("Refuse(", StringComparison.Ordinal)
                        || (body.Contains("LastEvent", StringComparison.Ordinal)
                            && (body.Contains("IsFailure", StringComparison.Ordinal)
                                || body.Contains("else", StringComparison.Ordinal)
                                || body.Contains("FailureCode", StringComparison.Ordinal)));
                    if (!hasMarker || !hasFailure)
                    {
                        failures.Add($"{file}::{method} does not surface a failure on LastEvent");
                    }
                }
            }

            Assert.True(failures.Count == 0,
                "Host ActionResult refusals must reach the panel's LastEvent surface " +
                "(docs/ACTION_RESULT_SURFACING_MATRIX.md § Host LastEvent convention). " +
                "Regressions:\n" + string.Join("\n", failures));
        }

        [Fact]
        public void HostBackedPanels_RenderLastEvent()
        {
            var failures = new List<string>();
            foreach (string panel in LastEventPanels)
            {
                string path = Path.Combine(RepoRoot(), panel.Replace('/', Path.DirectorySeparatorChar));
                Assert.True(File.Exists(path), $"pinned panel missing: {panel}");
                string src = File.ReadAllText(path);
                if (!src.Contains("_host.LastEvent", StringComparison.Ordinal))
                    failures.Add($"{panel} does not render the host LastEvent feedback line");
            }

            Assert.True(failures.Count == 0,
                "Host-backed panels must render the host LastEvent refusal line (T10). " +
                "Regressions:\n" + string.Join("\n", failures));
        }

        [Fact]
        public void DirectPanels_RenderRefusalsThroughSharedFormatter()
        {
            string formatterPath = Path.Combine(RepoRoot(), "src", "UI", "ActionRefusalText.cs");
            Assert.True(File.Exists(formatterPath),
                "src/UI/ActionRefusalText.cs is the shared Core-code -> prose formatter " +
                "for direct-system panels and must exist.");

            string formatter = File.ReadAllText(formatterPath);
            Assert.Contains("public static string Line(", formatter);
            Assert.Contains("public static string Describe(", formatter);

            var failures = new List<string>();
            foreach (string panel in DirectPanels)
            {
                string path = Path.Combine(RepoRoot(), panel.Replace('/', Path.DirectorySeparatorChar));
                Assert.True(File.Exists(path), $"pinned panel missing: {panel}");
                string src = File.ReadAllText(path);
                if (!src.Contains("ActionRefusalText.", StringComparison.Ordinal))
                    failures.Add($"{panel} does not route its refusal through ActionRefusalText");
            }

            Assert.True(failures.Count == 0,
                "Direct-system panels must render typed refusals (T10). Regressions:\n" +
                string.Join("\n", failures));
        }

        /// <summary>
        /// Extract every method body whose declaration ends with the given method
        /// name and an opening parenthesis. Brace-matched, so nested blocks are
        /// included. Handles overloads by returning all bodies.
        /// </summary>
        private static List<string> ExtractMethodBodies(string source, string methodName)
        {
            var result = new List<string>();
            var sig = new Regex(
                @"public\s+(?:static\s+)?(?:async\s+)?[A-Za-z0-9_<>\.\?\[\]]+\s+" +
                Regex.Escape(methodName) + @"\s*\(",
                RegexOptions.Compiled);

            foreach (Match m in sig.Matches(source))
            {
                int open = source.IndexOf('{', m.Index + m.Length);
                if (open < 0) continue;

                int depth = 0;
                int i = open;
                for (; i < source.Length; i++)
                {
                    char c = source[i];
                    if (c == '{') depth++;
                    else if (c == '}')
                    {
                        depth--;
                        if (depth == 0) break;
                    }
                }
                if (i >= source.Length) continue;
                result.Add(source.Substring(open, i - open + 1));
            }
            return result;
        }
    }
}
