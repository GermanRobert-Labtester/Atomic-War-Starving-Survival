// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Inventory;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Plan 50 Phase 3 — expedition overland vehicle customization &
    /// maintenance garage.
    ///
    /// Presentation only: renders the Core garage read model (fitted
    /// modifications, chassis/engine/transmission wear, immobilization, active
    /// recovery missions) and forwards player commands to
    /// <see cref="VehicleGarageSystem"/> (install / uninstall / service /
    /// complete recovery). Inventory consumption and effect math stay in Core.
    /// The garage's fitted-modification effects now decorate the expedition
    /// profile, so the displayed deltas are the ones the sortie actually uses.
    /// </summary>
    public partial class VehicleGaragePanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private Label _detailText = null!;
        private Label _vehicleText = null!;
        private Label _modText = null!;
        private Label _armorText = null!;
        private Label _armorCostText = null!;
        private Label _maintenanceText = null!;
        private Label _recoveryText = null!;
        private Label _commandResult = null!;

        private OptionButton _vehicleSelector = null!;
        private OptionButton _modSelector = null!;
        private OptionButton _armorSelector = null!;

        private Button _installButton = null!;
        private Button _uninstallButton = null!;
        private Button _armorInstallButton = null!;
        private Button _armorReforgeButton = null!;
        private Button _serviceChassisButton = null!;
        private Button _serviceEngineButton = null!;
        private Button _serviceTransmissionButton = null!;
        private Button _completeRecoveryButton = null!;

        private Label _modCostText = null!;

        private VehicleGarageSystem? _garage;
        private ExpeditionVehicleSystem? _vehicles;
        private Inventory? _inventory;

        private string _selectedVehicleId = string.Empty;
        private string _selectedModId = string.Empty;
        private string _selectedArmorGradeId = string.Empty;
        private string _selectedRecoveryId = string.Empty;
        private bool _syncing;

        public bool IsBound => _garage != null;

        public string LastFeedback { get; private set; } = string.Empty;

        public void Bind(VehicleGarageSystem garage, ExpeditionVehicleSystem vehicles, Inventory inventory)
        {
            _garage = garage;
            _vehicles = vehicles;
            _inventory = inventory;
            RefreshView();
        }

        public void Unbind()
        {
            _garage = null;
            _vehicles = null;
            _inventory = null;
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);

            _shell = new AshfallDashboardShell("VEHICLE GARAGE // OVERLAND MAINTENANCE", minWidth: 1180, minHeight: 700);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("vehicles", "Vehicles", "—", AshfallMetricCard.Criticality.Normal, minWidth: 100);
            _statusRail.AddCard("immobilized", "Immobilized", "—", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("recoveries", "Recoveries", "—", AshfallMetricCard.Criticality.Normal, minWidth: 110);
            _statusRail.AddCard("fitted", "Fitted Mods", "—", AshfallMetricCard.Criticality.Normal, minWidth: 110);

            var scroll = new ScrollContainer
            {
                SizeFlagsVertical = SizeFlags.ExpandFill,
                SizeFlagsHorizontal = SizeFlags.ExpandFill
            };
            _contentStack = new VBoxContainer();
            _contentStack.AddThemeConstantOverride("separation", 10);
            _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;
            scroll.AddChild(_contentStack);

            _contentStack.AddChild(_detailText = new Label { AutowrapMode = TextServer.AutowrapMode.WordSmart });

            _contentStack.AddChild(AshfallUiHelpers.MakeSeparator());
            _contentStack.AddChild(AshfallUiHelpers.MakeSectionHeader("VEHICLE"));

            var vehicleRow = new HBoxContainer();
            vehicleRow.AddThemeConstantOverride("separation", 10);
            vehicleRow.AddChild(AshfallUiHelpers.MakeBody("Select:"));
            _vehicleSelector = new OptionButton { CustomMinimumSize = new Vector2(320, 34) };
            _vehicleSelector.ItemSelected += OnVehicleSelected;
            vehicleRow.AddChild(_vehicleSelector);
            _contentStack.AddChild(vehicleRow);
            _contentStack.AddChild(_vehicleText = new Label { AutowrapMode = TextServer.AutowrapMode.WordSmart });

            _contentStack.AddChild(AshfallUiHelpers.MakeSeparator());
            _contentStack.AddChild(AshfallUiHelpers.MakeSectionHeader("MODIFICATION CATALOG"));

            var modRow = new HBoxContainer();
            modRow.AddThemeConstantOverride("separation", 10);
            modRow.AddChild(AshfallUiHelpers.MakeBody("Modification:"));
            _modSelector = new OptionButton { CustomMinimumSize = new Vector2(420, 34) };
            _modSelector.ItemSelected += OnModSelected;
            modRow.AddChild(_modSelector);
            _installButton = AshfallUiHelpers.MakeButton("INSTALL", OnInstallPressed);
            modRow.AddChild(_installButton);
            _uninstallButton = AshfallUiHelpers.MakeButton("UNINSTALL", OnUninstallPressed);
            modRow.AddChild(_uninstallButton);
            _contentStack.AddChild(modRow);
            _contentStack.AddChild(_modCostText = AshfallUiHelpers.MakeSmall("—"));
            _contentStack.AddChild(_modText = new Label { AutowrapMode = TextServer.AutowrapMode.WordSmart });

            _contentStack.AddChild(AshfallUiHelpers.MakeSeparator());
            _contentStack.AddChild(AshfallUiHelpers.MakeSectionHeader("MAINTENANCE"));

            var maintRow = new HBoxContainer();
            maintRow.AddThemeConstantOverride("separation", 8);
            _serviceChassisButton = AshfallUiHelpers.MakeButton("SERVICE CHASSIS", OnServiceChassisPressed);
            maintRow.AddChild(_serviceChassisButton);
            _serviceEngineButton = AshfallUiHelpers.MakeButton("SERVICE ENGINE", OnServiceEnginePressed);
            maintRow.AddChild(_serviceEngineButton);
            _serviceTransmissionButton = AshfallUiHelpers.MakeButton("SERVICE TRANSMISSION", OnServiceTransmissionPressed);
            maintRow.AddChild(_serviceTransmissionButton);
            _contentStack.AddChild(maintRow);
            _contentStack.AddChild(_maintenanceText = new Label { AutowrapMode = TextServer.AutowrapMode.WordSmart });

            _contentStack.AddChild(AshfallUiHelpers.MakeSeparator());
            _contentStack.AddChild(AshfallUiHelpers.MakeSectionHeader("ARMOR PLATING"));

            var armorRow = new HBoxContainer();
            armorRow.AddThemeConstantOverride("separation", 8);
            armorRow.AddChild(AshfallUiHelpers.MakeBody("Grade:"));
            _armorSelector = new OptionButton { CustomMinimumSize = new Vector2(360, 34) };
            _armorSelector.ItemSelected += OnArmorSelected;
            armorRow.AddChild(_armorSelector);
            _armorInstallButton = AshfallUiHelpers.MakeButton("FIT PLATE", OnInstallArmorPressed);
            armorRow.AddChild(_armorInstallButton);
            _armorReforgeButton = AshfallUiHelpers.MakeButton("RE-FORGE", OnReforgeArmorPressed);
            armorRow.AddChild(_armorReforgeButton);
            _contentStack.AddChild(armorRow);
            _contentStack.AddChild(_armorCostText = AshfallUiHelpers.MakeSmall("—"));
            _contentStack.AddChild(_armorText = new Label { AutowrapMode = TextServer.AutowrapMode.WordSmart });

            _contentStack.AddChild(AshfallUiHelpers.MakeSeparator());
            _contentStack.AddChild(AshfallUiHelpers.MakeSectionHeader("RECOVERY OPERATIONS"));

            var recRow = new HBoxContainer();
            recRow.AddThemeConstantOverride("separation", 8);
            _completeRecoveryButton = AshfallUiHelpers.MakeButton("COMPLETE RECOVERY", OnCompleteRecoveryPressed);
            recRow.AddChild(_completeRecoveryButton);
            _contentStack.AddChild(recRow);
            _contentStack.AddChild(_recoveryText = new Label { AutowrapMode = TextServer.AutowrapMode.WordSmart });

            _contentStack.AddChild(AshfallUiHelpers.MakeSeparator());
            _commandResult = AshfallUiHelpers.MakeSmall("—");
            _commandResult.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            _contentStack.AddChild(_commandResult);

            var note = AshfallUiHelpers.MakeBody(
                "Fitted modifications change the sortie profile the expedition authority uses "
                + "(cargo, speed, fuel); travelled distance feeds component wear. A vehicle whose "
                + "chassis, engine, or transmission reaches catastrophic wear is immobilized and "
                + "cannot be dispatched until a recovery team completes its work.");
            note.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            note.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            _contentStack.AddChild(note);

            _shell.SetContent(scroll);
            _shell.AttachHeaderCloseButton("CLOSE", () =>
            {
                Visible = false;
                OnClose?.Invoke();
            });

            RefreshView();
        }

        public override void _ExitTree()
        {
            Unbind();
            base._ExitTree();
        }

        // ── Selection ─────────────────────────────────────────────────────

        private void OnVehicleSelected(long index)
        {
            if (_syncing) return;
            var ids = OwnedVehicleIds();
            if (index >= 0 && index < ids.Count) _selectedVehicleId = ids[(int)index];
            RefreshView();
        }

        private void OnModSelected(long index)
        {
            if (_syncing) return;
            var mods = SortedMods();
            if (index >= 0 && index < mods.Count) _selectedModId = mods[(int)index].id;
            RefreshView();
        }

        private void OnArmorSelected(long index)
        {
            if (_syncing) return;
            var grades = SortedArmorGrades();
            if (index >= 0 && index < grades.Count) _selectedArmorGradeId = grades[(int)index].id;
            RefreshView();
        }

        // ── Commands (Core owns all math and inventory mutation) ──────────

        private void OnInstallPressed()
        {
            if (_garage == null || _inventory == null) return;
            string vehicleId = SelectedVehicleId();
            var mod = FindMod(_selectedModId);
            if (string.IsNullOrEmpty(vehicleId) || mod == null) return;
            bool ok = _garage.InstallModification(vehicleId, mod.slot_type, mod.id, _inventory, out string reason);
            SetResult(ok, ok ? $"Fitted {mod.display_name}." : reason);
        }

        private void OnUninstallPressed()
        {
            if (_garage == null || _inventory == null) return;
            string vehicleId = SelectedVehicleId();
            var mod = FindMod(_selectedModId);
            if (string.IsNullOrEmpty(vehicleId) || mod == null) return;
            bool ok = _garage.UninstallModification(vehicleId, mod.slot_type, _inventory, out string reason);
            SetResult(ok, ok ? $"Removed {mod.display_name}." : reason);
        }

        private void OnInstallArmorPressed()
        {
            if (_garage == null || _inventory == null) return;
            string vehicleId = SelectedVehicleId();
            var grade = _garage.GetArmorGrade(_selectedArmorGradeId);
            if (string.IsNullOrEmpty(vehicleId) || grade == null) return;
            bool ok = _garage.InstallArmorGrade(vehicleId, grade.id, _inventory, out string reason);
            SetResult(ok, ok ? $"Fitted {grade.display_name}." : reason);
        }

        private void OnReforgeArmorPressed()
        {
            if (_garage == null || _inventory == null) return;
            string vehicleId = SelectedVehicleId();
            if (string.IsNullOrEmpty(vehicleId)) return;
            bool ok = _garage.ReforgeArmorPlate(vehicleId, _inventory, out string reason);
            SetResult(ok, ok ? "Armor plate re-forged to its stamped integrity." : reason);
        }

        private void OnServiceChassisPressed()
        {
            if (_garage == null || _inventory == null) return;
            string vehicleId = SelectedVehicleId();
            var record = _garage.GetRecord(vehicleId);
            if (record == null) return;
            bool ok = _garage.ServiceChassis(vehicleId, _inventory, Math.Max(1, record.chassisStressPermille), out string reason);
            SetResult(ok, ok ? "Chassis serviced to nominal." : reason);
        }

        private void OnServiceEnginePressed()
        {
            if (_garage == null || _inventory == null) return;
            string vehicleId = SelectedVehicleId();
            var record = _garage.GetRecord(vehicleId);
            if (record == null) return;
            bool ok = _garage.ServiceEngine(vehicleId, _inventory, Math.Max(1, record.engineFoulingPermille), out string reason);
            SetResult(ok, ok ? "Engine serviced to nominal." : reason);
        }

        private void OnServiceTransmissionPressed()
        {
            if (_garage == null || _inventory == null) return;
            string vehicleId = SelectedVehicleId();
            var record = _garage.GetRecord(vehicleId);
            if (record == null) return;
            bool ok = _garage.ServiceTransmission(vehicleId, _inventory, Math.Max(1, record.transmissionWearPermille), out string reason);
            SetResult(ok, ok ? "Transmission serviced to nominal." : reason);
        }

        private void OnCompleteRecoveryPressed()
        {
            if (_garage == null || _inventory == null) return;
            if (string.IsNullOrEmpty(_selectedRecoveryId)) return;
            bool ok = _garage.CompleteRecoveryMission(_selectedRecoveryId, _inventory, out string reason);
            SetResult(ok, ok ? "Recovery complete — vehicle returned to the garage." : reason);
        }

        private void SetResult(bool ok, string message)
        {
            LastFeedback = message;
            if (_commandResult != null)
            {
                _commandResult.Text = message;
                _commandResult.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(
                    ok ? DesignTheme.Success : DesignTheme.Warning));
            }
            RefreshView();
        }

        // ── Read-model rendering ──────────────────────────────────────────

        public void RefreshView()
        {
            if (_garage == null || _statusRail == null) return;

            _syncing = true;
            try
            {
                SyncVehicles();
                SyncMods();
                SyncArmorGrades();
            }
            finally
            {
                _syncing = false;
            }

            var ids = OwnedVehicleIds();
            int immobilized = 0;
            int fitted = 0;
            foreach (var id in ids)
            {
                if (_garage.IsImmobilized(id)) immobilized++;
                fitted += _garage.GetInstalledSlots(id).Count;
            }

            _statusRail.Set("vehicles", ids.Count.ToString(), AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("immobilized", immobilized.ToString(),
                immobilized > 0 ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("recoveries", _garage.ActiveRecoveries.Count.ToString(),
                _garage.ActiveRecoveries.Count > 0 ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("fitted", fitted.ToString(), AshfallMetricCard.Criticality.Normal);

            if (_detailText != null)
            {
                _detailText.Text =
                    $"Owned vehicles: {ids.Count} | Immobilized: {immobilized} | "
                    + $"Active recoveries: {_garage.ActiveRecoveries.Count} | Fitted modifications: {fitted}";
            }

            RenderVehicle();
            RenderFittedAndEffective();
            RenderMaintenance();
            RenderArmor();
            RenderRecovery();
        }

        private void SyncVehicles()
        {
            if (_vehicleSelector == null) return;
            _vehicleSelector.Clear();
            var ids = OwnedVehicleIds();
            int selected = 0;
            for (int i = 0; i < ids.Count; i++)
            {
                _vehicleSelector.AddItem(VehicleLabel(ids[i]), i);
                if (ids[i] == _selectedVehicleId) selected = i;
            }
            if (string.IsNullOrEmpty(_selectedVehicleId) && ids.Count > 0) _selectedVehicleId = ids[0];
            _vehicleSelector.Selected = selected;
        }

        private void SyncMods()
        {
            if (_modSelector == null) return;
            _modSelector.Clear();
            var mods = SortedMods();
            int selected = 0;
            for (int i = 0; i < mods.Count; i++)
            {
                _modSelector.AddItem($"{mods[i].display_name} [{mods[i].slot_type}]", i);
                if (mods[i].id == _selectedModId) selected = i;
            }
            if (string.IsNullOrEmpty(_selectedModId) && mods.Count > 0) _selectedModId = mods[0].id;
            _modSelector.Selected = selected;
        }

        private void SyncArmorGrades()
        {
            if (_armorSelector == null || _garage == null) return;
            _armorSelector.Clear();
            var grades = SortedArmorGrades();
            int selected = 0;
            for (int i = 0; i < grades.Count; i++)
            {
                _armorSelector.AddItem($"{grades[i].display_name} [T{grades[i].tier}]", i);
                if (grades[i].id == _selectedArmorGradeId) selected = i;
            }
            if (string.IsNullOrEmpty(_selectedArmorGradeId) && grades.Count > 0)
                _selectedArmorGradeId = grades[0].id;
            _armorSelector.Selected = selected;
        }

        private void RenderVehicle()
        {
            if (_vehicleText == null || _garage == null) return;
            string vehicleId = SelectedVehicleId();
            if (string.IsNullOrEmpty(vehicleId))
            {
                _vehicleText.Text = "No vehicle is owned. Acquire one through the expedition authority first.";
                return;
            }
            var record = _garage.GetRecord(vehicleId);
            if (record == null)
            {
                _vehicleText.Text = $"{VehicleLabel(vehicleId)}: no garage record yet — fitting a modification or running a sortie opens one.";
                return;
            }
            _vehicleText.Text =
                $"{VehicleLabel(vehicleId)} | Chassis {record.chassisStressPermille}/1000 | "
                + $"Engine {record.engineFoulingPermille}/1000 | Transmission {record.transmissionWearPermille}/1000 | "
                + $"{(record.isImmobilized ? "IMMOBILIZED — " + record.immobilizedReason : "SERVICEABLE")}";
        }

        private void RenderFittedAndEffective()
        {
            if (_modText == null || _garage == null) return;
            string vehicleId = SelectedVehicleId();
            var mod = FindMod(_selectedModId);

            if (mod != null)
            {
                var costs = new List<string>();
                foreach (var c in mod.install_cost) costs.Add($"{c.amount} × {c.item_id}");
                string effects =
                    $"cargo {mod.effects.cargo_capacity_delta:+0.#;-0.#;0}, speed {mod.effects.speed_multiplier_delta:+0.##;-0.##;0}, "
                    + $"fuel ×{mod.effects.fuel_consumption_multiplier:0.##}, wear ×{mod.effects.wear_rate_multiplier:0.##}, "
                    + $"cab rad protection {mod.effects.radiation_protection_permille}‰";
                _modCostText!.Text =
                    $"Slot: {mod.slot_type} | Install cost: {(costs.Count == 0 ? "none" : string.Join(", ", costs))}";
                _modText.Text = $"{mod.display_name} — {effects}\n{mod.description}";
            }
            else
            {
                _modCostText!.Text = "No modification selected.";
                _modText.Text = string.Empty;
            }

            if (_installButton != null) _installButton.Disabled = mod == null || string.IsNullOrEmpty(vehicleId);
            bool installed = mod != null && !string.IsNullOrEmpty(vehicleId)
                && _garage.GetInstalledSlots(vehicleId).TryGetValue(mod.slot_type, out var fittedId)
                && string.Equals(fittedId, mod.id, StringComparison.Ordinal);
            if (_uninstallButton != null) _uninstallButton.Disabled = !installed;

            var fittedSlots = string.IsNullOrEmpty(vehicleId) ? null : _garage.GetInstalledSlots(vehicleId);
            if (fittedSlots != null && fittedSlots.Count > 0 && !string.IsNullOrEmpty(vehicleId))
            {
                var lines = new List<string> { "FITTED:" };
                foreach (var kv in fittedSlots)
                {
                    var def = _garage.GetModification(kv.Value);
                    lines.Add($"  {kv.Key}: {def?.display_name ?? kv.Value}");
                }
                _modText.Text += "\n" + string.Join("\n", lines);
            }
        }

        private void RenderMaintenance()
        {
            if (_maintenanceText == null || _garage == null) return;
            string vehicleId = SelectedVehicleId();
            var record = string.IsNullOrEmpty(vehicleId) ? null : _garage.GetRecord(vehicleId);
            if (record == null)
            {
                _maintenanceText.Text = "No component wear on file for the selected vehicle.";
                SetServiceButtons(false);
                return;
            }

            int scrap = _inventory?.CountById("scrap_metal") ?? 0;
            int parts = _inventory?.CountById("mechanical_parts") ?? 0;
            _maintenanceText.Text =
                $"Chassis {record.chassisStressPermille}/1000 (scrap metal on hand {scrap}) | "
                + $"Engine {record.engineFoulingPermille}/1000, Transmission {record.transmissionWearPermille}/1000 "
                + $"(mechanical parts on hand {parts})";
            SetServiceButtons(true);
            if (_serviceChassisButton != null) _serviceChassisButton.Disabled = record.chassisStressPermille <= 0;
            if (_serviceEngineButton != null) _serviceEngineButton.Disabled = record.engineFoulingPermille <= 0;
            if (_serviceTransmissionButton != null) _serviceTransmissionButton.Disabled = record.transmissionWearPermille <= 0;
        }

        private void RenderArmor()
        {
            if (_armorText == null || _armorCostText == null || _garage == null) return;
            string vehicleId = SelectedVehicleId();
            var profile = string.IsNullOrEmpty(vehicleId)
                ? new VehicleArmorProfile { IsDefault = true, DisplayName = "Stock Plating", ConditionBand = "none" }
                : _garage.GetArmorProfile(vehicleId);
            var selected = _garage.GetArmorGrade(_selectedArmorGradeId);

            if (selected != null)
            {
                var costs = new List<string>();
                foreach (var cost in selected.install_cost) costs.Add($"{cost.amount} × {cost.item_id}");
                _armorCostText.Text = $"Install cost: {(costs.Count == 0 ? "none" : string.Join(", ", costs))} | labor {selected.install_labor_ticks} ticks";
            }
            else _armorCostText.Text = "No armor grade selected.";

            if (profile.IsDefault)
                _armorText.Text = "Stock Plating — no armor fitted.";
            else
            {
                string material = string.IsNullOrEmpty(profile.MaterialProfileId) ? "unknown material" : profile.MaterialProfileId;
                _armorText.Text = $"{profile.DisplayName} — mitigation {profile.MitigationPermille}‰, "
                    + $"absorption {profile.WearAbsorptionPermille}‰, integrity {profile.IntegrityPermille}/{profile.IntegrityMaxPermille} — "
                    + $"{profile.ConditionBand} (material {material}, {profile.Purity}).";
                if (profile.ConditionBand == "depleted")
                    _armorText.Text += " Mitigation inactive; mass penalty remains. Re-forge to restore.";
            }

            bool canInstall = selected != null && !string.IsNullOrEmpty(vehicleId)
                && _garage.CanInstallArmorGrade(vehicleId, selected.id, _inventory, out _);
            if (_armorInstallButton != null) _armorInstallButton.Disabled = !canInstall;
            if (_armorReforgeButton != null)
                _armorReforgeButton.Disabled = profile.IsDefault || profile.IntegrityPermille >= profile.IntegrityMaxPermille
                    || _garage.IsImmobilized(vehicleId);
        }

        private void SetServiceButtons(bool enabled)
        {
            if (_serviceChassisButton != null) _serviceChassisButton.Disabled = !enabled;
            if (_serviceEngineButton != null) _serviceEngineButton.Disabled = !enabled;
            if (_serviceTransmissionButton != null) _serviceTransmissionButton.Disabled = !enabled;
        }

        private void RenderRecovery()
        {
            if (_recoveryText == null || _garage == null) return;
            if (_garage.ActiveRecoveries.Count == 0)
            {
                _recoveryText.Text = "No active recovery operations.";
                _selectedRecoveryId = string.Empty;
                if (_completeRecoveryButton != null) _completeRecoveryButton.Disabled = true;
                return;
            }

            var lines = new List<string>();
            string firstComplete = string.Empty;
            foreach (var kv in _garage.ActiveRecoveries)
            {
                var m = kv.Value;
                if (m == null) continue;
                lines.Add($"{m.missionId}: {m.strandedVehicleId} stranded at {m.locationId} — "
                    + $"progress {m.progressTicks}/{m.requiredTicks} ticks, fuel {m.requiredFuelUnits}, "
                    + $"{(m.isComplete ? "READY TO RECOVER" : "team en route")}");
                if (m.isComplete && string.IsNullOrEmpty(firstComplete)) firstComplete = m.missionId;
            }
            _recoveryText.Text = string.Join("\n", lines);
            _selectedRecoveryId = firstComplete;
            if (_completeRecoveryButton != null) _completeRecoveryButton.Disabled = string.IsNullOrEmpty(firstComplete);
        }

        // ── Helpers ───────────────────────────────────────────────────────

        private List<string> OwnedVehicleIds()
        {
            var ids = new List<string>();
            if (_vehicles == null) return ids;
            foreach (var kv in _vehicles.State.ownedVehicles)
            {
                if (!string.IsNullOrEmpty(kv.Key)) ids.Add(kv.Key);
            }
            ids.Sort(StringComparer.Ordinal);
            return ids;
        }

        private string SelectedVehicleId()
        {
            if (!string.IsNullOrEmpty(_selectedVehicleId)) return _selectedVehicleId;
            var ids = OwnedVehicleIds();
            return ids.Count > 0 ? ids[0] : string.Empty;
        }

        private string VehicleLabel(string vehicleId)
        {
            var inst = _vehicles?.GetVehicle(vehicleId);
            string name = inst?.displayName ?? string.Empty;
            return string.IsNullOrWhiteSpace(name) ? vehicleId : $"{name} ({vehicleId})";
        }

        private List<VehicleModificationDefinition> SortedMods()
        {
            var list = new List<VehicleModificationDefinition>();
            if (_garage == null) return list;
            foreach (var kv in _garage.GetAllModifications())
                if (kv.Value != null) list.Add(kv.Value);
            list.Sort((a, b) => string.CompareOrdinal(a.id, b.id));
            return list;
        }

        private List<VehicleArmorGradeDefinition> SortedArmorGrades()
        {
            var list = new List<VehicleArmorGradeDefinition>();
            if (_garage == null) return list;
            foreach (var kv in _garage.GetAllArmorGrades())
                if (kv.Value != null && !kv.Value.is_default) list.Add(kv.Value);
            list.Sort((a, b) => a.tier != b.tier ? a.tier.CompareTo(b.tier) : string.CompareOrdinal(a.id, b.id));
            return list;
        }

        private VehicleModificationDefinition? FindMod(string modId)
        {
            if (_garage == null || string.IsNullOrEmpty(modId)) return null;
            return _garage.GetModification(modId);
        }
    }
}
