// SPDX-License-Identifier: MIT
using System;
using Godot;
using Ashfall.Core.Radio;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.Localization;
using DesignTheme = Ashfall.Core.UI.Theme;

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

        /// <summary>Localization shim: catalog key with an English fallback.</summary>
        private static string T(string key, string fallback) => AshfallUiText.Tr(key, fallback);
        public event Action<string>? OnLocationDiscovered;

        private RadioHostSession? _radioHost;
        private string _activeSignalId = string.Empty;

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

        /// <summary>Test/selftest observable: event-driven refresh count — exactly one per publisher event while bound.</summary>
        public int RefreshCount { get; private set; }

        private void OnTriangulationStateChanged(TriangulationState _)
        {
            RefreshCount++;
            RefreshView();
        }

        private void HandleLocationRevealed(string id) => OnLocationDiscovered?.Invoke(id);

        public void Bind(RadioHostSession radioHost, string signalId = "")
        {
            if (_radioHost != null)
            {
                _radioHost.Triangulation.OnStateChanged -= OnTriangulationStateChanged;
                _radioHost.Triangulation.OnLocationRevealed -= HandleLocationRevealed;
            }

            _radioHost = radioHost;
            _activeSignalId = signalId;

            if (_radioHost != null)
            {
                _radioHost.Triangulation.OnStateChanged += OnTriangulationStateChanged;
                _radioHost.Triangulation.OnLocationRevealed += HandleLocationRevealed;
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

            _headerLabel = AshfallUiHelpers.MakeLabel("RADIO TRIANGULATION", 20, true);
            root.AddChild(_headerLabel);

            root.AddChild(AshfallUiHelpers.MakeSeparator());

            _signalLabel = AshfallUiHelpers.MakeBody(T("ui.triangulation.signal_dash", "Signal: —"));
            root.AddChild(_signalLabel);

            _observationCountLabel = AshfallUiHelpers.MakeBody(T("ui.triangulation.observations_none", "Observations: —"));
            root.AddChild(_observationCountLabel);

            _candidateLabel = AshfallUiHelpers.MakeBody(T("ui.triangulation.candidate_none_dash", "Candidate: —"));
            root.AddChild(_candidateLabel);

            _confidenceLabel = AshfallUiHelpers.MakeBody(T("ui.triangulation.confidence_none", "Confidence: —"));
            root.AddChild(_confidenceLabel);

            _uncertaintyLabel = AshfallUiHelpers.MakeBody(T("ui.triangulation.uncertainty_none", "Uncertainty: —"));
            root.AddChild(_uncertaintyLabel);

            _discoveryLabel = AshfallUiHelpers.MakeBody(T("ui.triangulation.discovery_none", "Discovery: —"));
            root.AddChild(_discoveryLabel);

            root.AddChild(AshfallUiHelpers.MakeSeparator());

            // Observation input
            var inputRow = new HBoxContainer();
            root.AddChild(inputRow);

            inputRow.AddChild(AshfallUiHelpers.MakeBody(T("ui.triangulation.bearing", "Bearing:")));
            _bearingInput = new SpinBox { MinValue = 0, MaxValue = 359, Value = 45 };
            inputRow.AddChild(_bearingInput);

            inputRow.AddChild(AshfallUiHelpers.MakeBody(T("ui.triangulation.strength", "Strength:")));
            _strengthInput = new SpinBox { MinValue = 0, MaxValue = 1, Value = 0.7, Step = 0.05 };
            inputRow.AddChild(_strengthInput);

            inputRow.AddChild(AshfallUiHelpers.MakeBody(T("ui.triangulation.noise", "Noise:")));
            _noiseInput = new SpinBox { MinValue = 0, MaxValue = 1, Value = 0.2, Step = 0.05 };
            inputRow.AddChild(_noiseInput);

            _feedbackLabel = AshfallUiHelpers.MakeBody("");
            root.AddChild(_feedbackLabel);

            root.AddChild(AshfallUiHelpers.MakeSeparator());

            var buttonRow = new HBoxContainer();
            root.AddChild(buttonRow);

            _recordButton = AshfallUiHelpers.MakeButton(T("ui.triangulation.record", "Record Observation"), OnRecordPressed);
            buttonRow.AddChild(_recordButton);

            _triangulateButton = AshfallUiHelpers.MakeButton(T("ui.triangulation.triangulate", "Triangulate"), OnTriangulatePressed);
            buttonRow.AddChild(_triangulateButton);

            _closeButton = AshfallUiHelpers.MakeButton(T("ui.triangulation.close", "Close"), () => OnClose?.Invoke());
            buttonRow.AddChild(_closeButton);
        }

        private void OnRecordPressed()
        {
            if (_radioHost == null || string.IsNullOrEmpty(_activeSignalId)) return;
            float bearing = (float)_bearingInput.Value;
            float strength = (float)_strengthInput.Value;
            float noise = (float)_noiseInput.Value;
            string result = _radioHost.RecordObservation(_activeSignalId, bearing, strength, noise);
            _feedbackLabel.Text = result;
            RefreshView();
        }

        private void OnTriangulatePressed()
        {
            if (_radioHost == null || string.IsNullOrEmpty(_activeSignalId)) return;
            string result = _radioHost.TriangulateSignal(_activeSignalId);
            _feedbackLabel.Text = result;
            RefreshView();
        }

        private void RefreshView()
        {
            if (_signalLabel == null)
                BuildUI();

            if (_signalLabel == null || _radioHost == null) return;

            if (string.IsNullOrEmpty(_activeSignalId))
            {
                // Explicit no-signal state: never a canned demo signal id (N16.6).
                _signalLabel.Text = T("ui.triangulation.signal_none", "Signal: None under direction-finding");
                _observationCountLabel.Text = T("ui.triangulation.observations_none", "Observations: —");
                _candidateLabel.Text = T("ui.triangulation.candidate_none_dash", "Candidate: —");
                _confidenceLabel.Text = T("ui.triangulation.confidence_none", "Confidence: —");
                _uncertaintyLabel.Text = T("ui.triangulation.uncertainty_none", "Uncertainty: —");
                _discoveryLabel.Text = T("ui.triangulation.discovery_none", "Discovery: —");
                _discoveryLabel.Modulate = AshfallUiHelpers.ToColor(DesignTheme.Pale);
                _recordButton.Disabled = true;
                _triangulateButton.Disabled = true;
                return;
            }

            _recordButton.Disabled = false;
            _triangulateButton.Disabled = false;
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
                _discoveryLabel.Modulate = discovered ? AshfallUiHelpers.ToColor(DesignTheme.Success) : AshfallUiHelpers.ToColor(DesignTheme.Warning);
            }
            else
            {
                _candidateLabel.Text = T("ui.triangulation.candidate_none", "Candidate: None");
                _confidenceLabel.Text = T("ui.triangulation.confidence_none", "Confidence: —");
                _uncertaintyLabel.Text = T("ui.triangulation.uncertainty_none", "Uncertainty: —");
                _discoveryLabel.Text = T("ui.triangulation.discovery_nodata", "Discovery: No data");
                _discoveryLabel.Modulate = AshfallUiHelpers.ToColor(DesignTheme.Pale);
            }
        }

        /// <summary>Detach the panel before its session authority is replaced (INV-16.5).</summary>
        public void Unbind()
        {
            if (_radioHost != null)
            {
                _radioHost.Triangulation.OnStateChanged -= OnTriangulationStateChanged;
                _radioHost.Triangulation.OnLocationRevealed -= HandleLocationRevealed;
                _radioHost = null;
            }
        }

        public override void _ExitTree()
        {
            Unbind();
            base._ExitTree();
        }
    }
}
