// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using Godot;
using AtomicWar.GodotApp.Settings;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Central, accessibility-gated entrance animation for player panels
    /// (UI/UX audit 2026-09-25). One shared seam so all panel opens animate
    /// without touching individual panels.
    ///
    /// Motion authority: <see cref="AccessibilityPresentation.MotionAllowed"/>
    /// (ReducedMotion under <c>UserSettingsStore</c>) — the same gate the audio
    /// fade paths use. Motion is additionally disabled under <c>--headless</c>
    /// and while <see cref="SnapshotOrchestrator"/> captures, so golden
    /// snapshots can never observe a mid-fade frame.
    ///
    /// The animation is a strict round trip: fade 0→rest alpha, an 8px rise,
    /// and a center-pivoted 0.975→1.0 scale, all ending exactly at the panel's
    /// original position/viewport scale/modulation, so no layout or tint state
    /// is left mutated. Closing stays synchronous — global Esc dismissal is
    /// never delayed by motion.
    /// </summary>
    public static class UiMotion
    {
        /// <summary>Hard suppression for capture/CI contexts.</summary>
        public static bool Suppress = false;

        /// <summary>
        /// True when shared motion may run: not suppressed (captures/CI), not
        /// headless, and allowed by the ReducedMotion accessibility preference.
        /// </summary>
        public static bool CanAnimate
            => !Suppress && DisplayServer.GetName() != "headless" && AccessibilityPresentation.MotionAllowed;

        /// <summary>Entrance duration in seconds (restrained, snappy).</summary>
        public const float OpenDurationSeconds = 0.14f;

        /// <summary>Exit duration in seconds (shorter than the entrance).</summary>
        public const float CloseDurationSeconds = 0.10f;

        /// <summary>Rise distance in pixels; the panel ends exactly at its rest position.</summary>
        public const float RisePixels = 8f;

        /// <summary>
        /// Scale factor the entrance grows from ("animating to full scale").
        /// Subtle enough to read as a settle rather than a zoom, and large
        /// enough that the panel visibly resolves into place.
        /// </summary>
        public const float OpenScaleFrom = 0.975f;

        /// <summary>
        /// Plays the shared open animation on <paramref name="panel"/>:
        /// alpha 0→rest, 8px rise→rest, <see cref="OpenScaleFrom"/>·rest→rest
        /// scale around the panel center. No-op when motion is suppressed,
        /// unavailable (headless), or disallowed by the accessibility
        /// ReducedMotion preference.
        /// </summary>
        public static void AnimateOpen(Control panel)
        {
            if (panel == null || !GodotObject.IsInstanceValid(panel) || !panel.IsInsideTree())
                return;
            if (!CanAnimate)
                return;
            if (IsClosing(panel))
                return;

            // Capture the exact rest state so the tween is a strict round trip
            // even for panels that carry a deliberate position/scale/tint.
            Vector2 restPosition = panel.Position;
            Vector2 restScale = panel.Scale;
            Color restModulate = panel.Modulate;

            // Center-pivot so the scale resolves from the panel's middle
            // instead of its top-left corner; pivot only affects transforms.
            panel.PivotOffset = panel.Size / 2f;

            panel.Modulate = new Color(restModulate.R, restModulate.G, restModulate.B, 0f);
            panel.Position = restPosition + new Vector2(0f, RisePixels);
            panel.Scale = restScale * OpenScaleFrom;

            Tween tween = panel.CreateTween();
            tween.SetTrans(Tween.TransitionType.Cubic).SetEase(Tween.EaseType.Out);
            tween.TweenProperty(panel, "modulate:a", restModulate.A, OpenDurationSeconds);
            tween.Parallel().TweenProperty(panel, "position", restPosition, OpenDurationSeconds);
            tween.Parallel().TweenProperty(panel, "scale", restScale, OpenDurationSeconds);
            tween.Finished += () =>
            {
                if (!GodotObject.IsInstanceValid(panel))
                    return;
                // Pin the exact rest state (guards against interrupted tweens
                // and floating-point residue).
                panel.Position = restPosition;
                panel.Scale = restScale;
                panel.Modulate = restModulate;
            };
        }

        private sealed class ClosingState
        {
            public Tween? Tween;
            public Vector2 Position;
            public Vector2 Scale;
            public Color Modulate;
            public Control.MouseFilterEnum MouseFilter;
        }

        private static readonly Dictionary<ulong, ClosingState> _closing = new();

        /// <summary>
        /// Plays the shared exit animation on <paramref name="panel"/>: a short
        /// fade to transparent with a small scale-down, after which the panel is
        /// hidden and its exact rest state restored. Returns <c>true</c> when the
        /// animation took ownership of hiding the panel; <c>false</c> when motion
        /// is unavailable and the caller must hide it synchronously.
        ///
        /// Closing is only applied at centralized dismissal seams (global Esc /
        /// panel switching) so gameplay state never lags behind the visual.
        /// </summary>
        public static bool AnimateClose(Control panel)
        {
            if (panel == null || !GodotObject.IsInstanceValid(panel) || !panel.IsInsideTree())
                return false;
            if (!CanAnimate)
                return false;
            if (!panel.Visible)
                return false;
            if (_closing.ContainsKey(panel.GetInstanceId()))
                return true; // already fading out; it will hide itself

            ulong instanceId = panel.GetInstanceId();
            var state = new ClosingState
            {
                Position = panel.Position,
                Scale = panel.Scale,
                Modulate = panel.Modulate,
                MouseFilter = panel.MouseFilter,
            };

            panel.PivotOffset = panel.Size / 2f;
            panel.MouseFilter = Control.MouseFilterEnum.Ignore;

            Tween tween = panel.CreateTween();
            tween.SetTrans(Tween.TransitionType.Cubic).SetEase(Tween.EaseType.In);
            tween.TweenProperty(panel, "modulate:a", 0d, CloseDurationSeconds);
            tween.Parallel().TweenProperty(panel, "scale", state.Scale * 0.985f, CloseDurationSeconds);
            tween.Parallel().TweenProperty(panel, "position", state.Position + new Vector2(0f, 4f), CloseDurationSeconds);
            state.Tween = tween;
            _closing[instanceId] = state;

            tween.Finished += () =>
            {
                if (!GodotObject.IsInstanceValid(panel))
                {
                    _closing.Remove(instanceId);
                    return;
                }
                if (!_closing.TryGetValue(instanceId, out var current) || !ReferenceEquals(current, state))
                    return; // superseded by a cancel
                _closing.Remove(instanceId);
                Restore(panel, state);
                panel.Visible = false;
            };
            return true;
        }

        /// <summary>
        /// True while <paramref name="panel"/> is fading out. Such panels are
        /// logically closed: open-state checks and close loops must ignore them.
        /// </summary>
        public static bool IsClosing(Control panel)
        {
            if (panel == null)
                return false;
            return _closing.ContainsKey(panel.GetInstanceId());
        }

        /// <summary>True while any panel is fading out.</summary>
        public static bool AnyClosing => _closing.Count > 0;

        /// <summary>
        /// Cancels a running exit animation (the panel was re-opened during the
        /// fade) and restores its rest visual state without hiding it.
        /// </summary>
        public static void CancelClose(Control panel)
        {
            if (panel == null || !GodotObject.IsInstanceValid(panel))
                return;
            if (!_closing.TryGetValue(panel.GetInstanceId(), out var state))
                return;

            _closing.Remove(panel.GetInstanceId());
            state.Tween?.Kill();
            Restore(panel, state);
        }

        private static void Restore(Control panel, ClosingState state)
        {
            panel.Position = state.Position;
            panel.Scale = state.Scale;
            panel.Modulate = state.Modulate;
            panel.MouseFilter = state.MouseFilter;
        }

        // ── UI special FX: interactive micro-motion (UI/UX audit follow-up) ──
        //
        // Every button gets a restrained hover lift and press kick, attached
        // centrally at UI build so raw `new Button` sites and helper-built
        // buttons behave identically. Scale-only (never layout), and each
        // handler re-checks the accessibility motion gate so toggling
        // ReducedMotion takes effect immediately.

        private static readonly Dictionary<ulong, Tween> _buttonTweens = new();

        /// <summary>Hover lift factor for interactive buttons.</summary>
        public const float ButtonHoverScale = 1.03f;

        /// <summary>Press kick factor for interactive buttons.</summary>
        public const float ButtonPressScale = 0.97f;

        /// <summary>Focus lift factor for keyboard/controller buttons.</summary>
        public const float ButtonFocusScale = 1.015f;

        /// <summary>
        /// Attaches hover/press micro-motion to <paramref name="button"/>
        /// (idempotent). The effect is visual only: scale is transformed around
        /// the button center and returns exactly to 1.0, so layout and hit tests
        /// are unaffected.
        /// </summary>
        public static void AttachButtonFx(Button button)
        {
            if (button == null || !GodotObject.IsInstanceValid(button))
                return;
            if (button.HasMeta("ashfall_button_fx"))
                return;
            button.SetMeta("ashfall_button_fx", true);

            button.MouseEntered += () => TweenButtonScale(button, ButtonHoverScale, 0.07f);
            button.MouseExited += () => TweenButtonScale(button, 1f, 0.07f);
            button.ButtonDown += () => TweenButtonScale(button, ButtonPressScale, 0.05f);
            button.ButtonUp += () =>
            {
                bool stillHovered = GodotObject.IsInstanceValid(button) && button.IsHovered();
                TweenButtonScale(button, stillHovered ? ButtonHoverScale : 1f, 0.06f);
            };
            // Keyboard/controller focus gets a subtle lift so D-pad navigation is
            // legible even before any press.
            button.FocusEntered += () => TweenButtonScale(button, ButtonFocusScale, 0.08f);
            button.FocusExited += () =>
            {
                bool stillHovered = GodotObject.IsInstanceValid(button) && button.IsHovered();
                TweenButtonScale(button, stillHovered ? ButtonHoverScale : 1f, 0.07f);
            };
        }

        private static void TweenButtonScale(Button button, float target, float duration)
        {
            if (!CanAnimate || !GodotObject.IsInstanceValid(button) || button.Disabled || !button.IsVisibleInTree())
                return;

            button.PivotOffset = button.Size / 2f;
            ulong id = button.GetInstanceId();
            if (_buttonTweens.TryGetValue(id, out var running) && running != null && GodotObject.IsInstanceValid(running))
                running.Kill();

            Tween tween = button.CreateTween();
            tween.SetTrans(Tween.TransitionType.Cubic).SetEase(Tween.EaseType.Out);
            tween.TweenProperty(button, "scale", Vector2.One * target, duration);
            _buttonTweens[id] = tween;
            tween.Finished += () =>
            {
                if (!GodotObject.IsInstanceValid(button))
                {
                    _buttonTweens.Remove(id);
                    return;
                }
                // Settle exactly at rest when the hover/press state is over so
                // no fractional scale can accumulate across interactions.
                if (target <= 1f && !button.IsHovered() && !button.ButtonPressed)
                    button.Scale = Vector2.One;
                _buttonTweens.Remove(id);
            };
        }
    }
}
