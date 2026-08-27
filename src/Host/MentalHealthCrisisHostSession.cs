using System;
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Survivors;
using Ashfall.Core.Medical;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Host session for MentalHealthCrisisSystem.
    /// Wraps the Core crisis pipeline (TriggerCrisis → BeginTreatment → TickDay)
    /// and forwards StateChanged for host wiring. Engine-agnostic Core authority.
    /// </summary>
    public sealed class MentalHealthCrisisHostSession
    : HostSessionBase{
        public MentalHealthCrisisSystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;
        public MentalHealthCrisisHostSession(
            MentalHealthCrisisSystem system,
            NeedsSystem needs,
            MedicalWardSystem ward,
            ChemicalDependencySystem chemical,
            DutyRosterSystem roster)
        {
            System = system
                ?? new MentalHealthCrisisSystem(new SeededRng(1986), needs, ward, chemical, roster, new GodotLog());

            System.OnCrisisResolved += _ => RaiseStateChanged();
            System.OnMentalHealthChanged += () => RaiseStateChanged();
        }

        public ActionResult TriggerCrisis(string survivorId, float stressInput, CrisisProfile profile)
        {
            var res = System.TriggerCrisis(survivorId, stressInput, profile);
            if (res.IsSuccess)
            {
                LastEvent = $"Crisis triggered: {survivorId}";
                RaiseStateChanged();
            }
            return res;
        }

        public ActionResult BeginTreatment(string caseId, string caregiverId, string intervention)
        {
            var res = System.BeginTreatment(caseId, caregiverId, intervention);
            if (res.IsSuccess)
            {
                LastEvent = $"Treatment begun: {caseId} by {caregiverId}";
                RaiseStateChanged();
            }
            return res;
        }

        public bool IsInCrisis(string survivorId) => System.IsInCrisis(survivorId);
        public bool IsEligibleForWork(string survivorId) => System.IsEligibleForWork(survivorId);

        public void TickDay(int day)
        {
            System.TickDay(day);
            RaiseStateChanged();
        }

        public override void Save()
        {
            if (!IsDirty) return;
            MentalHealthCrisisSaveStore.TrySave(System.CaptureState());
            base.Save();
        }
    }

    /// <summary>
    /// MentalHealthCrisisSaveStore save persistence — thin façade over the Core
    /// SaveStore&lt;T&gt; service (via SaveStoreHub, codec flavor). This
    /// shelter-batch section ships the legacy
    /// &lt;c&gt;{ SchemaVersion, State, Checksum }&lt;/c&gt; envelope, preserved
    /// byte-for-byte by the Core &lt;see cref="SchemaVersionedEnvelope{T}"/&gt;
    /// adapter (presence-only checksum, legacy bare-state fallback); path
    /// resolution, atomic write, and error handling live in the service.
    /// </summary>
    public static class MentalHealthCrisisSaveStore
    {
        public const string FileName = "mental_health_crisis_save.json";
        public const string SectionName = "mental_health_crisis";

        private static readonly SaveStore<MentalHealthState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(MentalHealthCrisisSaveStore),
            SchemaVersionedEnvelope<MentalHealthState>.Encode,
            SchemaVersionedEnvelope<MentalHealthState>.Decode);

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        public static bool TrySave(MentalHealthState state) => s_store.TrySave(state);

        public static MentalHealthState? TryLoad() => s_store.TryLoad();

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(MentalHealthState state) => s_store.CaptureBare(state);

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static MentalHealthState? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(MentalHealthState state) => s_store.CaptureBare(state);

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static MentalHealthState? TryRestore(string json) => s_store.RestoreBare(json);
    }
}
