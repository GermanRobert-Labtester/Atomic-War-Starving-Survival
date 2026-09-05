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
    /// Uses standard AshfallDataGrid with explicit MinWidth floors for lane-2 fit.
    /// </summary>
    public partial class CombatPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private CombatHostSession _combat = null!;
        private bool _bound;

        private Label _header = null!;
        private Label _summary = null!;
        private AshfallDataGrid _combatantsGrid = null!;
        private AshfallDataGrid _weaponsGrid = null!;
        private RichTextLabel _log = null!;
        private Label _outcome = null!;
        private OptionButton _targetSelect = null!;
        private Label _lastActionResult = null!;

        // Tactical action buttons
        private Button _btnFire = null!;
        private Button _btnSuppress = null!;
        private Button _btnTrap = null!;
        private Button _btnClearJam = null!;
        private Button _btnRepair = null!;
        private Button _btnDecon = null!;
        private Button _btnLastStand = null!;
        private Button _btnHold = null!;
        private Button _btnAdvance = null!;
        private Button _btnSuppressStance = null!;
        private Button _btnRetreat = null!;
        private Button _btnEndTurn = null!;

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

            // Rebuild target selector from living enemies first so action evaluation uses current target
            RebuildTargetList(snap);

            // Action button preflight & honest reason tooltips
            UpdateButtonPreflights(snap);

            // Combatants Grid (AshfallDataGrid)
            var combatantRows = new List<AshfallDataGrid.Row>();
            foreach (var c in snap.Combatants)
            {
                var hpState = c.IsDowned ? AshfallDataGrid.CellState.Critical
                    : (c.Health < c.MaxHealth * 0.4f ? AshfallDataGrid.CellState.Warning : AshfallDataGrid.CellState.Normal);

                var statusState = (c.IsDowned || c.IsLastStand) ? AshfallDataGrid.CellState.Critical
                    : (c.IsPinned ? AshfallDataGrid.CellState.Warning : AshfallDataGrid.CellState.Normal);

                var weaponState = c.WeaponJammed ? AshfallDataGrid.CellState.Critical : AshfallDataGrid.CellState.Normal;
                string weaponDisplay = $"{c.WeaponName} ({c.WeaponConditionPct}%)" + (c.WeaponJammed ? " [JAM]" : "");

                var cells = new List<AshfallDataGrid.Cell>
                {
                    new(c.IsPlayer ? "▶" : "●", c.IsPlayer ? AshfallDataGrid.CellState.Positive : AshfallDataGrid.CellState.Critical),
                    new(c.Name, AshfallDataGrid.CellState.Normal),
                    new(c.Lane, AshfallDataGrid.CellState.Normal),
                    new($"{c.Health}/{c.MaxHealth}", hpState),
                    new($"{c.CoverRating}%", AshfallDataGrid.CellState.Normal),
                    new($"{c.ArmorRating}%", AshfallDataGrid.CellState.Normal),
                    new(c.Status, statusState),
                    new(weaponDisplay, weaponState)
                };
                combatantRows.Add(new AshfallDataGrid.Row { Cells = cells, Selectable = true });
            }
            _combatantsGrid.SetRows(combatantRows);

            // Weapons Grid (AshfallDataGrid armory monitor)
            var weaponRows = new List<AshfallDataGrid.Row>();
            foreach (var w in snap.Weapons)
            {
                var condState = w.ConditionPct < 30 ? AshfallDataGrid.CellState.Critical
                    : (w.ConditionPct < 60 ? AshfallDataGrid.CellState.Warning : AshfallDataGrid.CellState.Normal);

                var jamState = w.JamChancePct > 20 ? AshfallDataGrid.CellState.Warning : AshfallDataGrid.CellState.Normal;
                var statusState = w.IsJammed ? AshfallDataGrid.CellState.Critical : AshfallDataGrid.CellState.Positive;
                var ammoState = w.AmmoRemaining <= 5 ? AshfallDataGrid.CellState.Warning : AshfallDataGrid.CellState.Normal;

                var cells = new List<AshfallDataGrid.Cell>
                {
                    new(w.WeaponName, AshfallDataGrid.CellState.Normal),
                    new($"{w.ConditionPct}%", condState),
                    new($"{w.JamChancePct}%", jamState),
                    new(w.IsJammed ? "✖ JAMMED" : "✔ READY", statusState),
                    new($"{w.ScrapRepairCost} scrap", AshfallDataGrid.CellState.Normal),
                    new(w.AmmoRemaining.ToString(), ammoState)
                };
                weaponRows.Add(new AshfallDataGrid.Row { Cells = cells, Selectable = true });
            }
            _weaponsGrid.SetRows(weaponRows);

            // Combat log
            _log.Clear();
            int start = Math.Max(0, snap.Events.Count - 40);
            for (int i = start; i < snap.Events.Count; i++)
                _log.AppendText($"[T{snap.Events[i].Turn}] {snap.Events[i].Detail}\n");

            // Outcome + loot
            _outcome.Text = snap.Resolved
                ? "OUTCOME: " + snap.OutcomeText + (snap.Loot.Count > 0 ? "  ·  loot " + snap.Loot.Count + " lines" : "")
                : "";
        }

        private void UpdateButtonPreflights(CombatSnapshot snap)
        {
            if (_combat == null) return;

            bool active = snap.IsActive;

            var firePf = _combat.EvaluateFire(SelectedTargetId());
            _btnFire.Disabled = !firePf.CanExecute;
            _btnFire.TooltipText = firePf.CanExecute ? "Fire equipped weapon at selected hostile target [1]" : firePf.Reason;

            var suppPf = _combat.EvaluateSuppress();
            _btnSuppress.Disabled = !suppPf.CanExecute;
            _btnSuppress.TooltipText = suppPf.CanExecute ? "Lay area suppressive fire across all hostile lanes [2]" : suppPf.Reason;

            _btnTrap.Disabled = !active;
            _btnTrap.TooltipText = active ? "Deploy an obstacle trap in active lanes" : "Encounter not active";

            var jamPf = _combat.EvaluateClearJam("survivor_yuki");
            _btnClearJam.Disabled = !jamPf.CanExecute;
            _btnClearJam.TooltipText = jamPf.CanExecute ? "Clear jammed weapon action [3]" : jamPf.Reason;

            var repPf = _combat.EvaluateRepair("survivor_yuki");
            _btnRepair.Disabled = !repPf.CanExecute;
            _btnRepair.TooltipText = repPf.CanExecute ? "Field repair weapon condition with scrap [4]" : repPf.Reason;

            _btnDecon.Disabled = !active;
            _btnDecon.TooltipText = active ? "Decontaminate weapon action from ash fouling" : "Encounter not active";

            _btnLastStand.Disabled = !active;
            _btnLastStand.TooltipText = active ? "Enter terminal Last Stand stance (drastic offense, no retreat)" : "Encounter not active";

            _btnHold.Disabled = !active;
            _btnAdvance.Disabled = !active;
            _btnSuppressStance.Disabled = !active;

            var retPf = _combat.EvaluateRetreat();
            _btnRetreat.Disabled = !retPf.CanExecute;
            _btnRetreat.TooltipText = retPf.CanExecute ? "Attempt tactical disengagement and retreat" : retPf.Reason;

            var endPf = _combat.EvaluateEndTurn();
            _btnEndTurn.Disabled = !endPf.CanExecute;
            _btnEndTurn.TooltipText = endPf.CanExecute ? "End turn and permit enemy actions [5]" : endPf.Reason;
        }

        private void RebuildTargetList(CombatSnapshot snap)
        {
            string previousTarget = SelectedTargetId();
            _targetSelect.Clear();
            int idx = 0;
            int restoredIdx = -1;
            foreach (var c in snap.Combatants)
            {
                if (c.IsPlayer || c.IsDowned) continue;
                _targetSelect.AddItem(c.Name + "  [" + c.Lane + "]  HP " + c.Health, idx);
                _targetSelect.SetItemMetadata(idx, c.Id);
                if (!string.IsNullOrEmpty(previousTarget) && string.Equals(c.Id, previousTarget, StringComparison.Ordinal))
                    restoredIdx = idx;
                idx++;
            }
            if (idx == 0)
            {
                _targetSelect.AddItem("(no living targets)", 0);
                _targetSelect.SetItemMetadata(0, string.Empty);
            }
            else if (restoredIdx >= 0)
            {
                _targetSelect.Select(restoredIdx);
            }
        }

        private string SelectedTargetId()
        {
            int i = _targetSelect.Selected;
            if (i < 0 || _targetSelect.ItemCount == 0) return string.Empty;
            var meta = _targetSelect.GetItemMetadata(i);
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
            var startBtn = Btn("ENGAGE ENCOUNTER", () => DoAction(() => _combat.StartCombat("loc_denial_cut", "The Denial Cut")));
            startRow.AddChild(startBtn);
            _targetSelect = new OptionButton();
            _targetSelect.CustomMinimumSize = new Vector2(240, 36);
            startRow.AddChild(_targetSelect);
            vbox.AddChild(startRow);

            // Fire / suppression / tactical
            var row1 = Row();
            _btnFire = Btn("FIRE [1]", () => DoAction(() => _combat.ActionFire(SelectedTargetId())));
            row1.AddChild(_btnFire);
            _btnSuppress = Btn("SUPPRESS [2]", () => DoAction(_combat.ActionSuppress));
            row1.AddChild(_btnSuppress);
            _btnTrap = Btn("DEPLOY TRAP", () => DoAction(_combat.ActionDeployTrap));
            row1.AddChild(_btnTrap);
            vbox.AddChild(row1);

            var row2 = Row();
            _btnClearJam = Btn("CLEAR JAM [3]", () => DoAction(() => _combat.ActionClearJam("survivor_yuki")));
            row2.AddChild(_btnClearJam);
            _btnRepair = Btn("REPAIR [4]", () => DoAction(() => _combat.ActionRepair("survivor_yuki").MessageKey));
            row2.AddChild(_btnRepair);
            _btnDecon = Btn("DECON FLUSH", () => DoAction(_combat.ActionDecontaminate));
            row2.AddChild(_btnDecon);
            _btnLastStand = Btn("LAST STAND", () => DoAction(() => _combat.ActionLastStand("survivor_yuki")));
            row2.AddChild(_btnLastStand);
            vbox.AddChild(row2);

            var row3 = Row();
            _btnHold = Btn("HOLD LINE", () => DoAction(() => _combat.ActionStance(TacticalCombatSystem.StanceId(TacticalStance.HoldPosition))));
            row3.AddChild(_btnHold);
            _btnAdvance = Btn("ADVANCE", () => DoAction(() => _combat.ActionStance(TacticalCombatSystem.StanceId(TacticalStance.Advance))));
            row3.AddChild(_btnAdvance);
            _btnSuppressStance = Btn("SUPPRESS STANCE", () => DoAction(() => _combat.ActionStance(TacticalCombatSystem.StanceId(TacticalStance.SuppressiveFire))));
            row3.AddChild(_btnSuppressStance);
            _btnRetreat = Btn("RETREAT", () => DoAction(_combat.ActionRetreat));
            row3.AddChild(_btnRetreat);
            _btnEndTurn = Btn("END TURN [5]", () => DoAction(_combat.ActionEndTurn));
            row3.AddChild(_btnEndTurn);
            vbox.AddChild(row3);

            _lastActionResult = AshfallUiHelpers.MakeBody("");
            _lastActionResult.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
            vbox.AddChild(_lastActionResult);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Combatants Grid (AshfallDataGrid)
            vbox.AddChild(AshfallUiHelpers.MakeSectionHeader("COMBATANTS"));
            var combatantCols = new List<AshfallDataGrid.Column>
            {
                new() { Header = "Mark", MinWidth = 40, Alignment = AshfallDataGrid.ColumnAlign.Center },
                new() { Header = "Combatant", MinWidth = 160, Alignment = AshfallDataGrid.ColumnAlign.Left },
                new() { Header = "Lane", MinWidth = 70, Alignment = AshfallDataGrid.ColumnAlign.Center },
                new() { Header = "Health", MinWidth = 90, Alignment = AshfallDataGrid.ColumnAlign.Right },
                new() { Header = "Cover", MinWidth = 70, Alignment = AshfallDataGrid.ColumnAlign.Right },
                new() { Header = "Armor", MinWidth = 70, Alignment = AshfallDataGrid.ColumnAlign.Right },
                new() { Header = "Status", MinWidth = 130, Alignment = AshfallDataGrid.ColumnAlign.Left },
                new() { Header = "Weapon / Cond", MinWidth = 190, Alignment = AshfallDataGrid.ColumnAlign.Left }
            };
            _combatantsGrid = new AshfallDataGrid(combatantCols, showHeader: true, minWidth: 820, minHeight: 140);
            vbox.AddChild(_combatantsGrid);

            // Armory / Weapon Monitor Grid (AshfallDataGrid)
            vbox.AddChild(AshfallUiHelpers.MakeSectionHeader("ARMORY / WEAPON MONITOR"));
            var weaponCols = new List<AshfallDataGrid.Column>
            {
                new() { Header = "Weapon", MinWidth = 180, Alignment = AshfallDataGrid.ColumnAlign.Left },
                new() { Header = "Condition", MinWidth = 90, Alignment = AshfallDataGrid.ColumnAlign.Right },
                new() { Header = "Jam Risk", MinWidth = 80, Alignment = AshfallDataGrid.ColumnAlign.Right },
                new() { Header = "Status", MinWidth = 110, Alignment = AshfallDataGrid.ColumnAlign.Center },
                new() { Header = "Scrap Repair", MinWidth = 100, Alignment = AshfallDataGrid.ColumnAlign.Right },
                new() { Header = "Ammo", MinWidth = 80, Alignment = AshfallDataGrid.ColumnAlign.Right }
            };
            _weaponsGrid = new AshfallDataGrid(weaponCols, showHeader: true, minWidth: 820, minHeight: 100);
            vbox.AddChild(_weaponsGrid);

            vbox.AddChild(AshfallUiHelpers.MakeSectionHeader("COMBAT LOG"));
            _log = new RichTextLabel { BbcodeEnabled = false, ScrollActive = true };
            _log.CustomMinimumSize = new Vector2(820, 150);
            vbox.AddChild(_log);

            _outcome = AshfallUiHelpers.MakeBody("");
            _outcome.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Critical));
            vbox.AddChild(_outcome);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            var btnClose = AshfallUiHelpers.MakeButton("CLOSE [Esc]", () => OnClose?.Invoke());
            btnClose.CustomMinimumSize = new Vector2(200, 40);
            vbox.AddChild(btnClose);

            var hint = AshfallUiHelpers.MakeSmall("[Tab] Cycle Target · [1-5] Tactical Actions · [Esc] Close · Resolves via deterministic Core engine");
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
            if (@event is InputEventKey key && key.Pressed)
            {
                if (key.Keycode == Key.Escape)
                {
                    OnClose?.Invoke();
                    GetViewport().SetInputAsHandled();
                }
                else if (key.Keycode == Key.Tab)
                {
                    if (_targetSelect.ItemCount > 0)
                    {
                        int next = (_targetSelect.Selected + 1) % _targetSelect.ItemCount;
                        _targetSelect.Select(next);
                        RefreshView();
                        GetViewport().SetInputAsHandled();
                    }
                }
                else if (key.Keycode == Key.Key1 || key.Keycode == Key.Kp1)
                {
                    if (!_btnFire.Disabled)
                    {
                        DoAction(() => _combat.ActionFire(SelectedTargetId()));
                        GetViewport().SetInputAsHandled();
                    }
                }
                else if (key.Keycode == Key.Key2 || key.Keycode == Key.Kp2)
                {
                    if (!_btnSuppress.Disabled)
                    {
                        DoAction(_combat.ActionSuppress);
                        GetViewport().SetInputAsHandled();
                    }
                }
                else if (key.Keycode == Key.Key3 || key.Keycode == Key.Kp3)
                {
                    if (!_btnClearJam.Disabled)
                    {
                        DoAction(() => _combat.ActionClearJam("survivor_yuki"));
                        GetViewport().SetInputAsHandled();
                    }
                }
                else if (key.Keycode == Key.Key4 || key.Keycode == Key.Kp4)
                {
                    if (!_btnRepair.Disabled)
                    {
                        DoAction(() => _combat.ActionRepair("survivor_yuki").MessageKey);
                        GetViewport().SetInputAsHandled();
                    }
                }
                else if (key.Keycode == Key.Key5 || key.Keycode == Key.Kp5)
                {
                    if (!_btnEndTurn.Disabled)
                    {
                        DoAction(_combat.ActionEndTurn);
                        GetViewport().SetInputAsHandled();
                    }
                }
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
