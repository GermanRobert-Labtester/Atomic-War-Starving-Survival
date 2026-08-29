using System;
using System.Collections.Generic;

namespace Ashfall.Core.UI
{
    /// <summary>
    /// Shared, engine-neutral paths for the first playable UI surfaces.
    ///
    /// Paths are project-relative rather than engine-specific so the active
    /// host and the standalone preview harness validate the same files.
    /// Keep the casing exact: the active host is commonly run on Linux.
    /// </summary>
    public static class UiAssetManifest
    {
        public const string TitleBackground = "assets/ui/Textures/Backgrounds/title_screen_bg.png";
        public const string InventoryBackground = "assets/ui/Textures/Backgrounds/inventory_bg.png";
        public const string MedicalBackground = "assets/ui/Textures/Backgrounds/medical_bg.png";
        public const string GameOverBackground = "assets/ui/Textures/Backgrounds/game_over_bg.png";

        public const string PanelBackground = "assets/ui/Textures/panel_bg_9slice.png";
        public const string HeaderBar = "assets/ui/Textures/header_bar_9slice.png";

        public static IReadOnlyList<string> MainMenuBackgrounds { get; } = new[]
        {
            TitleBackground,
            InventoryBackground,
            MedicalBackground
        };

        public static IReadOnlyList<string> GameOverBackgrounds { get; } = new[]
        {
            InventoryBackground,
            MedicalBackground
        };

        /// <summary>
        /// All files needed by the menu/HUD/game-over boot preview.
        /// </summary>
        public static IEnumerable<string> RequiredPreviewTextures()
        {
            foreach (string path in MainMenuBackgrounds)
                yield return path;

            foreach (string path in GameOverBackgrounds)
                if (!Contains(MainMenuBackgrounds, path))
                    yield return path;

            yield return PanelBackground;
            yield return HeaderBar;
        }

        private static bool Contains(IReadOnlyList<string> paths, string value)
        {
            for (int i = 0; i < paths.Count; i++)
                if (string.Equals(paths[i], value, StringComparison.Ordinal))
                    return true;

            return false;
        }
    }
}
