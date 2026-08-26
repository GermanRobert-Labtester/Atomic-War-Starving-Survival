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
        private static readonly FileSystemIO s_files = new FileSystemIO();
        private static readonly SystemTextJsonSerializer s_json = new SystemTextJsonSerializer();

        public static string SavePath =>
            Path.Combine(ProjectSettings.GlobalizePath("user://"), FileName);
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
