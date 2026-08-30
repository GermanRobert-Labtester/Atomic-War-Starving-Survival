using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Medical;
using Ashfall.Core.UI;
using AtomicWar.GodotApp;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Medical Ward & Trauma Triage Management Interface.
    /// High-density terminal UI corresponding to Stitch screen d2da12a6fdaa41d1a7451f1241cba24b.
    /// Manages ward beds, patient admission/discharge, surgical procedures, and trauma triage.
    /// </summary>
    public partial class MedicalWardPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _bedGrid = null!;
        private VBoxContainer _inspectorContainer = null!;
        private VBoxContainer _procedureQueueContainer = null!;
        private Label _eventLogLabel = null!;

        private MedicalWardHostSession? _host;
        private string? _selectedBedId;

        public bool IsBound => _host != null;

        public void Bind(MedicalWardHostSession session)
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

            _shell = new AshfallDashboardShell("SYS: TRAUMA WARD & SURGICAL TRIAGE BAY-03", minWidth: 1060, minHeight: 680);
            center.AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("beds", "BED OCCUPANCY", "0/0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("admitted", "ACTIVE PATIENTS", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("procedures", "PROCEDURES RUN", "0", AshfallMetricCard.Criticality.Normal, minWidth: 130);
            _statusRail.AddCard("infection", "INFECTION RISK", "LOW", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("airlock", "AIRLOCK STATUS", "ENGAGED", AshfallMetricCard.Criticality.Caution, minWidth: 130);

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

            // Column 1: Ward Bed Grid
            var leftPanel = AshfallUiHelpers.MakePanel(minWidth: 320);
            leftPanel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            leftPanel.SizeFlagsStretchRatio = 0.95f;
            var leftMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingSm);
            leftPanel.AddChild(leftMargin);
            var leftVbox = new VBoxContainer();
            leftVbox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            leftMargin.AddChild(leftVbox);
            leftVbox.AddChild(AshfallUiHelpers.MakeSectionHeader("WARD BED MATRIX"));
            var leftScroll = new ScrollContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            _bedGrid = new VBoxContainer();
            _bedGrid.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
            _bedGrid.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            leftScroll.AddChild(_bedGrid);
            leftVbox.AddChild(leftScroll);
            gridRow.AddChild(leftPanel);

            // Column 2: Patient Triage & Vitals Inspector
            var centerPanel = AshfallUiHelpers.MakePanel(minWidth: 380);
            centerPanel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            centerPanel.SizeFlagsStretchRatio = 1.2f;
            var centerMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingSm);
            centerPanel.AddChild(centerMargin);
            var centerVbox = new VBoxContainer();
            centerVbox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            centerMargin.AddChild(centerVbox);
            centerVbox.AddChild(AshfallUiHelpers.MakeSectionHeader("BAY TELEMETRY & PROCEDURES"));
            var centerScroll = new ScrollContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            _inspectorContainer = new VBoxContainer();
            _inspectorContainer.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            _inspectorContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            centerScroll.AddChild(_inspectorContainer);
            centerVbox.AddChild(centerScroll);
            gridRow.AddChild(centerPanel);

            // Column 3: Surgical Protocols & Supply Logs
            var rightPanel = AshfallUiHelpers.MakePanel(minWidth: 310);
            rightPanel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            rightPanel.SizeFlagsStretchRatio = 0.95f;
            var rightMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingSm);
            rightPanel.AddChild(rightMargin);
            var rightVbox = new VBoxContainer();
            rightVbox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            rightMargin.AddChild(rightVbox);
            rightVbox.AddChild(AshfallUiHelpers.MakeSectionHeader("SURGICAL CATALOG & LOGS"));
            var rightScroll = new ScrollContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            _procedureQueueContainer = new VBoxContainer();
            _procedureQueueContainer.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            _procedureQueueContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            rightScroll.AddChild(_procedureQueueContainer);
            rightVbox.AddChild(rightScroll);

            rightVbox.AddChild(AshfallUiHelpers.MakeSeparator());
            _eventLogLabel = AshfallUiHelpers.MakeMetadata("No recent ward events.");
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

            AshfallUiHelpers.EmptyChildren(_bedGrid);
            AshfallUiHelpers.EmptyChildren(_inspectorContainer);
            AshfallUiHelpers.EmptyChildren(_procedureQueueContainer);

            var beds = _host.System.Beds;
            var admissions = _host.System.State.Admissions;
            int totalBeds = beds.Count;
            int occupiedBeds = admissions.Count(a => a.Status == MedicalAdmissionStatus.Active);
            int proceduresRunCount = _host.System.State.ProceduresRun.Count;

            _statusRail.Set("beds", $"{occupiedBeds}/{totalBeds}", occupiedBeds >= totalBeds ? AshfallMetricCard.Criticality.Critical : occupiedBeds > 0 ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("admitted", $"{occupiedBeds}", occupiedBeds > 0 ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("procedures", $"{proceduresRunCount}", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("infection", occupiedBeds > 2 ? "ELEVATED" : "LOW", occupiedBeds > 2 ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("airlock", "SEALED", AshfallMetricCard.Criticality.Normal);

            if (!string.IsNullOrEmpty(_host.LastEvent))
            {
                _eventLogLabel.Text = _host.LastEvent;
            }

            if (_selectedBedId == null && beds.Count > 0)
            {
                _selectedBedId = beds[0].BedId;
            }

            // Populate Bed Grid
            foreach (var bed in beds)
            {
                var occupant = _host.System.GetBedOccupant(bed.BedId);
                bool isOccupied = !string.IsNullOrEmpty(occupant);

                var bedCard = AshfallUiHelpers.MakePanel();
                var cardMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingXs);
                bedCard.AddChild(cardMargin);
                var cardVbox = new VBoxContainer();
                cardVbox.AddThemeConstantOverride("separation", 3);
                cardMargin.AddChild(cardVbox);

                var headerRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
                headerRow.AddChild(AshfallUiHelpers.MakeBadgeIcon(bed.Isolation ? "badge_rad_sickness" : "badge_exhaustion", 18));
                var bedLbl = AshfallUiHelpers.MakeBody($"{bed.DisplayName} [{bed.Category}]");
                bedLbl.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                headerRow.AddChild(bedLbl);
                cardVbox.AddChild(headerRow);

                var statusRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
                var statusLbl = AshfallUiHelpers.MakeMono(isOccupied ? $"PATIENT: {FormatSurvivorName(occupant!)}" : "STATUS: EMPTY / STERILIZED");
                statusLbl.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                statusLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(isOccupied ? DesignTheme.Warm : DesignTheme.Lethe));
                statusRow.AddChild(statusLbl);
                cardVbox.AddChild(statusRow);

                var btnSelect = AshfallUiHelpers.MakeButton(isOccupied ? $"INSPECT [{bed.BedId}]" : $"SELECT BED [{bed.BedId}]", () =>
                {
                    _selectedBedId = bed.BedId;
                    RefreshView();
                });
                btnSelect.CustomMinimumSize = new Vector2(0, 24);
                cardVbox.AddChild(btnSelect);

                _bedGrid.AddChild(bedCard);
            }

            // Populate Center Inspector
            var currentBed = beds.FirstOrDefault(b => b.BedId == _selectedBedId);
            if (currentBed != null)
            {
                var occupant = _host.System.GetBedOccupant(currentBed.BedId);
                var activeAdmission = occupant != null ? _host.System.GetActiveAdmission(occupant) : null;

                _inspectorContainer.AddChild(AshfallUiHelpers.MakeSectionHeader($"BED UNIT: {currentBed.DisplayName.ToUpperInvariant()}"));
                _inspectorContainer.AddChild(AshfallUiHelpers.MakeDataRow("Category", currentBed.Category.ToString(), AshfallUiHelpers.ToColor(DesignTheme.Lethe)));
                _inspectorContainer.AddChild(AshfallUiHelpers.MakeDataRow("Isolation Protocol", currentBed.Isolation ? "BIOHAZARD ISOLATION SEALED" : "STANDARD VENTILATION", AshfallUiHelpers.ToColor(currentBed.Isolation ? DesignTheme.Critical : DesignTheme.Pale)));
                _inspectorContainer.AddChild(AshfallUiHelpers.MakeDataRow("Occupancy State", occupant != null ? $"OCCUPIED by {FormatSurvivorName(occupant)}" : "VACANT", AshfallUiHelpers.ToColor(occupant != null ? DesignTheme.Warm : DesignTheme.Dim)));

                if (activeAdmission != null)
                {
                    _inspectorContainer.AddChild(AshfallUiHelpers.MakeDataRow("Admitted Day", $"Sim Day {activeAdmission.AdmittedDay}", AshfallUiHelpers.ToColor(DesignTheme.Pale)));
                    _inspectorContainer.AddChild(AshfallUiHelpers.MakeSeparator());
                    _inspectorContainer.AddChild(AshfallUiHelpers.MakeSubsectionHeader("AVAILABLE PROCEDURES"));

                    foreach (var proc in _host.System.Procedures)
                    {
                        // Task #133 P1b: through the host wrapper — pipeline
                        // treatment first (bandage/chelation), ward log only on
                        // success; refusals surface in the event log.
                        var btnProc = AshfallUiHelpers.MakeButton($"RUN {proc.DisplayName.ToUpperInvariant()}", () =>
                        {
                            _host.RunProcedure(activeAdmission.PatientId, proc.ProcedureId, _host.SimDay);
                            RefreshView();
                        });
                        _inspectorContainer.AddChild(btnProc);
                    }

                    var btnDischarge = AshfallUiHelpers.MakeButton("DISCHARGE PATIENT", () =>
                    {
                        _host.System.Discharge(activeAdmission.PatientId, _host.SimDay);
                        RefreshView();
                    });
                    _inspectorContainer.AddChild(btnDischarge);
                }
                else
                {
                    _inspectorContainer.AddChild(AshfallUiHelpers.MakeSeparator());
                    _inspectorContainer.AddChild(AshfallUiHelpers.MakeBody("Bed unit is currently vacant and sterilized. Patients requiring intensive trauma triage or surgical intervention will be admitted from shelter wards."));
                }
            }
            else
            {
                _inspectorContainer.AddChild(AshfallUiHelpers.MakeMetadata("Select a bed from the matrix to inspect telemetry and run procedures."));
            }

            // Populate Procedures Catalog
            _procedureQueueContainer.AddChild(AshfallUiHelpers.MakeSectionHeader("PROCEDURE CATALOG"));
            foreach (var proc in _host.System.Procedures)
            {
                _procedureQueueContainer.AddChild(AshfallUiHelpers.MakeDataRow(proc.DisplayName, $"System: {proc.DelegatedSystemId}", AshfallUiHelpers.ToColor(DesignTheme.Pale)));
            }

            _procedureQueueContainer.AddChild(AshfallUiHelpers.MakeSeparator());
            _procedureQueueContainer.AddChild(AshfallUiHelpers.MakeSubsectionHeader("RECENT PROCEDURES RUN"));
            var recentRuns = _host.System.State.ProceduresRun;
            if (recentRuns.Count == 0)
            {
                _procedureQueueContainer.AddChild(AshfallUiHelpers.MakeMetadata("No procedures logged yet."));
            }
            else
            {
                foreach (var rec in recentRuns.TakeLast(5))
                {
                    _procedureQueueContainer.AddChild(AshfallUiHelpers.MakeMono($"Day {rec.Day}: {rec.ProcedureId} on {FormatSurvivorName(rec.PatientId)} ({rec.BedId})"));
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
