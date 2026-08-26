using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Ashfall.Core;
using Ashfall.Core.UI;
using AtomicWar.GodotApp;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Equipment Condition & Maintenance Workbench Panel.
    /// Manages gear wear & tear, slip risk, maintenance stations, and part repairs.
    /// </summary>
    public partial class EquipmentConditionPanel : Control
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _gearList = null!;
        private VBoxContainer _workbenchInspector = null!;
        private VBoxContainer _maintenanceLogContainer = null!;
        private Label _eventLogLabel = null!;

        private EquipmentConditionHostSession? _host;
        private string? _selectedInstanceId;

        public bool IsBound => _host != null;

        public void Bind(EquipmentConditionHostSession session)
        {
            if (_host != null)
            {
                _host.StateChanged -= RefreshView;
            }
            _host = session;
            if (_host != null)
            {
                _host.StateChanged += RefreshView;
            }
            RefreshView();
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            var bg = new ColorRect { Color = new Color(0.04f, 0.04f, 0.05f, 0.92f) };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            var center = new CenterContainer();
            center.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(center);

            _shell = new AshfallDashboardShell("SYS: ARMORY WORKBENCH & CONDITION // DURABILITY MATRIX", minWidth: 1040, minHeight: 680);
            center.AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("tracked_items", "TRACKED GEAR", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("in_maintenance", "IN REPAIR", "0", AshfallMetricCard.Criticality.Normal, minWidth: 110);
            _statusRail.AddCard("critical_wear", "CRITICAL WEAR", "0", AshfallMetricCard.Criticality.Normal, minWidth: 130);
            _statusRail.AddCard("slip_risk", "FAILURE RISK", "LOW", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("status", "WORKBENCH", "ONLINE", AshfallMetricCard.Criticality.Normal, minWidth: 120);

            _shell.AttachHeaderCloseButton("CLOSE [Esc]", () =>
            {
                Visible = false;
                OnClose?.Invoke();
            });

            // 3-Column Layout
            var gridRow = new HBoxContainer();
            gridRow.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
            gridRow.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            gridRow.SizeFlagsVertical = SizeFlags.ExpandFill;

            // Column 1: Tracked Equipment List
            var leftPanel = AshfallUiHelpers.MakePanel(minWidth: 310);
            leftPanel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            leftPanel.SizeFlagsStretchRatio = 0.95f;
            var leftMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingSm);
            leftPanel.AddChild(leftMargin);
            var leftVbox = new VBoxContainer();
            leftVbox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            leftMargin.AddChild(leftVbox);
            leftVbox.AddChild(AshfallUiHelpers.MakeSectionHeader("ARMORY INVENTORY"));
            var leftScroll = new ScrollContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            _gearList = new VBoxContainer();
            _gearList.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
            _gearList.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            leftScroll.AddChild(_gearList);
            leftVbox.AddChild(leftScroll);
            gridRow.AddChild(leftPanel);

            // Column 2: Workbench Inspector & Maintenance Actions
            var centerPanel = AshfallUiHelpers.MakePanel(minWidth: 380);
            centerPanel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            centerPanel.SizeFlagsStretchRatio = 1.2f;
            var centerMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingSm);
            centerPanel.AddChild(centerMargin);
            var centerVbox = new VBoxContainer();
            centerVbox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            centerMargin.AddChild(centerVbox);
            centerVbox.AddChild(AshfallUiHelpers.MakeSectionHeader("DIAGNOSTICS & REPAIR BENCH"));
            var centerScroll = new ScrollContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            _workbenchInspector = new VBoxContainer();
            _workbenchInspector.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            _workbenchInspector.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            centerScroll.AddChild(_workbenchInspector);
            centerVbox.AddChild(centerScroll);
            gridRow.AddChild(centerPanel);

            // Column 3: Maintenance Log
            var rightPanel = AshfallUiHelpers.MakePanel(minWidth: 310);
            rightPanel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            rightPanel.SizeFlagsStretchRatio = 0.95f;
            var rightMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingSm);
            rightPanel.AddChild(rightMargin);
            var rightVbox = new VBoxContainer();
            rightVbox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            rightMargin.AddChild(rightVbox);
            rightVbox.AddChild(AshfallUiHelpers.MakeSectionHeader("WORKBENCH LOGS"));
            var rightScroll = new ScrollContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            _maintenanceLogContainer = new VBoxContainer();
            _maintenanceLogContainer.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            _maintenanceLogContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            rightScroll.AddChild(_maintenanceLogContainer);
            rightVbox.AddChild(rightScroll);

            rightVbox.AddChild(AshfallUiHelpers.MakeSeparator());
            _eventLogLabel = AshfallUiHelpers.MakeMetadata("No recent maintenance events.");
            _eventLogLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            rightVbox.AddChild(_eventLogLabel);

            gridRow.AddChild(rightPanel);

            _shell.SetContent(gridRow);
            RefreshView();
        }

        public void Open()
        {
            Visible = true;
            RefreshView();
            QueueRedraw();
        }

        public void RefreshView()
        {
            if (_host == null || _statusRail == null) return;

            AshfallUiHelpers.EmptyChildren(_gearList);
            AshfallUiHelpers.EmptyChildren(_workbenchInspector);
            AshfallUiHelpers.EmptyChildren(_maintenanceLogContainer);

            var s = _host.System.State;
            int totalGear = s.items.Count;
            int inRepair = s.pendingJobs.Count(j => !j.isComplete);
            int criticalWear = s.items.Count(i => i.maxCondition > 0f && (i.condition / i.maxCondition < 0.25f));

            _statusRail.Set("tracked_items", totalGear.ToString(), AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("in_repair", inRepair.ToString(), inRepair > 0 ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("critical_wear", criticalWear.ToString(), criticalWear > 0 ? AshfallMetricCard.Criticality.Critical : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("slip_risk", criticalWear > 0 ? "ELEVATED" : "NOMINAL", criticalWear > 0 ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("status", inRepair > 0 ? "REPAIRING" : "READY", AshfallMetricCard.Criticality.Normal);

            if (!string.IsNullOrEmpty(_host.LastEvent))
            {
                _eventLogLabel.Text = _host.LastEvent;
            }

            // If empty, allow seeding
            if (s.items.Count == 0)
            {
                _gearList.AddChild(AshfallUiHelpers.MakeMetadata("No equipment registered in armory."));
                var btnSeed = AshfallUiHelpers.MakeButton("REGISTER STANDARD LOADOUT", () =>
                {
                    _host.RegisterItem("eq_rifle_01", "assault_rifle_ak", "survivor_gunner_mikhail", EquipmentFamily.Weapon, 100f);
                    _host.RegisterItem("eq_hazmat_01", "hazmat_suit_lead", "elena_vasquez", EquipmentFamily.Clothing, 100f);
                    _host.RegisterItem("eq_gasmask_01", "gas_mask_m40", "the_teacher", EquipmentFamily.Clothing, 80f);
                    _host.RegisterItem("eq_tool_01", "multitool_heavy", "survivor_dweller_1", EquipmentFamily.Tool, 90f);
                    RefreshView();
                });
                _gearList.AddChild(btnSeed);
            }
            else
            {
                if (_selectedInstanceId == null || !s.items.Exists(i => i.instanceId == _selectedInstanceId))
                {
                    _selectedInstanceId = s.items[0].instanceId;
                }

                foreach (var item in s.items)
                {
                    var card = AshfallUiHelpers.MakePanel();
                    var cardMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingXs);
                    card.AddChild(cardMargin);
                    var cardVbox = new VBoxContainer();
                    cardVbox.AddThemeConstantOverride("separation", 3);
                    cardMargin.AddChild(cardVbox);

                    var headerRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
                    headerRow.AddChild(AshfallUiHelpers.MakeBadgeIcon(item.condition < 30f ? "badge_corneal_burn" : "badge_exhaustion", 18));
                    var nameLbl = AshfallUiHelpers.MakeBody($"{item.itemId.ToUpperInvariant()} [{item.family}]");
                    nameLbl.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                    headerRow.AddChild(nameLbl);
                    cardVbox.AddChild(headerRow);

                    float ratio = item.maxCondition > 0f ? item.condition / item.maxCondition : 0f;
                    var condRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
                    var condLbl = AshfallUiHelpers.MakeMono($"CONDITION: {BuildGauge(ratio)} {ratio:P0}");
                    condLbl.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                    condLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(ratio < 0.25f ? DesignTheme.Critical : ratio < 0.6f ? DesignTheme.Warm : DesignTheme.Pale));
                    condRow.AddChild(condLbl);
                    cardVbox.AddChild(condRow);

                    var ownerLbl = AshfallUiHelpers.MakeSmall($"OWNER: {item.ownerId}");
                    ownerLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Dim));
                    cardVbox.AddChild(ownerLbl);

                    var selectBtn = AshfallUiHelpers.MakeButton($"BENCH INSPECT // {item.instanceId}", () =>
                    {
                        _selectedInstanceId = item.instanceId;
                        RefreshView();
                    });
                    selectBtn.CustomMinimumSize = new Vector2(0, 24);
                    cardVbox.AddChild(selectBtn);

                    _gearList.AddChild(card);
                }
            }

            // Workbench Inspector
            var curItem = s.items.FirstOrDefault(i => i.instanceId == _selectedInstanceId);
            if (curItem != null)
            {
                float ratio = curItem.maxCondition > 0f ? curItem.condition / curItem.maxCondition : 0f;
                float slipRisk = _host.GetSlipRisk(curItem.instanceId);

                _workbenchInspector.AddChild(AshfallUiHelpers.MakeSectionHeader($"ITEM: {curItem.itemId.ToUpperInvariant()}"));
                _workbenchInspector.AddChild(AshfallUiHelpers.MakeDataRow("Instance ID", curItem.instanceId, AshfallUiHelpers.ToColor(DesignTheme.Pale)));
                _workbenchInspector.AddChild(AshfallUiHelpers.MakeDataRow("Equipment Family", curItem.family.ToString(), AshfallUiHelpers.ToColor(DesignTheme.Lethe)));
                _workbenchInspector.AddChild(AshfallUiHelpers.MakeDataRow("Durability Condition", $"{BuildGauge(ratio)} {curItem.condition:F0}/{curItem.maxCondition:F0} ({ratio:P0})", AshfallUiHelpers.ToColor(ratio < 0.3f ? DesignTheme.Critical : DesignTheme.Warm)));
                _workbenchInspector.AddChild(AshfallUiHelpers.MakeDataRow("Failure / Jam Risk", $"{slipRisk:P1}", AshfallUiHelpers.ToColor(slipRisk > 0.3f ? DesignTheme.Critical : DesignTheme.Pale)));
                _workbenchInspector.AddChild(AshfallUiHelpers.MakeDataRow("Assigned Owner", curItem.ownerId, AshfallUiHelpers.ToColor(DesignTheme.Pale)));

                _workbenchInspector.AddChild(AshfallUiHelpers.MakeSeparator());
                _workbenchInspector.AddChild(AshfallUiHelpers.MakeSubsectionHeader("WORKBENCH ACTIONS"));

                var btnMaintain = AshfallUiHelpers.MakeButton("ROUTINE FIELD MAINTENANCE (CLEAN & SHARPEN)", () =>
                {
                    _host.StartMaintenance(curItem.instanceId, "station_workbench_01", MaintenanceType.Sharpen, new List<string>());
                    RefreshView();
                });
                _workbenchInspector.AddChild(btnMaintain);

                var btnOverhaul = AshfallUiHelpers.MakeButton("COMPLETE OVERHAUL & PARTS REPLACEMENT", () =>
                {
                    _host.StartMaintenance(curItem.instanceId, "station_workbench_01", MaintenanceType.Repair, new List<string> { "scrap_mechanical" });
                    RefreshView();
                });
                _workbenchInspector.AddChild(btnOverhaul);

                var btnWear = AshfallUiHelpers.MakeButton("SIMULATE FIELD USE (WEAR -15%)", () =>
                {
                    _host.UseItem(curItem.instanceId, 15f);
                    RefreshView();
                });
                _workbenchInspector.AddChild(btnWear);
            }
            else
            {
                _workbenchInspector.AddChild(AshfallUiHelpers.MakeMetadata("Select an item from the armory to perform maintenance."));
            }

            // Maintenance Log
            if (s.pendingJobs.Count == 0)
            {
                _maintenanceLogContainer.AddChild(AshfallUiHelpers.MakeMetadata("No items currently on workbench."));
            }
            else
            {
                foreach (var job in s.pendingJobs)
                {
                    _maintenanceLogContainer.AddChild(AshfallUiHelpers.MakeMono($"[{(job.isComplete ? "DONE" : "IN PROGRESS")}] {job.instanceId} at {job.stationId} ({job.type})"));
                }
            }
        }

        private static string BuildGauge(float ratio)
        {
            int totalBars = 8;
            int filled = Math.Clamp((int)Math.Round(ratio * totalBars), 0, totalBars);
            return "[" + new string('|', filled) + new string('-', totalBars - filled) + "]";
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (!Visible) return;
            if (@event is InputEventKey key && key.Pressed && key.Keycode == Key.Escape)
            {
                OnClose?.Invoke();
                Visible = false;
                GetViewport().SetInputAsHandled();
            }
        }

        public override void _ExitTree()
        {
            if (_host != null)
            {
                _host.StateChanged -= RefreshView;
            }
            base._ExitTree();
        }
    }
}
