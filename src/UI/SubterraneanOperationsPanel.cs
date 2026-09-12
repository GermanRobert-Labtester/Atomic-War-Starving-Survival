// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Excavation;
using Ashfall.Core.Survivors;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Subterranean Hazard Operations & Cave-In Rescue Panel.
    /// Thin presentation layer over ExcavationHazardSystem.
    /// </summary>
    public partial class SubterraneanOperationsPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        public bool IsBound => _excavation != null;
        private ExcavationHazardSystem? _excavation;
        private Ashfall.Core.Inventory.Inventory? _inventory;
        private SurvivorsHostSession? _survivors;

        private Button _closeButton = null!;
        private OptionButton _sectorSelector = null!;
        private ProgressBar _methaneMeter = null!;
        private ProgressBar _floodMeter = null!;
        private ProgressBar _sporeMeter = null!;
        private ProgressBar _shoringMeter = null!;
        private VBoxContainer _mitigationListContainer = null!;
        private Button _bulkheadToggleButton = null!;
        private VBoxContainer _rescueContainer = null!;
        private ProgressBar _rescueProgressBar = null!;
        private Button _rescueLaborButton = null!;
        private Label _statusLabel = null!;

        private string _selectedSectorId = "sector_excavation_alpha";

        public void Bind(
            ExcavationHazardSystem excavation,
            Ashfall.Core.Inventory.Inventory? inventory = null,
            SurvivorsHostSession? survivors = null)
        {
            if (_excavation != null) Unbind();

            _excavation = excavation;
            _inventory = inventory;
            _survivors = survivors;

            _excavation.OnMitigationInstalled += OnMitigationInstalled;
            _excavation.OnMethaneIgnition += OnMethaneIgnition;
            _excavation.OnSectorFlooded += OnSectorFlooded;
            _excavation.OnRescueStarted += OnRescueStarted;
            _excavation.OnRescueSucceeded += OnRescueSucceeded;
            _excavation.OnRescueFailed += OnRescueFailed;
            _excavation.OnHazardStateChanged += RefreshView;

            RefreshView();
        }

        public void Unbind()
        {
            if (_excavation != null)
            {
                _excavation.OnMitigationInstalled -= OnMitigationInstalled;
                _excavation.OnMethaneIgnition -= OnMethaneIgnition;
                _excavation.OnSectorFlooded -= OnSectorFlooded;
                _excavation.OnRescueStarted -= OnRescueStarted;
                _excavation.OnRescueSucceeded -= OnRescueSucceeded;
                _excavation.OnRescueFailed -= OnRescueFailed;
                _excavation.OnHazardStateChanged -= RefreshView;
            }
            _excavation = null;
            _inventory = null;
            _survivors = null;
        }

        public override void _Ready()
        {
            var binder = new SceneBinder(this, typeof(SubterraneanOperationsPanel));
            binder.Require<Button>("CloseButton");
            binder.Require<OptionButton>("SectorSelector");
            binder.Require<ProgressBar>("MethaneMeter");
            binder.Require<ProgressBar>("FloodMeter");
            binder.Require<ProgressBar>("SporeMeter");
            binder.Require<ProgressBar>("ShoringMeter");
            binder.Require<VBoxContainer>("MitigationListContainer");
            binder.Require<Button>("BulkheadToggleButton");
            binder.Require<VBoxContainer>("RescueContainer");
            binder.Require<ProgressBar>("RescueProgressBar");
            binder.Require<Button>("RescueLaborButton");
            binder.Require<Label>("StatusLabel");

            _closeButton = binder.Get<Button>("CloseButton");
            _sectorSelector = binder.Get<OptionButton>("SectorSelector");
            _methaneMeter = binder.Get<ProgressBar>("MethaneMeter");
            _floodMeter = binder.Get<ProgressBar>("FloodMeter");
            _sporeMeter = binder.Get<ProgressBar>("SporeMeter");
            _shoringMeter = binder.Get<ProgressBar>("ShoringMeter");
            _mitigationListContainer = binder.Get<VBoxContainer>("MitigationListContainer");
            _bulkheadToggleButton = binder.Get<Button>("BulkheadToggleButton");
            _rescueContainer = binder.Get<VBoxContainer>("RescueContainer");
            _rescueProgressBar = binder.Get<ProgressBar>("RescueProgressBar");
            _rescueLaborButton = binder.Get<Button>("RescueLaborButton");
            _statusLabel = binder.Get<Label>("StatusLabel");

            _closeButton.Pressed += () => { Visible = false; OnClose?.Invoke(); };
            _sectorSelector.ItemSelected += OnSectorSelected;
            _bulkheadToggleButton.Pressed += OnBulkheadTogglePressed;
            _rescueLaborButton.Pressed += OnRescueLaborPressed;

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
            if (_sectorSelector == null || _mitigationListContainer == null) return;

            AshfallUiHelpers.EmptyChildren(_mitigationListContainer);
            AshfallUiHelpers.EmptyChildren(_rescueContainer);

            if (_excavation == null)
            {
                _statusLabel.Text = "No excavation hazard session bound.";
                _bulkheadToggleButton.Disabled = true;
                _rescueLaborButton.Disabled = true;
                return;
            }

            // Populate sector selector if empty or changed
            var sectorIds = _excavation.State.sectors.Keys.ToList();
            if (sectorIds.Count == 0) sectorIds.Add("sector_excavation_alpha");

            if (_sectorSelector.ItemCount != sectorIds.Count)
            {
                _sectorSelector.Clear();
                foreach (var id in sectorIds)
                {
                    _sectorSelector.AddItem(id);
                }
            }

            var sector = _excavation.GetOrCreateSector(_selectedSectorId);

            // Hazard telemetry meters
            _methaneMeter.Value = Math.Clamp(sector.MethanePpm / 50f, 0f, 100f);
            if (sector.MethanePpm >= 4000)
            {
                _statusLabel.Text = $"[CRITICAL HAZARD] Explosive methane concentration ({sector.MethanePpm} PPM) in {_selectedSectorId}!";
            }
            else if (sector.MethanePpm >= 2000)
            {
                _statusLabel.Text = $"[WARNING] Methane build-up detected ({sector.MethanePpm} PPM) in {_selectedSectorId}.";
            }

            _floodMeter.Value = Math.Clamp(sector.FloodLevelPermille / 10f, 0f, 100f);
            _sporeMeter.Value = Math.Clamp(sector.SporeConcentrationPermille / 10f, 0f, 100f);
            _shoringMeter.Value = Math.Clamp(sector.ShoringHealthPermille / 10f, 0f, 100f);

            // Bulkhead Toggle Button
            _bulkheadToggleButton.Text = sector.IsBulkheadSealed ? $"[UNSEAL BULKHEAD] {_selectedSectorId.ToUpperInvariant()}" : $"[EMERGENCY SEAL] {_selectedSectorId.ToUpperInvariant()}";
            bool hasTrapped = sector.ActiveTrappedMiners.Count > 0;
            if (hasTrapped && !sector.IsBulkheadSealed)
            {
                _bulkheadToggleButton.Disabled = true;
                _bulkheadToggleButton.Text = "[LOCKED] TRAPPED MINERS DETECTED";
            }
            else
            {
                _bulkheadToggleButton.Disabled = false;
            }

            // Mitigation Catalog
            var mitigations = _excavation.Catalog.Values.ToList();
            if (mitigations.Count == 0)
            {
                _mitigationListContainer.AddChild(AshfallUiHelpers.MakeMetadata("No hazard mitigation protocols cataloged."));
            }
            else
            {
                foreach (var mit in mitigations)
                {
                    var row = new HBoxContainer();
                    string costSummary = string.Join(", ", mit.RequiredItems.Select(i => $"{i.ItemId} x{i.Amount}"));
                    bool canApply = _excavation.CanApplyMitigation(_selectedSectorId, mit.Id, out var reason);

                    var label = new Label
                    {
                        Text = $"{mit.DisplayName} // Cost: [{costSummary}]",
                        SizeFlagsHorizontal = SizeFlags.ExpandFill
                    };
                    row.AddChild(label);

                    var btn = new Button
                    {
                        Text = canApply ? "INSTALL" : $"BLOCKED ({reason})"
                    };
                    btn.Disabled = !canApply;
                    string capMit = mit.Id;
                    btn.Pressed += () =>
                    {
                        var res = _excavation.TryApplyMitigation(_selectedSectorId, capMit);
                        _statusLabel.Text = res.IsSuccess ? $"Mitigation installed: {capMit}." : $"Install failed: {res.FailureCode}";
                        RefreshView();
                    };
                    row.AddChild(btn);

                    _mitigationListContainer.AddChild(row);
                }
            }

            // Rescue Operation HUD
            if (hasTrapped && !sector.RescueCompleted && !sector.RescueFailed)
            {
                int trappedCount = sector.ActiveTrappedMiners.Count;
                int remainingLabor = sector.RescueLaborRemaining;
                int deadlineDay = sector.RescueDeadlineDay ?? (_excavation.State.currentDay + 3);

                var alertLabel = new Label
                {
                    Text = $"[CAVE-IN RESCUE IN PROGRESS] {trappedCount} Miner(s) Trapped | Deadline: Day {deadlineDay} | Remaining Labor: {remainingLabor} Ticks"
                };
                alertLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Hot));
                _rescueContainer.AddChild(alertLabel);

                _rescueProgressBar.Value = Math.Clamp(100f - (remainingLabor / 2.4f), 0f, 100f);
                _rescueLaborButton.Disabled = false;
                _rescueLaborButton.Text = "DISPATCH CLEARANCE CREW (60 Ticks)";
            }
            else if (sector.RescueCompleted)
            {
                _rescueContainer.AddChild(AshfallUiHelpers.MakeMetadata("Cave-in cleared. All miners successfully extracted."));
                _rescueProgressBar.Value = 100f;
                _rescueLaborButton.Disabled = true;
                _rescueLaborButton.Text = "RESCUE COMPLETED";
            }
            else if (sector.RescueFailed)
            {
                var failLabel = new Label { Text = "[CATASTROPHIC COLLAPSE] Rescue window expired. Sector lost." };
                failLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Hot));
                _rescueContainer.AddChild(failLabel);
                _rescueProgressBar.Value = 0f;
                _rescueLaborButton.Disabled = true;
                _rescueLaborButton.Text = "SECTOR ABANDONED";
            }
            else
            {
                _rescueContainer.AddChild(AshfallUiHelpers.MakeMetadata("Sector structural integrity stable. No active cave-in emergencies."));
                _rescueProgressBar.Value = 100f;
                _rescueLaborButton.Disabled = true;
                _rescueLaborButton.Text = "NO RESCUE REQUIRED";
            }
        }

        private void OnSectorSelected(long index)
        {
            _selectedSectorId = _sectorSelector.GetItemText((int)index);
            RefreshView();
        }

        private void OnBulkheadTogglePressed()
        {
            if (_excavation == null) return;
            var sector = _excavation.GetOrCreateSector(_selectedSectorId);
            bool targetState = !sector.IsBulkheadSealed;
            var res = _excavation.TryToggleBulkhead(_selectedSectorId, targetState, out var reason);
            _statusLabel.Text = res.IsSuccess ? (targetState ? $"Bulkhead sealed for {_selectedSectorId}." : $"Bulkhead opened for {_selectedSectorId}.") : $"Bulkhead action blocked: {reason}";
            RefreshView();
        }

        private void OnRescueLaborPressed()
        {
            if (_excavation == null) return;
            _excavation.ProgressRescueLabor(_selectedSectorId, 60);
            _statusLabel.Text = $"Dispatched clearance labor to {_selectedSectorId}.";
            RefreshView();
        }

        private void OnMitigationInstalled(string sec, string mit) => RefreshView();
        private void OnMethaneIgnition(string sec) => RefreshView();
        private void OnSectorFlooded(string sec) => RefreshView();
        private void OnRescueStarted(string sec, int count) => RefreshView();
        private void OnRescueSucceeded(string sec) => RefreshView();
        private void OnRescueFailed(string sec) => RefreshView();
    }
}
