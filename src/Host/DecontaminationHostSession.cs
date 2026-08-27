using System;
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Radiation;
using Ashfall.Core.StartingLevel;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Host session for DecontaminationSystem.
    /// Wraps the Core decontamination pipeline (Enqueue → ProcessQueue → CompleteCycle)
    /// and forwards StateChanged for host wiring. Engine-agnostic Core authority.
    /// </summary>
    public sealed class DecontaminationHostSession
    : HostSessionBase{
        public DecontaminationSystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;
        public DecontaminationHostSession(
            DecontaminationSystem system,
            RadiationSystem radiation,
            Inventory inventory,
            AirlockSecuritySystem airlock,
            StartingLevelSystem startingLevel)
        {
            System = system
                ?? new DecontaminationSystem(new SeededRng(1986), radiation, inventory, airlock, startingLevel, new GodotLog());

            System.OnCaseCompleted += _ => RaiseStateChanged();
            System.OnDeconChanged += () => RaiseStateChanged();
        }

        public ActionResult Enqueue(string survivorId, string gearId, float surfaceContamination)
        {
            var res = System.Enqueue(survivorId, gearId, surfaceContamination);
            if (res.IsSuccess)
            {
                LastEvent = $"Decon case queued: {survivorId} ({gearId})";
                RaiseStateChanged();
            }
            return res;
        }

        public ActionResult ProcessQueue()
        {
            var res = System.ProcessQueue();
            if (res.IsSuccess)
            {
                LastEvent = "Decon queue processed";
                RaiseStateChanged();
            }
            return res;
        }

        public ActionResult CompleteCycle(bool safeRelease)
        {
            var res = System.CompleteCycle(safeRelease);
            if (res.IsSuccess)
            {
                LastEvent = safeRelease ? "Decon cycle completed (safe release)" : "Decon cycle completed (unsafe release)";
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
            DecontaminationSaveStore.TrySave(System.CaptureState());
            base.Save();
        }
    }

    /// <summary>
    /// DecontaminationSaveStore save persistence — thin façade over the Core
    /// SaveStore&lt;T&gt; service (via SaveStoreHub, codec flavor). This
    /// shelter-batch section ships the legacy
    /// &lt;c&gt;{ SchemaVersion, State, Checksum }&lt;/c&gt; envelope, preserved
    /// byte-for-byte by the Core &lt;see cref="SchemaVersionedEnvelope{T}"/&gt;
    /// adapter (presence-only checksum, legacy bare-state fallback); path
    /// resolution, atomic write, and error handling live in the service.
    /// </summary>
    public static class DecontaminationSaveStore
    {
        public const string FileName = "decontamination_save.json";
        public const string SectionName = "decontamination";

        private static readonly SaveStore<DecontaminationState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(DecontaminationSaveStore),
            SchemaVersionedEnvelope<DecontaminationState>.Encode,
            SchemaVersionedEnvelope<DecontaminationState>.Decode);

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        public static bool TrySave(DecontaminationState state) => s_store.TrySave(state);

        public static DecontaminationState? TryLoad() => s_store.TryLoad();

        /// <summary>Capture the exact persisted bytes for the campaign envelope without writing to disk.</summary>
        public static string TryCapturePersisted(DecontaminationState state) => s_store.CapturePersisted(state);

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(DecontaminationState state) => s_store.CaptureBare(state);

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static DecontaminationState? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(DecontaminationState state) => s_store.CaptureBare(state);

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static DecontaminationState? TryRestore(string json) => s_store.RestoreBare(json);
    }
}
