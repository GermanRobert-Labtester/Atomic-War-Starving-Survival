using System;
using Godot;
using Ashfall.Core.Radio;
using Ashfall.Core.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Radio Triangulation panel.
    /// Thin presentation layer for signal direction finding.
    /// Shows observations, bearing, signal quality, candidate locations,
    /// confidence, and discovery status.
    /// All gameplay logic delegates to RadioHostSession → SignalTriangulationSystem.
    /// </summary>
    public partial class TriangulationPanel : Control
    {
        public event Action? OnClose;
        public event Action<string>? OnLocationDiscovered;

        private RadioHostSession? _radioHost;
        private string _activeSignalId = "sig_distress";

        private Label _headerLabel = null!;
        private Label _signalLabel = null!;
        private Label _observationCountLabel = null!;
        private Label _candidateLabel = null!;
        private Label _confidenceLabel = null!;
        private Label _uncertaintyLabel = null!;
        private Label _discoveryLabel = null!;
        private Label _feedbackLabel = null!;
        private SpinBox _bearingInput = null!;
        private SpinBox _strengthInput = null!;
        private SpinBox _noiseInput = null!;
        private Button _recordButton = null!;
        private Button _triangulateButton = null!;
        private Button _closeButton = null!;

        public bool IsBound => _radioHost != null;

        public void Bind(RadioHostSession radioHost, string signalId = "sig_distress")
        {
            if (_radioHost != null)
            {
                _radioHost.Triangulation.OnStateChanged -= _ => RefreshView();
            }

            _radioHost = radioHost;
            _activeSignalId = signalId;

            if (_radioHost != null)
            {
                _radioHost.Triangulation.OnStateChanged += _ => RefreshView();
                _radioHost.Triangulation.OnLocationRevealed += id => OnLocationDiscovered?.Invoke(id);
                RefreshView();
            }
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

            _headerLabel = AshfallUiHelpers.MakeLabel("RADIO TRIANGULATION", 20, true);
            root.AddChild(_headerLabel);

            root.AddChild(AshfallUiHelpers.MakeSeparator());

            _signalLabel = AshfallUiHelpers.MakeBody("Signal: —");
            root.AddChild(_signalLabel);

            _observationCountLabel = AshfallUiHelpers.MakeBody("Observations: —");
            root.AddChild(_observationCountLabel);

            _candidateLabel = AshfallUiHelpers.MakeBody("Candidate: —");
            root.AddChild(_candidateLabel);

            _confidenceLabel = AshfallUiHelpers.MakeBody("Confidence: —");
            root.AddChild(_confidenceLabel);

            _uncertaintyLabel = AshfallUiHelpers.MakeBody("Uncertainty: —");
            root.AddChild(_uncertaintyLabel);

            _discoveryLabel = AshfallUiHelpers.MakeBody("Discovery: —");
            root.AddChild(_discoveryLabel);

            root.AddChild(AshfallUiHelpers.MakeSeparator());

            // Observation input
            var inputRow = new HBoxContainer();
            root.AddChild(inputRow);

            inputRow.AddChild(AshfallUiHelpers.MakeBody("Bearing:"));
            _bearingInput = new SpinBox { MinValue = 0, MaxValue = 359, Value = 45 };
            inputRow.AddChild(_bearingInput);

            inputRow.AddChild(AshfallUiHelpers.MakeBody("Strength:"));
            _strengthInput = new SpinBox { MinValue = 0, MaxValue = 1, Value = 0.7, Step = 0.05 };
            inputRow.AddChild(_strengthInput);

            inputRow.AddChild(AshfallUiHelpers.MakeBody("Noise:"));
            _noiseInput = new SpinBox { MinValue = 0, MaxValue = 1, Value = 0.2, Step = 0.05 };
            inputRow.AddChild(_noiseInput);

            _feedbackLabel = AshfallUiHelpers.MakeBody("");
            root.AddChild(_feedbackLabel);

            root.AddChild(AshfallUiHelpers.MakeSeparator());

            var buttonRow = new HBoxContainer();
            root.AddChild(buttonRow);

            _recordButton = AshfallUiHelpers.MakeButton("Record Observation", OnRecordPressed);
            buttonRow.AddChild(_recordButton);

            _triangulateButton = AshfallUiHelpers.MakeButton("Triangulate", OnTriangulatePressed);
            buttonRow.AddChild(_triangulateButton);

            _closeButton = AshfallUiHelpers.MakeButton("Close", () => OnClose?.Invoke());
            buttonRow.AddChild(_closeButton);
        }

        private void OnRecordPressed()
        {
            if (_radioHost == null) return;
            float bearing = (float)_bearingInput.Value;
            float strength = (float)_strengthInput.Value;
            float noise = (float)_noiseInput.Value;
            string result = _radioHost.RecordObservationDemo(_activeSignalId, bearing, strength, noise);
            _feedbackLabel.Text = result;
            RefreshView();
        }

        private void OnTriangulatePressed()
        {
            if (_radioHost == null) return;
            string result = _radioHost.TriangulateDemo(_activeSignalId);
            _feedbackLabel.Text = result;
            RefreshView();
        }

        private void RefreshView()
        {
            if (_radioHost == null) return;

            _signalLabel.Text = $"Signal: {_activeSignalId}";
            _observationCountLabel.Text = $"Observations: {_radioHost.Triangulation.GetObservationCount(_activeSignalId)}";

            var candidate = _radioHost.Triangulation.GetCandidate(_activeSignalId);
            if (candidate != null)
            {
                _candidateLabel.Text = $"Candidate: {candidate.locationId}";
                _confidenceLabel.Text = $"Confidence: {candidate.confidence:P0}";
                _uncertaintyLabel.Text = $"Uncertainty: ±{candidate.uncertaintyRadiusKm:F0} km";

                bool discovered = _radioHost.Triangulation.IsLocationDiscovered(candidate.locationId);
                _discoveryLabel.Text = discovered ? "Discovery: CONFIRMED" : "Discovery: Pending";
                _discoveryLabel.Modulate = discovered ? Colors.Green : Colors.Yellow;
            }
            else
            {
                _candidateLabel.Text = "Candidate: None";
                _confidenceLabel.Text = "Confidence: —";
                _uncertaintyLabel.Text = "Uncertainty: —";
                _discoveryLabel.Text = "Discovery: No data";
                _discoveryLabel.Modulate = Colors.White;
            }
        }

        public override void _ExitTree()
        {
            if (_radioHost != null)
            {
                _radioHost.Triangulation.OnStateChanged -= _ => RefreshView();
            }
        }
    }
}
