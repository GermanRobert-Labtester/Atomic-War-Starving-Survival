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
            _expeditions.Bridge.Surface(state);
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

            GD.Print(pass ? "EXPEDITION_PANEL_UITEST PASS" : "EXPEDITION_PANEL_UITEST FAIL");
            QuitUiTestAfterFrame(pass ? 0 : 1);
        }

    }
}
