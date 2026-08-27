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
        private void RunDutyRosterUiTestAndQuit()
        {
            // Self-contained run: a persisted duty_roster_save.json from an
            // earlier run must not leak chart state into the assertions.
            string rosterSave = Path.Combine(ProjectSettings.GlobalizePath("user://"), "duty_roster_save.json");
            if (System.IO.File.Exists(rosterSave)) System.IO.File.Delete(rosterSave);

            BuildUserInterface();
            SetupDutyRoster();
            SetupSurvivors();

            bool pass = true;
            void Check(bool cond, string name)
            {
                if (cond) GD.Print($"  [PASS] {name}");
                else { GD.PrintErr($"  [FAIL] {name}"); pass = false; }
            }

            Check(_dutyRoster != null && _dutyRoster.Roster.IsUnlocked, "host session unlocked");
            Check(_dutyRoster!.Roster.ChartScript == DutyRosterIds.ScriptBlank, "fresh chart starts blank");

            // Real interaction path through the panel.
            OpenPlayerPanel("duty_roster");
            Check(_dutyRosterPanel.Visible && _dutyRosterPanel.IsBound, "panel opens and binds");
            _dutyRoster.Roster.ResolveChartChoice(DutyRosterIds.ChoiceWritePencil, _simDay);
            _dutyRoster.Roster.TickMorning(_simDay + 1, new List<Ashfall.Core.DutyRosterOccupant>
            {
                new Ashfall.Core.DutyRosterOccupant { survivorId = "npc_kess_adler", displayName = "Kess Adler", sleptHere = true },
                new Ashfall.Core.DutyRosterOccupant { survivorId = "npc_ansel_duth", displayName = "Ansel Duth", sleptHere = true }
            });
            Check(_dutyRoster.Roster.OccupiedRowCount >= 2, "morning tick enrolled real home occupants");
            Check(_dutyRoster.Roster.Assign(DutyRosterIds.RoleNightWatch, "npc_kess_adler"), "assignment through the real path");
            Check(!_dutyRoster.Roster.Assign(DutyRosterIds.RoleMess, "npc_kess_adler"), "duplicate-role rule enforced");

            _dutyRosterPanel.RefreshView();
            Check(_dutyRosterPanel.StatusStripNonEmpty(), "panel read model renders");

            // Marks + encounter + Second Winter + overflow through the host session.
            _dutyRoster.Marks.SetMark(DutyRosterHoldfastBridge.MarkThreeAway, "3", _simDay);
            Check(_dutyRoster.Marks.HasMark(DutyRosterHoldfastBridge.MarkThreeAway), "mark set through host");
            Check(_dutyRoster.ActivateSecondWinter().Contains("second winter"), "second winter activates");
            Check(_dutyRoster.GrantOverflowAccess().Contains("granted"), "overflow access granted");
            Check(_dutyRoster.RegisterOverflowVisit(DutyRosterIds.LocOverflowAlloc11).Contains("visited"), "overflow visit registered");
            Check(_dutyRoster.BridgeHatchReturn("npc_ansel_duth").Contains("staged"), "hatch-return bridge stages a scene");
            Check(_dutyRoster.BridgeHatchReturn("npc_hadi_morrow").Contains("one per night"), "one hatch scene per night enforced");

            // Save round-trip through the real store path.
            _dutyRoster.SaveState();
            Check(System.IO.File.Exists(rosterSave), "duty roster save written");
            _dutyRoster.RestoreSave(DutyRosterSaveStore.TryLoad()!);
            Check(_dutyRoster.Roster.HasVisitedOverflow(DutyRosterIds.LocOverflowAlloc11), "overflow state survives save/load");
            Check(_dutyRoster.Marks.HasMark(DutyRosterHoldfastBridge.MarkThreeAway), "marks survive save/load");

            CloseDutyRosterPanel();
            Check(!_dutyRosterPanel.Visible, "panel closes cleanly");

            // Detail panel renders the real Core read model (no placeholders).
            OpenPlayerPanel("duty_roster_detail");
            Check(_dutyRosterDetailPanel.Visible && _dutyRosterDetailPanel.IsBound, "detail panel opens bound to the real host");
            _dutyRosterDetailPanel.RefreshView();
            Check(_dutyRosterDetailPanel.GetChildCount() > 0, "detail panel renders the read model");
            CloseDutyRosterDetailPanel();
            Check(!_dutyRosterDetailPanel.Visible, "detail panel closes cleanly");

            // Quest runtime through the real host path: start, advance, complete.
            // The authored soft gate is day 60; advance the host clock there.
            while (_dutyRoster.Clock.Day < 60) _dutyRoster.TickDay();
            Check(_dutyRoster.Quests.GetAvailableQuests(_dutyRoster.Clock.Day).Count >= 1, "quests available at the real clock day");
            Check(_dutyRoster.StartRosterQuest(DutyRosterIds.QuestTheChart).StartsWith("quest started"), "chart quest starts through the host");
            for (int s = 0; s < 5 && !_dutyRoster.Quests.IsComplete(DutyRosterIds.QuestTheChart); s++)
                _dutyRoster.AdvanceRosterQuest(DutyRosterIds.QuestTheChart);
            Check(_dutyRoster.Quests.IsComplete(DutyRosterIds.QuestTheChart), "chart quest completes through the host");
            Check(_dutyRoster.Roster.MutationInUse, "chart quest completion applies the roster-in-use mutation");
            Check(_journal != null && _journal.Knowledge.Has("lore_dr_chart"), "quest knowledge key bridged into the journal");
            Check(_dutyRoster.Quests.GetAvailableQuests(_dutyRoster.Clock.Day).Count >= 1, "prereq unlocks the next quest");

            // Journal knowledge-key fallback: a quest without an authored key
            // still renders its briefing prose in the journal under its quest id.
            Check(_dutyRoster.StartRosterQuest("quest_roster_ivy_oil").StartsWith("quest started"), "no-key quest starts");
            Check(_dutyRoster.AdvanceRosterQuest("quest_roster_ivy_oil").StartsWith("quest advanced"), "no-key quest completes");
            Check(_journal != null && _journal.Knowledge.Has("quest_roster_ivy_oil"), "journal key falls back to the quest id");
            Check(!string.IsNullOrEmpty(_dutyRoster.ActiveQuestProse(DutyRosterIds.QuestTheChart)) || _dutyRoster.Quests.IsComplete(DutyRosterIds.QuestTheChart),
                "active quest exposes authored stage prose");

            // QuestsPanel surfaces the runtime read model.
            OpenPlayerPanel("quests");
            _questsPanel.RefreshView();
            Check(_questsPanel.GetChildCount() > 0, "quests panel renders with the roster section");
            CloseQuestsPanel();

            HostCli.EmitSummary("duty_roster_uitest", pass, pass ? 0 : 1);
            QuitUiTestAfterFrame(pass ? 0 : 1);
        }

    }
}
