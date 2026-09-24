// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Expeditions
{
    /// <summary>
    /// ASHFALL Encounter Choice Resolver (item 5).
    ///
    /// Ensures pending encounters are resolved atomically: a choice is
    /// applied at most once per (expedition_id, encounter_id) pair, and
    /// the resolver refuses duplicate rewards or duplicate combat-start
    /// requests for the same encounter. The host wires
    /// <see cref="ExpeditionEncounterBridge"/> to surface encounters and
    /// routes the player's choice through <see cref="Resolve"/>.
    /// </summary>
    public sealed class EncounterChoiceResolver
    {
        private readonly EncounterChoiceState _state;

        public event Action<EncounterResolution>? OnResolved;

        public EncounterChoiceResolver(EncounterChoiceState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            _state = state.Capture();
        }

        public IReadOnlyList<EncounterResolution> History => _state.History;

        public bool IsResolved(string expeditionId, string encounterId)
        {
            if (_state.History == null) return false;
            for (int i = 0; i < _state.History.Count; i++)
            {
                var h = _state.History[i];
                if (h != null
                    && string.Equals(h.ExpeditionId, expeditionId, StringComparison.Ordinal)
                    && string.Equals(h.EncounterId, encounterId, StringComparison.Ordinal))
                    return true;
            }
            return false;
        }

        public EncounterChoiceResult Resolve(EncounterChoiceRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (string.IsNullOrWhiteSpace(request.ExpeditionId))
                return EncounterChoiceResult.Fail("missing_expedition_id");
            if (string.IsNullOrWhiteSpace(request.EncounterId))
                return EncounterChoiceResult.Fail("missing_encounter_id");
            if (string.IsNullOrWhiteSpace(request.ChoiceId))
                return EncounterChoiceResult.Fail("missing_choice_id");
            if (IsResolved(request.ExpeditionId, request.EncounterId))
                return EncounterChoiceResult.Fail("already_resolved");

            _state.History ??= new List<EncounterResolution>();
            var resolution = new EncounterResolution
            {
                ExpeditionId = request.ExpeditionId,
                EncounterId = request.EncounterId,
                ChoiceId = request.ChoiceId,
                Day = request.Day,
                Outcome = request.PredictedOutcome ?? "pending",
                TriggeredCombat = request.TriggerCombat,
                LootSummary = request.LootSummary ?? string.Empty
            };
            _state.History.Add(resolution);
            OnResolved?.Invoke(resolution);
            return EncounterChoiceResult.Ok(resolution);
        }

        public EncounterChoiceState CaptureState() => _state.Capture();

        public void RestoreState(EncounterChoiceState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            _state.RestoreInto(state);
        }
    }

    [Serializable]
    public sealed class EncounterChoiceRequest
    {
        public string ExpeditionId;
        public string EncounterId;
        public string ChoiceId;
        public int Day;
        public string PredictedOutcome;
        public bool TriggerCombat;
        public string LootSummary;
    }

    [Serializable]
    public sealed class EncounterResolution
    {
        public string ExpeditionId;
        public string EncounterId;
        public string ChoiceId;
        public int Day;
        public string Outcome;
        public bool TriggeredCombat;
        public string LootSummary;
    }

    [Serializable]
    public sealed class EncounterChoiceState
    {
        public List<EncounterResolution> History = new List<EncounterResolution>();

        public EncounterChoiceState Capture()
        {
            var copy = new EncounterChoiceState
            {
                History = new List<EncounterResolution>()
            };
            if (History == null) return copy;

            foreach (var resolution in History)
            {
                var cloned = CloneResolution(resolution);
                if (cloned != null)
                    copy.History.Add(cloned);
            }
            return copy;
        }

        public void RestoreInto(EncounterChoiceState state)
        {
            History = state?.Capture().History ?? new List<EncounterResolution>();
        }

        private static EncounterResolution? CloneResolution(EncounterResolution? source)
        {
            if (source == null
                || string.IsNullOrWhiteSpace(source.ExpeditionId)
                || string.IsNullOrWhiteSpace(source.EncounterId)
                || string.IsNullOrWhiteSpace(source.ChoiceId))
                return null;

            return new EncounterResolution
            {
                ExpeditionId = source.ExpeditionId,
                EncounterId = source.EncounterId,
                ChoiceId = source.ChoiceId,
                Day = source.Day,
                Outcome = source.Outcome ?? string.Empty,
                TriggeredCombat = source.TriggeredCombat,
                LootSummary = source.LootSummary ?? string.Empty
            };
        }
    }

    [Serializable]
    public sealed class EncounterChoiceResult
    {
        public bool Succeeded;
        public string ReasonCode;
        public EncounterResolution Resolution;

        public static EncounterChoiceResult Ok(EncounterResolution r)
            => new EncounterChoiceResult { Succeeded = true, ReasonCode = "ok", Resolution = r };

        public static EncounterChoiceResult Fail(string reason)
            => new EncounterChoiceResult { Succeeded = false, ReasonCode = reason ?? "fail",
                Resolution = null! };
    }
}
