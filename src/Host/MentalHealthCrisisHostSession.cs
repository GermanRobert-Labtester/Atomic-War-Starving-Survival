using System;
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Survivors;
using Ashfall.Core.Medical;

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

    [Serializable]
    public sealed class MentalHealthCrisisHostSave
    {
        public string SchemaVersion { get; set; } = "1.0";
        public MentalHealthState State { get; set; }
        public string Checksum { get; set; } = string.Empty;
    }

    public static class MentalHealthCrisisSaveStore
    {
        public const string FileName = "mental_health_crisis_save.json";
        public const string SectionName = "mental_health_crisis";

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(MentalHealthState state)
        {
            return TryCapture(state);
        }

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static MentalHealthState? TryRestoreDirect(string json)
        {
            return TryRestore(json);
        }

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(MentalHealthState state)
        {
            try
            {
                if (state == null) return string.Empty;
                return s_json.Serialize(state);
            }
            catch (Exception e)
            {
                GD.PrintErr("[MentalHealthCrisisSaveStore] capture failed: " + e.Message);
                return string.Empty;
            }
        }

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static MentalHealthState? TryRestore(string json)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(json)) return null;
                return s_json.Deserialize<MentalHealthState>(json);
            }
            catch (Exception e)
            {
                GD.PrintErr("[MentalHealthCrisisSaveStore] restore failed: " + e.Message);
                return null;
            }
        }

        private static readonly FileSystemIO s_files = new FileSystemIO();
        private static readonly SystemTextJsonSerializer s_json = new SystemTextJsonSerializer();

        public static string SavePath => SaveSlotRoot.Resolve(FileName);
        public static bool Exists => s_files.FileExists(SavePath);

        public static bool TrySave(MentalHealthState state)
        {
            try
            {
                if (state == null) return false;
                var envelope = new MentalHealthCrisisHostSave { State = state };
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
                GD.PrintErr("[MentalHealth] save failed: " + e.Message);
                return false;
            }
        }

        public static MentalHealthState? TryLoad()
        {
            try
            {
                string path = SavePath;
                if (!s_files.FileExists(path)) return null;
                string raw = s_files.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(raw)) return null;
                var envelope = s_json.Deserialize<MentalHealthCrisisHostSave>(raw);
                if (envelope != null && envelope.State != null)
                {
                    if (string.IsNullOrEmpty(envelope.Checksum)) return null;
                    return envelope.State;
                }
                return s_json.Deserialize<MentalHealthState>(raw);
            }
            catch (Exception e)
            {
                GD.PrintErr("[MentalHealth] load failed: " + e.Message);
                return null;
            }
        }
    }
}
