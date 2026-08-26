using System;
using System.IO;
using Godot;
using Ashfall.Core;

namespace AtomicWar.GodotApp;

/// <summary>
/// Host-side slot root router. When a slot root is set, all participating
/// save stores resolve their paths under that root instead of the global user://
/// directory. This lets the Godot host support multiple isolated campaigns
/// without changing each store's capture/restore logic.
/// </summary>
public static class SaveSlotRoot
{
    /// <summary>
    /// Current slot root, or null if stores should use the legacy global user:// path.
    /// </summary>
    public static string? CurrentRoot { get; set; }

    /// <summary>
    /// Resolve a save file path. If a slot root is active, the file is placed
    /// under that root. Otherwise the legacy global user:// path is used.
    /// </summary>
    public static string Resolve(string fileName)
    {
        if (!string.IsNullOrEmpty(CurrentRoot))
            return Path.Combine(CurrentRoot, fileName);
        return Path.Combine(ProjectSettings.GlobalizePath("user://"), fileName);
    }

    /// <summary>
    /// Resolve a save file path under an explicit root, ignoring CurrentRoot.
    /// Used by the save slot service when building aggregate paths.
    /// </summary>
    public static string ResolveUnder(string root, string fileName)
    {
        if (string.IsNullOrEmpty(root))
            return Path.Combine(ProjectSettings.GlobalizePath("user://"), fileName);
        return Path.Combine(root, fileName);
    }
}
