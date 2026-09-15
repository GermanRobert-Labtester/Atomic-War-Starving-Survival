// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core.Foundry;
using Ashfall.Core.UI;
using AtomicWar.GodotApp;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Plan 140 Phase 3 — hydraulic extrusion press. Presentation only: shows
    /// machine/tooling condition, active phase, quality forecast, and the
    /// bounded nature of grade benefits. Never claims a perfect tube.
    /// </summary>
    public partial class HydraulicExtrusionPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private Label _detailText = null!;
        private Label _batchText = null!;
        private Label _machineText = null!;
        private Button _startBtn = null!;
        private Button _advanceBtn = null!;
        private Button _completeBtn = null!;
        private OptionButton _machineSelector = null!;
        private OptionButton _productSelector = null!;
        private SpinBox _unitsInput = null!;
        private string _selectedMachineId = string.Empty;
        private string _selectedProductId = string.Empty;

        private HydraulicExtrusionHostSession? _host;

        public bool IsBound => _host != null;

        public Func<int>? AppDayProvider { get; set; }

        public void Bind(HydraulicExtrusionHostSession session)
        {
            _host = session;
            if (_host != null) _host.StateChanged += RefreshView;
            RefreshView();
        }

        public void Unbind()
        {
            if (_host != null)
            {
                _host.StateChanged -= RefreshView;
                _host = null;
            }
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);

            _shell = new AshfallDashboardShell("Heavy Fabrication // Hydraulic Extrusion", minWidth: 1000, minHeight: 650);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("tooling", "Tooling", "—", AshfallMetricCard.Criticality.Normal, minWidth: 130);
            _statusRail.AddCard("power", "Power", "—", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("cooling", "Cooling", "—", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("batch", "Active Batch", "NONE", AshfallMetricCard.Criticality.Normal, minWidth: 150);

            _contentStack = new VBoxContainer();
            _contentStack.AddThemeConstantOverride("separation", 12);
            _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;
            _contentStack.AddChild(_detailText = new Label { AutowrapMode = TextServer.AutowrapMode.WordSmart });

            _contentStack.AddChild(AshfallUiHelpers.MakeSeparator());
            _contentStack.AddChild(AshfallUiHelpers.MakeSectionHeader("PRESS & PRODUCT"));

            var machineRow = new HBoxContainer();
            machineRow.AddThemeConstantOverride("separation", 10);
            machineRow.AddChild(AshfallUiHelpers.MakeBody("Press:"));
            _machineSelector = new OptionButton { CustomMinimumSize = new Vector2(340, 36) };
            _machineSelector.ItemSelected += idx =>
            {
                var machines = SortedMachines();
                if ((int)idx >= 0 && (int)idx < machines.Count) _selectedMachineId = machines[(int)idx].id;
            };
            machineRow.AddChild(_machineSelector);
            _contentStack.AddChild(machineRow);

            var productRow = new HBoxContainer();
            productRow.AddThemeConstantOverride("separation", 10);
            productRow.AddChild(AshfallUiHelpers.MakeBody("Product:"));
            _productSelector = new OptionButton { CustomMinimumSize = new Vector2(440, 36) };
            _productSelector.ItemSelected += idx =>
            {
                var products = SortedProducts();
                if ((int)idx >= 0 && (int)idx < products.Count) _selectedProductId = products[(int)idx].id;
            };
            productRow.AddChild(_productSelector);
            productRow.AddChild(AshfallUiHelpers.MakeBody("Units:"));
            _unitsInput = new SpinBox { MinValue = 1, MaxValue = 50, Step = 1, Value = 5, CustomMinimumSize = new Vector2(90, 36) };
            productRow.AddChild(_unitsInput);
            _contentStack.AddChild(productRow);

            var btnRow = new HBoxContainer();
            btnRow.AddThemeConstantOverride("separation", 10);
            _startBtn = new Button { Text = "Start Batch", CustomMinimumSize = new Vector2(150, 36) };
            _startBtn.Pressed += () =>
            {
                if (_host == null) return;
                _host.StartBatch(_selectedProductId, _selectedMachineId, (int)_unitsInput.Value, SystemDay());
            };
            btnRow.AddChild(_startBtn);

            _advanceBtn = new Button { Text = "Advance Phase", CustomMinimumSize = new Vector2(160, 36) };
            _advanceBtn.Pressed += () =>
            {
                var open = FindOpenBatch();
                if (open != null) _host?.AdvanceBatch(open.BatchId);
            };
            btnRow.AddChild(_advanceBtn);

            _completeBtn = new Button { Text = "Complete at QA", CustomMinimumSize = new Vector2(160, 36) };
            _completeBtn.Pressed += () =>
            {
                var open = FindOpenBatch();
                if (open != null) _host?.CompleteBatch(open.BatchId, 0.5);
            };
            btnRow.AddChild(_completeBtn);
            _contentStack.AddChild(btnRow);

            var note = AshfallUiHelpers.MakeBody(
                "Grade improves downstream reliability but never guarantees it. High-pressure and cryogenic tubing reduce failure risk within authored bounds; rejected stock is scrap. A defect roll is always possible.");
            note.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            note.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            _contentStack.AddChild(note);

            _machineText = new Label { AutowrapMode = TextServer.AutowrapMode.WordSmart };
            _contentStack.AddChild(_machineText);
            _batchText = new Label { AutowrapMode = TextServer.AutowrapMode.WordSmart };
            _contentStack.AddChild(_batchText);

            _shell.SetContent(_contentStack);
            _shell.AttachHeaderCloseButton("CLOSE", () =>
            {
                Visible = false;
                OnClose?.Invoke();
            });

            RefreshView();
        }

        private int SystemDay() => AppDayProvider?.Invoke() ?? 1;

        private List<ExtrusionMachineDef> SortedMachines()
        {
            var list = new List<ExtrusionMachineDef>();
            if (_host != null)
            {
                foreach (var m in _host.System.Catalog.AllMachines) list.Add(m);
                list.Sort((a, b) => string.CompareOrdinal(a.id, b.id));
            }
            return list;
        }

        private List<ExtrusionProductDef> SortedProducts()
        {
            var list = new List<ExtrusionProductDef>();
            if (_host != null)
            {
                foreach (var p in _host.System.Catalog.AllProducts) list.Add(p);
                list.Sort((a, b) => string.CompareOrdinal(a.id, b.id));
            }
            return list;
        }

        private ExtrusionBatch? FindOpenBatch()
        {
            if (_host == null) return null;
            var batches = _host.System.State.Batches;
            for (int i = batches.Count - 1; i >= 0; i--)
                if (!batches[i].Completed) return batches[i];
            return null;
        }

        public void RefreshView()
        {
            if (_host == null || _statusRail == null) return;

            var open = FindOpenBatch();
            var activeMachine = !string.IsNullOrEmpty(open?.MachineId)
                ? _host.System.FindMachine(open!.MachineId)
                : _host.System.FindMachine(_selectedMachineId);

            _statusRail.Set("tooling",
                activeMachine == null ? "—" : $"{activeMachine.ToolingConditionBp}%",
                activeMachine != null && activeMachine.ToolingConditionBp < 35
                    ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);

            int power = _host.EnergyAvailableBp();
            int cooling = _host.CoolingAvailableBp();
            _statusRail.Set("power", $"{power}%", power < 50 ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("cooling", $"{cooling}%", cooling < 30 ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("batch", open == null ? "NONE" : open.Phase, AshfallMetricCard.Criticality.Normal);

            if (_detailText != null)
            {
                _detailText.Text =
                    $"Presses registered: {_host.System.State.Machines.Count} | Completed batches: {_host.System.State.TotalCompleted}\n" +
                    $"Power available {power}% | Cooling water available {cooling}%\n" +
                    $"Last event: {_host.LastEvent}";
            }

            SyncSelectors();

            if (_machineText != null)
            {
                var lines = new List<string>();
                foreach (var m in _host.System.State.Machines)
                    lines.Add($"{m.MachineId}: tooling {m.ToolingConditionBp}% | die {m.DieConditionBp}% | hours {m.OperatingHours}");
                _machineText.Text = lines.Count == 0 ? "No presses registered." : string.Join("\n", lines);
            }

            if (_batchText != null)
            {
                if (open == null)
                {
                    _batchText.Text = "No open batch.";
                }
                else
                {
                    int atQa = HydraulicExtrusionEngine.Phases.Length - 1;
                    _batchText.Text =
                        $"Open batch {open.BatchId}: {open.ProductProfileId} on {open.MachineId}, {open.Units} units, " +
                        $"phase {open.Phase} ({open.PhaseIndex + 1}/{HydraulicExtrusionEngine.Phases.Length})" +
                        (open.PhaseIndex >= atQa ? " — ready to complete." : ".");
                }
            }

            if (_startBtn != null)
                _startBtn.Disabled = string.IsNullOrEmpty(_selectedProductId) || string.IsNullOrEmpty(_selectedMachineId) || open != null;
            if (_advanceBtn != null)
                _advanceBtn.Disabled = open == null || open.PhaseIndex >= HydraulicExtrusionEngine.Phases.Length - 1;
            if (_completeBtn != null)
                _completeBtn.Disabled = open == null || open.PhaseIndex < HydraulicExtrusionEngine.Phases.Length - 1;
        }

        private void SyncSelectors()
        {
            if (_host == null) return;

            if (_machineSelector != null)
            {
                var machines = SortedMachines();
                _machineSelector.Clear();
                int selected = 0;
                for (int i = 0; i < machines.Count; i++)
                {
                    _machineSelector.AddItem($"{machines[i].display_name} [{machines[i].id}]", i);
                    if (machines[i].id == _selectedMachineId || (string.IsNullOrEmpty(_selectedMachineId) && i == 0))
                    {
                        selected = i;
                        _selectedMachineId = machines[i].id;
                    }
                }
                _machineSelector.Selected = selected;
            }

            if (_productSelector != null)
            {
                var products = SortedProducts();
                _productSelector.Clear();
                int selected = 0;
                for (int i = 0; i < products.Count; i++)
                {
                    var p = products[i];
                    _productSelector.AddItem($"{p.display_name} [{p.machine_class}]", i);
                    if (p.id == _selectedProductId || (string.IsNullOrEmpty(_selectedProductId) && i == 0))
                    {
                        selected = i;
                        _selectedProductId = p.id;
                    }
                }
                _productSelector.Selected = selected;
            }
        }

        public override void _ExitTree()
        {
            Unbind();
            base._ExitTree();
        }
    }
}
