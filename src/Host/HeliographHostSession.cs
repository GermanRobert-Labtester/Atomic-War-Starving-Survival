using System;
using Ashfall.Core;
using Ashfall.Core.Save;
using Ashfall.Core.Radio;

namespace AtomicWar.GodotApp
{
    /// <summary>Thin Godot adapter for the Core heliograph authority.</summary>
    public sealed class HeliographHostSession : HostSessionBase
    {
        public HeliographSystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;

        public HeliographHostSession(HeliographSystem system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
            System.OnMessageDelivered += message =>
            {
                LastEvent = $"Heliograph message delivered: {message.payload_key}.";
                RaiseStateChanged();
            };
            System.OnMessageBlocked += message =>
            {
                LastEvent = $"Heliograph message blocked: {message.block_reason}.";
                RaiseStateChanged();
            };
            System.OnStateChanged += () => { RaiseStateChanged(); };
        }

        public ActionResult Transmit(
            string messageId,
            string originStationId,
            string targetStationId,
            string payloadKey,
            int day,
            string revealLocationId = "",
            string distressSignalId = "")
            => System.Transmit(messageId, originStationId, targetStationId, payloadKey, day, revealLocationId, distressSignalId);

        public override void Save()
        {
            if (!IsDirty) return;
            if (HeliographSaveStore.TrySave(System.CaptureState()))
                base.Save();
        }
    }

    public static class HeliographSaveStore
    {
        public const string FileName = "heliograph_save.json";
        public const string SectionName = "heliograph";

        private static readonly SaveStore<HeliographState> s_store =
            SaveStoreHub.Checksummed<HeliographState>(FileName, nameof(HeliographSaveStore));

        public static bool TrySave(HeliographState state) => s_store.TrySave(state);
        public static HeliographState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(HeliographState state) => s_store.CapturePersisted(state);
        public static HeliographState? TryRestore(string json) => s_store.RestoreBare(json);
    }
}
