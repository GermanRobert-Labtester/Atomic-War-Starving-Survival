using System;
using System.Collections.Generic;
#pragma warning disable CS8618

using Ashfall.Core.Campaign;
using Ashfall.Core.Flags;
using Ashfall.Core.Journal;
using Ashfall.Core.Medical;
using Ashfall.Core.Memorial;

namespace Ashfall.Core.Survivors
{
    /// <summary>Normalized death origin. Every death path funnels into one of these.</summary>
    public enum SurvivorDeathCause
    {
        Unknown = 0,
        Needs = 1,        // starvation / dehydration / cold (health reached zero via NeedsSystem)
        Radiation = 2,    // acute radiation sickness health drain
        Disease = 3,      // lethal disease outcome
        Combat = 4,       // tactical combat fatality
        Expedition = 5,   // lost on expedition
        Medical = 6,      // died under care / procedure complication
        Scripted = 7      // narrative / scripted event death
    }

    /// <summary>
    /// One idempotent survivor-fate record. The first report for a survivor id
    /// wins; later reports (any cause, any source) return the existing record
    /// without re-running the cascade.
    /// </summary>
    [Serializable]
    public sealed class SurvivorFateEvent
    {
        public string survivorId = string.Empty;
        public SurvivorDeathCause cause = SurvivorDeathCause.Unknown;
        public string causeDetail = string.Empty;  // e.g. disease id, encounter id
        public int day;
        public string source = string.Empty;       // reporting system id, for audit
        public bool isPlayerAvatar;                // distinguishes avatar death from roster death

        public SurvivorFateEvent Clone() => new SurvivorFateEvent
        {
            survivorId = survivorId,
            cause = cause,
            causeDetail = causeDetail,
            day = day,
            source = source,
            isPlayerAvatar = isPlayerAvatar
        };
    }

    /// <summary>Serialized fate ledger.</summary>
    [Serializable]
    public sealed class SurvivorFateSaveState
    {
        public string systemId = SurvivorFateSystem.SystemId;
        public List<SurvivorFateEvent> fates = new List<SurvivorFateEvent>();
    }

    /// <summary>Checksummed save envelope for the survivor-fate ledger.</summary>
    [Serializable]
    public sealed class SurvivorFateSave
    {
        public const int CurrentSaveVersion = 1;
        public int saveVersion = CurrentSaveVersion;
        public int simDay;
        public SurvivorFateSaveState State = new SurvivorFateSaveState();
        public string Checksum = string.Empty;
    }

    /// <summary>
    /// Survivor-fate pipeline (Task 121). The single Core authority every death
    /// source reports into. One report per survivor id runs one cascade:
    ///
    ///   1. roster entry marked dead with an immutable reason
    ///   2. needs state forced dead (guards duplicate if already dead)
    ///   3. duty-roster, caregiving, medical-ward and expedition assignments cleared
    ///   4. leadership stress + shelter-wide grief morale applied
    ///   5. final wish resolved (completed wishes stand; active wishes fail)
    ///   6. memorial entry recorded exactly once
    ///   7. journal entry recorded exactly once (deduped by knowledge key)
    ///   8. consequence-ledger counter/flag updated for faction & epilogue queries
    ///   9. a survivor_perished day event buffered for the daily briefing
    ///
    /// Engine-agnostic. All side effects route through injected system references
    /// (any may be null — the cascade degrades gracefully per lane). The host
    /// wires the live sessions; headless tests wire only what they need.
    /// </summary>
    public sealed class SurvivorFateSystem
    {
        public const string SystemId = "survivor_fate_system";

        /// <summary>Consequence-ledger counter: total deaths this campaign.</summary>
        public const string CounterDeathsTotal = "deaths_total";
        /// <summary>Consequence-ledger flag prefix: set once per deceased survivor.</summary>
        public const string FlagSurvivorDiedPrefix = "flag_survivor_died_";
        /// <summary>Journal knowledge-key prefix: one entry per deceased survivor.</summary>
        public const string JournalKeyPrefix = "survivor_death_";

        /// <summary>Shelter-wide morale hit when a survivor dies.</summary>
        public const float GriefMoraleDelta = -8f;

        private readonly SurvivorFateSaveState _state;
        private readonly Dictionary<string, SurvivorFateEvent> _byId =
            new Dictionary<string, SurvivorFateEvent>(StringComparer.Ordinal);

        // ── Injected cascade lanes (all optional) ──────────────────────
        private readonly SurvivorRosterSystem _roster;
        private readonly NeedsSystem _needs;
        private readonly DutyRosterSystem _dutyRoster;
        private readonly CaregivingSystem _caregiving;
        private readonly MedicalWardSystem _medicalWard;
        private readonly SurvivorSocialCoordinator _social;
        private readonly FinalWishSystem _finalWish;
        private readonly MemorialSystem _memorial;
        private readonly JournalSystem _journal;
        private readonly IFlagLedger _flags;
        private readonly Func<int> _getDay;
        private readonly Func<string, string> _displayNameFor;
        private readonly Action<string> _expeditionRecall;

        /// <summary>Raised exactly once per survivor when the fate cascade runs.</summary>
        public event Action<SurvivorFateEvent> OnSurvivorFate;

        /// <summary>Raised when the cascade leaves zero living roster members.</summary>
        public event Action<SurvivorFateEvent> OnLastSurvivorDied;

        /// <summary>Day events buffered since the last drain (briefing feed).</summary>
        private readonly List<DayStateChangeEvent> _pendingDayEvents = new List<DayStateChangeEvent>();

        public SurvivorFateSystem(
            SurvivorRosterSystem roster = null,
            NeedsSystem needs = null,
            DutyRosterSystem dutyRoster = null,
            CaregivingSystem caregiving = null,
            MedicalWardSystem medicalWard = null,
            SurvivorSocialCoordinator social = null,
            FinalWishSystem finalWish = null,
            MemorialSystem memorial = null,
            JournalSystem journal = null,
            IFlagLedger flags = null,
            Func<int> getDay = null,
            Func<string, string> displayNameFor = null,
            Action<string> expeditionRecall = null,
            SurvivorFateSaveState state = null)
        {
            _roster = roster;
            _needs = needs;
            _dutyRoster = dutyRoster;
            _caregiving = caregiving;
            _medicalWard = medicalWard;
            _social = social;
            _finalWish = finalWish;
            _memorial = memorial;
            _journal = journal;
            _flags = flags;
            _getDay = getDay ?? (() => 1);
            _displayNameFor = displayNameFor ?? (id => id);
            _expeditionRecall = expeditionRecall;
            _state = state ?? new SurvivorFateSaveState();
            if (_state.fates == null) _state.fates = new List<SurvivorFateEvent>();
            RebuildIndex();
        }

        public IReadOnlyList<SurvivorFateEvent> Fates => _state.fates;

        public bool HasFate(string survivorId) =>
            !string.IsNullOrEmpty(survivorId) && _byId.ContainsKey(survivorId);

        public SurvivorFateEvent FindFate(string survivorId) =>
            !string.IsNullOrEmpty(survivorId) && _byId.TryGetValue(survivorId, out var f) ? f : null;

        public int DeathCount => _state.fates.Count;

        // ── Report (the cascade) ───────────────────────────────────────

        /// <summary>
        /// Report a survivor death. Idempotent: the first report for a survivor
        /// id runs the full cascade and is recorded; subsequent reports return
        /// the original record with no side effects. Returns the canonical record.
        /// </summary>
        public SurvivorFateEvent ReportDeath(SurvivorFateEvent fate)
        {
            if (fate == null) throw new ArgumentNullException(nameof(fate));
            if (string.IsNullOrEmpty(fate.survivorId))
                throw new ArgumentException("survivorId required", nameof(fate));

            if (_byId.TryGetValue(fate.survivorId, out var existing))
                return existing;

            if (fate.day <= 0) fate.day = _getDay();

            var record = fate.Clone();
            _state.fates.Add(record);
            _byId[record.survivorId] = record;

            RunCascade(record);

            OnSurvivorFate?.Invoke(record);

            if (_roster != null && _roster.LivingCount == 0)
                OnLastSurvivorDied?.Invoke(record);

            return record;
        }

        /// <summary>Convenience overload for the common case.</summary>
        public SurvivorFateEvent ReportDeath(
            string survivorId,
            SurvivorDeathCause cause,
            string causeDetail = "",
            string source = "",
            bool isPlayerAvatar = false,
            int day = 0)
        {
            return ReportDeath(new SurvivorFateEvent
            {
                survivorId = survivorId,
                cause = cause,
                causeDetail = causeDetail ?? string.Empty,
                day = day,
                source = source ?? string.Empty,
                isPlayerAvatar = isPlayerAvatar
            });
        }

        /// <summary>
        /// Legacy-save repair: synthesize fate records for roster entries that
        /// are dead but have no fate record (pre-pipeline saves). The roster's
        /// stored deathReason is preserved as the cause detail. Runs at most one
        /// synthesized cascade per survivor; safe to call on every load.
        /// Returns the number of records synthesized.
        /// </summary>
        public int ReconcileFromRoster()
        {
            if (_roster == null) return 0;
            int synthesized = 0;
            var entries = _roster.Roster;
            for (int i = 0; i < entries.Count; i++)
            {
                var e = entries[i];
                if (e == null || e.isAlive || string.IsNullOrEmpty(e.survivorId)) continue;
                if (_byId.ContainsKey(e.survivorId)) continue;

                ReportDeath(new SurvivorFateEvent
                {
                    survivorId = e.survivorId,
                    cause = SurvivorDeathCause.Unknown,
                    causeDetail = string.IsNullOrEmpty(e.deathReason) ? "pre-pipeline death (legacy save)" : e.deathReason,
                    day = _getDay(),
                    source = "legacy_reconcile"
                });
                synthesized++;
            }
            return synthesized;
        }

        private void RunCascade(SurvivorFateEvent fate)
        {
            string id = fate.survivorId;
            string reason = DescribeCause(fate);

            // 1. Authoritative roster mark (idempotent inside the roster).
            _roster?.Die(id, reason);

            // 2. Needs state forced dead — ForceDeath guards IsDead, and
            //    Needs.OnDied subscribers see exactly one transition.
            var needsState = _needs?.Get(id);
            if (needsState != null && !needsState.IsDead)
                _needs!.ForceDeath(needsState);

            // 3. Clear assignments: duty roster, caregiving (both directions),
            //    medical ward admission, active expedition recall.
            _dutyRoster?.RemoveAssignmentsFor(id);
            if (_caregiving != null)
            {
                _caregiving.UnassignCaregiver(id);              // as patient
                _caregiving.UnassignCaregiverByCaregiver(id);   // as caregiver
            }
            if (_medicalWard != null && _medicalWard.GetActiveAdmission(id) != null)
                _medicalWard.Discharge(id, fate.day);
            _expeditionRecall?.Invoke(id);

            // 4. Social: leadership stress / succession.
            _social?.OnSurvivorDied(id);

            // Shelter-wide grief for the living.
            if (_needs != null && _roster != null)
            {
                var entries = _roster.Roster;
                for (int i = 0; i < entries.Count; i++)
                {
                    var e = entries[i];
                    if (e == null || !e.isAlive) continue;
                    if (string.Equals(e.survivorId, id, StringComparison.Ordinal)) continue;
                    _needs.Modify(e.survivorId, NeedKind.Morale, GriefMoraleDelta);
                }
            }

            // 5. Final wishes: completed wishes stand; active wishes fail on death.
            bool wishResolved = false;
            if (_finalWish != null)
            {
                if (_finalWish.HasCompletedWish(id))
                {
                    wishResolved = true;
                }
                else if (_finalWish.HasActiveWish(id) || _finalWish.HasTerminalPrognosis(id))
                {
                    _finalWish.OnPrognosisExpired(id); // fails the wish, applies grief penalty
                }
            }

            // 6. Memorial (idempotent by survivor id inside MemorialSystem).
            if (_memorial != null)
            {
                int joinedDay = _roster?.Find(id)?.joinedDay ?? 0;
                _memorial.Memorialize(new MemorialInput
                {
                    SurvivorId = id,
                    Cause = reason,
                    Day = fate.day,
                    BirthDay = joinedDay,
                    FinalWishResolved = wishResolved,
                    Epitaph = string.Empty
                });
            }

            // 7. Journal (deduped by knowledge key inside JournalSystem).
            if (_journal != null)
            {
                string name = _displayNameFor(id);
                _journal.TryAddRawEntry(
                    JournalKeyPrefix + id,
                    $"{name} {DescribeCausePastTense(fate)} on day {fate.day}.",
                    null!,
                    fate.day);
            }

            // 8. Consequence ledger (faction / epilogue queries).
            if (_flags != null)
            {
                _flags.Increment(CounterDeathsTotal, 1, SystemId, fate.source, fate.day, id);
                _flags.Set(FlagSurvivorDiedPrefix + id, SystemId, fate.source, fate.day, id);
            }

            // 9. Briefing feed.
            _pendingDayEvents.Add(new DayStateChangeEvent(
                "survivor_perished", SystemId, id, reason));
        }

        // ── Briefing feed ──────────────────────────────────────────────

        /// <summary>
        /// Drain buffered survivor_perished day events into the owner's event
        /// list. Called by the campaign-day owner so deaths land in the daily
        /// briefing exactly once, regardless of which hour the death fired.
        /// </summary>
        public void DrainDayEvents(List<DayStateChangeEvent> target)
        {
            if (target == null) return;
            for (int i = 0; i < _pendingDayEvents.Count; i++)
                target.Add(_pendingDayEvents[i]);
            _pendingDayEvents.Clear();
        }

        public int PendingDayEventCount => _pendingDayEvents.Count;

        // ── Save / Load ────────────────────────────────────────────────

        public SurvivorFateSaveState CaptureState()
        {
            var copy = new SurvivorFateSaveState { systemId = SystemId };
            var ordered = new List<SurvivorFateEvent>(_state.fates);
            ordered.Sort((a, b) =>
            {
                int c = a.day.CompareTo(b.day);
                return c != 0 ? c : string.CompareOrdinal(a.survivorId, b.survivorId);
            });
            for (int i = 0; i < ordered.Count; i++)
                copy.fates.Add(ordered[i].Clone());
            return copy;
        }

        public void RestoreState(SurvivorFateSaveState saved)
        {
            _state.fates.Clear();
            _byId.Clear();
            _pendingDayEvents.Clear();
            _state.systemId = SystemId;
            if (saved?.fates != null)
            {
                for (int i = 0; i < saved.fates.Count; i++)
                {
                    var f = saved.fates[i];
                    if (f == null || string.IsNullOrEmpty(f.survivorId)) continue;
                    if (_byId.ContainsKey(f.survivorId)) continue; // first record wins
                    var copy = f.Clone();
                    _state.fates.Add(copy);
                    _byId[copy.survivorId] = copy;
                }
            }
        }

        private void RebuildIndex()
        {
            _byId.Clear();
            for (int i = 0; i < _state.fates.Count; i++)
            {
                var f = _state.fates[i];
                if (f != null && !string.IsNullOrEmpty(f.survivorId) && !_byId.ContainsKey(f.survivorId))
                    _byId[f.survivorId] = f;
            }
        }

        // ── Cause text ─────────────────────────────────────────────────

        public static string DescribeCause(SurvivorFateEvent fate)
        {
            if (fate == null) return "died";
            string detail = string.IsNullOrEmpty(fate.causeDetail) ? null : fate.causeDetail;
            return fate.cause switch
            {
                SurvivorDeathCause.Needs => detail != null ? $"succumbed to {detail}" : "succumbed to exposure and privation",
                SurvivorDeathCause.Radiation => "died of acute radiation sickness",
                SurvivorDeathCause.Disease => detail != null ? $"died of {detail}" : "died of disease",
                SurvivorDeathCause.Combat => detail != null ? $"killed in combat ({detail})" : "killed in combat",
                SurvivorDeathCause.Expedition => detail != null ? $"lost on expedition ({detail})" : "lost on expedition",
                SurvivorDeathCause.Medical => detail != null ? $"died under care ({detail})" : "died under care",
                SurvivorDeathCause.Scripted => detail != null ? detail : "died",
                _ => detail != null ? $"died ({detail})" : "died",
            };
        }

        private static string DescribeCausePastTense(SurvivorFateEvent fate) => DescribeCause(fate);
    }
}
