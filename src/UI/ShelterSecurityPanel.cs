// SPDX-License-Identifier: MIT
using System;
using System.Linq;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Ashfall.Core.UI;
using AtomicWar.GodotApp;

namespace AtomicWar.GodotApp.UI
{
    public partial class ShelterSecurityPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private HBoxContainer _splitBody = null!;
        private VBoxContainer _leftColumn = null!;
        private VBoxContainer _zonesContainer = null!;
        private VBoxContainer _rightColumn = null!;
        private Button _lockdownButton = null!;
        private VBoxContainer _breachesContainer = null!;
        private VBoxContainer _clearancesContainer = null!;

        private ShelterSecurityHostSession? _host;

        public bool IsBound => _host != null;

        public void Bind(ShelterSecurityHostSession session)
        {
            _host = session;
            if (_host != null)
            {
                _host.StateChanged += RefreshView;
            }
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

            _shell = new AshfallDashboardShell("Shelter Security // Access Control & Lockdown Protocols", minWidth: 1000, minHeight: 650);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("lockdown", "Lockdown State", "NORMAL", AshfallMetricCard.Criticality.Normal, minWidth: 130);
            _statusRail.AddCard("breaches", "Active Breaches", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("zones", "Protected Zones", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("clearances", "Clearances", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);

            _contentStack = new VBoxContainer();
            _contentStack.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingMd);
            _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;

            _splitBody = new HBoxContainer();
            _splitBody.AddThemeConstantOverride("separation", 16);
            _splitBody.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _splitBody.SizeFlagsVertical = SizeFlags.ExpandFill;

            // ── Left Column: Security Zones ─────────────────────────────
            _leftColumn = new VBoxContainer();
            _leftColumn.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingMd);
            _leftColumn.CustomMinimumSize = new Vector2(480, 400);
            _leftColumn.SizeFlagsHorizontal = SizeFlags.Fill;
            _leftColumn.SizeFlagsVertical = SizeFlags.ExpandFill;

            var zoneHdr = new Label();
            zoneHdr.Text = "SHELTER SECURITY ZONES & BULKHEADS";
            _leftColumn.AddChild(zoneHdr);

            var zoneScroll = new ScrollContainer();
            zoneScroll.SizeFlagsVertical = SizeFlags.ExpandFill;
            zoneScroll.SizeFlagsHorizontal = SizeFlags.ExpandFill;

            _zonesContainer = new VBoxContainer();
            _zonesContainer.AddThemeConstantOverride("separation", 8);
            _zonesContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            zoneScroll.AddChild(_zonesContainer);
            _leftColumn.AddChild(zoneScroll);

            _splitBody.AddChild(_leftColumn);

            // ── Right Column: Lockdown, Breaches & Clearances ───────────
            _rightColumn = new VBoxContainer();
            _rightColumn.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingMd);
            _rightColumn.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _rightColumn.SizeFlagsVertical = SizeFlags.ExpandFill;

            _lockdownButton = new Button();
            _lockdownButton.Text = "INITIATE EMERGENCY LOCKDOWN";
            _lockdownButton.Pressed += OnToggleLockdownPressed;
            _rightColumn.AddChild(_lockdownButton);

            var breachHdr = new Label();
            breachHdr.Text = "SECURITY ALARMS & BREACH ALERTS";
            _rightColumn.AddChild(breachHdr);

            _breachesContainer = new VBoxContainer();
            _breachesContainer.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _rightColumn.AddChild(_breachesContainer);

            var clearHdr = new Label();
            clearHdr.Text = "SURVIVOR CLEARANCE REGISTRY";
            _rightColumn.AddChild(clearHdr);

            var clearScroll = new ScrollContainer();
            clearScroll.SizeFlagsVertical = SizeFlags.ExpandFill;
            clearScroll.SizeFlagsHorizontal = SizeFlags.ExpandFill;

            _clearancesContainer = new VBoxContainer();
            _clearancesContainer.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _clearancesContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            clearScroll.AddChild(_clearancesContainer);
            _rightColumn.AddChild(clearScroll);

            _splitBody.AddChild(_rightColumn);

            _contentStack.AddChild(_splitBody);
            _shell.SetContent(_contentStack);

            _shell.AttachHeaderCloseButton("CLOSE", () =>
            {
                Visible = false;
                OnClose?.Invoke();
            });

            RefreshView();
        }

        private void OnToggleLockdownPressed()
        {
            if (_host == null) return;
            bool newLockdown = !_host.IsInLockdown;
            _host.SetShelterLockdown(newLockdown, currentDay: 1);
            RefreshView();
        }

        public void RefreshView()
        {
            if (_host == null || !IsInsideTree()) return;

            // 1. Update Status Rail
            bool isLockdown = _host.IsInLockdown;
            int breachesCount = _host.ActiveBreachCount;
            int zonesCount = _host.ZoneCount;
            int clearancesCount = _host.Clearances.Count;

            _statusRail?.Set("lockdown", isLockdown ? "LOCKDOWN" : "NORMAL",
                isLockdown ? AshfallMetricCard.Criticality.Critical : AshfallMetricCard.Criticality.Normal);

            _statusRail?.Set("breaches", breachesCount.ToString(),
                breachesCount > 0 ? AshfallMetricCard.Criticality.Critical : AshfallMetricCard.Criticality.Normal);

            _statusRail?.Set("zones", zonesCount.ToString());
            _statusRail?.Set("clearances", clearancesCount.ToString());

            _lockdownButton.Text = isLockdown ? "LIFT EMERGENCY LOCKDOWN" : "INITIATE EMERGENCY LOCKDOWN";

            // 2. Render Zones
            foreach (Node child in _zonesContainer.GetChildren())
            {
                child.QueueFree();
            }

            foreach (var zone in _host.Zones)
            {
                var card = new PanelContainer();
                var cardBox = new VBoxContainer();
                cardBox.AddThemeConstantOverride("separation", 4);

                var title = new Label();
                string alarmTag = zone.AlarmState != SecurityAlarmState.Normal ? $" [{zone.AlarmState.ToString().ToUpperInvariant()}]" : "";
                title.Text = $"{zone.ZoneName} ({zone.ZoneId}){alarmTag}";

                var details = new Label();
                details.Text = $"Level: {zone.Level} | Lock: {zone.LockState} | Room: {zone.RoomId}";

                var btnRow = new HBoxContainer();
                btnRow.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);

                var unlockBtn = new Button { Text = "Unlock" };
                unlockBtn.Pressed += () =>
                {
                    _host.SetDoorLockState(zone.ZoneId, DoorLockState.Unlocked);
                    RefreshView();
                };

                var lockBtn = new Button { Text = "Lock" };
                lockBtn.Pressed += () =>
                {
                    _host.SetDoorLockState(zone.ZoneId, DoorLockState.Locked);
                    RefreshView();
                };

                var sealBtn = new Button { Text = "Seal" };
                sealBtn.Pressed += () =>
                {
                    _host.SetDoorLockState(zone.ZoneId, DoorLockState.Sealed);
                    RefreshView();
                };

                btnRow.AddChild(unlockBtn);
                btnRow.AddChild(lockBtn);
                btnRow.AddChild(sealBtn);

                cardBox.AddChild(title);
                cardBox.AddChild(details);
                cardBox.AddChild(btnRow);
                card.AddChild(cardBox);
                _zonesContainer.AddChild(card);
            }

            // 3. Render Breaches
            foreach (Node child in _breachesContainer.GetChildren())
            {
                child.QueueFree();
            }

            var breaches = _host.GetActiveBreaches();
            if (breaches == null || breaches.Count == 0)
            {
                var noBreach = new Label();
                noBreach.Text = "All zones secure. No active alarms.";
                _breachesContainer.AddChild(noBreach);
            }
            else
            {
                foreach (var b in breaches)
                {
                    var breachCard = new PanelContainer();
                    var bBox = new VBoxContainer();
                    bBox.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingXs);

                    var bTitle = new Label();
                    bTitle.Text = $"[ALARM] {b.BreachType.ToUpperInvariant()} in {b.ZoneId}";

                    var bDetails = new Label();
                    bDetails.Text = $"Intruder: {b.IntruderId} | Day: {b.DetectedDay}";

                    var resolveBtn = new Button { Text = "Resolve & Reset Alarm" };
                    resolveBtn.Pressed += () =>
                    {
                        _host.ResolveBreach(b.BreachId, "Secured by player");
                        RefreshView();
                    };

                    bBox.AddChild(bTitle);
                    bBox.AddChild(bDetails);
                    bBox.AddChild(resolveBtn);
                    breachCard.AddChild(bBox);
                    _breachesContainer.AddChild(breachCard);
                }
            }

            // 4. Render Clearances
            foreach (Node child in _clearancesContainer.GetChildren())
            {
                child.QueueFree();
            }

            var clearances = _host.Clearances;
            if (clearances == null || clearances.Count == 0)
            {
                var noClear = new Label();
                noClear.Text = "No individual clearances granted. Survivors hold default clearance.";
                _clearancesContainer.AddChild(noClear);
            }
            else
            {
                foreach (var cl in clearances)
                {
                    var clCard = new PanelContainer();
                    var clBox = new VBoxContainer();
                    clBox.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingXs);

                    var clLbl = new Label();
                    clLbl.Text = $"{cl.SurvivorId} -> [{cl.Level}] (Day {cl.GrantedDay})";

                    var reasonLbl = new Label();
                    reasonLbl.Text = $"Granted By: {cl.GrantedBy} | Reason: {cl.Reason}";

                    clBox.AddChild(clLbl);
                    clBox.AddChild(reasonLbl);
                    clCard.AddChild(clBox);
                    _clearancesContainer.AddChild(clCard);
                }
            }
        }
    }
}
