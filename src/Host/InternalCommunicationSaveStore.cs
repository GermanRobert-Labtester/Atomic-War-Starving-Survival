// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : InternalCommunicationSaveStore
// Core State : Ashfall.Core.Communication.InternalCommunicationState
// Host Caller: Main.InternalCommunication / InternalCommunicationHostSession
// Purpose    : Plan 211 — durable internal shelter notices, boards, intercom
//              announcements, and private-message state. This is deliberately
//              separate from the external antenna `communications` section.
// ============================================================================

using Ashfall.Core.Communication;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class InternalCommunicationSaveStore
    {
        public const string FileName = "internal_communication_save.json";
        public const string SectionName = "internal_communication";

        private static readonly SaveStore<InternalCommunicationState> s_store =
            SaveStoreHub.Checksummed<InternalCommunicationState>(FileName, nameof(InternalCommunicationSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(InternalCommunicationState state) => s_store.TrySave(state);
        public static InternalCommunicationState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(InternalCommunicationState state) => s_store.CapturePersisted(state);
        public static InternalCommunicationState? TryRestorePersisted(string payload) => s_store.RestoreEnvelope(payload);
    }
}
