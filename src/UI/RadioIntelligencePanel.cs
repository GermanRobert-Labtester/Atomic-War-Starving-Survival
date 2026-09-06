using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Radio;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Wasteland Radio Frequency Intercepts & Oscilloscope Console.
    /// Thin presentation layer over ShelterRadioStationSystem.
    /// </summary>
    public partial class RadioIntelligencePanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        public bool IsBound => _radio != null;
        private ShelterRadioStationSystem? _radio;
        private OrbitalHarrowTelemetrySystem? _harrow;
        private int _currentDay;

        private Button _closeButton = null!;
        private Button _scanButton = null!;
        private Label _frequencyLabel = null!;
        private Label _bandLabel = null!;
        private ProgressBar _signalMeter = null!;
        private ColorRect _oscilloscope = null!;
        private Button _decryptButton = null!;
        private ProgressBar _decryptProgressBar = null!;
        private Button _recordBearingButton = null!;
        private Control _bearingRadar = null!;
        private VBoxContainer _sosContainer = null!;
        private VBoxContainer _interceptListContainer = null!;
        private Label _statusLabel = null!;

        private string _selectedInterceptId = string.Empty;

        public void Bind(ShelterRadioStationSystem radio, OrbitalHarrowTelemetrySystem? harrow = null, int currentDay = 0)
        {
            if (_radio != null) Unbind();

            _radio = radio;
            _harrow = harrow;
            _currentDay = currentDay;

            _radio.OnInterceptDetected += OnInterceptDetected;
            _radio.OnInterceptDecrypted += OnInterceptDecrypted;
            _radio.OnLocationTriangulated += OnLocationTriangulated;
            _radio.OnDistressExpired += OnDistressExpired;
            _radio.OnRadioStateChanged += RefreshView;

            RefreshView();
        }

        public void Unbind()
        {
            if (_radio != null)
            {
                _radio.OnInterceptDetected -= OnInterceptDetected;
                _radio.OnInterceptDecrypted -= OnInterceptDecrypted;
                _radio.OnLocationTriangulated -= OnLocationTriangulated;
                _radio.OnDistressExpired -= OnDistressExpired;
                _radio.OnRadioStateChanged -= RefreshView;
            }
            _radio = null;
            _harrow = null;
        }

        public override void _Ready()
        {
            var binder = new SceneBinder(this, typeof(RadioIntelligencePanel));
            binder.Require<Button>("CloseButton");
            binder.Require<Button>("ScanButton");
            binder.Require<Label>("FrequencyLabel");
            binder.Require<Label>("BandLabel");
            binder.Require<ProgressBar>("SignalMeter");
            binder.Require<ColorRect>("Oscilloscope");
            binder.Require<Button>("DecryptButton");
            binder.Require<ProgressBar>("DecryptProgressBar");
            binder.Require<Button>("RecordBearingButton");
            binder.Require<Control>("BearingRadar");
            binder.Require<VBoxContainer>("SosContainer");
            binder.Require<VBoxContainer>("InterceptListContainer");
            binder.Require<Label>("StatusLabel");

            _closeButton = binder.Get<Button>("CloseButton");
            _scanButton = binder.Get<Button>("ScanButton");
            _frequencyLabel = binder.Get<Label>("FrequencyLabel");
            _bandLabel = binder.Get<Label>("BandLabel");
            _signalMeter = binder.Get<ProgressBar>("SignalMeter");
            _oscilloscope = binder.Get<ColorRect>("Oscilloscope");
            _decryptButton = binder.Get<Button>("DecryptButton");
            _decryptProgressBar = binder.Get<ProgressBar>("DecryptProgressBar");
            _recordBearingButton = binder.Get<Button>("RecordBearingButton");
            _bearingRadar = binder.Get<Control>("BearingRadar");
            _sosContainer = binder.Get<VBoxContainer>("SosContainer");
            _interceptListContainer = binder.Get<VBoxContainer>("InterceptListContainer");
            _statusLabel = binder.Get<Label>("StatusLabel");

            _closeButton.Pressed += () => { Visible = false; OnClose?.Invoke(); };
            _scanButton.Pressed += OnScanPressed;
            _decryptButton.Pressed += OnDecryptPressed;
            _recordBearingButton.Pressed += OnRecordBearingPressed;

            RefreshView();
        }

        public override void _ExitTree() => Unbind();

        public void Open()
        {
            Visible = true;
            RefreshView();
        }

        public void RefreshView()
        {
            if (_frequencyLabel == null || _interceptListContainer == null) return;

            AshfallUiHelpers.EmptyChildren(_interceptListContainer);
            AshfallUiHelpers.EmptyChildren(_sosContainer);

            if (_radio == null)
            {
                _frequencyLabel.Text = "TUNED: --- kHz";
                _bandLabel.Text = "BAND: OFFLINE";
                _statusLabel.Text = "No radio station session bound.";
                _signalMeter.Value = 0;
                _decryptProgressBar.Value = 0;
                _decryptButton.Disabled = true;
                _recordBearingButton.Disabled = true;
                return;
            }

            var state = _radio.State;
            _frequencyLabel.Text = $"TUNED: {state.tunedFrequencyKhz:N0} kHz";
            _bandLabel.Text = $"BAND: {state.bandId.ToUpperInvariant()} (3,000–30,000 kHz)";

            // Intercept List
            var intercepts = state.intercepts;
            if (intercepts.Count == 0)
            {
                _interceptListContainer.AddChild(AshfallUiHelpers.MakeMetadata("No intercepts detected. Sweep frequency to scan."));
            }
            else
            {
                foreach (var intercept in intercepts)
                {
                    _radio.Catalog.TryGetValue(intercept.InterceptId, out var def);
                    string name = def != null ? $"[{def.Callsign}] {def.FrequencyKhz} kHz" : intercept.InterceptId;
                    string status = intercept.Resolved ? "TRIANGULATED" : (intercept.IsDecrypted ? "DECRYPTED" : $"LOCK {intercept.SignalLockPermille / 10f:F0}%");

                    var btn = new Button
                    {
                        Text = $"{name} // {status}",
                        Alignment = HorizontalAlignment.Left
                    };
                    string capturedId = intercept.InterceptId;
                    if (capturedId == _selectedInterceptId)
                    {
                        btn.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
                    }
                    btn.Pressed += () =>
                    {
                        _selectedInterceptId = capturedId;
                        RefreshView();
                    };
                    _interceptListContainer.AddChild(btn);
                }
            }

            // Selected intercept detail
            var activeProgress = intercepts.FirstOrDefault(i => i.InterceptId == _selectedInterceptId) ?? intercepts.FirstOrDefault();
            if (activeProgress != null)
            {
                _selectedInterceptId = activeProgress.InterceptId;
                _radio.Catalog.TryGetValue(activeProgress.InterceptId, out var def);

                _signalMeter.Value = Math.Clamp(activeProgress.SignalLockPermille / 10f, 0f, 100f);
                _decryptProgressBar.Value = Math.Clamp(activeProgress.DecryptProgressPermille / 10f, 0f, 100f);

                _decryptButton.Disabled = activeProgress.IsDecrypted || activeProgress.IsExpired;
                _decryptButton.Text = activeProgress.IsDecrypted ? "DECRYPTED (CLEARED)" : $"DECRYPT ({activeProgress.DecryptProgressPermille / 10f:F0}%)";

                _recordBearingButton.Disabled = activeProgress.Resolved || activeProgress.IsExpired;
                _recordBearingButton.Text = activeProgress.Resolved ? "POSITION LOCATED" : $"RECORD BEARING ({activeProgress.BearingsCollected}/{def?.Triangulation?.RequiredBearings ?? 2})";

                if (def != null)
                {
                    _statusLabel.Text = activeProgress.IsDecrypted ? $"MESSAGE: {def.Message}" : $"Signal detected from {def.SourceFactionId}. Encryption: {def.Encryption.Scheme} (Diff: {def.Encryption.Difficulty})";
                }
            }
            else
            {
                _signalMeter.Value = 0;
                _decryptProgressBar.Value = 0;
                _decryptButton.Disabled = true;
                _recordBearingButton.Disabled = true;
                _statusLabel.Text = "Scan the frequency dial to search for active wasteland emissions.";
            }

            // SOS Trackers
            var activeSos = intercepts.Where(i => !i.IsExpired && i.ExpiresOnDay.HasValue).ToList();
            if (activeSos.Count == 0)
            {
                _sosContainer.AddChild(AshfallUiHelpers.MakeMetadata("No emergency distress beacons in range."));
            }
            else
            {
                foreach (var sos in activeSos)
                {
                    _radio.Catalog.TryGetValue(sos.InterceptId, out var def);
                    int daysLeft = Math.Max(0, (sos.ExpiresOnDay ?? _currentDay) - _currentDay);
                    var sosLabel = new Label
                    {
                        Text = $"[SOS EMERGENCY] {def?.Callsign ?? sos.InterceptId} — {daysLeft} day(s) remaining"
                    };
                    sosLabel.AddThemeColorOverride("font_color", daysLeft <= 1 ? AshfallUiHelpers.ToColor(DesignTheme.Hot) : AshfallUiHelpers.ToColor(DesignTheme.Warm));
                    _sosContainer.AddChild(sosLabel);
                }
            }
        }

        private void OnScanPressed()
        {
            if (_radio == null) return;
            var result = _radio.ScanFrequency(_currentDay);
            _statusLabel.Text = result.FoundSignal ? $"Acquired emission at {_radio.State.tunedFrequencyKhz} kHz: {result.StatusMessage}" : "Static noise. No coherent signal lock acquired.";
            RefreshView();
        }

        private void OnDecryptPressed()
        {
            if (_radio == null || string.IsNullOrEmpty(_selectedInterceptId)) return;
            int gain = _radio.ProgressDecryption(_selectedInterceptId);
            _statusLabel.Text = $"Decryption advanced (+{gain / 10f:F1}%).";
            RefreshView();
        }

        private void OnRecordBearingPressed()
        {
            if (_radio == null || string.IsNullOrEmpty(_selectedInterceptId)) return;
            int azimuth = _radio.State.antennaAzimuthDegrees;
            bool added = _radio.RecordBearing(_selectedInterceptId, azimuth);
            _statusLabel.Text = added ? $"Bearing recorded at {azimuth}°." : $"Bearing at {azimuth}° not sufficiently distinct (>= 20° required).";
            RefreshView();
        }

        private void OnInterceptDetected(string id) => RefreshView();
        private void OnInterceptDecrypted(string id) => RefreshView();
        private void OnLocationTriangulated(string id, string loc) => RefreshView();
        private void OnDistressExpired(string id) => RefreshView();
    }
}
