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
            EnsureUI();

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
            if (_survivors != null && _survivors.RosterState.Count > 0)
            {
                foreach (var state in _survivors.RosterState)
                {
                    if (state == null) continue;
                    var rad = _survivors.RadStateFor(state.Id);
                    var env = _survivors.GetLastExposureEnvironment(state.Id);
                    string reason = !string.IsNullOrEmpty(env?.ExposureReason)
                        ? env.ExposureReason
                        : (!string.IsNullOrEmpty(rad?.LastExposureReason) ? rad.LastExposureReason : "Shelter Interior");
                    float dose = rad?.RadiationDose ?? 0f;
                    float zone = env?.EffectiveZoneRadLevel ?? 0f;
                    var row = AshfallUiHelpers.MakeDataRow(Name(state.Id),
                        $"{dose:0.0}/100 mSv · {reason} ({zone:0.0} mSv/h zone)",
                        AshfallUiHelpers.ToColor(dose >= 50f
                            ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Lethe));
                    _currentData.AddChild(row);
                    RenderedCurrentCount++;
                }
                return;
            }

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
            if (_dose?.Calibration != null && _dose.Calibration.Devices.Count > 0)
            {
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
                return;
            }

            if (_survivors?.Radiation != null && _survivors.RosterState.Count > 0)
            {
                foreach (var s in _survivors.RosterState)
                {
                    if (s == null) continue;
                    var dosimeter = _survivors.Radiation.GetDosimeter(s.Id);
                    if (dosimeter == null) continue;
                    string reason = !string.IsNullOrEmpty(dosimeter.LastExposureReason)
                        ? $" · {dosimeter.LastExposureReason}"
                        : "";
                    _dosimeterData.AddChild(AshfallUiHelpers.MakeDataRow(
                        $"{Name(s.Id)} Pen",
                        $"Rate {dosimeter.CurrentReading:0.0} mSv/h · Lifetime {dosimeter.LifetimeDose:0.0} mSv{reason}",
                        AshfallUiHelpers.ToColor(dosimeter.CurrentReading > 10f
                            ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Lethe)));
                }
                return;
            }

            _dosimeterData.AddChild(MakeDimLine("No dosimeters registered."));
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

        private void EnsureUI()
        {
            if (_currentData == null)
            {
                _currentData = new VBoxContainer { Name = "CurrentData" };
                _dosimeterData = new VBoxContainer { Name = "DosimeterData" };
                _protectionData = new VBoxContainer { Name = "ProtectionData" };
                _eventsList = new VBoxContainer { Name = "EventsList" };
                AddChild(_currentData);
                AddChild(_dosimeterData);
                AddChild(_protectionData);
                AddChild(_eventsList);
            }
        }

        public override void _Ready()
        {
            // Ticket #125: layout chrome owned by res://assets/ui/panels/RadiationDetailPanel.tscn; SceneBinder resolves typed unique-name nodes once.
            // Sibling refresh code is unchanged.
            var binder = new SceneBinder(this, typeof(RadiationDetailPanel));
            binder.Require<VBoxContainer>("CurrentData");
            binder.Require<VBoxContainer>("DosimeterData");
            binder.Require<VBoxContainer>("ProtectionData");
            binder.Require<VBoxContainer>("EventsList");
            binder.Require<Button>("CloseButton");
            _currentData = binder.Get<VBoxContainer>("CurrentData");
            _dosimeterData = binder.Get<VBoxContainer>("DosimeterData");
            _protectionData = binder.Get<VBoxContainer>("ProtectionData");
            _eventsList = binder.Get<VBoxContainer>("EventsList");
            binder.Get<Button>("CloseButton").Pressed += () => OnClose?.Invoke();

            Visible = false;
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
