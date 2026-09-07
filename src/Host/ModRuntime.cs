using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Mods;

namespace AtomicWar.GodotApp.Host
{
    /// <summary>
    /// Host boundary for JSON mods. Core validates and layers content; this
    /// adapter materializes one private read-only-at-runtime data directory so
    /// existing catalog loaders all observe the same effective authority.
    /// </summary>
    public static class ModRuntime
    {
        private static readonly List<string> s_diagnostics = new();

        public static IReadOnlyList<string> Diagnostics => s_diagnostics;
        public static IReadOnlyList<string> AcceptedModIds { get; private set; } = Array.Empty<string>();
        public static IReadOnlyList<string> RejectedModIds { get; private set; } = Array.Empty<string>();

        public static string Prepare(string baseDataDirectory)
        {
            s_diagnostics.Clear();
            AcceptedModIds = Array.Empty<string>();
            RejectedModIds = Array.Empty<string>();

            if (string.IsNullOrWhiteSpace(baseDataDirectory))
                return baseDataDirectory;

            var settings = AtomicWar.GodotApp.Settings.UserSettingsStore.Current;
            if (!settings.ModsEnabled)
                return baseDataDirectory;

            string modsDirectory = System.Environment.GetEnvironmentVariable("ASHFALL_MODS_DIR")
                ?? Path.Combine(SaveSlotRoot.ResolveBaseDirectory(), "mods");
            IFileIO files = CatalogPath.CreateFileIOForDataDir(baseDataDirectory);
            var result = new JsonModLayering().Build(
                baseDataDirectory,
                modsDirectory,
                files,
                new SystemTextJsonSerializer(),
                settings.EnabledMods.Count == 0
                    ? null
                    : new HashSet<string>(settings.EnabledMods, StringComparer.Ordinal));

            AcceptedModIds = result.AcceptedModIds;
            RejectedModIds = result.RejectedModIds;
            foreach (ModDiagnostic diagnostic in result.Diagnostics)
            {
                string message = "[Mods] " + diagnostic;
                s_diagnostics.Add(message);
                GD.PrintErr(message);
            }

            if (result.CatalogOverlays.Count == 0)
                return baseDataDirectory;

            try
            {
                string stagingRoot = Path.Combine(SaveSlotRoot.ResolveBaseDirectory(), "mod_staging");
                string stagingData = Path.Combine(stagingRoot, "Data");
                if (Directory.Exists(stagingRoot))
                    Directory.Delete(stagingRoot, recursive: true);
                Directory.CreateDirectory(stagingData);

                foreach (string source in files.EnumerateFiles(baseDataDirectory, "*.json", SearchOption.TopDirectoryOnly)
                    .OrderBy(path => path, StringComparer.Ordinal))
                {
                    string fileName = Path.GetFileName(source);
                    string destination = Path.Combine(stagingData, fileName);
                    File.WriteAllText(destination, files.ReadAllText(source));
                }

                foreach (var overlay in result.CatalogOverlays.OrderBy(pair => pair.Key, StringComparer.Ordinal))
                    File.WriteAllText(Path.Combine(stagingData, overlay.Key), overlay.Value);

                GD.Print($"[Mods] Activated {AcceptedModIds.Count} mod(s): {string.Join(", ", AcceptedModIds)}");
                return stagingData;
            }
            catch (Exception ex)
            {
                string message = "[Mods] Staging failed; base catalogs remain active: " + ex.Message;
                s_diagnostics.Add(message);
                GD.PrintErr(message);
                return baseDataDirectory;
            }
        }
    }
}
