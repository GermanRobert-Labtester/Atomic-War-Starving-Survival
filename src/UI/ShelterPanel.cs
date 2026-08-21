using System;
using System.Linq;
using Godot;
using Ashfall.Core.Shelter;
using Ashfall.Core.UI;
using Ashfall.Core.World;
using AtomicWar.GodotApp.UI;
using AtomicWar.GodotApp.World;
using DesignTheme = Ashfall.Core.UI.Theme;

using Ashfall.Core.IO;
namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Shelter panel.
    /// Shows shelter status, radiation shielding, air filtration, structural integrity, and shelter upgrades
    /// using tactile 9-slice framing and structured data cards. Wraps content
    /// in the ASHFALL Dashboard Shell so a sidebar + status rail carry the
    /// navigation and headline metrics that the Stitch reference requires.
    /// </summary>
    public partial class ShelterPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _statusList = null!;
        private VBoxContainer _radiationData = null!;
        private VBoxContainer _structureList = null!;
        private VBoxContainer _upgradesList = null!;

        // Dashboard shell + reusable chrome. Owned by this panel; bound to
        // real Core state in RefreshView synchronously with the section lists.
        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private AshfallSidebar? _sidebar;

        private SurvivorsHostSession? _survivorsHost;
        private WorldHostSession? _worldHost;
        private InventoryHostSession? _inventoryHost;

        // 2D shelter layout viewport — the visual anchor. Hosts a HoldfastInteriorView
        // whose survivor actors reflect the authoritative roster, so opening the shelter
        // shows rooms + occupants as part of the same simulation.
        private SubViewportContainer? _interiorViewportContainer;
        private SubViewport? _interiorViewport;
        private HoldfastInteriorView? _interiorView;

        public bool IsBound => _survivorsHost != null && _worldHost != null;
        public int RenderedStructureCount => _structureList?.GetChildCount() ?? 0;

        public void Bind(
            SurvivorsHostSession survivors,
            WorldHostSession world,
            InventoryHostSession? inventory = null)
        {
            _survivorsHost = survivors;
            _worldHost = world;
            _inventoryHost = inventory;
            RefreshView();
        }

        public void RefreshView()
        {
            if (_statusList == null || _radiationData == null || _structureList == null || _upgradesList == null) return;

            RefreshStatusRail();

            ClearChildren(_statusList);
            ClearChildren(_radiationData);
            ClearChildren(_structureList);
            ClearChildren(_upgradesList);

            if (!IsBound)
            {
                _statusList.AddChild(AshfallUiHelpers.MakeMetadata("Shelter readout is waiting for live world and survivor sessions."));
                _radiationData.AddChild(AshfallUiHelpers.MakeMetadata("No shielding state available."));
                _structureList.AddChild(AshfallUiHelpers.MakeMetadata("No structural state available."));
                _upgradesList.AddChild(AshfallUiHelpers.MakeMetadata("No maintenance state available."));
                return;
            }

            // Keep the 2D layout anchor in sync with the authoritative roster.
            if (_interiorView != null && _survivorsHost != null)
            {
                _interiorView.Initialize(_survivorsHost);
                _interiorView.UpdateSurvivorPositions();
            }

            var materialSave = _survivorsHost!.Shelter.CaptureState();
            var skySave = _worldHost!.SkyArmor.CaptureState();
            int living = _survivorsHost.RosterState.Count(s => s != null && s.IsAliveState);
            float weakestMaterial = _survivorsHost.Shelter.GetWeakestCeilingAttenuation();
            float skyAverage = AverageSkyBleed(skySave);
            float exteriorRad = _worldHost.Weather.OutdoorRadModifier;

            _statusList.AddChild(AshfallUiHelpers.MakeDataRow("Living Residents", $"{living}/{_survivorsHost.RosterState.Count}", new Color(0.9f, 0.9f, 0.9f)));
            _statusList.AddChild(AshfallUiHelpers.MakeDataRow("Shielded Sectors", $"{materialSave.RoomIds?.Length ?? 0} Rooms", new Color(0.83f, 0.67f, 0.38f)));
            _statusList.AddChild(AshfallUiHelpers.MakeDataRow("Sky Armor Grid", $"{skySave.cells?.Count ?? 0} Cells", new Color(0.43f, 0.64f, 0.66f)));
            _statusList.AddChild(AshfallUiHelpers.MakeDataRow("External Atmosphere", $"{_worldHost.Weather.Current}".ToUpperInvariant(), new Color(0.58f, 0.56f, 0.52f)));

            _radiationData.AddChild(AshfallUiHelpers.MakeDataRow("Exterior Exposure Rate", $"+{exteriorRad:0} mSv/hr", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm)));
            _radiationData.AddChild(AshfallUiHelpers.MakeDataRow("Lead Shielding Attenuation", $"{weakestMaterial:P0} Weakest Ceiling", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
            _radiationData.AddChild(AshfallUiHelpers.MakeDataRow("Interior Penetration Rate", $"{_survivorsHost.Shelter.GetRadiationBleed(exteriorRad):0} mSv/hr", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Lethe)));
            _radiationData.AddChild(AshfallUiHelpers.MakeDataRow("Sky Armor Multiplier", $"{skyAverage:0.000} Avg Bleed", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim)));

            if (materialSave.RoomIds != null && materialSave.Materials != null)
            {
                int count = Math.Min(materialSave.RoomIds.Length, materialSave.Materials.Length);
                for (int i = 0; i < count; i++)
                {
                    var material = (MaterialShieldingSystem.WallMaterial)materialSave.Materials[i];
                    _structureList.AddChild(AshfallUiHelpers.MakeDataRow(
                        $"Room {materialSave.RoomIds[i]} ({material})",
                        $"Attenuation {_survivorsHost.Shelter.GetCeilingAttenuation(materialSave.RoomIds[i]):P0}",
                        AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
                }
            }
            foreach (var cell in skySave.cells ?? new System.Collections.Generic.List<CeilingCellArmor>())
            {
                _structureList.AddChild(AshfallUiHelpers.MakeDataRow(
                    $"Sky Cell #{cell.gridX} [{cell.material}]",
                    $"Durability {cell.currentDurability:0}%",
                    cell.currentDurability < 50f ? AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Critical) : AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Lethe)));
            }
            if (_structureList.GetChildCount() == 0)
                _structureList.AddChild(AshfallUiHelpers.MakeMetadata("No material rooms or sky armor cells configured."));

            int damagedCells = 0;
            foreach (var cell in skySave.cells ?? new System.Collections.Generic.List<CeilingCellArmor>())
                if (cell.currentDurability < 100f) damagedCells++;

            _upgradesList.AddChild(AshfallUiHelpers.MakeDataRow("Armor Cells Needing Repair", $"{damagedCells}", damagedCells > 0 ? AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Entropy) : AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
            _upgradesList.AddChild(AshfallUiHelpers.MakeDataRow("Mechanical Scrap on Hand", $"{Count("scrap_mechanical")} units", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Lethe)));
            _upgradesList.AddChild(AshfallUiHelpers.MakeDataRow("Electronic Scrap on Hand", $"{Count("scrap_electronic")} units", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Lethe)));
        }

        private static void ClearChildren(Node parent)
        {
            while (parent.GetChildCount() > 0)
            {
                var child = parent.GetChild(0);
                parent.RemoveChild(child);
                // These rows are detached before disposal. QueueFree() only
                // flushes reliably for nodes still inside the SceneTree, so
                // free the removed row synchronously to avoid orphaned UI rows
                // when the panel is rebound or the headless smoke test exits.
                child.Free();
            }
        }

        private int Count(string itemId)
        {
            return _inventoryHost?.Inventory.CountById(itemId) ?? 0;
        }

        private float AverageSkyBleed(SkyArmorSaveState state)
        {
            if (state?.cells == null || state.cells.Count == 0) return 1f;
            float total = 0f;
            foreach (var cell in state.cells)
                total += _worldHost!.SkyArmor.GetAttenuationFactor(cell.gridX);
            return total / state.cells.Count;
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            var bg = new ColorRect { Color = new Color(0.04f, 0.05f, 0.06f, 0.88f) };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            var center = new CenterContainer();
            center.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(center);

            // Dashboard shell — sidebar provides nav between sub-sections;
            // status rail holds the headline metrics that the Stitch reference
            // puts in its SHELTER ENGINEERING header row.
            _shell = new AshfallDashboardShell(
                "SHELTER INTEGRITY & SHIELDING", 880, 600);
            center.AddChild(_shell);
            _sidebar = _shell.SetSidebar(new[]
            {
                new AshfallSidebar.Item { Id = "overview",  Label = "Overview",        Hint = "SHELTER STATE" },
                new AshfallSidebar.Item { Id = "shielding", Label = "Shielding",        Hint = "EXPOSURE + BLEED" },
                new AshfallSidebar.Item { Id = "structure", Label = "Structure",        Hint = "WALLS + SKY" },
                new AshfallSidebar.Item { Id = "upgrades",  Label = "Maintenance",      Hint = "REPAIR QUEUE" },
            }, "SHELTER OPS", "overview");

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("occupants", "OCCUPANTS",     "—", AshfallMetricCard.Criticality.Normal, 130);
            _statusRail.AddCard("extRad",    "EXT RAD",       "+0 mSv/h", AshfallMetricCard.Criticality.Normal, 140);
            _statusRail.AddCard("bleed",     "INTERIOR BLEED","0 mSv/h", AshfallMetricCard.Criticality.Normal, 140);
            _statusRail.AddCard("shieldMin", "WEAKEST SHIELD","—", AshfallMetricCard.Criticality.Normal, 140);
            _statusRail.AddCard("skyAvg",    "SKY BLEED",     "—", AshfallMetricCard.Criticality.Normal, 140);
            _statusRail.AddCard("damaged",   "DAMAGED CELLS", "0", AshfallMetricCard.Criticality.Normal, 140);

            _shell.AttachHeaderCloseButton("CLOSE [Esc]", () => OnClose?.Invoke());

            // Content slot — scroll container with four named sub-sections.
            var scrollRoot = new ScrollContainer();
            scrollRoot.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            scrollRoot.SizeFlagsVertical = SizeFlags.ExpandFill;
            var scrollMargin = new MarginContainer();
            scrollMargin.AddThemeConstantOverride("margin_left", DesignTheme.SpacingMd);
            scrollMargin.AddThemeConstantOverride("margin_top", DesignTheme.SpacingMd);
            scrollMargin.AddThemeConstantOverride("margin_right", DesignTheme.SpacingMd);
            scrollMargin.AddThemeConstantOverride("margin_bottom", DesignTheme.SpacingMd);
            scrollRoot.AddChild(scrollMargin);
            _shell.SetContent(scrollRoot);

            var contentBox = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingMd);
            contentBox.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            scrollMargin.AddChild(contentBox);

            // ── 2D shelter layout anchor ──
            contentBox.AddChild(AshfallUiHelpers.MakeSectionHeader("SHELTER LAYOUT"));

            // Use a proper container with explicit sizing and anchoring
            var viewportWrapper = new MarginContainer();
            viewportWrapper.AddThemeConstantOverride("margin_left", DesignTheme.SpacingSm);
            viewportWrapper.AddThemeConstantOverride("margin_top", DesignTheme.SpacingSm);
            viewportWrapper.AddThemeConstantOverride("margin_right", DesignTheme.SpacingSm);
            viewportWrapper.AddThemeConstantOverride("margin_bottom", DesignTheme.SpacingSm);
            viewportWrapper.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            viewportWrapper.SizeFlagsVertical = SizeFlags.ShrinkEnd;
            contentBox.AddChild(viewportWrapper);

            _interiorViewportContainer = new SubViewportContainer
            {
                CustomMinimumSize = new Vector2(760, 420),
                Stretch = true,
                SizeFlagsHorizontal = SizeFlags.ExpandFill,
                SizeFlagsVertical = SizeFlags.ShrinkEnd
            };
            viewportWrapper.AddChild(_interiorViewportContainer);

            _interiorViewport = new SubViewport
            {
                Size = new Vector2I(760, 420),
                RenderTargetUpdateMode = SubViewport.UpdateMode.Always,
                Disable3D = true
            };
            _interiorViewportContainer.AddChild(_interiorViewport);

            _interiorView = new HoldfastInteriorView();
            _interiorViewport.AddChild(_interiorView);

            contentBox.AddChild(AshfallUiHelpers.MakeSeparator());

            contentBox.AddChild(AshfallUiHelpers.MakeSectionHeader("SHELTER OVERVIEW"));
            _statusList = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingXs);
            contentBox.AddChild(_statusList);

            contentBox.AddChild(AshfallUiHelpers.MakeSeparator());

            contentBox.AddChild(AshfallUiHelpers.MakeSectionHeader("RADIATION SHIELDING"));
            _radiationData = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingXs);
            contentBox.AddChild(_radiationData);

            contentBox.AddChild(AshfallUiHelpers.MakeSeparator());

            contentBox.AddChild(AshfallUiHelpers.MakeSectionHeader("STRUCTURAL WALL & SKY ARMOR CELLS"));
            _structureList = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingXs);
            contentBox.AddChild(_structureList);

            contentBox.AddChild(AshfallUiHelpers.MakeSeparator());

            contentBox.AddChild(AshfallUiHelpers.MakeSectionHeader("MAINTENANCE & UPGRADE QUEUE"));
            _upgradesList = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingXs);
            contentBox.AddChild(_upgradesList);

            if (_sidebar != null)
            {
                _sidebar.OnSelected += id =>
                {
                    if (id == "overview" && _statusList != null)
                        ScrollToChild(scrollRoot, _statusList);
                    else if (id == "shielding" && _radiationData != null)
                        ScrollToChild(scrollRoot, _radiationData);
                    else if (id == "structure" && _structureList != null)
                        ScrollToChild(scrollRoot, _structureList);
                    else if (id == "upgrades" && _upgradesList != null)
                        ScrollToChild(scrollRoot, _upgradesList);
                };
            }

            RefreshView();
        }

        private static void ScrollToChild(ScrollContainer scroll, Control child)
        {
            if (scroll == null || child == null) return;
            try
            {
                float targetOffset = 0f;
                Node walker = child;
                while (walker != null && walker != scroll)
                {
                    if (walker is Control w && walker != scroll)
                        targetOffset += w.Position.Y;
                    walker = walker.GetParent();
                }
                if (targetOffset > 0)
                    scroll.ScrollVertical = (int)Math.Max(0, targetOffset - 8);
            }
            catch (Exception ex_CATDIAG)
            {
                CatalogDiagnostics.Warn("<unknown>", "unknown", ex_CATDIAG);
                // best-effort
            }
        }

        /// <summary>
        /// Populate the top status rail from Core shelter / world state. The
        /// values map onto the headline metrics the Stitch SHELTER ENGINEERING
        /// reference puts in its header row.
        /// </summary>
        private void RefreshStatusRail()
        {
            if (_statusRail == null) return;

            if (!IsBound)
            {
                _statusRail.Set("occupants", "—",            AshfallMetricCard.Criticality.Normal);
                _statusRail.Set("extRad",    "+0 mSv/h",      AshfallMetricCard.Criticality.Normal);
                _statusRail.Set("bleed",     "0 mSv/h",       AshfallMetricCard.Criticality.Normal);
                _statusRail.Set("shieldMin", "—",             AshfallMetricCard.Criticality.Normal);
                _statusRail.Set("skyAvg",    "—",             AshfallMetricCard.Criticality.Normal);
                _statusRail.Set("damaged",   "0",             AshfallMetricCard.Criticality.Normal);
                return;
            }

            var materialSave = _survivorsHost!.Shelter.CaptureState();
            var skySave = _worldHost!.SkyArmor.CaptureState();
            int living = _survivorsHost.RosterState.Count(s => s != null && s.IsAliveState);
            int cohort = _survivorsHost.RosterState.Count;
            float weakestMaterial = _survivorsHost.Shelter.GetWeakestCeilingAttenuation();
            float skyAverage = AverageSkyBleed(skySave);
            float exteriorRad = _worldHost.Weather.OutdoorRadModifier;
            float bleed = _survivorsHost.Shelter.GetRadiationBleed(exteriorRad);
            int damaged = 0;
            foreach (var c in skySave.cells ?? new System.Collections.Generic.List<CeilingCellArmor>())
                if (c.currentDurability < 100f) damaged++;

            AshfallMetricCard.Criticality extCrit =
                exteriorRad < 25 ? AshfallMetricCard.Criticality.Normal
                : exteriorRad < 50 ? AshfallMetricCard.Criticality.Caution
                : exteriorRad < 100 ? AshfallMetricCard.Criticality.Warn
                : AshfallMetricCard.Criticality.Critical;

            AshfallMetricCard.Criticality bleedCrit =
                bleed < 5 ? AshfallMetricCard.Criticality.Normal
                : bleed < 15 ? AshfallMetricCard.Criticality.Caution
                : bleed < 30 ? AshfallMetricCard.Criticality.Warn
                : AshfallMetricCard.Criticality.Critical;

            AshfallMetricCard.Criticality shieldCrit =
                weakestMaterial >= 0.9f ? AshfallMetricCard.Criticality.Normal
                : weakestMaterial >= 0.7f ? AshfallMetricCard.Criticality.Caution
                : weakestMaterial >= 0.4f ? AshfallMetricCard.Criticality.Warn
                : AshfallMetricCard.Criticality.Critical;

            AshfallMetricCard.Criticality damagedCrit =
                damaged == 0 ? AshfallMetricCard.Criticality.Normal
                : damaged <= 2 ? AshfallMetricCard.Criticality.Caution
                : damaged <= 5 ? AshfallMetricCard.Criticality.Warn
                : AshfallMetricCard.Criticality.Critical;

            _statusRail.Set("occupants", $"{living}/{cohort}", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("extRad",    $"+{exteriorRad:0} mSv/h",   extCrit);
            _statusRail.Set("bleed",     $"{bleed:0} mSv/h",            bleedCrit);
            _statusRail.Set("shieldMin", $"{weakestMaterial:P0}",       shieldCrit);
            _statusRail.Set("skyAvg",    $"{skyAverage:0.000}",          AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("damaged",   $"{damaged}",                   damagedCrit);
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
