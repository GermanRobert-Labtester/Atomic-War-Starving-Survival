// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Crafting;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Chemical synthesis and retort laboratory panel (Plan IX / CHEM-01).
    /// Bound to ChemicalSynthesisHostSession -> ChemicalSynthesisSystem.
    /// Manages industrial retorts, catalyst degradation, scrubber reserves,
    /// and hazardous chemical batch synthesis.
    /// Presentation only — all simulation logic resides in Ashfall.Core.
    /// </summary>
    public partial class ChemicalLabPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _vesselsContainer = null!;
        private VBoxContainer _dossierContainer = null!;
        private Label _feedbackLabel = null!;
        private ChemicalSynthesisHostSession? _host;

        private string? _selectedVesselId;
        private string? _selectedProcessId;
        private string _lastFeedbackMessage = "Chemical synthesis apparatus online — ready for batch sequencing.";

        public bool IsBound => _host != null;

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            BuildUi();
            RefreshView();
        }

        public void Bind(ChemicalSynthesisHostSession? host)
        {
            if (_host != null)
                _host.StateChanged -= HandleStateChanged;

            _host = host;

            if (_host != null)
                _host.StateChanged += HandleStateChanged;

            RefreshView();
        }

        public void Bind(object? session)
        {
            Bind(session as ChemicalSynthesisHostSession);
        }

        public void Unbind()
        {
            if (_host != null)
            {
                _host.StateChanged -= HandleStateChanged;
                _host = null;
            }
            RefreshView();
        }

        public void Open()
        {
            Visible = true;
            RefreshView();
        }

        public void Close()
        {
            Visible = false;
            OnClose?.Invoke();
        }

        public override void _ExitTree()
        {
            if (_host != null)
                _host.StateChanged -= HandleStateChanged;
        }

        private void HandleStateChanged()
        {
            if (_host != null && !string.IsNullOrEmpty(_host.LastEvent))
                _lastFeedbackMessage = _host.LastEvent;
            RefreshView();
        }

        private void BuildUi()
        {
            if (_shell != null) return;

            _shell = new AshfallDashboardShell("CHEMICAL SYNTHESIS LAB // INDUSTRIAL REAGENTS", minWidth: 1060, minHeight: 680);
            AddChild(_shell);
            _shell.AttachHeaderCloseButton("CLOSE", Close);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("apparatus_tier", "Apparatus Tier", "Tier 1", AshfallMetricCard.Criticality.Normal, minWidth: 130);
            _statusRail.AddCard("active_retorts", "Active Retorts", "0 / 2", AshfallMetricCard.Criticality.Normal, minWidth: 130);
            _statusRail.AddCard("scrubber_reserve", "Scrubber Reserve", "100%", AshfallMetricCard.Criticality.Normal, minWidth: 140);
            _statusRail.AddCard("catalyst_health", "Catalyst Health", "100%", AshfallMetricCard.Criticality.Normal, minWidth: 140);
            _statusRail.AddCard("lab_status", "Lab Status", "NOMINAL", AshfallMetricCard.Criticality.Normal, minWidth: 150);

            var contentVBox = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingSm);
            contentVBox.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            contentVBox.SizeFlagsVertical = SizeFlags.ExpandFill;

            var introLabel = AshfallUiHelpers.MakeBody(
                "Industrial retort battery for synthesizing pharmaceutical reagents, acids, munitions feedstock, and fertilizer.\n" +
                "Reactions demand continuous atmospheric scrubbing; depleted scrubbers risk toxic vapor blowout and operator casualties."
            );
            introLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Dim));
            contentVBox.AddChild(introLabel);

            var hsplit = new HSplitContainer
            {
                SizeFlagsHorizontal = SizeFlags.ExpandFill,
                SizeFlagsVertical = SizeFlags.ExpandFill
            };
            contentVBox.AddChild(hsplit);

            // Left Column: Retort Battery Register
            var leftCard = AshfallUiHelpers.MakePanel();
            leftCard.CustomMinimumSize = new Vector2(440, 0);
            var leftMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingSm);
            leftCard.AddChild(leftMargin);
            var leftVBox = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingSm);
            leftVBox.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            leftVBox.SizeFlagsVertical = SizeFlags.ExpandFill;
            leftMargin.AddChild(leftVBox);

            leftVBox.AddChild(AshfallUiHelpers.MakeSectionHeader("RETORT BATTERY REGISTER"));
            leftVBox.AddChild(AshfallUiHelpers.MakeSeparator());

            var leftScroll = new ScrollContainer
            {
                SizeFlagsHorizontal = SizeFlags.ExpandFill,
                SizeFlagsVertical = SizeFlags.ExpandFill,
                HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled
            };
            leftVBox.AddChild(leftScroll);

            _vesselsContainer = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingSm);
            _vesselsContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            leftScroll.AddChild(_vesselsContainer);
            hsplit.AddChild(leftCard);

            // Right Column: Reaction Dossier & Retort Controls
            var rightCard = AshfallUiHelpers.MakePanel();
            rightCard.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            rightCard.SizeFlagsVertical = SizeFlags.ExpandFill;
            var rightMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingSm);
            rightCard.AddChild(rightMargin);
            var rightVBox = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingSm);
            rightVBox.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            rightVBox.SizeFlagsVertical = SizeFlags.ExpandFill;
            rightMargin.AddChild(rightVBox);

            rightVBox.AddChild(AshfallUiHelpers.MakeSectionHeader("SYNTHESIS DOSSIER & RETORT CONTROLS"));
            rightVBox.AddChild(AshfallUiHelpers.MakeSeparator());

            var rightScroll = new ScrollContainer
            {
                SizeFlagsHorizontal = SizeFlags.ExpandFill,
                SizeFlagsVertical = SizeFlags.ExpandFill,
                HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled
            };
            rightVBox.AddChild(rightScroll);

            _dossierContainer = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingMd);
            _dossierContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            rightScroll.AddChild(_dossierContainer);
            hsplit.AddChild(rightCard);

            // Feedback strip
            var feedbackBox = AshfallUiHelpers.MakePanel(0, 48);
            feedbackBox.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            var feedbackMargin = AshfallUiHelpers.MakeMargins(12, 6, 12, 6);
            feedbackBox.AddChild(feedbackMargin);

            _feedbackLabel = AshfallUiHelpers.MakeBody(_lastFeedbackMessage);
            _feedbackLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            feedbackMargin.AddChild(_feedbackLabel);
            contentVBox.AddChild(feedbackBox);

            _shell.SetContent(contentVBox);
        }

        public void RefreshView()
        {
            if (!IsInsideTree() || _shell == null) return;

            if (_feedbackLabel != null)
                _feedbackLabel.Text = _lastFeedbackMessage;

            if (_host == null)
            {
                RenderUnbound();
                return;
            }

            var sys = _host.System;
            var cat = _host.Catalog;

            // Update Status Rail
            int activeCount = sys.Vessels.Count(v => !string.IsNullOrEmpty(v.activeProcessId));
            float avgCat = sys.Vessels.Count > 0 ? sys.Vessels.Average(v => v.catalystCondition) : 100f;
            bool hasIncident = sys.Vessels.Any(v => v.failureState != "None");

            _statusRail?.Set("apparatus_tier", $"Tier {sys.ApparatusTier}",
                sys.ApparatusTier >= 3 ? AshfallMetricCard.Criticality.Normal : AshfallMetricCard.Criticality.Caution);

            _statusRail?.Set("active_retorts", $"{activeCount} / {sys.Vessels.Count}",
                activeCount > 0 ? AshfallMetricCard.Criticality.Normal : AshfallMetricCard.Criticality.Caution);

            _statusRail?.Set("scrubber_reserve", $"{sys.ScrubberReserve:F0}%",
                sys.ScrubberReserve < 30f ? AshfallMetricCard.Criticality.Critical : (sys.ScrubberReserve < 60f ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal));

            _statusRail?.Set("catalyst_health", $"{avgCat:F0}%",
                avgCat < 40f ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);

            _statusRail?.Set("lab_status", hasIncident ? "BREACH / LOSS" : (activeCount > 0 ? "REACTION ACTIVE" : "IDLE"),
                hasIncident ? AshfallMetricCard.Criticality.Critical : AshfallMetricCard.Criticality.Normal);

            // Rebuild Vessels List
            AshfallUiHelpers.EmptyChildren(_vesselsContainer);

            if (_selectedVesselId == null && sys.Vessels.Count > 0)
                _selectedVesselId = sys.Vessels[0].vesselId;

            foreach (var vessel in sys.Vessels)
            {
                var card = CreateVesselCard(vessel, cat);
                _vesselsContainer.AddChild(card);
            }

            // Rebuild Dossier
            RenderDossier(sys, cat);
        }

        private Control CreateVesselCard(ChemicalRetortState vessel, ChemicalSynthesisCatalog cat)
        {
            var isSelected = vessel.vesselId == _selectedVesselId;
            var panel = AshfallUiHelpers.MakePanel();
            panel.SizeFlagsHorizontal = SizeFlags.ExpandFill;

            var margin = AshfallUiHelpers.MakeMargins(8);
            panel.AddChild(margin);

            var vbox = AshfallUiHelpers.MakeVBox(4);
            vbox.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            margin.AddChild(vbox);

            var header = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
            header.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            vbox.AddChild(header);

            var lblTitle = AshfallUiHelpers.MakeLabel($"RETORT VESSEL {vessel.vesselId.ToUpperInvariant()}", DesignTheme.FontSizeBody, true);
            if (isSelected)
                lblTitle.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            header.AddChild(lblTitle);

            header.AddChild(new Control { SizeFlagsHorizontal = SizeFlags.ExpandFill });

            bool hasIncident = vessel.failureState != "None";
            bool isReacting = !string.IsNullOrEmpty(vessel.activeProcessId);
            var badgeSeverity = hasIncident ? SeverityLevel.Critical : (isReacting ? SeverityLevel.Attention : SeverityLevel.Normal);
            string badgeText = hasIncident ? vessel.failureState.ToUpperInvariant() : (isReacting ? "REACTING" : "IDLE");
            header.AddChild(AshfallUiHelpers.MakeSeverityBadge(badgeSeverity, badgeText));

            if (!string.IsNullOrEmpty(vessel.activeProcessId))
            {
                var def = cat.GetProcess(vessel.activeProcessId);
                string procName = def?.displayName ?? vessel.activeProcessId;
                vbox.AddChild(AshfallUiHelpers.MakeBody($"Batch: {procName} ({vessel.processProgress}/{vessel.processingTicksRequired} ticks)"));
            }

            var stats = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingMd);
            stats.AddChild(AshfallUiHelpers.MakeSmall($"Scrubber: {vessel.scrubberCondition:F0}%"));
            stats.AddChild(AshfallUiHelpers.MakeSmall($"Catalyst: {vessel.catalystCondition:F0}%"));
            stats.AddChild(AshfallUiHelpers.MakeSmall($"Heat: {vessel.heatBand}"));
            vbox.AddChild(stats);

            var selectBtn = AshfallUiHelpers.MakeButton(isSelected ? "[SELECTED]" : "SELECT VESSEL", () =>
            {
                _selectedVesselId = vessel.vesselId;
                RefreshView();
            });
            selectBtn.SizeFlagsHorizontal = SizeFlags.ShrinkEnd;
            vbox.AddChild(selectBtn);

            return panel;
        }

        private void RenderDossier(ChemicalSynthesisSystem sys, ChemicalSynthesisCatalog cat)
        {
            AshfallUiHelpers.EmptyChildren(_dossierContainer);

            var vessel = sys.GetVessel(_selectedVesselId ?? "");
            if (vessel == null)
            {
                _dossierContainer.AddChild(AshfallUiHelpers.MakeBody("No vessel selected. Choose a retort vessel from the battery."));
                return;
            }

            var titleLabel = AshfallUiHelpers.MakeLabel($"RETORT VESSEL {vessel.vesselId.ToUpperInvariant()} DOSSIER", DesignTheme.FontSizeH3, true);
            titleLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            _dossierContainer.AddChild(titleLabel);

            _dossierContainer.AddChild(AshfallUiHelpers.MakeSeparator());

            // Dossier Metrics Grid
            var grid = new GridContainer { Columns = 2, SizeFlagsHorizontal = SizeFlags.ExpandFill };
            grid.AddThemeConstantOverride("h_separation", 16);
            grid.AddThemeConstantOverride("v_separation", 6);
            _dossierContainer.AddChild(grid);

            grid.AddChild(AshfallUiHelpers.MakeSmall("Chamber Seal:"));
            grid.AddChild(AshfallUiHelpers.MakeBody(vessel.isSealed ? "Hermetically Sealed" : "Unsealed / Leaking"));

            grid.AddChild(AshfallUiHelpers.MakeSmall("Heat Band:"));
            grid.AddChild(AshfallUiHelpers.MakeBody(vessel.heatBand));

            grid.AddChild(AshfallUiHelpers.MakeSmall("Pressure Band:"));
            grid.AddChild(AshfallUiHelpers.MakeBody(vessel.pressureBand));

            grid.AddChild(AshfallUiHelpers.MakeSmall("Failure State:"));
            grid.AddChild(AshfallUiHelpers.MakeBody(vessel.failureState == "None" ? "None (Nominal)" : vessel.failureState));

            _dossierContainer.AddChild(AshfallUiHelpers.MakeSeparator());

            // Active Reaction or Recipe Selection
            if (!string.IsNullOrEmpty(vessel.activeProcessId))
            {
                var def = cat.GetProcess(vessel.activeProcessId);

                _dossierContainer.AddChild(AshfallUiHelpers.MakeSectionHeader("ACTIVE BATCH REACTION"));
                _dossierContainer.AddChild(AshfallUiHelpers.MakeLabel(def?.displayName ?? vessel.activeProcessId, DesignTheme.FontSizeBody, true));
                if (def != null)
                    _dossierContainer.AddChild(AshfallUiHelpers.MakeBody(def.description));

                _dossierContainer.AddChild(AshfallUiHelpers.MakeBody($"Progress: {vessel.processProgress} / {vessel.processingTicksRequired} Days"));

                bool canHarvest = vessel.processProgress >= vessel.processingTicksRequired && vessel.failureState == "None";

                var btnRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
                btnRow.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                _dossierContainer.AddChild(btnRow);

                var harvestBtn = AshfallUiHelpers.MakeButton("[HARVEST SYNTHESIZED PRODUCT]", () =>
                {
                    _host?.TryHarvestOutput(vessel.vesselId);
                });
                harvestBtn.Disabled = !canHarvest;
                btnRow.AddChild(harvestBtn);

                var purgeBtn = AshfallUiHelpers.MakeButton("[PURGE / NEUTRALIZE BATCH]", () =>
                {
                    _host?.TryPurgeVessel(vessel.vesselId);
                });
                btnRow.AddChild(purgeBtn);
            }
            else
            {
                _dossierContainer.AddChild(AshfallUiHelpers.MakeSectionHeader("SYNTHESIS RECIPES"));
                _dossierContainer.AddChild(AshfallUiHelpers.MakeBody("Select a chemical reaction to sequence in this retort:"));

                var processes = cat.Processes.Values.ToList();
                if (_selectedProcessId == null && processes.Count > 0)
                    _selectedProcessId = processes[0].id;

                foreach (var proc in processes)
                {
                    var isProcSelected = proc.id == _selectedProcessId;
                    var procRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
                    procRow.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                    _dossierContainer.AddChild(procRow);

                    string req = $"[Tier {proc.requiredApparatusTier}]";
                    var nameLabel = AshfallUiHelpers.MakeLabel($"{req} {proc.displayName}", DesignTheme.FontSizeMono, isProcSelected);
                    nameLabel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                    procRow.AddChild(nameLabel);

                    var pSelect = AshfallUiHelpers.MakeButton(isProcSelected ? "[SELECTED]" : "SELECT", () =>
                    {
                        _selectedProcessId = proc.id;
                        RefreshView();
                    });
                    procRow.AddChild(pSelect);
                }

                var selectedProc = cat.GetProcess(_selectedProcessId ?? "");
                if (selectedProc != null)
                {
                    _dossierContainer.AddChild(AshfallUiHelpers.MakeSeparator());

                    // State -> Blocker -> Cost -> Consequence Box
                    _dossierContainer.AddChild(AshfallUiHelpers.MakeSectionHeader("PROCESS SPECIFICATIONS"));

                    string inputs = string.Join(", ", selectedProc.inputItems.Select(kv => $"{kv.Key} ×{kv.Value}"));
                    string outputs = string.Join(", ", selectedProc.outputItems.Select(kv => $"{kv.Key} ×{kv.Value}"));

                    _dossierContainer.AddChild(AshfallUiHelpers.MakeBody($"• STATE: Retort {vessel.vesselId} idle and ready."));

                    bool tierMet = sys.ApparatusTier >= selectedProc.requiredApparatusTier;
                    string blocker = tierMet ? "None (Apparatus tier satisfied)" : $"Requires Apparatus Tier {selectedProc.requiredApparatusTier} (Current: Tier {sys.ApparatusTier})";
                    _dossierContainer.AddChild(AshfallUiHelpers.MakeBody($"• BLOCKER: {blocker}"));

                    _dossierContainer.AddChild(AshfallUiHelpers.MakeBody($"• REAGENT COST: {inputs}"));
                    _dossierContainer.AddChild(AshfallUiHelpers.MakeBody($"• YIELD: {outputs} (Duration: {selectedProc.processingTicks} ticks)"));
                    _dossierContainer.AddChild(AshfallUiHelpers.MakeBody($"• RISK / DEMAND: Volatility {selectedProc.volatilityRating:P0}, Scrubber Demand {selectedProc.scrubberDemand:F1}/tick"));

                    var startBtn = AshfallUiHelpers.MakeButton("[START REACTION BATCH]", () =>
                    {
                        _host?.TryStartProcess(selectedProc.id, vessel.vesselId);
                    });
                    startBtn.Disabled = !tierMet;
                    _dossierContainer.AddChild(startBtn);
                }
            }

            _dossierContainer.AddChild(AshfallUiHelpers.MakeSeparator());

            // Maintenance & Upgrades Box
            _dossierContainer.AddChild(AshfallUiHelpers.MakeSectionHeader("FACILITY MAINTENANCE & EXPANSION"));

            var maintRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingMd);
            maintRow.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _dossierContainer.AddChild(maintRow);

            var serviceBtn = AshfallUiHelpers.MakeButton("[SERVICE SCRUBBER MEDIA]", () =>
            {
                _host?.TryServiceScrubber(vessel.vesselId);
            });
            serviceBtn.TooltipText = "Restores scrubber to 100% (Costs: scrap_chemical ×2, clean_water ×1)";
            maintRow.AddChild(serviceBtn);

            if (sys.ApparatusTier < 3)
            {
                int nextTier = sys.ApparatusTier + 1;
                var upgradeBtn = AshfallUiHelpers.MakeButton($"[UPGRADE APPARATUS TO TIER {nextTier}]", () =>
                {
                    _host?.TryUpgradeApparatus(nextTier);
                });
                upgradeBtn.TooltipText = $"Unlocks advanced chemical processes (Costs: scrap_metal ×{nextTier * 4}, copper_wire ×{nextTier * 2})";
                maintRow.AddChild(upgradeBtn);
            }
        }

        private void RenderUnbound()
        {
            _statusRail?.Set("apparatus_tier", "OFFLINE", AshfallMetricCard.Criticality.Caution);
            _statusRail?.Set("active_retorts", "0", AshfallMetricCard.Criticality.Normal);
            _statusRail?.Set("scrubber_reserve", "0%", AshfallMetricCard.Criticality.Warn);
            _statusRail?.Set("catalyst_health", "0%", AshfallMetricCard.Criticality.Warn);
            _statusRail?.Set("lab_status", "OFFLINE", AshfallMetricCard.Criticality.Caution);

            AshfallUiHelpers.EmptyChildren(_vesselsContainer);
            AshfallUiHelpers.EmptyChildren(_dossierContainer);

            var unboundLabel = AshfallUiHelpers.MakeBody("[CONSOLE OFFLINE] No connection to ChemicalSynthesisHostSession.");
            unboundLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Dim));
            _dossierContainer.AddChild(unboundLabel);

            _feedbackLabel.Text = "Chemical laboratory is offline. Bind a live ChemicalSynthesisHostSession to initialize controls.";
        }
    }
}
