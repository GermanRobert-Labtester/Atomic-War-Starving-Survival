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
        public VisualElement EventPanel { get; private set; }
        public Label EventTitle { get; private set; }
        public Label EventBody { get; private set; }
        public VisualElement EventChoices { get; private set; }

        /// <summary>Build the full tree under <paramref name="host"/> (or a new root).</summary>
        public VisualElement Build(VisualElement host = null)
        {
            Root = host ?? new VisualElement { name = RootName };
            if (string.IsNullOrEmpty(Root.name)) Root.name = RootName;
            Root.AddToClassList("diegetic-root");
            Root.pickingMode = PickingMode.Ignore;

            // Vitals reads first and, unlike the others, never hides: it is the
            // only on-screen report of the core loop.
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

            SetVisible(HatchPanel, false);
            SetVisible(StoresPanel, false);
            SetVisible(EventPanel, false);
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
            // Every panel is part of the contract: a UXML missing one must fall
            // back to Build() rather than bind a half-tree and render nothing.
            return HatchPanel != null && EncounterPanel != null
                && StoresPanel != null && VitalsPanel != null && EventPanel != null;
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
        /// Paint the core-loop readout. Rows are emitted for every id in
        /// <see cref="CoreNeedIds"/> whether or not the model carries it, so the
        /// panel keeps a stable height and an absent need reads as "--" rather
        /// than as zero -- zero means starving, absent means unknown.
        /// </summary>
        public void PaintVitals(
            int day, float hour, float cumulativeDose, float currentRate,
            IReadOnlyDictionary<string, NeedBarData> needs)
        {
            if (VitalsPanel == null) return;

            int h = Mathf.Clamp(Mathf.FloorToInt(hour), 0, 23);
            int m = Mathf.Clamp(Mathf.FloorToInt((hour - Mathf.Floor(hour)) * 60f), 0, 59);
            if (VitalsClock != null)
                VitalsClock.text = $"DAY {day}   {h:00}:{m:00}";

            if (VitalsDose != null)
                VitalsDose.text = $"☢ {cumulativeDose:0.00} Sv   ({currentRate:0.0}/hr)";

            if (VitalsNeeds == null) return;
            VitalsNeeds.Clear();

            for (int i = 0; i < CoreNeedIds.Length; i++)
            {
                string id = CoreNeedIds[i];
                NeedBarData data = null;
                needs?.TryGetValue(id, out data);
                VitalsNeeds.Add(MakeNeedRow(id, data));
            }
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

        private static VisualElement MakeNeedRow(string id, NeedBarData data)
        {
            var row = new VisualElement { name = "vitals-need-" + id };
            row.AddToClassList("vitals-row");

            var label = new Label(data?.DisplayName ?? id.ToUpperInvariant())
            {
                name = "vitals-need-" + id + "-label"
            };
            label.AddToClassList("vitals-row__label");
            row.Add(label);

            var track = new VisualElement { name = "vitals-need-" + id + "-track" };
            track.AddToClassList("vitals-row__track");
            var fill = new VisualElement { name = "vitals-need-" + id + "-fill" };
            fill.AddToClassList("vitals-row__fill");
            fill.style.width = data != null && data.MaxValue > 0f
                ? Length.Percent(Mathf.Clamp01(data.CurrentValue / data.MaxValue) * 100f)
                : Length.Percent(0f);
            fill.EnableInClassList("critical", data != null && data.IsCritical);
            track.Add(fill);
            row.Add(track);

            var value = new Label(data == null
                ? "--"
                : Mathf.RoundToInt(data.CurrentValue).ToString() + "%")
            {
                name = "vitals-need-" + id + "-value"
            };
            value.AddToClassList("vitals-row__value");
            row.Add(value);

            return row;
        }

        public static void SetVisible(VisualElement el, bool visible)
        {
            if (el == null) return;
            el.EnableInClassList("hidden", !visible);
            el.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
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
