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
    /// ASHFALL — Archival Script & Transcription Desk Management Interface.
    /// Manages ink formulations, field evidence transcription, lore preservation,
    /// and archivist duty assignments.
    /// </summary>
    public partial class ArchiveDeskPanel : Control
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _inkList = null!;
        private VBoxContainer _transcriptionDesk = null!;
        private VBoxContainer _archiveLogContainer = null!;
        private Label _eventLogLabel = null!;

        private ArchiveDeskHostSession? _host;
        private string _selectedInkId = "ink_carbon_lampblack";

        public bool IsBound => _host != null;

        public void Bind(ArchiveDeskHostSession session)
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

            _shell = new AshfallDashboardShell("SYS: ARCHIVAL SCRIPT & TRANSCRIPTION // ARCHIVE DESK", minWidth: 1040, minHeight: 680);
            center.AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("inks", "INK FORMULAS", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("active_jobs", "TRANSCRIPTIONS", "0", AshfallMetricCard.Criticality.Normal, minWidth: 130);
            _statusRail.AddCard("completed", "ARCHIVED DOCS", "0", AshfallMetricCard.Criticality.Normal, minWidth: 130);
            _statusRail.AddCard("paper", "PULP PAPER", "12 SHEETS", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("status", "ARCHIVIST", "READY", AshfallMetricCard.Criticality.Normal, minWidth: 120);

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

            // Column 1: Ink Catalog
            var leftPanel = AshfallUiHelpers.MakePanel(minWidth: 310);
            leftPanel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            leftPanel.SizeFlagsStretchRatio = 0.95f;
            var leftMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingSm);
            leftPanel.AddChild(leftMargin);
            var leftVbox = new VBoxContainer();
            leftVbox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            leftMargin.AddChild(leftVbox);
            leftVbox.AddChild(AshfallUiHelpers.MakeSectionHeader("ARCHIVAL INK CATALOG"));
            var leftScroll = new ScrollContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            _inkList = new VBoxContainer();
            _inkList.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
            _inkList.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            leftScroll.AddChild(_inkList);
            leftVbox.AddChild(leftScroll);
            gridRow.AddChild(leftPanel);

            // Column 2: Transcription Desk
            var centerPanel = AshfallUiHelpers.MakePanel(minWidth: 380);
            centerPanel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            centerPanel.SizeFlagsStretchRatio = 1.2f;
            var centerMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingSm);
            centerPanel.AddChild(centerMargin);
            var centerVbox = new VBoxContainer();
            centerVbox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            centerMargin.AddChild(centerVbox);
            centerVbox.AddChild(AshfallUiHelpers.MakeSectionHeader("TRANSCRIPTION & DRAFTING"));
            var centerScroll = new ScrollContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            _transcriptionDesk = new VBoxContainer();
            _transcriptionDesk.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            _transcriptionDesk.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            centerScroll.AddChild(_transcriptionDesk);
            centerVbox.AddChild(centerScroll);
            gridRow.AddChild(centerPanel);

            // Column 3: Completed Transcriptions Log
            var rightPanel = AshfallUiHelpers.MakePanel(minWidth: 310);
            rightPanel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            rightPanel.SizeFlagsStretchRatio = 0.95f;
            var rightMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingSm);
            rightPanel.AddChild(rightMargin);
            var rightVbox = new VBoxContainer();
            rightVbox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            rightMargin.AddChild(rightVbox);
            rightVbox.AddChild(AshfallUiHelpers.MakeSectionHeader("ARCHIVE LEDGER"));
            var rightScroll = new ScrollContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            _archiveLogContainer = new VBoxContainer();
            _archiveLogContainer.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            _archiveLogContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            rightScroll.AddChild(_archiveLogContainer);
            rightVbox.AddChild(rightScroll);

            rightVbox.AddChild(AshfallUiHelpers.MakeSeparator());
            _eventLogLabel = AshfallUiHelpers.MakeMetadata("No recent archival events.");
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

            AshfallUiHelpers.EmptyChildren(_inkList);
            AshfallUiHelpers.EmptyChildren(_transcriptionDesk);
            AshfallUiHelpers.EmptyChildren(_archiveLogContainer);

            var s = _host.System.State;
            var catalog = _host.System.Catalog.Values.ToList();
            int totalInks = catalog.Count;
            int activeJobs = s.queue.Count(j => !j.isComplete && !j.isCancelled);
            int completedJobs = s.totalTranscriptions;

            _statusRail.Set("inks", totalInks.ToString(), AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("active_jobs", activeJobs.ToString(), activeJobs > 0 ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("completed", completedJobs.ToString(), AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("paper", "12 SHEETS", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("status", activeJobs > 0 ? "TRANSCRIBING" : "STANDBY", AshfallMetricCard.Criticality.Normal);

            if (!string.IsNullOrEmpty(_host.LastEvent))
            {
                _eventLogLabel.Text = _host.LastEvent;
            }

            if (catalog.Count == 0)
            {
                _inkList.AddChild(AshfallUiHelpers.MakeMetadata("No ink formulations in catalog."));
                var btnSeed = AshfallUiHelpers.MakeButton("LOAD INK FORMULATIONS", () =>
                {
                    _host.LoadInkCatalog(new List<InkMaterialDefinition>
                    {
                        new InkMaterialDefinition { ink_id = "ink_carbon_lampblack", display_name = "Carbon Lampblack", legibilityScore = 0.95f, archivalLongevityDays = 720f },
                        new InkMaterialDefinition { ink_id = "ink_iron_gall", display_name = "Iron Gall Extract", legibilityScore = 0.90f, archivalLongevityDays = 500f },
                        new InkMaterialDefinition { ink_id = "ink_fungal_bioluminescence", display_name = "Phosphor Fungal Dye", legibilityScore = 0.70f, archivalLongevityDays = 180f }
                    });
                    RefreshView();
                });
                _inkList.AddChild(btnSeed);
            }
            else
            {
                foreach (var ink in catalog)
                {
                    var card = AshfallUiHelpers.MakePanel();
                    var cardMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingXs);
                    card.AddChild(cardMargin);
                    var cardVbox = new VBoxContainer();
                    cardVbox.AddThemeConstantOverride("separation", 3);
                    cardMargin.AddChild(cardVbox);

                    var headerRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
                    headerRow.AddChild(AshfallUiHelpers.MakeBadgeIcon("badge_crossing_terms", 18));
                    var nameLbl = AshfallUiHelpers.MakeBody(ink.display_name);
                    nameLbl.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                    headerRow.AddChild(nameLbl);
                    cardVbox.AddChild(headerRow);

                    var catLbl = AshfallUiHelpers.MakeMono($"LEGIBILITY: {ink.legibilityScore:P0} ({ink.archivalLongevityDays:F0}d Longevity)");
                    catLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
                    cardVbox.AddChild(catLbl);

                    var selectBtn = AshfallUiHelpers.MakeButton($"SELECT // {ink.ink_id}", () =>
                    {
                        _selectedInkId = ink.ink_id;
                        RefreshView();
                    });
                    selectBtn.CustomMinimumSize = new Vector2(0, 24);
                    cardVbox.AddChild(selectBtn);

                    _inkList.AddChild(card);
                }
            }

            // Transcription Desk Controls
            var curInk = catalog.FirstOrDefault(i => i.ink_id == _selectedInkId) ?? (catalog.Count > 0 ? catalog[0] : null);
            _transcriptionDesk.AddChild(AshfallUiHelpers.MakeSectionHeader("TRANSCRIBE FIELD EVIDENCE"));
            _transcriptionDesk.AddChild(AshfallUiHelpers.MakeDataRow("Active Formulation", curInk != null ? curInk.display_name : "None", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _transcriptionDesk.AddChild(AshfallUiHelpers.MakeDataRow("Archival Substrate", "Pressed Birch Pulp (Grade A)", AshfallUiHelpers.ToColor(DesignTheme.Pale)));
            _transcriptionDesk.AddChild(AshfallUiHelpers.MakeDataRow("Permanent Durability", curInk != null ? $"{curInk.archivalLongevityDays:F0} Days Fade Resistance" : "—", AshfallUiHelpers.ToColor(DesignTheme.Lethe)));

            _transcriptionDesk.AddChild(AshfallUiHelpers.MakeSeparator());
            _transcriptionDesk.AddChild(AshfallUiHelpers.MakeSubsectionHeader("DISPATCH SCRIPT WORK"));

            var btnTranscribeLog = AshfallUiHelpers.MakeButton("TRANSCRIBE EXPEDITION DIARY", () =>
            {
                if (curInk != null)
                {
                    _host.QueueTranscription("evidence_bunker_log_001", "the_teacher", curInk.ink_id);
                    RefreshView();
                }
            });
            _transcriptionDesk.AddChild(btnTranscribeLog);

            var btnTranscribeMap = AshfallUiHelpers.MakeButton("DRAFT SECTOR CARTOGRAPHY MAP", () =>
            {
                if (curInk != null)
                {
                    _host.QueueTranscription("evidence_sector_map_km19", "the_teacher", curInk.ink_id);
                    RefreshView();
                }
            });
            _transcriptionDesk.AddChild(btnTranscribeMap);

            // Populate Completed / Queued Jobs
            if (s.queue.Count == 0 && s.unlockedEvidenceIds.Count == 0)
            {
                _archiveLogContainer.AddChild(AshfallUiHelpers.MakeMetadata("No manuscript transcriptions on record."));
            }
            else
            {
                foreach (var job in s.queue)
                {
                    _archiveLogContainer.AddChild(AshfallUiHelpers.MakeMono($"[{(job.isComplete ? "COMPLETED" : "IN PROGRESS")}] {job.evidenceId} by {job.archivistId}"));
                }
                foreach (var evId in s.unlockedEvidenceIds)
                {
                    _archiveLogContainer.AddChild(AshfallUiHelpers.MakeMono($"[UNLOCKED] {evId}"));
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
            if (_host != null)
            {
                _host.StateChanged -= RefreshView;
            }
            base._ExitTree();
        }
    }
}
