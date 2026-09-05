using System;
using System.Collections.Generic;
#pragma warning disable CS8618
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

        public TravelEncounterSystem? TravelEngine { get; set; }
        public int CurrentDay { get; set; } = 1;
        public string CurrentSeason { get; set; } = "all";
        public Func<string, string>? RegionResolver { get; set; }
        public EncounterSurfaced? LastSurfaced => _lastSurfaced;

        /// <summary>F2/F3/F4 — the consequence payload of the most recent
        /// successful <see cref="ResolveChoice"/>, or null. The Host reads it
        /// immediately after a successful resolve to apply item/journal/
        /// location effects through their owning subsystems. Core never
        /// mutates those systems itself.</summary>
        public NarrativeEncounterResolutionResult? LastResolution { get; private set; }

        /// <summary>
        /// Construct with the host's NarrativeEncounterSystem and the shared
        /// ISeededRng stream (same instance used for ExpeditionSystem.TickHours).
        /// </summary>
        public ExpeditionEncounterBridge(NarrativeEncounterSystem narrative, ISeededRng rng)
            : this(narrative, rng, null)
        {
        }

        public ExpeditionEncounterBridge(NarrativeEncounterSystem narrative, ISeededRng rng, TravelEncounterSystem? travel)
        {
            _narrative = narrative ?? throw new ArgumentNullException(nameof(narrative));
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            TravelEngine = travel;
        }

        private string ResolveRegion(string? locationId)
        {
            if (string.IsNullOrWhiteSpace(locationId)) return string.Empty;
            if (RegionResolver != null)
            {
                string resolved = RegionResolver(locationId);
                if (!string.IsNullOrEmpty(resolved)) return resolved;
            }

            string lower = locationId.ToLowerInvariant();
            if (lower.Contains("high_scarp")) return "high_scarp";
            if (lower.Contains("the_toll") || lower.Contains("toll")) return "the_toll";
            if (lower.Contains("industrial_belt") || lower.Contains("foundry") || lower.Contains("industrial")) return "industrial_belt";
            if (lower.Contains("dead_suburbs") || lower.Contains("suburbs")) return "dead_suburbs";

            return string.Empty;
        }

        /// <summary>
        /// Surface an encounter for the given expedition state. Consumes RNG
        /// from the shared stream. Raises OnSurfaced exactly once per call with
        /// either a resolved DTO or an honest-bare-notice DTO when nothing
        /// qualifies. Merges narrative and travel encounter candidates into
        /// a single weighted list rolled with exactly one RNG draw.
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

            // Enumerate narrative candidates (0 RNG)
            var narrativeCandidates = _narrative.GetEligibleCandidates(
                state.stance ?? string.Empty,
                state.dangerLevel,
                state.locationId ?? string.Empty);

            // Enumerate patrol candidates (0 RNG)
            List<(TravelEncounterDefinition encounter, float weight)>? patrolCandidates = null;
            if (TravelEngine != null)
            {
                string region = ResolveRegion(state.locationId);
                int day = state.startedDay > 0 ? state.startedDay : CurrentDay;
                patrolCandidates = TravelEngine.GetEligiblePatrolCandidates(
                    region,
                    state.dangerLevel,
                    state.stance ?? string.Empty,
                    CurrentSeason,
                    day);
            }

            double totalWeight = 0d;
            for (int i = 0; i < narrativeCandidates.Count; i++)
            {
                totalWeight += narrativeCandidates[i].weight;
            }
            if (patrolCandidates != null)
            {
                for (int i = 0; i < patrolCandidates.Count; i++)
                {
                    totalWeight += patrolCandidates[i].weight;
                }
            }

            if (totalWeight <= 0d)
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
                // Single deterministic draw
                double roll = _rng.NextDouble() * totalWeight;
                double acc = 0d;
                bool picked = false;

                for (int i = 0; i < narrativeCandidates.Count; i++)
                {
                    acc += narrativeCandidates[i].weight;
                    if (roll < acc)
                    {
                        var def = narrativeCandidates[i].def;
                        dto.encounter_id = def.id;
                        dto.title = def.title;
                        dto.description = def.description;
                        dto.category = def.category;
                        dto.choices = def.choices ?? new List<EncounterChoiceDefinition>();
                        _narrative.RecordEncounterSelected(def);
                        picked = true;
                        break;
                    }
                }

                if (!picked && patrolCandidates != null)
                {
                    for (int i = 0; i < patrolCandidates.Count; i++)
                    {
                        acc += patrolCandidates[i].weight;
                        if (roll < acc || i == patrolCandidates.Count - 1)
                        {
                            var pDef = patrolCandidates[i].encounter;
                            dto.encounter_id = pDef.Id;
                            dto.title = pDef.Title;
                            dto.description = pDef.Description;
                            dto.category = pDef.Category;
                            dto.choices = new List<EncounterChoiceDefinition>();
                            if (pDef.Choices != null)
                            {
                                foreach (var c in pDef.Choices)
                                {
                                    dto.choices.Add(new EncounterChoiceDefinition
                                    {
                                        choiceId = c.ChoiceId,
                                        text = c.Text,
                                        moraleDelta = c.MoraleDelta,
                                        guiltDelta = c.GuiltDelta,
                                        requiredItemId = c.RequiredItemId,
                                        requiredItemQuantity = c.RequiredItemQuantity,
                                        factionId = !string.IsNullOrWhiteSpace(c.FactionId) ? c.FactionId : pDef.FactionId,
                                        factionStandingDelta = c.FactionStandingDelta,
                                        costItems = new List<string>(c.CostItems ?? new List<string>())
                                    });
                                }
                            }
                            picked = true;
                            break;
                        }
                    }
                }
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

            // Route patrol encounter resolution through TravelEngine
            if (TravelEngine != null && TravelEngine.Catalog.TryGetEncounter(encounterId, out var patrolDef))
            {
                bool ok = TravelEngine.ResolveChoice(encounterId, choiceId, day, out var travelRes);
                if (ok && travelRes != null)
                {
                    LastResolution = new NarrativeEncounterResolutionResult
                    {
                        EncounterId = encounterId,
                        ChoiceId = choiceId,
                        LocationId = locationId ?? _lastSurfaced?.trigger?.locationId ?? string.Empty,
                        Day = day,
                        MoraleDelta = travelRes.MoraleDelta,
                        GuiltDelta = travelRes.GuiltDelta
                    };

                    if (_lastSurfaced != null && _lastSurfaced.encounter_id == encounterId)
                    {
                        _lastSurfaced.resolved_at_lead = true;
                        _lastSurfaced.encounter_record_resolution_id = encounterId + ":" + choiceId + ":" + day;
                    }
                    return true;
                }
                return false;
            }

            string effectiveLocation = locationId ?? _lastSurfaced?.trigger?.locationId!;
            if (effectiveLocation == null) return false;

            // Flagship §14.1 — a surfaced encounter that was already resolved must
            // not resolve again through the bridge. Core permits direct re-
            // resolution for backlog defense; the bridge's surfaced-queue flow
            // does not: the player has already acknowledged this encounter, and
            // a second resolution would reapply its consequences. Older backlog
            // rows (different encounter ids) remain resolvable.
            if (_lastSurfaced != null
                && _lastSurfaced.encounter_id == encounterId
                && _lastSurfaced.resolved_at_lead == true)
            {
                LastResolution = null;
                return false;
            }

            NarrativeEncounterResolutionResult? result = _narrative.TryResolve(encounterId, choiceId, effectiveLocation, day);
            LastResolution = result;
            bool okNarrative = result != null;

            // Only stamp the cached DTO when it is actually the encounter that was
            // resolved. Resolving an older backlog row must not mark the newest
            // surfaced encounter as decided.
            if (okNarrative && _lastSurfaced != null && _lastSurfaced.encounter_id == encounterId)
            {
                _lastSurfaced.resolved_at_lead = true;
                _lastSurfaced.encounter_record_resolution_id = encounterId + ":" + choiceId + ":" + day;
            }
            return okNarrative;
        }
    }
}
