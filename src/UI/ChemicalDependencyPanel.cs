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
    /// ASHFALL — Chemical Dependency & Detox Management Interface.
    /// High-density terminal UI corresponding to Stitch screen 6f637fba7e5341d38ce7c0c37fc6493c.
    /// Manages addiction profiles, substance blood saturation, withdrawal symptoms,
    /// managed detox vs cold turkey protocols, and medical lockbox inventories.
    /// </summary>
    public partial class ChemicalDependencyPanel : Control
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _patientList = null!;
        private VBoxContainer _dossierContainer = null!;
        private VBoxContainer _protocolContainer = null!;
        private Label _eventLogLabel = null!;

        private ChemicalDependencyHostSession? _host;
        private string? _selectedSurvivorId;
        private string? _selectedItemId;

        public bool IsBound => _host != null;

        public void Bind(ChemicalDependencyHostSession session)
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

            _shell = new AshfallDashboardShell("SYS: CHEMICAL DEPENDENCY & DETOX MATRIX v2.4", minWidth: 1040, minHeight: 680);
            center.AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("active_deps", "DEPENDENCIES", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("in_detox", "IN DETOX", "0", AshfallMetricCard.Criticality.Normal, minWidth: 110);
            _statusRail.AddCard("craft_pen", "CRAFT PENALTY", "0%", AshfallMetricCard.Criticality.Normal, minWidth: 130);
            _statusRail.AddCard("combat_pen", "COMBAT PENALTY", "0%", AshfallMetricCard.Criticality.Normal, minWidth: 130);
            _statusRail.AddCard("status", "LOCKOUT STATUS", "ACTIVE", AshfallMetricCard.Criticality.Caution, minWidth: 130);

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

            // Column 1: Patient Roster
            var leftPanel = AshfallUiHelpers.MakePanel(minWidth: 310);
            leftPanel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            leftPanel.SizeFlagsStretchRatio = 0.9f;
            var leftMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingSm);
            leftPanel.AddChild(leftMargin);
            var leftVbox = new VBoxContainer();
            leftVbox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            leftMargin.AddChild(leftVbox);
            leftVbox.AddChild(AshfallUiHelpers.MakeSectionHeader("SURVIVOR PATIENT ROSTER"));
            var leftScroll = new ScrollContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            _patientList = new VBoxContainer();
            _patientList.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
            _patientList.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            leftScroll.AddChild(_patientList);
            leftVbox.AddChild(leftScroll);
            gridRow.AddChild(leftPanel);

            // Column 2: Active Patient Dossier
            var centerPanel = AshfallUiHelpers.MakePanel(minWidth: 380);
            centerPanel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            centerPanel.SizeFlagsStretchRatio = 1.2f;
            var centerMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingSm);
            centerPanel.AddChild(centerMargin);
            var centerVbox = new VBoxContainer();
            centerVbox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            centerMargin.AddChild(centerVbox);
            centerVbox.AddChild(AshfallUiHelpers.MakeSectionHeader("ACTIVE PATIENT DOSSIER"));
            var centerScroll = new ScrollContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            _dossierContainer = new VBoxContainer();
            _dossierContainer.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            _dossierContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            centerScroll.AddChild(_dossierContainer);
            centerVbox.AddChild(centerScroll);
            gridRow.AddChild(centerPanel);

            // Column 3: Treatment & Detox Protocols
            var rightPanel = AshfallUiHelpers.MakePanel(minWidth: 310);
            rightPanel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            rightPanel.SizeFlagsStretchRatio = 0.9f;
            var rightMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingSm);
            rightPanel.AddChild(rightMargin);
            var rightVbox = new VBoxContainer();
            rightVbox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            rightMargin.AddChild(rightVbox);
            rightVbox.AddChild(AshfallUiHelpers.MakeSectionHeader("TREATMENT & LOCKBOX"));
            var rightScroll = new ScrollContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            _protocolContainer = new VBoxContainer();
            _protocolContainer.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            _protocolContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            rightScroll.AddChild(_protocolContainer);
            rightVbox.AddChild(rightScroll);

            rightVbox.AddChild(AshfallUiHelpers.MakeSeparator());
            _eventLogLabel = AshfallUiHelpers.MakeMetadata("No recent medical events.");
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
            if (_patientList == null || _dossierContainer == null || _protocolContainer == null) return;

            AshfallUiHelpers.EmptyChildren(_patientList);
            AshfallUiHelpers.EmptyChildren(_dossierContainer);
            AshfallUiHelpers.EmptyChildren(_protocolContainer);

            if (_host == null || _statusRail == null)
            {
                _patientList.AddChild(AshfallUiHelpers.MakeEmptyStateLabel("No chemical dependency session bound", "offline"));
                _dossierContainer.AddChild(AshfallUiHelpers.MakeEmptyStateLabel("Patient dossier offline", "offline"));
                _protocolContainer.AddChild(AshfallUiHelpers.MakeEmptyStateLabel("Treatment protocols unavailable", "offline"));
                return;
            }

            var ledger = _host.System.Ledger;
            int totalDeps = 0;
            int inDetoxCount = 0;
            float maxCraftPen = 0f;
            float maxCombatPen = 0f;

            foreach (var kvp in ledger)
            {
                foreach (var dep in kvp.Value)
                {
                    totalDeps++;
                    if (dep.inManagedDetox || dep.inColdTurkey)
                    {
                        inDetoxCount++;
                        if (dep.inColdTurkey)
                        {
                            maxCraftPen = Math.Max(maxCraftPen, ChemicalDependencySystem.ColdTurkeyTremorCraftingPenalty);
                            maxCombatPen = Math.Max(maxCombatPen, ChemicalDependencySystem.ColdTurkeyTremorCombatPenalty);
                        }
                    }
                }
            }

            _statusRail.Set("active_deps", totalDeps.ToString(), totalDeps > 0 ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("in_detox", inDetoxCount.ToString(), inDetoxCount > 0 ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("craft_pen", $"{maxCraftPen:P0}", maxCraftPen > 0f ? AshfallMetricCard.Criticality.Critical : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("combat_pen", $"{maxCombatPen:P0}", maxCombatPen > 0f ? AshfallMetricCard.Criticality.Critical : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("status", totalDeps > 0 ? "MONITORED" : "STABLE", totalDeps > 0 ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);

            if (!string.IsNullOrEmpty(_host.LastEvent))
            {
                _eventLogLabel.Text = _host.LastEvent;
            }

            // Populate Roster
            if (ledger.Count == 0)
            {
                _patientList.AddChild(AshfallUiHelpers.MakeMetadata("No chemical dependency records on file."));
            }
            else
            {
                foreach (var kvp in ledger)
                {
                    string survivorId = kvp.Key;
                    var deps = kvp.Value;
                    if (deps == null || deps.Count == 0) continue;

                    if (_selectedSurvivorId == null)
                    {
                        _selectedSurvivorId = survivorId;
                        _selectedItemId = deps[0].itemId;
                    }

                    var patientCard = AshfallUiHelpers.MakePanel();
                    var cardMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingXs);
                    patientCard.AddChild(cardMargin);
                    var cardVbox = new VBoxContainer();
                    cardVbox.AddThemeConstantOverride("separation", 4);
                    cardMargin.AddChild(cardVbox);

                    var nameRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
                    nameRow.AddChild(AshfallUiHelpers.MakeBadgeIcon("badge_chemical_dependency", 18));
                    var nameLbl = AshfallUiHelpers.MakeBody(FormatSurvivorName(survivorId));
                    nameLbl.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                    nameRow.AddChild(nameLbl);
                    cardVbox.AddChild(nameRow);

                    foreach (var dep in deps)
                    {
                        string mode = dep.inManagedDetox ? "[MANAGED DETOX]" : dep.inColdTurkey ? "[COLD TURKEY]" : dep.dependencyLevel >= ChemicalDependencySystem.DependencyThreshold ? "[ADDICTED]" : "[HABITUATED]";
                        string gauge = BuildGauge(dep.dependencyLevel);

                        var depRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
                        var itemLbl = AshfallUiHelpers.MakeMono($"{dep.itemId.ToUpperInvariant()}: {gauge} {dep.dependencyLevel:P0}");
                        itemLbl.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                        itemLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(dep.dependencyLevel >= 0.6f ? DesignTheme.Critical : dep.dependencyLevel >= 0.3f ? DesignTheme.Hot : DesignTheme.Pale));
                        depRow.AddChild(itemLbl);

                        var modeLbl = AshfallUiHelpers.MakeSmall(mode);
                        modeLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(dep.inColdTurkey ? DesignTheme.Critical : dep.inManagedDetox ? DesignTheme.Lethe : DesignTheme.Warm));
                        depRow.AddChild(modeLbl);
                        cardVbox.AddChild(depRow);

                        var selectBtn = AshfallUiHelpers.MakeButton($"SELECT // {dep.itemId}", () =>
                        {
                            _selectedSurvivorId = survivorId;
                            _selectedItemId = dep.itemId;
                            RefreshView();
                        });
                        selectBtn.CustomMinimumSize = new Vector2(0, 24);
                        cardVbox.AddChild(selectBtn);
                    }

                    _patientList.AddChild(patientCard);
                }
            }

            // Populate Dossier for selected survivor & item
            if (_selectedSurvivorId != null && _selectedItemId != null)
            {
                var deps = _host.System.DependenciesFor(_selectedSurvivorId);
                var activeDep = deps.FirstOrDefault(d => d.itemId == _selectedItemId);
                if (activeDep != null)
                {
                    _dossierContainer.AddChild(AshfallUiHelpers.MakeSectionHeader($"PATIENT: {FormatSurvivorName(_selectedSurvivorId)}"));
                    _dossierContainer.AddChild(AshfallUiHelpers.MakeDataRow("Target Substance", activeDep.itemId.ToUpperInvariant(), AshfallUiHelpers.ToColor(DesignTheme.Warm)));
                    _dossierContainer.AddChild(AshfallUiHelpers.MakeDataRow("Substance Category", activeDep.kind, AshfallUiHelpers.ToColor(DesignTheme.Lethe)));
                    _dossierContainer.AddChild(AshfallUiHelpers.MakeDataRow("Blood Saturation / Level", $"{BuildGauge(activeDep.dependencyLevel)} {activeDep.dependencyLevel:P1}", AshfallUiHelpers.ToColor(activeDep.dependencyLevel >= 0.5f ? DesignTheme.Critical : DesignTheme.Hot)));
                    _dossierContainer.AddChild(AshfallUiHelpers.MakeDataRow("Detox Program", activeDep.inManagedDetox ? "Managed Clinical Detox (120h)" : activeDep.inColdTurkey ? "Cold Turkey Withdrawal (72h)" : "Uncontrolled Active Use", AshfallUiHelpers.ToColor(activeDep.inColdTurkey ? DesignTheme.Critical : activeDep.inManagedDetox ? DesignTheme.Lethe : DesignTheme.Dim)));
                    _dossierContainer.AddChild(AshfallUiHelpers.MakeDataRow("Detox Progress", $"{activeDep.detoxProgressHours:F1} Hours Completed", AshfallUiHelpers.ToColor(DesignTheme.Pale)));

                    _dossierContainer.AddChild(AshfallUiHelpers.MakeSeparator());
                    _dossierContainer.AddChild(AshfallUiHelpers.MakeSubsectionHeader("WITHDRAWAL SYMPTOM PROFILES"));

                    if (activeDep.inColdTurkey)
                    {
                        _dossierContainer.AddChild(AshfallUiHelpers.MakeCritical($"• Severe Muscular Tremors: Crafting Penalty -{ChemicalDependencySystem.ColdTurkeyTremorCraftingPenalty:P0}"));
                        _dossierContainer.AddChild(AshfallUiHelpers.MakeCritical($"• Combat Reflex Impairment: Combat Penalty -{ChemicalDependencySystem.ColdTurkeyTremorCombatPenalty:P0}"));
                        _dossierContainer.AddChild(AshfallUiHelpers.MakeCritical($"• Acute Psychological Distress: Morale Drain -{ChemicalDependencySystem.ColdTurkeyMoraleDrainPerHour:F1}/hr"));
                    }
                    else if (activeDep.inManagedDetox)
                    {
                        _dossierContainer.AddChild(AshfallUiHelpers.MakeWarning($"• Controlled Weaning: Mild Morale Drain -{ChemicalDependencySystem.ManagedDetoxMoraleDrainPerHour:F1}/hr"));
                        _dossierContainer.AddChild(AshfallUiHelpers.MakeBody("• Vital stabilization active — zero motor tremor penalties under medical supervision."));
                    }
                    else
                    {
                        _dossierContainer.AddChild(AshfallUiHelpers.MakeBody("• Patient is not currently in an active detox protocol. Natural metabolic decay rate: 5%/day clean."));
                    }

                    // Protocols
                    _protocolContainer.AddChild(AshfallUiHelpers.MakeSectionHeader("ADMINISTER TREATMENT"));

                    var btnManaged = AshfallUiHelpers.MakeButton("BEGIN MANAGED DETOX (120h)", () =>
                    {
                        if (_host.System.BeginManagedDetox(_selectedSurvivorId, _selectedItemId))
                        {
                            RefreshView();
                        }
                    });
                    btnManaged.Disabled = activeDep.inManagedDetox || activeDep.dependencyLevel < ChemicalDependencySystem.DependencyThreshold;
                    _protocolContainer.AddChild(btnManaged);

                    var btnCold = AshfallUiHelpers.MakeButton("COMMENCE COLD TURKEY (72h)", () =>
                    {
                        if (_host.System.BeginColdTurkey(_selectedSurvivorId, _selectedItemId))
                        {
                            RefreshView();
                        }
                    });
                    btnCold.Disabled = activeDep.inColdTurkey || activeDep.dependencyLevel < ChemicalDependencySystem.DependencyThreshold;
                    _protocolContainer.AddChild(btnCold);

                    _protocolContainer.AddChild(AshfallUiHelpers.MakeSeparator());
                    _protocolContainer.AddChild(AshfallUiHelpers.MakeSubsectionHeader("MEDICAL LOCKBOX CONTROLS"));
                    _protocolContainer.AddChild(AshfallUiHelpers.MakeDataRow("Clean IV Fluids", "Available", AshfallUiHelpers.ToColor(DesignTheme.Pale)));
                    _protocolContainer.AddChild(AshfallUiHelpers.MakeDataRow("Neuro-blockers", "Secured", AshfallUiHelpers.ToColor(DesignTheme.Lethe)));
                    _protocolContainer.AddChild(AshfallUiHelpers.MakeDataRow("Herbal Sedatives", "In Stock", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
                    _protocolContainer.AddChild(AshfallUiHelpers.MakeDataRow("Emergency Purge System", "ARMED", AshfallUiHelpers.ToColor(DesignTheme.Critical)));
                }
            }
            else
            {
                _dossierContainer.AddChild(AshfallUiHelpers.MakeMetadata("Select a patient and substance from the roster to view telemetry."));
            }
        }

        private static string BuildGauge(float ratio)
        {
            int totalBars = 10;
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
