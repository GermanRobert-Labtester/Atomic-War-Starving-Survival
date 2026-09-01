// ============================================================================
// Save Store : OnboardingSaveStore
// Core State : Ashfall.Core.Onboarding.OnboardingSaveState
// Host Caller: Main.Onboarding / OnboardingHostSession
// Purpose    : First-hour onboarding journey progress (stage, dismissed
//              hints, assistance level, completion) — survives both
//              game-shutdown and reset, so the player resumes the journey
//              at the correct step after save/load.
// ============================================================================
using System;
using Ashfall.Core;
using Ashfall.Core.Onboarding;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Persists <see cref="OnboardingSaveState"/> as checksummed JSON under
    /// <c>user://onboarding_save.json</c>. Thin façade over the Core
    /// <c>SaveStore&lt;T&gt;</c> service (via <see cref="SaveStoreHub.Checksummed{T}"/>):
    /// pattern, path resolution, atomic write, and error handling live in the
    /// service. The legacy bare-state fallback is disabled (no pre-checksum
    /// format existed for this section).
    /// </summary>
    public static class OnboardingSaveStore
    {
        public const string FileName = "onboarding_save.json";
        public const string SectionName = "onboarding";

        private static readonly SaveStore<OnboardingSaveState> s_store = SaveStoreHub.Checksummed<OnboardingSaveState>(
            FileName, nameof(OnboardingSaveStore), createBackup: false, allowLegacyBareState: false);

        public static string SavePath => s_store.SavePath;

        public static bool TrySave(OnboardingSaveState state, string? pathOverride = null)
            => s_store.TrySave(state, pathOverride);

        public static OnboardingSaveState? TryLoad(string? pathOverride = null)
            => s_store.TryLoad(pathOverride);

        /// <summary>
        /// Capture the exact persisted bytes for the campaign envelope without
        /// writing to disk (preserves byte shape used by <c>SaveAll</c>).
        /// </summary>
        public static string TryCapturePersisted(OnboardingSaveState state)
            => s_store.CapturePersisted(state);
    }
}
