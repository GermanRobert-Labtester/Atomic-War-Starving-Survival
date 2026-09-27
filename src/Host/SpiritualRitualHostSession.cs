// SPDX-License-Identifier: MIT
// ============================================================================
// Host Session : SpiritualRitualHostSession
// Purpose      : EXPANSION-13-THE-FAITHFUL-AND-THE-FRACTURED — binds the sealed
//                SpiritualRitualCalendarEngine to the authored ritual corpus
//                already loaded by the live SpiritualCatalog. The engine stays
//                the sole verdict authority; the host only tracks the cooldown
//                ledger and routes effects into canonical owners.
// ============================================================================

using System;
using System.Collections.Generic;
using Ashfall.Core.Spiritual;

namespace AtomicWar.GodotApp
{
    /// <summary>Immutable projection of one ritual observance evaluation.</summary>
    public sealed class SpiritualRitualProjection
    {
        public string RitualId = string.Empty;
        public string RitualTitle = string.Empty;
        public bool IsAllowed;
        public int MoraleDeltaPermille;
        public int FrictionReductionPermille;
        public int CooldownRemainingDays;
        public string Reason = string.Empty;
        public int AuthoredRitualCount;
    }

    public sealed class SpiritualRitualHostSession : HostSessionBase
    {
        private SpiritualCatalog? _catalog;
        private readonly Dictionary<string, int> _lastPerformedDay = new(StringComparer.Ordinal);
        private string _lastEvent = string.Empty;

        public SpiritualCatalog? Catalog => _catalog;
        public string LastEvent => _lastEvent;
        public IReadOnlyDictionary<string, int> LastPerformedDay => _lastPerformedDay;
        public int AuthoredRitualCount => _catalog?.Rituals.Count ?? 0;

        /// <summary>Binds the already-loaded authored catalog. No reload, no copy.</summary>
        public void BindCatalog(SpiritualCatalog? catalog)
        {
            _catalog = catalog;
            _lastEvent = catalog == null
                ? "Spiritual ritual catalog unbound."
                : $"Spiritual ritual catalog bound ({catalog.Rituals.Count} authored rituals).";
            RaiseStateChanged();
        }

        public SpiritualRitualDefinition? GetRitual(string ritualId)
        {
            if (_catalog == null || string.IsNullOrEmpty(ritualId)) return null;
            return _catalog.GetRitual(ritualId);
        }

        public int DaysSinceLastPerformed(string ritualId, int currentDay)
        {
            if (string.IsNullOrEmpty(ritualId) || currentDay <= 0) return int.MaxValue;
            return _lastPerformedDay.TryGetValue(ritualId, out int last)
                ? Math.Max(0, currentDay - last)
                : int.MaxValue;
        }

        /// <summary>
        /// Evaluates one authored ritual through the engine. A cooling-down or
        /// unknown ritual is refused without mutating the ledger.
        /// </summary>
        public SpiritualRitualProjection TryEvaluateRitual(string ritualId, int currentDay, int shelterMoralePermille)
        {
            var ritual = GetRitual(ritualId);
            if (ritual == null)
            {
                _lastEvent = $"Ritual '{ritualId}' is not in the authored catalog; refused.";
                return new SpiritualRitualProjection
                {
                    RitualId = ritualId ?? string.Empty,
                    Reason = "Ritual is not in the authored catalog.",
                    AuthoredRitualCount = AuthoredRitualCount
                };
            }

            var result = SpiritualRitualCalendarEngine.EvaluateRitualObservance(
                ritual,
                DaysSinceLastPerformed(ritualId, currentDay),
                shelterMoralePermille);

            return new SpiritualRitualProjection
            {
                RitualId = ritual.Id,
                RitualTitle = ritual.Title,
                IsAllowed = result.IsAllowed,
                MoraleDeltaPermille = result.MoraleDeltaPermille,
                FrictionReductionPermille = result.FrictionReductionPermille,
                CooldownRemainingDays = result.CooldownRemainingDays,
                Reason = result.Reason,
                AuthoredRitualCount = AuthoredRitualCount
            };
        }

        /// <summary>
        /// Records a performed ritual. Returns the projection the caller routes
        /// into the canonical morale owner. The ledger write happens only when
        /// the engine allowed the observance.
        /// </summary>
        public SpiritualRitualProjection? TryPerformRitual(string ritualId, int currentDay, int shelterMoralePermille)
        {
            var projection = TryEvaluateRitual(ritualId, currentDay, shelterMoralePermille);
            if (!projection.IsAllowed)
            {
                _lastEvent = $"Ritual '{projection.RitualId}' refused: {projection.Reason}";
                RaiseStateChanged();
                return null;
            }

            _lastPerformedDay[projection.RitualId] = currentDay;
            _lastEvent = $"Ritual '{projection.RitualId}' observed on day {currentDay}.";
            RaiseStateChanged();
            return projection;
        }

        /// <summary>Holy-day observance scheduled for a movement on a campaign day.</summary>
        public HolyDayObservance? GetScheduledObservance(string movementId, int campaignDay) =>
            SpiritualRitualCalendarEngine.GetScheduledObservance(campaignDay, movementId);

        /// <summary>Ideological-friction mitigation the engine derives for a pair of movements.</summary>
        public int GetFrictionMitigation(string movementA, string movementB, bool sharedRitualObserved) =>
            SpiritualRitualCalendarEngine.CalculateIdeologicalFrictionMitigation(movementA, movementB, sharedRitualObserved);

        // ── Save / Load ──────────────────────────────────────────────────

        public SpiritualRitualSaveState CaptureState()
        {
            var state = new SpiritualRitualSaveState
            {
                schema_version = 1,
                LastPerformedDay = new Dictionary<string, int>(_lastPerformedDay, StringComparer.Ordinal)
            };
            return state;
        }

        public void RestoreState(SpiritualRitualSaveState? state)
        {
            _lastPerformedDay.Clear();
            if (state?.LastPerformedDay != null)
            {
                foreach (var kvp in state.LastPerformedDay)
                {
                    if (string.IsNullOrEmpty(kvp.Key)) continue;
                    _lastPerformedDay[kvp.Key] = kvp.Value;
                }
            }
            RaiseStateChanged();
        }

        public void Reset()
        {
            _lastPerformedDay.Clear();
            _lastEvent = string.Empty;
            RaiseStateChanged();
        }
    }
}
