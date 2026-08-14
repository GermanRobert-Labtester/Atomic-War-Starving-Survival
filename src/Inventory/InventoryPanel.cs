using Godot;
using Ashfall.Core.Inventory;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;

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
            CustomMinimumSize = new Vector2(420, 320);

            var rootVbox = new VBoxContainer();
            rootVbox.AddThemeConstantOverride("separation", 6);
            AddChild(rootVbox);

            var title = new Label
            {
                Text = "INVENTORY — STORAGE & GEAR",
                HorizontalAlignment = HorizontalAlignment.Center
            };
            title.AddThemeFontSizeOverride("font_size", 13);
            rootVbox.AddChild(title);

            _lblSummary = new Label { Text = "Empty." };
            _lblSummary.AddThemeFontSizeOverride("font_size", 12);
            rootVbox.AddChild(_lblSummary);

            var scroll = new ScrollContainer
            {
                HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled,
                CustomMinimumSize = new Vector2(0, 160)
            };
            rootVbox.AddChild(scroll);

            var list = new VBoxContainer();
            list.AddThemeConstantOverride("separation", 2);
            scroll.AddChild(list);

            _lblItems = new Label { Text = string.Empty, AutowrapMode = TextServer.AutowrapMode.WordSmart };
            _lblItems.AddThemeFontSizeOverride("font_size", 11);
            list.AddChild(_lblItems);

            _lblEquip = new Label { Text = string.Empty, AutowrapMode = TextServer.AutowrapMode.WordSmart };
            _lblEquip.AddThemeFontSizeOverride("font_size", 11);
            rootVbox.AddChild(_lblEquip);

            _lblCheck = new Label { Text = string.Empty, AutowrapMode = TextServer.AutowrapMode.WordSmart };
            _lblCheck.AddThemeFontSizeOverride("font_size", 11);
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
                $"Items held: {inv.Slots.Count} stacks · weight {inv.GetCurrentWeight():F1}/{inv.MaxWeight:F0} kg · " +
                $"food fill {inv.FoodFillRatio() * 100f:F0}% · water fill {inv.WaterFillRatio() * 100f:F0}%";

            var sb = new System.Text.StringBuilder();
            for (int i = 0; i < inv.Slots.Count; i++)
            {
                var s = inv.Slots[i];
                if (s == null || s.Item == null) continue;
                sb.Append(s.Item.displayName).Append(" ×").Append(s.Amount);
                if (s.Item.type == ItemType.Device && s.Device != null)
                    sb.Append(" [bat ").Append((s.Device.Battery * 100f).ToString("F0"))
                      .Append("% cal ").Append((s.Device.Calibration * 100f).ToString("F0"))
                      .Append("%").Append(s.Device.Broken ? " BROKEN" : "").Append("]");
                sb.Append('\n');
            }
            _lblItems.Text = sb.Length == 0 ? "Nothing stored. The shelves are bare." : sb.ToString().TrimEnd();

            _lblEquip.Text = _session.EquipLine();

            RunItemCheck();
        }

        /// <summary>
        /// The item-check: verifies critical consumables and gear are present,
        /// exactly as a player would glance at the crate before a trip.
        /// </summary>
        private void RunItemCheck()
        {
            var inv = _session.Inventory;
            var sb = new System.Text.StringBuilder();
            sb.Append("ITEM CHECK\n");

            sb.Append(CheckItem(inv, "canned_food", 3, "food for the trip"));
            sb.Append(CheckItem(inv, "clean_water", 2, "potable water"));
            sb.Append(CheckItem(inv, "iodine_pills", 1, "thyroid protection"));
            sb.Append(CheckItem(inv, "gas_mask", 1, "respiratory protection"));
            sb.Append(CheckItem(inv, "battery", 2, "power for instruments"));

            sb.Append(inv.HasWorkingGeiger()
                ? "  [OK] a working geiger counter is on hand\n"
                : "  [!!] NO WORKING GEIGER — entering fallout blind\n");
            sb.Append($"  [{(inv.GetEquippedProtection() >= 0.3f ? "OK" : "!!")}] equipped rad protection {inv.GetEquippedProtection():F2}");

            _lblCheck.Text = sb.ToString().TrimEnd();
        }

        private static string CheckItem(InventoryContainer inv, string itemId, int need, string label)
        {
            int held = inv.CountById(itemId);
            return held >= need
                ? $"  [OK] {label}: {held} on hand (need {need})\n"
                : $"  [!!] {label}: only {held} (need {need})\n";
        }
    }
}
