// SPDX-License-Identifier: MIT
using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Radiation;
using Ashfall.Core.UI;
using AtomicWar.GodotApp;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Plan 138 Phase 3 — low-background metrology bench. Presentation only:
    /// shows detector baseline, shield class, calibration, detection limit, and
    /// assay results with honest uncertainty language. Never purifies, never
    /// claims zero activity or guaranteed-clean samples.
    /// </summary>
    public partial class LowBackgroundLeadPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private Label _detailText = null!;
        private Label _batchText = null!;
        private Button _assayBtn = null!;
        private Button _calibrateBtn = null!;
        private Button _startBatchBtn = null!;
        private Button _commitBatchBtn = null!;
        private OptionButton _profileSelector = null!;
        private string _selectedProfileId = string.Empty;

        private LowBackgroundMetrologyHostSession? _host;

        public bool IsBound => _host != null;

        public void Bind(LowBackgroundMetrologyHostSession session)
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

            _shell = new AshfallDashboardShell("Assay Bench // Low-Background Metrology", minWidth: 1000, minHeight: 650);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("detector", "Detector", "UNAVAILABLE", AshfallMetricCard.Criticality.Critical, minWidth: 140);
            _statusRail.AddCard("shield_class", "Shield Class", "NONE", AshfallMetricCard.Criticality.Normal, minWidth: 130);
            _statusRail.AddCard("detection_limit", "Detection Limit", "—", AshfallMetricCard.Criticality.Normal, minWidth: 130);
            _statusRail.AddCard("last_assay", "Last Assay", "—", AshfallMetricCard.Criticality.Normal, minWidth: 150);

            _contentStack = new VBoxContainer();
            _contentStack.AddThemeConstantOverride("separation", 12);
            _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;

            _detailText = new Label();
            _detailText.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            _contentStack.AddChild(_detailText);

            _contentStack.AddChild(AshfallUiHelpers.MakeSeparator());

            var calibrateHeader = AshfallUiHelpers.MakeSectionHeader("DETECTOR CALIBRATION");
            _contentStack.AddChild(calibrateHeader);
            _calibrateBtn = new Button { Text = "Calibrate Detector", CustomMinimumSize = new Vector2(200, 36) };
            _calibrateBtn.Pressed += () => _host?.Calibrate(SystemDay());
            _contentStack.AddChild(_calibrateBtn);

            _contentStack.AddChild(AshfallUiHelpers.MakeSeparator());

            var assayHeader = AshfallUiHelpers.MakeSectionHeader("SAMPLE ASSAY");
            _contentStack.AddChild(assayHeader);
            var assayRow = new HBoxContainer();
            assayRow.AddThemeConstantOverride("separation", 10);
            _assayBtn = new Button { Text = "Assay Latest Raw-Water Intake", CustomMinimumSize = new Vector2(280, 36) };
            _assayBtn.Pressed += () =>
            {
                if (_host == null) return;
                var (sampleId, contaminationBp) = _host.CurrentSample();
                _host.RunAssay(
                    new AssaySample(sampleId, contaminationBp),
                    Main.LowBackgroundNativeDetectorBp,
                    _host.EnvironmentBackgroundBp(),
                    6, 0.8, 0.5, SystemDay());
            };
            assayRow.AddChild(_assayBtn);
            _contentStack.AddChild(assayRow);

            var assayNote = AshfallUiHelpers.MakeBody(
                "Assay screening is advisory. A result below the detection limit means the sample could not be measured — it is not a guarantee the sample is clean.");
            assayNote.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            assayNote.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            _contentStack.AddChild(assayNote);

            _contentStack.AddChild(AshfallUiHelpers.MakeSeparator());

            var batchHeader = AshfallUiHelpers.MakeSectionHeader("SHIELD SMELTING BATCH");
            _contentStack.AddChild(batchHeader);

            var profileRow = new HBoxContainer();
            profileRow.AddThemeConstantOverride("separation", 10);
            var profileLabel = AshfallUiHelpers.MakeBody("Feedstock:");
            profileRow.AddChild(profileLabel);
            _profileSelector = new OptionButton { CustomMinimumSize = new Vector2(380, 36) };
            _profileSelector.ItemSelected += idx =>
            {
                if (_host != null && idx >= 0 && idx < _host.System.Catalog.All.Count)
                {
                    var profiles = new System.Collections.Generic.List<LowBackgroundMaterialDef>(_host.System.Catalog.All);
                    profiles.Sort((a, b) => string.CompareOrdinal(a.id, b.id));
                    if ((int)idx < profiles.Count)
                        _selectedProfileId = profiles[(int)idx].id;
                }
            };
            profileRow.AddChild(_profileSelector);
            _contentStack.AddChild(profileRow);

            var batchRow = new HBoxContainer();
            batchRow.AddThemeConstantOverride("separation", 10);
            _startBatchBtn = new Button { Text = "Start Batch (10 units)", CustomMinimumSize = new Vector2(200, 36) };
            _startBatchBtn.Pressed += () => _host?.StartBatch(_selectedProfileId, 10, SystemDay());
            batchRow.AddChild(_startBatchBtn);

            _commitBatchBtn = new Button { Text = "Commit Open Batch", CustomMinimumSize = new Vector2(200, 36) };
            _commitBatchBtn.Pressed += () =>
            {
                var open = FindOpenBatch();
                if (open != null) _host?.CommitBatch(open.BatchId);
            };
            batchRow.AddChild(_commitBatchBtn);
            _contentStack.AddChild(batchRow);

            _batchText = new Label();
            _batchText.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            _contentStack.AddChild(_batchText);

            _shell.SetContent(_contentStack);

            _shell.AttachHeaderCloseButton("CLOSE", () =>
            {
                Visible = false;
                OnClose?.Invoke();
            });

            RefreshView();
        }

        private int SystemDay()
        {
            // Panel-level day accessor — host session owns the authoritative day.
            return AppDayProvider?.Invoke() ?? 1;
        }

        /// <summary>Wired by Main; panel does not reach into campaign state itself.</summary>
        public Func<int>? AppDayProvider { get; set; }

        private LowBackgroundBatch? FindOpenBatch()
        {
            if (_host == null) return null;
            var batches = _host.System.State.Batches;
            for (int i = batches.Count - 1; i >= 0; i--)
                if (!batches[i].Committed) return batches[i];
            return null;
        }

        public void RefreshView()
        {
            if (_host == null || _statusRail == null) return;

            var s = _host.System.State;
            bool installed = s.DetectorInstalled;

            _statusRail.Set("detector",
                !installed ? "UNAVAILABLE" : (s.DetectorCalibrated ? $"CALIBRATED (d{s.CalibratedOnDay})" : "UNCALIBRATED"),
                !installed ? AshfallMetricCard.Criticality.Critical
                    : s.DetectorCalibrated ? AshfallMetricCard.Criticality.Normal
                    : AshfallMetricCard.Criticality.Caution);

            string shieldClass = "NONE";
            if (s.InstalledModuleProfileIds.Count > 0)
            {
                int residual = _host.System.InstalledResidualBp();
                shieldClass = residual < 25 ? "VERY LOW" : residual < 50 ? "LOW" : "ORDINARY";
            }
            _statusRail.Set("shield_class", shieldClass,
                shieldClass == "NONE" ? AshfallMetricCard.Criticality.Normal
                : shieldClass == "ORDINARY" ? AshfallMetricCard.Criticality.Caution
                : AshfallMetricCard.Criticality.Normal);

            int detectionLimit = s.DetectorInstalled
                ? Math.Max(5, 10 + _host.System.EffectiveBackgroundBp(Main.LowBackgroundNativeDetectorBp, _host.EnvironmentBackgroundBp()) / 4)
                : 0;
            _statusRail.Set("detection_limit", detectionLimit > 0 ? $"{detectionLimit} bp" : "—", AshfallMetricCard.Criticality.Normal);

            var last = s.AssayHistory.Count > 0 ? s.AssayHistory[s.AssayHistory.Count - 1] : null;
            _statusRail.Set("last_assay",
                last == null ? "—" : $"{last.EstimatedBand} ({last.Confidence:P0})",
                last != null && last.Detected ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);

            if (_detailText != null)
            {
                string baselineLine = installed
                    ? $"Effective bench background: {_host.System.EffectiveBackgroundBp(Main.LowBackgroundNativeDetectorBp, _host.EnvironmentBackgroundBp())} bp of ordinary baseline | Shield modules: {s.InstalledModuleProfileIds.Count}"
                    : "Detector not installed — no assay capability.";
                string historyLine = last == null
                    ? "No assays recorded yet."
                    : $"Last assay (day {last.Day}): {(last.BelowDetectionLimit ? "below detection limit — indeterminate, not clean" : (last.Detected ? $"contamination {last.EstimatedBand}" : "no contamination above limit"))} at {last.Confidence:P0} confidence.";
                _detailText.Text = $"{baselineLine}\n" +
                                   $"{historyLine}\n" +
                                   $"Total assays: {s.TotalAssays} | Environmental baseline: {_host.EnvironmentBackgroundBp()} bp\n" +
                                   $"Last event: {_host.LastEvent}";
            }

            if (_profileSelector != null)
            {
                _profileSelector.Clear();
                var profiles = new System.Collections.Generic.List<LowBackgroundMaterialDef>(_host.System.Catalog.All);
                profiles.Sort((a, b) => string.CompareOrdinal(a.id, b.id));
                int selectedIdx = 0;
                for (int i = 0; i < profiles.Count; i++)
                {
                    var def = profiles[i];
                    _profileSelector.AddItem($"{def.display_name} [{def.background_activity_class}] — residual {def.residual_activity_bp} bp", i);
                    if (def.id == _selectedProfileId || (string.IsNullOrEmpty(_selectedProfileId) && i == 0))
                    {
                        selectedIdx = i;
                        _selectedProfileId = def.id;
                    }
                }
                _profileSelector.Selected = selectedIdx;
                _startBatchBtn.Disabled = !installed || profiles.Count == 0;
                _assayBtn.Disabled = !installed;
                _calibrateBtn.Disabled = !installed;
            }

            if (_batchText != null)
            {
                var open = FindOpenBatch();
                _batchText.Text = open == null
                    ? "No open batch. Select feedstock and start a batch."
                    : $"Open batch {open.BatchId}: {open.UnitsCommitted} units, class {open.BackgroundClass}{(open.Contaminated ? $" — CONTAMINATED ({open.ContaminationNote})" : string.Empty)}.";
            }
        }

        public override void _ExitTree()
        {
            Unbind();
            base._ExitTree();
        }
    }
}
