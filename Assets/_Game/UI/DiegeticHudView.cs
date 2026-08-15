using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// Pure UI Toolkit view for the diegetic HUD: hatch ammo/arms, expedition
    /// encounter log, and inventory focus tooltip. Builds a VisualElement tree
    /// (no MonoBehaviour) so EditMode tests can paint without a UIDocument.
    /// </summary>
    public class DiegeticHudView
    {
        private enum PanelStatus
        {
            Default,
            Warning,
            Critical
        }

        public const string RootName = "diegetic-root";
        public const string HatchPanelName = "hatch-panel";
        public const string HatchStatusName = "hatch-status";
        public const string HatchAmmoName = "hatch-ammo";
        public const string HatchArmsName = "hatch-arms";
        public const string EncounterPanelName = "encounter-panel";
        public const string EncounterStatusName = "encounter-status";
        public const string EncounterListName = "encounter-list";
        public const string StoresPanelName = "stores-panel";
        public const string StoresSummaryName = "stores-summary";
        public const string StoresTooltipName = "stores-tooltip";
        public const string VitalsPanelName = "vitals-panel";
        public const string VitalsClockName = "vitals-clock";
        public const string VitalsDoseName = "vitals-dose";
        public const string VitalsNeedsName = "vitals-needs";
        public const string EventPanelName = "event-panel";
        public const string EventTitleName = "event-title";
        public const string EventBodyName = "event-body";
        public const string EventChoicesName = "event-choices";
        public const string WorkbenchPanelName = "workbench-panel";
        public const string WorkbenchBodyName = "workbench-body";
        public const string EndgamePanelName = "endgame-panel";
        public const string EndgameStatusName = "endgame-status";
        public const string EndgameBodyName = "endgame-body";
        public const string PowerGridPanelName = "power-grid-panel";
        public const string PowerGridBudgetName = "power-grid-budget";
        public const string PowerGridSourcesName = "power-grid-sources";
        public const string PowerGridLoadsName = "power-grid-loads";
        public const string ScavengePanelName = "scavenge-panel";
        public const string ScavengeBodyName = "scavenge-body";
        public const string OverflowCratePanelName = "overflow-crate-panel";
        public const string OverflowCrateBodyName = "overflow-crate-body";
        public const string FieldGearLoadoutPanelName = "field-gear-loadout-panel";
        public const string FieldGearLoadoutBodyName = "field-gear-loadout-body";
        public const string BunkerRationingPanelName = "bunker-rationing-panel";
        public const string BunkerRationingBodyName = "bunker-rationing-body";
        public const string WaterPurificationPanelName = "water-purification-panel";
        public const string WaterPurificationBodyName = "water-purification-body";
        public const string AirHeatManagementPanelName = "air-heat-management-panel";
        public const string AirHeatManagementBodyName = "air-heat-management-body";
        public const string BunkerMaintenancePanelName = "bunker-maintenance-panel";
        public const string BunkerMaintenanceBodyName = "bunker-maintenance-body";
        public const string SurvivorTaskBoardPanelName = "survivor-task-board-panel";
        public const string SurvivorTaskBoardBodyName = "survivor-task-board-body";
        // Expansion II: faction-pressure panel (Garrison/Militia/Cult/Warlord).
        public const string FactionPressurePanelName = "faction-pressure-panel";
        public const string FactionPressureBodyName = "faction-pressure-body";

        // Expansion IV: structural entropy wireframe (rebar grid overlay).
        public const string StructuralEntropyPanelName   = "structural-entropy-panel";
        public const string StructuralEntropyStatusName  = "structural-entropy-status";
        public const string StructuralEntropyBarName     = "structural-entropy-bar-fill";
        public const string RebarGridName                = "rebar-grid";

        // Expansion IV: lethe drip gauge (amnestic reservoir sight-glass).
        public const string LetheDripPanelName     = "lethe-drip-panel";
        public const string LetheDripStatusName    = "lethe-drip-status";
        public const string LetheDripGaugeFillName = "lethe-drip-gauge-fill";
        public const string LetheDropletsName      = "lethe-droplets";

        // Expansion IV: ozone scourge overlay + warning panel.
        public const string OzoneScourgeOverlayName = "ozone-scourge-overlay";
        public const string OzoneWarningPanelName   = "ozone-warning-panel";
        public const string OzoneWarningStatusName  = "ozone-warning-status";
        public const string OzoneTimerBarFillName   = "ozone-timer-bar-fill";

        // Expansion IV: memory flash vignette (full-screen monochrome burst).
        public const string MemoryFlashVignetteName = "memory-flash-vignette";
        public const string MemoryFlashTextName     = "memory-flash-text";

        // Expansion IV: generational psychology cohort readout.
        public const string GenerationalPanelName    = "generational-psychology-panel";
        public const string GenerationalBodyName     = "generational-body";
        public const string GenerationalEventName    = "generational-event-text";

        /// <summary>Core needs, in fixed display order. Fixed so the rows do not
        /// reshuffle between paints as the model's dictionary ordering changes.</summary>
        public static readonly string[] CoreNeedIds = { "hunger", "thirst", "fatigue", "warmth" };

        public VisualElement Root { get; private set; }
        public VisualElement HatchPanel { get; private set; }
        public Label HatchStatus { get; private set; }
        public Label HatchAmmo { get; private set; }
        public Label HatchArms { get; private set; }
        public VisualElement EncounterPanel { get; private set; }
        public Label EncounterStatus { get; private set; }
        public VisualElement EncounterList { get; private set; }
        public VisualElement StoresPanel { get; private set; }
        public Label StoresSummary { get; private set; }
        public Label StoresTooltip { get; private set; }
        public VisualElement VitalsPanel { get; private set; }
        public Label VitalsClock { get; private set; }
        public Label VitalsDose { get; private set; }
        public VisualElement VitalsNeeds { get; private set; }

        // Cached per-need row parts, indexed by CoreNeedIds. Built once in
        // Build()/BindExisting(); PaintVitals only mutates style.width and
        // text on these rather than clearing and re-adding new VisualElements
        // each frame. Needs change 4x per survivor per tick -- rebuilding the
        // tree each time was a per-frame allocation firehose.
        private Label[] _rowLabels;
        private VisualElement[] _rowFills;
        private Label[] _rowValues;
        // Fingerprint of the last PaintVitals input. When the new frame's
        // packed key matches, we bail before any string allocation, so
        // hourly clock ticks that don't change a need skip the entire body.
        private int _lastVitalsKey;
        // Knuth's golden-ratio hash multiplier. Written as an unchecked cast
        // because 2654435761 does not fit in an int: the bare literal types as
        // uint and silently widens the whole key expression to long.
        private const int GoldenRatioHashMul = unchecked((int)2654435761u);
        public VisualElement EventPanel { get; private set; }
        public Label EventTitle { get; private set; }
        public Label EventBody { get; private set; }
        public VisualElement EventChoices { get; private set; }
        public VisualElement WorkbenchPanel { get; private set; }
        public Label WorkbenchBody { get; private set; }
        public VisualElement EndgamePanel { get; private set; }
        public Label EndgameStatus { get; private set; }
        public Label EndgameBody { get; private set; }
        public VisualElement PowerGridPanel { get; private set; }
        public Label PowerGridBudget { get; private set; }
        public Label PowerGridSources { get; private set; }
        public Label PowerGridLoads { get; private set; }
        public VisualElement ScavengePanel { get; private set; }
        public Label ScavengeBody { get; private set; }
        public VisualElement OverflowCratePanel { get; private set; }
        public Label OverflowCrateBody { get; private set; }
        public VisualElement FieldGearLoadoutPanel { get; private set; }
        public Label FieldGearLoadoutBody { get; private set; }
        public VisualElement BunkerRationingPanel { get; private set; }
        public Label BunkerRationingBody { get; private set; }
        public VisualElement WaterPurificationPanel { get; private set; }
        public Label WaterPurificationBody { get; private set; }
        public VisualElement AirHeatManagementPanel { get; private set; }
        public Label AirHeatManagementBody { get; private set; }
        public VisualElement BunkerMaintenancePanel { get; private set; }
        public Label BunkerMaintenanceBody { get; private set; }
        public VisualElement SurvivorTaskBoardPanel { get; private set; }
        public Label SurvivorTaskBoardBody { get; private set; }
        // Expansion II: faction-pressure panel.
        public VisualElement FactionPressurePanel { get; private set; }
        public Label FactionPressureBody { get; private set; }

        // Expansion IV: structural entropy wireframe.
        public VisualElement StructuralEntropyPanel    { get; private set; }
        public Label         StructuralEntropyStatus   { get; private set; }
        public VisualElement StructuralEntropyBarFill  { get; private set; }
        public VisualElement RebarGrid                 { get; private set; }

        // Expansion IV: lethe drip gauge.
        public VisualElement LetheDripPanel     { get; private set; }
        public Label         LetheDripStatus    { get; private set; }
        public VisualElement LetheDripGaugeFill { get; private set; }
        public VisualElement LetheDropletsRow   { get; private set; }

        // Expansion IV: ozone scourge overlay.
        public VisualElement OzoneScourgeOverlay { get; private set; }
        public VisualElement OzoneWarningPanel   { get; private set; }
        public Label         OzoneWarningStatus  { get; private set; }
        public VisualElement OzoneTimerBarFill   { get; private set; }

        // Expansion IV: memory flash vignette.
        public VisualElement MemoryFlashVignette { get; private set; }
        public Label         MemoryFlashText     { get; private set; }

        // Expansion IV: generational psychology cohort readout.
        public VisualElement GenerationalPanel { get; private set; }
        public Label         GenerationalBody  { get; private set; }
        public Label         GenerationalEvent { get; private set; }

        /// <summary>Build the full tree under <paramref name="host"/> (or a new root).</summary>
        public VisualElement Build(VisualElement host = null)
        {
            Root = host ?? new VisualElement { name = RootName };
            if (string.IsNullOrEmpty(Root.name)) Root.name = RootName;
            Root.AddToClassList("diegetic-root");
            Root.pickingMode = PickingMode.Ignore;

            // Vitals reads first and, unlike the others, never hides: it is the
            // one panel with no toggle -- the others hide until their subsystem
            // is relevant. Rows are pre-built once and reused on every paint;
            // PaintVitals only updates their fill width and value text.
            VitalsPanel = MakePanel(VitalsPanelName, "vitals-panel");
            VitalsClock = MakeLabel(VitalsClockName, "diegetic-title");
            VitalsDose = MakeLabel(VitalsDoseName, "diegetic-status");
            VitalsNeeds = new VisualElement { name = VitalsNeedsName };
            VitalsNeeds.AddToClassList("vitals-needs");
            VitalsPanel.Add(VitalsClock);
            VitalsPanel.Add(VitalsDose);
            VitalsPanel.Add(VitalsNeeds);
            VitalsPanel.Add(MakeHint("vitals-hint",
                "[F1] eat  ·  [F2] drink  ·  [SPACE] pause  ·  [F5] save"));
            BuildVitalsRows(VitalsNeeds);
            Root.Add(VitalsPanel);

            EventPanel = MakePanel(EventPanelName, "event-panel");
            EventTitle = MakeLabel(EventTitleName, "diegetic-title");
            EventBody = MakeLabel(EventBodyName, "diegetic-body");
            EventChoices = new VisualElement { name = EventChoicesName };
            EventChoices.AddToClassList("event-choices");
            EventPanel.Add(EventTitle);
            EventPanel.Add(EventBody);
            EventPanel.Add(EventChoices);
            Root.Add(EventPanel);

            HatchPanel = MakePanel(HatchPanelName, "hatch-panel");
            HatchPanel.Add(MakeTitle("hatch-title", "HATCH DEFENSE"));
            HatchStatus = MakeLabel(HatchStatusName, "diegetic-status");
            HatchAmmo = MakeLabel(HatchAmmoName, "diegetic-body");
            HatchArms = MakeLabel(HatchArmsName, "diegetic-body", "emphasis");
            HatchPanel.Add(HatchStatus);
            HatchPanel.Add(HatchAmmo);
            HatchPanel.Add(HatchArms);
            HatchPanel.Add(MakeHint("hatch-hint", "[H] close  ·  [B] workbench upgrades"));
            Root.Add(HatchPanel);

            EncounterPanel = MakePanel(EncounterPanelName, "encounter-panel");
            EncounterPanel.Add(MakeTitle("encounter-title", "FIELD CONTACT"));
            EncounterStatus = MakeLabel(EncounterStatusName, "diegetic-status");
            EncounterList = new VisualElement { name = EncounterListName };
            EncounterList.AddToClassList("encounter-list");
            EncounterPanel.Add(EncounterStatus);
            EncounterPanel.Add(EncounterList);
            EncounterPanel.Add(MakeHint("encounter-hint", "Expedition combat feeds this strip."));
            Root.Add(EncounterPanel);

            StoresPanel = MakePanel(StoresPanelName, "stores-panel");
            StoresPanel.Add(MakeTitle("stores-title", "STORES FOCUS"));
            StoresSummary = MakeLabel(StoresSummaryName, "diegetic-status");
            StoresTooltip = MakeLabel(StoresTooltipName, "diegetic-body");
            StoresPanel.Add(StoresSummary);
            StoresPanel.Add(StoresTooltip);
            StoresPanel.Add(MakeHint("stores-hint", "[I] next  ·  [Shift+I] prev  ·  [E] use"));
            Root.Add(StoresPanel);

            // WorkbenchUI already assembles a formatted, numbered readout
            // (header, notes, [OK]/[--] lines) -- painted verbatim, like
            // HatchAmmo/HatchArms paint provider-supplied text.
            WorkbenchPanel = MakePanel(WorkbenchPanelName, "workbench-panel");
            WorkbenchBody = MakeLabel(WorkbenchBodyName, "diegetic-body", "workbench-readout");
            WorkbenchPanel.Add(WorkbenchBody);
            Root.Add(WorkbenchPanel);

            // Terminal campaign readout. EndgameSummaryUI.Refresh already
            // assembles both strings, so this paints them verbatim like the
            // workbench panel. No hint row: there is nothing left to press.
            EndgamePanel = MakePanel(EndgamePanelName, "endgame-panel");
            EndgamePanel.Add(MakeTitle("endgame-title", "CAMPAIGN OVER"));
            EndgameStatus = MakeLabel(EndgameStatusName, "diegetic-status");
            EndgameBody = MakeLabel(EndgameBodyName, "diegetic-body", "endgame-readout");
            EndgamePanel.Add(EndgameStatus);
            EndgamePanel.Add(EndgameBody);
            Root.Add(EndgamePanel);

            // Three labels rather than one joined block: PowerGridHUD.Refresh
            // already keeps the budget line, source list and load list apart,
            // and joining them would allocate a copy on every repaint.
            PowerGridPanel = MakePanel(PowerGridPanelName, "power-grid-panel");
            PowerGridPanel.Add(MakeTitle("power-grid-title", "POWER BUDGET"));
            PowerGridBudget = MakeLabel(PowerGridBudgetName, "diegetic-status");
            PowerGridSources = MakeLabel(PowerGridSourcesName, "diegetic-body", "power-grid-readout");
            PowerGridLoads = MakeLabel(PowerGridLoadsName, "diegetic-body", "power-grid-readout");
            PowerGridPanel.Add(PowerGridBudget);
            PowerGridPanel.Add(PowerGridSources);
            PowerGridPanel.Add(PowerGridLoads);
            Root.Add(PowerGridPanel);

            ScavengePanel = MakePanel(ScavengePanelName, "scavenge-panel");
            ScavengeBody = MakeLabel(ScavengeBodyName, "diegetic-body", "scavenge-readout");
            ScavengePanel.Add(ScavengeBody);
            Root.Add(ScavengePanel);

            OverflowCratePanel = MakePanel(OverflowCratePanelName, "overflow-crate-panel");
            OverflowCrateBody = MakeLabel(OverflowCrateBodyName, "diegetic-body", "overflow-crate-readout");
            OverflowCratePanel.Add(OverflowCrateBody);
            Root.Add(OverflowCratePanel);

            FieldGearLoadoutPanel = MakePanel(FieldGearLoadoutPanelName, "field-gear-loadout-panel");
            FieldGearLoadoutBody = MakeLabel(FieldGearLoadoutBodyName, "diegetic-body", "field-gear-loadout-readout");
            FieldGearLoadoutPanel.Add(FieldGearLoadoutBody);
            Root.Add(FieldGearLoadoutPanel);

            BunkerRationingPanel = MakePanel(BunkerRationingPanelName, "bunker-rationing-panel");
            BunkerRationingBody = MakeLabel(BunkerRationingBodyName, "diegetic-body", "bunker-rationing-readout");
            BunkerRationingPanel.Add(BunkerRationingBody);
            Root.Add(BunkerRationingPanel);

            WaterPurificationPanel = MakePanel(WaterPurificationPanelName, "water-purification-panel");
            WaterPurificationBody = MakeLabel(WaterPurificationBodyName, "diegetic-body", "water-purification-readout");
            WaterPurificationPanel.Add(WaterPurificationBody);
            Root.Add(WaterPurificationPanel);

            AirHeatManagementPanel = MakePanel(AirHeatManagementPanelName, "air-heat-management-panel");
            AirHeatManagementBody = MakeLabel(AirHeatManagementBodyName, "diegetic-body", "air-heat-management-readout");
            AirHeatManagementPanel.Add(AirHeatManagementBody);
            Root.Add(AirHeatManagementPanel);

            BunkerMaintenancePanel = MakePanel(BunkerMaintenancePanelName, "bunker-maintenance-panel");
            BunkerMaintenanceBody = MakeLabel(BunkerMaintenanceBodyName, "diegetic-body", "bunker-maintenance-readout");
            BunkerMaintenancePanel.Add(BunkerMaintenanceBody);
            Root.Add(BunkerMaintenancePanel);

            SurvivorTaskBoardPanel = MakePanel(SurvivorTaskBoardPanelName, "survivor-task-board-panel");
            SurvivorTaskBoardBody = MakeLabel(SurvivorTaskBoardBodyName, "diegetic-body", "survivor-task-board-readout");
            SurvivorTaskBoardPanel.Add(SurvivorTaskBoardBody);
            Root.Add(SurvivorTaskBoardPanel);

            // Expansion II — faction-pressure readout.
            FactionPressurePanel = MakePanel(FactionPressurePanelName, "faction-pressure-panel");
            FactionPressurePanel.Add(MakeTitle("faction-pressure-title", "FACTION PRESSURE"));
            FactionPressureBody = MakeLabel(FactionPressureBodyName, "diegetic-body", "faction-pressure-readout");
            FactionPressurePanel.Add(FactionPressureBody);
            FactionPressurePanel.Add(MakeHint("faction-pressure-hint", "Compliant. Patrolled. Tithed. Fed. Or not."));
            Root.Add(FactionPressurePanel);

            // ----------------------------------------------------------------
            // Expansion IV — CHRONOS DECAY & LETHE PROTOCOL
            // ----------------------------------------------------------------

            // 1. Structural Entropy Wireframe — bottom-right, inspector role only.
            StructuralEntropyPanel = MakePanel(StructuralEntropyPanelName, "structural-entropy-panel");
            StructuralEntropyPanel.Add(MakeTitle("structural-entropy-title", "STRUCTURAL INTEGRITY"));
            StructuralEntropyStatus = MakeLabel(StructuralEntropyStatusName, "diegetic-status");
            StructuralEntropyPanel.Add(StructuralEntropyStatus);
            RebarGrid = new VisualElement { name = RebarGridName };
            RebarGrid.AddToClassList("rebar-grid");
            StructuralEntropyPanel.Add(RebarGrid);
            var entropyBarTrack = new VisualElement { name = "structural-entropy-bar-track" };
            entropyBarTrack.AddToClassList("entropy-integrity-bar");
            StructuralEntropyBarFill = new VisualElement { name = StructuralEntropyBarName };
            StructuralEntropyBarFill.AddToClassList("entropy-integrity-fill");
            StructuralEntropyBarFill.style.width = Length.Percent(100f);
            entropyBarTrack.Add(StructuralEntropyBarFill);
            StructuralEntropyPanel.Add(entropyBarTrack);
            StructuralEntropyPanel.Add(MakeHint("structural-entropy-hint",
                "Select Concrete Boss or Architect to activate X-ray scan."));
            Root.Add(StructuralEntropyPanel);

            // 2. Lethe Drip Gauge — attached to water purifier terminal corner.
            LetheDripPanel = MakePanel(LetheDripPanelName, "lethe-drip-panel");
            LetheDripPanel.Add(MakeTitle("lethe-drip-title", "AMNESTIC RESERVOIR"));
            LetheDripStatus = new Label { name = LetheDripStatusName };
            LetheDripStatus.AddToClassList("lethe-drip-status");
            LetheDripPanel.Add(LetheDripStatus);
            var lethTrack = new VisualElement { name = "lethe-sight-gauge-track" };
            lethTrack.AddToClassList("lethe-sight-gauge-track");
            LetheDripGaugeFill = new VisualElement { name = LetheDripGaugeFillName };
            LetheDripGaugeFill.AddToClassList("lethe-sight-gauge-fill");
            LetheDripGaugeFill.style.width = Length.Percent(100f);
            lethTrack.Add(LetheDripGaugeFill);
            LetheDripPanel.Add(lethTrack);
            LetheDropletsRow = new VisualElement { name = LetheDropletsName };
            LetheDropletsRow.AddToClassList("lethe-droplets");
            for (int d = 0; d < 3; d++)
            {
                var dot = new VisualElement { name = "lethe-droplet-" + d };
                dot.AddToClassList("lethe-droplet");
                LetheDropletsRow.Add(dot);
            }
            LetheDripPanel.Add(LetheDropletsRow);
            LetheDripPanel.Add(MakeHint("lethe-drip-hint",
                "< 20% reservoir: droplet cadence slows. Breathing changes."));
            Root.Add(LetheDripPanel);

            // 3. Ozone Scourge — full-screen overlay + top-centre warning panel.
            OzoneScourgeOverlay = new VisualElement { name = OzoneScourgeOverlayName };
            OzoneScourgeOverlay.AddToClassList("ozone-scourge-overlay");
            OzoneScourgeOverlay.pickingMode = PickingMode.Ignore;
            OzoneWarningPanel = MakePanel(OzoneWarningPanelName, "ozone-warning-panel");
            OzoneWarningPanel.Add(MakeTitle("ozone-warning-title", "OPTIC NERVE DEGRADATION DETECTED"));
            OzoneWarningStatus = MakeLabel(OzoneWarningStatusName, "diegetic-status");
            OzoneWarningPanel.Add(OzoneWarningStatus);
            var ozoneBarTrack = new VisualElement { name = "ozone-timer-bar-track" };
            ozoneBarTrack.AddToClassList("ozone-timer-bar-track");
            OzoneTimerBarFill = new VisualElement { name = OzoneTimerBarFillName };
            OzoneTimerBarFill.AddToClassList("ozone-timer-bar-fill");
            OzoneTimerBarFill.style.width = Length.Percent(0f);
            ozoneBarTrack.Add(OzoneTimerBarFill);
            OzoneWarningPanel.Add(ozoneBarTrack);
            OzoneWarningPanel.Add(MakeHint("ozone-hint",
                "Equip item_welders_glass to block UV exposure."));
            OzoneScourgeOverlay.Add(OzoneWarningPanel);
            Root.Add(OzoneScourgeOverlay);

            // 4. Memory Flash Vignette — full-screen monochrome burst.
            MemoryFlashVignette = new VisualElement { name = MemoryFlashVignetteName };
            MemoryFlashVignette.AddToClassList("memory-flash-vignette");
            MemoryFlashVignette.pickingMode = PickingMode.Ignore;
            MemoryFlashText = MakeLabel(MemoryFlashTextName, "memory-flash-text");
            MemoryFlashText.text = "REMEMBER";
            MemoryFlashVignette.Add(MemoryFlashText);
            Root.Add(MemoryFlashVignette);

            // 5. Generational Psychology Cohort Readout — left side, below vitals.
            GenerationalPanel = MakePanel(GenerationalPanelName, "generational-psychology-panel");
            GenerationalPanel.Add(MakeTitle("generational-title", "POPULATION COHORTS"));
            GenerationalBody = MakeLabel(GenerationalBodyName, "diegetic-body");
            GenerationalPanel.Add(GenerationalBody);
            GenerationalEvent = new Label { name = GenerationalEventName };
            GenerationalEvent.AddToClassList("gen-event-text");
            GenerationalPanel.Add(GenerationalEvent);
            GenerationalPanel.Add(MakeHint("generational-hint",
                "Bunker-Born know no sunlight. They grieve differently."));
            Root.Add(GenerationalPanel);

            SetVisible(HatchPanel, false);
            SetVisible(StoresPanel, false);
            SetVisible(EventPanel, false);
            SetVisible(WorkbenchPanel, false);
            SetVisible(EndgamePanel, false);
            SetVisible(PowerGridPanel, false);
            SetVisible(ScavengePanel, false);
            SetVisible(OverflowCratePanel, false);
            SetVisible(FieldGearLoadoutPanel, false);
            SetVisible(BunkerRationingPanel, false);
            SetVisible(WaterPurificationPanel, false);
            SetVisible(AirHeatManagementPanel, false);
            SetVisible(BunkerMaintenancePanel, false);
            SetVisible(SurvivorTaskBoardPanel, false);
            SetVisible(FactionPressurePanel, false);
            // Expansion IV — hidden by default.
            SetVisible(StructuralEntropyPanel, false);
            SetVisible(LetheDripPanel, false);
            SetVisible(OzoneScourgeOverlay, false);
            SetVisible(MemoryFlashVignette, false);
            SetVisible(GenerationalPanel, false);
            return Root;
        }

        /// <summary>Wire labels from an existing UXML-instantiated tree.</summary>
        public bool BindExisting(VisualElement root)
        {
            if (root == null) return false;
            Root = root.Q<VisualElement>(RootName) ?? root;
            HatchPanel = Root.Q<VisualElement>(HatchPanelName);
            HatchStatus = Root.Q<Label>(HatchStatusName);
            HatchAmmo = Root.Q<Label>(HatchAmmoName);
            HatchArms = Root.Q<Label>(HatchArmsName);
            EncounterPanel = Root.Q<VisualElement>(EncounterPanelName);
            EncounterStatus = Root.Q<Label>(EncounterStatusName);
            EncounterList = Root.Q<VisualElement>(EncounterListName);
            StoresPanel = Root.Q<VisualElement>(StoresPanelName);
            StoresSummary = Root.Q<Label>(StoresSummaryName);
            StoresTooltip = Root.Q<Label>(StoresTooltipName);
            VitalsPanel = Root.Q<VisualElement>(VitalsPanelName);
            VitalsClock = Root.Q<Label>(VitalsClockName);
            VitalsDose = Root.Q<Label>(VitalsDoseName);
            VitalsNeeds = Root.Q<VisualElement>(VitalsNeedsName);
            EventPanel = Root.Q<VisualElement>(EventPanelName);
            EventTitle = Root.Q<Label>(EventTitleName);
            EventBody = Root.Q<Label>(EventBodyName);
            EventChoices = Root.Q<VisualElement>(EventChoicesName);
            WorkbenchPanel = Root.Q<VisualElement>(WorkbenchPanelName);
            WorkbenchBody = Root.Q<Label>(WorkbenchBodyName);
            EndgamePanel = Root.Q<VisualElement>(EndgamePanelName);
            EndgameStatus = Root.Q<Label>(EndgameStatusName);
            EndgameBody = Root.Q<Label>(EndgameBodyName);
            PowerGridPanel = Root.Q<VisualElement>(PowerGridPanelName);
            PowerGridBudget = Root.Q<Label>(PowerGridBudgetName);
            PowerGridSources = Root.Q<Label>(PowerGridSourcesName);
            PowerGridLoads = Root.Q<Label>(PowerGridLoadsName);
            ScavengePanel = Root.Q<VisualElement>(ScavengePanelName);
            ScavengeBody = Root.Q<Label>(ScavengeBodyName);
            OverflowCratePanel = Root.Q<VisualElement>(OverflowCratePanelName);
            OverflowCrateBody = Root.Q<Label>(OverflowCrateBodyName);
            FieldGearLoadoutPanel = Root.Q<VisualElement>(FieldGearLoadoutPanelName);
            FieldGearLoadoutBody = Root.Q<Label>(FieldGearLoadoutBodyName);
            BunkerRationingPanel = Root.Q<VisualElement>(BunkerRationingPanelName);
            BunkerRationingBody = Root.Q<Label>(BunkerRationingBodyName);
            WaterPurificationPanel = Root.Q<VisualElement>(WaterPurificationPanelName);
            WaterPurificationBody = Root.Q<Label>(WaterPurificationBodyName);
            AirHeatManagementPanel = Root.Q<VisualElement>(AirHeatManagementPanelName);
            AirHeatManagementBody = Root.Q<Label>(AirHeatManagementBodyName);
            BunkerMaintenancePanel = Root.Q<VisualElement>(BunkerMaintenancePanelName);
            BunkerMaintenanceBody = Root.Q<Label>(BunkerMaintenanceBodyName);
            SurvivorTaskBoardPanel = Root.Q<VisualElement>(SurvivorTaskBoardPanelName);
            SurvivorTaskBoardBody = Root.Q<Label>(SurvivorTaskBoardBodyName);
            FactionPressurePanel = Root.Q<VisualElement>(FactionPressurePanelName);
            FactionPressureBody = Root.Q<Label>(FactionPressureBodyName);
            // Expansion IV panels — not contract-blocking if absent in legacy UXML;
            // the Build() path always creates them, so they are available after
            // a fresh tree is built. Bind them opportunistically.
            StructuralEntropyPanel   = Root.Q<VisualElement>(StructuralEntropyPanelName);
            StructuralEntropyStatus  = Root.Q<Label>(StructuralEntropyStatusName);
            StructuralEntropyBarFill = Root.Q<VisualElement>(StructuralEntropyBarName);
            RebarGrid                = Root.Q<VisualElement>(RebarGridName);
            LetheDripPanel     = Root.Q<VisualElement>(LetheDripPanelName);
            LetheDripStatus    = Root.Q<Label>(LetheDripStatusName);
            LetheDripGaugeFill = Root.Q<VisualElement>(LetheDripGaugeFillName);
            LetheDropletsRow   = Root.Q<VisualElement>(LetheDropletsName);
            OzoneScourgeOverlay = Root.Q<VisualElement>(OzoneScourgeOverlayName);
            OzoneWarningPanel   = Root.Q<VisualElement>(OzoneWarningPanelName);
            OzoneWarningStatus  = Root.Q<Label>(OzoneWarningStatusName);
            OzoneTimerBarFill   = Root.Q<VisualElement>(OzoneTimerBarFillName);
            MemoryFlashVignette = Root.Q<VisualElement>(MemoryFlashVignetteName);
            MemoryFlashText     = Root.Q<Label>(MemoryFlashTextName);
            GenerationalPanel = Root.Q<VisualElement>(GenerationalPanelName);
            GenerationalBody  = Root.Q<Label>(GenerationalBodyName);
            GenerationalEvent = Root.Q<Label>(GenerationalEventName);
            // Every pre-Expansion-IV panel is part of the contract: a UXML missing
            // one must fall back to Build() rather than bind a half-tree and render nothing.
            // FactionPressurePanel is Expansion II and is opportunistic — requiring it
            // caused EnsureBuilt to Clear() authored UXML and destroy Phase 11 / expansion widgets.
            if (HatchPanel == null || EncounterPanel == null
                || StoresPanel == null || VitalsPanel == null || EventPanel == null
                || WorkbenchPanel == null || EndgamePanel == null
                || PowerGridPanel == null || ScavengePanel == null || OverflowCratePanel == null
                || FieldGearLoadoutPanel == null || BunkerRationingPanel == null
                || WaterPurificationPanel == null || AirHeatManagementPanel == null
                || BunkerMaintenancePanel == null || SurvivorTaskBoardPanel == null)
            {
                return false;
            }
            // Populate the row cache from the UXML-cloned tree. If any row is
            // missing the panel is malformed and PaintVitals would no-op for
            // it, so fall back to Build() instead.
            if (!BindVitalsRows(VitalsNeeds)) return false;
            return true;
        }

        public void PaintHatch(bool open, string status, string ammoBreakdown, string armsPreview)
        {
            if (HatchPanel == null) return;
            SetVisible(HatchPanel, open);
            if (!open) return;
            if (HatchStatus != null) HatchStatus.text = status ?? string.Empty;
            if (HatchAmmo != null) HatchAmmo.text = ammoBreakdown ?? string.Empty;
            if (HatchArms != null)
            {
                HatchArms.text = armsPreview ?? string.Empty;
                HatchArms.EnableInClassList("emphasis", true);
            }
        }

        public void PaintEncounter(string status, IReadOnlyList<string> lines, int maxLines = 6)
        {
            if (EncounterStatus != null)
                EncounterStatus.text = string.IsNullOrEmpty(status) ? "ENCOUNTER LOG: quiet." : status;

            if (EncounterList == null) return;
            EncounterList.Clear();
            if (lines == null || lines.Count == 0) return;

            int n = Math.Min(maxLines, lines.Count);
            for (int i = 0; i < n; i++)
            {
                var line = new Label(lines[i] ?? string.Empty) { name = "encounter-line-" + i };
                line.AddToClassList("diegetic-line");
                EncounterList.Add(line);
            }
        }

        public void PaintStoresFocus(bool show, string summary, string tooltip, bool militaryExclusive)
        {
            if (StoresPanel == null) return;
            SetVisible(StoresPanel, show);
            StoresPanel.EnableInClassList("exclusive-panel", show && militaryExclusive);
            if (!show) return;
            if (StoresSummary != null) StoresSummary.text = summary ?? string.Empty;
            if (StoresTooltip != null)
            {
                StoresTooltip.text = tooltip ?? string.Empty;
                StoresTooltip.EnableInClassList("exclusive", militaryExclusive);
                StoresTooltip.EnableInClassList("emphasis", !militaryExclusive && !string.IsNullOrEmpty(tooltip));
            }
        }

        /// <summary>
        /// Paint the core-loop readout. Rows are pre-built once in
        /// <see cref="Build"/> / <see cref="BindExisting"/>; this method only
        /// updates their fill width and value text. Zero allocation on the
        /// happy path: a 4-int packed key short-circuits the whole body
        /// when nothing changed (clock, dose, per-row need value/critical),
        /// and the per-row path only writes label text when the formatted
        /// value or critical flag actually changed.
        /// </summary>
        public void PaintVitals(
            int day, float hour, float cumulativeDose, float currentRate,
            IReadOnlyDictionary<string, NeedBarData> needs)
        {
            if (VitalsPanel == null) return;

            int h = Mathf.Clamp(Mathf.FloorToInt(hour), 0, 23);
            int m = Mathf.Clamp(Mathf.FloorToInt((hour - Mathf.Floor(hour)) * 60f), 0, 59);
            int doseCenti = Mathf.RoundToInt(Mathf.Clamp(cumulativeDose, 0f, 1000f) * 100f);
            int rateDeci = Mathf.RoundToInt(Mathf.Clamp(currentRate, 0f, 1000f) * 10f);

            if (_rowFills == null) return;
            // Pack a fingerprint of what would actually change on screen.
            // Skip the whole paint when every component matches the last frame.
            unchecked
            {
                int key = (day * 73856093)
                        ^ (h * 19349663)
                        ^ (m * 83492791)
                        ^ (doseCenti * GoldenRatioHashMul)
                        ^ (rateDeci * 805459861);
                for (int i = 0; i < CoreNeedIds.Length; i++)
                {
                    NeedBarData data = null;
                    needs?.TryGetValue(CoreNeedIds[i], out data);
                    if (data == null) { key ^= 0x5A5A5A5A; continue; }
                    key ^= (Mathf.RoundToInt(data.CurrentValue) * GoldenRatioHashMul);
                    if (data.IsCritical) key ^= 0x3C3C3C3C;
                }
                if (key == _lastVitalsKey) return;
                _lastVitalsKey = key;
            }

            if (VitalsClock != null)
                VitalsClock.text = $"DAY {day}   {h:00}:{m:00}";

            if (VitalsDose != null)
                VitalsDose.text = $"☢ {cumulativeDose:0.00} Sv   ({currentRate:0.0}/hr)";

            bool hasCriticalNeed = false;
            for (int i = 0; i < CoreNeedIds.Length; i++)
            {
                NeedBarData data = null;
                needs?.TryGetValue(CoreNeedIds[i], out data);

                var fill = _rowFills[i];
                if (fill != null)
                {
                    bool haveData = data != null;
                    float pct = haveData && data.MaxValue > 0f
                        ? Mathf.Clamp01(data.CurrentValue / data.MaxValue) * 100f
                        : 0f;
                    fill.style.width = Length.Percent(pct);
                    bool critical = haveData && data.IsCritical;
                    hasCriticalNeed |= critical;
                    if (fill.ClassListContains("critical") != critical)
                        fill.EnableInClassList("critical", critical);
                }

                var value = _rowValues[i];
                if (value != null)
                {
                    string next = data == null
                        ? "--"
                        : Mathf.RoundToInt(data.CurrentValue).ToString() + "%";
                    if (value.text != next) value.text = next;
                }
            }

            SetPanelStatus(VitalsPanel,
                hasCriticalNeed ? PanelStatus.Critical : PanelStatus.Default);
        }

        private List<Label> _eventChoicePool = new List<Label>();

        /// <summary>
        /// Draw the event prompt. The row numbers are the control scheme, not
        /// decoration: PlayerInputHandler maps Alpha1 to visible index 0, so a
        /// row that does not show its number cannot be chosen.
        ///
        /// H-6: Labels are pooled. New Labels are only allocated when the
        /// number of choices exceeds what was shown before; a steady-state
        /// event modal produces zero GC.
        /// </summary>
        public void PaintEventModal(
            bool open, string title, string body, IReadOnlyList<EventChoiceLine> choices)
        {
            if (EventPanel == null) return;
            SetVisible(EventPanel, open);
            SetPanelStatus(EventPanel, open ? PanelStatus.Warning : PanelStatus.Default);
            if (!open) return;

            if (EventTitle != null) EventTitle.text = title ?? string.Empty;
            if (EventBody != null) EventBody.text = body ?? string.Empty;
            if (EventChoices == null) return;

            if (choices == null)
            {
                EventChoices.Clear();
                return;
            }

            int needed = choices.Count;

            // Grow the pool only when the modal shows more choices than ever before.
            while (_eventChoicePool.Count < needed)
            {
                var label = new Label();
                label.AddToClassList("event-choice");
                _eventChoicePool.Add(label);
                EventChoices.Add(label);
            }

            // Reuse pooled labels, adding any that were previously removed.
            for (int i = 0; i < needed; i++)
            {
                var row = _eventChoicePool[i];
                if (row.parent != EventChoices)
                    EventChoices.Add(row);
                row.name = "event-choice-" + i;
                row.text = $"[{i + 1}] {choices[i].Text}";
                row.EnableInClassList("event-choice--disabled", !choices[i].IsEnabled);
            }

            // Hide (remove) excess pooled labels that are not needed this paint.
            for (int i = needed; i < _eventChoicePool.Count; i++)
            {
                var row = _eventChoicePool[i];
                if (row.parent == EventChoices)
                    row.RemoveFromHierarchy();
            }
        }

        /// <summary>
        /// Paint the workbench readout verbatim. WorkbenchUI.PanelSummary already
        /// carries its own header, notes, and numbered [OK]/[--] lines (see
        /// WorkbenchUI.RebuildPanel), so this panel does not re-parse it into rows
        /// -- it is a formatted terminal readout, not a list of interactive rows.
        /// </summary>
        public void PaintWorkbench(bool open, string panelSummary)
        {
            if (WorkbenchPanel == null) return;
            SetVisible(WorkbenchPanel, open);
            if (!open) return;
            if (WorkbenchBody != null) WorkbenchBody.text = panelSummary ?? string.Empty;
        }

        /// <summary>
        /// Paint the terminal campaign readout. EndgameSummaryUI.Refresh already
        /// formats both the one-line StatusLine and the multi-line DetailSummary
        /// (outcome, death-screen label, tallies), so both are painted verbatim
        /// rather than re-derived from the raw counters.
        /// </summary>
        public void PaintEndgame(bool visible, string statusLine, string detailSummary)
        {
            if (EndgamePanel == null) return;
            SetVisible(EndgamePanel, visible);
            if (!visible) return;
            if (EndgameStatus != null) EndgameStatus.text = statusLine ?? string.Empty;
            if (EndgameBody != null) EndgameBody.text = detailSummary ?? string.Empty;
        }

        /// <summary>
        /// Paint the power budget readout. Takes PowerGridHUD's three cached
        /// summary strings verbatim -- never BuildPanelText(), which calls
        /// Refresh() internally and would recompute the whole network model on
        /// every paint.
        /// </summary>
        public void PaintPowerGrid(bool open, string budget, string sources, string loads)
        {
            if (PowerGridPanel == null) return;
            SetVisible(PowerGridPanel, open);
            if (!open) return;
            if (PowerGridBudget != null) PowerGridBudget.text = budget ?? string.Empty;
            if (PowerGridSources != null) PowerGridSources.text = sources ?? string.Empty;
            if (PowerGridLoads != null) PowerGridLoads.text = loads ?? string.Empty;
        }

        /// <summary>
        /// Paint the formatted location dispatch board. Mission state remains in
        /// LocationScavengingSystem; the view only owns its terminal surface.
        /// </summary>
        public void PaintScavengeDispatch(bool open, string panelSummary)
        {
            if (ScavengePanel == null) return;
            SetVisible(ScavengePanel, open);
            if (!open) return;
            if (ScavengeBody != null) ScavengeBody.text = panelSummary ?? string.Empty;
        }

        /// <summary>Paint the bunker receiving crate terminal from its UI-owned string view-model.</summary>
        public void PaintOverflowCrate(bool open, string panelSummary)
        {
            if (OverflowCratePanel == null) return;
            SetVisible(OverflowCratePanel, open);
            if (!open) return;
            if (OverflowCrateBody != null) OverflowCrateBody.text = panelSummary ?? string.Empty;
        }

        /// <summary>Paint the field face/body protection terminal.</summary>
        public void PaintFieldGearLoadout(bool open, string panelSummary)
        {
            if (FieldGearLoadoutPanel == null) return;
            SetVisible(FieldGearLoadoutPanel, open);
            if (!open) return;
            if (FieldGearLoadoutBody != null) FieldGearLoadoutBody.text = panelSummary ?? string.Empty;
        }

        /// <summary>Paint the bunker food and clean-water policy terminal.</summary>
        public void PaintBunkerRationing(bool open, string panelSummary)
        {
            if (BunkerRationingPanel == null) return;
            SetVisible(BunkerRationingPanel, open);
            if (!open) return;
            if (BunkerRationingBody != null) BunkerRationingBody.text = panelSummary ?? string.Empty;
        }

        /// <summary>Paint the clean/dirty/irradiated cistern and purifier terminal.</summary>
        public void PaintWaterPurification(bool open, string panelSummary)
        {
            if (WaterPurificationPanel == null) return;
            SetVisible(WaterPurificationPanel, open);
            if (!open) return;
            if (WaterPurificationBody != null) WaterPurificationBody.text = panelSummary ?? string.Empty;
        }

        /// <summary>Paint the bunker air-filtration and heater management terminal.</summary>
        public void PaintAirHeatManagement(bool open, string panelSummary)
        {
            if (AirHeatManagementPanel == null) return;
            SetVisible(AirHeatManagementPanel, open);
            if (!open) return;
            if (AirHeatManagementBody != null) AirHeatManagementBody.text = panelSummary ?? string.Empty;
        }

        /// <summary>Paint the bunker module and generator repair-order terminal.</summary>
        public void PaintBunkerMaintenance(bool open, string panelSummary)
        {
            if (BunkerMaintenancePanel == null) return;
            SetVisible(BunkerMaintenancePanel, open);
            if (!open) return;
            if (BunkerMaintenanceBody != null) BunkerMaintenanceBody.text = panelSummary ?? string.Empty;
        }

        /// <summary>Paint the survivor allocation and work-order terminal.</summary>
        public void PaintSurvivorTaskBoard(bool open, string panelSummary)
        {
            if (SurvivorTaskBoardPanel == null) return;
            SetVisible(SurvivorTaskBoardPanel, open);
            if (!open) return;
            if (SurvivorTaskBoardBody != null) SurvivorTaskBoardBody.text = panelSummary ?? string.Empty;
        }

        /// <summary>Paint the faction-pressure terminal (Garrison / Militia / Cult / Warlord).</summary>
        public void PaintFactionPressure(bool open, string body)
        {
            if (FactionPressurePanel == null) return;
            SetVisible(FactionPressurePanel, open);
            if (!open) return;
            if (FactionPressureBody != null) FactionPressureBody.text = body ?? string.Empty;
        }

        private static VisualElement MakeNeedRow(string id, NeedBarData data,
            out Label label, out VisualElement fill, out Label value)
        {
            var row = new VisualElement { name = "vitals-need-" + id };
            row.AddToClassList("vitals-row");

            label = new Label(data?.DisplayName ?? id.ToUpperInvariant())
            {
                name = "vitals-need-" + id + "-label"
            };
            label.AddToClassList("vitals-row__label");
            row.Add(label);

            var track = new VisualElement { name = "vitals-need-" + id + "-track" };
            track.AddToClassList("vitals-row__track");
            fill = new VisualElement { name = "vitals-need-" + id + "-fill" };
            fill.AddToClassList("vitals-row__fill");
            fill.style.width = data != null && data.MaxValue > 0f
                ? Length.Percent(Mathf.Clamp01(data.CurrentValue / data.MaxValue) * 100f)
                : Length.Percent(0f);
            fill.EnableInClassList("critical", data != null && data.IsCritical);
            track.Add(fill);
            row.Add(track);

            value = new Label(data == null
                ? "--"
                : Mathf.RoundToInt(data.CurrentValue).ToString() + "%")
            {
                name = "vitals-need-" + id + "-value"
            };
            value.AddToClassList("vitals-row__value");
            row.Add(value);

            return row;
        }

        /// <summary>
        /// Build and cache the four vitals rows in fixed CoreNeedIds order.
        /// Called from <see cref="Build"/>; <see cref="BindExisting"/> uses
        /// <see cref="BindVitalsRows"/> to look the same parts up by name in
        /// a UXML-cloned tree.
        /// </summary>
        private void BuildVitalsRows(VisualElement needsContainer)
        {
            int n = CoreNeedIds.Length;
            _rowLabels = new Label[n];
            _rowFills = new VisualElement[n];
            _rowValues = new Label[n];
            for (int i = 0; i < n; i++)
            {
                string id = CoreNeedIds[i];
                Label label, value;
                VisualElement fill;
                var row = MakeNeedRow(id, null, out label, out fill, out value);
                _rowLabels[i] = label;
                _rowFills[i] = fill;
                _rowValues[i] = value;
                needsContainer.Add(row);
            }
        }

        /// <summary>
        /// Populate the row cache from a UXML-cloned tree (no rebuilds). If
        /// any expected name is missing, return false so the caller falls
        /// back to <see cref="Build"/>.
        /// </summary>
        private bool BindVitalsRows(VisualElement needsContainer)
        {
            int n = CoreNeedIds.Length;
            _rowLabels = new Label[n];
            _rowFills = new VisualElement[n];
            _rowValues = new Label[n];
            for (int i = 0; i < n; i++)
            {
                string id = CoreNeedIds[i];
                var label = needsContainer.Q<Label>("vitals-need-" + id + "-label");
                var fill = needsContainer.Q<VisualElement>("vitals-need-" + id + "-fill");
                var value = needsContainer.Q<Label>("vitals-need-" + id + "-value");
                if (label == null || fill == null || value == null) return false;
                _rowLabels[i] = label;
                _rowFills[i] = fill;
                _rowValues[i] = value;
            }
            return true;
        }

        public static void SetVisible(VisualElement el, bool visible)
        {
            if (el == null) return;
            el.EnableInClassList("hidden", !visible);
            el.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
        }

        /// <summary>
        /// Apply the Figma HUD Panel state without changing the panel's content.
        /// Warning is reserved for a decision awaiting the player's attention;
        /// critical is driven by an actual critical need, never by decorative UI.
        /// </summary>
        private static void SetPanelStatus(VisualElement panel, PanelStatus status)
        {
            if (panel == null) return;
            panel.EnableInClassList("diegetic-panel--warning", status == PanelStatus.Warning);
            panel.EnableInClassList("diegetic-panel--critical", status == PanelStatus.Critical);
        }

        private static VisualElement MakePanel(string name, string extraClass)
        {
            var panel = new VisualElement { name = name };
            panel.AddToClassList("diegetic-panel");
            if (!string.IsNullOrEmpty(extraClass))
                panel.AddToClassList(extraClass);
            return panel;
        }

        private static Label MakeTitle(string name, string text)
        {
            var l = new Label(text) { name = name };
            l.AddToClassList("diegetic-title");
            return l;
        }

        private static Label MakeLabel(string name, params string[] classes)
        {
            var l = new Label(string.Empty) { name = name };
            for (int i = 0; i < classes.Length; i++)
                l.AddToClassList(classes[i]);
            return l;
        }

        private static Label MakeHint(string name, string text)
        {
            var l = new Label(text) { name = name };
            l.AddToClassList("diegetic-hint");
            return l;
        }

        // ----------------------------------------------------------------
        // Expansion IV Paint Methods
        // ----------------------------------------------------------------

        /// <summary>
        /// Paint the structural entropy wireframe panel.
        /// <paramref name="open"/> is true when a Concrete-Boss/Architect survivor is selected.
        /// <paramref name="integrity"/> is [0,1] — overall shelter integrity.
        /// <paramref name="rooms"/> is a flat list of (isSpalling, corrosion01) tuples,
        /// one per visible room cell (max ~20 for the grid).
        /// </summary>
        public void PaintStructuralEntropy(
            bool open,
            float integrity,
            string statusLine,
            System.Collections.Generic.IReadOnlyList<(bool isSpalling, float corrosion)> rooms)
        {
            if (StructuralEntropyPanel == null) return;
            SetVisible(StructuralEntropyPanel, open);
            if (!open) return;

            if (StructuralEntropyStatus != null)
                StructuralEntropyStatus.text = statusLine ?? string.Empty;

            // Rebuild the rebar grid cells each paint (max 20 cells — cheap).
            if (RebarGrid != null)
            {
                RebarGrid.Clear();
                if (rooms != null)
                {
                    int n = UnityEngine.Mathf.Min(rooms.Count, 20);
                    for (int i = 0; i < n; i++)
                    {
                        var cell = new VisualElement { name = "rebar-cell-" + i };
                        cell.AddToClassList("rebar-cell");
                        if (rooms[i].isSpalling)
                            cell.AddToClassList("rebar-cell--spalling");
                        else if (rooms[i].corrosion > 0.35f)
                            cell.AddToClassList("rebar-cell--rusting");
                        RebarGrid.Add(cell);
                    }
                }
            }

            if (StructuralEntropyBarFill != null)
            {
                float pct = UnityEngine.Mathf.Clamp01(integrity) * 100f;
                StructuralEntropyBarFill.style.width = Length.Percent(pct);
                StructuralEntropyBarFill.EnableInClassList("critical", integrity < 0.3f);
            }
        }

        /// <summary>
        /// Paint the Lethe drip gauge on the water purifier terminal.
        /// <paramref name="open"/> mirrors WaterPurificationPanel visibility.
        /// <paramref name="level"/> is [0,1] — amnestic reservoir fill.
        /// </summary>
        public void PaintLetheDripGauge(bool open, float level, string statusLine, bool isRedLine)
        {
            if (LetheDripPanel == null) return;
            SetVisible(LetheDripPanel, open);
            if (!open) return;

            if (LetheDripStatus != null)
            {
                LetheDripStatus.text = statusLine ?? string.Empty;
                LetheDripStatus.EnableInClassList("redline", isRedLine);
            }

            if (LetheDripGaugeFill != null)
            {
                LetheDripGaugeFill.style.width = Length.Percent(UnityEngine.Mathf.Clamp01(level) * 100f);
                LetheDripGaugeFill.EnableInClassList("redline", isRedLine);
            }

            if (LetheDropletsRow != null)
            {
                foreach (var child in LetheDropletsRow.Children())
                    child.EnableInClassList("slow", isRedLine);
            }
        }

        /// <summary>
        /// Paint the ozone scourge full-screen overlay.
        /// <paramref name="scourgeActive"/> true during Weather_FalseSpring/SilentSpring.
        /// <paramref name="stareProgress"/> is [0,1]: how far into unshielded exposure.
        /// <paramref name="warningVisible"/> true when stare exceeds 2 s threshold.
        /// </summary>
        public void PaintOzoneScourge(
            bool scourgeActive,
            float stareProgress,
            bool warningVisible,
            string statusLine)
        {
            if (OzoneScourgeOverlay == null) return;
            SetVisible(OzoneScourgeOverlay, scourgeActive);
            if (!scourgeActive) return;

            OzoneScourgeOverlay.EnableInClassList("active", stareProgress > 0.05f);

            if (OzoneWarningPanel != null)
            {
                SetVisible(OzoneWarningPanel, warningVisible);
                if (warningVisible && OzoneWarningStatus != null)
                    OzoneWarningStatus.text = statusLine ?? string.Empty;
            }

            if (OzoneTimerBarFill != null)
            {
                OzoneTimerBarFill.style.width = Length.Percent(UnityEngine.Mathf.Clamp01(stareProgress) * 100f);
                OzoneTimerBarFill.EnableInClassList("danger", stareProgress > 0.6f);
            }
        }

        /// <summary>
        /// Trigger or clear the memory flash vignette.
        /// <paramref name="flashing"/> true for 0.2 s after TriggerMemoryFlash fires.
        /// </summary>
        public void PaintMemoryFlash(bool flashing)
        {
            if (MemoryFlashVignette == null) return;
            SetVisible(MemoryFlashVignette, flashing);
            MemoryFlashVignette.EnableInClassList("active", flashing);
        }

        /// <summary>
        /// Paint the generational psychology cohort readout.
        /// <paramref name="open"/> is true once any Bunker-Born survivor has come of age.
        /// <paramref name="preWarCount"/> / <paramref name="bunkerBornCount"/> are survivor headcounts.
        /// <paramref name="eventLine"/> is the last ComingOfAge or faction-psychology event.
        /// </summary>
        public void PaintGenerationalReadout(
            bool open,
            int preWarCount,
            int bunkerBornCount,
            string bodyText,
            string eventLine)
        {
            if (GenerationalPanel == null) return;
            SetVisible(GenerationalPanel, open);
            if (!open) return;

            if (GenerationalBody != null) GenerationalBody.text = bodyText ?? string.Empty;
            if (GenerationalEvent != null)
            {
                GenerationalEvent.text = eventLine ?? string.Empty;
                SetVisible(GenerationalEvent, !string.IsNullOrEmpty(eventLine));
            }
        }
    }
}
