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
    /// ASHFALL — Psychiatric Ward & Crisis Intervention Interface.
    /// Manages survivor acute trauma, psychotic breaks, grief breakdowns,
    /// caregiver therapies, and work-fitness lockout.
    /// </summary>
    public partial class MentalHealthCrisisPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _caseList = null!;
        private VBoxContainer _interventionDesk = null!;
        private VBoxContainer _crisisLogContainer = null!;
        private Label _eventLogLabel = null!;

        private MentalHealthCrisisHostSession? _host;
        private string? _selectedCaseId;

        public bool IsBound => _host != null;

        public void Bind(MentalHealthCrisisHostSession session)
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

            _shell = new AshfallDashboardShell("SYS: PSYCHIATRIC WARD & CRISIS INTERVENTION // TRIAGE MATRIX", minWidth: 1040, minHeight: 680);
            center.AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("in_crisis", "ACUTE CRISIS", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("in_treatment", "IN THERAPY", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("work_unfit", "UNFIT FOR DUTY", "0", AshfallMetricCard.Criticality.Normal, minWidth: 130);
            _statusRail.AddCard("ward_state", "ISOLATION WARD", "READY", AshfallMetricCard.Criticality.Normal, minWidth: 130);
            _statusRail.AddCard("status", "PSYCH STATUS", "STABLE", AshfallMetricCard.Criticality.Normal, minWidth: 120);

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

            // Column 1: Active Mental Health Cases
            var leftPanel = AshfallUiHelpers.MakePanel(minWidth: 310);
            leftPanel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            leftPanel.SizeFlagsStretchRatio = 0.95f;
            var leftMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingSm);
            leftPanel.AddChild(leftMargin);
            var leftVbox = new VBoxContainer();
            leftVbox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            leftMargin.AddChild(leftVbox);
            leftVbox.AddChild(AshfallUiHelpers.MakeSectionHeader("PSYCH PATIENT ROSTER"));
            var leftScroll = new ScrollContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            _caseList = new VBoxContainer();
            _caseList.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
            _caseList.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            leftScroll.AddChild(_caseList);
            leftVbox.AddChild(leftScroll);
            gridRow.AddChild(leftPanel);

            // Column 2: Intervention Desk & Therapy Assignment
            var centerPanel = AshfallUiHelpers.MakePanel(minWidth: 380);
            centerPanel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            centerPanel.SizeFlagsStretchRatio = 1.2f;
            var centerMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingSm);
            centerPanel.AddChild(centerMargin);
            var centerVbox = new VBoxContainer();
            centerVbox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            centerMargin.AddChild(centerVbox);
            centerVbox.AddChild(AshfallUiHelpers.MakeSectionHeader("CRISIS INTERVENTION & CARE"));
            var centerScroll = new ScrollContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            _interventionDesk = new VBoxContainer();
            _interventionDesk.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            _interventionDesk.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            centerScroll.AddChild(_interventionDesk);
            centerVbox.AddChild(centerScroll);
            gridRow.AddChild(centerPanel);

            // Column 3: Telemetry & Logs
            var rightPanel = AshfallUiHelpers.MakePanel(minWidth: 310);
            rightPanel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            rightPanel.SizeFlagsStretchRatio = 0.95f;
            var rightMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingSm);
            rightPanel.AddChild(rightMargin);
            var rightVbox = new VBoxContainer();
            rightVbox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            rightMargin.AddChild(rightVbox);
            rightVbox.AddChild(AshfallUiHelpers.MakeSectionHeader("INCIDENTS & RECOVERY"));
            var rightScroll = new ScrollContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            _crisisLogContainer = new VBoxContainer();
            _crisisLogContainer.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            _crisisLogContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            rightScroll.AddChild(_crisisLogContainer);
            rightVbox.AddChild(rightScroll);

            rightVbox.AddChild(AshfallUiHelpers.MakeSeparator());
            _eventLogLabel = AshfallUiHelpers.MakeMetadata("No recent psychiatric events.");
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

            AshfallUiHelpers.EmptyChildren(_caseList);
            AshfallUiHelpers.EmptyChildren(_interventionDesk);
            AshfallUiHelpers.EmptyChildren(_crisisLogContainer);

            var s = _host.System.State;
            int inCrisisCount = s.activeCases.Count(c => c.status == CrisisStatus.Active);
            int inTreatmentCount = s.activeCases.Count(c => c.status == CrisisStatus.InTreatment);
            int workUnfitCount = inCrisisCount + inTreatmentCount;

            _statusRail.Set("in_crisis", inCrisisCount.ToString(), inCrisisCount > 0 ? AshfallMetricCard.Criticality.Critical : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("in_treatment", inTreatmentCount.ToString(), inTreatmentCount > 0 ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("work_unfit", workUnfitCount.ToString(), workUnfitCount > 0 ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("ward_state", inTreatmentCount > 0 ? "OCCUPIED" : "VACANT", inTreatmentCount > 0 ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("status", inCrisisCount > 0 ? "TRAUMA ALERT" : "STABLE", inCrisisCount > 0 ? AshfallMetricCard.Criticality.Critical : AshfallMetricCard.Criticality.Normal);

            if (!string.IsNullOrEmpty(_host.LastEvent))
            {
                _eventLogLabel.Text = _host.LastEvent;
            }

            // Populate Cases
            if (s.activeCases.Count == 0)
            {
                _caseList.AddChild(AshfallUiHelpers.MakeMetadata("No survivors currently in psychological crisis."));
                var btnTrigger = AshfallUiHelpers.MakeButton("SIMULATE TRAUMA BREAKDOWN", () =>
                {
                    _host.TriggerCrisis("survivor_gunner_mikhail", 85f, CrisisProfile.AcuteStress);
                    RefreshView();
                });
                _caseList.AddChild(btnTrigger);
            }
            else
            {
                if (_selectedCaseId == null || !s.activeCases.Exists(c => c.caseId == _selectedCaseId))
                {
                    _selectedCaseId = s.activeCases[0].caseId;
                }

                foreach (var c in s.activeCases)
                {
                    var card = AshfallUiHelpers.MakePanel();
                    var cardMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingXs);
                    card.AddChild(cardMargin);
                    var cardVbox = new VBoxContainer();
                    cardVbox.AddThemeConstantOverride("separation", 3);
                    cardMargin.AddChild(cardVbox);

                    var headerRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
                    headerRow.AddChild(AshfallUiHelpers.MakeBadgeIcon("badge_insomnia", 18));
                    var nameLbl = AshfallUiHelpers.MakeBody(FormatSurvivorName(c.survivorId));
                    nameLbl.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                    headerRow.AddChild(nameLbl);
                    cardVbox.AddChild(headerRow);

                    var statusRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
                    var statusLbl = AshfallUiHelpers.MakeMono($"PROFILE: [{c.profile}] // [{c.status}]");
                    statusLbl.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                    statusLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(c.status == CrisisStatus.Active ? DesignTheme.Critical : DesignTheme.Lethe));
                    statusRow.AddChild(statusLbl);
                    cardVbox.AddChild(statusRow);

                    var selectBtn = AshfallUiHelpers.MakeButton($"TRIAGE // {c.caseId}", () =>
                    {
                        _selectedCaseId = c.caseId;
                        RefreshView();
                    });
                    selectBtn.CustomMinimumSize = new Vector2(0, 24);
                    cardVbox.AddChild(selectBtn);

                    _caseList.AddChild(card);
                }
            }

            // Intervention Desk
            var curCase = s.activeCases.FirstOrDefault(c => c.caseId == _selectedCaseId);
            if (curCase != null)
            {
                _interventionDesk.AddChild(AshfallUiHelpers.MakeSectionHeader($"PATIENT: {FormatSurvivorName(curCase.survivorId)}"));
                _interventionDesk.AddChild(AshfallUiHelpers.MakeDataRow("Crisis Case ID", curCase.caseId, AshfallUiHelpers.ToColor(DesignTheme.Pale)));
                _interventionDesk.AddChild(AshfallUiHelpers.MakeDataRow("Diagnostic Profile", curCase.profile.ToString(), AshfallUiHelpers.ToColor(DesignTheme.Critical)));
                _interventionDesk.AddChild(AshfallUiHelpers.MakeDataRow("Duty Fitness", "UNFIT - LOCKED OUT OF ALL WORK", AshfallUiHelpers.ToColor(DesignTheme.Hot)));
                _interventionDesk.AddChild(AshfallUiHelpers.MakeDataRow("Caregiver Assigned", string.IsNullOrEmpty(curCase.assignedCaregiverId) ? "None (Awaiting Intervention)" : FormatSurvivorName(curCase.assignedCaregiverId), AshfallUiHelpers.ToColor(DesignTheme.Lethe)));

                _interventionDesk.AddChild(AshfallUiHelpers.MakeSeparator());
                _interventionDesk.AddChild(AshfallUiHelpers.MakeSubsectionHeader("TREATMENT PROTOCOLS"));

                var btnCounsel = AshfallUiHelpers.MakeButton("COMMENCE COUNSELING (THE TEACHER)", () =>
                {
                    _host.BeginTreatment(curCase.caseId, "the_teacher", "Counseling");
                    RefreshView();
                });
                btnCounsel.Disabled = curCase.status == CrisisStatus.InTreatment;
                _interventionDesk.AddChild(btnCounsel);

                var btnElena = AshfallUiHelpers.MakeButton("ADMINISTER SEDATION (ELENA VASQUEZ)", () =>
                {
                    _host.BeginTreatment(curCase.caseId, "elena_vasquez", "Sedation");
                    RefreshView();
                });
                btnElena.Disabled = curCase.status == CrisisStatus.InTreatment;
                _interventionDesk.AddChild(btnElena);
            }
            else
            {
                _interventionDesk.AddChild(AshfallUiHelpers.MakeMetadata("Select a patient from the roster to dispatch psychiatric care."));
            }

            // Populate Log
            _crisisLogContainer.AddChild(AshfallUiHelpers.MakeSectionHeader("ACTIVE PSYCH WARD LOG"));
            if (s.activeCases.Count == 0)
            {
                _crisisLogContainer.AddChild(AshfallUiHelpers.MakeMetadata("No psychological incidents on record."));
            }
            else
            {
                foreach (var c in s.activeCases)
                {
                    _crisisLogContainer.AddChild(AshfallUiHelpers.MakeMono($"[{c.status}] {FormatSurvivorName(c.survivorId)} ({c.profile})"));
                }
            }
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
            Unbind();
            base._ExitTree();
        }
    }
}
