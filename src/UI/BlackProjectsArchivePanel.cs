// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Journal;
using Ashfall.Core.Narrative;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Classified Black Projects Intelligence Archive Browser (Plan 152 Follow-up).
    /// Programmatic UI implementation for the dedicated intelligence archive browser bound to
    /// <see cref="BlackProjectsArchiveSystem"/>.
    ///
    /// Implements:
    ///   • Family filter tabs (All, Orbital Telemetry, Drone Blackboxes, Cobalt Directives, Vault Audits)
    ///   • Quick search by callsign, carrier, code, or facility
    ///   • Scrollable record navigator showing recovery site, taxonomy badges, and index
    ///   • Comprehensive intelligence dossier with TOP SECRET classification banner,
    ///     metadata matrix, declassified CRT transcript, and relational cross-reference graph
    ///   • Clickable relation nodes allowing immediate graph traversal
    ///   • Archival presentation discipline: historical observations only, zero executable weapon fields
    ///   • Journal codex export integration with single feedback strip
    ///
    /// Conforms to UI-07 (non-empty body, complete workflow) and UI-09 (registered navigable route)
    /// and reconciled with Google Stitch Screen 2cda40aadcf441e9b995f3c4f1f05a90.
    /// </summary>
    public partial class BlackProjectsArchivePanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        public enum FamilyFilter
        {
            All,
            Orbital,
            Drones,
            Cobalt,
            Vaults
        }

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail _statusRail = null!;

        // Filter & Search Controls
        private Button _btnFilterAll = null!;
        private Button _btnFilterOrbital = null!;
        private Button _btnFilterDrones = null!;
        private Button _btnFilterCobalt = null!;
        private Button _btnFilterVaults = null!;
        private LineEdit _searchBox = null!;
        private ItemList _recordList = null!;
        private Label _listSummaryLabel = null!;

        // Dossier Stage Controls
        private ScrollContainer _dossierScroll = null!;
        private VBoxContainer _dossierContainer = null!;
        private Control _emptyDossierView = null!;
        private VBoxContainer _dossierContentView = null!;

        // Dossier Content
        private PanelContainer _classificationBanner = null!;
        private Label _bannerWarningLabel = null!;
        private Label _bannerSiteLabel = null!;
        private Label _recordTitleLabel = null!;
        private Label _recordSubtitleLabel = null!;

        // Metadata grid labels
        private Label _metaDesignationValue = null!;
        private Label _metaTruthClassValue = null!;
        private Label _metaTimestampValue = null!;
        private Label _metaSpecsValue = null!;

        // Transcript
        private Label _transcriptBodyLabel = null!;

        // Relational Graph
        private VBoxContainer _relatedCardsContainer = null!;

        // Footer Actions & Feedback
        private Button _logToJournalBtn = null!;
        private Label _feedbackLabel = null!;

        // System binding state
        private BlackProjectsArchiveSystem? _system;
        private JournalSystem? _journal;
        private FamilyFilter _currentFilter = FamilyFilter.All;
        private string _searchFilter = string.Empty;
        private string? _selectedRecordId;
        private readonly List<string> _displayedRecordIds = new();

        public bool IsBound => _system != null;
        public string? SelectedRecordId => _selectedRecordId;
        public FamilyFilter CurrentFilter => _currentFilter;

        public void Bind(BlackProjectsArchiveSystem system, JournalSystem? journal = null)
        {
            if (_system != null)
            {
                _system.OnStateChanged -= HandleSystemStateChanged;
                _system.OnRecordFirstDiscovered -= HandleRecordFirstDiscovered;
            }

            _system = system ?? throw new ArgumentNullException(nameof(system));
            _journal = journal;

            _system.OnStateChanged += HandleSystemStateChanged;
            _system.OnRecordFirstDiscovered += HandleRecordFirstDiscovered;

            RefreshView();
        }

        public void Unbind()
        {
            if (_system != null)
            {
                _system.OnStateChanged -= HandleSystemStateChanged;
                _system.OnRecordFirstDiscovered -= HandleRecordFirstDiscovered;
                _system = null;
            }
            _journal = null;
        }

        public override void _ExitTree()
        {
            Unbind();
            base._ExitTree();
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);

            _shell = new AshfallDashboardShell(
                "CLASSIFIED BLACK PROJECTS // INTELLIGENCE ARCHIVE",
                minWidth: 1200,
                minHeight: 700);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("discovered", "RECOVERED", "0 / 30", AshfallMetricCard.Criticality.Normal, 140);
            _statusRail.AddCard("status", "SYSTEM STATE", "ARCHIVAL / INERT", AshfallMetricCard.Criticality.Normal, 160);
            _statusRail.AddCard("corroborations", "CORROBORATIONS", "0 ACTIVE", AshfallMetricCard.Criticality.Normal, 150);
            _statusRail.AddCard("filter", "ACTIVE FILTER", "ALL FAMILIES", AshfallMetricCard.Criticality.Normal, 150);

            _shell.AttachHeaderCloseButton("CLOSE ARCHIVE", () => Close());

            var mainHBox = new HBoxContainer();
            mainHBox.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
            mainHBox.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            mainHBox.SizeFlagsVertical = SizeFlags.ExpandFill;

            // ── Left Column: Navigator ──────────────────────────────────────
            var leftColumn = BuildLeftNavigatorColumn();
            mainHBox.AddChild(leftColumn);

            // ── Right Column: Dossier & Relational Graph ─────────────────────
            var rightColumn = BuildRightDossierColumn();
            mainHBox.AddChild(rightColumn);

            _shell.SetContent(mainHBox);
            AddChild(_shell);

            Visible = false;
        }

        public void Open()
        {
            Visible = true;
            RefreshView();
        }

        public void Close() {
            if (!AtomicWar.GodotApp.UI.UiMotion.AnimateClose(this))
                Visible = false;
            OnClose?.Invoke();
        }

        // ── View Construction ───────────────────────────────────────────────

        private Control BuildLeftNavigatorColumn()
        {
            var container = new VBoxContainer
            {
                CustomMinimumSize = new Vector2(400, 0),
                SizeFlagsVertical = SizeFlags.ExpandFill
            };
            container.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);

            var filterHeader = AshfallUiHelpers.MakeSectionHeader("ARCHIVAL FAMILIES");
            container.AddChild(filterHeader);

            // Filter Tabs Row
            var filterRow = new HBoxContainer();
            filterRow.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);

            _btnFilterAll = AshfallUiHelpers.MakeButton("ALL", () => SetFilter(FamilyFilter.All));
            _btnFilterOrbital = AshfallUiHelpers.MakeButton("ORBITAL", () => SetFilter(FamilyFilter.Orbital));
            _btnFilterDrones = AshfallUiHelpers.MakeButton("DRONES", () => SetFilter(FamilyFilter.Drones));
            _btnFilterCobalt = AshfallUiHelpers.MakeButton("COBALT", () => SetFilter(FamilyFilter.Cobalt));
            _btnFilterVaults = AshfallUiHelpers.MakeButton("VAULTS", () => SetFilter(FamilyFilter.Vaults));

            filterRow.AddChild(_btnFilterAll);
            filterRow.AddChild(_btnFilterOrbital);
            filterRow.AddChild(_btnFilterDrones);
            filterRow.AddChild(_btnFilterCobalt);
            filterRow.AddChild(_btnFilterVaults);
            container.AddChild(filterRow);

            // Search Bar
            _searchBox = new LineEdit
            {
                PlaceholderText = "Search callsign, carrier, directive, site...",
                SizeFlagsHorizontal = SizeFlags.ExpandFill
            };
            _searchBox.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeSmall);
            if (AshfallUiHelpers.FontShareTechMono != null)
                _searchBox.AddThemeFontOverride("font", AshfallUiHelpers.FontShareTechMono);
            _searchBox.TextChanged += query =>
            {
                _searchFilter = query?.Trim() ?? string.Empty;
                PopulateRecordList();
            };
            container.AddChild(_searchBox);

            container.AddChild(AshfallUiHelpers.MakeSeparator());

            // Scrollable Record List
            _recordList = new ItemList
            {
                SizeFlagsVertical = SizeFlags.ExpandFill,
                SizeFlagsHorizontal = SizeFlags.ExpandFill,
                SelectMode = ItemList.SelectModeEnum.Single,
                AutoHeight = false
            };
            _recordList.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeBody);
            if (AshfallUiHelpers.FontShareTechMono != null)
                _recordList.AddThemeFontOverride("font", AshfallUiHelpers.FontShareTechMono);
            _recordList.ItemSelected += OnRecordSelected;
            container.AddChild(_recordList);

            _listSummaryLabel = AshfallUiHelpers.MakeSmall("0 records recovered.", autowrap: false);
            container.AddChild(_listSummaryLabel);

            return container;
        }

        private Control BuildRightDossierColumn()
        {
            _dossierScroll = new ScrollContainer
            {
                SizeFlagsHorizontal = SizeFlags.ExpandFill,
                SizeFlagsVertical = SizeFlags.ExpandFill
            };

            _dossierContainer = new VBoxContainer
            {
                SizeFlagsHorizontal = SizeFlags.ExpandFill,
                SizeFlagsVertical = SizeFlags.ExpandFill
            };
            _dossierContainer.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
            _dossierScroll.AddChild(_dossierContainer);

            // Empty State
            _emptyDossierView = AshfallUiHelpers.MakeEmptyState(
                "Recover telemetry, blackboxes, and directives across expedition sites to declassify records.",
                "NO RECORD SELECTED",
                "Select a declassified entry from the archive navigator on the left.");
            _emptyDossierView.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _emptyDossierView.SizeFlagsVertical = SizeFlags.ExpandFill;
            _dossierContainer.AddChild(_emptyDossierView);

            // Content View (shown when record selected)
            _dossierContentView = new VBoxContainer
            {
                SizeFlagsHorizontal = SizeFlags.ExpandFill,
                SizeFlagsVertical = SizeFlags.ExpandFill,
                Visible = false
            };
            _dossierContentView.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);

            // 1. Classification Banner
            _classificationBanner = BuildClassificationBanner();
            _dossierContentView.AddChild(_classificationBanner);

            // 2. Title & Taxonomy
            var titleBox = new VBoxContainer();
            titleBox.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
            _recordTitleLabel = AshfallUiHelpers.MakeTitle("RECORD DESIGNATION", DesignTheme.FontSizeH2);
            _recordTitleLabel.HorizontalAlignment = HorizontalAlignment.Left;
            _recordSubtitleLabel = AshfallUiHelpers.MakeSectionHeader("TRUTH TAXONOMY: INSTRUMENT TELEMETRY");
            titleBox.AddChild(_recordTitleLabel);
            titleBox.AddChild(_recordSubtitleLabel);
            _dossierContentView.AddChild(titleBox);

            // 3. Metadata Matrix (4 Cards)
            var metaGrid = BuildMetadataGrid();
            _dossierContentView.AddChild(metaGrid);

            _dossierContentView.AddChild(AshfallUiHelpers.MakeSeparator());

            // 4. Verbatim Decrypted Transcript
            var transcriptSection = BuildTranscriptSection();
            _dossierContentView.AddChild(transcriptSection);

            _dossierContentView.AddChild(AshfallUiHelpers.MakeSeparator());

            // 5. Relational Graph
            var relationSection = BuildRelationalGraphSection();
            _dossierContentView.AddChild(relationSection);

            _dossierContentView.AddChild(AshfallUiHelpers.MakeSeparator());

            // 6. Footer Disclaimer & Codex Logging
            var footerBox = BuildFooterActions();
            _dossierContentView.AddChild(footerBox);

            _dossierContainer.AddChild(_dossierContentView);

            return _dossierScroll;
        }

        private PanelContainer BuildClassificationBanner()
        {
            var panel = new PanelContainer();
            var sb = new StyleBoxFlat
            {
                BgColor = new Color(0.12f, 0.04f, 0.04f, 0.85f),
                BorderColor = AshfallUiHelpers.ColorWarning,
            };
            sb.SetBorderWidthAll(2);
            panel.AddThemeStyleboxOverride("panel", sb);

            var margin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingSm, DesignTheme.SpacingXs, DesignTheme.SpacingSm, DesignTheme.SpacingXs);
            panel.AddChild(margin);

            var vbox = new VBoxContainer();
            vbox.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
            margin.AddChild(vbox);

            var topBanner = AshfallUiHelpers.MakeLabel("[ TOP SECRET // RESTRICTED ARCHIVAL REPOSIT // EYES ONLY ]", DesignTheme.FontSizeH3, AshfallUiHelpers.ColorWarning);
            topBanner.HorizontalAlignment = HorizontalAlignment.Center;
            AshfallUiHelpers.ApplyFont(topBanner, AshfallUiHelpers.FontBarlowBold);
            vbox.AddChild(topBanner);

            _bannerWarningLabel = AshfallUiHelpers.MakeSmall(
                "ARCHIVAL PROJECTION — ZERO OPERATIONAL REMOTE CONTROL. WEAPONS AND SITES CANNOT BE COMMANDED.",
                autowrap: true);
            _bannerWarningLabel.HorizontalAlignment = HorizontalAlignment.Center;
            _bannerWarningLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Pale));
            vbox.AddChild(_bannerWarningLabel);

            _bannerSiteLabel = AshfallUiHelpers.MakeSmall("RECOVERY SITE: UNKNOWN", autowrap: false);
            _bannerSiteLabel.HorizontalAlignment = HorizontalAlignment.Center;
            _bannerSiteLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Muted));
            vbox.AddChild(_bannerSiteLabel);

            return panel;
        }

        private Control BuildMetadataGrid()
        {
            var grid = new HBoxContainer();
            grid.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            grid.SizeFlagsHorizontal = SizeFlags.ExpandFill;

            grid.AddChild(BuildMetaCard("PLATFORM / CODE", out _metaDesignationValue));
            grid.AddChild(BuildMetaCard("CLASSIFICATION", out _metaTruthClassValue));
            grid.AddChild(BuildMetaCard("TIMESTAMP / ERA", out _metaTimestampValue));
            grid.AddChild(BuildMetaCard("TECHNICAL TELEMETRY", out _metaSpecsValue));

            return grid;
        }

        private static PanelContainer BuildMetaCard(string headerText, out Label valueLabel)
        {
            var panel = AshfallUiHelpers.MakePanel();
            panel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            var margin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingSm, DesignTheme.SpacingXs, DesignTheme.SpacingSm, DesignTheme.SpacingXs);
            panel.AddChild(margin);

            var vbox = new VBoxContainer();
            vbox.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
            margin.AddChild(vbox);

            var hdr = AshfallUiHelpers.MakeLabel(headerText, DesignTheme.FontSizeSmall, AshfallUiHelpers.ToColor(DesignTheme.Dim));
            vbox.AddChild(hdr);

            valueLabel = AshfallUiHelpers.MakeLabel("—", DesignTheme.FontSizeBody, AshfallUiHelpers.ToColor(DesignTheme.Warm));
            AshfallUiHelpers.ApplyFont(valueLabel, AshfallUiHelpers.FontBarlowSemiBold);
            vbox.AddChild(valueLabel);

            return panel;
        }

        private Control BuildTranscriptSection()
        {
            var vbox = new VBoxContainer();
            vbox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);

            var header = AshfallUiHelpers.MakeSectionHeader("DECLASSIFIED INTELLIGENCE TRANSCRIPT");
            vbox.AddChild(header);

            var transcriptPanel = new PanelContainer();
            var sb = new StyleBoxFlat
            {
                BgColor = new Color(0.04f, 0.05f, 0.04f, 0.95f),
                BorderColor = AshfallUiHelpers.ToColor(DesignTheme.LineSoft),
            };
            sb.SetBorderWidthAll(1);
            transcriptPanel.AddThemeStyleboxOverride("panel", sb);

            var margin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingMd);
            transcriptPanel.AddChild(margin);

            _transcriptBodyLabel = new Label
            {
                Text = "No decrypted transcript available.",
                AutowrapMode = TextServer.AutowrapMode.WordSmart,
                SizeFlagsHorizontal = SizeFlags.ExpandFill
            };
            _transcriptBodyLabel.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeBody);
            _transcriptBodyLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Pale));
            AshfallUiHelpers.ApplyFont(_transcriptBodyLabel, AshfallUiHelpers.FontShareTechMono);
            margin.AddChild(_transcriptBodyLabel);

            vbox.AddChild(transcriptPanel);
            return vbox;
        }

        private Control BuildRelationalGraphSection()
        {
            var vbox = new VBoxContainer();
            vbox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);

            var header = AshfallUiHelpers.MakeSectionHeader("RELATIONAL INTELLIGENCE & CORROBORATION GRAPH");
            vbox.AddChild(header);

            var subHeader = AshfallUiHelpers.MakeSmall(
                "Cross-references between recovered records. Undiscovered linked files remain classified until expedition recovery.",
                autowrap: true);
            vbox.AddChild(subHeader);

            _relatedCardsContainer = new VBoxContainer();
            _relatedCardsContainer.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            vbox.AddChild(_relatedCardsContainer);

            return vbox;
        }

        private Control BuildFooterActions()
        {
            var vbox = new VBoxContainer();
            vbox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);

            var disclaimer = AshfallUiHelpers.MakeSmall(
                "ARCHIVAL NOTICE: This console acts solely as an intelligence repository. No remote missile launches, drone dispatches, or vault breach protocols are executable.",
                autowrap: true);
            vbox.AddChild(disclaimer);

            var actionHBox = new HBoxContainer();
            actionHBox.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);

            _logToJournalBtn = AshfallUiHelpers.MakeButton("LOG TO JOURNAL CODEX", OnLogToJournalPressed);
            actionHBox.AddChild(_logToJournalBtn);

            _feedbackLabel = AshfallUiHelpers.MakeSmall("Repository ready.", autowrap: true);
            _feedbackLabel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            actionHBox.AddChild(_feedbackLabel);

            vbox.AddChild(actionHBox);
            return vbox;
        }

        // ── Interaction & View Refresh ──────────────────────────────────────

        private void SetFilter(FamilyFilter filter)
        {
            _currentFilter = filter;
            UpdateFilterButtonStates();
            PopulateRecordList();
            UpdateStatusRail();
        }

        private void UpdateFilterButtonStates()
        {
            _btnFilterAll.Text = _currentFilter == FamilyFilter.All ? "> ALL <" : "ALL";
            _btnFilterOrbital.Text = _currentFilter == FamilyFilter.Orbital ? "> ORBITAL <" : "ORBITAL";
            _btnFilterDrones.Text = _currentFilter == FamilyFilter.Drones ? "> DRONES <" : "DRONES";
            _btnFilterCobalt.Text = _currentFilter == FamilyFilter.Cobalt ? "> COBALT <" : "COBALT";
            _btnFilterVaults.Text = _currentFilter == FamilyFilter.Vaults ? "> VAULTS <" : "VAULTS";
        }

        public void RefreshView()
        {
            UpdateFilterButtonStates();
            PopulateRecordList();
            UpdateStatusRail();
            RefreshDossierView();
        }

        private void UpdateStatusRail()
        {
            if (_system == null || _statusRail == null) return;

            int discoveredCount = _system.State.discoveredRecordIds.Count;
            _statusRail.Set("discovered", $"{discoveredCount} / 30",
                discoveredCount > 0 ? AshfallMetricCard.Criticality.Normal : AshfallMetricCard.Criticality.Warn);

            int totalCorroborations = 0;
            foreach (var id in _system.State.discoveredRecordIds)
            {
                totalCorroborations += _system.GetRelated(id).Count;
            }
            _statusRail.Set("corroborations", $"{totalCorroborations / 2} PAIRS", AshfallMetricCard.Criticality.Normal);

            string filterName = _currentFilter switch
            {
                FamilyFilter.Orbital => "ORBITAL",
                FamilyFilter.Drones => "DRONES",
                FamilyFilter.Cobalt => "COBALT",
                FamilyFilter.Vaults => "VAULTS",
                _ => "ALL FAMILIES"
            };
            _statusRail.Set("filter", filterName, AshfallMetricCard.Criticality.Normal);
        }

        private void PopulateRecordList()
        {
            _recordList.Clear();
            _displayedRecordIds.Clear();

            if (_system == null) return;

            var catalog = _system.Catalog;
            var discoveredIds = new HashSet<string>(_system.State.discoveredRecordIds, StringComparer.Ordinal);

            // Collect matching discovered entries
            var candidateIds = new List<string>();

            if (_currentFilter == FamilyFilter.All || _currentFilter == FamilyFilter.Orbital)
                candidateIds.AddRange(catalog.OrbitalEntries.Select(e => e.Id));

            if (_currentFilter == FamilyFilter.All || _currentFilter == FamilyFilter.Drones)
                candidateIds.AddRange(catalog.DroneEntries.Select(e => e.Id));

            if (_currentFilter == FamilyFilter.All || _currentFilter == FamilyFilter.Cobalt)
                candidateIds.AddRange(catalog.CobaltEntries.Select(e => e.Id));

            if (_currentFilter == FamilyFilter.All || _currentFilter == FamilyFilter.Vaults)
                candidateIds.AddRange(catalog.VaultEntries.Select(e => e.Id));

            int totalDiscoveredMatching = 0;
            int itemIndex = 0;

            foreach (var id in candidateIds.OrderBy(x => x, StringComparer.Ordinal))
            {
                if (!discoveredIds.Contains(id)) continue;
                totalDiscoveredMatching++;

                string title = GetRecordShortTitle(id);
                string producer = DescribeProducer(_system.GetProducer(id));

                if (!string.IsNullOrEmpty(_searchFilter))
                {
                    bool match = title.Contains(_searchFilter, StringComparison.OrdinalIgnoreCase) ||
                                 id.Contains(_searchFilter, StringComparison.OrdinalIgnoreCase) ||
                                 producer.Contains(_searchFilter, StringComparison.OrdinalIgnoreCase);
                    if (!match) continue;
                }

                _displayedRecordIds.Add(id);
                _recordList.AddItem($"[{_displayedRecordIds.Count:D2}] {title} ({producer})");

                if (_selectedRecordId == id)
                {
                    _recordList.Select(itemIndex);
                }
                itemIndex++;
            }

            _listSummaryLabel.Text = $"{totalDiscoveredMatching} declassified records in archive (out of 30 total).";

            if (string.IsNullOrEmpty(_selectedRecordId) && _displayedRecordIds.Count > 0)
            {
                _selectedRecordId = _displayedRecordIds[0];
                _recordList.Select(0);
            }

            RefreshDossierView();
        }

        private void OnRecordSelected(long index)
        {
            if (index >= 0 && index < _displayedRecordIds.Count)
            {
                _selectedRecordId = _displayedRecordIds[(int)index];
                RefreshDossierView();
            }
        }

        public void SelectRecord(string recordId)
        {
            if (string.IsNullOrEmpty(recordId) || _system == null) return;
            if (!_system.IsDiscovered(recordId)) return;

            _selectedRecordId = recordId;

            // Ensure filter displays the target record
            var truth = _system.TruthClassFor(recordId);
            bool requiresFilterChange = _currentFilter switch
            {
                FamilyFilter.Orbital => truth != BlackProjectsTruthClass.InstrumentTelemetry,
                FamilyFilter.Drones => truth != BlackProjectsTruthClass.VehicleBlackbox,
                FamilyFilter.Cobalt => truth != BlackProjectsTruthClass.ClassifiedDirective,
                FamilyFilter.Vaults => truth != BlackProjectsTruthClass.ComplianceAudit,
                _ => false
            };

            if (requiresFilterChange)
            {
                _currentFilter = FamilyFilter.All;
                UpdateFilterButtonStates();
            }

            PopulateRecordList();
        }

        private void RefreshDossierView()
        {
            if (_system == null || string.IsNullOrEmpty(_selectedRecordId) || !_system.IsDiscovered(_selectedRecordId))
            {
                _emptyDossierView.Visible = true;
                _dossierContentView.Visible = false;
                return;
            }

            _emptyDossierView.Visible = false;
            _dossierContentView.Visible = true;

            var catalog = _system.Catalog;
            string id = _selectedRecordId;
            var truth = _system.TruthClassFor(id);
            string producer = DescribeProducer(_system.GetProducer(id));

            _bannerSiteLabel.Text = $"PRIMARY RECOVERY SITE: {producer.ToUpperInvariant()} | ARCHIVAL CLASSIFICATION: TOP SECRET";

            // Orbital Record
            var orbital = catalog.GetOrbital(id);
            if (orbital != null)
            {
                _recordTitleLabel.Text = $"{orbital.Callsign} // {orbital.EntryType.Replace('_', ' ').ToUpperInvariant()}";
                _recordSubtitleLabel.Text = "TRUTH TAXONOMY: INSTRUMENT TELEMETRY // ORBITAL KINETIC PLATFORM";
                _metaDesignationValue.Text = orbital.Callsign;
                _metaTruthClassValue.Text = "Instrument Telemetry";
                _metaTimestampValue.Text = orbital.TimestampRelative;
                _metaSpecsValue.Text = $"Alt: {orbital.OrbitalAltitudeKm:F1} km | Decay: {orbital.DecayRateMetersPerDay:F0} m/d | Channel: {orbital.TelemetryChannel}";
                _transcriptBodyLabel.Text = orbital.Prose;
            }

            // Drone Record
            var drone = catalog.GetDrone(id);
            if (drone != null)
            {
                _recordTitleLabel.Text = $"{drone.CarrierId} // {drone.RecordType.Replace('_', ' ').ToUpperInvariant()}";
                _recordSubtitleLabel.Text = "TRUTH TAXONOMY: VEHICLE BLACKBOX // AUTONOMOUS RECON DRONE";
                _metaDesignationValue.Text = drone.CarrierId;
                _metaTruthClassValue.Text = "Vehicle Blackbox";
                _metaTimestampValue.Text = drone.TimestampRelative;
                _metaSpecsValue.Text = $"Alt: {drone.AltitudeFeet} ft | Airspeed: {drone.AirspeedKnots} kts | Status: {drone.SystemHealth}";
                _transcriptBodyLabel.Text = drone.Prose;
            }

            // Cobalt Record
            var cobalt = catalog.GetCobalt(id);
            if (cobalt != null)
            {
                _recordTitleLabel.Text = $"DIRECTIVE {cobalt.DirectiveCode} // {cobalt.Classification.ToUpperInvariant()}";
                _recordSubtitleLabel.Text = $"TRUTH TAXONOMY: CLASSIFIED DIRECTIVE // {cobalt.IssuingAuthority.ToUpperInvariant()}";
                _metaDesignationValue.Text = $"Directive {cobalt.DirectiveCode}";
                _metaTruthClassValue.Text = "Classified Directive";
                _metaTimestampValue.Text = cobalt.EffectiveDayRange;
                _metaSpecsValue.Text = $"Salvo: {cobalt.AuthorizedSalvoSize} Warheads | Authority: {cobalt.IssuingAuthority}";
                _transcriptBodyLabel.Text = cobalt.Prose;
            }

            // Vault Record
            var vault = catalog.GetVault(id);
            if (vault != null)
            {
                _recordTitleLabel.Text = $"{vault.VaultId} // {vault.AuditType.Replace('_', ' ').ToUpperInvariant()}";
                _recordSubtitleLabel.Text = $"TRUTH TAXONOMY: COMPLIANCE AUDIT // AUDITOR: {vault.AuditorDesignation.ToUpperInvariant()}";
                _metaDesignationValue.Text = vault.VaultId;
                _metaTruthClassValue.Text = "Compliance Audit";
                _metaTimestampValue.Text = vault.TimestampRelative;
                _metaSpecsValue.Text = $"Sub-Level: {vault.SubLevel} | Status: {vault.ComplianceStatus}";
                _transcriptBodyLabel.Text = vault.Prose;
            }

            // Relational Cross-References
            PopulateRelatedCards(id);
        }

        private void PopulateRelatedCards(string recordId)
        {
            foreach (Node child in _relatedCardsContainer.GetChildren())
            {
                child.QueueFree();
            }

            if (_system == null) return;

            var relations = _system.GetRelated(recordId);

            if (relations.Count == 0)
            {
                var emptyCard = AshfallUiHelpers.MakeEmptyStateLabel(
                    "No discovered cross-references for this record.",
                    "Scout other wasteland sites to declassify corroborating files.");
                _relatedCardsContainer.AddChild(emptyCard);
                return;
            }

            foreach (var rel in relations)
            {
                var card = BuildRelationCard(rel);
                _relatedCardsContainer.AddChild(card);
            }
        }

        private PanelContainer BuildRelationCard(BlackProjectsRelatedRecord rel)
        {
            var panel = AshfallUiHelpers.MakePanel();
            var margin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingSm);
            panel.AddChild(margin);

            var hbox = new HBoxContainer();
            hbox.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
            margin.AddChild(hbox);

            // Left: Badge + Title + Detail
            var vbox = new VBoxContainer
            {
                SizeFlagsHorizontal = SizeFlags.ExpandFill
            };
            vbox.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
            hbox.AddChild(vbox);

            var badgeRow = new HBoxContainer();
            badgeRow.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);

            string badgeText = $"[ {FormatRelationKind(rel.RelationKind)} ]";
            Color badgeColor = GetRelationColor(rel.RelationKind);
            var badgeLabel = AshfallUiHelpers.MakeLabel(badgeText, DesignTheme.FontSizeSmall, badgeColor);
            AshfallUiHelpers.ApplyFont(badgeLabel, AshfallUiHelpers.FontBarlowBold);
            badgeRow.AddChild(badgeLabel);

            string targetTitle = GetRecordShortTitle(rel.RecordId);
            var titleLabel = AshfallUiHelpers.MakeLabel(targetTitle, DesignTheme.FontSizeBody, AshfallUiHelpers.ToColor(DesignTheme.Warm));
            AshfallUiHelpers.ApplyFont(titleLabel, AshfallUiHelpers.FontBarlowSemiBold);
            badgeRow.AddChild(titleLabel);

            vbox.AddChild(badgeRow);

            var detailLabel = AshfallUiHelpers.MakeSmall(rel.Detail, autowrap: true);
            detailLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Pale));
            vbox.AddChild(detailLabel);

            // Right: Navigation Action
            var inspectBtn = AshfallUiHelpers.MakeButton("INSPECT DOSSIER", () => SelectRecord(rel.RecordId));
            inspectBtn.CustomMinimumSize = new Vector2(130, 0);
            hbox.AddChild(inspectBtn);

            return panel;
        }

        private void OnLogToJournalPressed()
        {
            if (_system == null || string.IsNullOrEmpty(_selectedRecordId)) return;

            string title = GetRecordShortTitle(_selectedRecordId);

            // De-theater: only file a journal reference for records the archive
            // has already recovered via a producer site. Never invent recovery.
            if (!_system.IsDiscovered(_selectedRecordId))
            {
                string? producer = _system.GetProducer(_selectedRecordId);
                _feedbackLabel.Text = string.IsNullOrEmpty(producer)
                    ? $"No producer site is activated for {title}. Record stays deferred."
                    : $"Recover {title} by discovering its producer site first.";
                return;
            }

            if (_journal == null)
            {
                _feedbackLabel.Text = $"Dossier reference noted for {title}. (Journal offline).";
                return;
            }

            string journalKey = $"black_projects_archive_manual_{_selectedRecordId}";
            string text = $"[ARCHIVAL INTEL] Filed reference for {title}. Preserved in Journal Codex for historical analysis.";
            var added = _journal.TryAddRawEntry(journalKey, text, null!, 0);
            _feedbackLabel.Text = added != null
                ? $"Dossier for {title} successfully logged to Journal Codex."
                : $"Dossier for {title} is already recorded in Journal Codex.";
        }

        // ── Event Handlers ──────────────────────────────────────────────────

        private void HandleSystemStateChanged()
        {
            RefreshView();
        }

        private void HandleRecordFirstDiscovered(string recordId, BlackProjectsTruthClass truthClass)
        {
            _feedbackLabel.Text = $"New intelligence recovered: {GetRecordShortTitle(recordId)}!";
            RefreshView();
        }

        // ── Formatters ──────────────────────────────────────────────────────

        private string GetRecordShortTitle(string recordId)
        {
            if (_system == null) return recordId;
            var catalog = _system.Catalog;

            var o = catalog.GetOrbital(recordId);
            if (o != null) return $"{o.Callsign} ({o.EntryType.Replace('_', ' ')})";

            var d = catalog.GetDrone(recordId);
            if (d != null) return $"{d.CarrierId} ({d.RecordType.Replace('_', ' ')})";

            var c = catalog.GetCobalt(recordId);
            if (c != null) return $"Directive {c.DirectiveCode}";

            var v = catalog.GetVault(recordId);
            if (v != null) return $"{v.VaultId} ({v.AuditType.Replace('_', ' ')})";

            return recordId;
        }

        private static string DescribeProducer(string? producerId)
        {
            if (string.IsNullOrEmpty(producerId)) return "Unknown Site";
            return producerId switch
            {
                "location_radar_site" => "Radar Site",
                "location_weather_station" => "Weather Station",
                "location_ammunition_depot" => "Ammunition Depot",
                "location_irradiated_forest" => "Irradiated Forest",
                "location_chemical_plant" => "Chemical Plant",
                "location_steelworks" => "Steelworks",
                "location_agricultural_research" => "Agri-Research",
                "location_metro_station" => "Metro Station",
                _ => producerId.Replace("location_", "").Replace('_', ' ')
            };
        }

        private static string FormatRelationKind(string kind) => kind switch
        {
            "corroborates" => "CORROBORATES",
            "contradicts" => "CONTRADICTS",
            "chronology_relation" => "CHRONOLOGY",
            "related_by_carrier" => "SAME CARRIER",
            "related_by_vault" => "SAME VAULT",
            "related_by_callsign" => "SAME CALLSIGN",
            "related_by_authority" => "AUTHORITY",
            _ => kind.ToUpperInvariant().Replace('_', ' ')
        };

        private static Color GetRelationColor(string kind) => kind switch
        {
            "corroborates" => AshfallUiHelpers.ColorSuccess,
            "contradicts" => AshfallUiHelpers.ColorRadiationAcute,
            "chronology_relation" => AshfallUiHelpers.ColorInfo,
            _ => AshfallUiHelpers.ToColor(DesignTheme.Muted)
        };
    }
}
