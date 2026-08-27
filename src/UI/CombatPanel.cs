using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core.Combat;
using AtomicWar.GodotApp;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Combat &amp; Encounters panel (real integration).
    ///
    /// Presents the live CombatHostSession: active encounter, phase, turn, stance,
    /// combatants (lane / health / cover / armor / downed / pinned / last-stand),
    /// weapon condition, jam state and ammunition, combat log and captured loot.
    /// Exposes real player actions (fire, suppress, clear jam, repair, move lane,
    /// deploy trap, decontaminate, bandage, retreat, last stand, end turn) — the
    /// same commands the Core engine resolves. UI refreshes after every action.
    /// </summary>
    public partial class CombatPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private CombatHostSession _combat = null!;
        private bool _bound;

        private Label _header = null!;
        private Label _summary = null!;
        private RichTextLabel _combatants = null!;
        private RichTextLabel _weapons = null!;
        private RichTextLabel _log = null!;
        private Label _outcome = null!;
        private OptionButton _targetSelect = null!;
        private Label _lastActionResult = null!;

        public bool IsBound => _bound;

        /// <summary>Typed binding to the real combat host session.</summary>
        public void Bind(CombatHostSession combat)
        {
            _combat = combat;
            _bound = true;
            if (_combat != null)
                _combat.StateChanged += RefreshView;
            RefreshView();
        }

        public void RefreshView()
        {
            if (_combat == null || _header == null) return;

            var snap = _combat.Snapshot();

            _header.Text = "COMBAT & ENCOUNTERS  ·  " + (string.IsNullOrEmpty(snap.LocationName) ? "NO ACTIVE" : snap.LocationName);
            if (snap.IsActive)
            {
                _summary.Text = $"Phase: {snap.Phase}  ·  Turn: {snap.Turn}  ·  Day: {snap.Day}  ·  Stance: {snap.StanceId}";
            }
            else if (snap.Resolved)
            {
                _summary.Text = $"RESOLVED — {snap.OutcomeText}";
            }
            else
            {
                _summary.Text = "No active combat. Use START to engage a raider skirmish.";
            }

            // Combatants
            _combatants.Clear();
            foreach (var c in snap.Combatants)
            {
                string mark = c.IsPlayer ? "▶" : "●";
                string line = $"{mark}  {c.Name}  [{c.Lane}]  HP {c.Health}/{c.MaxHealth}  cover {c.CoverRating}%  armor {c.ArmorRating}%";
                line += c.IsDowned ? "  ✖ DOWNED" : (c.IsPinned ? "  ⊘ PINNED" : "");
                line += c.IsLastStand ? "  ☠ LAST STAND" : "";
                line += $"  · {c.WeaponName} {c.WeaponConditionPct}%" + (c.WeaponJammed ? " [JAM]" : "");
                _combatants.AppendText(line + "\n");
            }

            // Weapons (armory monitor — real condition/jam)
            _weapons.Clear();
            if (snap.Weapons.Count == 0)
            {
                _weapons.AppendText("No firearms registered.\n");
            }
            foreach (var w in snap.Weapons)
            {
                _weapons.AppendText(
                    $"• {w.WeaponName} — cond {w.ConditionPct}% · jam {w.JamChancePct}% · " +
                    (w.IsJammed ? "✖ JAMMED" : "✔ functional") +
                    " · scrap repair " + w.ScrapRepairCost + " · ammo " + w.AmmoRemaining + "\n");
            }

            // Combat log
            _log.Clear();
            int start = Math.Max(0, snap.Events.Count - 40);
            for (int i = start; i < snap.Events.Count; i++)
                _log.AppendText($"[T{snap.Events[i].Turn}] {snap.Events[i].Detail}\n");

            // Outcome + loot
            _outcome.Text = snap.Resolved
                ? "OUTCOME: " + snap.OutcomeText + (snap.Loot.Count > 0 ? "  ·  loot " + snap.Loot.Count + " lines" : "")
                : "";

            // Rebuild target selector from living enemies
            RebuildTargetList(snap);
        }

        private void RebuildTargetList(CombatSnapshot snap)
        {
            _targetSelect.Clear();
            int idx = 0;
            foreach (var c in snap.Combatants)
            {
                if (c.IsPlayer || c.IsDowned) continue;
                _targetSelect.AddItem(c.Name + "  [" + c.Lane + "]  HP " + c.Health, idx++);
                _targetSelect.SetItemMetadata(idx - 1, c.Id);
            }
            if (idx == 0)
                _targetSelect.AddItem("(no living targets)", 0);
        }

        private string SelectedTargetId()
        {
            int i = _targetSelect.Selected;
            var meta = _targetSelect.GetItemMetadata(Math.Max(0, i));
            return meta.AsString();
        }

        private void DoAction(Func<string> act)
        {
            if (_combat == null) return;
            _lastActionResult.Text = act();
            RefreshView();
        }

        private HBoxContainer Row()
        {
            var row = AshfallUiHelpers.MakeHBox(separation: 8);
            return row;
        }

        private Button Btn(string text, Action onClick)
        {
            var b = AshfallUiHelpers.MakeButton(text, onClick);
            b.CustomMinimumSize = new Vector2(150, 36);
            return b;
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            var bg = new ColorRect { Color = new Color(0.05f, 0.05f, 0.05f, 0.94f) };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            var scroll = new ScrollContainer();
            scroll.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(scroll);

            var vbox = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingMd);
            vbox.CustomMinimumSize = new Vector2(860, 0);
            vbox.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            scroll.AddChild(vbox);

            _header = AshfallUiHelpers.MakeTitle("COMBAT & ENCOUNTERS", Ashfall.Core.UI.Theme.FontSizeH1);
            _header.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(_header);

            _summary = AshfallUiHelpers.MakeBody("");
            _summary.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
            vbox.AddChild(_summary);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Target selection + start
            var startRow = Row();
            var startBtn = Btn("START RAID", () => DoAction(() => _combat.StartDemoCombat("loc_denial_cut", "The Denial Cut")));
            startRow.AddChild(startBtn);
            _targetSelect = new OptionButton();
            _targetSelect.CustomMinimumSize = new Vector2(240, 36);
            startRow.AddChild(_targetSelect);
            vbox.AddChild(startRow);

            // Fire / suppression / tactical
            var row1 = Row();
            row1.AddChild(Btn("FIRE", () => DoAction(() => _combat.ActionFire(SelectedTargetId()))));
            row1.AddChild(Btn("SUPPRESS", () => DoAction(_combat.ActionSuppress)));
            row1.AddChild(Btn("DEPLOY TRAP", () => DoAction(_combat.ActionDeployTrap)));
            vbox.AddChild(row1);

            var row2 = Row();
            row2.AddChild(Btn("CLEAR JAM", () => DoAction(() => _combat.ActionClearJam("survivor_yuki"))));
            row2.AddChild(Btn("REPAIR", () => DoAction(() => _combat.ActionRepair("survivor_yuki"))));
            row2.AddChild(Btn("DECON FLUSH", () => DoAction(_combat.ActionDecontaminate)));
            row2.AddChild(Btn("LAST STAND", () => DoAction(() => _combat.ActionLastStand("survivor_yuki"))));
            vbox.AddChild(row2);

            var row3 = Row();
            row3.AddChild(Btn("HOLD LINE", () => DoAction(() => _combat.ActionStance(TacticalCombatSystem.StanceId(TacticalStance.HoldPosition)))));
            row3.AddChild(Btn("ADVANCE", () => DoAction(() => _combat.ActionStance(TacticalCombatSystem.StanceId(TacticalStance.Advance)))));
            row3.AddChild(Btn("SUPPRESS STANCE", () => DoAction(() => _combat.ActionStance(TacticalCombatSystem.StanceId(TacticalStance.SuppressiveFire)))));
            row3.AddChild(Btn("RETREAT", () => DoAction(_combat.ActionRetreat)));
            row3.AddChild(Btn("END TURN", () => DoAction(_combat.ActionEndTurn)));
            vbox.AddChild(row3);

            _lastActionResult = AshfallUiHelpers.MakeBody("");
            _lastActionResult.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
            vbox.AddChild(_lastActionResult);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            vbox.AddChild(AshfallUiHelpers.MakeSectionHeader("COMBATANTS"));
            _combatants = new RichTextLabel { BbcodeEnabled = false, ScrollActive = true };
            _combatants.CustomMinimumSize = new Vector2(820, 130);
            vbox.AddChild(_combatants);

            vbox.AddChild(AshfallUiHelpers.MakeSectionHeader("ARMORY / WEAPON MONITOR"));
            _weapons = new RichTextLabel { BbcodeEnabled = false, ScrollActive = true };
            _weapons.CustomMinimumSize = new Vector2(820, 90);
            vbox.AddChild(_weapons);

            vbox.AddChild(AshfallUiHelpers.MakeSectionHeader("COMBAT LOG"));
            _log = new RichTextLabel { BbcodeEnabled = false, ScrollActive = true };
            _log.CustomMinimumSize = new Vector2(820, 170);
            vbox.AddChild(_log);

            _outcome = AshfallUiHelpers.MakeBody("");
            _outcome.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Critical));
            vbox.AddChild(_outcome);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            var btnClose = AshfallUiHelpers.MakeButton("CLOSE [Esc]", () => OnClose?.Invoke());
            btnClose.CustomMinimumSize = new Vector2(200, 40);
            vbox.AddChild(btnClose);

            var hint = AshfallUiHelpers.MakeSmall("[Esc] to close  ·  every action resolves through the deterministic Core engine");
            vbox.AddChild(hint);

            RefreshView();
        }

        public void Open()
        {
            Visible = true;
            RefreshView();
            QueueRedraw();
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (!Visible) return;
            if (@event is InputEventKey key && key.Pressed && key.Keycode == Key.Escape)
            {
                OnClose?.Invoke();
                GetViewport().SetInputAsHandled();
            }
        }


    public void Unbind()
    {
        if (_combat != null)
            {
                _combat.StateChanged -= RefreshView;
            }
    }

    public override void _ExitTree()
        {
            Unbind();
            base._ExitTree();
        }
    }
}
