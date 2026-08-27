using System;
using System.Collections.Generic;
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Journal;
using Ashfall.Core.Survivors;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Host session for LibraryStudySystem.
    /// Wraps the Core library pipeline (LoadCatalog → StartStudy → TickDay)
    /// and forwards StateChanged for host wiring. Engine-agnostic Core authority.
    /// </summary>
    public sealed class LibraryStudyHostSession
    : HostSessionBase{
        public LibraryStudySystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;
        public LibraryStudyHostSession(
            LibraryStudySystem system,
            SkillProgressionSystem skills,
            ResearchSystem research,
            JournalSystem journal,
            DutyRosterSystem roster)
        {
            System = system
                ?? new LibraryStudySystem(skills, research, journal, roster, new GodotLog());

            System.OnJobCompleted += _ =>
            {
                LastEvent = "Study completed";
                RaiseStateChanged();
            };
            System.OnLibraryChanged += () => RaiseStateChanged();
        }

        public void LoadCatalog(List<ManualDefinition> manuals)
        {
            System.LoadCatalog(manuals);
            LastEvent = $"Library catalog loaded: {manuals.Count} manuals";
            RaiseStateChanged();
        }

        /// <summary>Load the library_manuals.json catalog into the Core system (the authority).</summary>
        public void LoadCatalog(string dataDir)
        {
            if (string.IsNullOrEmpty(dataDir)) return;
            var fileIO = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();
            int count = LibraryManualCatalogLoader.LoadAndRegister(System, dataDir, fileIO, serializer);
            if (count > 0)
            {
                LastEvent = $"Library manual catalog loaded: {count} manuals";
                RaiseStateChanged();
            }
        }

        public ActionResult StartStudy(string manualId, string readerId)
        {
            var res = System.StartStudy(manualId, readerId);
            if (res.IsSuccess)
            {
                LastEvent = $"Study started: {manualId} by {readerId}";
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
            LibraryStudySaveStore.TrySave(System.CaptureState());
            base.Save();
        }
    }

    /// <summary>
    /// LibraryStudySaveStore save persistence — thin façade over the Core
    /// SaveStore&lt;T&gt; service (via SaveStoreHub, codec flavor). This
    /// shelter-batch section ships the legacy
    /// &lt;c&gt;{ SchemaVersion, State, Checksum }&lt;/c&gt; envelope, preserved
    /// byte-for-byte by the Core &lt;see cref="SchemaVersionedEnvelope{T}"/&gt;
    /// adapter (presence-only checksum, legacy bare-state fallback); path
    /// resolution, atomic write, and error handling live in the service.
    /// </summary>
    public static class LibraryStudySaveStore
    {
        public const string FileName = "library_study_save.json";
        public const string SectionName = "library_study";

        private static readonly SaveStore<LibraryStudyState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(LibraryStudySaveStore),
            SchemaVersionedEnvelope<LibraryStudyState>.Encode,
            SchemaVersionedEnvelope<LibraryStudyState>.Decode);

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        public static bool TrySave(LibraryStudyState state) => s_store.TrySave(state);

        public static LibraryStudyState? TryLoad() => s_store.TryLoad();

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(LibraryStudyState state) => s_store.CaptureBare(state);

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static LibraryStudyState? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(LibraryStudyState state) => s_store.CaptureBare(state);

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static LibraryStudyState? TryRestore(string json) => s_store.RestoreBare(json);
    }
}
