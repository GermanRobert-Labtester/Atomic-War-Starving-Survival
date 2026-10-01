// SPDX-License-Identifier: MIT
// P105/P106 — expedition preparation projection.
//
// A pure, engine-free read model over the canonical expedition definition,
// the existing dispatch estimate, and the shelter inventory. It invents no
// state, owns no save section, and mutates nothing: the panel renders it, the
// player decides. Every threshold here is a projection of authored values
// (definition danger, catalog encounter risk, camp consumption rates) rather
// than a parallel gameplay authority.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Ashfall.Core.Expeditions
{
    /// <summary>Broad lifecycle class of a sortie that has just ended.</summary>
    public enum ExpeditionReturnClass
    {
        /// <summary>The sortie reached the shelter and delivered its salvage.</summary>
        Completed = 0,

        /// <summary>The sortie failed; the survivor came home hurt but alive.</summary>
        Injured = 1,

        /// <summary>The sortie failed fatally; the survivor is lost in the wastes.</summary>
        Lost = 2
    }

    /// <summary>Four-band risk grade for a pre-dispatch projection.</summary>
    public enum ExpeditionRiskGrade
    {
        Low = 0,
        Moderate = 1,
        High = 2,
        Extreme = 3
    }

    /// <summary>One row of the pre-dispatch readiness checklist.</summary>
    public sealed class ExpeditionPrepChecklistItem
    {
        /// <summary>Stable planning key ("food", "water", "rad_meds", "weapon", "light").</summary>
        public string Id = string.Empty;

        /// <summary>Player-facing short label.</summary>
        public string Label = string.Empty;

        /// <summary>Acceptable shelter item ids (empty for equipment checks).</summary>
        public IReadOnlyList<string> ItemIds = Array.Empty<string>();

        /// <summary>Recommended amount from the projected trip.</summary>
        public int Required;

        /// <summary>Amount currently held in the shelter inventory.</summary>
        public int Held;

        /// <summary>True when this row is an equipment readiness check, not a count.</summary>
        public bool IsEquipment;

        /// <summary>
        /// True when the row is a precaution rather than a mechanical gate.
        /// Advisory rows are displayed but never make the plan "not ready",
        /// because no current mechanic requires them (e.g. a light source —
        /// `hasFlashlight` is set but never read by the travel simulation).
        /// </summary>
        public bool Advisory;

        /// <summary>True when the recommendation is met (or not applicable).</summary>
        public bool Satisfied;

        /// <summary>How many units short of the recommendation (0 when satisfied).</summary>
        public int Missing => Math.Max(0, Required - Held);
    }

    /// <summary>
    /// The rendered pre-dispatch plan: expected duration, supply burn, risk
    /// grade, and the readiness checklist. Deterministic; no RNG, no state.
    /// </summary>
    public sealed class ExpeditionPrepPlan
    {
        /// <summary>Checklist rows in fixed order (food, water, rad meds, weapon, light).</summary>
        public List<ExpeditionPrepChecklistItem> Checklist = new List<ExpeditionPrepChecklistItem>();

        /// <summary>Projected round-trip hours from the canonical estimate.</summary>
        public float ProjectedHours;

        /// <summary>Projected round-trip ticks from the canonical estimate.</summary>
        public float ProjectedTicks;

        /// <summary>Recommended food units for the projected trip.</summary>
        public int FoodBurn;

        /// <summary>Recommended water units for the projected trip.</summary>
        public int WaterBurn;

        /// <summary>Four-band risk grade.</summary>
        public ExpeditionRiskGrade Risk;

        /// <summary>Dominant driver behind <see cref="Risk"/>, e.g. "danger lvl 3".</summary>
        public string RiskNote = string.Empty;

        /// <summary>Overnight windows represented by the projected trip.</summary>
        public int OvernightNights;

        /// <summary>Firewood units the authored camp would consume (CampState constants).</summary>
        public float CampFirewoodUnits;

        /// <summary>Water units the authored camp would consume.</summary>
        public float CampWaterUnits;

        /// <summary>Food units the authored camp would consume.</summary>
        public float CampFoodUnits;

        /// <summary>True when every applicable (non-advisory) checklist row is satisfied.</summary>
        public bool Ready
        {
            get
            {
                for (int i = 0; i < Checklist.Count; i++)
                    if (!Checklist[i].Advisory && !Checklist[i].Satisfied) return false;
                return true;
            }
        }

        /// <summary>Number of unsatisfied, applicable (non-advisory) rows.</summary>
        public int MissingCount
        {
            get
            {
                int n = 0;
                for (int i = 0; i < Checklist.Count; i++)
                    if (!Checklist[i].Advisory && !Checklist[i].Satisfied) n++;
                return n;
            }
        }

        /// <summary>Single-line readiness glyph string, e.g. "[OK] FOOD 3/3 · [!] LIGHT 0/1".</summary>
        public string ChecklistLine()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < Checklist.Count; i++)
            {
                if (i > 0) sb.Append(" · ");
                var row = Checklist[i];
                sb.Append(row.Satisfied ? "[OK] " : "[!] ");
                sb.Append(row.Label.ToUpperInvariant());
                if (!row.IsEquipment) sb.Append(' ').Append(row.Held).Append('/').Append(row.Required);
                if (row.Advisory) sb.Append(" (adv)");
            }
            return sb.ToString();
        }

        /// <summary>Risk-grade word, uppercase.</summary>
        public string RiskLabel() => Risk switch
        {
            ExpeditionRiskGrade.Low => "LOW",
            ExpeditionRiskGrade.Moderate => "MODERATE",
            ExpeditionRiskGrade.High => "HIGH",
            _ => "EXTREME"
        };

        /// <summary>
        /// One projected planning line for the panel: duration, supply burn,
        /// risk band and its dominant driver, and the readiness checklist.
        /// </summary>
        public string Summary()
        {
            var ci = CultureInfo.InvariantCulture;
            string risk = RiskLabel();
            if (!string.IsNullOrEmpty(RiskNote)) risk += $" ({RiskNote})";
            string head = string.Format(
                ci,
                "PREP · ~{0:0.#} h / {1:0.#} ticks · burn food {2} · water {3} · risk {4}{5}",
                ProjectedHours,
                ProjectedTicks,
                FoodBurn,
                WaterBurn,
                risk,
                Ready ? " · READY" : $" · {MissingCount} SHORT");
            return head + "\n" + ChecklistLine();
        }

        /// <summary>
        /// Overnight camp burn line, or empty when the projection has no
        /// overnight window. Uses the authored camp consumption constants.
        /// </summary>
        public string OvernightLine()
        {
            if (OvernightNights <= 0) return string.Empty;
            var ci = CultureInfo.InvariantCulture;
            return string.Format(
                ci,
                "CAMP BURN · {0} night(s) · firewood {1:0.#} · water {2:0.#} · food {3:0.#}",
                OvernightNights, CampFirewoodUnits, CampWaterUnits, CampFoodUnits);
        }
    }

    /// <summary>
    /// Pure projection that turns a dispatch estimate plus shelter inventory
    /// into an advisory loadout checklist and risk grade.
    /// </summary>
    public static class ExpeditionPrepPlanner
    {
        /// <summary>Primary food item id (used for the burn label and docs).</summary>
        public const string FoodItemId = "canned_food";

        /// <summary>Primary water item id.</summary>
        public const string WaterItemId = "clean_water";

        /// <summary>Primary rad-countermeasure item id.</summary>
        public const string RadMedItemId = "anti_rad";

        /// <summary>Light source item id.</summary>
        public const string LightItemId = "headlamp";

        /// <summary>Acceptable food ids, first is canonical.</summary>
        public static readonly string[] FoodItemIds = { "canned_food", "dried_rations", "military_rations" };

        /// <summary>Acceptable potable water ids, first is canonical.</summary>
        public static readonly string[] WaterItemIds = { "clean_water", "clean_water_jug" };

        /// <summary>Acceptable rad-countermeasure ids, first is canonical.</summary>
        public static readonly string[] RadMedItemIds = { "anti_rad", "iodine_pills", "iodine_tablets", "rad_away" };

        // Camp consumption (ExpeditionSystem): 4 night segments × 0.5 units per
        // segment = 2 food and 2 water per overnight window. Twelve hours is the
        // authored travel+rest window used to convert the estimate into nights.
        /// <summary>Food/water units consumed per overnight window.</summary>
        public const float UnitsPerOvernight = 2f;

        /// <summary>Hours represented by one overnight window.</summary>
        public const float HoursPerOvernight = 12f;

        /// <summary>
        /// Upper bound on projected overnight windows. A sanity clamp so an
        /// absurd authored distance cannot produce an unbounded burn figure;
        /// trips longer than this are already far past any dispatchable range.
        /// </summary>
        public const int MaxOvernightNights = 30;

        /// <summary>Risk note returned when no meaningful driver exists; callers
        /// suppress it to keep low-danger rows quiet.</summary>
        public const string LowRiskNote = "low danger";

        /// <summary>Health lost when a sortie fails non-fatally.</summary>
        public const float FailureInjuryHealthLoss = 18f;

        /// <summary>
        /// Build the advisory pre-dispatch plan. <paramref name="countById"/>
        /// reads the shelter inventory; <paramref name="weaponReady"/> and
        /// <paramref name="hasLight"/> come from the equipment/inventory owners
        /// and are never recomputed here.
        /// </summary>
        public static ExpeditionPrepPlan Build(
            ExpeditionDefinition def,
            ExpeditionEstimate estimate,
            Func<string, int> countById,
            bool weaponReady,
            bool hasLight)
        {
            if (def == null) throw new ArgumentNullException(nameof(def));
            if (estimate == null) throw new ArgumentNullException(nameof(estimate));
            if (countById == null) countById = _ => 0;

            float hours = estimate.projectedTripHours > 0f
                ? estimate.projectedTripHours
                : Math.Max(1f, estimate.totalTicks);
            int nights = Math.Max(1, (int)Math.Ceiling(hours / HoursPerOvernight));
            nights = Math.Min(nights, MaxOvernightNights);
            int burn = Math.Max(1, (int)Math.Ceiling(nights * UnitsPerOvernight));

            var plan = new ExpeditionPrepPlan
            {
                ProjectedHours = hours,
                ProjectedTicks = estimate.totalTicks,
                FoodBurn = burn,
                WaterBurn = burn,
                Risk = GradeRisk(def, estimate),
                RiskNote = DescribeRiskDriver(def, estimate),
                OvernightNights = nights,
                CampFirewoodUnits = nights * ExpeditionSystem.CampFirewoodPerSegment * ExpeditionSystem.CampNightSegments,
                CampWaterUnits = nights * ExpeditionSystem.CampWaterPerSegment * ExpeditionSystem.CampNightSegments,
                CampFoodUnits = nights * ExpeditionSystem.CampFoodPerSegment * ExpeditionSystem.CampNightSegments
            };

            plan.Checklist.Add(CountItem("food", "Food", FoodItemIds, burn, countById));
            plan.Checklist.Add(CountItem("water", "Water", WaterItemIds, burn, countById));

            // Radiation countermeasures are only recommended when the canonical
            // estimate projects a dose. No dose → not applicable, never a nag.
            bool radNeeded = estimate.projectedDoseTotal > 0f || estimate.unprotectedCount > 0;
            int radRequired = radNeeded ? 1 : 0;
            plan.Checklist.Add(CountItem("rad_meds", "Rad meds", RadMedItemIds, radRequired, countById));

            plan.Checklist.Add(EquipmentItem("weapon", "Weapon", weaponReady, required: def.dangerLevel >= 1));
            // A light source is a sensible precaution, but no live mechanic
            // consumes it (hasFlashlight is written and never read), so it is
            // advisory: displayed, never a readiness gate.
            plan.Checklist.Add(EquipmentItem("light", "Light", hasLight, required: true, advisory: true));

            return plan;
        }

        private static ExpeditionPrepChecklistItem CountItem(
            string id, string label, string[] itemIds, int required, Func<string, int> countById)
        {
            int held = 0;
            for (int i = 0; i < itemIds.Length; i++)
                held += Math.Max(0, countById(itemIds[i]));

            return new ExpeditionPrepChecklistItem
            {
                Id = id,
                Label = label,
                ItemIds = itemIds,
                Required = required,
                Held = held,
                IsEquipment = false,
                Satisfied = held >= required
            };
        }

        private static ExpeditionPrepChecklistItem EquipmentItem(string id, string label, bool ready, bool required, bool advisory = false)
        {
            return new ExpeditionPrepChecklistItem
            {
                Id = id,
                Label = label,
                ItemIds = Array.Empty<string>(),
                Required = required ? 1 : 0,
                Held = ready && required ? 1 : 0,
                IsEquipment = true,
                Advisory = advisory,
                Satisfied = !required || ready
            };
        }

        /// <summary>
        /// Grade the projected risk from authored definition danger plus the
        /// estimate's encounter, breakdown, dose, and mid-route-failure signals.
        /// </summary>
        public static ExpeditionRiskGrade GradeRisk(ExpeditionDefinition def, ExpeditionEstimate estimate)
        {
            if (def == null || estimate == null) return ExpeditionRiskGrade.Low;

            int score = Math.Max(0, def.dangerLevel);
            if (estimate.encounterRiskPerTick >= 0.20f) score++;
            if (estimate.breakdownRiskTotal >= 0.15f) score++;
            if (estimate.projectedDoseTotal >= 20f) score++;
            if (estimate.predictsMidRouteFailure) score++;

            if (score <= 1) return ExpeditionRiskGrade.Low;
            if (score <= 3) return ExpeditionRiskGrade.Moderate;
            if (score <= 5) return ExpeditionRiskGrade.High;
            return ExpeditionRiskGrade.Extreme;
        }

        /// <summary>
        /// Name the largest single contributor to the risk grade so the player
        /// can see <em>why</em> a run reads HIGH/EXTREME, not just that it does.
        /// </summary>
        public static string DescribeRiskDriver(ExpeditionDefinition def, ExpeditionEstimate estimate)
        {
            if (def == null || estimate == null) return string.Empty;

            string note = Math.Max(0, def.dangerLevel) > 0 ? $"danger lvl {def.dangerLevel}" : "low danger";
            int best = Math.Max(0, def.dangerLevel);

            if (estimate.encounterRiskPerTick >= 0.20f && 2 > best)
            {
                best = 2;
                note = $"encounter {estimate.encounterRiskPerTick:P0}/tick";
            }
            if (estimate.breakdownRiskTotal >= 0.15f && 2 > best)
            {
                best = 2;
                note = $"breakdown {estimate.breakdownRiskTotal:P0}";
            }
            if (estimate.projectedDoseTotal >= 20f && 2 > best)
            {
                best = 2;
                note = $"dose {estimate.projectedDoseTotal:0} mSv";
            }
            if (estimate.predictsMidRouteFailure && 2 > best)
            {
                best = 2;
                note = "gear fails mid-route";
            }
            return note;
        }

        /// <summary>
        /// Name the dominant risk driver for an already-dispatched sortie from
        /// the live <see cref="ExpeditionState"/> (MapPanel active-sortie card).
        /// Mirrors <see cref="DescribeRiskDriver"/> without needing a re-estimate.
        /// </summary>
        public static string DescribeStateRisk(ExpeditionState state)
        {
            if (state == null) return string.Empty;

            int score = Math.Max(0, state.dangerLevel);
            string note = state.dangerLevel > 0 ? $"danger lvl {state.dangerLevel}" : LowRiskNote;

            if (state.encounterChancePerTick >= 0.20f && 2 > score)
            {
                score = 2;
                note = $"encounter {state.encounterChancePerTick:P0}/tick";
            }
            if (state.vehicleBrokenDown && 2 > score)
            {
                score = 2;
                note = "vehicle disabled";
            }
            return note;
        }

        /// <summary>
        /// Health lost by a failed sortie. Both authored failure sources
        /// (tick exhaustion, overnight collapse) are deprivation/injury events;
        /// an unknown reason still costs something so a failure is never free.
        /// </summary>
        public static float FailureHealthLoss(string reason)
        {
            if (string.IsNullOrWhiteSpace(reason)) return 12f;
            if (reason.IndexOf("exhaustion", StringComparison.OrdinalIgnoreCase) >= 0) return FailureInjuryHealthLoss;
            if (reason.IndexOf("camp", StringComparison.OrdinalIgnoreCase) >= 0) return FailureInjuryHealthLoss;
            return 12f;
        }

        /// <summary>
        /// Resolve whether a failed sortie leaves the survivor injured or lost.
        /// There is no "missing" state in ASHFALL (see <c>SurvivorLifecycleState</c>
        /// docs); a survivor lost in the wastes is recorded through the existing
        /// fate owner with <c>SurvivorDeathCause.Expedition</c>.
        /// </summary>
        public static ExpeditionReturnClass ResolveFailureOutcome(string reason, float healthBefore, float healthLoss)
        {
            return healthBefore - Math.Max(0f, healthLoss) <= 0f
                ? ExpeditionReturnClass.Lost
                : ExpeditionReturnClass.Injured;
        }
    }

    /// <summary>
    /// Shared, engine-free formatter for the return-loot ceremony and the
    /// failure aftermath. Used by both the host session (so the summary
    /// survives an unbound panel) and the panel (so the text has one owner).
    /// </summary>
    public static class ExpeditionReturnReport
    {
        /// <summary>Itemised ceremony for a returned sortie.</summary>
        public static string BuildCeremony(ExpeditionState state, Func<string, string>? itemName)
        {
            if (state == null) return string.Empty;
            var sb = new StringBuilder();
            sb.Append("RETURN LOOT CEREMONY");
            sb.Append(" — ").Append(DisplayName(state.survivorId))
              .Append(" returned from ").Append(state.displayName).Append('.');

            if (state.loot == null || state.loot.Count == 0)
            {
                sb.Append("\nNo salvage recovered. Stores unchanged.");
                return sb.ToString();
            }

            var aggregated = new Dictionary<string, (int qty, float weight)>(StringComparer.OrdinalIgnoreCase);
            int totalQty = 0;
            float totalWeight = 0f;
            for (int i = 0; i < state.loot.Count; i++)
            {
                var entry = state.loot[i];
                // Same defensive floor as the depositor: a valid id with a
                // non-positive quantity is still one recovered unit, so the
                // ceremony never names less than the inventory receives.
                if (entry == null || string.IsNullOrEmpty(entry.itemId)) continue;
                int qty = Math.Max(1, entry.quantity);
                aggregated.TryGetValue(entry.itemId, out var prior);
                aggregated[entry.itemId] = (prior.qty + qty, prior.weight + entry.weightKg);
                totalQty += qty;
                totalWeight += entry.weightKg;
            }

            if (totalQty == 0)
            {
                sb.Append("\nNo salvage recovered. Stores unchanged.");
                return sb.ToString();
            }

            var parts = new List<string>();
            foreach (var kv in aggregated)
                parts.Add($"{ResolveName(itemName, kv.Key)} ×{kv.Value.qty}");
            sb.Append('\n').Append(string.Join(" · ", parts));
            sb.Append($"\nDeposited to shelter stores — {totalQty} item(s), {totalWeight:0.#} kg.");
            return sb.ToString();
        }

        /// <summary>Failure aftermath for a lost sortie, naming the reason.</summary>
        public static string BuildAftermath(ExpeditionState state, string reason)
        {
            if (state == null) return string.Empty;
            var sb = new StringBuilder();
            sb.Append("SORTIE AFTERMATH");
            sb.Append(" — ").Append(DisplayName(state.survivorId))
              .Append(" at ").Append(state.displayName).Append('.');
            sb.Append('\n').Append(string.IsNullOrWhiteSpace(reason) ? "The sortie failed." : reason);
            if (state.loot != null && state.loot.Count > 0)
                sb.Append($"\n{state.loot.Count} salvage line(s) lost in the field — not deposited.");
            return sb.ToString();
        }

        private static string ResolveName(Func<string, string>? itemName, string id)
        {
            string? resolved = itemName?.Invoke(id);
            return string.IsNullOrWhiteSpace(resolved) ? HumanizeToken(id) : resolved!;
        }

        private static string DisplayName(string id)
        {
            if (string.IsNullOrEmpty(id)) return "[UNNAMED]";
            return id.Replace("survivor_", "").Replace("_", " ").ToUpperInvariant();
        }

        /// <summary>Stable-id → Title Case fallback when the item catalog has no record.</summary>
        public static string HumanizeToken(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return string.Empty;
            string[] parts = id.Split('_', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i].Length == 0) continue;
                parts[i] = char.ToUpperInvariant(parts[i][0]) + (parts[i].Length > 1 ? parts[i][1..] : string.Empty);
            }
            return string.Join(" ", parts);
        }
    }
}
