using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Muster
{
    /// <summary>
    /// Core port the host binds to campaign reality: world flags, the survivor
    /// census (alive/dead — a dead witness never testifies), and faction presence.
    /// Engine-agnostic; the Godot host adapts its flag ledger + survivors host session.
    /// </summary>
    public interface IWitnessEligibility
    {
        bool IsFlagSet(string flagId);

        /// <summary>Only consulted for witnesses carrying a non-empty subject_id.
        /// Implementations must answer from the persisted census — no resurrection.</summary>
        bool IsSubjectAlive(string subjectId);

        /// <summary>Only consulted for witnesses carrying a non-empty faction_id.
        /// Institutional witnesses (summoned faction reps) may be answered true
        /// regardless of personal encounter; implementors document their exception.</summary>
        bool IsFactionPresent(string factionId);
    }

    /// <summary>Default eligibility: everything passes. Tests and headless demos
    /// without a bound campaign use this.</summary>
    public sealed class PassAllWitnessEligibility : IWitnessEligibility
    {
        public static readonly PassAllWitnessEligibility Instance = new PassAllWitnessEligibility();
        public bool IsFlagSet(string flagId) => true;
        public bool IsSubjectAlive(string subjectId) => true;
        public bool IsFactionPresent(string factionId) => true;
    }

    /// <summary>One selectable witness with the testimony variant that matches the
    /// campaign's flag state right now.</summary>
    public class WitnessDelivery
    {
        public WitnessDefinition Witness;
        public WitnessTestimony Testimony;
        public string VariantId => Testimony?.variantId ?? string.Empty;
    }

    /// <summary>
    /// Deterministic witness selection for the Muster gathering (Plan 25 · 25B).
    /// Day gate → alive/dead → faction presence → first-matching testimony variant
    /// → ordering (priority descending, then witness id ordinal) → optional cap.
    /// No RNG anywhere: the same campaign state always yields the same gathering.
    /// </summary>
    public static class WitnessSelector
    {
        /// <param name="witnesses">Full catalog (loaded from muster_witnesses.json).</param>
        /// <param name="day">Current campaign day (gates each witness's day_min).</param>
        /// <param name="eligibility">Campaign-bound port (never null-safe: PassAll used when null).</param>
        /// <param name="maxCount">0 = every eligible witness speaks; N &gt; 0 = capped,
        /// preserving priority order and faction diversity (first witness per faction
        /// wins a slot before second witnesses from any faction).</param>
        public static List<WitnessDelivery> Select(
            IEnumerable<WitnessDefinition> witnesses,
            int day,
            IWitnessEligibility eligibility,
            int maxCount = 0)
        {
            var result = new List<WitnessDelivery>();
            if (witnesses == null) return result;
            var gate = eligibility ?? PassAllWitnessEligibility.Instance;

            var eligible = new List<WitnessDelivery>();
            var seen = new HashSet<string>(StringComparer.Ordinal);
            foreach (var w in witnesses)
            {
                if (w == null || string.IsNullOrEmpty(w.id) || !seen.Add(w.id)) continue;
                if (day >= 0 && day < w.dayMin) continue;
                if (!string.IsNullOrEmpty(w.subjectId) && !gate.IsSubjectAlive(w.subjectId)) continue;
                if (!string.IsNullOrEmpty(w.factionId) && !gate.IsFactionPresent(w.factionId)) continue;
                var testimony = SelectTestimony(w, gate);
                if (testimony == null) continue;
                eligible.Add(new WitnessDelivery { Witness = w, Testimony = testimony });
            }

            // Deterministic order: priority descending, then witness id ordinal.
            eligible.Sort((a, b) =>
            {
                int byPriority = b.Witness.priority.CompareTo(a.Witness.priority);
                if (byPriority != 0) return byPriority;
                return string.CompareOrdinal(a.Witness.id, b.Witness.id);
            });

            if (maxCount <= 0 || eligible.Count <= maxCount)
            {
                result.AddRange(eligible);
                return result;
            }

            // Cap with faction diversity: round-robin by faction group, then the
            // unfactioned, always in the established order within each group.
            var byFaction = new Dictionary<string, List<WitnessDelivery>>(StringComparer.Ordinal);
            var ungrouped = new List<WitnessDelivery>();
            foreach (var d in eligible)
            {
                string key = string.IsNullOrEmpty(d.Witness.factionId) ? string.Empty : d.Witness.factionId;
                if (key.Length == 0) { ungrouped.Add(d); continue; }
                if (!byFaction.TryGetValue(key, out var list))
                    byFaction[key] = list = new List<WitnessDelivery>();
                list.Add(d);
            }
            var factionLists = new List<List<WitnessDelivery>>(byFaction.Values);
            factionLists.Sort((a, b) =>
            {
                int p = b[0].Witness.priority.CompareTo(a[0].Witness.priority);
                if (p != 0) return p;
                return string.CompareOrdinal(a[0].Witness.factionId, b[0].Witness.factionId);
            });

            while (result.Count < maxCount)
            {
                bool progressed = false;
                foreach (var list in factionLists)
                {
                    if (result.Count >= maxCount) break;
                    if (list.Count == 0) continue;
                    result.Add(list[0]);
                    list.RemoveAt(0);
                    progressed = true;
                }
                foreach (var d in ungrouped)
                {
                    if (result.Count >= maxCount) break;
                    result.Add(d);
                    progressed = true;
                }
                ungrouped.Clear();
                if (!progressed) break;
            }
            return result;
        }

        /// <summary>First authored testimony whose flag conditions match; an
        /// unconditional variant is the terminal fallback. Null when conditions
        /// exclude every variant (the witness stays silent).</summary>
        public static WitnessTestimony SelectTestimony(WitnessDefinition witness, IWitnessEligibility gate)
        {
            if (witness == null || witness.testimonies == null) return null;
            for (int i = 0; i < witness.testimonies.Count; i++)
            {
                var t = witness.testimonies[i];
                if (t == null || string.IsNullOrEmpty(t.body)) continue;
                if (!Matches(t, gate)) continue;
                return t;
            }
            return null;
        }

        private static bool Matches(WitnessTestimony t, IWitnessEligibility gate)
        {
            if (t.requiresAnyFlags.Count > 0)
            {
                bool any = false;
                for (int i = 0; i < t.requiresAnyFlags.Count; i++)
                    if (gate.IsFlagSet(t.requiresAnyFlags[i])) { any = true; break; }
                if (!any) return false;
            }
            for (int i = 0; i < t.requiresAllFlags.Count; i++)
                if (!gate.IsFlagSet(t.requiresAllFlags[i])) return false;
            for (int i = 0; i < t.forbidsFlags.Count; i++)
                if (gate.IsFlagSet(t.forbidsFlags[i])) return false;
            return true;
        }
    }
}
