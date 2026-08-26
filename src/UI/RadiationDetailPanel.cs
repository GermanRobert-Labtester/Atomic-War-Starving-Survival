using System;
using System.Linq;
#pragma warning disable CS8618
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Radiation Detail panel.
    /// Shows per-survivor dosimetry, dosimeter calibration state, protection
    /// levels, and the dose-reading event log — bound to the live DoseLedger
    /// and Survivors sessions. Unbound systems render "NOT MONITORED".
    /// </summary>
    public partial class RadiationDetailPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblCurrentTitle;
        private VBoxContainer _currentData;
        private Label _lblDosimeterTitle;
        private VBoxContainer _dosimeterData;
        private Label _lblProtectionTitle;
        private VBoxContainer _protectionData;
        private Label _lblEventsTitle;
        private VBoxContainer _eventsList;

        private DoseLedgerHostSession? _dose;
        private SurvivorsHostSession? _survivors;

        public bool IsBound => _dose != null || _survivors != null;
        public int RenderedCurrentCount { get; private set; }

        public void Bind(DoseLedgerHostSession? dose = null, SurvivorsHostSession? survivors = null)
        {
            _dose = dose;
            _survivors = survivors;
            RefreshView();
        }

        public void RefreshView()
        {
            if (_currentData == null || _dosimeterData == null || _protectionData == null || _eventsList == null) return;

            AshfallUiHelpers.EmptyChildren(_currentData);
            AshfallUiHelpers.EmptyChildren(_dosimeterData);
            AshfallUiHelpers.EmptyChildren(_protectionData);
            AshfallUiHelpers.EmptyChildren(_eventsList);

            RenderedCurrentCount = 0;
            RenderCurrent();
            RenderDosimeter();
            RenderProtection();
            RenderEvents();
        }

        private void RenderCurrent()
        {
            if (_dose?.Ledger == null || _dose.Ledger.Entries.Count == 0)
            {
                _currentData.AddChild(MakeDimLine("No dose ledger bound."));
                return;
            }

            foreach (var entry in _dose.Ledger.Entries)
            {
                if (entry == null) continue;
                var row = AshfallUiHelpers.MakeDataRow(Name(entry.survivorId),
                    $"{entry.cumulativeMsv:0.0} mSv cumulative · baseline {entry.baselineMsv:0.0}",
                    AshfallUiHelpers.ToColor(entry.cumulativeMsv >= 50f
                        ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Lethe));
                _currentData.AddChild(row);
                RenderedCurrentCount++;
            }
        }

        private void RenderDosimeter()
        {
            if (_dose?.Calibration == null || _dose.Calibration.Devices.Count == 0)
            {
                _dosimeterData.AddChild(MakeDimLine("No dosimeters registered."));
                return;
            }

            foreach (var dev in _dose.Calibration.Devices.Values)
            {
                if (dev == null) continue;
                string assigned = string.IsNullOrEmpty(dev.assignedSurvivorId)
                    ? "unassigned" : Name(dev.assignedSurvivorId);
                _dosimeterData.AddChild(AshfallUiHelpers.MakeDataRow(
                    $"{dev.deviceTag} ({assigned})",
                    $"Battery {dev.batteryLevel * 100f:0}% · Cal {dev.calibrationQuality * 100f:0}% · ±{dev.errorBandMsv:0.0} mSv{(dev.isOverdue ? " · OVERDUE" : "")}",
                    AshfallUiHelpers.ToColor(dev.isOverdue
                        ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Lethe)));
            }
        }

        private void RenderProtection()
        {
            if (_survivors?.Shelter != null)
            {
                float weakest = _survivors.Shelter.GetWeakestCeilingAttenuation();
                _protectionData.AddChild(AshfallUiHelpers.MakeDataRow("Shelter Shielding",
                    $"Weakest ceiling {weakest * 100f:0}% attenuation",
                    AshfallUiHelpers.ToColor(weakest >= 0.5f
                        ? Ashfall.Core.UI.Theme.Lethe : Ashfall.Core.UI.Theme.Warm)));
            }
            else
            {
                _protectionData.AddChild(MakeDimLine("Shelter shielding not monitored."));
            }

            if (_dose?.Ledger != null)
            {
                foreach (var entry in _dose.Ledger.Entries)
                {
                    if (entry == null || entry.shieldingFactor >= 1f) continue;
                    _protectionData.AddChild(AshfallUiHelpers.MakeDataRow(
                        $"{Name(entry.survivorId)} shielding",
                        $"{entry.shieldingFactor * 100f:0}% of outdoor dose",
                        AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm)));
                }
            }
        }

        private void RenderEvents()
        {
            if (_dose?.Ledger == null || _dose.Ledger.Entries.Count == 0)
            {
                _eventsList.AddChild(MakeDimLine("No dose readings logged."));
                return;
            }

            int shown = 0;
            foreach (var entry in _dose.Ledger.Entries)
            {
                if (entry == null) continue;
                foreach (var reading in entry.readingsHistory)
                {
                    if (reading == null || shown >= 20) continue;
                    _eventsList.AddChild(AshfallUiHelpers.MakeDataRow(
                        $"[Day {reading.day}] {Name(entry.survivorId)}",
                        $"{reading.bookedMsv:0.0} mSv booked ({reading.source})",
                        AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Critical)));
                    shown++;
                }
            }

            if (shown == 0)
                _eventsList.AddChild(MakeDimLine("No reading events yet."));
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

            var title = AshfallUiHelpers.MakeTitle("RADIATION DETAIL", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Current radiation section
            _lblCurrentTitle = AshfallUiHelpers.MakeSectionHeader("CURRENT RADIATION");
            vbox.AddChild(_lblCurrentTitle);

            _currentData = new VBoxContainer();
            _currentData.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _currentData.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_currentData);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Dosimeter section
            _lblDosimeterTitle = AshfallUiHelpers.MakeSectionHeader("DOSIMETER STATUS");
            vbox.AddChild(_lblDosimeterTitle);

            _dosimeterData = new VBoxContainer();
            _dosimeterData.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _dosimeterData.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_dosimeterData);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Protection section
            _lblProtectionTitle = AshfallUiHelpers.MakeSectionHeader("PROTECTION LEVELS");
            vbox.AddChild(_lblProtectionTitle);

            _protectionData = new VBoxContainer();
            _protectionData.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _protectionData.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_protectionData);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Events section
            _lblEventsTitle = AshfallUiHelpers.MakeSectionHeader("RADIATION EVENTS");
            vbox.AddChild(_lblEventsTitle);

            _eventsList = new VBoxContainer();
            _eventsList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _eventsList.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_eventsList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            var btnClose = AshfallUiHelpers.MakeButton("CLOSE [Esc]", () => OnClose?.Invoke());
            btnClose.CustomMinimumSize = new Vector2(200, 40);
            vbox.AddChild(btnClose);

            var hint = AshfallUiHelpers.MakeSmall("[Esc] to close");
            hint.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeLabel);
            hint.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
            vbox.AddChild(hint);
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
