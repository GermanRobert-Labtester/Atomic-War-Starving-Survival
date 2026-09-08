using Godot;
using System;
using System.Collections.Generic;
using Ashfall.Core.Feedback;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Player-facing transient feedback and toast notification overlay in Godot.
    /// Manages presentation queue, multi-card stacking, auto-dismiss timers,
    /// accessible severity badges, pause-on-hover, and dismiss actions.
    /// Matches ContentUtilizationScanner's canonical UI consumer requirement.
    /// </summary>
    public partial class FeedbackPanel : Control
    {
        public const int MaxVisibleToasts = 4;

        private sealed class ActiveToast
        {
            public PanelContainer Container { get; set; } = null!;
            public float RemainingSeconds { get; set; }
            public bool IsHovered { get; set; }
            public ResolvedFeedbackMessage Message { get; set; } = null!;
        }

        private VBoxContainer _toastStack = null!;
        private readonly List<ActiveToast> _activeToasts = new List<ActiveToast>();
        private readonly Queue<ResolvedFeedbackMessage> _pendingQueue = new Queue<ResolvedFeedbackMessage>();
        private IFeedbackService? _feedbackService;

        public int ActiveToastCount => _activeToasts.Count;
        public int PendingQueueCount => _pendingQueue.Count;

        public override void _Ready()
        {
            // Position toast stack at top-right, just below the top HUD
            SetAnchorsPreset(LayoutPreset.TopRight);
            Position = new Vector2(-420, 56);
            CustomMinimumSize = new Vector2(400, 400);
            MouseFilter = MouseFilterEnum.Ignore;

            _toastStack = new VBoxContainer();
            _toastStack.SetAnchorsPreset(LayoutPreset.TopRight);
            _toastStack.CustomMinimumSize = new Vector2(400, 0);
            _toastStack.AddThemeConstantOverride("separation", (int)DesignTheme.SpacingSm);
            _toastStack.MouseFilter = MouseFilterEnum.Ignore;
            AddChild(_toastStack);
        }

        public void Bind(IFeedbackService feedbackService)
        {
            Unbind();
            _feedbackService = feedbackService;
            if (_feedbackService != null)
            {
                _feedbackService.OnFeedbackEmitted += HandleFeedbackEmitted;
            }
        }

        public void Unbind()
        {
            if (_feedbackService != null)
            {
                _feedbackService.OnFeedbackEmitted -= HandleFeedbackEmitted;
                _feedbackService = null;
            }
            ClearAllToasts();
        }

        private void HandleFeedbackEmitted(ResolvedFeedbackMessage message)
        {
            // Callable.From ensures thread-safe dispatch onto Godot's main thread
            Callable.From(() => ShowToast(message)).CallDeferred();
        }

        public void ShowToast(ResolvedFeedbackMessage message)
        {
            if (message == null) return;

            if (_activeToasts.Count >= MaxVisibleToasts)
            {
                _pendingQueue.Enqueue(message);
                return;
            }

            CreateToastCard(message);
        }

        private void CreateToastCard(ResolvedFeedbackMessage message)
        {
            var card = new PanelContainer();
            card.CustomMinimumSize = new Vector2(380, 52);
            card.MouseFilter = MouseFilterEnum.Pass;

            Color borderColor;
            string prefix;
            switch (message.Severity)
            {
                case FeedbackSeverity.Critical:
                    borderColor = AshfallUiHelpers.ToColor(DesignTheme.Critical);
                    prefix = "[CRITICAL]";
                    break;
                case FeedbackSeverity.Error:
                    borderColor = AshfallUiHelpers.ToColor(DesignTheme.Critical);
                    prefix = "[ERROR]";
                    break;
                case FeedbackSeverity.Warning:
                    borderColor = AshfallUiHelpers.ToColor(DesignTheme.Entropy);
                    prefix = "[WARNING]";
                    break;
                case FeedbackSeverity.Success:
                    borderColor = AshfallUiHelpers.ToColor(DesignTheme.Warm);
                    prefix = "[SUCCESS]";
                    break;
                default:
                    borderColor = AshfallUiHelpers.ToColor(DesignTheme.Pale);
                    prefix = "[INFO]";
                    break;
            }

            var style = new StyleBoxFlat
            {
                BgColor = new Color(0.10f, 0.10f, 0.12f, 0.94f),
                BorderColor = borderColor,
                BorderWidthBottom = 2,
                BorderWidthLeft = 4, // distinct left severity stripe
                BorderWidthTop = 1,
                BorderWidthRight = 1,
                CornerRadiusBottomLeft = 3,
                CornerRadiusBottomRight = 3,
                CornerRadiusTopLeft = 3,
                CornerRadiusTopRight = 3
            };
            card.AddThemeStyleboxOverride("panel", style);

            var margin = new MarginContainer();
            margin.AddThemeConstantOverride("margin_top", (int)DesignTheme.SpacingSm);
            margin.AddThemeConstantOverride("margin_bottom", (int)DesignTheme.SpacingSm);
            margin.AddThemeConstantOverride("margin_left", (int)DesignTheme.SpacingMd);
            margin.AddThemeConstantOverride("margin_right", (int)DesignTheme.SpacingSm);
            margin.MouseFilter = MouseFilterEnum.Pass;
            card.AddChild(margin);

            var hbox = new HBoxContainer();
            hbox.AddThemeConstantOverride("separation", (int)DesignTheme.SpacingSm);
            hbox.MouseFilter = MouseFilterEnum.Pass;
            margin.AddChild(hbox);

            // Badge / Prefix
            var lblPrefix = AshfallUiHelpers.MakeSmall(prefix);
            lblPrefix.AddThemeColorOverride("font_color", borderColor);
            hbox.AddChild(lblPrefix);

            // Message text
            var lblText = AshfallUiHelpers.MakeBody(message.FormattedText);
            lblText.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            lblText.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            lblText.MouseFilter = MouseFilterEnum.Pass;
            hbox.AddChild(lblText);

            var activeToast = new ActiveToast
            {
                Container = card,
                RemainingSeconds = message.DisplayDurationSeconds > 0 ? message.DisplayDurationSeconds : 3.0f,
                IsHovered = false,
                Message = message
            };

            // Close button
            var btnClose = AshfallUiHelpers.MakeButton("×", () => DismissToast(activeToast));
            btnClose.CustomMinimumSize = new Vector2(24, 24);
            btnClose.TooltipText = "Dismiss";
            hbox.AddChild(btnClose);

            // Hover pause listeners
            card.MouseEntered += () => activeToast.IsHovered = true;
            card.MouseExited += () => activeToast.IsHovered = false;

            _toastStack.AddChild(card);
            _activeToasts.Add(activeToast);
        }

        public override void _Process(double delta)
        {
            float dt = (float)delta;
            for (int i = _activeToasts.Count - 1; i >= 0; i--)
            {
                var toast = _activeToasts[i];
                if (!toast.IsHovered)
                {
                    toast.RemainingSeconds -= dt;
                    if (toast.RemainingSeconds <= 0f)
                    {
                        DismissToast(toast);
                    }
                }
            }
        }

        private void DismissToast(ActiveToast toast)
        {
            if (!_activeToasts.Contains(toast)) return;

            _activeToasts.Remove(toast);
            if (IsInstanceValid(toast.Container))
            {
                toast.Container.QueueFree();
            }

            // Dequeue next pending toast if available
            if (_pendingQueue.Count > 0 && _activeToasts.Count < MaxVisibleToasts)
            {
                var next = _pendingQueue.Dequeue();
                CreateToastCard(next);
            }
        }

        public void ClearAllToasts()
        {
            foreach (var t in _activeToasts)
            {
                if (IsInstanceValid(t.Container))
                {
                    t.Container.QueueFree();
                }
            }
            _activeToasts.Clear();
            _pendingQueue.Clear();
        }
    }
}
