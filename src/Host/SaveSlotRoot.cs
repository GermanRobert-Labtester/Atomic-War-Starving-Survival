using System;
using System.IO;
using Godot;
using Ashfall.Core;

namespace AtomicWar.GodotApp;

/// <summary>
/// Host-side slot root router. When a slot root is set, all participating
/// save stores resolve their paths under that root instead of the global user://
/// directory. This lets the Godot host support multiple isolated campaigns
/// and headless runs without changing each store's capture/restore logic.
/// </summary>
public static class SaveSlotRoot
{
    private static string? s_currentRoot;

    /// <summary>
    /// Current slot root, or null if stores should use the default user:// path.
    /// Also checks ASHFALL_USER_DIR if set and no programmatic override is active.
    /// </summary>
    public static string? CurrentRoot
    {
        get => s_currentRoot ?? System.Environment.GetEnvironmentVariable("ASHFALL_USER_DIR");
        set => s_currentRoot = value;
    }

    /// <summary>
    /// Configure an explicit user data directory for isolated headless runs.
    /// </summary>
    public static void ConfigureUserDataDirectory(string? userDir)
    {
        if (string.IsNullOrWhiteSpace(userDir))
        {
            s_currentRoot = null;
            return;
        }

        try
        {
            Directory.CreateDirectory(userDir);
            s_currentRoot = userDir;
        }
        catch (Exception ex)
        {
            GD.PrintErr($"[SaveSlotRoot] Failed to create user directory '{userDir}': {ex.Message}");
            s_currentRoot = null;
        }
    }

    /// <summary>
    /// Returns the effective base directory for user data (custom root or globalized user://).
    /// </summary>
    public static string ResolveBaseDirectory()
    {
        string? root = CurrentRoot;
        if (!string.IsNullOrEmpty(root))
            return root;
        return ProjectSettings.GlobalizePath("user://");
    }

    /// <summary>
    /// Resolve a save file path. If a slot root is active, the file is placed
    /// under that root. Otherwise the legacy global user:// path is used.
    /// </summary>
    public static string Resolve(string fileName)
    {
        return Path.Combine(ResolveBaseDirectory(), fileName);
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
