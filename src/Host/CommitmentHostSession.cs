// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 38 / C1[11] — Commitments & Deadlines host adapter.
//
// Thin host session over Ashfall.Core.Commitments.CommitmentSystem. It owns no
// gameplay rules: it forwards the campaign day tick, exposes the canonical
// commands, buffers the system's semantic day events for the briefing pipeline,
// and persists through the registered "commitment" save section.
// ============================================================================
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Commitments;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public sealed class CommitmentHostSession : HostSessionBase
    {
        private readonly List<DayStateChangeEvent> _dayEvents = new List<DayStateChangeEvent>();

        public CommitmentSystem System { get; }

        public CommitmentHostSession(CommitmentSystem system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
        }

        /// <summary>Campaign-day evaluation: warning ladder + miss consequences.</summary>
        public void TickDay(int day)
        {
            System.TickDay(day, _dayEvents);
            if (_dayEvents.Count > 0) RaiseStateChanged();
        }

        /// <summary>Drain buffered semantic events into the day owner's report.</summary>
        public void DrainDayEvents(List<DayStateChangeEvent> events)
        {
            if (events == null || _dayEvents.Count == 0) return;
            events.AddRange(_dayEvents);
            _dayEvents.Clear();
        }

        /// <summary>Partial payment / delivery progress. Returns true when the
        /// commitment becomes Met.</summary>
        public bool RecordProgress(string commitmentId, int quantityAdded, int day)
        {
            bool met = System.RecordProgress(commitmentId, quantityAdded);
            if (met)
            {
                _dayEvents.Add(new DayStateChangeEvent(
                    "obligation_met", CommitmentSystem.OwnerId, commitmentId, null, day));
            }
            RaiseStateChangedIf(met);
            return met;
        }

        /// <summary>Immediate settlement (action/flag settlement model).</summary>
        public bool Settle(string commitmentId, int currentDay)
            => RaiseStateChangedIf(System.Settle(commitmentId, currentDay));

        public CommitmentReadModel? GetCommitment(string commitmentId, int currentDay)
            => System.GetCommitment(commitmentId, currentDay);

        public IReadOnlyList<CommitmentReadModel> GetCommitments(int currentDay)
            => System.GetCommitments(currentDay);

        public override void Save()
        {
            if (!IsDirty) return;
            if (CommitmentSaveStore.TrySave(System.CaptureState()))
                base.Save();
        }
    }

    public static class CommitmentSaveStore
    {
        public const string FileName = "commitment_save.json";
        public const string SectionName = "commitment";

        private static readonly SaveStore<CommitmentSaveState> s_store =
            SaveStoreHub.Checksummed<CommitmentSaveState>(FileName, nameof(CommitmentSaveStore));

        public static bool TrySave(CommitmentSaveState state) => s_store.TrySave(state);
        public static CommitmentSaveState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(CommitmentSaveState state) => s_store.CapturePersisted(state);
        public static CommitmentSaveState? TryRestore(string json) => s_store.RestoreBare(json);
    }
}
