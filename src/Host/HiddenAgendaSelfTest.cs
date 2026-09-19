// SPDX-License-Identifier: MIT
using System;
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Survivors;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Headless selftest for Plan 132 (Survivor Hidden Agendas & Betrayal Arc System).
    /// </summary>
    internal static class HiddenAgendaSelfTest
    {
        public static int Run(string dataDirectory)
        {
            int failures = 0;
            void Check(bool condition, string message)
            {
                if (condition)
                {
                    GD.Print("[PASS] " + message);
                }
                else
                {
                    GD.PrintErr("[FAIL] " + message);
                    failures++;
                }
            }

            try
            {
                GD.Print("[HiddenAgendaSelfTest] Starting Plan 132 verification...");

                // 1. Core Hidden Agenda System
                var system = new HiddenAgendaSystem();
                var agenda = system.AssignAgenda(
                    survivorId: "surv_test_01",
                    type: AgendaType.Sabotage,
                    targetFaction: "faction_hostile",
                    targetSurvivor: null,
                    startDay: 1,
                    notes: "Infiltrator attempting sabotage"
                );

                Check(agenda != null, "Core: Agenda assigned successfully");
                Check(system.ActiveAgendaCount == 1, "Core: Active agenda count is 1");
                Check(!agenda!.IsDiscovered, "Core: Agenda starts undiscovered");

                // Investigation under threshold
                var clue1 = system.InvestigateAgenda("surv_test_01", investigatorSkill: 30f, currentDay: 1);
                Check(clue1 != null, "Core: Investigation generated clue");
                Check(!agenda.IsDiscovered, "Core: Agenda remains secret under 60% threshold");
                Check(agenda.DiscoveryProgress == 30f, "Core: Discovery progress updated to 30%");

                // Investigation crossing threshold (30 + 35 = 65% >= 60%)
                var clue2 = system.InvestigateAgenda("surv_test_01", investigatorSkill: 35f, currentDay: 2);
                Check(clue2 != null, "Core: Second investigation generated clue");
                Check(agenda.IsDiscovered, "Core: Agenda exposed after crossing 60% threshold");
                Check(system.GetCluesForAgenda(agenda.AgendaId).Count == 2, "Core: Clues tracked for agenda");

                // 2. Confrontation
                bool confronted = system.ConfrontSurvivor(agenda.AgendaId, AgendaResolution.Reconciled, currentDay: 3);
                Check(confronted, "Core: Confrontation succeeded with sufficient evidence");
                Check(agenda.IsResolved, "Core: Agenda marked resolved");
                Check(agenda.Resolution == AgendaResolution.Reconciled, "Core: Resolution set to Reconciled");
                Check(system.ActiveAgendaCount == 0, "Core: Resolved agenda removed from active list");

                // 3. Passive Slip-Up Over Time
                var slipAgenda = system.AssignAgenda("surv_slip_02", AgendaType.ResourceTheft, startDay: 1);
                for (int d = 1; d <= 10; d++)
                {
                    system.TickDay(d);
                }
                Check(slipAgenda.DiscoveryProgress == 0f, "Core: No passive slip-up during first 10 days");
                system.TickDay(11);
                system.TickDay(12);
                system.TickDay(13);
                Check(slipAgenda.DiscoveryProgress == 2f, "Core: Passive slip-up increments after day 10");

                // 4. Host Session Orchestration
                var hostSys = new HiddenAgendaSystem();
                var host = new HiddenAgendaHostSession(hostSys);
                bool stateChangedFired = false;
                host.StateChanged += () => stateChangedFired = true;

                host.AssignAgenda("surv_host_01", AgendaType.FactionLoyalty);
                Check(stateChangedFired, "Host: StateChanged fired on AssignAgenda");
                Check(hostSys.ActiveAgendaCount == 1, "Host: Active agenda registered in host session");

                stateChangedFired = false;
                host.Investigate("surv_host_01", 30f, 1);
                Check(stateChangedFired, "Host: StateChanged fired on Investigate");

                // 5. Persistence Roundtrip via HiddenAgendaSaveStore
                var saveState = hostSys.CaptureState();
                bool saveOk = HiddenAgendaSaveStore.TrySave(saveState);
                Check(saveOk, "Persistence: HiddenAgendaSaveStore.TrySave succeeded");

                var loadedState = HiddenAgendaSaveStore.TryLoad();
                Check(loadedState != null, "Persistence: HiddenAgendaSaveStore.TryLoad succeeded");
                if (loadedState != null)
                {
                    Check(loadedState.Agendas.Count == saveState.Agendas.Count, "Persistence: Active agendas count preserved");
                    Check(loadedState.Clues.Count == saveState.Clues.Count, "Persistence: Discovered clues count preserved");
                }

                // Verify CapturePersisted for campaign envelope
                string persisted = HiddenAgendaSaveStore.TryCapturePersisted(saveState);
                Check(!string.IsNullOrEmpty(persisted), "Persistence: HiddenAgendaSaveStore.TryCapturePersisted produced valid envelope payload");

                // 6. UI Control Lifecycle
                var panel = new HiddenAgendaPanel();
                panel.Bind(host);
                Check(panel.IsBound, "UI: HiddenAgendaPanel bound successfully");
                panel.RefreshView();
                Check(true, "UI: HiddenAgendaPanel refreshed cleanly");
                panel.Unbind();
                Check(!panel.IsBound, "UI: HiddenAgendaPanel unbound cleanly");
                panel.QueueFree();

                GD.Print($"[HiddenAgendaSelfTest] Complete with {failures} failure(s).");
                return HostCli.EmitSummary("hidden_agenda_selftest", failures == 0, failures == 0 ? 0 : 1, passedCount: 26 - failures, failedCount: failures,
                    details: failures == 0 ? "All hidden agenda & betrayal arc gates passed" : $"{failures} failures detected");
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[HiddenAgendaSelfTest] Unexpected exception: {ex}");
                return HostCli.EmitSummary("hidden_agenda_selftest", false, 1, 0, 26, ex.Message);
            }
        }
    }
}
