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
    /// ASHFALL — Airlock Decontamination & Scrubber Management Interface.
    /// Manages hazmat washdown showers, surface radiation clearing, decon queues,
    /// and shelter air contamination mitigation.
    /// </summary>
    public partial class DecontaminationPanel : Control
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _queueList = null!;
        private VBoxContainer _caseInspector = null!;
        private VBoxContainer _incidentLogContainer = null!;
        private Label _eventLogLabel = null!;

        private DecontaminationHostSession? _host;
        private string? _selectedCaseId;

        public bool IsBound => _host != null;

        public void Bind(DecontaminationHostSession session)
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

            _shell = new AshfallDashboardShell("SYS: AIRLOCK DECONTAMINATION // SCRUBBER MATRIX", minWidth: 1040, minHeight: 680);
            center.AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("queue", "QUEUED CASES", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("active", "CHAMBER STATE", "VACANT", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("shelter_rad", "SHELTER CONTAM", "0.0%", AshfallMetricCard.Criticality.Normal, minWidth: 130);
            _statusRail.AddCard("safe_release", "AIRLOCK SEAL", "SECURED", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("status", "WASHDOWN PUMPS", "READY", AshfallMetricCard.Criticality.Caution, minWidth: 130);

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

            // Column 1: Decon Queue List
            var leftPanel = AshfallUiHelpers.MakePanel(minWidth: 310);
            leftPanel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            leftPanel.SizeFlagsStretchRatio = 0.95f;
            var leftMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingSm);
            leftPanel.AddChild(leftMargin);
            var leftVbox = new VBoxContainer();
            leftVbox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            leftMargin.AddChild(leftVbox);
            leftVbox.AddChild(AshfallUiHelpers.MakeSectionHeader("DECON CASE QUEUE"));
            var leftScroll = new ScrollContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            _queueList = new VBoxContainer();
            _queueList.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
            _queueList.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            leftScroll.AddChild(_queueList);
            leftVbox.AddChild(leftScroll);
            gridRow.AddChild(leftPanel);

            // Column 2: Active Chamber Inspector & Scrub Controls
            var centerPanel = AshfallUiHelpers.MakePanel(minWidth: 380);
            centerPanel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            centerPanel.SizeFlagsStretchRatio = 1.2f;
            var centerMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingSm);
            centerPanel.AddChild(centerMargin);
            var centerVbox = new VBoxContainer();
            centerVbox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            centerMargin.AddChild(centerVbox);
            centerVbox.AddChild(AshfallUiHelpers.MakeSectionHeader("WASHDOWN CHAMBER & SCRUB"));
            var centerScroll = new ScrollContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            _caseInspector = new VBoxContainer();
            _caseInspector.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            _caseInspector.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            centerScroll.AddChild(_caseInspector);
            centerVbox.AddChild(centerScroll);
            gridRow.AddChild(centerPanel);

            // Column 3: Incident Log & Diagnostics
            var rightPanel = AshfallUiHelpers.MakePanel(minWidth: 310);
            rightPanel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            rightPanel.SizeFlagsStretchRatio = 0.95f;
            var rightMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingSm);
            rightPanel.AddChild(rightMargin);
            var rightVbox = new VBoxContainer();
            rightVbox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            rightMargin.AddChild(rightVbox);
            rightVbox.AddChild(AshfallUiHelpers.MakeSectionHeader("DECON LOGS & QUEUING"));
            var rightScroll = new ScrollContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            _incidentLogContainer = new VBoxContainer();
            _incidentLogContainer.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            _incidentLogContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            rightScroll.AddChild(_incidentLogContainer);
            rightVbox.AddChild(rightScroll);

            rightVbox.AddChild(AshfallUiHelpers.MakeSeparator());
            _eventLogLabel = AshfallUiHelpers.MakeMetadata("No recent decon events.");
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
            if (_queueList == null || _caseInspector == null || _incidentLogContainer == null) return;

            AshfallUiHelpers.EmptyChildren(_queueList);
            AshfallUiHelpers.EmptyChildren(_caseInspector);
            AshfallUiHelpers.EmptyChildren(_incidentLogContainer);

            if (_host == null || _statusRail == null)
            {
                _queueList.AddChild(AshfallUiHelpers.MakeEmptyStateLabel("No decontamination session bound", "offline"));
                _caseInspector.AddChild(AshfallUiHelpers.MakeEmptyStateLabel("Airlock chamber offline", "offline"));
                _incidentLogContainer.AddChild(AshfallUiHelpers.MakeEmptyStateLabel("Incident log unavailable", "offline"));
                return;
            }

            var s = _host.System.State;
            int queuedCount = s.queue.Count(c => c.status == DeconStatus.Queued);
            bool chamberActive = _host.System.HasActiveCase;

            _statusRail.Set("queue", queuedCount.ToString(), queuedCount > 0 ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("active", chamberActive ? "IN CYCLE" : "VACANT", chamberActive ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("shelter_rad", $"{s.shelterContaminationLevel:P1}", s.shelterContaminationLevel > 0.3f ? AshfallMetricCard.Criticality.Critical : s.shelterContaminationLevel > 0.1f ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("safe_release", s.shelterContaminated ? "COMPROMISED" : "SEALED", s.shelterContaminated ? AshfallMetricCard.Criticality.Critical : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("status", chamberActive ? "DECONTAMINATING" : "READY", chamberActive ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);

            if (!string.IsNullOrEmpty(_host.LastEvent))
            {
                _eventLogLabel.Text = _host.LastEvent;
            }

            // Populate Queue List
            if (s.queue.Count == 0 && s.activeCase == null)
            {
                _queueList.AddChild(AshfallUiHelpers.MakeMetadata("No survivors waiting in airlock queue."));
            }
            else
            {
                var allCases = new List<DeconCase>();
                if (s.activeCase != null) allCases.Add(s.activeCase);
                allCases.AddRange(s.queue);

                if (_selectedCaseId == null || !allCases.Exists(c => c.caseId == _selectedCaseId))
                {
                    _selectedCaseId = allCases[0].caseId;
                }

                foreach (var c in allCases)
                {
                    var caseCard = AshfallUiHelpers.MakePanel();
                    var cardMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingXs);
                    caseCard.AddChild(cardMargin);
                    var cardVbox = new VBoxContainer();
                    cardVbox.AddThemeConstantOverride("separation", 3);
                    cardMargin.AddChild(cardVbox);

                    var headerRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
                    headerRow.AddChild(AshfallUiHelpers.MakeBadgeIcon(c.surfaceContamination > 0.5f ? "badge_radon_poisoning" : "badge_exhaustion", 18));
                    var nameLbl = AshfallUiHelpers.MakeBody(FormatSurvivorName(c.survivorId));
                    nameLbl.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                    headerRow.AddChild(nameLbl);
                    cardVbox.AddChild(headerRow);

                    var statusRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
                    var statusLbl = AshfallUiHelpers.MakeMono($"SURFACE CONTAM: {BuildGauge(c.surfaceContamination)} {c.surfaceContamination:P0}");
                    statusLbl.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                    statusLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(c.surfaceContamination > 0.5f ? DesignTheme.Critical : DesignTheme.Warm));
                    statusRow.AddChild(statusLbl);
                    cardVbox.AddChild(statusRow);

                    var stageLbl = AshfallUiHelpers.MakeSmall($"STATUS: [{c.status}] // GEAR: {c.gearId}");
                    stageLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(c.status == DeconStatus.InProgress ? DesignTheme.Lethe : DesignTheme.Pale));
                    cardVbox.AddChild(stageLbl);

                    var selectBtn = AshfallUiHelpers.MakeButton($"SELECT // {c.caseId}", () =>
                    {
                        _selectedCaseId = c.caseId;
                        RefreshView();
                    });
                    selectBtn.CustomMinimumSize = new Vector2(0, 24);
                    cardVbox.AddChild(selectBtn);

                    _queueList.AddChild(caseCard);
                }
            }

            // Populate Active Case Inspector
            var selectedCase = (s.activeCase?.caseId == _selectedCaseId ? s.activeCase : null)
                ?? s.queue.FirstOrDefault(c => c.caseId == _selectedCaseId)
                ?? s.activeCase;

            if (selectedCase != null)
            {
                _caseInspector.AddChild(AshfallUiHelpers.MakeSectionHeader($"CASE: {FormatSurvivorName(selectedCase.survivorId)}"));
                _caseInspector.AddChild(AshfallUiHelpers.MakeDataRow("Case Reference", selectedCase.caseId, AshfallUiHelpers.ToColor(DesignTheme.Pale)));
                _caseInspector.AddChild(AshfallUiHelpers.MakeDataRow("Contaminated Gear", selectedCase.gearId, AshfallUiHelpers.ToColor(DesignTheme.Warm)));
                _caseInspector.AddChild(AshfallUiHelpers.MakeDataRow("Surface Fallout Dust", $"{BuildGauge(selectedCase.surfaceContamination)} {selectedCase.surfaceContamination:P1}", AshfallUiHelpers.ToColor(selectedCase.surfaceContamination > 0.4f ? DesignTheme.Critical : DesignTheme.Hot)));
                _caseInspector.AddChild(AshfallUiHelpers.MakeDataRow("Pre-Decon Total Dose", $"{selectedCase.radiationDoseBeforeDecon:F1} mSv", AshfallUiHelpers.ToColor(DesignTheme.Pale)));
                _caseInspector.AddChild(AshfallUiHelpers.MakeDataRow("Current Status", selectedCase.status.ToString(), AshfallUiHelpers.ToColor(selectedCase.status == DeconStatus.InProgress ? DesignTheme.Lethe : DesignTheme.Dim)));

                _caseInspector.AddChild(AshfallUiHelpers.MakeSeparator());
                _caseInspector.AddChild(AshfallUiHelpers.MakeSubsectionHeader("DECONTAMINATION ACTIONS"));

                var btnProcess = AshfallUiHelpers.MakeButton("START DECON CYCLE (PROCESS QUEUE)", () =>
                {
                    _host.ProcessQueue();
                    RefreshView();
                });
                btnProcess.Disabled = chamberActive && selectedCase.status != DeconStatus.Queued;
                _caseInspector.AddChild(btnProcess);

                var btnSafeRelease = AshfallUiHelpers.MakeButton("COMPLETE SAFE AIRLOCK RELEASE", () =>
                {
                    _host.CompleteCycle(safeRelease: true);
                    RefreshView();
                });
                btnSafeRelease.Disabled = !chamberActive;
                _caseInspector.AddChild(btnSafeRelease);

                var btnBypass = AshfallUiHelpers.MakeButton("EMERGENCY AIRLOCK BYPASS (UNSAFE)", () =>
                {
                    _host.CompleteCycle(safeRelease: false);
                    RefreshView();
                });
                btnBypass.Disabled = !chamberActive;
                _caseInspector.AddChild(btnBypass);
            }
            else
            {
                _caseInspector.AddChild(AshfallUiHelpers.MakeMetadata("Chamber is empty. Queue returning scavengers for surface scrub down."));
            }

            // Quick Enqueue & Incidents
            _incidentLogContainer.AddChild(AshfallUiHelpers.MakeSectionHeader("QUEUE SCAVENGER"));
            var btnQueueDemo = AshfallUiHelpers.MakeButton("ENQUEUE HAZMAT RETURNEE", () =>
            {
                string id = $"survivor_dweller_{s.queue.Count + 1}";
                _host.Enqueue(id, "hazmat_suit", 0.75f);
                RefreshView();
            });
            _incidentLogContainer.AddChild(btnQueueDemo);

            _incidentLogContainer.AddChild(AshfallUiHelpers.MakeSeparator());
            _incidentLogContainer.AddChild(AshfallUiHelpers.MakeSubsectionHeader("AIRLOCK LOGS"));

            if (s.incidentLog.Count == 0)
            {
                _incidentLogContainer.AddChild(AshfallUiHelpers.MakeMetadata("No recent decon incidents."));
            }
            else
            {
                foreach (var inc in s.incidentLog.TakeLast(6))
                {
                    _incidentLogContainer.AddChild(AshfallUiHelpers.MakeMono($"Day {inc.day} [{inc.caseId}]: {inc.description}"));
                }
            }
        }

        private static string BuildGauge(float ratio)
        {
            int totalBars = 8;
            int filled = Math.Clamp((int)Math.Round(ratio * totalBars), 0, totalBars);
            return "[" + new string('|', filled) + new string('-', totalBars - filled) + "]";
        }

        private static string FormatSurvivorName(string id)
        {
            if (string.IsNullOrEmpty(id)) return "UNKNOWN";
            return id.Replace("survivor_", "").Replace("_", " ").ToUpperInvariant();
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
