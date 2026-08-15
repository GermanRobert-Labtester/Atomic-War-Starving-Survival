using Godot;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// ASHFALL — Godot-aware wrapper around the engine-agnostic
    /// <see cref="Ashfall.Core.UI.FactionIconCatalog"/>. Translates its
    /// `Assets/UI/Icons/...` paths into `res://...` Godot resource URIs
    /// and returns a <see cref="Texture2D"/>. Falls back to the
    /// canonical unknown emblem if neither the mapped nor the fallback
    /// PNG exists on disk.
    /// </summary>
    public static class FactionIconLoader
    {
        /// <summary>
        /// Resolve `factionId` to a Godot-loadable Texture2D (or null if
        /// neither the mapped nor fallback file exists). Caches by id.
        /// </summary>
        public static Texture2D? LoadFor(string factionId)
        {
            string path = Ashfall.Core.UI.FactionIconCatalog.Resolve(factionId);
            if (string.IsNullOrEmpty(path)) return null;

            string resPath = path.StartsWith("res://") ? path : ("res://" + path);
            var texture = AtomicWar.GodotApp.UI.AshfallUiHelpers.TryLoadTexture(resPath);
            if (texture != null) return texture;

            string fallbackPath = Ashfall.Core.UI.FactionIconCatalog.FallbackIconPath;
            if (fallbackPath != path)
            {
                string fallbackRes = fallbackPath.StartsWith("res://") ? fallbackPath : ("res://" + fallbackPath);
                texture = AtomicWar.GodotApp.UI.AshfallUiHelpers.TryLoadTexture(fallbackRes);
                if (texture != null) return texture;
            }
            return null;
        }

        /// <summary>
        /// Friendly name for diagnostics: returns "id → path" or
        /// "id → FALLBACK" so a developer can spot a missing emblem in
        /// logs at a glance.
        /// </summary>
        public static string Describe(string factionId)
        {
            if (string.IsNullOrEmpty(factionId)) return "<empty> → FALLBACK";
            var mapped = Ashfall.Core.UI.FactionIconCatalog.Resolve(factionId);
            if (!Ashfall.Core.UI.FactionIconCatalog.HasExplicitMapping(factionId))
                return factionId + " → FALLBACK";
            return factionId + " → " + mapped;
        }
    }
}
