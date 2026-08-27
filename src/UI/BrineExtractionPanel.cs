using System;
using Godot;
using Ashfall.Core.Foundry;
using Ashfall.Core.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Brine Extraction panel.
    /// Thin presentation layer for salt mine management.
    /// Shows vein state, workers, drill/pump condition, storage,
    /// treaty delivery status, and maintenance controls.
    /// All gameplay logic delegates to SilentFoundryHostSession → SaltMineExtractionSystem.
    /// </summary>
    public partial class BrineExtractionPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private SilentFoundryHostSession? _foundryHost;

        private Label _headerLabel = null!;
        private Label _veinLabel = null!;
        private Label _workersLabel = null!;
        private Label _drillLabel = null!;
        private Label _pumpLabel = null!;
        private Label _contaminationLabel = null!;
        private Label _storageLabel = null!;
        private Label _treatyLabel = null!;
        private Label _powerLabel = null!;
        private Label _feedbackLabel = null!;
        private Button _openMineButton = null!;
        private Button _tickButton = null!;
        private Button _deliverButton = null!;
        private Button _replaceDrillButton = null!;
        private Button _repairPumpButton = null!;
        private Button _closeButton = null!;

        public bool IsBound => _foundryHost != null;
        public int SimDay { get; set; } = 1;

        public void Bind(SilentFoundryHostSession foundryHost)
        {
            if (_foundryHost != null)
            {
                _foundryHost.StateChanged -= RefreshView;
            }

            _foundryHost = foundryHost;

            if (_foundryHost != null)
            {
                _foundryHost.StateChanged += RefreshView;
                RefreshView();
            }
        }

        public void Unbind()
        {
            if (_foundryHost != null)
            {
                _foundryHost.StateChanged -= RefreshView;
                _foundryHost = null;
            }
        }

        public override void _ExitTree()
        {
            Unbind();
            base._ExitTree();
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

            _headerLabel = AshfallUiHelpers.MakeLabel("BRINE EXTRACTION", 20, true);
            root.AddChild(_headerLabel);

            root.AddChild(AshfallUiHelpers.MakeSeparator());

            _veinLabel = AshfallUiHelpers.MakeBody("Vein: —");
            root.AddChild(_veinLabel);

            _workersLabel = AshfallUiHelpers.MakeBody("Workers: —");
            root.AddChild(_workersLabel);

            _drillLabel = AshfallUiHelpers.MakeBody("Drill: —");
            root.AddChild(_drillLabel);

            _pumpLabel = AshfallUiHelpers.MakeBody("Pump: —");
            root.AddChild(_pumpLabel);

            _contaminationLabel = AshfallUiHelpers.MakeBody("Contamination: —");
            root.AddChild(_contaminationLabel);

            _powerLabel = AshfallUiHelpers.MakeBody("Power: —");
            root.AddChild(_powerLabel);

            root.AddChild(AshfallUiHelpers.MakeSeparator());

            _storageLabel = AshfallUiHelpers.MakeBody("Storage: —");
            root.AddChild(_storageLabel);

            _treatyLabel = AshfallUiHelpers.MakeBody("Treaty: —");
            root.AddChild(_treatyLabel);

            _feedbackLabel = AshfallUiHelpers.MakeBody("");
            root.AddChild(_feedbackLabel);

            root.AddChild(AshfallUiHelpers.MakeSeparator());

            var buttonRow = AshfallUiHelpers.MakeActionBar();
            root.AddChild(buttonRow);

            _openMineButton = AshfallUiHelpers.MakeButton("Open Mine", OnOpenMine);
            buttonRow.AddChild(_openMineButton);

            _tickButton = AshfallUiHelpers.MakeButton("Tick Day", OnTickDay);
            buttonRow.AddChild(_tickButton);

            _deliverButton = AshfallUiHelpers.MakeButton("Deliver to Treaty", OnDeliver);
            buttonRow.AddChild(_deliverButton);

            _replaceDrillButton = AshfallUiHelpers.MakeButton("Replace Drill", OnReplaceDrill);
            buttonRow.AddChild(_replaceDrillButton);

            _repairPumpButton = AshfallUiHelpers.MakeButton("Repair Pump", OnRepairPump);
            buttonRow.AddChild(_repairPumpButton);

            _closeButton = AshfallUiHelpers.MakeButton("Close", () => OnClose?.Invoke());
            buttonRow.AddChild(_closeButton);
        }

        private void OnOpenMine()
        {
            if (_foundryHost == null) return;
            string result = _foundryHost.OpenSaltMine();
            _feedbackLabel.Text = result;
            RefreshView();
        }

        private void OnTickDay()
        {
            if (_foundryHost == null) return;
            string result = _foundryHost.TickSaltMine(SimDay);
            _feedbackLabel.Text = result;
            RefreshView();
        }

        private void OnDeliver()
        {
            if (_foundryHost == null) return;
            string result = _foundryHost.DeliverSaltTreaty(SimDay);
            _feedbackLabel.Text = result;
            RefreshView();
        }

        private void OnReplaceDrill()
        {
            if (_foundryHost == null) return;
            _foundryHost.SaltMine.ReplaceDrill("vein_salt_01");
            _feedbackLabel.Text = "Drill replaced.";
            RefreshView();
        }

        private void OnRepairPump()
        {
            if (_foundryHost == null) return;
            _foundryHost.SaltMine.RepairPump("vein_salt_01");
            _feedbackLabel.Text = "Pump repaired.";
            RefreshView();
        }

        private void RefreshView()
        {
            if (_foundryHost == null)
            {
                if (_veinLabel != null) _veinLabel.Text = "Vein: No salt mine session bound";
                if (_openMineButton != null) _openMineButton.Disabled = true;
                if (_tickButton != null) _tickButton.Disabled = true;
                if (_deliverButton != null) _deliverButton.Disabled = true;
                if (_replaceDrillButton != null) _replaceDrillButton.Disabled = true;
                if (_repairPumpButton != null) _repairPumpButton.Disabled = true;
                return;
            }

            var vein = _foundryHost.SaltMine.GetVein("vein_salt_01");
            if (vein == null)
            {
                _veinLabel.Text = "Vein: Not opened";
                return;
            }

            _veinLabel.Text = $"Vein: {vein.displayName} (ore: {vein.remainingOre:F0} kg)";
            _workersLabel.Text = $"Workers: {vein.assignedWorkers}/{vein.maxWorkers}";
            _drillLabel.Text = $"Drill: {vein.drillCondition:P0}";
            _drillLabel.Modulate = vein.drillCondition < 0.3f ? Colors.Red : Colors.White;
            _pumpLabel.Text = $"Pump: {vein.pumpPressure:P0}";
            _pumpLabel.Modulate = vein.pumpPressure < 0.3f ? Colors.Red : Colors.White;
            _contaminationLabel.Text = $"Contamination: {vein.contamination:P0}";
            _contaminationLabel.Modulate = vein.contamination > 0.3f ? Colors.Red : Colors.White;

            var state = _foundryHost.SaltMine.State;
            _powerLabel.Text = $"Power: {(state.isPowered ? "ON" : "OFF")}";
            _storageLabel.Text = $"Storage: salt={state.saltStorage:F1} kg, brine={state.brineStorage:F1} brl, sulfur={state.sulfurStorage:F1} kg";

            int deliveries = _foundryHost.SaltMine.GetDeliveryCount();
            _treatyLabel.Text = $"Treaty deliveries: {deliveries}";

            _openMineButton.Disabled = vein.isUnlocked;
            _tickButton.Disabled = !vein.isUnlocked || vein.isShutdown || vein.assignedWorkers <= 0;
            _deliverButton.Disabled = state.brineStorage < SaltMineExtractionSystem.TreatyBrineQuotaBarrels;
            _replaceDrillButton.Disabled = vein.drillCondition > 0.8f;
            _repairPumpButton.Disabled = vein.pumpPressure > 0.8f;
        }
    }
}
