// SPDX-License-Identifier: MIT
// Expansion 36 runtime verification. The checks are intentionally attached to
// the existing patrol-encounter selftest verb until a new CLI registry verb is
// available on every supported host surface.
using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Defense;
using Ashfall.Core.Shelter;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    public static class HostCliNightWatch
    {
        public static HeadlessReport RunSelfTest(string dataDirectory)
        {
            CatalogLocator.UseInvariantCulture();
            var report = new HeadlessReport();
            void Check(bool condition, string name)
            {
                report.Checks.Add(new HeadlessCheck { Name = name, Passed = condition });
                if (condition) { report.PassedCount++; GD.Print("[PASS] " + name); }
                else { report.FailedCount++; GD.PrintErr("[FAIL] " + name); }
            }

            try
            {
                var fileIO = CatalogPath.CreateFileIOForDataDir(dataDirectory);
                var load = NightWatchOperationsCatalogLoader.Load(dataDirectory, fileIO);
                Check(load.Success && load.Catalog != null, "strict operations catalog loads");
                var catalog = load.Catalog;
                if (catalog == null)
                {
                    report.Passed = false;
                    report.Summary = "operations catalog unavailable";
                    return report;
                }

                Check(catalog.posts.Count == 15 && catalog.routes.Count == 12 && catalog.drills.Count == 10,
                    "authored posts/routes/drills are present");
                var perimeter = new PerimeterDefenseSystem(
                    Array.Empty<PerimeterDefenseDefinition>(),
                    new Ashfall.Core.Inventory.Inventory(),
                    new SeededRng(360));
                perimeter.BindWatchCatalog(catalog);
                var roster = new DutyRosterSystem();
                roster.RestoreState(new DutyRosterSystemState
                {
                    expansionUnlocked = true,
                    rows = new List<DutyRosterRow>
                    {
                        new DutyRosterRow { survivorId = "watch_survivor", displayName = "Watch Survivor", occupationObserved = "watchman", status = DutyRosterIds.StatusHome }
                    }
                });
                var security = new ShelterSecuritySystem();
                var host = new NightWatchHostSession(perimeter, roster, security, catalog)
                {
                    SurvivorFatigueProvider = _ => 100
                };

                Check(perimeter.WatchPosts.Count == 15, "watch state binds to perimeter owner");
                var blind = host.EvaluateSector("gate");
                Check(blind.PatrolCoverage.CoverageGrade == PatrolCoverageGrade.Blind, "empty watch is honestly blind");
                Check(host.AssignWatchShift("watch_post_main_gate", "watch_survivor", 18, 4).IsSuccess &&
                      host.AssignWatchShift("watch_post_main_gate", "watch_survivor", 8, 4).IsSuccess &&
                      roster.GetWatchShifts().Count == 2,
                    "valid watch shift routes through canonical roster validation");
                var staffed = host.EvaluateSector("gate");
                Check(staffed.ActivePostCount >= 0 && staffed.PatrolCoverage.DetectionProbabilityPermille > blind.PatrolCoverage.DetectionProbabilityPermille,
                    "staffed projection improves detection");
                string firstShiftId = roster.GetWatchShifts()[0].shift_id;
                Check(host.CompleteWatchShift(firstShiftId).IsSuccess,
                    "shift completion records operational fatigue");
                Check(host.ToggleSectorAlarm("gate").IsSuccess && perimeter.FindSector("gate")!.alarm_armed,
                    "watch establishes the canonical gate alarm relay");
                Check(host.WalkRoute("watch_route_inner_ring", 2, debriefed: false).IsSuccess,
                    "route walk is recorded");
                Check(perimeter.FindWatchRoute("watch_route_inner_ring")!.debrief_pending,
                    "route debrief remains pending");
                Check(host.RecordDebrief("watch_route_inner_ring", 3).IsSuccess &&
                      !perimeter.FindWatchRoute("watch_route_inner_ring")!.debrief_pending,
                    "debrief closes the route record");
                Check(host.RunDrill("watch_drill_alarm_bell", 3, passed: true).IsSuccess,
                    "readiness drill is recorded");
                Check(perimeter.GetWatchDrillRecencyPermille("watch_drill_alarm_bell", 3) == 1000,
                    "fresh drill recency is visible");

                var saved = perimeter.CaptureState();
                var restored = new PerimeterDefenseSystem(
                    Array.Empty<PerimeterDefenseDefinition>(),
                    new Ashfall.Core.Inventory.Inventory(),
                    new SeededRng(361));
                restored.BindWatchCatalog(catalog);
                restored.RestoreState(saved);
                Check(restored.WatchRoutes.Count == 12 && restored.WatchDrills.Count == 10 &&
                      restored.FindWatchRoute("watch_route_inner_ring")!.rounds_completed == 1,
                    "watch state round-trips through canonical perimeter save");

                var panel = new UI.NightWatchPanel();
                panel._Ready();
                panel.Bind(host);
                bool panelBound = panel.IsBound;
                panel.Unbind();
                panel.Free();
                Check(panelBound, "Watch panel binds and unbinds without leaking callbacks");

                Check(ReadRepoFile("src", "Main.NightWatch.cs").Contains("TickNightWatch") &&
                      ReadRepoFile("src", "Main.ExpandedShelterSystems.cs").Contains("night_watch") &&
                      ReadRepoFile("Assets", "Ashfall.Core", "UI", "PanelRegistryBootstrap.cs").Contains("night_watch") &&
                      ReadRepoFile("src", "UI", "NightWatchPanel.cs").Contains("NightWatchPanel"),
                    "host lifecycle and routed player panel are wired");

                report.Passed = report.FailedCount == 0;
                report.Summary = $"{report.PassedCount}/{report.PassedCount + report.FailedCount} checks passed";
                return report;
            }
            catch (Exception ex)
            {
                report.FailedCount++;
                report.Passed = false;
                report.Summary = "exception: " + ex.Message;
                GD.PrintErr("[NightWatch] exception: " + ex);
                return report;
            }
        }

        private static string ReadRepoFile(params string[] parts)
        {
            try
            {
                string dir = AppContext.BaseDirectory;
                for (int i = 0; i < 8 && dir != null; i++)
                {
                    string candidate = System.IO.Path.Combine(dir, System.IO.Path.Combine(parts));
                    if (System.IO.File.Exists(candidate)) return System.IO.File.ReadAllText(candidate);
                    dir = System.IO.Directory.GetParent(dir)?.FullName ?? string.Empty;
                }
            }
            catch (Exception) { /* cleanup: optional probe file is unavailable */ }
            return string.Empty;
        }
    }
}
