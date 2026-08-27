using System;
#pragma warning disable CS8618
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Ashfall.Core.World;
using Ashfall.Core.YearOfAsh;
using Ashfall.Core.Save;

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

    /// <summary>
    /// SumpFloodingSaveStore save persistence — thin façade over the Core
    /// SaveStore&lt;T&gt; service (via SaveStoreHub, codec flavor). This
    /// shelter-batch section ships the legacy
    /// &lt;c&gt;{ SchemaVersion, State, Checksum }&lt;/c&gt; envelope, preserved
    /// byte-for-byte by the Core &lt;see cref="SchemaVersionedEnvelope{T}"/&gt;
    /// adapter (presence-only checksum, legacy bare-state fallback); path
    /// resolution, atomic write, and error handling live in the service.
    /// </summary>
    public static class SumpFloodingSaveStore
    {
        public const string FileName = "sump_flooding_save.json";
        public const string SectionName = "sump_flooding";

        private static readonly SaveStore<SumpFloodingState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(SumpFloodingSaveStore),
            SchemaVersionedEnvelope<SumpFloodingState>.Encode,
            SchemaVersionedEnvelope<SumpFloodingState>.Decode);

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        public static bool TrySave(SumpFloodingState state) => s_store.TrySave(state);

        public static SumpFloodingState? TryLoad() => s_store.TryLoad();

        /// <summary>Capture the exact persisted bytes for the campaign envelope without writing to disk.</summary>
        public static string TryCapturePersisted(SumpFloodingState state) => s_store.CapturePersisted(state);

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(SumpFloodingState state) => s_store.CaptureBare(state);

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static SumpFloodingState? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(SumpFloodingState state) => s_store.CaptureBare(state);

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static SumpFloodingState? TryRestore(string json) => s_store.RestoreBare(json);
    }
}
