using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core.Combat;
using AtomicWar.GodotApp;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Combat Detail panel (real integration).
    /// Reads the live CombatHostSession snapshot: battle information, current
    /// tactical stance and its trade-offs, casualties &amp; losses, and outcomes
    /// (captured loot, morale/injury consequences reaching real state).
    /// </summary>
    public partial class CombatDetailPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private CombatHostSession _combat = null!;
        private bool _bound;

        private VBoxContainer _contentVBox = null!;
        private VBoxContainer _battleInfo = null!;
        private VBoxContainer _tacticsData = null!;
        private VBoxContainer _casualtyData = null!;
        private VBoxContainer _outcomesData = null!;

        public bool IsBound => _bound;

        public void Bind(CombatHostSession combat)
        {
            _combat = combat;
            _bound = true;
            if (_combat != null) _combat.StateChanged += RefreshView;
            RefreshView();
        }

        public void RefreshView()
        {
            if (_combat == null || _battleInfo == null) return;
            AshfallUiHelpers.EmptyChildren(_battleInfo);
            AshfallUiHelpers.EmptyChildren(_tacticsData);
            AshfallUiHelpers.EmptyChildren(_casualtyData);
            AshfallUiHelpers.EmptyChildren(_outcomesData);
            var snap = _combat.Snapshot();

            AddLine(_battleInfo, "Encounter: " + (string.IsNullOrEmpty(snap.LocationName) ? "—" : snap.LocationName));
            AddLine(_battleInfo, "Phase: " + snap.Phase);
            AddLine(_battleInfo, "Turn: " + snap.Turn + "  ·  Day: " + snap.Day);
            AddLine(_battleInfo, "Stance: " + snap.StanceId);
            AddLine(_battleInfo, "Combatants: " + snap.Combatants.Count);

            // Tactics — the current stance plus its tested trade-offs.
            AddLine(_tacticsData, "Current Stance: " + snap.StanceId);
            var hold = TacticalCombatSystem.GetStanceMods(TacticalStance.HoldPosition);
            var adv = TacticalCombatSystem.GetStanceMods(TacticalStance.Advance);
            var sup = TacticalCombatSystem.GetStanceMods(TacticalStance.SuppressiveFire);
            var ret = TacticalCombatSystem.GetStanceMods(TacticalStance.Retreat);
            var last = TacticalCombatSystem.GetStanceMods(TacticalStance.LastStand);
            AddLine(_tacticsData, $"Hold Position: +{hold.Defense:P0} defense, flee {hold.Mobility:P0}");
            AddLine(_tacticsData, $"Advance: +{adv.Accuracy:P0} acc, {adv.Damage:P0} dmg, -defense, +degradation");
            AddLine(_tacticsData, $"Suppressive Fire: pins enemies, heavy ammo ({sup.AmmoUse:P0}x)");
            AddLine(_tacticsData, $"Retreat: {ret.Mobility:P0} escape, injure risk on a failed break");
            AddLine(_tacticsData, $"Last Stand: ×{last.Accuracy:P0} acc & dmg, no retreat, instant death");

            // Casualties & losses (downed / dead combatants).
            int downed = 0, dead = 0;
            foreach (var c in snap.Combatants)
            {
                if (c.IsDowned) { downed++; AddLine(_casualtyData, c.Name + " — downed, bleeding out", critical: true); }
                if (c.Health <= 0 && !c.IsPlayer) { dead++; AddLine(_casualtyData, c.Name + " — eliminated", critical: true); }
            }
            if (downed == 0 && dead == 0)
                AddLine(_casualtyData, "No casualties recorded.");
            AddLine(_casualtyData, $"Downed: {downed}  ·  Enemy eliminated: {dead}");

            // Outcomes (loot + resolved state).
            if (snap.Resolved)
            {
                AddLine(_outcomesData, "OUTCOME: " + snap.OutcomeText);
                foreach (var l in snap.Loot)
                    AddLine(_outcomesData, $"Loot: +{l.quantity} {l.itemId}");
            }
            else
            {
                AddLine(_outcomesData, "Encounter ongoing — no outcome yet.");
            }
        }

        private static void AddLine(VBoxContainer box, string text, bool critical = false)
        {
            var label = AshfallUiHelpers.MakeBody(text);
            label.CustomMinimumSize = new Vector2(400, 26);
            if (critical)
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Critical));
            box.AddChild(label);
        }

        public override void _Ready()
        {
            // Ticket #125: layout chrome owned by res://assets/ui/panels/CombatDetailPanel.tscn; SceneBinder resolves typed unique-name nodes once.
            // Sibling refresh code is unchanged.
            var binder = new SceneBinder(this, typeof(CombatDetailPanel));
            binder.Require<VBoxContainer>("BattleInfo");
            binder.Require<VBoxContainer>("TacticsData");
            binder.Require<VBoxContainer>("CasualtyData");
            binder.Require<VBoxContainer>("OutcomesData");
            binder.Require<Button>("CloseButton");
            _battleInfo = binder.Get<VBoxContainer>("BattleInfo");
            _tacticsData = binder.Get<VBoxContainer>("TacticsData");
            _casualtyData = binder.Get<VBoxContainer>("CasualtyData");
            _outcomesData = binder.Get<VBoxContainer>("OutcomesData");
            binder.Get<Button>("CloseButton").Pressed += () => OnClose?.Invoke();

            Visible = false;
        }

        private static VBoxContainer MakeBox()
        {
            var box = new VBoxContainer();
            box.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            box.CustomMinimumSize = new Vector2(600, 0);
            return box;
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
