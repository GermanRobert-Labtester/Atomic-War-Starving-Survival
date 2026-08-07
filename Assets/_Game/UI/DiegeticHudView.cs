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

        /// <summary>Build the full tree under <paramref name="host"/> (or a new root).</summary>
        public VisualElement Build(VisualElement host = null)
        {
            Root = host ?? new VisualElement { name = RootName };
            if (string.IsNullOrEmpty(Root.name)) Root.name = RootName;
            Root.AddToClassList("diegetic-root");
            Root.pickingMode = PickingMode.Ignore;

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
            return HatchPanel != null && EncounterPanel != null && StoresPanel != null;
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
