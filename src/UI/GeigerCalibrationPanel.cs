using System;
using Godot;
using Ashfall.Core.Radiation;
using Ashfall.Core.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Geiger Counter Calibration panel.
    /// Thin presentation layer for dosimeter device management.
    /// Shows device condition, battery, sensor, calibration quality,
    /// error band, and calibration controls.
    /// All gameplay logic delegates to DoseLedgerHostSession → DosimeterCalibrationSystem.
    /// </summary>
    public partial class GeigerCalibrationPanel : Control
    {
        public event Action? OnClose;

        private DoseLedgerHostSession? _doseHost;
        private string _selectedDeviceTag = "tag_1";

        private Label _headerLabel = null!;
        private Label _deviceTagLabel = null!;
        private Label _batteryLabel = null!;
        private Label _sensorLabel = null!;
        private Label _qualityLabel = null!;
        private Label _readingsLabel = null!;
        private Label _errorBandLabel = null!;
        private Label _confidenceLabel = null!;
        private Label _statusLabel = null!;
        private Label _calibrationLabel = null!;
        private ProgressBar _batteryBar = null!;
        private ProgressBar _sensorBar = null!;
        private ProgressBar _qualityBar = null!;
        private Button _calibrateButton = null!;
        private Button _replaceBatteryButton = null!;
        private Button _serviceSensorButton = null!;
        private Button _closeButton = null!;

        public bool IsBound => _doseHost != null;
        public int SimDay { get; set; } = 1;

        public void Bind(DoseLedgerHostSession doseHost, string deviceTag = "tag_1")
        {
            if (_doseHost != null)
            {
                _doseHost.Calibration.OnStateChanged -= _ => RefreshView();
            }

            _doseHost = doseHost;
            _selectedDeviceTag = deviceTag;

            if (_doseHost != null)
            {
                _doseHost.Calibration.OnStateChanged += _ => RefreshView();
                RefreshView();
            }
        }

        public void Open()
        {
            Visible = true;
            RefreshView();
        }

        public override void _Ready()
        {
            BuildUI();
        }

        private void BuildUI()
        {
            var margin = AshfallUiHelpers.MakeMargins(16);
            AddChild(margin);

            var root = new VBoxContainer();
            margin.AddChild(root);

            _headerLabel = AshfallUiHelpers.MakeLabel("DOSIMETER CALIBRATION", 20, true);
            root.AddChild(_headerLabel);

            root.AddChild(AshfallUiHelpers.MakeSeparator());

            _deviceTagLabel = AshfallUiHelpers.MakeBody("Device: —");
            root.AddChild(_deviceTagLabel);

            // Battery
            _batteryLabel = AshfallUiHelpers.MakeBody("Battery: —");
            root.AddChild(_batteryLabel);
            _batteryBar = new ProgressBar { MinValue = 0, MaxValue = 100, Value = 100 };
            root.AddChild(_batteryBar);

            // Sensor
            _sensorLabel = AshfallUiHelpers.MakeBody("Sensor: —");
            root.AddChild(_sensorLabel);
            _sensorBar = new ProgressBar { MinValue = 0, MaxValue = 100, Value = 100 };
            root.AddChild(_sensorBar);

            // Calibration quality
            _qualityLabel = AshfallUiHelpers.MakeBody("Calibration Quality: —");
            root.AddChild(_qualityLabel);
            _qualityBar = new ProgressBar { MinValue = 0, MaxValue = 100, Value = 100 };
            root.AddChild(_qualityBar);

            root.AddChild(AshfallUiHelpers.MakeSeparator());

            _readingsLabel = AshfallUiHelpers.MakeBody("Readings: —");
            root.AddChild(_readingsLabel);

            _errorBandLabel = AshfallUiHelpers.MakeBody("Error Band: —");
            root.AddChild(_errorBandLabel);

            _confidenceLabel = AshfallUiHelpers.MakeBody("Confidence: —");
            root.AddChild(_confidenceLabel);

            _statusLabel = AshfallUiHelpers.MakeBody("Status: —");
            root.AddChild(_statusLabel);

            _calibrationLabel = AshfallUiHelpers.MakeBody("");
            root.AddChild(_calibrationLabel);

            root.AddChild(AshfallUiHelpers.MakeSeparator());

            var buttonRow = AshfallUiHelpers.MakeActionBar();
            root.AddChild(buttonRow);

            _calibrateButton = AshfallUiHelpers.MakeButton("Calibrate", OnCalibratePressed);
            buttonRow.AddChild(_calibrateButton);

            _replaceBatteryButton = AshfallUiHelpers.MakeButton("Replace Battery", OnReplaceBattery);
            buttonRow.AddChild(_replaceBatteryButton);

            _serviceSensorButton = AshfallUiHelpers.MakeButton("Service Sensor", OnServiceSensor);
            buttonRow.AddChild(_serviceSensorButton);

            _closeButton = AshfallUiHelpers.MakeButton("Close", () => OnClose?.Invoke());
            buttonRow.AddChild(_closeButton);
        }

        private void OnCalibratePressed()
        {
            if (_doseHost == null) return;
            int currentDay = SimDay; // from bound host or caller
            var device = _doseHost.Calibration.GetDevice(_selectedDeviceTag);
            if (device == null) return;

            if (device.isStationOccupied)
            {
                // Try to complete
                string result = _doseHost.CompleteCalibration(_selectedDeviceTag, currentDay + 1);
                _calibrationLabel.Text = result;
            }
            else
            {
                // Start calibration
                string result = _doseHost.StartCalibration(_selectedDeviceTag, currentDay);
                _calibrationLabel.Text = result;
            }
            RefreshView();
        }

        private void OnReplaceBattery()
        {
            if (_doseHost == null) return;
            string result = _doseHost.ReplaceBattery(_selectedDeviceTag);
            _calibrationLabel.Text = result;
            RefreshView();
        }

        private void OnServiceSensor()
        {
            if (_doseHost == null) return;
            string result = _doseHost.ServiceSensor(_selectedDeviceTag);
            _calibrationLabel.Text = result;
            RefreshView();
        }

        private void RefreshView()
        {
            if (_doseHost == null)
            {
                if (_deviceTagLabel != null) _deviceTagLabel.Text = "Device: No dosimeter session bound";
                if (_statusLabel != null) _statusLabel.Text = "Status: Calibration station offline";
                if (_calibrateButton != null) _calibrateButton.Disabled = true;
                if (_replaceBatteryButton != null) _replaceBatteryButton.Disabled = true;
                if (_serviceSensorButton != null) _serviceSensorButton.Disabled = true;
                return;
            }

            var device = _doseHost.Calibration.GetDevice(_selectedDeviceTag);
            if (device == null)
            {
                _deviceTagLabel.Text = "Device: Not registered";
                _statusLabel.Text = "Status: No dosimeter profile found";
                if (_calibrateButton != null) _calibrateButton.Disabled = true;
                return;
            }

            _deviceTagLabel.Text = $"Device: {device.deviceTag} (assigned to {device.assignedSurvivorId})";

            _batteryLabel.Text = $"Battery: {device.batteryLevel:P0}";
            _batteryBar.Value = device.batteryLevel * 100;
            _batteryBar.Modulate = device.batteryLevel < 0.2f ? Colors.Red : Colors.White;

            _sensorLabel.Text = $"Sensor Condition: {device.sensorCondition:P0}";
            _sensorBar.Value = device.sensorCondition * 100;
            _sensorBar.Modulate = device.sensorCondition < 0.3f ? Colors.Red : Colors.White;

            _qualityLabel.Text = $"Calibration Quality: {device.calibrationQuality:F2}";
            _qualityBar.Value = device.calibrationQuality * 100;

            _readingsLabel.Text = $"Readings since calibration: {device.readingsSinceCalibration}/{DosimeterCalibrationSystem.ReadingsPerCalibration}";

            _errorBandLabel.Text = $"Error Band: ±{device.errorBandMsv:F1} mSv";
            _errorBandLabel.Modulate = device.isOverdue ? Colors.Red : Colors.White;

            float confidence = _doseHost.Calibration.GetConfidence(_selectedDeviceTag);
            _confidenceLabel.Text = $"Measurement Confidence: {confidence:P0}";

            if (device.isStationOccupied)
            {
                _statusLabel.Text = "Status: Calibrating...";
                _calibrateButton.Text = "Complete Calibration";
                _calibrateButton.Disabled = false;
            }
            else if (device.isOverdue)
            {
                _statusLabel.Text = "Status: CALIBRATION OVERDUE [!]";
                _calibrateButton.Text = "Start Calibration";
                _calibrateButton.Disabled = false;
            }
            else
            {
                _statusLabel.Text = "Status: Operational";
                _calibrateButton.Text = "Start Calibration";
                _calibrateButton.Disabled = false;
            }

            _replaceBatteryButton.Disabled = device.batteryLevel >= 0.95f;
            _serviceSensorButton.Disabled = device.sensorCondition >= 0.95f;
        }

        public override void _ExitTree()
        {
            if (_doseHost != null)
            {
                _doseHost.Calibration.OnStateChanged -= _ => RefreshView();
            }
        }
    }
}
