using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core.Combat;
using AtomicWar.GodotApp;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Combat History panel (real integration).
    /// Presents the full combat event log, aggregated battle outcomes (win/loss/
    /// retreat), and tactical analysis from the live CombatHostSession snapshot.
    /// </summary>
    public partial class CombatHistoryPanel : Control
    {
        public event Action? OnClose;

        private CombatHostSession _combat = null!;
        private bool _bound;

        private VBoxContainer _combatHistory = null!;
        private VBoxContainer _battleOutcomes = null!;
        private VBoxContainer _tacticalAnalysis = null!;

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
            if (_combatHistory == null || _battleOutcomes == null || _tacticalAnalysis == null) return;
            AshfallUiHelpers.EmptyChildren(_combatHistory);
            AshfallUiHelpers.EmptyChildren(_battleOutcomes);
            AshfallUiHelpers.EmptyChildren(_tacticalAnalysis);

            if (_combat == null)
            {
                _combatHistory.AddChild(AshfallUiHelpers.MakeEmptyStateLabel("No combat session bound", "offline"));
                _battleOutcomes.AddChild(AshfallUiHelpers.MakeEmptyStateLabel("No active combat outcomes", "offline"));
                _tacticalAnalysis.AddChild(AshfallUiHelpers.MakeEmptyStateLabel("Tactical weapon telemetry unavailable", "offline"));
                return;
            }

            var snap = _combat.Snapshot();

            // Battle log (real combat history).
            if (snap.Events == null || snap.Events.Count == 0)
            {
                _combatHistory.AddChild(AshfallUiHelpers.MakeEmptyStateLabel("No combat events logged in current encounter"));
            }
            else
            {
                foreach (var e in snap.Events)
                    AddLine(_combatHistory, $"[T{e.Turn}] {e.Detail}");
            }

            // Aggregate outcomes.
            int fireEvents = 0, jams = 0, downs = 0, deaths = 0, retreats = 0, winsOrLosses = 0;
            if (snap.Events != null)
            {
                foreach (var e in snap.Events)
                {
                    switch (e.Kind)
                    {
                        case "fire": fireEvents++; break;
                        case "weapon_jam": jams++; break;
                        case "downed": downs++; break;
                        case "death": deaths++; break;
                        case "retreat": retreats++; break;
                        case "victory": case "defeat": winsOrLosses++; break;
                    }
                }
            }
            AddLine(_battleOutcomes, "Shots fired: " + fireEvents);
            AddLine(_battleOutcomes, "Weapon jams: " + jams);
            AddLine(_battleOutcomes, "Combatants downed: " + downs);
            AddLine(_battleOutcomes, "Eliminations: " + deaths);
            AddLine(_battleOutcomes, "Retreats ordered: " + retreats);
            AddLine(_battleOutcomes, "Resolutions (victory/defeat): " + winsOrLosses);
            AddLine(_battleOutcomes, snap.Resolved ? "Final: " + snap.OutcomeText : "Encounter ongoing.");

            // Tactical analysis (stance trade-offs currently in force).
            if (snap.Weapons == null || snap.Weapons.Count == 0)
            {
                _tacticalAnalysis.AddChild(AshfallUiHelpers.MakeEmptyStateLabel("No weapons monitored in squad loadout"));
            }
            else
            {
                AddLine(_tacticalAnalysis, "Weapons monitored: " + snap.Weapons.Count);
                foreach (var w in snap.Weapons)
                    AddLine(_tacticalAnalysis, $"{w.WeaponName} — cond {w.ConditionPct}%, jam {w.JamChancePct}%, ammo {w.AmmoRemaining}");
            }

            if (snap.Loot != null && snap.Loot.Count > 0)
            {
                foreach (var l in snap.Loot)
                    AddLine(_tacticalAnalysis, $"Loot captured: +{l.quantity} {l.itemId}");
            }
        }

        private static void AddLine(VBoxContainer box, string text)
        {
            var label = AshfallUiHelpers.MakeBody(text);
            label.CustomMinimumSize = new Vector2(420, 26);
            box.AddChild(label);
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            var bg = new ColorRect { Color = new Color(0.05f, 0.05f, 0.05f, 0.92f) };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            var scroll = new ScrollContainer();
            scroll.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(scroll);

            var vbox = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingLg);
            vbox.CustomMinimumSize = new Vector2(680, 0);
            scroll.AddChild(vbox);

            var title = AshfallUiHelpers.MakeTitle("COMBAT HISTORY", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);
            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            vbox.AddChild(AshfallUiHelpers.MakeSectionHeader("BATTLE LOG"));
            _combatHistory = MakeBox(); vbox.AddChild(_combatHistory);
            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            vbox.AddChild(AshfallUiHelpers.MakeSectionHeader("BATTLE OUTCOMES"));
            _battleOutcomes = MakeBox(); vbox.AddChild(_battleOutcomes);
            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            vbox.AddChild(AshfallUiHelpers.MakeSectionHeader("TACTICAL ANALYSIS"));
            _tacticalAnalysis = MakeBox(); vbox.AddChild(_tacticalAnalysis);
            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            var btnClose = AshfallUiHelpers.MakeButton("CLOSE [Esc]", () => OnClose?.Invoke());
            btnClose.CustomMinimumSize = new Vector2(200, 40);
            vbox.AddChild(btnClose);
            vbox.AddChild(AshfallUiHelpers.MakeSmall("[Esc] to close"));

            RefreshView();
        }

        private static VBoxContainer MakeBox()
        {
            var box = new VBoxContainer();
            box.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            box.CustomMinimumSize = new Vector2(640, 0);
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

        public override void _ExitTree()
        {
            if (_combat != null)
            {
                _combat.StateChanged -= RefreshView;
            }
            base._ExitTree();
        }
    }
}
