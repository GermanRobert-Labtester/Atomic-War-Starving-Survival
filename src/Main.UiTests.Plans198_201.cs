// SPDX-License-Identifier: MIT
using Godot;
using Ashfall.Core.Combat;
using Ashfall.Core.Crafting;
using Ashfall.Core.Narrative;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        // ── Plans 198–201 end-to-end UI contract ────────────────────────────
        // Per panel: route (registry resolve) → bind → visible → command →
        // Core state delta → feedback strip. Any engine exception fails the
        // gate (UI-21: fail on exceptions, not just non-null checks).

        private sealed class UiTestFailure : System.Exception
        {
            public UiTestFailure(string message) : base(message) { }
        }

        private static void Check(bool condition, string message)
        {
            if (!condition) throw new UiTestFailure(message);
        }

        private void RunPlans198To201UiTestAndQuit()
        {
            int failures = 0;
            try
            {
                BuildUserInterface();
                RunChemWarfareUiContract();
                RunCommsArrayUiContract();
                RunCeremonyUiContract();
                RunRoboticsUiContract();
                RunRegistryRouteContract();
            }
            catch (System.Exception ex)
            {
                GD.PrintErr($"[FAIL] Plans198-201 UI test exception: {ex.GetType().Name}: {ex.Message}\n{ex.StackTrace}");
                failures++;
            }

            if (failures == 0)
                GD.Print("Plans198To201UiTest PASS");
            else
                GD.PrintErr($"[FAIL] Plans198To201UiTest: {failures} failure(s)");
            GetTree().Quit(failures == 0 ? 0 : 1);
        }

        // ── Plan 198: CBRN hazard monitor ──────────────────────────────────

        private void RunChemWarfareUiContract()
        {
            var system = EnsureChemWarfare();
            Check(_chemWarfareDefensePanel != null, "chem: panel not constructed");

            // Bind → visible
            _chemWarfareDefensePanel.Bind(system);
            Check(_chemWarfareDefensePanel.IsBound, "chem: bind did not take");
            _chemWarfareDefensePanel.Open();
            Check(_chemWarfareDefensePanel.Visible, "chem: Open() did not make panel visible");

            // Command → state delta: hazard appears; exposure evaluated while active.
            var hazard = system.DeployHazard("chem_agent_irritant_prewar", 1, "uitest_source");
            Check(hazard != null, "chem: DeployHazard returned null");
            Check(system.State.ActiveHazards.Count == 1, "chem: hazard not registered in state");
            _chemWarfareDefensePanel.RefreshView();

            // Exposure evaluation contract (deterministic, no RNG) — hazard is live.
            int severity = system.EvaluateActorExposure("actor_test", 1, maskCondition01: 1.0f, out float wear);
            Check(severity == 0, $"chem: intact respirator must fully absorb, got severity {severity}");
            Check(wear > 0f, "chem: intact respirator must still accumulate filter wear");
            int unprot = system.EvaluateActorExposure("actor_test", 1, maskCondition01: 0.0f, out _);
            Check(unprot > 0, "chem: unprotected exposure in hazard lane must produce severity");

            // Command → state delta: decon clears the hazard.
            HandleChemWarfareAction("clear_hazard", hazard!.HazardId);
            Check(system.State.ActiveHazards.Count == 0, "chem: clear_hazard command produced no state delta");
            Check(!string.IsNullOrEmpty(_chemWarfareDefensePanel.LastFeedback), "chem: no feedback after clear command");

            // Blocked path: clearing an absent hazard reports failure feedback.
            HandleChemWarfareAction("clear_hazard", "hazard_does_not_exist");
            Check(!string.IsNullOrEmpty(_chemWarfareDefensePanel.LastFeedback), "chem: no feedback on blocked clear");
            _chemWarfareDefensePanel.Close();
            Check(!_chemWarfareDefensePanel.Visible, "chem: Close() did not hide panel");
            GD.Print("  [PASS] chem_warfare: route/bind/visible/command/delta/feedback");
        }

        // ── Plan 199: comms array ─────────────────────────────────────────

        private void RunCommsArrayUiContract()
        {
            var system = EnsureCommsArray();
            Check(_commsArrayTransceiverPanel != null, "comms: panel not constructed");

            _commsArrayTransceiverPanel.Bind(system);
            Check(_commsArrayTransceiverPanel.IsBound, "comms: bind did not take");
            _commsArrayTransceiverPanel.SetDisplayClock(3, 12);
            _commsArrayTransceiverPanel.Open();
            Check(_commsArrayTransceiverPanel.Visible, "comms: Open() did not make panel visible");

            // Command → state delta: tuning moves the carrier.
            const string targetId = "comms_target_weather_beacon_alpha";
            Check(system.TargetCatalog.TryGetValue(targetId, out var target), "comms: known target missing from catalog");
            HandleCommsArrayAction("tune", targetId);
            Check(system.State.CurrentFrequencyKhz == target!.FrequencyKhz,
                $"comms: tune command produced no frequency delta ({system.State.CurrentFrequencyKhz} != {target.FrequencyKhz})");
            Check(!string.IsNullOrEmpty(_commsArrayTransceiverPanel.LastFeedback), "comms: no feedback after tune");

            // Deterministic scan → contact state delta (tier-1 target, powered grid).
            system.SetArrayTier(1);
            system.SetPowerState(true, 1000f);
            string? contact = null;
            for (int day = 1; day <= 6 && contact == null; day++)
                contact = system.TickScan(day, 12, 0.5f);
            Check(contact != null, "comms: powered tier-1 scan against tuned tier-1 target never established contact");
            var lockState = system.GetOrCreateLock(targetId);
            Check(lockState.IsContactEstablished, "comms: lock state did not flip to contact");
            _commsArrayTransceiverPanel.RefreshView();

            // Blocked path: strike request without intercepted code reports failure.
            string strategicId = "comms_target_strategic_uplink_cerberus";
            HandleCommsArrayAction("request_strike", strategicId);
            Check(!string.IsNullOrEmpty(_commsArrayTransceiverPanel.LastFeedback), "comms: no feedback on blocked strike request");

            _commsArrayTransceiverPanel.Close();
            Check(!_commsArrayTransceiverPanel.Visible, "comms: Close() did not hide panel");
            GD.Print("  [PASS] comms_array: route/bind/visible/command/delta/feedback");
        }

        // ── Plan 200: ceremonies ──────────────────────────────────────────

        private void RunCeremonyUiContract()
        {
            var system = EnsureCeremonySystem();
            Check(_ceremonyFestivalPanel != null, "ceremony: panel not constructed");

            _ceremonyFestivalPanel.Bind(system);
            Check(_ceremonyFestivalPanel.IsBound, "ceremony: bind did not take");
            _ceremonyFestivalPanel.Open();
            Check(_ceremonyFestivalPanel.Visible, "ceremony: Open() did not make panel visible");

            // Blocked path: unknown ceremony id → player-readable blocker, no state.
            HandleCeremonyAction("schedule", "ceremony_does_not_exist");
            Check(!string.IsNullOrEmpty(_ceremonyFestivalPanel.LastFeedback), "ceremony: no feedback on blocked schedule");
            Check(system.ActiveCeremony == null, "ceremony: schedule with unknown id must not create state");

            // Success path: schedule with adequate population → state delta.
            Check(system.ScheduleCeremony("ceremony_remembrance_vigil", currentDay: 1, currentPopulation: 10, out string err),
                $"ceremony: valid schedule failed: {err}");
            Check(system.ActiveCeremony != null, "ceremony: schedule produced no active state");
            _ceremonyFestivalPanel.RefreshView();

            // Full cycle through Core: contribute materials, advance days,
            // verify completion delta (morale/truce events fire via host wiring).
            var def = system.CeremonyCatalog["ceremony_remembrance_vigil"];
            foreach (var req in def.RequiredItems)
                Check(system.ContributeResource(req.ItemId, req.Quantity), $"ceremony: contribution rejected for {req.ItemId}");
            for (int day = 2; day <= 10 && system.ActiveCeremony!.Phase != CeremonyPhase.Completed; day++)
                system.TickDay(day, out _);
            Check(system.ActiveCeremony.Phase == CeremonyPhase.Completed, "ceremony: full preparation cycle never completed");
            Check(system.State.TotalCeremoniesHeld == 1, $"ceremony: held count delta wrong ({system.State.TotalCeremoniesHeld})");
            _ceremonyFestivalPanel.RefreshView();

            // Duplicate-completion guard: schedule while Completed releases the slot.
            Check(system.ScheduleCeremony("ceremony_founding_day", currentDay: 11, currentPopulation: 10, out _),
                "ceremony: new schedule after completion must be accepted");

            _ceremonyFestivalPanel.Close();
            Check(!_ceremonyFestivalPanel.Visible, "ceremony: Close() did not hide panel");
            GD.Print("  [PASS] ceremony: route/bind/visible/command/delta/feedback");
        }

        // ── Plan 201: robotics ────────────────────────────────────────────

        private void RunRoboticsUiContract()
        {
            var system = EnsureRobotics();
            Check(_roboticsWorkshopPanel != null, "robotics: panel not constructed");

            _roboticsWorkshopPanel.Bind(system);
            Check(_roboticsWorkshopPanel.IsBound, "robotics: bind did not take");
            _roboticsWorkshopPanel.Open();
            Check(_roboticsWorkshopPanel.Visible, "robotics: Open() did not make panel visible");

            // Success path: raise a unit through Core (materials flow is the
            // host inventory authority, exercised by the blocked path below).
            var unit = system.ReactivateRobot("robot_utility_maintenance_drone", programmerSkill01: 0.5f, out string err);
            Check(unit != null, $"robotics: reactivation failed: {err}");
            Check(system.Units.Count == 1, "robotics: unit registry delta missing");

            // Command → state delta: directive programming.
            HandleRoboticsAction("program", $"{unit!.UnitId}:directive_haul");
            Check(string.Equals(unit.AssignedDirective, "directive_haul", System.StringComparison.Ordinal),
                $"robotics: program command produced no directive delta ({unit.AssignedDirective})");
            Check(!string.IsNullOrEmpty(_roboticsWorkshopPanel.LastFeedback), "robotics: no feedback after program");

            // Blocked path: repair needs inventory; headless has none.
            HandleRoboticsAction("repair", unit.UnitId);
            Check(!string.IsNullOrEmpty(_roboticsWorkshopPanel.LastFeedback), "robotics: no feedback on blocked repair");

            // EMP determinism: canonical disable → tick recovery.
            system.ApplyEmpShock(2);
            Check(unit.IsEmpDisabled && unit.EmpDisableHoursRemaining == 2, "robotics: EMP shock must disable with exact duration");
            system.TickLabor(2, isDockedToGrid: false, gridPowerAvailableWatts: 0f);
            Check(!unit.IsEmpDisabled, "robotics: EMP disable must expire after exactly the authored hours");
            _roboticsWorkshopPanel.RefreshView();

            // Rogue-path coherence: at fragile logic + zero skill the Core may
            // corrupt (deterministic 35% roll). Either way state must stay coherent.
            unit.LogicIntegrity = 100;
            system.ProgramDirective(unit.UnitId, "directive_guard", programmerSkill01: 0.0f, out _);
            Check(unit.IsRogue || string.Equals(unit.AssignedDirective, "directive_guard", System.StringComparison.Ordinal),
                "robotics: incoherent directive state after fragile programming");
            _roboticsWorkshopPanel.RefreshView();

            _roboticsWorkshopPanel.Close();
            Check(!_roboticsWorkshopPanel.Visible, "robotics: Close() did not hide panel");
            GD.Print("  [PASS] robotics: route/bind/visible/command/delta/feedback");
        }

        // ── Registry route contract (UI-09 closure evidence) ──────────────

        private void RunRegistryRouteContract()
        {
            string[] ids = { "chem_warfare_defense", "comms_array_transceiver", "ceremony_ritual", "robotics_assembly" };
            foreach (string id in ids)
            {
                var descriptor = Ashfall.Core.UI.PanelRegistry.Resolve(id, msg => GD.PrintErr(msg));
                Check(descriptor != null, $"route: '{id}' is not registered in PanelRegistry");
                Check(descriptor!.Maturity == Ashfall.Core.UI.PanelMaturity.Live, $"route: '{id}' is not Live");
                // Bind + Open through the registered lambdas — the exact player path.
                descriptor.Bind();
                descriptor.Open();
                Check(descriptor.IsPlayerNavigable, $"route: '{id}' is not player-navigable");
            }
            // Close everything the route contract opened.
            CloseChemWarfareDefensePanel();
            CloseCommsArrayTransceiverPanel();
            CloseCeremonyFestivalPanel();
            CloseRoboticsWorkshopPanel();
            Check(!_chemWarfareDefensePanel.Visible && !_commsArrayTransceiverPanel.Visible
                && !_ceremonyFestivalPanel.Visible && !_roboticsWorkshopPanel.Visible,
                "route: close path left one of the four consoles visible");
            // Note: the global AnyOverlayPanelOpen() contract also covers the
            // journal book and briefing modal, whose headless boot state is
            // environment-dependent — those are owned by the lifecycle suite.
            GD.Print("  [PASS] registry routes: 4/4 descriptors Live, bind+open+close via player path");
        }
    }
}
