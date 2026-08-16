using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Ashfall.Core.UI;
using Ashfall.Core.Radiation;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Survivors panel.
    /// Shows survivor roster, needs, duty shifts, and radiation status with badge icons.
    /// </summary>
    public partial class SurvivorsPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _survivorList = null!;
        private VBoxContainer _statsGroup = null!;
        private SurvivorsHostSession? _survivorsHost;

        public bool IsBound => _survivorsHost != null;
        public int RenderedSurvivorCount => _survivorList?.GetChildCount() ?? 0;

        public void Bind(SurvivorsHostSession survivors)
        {
            _survivorsHost = survivors;
            RefreshView();
        }

        public void RefreshView()
        {
            if (_survivorList == null || _statsGroup == null) return;

            while (_survivorList.GetChildCount() > 0)
                _survivorList.RemoveChild(_survivorList.GetChild(0));
            while (_statsGroup.GetChildCount() > 0)
                _statsGroup.RemoveChild(_statsGroup.GetChild(0));

            if (_survivorsHost == null)
            {
                _survivorList.AddChild(AshfallUiHelpers.MakeMetadata("No survivor session bound."));
                return;
            }

            var slices = _survivorsHost.CaptureSave().survivors
                .Where(slice => slice != null)
                .ToDictionary(slice => slice.id, StringComparer.Ordinal);

            foreach (var survivor in _survivorsHost.RosterState)
            {
                if (survivor == null) continue;
                slices.TryGetValue(survivor.Id, out var slice);
                var definition = _survivorsHost.Roster.FindDefinition(survivor.Id);
                string displayName = !string.IsNullOrWhiteSpace(definition?.displayName)
                    ? definition.displayName
                    : survivor.Id;
                string status = !survivor.IsAliveState
                    ? "DEAD"
                    : survivor.Health < 25f
                        ? "CRITICAL"
                        : survivor.Hunger >= 90f || survivor.Thirst >= 90f || survivor.Warmth <= 20f
                            ? "STRAINED"
                            : "STABLE";
                float lifetimeDose = slice?.lifetimeRadiationExposure ?? 0f;

                var row = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
                var icon = AshfallUiHelpers.MakeBadgeIcon(lifetimeDose >= 50f ? "badge_rad_sickness" : survivor.Health < 30f ? "badge_trench_foot" : "badge_exhaustion", 22);
                row.AddChild(icon);

                var nameLbl = AshfallUiHelpers.MakeSmall(displayName);
                nameLbl.CustomMinimumSize = new Vector2(140, 0);
                row.AddChild(nameLbl);

                var statsText = AshfallUiHelpers.MakeMono(
                    $"HP {survivor.Health:0} · HUN {survivor.Hunger:0} · THI {survivor.Thirst:0} · " +
                    $"WARM {survivor.Warmth:0} · RAD {lifetimeDose:0} mSv");
                statsText.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
                statsText.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Lethe));
                row.AddChild(statsText);

                var statusLbl = AshfallUiHelpers.MakeSmall($"[{status}]");
                statusLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(StatusColor(status)));
                row.AddChild(statusLbl);

                _survivorList.AddChild(row);
            }

            if (_survivorsHost.RosterState.Count == 0)
                _survivorList.AddChild(AshfallUiHelpers.MakeMetadata("Roster empty. No registered shelter survivors."));

            float averageHealth = _survivorsHost.RosterState.Count == 0
                ? 0f
                : _survivorsHost.RosterState.Average(s => s?.Health ?? 0f);
            float averageDose = slices.Count == 0
                ? 0f
                : slices.Values.Average(s => s.lifetimeRadiationExposure);
            float averageMorale = _survivorsHost.RosterState.Count == 0
                ? 0f
                : _survivorsHost.RosterState.Average(s => s?.Morale ?? 0f);

            _statsGroup.AddChild(AshfallUiHelpers.MakeDataRow(
                "Living Residents",
                $"{_survivorsHost.RosterState.Count(s => s != null && s.IsAliveState)}/{_survivorsHost.RosterState.Count} · Avg HP {averageHealth:0}%",
                new Color(0.9f, 0.9f, 0.9f)));
            _statsGroup.AddChild(AshfallUiHelpers.MakeDataRow(
                "Average Lifetime Dose",
                $"{averageDose:0} mSv",
                AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Lethe)));
            _statsGroup.AddChild(AshfallUiHelpers.MakeDataRow(
                "Average Bunker Morale",
                $"{averageMorale:0}%",
                AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm)));

            if (!string.IsNullOrWhiteSpace(_survivorsHost.LastEvent))
                _statsGroup.AddChild(AshfallUiHelpers.MakeMetadata($"Last roster event: {_survivorsHost.LastEvent}"));
        }

        private static (float r, float g, float b, float a) StatusColor(string status)
        {
            return status switch
            {
                "CRITICAL" => Ashfall.Core.UI.Theme.Critical,
                "STRAINED" => Ashfall.Core.UI.Theme.Warm,
                "DEAD" => Ashfall.Core.UI.Theme.Muted,
                _ => Ashfall.Core.UI.Theme.Pale
            };
        }

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

            var panel = AshfallUiHelpers.MakePanel(700, 560);
            center.AddChild(panel);

            var margins = AshfallUiHelpers.MakeMargins(Ashfall.Core.UI.Theme.SpacingMd);
            panel.AddChild(margins);

            var vbox = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingMd);
            margins.AddChild(vbox);

            var header = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
            var title = AshfallUiHelpers.MakeTitle("SURVIVOR ROSTER & DUTY COHORT", Ashfall.Core.UI.Theme.FontSizeH2);
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
                CustomMinimumSize = new Vector2(660, 440),
                SizeFlagsVertical = SizeFlags.ExpandFill
            };
            vbox.AddChild(scroll);

            var contentBox = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingMd);
            contentBox.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            scroll.AddChild(contentBox);

            contentBox.AddChild(AshfallUiHelpers.MakeSectionHeader("RESIDENT ROSTER"));
            _survivorList = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingXs);
            contentBox.AddChild(_survivorList);

            contentBox.AddChild(AshfallUiHelpers.MakeSeparator());

            contentBox.AddChild(AshfallUiHelpers.MakeSectionHeader("COHORT HEALTH & MORALE TELEMETRY"));
            _statsGroup = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingXs);
            contentBox.AddChild(_statsGroup);

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
    }
}
