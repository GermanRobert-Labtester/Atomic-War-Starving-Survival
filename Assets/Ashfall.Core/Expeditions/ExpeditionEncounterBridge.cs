using System;
using System.Collections.Generic;
using Ashfall.Core.Narrative;

namespace Ashfall.Core.Expeditions
{
    /// <summary>
    /// Engine-agnostic bridge from ExpeditionSystem.OnEncounterTriggered
    /// into NarrativeEncounterSystem. Resolves a selectable encounter by
    /// weight using the expedition's stance / dangerLevel / locationId,
    /// deterministically, and exposes the result as an EncounterSurfaced DTO.
    /// Does NOT auto-resolve: choices require an explicit player call back
    /// into NarrativeEncounterSystem.Resolve(...). When the underlying
    /// catalog has no eligible entry, emits an honest null-encounter DTO
    /// describing only what the state can prove.
    ///
    /// Ordering decision: the host passes the same ISeededRng instance it used
    /// for ExpeditionSystem.TickHours. TickHours consumes RNG for stamina and
    /// loot rolls before RollEncounter fires OnEncounterTriggered; the bridge
    /// then continues consuming from the same stream for selection. This keeps
    /// the full encounter-roll → selection sequence on one deterministic stream
    /// so the same seed produces identical surfaced sequences across hosts.
    /// </summary>
    public sealed class ExpeditionEncounterBridge
    {
        public sealed class EncounterSurfaced
        {
            /// <summary>Selected encounter id, or null when no eligible encounter exists.</summary>
            public string encounter_id;

            /// <summary>Verbatim title from the catalog, or "Encounter" for bare notices.</summary>
            public string title;

            /// <summary>Verbatim description from the catalog, or honest-bare text.</summary>
            public string description;

            /// <summary>Catalog category (Discovery / Hazard / Social / Trade), empty for bare notices.</summary>
            public string category;

            /// <summary>Ordered choices from the catalog, empty for bare notices.</summary>
            public List<EncounterChoiceDefinition> choices;

            /// <summary>The expedition state that triggered this surfacing.</summary>
            public ExpeditionState trigger;

            /// <summary>Null from the bridge; reserved for host-filled lead-resolution flag.</summary>
            public bool? resolved_at_lead;

            /// <summary>Null from the bridge; populated after player choice via Resolve.</summary>
            public string encounter_record_resolution_id;
        }

        public event Action<EncounterSurfaced> OnSurfaced;

        private readonly NarrativeEncounterSystem _narrative;
        private readonly ISeededRng _rng;
        private EncounterSurfaced _lastSurfaced;

        /// <summary>
        /// Construct with the host's NarrativeEncounterSystem and the shared
        /// ISeededRng stream (same instance used for ExpeditionSystem.TickHours).
        /// </summary>
        public ExpeditionEncounterBridge(NarrativeEncounterSystem narrative, ISeededRng rng)
        {
            _narrative = narrative ?? throw new ArgumentNullException(nameof(narrative));
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
        }

        /// <summary>
        /// Surface an encounter for the given expedition state. Consumes RNG
        /// from the shared stream. Raises OnSurfaced exactly once per call with
        /// either a resolved DTO or an honest-bare-notice DTO when nothing
        /// qualifies.
        /// </summary>
        public void Surface(ExpeditionState state)
        {
            if (state == null) return;

            var dto = new EncounterSurfaced
            {
                trigger = state,
                resolved_at_lead = null,
                encounter_record_resolution_id = null!
            };

            var def = _narrative.SelectEncounter(
                state.stance ?? string.Empty,
                state.dangerLevel,
                state.locationId ?? string.Empty,
                _rng);

            if (def == null)
            {
                dto.encounter_id = null!;
                dto.title = "Encounter";
                dto.description = "Something is happening on this leg. No record of it survives.";
                dto.category = string.Empty;
                dto.choices = new List<EncounterChoiceDefinition>();
                dto.resolved_at_lead = false;
            }
            else
            {
                dto.encounter_id = def.id;
                dto.title = def.title;
                dto.description = def.description;
                dto.category = def.category;
                dto.choices = def.choices ?? new List<EncounterChoiceDefinition>();
            }

            _lastSurfaced = dto;
            OnSurfaced?.Invoke(dto);
        }

        /// <summary>
        /// Resolve the most recently surfaced encounter through Core. Returns false
        /// when nothing has been surfaced yet.
        /// </summary>
        public bool ResolveChoice(string encounterId, string choiceId, int day)
            => ResolveChoice(encounterId, choiceId, day, null!);

        /// <summary>
        /// Resolve an encounter against an explicit locationId. Use this when the
        /// player works through a backlog of surfaced encounters: the row being
        /// resolved is not necessarily the most recently surfaced one, so the
        /// location must come from that row rather than from _lastSurfaced.
        /// Pass null for locationId to fall back to the last surfaced DTO.
        /// Returns false when the location cannot be established, because
        /// inventing one would put a false place in the resolution history.
        /// </summary>
        public bool ResolveChoice(string encounterId, string choiceId, int day, string locationId)
        {
            if (string.IsNullOrEmpty(encounterId)) return false;

            string effectiveLocation = locationId ?? _lastSurfaced?.trigger?.locationId!;
            if (effectiveLocation == null) return false;

            bool ok = _narrative.Resolve(encounterId, choiceId, effectiveLocation, day);

            // Only stamp the cached DTO when it is actually the encounter that was
            // resolved. Resolving an older backlog row must not mark the newest
            // surfaced encounter as decided.
            if (ok && _lastSurfaced != null && _lastSurfaced.encounter_id == encounterId)
            {
                _lastSurfaced.resolved_at_lead = true;
                _lastSurfaced.encounter_record_resolution_id = encounterId + ":" + choiceId + ":" + day;
            }
            return ok;
        }
    }
}
