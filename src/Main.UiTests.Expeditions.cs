using Godot;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using AtomicWar.Journal;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Economy;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Foundry;
using Ashfall.Core.Inventory;
using Ashfall.Core.Journal;
using Ashfall.Core.Muster;
using Ashfall.Core.YearOfAsh;
using Ashfall.Core.Radio;
using Ashfall.Core.Survivors;
using AtomicWar.GodotApp.Economy;
using AtomicWar.GodotApp.YearOfAsh;
using AtomicWar.GodotApp.Muster;
using AtomicWar.GodotApp.Dose;
using AtomicWar.GodotApp.UtilityAI;
using AtomicWar.GodotApp.Radio;
using AtomicWar.GodotApp.Audio;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        /// <summary>
        /// Expedition panel encounter-notice lifecycle: open → surface → close →
        /// reopen → surface. Verifies the host's OnEncounterSurfaced subscription
        /// delivers exactly one notice per surface (no double-subscribe) and that
        /// a closed panel does not leak a stale handler that double-fires after
        /// reopen.
        /// </summary>
        private void RunExpeditionPanelUiTestAndQuit()
        {
            BuildUserInterface();
            SetupExpeditions();

            bool pass = true;
            void Check(bool cond, string name)
            {
                if (cond) GD.Print($"  [PASS] {name}");
                else { GD.PrintErr($"  [FAIL] {name}"); pass = false; }
            }

            Check(_expeditions != null, "expedition host ready");
            Check(_expeditionPanel != null, "expedition panel exists");

            // Bind + open through the real path.
            _expeditionPanel!.Bind(_expeditions!, _survivors!, _inventory!);
            _expeditionPanel.Open();
            Check(_expeditionPanel.Visible && _expeditionPanel.IsBound, "panel opens bound");

            // Surface a synthetic expedition state through the bridge:
            // host -> OnEncounterSurfaced -> Main.OnExpeditionEncounterSurfaced -> panel.
            var state = new ExpeditionState
            {
                survivorId = "survivor_gunner_mikhail",
                locationId = "loc_the_allotments",
                displayName = "The Works Allotment Commune",
                phase = (int)ExpeditionPhase.Outbound,
                encounterCount = 1
            };
            _expeditions!.Bridge.Surface(state);
            Check(_expeditionPanel.TotalEncounterNotices == 1, "one notice delivered on first surface");

            // Close, reopen, surface again — count must advance by exactly one
            // (no double-subscribe, no stale handler after reopen).
            _expeditionPanel.Close();
            Check(!_expeditionPanel.Visible, "panel closes cleanly");
            _expeditionPanel.Open();
            Check(_expeditionPanel.Visible, "panel reopens");
            _expeditions.Bridge.Surface(state);
            Check(_expeditionPanel.TotalEncounterNotices == 2, "second surface delivers exactly one more notice");

            // A resolvable encounter should render choice buttons into the modal.
            var def = _expeditions.FindEncounter(_expeditions.Pending.Count > 0
                ? _expeditions.Pending[0].encounterId
                : string.Empty);
            Check(def != null || _expeditions.Pending.Count == 0, "pending queue consistent with surfaced encounters");

            // Close and reopen to ensure modal is dismissed before exemplar test sequence
            _expeditionPanel.Close();
            _expeditionPanel.Open();

            // ── Plan 49 (F21): 4 Exemplar Micro-Locations UI & Navigation Verification ──
            string[] exemplars = new[]
            {
                "micro_roadside_memorial",
                "micro_crashed_truck",
                "micro_observation_post",
                "micro_frozen_bus"
            };

            foreach (var exemplarId in exemplars)
            {
                var exDef = _expeditions.FindEncounter(exemplarId);
                Check(exDef != null, $"exemplar {exemplarId} found in catalog");
                if (exDef == null) continue;

                var surfaced = new ExpeditionEncounterBridge.EncounterSurfaced
                {
                    encounter_id = exemplarId,
                    title = exDef.title,
                    description = exDef.description,
                    category = exDef.category,
                    choices = exDef.choices ?? new List<Ashfall.Core.Narrative.EncounterChoiceDefinition>(),
                    is_micro_location = exDef.isMicroLocation,
                    trigger = state,
                    resolved_at_lead = true
                };

                _expeditionPanel.ShowEncounterNotice(surfaced);
                Check(_expeditionPanel.EncounterModal != null && _expeditionPanel.EncounterModal.Visible, $"modal visible for {exemplarId}");
                Check(_expeditionPanel.EncounterTitleLabel != null && !string.IsNullOrEmpty(_expeditionPanel.EncounterTitleLabel.Text), $"{exemplarId} title rendered");
                Check(_expeditionPanel.EncounterContextLabel != null && _expeditionPanel.EncounterContextLabel.Text == "DISCOVERY · MICRO-LOCATION", $"{exemplarId} shows DISCOVERY · MICRO-LOCATION context");
                Check(_expeditionPanel.EncounterBodyLabel != null && _expeditionPanel.EncounterBodyLabel.SizeFlagsHorizontal.HasFlag(Control.SizeFlags.ExpandFill), $"{exemplarId} body has ExpandFill");
                Check(_expeditionPanel.EncounterBodyLabel?.AutowrapMode == TextServer.AutowrapMode.WordSmart, $"{exemplarId} body autowraps WordSmart");

                if (exemplarId == "micro_frozen_bus")
                {
                    Check(exDef.description.Length >= 190, "micro_frozen_bus has longest description (~198 chars)");
                    Check(_expeditionPanel.EncounterBodyLabel?.Text.Contains(exDef.description) == true, "longest description text preserved without truncation");
                }

                Check(_expeditionPanel.ChoicesContainer != null, $"{exemplarId} choices container created");

                _expeditionPanel.Close();
                _expeditionPanel.Open();
            }

            // German localization exemplar check
            AtomicWar.GodotApp.Localization.AshfallLocalization.SetLocale("de");
            var memorialDef = _expeditions.FindEncounter("micro_roadside_memorial");
            if (memorialDef != null)
            {
                var surfacedDe = new ExpeditionEncounterBridge.EncounterSurfaced
                {
                    encounter_id = "micro_roadside_memorial",
                    title = memorialDef.title,
                    description = memorialDef.description,
                    category = memorialDef.category,
                    choices = memorialDef.choices,
                    is_micro_location = memorialDef.isMicroLocation,
                    trigger = state,
                    resolved_at_lead = true
                };
                _expeditionPanel.ShowEncounterNotice(surfacedDe);
                Check(_expeditionPanel.EncounterTitleLabel?.Text == "Straßenrand-Gedenkstätte", "German title resolves for micro_roadside_memorial");
                Check(_expeditionPanel.EncounterBodyLabel?.Text.Contains("Geschmolzene Talgreste") == true, "German description resolves for micro_roadside_memorial");
                _expeditionPanel.Close();
                _expeditionPanel.Open();
            }
            AtomicWar.GodotApp.Localization.AshfallLocalization.SetLocale("en");

            HostCli.EmitSummary("expedition_panel_uitest", pass, pass ? 0 : 1);
            QuitUiTestAfterFrame(pass ? 0 : 1);
        }

    }
}
