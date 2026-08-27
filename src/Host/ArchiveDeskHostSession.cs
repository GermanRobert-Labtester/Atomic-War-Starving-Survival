using System;
using System.Collections.Generic;
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Journal;
using Ashfall.Core.Save;

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

        public override void Save()
        {
            if (!IsDirty) return;
            ArchiveDeskSaveStore.TrySave(System.CaptureState());
            base.Save();
        }
    }

    /// <summary>
    /// Archive desk save persistence — thin façade over the Core
    /// SaveStore&lt;T&gt; service (via SaveStoreHub, codec flavor). This
    /// shelter-batch section ships the legacy
    /// <c>{ SchemaVersion, State, Checksum }</c> envelope, preserved
    /// byte-for-byte by the Core <see cref="SchemaVersionedEnvelope{T}"/>
    /// adapter (presence-only checksum, legacy bare-state fallback); path
    /// resolution, atomic write, and error handling live in the service.
    /// </summary>
    public static class ArchiveDeskSaveStore
    {
        public const string FileName = "archive_desk_save.json";
        public const string SectionName = "archive_desk";

        private static readonly SaveStore<ArchiveDeskState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(ArchiveDeskSaveStore),
            SchemaVersionedEnvelope<ArchiveDeskState>.Encode,
            SchemaVersionedEnvelope<ArchiveDeskState>.Decode);

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        public static bool TrySave(ArchiveDeskState state) => s_store.TrySave(state);

        public static ArchiveDeskState? TryLoad() => s_store.TryLoad();

        /// <summary>Capture the exact persisted bytes for the campaign envelope without writing to disk.</summary>
        public static string TryCapturePersisted(ArchiveDeskState state) => s_store.CapturePersisted(state);

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(ArchiveDeskState state) => s_store.CaptureBare(state);

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static ArchiveDeskState? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(ArchiveDeskState state) => s_store.CaptureBare(state);

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static ArchiveDeskState? TryRestore(string json) => s_store.RestoreBare(json);
    }
}
