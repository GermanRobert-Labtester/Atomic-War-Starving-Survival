// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 39 / C2[16] — Session Durability host adapter.
//
// Authority boundary (deliberate): SaveSlotService + SaveLoadHostSession own
// slot truth, envelope writes, quarantine and recovery. SessionDurabilityManager
// is the *durability audit and release-gate read model* over that pipeline:
// observed slot summaries, day-advance soak samples, corruption/recovery
// events, and the stability verdict. It is never consulted for a recovery
// decision — the canonical service performs the recovery and this session only
// records the event.
// ============================================================================
using System;
using System.Collections.Generic;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public sealed class SessionDurabilityHostSession : HostSessionBase
    {
        public SessionDurabilityManager System { get; }

        public SessionDurabilityHostSession(SessionDurabilityManager system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
        }

        /// <summary>Mirror one successful canonical save into the audit read model.</summary>
        public void RecordSuccessfulSave(
            string slotId, string displayName, int campaignDay, int survivorCount, string checksum)
        {
            RaiseStateChangedIf(System.RegisterOrUpdateSlot(
                slotId, displayName, campaignDay, survivorCount, checksum));
        }

        /// <summary>Audit one failed canonical load (no recovery decision here).</summary>
        public void RecordLoadFailure(string slotId, string reason)
            => RaiseStateChangedIf(System.RecordInterruptedWrite(slotId, reason));

        /// <summary>Record one measured campaign day advance (soak sample).</summary>
        public void RecordDaySample(int day, float durationMs, long trackedStateBytes)
        {
            System.RecordDayAdvance(day, durationMs, trackedStateBytes);
            RaiseStateChanged();
        }

        public SoakStabilityReport EvaluateSoak(
            float maxAllowedP95Ms = 2500f,
            float maxAllowedSlopeBytesPerDay = 50000f)
            => System.EvaluateSoakStability(maxAllowedP95Ms, maxAllowedSlopeBytesPerDay);

        public override void Save()
        {
            if (!IsDirty) return;
            if (SessionDurabilitySaveStore.TrySave(System.CaptureState()))
                base.Save();
        }
    }

    public static class SessionDurabilitySaveStore
    {
        public const string FileName = "session_durability_save.json";
        public const string SectionName = "session_durability";

        private static readonly SaveStore<SessionDurabilityState> s_store =
            SaveStoreHub.Checksummed<SessionDurabilityState>(FileName, nameof(SessionDurabilitySaveStore));

        public static bool TrySave(SessionDurabilityState state) => s_store.TrySave(state);
        public static SessionDurabilityState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(SessionDurabilityState state) => s_store.CapturePersisted(state);
        public static SessionDurabilityState? TryRestore(string json) => s_store.RestoreBare(json);
    }
}
