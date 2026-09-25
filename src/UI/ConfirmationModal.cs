// SPDX-License-Identifier: MIT
using Godot;
using System;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Reusable confirmation dialog implementing IModalPanel for high-consequence actions.
    /// Strictly gates action execution: Confirm executes exactly once, Cancel executes zero domain mutations.
    /// Focus defaults to Cancel for safety.
    /// </summary>
    public partial class ConfirmationModal : PanelContainer, IModalPanel
    {
        private Label _lblTitle = null!;
        private Label _lblMessage = null!;
        private Button _btnConfirm = null!;
        private Button _btnCancel = null!;

        private ConfirmationFlowGate? _activeGate;

        public bool IsModalOpen => Visible;
        public event Action? OnModalClosed;
        public Control? InitialFocusControl => _btnCancel;

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.Center);
            CustomMinimumSize = new Vector2(480, 220);
            Visible = false;

            // UI/UX wave: dialogs are movable — drag anywhere that is not a
            // button, and the position persists in user://ui_layout.json.
            UiPanelFlow.AttachDrag(this, this, "confirmation_modal");

            // Background styling
            var styleBox = new StyleBoxFlat
            {
                BgColor = new Color(0.08f, 0.08f, 0.09f, 0.95f),
                BorderColor = AshfallUiHelpers.ToColor(DesignTheme.Entropy),
                BorderWidthBottom = 2,
                BorderWidthTop = 2,
                BorderWidthLeft = 2,
                BorderWidthRight = 2,
                CornerRadiusBottomLeft = 4,
                CornerRadiusBottomRight = 4,
                CornerRadiusTopLeft = 4,
                CornerRadiusTopRight = 4
            };
            AddThemeStyleboxOverride("panel", styleBox);

            var margin = new MarginContainer();
            margin.AddThemeConstantOverride("margin_top", (int)DesignTheme.SpacingLg);
            margin.AddThemeConstantOverride("margin_bottom", (int)DesignTheme.SpacingLg);
            margin.AddThemeConstantOverride("margin_left", (int)DesignTheme.SpacingLg);
            margin.AddThemeConstantOverride("margin_right", (int)DesignTheme.SpacingLg);
            AddChild(margin);

            var vbox = new VBoxContainer();
            vbox.AddThemeConstantOverride("separation", (int)DesignTheme.SpacingMd);
            margin.AddChild(vbox);

            // Title
            _lblTitle = AshfallUiHelpers.MakeTitle("CONFIRM ACTION", DesignTheme.FontSizeH3);
            _lblTitle.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Entropy));
            vbox.AddChild(_lblTitle);

            // Message
            _lblMessage = AshfallUiHelpers.MakeBody("Are you sure you want to proceed?");
            _lblMessage.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            _lblMessage.CustomMinimumSize = new Vector2(420, 60);
            vbox.AddChild(_lblMessage);

            // Spacer
            vbox.AddChild(new Control { SizeFlagsVertical = SizeFlags.ExpandFill });

            // Button bar
            var btnBar = new HBoxContainer();
            btnBar.AddThemeConstantOverride("separation", (int)DesignTheme.SpacingMd);
            btnBar.Alignment = BoxContainer.AlignmentMode.End;

            _btnCancel = AshfallUiHelpers.MakeButton("CANCEL", OnCancelPressed);
            _btnCancel.CustomMinimumSize = new Vector2(120, 32);
            btnBar.AddChild(_btnCancel);

            _btnConfirm = AshfallUiHelpers.MakeButton("CONFIRM", OnConfirmPressed);
            _btnConfirm.CustomMinimumSize = new Vector2(120, 32);
            _btnConfirm.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Critical));
            btnBar.AddChild(_btnConfirm);

            vbox.AddChild(btnBar);
        }

        public void Prompt(string title, string message, Action onConfirm, Action? onCancel = null, string confirmText = "CONFIRM", string cancelText = "CANCEL", Func<bool>? isValidityPredicate = null)
        {
            _activeGate = new ConfirmationFlowGate(onConfirm, onCancel, isValidityPredicate);

            _lblTitle.Text = string.IsNullOrEmpty(title) ? "CONFIRM ACTION" : title;
            _lblMessage.Text = message;
            _btnConfirm.Text = confirmText;
            _btnCancel.Text = cancelText;

            Visible = true;
            _btnCancel.GrabFocus();
        }

        private void OnConfirmPressed()
        {
            var gate = _activeGate;
            _activeGate = null;
            CloseModal();
            gate?.Confirm();
        }

        private void OnCancelPressed()
        {
            var gate = _activeGate;
            _activeGate = null;
            CloseModal();
            gate?.Cancel();
        }

        public void CloseModal()
        {
            if (!Visible) return;
            Visible = false;
            _activeGate?.Cancel();
            _activeGate = null;
            OnModalClosed?.Invoke();
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (!Visible) return;

            if (@event.IsActionPressed("ui_cancel"))
            {
                GetViewport().SetInputAsHandled();
                OnCancelPressed();
            }
        }
    }
}
