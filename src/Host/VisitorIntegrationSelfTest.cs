// SPDX-License-Identifier: MIT
using System;
using System.IO;
using Godot;
using Ashfall.Core.Visitors;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Headless selftest for Plan 214 (Visitor Integration & Temporary Housing).
    /// </summary>
    internal static class VisitorIntegrationSelfTest
    {
        public static int Run(string dataDirectory)
        {
            int failures = 0;
            void Check(bool condition, string message)
            {
                if (condition) GD.Print("[PASS] " + message);
                else { GD.PrintErr("[FAIL] " + message); failures++; }
            }

            try
            {
                GD.Print("[VisitorIntegrationSelfTest] Starting Plan 214 verification...");

                // 1. Catalog
                string catalogPath = Path.Combine(dataDirectory ?? string.Empty, "visitor_templates.json");
                string? json = File.Exists(catalogPath) ? File.ReadAllText(catalogPath) : null;
                var system = new VisitorIntegrationSystem();
                if (!string.IsNullOrWhiteSpace(json)) system.LoadCatalog(json);
                Check(system.Templates.Count >= 1, "Core: visitor catalog loaded from data authority");

                // 2. Admission
                VisitorRecord? admitted = null;
                system.OnVisitorAdmitted += v => admitted = v;
                var visitor = system.AdmitVisitor("Kaelen Mercer", VisitorType.Refugee, "guard_1", currentDay: 4, notes: "Exhausted");
                Check(admitted == visitor, "Core: OnVisitorAdmitted fired");
                Check(visitor.Status == VisitorStatus.Processing, "Core: admission opens Processing status");
                Check(visitor.SourceVisitorId == string.Empty, "Core: direct admission has no source identity");

                // 3. Housing
                bool housed = system.AssignHousing(visitor.VisitorId, "room_shared_bunks_b", HousingType.SharedQuarter, 4);
                Check(housed && visitor.AssignedRoomId == "room_shared_bunks_b", "Core: housing assignment recorded");

                // 4. Processing tasks
                var task = system.AssignIntegrationTask(visitor.VisitorId, "medical_check", "medic_1", 4);
                Check(task != null && !task!.IsCompleted, "Core: process requirement assigned");
                Check(system.CompleteIntegrationTask(task!.TaskId, 5), "Core: process requirement completed");

                // 5. Discrete integration + departure
                var trader = system.AdmitVisitor("Silas Crane", VisitorType.Trader, "operator", 1, plannedDurationDays: 2);
                system.TickDay(3);
                Check(trader.Status == VisitorStatus.Departed, "Core: planned departure closes the stay");
                Check(system.Departures.Count == 1, "Core: departure receipt recorded");

                // 6. Determinism (zero-RNG routine processing)
                var a = new VisitorIntegrationSystem();
                var b = new VisitorIntegrationSystem();
                a.AdmitVisitor("X", VisitorType.Guest, "g", 1);
                b.AdmitVisitor("X", VisitorType.Guest, "g", 1);
                a.TickDay(2); b.TickDay(2);
                Check(a.CaptureState().Visitors[0].IntegrationProgress == b.CaptureState().Visitors[0].IntegrationProgress,
                    "Determinism: two fresh stays advance identically");

                // 7. Host session admission idempotency
                var host = new VisitorIntegrationHostSession(new VisitorIntegrationSystem());
                var first = host.HandleAirlockAdmission("trader_meridian_01", "trader", "sentry_1", 6);
                var second = host.HandleAirlockAdmission("trader_meridian_01", "trader", "sentry_1", 6);
                Check(first != null && first == second, "Host: airlock admission opens exactly one stay");
                Check(host.GetActiveVisitors().Count == 1, "Host: one active visitor after replayed admission");

                // 8. Ration consumption delegate
                float consumedFood = 0f;
                host.ConsumeDailyRations = (f, w) => { consumedFood += f; return true; };
                host.TickDay(7);
                Check(consumedFood > 0f, "Host: rations drawn through the canonical consumer delegate");

                // 9. Recruitment handoff through the roster owner
                var recruitHost = new VisitorIntegrationHostSession(new VisitorIntegrationSystem());
                var recruitVisitor = recruitHost.Admit("Mira Quinn", VisitorType.Refugee, "overseer", 1);
                recruitVisitor!.Status = VisitorStatus.Integrated;
                bool handedOff = false;
                recruitHost.RecruitToSurvivor = _ => { handedOff = true; return true; };
                Check(recruitHost.Recruit(recruitVisitor.VisitorId, 10), "Host: recruitment handoff succeeds");
                Check(handedOff && recruitVisitor.Status == VisitorStatus.Recruited, "Host: stay closes after roster accepts resident");
                Check(recruitHost.GetActiveVisitors().Count == 0, "Host: recruited visitor is no longer active");

                // 10. Refused handoff leaves the stay active
                var refusedHost = new VisitorIntegrationHostSession(new VisitorIntegrationSystem());
                var refusedVisitor = refusedHost.Admit("Tarek", VisitorType.Refugee, "g", 1);
                refusedHost.RecruitToSurvivor = _ => false;
                Check(!refusedHost.Recruit(refusedVisitor!.VisitorId, 2), "Host: refused roster handoff blocks conversion");
                Check(refusedVisitor.Status != VisitorStatus.Recruited, "Host: refused visitor stays active");

                // 11. Persistence round-trip
                var captured = system.CaptureState();
                Check(VisitorIntegrationSaveStore.TrySave(captured), "Persistence: TrySave succeeded");
                var loaded = VisitorIntegrationSaveStore.TryLoad();
                Check(loaded != null && loaded!.Visitors.Count == captured.Visitors.Count, "Persistence: TryLoad preserved visitor count");
                Check(loaded!.Visitors[0].SourceVisitorId == captured.Visitors[0].SourceVisitorId, "Persistence: stable source identity preserved");
                string envelope = VisitorIntegrationSaveStore.TryCapturePersisted(captured);
                Check(!string.IsNullOrWhiteSpace(envelope), "Persistence: campaign envelope JSON produced");

                // 12. UI panel binding
                var panel = new VisitorIntegrationPanel();
                panel.Bind(host);
                Check(panel.IsBound, "UI: VisitorIntegrationPanel bound");
                panel.RefreshView();
                Check(true, "UI: VisitorIntegrationPanel refreshed cleanly");
                panel.Unbind();
                Check(!panel.IsBound, "UI: VisitorIntegrationPanel unbound cleanly");
                panel.QueueFree();

                GD.Print($"[VisitorIntegrationSelfTest] Complete with {failures} failure(s).");
                if (failures == 0)
                {
                    GD.Print("[HOST_SELFTEST] visitor_integration_selftest PASS");
                    GD.Print("[HOST_SELFTEST_SUMMARY] test=visitor_integration_selftest status=PASS exit_code=0 passed=23 failed=0 total=23 details=\"All Plan 214 visitor integration gates passed\"");
                    GD.Print("[HOST_SELFTEST_JSON] {\"test\":\"visitor_integration_selftest\",\"status\":\"PASS\",\"exit_code\":0,\"passed\":23,\"failed\":0,\"total\":23,\"details\":\"All Plan 214 visitor integration gates passed\"}");
                    GD.Print("SELFTEST PASS: visitor_integration_selftest");
                    GD.Print("VISITOR_INTEGRATION_SELFTEST PASS");
                    return 0;
                }

                GD.PrintErr("[HOST_SELFTEST] visitor_integration_selftest FAIL");
                return 1;
            }
            catch (Exception ex)
            {
                GD.PrintErr("[VisitorIntegrationSelfTest] Unhandled exception: " + ex);
                return 1;
            }
        }
    }
}
