using System;
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Radiation;
using Ashfall.Core.StartingLevel;

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

    [Serializable]
    public sealed class DecontaminationHostSave
    {
        public string SchemaVersion { get; set; } = "1.0";
        public DecontaminationState State { get; set; }
        public string Checksum { get; set; } = string.Empty;
    }

    public static class DecontaminationSaveStore
    {
        public const string FileName = "decontamination_save.json";
        public const string SectionName = "decontamination";

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(DecontaminationState state)
        {
            return TryCapture(state);
        }

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static DecontaminationState? TryRestoreDirect(string json)
        {
            return TryRestore(json);
        }

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(DecontaminationState state)
        {
            try
            {
                if (state == null) return string.Empty;
                return s_json.Serialize(state);
            }
            catch (Exception e)
            {
                GD.PrintErr("[DecontaminationSaveStore] capture failed: " + e.Message);
                return string.Empty;
            }
        }

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static DecontaminationState? TryRestore(string json)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(json)) return null;
                return s_json.Deserialize<DecontaminationState>(json);
            }
            catch (Exception e)
            {
                GD.PrintErr("[DecontaminationSaveStore] restore failed: " + e.Message);
                return null;
            }
        }

        private static readonly FileSystemIO s_files = new FileSystemIO();
        private static readonly SystemTextJsonSerializer s_json = new SystemTextJsonSerializer();

        public static string SavePath => SaveSlotRoot.Resolve(FileName);
        public static bool Exists => s_files.FileExists(SavePath);

        public static bool TrySave(DecontaminationState state)
        {
            try
            {
                if (state == null) return false;
                var envelope = new DecontaminationHostSave { State = state };
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
                GD.PrintErr("[Decon] save failed: " + e.Message);
                return false;
            }
        }

        public static DecontaminationState? TryLoad()
        {
            try
            {
                string path = SavePath;
                if (!s_files.FileExists(path)) return null;
                string raw = s_files.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(raw)) return null;
                var envelope = s_json.Deserialize<DecontaminationHostSave>(raw);
                if (envelope != null && envelope.State != null)
                {
                    if (string.IsNullOrEmpty(envelope.Checksum)) return null;
                    return envelope.State;
                }
                return s_json.Deserialize<DecontaminationState>(raw);
            }
            catch (Exception e)
            {
                GD.PrintErr("[Decon] load failed: " + e.Message);
                return null;
            }
        }
    }
}
