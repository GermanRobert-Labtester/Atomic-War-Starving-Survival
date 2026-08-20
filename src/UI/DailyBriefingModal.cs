using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core.Campaign;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Daily Briefing Modal (item 01) — shows the day's authoritative
    /// <see cref="DailyBriefingReport"/> before any further simulation runs.
    /// Blocked UI: the host does not advance another day until the player
    /// acknowledges via Acknowledge, Enter, or Space. Skip-to-complete via Tab.
    /// CRT styling + typewriter animation + clicker audio are wired through
    /// <see cref="AshfallUiHelpers"/>.
    /// </summary>
    public partial class DailyBriefingModal : Control
    {
        public event Action<int>? OnAcknowledged;

        private Label _titleLabel = null!;
        private RichTextLabel _bodyLabel = null!;
        private Label _ackLabel = null!;
        private Button _ackButton = null!;
        private Button _skipButton = null!;
        private ScrollContainer _scroll = null!;

        private DailyBriefingReport? _report;
        private int _revealedChars;
        private int _totalChars;
        private double _revealTimerMs;
        private const double RevealIntervalMs = 14.0; // typewriter cadence
        private bool _complete;

        public bool IsComplete => _complete;
        public bool IsOpen => Visible;

        public void Show(DailyBriefingReport report)
        {
            _report = report ?? throw new ArgumentNullException(nameof(report));
            _revealedChars = 0;
            _complete = false;
            _revealTimerMs = 0;
            _totalChars = ComposeText(report).Length;
            _titleLabel.Text = report.Title;
            _bodyLabel.Text = string.Empty;
            _ackLabel.Text = "PRESS [ENTER] / [SPACE] / [ACK] TO CONTINUE";
            _ackButton.Disabled = false;
            _skipButton.Disabled = false;
            _scroll.ScrollVertical = 0;
            Visible = true;
            QueueRedraw();
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            var bg = new ColorRect { Color = new Color(0.04f, 0.05f, 0.06f, 0.92f) };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            var center = new CenterContainer();
            center.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(center);

            var panel = AshfallUiHelpers.MakePanel(820, 660);
            center.AddChild(panel);

            var margins = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingMd);
            panel.AddChild(margins);

            var vbox = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingMd);
            margins.AddChild(vbox);

            var header = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
            _titleLabel = AshfallUiHelpers.MakeTitle("DAY BRIEFING", DesignTheme.FontSizeH2);
            _titleLabel.HorizontalAlignment = HorizontalAlignment.Left;
            _titleLabel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            header.AddChild(_titleLabel);

            _skipButton = AshfallUiHelpers.MakeButton("SKIP [Tab]", SkipToComplete);
            _skipButton.CustomMinimumSize = new Vector2(110, 32);
            header.AddChild(_skipButton);
            vbox.AddChild(header);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _scroll = new ScrollContainer
            {
                CustomMinimumSize = new Vector2(780, 520),
                SizeFlagsVertical = SizeFlags.ExpandFill
            };
            vbox.AddChild(_scroll);

            _bodyLabel = new RichTextLabel
            {
                BbcodeEnabled = false,
                FitContent = true,
                ScrollActive = false,
                SizeFlagsHorizontal = SizeFlags.ExpandFill,
                AutowrapMode = TextServer.AutowrapMode.WordSmart
            };
            _bodyLabel.AddThemeFontSizeOverride("normal_font_size", DesignTheme.FontSizeBody);
            _bodyLabel.AddThemeColorOverride("default_color", AshfallUiHelpers.ToColor(DesignTheme.Pale));
            _scroll.AddChild(_bodyLabel);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            var footer = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
            _ackLabel = AshfallUiHelpers.MakeMono("PRESS [ENTER] / [SPACE] / [ACK] TO CONTINUE");
            _ackLabel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            footer.AddChild(_ackLabel);

            _ackButton = AshfallUiHelpers.MakeButton("ACKNOWLEDGE", () => Acknowledge());
            _ackButton.CustomMinimumSize = new Vector2(150, 36);
            footer.AddChild(_ackButton);
            vbox.AddChild(footer);
        }

        public override void _Process(double delta)
        {
            if (!Visible || _complete || _report == null) return;
            if (_revealedChars >= _totalChars)
            {
                _complete = true;
                _bodyLabel.Text = ComposeText(_report);
                return;
            }
            _revealTimerMs += delta * 1000.0;
            if (_revealTimerMs < RevealIntervalMs) return;
            _revealTimerMs = 0;
            int step = Math.Max(1, (int)(delta * 1000.0 / RevealIntervalMs));
            _revealedChars = Math.Min(_totalChars, _revealedChars + step);
            var full = ComposeText(_report);
            _bodyLabel.Text = _revealedChars >= full.Length
                ? full
                : full.Substring(0, _revealedChars);
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (!Visible) return;
            if (@event is InputEventKey key && key.Pressed)
            {
                if (key.Keycode == Key.Enter || key.Keycode == Key.Space)
                {
                    Acknowledge();
                    GetViewport().SetInputAsHandled();
                    return;
                }
                if (key.Keycode == Key.Tab)
                {
                    SkipToComplete();
                    GetViewport().SetInputAsHandled();
                }
            }
        }

        private void SkipToComplete()
        {
            if (_report == null) return;
            _complete = true;
            _revealedChars = _totalChars;
            _bodyLabel.Text = ComposeText(_report);
            _skipButton.Disabled = true;
            _ackLabel.Text = "REVEALED // PRESS [ENTER] / [SPACE] / [ACK] TO CONTINUE";
        }

        private void Acknowledge()
        {
            if (!_complete)
            {
                SkipToComplete();
                return;
            }
            if (_report == null) return;
            int day = _report.Day;
            Visible = false;
            _report = null;
            OnAcknowledged?.Invoke(day);
        }

        private static string ComposeText(DailyBriefingReport report)
        {
            if (report == null) return string.Empty;
            var sb = new System.Text.StringBuilder();
            if (report.Sections == null) return sb.ToString();
            for (int i = 0; i < report.Sections.Count; i++)
            {
                var sec = report.Sections[i];
                if (sec == null || sec.Entries == null || sec.Entries.Length == 0) continue;
                sb.Append("── ").Append(string.IsNullOrEmpty(sec.Title) ? "(untitled)" : sec.Title)
                  .Append(" ──\n");
                for (int j = 0; j < sec.Entries.Length; j++)
                {
                    var e = sec.Entries[j];
                    if (e == null) continue;
                    if (!string.IsNullOrEmpty(e.PrimaryId))
                        sb.Append("  • [").Append(e.PrimaryId).Append("] ");
                    sb.AppendLine(string.IsNullOrEmpty(e.Text) ? "(no detail)" : e.Text);
                }
                sb.Append('\n');
            }
            return sb.ToString().TrimEnd();
        }
    }
}
