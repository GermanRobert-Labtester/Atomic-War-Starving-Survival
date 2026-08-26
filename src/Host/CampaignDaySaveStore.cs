using System;
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Campaign;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Persists <see cref="CampaignDaySave"/> as JSON under
    /// <c>user://campaign_day_save.json</c> using the core
    /// <see cref="IFileIO"/> / <see cref="SystemTextJsonSerializer"/> ports.
    /// Shape and validation live in
    /// <see cref="Ashfall.Core.Campaign.CampaignDaySaveCodec"/>.
    /// </summary>
    public static class CampaignDaySaveStore
    {
        public const string FileName = "campaign_day_save.json";
        public const string SectionName = "campaign_day";

        private static readonly IFileIO s_files = new FileSystemIO();
        private static readonly IJsonSerializer s_json = new SystemTextJsonSerializer();
        private static readonly ILog s_log = new GodotLog();

        public static string SavePath =>
            SaveSlotRoot.Resolve(FileName);

        public static bool Exists => s_files.FileExists(SavePath);

        public static string TryCapture(CampaignDaySave state)
        {
            try
            {
                if (state == null) return string.Empty;
                return CampaignDaySaveCodec.EncodeToString(state, s_json);
            }
            catch (Exception e)
            {
                GD.PrintErr("[CampaignDaySaveStore] capture failed: " + e.Message);
                return string.Empty;
            }
        }

        public static CampaignDaySave? TryRestore(string json)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(json)) return null;
                return CampaignDaySaveCodec.Decode(json, s_json);
            }
            catch (Exception e)
            {
                GD.PrintErr("[CampaignDaySaveStore] restore failed: " + e.Message);
                return null;
            }
        }

        public static bool TrySave(CampaignDaySave save)
        {
            if (save == null) return false;
            try
            {
                s_files.WriteAllText(SavePath, TryCapture(save));
                return true;
            }
            catch (Exception e)
            {
                s_log.Error("[CampaignDaySaveStore] save failed: " + e.Message);
                return false;
            }
        }

        public static CampaignDaySave? TryLoad()
        {
            try
            {
                if (!s_files.FileExists(SavePath)) return null;
                string json = s_files.ReadAllText(SavePath);
                return TryRestore(json);
            }
            catch (Exception e)
            {
                s_log.Error("[CampaignDaySaveStore] load failed: " + e.Message);
                return null;
            }
        }
    }
}
