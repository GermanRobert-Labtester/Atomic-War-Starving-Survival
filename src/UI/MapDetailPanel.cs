// SPDX-License-Identifier: MIT
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
            List<string>? lootCategories = null,
            Ashfall.Core.Narrative.BunkerGraffitiCatalog? graffitiCatalog = null,
            int currentDay = int.MaxValue,
            bool uncharted = false)
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

            // ── 0. Location establishing art (only when the art wave shipped one) ──
            var sectorArt = AshfallUiHelpers.MakeLocationArt(locationId, 140);
            if (sectorArt != null) _infoContainer.AddChild(sectorArt);

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

            // ── Field Markings & Environmental Text (Plan 145) ──
            if (graffitiCatalog != null)
            {
                var postings = graffitiCatalog.GetPostingsForLocation(locationId, currentDay);
                if (postings != null && postings.Count > 0)
                {
                    var graffitiCard = AshfallUiHelpers.MakeCardFrame("FIELD MARKINGS & ENVIRONMENTAL TEXT", "SCRATCHES");
                    var graffitiBox = graffitiCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);
                    for (int i = 0; i < postings.Count; i++)
                    {
                        if (i > 0)
                            graffitiBox.AddChild(AshfallUiHelpers.MakeSeparator());

                        var p = postings[i];
                        string mediumTag = !string.IsNullOrWhiteSpace(p.medium) ? $"[{p.medium.ToUpperInvariant()}]" : "[SCRATCH]";
                        string author = !string.IsNullOrWhiteSpace(p.author_signature) ? $" — {p.author_signature}" : " — Unsigned";
                        var quoteLbl = AshfallUiHelpers.MakeBody($"{mediumTag} \"{p.content}\"{author}");
                        quoteLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                        graffitiBox.AddChild(quoteLbl);

                        if (p.recorded_day > 0)
                        {
                            var metaLbl = AshfallUiHelpers.MakeSmall($"Recorded Day {p.recorded_day} · Origin: {p.location}");
                            metaLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Muted));
                            graffitiBox.AddChild(metaLbl);
                        }
                    }
                    _infoContainer.AddChild(graffitiCard);
                }
            }

            // ── 2. Hazard Profile ──
            var hazardCard = AshfallUiHelpers.MakeCardFrame("ENVIRONMENTAL HAZARDS & THREAT RATING", "DOSIMETRY");
            var hazardBox = hazardCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);

            if (uncharted)
            {
                // Same fog gate as the MapPanel location list (Plan 32): a
                // fog-Unknown sector has no verified dosimetry, so catalog
                // numbers must not be presented as surveyed fact.
                hazardBox.AddChild(AshfallUiHelpers.MakeDataRow("Survey Status", "UNCHARTED — no survey data", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim)));
                hazardBox.AddChild(AshfallUiHelpers.MakeDataRow("Verified Dosimetry", "None on record — dispatch refused until this sector is mapped", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Muted)));
            }
            else
            {
                hazardBox.AddChild(AshfallUiHelpers.MakeDataRow("Threat Tier", $"Level {dangerLevel:F0} / 5", AshfallUiHelpers.ToColor(dangerLevel >= 4 ? Ashfall.Core.UI.Theme.Critical : (dangerLevel >= 2 ? Ashfall.Core.UI.Theme.Warm : Ashfall.Core.UI.Theme.Pale))));
                hazardBox.AddChild(AshfallUiHelpers.MakeDataRow("Ambient Radiation Rate", $"+{baseRadsPerHour:F1} mSv / hr", AshfallUiHelpers.ToColor(baseRadsPerHour > 10 ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Warm)));
            }
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
                // No authored sub-sector survey exists for this location; a
                // truthful empty state replaces the former fabricated rooms.
                layoutBox.AddChild(AshfallUiHelpers.MakeDataRow("Sub-Sector Survey", "No architectural survey on record for this sector.", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Muted)));
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
                // No authored salvage survey exists for this location; a
                // truthful empty state replaces the former fabricated yields.
                salvageBox.AddChild(AshfallUiHelpers.MakeDataRow("Salvage Survey", "No salvage survey on record — dispatch a reconnaissance sortie.", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Muted)));
            }
            _salvageContainer.AddChild(salvageCard);
        }

        public void Bind(
            HoldfastLocationEntry? holdfastLoc,
            LocationDefinitionData? journalLoc = null,
            Ashfall.Core.Narrative.BunkerGraffitiCatalog? graffitiCatalog = null,
            int currentDay = int.MaxValue,
            bool uncharted = false,
            List<string>? subLayouts = null,
            List<string>? salvageSurvey = null)
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
                    holdfastLoc.inspect ?? "",
                    lootCategories: salvageSurvey,
                    graffitiCatalog: graffitiCatalog,
                    currentDay: currentDay,
                    uncharted: uncharted,
                    subLayouts: subLayouts);
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
                    journalLoc.description ?? "",
                    inspectNotes: "",
                    subLayouts: null,
                    lootCategories: salvageSurvey,
                    graffitiCatalog: graffitiCatalog,
                    currentDay: currentDay,
                    uncharted: uncharted);
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

            // Placeholder hatch-approach backdrop (days 1–7, intact surface access).
            // Scene-backed panel: insert behind the existing Backdrop ColorRect and
            // soften that overlay so the art shows through.
            var sceneBackdrop = GetNodeOrNull<ColorRect>("Backdrop");
            if (sceneBackdrop != null)
                sceneBackdrop.Color = AshfallUiHelpers.PanelScrim(); // A11Y §3: the shared InkPanelStrong scrim keeps AA worst-case contrast under bright art
            BackdropArt.Apply(this, BackdropArt.SurfaceHatchApproach, 0f, insertBehind: true);
        }

        public void Open()
        {
            Visible = true;
            QueueRedraw();
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (!Visible) return;
            if (AshfallInputActions.IsCloseOrCancel(@event))
            {
                OnClose?.Invoke();
                GetViewport().SetInputAsHandled();
            }
        }
    }
}
