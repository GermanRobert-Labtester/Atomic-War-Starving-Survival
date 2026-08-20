using System;
using System.Collections.Generic;

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
            _state = state ?? throw new ArgumentNullException(nameof(state));
        }

        public IReadOnlyList<EncounterResolution> History => _state.History;

        public bool IsResolved(string expeditionId, string encounterId)
        {
            for (int i = 0; i < _state.History.Count; i++)
            {
                var h = _state.History[i];
                if (h.ExpeditionId == expeditionId && h.EncounterId == encounterId)
                    return true;
            }
            return false;
        }

        public EncounterChoiceResult Resolve(EncounterChoiceRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (string.IsNullOrEmpty(request.ExpeditionId))
                return EncounterChoiceResult.Fail("missing_expedition_id");
            if (string.IsNullOrEmpty(request.EncounterId))
                return EncounterChoiceResult.Fail("missing_encounter_id");
            if (string.IsNullOrEmpty(request.ChoiceId))
                return EncounterChoiceResult.Fail("missing_choice_id");
            if (IsResolved(request.ExpeditionId, request.EncounterId))
                return EncounterChoiceResult.Fail("already_resolved");

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

        public EncounterChoiceState Capture() => new EncounterChoiceState
        {
            History = new List<EncounterResolution>(History)
        };

        public void RestoreInto(EncounterChoiceState state)
        {
            History = state.History ?? new List<EncounterResolution>();
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
                Resolution = null };
    }
}
