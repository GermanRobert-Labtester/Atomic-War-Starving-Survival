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
    /// ASHFALL — Cohort Library & Tech Study Management Interface.
    /// Manages study manuals, research progression, skill gains, and reader assignments.
    /// </summary>
    public partial class LibraryStudyPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _manualList = null!;
        private VBoxContainer _studyDesk = null!;
        private VBoxContainer _studyLogContainer = null!;
        private Label _eventLogLabel = null!;

        private LibraryStudyHostSession? _host;
        private string? _selectedManualId;

        public bool IsBound => _host != null;

        public void Bind(LibraryStudyHostSession session)
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
            Visible = false;

            var bg = new ColorRect { Color = new Color(0.04f, 0.04f, 0.05f, 0.92f) };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            var center = new CenterContainer();
            center.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(center);

            _shell = new AshfallDashboardShell("SYS: COHORT LIBRARY & TECH STUDY // STUDY MATRIX", minWidth: 1040, minHeight: 680);
            center.AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("manuals", "ARCHIVED MANUALS", "0", AshfallMetricCard.Criticality.Normal, minWidth: 130);
            _statusRail.AddCard("active_study", "ACTIVE READERS", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("completed", "COMPLETED", "0", AshfallMetricCard.Criticality.Normal, minWidth: 110);
            _statusRail.AddCard("study_speed", "LITERACY BUFF", "1.0x", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("status", "LIBRARY DESK", "ONLINE", AshfallMetricCard.Criticality.Normal, minWidth: 120);

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

            // Column 1: Manuals List
            var leftPanel = AshfallUiHelpers.MakePanel(minWidth: 310);
            leftPanel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            leftPanel.SizeFlagsStretchRatio = 0.95f;
            var leftMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingSm);
            leftPanel.AddChild(leftMargin);
            var leftVbox = new VBoxContainer();
            leftVbox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            leftMargin.AddChild(leftVbox);
            leftVbox.AddChild(AshfallUiHelpers.MakeSectionHeader("ARCHIVED FIELD MANUALS"));
            var leftScroll = new ScrollContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            _manualList = new VBoxContainer();
            _manualList.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
            _manualList.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            leftScroll.AddChild(_manualList);
            leftVbox.AddChild(leftScroll);
            gridRow.AddChild(leftPanel);

            // Column 2: Study Desk & Reader Assignment
            var centerPanel = AshfallUiHelpers.MakePanel(minWidth: 380);
            centerPanel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            centerPanel.SizeFlagsStretchRatio = 1.2f;
            var centerMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingSm);
            centerPanel.AddChild(centerMargin);
            var centerVbox = new VBoxContainer();
            centerVbox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            centerMargin.AddChild(centerVbox);
            centerVbox.AddChild(AshfallUiHelpers.MakeSectionHeader("STUDY DESK & READER SELECTION"));
            var centerScroll = new ScrollContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            _studyDesk = new VBoxContainer();
            _studyDesk.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            _studyDesk.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            centerScroll.AddChild(_studyDesk);
            centerVbox.AddChild(centerScroll);
            gridRow.AddChild(centerPanel);

            // Column 3: Reading Logs & Progression
            var rightPanel = AshfallUiHelpers.MakePanel(minWidth: 310);
            rightPanel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            rightPanel.SizeFlagsStretchRatio = 0.95f;
            var rightMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingSm);
            rightPanel.AddChild(rightMargin);
            var rightVbox = new VBoxContainer();
            rightVbox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            rightMargin.AddChild(rightVbox);
            rightVbox.AddChild(AshfallUiHelpers.MakeSectionHeader("STUDY SESSIONS LOG"));
            var rightScroll = new ScrollContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            _studyLogContainer = new VBoxContainer();
            _studyLogContainer.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            _studyLogContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            rightScroll.AddChild(_studyLogContainer);
            rightVbox.AddChild(rightScroll);

            rightVbox.AddChild(AshfallUiHelpers.MakeSeparator());
            _eventLogLabel = AshfallUiHelpers.MakeMetadata("No recent study events.");
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
            if (_manualList == null || _studyDesk == null || _studyLogContainer == null) return;

            AshfallUiHelpers.EmptyChildren(_manualList);
            AshfallUiHelpers.EmptyChildren(_studyDesk);
            AshfallUiHelpers.EmptyChildren(_studyLogContainer);

            if (_host == null || _statusRail == null)
            {
                _manualList.AddChild(AshfallUiHelpers.MakeEmptyStateLabel("No library study session bound", "offline"));
                _studyDesk.AddChild(AshfallUiHelpers.MakeEmptyStateLabel("Study desk offline", "offline"));
                _studyLogContainer.AddChild(AshfallUiHelpers.MakeEmptyStateLabel("Study log unavailable", "offline"));
                return;
            }

            var s = _host.System.State;
            var catalog = _host.System.Catalog.Values.ToList();
            int totalManuals = catalog.Count;
            int activeJobs = s.activeJobs.Count;
            int completedJobs = s.completedManualIds.Count;

            _statusRail.Set("manuals", totalManuals.ToString(), AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("active_study", activeJobs.ToString(), activeJobs > 0 ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("completed", completedJobs.ToString(), AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("study_speed", "1.25x", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("status", activeJobs > 0 ? "STUDYING" : "IDLE", AshfallMetricCard.Criticality.Normal);

            if (!string.IsNullOrEmpty(_host.LastEvent))
            {
                _eventLogLabel.Text = _host.LastEvent;
            }

            if (catalog.Count == 0)
            {
                _manualList.AddChild(AshfallUiHelpers.MakeMetadata("No manuals cataloged in library archive."));
            }
            else
            {
                if (_selectedManualId == null || !catalog.Exists(m => m.manual_id == _selectedManualId))
                {
                    _selectedManualId = catalog[0].manual_id;
                }

                foreach (var manual in catalog)
                {
                    bool isCompleted = s.completedManualIds.Contains(manual.manual_id);
                    bool isInProgress = s.activeJobs.Exists(j => j.manualId == manual.manual_id);

                    var card = AshfallUiHelpers.MakePanel();
                    var cardMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingXs);
                    card.AddChild(cardMargin);
                    var cardVbox = new VBoxContainer();
                    cardVbox.AddThemeConstantOverride("separation", 3);
                    cardMargin.AddChild(cardVbox);

                    var headerRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
                    headerRow.AddChild(AshfallUiHelpers.MakeBadgeIcon(isCompleted ? "badge_crossing_terms" : "badge_scurvy", 18));
                    var nameLbl = AshfallUiHelpers.MakeBody(manual.display_name);
                    nameLbl.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                    headerRow.AddChild(nameLbl);
                    cardVbox.AddChild(headerRow);

                    var skillLbl = AshfallUiHelpers.MakeMono($"DISCIPLINE: {manual.category} ({manual.studyHoursRequired}h Study)");
                    skillLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
                    cardVbox.AddChild(skillLbl);

                    var statusLbl = AshfallUiHelpers.MakeSmall(isCompleted ? "STATUS: [COMPLETED]" : isInProgress ? "STATUS: [IN PROGRESS]" : "STATUS: [AVAILABLE]");
                    statusLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(isCompleted ? DesignTheme.Lethe : isInProgress ? DesignTheme.Hot : DesignTheme.Pale));
                    cardVbox.AddChild(statusLbl);

                    var selectBtn = AshfallUiHelpers.MakeButton($"SELECT // {manual.manual_id}", () =>
                    {
                        _selectedManualId = manual.manual_id;
                        RefreshView();
                    });
                    selectBtn.CustomMinimumSize = new Vector2(0, 24);
                    cardVbox.AddChild(selectBtn);

                    _manualList.AddChild(card);
                }
            }

            // Study Desk Inspector
            var curManual = catalog.FirstOrDefault(m => m.manual_id == _selectedManualId);
            if (curManual != null)
            {
                bool isCompleted = s.completedManualIds.Contains(curManual.manual_id);
                var activeJob = s.activeJobs.FirstOrDefault(j => j.manualId == curManual.manual_id);

                _studyDesk.AddChild(AshfallUiHelpers.MakeSectionHeader($"MANUAL: {curManual.display_name.ToUpperInvariant()}"));
                _studyDesk.AddChild(AshfallUiHelpers.MakeDataRow("Manual ID", curManual.manual_id, AshfallUiHelpers.ToColor(DesignTheme.Pale)));
                _studyDesk.AddChild(AshfallUiHelpers.MakeDataRow("Target Discipline", curManual.category, AshfallUiHelpers.ToColor(DesignTheme.Lethe)));
                _studyDesk.AddChild(AshfallUiHelpers.MakeDataRow("Study Duration", $"{curManual.studyHoursRequired} Hours", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
                _studyDesk.AddChild(AshfallUiHelpers.MakeDataRow("Archival Status", isCompleted ? "Fully Mastered & Transcribed" : activeJob != null ? $"Under Study ({activeJob.progressHours:F0}/{curManual.studyHoursRequired}h)" : "On Shelf", AshfallUiHelpers.ToColor(isCompleted ? DesignTheme.Lethe : activeJob != null ? DesignTheme.Hot : DesignTheme.Dim)));

                _studyDesk.AddChild(AshfallUiHelpers.MakeSeparator());
                _studyDesk.AddChild(AshfallUiHelpers.MakeSubsectionHeader("READER STATUS"));

                if (isCompleted)
                {
                    _studyDesk.AddChild(AshfallUiHelpers.MakeBody("Manual has been fully mastered and transcribed across the shelter."));
                }
                else if (activeJob != null)
                {
                    _studyDesk.AddChild(AshfallUiHelpers.MakeBody($"Reader {activeJob.readerId.ToUpperInvariant()} is currently assigned to study ({activeJob.progressHours:F0}/{curManual.studyHoursRequired}h)."));
                }
                else
                {
                    _studyDesk.AddChild(AshfallUiHelpers.MakeBody("Manual is available on archive shelf for reader study assignment through the Duty Roster."));
                }
            }
            else
            {
                _studyDesk.AddChild(AshfallUiHelpers.MakeMetadata("Select a manual from the library archive to assign reader."));
            }

            // Study Sessions Log
            if (s.activeJobs.Count == 0 && s.completedManualIds.Count == 0)
            {
                _studyLogContainer.AddChild(AshfallUiHelpers.MakeMetadata("No active study sessions."));
            }
            else
            {
                foreach (var job in s.activeJobs)
                {
                    _studyLogContainer.AddChild(AshfallUiHelpers.MakeMono($"[STUDYING] {job.manualId} by {job.readerId} ({job.progressHours:F0}h logged)"));
                }
                foreach (var doneId in s.completedManualIds)
                {
                    _studyLogContainer.AddChild(AshfallUiHelpers.MakeMono($"[MASTERED] {doneId}"));
                }
            }
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
            Unbind();
            base._ExitTree();
        }
    }
}
