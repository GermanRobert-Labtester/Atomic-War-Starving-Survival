// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Godot;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// UI/UX wave — shared panel flow helpers.
    ///
    /// Three concerns, one owner each:
    ///   • <see cref="TransitionSwap"/> — in-panel content swaps animate instead
    ///     of popping (near-instant: 60 ms out, 100 ms in, honouring the
    ///     reduced-motion preference through UiMotion).
    ///   • <see cref="AttachDrag"/> — movable windows with safe clamping and
    ///     optional persisted positions.
    ///   • <see cref="Pulse"/> — value-change feedback for status readouts.
    ///
    /// Nothing here owns gameplay state. Positions persist to a UI-only file
    /// (user://ui_layout.json); no gameplay save section is involved.
    /// </summary>
    public static class UiPanelFlow
    {
        private const float SwapOutSeconds = 0.06f;
        private const float SwapInSeconds = 0.10f;

        // ── In-panel transitions ───────────────────────────────────────────

        /// <summary>
        /// Swap the visual content of <paramref name="host"/> through a short
        /// fade/rise: the host fades and drops 6 px, <paramref name="rebuild"/>
        /// runs while it is invisible, then it fades back with a 0.99→1.0
        /// settle. Reduced motion collapses this to an instant swap.
        /// </summary>
        public static void TransitionSwap(Control host, Action rebuild)
        {
            if (host == null || !GodotObject.IsInstanceValid(host)) { rebuild?.Invoke(); return; }
            if (!UiMotion.CanAnimate) { rebuild?.Invoke(); return; }

            var tweener = host.CreateTween();
            tweener.SetParallel(false);
            tweener.TweenProperty(host, "modulate:a", 0f, SwapOutSeconds)
                .SetTrans(Tween.TransitionType.Cubic).SetEase(Tween.EaseType.Out);
            tweener.TweenCallback(Callable.From(() =>
            {
                if (!GodotObject.IsInstanceValid(host)) return;
                rebuild?.Invoke();
                host.Position = host.Position with { Y = host.Position.Y + 6f };
                host.PivotOffset = host.Size / 2f;
                host.Scale = new Vector2(0.99f, 0.99f);
            }));
            tweener.TweenProperty(host, "modulate:a", 1f, SwapInSeconds)
                .SetTrans(Tween.TransitionType.Cubic).SetEase(Tween.EaseType.Out);
            tweener.Parallel().TweenProperty(host, "scale", Vector2.One, SwapInSeconds)
                .SetTrans(Tween.TransitionType.Back).SetEase(Tween.EaseType.Out);
            tweener.Parallel().TweenProperty(host, "position:y", host.Position.Y, SwapInSeconds)
                .SetTrans(Tween.TransitionType.Cubic).SetEase(Tween.EaseType.Out);
        }

        // ── Movable windows ────────────────────────────────────────────────

        /// <summary>
        /// Make <paramref name="window"/> draggable by <paramref name="handle"/>.
        /// The offset is clamped to the viewport; when
        /// <paramref name="layoutKey"/> is non-empty the position is restored on
        /// attach and persisted when the drag ends.
        /// </summary>
        public static void AttachDrag(Control window, Control handle, string? layoutKey = null)
        {
            if (window == null || handle == null) return;
            if (!GodotObject.IsInstanceValid(window) || !GodotObject.IsInstanceValid(handle)) return;

            bool dragging = false;
            Vector2 grabOffset = Vector2.Zero;

            if (!string.IsNullOrEmpty(layoutKey) && UiLayoutStore.TryLoad(layoutKey!, out var saved))
            {
                window.Position = saved;
            }

            handle.MouseFilter = Control.MouseFilterEnum.Stop;
            handle.GuiInput += @event =>
            {
                if (@event is not InputEventMouseButton mouse) return;
                if (mouse.ButtonIndex != MouseButton.Left) return;

                if (mouse.Pressed)
                {
                    dragging = true;
                    grabOffset = window.GetGlobalMousePosition() - window.GlobalPosition;
                }
                else
                {
                    dragging = false;
                    if (!string.IsNullOrEmpty(layoutKey)) UiLayoutStore.Save(layoutKey!, ClampToViewport(window, window.Position));
                }
            };

            handle.MouseDefaultCursorShape = Control.CursorShape.Move;
            window.TreeExiting += () => { if (dragging && !string.IsNullOrEmpty(layoutKey)) UiLayoutStore.Save(layoutKey!, window.Position); };

            window.GuiInput += @event =>
            {
                if (!dragging || @event is not InputEventMouseMotion) return;
                var target = window.GetGlobalMousePosition() - grabOffset;
                window.Position = ClampToViewport(window, target);
            };
        }

        private static Vector2 ClampToViewport(Control window, Vector2 position)
        {
            var viewport = window.GetViewport()?.GetVisibleRect().Size ?? new Vector2(1920, 1080);
            var size = window.Size;
            float maxX = Math.Max(0f, viewport.X - size.X);
            float maxY = Math.Max(0f, viewport.Y - size.Y);
            return new Vector2(Math.Clamp(position.X, 0f, maxX), Math.Clamp(position.Y, 0f, maxY));
        }

        // ── Value-change feedback ──────────────────────────────────────────

        /// <summary>
        /// Brief settle pulse for a value that just changed (1.0 → 1.05 → 1.0).
        /// Skipped entirely under reduced motion.
        /// </summary>
        public static void Pulse(Control target, float strength = 1.05f)
        {
            if (target == null || !GodotObject.IsInstanceValid(target)) return;
            if (!UiMotion.CanAnimate) return;

            var tweener = target.CreateTween();
            tweener.TweenProperty(target, "scale", new Vector2(strength, strength), 0.06f)
                .SetTrans(Tween.TransitionType.Cubic).SetEase(Tween.EaseType.Out);
            tweener.TweenProperty(target, "scale", Vector2.One, 0.10f)
                .SetTrans(Tween.TransitionType.Cubic).SetEase(Tween.EaseType.Out);
        }
    }

    /// <summary>
    /// UI-only layout persistence (window offsets). Writes atomically to
    /// user://ui_layout.json. Not a gameplay save section: losing it resets
    /// window positions and nothing else.
    /// </summary>
    public static class UiLayoutStore
    {
        public const string FileName = "ui_layout.json";

        private static Dictionary<string, Vector2Dto>? _cache;
        private static bool _dirty;

        public static string SavePath => ProjectSettings.GlobalizePath($"user://{FileName}");

        private sealed class Vector2Dto
        {
            public float x { get; set; }
            public float y { get; set; }
        }

        private static void EnsureLoaded()
        {
            if (_cache != null) return;
            _cache = new Dictionary<string, Vector2Dto>(StringComparer.Ordinal);
            try
            {
                string path = ProjectSettings.GlobalizePath($"user://{FileName}");
                if (!File.Exists(path)) return;
                var parsed = JsonSerializer.Deserialize<Dictionary<string, Vector2Dto>>(File.ReadAllText(path));
                if (parsed != null) _cache = new Dictionary<string, Vector2Dto>(parsed, StringComparer.Ordinal);
            }
            catch (Exception)
            {
                // A corrupt layout file must never block the UI; start fresh.
                _cache = new Dictionary<string, Vector2Dto>(StringComparer.Ordinal);
            }
        }

        public static bool TryLoad(string key, out Vector2 position)
        {
            EnsureLoaded();
            if (_cache!.TryGetValue(key, out var dto) && dto != null)
            {
                position = new Vector2(dto.x, dto.y);
                return true;
            }
            position = Vector2.Zero;
            return false;
        }

        public static void Save(string key, Vector2 position)
        {
            if (string.IsNullOrEmpty(key)) return;
            EnsureLoaded();
            _cache![key] = new Vector2Dto { x = position.X, y = position.Y };
            _dirty = true;
            Flush();
        }

        public static void Flush()
        {
            if (!_dirty || _cache == null) return;
            try
            {
                string global = ProjectSettings.GlobalizePath($"user://{FileName}");
                string tmp = global + ".tmp";
                File.WriteAllText(tmp, JsonSerializer.Serialize(_cache, new JsonSerializerOptions { WriteIndented = true }));
                File.Move(tmp, global, overwrite: true);
                _dirty = false;
            }
            catch (Exception)
            {
                // Layout persistence is best-effort; a full disk or sandboxed
                // path must not crash the UI.
            }
        }

        /// <summary>Test/headless hook: forget cached positions.</summary>
        public static void ResetCache()
        {
            _cache = null;
            _dirty = false;
        }
    }
}
