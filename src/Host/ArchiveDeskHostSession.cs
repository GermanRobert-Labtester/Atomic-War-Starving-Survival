using System;
using System.Collections.Generic;
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Journal;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Host session for ArchiveDeskSystem.
    /// Wraps the Core archive pipeline (LoadInkCatalog → QueueTranscription → TickDay)
    /// and forwards StateChanged for host wiring. Engine-agnostic Core authority.
    /// </summary>
    public sealed class ArchiveDeskHostSession
    : HostSessionBase{
        public ArchiveDeskSystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;
        public ArchiveDeskHostSession(
            ArchiveDeskSystem system,
            JournalSystem journal,
            KnowledgeBase knowledge,
            Ashfall.Core.Inventory.Inventory inventory,
            DutyRosterSystem roster)
        {
            System = system
                ?? new ArchiveDeskSystem(journal, knowledge, inventory, roster, new GodotLog());

            System.OnJobCompleted += job =>
            {
                LastEvent = $"Transcription completed: {job.evidenceId}";
                RaiseStateChanged();
            };
            System.OnArchiveChanged += () => RaiseStateChanged();
        }

        public void LoadInkCatalog(List<InkMaterialDefinition> inks)
        {
            System.LoadInkCatalog(inks);
            LastEvent = $"Ink catalog loaded: {inks.Count} inks";
            RaiseStateChanged();
        }

        /// <summary>Load the archive_inks.json catalog into the Core system (the authority).</summary>
        public void LoadInkCatalog(string dataDir)
        {
            if (string.IsNullOrEmpty(dataDir)) return;
            var fileIO = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();
            int count = ArchiveInkCatalogLoader.LoadAndRegister(System, dataDir, fileIO, serializer);
            if (count > 0)
            {
                LastEvent = $"Ink catalog loaded: {count} inks";
                RaiseStateChanged();
            }
        }

        public ActionResult QueueTranscription(string evidenceId, string archivistId, string inkId)
        {
            var res = System.QueueTranscription(evidenceId, archivistId, inkId);
            if (res.IsSuccess)
            {
                LastEvent = $"Transcription queued: {evidenceId} by {archivistId}";
                RaiseStateChanged();
            }
            return res;
        }

        public ActionResult CancelJob(string jobId)
        {
            var res = System.CancelJob(jobId);
            if (res.IsSuccess)
            {
                LastEvent = $"Transcription cancelled: {jobId}";
                RaiseStateChanged();
            }
            return res;
        }

        public void TickDay(int day)
        {
            System.TickDay(day);
            RaiseStateChanged();
        }
    }

    [Serializable]
    public sealed class ArchiveDeskHostSave
    {
        public string SchemaVersion { get; set; } = "1.0";
        public ArchiveDeskState State { get; set; }
        public string Checksum { get; set; } = string.Empty;
    }

    public static class ArchiveDeskSaveStore
    {
        public const string FileName = "archive_desk_save.json";
        private static readonly FileSystemIO s_files = new FileSystemIO();
        private static readonly SystemTextJsonSerializer s_json = new SystemTextJsonSerializer();

        public static string SavePath =>
            Path.Combine(ProjectSettings.GlobalizePath("user://"), FileName);
        public static bool Exists => s_files.FileExists(SavePath);

        public static bool TrySave(ArchiveDeskState state)
        {
            try
            {
                if (state == null) return false;
                var envelope = new ArchiveDeskHostSave { State = state };
                envelope.Checksum = SaveChecksum.Compute(envelope);
                string path = SavePath;
                string? dir = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                File.WriteAllText(path, s_json.Serialize(envelope));
                return true;
            }
            catch (Exception e)
            {
                GD.PrintErr("[Archive] save failed: " + e.Message);
                return false;
            }
        }

        public static ArchiveDeskState? TryLoad()
        {
            try
            {
                string path = SavePath;
                if (!s_files.FileExists(path)) return null;
                string raw = s_files.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(raw)) return null;
                var envelope = s_json.Deserialize<ArchiveDeskHostSave>(raw);
                if (envelope != null && envelope.State != null)
                {
                    if (string.IsNullOrEmpty(envelope.Checksum)) return null;
                    return envelope.State;
                }
                return s_json.Deserialize<ArchiveDeskState>(raw);
            }
            catch (Exception e)
            {
                GD.PrintErr("[Archive] load failed: " + e.Message);
                return null;
            }
        }
    }
}
