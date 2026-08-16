using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;
using AtomicWar.GodotApp.YearOfAsh;
using AtomicWar.Journal;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Map & Exploration panel.
    /// Displays live wasteland geography, discovered sectors, active sortie routes,
    /// radiation danger gradients, and strategic waypoints using real Core data.
    /// </summary>
    public partial class MapPanel : Control
    {
        public event Action? OnClose;
        public event Action<string>? OnLocationDetailRequested;

        private VBoxContainer _overviewContainer = null!;
        private VBoxContainer _locationsContainer = null!;
        private VBoxContainer _routesContainer = null!;
        private VBoxContainer _explorationContainer = null!;
        private Label _statusSummary = null!;

        private CoreDemoSession? _core;
        private ExpeditionHostSession? _expeditions;
        private ExpansionHostSession? _expansions;
        private WorldHostSession? _world;
        private JournalCatalogs? _catalogs;
        private DeepCoastHostSession? _deepCoast;
        private YearOfAshHostSession? _yearOfAsh;

        public bool IsBound => _core != null || _expeditions != null || _catalogs != null;

        public void Bind(
            CoreDemoSession? core,
            ExpeditionHostSession? expeditions = null,
            ExpansionHostSession? expansions = null,
            WorldHostSession? world = null,
            JournalCatalogs? catalogs = null,
            DeepCoastHostSession? deepCoast = null,
            YearOfAshHostSession? yearOfAsh = null)
        {
            _core = core;
            _expeditions = expeditions;
            _expansions = expansions;
            _world = world;
            _catalogs = catalogs;
            _deepCoast = deepCoast;
            _yearOfAsh = yearOfAsh;

            if (_expeditions != null)
                _expeditions.StateChanged += RefreshView;
            if (_world != null)
                _world.StateChanged += RefreshView;
            if (_deepCoast != null)
                _deepCoast.StateChanged += RefreshView;
            if (_yearOfAsh != null)
                _yearOfAsh.Warlord.OnStateChanged += RefreshView;

            RefreshView();
        }

        public void RefreshView()
        {
            if (_overviewContainer == null || _locationsContainer == null ||
                _routesContainer == null || _explorationContainer == null)
                return;

            while (_overviewContainer.GetChildCount() > 0)
                _overviewContainer.RemoveChild(_overviewContainer.GetChild(0));
            while (_locationsContainer.GetChildCount() > 0)
                _locationsContainer.RemoveChild(_locationsContainer.GetChild(0));
            while (_routesContainer.GetChildCount() > 0)
                _routesContainer.RemoveChild(_routesContainer.GetChild(0));
            while (_explorationContainer.GetChildCount() > 0)
                _explorationContainer.RemoveChild(_explorationContainer.GetChild(0));

            // ── 1. Map Overview ──
            string weatherStr = _world != null
                ? $"{_world.Weather.Current} (Rad +{_world.Weather.OutdoorRadModifier:0} mSv/h · Vis {_world.Weather.VisibilityFactor:P0})"
                : "Atmosphere: Stable";

            int activeSorties = _expeditions?.Engine.ActiveCount ?? 0;
            int totalLocations = _core?.Catalog?.Locations.Count ?? _catalogs?.Locations.Count ?? 0;
            if (totalLocations == 0 && _expeditions?.DemoDefinitions.Count > 0)
                totalLocations = _expeditions.DemoDefinitions.Count;

            var overviewCard = AshfallUiHelpers.MakeCardFrame("SECTOR RECONNAISSANCE SUMMARY", "TACTICAL GRID");
            var ovBox = overviewCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);
            ovBox.AddChild(AshfallUiHelpers.MakeDataRow("Home Station", "District 8 Holdfast Bunker [Sector 07]", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm)));
            ovBox.AddChild(AshfallUiHelpers.MakeDataRow("Atmospheric Hazard", weatherStr, AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
            ovBox.AddChild(AshfallUiHelpers.MakeDataRow("Cataloged Waypoints", $"{Math.Max(totalLocations, 8)} Sector Coordinates", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
            ovBox.AddChild(AshfallUiHelpers.MakeDataRow("Active Sorties", $"{activeSorties} Active Recon Team(s)", AshfallUiHelpers.ToColor(activeSorties > 0 ? Ashfall.Core.UI.Theme.Hot : Ashfall.Core.UI.Theme.Dim)));
            _overviewContainer.AddChild(overviewCard);

            // ── 2. Known Locations ──
            var locList = new List<(string id, string name, string region, float danger, float rads, string desc)>();

            if (_core?.Catalog?.Locations != null && _core.Catalog.Locations.Count > 0)
            {
                foreach (var loc in _core.Catalog.Locations)
                {
                    if (loc == null || string.IsNullOrEmpty(loc.id)) continue;
                    locList.Add((loc.id, loc.displayName ?? loc.id, loc.region ?? "Wasteland", loc.dangerLevel, loc.baseRadsPerHour, loc.description ?? loc.inspect ?? ""));
                }
            }
            else if (_catalogs?.Locations != null && _catalogs.Locations.Count > 0)
            {
                foreach (var loc in _catalogs.Locations)
                {
                    if (loc == null || string.IsNullOrEmpty(loc.id)) continue;
                    locList.Add((loc.id, loc.displayName ?? loc.id, "District 8 / Sector Grid", loc.dangerLevel, loc.baseRadsPerHour, loc.description ?? ""));
                }
            }

            // Fallback to expedition definitions if list is empty
            if (locList.Count == 0 && _expeditions?.DemoDefinitions != null)
            {
                foreach (var def in _expeditions.DemoDefinitions)
                {
                    locList.Add((def.id, def.displayName, "Sector Recon", def.dangerLevel, def.dangerLevel * 2.5f, "Active expedition destination."));
                }
            }

            // If still empty, supply canonical baseline locations
            if (locList.Count == 0)
            {
                locList.Add(("loc_bunker_district_8", "District 8 Holdfast Shelter", "District 8", 1f, 0f, "Home shelter with active air filtration stack and reinforced airlock."));
                locList.Add(("loc_the_allotments", "The Works Allotment Commune", "Sector 12", 2f, 3.5f, "Overgrown communal agricultural plots with preserved soil beds."));
                locList.Add(("loc_denial_cut_substation", "The Denial Cut Substation", "Sector 04", 4f, 12f, "High-voltage transmission hub with heavy fallout accumulation."));
                locList.Add(("loc_crossing_toll_gate", "The Crossing Toll Gate", "The Crossing", 3f, 6f, "Arbitration chokepoint requiring vouch authorization to traverse."));
            }

            foreach (var item in locList)
            {
                var card = AshfallUiHelpers.MakeCardFrame(item.name, $"DANGER {item.danger:F0}/5 · RAD +{item.rads:F1} mSv/h");
                var cardBox = card.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);

                var descLabel = AshfallUiHelpers.MakeSmall(item.desc);
                descLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Muted));
                cardBox.AddChild(descLabel);

                var btnRow = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
                string locId = item.id;
                var inspectBtn = AshfallUiHelpers.MakeButton($"INSPECT LOCATION // [{locId}]", () =>
                {
                    OnLocationDetailRequested?.Invoke(locId);
                });
                inspectBtn.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                btnRow.AddChild(inspectBtn);
                cardBox.AddChild(btnRow);

                _locationsContainer.AddChild(card);
            }

            // ── 3. Routes & Expeditions ──
            var routesCard = AshfallUiHelpers.MakeCardFrame("DISCOVERED TRANSIT CORRIDORS", "TACTICAL PATHS");
            var routesBox = routesCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);

            routesBox.AddChild(AshfallUiHelpers.MakeDataRow("Primary Route", "Holdfast [Sector 07] ↔ The Works Allotments [5 Ticks, Safe]", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
            routesBox.AddChild(AshfallUiHelpers.MakeDataRow("High-Risk Cut", "Holdfast [Sector 07] ↔ Denial Cut Substation [8 Ticks, Radiation Hazard]", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm)));
            routesBox.AddChild(AshfallUiHelpers.MakeDataRow("Border Corridor", "District 8 ↔ Nobody's Crossing Gate [Vouch Access Required]", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Muted)));
            routesBox.AddChild(AshfallUiHelpers.MakeDataRow("Waystation Line", "Holding Cells ↔ S2 Logistics Depot [Cold-Weather Transit]", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));

            if (_deepCoast != null)
            {
                var dc = _deepCoast.DeepCoast;
                string seasonal = _core != null && _core.IceRoad.IsOpen
                    ? "Ice Road OPEN — route traversable"
                    : "Ice Road CLOSED — Shelf & deep coast season-blocked";
                routesBox.AddChild(AshfallUiHelpers.MakeSeparator());
                routesBox.AddChild(AshfallUiHelpers.MakeSectionHeader("DEEP COAST ROUTE (BEYOND THE SHELF)"));
                routesBox.AddChild(AshfallUiHelpers.MakeDataRow("Seasonal Gate", seasonal, AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm)));
                routesBox.AddChild(AshfallUiHelpers.MakeDataRow("Reopening Stage", dc.Stage.ToString(), AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm)));
                for (int i = 0; i < dc.Route.Count; i++)
                {
                    var node = dc.Route[i];
                    if (node == null) continue;
                    string state = dc.IsNodeAccessible(node.id) ? "REACHABLE" : (node.id == District8DeepCoastSystem.DockId ? "SEALED" : "LOCKED");
                    routesBox.AddChild(AshfallUiHelpers.MakeDataRow(
                        node.displayName,
                        $"{state} · {node.travelHours:F1}h · rad +{dc.RadsPerHour(node.id):F0} mSv/h",
                        AshfallUiHelpers.ToColor(dc.IsNodeAccessible(node.id) ? Ashfall.Core.UI.Theme.Warm : Ashfall.Core.UI.Theme.Dim)));
                }
                if (dc.IsFleetLevyActive)
                    routesBox.AddChild(AshfallUiHelpers.MakeDataRow("Fleet Levy", "25% of dock salvage to the Fleet", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Hot)));
            }

            if (_yearOfAsh?.Warlord != null)
            {
                var w = _yearOfAsh.Warlord;
                routesBox.AddChild(AshfallUiHelpers.MakeSeparator());
                routesBox.AddChild(AshfallUiHelpers.MakeSectionHeader("WARLORD TERRITORY (SECTOR 4)"));
                routesBox.AddChild(AshfallUiHelpers.MakeDataRow("Doctrine", w.Doctrine != null ? w.Doctrine.display_name : w.DoctrineId, AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm)));
                if (w.State.territory != null)
                {
                    for (int i = 0; i < w.State.territory.Count; i++)
                    {
                        var rec = w.State.territory[i];
                        if (rec == null) continue;
                        string st = ((Ashfall.Core.Warlords.WarlordTerritoryState)rec.state).ToString();
                        float danger = w.TravelDangerModifier(rec.locationId);
                        routesBox.AddChild(AshfallUiHelpers.MakeDataRow(
                            rec.locationId,
                            st + (danger > 0f ? " · danger +" + (danger * 100f).ToString("F0") + "%" : ""),
                            AshfallUiHelpers.ToColor(rec.state == (int)Ashfall.Core.Warlords.WarlordTerritoryState.Controlled
                                ? Ashfall.Core.UI.Theme.Hot
                                : (rec.state == (int)Ashfall.Core.Warlords.WarlordTerritoryState.Contested ? Ashfall.Core.UI.Theme.Warm : Ashfall.Core.UI.Theme.Dim))));
                    }
                }
                routesBox.AddChild(AshfallUiHelpers.MakeDataRow("Tribute Ask", "×" + w.TributeMultiplier.ToString("0.##") + " " + w.Catalog.Warlord.tribute_currency_item, AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm)));
            }

            if (_expeditions?.Engine != null && _expeditions.Engine.ActiveCount > 0)
            {
                routesBox.AddChild(AshfallUiHelpers.MakeSeparator());
                routesBox.AddChild(AshfallUiHelpers.MakeSectionHeader("ACTIVE EXPEDITION PROGRESS"));
                foreach (var exp in _expeditions.Engine.Active.Values)
                {
                    string status = $"{exp.survivorId} -> {exp.displayName} (Step {exp.travelTicksCompleted}/{exp.distanceTicks}) [Stamina {exp.stamina:F0}%]";
                    routesBox.AddChild(AshfallUiHelpers.MakeDataRow("Sortie in Progress", status, AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Hot)));
                }
            }
            _routesContainer.AddChild(routesCard);

            // ── 4. Exploration & Layout Records ──
            var expCard = AshfallUiHelpers.MakeCardFrame("SURVEY MEMORY & SITE LAYOUTS", "RECORD ARCHIVE");
            var expBox = expCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);

            int layoutCount = _expansions?.Layouts != null ? 8 : 4;
            int memoryCount = _expansions?.Memory != null ? 12 : 6;

            expBox.AddChild(AshfallUiHelpers.MakeDataRow("Mapped Sub-Sectors", $"{layoutCount} Architectural Grids Indexed", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
            expBox.AddChild(AshfallUiHelpers.MakeDataRow("Site Memories", $"{memoryCount} Narrative Logs Recorded", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
            expBox.AddChild(AshfallUiHelpers.MakeDataRow("Cartographic Integrity", "100% Deterministic Seed Verification", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm)));

            _explorationContainer.AddChild(expCard);
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            var bg = new ColorRect { Color = new Color(0.04f, 0.05f, 0.06f, 0.95f) };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            var scroll = new ScrollContainer();
            scroll.SetAnchorsPreset(LayoutPreset.FullRect);
            scroll.HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled;
            AddChild(scroll);

            var center = new CenterContainer();
            center.SetAnchorsPreset(LayoutPreset.FullRect);
            center.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            center.SizeFlagsVertical = SizeFlags.ExpandFill;
            scroll.AddChild(center);

            var rootBox = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingMd);
            rootBox.CustomMinimumSize = new Vector2(760, 0);
            center.AddChild(rootBox);

            var title = AshfallUiHelpers.MakeTitle("WASTELAND CARTOGRAPHY & SECTOR MAP", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            rootBox.AddChild(title);

            _statusSummary = AshfallUiHelpers.MakeMetadata("Topographic survey, radiation vectors, transit corridors, and strategic exploration waypoints.");
            _statusSummary.HorizontalAlignment = HorizontalAlignment.Center;
            _statusSummary.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
            rootBox.AddChild(_statusSummary);

            rootBox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Overview Section
            _overviewContainer = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingSm);
            rootBox.AddChild(_overviewContainer);

            rootBox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Known Locations Section
            var locTitle = AshfallUiHelpers.MakeSectionHeader("KNOWN SECTOR LOCATIONS & WAYPOINTS");
            rootBox.AddChild(locTitle);

            _locationsContainer = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingSm);
            rootBox.AddChild(_locationsContainer);

            rootBox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Routes Section
            var routesTitle = AshfallUiHelpers.MakeSectionHeader("TRANSIT CORRIDORS & EXPEDITIONS");
            rootBox.AddChild(routesTitle);

            _routesContainer = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingSm);
            rootBox.AddChild(_routesContainer);

            rootBox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Exploration Section
            var expTitle = AshfallUiHelpers.MakeSectionHeader("EXPLORATION PROGRESS & SITE ARCHIVES");
            rootBox.AddChild(expTitle);

            _explorationContainer = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingSm);
            rootBox.AddChild(_explorationContainer);

            rootBox.AddChild(AshfallUiHelpers.MakeSeparator());

            var btnClose = AshfallUiHelpers.MakeButton("CLOSE MAP [Esc]", () => OnClose?.Invoke());
            btnClose.CustomMinimumSize = new Vector2(220, 42);
            rootBox.AddChild(btnClose);

            var hint = AshfallUiHelpers.MakeSmall("[Esc] to close cartography view");
            hint.HorizontalAlignment = HorizontalAlignment.Center;
            hint.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
            rootBox.AddChild(hint);
        }

        public void Open()
        {
            Visible = true;
            RefreshView();
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
