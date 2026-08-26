using System;
using System.Linq;
#pragma warning disable CS8618
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Radiation History panel.
    /// Shows dose history, cumulative exposure, and reading events — bound
    /// to the live DoseLedgerHostSession. Unbound renders an honest empty state.
    /// </summary>
    public partial class RadiationHistoryPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblHistoryTitle;
        private VBoxContainer _historyList;
        private Label _lblCumulativeTitle;
        private VBoxContainer _cumulativeList;
        private Label _lblEventsTitle;
        private VBoxContainer _eventsList;

        private DoseLedgerHostSession? _dose;

        public bool IsBound => _dose != null;
        public int RenderedRowCount { get; private set; }

        public void Bind(DoseLedgerHostSession? dose)
        {
            _dose = dose;
            RefreshView();
        }

        public void RefreshView()
        {
            if (_historyList == null || _cumulativeList == null || _eventsList == null) return;

            AshfallUiHelpers.EmptyChildren(_historyList);
            AshfallUiHelpers.EmptyChildren(_cumulativeList);
            AshfallUiHelpers.EmptyChildren(_eventsList);

            RenderedRowCount = 0;

            if (_dose?.Ledger == null || _dose.Ledger.Entries.Count == 0)
            {
                _historyList.AddChild(MakeDimLine("No dose ledger bound."));
                return;
            }

            // ── Per-survivor dose history ──
            foreach (var entry in _dose.Ledger.Entries)
            {
                if (entry == null) continue;
                AddRow(_historyList, $"{Name(entry.survivorId)} — baseline {entry.baselineMsv:0.0} · cumulative {entry.cumulativeMsv:0.0} mSv",
                    entry.cumulativeMsv >= 50f ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Lethe);
                RenderedRowCount++;
            }

            // ── Cumulative summary ──
            float totalCumulative = _dose.Ledger.Entries.Sum(e => e?.cumulativeMsv ?? 0f);
            float totalBaseline = _dose.Ledger.Entries.Sum(e => e?.baselineMsv ?? 0f);
            AddRow(_cumulativeList, $"Total cumulative dose: {totalCumulative:0.0} mSv", Ashfall.Core.UI.Theme.Pale);
            AddRow(_cumulativeList, $"Total inherited baseline: {totalBaseline:0.0} mSv", Ashfall.Core.UI.Theme.Dim);
            AddRow(_cumulativeList, $"Tracked survivors: {_dose.Ledger.Entries.Count}", Ashfall.Core.UI.Theme.Dim);
            RenderedRowCount += 3;

            // ── Reading events (capped at 20) ──
            int shown = 0;
            foreach (var entry in _dose.Ledger.Entries)
            {
                if (entry == null) continue;
                foreach (var reading in entry.readingsHistory)
                {
                    if (reading == null || shown >= 20) continue;
                    AddRow(_eventsList, $"[Day {reading.day}] {Name(entry.survivorId)} — {reading.bookedMsv:0.0} mSv ({reading.source})",
                        Ashfall.Core.UI.Theme.Critical);
                    shown++;
                    RenderedRowCount++;
                }
            }
            if (shown == 0)
                _eventsList.AddChild(MakeDimLine("No reading events logged."));
        }

        private void AddRow(VBoxContainer parent, string text, (float r, float g, float b, float a) col)
        {
            var label = new Label { Text = text };
            label.CustomMinimumSize = new Vector2(400, 0);
            label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
            label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(col));
            parent.AddChild(label);
        }

        private Label MakeDimLine(string text)
        {
            var l = new Label { Text = text };
            l.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
            l.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
            return l;
        }

        private static string Name(string id)
        {
            if (string.IsNullOrEmpty(id)) return "Unknown";
            int us = id.IndexOf('_');
            return us >= 0 ? id.Substring(us + 1).Replace('_', ' ') : id;
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            var bg = new ColorRect { Color = new Color(0.05f, 0.05f, 0.05f, 0.92f) };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            var container = new CenterContainer();
            container.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(container);

            var vbox = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingLg);
            vbox.CustomMinimumSize = new Vector2(550, 0);
            container.AddChild(vbox);

            var title = AshfallUiHelpers.MakeTitle("RADIATION HISTORY", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblHistoryTitle = AshfallUiHelpers.MakeSectionHeader("PER-SURVIVOR DOSE");
            vbox.AddChild(_lblHistoryTitle);
            _historyList = new VBoxContainer();
            _historyList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _historyList.CustomMinimumSize = new Vector2(450, 0);
            vbox.AddChild(_historyList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblCumulativeTitle = AshfallUiHelpers.MakeSectionHeader("CUMULATIVE SUMMARY");
            vbox.AddChild(_lblCumulativeTitle);
            _cumulativeList = new VBoxContainer();
            _cumulativeList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _cumulativeList.CustomMinimumSize = new Vector2(450, 0);
            vbox.AddChild(_cumulativeList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblEventsTitle = AshfallUiHelpers.MakeSectionHeader("READING EVENTS");
            vbox.AddChild(_lblEventsTitle);
            _eventsList = new VBoxContainer();
            _eventsList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _eventsList.CustomMinimumSize = new Vector2(450, 0);
            vbox.AddChild(_eventsList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            var btnClose = AshfallUiHelpers.MakeButton("CLOSE [Esc]", () => OnClose?.Invoke());
            btnClose.CustomMinimumSize = new Vector2(200, 40);
            vbox.AddChild(btnClose);
        }

        public void Open()
        {
            Visible = true;
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
