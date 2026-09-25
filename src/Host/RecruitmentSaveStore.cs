// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : RecruitmentSaveStore
// Core State : Ashfall.Core.Survivors.RecruitmentState
// Host Caller: Main.Recruitment / RecruitmentHostSession
// Purpose    : Plan 204 — durable survivor recruitment campaigns, wilderness
//              discovery, defection offers, and asylum intake state.
// ============================================================================

using Ashfall.Core.Save;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public static class RecruitmentSaveStore
    {
        public const string FileName = "recruitment_save.json";
        public const string SectionName = "recruitment";

        private static readonly SaveStore<RecruitmentState> s_store =
            SaveStoreHub.Checksummed<RecruitmentState>(FileName, nameof(RecruitmentSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(RecruitmentState state) => s_store.TrySave(state);
        public static RecruitmentState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(RecruitmentState state) => s_store.CapturePersisted(state);
        public static RecruitmentState? TryRestorePersisted(string payload) => s_store.RestoreEnvelope(payload);
    }
}
