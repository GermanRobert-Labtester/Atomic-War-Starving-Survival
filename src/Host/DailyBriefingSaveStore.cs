// ============================================================================
// Save Store : DailyBriefingSaveStore
// Core State : Ashfall.Core.DailyBriefingSave
// Host Caller: Main.Campaign / DailyBriefingHostSession
// Purpose    : Daily morning briefings, priority bulletins, and broadcast log archives
// ============================================================================
using System;
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Campaign;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Persists <see cref="DailyBriefingSave"/> as JSON under
    /// <c>user://daily_briefing_save.json</c> using the core
    /// <see cref="IFileIO"/> / <see cref="SystemTextJsonSerializer"/> ports.
    /// Shape and validation live in
    /// <see cref="Ashfall.Core.Campaign.DailyBriefingSaveCodec"/>. This type
    /// only picks the Godot path and the log, mirroring the other expansion
    /// save stores.
    /// </summary>
    public static class DailyBriefingSaveStore
    {
        public const string FileName = "daily_briefing_save.json";
        public const string SectionName = "daily_briefing";
    /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
    public static string TryCaptureDirect(DailyBriefingSave state)
    {
        return TryCapture(state);
    }

    /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
    public static DailyBriefingSave? TryRestoreDirect(string json)
    {
        return TryRestore(json);
    }

    /// <summary>Capture state to JSON without writing to disk.</summary>
    public static string TryCapture(DailyBriefingSave state)
    {
        try
        {
            if (state == null) return string.Empty;
            return new SystemTextJsonSerializer().Serialize(state);
        }
        catch (Exception e)
        {
            GD.PrintErr("[DailyBriefingSaveStore] capture failed: " + e.Message);
            return string.Empty;
        }
    }

    /// <summary>Restore state from JSON without reading from disk.</summary>
    public static DailyBriefingSave? TryRestore(string json)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(json)) return null;
            return new SystemTextJsonSerializer().Deserialize<DailyBriefingSave>(json);
        }
        catch (Exception e)
        {
            GD.PrintErr("[DailyBriefingSaveStore] restore failed: " + e.Message);
            return null;
        }
    }


        private static readonly IFileIO s_files = new FileSystemIO();
        private static readonly IJsonSerializer s_json = new SystemTextJsonSerializer();
        private static readonly ILog s_log = new GodotLog();

        public static string SavePath =>
            SaveSlotRoot.Resolve(FileName);

        public static bool Exists => s_files.FileExists(SavePath);

        public static bool TrySave(DailyBriefingSave save)
        {
            if (save == null) return false;
            try
            {
                s_files.WriteAllText(SavePath,
                    DailyBriefingSaveCodec.EncodeToString(save, s_json));
                return true;
            }
            catch (Exception e)
            {
                s_log.Error("[DailyBriefingSaveStore] save failed: " + e.Message);
                return false;
            }
        }

        public static DailyBriefingSave? TryLoad()
        {
            try
            {
                if (!s_files.FileExists(SavePath)) return null;
                return DailyBriefingSaveCodec.Decode(s_files.ReadAllText(SavePath), s_json);
            }
            catch (Exception e)
            {
                s_log.Error("[DailyBriefingSaveStore] load failed: " + e.Message);
                return null;
            }
        }
    }
}
