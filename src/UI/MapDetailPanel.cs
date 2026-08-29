using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.UI;
using AtomicWar.Journal;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Map Detail panel.
    /// Shows detailed sector intelligence, radiation readings, transit requirements,
    /// architectural sub-layouts, and site salvage potential for a chosen location.
    /// </summary>
    public partial class MapDetailPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _infoContainer = null!;
        private VBoxContainer _hazardsContainer = null!;
        private VBoxContainer _layoutsContainer = null!;
        private VBoxContainer _salvageContainer = null!;
        private Label _titleLabel = null!;

        public void Bind(
            string locationId,
            string displayName,
            string region,
            float dangerLevel,
            float baseRadsPerHour,
            float travelHours,
            string description,
            string inspectNotes = "",
            List<string>? subLayouts = null,
            List<string>? lootCategories = null)
        {
            if (_titleLabel != null)
                _titleLabel.Text = $"SECTOR INTELLIGENCE // {displayName.ToUpperInvariant()}";

            if (_infoContainer == null || _hazardsContainer == null ||
                _layoutsContainer == null || _salvageContainer == null)
                return;

            AshfallUiHelpers.EmptyChildren(_infoContainer);
            AshfallUiHelpers.EmptyChildren(_hazardsContainer);
            AshfallUiHelpers.EmptyChildren(_layoutsContainer);
            AshfallUiHelpers.EmptyChildren(_salvageContainer);

            // ── 1. Sector Geography & Description ──
            var infoCard = AshfallUiHelpers.MakeCardFrame("SECTOR OVERVIEW", locationId);
            var infoBox = infoCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);

            infoBox.AddChild(AshfallUiHelpers.MakeDataRow("Region / Zone", string.IsNullOrEmpty(region) ? "District 8 Periphery" : region, AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm)));
            infoBox.AddChild(AshfallUiHelpers.MakeDataRow("Travel Time (Foot Sortie)", $"{Math.Max(1f, travelHours):F1} Hours", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
            infoBox.AddChild(AshfallUiHelpers.MakeSeparator());

            string fullDesc = string.IsNullOrEmpty(description) ? "Uncataloged wasteland sector. Scavengers advise caution due to structural instability and background ionizing radiation." : description;
            var bodyLbl = AshfallUiHelpers.MakeBody(fullDesc);
            infoBox.AddChild(bodyLbl);

            if (!string.IsNullOrEmpty(inspectNotes))
            {
                infoBox.AddChild(AshfallUiHelpers.MakeSeparator());
                var noteHeader = AshfallUiHelpers.MakeSubsectionHeader("TACTICAL FIELD NOTES");
                infoBox.AddChild(noteHeader);
                var noteLbl = AshfallUiHelpers.MakeSmall(inspectNotes);
                noteLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
                infoBox.AddChild(noteLbl);
            }
            _infoContainer.AddChild(infoCard);

            // ── 2. Hazard Profile ──
            var hazardCard = AshfallUiHelpers.MakeCardFrame("ENVIRONMENTAL HAZARDS & THREAT RATING", "DOSIMETRY");
            var hazardBox = hazardCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);

            hazardBox.AddChild(AshfallUiHelpers.MakeDataRow("Threat Tier", $"Level {dangerLevel:F0} / 5", AshfallUiHelpers.ToColor(dangerLevel >= 4 ? Ashfall.Core.UI.Theme.Critical : (dangerLevel >= 2 ? Ashfall.Core.UI.Theme.Warm : Ashfall.Core.UI.Theme.Pale))));
            hazardBox.AddChild(AshfallUiHelpers.MakeDataRow("Ambient Radiation Rate", $"+{baseRadsPerHour:F1} mSv / hr", AshfallUiHelpers.ToColor(baseRadsPerHour > 10 ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Warm)));
            hazardBox.AddChild(AshfallUiHelpers.MakeDataRow("Required Protective Gear", baseRadsPerHour > 8 ? "Lead Shielding / Hazmat Suit + Gas Mask" : "Standard Dosimeter + Particulate Filter", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
            hazardBox.AddChild(AshfallUiHelpers.MakeDataRow("Transit Stance Advice", dangerLevel >= 3 ? "Stealth Stance Recommended" : "Standard March", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Muted)));
            _hazardsContainer.AddChild(hazardCard);

            // ── 3. Sub-Layouts & Architectural Grids ──
            var layoutCard = AshfallUiHelpers.MakeCardFrame("MAPPED SUB-SECTORS & ACCESS POINTS", "BLUEPRINT");
            var layoutBox = layoutCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);

            if (subLayouts != null && subLayouts.Count > 0)
            {
                foreach (var layout in subLayouts)
                {
                    layoutBox.AddChild(AshfallUiHelpers.MakeDataRow("Accessible Chamber", layout, AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
                }
            }
            else
            {
                layoutBox.AddChild(AshfallUiHelpers.MakeDataRow("Primary Concourse", "Surface Access Tunnel // Reinforced Hatch", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
                layoutBox.AddChild(AshfallUiHelpers.MakeDataRow("Sub-Level 1", "Utility Corridors & Piping Vaults", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
                layoutBox.AddChild(AshfallUiHelpers.MakeDataRow("Perimeter Node", "Collapsed Overpass & Debris Fields", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Muted)));
            }
            _layoutsContainer.AddChild(layoutCard);

            // ── 4. Salvage Potential & Scavenging Yields ──
            var salvageCard = AshfallUiHelpers.MakeCardFrame("SALVAGE RECOVERY POTENTIAL", "RESOURCES");
            var salvageBox = salvageCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);

            if (lootCategories != null && lootCategories.Count > 0)
            {
                foreach (var cat in lootCategories)
                {
                    salvageBox.AddChild(AshfallUiHelpers.MakeDataRow("Potential Yield", cat.Replace('_', ' ').ToUpperInvariant(), AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm)));
                }
            }
            else
            {
                salvageBox.AddChild(AshfallUiHelpers.MakeDataRow("Primary Scavenge", "Structural Scrap Metal & Mechanical Parts", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm)));
                salvageBox.AddChild(AshfallUiHelpers.MakeDataRow("Secondary Scavenge", "Electrical Wiring & Electronic Components", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
                salvageBox.AddChild(AshfallUiHelpers.MakeDataRow("Rare Recovery", "Sealed Medical Supplies & Anti-Rad Compounds", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Hot)));
            }
            _salvageContainer.AddChild(salvageCard);
        }

        public void Bind(HoldfastLocationEntry? holdfastLoc, LocationDefinitionData? journalLoc = null)
        {
            if (holdfastLoc != null)
            {
                Bind(
                    holdfastLoc.id,
                    HoldfastCatalogLoader.StripAuthorNotes(holdfastLoc.displayName ?? holdfastLoc.id),
                    holdfastLoc.region ?? "District 8 / Sector 4",
                    holdfastLoc.dangerLevel,
                    holdfastLoc.baseRadsPerHour,
                    holdfastLoc.travelHours,
                    holdfastLoc.description ?? "",
                    holdfastLoc.inspect ?? "");
            }
            else if (journalLoc != null)
            {
                Bind(
                    journalLoc.id ?? "loc_unknown",
                    journalLoc.displayName ?? "Unknown Sector",
                    "Wasteland Sector",
                    journalLoc.dangerLevel,
                    journalLoc.baseRadsPerHour,
                    4.0f,
                    journalLoc.description ?? "");
            }
        }

        public override void _Ready()
        {
            // Ticket #125: layout chrome owned by
            // res://assets/ui/panels/MapDetailPanel.tscn. SceneBinder resolves
            // typed unique-name nodes; sibling bind logic is unchanged.
            var binder = new SceneBinder(this, typeof(MapDetailPanel));
            binder.Require<VBoxContainer>("InfoContainer");
            binder.Require<VBoxContainer>("HazardsContainer");
            binder.Require<VBoxContainer>("LayoutsContainer");
            binder.Require<VBoxContainer>("SalvageContainer");
            binder.Require<Label>("Title");
            binder.Require<Button>("CloseButton");

            _infoContainer = binder.Get<VBoxContainer>("InfoContainer");
            _hazardsContainer = binder.Get<VBoxContainer>("HazardsContainer");
            _layoutsContainer = binder.Get<VBoxContainer>("LayoutsContainer");
            _salvageContainer = binder.Get<VBoxContainer>("SalvageContainer");
            _titleLabel = binder.Get<Label>("Title");
            binder.Get<Button>("CloseButton").Pressed += () => OnClose?.Invoke();

            Visible = false;
        }

        public void Open()
        {
            Visible = true;
            QueueRedraw();
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (!Visible) return;
            if (@event is InputEventKey key && key.Pressed && key.Keycode == Key.Escape)
            {
                OnClose?.Invoke();
                GetViewport().SetInputAsHandled();
            }
        }
    }
}
