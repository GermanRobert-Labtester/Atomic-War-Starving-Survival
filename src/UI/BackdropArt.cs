// SPDX-License-Identifier: MIT
using System;
using Godot;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Placeholder backdrop art loader for surface/shelter panels (days 1-7,
    /// intact world). Every path points at a clearly-labelled placeholder PNG
    /// under assets/sprites/Surface or assets/sprites/Shelter. Missing art is
    /// non-fatal: the panel keeps a flat dim overlay so headless runs without
    /// an import cache stay clean.
    /// </summary>
    public static class BackdropArt
    {
        public const string SurfaceDir = "res://assets/sprites/Surface/";
        public const string ShelterDir = "res://assets/sprites/Shelter/";

        public const string WastelandSky = SurfaceDir + "wasteland_sky_day1_7.png";
        public const string SurfaceHatchApproach = SurfaceDir + "surface_hatch_approach_day1_7.png";
        public const string ExpeditionDeparture = SurfaceDir + "expedition_departure_day1_7.png";
        public const string ShelterInterior = ShelterDir + "shelter_interior_day1_7.png";

        /// <summary>Normalises a schedule/clock phase to the authored file suffix.</summary>
        public static string NormalizePhase(string? phase) => phase?.ToLowerInvariant() switch
        {
            "dawn" or "morning" => "dawn",
            "dusk" or "evening" or "curfew" => "dusk",
            "night" => "night",
            _ => "day1_7"
        };

        public static string WastelandSkyFor(string? phase) => $"{SurfaceDir}wasteland_sky_{NormalizePhase(phase)}.png";
        public static string ExpeditionDepartureFor(string? phase) => $"{SurfaceDir}expedition_departure_{NormalizePhase(phase)}.png";
        public static string SurfaceHatchApproachFor(string? phase) => $"{SurfaceDir}surface_hatch_approach_{NormalizePhase(phase)}.png";
        public static string ShelterInteriorFor(string? phase) => $"{ShelterDir}shelter_interior_{NormalizePhase(phase)}.png";

        /// <summary>Swaps the texture on a backdrop previously added by <see cref="Apply"/>.</summary>
        public static void SetTexture(Control parent, string resPath)
        {
            if (parent == null) return;
            var rect = parent.GetNodeOrNull<TextureRect>("PlaceholderBackdrop");
            if (rect == null) return;
            var texture = TryLoad(resPath);
            if (texture != null) rect.Texture = texture;
        }

        /// <summary>Loads a backdrop texture, or null when absent (never throws).</summary>
        public static Texture2D? TryLoad(string resPath)
        {
            if (string.IsNullOrEmpty(resPath) || !ResourceLoader.Exists(resPath))
                return null;
            try
            {
                return ResourceLoader.Load<Texture2D>(resPath);
            }
            catch (Exception ex)
            {
                GD.PushWarning($"[BackdropArt] backdrop load failed: {resPath}: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Adds a cover-fit backdrop plus a dim overlay so foreground UI stays
        /// readable. Call this before adding panel content (default), or with
        /// <paramref name="insertBehind"/> for scene-backed panels that already
        /// have children.
        /// </summary>
        public static void Apply(Control parent, string resPath, float dimAlpha, bool insertBehind = false)
        {
            if (parent == null) return;

            var texture = TryLoad(resPath);
            if (texture != null)
            {
                var rect = new TextureRect
                {
                    Name = "PlaceholderBackdrop",
                    Texture = texture,
                    ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize,
                    StretchMode = TextureRect.StretchModeEnum.KeepAspectCovered,
                    MouseFilter = Control.MouseFilterEnum.Ignore
                };
                rect.SetAnchorsPreset(Control.LayoutPreset.FullRect);
                parent.AddChild(rect);
                if (insertBehind) parent.MoveChild(rect, 0);
            }

            var dim = new ColorRect
            {
                Name = "PlaceholderBackdropDim",
                Color = new Color(0.04f, 0.05f, 0.06f, Math.Clamp(dimAlpha, 0f, 1f)),
                MouseFilter = Control.MouseFilterEnum.Ignore
            };
            dim.SetAnchorsPreset(Control.LayoutPreset.FullRect);
            parent.AddChild(dim);
            if (insertBehind)
                parent.MoveChild(dim, texture != null ? 1 : 0);
        }
    }
}
