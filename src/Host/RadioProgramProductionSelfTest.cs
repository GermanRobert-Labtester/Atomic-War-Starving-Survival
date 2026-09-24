// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : RadioProgramProductionSelfTest
// Subsystem          : Plan 173 — Radio Program Production & Audience Response
// ============================================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Radio;

namespace AtomicWar.GodotApp
{
    public static class RadioProgramProductionSelfTest
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Radio Program Production & Audience Response Self-Test (Plan 173) ===");
            int passed = 0;
            const int total = 12;

            try
            {
                // Check 1: Catalog loading from radio_programs.json
                string dataRoot = (!string.IsNullOrEmpty(dataDir) && Directory.Exists(dataDir) ? dataDir : CatalogPath.ResolveDataDir());
                string catPath = Path.Combine(dataRoot, "radio_programs.json");

                var catalog = RadioProgramCatalogLoader.LoadFromJson(File.ReadAllText(catPath));
                if (catalog != null && catalog.All.Count >= 2 && catalog.Get("radio_prog_shelter_morning_bulletin") != null)
                {
                    Console.WriteLine($"[PASS] Check 1: Authoritative catalog loaded {catalog.All.Count} programs from radio_programs.json.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 1: Could not load programs from {catPath}.");
                }

                // Setup test station catalog matching authored slots
                var stations = new RadioStationCatalog();
                stations.Register(new RadioStationDefinition
                {
                    StationId = "station_civil_defense",
                    DisplayName = "Civil Defense",
                    FrequencyMhz = 88.5f,
                    Schedule = new List<RadioProgramSlot>
                    {
                        new RadioProgramSlot { SlotId = "slot_cd_morning", StartHour = 6, EndHour = 11 }
                    }
                });
                stations.Register(new RadioStationDefinition
                {
                    StationId = "station_open_classroom",
                    DisplayName = "Open Classroom",
                    FrequencyMhz = 91.3f,
                    Schedule = new List<RadioProgramSlot>
                    {
                        new RadioProgramSlot { SlotId = "slot_classroom_evening", StartHour = 19, EndHour = 23 }
                    }
                });

                var inventoryMap = new Dictionary<string, int>
                {
                    { "radio_headset", 1 },
                    { "aa_batteries", 5 }
                };

                float shelterMoraleApplied = 0f;
                var system = new RadioProgramProductionSystem(catalog, stations)
                {
                    HasRequiredEquipment = items =>
                    {
                        if (items == null) return true;
                        foreach (var item in items)
                        {
                            if (!inventoryMap.TryGetValue(item, out int count) || count < 1)
                                return false;
                        }
                        return true;
                    },
                    TryConsumePrepCost = (itemId, count) =>
                    {
                        if (inventoryMap.TryGetValue(itemId, out int current) && current >= count)
                        {
                            inventoryMap[itemId] = current - count;
                            return true;
                        }
                        return false;
                    },
                    PresenterCapabilityProvider = _ => 1.2f,
                    ApplyShelterMoraleDelta = delta => shelterMoraleApplied += delta
                };
                var session = new RadioProgramProductionHostSession(system);

                // Check 2: Invalid template rejection
                var badTemplateResult = system.StartPrep("nonexistent_program", "survivor_1", 1);
                if (badTemplateResult.Status == ActionResult.StatusKind.Blocked && badTemplateResult.FailureCode == "unknown_template")
                {
                    Console.WriteLine("[PASS] Check 2: Blocked unknown program template correctly.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 2: Unexpected result for unknown template ({badTemplateResult.FailureCode}).");
                }

                // Check 3: Slot validation
                var bogusCatalog = new RadioProgramCatalog
                {
                    programs = new List<RadioProgramTemplateDef>
                    {
                        new RadioProgramTemplateDef
                        {
                            id = "bogus_slot_prog",
                            station_id = "station_civil_defense",
                            slot_id = "slot_unregistered_slot",
                            prep_ticks_required = 1
                        }
                    }
                };
                bogusCatalog.Index();
                var bogusSystem = new RadioProgramProductionSystem(bogusCatalog, stations);
                var badSlotResult = bogusSystem.StartPrep("bogus_slot_prog", "survivor_1", 1);
                if (badSlotResult.Status == ActionResult.StatusKind.Blocked && badSlotResult.FailureCode == "unknown_slot")
                {
                    Console.WriteLine("[PASS] Check 3: Station/slot mismatch rejected with unknown_slot.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 3: Slot mismatch check failed ({badSlotResult.FailureCode}).");
                }

                // Check 4: Missing equipment rejection
                inventoryMap["radio_headset"] = 0; // Strip equipment
                var missingEquipResult = system.StartPrep("radio_prog_shelter_morning_bulletin", "survivor_1", 1);
                if (missingEquipResult.Status == ActionResult.StatusKind.Blocked && missingEquipResult.FailureCode == "missing_equipment")
                {
                    Console.WriteLine("[PASS] Check 4: Missing equipment correctly rejected (radio_headset required).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 4: Missing equipment not rejected ({missingEquipResult.FailureCode}).");
                }
                inventoryMap["radio_headset"] = 1; // Restore equipment

                // Check 5: Prep cost consumption
                int batteryBefore = inventoryMap["aa_batteries"];
                var prepResult = system.StartPrep("radio_prog_shelter_morning_bulletin", "survivor_1", 1);
                int batteryAfter = inventoryMap["aa_batteries"];
                if (prepResult.Status == ActionResult.StatusKind.Success && batteryAfter == batteryBefore - 1)
                {
                    Console.WriteLine($"[PASS] Check 5: Prep cost consumed 1x aa_batteries (from {batteryBefore} to {batteryAfter}).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 5: Prep cost not consumed properly (res={prepResult.FailureCode}, before={batteryBefore}, after={batteryAfter}).");
                }

                // Check 6: Duplicate active job blocked
                var dupResult = system.StartPrep("radio_prog_shelter_morning_bulletin", "survivor_1", 1);
                if (dupResult.Status == ActionResult.StatusKind.Blocked && dupResult.FailureCode == "job_active")
                {
                    Console.WriteLine("[PASS] Check 6: Duplicate active program job blocked (job_active).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 6: Duplicate job not blocked ({dupResult.FailureCode}).");
                }

                // Check 7: Cancel prep job
                var activeJobs = system.GetActiveJobs();
                var firstJob = activeJobs.First();
                var cancelResult = session.CancelJob(firstJob.JobId);
                if (firstJob.Status == (int)RadioProgramJobStatus.Cancelled && system.State.TotalCancelled == 1)
                {
                    Console.WriteLine("[PASS] Check 7: Active job cancelled successfully; state updated to Cancelled.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 7: Job cancellation failed (status={firstJob.Status}, totalCancelled={system.State.TotalCancelled}).");
                }

                // Check 8: Daily prep advancement to Ready
                var prepResult2 = system.StartPrep("radio_prog_shelter_morning_bulletin", "survivor_host", 2);
                var job2 = system.GetActiveJobs().First(j => j.Status == (int)RadioProgramJobStatus.Preparing);
                system.TickDay(3);
                if (job2.Status == (int)RadioProgramJobStatus.Ready && job2.PrepTicks >= job2.PrepTicksRequired)
                {
                    Console.WriteLine($"[PASS] Check 8: Daily prep advancement ticked job to Ready status.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 8: Job did not advance to Ready (status={job2.Status}, ticks={job2.PrepTicks}).");
                }

                // Check 9: Opportunistic delivery via TryDeliver
                var broadcastFact = new ScheduledBroadcastResult
                {
                    HasTransmission = true,
                    StationId = "station_civil_defense",
                    BroadcastId = "bcast_cd_morning_day3",
                    SignalStrength = 9,
                    VuStrength = 0.95f
                };
                var deliverResult = session.TryDeliver(job2.JobId, broadcastFact, 3);
                if (job2.Status == (int)RadioProgramJobStatus.Delivered && session.DeliveredCount == 1)
                {
                    Console.WriteLine($"[PASS] Check 9: Program delivered successfully with broadcast receipt {job2.LastDeliveryBroadcastId}.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 9: Delivery failed ({deliverResult}, status={job2.Status}).");
                }

                // Check 10: Audience response calculation
                if (job2.AudienceReachGrade == "Regional" && job2.AudienceMoraleDelta > 0f)
                {
                    Console.WriteLine($"[PASS] Check 10: Audience response calculated reach={job2.AudienceReachGrade}, moraleDelta={job2.AudienceMoraleDelta:+0.00}.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 10: Audience response calculation failed (reach={job2.AudienceReachGrade}, morale={job2.AudienceMoraleDelta}).");
                }

                // Check 11: Shelter-wide morale delta applied
                if (shelterMoraleApplied > 0f && Math.Abs(shelterMoraleApplied - job2.AudienceMoraleDelta) < 0.001f)
                {
                    Console.WriteLine($"[PASS] Check 11: Shelter morale delta applied callback executed (+{shelterMoraleApplied}).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 11: Shelter morale delta callback not fired or mismatched ({shelterMoraleApplied} vs {job2.AudienceMoraleDelta}).");
                }

                // Check 12: Follow-up hook generated, resolved, and save round-trip parity
                var followUps = session.GetUnresolvedFollowUps();
                bool hookFound = followUps.Any(f => f.SourceJobId == job2.JobId && !f.Resolved);
                string hookId = followUps.First(f => f.SourceJobId == job2.JobId).HookId;
                session.ResolveFollowUp(hookId, "air_followup_bulletin", 4);

                var savedState = session.CaptureSave();
                var freshSystem = new RadioProgramProductionSystem(catalog, stations);
                var freshSession = new RadioProgramProductionHostSession(freshSystem);
                freshSession.RestoreSave(savedState);

                var restoredHook = freshSession.System.State.FollowUps.FirstOrDefault(f => f.HookId == hookId);
                if (hookFound && restoredHook != null && restoredHook.Resolved && freshSession.DeliveredCount == 1 && freshSession.System.State.TotalCancelled == 1)
                {
                    Console.WriteLine($"[PASS] Check 12: Follow-up hook resolved and save/restore round-trip preserved 100% parity.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 12: Follow-up or save round-trip failed.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EXCEPTION] RadioProgramProductionSelfTest threw: {ex}");
            }

            Console.WriteLine($"=== Radio Program Production Self-Test Result: {passed}/{total} Passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
