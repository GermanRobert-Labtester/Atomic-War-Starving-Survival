using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Full-rect background carousel with subtle parallax effect.
    /// Shares presentation state: two texture layers, dark overlay, crossfade timer,
    /// and parallax offset based on viewport position.
    /// </summary>
    public partial class UiBackgroundCarousel : Control
    {
        private readonly string[] _backgroundPaths;
        private readonly float _overlayAlpha;
        private readonly float _transitionDuration;
        private readonly float _parallaxStrength;

        private TextureRect _layerA = null!;
        private TextureRect _layerB = null!;
        private int _currentIndex;
        private float _transitionProgress;
        private bool _transitioning;

        // Parallax state
        private Vector2 _parallaxOffset = Vector2.Zero;
        private Vector2 _targetParallax = Vector2.Zero;

        public UiBackgroundCarousel(
            IReadOnlyList<string> projectRelativePaths,
            float overlayAlpha,
            float transitionDuration = 1.5f,
            float parallaxStrength = 0.08f)
        {
            if (projectRelativePaths == null)
                throw new ArgumentNullException(nameof(projectRelativePaths));

            _backgroundPaths = new string[projectRelativePaths.Count];
            for (int i = 0; i < projectRelativePaths.Count; i++)
                _backgroundPaths[i] = projectRelativePaths[i];

            _overlayAlpha = Mathf.Clamp(overlayAlpha, 0f, 1f);
            _transitionDuration = Mathf.Max(0.01f, transitionDuration);
            _parallaxStrength = Mathf.Clamp(parallaxStrength, 0f, 0.2f); // 0-20% parallax
            MouseFilter = MouseFilterEnum.Ignore;
        }

        public int CurrentBackgroundIndex => _currentIndex;

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);

            var fallback = new ColorRect
            {
                Color = new Color(0.035f, 0.043f, 0.047f, 1f),
                MouseFilter = MouseFilterEnum.Ignore
            };
            fallback.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(fallback);

            _layerA = MakeLayer();
            _layerA.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(_layerA);

            _layerB = MakeLayer();
            _layerB.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(_layerB);

            var overlay = new ColorRect
            {
                Color = new Color(0f, 0f, 0f, _overlayAlpha),
                MouseFilter = MouseFilterEnum.Ignore
            };
            overlay.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(overlay);

            if (_backgroundPaths.Length == 0)
                return;

            LoadBackgroundToLayer(_layerA, _currentIndex);
            if (_backgroundPaths.Length > 1)
            {
                LoadBackgroundToLayer(_layerB, NextIndex());
                _layerB.Modulate = new Color(1f, 1f, 1f, 0f);
                StartCrossfade();
            }
        }

        public override void _Process(double delta)
        {
            // Update parallax based on viewport position
            UpdateParallax(delta);

            if (!Visible || !_transitioning || _backgroundPaths.Length < 2)
                return;

            _transitionProgress += (float)delta / _transitionDuration;
            if (_transitionProgress >= 1f)
            {
                CompleteTransition();
                return;
            }

            float alpha = SmoothStep(_transitionProgress);
            _layerA.Modulate = new Color(1f, 1f, 1f, 1f - alpha);
            _layerB.Modulate = new Color(1f, 1f, 1f, alpha);
        }

        private void UpdateParallax(double delta)
        {
            if (_parallaxStrength <= 0f) return;

            // Calculate target parallax based on viewport position
            Vector2 viewportSize = GetSize(); // Control's size, which matches viewport for full-rect
            float viewportWidth = viewportSize.X;
            float viewportHeight = viewportSize.Y;

            // Target offset: center of screen = (0,0), edges = ±strength
            Vector2 windowPos = GetWindow().GetPosition();
            _targetParallax.X = (windowPos.X / viewportWidth - 0.5f) * _parallaxStrength * 2f;
            _targetParallax.Y = (windowPos.Y / viewportHeight - 0.5f) * _parallaxStrength * 2f;

            // Smooth interpolation using manual lerp
            float lerpFactor = Mathf.Clamp((float)(delta * 3f), 0f, 1f);
            _parallaxOffset.X += (_targetParallax.X - _parallaxOffset.X) * lerpFactor;
            _parallaxOffset.Y += (_targetParallax.Y - _parallaxOffset.Y) * lerpFactor;

            // Apply parallax to layers
            if (_layerA != null && _layerA.Visible)
            {
                _layerA.Position = new Vector2(_parallaxOffset.X * viewportSize.X, _parallaxOffset.Y * viewportSize.Y);
            }

            if (_layerB != null && _layerB.Visible)
            {
                _layerB.Position = new Vector2(_parallaxOffset.X * viewportSize.X, _parallaxOffset.Y * viewportSize.Y);
            }
        }

        private static TextureRect MakeLayer()
        {
            return new TextureRect
            {
                StretchMode = TextureRect.StretchModeEnum.KeepAspectCovered,
                ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize,
                MouseFilter = MouseFilterEnum.Ignore
            };
        }

        private void LoadBackgroundToLayer(TextureRect layer, int index)
        {
            if (index < 0 || index >= _backgroundPaths.Length)
            {
                layer.Texture = null;
                layer.Visible = false;
                return;
            }

            Texture2D? texture = AshfallUiHelpers.TryLoadTexture(
                ToResourcePath(_backgroundPaths[index]));
            layer.Texture = texture;
            layer.Visible = texture != null;
        }

        private static string ToResourcePath(string projectRelativePath)
        {
            return "res://" + projectRelativePath.Replace('\\', '/').TrimStart('/');
        }

        private void StartCrossfade()
        {
            _transitionProgress = 0f;
            _transitioning = true;
        }

        private void CompleteTransition()
        {
            _transitioning = false;
            _transitionProgress = 0f;

            var oldForeground = _layerA;
            _layerA = _layerB;
            _layerB = oldForeground;
            _currentIndex = NextIndex();

            _layerA.Modulate = new Color(1f, 1f, 1f, 1f);
            LoadBackgroundToLayer(_layerB, NextIndex());
            _layerB.Modulate = new Color(1f, 1f, 1f, 0f);
            StartCrossfade();
        }

        private int NextIndex()
        {
            return (_currentIndex + 1) % _backgroundPaths.Length;
        }

        private static float SmoothStep(float value)
        {
            value = Mathf.Clamp(value, 0f, 1f);
            return value * value * (3f - 2f * value);
        }
    }
}
