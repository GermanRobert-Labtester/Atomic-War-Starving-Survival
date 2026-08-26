using System;
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Phase 0 panel (psychological &amp; medical effects).
    /// Shared survivor-condition surface for the ten Phase-0 systems: radiation
    /// phase progression, phantom memory, guilt insomnia, combat trauma, somatic
    /// flashback, moral branching, chemical dependency, trade specialty, final
    /// wish, and respiratory degeneration. Reads Core/session state and calls
    /// existing host commands only — no eligibility, tick, resource, medical,
    /// morale, or narrative rules live here.
    /// </summary>
    public partial class Phase0Panel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _conditionList = null!;
        private VBoxContainer _commandList = null!;

        private Phase0HostSession? _phase0;
        private SurvivorsHostSession? _survivors;

        public bool IsBound => _phase0 != null;
        public int RenderedConditionCount => _conditionList?.GetChildCount() ?? 0;

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            var bg = new ColorRect { Color = new Color(0.04f, 0.05f, 0.06f, 0.88f) };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            var center = new CenterContainer();
            center.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(center);

            var panel = AshfallUiHelpers.MakePanel(760, 640);
            center.AddChild(panel);

            var margins = AshfallUiHelpers.MakeMargins(Ashfall.Core.UI.Theme.SpacingMd);
            panel.AddChild(margins);

            var vbox = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingMd);
            margins.AddChild(vbox);

            var header = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
            var title = AshfallUiHelpers.MakeTitle(
                "PHASE-0 // PSYCHOLOGICAL & MEDICAL CONDITIONS", Ashfall.Core.UI.Theme.FontSizeH2);
            title.HorizontalAlignment = HorizontalAlignment.Left;
            title.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            header.AddChild(title);
            var btnClose = AshfallUiHelpers.MakeButton("CLOSE [Esc]", () => OnClose?.Invoke());
            btnClose.CustomMinimumSize = new Vector2(110, 32);
            header.AddChild(btnClose);
            vbox.AddChild(header);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            var scroll = new ScrollContainer
            {
                CustomMinimumSize = new Vector2(720, 500),
                SizeFlagsVertical = Control.SizeFlags.ExpandFill
            };
            vbox.AddChild(scroll);

            var contentBox = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingMd);
            contentBox.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            scroll.AddChild(contentBox);

            contentBox.AddChild(AshfallUiHelpers.MakeSectionHeader("SURVIVOR CONDITIONS"));
            _conditionList = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingXs);
            _conditionList.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            contentBox.AddChild(_conditionList);

            contentBox.AddChild(AshfallUiHelpers.MakeSeparator());

            contentBox.AddChild(AshfallUiHelpers.MakeSectionHeader("TREATMENTS & RECORD"));
            _commandList = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingXs);
            _commandList.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            contentBox.AddChild(_commandList);
        }

        public void Bind(Phase0HostSession phase0, SurvivorsHostSession? survivors = null)
        {
            if (_phase0 != null)
                _phase0.StateChanged -= OnPhase0StateChanged;
            _phase0 = phase0;
            if (_phase0 != null)
                _phase0.StateChanged += OnPhase0StateChanged;
            _survivors = survivors;
            RefreshView();
        }

        private void OnPhase0StateChanged() => RefreshView();

        public void RefreshView()
        {
            if (_conditionList == null || _commandList == null) return;

            AshfallUiHelpers.EmptyChildren(_conditionList);
            AshfallUiHelpers.EmptyChildren(_commandList);

            if (_phase0 == null)
            {
                _conditionList.AddChild(AshfallUiHelpers.MakeMetadata("No Phase-0 session bound."));
                return;
            }

            _conditionList.AddChild(AshfallUiHelpers.MakeMetadata(
                $"Permanent shelter morale: +{_phase0.PermanentShelterMoraleBuff:0}"));

            var roster = _survivors != null ? _survivors.RosterState : null;
            int shown = 0;
            for (int i = 0; i < (_phase0.Effects?.Count ?? 0); i++)
            {
                var fx = _phase0.Effects![i];
                if (fx == null || string.IsNullOrEmpty(fx.survivorId)) continue;
                bool alive = roster == null || roster.Exists(s => s != null && s.Id == fx.survivorId && s.IsAliveState);
                if (!alive) continue;

                var card = BuildSurvivorCard(fx);
                _conditionList.AddChild(card);
                shown++;
                if (shown >= 40) break;
            }

            if (shown == 0)
                _conditionList.AddChild(AshfallUiHelpers.MakeMetadata("No survivors with Phase-0 conditions."));

            BuildCommands();
        }

        private Control BuildSurvivorCard(Phase0SurvivorEffects fx)
        {
            var card = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingXs);
            card.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;

            var nameRow = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
            var name = AshfallUiHelpers.MakeSmall(FormatName(fx.survivorId));
            name.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            nameRow.AddChild(name);

            var radColor = fx.radiationPhase == "ManifestIllness" || fx.radiationPhase == "ChronicFibrosis"
                ? AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Critical)
                : fx.radiationPhase == "Prodromal"
                    ? AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Hot)
                    : AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Lethe);
            nameRow.AddChild(AshfallUiHelpers.MakeMono($"RAD {fx.radiationPhase}").WithColor(radColor));
            card.AddChild(nameRow);

            // ── Modifier rows with their gameplay source ──────────────
            card.AddChild(AshfallUiHelpers.MakeDataRow("WORK", $"×{fx.workEfficiencyMultiplier:F2}",
                fx.workEfficiencyMultiplier < 1f ? AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Critical) : AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
            if (fx.workRefusalHours > 0f)
                card.AddChild(AshfallUiHelpers.MakeDataRow("WORK REFUSAL", $"{fx.workRefusalHours:0.0}h",
                    AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Critical)));
            card.AddChild(AshfallUiHelpers.MakeDataRow("STAMINA", $"×{fx.staminaMultiplier:F2}",
                fx.staminaMultiplier < 1f ? AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Critical) : AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
            card.AddChild(AshfallUiHelpers.MakeDataRow("GUILT INSOMNIA", SeverityLabel(fx.guiltInsomniaSeverity),
                fx.guiltInsomniaSeverity >= Ashfall.Core.Survivors.GuiltInsomniaSystem.HighSeverityThreshold
                    ? AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Critical) : AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Muted)));
            card.AddChild(AshfallUiHelpers.MakeDataRow("HYPERVIGILANCE", $"{fx.hypervigilance:F2}",
                fx.hypervigilance > 0.5f ? AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Hot) : AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Muted)));
            card.AddChild(AshfallUiHelpers.MakeDataRow("MORAL BRANCH", fx.moralBranch));
            if (fx.dependencyCraftingPenalty > 0f)
                card.AddChild(AshfallUiHelpers.MakeDataRow("CRAFT PENALTY", $"-{fx.dependencyCraftingPenalty * 100:0}%",
                    AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Critical)));
            if (fx.dependencyCombatPenalty > 0f)
                card.AddChild(AshfallUiHelpers.MakeDataRow("COMBAT PENALTY", $"-{fx.dependencyCombatPenalty * 100:0}%",
                    AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Critical)));
            if (!string.IsNullOrEmpty(fx.finalWishState))
                card.AddChild(AshfallUiHelpers.MakeDataRow("FINAL WISH", fx.finalWishState.ToUpperInvariant(),
                    fx.finalWishState == "active" ? AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Hot) : AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Lethe)));

            card.AddChild(AshfallUiHelpers.MakeSeparator());
            return card;
        }

        private void BuildCommands()
        {
            _commandList.AddChild(AshfallUiHelpers.MakeSectionHeader("TREATMENTS & RECORD"));

            foreach (var fx in _phase0!.Effects)
            {
                if (fx == null || string.IsNullOrEmpty(fx.survivorId)) continue;
                string id = fx.survivorId;

                _commandList.AddChild(AshfallUiHelpers.MakeButton(
                    $"RECORD MORAL CHOICE — {FormatName(id)} (EMPATHY)",
                    () => _phase0.RecordMoralChoice(id, true)));
                _commandList.AddChild(AshfallUiHelpers.MakeButton(
                    $"RECORD MORAL CHOICE — {FormatName(id)} (PRAGMATISM)",
                    () => _phase0.RecordMoralChoice(id, false)));
                _commandList.AddChild(AshfallUiHelpers.MakeButton(
                    $"RECORD GUILT — {FormatName(id)} (SEVERE)",
                    () => _phase0.RecordGuilt(id, "choice_imposed_hardship", 0.8f)));
                _commandList.AddChild(AshfallUiHelpers.MakeButton(
                    $"SURVIVED COMBAT — {FormatName(id)}",
                    () => _phase0.RegisterCombatSurvived(id)));
                _commandList.AddChild(AshfallUiHelpers.MakeButton(
                    $"ADVANCE FINAL WISH — {FormatName(id)}",
                    () => _phase0.AdvanceFinalWish(id, "step_next")));
                _commandList.AddChild(AshfallUiHelpers.MakeButton(
                    $"APPLY INHALER — {FormatName(id)}",
                    () => _phase0.ApplyInhaler(id)));
            }
        }

        private static string SeverityLabel(float v)
        {
            if (v <= 0f) return "NONE";
            if (v < 0.5f) return "LIGHT";
            if (v < Ashfall.Core.Survivors.GuiltInsomniaSystem.HighSeverityThreshold) return "MODERATE";
            return "CRITICAL";
        }

        private static string FormatName(string id)
        {
            if (string.IsNullOrEmpty(id)) return "[UNNAMED]";
            return id switch
            {
                "survivor_dr_sarah_chen" => "Dr. Sarah Chen",
                "survivor_gunner_mikhail" => "Gunner Mikhail",
                "elena_vasquez" => "Elena Vasquez",
                _ => id.Replace("survivor_", "").Replace("_", " ").ToUpperInvariant()
            };
        }

        public void Open()
        {
            RefreshView();
            Visible = true;
        }

        public override void _ExitTree()
        {
            if (_phase0 != null)
            {
                _phase0.StateChanged -= OnPhase0StateChanged;
            }
            base._ExitTree();
        }
    }

    internal static class Phase0PanelLabelExtensions
    {
        public static Label? WithColor(this Label label, Color color)
        {
            if (label != null) label.AddThemeColorOverride("font_color", color);
            return label;
        }
    }
}
