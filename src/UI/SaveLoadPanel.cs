using System;
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Save/Load panel.
    /// Shows save slots, game state information, and save/load operations.
    /// </summary>
    public partial class SaveLoadPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblSlotsTitle;
        private VBoxContainer _slotsList;
        private Label _lblInfoTitle;
        private VBoxContainer _infoList;
        private VBoxContainer _actionButtons;

        // Placeholder save data
        private readonly string[] _placeholderSlots = {
            "Slot 1 — Day 25 — Bunker Status: Active — 5 Survivors",
            "Slot 2 — Day 18 — Bunker Status: Active — 4 Survivors",
            "Slot 3 — Day 12 — Bunker Status: Active — 3 Survivors",
            "Slot 4 — Day 8 — Bunker Status: Active — 2 Survivors",
            "Slot 5 — Day 3 — Bunker Status: Active — 1 Survivor"
        };

        private readonly string[] _placeholderInfo = {
            "Current Save: Slot 1 (Day 25)",
            "Total Playtime: 47 hours",
            "Last Modified: Day 25, 14:32",
            "Game Version: v0.1",
            "Platform: Godot 4.7+ .NET Edition",
            "Save Size: 2.4 MB"
        };

        // Real data from host session
        // private SaveLoadHostSession? _saveLoadHost;

        public void Bind(object saveLoad) // placeholder for SaveLoadHostSession
        {
            // _saveLoadHost = (SaveLoadHostSession)saveLoad;
            // RefreshView();
        }

        public void RefreshView()
        {
            if (_slotsList == null || _infoList == null || _actionButtons == null) return;

            // Clear existing lists
            AshfallUiHelpers.EmptyChildren(_slotsList);
            AshfallUiHelpers.EmptyChildren(_infoList);
            AshfallUiHelpers.EmptyChildren(_actionButtons);

            // Display placeholder save slots
            foreach (string slot in _placeholderSlots)
            {
                var label = new Label { Text = slot };
                label.CustomMinimumSize = new Vector2(400, 40);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _slotsList.AddChild(label);
            }

            // Display placeholder save info
            foreach (string info in _placeholderInfo)
            {
                var label = new Label { Text = info };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _infoList.AddChild(label);
            }

            // Display action buttons
            var btnSave = AshfallUiHelpers.MakeButton("SAVE CURRENT", () => GD.Print("Save clicked"));
            btnSave.CustomMinimumSize = new Vector2(200, 40);
            _actionButtons.AddChild(btnSave);

            var btnLoad = AshfallUiHelpers.MakeButton("LOAD SLOT", () => GD.Print("Load clicked"));
            btnLoad.CustomMinimumSize = new Vector2(200, 40);
            _actionButtons.AddChild(btnLoad);

            var btnDelete = AshfallUiHelpers.MakeButton("DELETE SLOT", () => GD.Print("Delete clicked"));
            btnDelete.CustomMinimumSize = new Vector2(200, 40);
            _actionButtons.AddChild(btnDelete);
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            var bg = new ColorRect { Color = new Color(0.05f, 0.05f, 0.05f, 0.92f) };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            var container = new CenterContainer();
            container.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(container);

            var vbox = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingLg);
            vbox.CustomMinimumSize = new Vector2(550, 0);
            container.AddChild(vbox);

            var title = AshfallUiHelpers.MakeTitle("SAVE & LOAD", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Save slots section
            _lblSlotsTitle = AshfallUiHelpers.MakeSectionHeader("SAVE SLOTS");
            vbox.AddChild(_lblSlotsTitle);

            _slotsList = new VBoxContainer();
            _slotsList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _slotsList.CustomMinimumSize = new Vector2(450, 0);
            vbox.AddChild(_slotsList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Save info section
            _lblInfoTitle = AshfallUiHelpers.MakeSectionHeader("SAVE INFORMATION");
            vbox.AddChild(_lblInfoTitle);

            _infoList = new VBoxContainer();
            _infoList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _infoList.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_infoList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Action buttons section
            _actionButtons = new VBoxContainer();
            _actionButtons.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingMd);
            _actionButtons.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_actionButtons);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            var btnClose = AshfallUiHelpers.MakeButton("CLOSE [Esc]", () => OnClose?.Invoke());
            btnClose.CustomMinimumSize = new Vector2(200, 40);
            vbox.AddChild(btnClose);

            var hint = AshfallUiHelpers.MakeSmall("[Esc] to close");
            hint.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeLabel);
            hint.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
            vbox.AddChild(hint);
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
