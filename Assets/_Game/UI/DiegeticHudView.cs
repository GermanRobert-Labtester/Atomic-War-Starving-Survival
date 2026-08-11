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
            // Every panel is part of the contract: a UXML missing one must fall
            // back to Build() rather than bind a half-tree and render nothing.
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

        /// <summary>
        /// Draw the event prompt. The row numbers are the control scheme, not
        /// decoration: PlayerInputHandler maps Alpha1 to visible index 0, so a
        /// row that does not show its number cannot be chosen.
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

            EventChoices.Clear();
            if (choices == null) return;

            for (int i = 0; i < choices.Count; i++)
            {
                var row = new Label($"[{i + 1}] {choices[i].Text}")
                {
                    name = "event-choice-" + i
                };
                row.AddToClassList("event-choice");
                row.EnableInClassList("event-choice--disabled", !choices[i].IsEnabled);
                EventChoices.Add(row);
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
    }
}
