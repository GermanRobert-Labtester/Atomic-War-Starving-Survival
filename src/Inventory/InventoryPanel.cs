using Godot;
using Ashfall.Core.Inventory;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;
using CoreTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.Inventory
{
    /// <summary>
    /// Godot 4.7+ UI Control presenting the inventory (items + equipment) and an
    /// item-check readout: totals by id, weight, capacity fill, working devices,
    /// and equipped protection. Thin presentation only — zero simulation logic.
    /// </summary>
    public partial class InventoryPanel : PanelContainer
    {
        private InventoryHostSession _session;
        private Label _lblSummary;
        private Label _lblItems;
        private Label _lblEquip;
        private Label _lblCheck;

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.TopRight);
            CustomMinimumSize = new Vector2(CoreTheme.PanelMaxWidth, 320);

            // Apply standard panel 9-slice
            var tex = AshfallUiHelpers.TryLoadTexture("res://Assets/UI/Textures/panel_bg_9slice.png");
            if (tex != null)
            {
                var sb = new StyleBoxTexture
                {
                    Texture = tex,
                    TextureMarginLeft = 16,
                    TextureMarginTop = 16,
                    TextureMarginRight = 16,
                    TextureMarginBottom = 16
                };
                AddThemeStyleboxOverride("panel", sb);
            }

            var rootVbox = AshfallUiHelpers.MakeVBox(CoreTheme.SpacingSm);
            AddChild(rootVbox);

            // ── Title ──
            rootVbox.AddChild(AshfallUiHelpers.MakeTitle("INVENTORY", CoreTheme.FontSizeH3));
            rootVbox.AddChild(AshfallUiHelpers.MakeLabel("STORAGE & GEAR"));

            // ── Summary ──
            _lblSummary = AshfallUiHelpers.MakeSmall("Empty.");
            rootVbox.AddChild(_lblSummary);

            rootVbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // ── Items scroll ──
            var scroll = new ScrollContainer
            {
                HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled,
                CustomMinimumSize = new Vector2(0, 160)
            };
            rootVbox.AddChild(scroll);

            var list = AshfallUiHelpers.MakeVBox(CoreTheme.SpacingXs);
            scroll.AddChild(list);

            _lblItems = new Label { Text = string.Empty, AutowrapMode = TextServer.AutowrapMode.WordSmart };
            _lblItems.AddThemeFontSizeOverride("font_size", CoreTheme.FontSizeSmall);
            _lblItems.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Pale));
            list.AddChild(_lblItems);

            rootVbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // ── Equipment ──
            _lblEquip = new Label { Text = string.Empty, AutowrapMode = TextServer.AutowrapMode.WordSmart };
            _lblEquip.AddThemeFontSizeOverride("font_size", CoreTheme.FontSizeSmall);
            _lblEquip.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Pale));
            rootVbox.AddChild(_lblEquip);

            rootVbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // ── Item check ──
            rootVbox.AddChild(AshfallUiHelpers.MakeSectionHeader("ITEM CHECK"));
            _lblCheck = new Label { Text = string.Empty, AutowrapMode = TextServer.AutowrapMode.WordSmart };
            _lblCheck.AddThemeFontSizeOverride("font_size", CoreTheme.FontSizeSmall);
            _lblCheck.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Pale));
            rootVbox.AddChild(_lblCheck);
        }

        public void Bind(InventoryHostSession session)
        {
            _session = session;
        }

        public void RefreshView()
        {
            if (_session == null) return;
            var inv = _session.Inventory;

            _lblSummary.Text =
                $"Items held: {inv.Slots.Count} stacks · weight {inv.GetCurrentWeight():F1}/{inv.MaxWeight:F0} kg";

            _lblItems.Text = _session.InventoryLine();
            _lblEquip.Text = _session.EquipLine();

            var checkSb = new System.Text.StringBuilder();
            checkSb.Append($"Has dosimeter: {inv.FindBestWorkingDevice("dosimeter") != null}\n");
            checkSb.Append($"Has geiger: {inv.HasWorkingGeiger()}\n");
            checkSb.Append($"Clean water: {inv.CountById("clean_water")} units\n");
            checkSb.Append($"Rations: {inv.CountById("canned_food")} units\n");
            checkSb.Append($"Meds: {inv.CountByType(ItemType.Medical)} units\n");
            checkSb.Append($"Equipped protection: {inv.GetEquippedProtection():P0}");
            _lblCheck.Text = checkSb.ToString();
        }
    }
}
