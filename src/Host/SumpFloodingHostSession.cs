using System;
#pragma warning disable CS8618
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Ashfall.Core.World;
using Ashfall.Core.YearOfAsh;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Host session for SumpFloodingSystem.
    /// Owns sublevel nodes, sump pumps, float valve / sandbag mitigations, and
    /// weather-driven flooding. Engine-agnostic Core authority; this session
    /// only adapts the OnIncident / StateChanged surface for Godot wiring.
    /// </summary>
    public sealed class SumpFloodingHostSession
    : HostSessionBase{
        public SumpFloodingSystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;
        public SumpFloodingHostSession(
            SumpFloodingSystem system,
            WeatherSystem weather,
            PowerGridSystem powerGrid,
            YearOfAshDeepFreezeSystem deepFreeze)
        {
            System = system
                ?? new SumpFloodingSystem(new SeededRng(1986), weather, powerGrid, deepFreeze, new GodotLog());

            System.OnIncident += inc =>
            {
                LastEvent = $"[Sump] INCIDENT: {inc.kind} in {inc.nodeId} — {inc.description}";
                RaiseStateChanged();
            };

            System.OnFloodingChanged += () =>
            {
                RaiseStateChanged();
            };
        }

        public ActionResult AddNode(string nodeId, string displayName, float maxWaterLevelCm = 200f)
        {
            var res = System.AddNode(nodeId, displayName, maxWaterLevelCm);
            if (res.IsSuccess)
            {
                LastEvent = $"Sump node registered: {displayName} (cap {maxWaterLevelCm}cm)";
                RaiseStateChanged();
            }
            return res;
        }

        public ActionResult InstallPump(string nodeId)
        {
            var res = System.InstallPump(nodeId);
            if (res.IsSuccess)
            {
                LastEvent = $"Sump pump installed at node {nodeId}";
                RaiseStateChanged();
            }
            return res;
        }

        public ActionResult SetNodePower(string nodeId, bool powered)
        {
            var res = System.SetNodePower(nodeId, powered);
            if (res.IsSuccess)
            {
                LastEvent = $"Sump pump power set: node {nodeId} -> {(powered ? "ON" : "OFF")}";
                RaiseStateChanged();
            }
            return res;
        }

        public ActionResult AddMitigation(string nodeId, string mitigationType)
        {
            var res = System.AddMitigation(nodeId, mitigationType);
            if (res.IsSuccess)
            {
                LastEvent = $"Sump mitigation added: {mitigationType} on node {nodeId}";
                RaiseStateChanged();
            }
            return res;
        }

        public ActionResult DrainNode(string nodeId)
        {
            var res = System.DrainNode(nodeId);
            if (res.IsSuccess)
            {
                LastEvent = $"Sump node drained: {nodeId}";
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
            SumpFloodingSaveStore.TrySave(System.CaptureState());
            base.Save();
        }
    }

    [Serializable]
    public sealed class SumpFloodingHostSave
    {
        public string SchemaVersion { get; set; } = "1.0";
        public SumpFloodingState State { get; set; }
        public string Checksum { get; set; } = string.Empty;
    }

    public static class SumpFloodingSaveStore
    {
        public const string FileName = "sump_flooding_save.json";
        public const string SectionName = "sump_flooding";

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(SumpFloodingState state)
        {
            return TryCapture(state);
        }

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static SumpFloodingState? TryRestoreDirect(string json)
        {
            return TryRestore(json);
        }

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(SumpFloodingState state)
        {
            try
            {
                if (state == null) return string.Empty;
                return s_json.Serialize(state);
            }
            catch (Exception e)
            {
                GD.PrintErr("[SumpFloodingSaveStore] capture failed: " + e.Message);
                return string.Empty;
            }
        }

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static SumpFloodingState? TryRestore(string json)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(json)) return null;
                return s_json.Deserialize<SumpFloodingState>(json);
            }
            catch (Exception e)
            {
                GD.PrintErr("[SumpFloodingSaveStore] restore failed: " + e.Message);
                return null;
            }
        }

        private static readonly FileSystemIO s_files = new FileSystemIO();
        private static readonly SystemTextJsonSerializer s_json = new SystemTextJsonSerializer();

        public static string SavePath => SaveSlotRoot.Resolve(FileName);
        public static bool Exists => s_files.FileExists(SavePath);

        public static bool TrySave(SumpFloodingState state)
        {
            try
            {
                if (state == null) return false;
                var envelope = new SumpFloodingHostSave { State = state };
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
                GD.PrintErr("[Sump] save failed: " + e.Message);
                return false;
            }
        }

        public static SumpFloodingState? TryLoad()
        {
            try
            {
                string path = SavePath;
                if (!s_files.FileExists(path)) return null;
                string raw = s_files.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(raw)) return null;
                var envelope = s_json.Deserialize<SumpFloodingHostSave>(raw);
                if (envelope != null && envelope.State != null)
                {
                    if (string.IsNullOrEmpty(envelope.Checksum)) return null;
                    return envelope.State;
                }
                return s_json.Deserialize<SumpFloodingState>(raw);
            }
            catch (Exception e)
            {
                GD.PrintErr("[Sump] load failed: " + e.Message);
                return null;
            }
        }
    }
}
