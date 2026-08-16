using System;
using System.Linq;
using Godot;
using Ashfall.Core.Shelter;
using Ashfall.Core.UI;
using Ashfall.Core.World;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Shelter panel.
    /// Shows shelter status, radiation shielding, air filtration, structural integrity, and shelter upgrades
    /// using tactile 9-slice framing and structured data cards.
    /// </summary>
    public partial class ShelterPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _statusList = null!;
        private VBoxContainer _radiationData = null!;
        private VBoxContainer _structureList = null!;
        private VBoxContainer _upgradesList = null!;

        private SurvivorsHostSession? _survivorsHost;
        private WorldHostSession? _worldHost;
        private InventoryHostSession? _inventoryHost;

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

            while (_statusList.GetChildCount() > 0)
                _statusList.RemoveChild(_statusList.GetChild(0));
            while (_radiationData.GetChildCount() > 0)
                _radiationData.RemoveChild(_radiationData.GetChild(0));
            while (_structureList.GetChildCount() > 0)
                _structureList.RemoveChild(_structureList.GetChild(0));
            while (_upgradesList.GetChildCount() > 0)
                _upgradesList.RemoveChild(_upgradesList.GetChild(0));

            if (!IsBound)
            {
                _statusList.AddChild(AshfallUiHelpers.MakeMetadata("Shelter readout is waiting for live world and survivor sessions."));
                _radiationData.AddChild(AshfallUiHelpers.MakeMetadata("No shielding state available."));
                _structureList.AddChild(AshfallUiHelpers.MakeMetadata("No structural state available."));
                _upgradesList.AddChild(AshfallUiHelpers.MakeMetadata("No maintenance state available."));
                return;
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

            var panel = AshfallUiHelpers.MakePanel(700, 560);
            center.AddChild(panel);

            var margins = AshfallUiHelpers.MakeMargins(Ashfall.Core.UI.Theme.SpacingMd);
            panel.AddChild(margins);

            var vbox = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingMd);
            margins.AddChild(vbox);

            var header = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
            var title = AshfallUiHelpers.MakeTitle("SHELTER INTEGRITY & SHIELDING", Ashfall.Core.UI.Theme.FontSizeH2);
            title.HorizontalAlignment = HorizontalAlignment.Left;
            title.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            header.AddChild(title);

            var btnClose = AshfallUiHelpers.MakeButton("CLOSE [Esc]", () => OnClose?.Invoke());
            btnClose.CustomMinimumSize = new Vector2(110, 32);
            header.AddChild(btnClose);
            vbox.AddChild(header);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            var scroll = new ScrollContainer
            {
                CustomMinimumSize = new Vector2(660, 440),
                SizeFlagsVertical = SizeFlags.ExpandFill
            };
            vbox.AddChild(scroll);

            var contentBox = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingMd);
            contentBox.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            scroll.AddChild(contentBox);

            contentBox.AddChild(AshfallUiHelpers.MakeSectionHeader("SHELTER OVERVIEW"));
            _statusList = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingXs);
            contentBox.AddChild(_statusList);

            contentBox.AddChild(AshfallUiHelpers.MakeSeparator());

            contentBox.AddChild(AshfallUiHelpers.MakeSectionHeader("RADIATION SHIELDING"));
            _radiationData = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingXs);
            contentBox.AddChild(_radiationData);

            contentBox.AddChild(AshfallUiHelpers.MakeSeparator());

            contentBox.AddChild(AshfallUiHelpers.MakeSectionHeader("STRUCTURAL WALL & SKY ARMOR CELLS"));
            _structureList = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingXs);
            contentBox.AddChild(_structureList);

            contentBox.AddChild(AshfallUiHelpers.MakeSeparator());

            contentBox.AddChild(AshfallUiHelpers.MakeSectionHeader("MAINTENANCE & UPGRADE QUEUE"));
            _upgradesList = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingXs);
            contentBox.AddChild(_upgradesList);

            RefreshView();
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
