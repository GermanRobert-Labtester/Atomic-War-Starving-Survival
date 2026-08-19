using System;
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Survivor Detail panel.
    /// Shows individual survivor information, needs, traits, and status.
    /// </summary>
    public partial class SurvivorDetailPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblSurvivorInfoTitle;
        private VBoxContainer _survivorInfo;
        private Label _lblNeedsTitle;
        private VBoxContainer _needsList;
        private Label _lblTraitsTitle;
        private VBoxContainer _traitsList;
        private Label _lblStatusTitle;
        private VBoxContainer _statusList;

        // Placeholder survivor data
        private readonly string[] _placeholderSurvivorInfo = {
            "Name: Elena",
            "Role: Leader",
            "Age: 42",
            "Health: 85/100",
            "Radiation: 12 mSv (Low)",
            "Morale: Good (75/100)"
        };

        private readonly string[] _placeholderNeeds = {
            "Hunger: Normal (80/100)",
            "Thirst: Normal (75/100)",
            "Fatigue: Moderate (60/100)",
            "Warmth: Adequate (85/100)",
            "Hygiene: Good (90/100)"
        };

        private readonly string[] _placeholderTraits = {
            "Decisive — Makes tough calls quickly",
            "Resilient — Recovers from setbacks fast",
            "Empathetic — Cares for survivors well",
            "Pragmatic — Focuses on survival needs",
            "Stressed — Carries leadership burden"
        };

        private readonly string[] _placeholderStatus = {
            "Current Activity: Commanding bunker operations",
            "Location: Main Hallway",
            "Mood: Focused but tired",
            "Recent Events: Managed supply distribution",
            "Relationships: Strong with Marcus (Medic)"
        };

        // Real data from host session
        // private SurvivorHostSession? _survivorHost;
        // private string _selectedSurvivorId;

        public void Bind(object survivor, string survivorId) // placeholder for SurvivorHostSession
        {
            // _survivorHost = (SurvivorHostSession)survivor;
            // _selectedSurvivorId = survivorId;
            // RefreshView();
        }

        public void RefreshView()
        {
            if (_survivorInfo == null || _needsList == null || _traitsList == null || _statusList == null) return;

            // Clear existing lists
            AshfallUiHelpers.EmptyChildren(_survivorInfo);
            AshfallUiHelpers.EmptyChildren(_needsList);
            AshfallUiHelpers.EmptyChildren(_traitsList);
            AshfallUiHelpers.EmptyChildren(_statusList);

            // Display placeholder survivor info
            foreach (string info in _placeholderSurvivorInfo)
            {
                var label = new Label { Text = info };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _survivorInfo.AddChild(label);
            }

            // Display placeholder needs
            foreach (string need in _placeholderNeeds)
            {
                var label = new Label { Text = need };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _needsList.AddChild(label);
            }

            // Display placeholder traits
            foreach (string trait in _placeholderTraits)
            {
                var label = new Label { Text = trait };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                _traitsList.AddChild(label);
            }

            // Display placeholder status
            foreach (string status in _placeholderStatus)
            {
                var label = new Label { Text = status };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _statusList.AddChild(label);
            }
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

            var title = AshfallUiHelpers.MakeTitle("SURVIVOR DETAIL", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Survivor info section
            _lblSurvivorInfoTitle = AshfallUiHelpers.MakeSectionHeader("SURVIVOR INFORMATION");
            vbox.AddChild(_lblSurvivorInfoTitle);

            _survivorInfo = new VBoxContainer();
            _survivorInfo.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _survivorInfo.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_survivorInfo);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Needs section
            _lblNeedsTitle = AshfallUiHelpers.MakeSectionHeader("SURVIVAL NEEDS");
            vbox.AddChild(_lblNeedsTitle);

            _needsList = new VBoxContainer();
            _needsList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _needsList.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_needsList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Traits section
            _lblTraitsTitle = AshfallUiHelpers.MakeSectionHeader("PERSONALITY TRAITS");
            vbox.AddChild(_lblTraitsTitle);

            _traitsList = new VBoxContainer();
            _traitsList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _traitsList.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_traitsList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Status section
            _lblStatusTitle = AshfallUiHelpers.MakeSectionHeader("CURRENT STATUS");
            vbox.AddChild(_lblStatusTitle);

            _statusList = new VBoxContainer();
            _statusList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _statusList.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_statusList);

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
