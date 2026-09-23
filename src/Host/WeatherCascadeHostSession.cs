// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Records;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;
using Ashfall.Core.Weather;
using Ashfall.Core.World;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// What the weather cascade did to one canonical owner for one front.
    /// Reporting only — the owners themselves hold every mutated fact.
    /// </summary>
    public sealed class WeatherCascadeContribution
    {
        public readonly string TargetSystem;
        public readonly string OwnerName;
        public readonly string Detail;

        public WeatherCascadeContribution(string targetSystem, string ownerName, string detail)
        {
            TargetSystem = targetSystem ?? string.Empty;
            OwnerName = ownerName ?? string.Empty;
            Detail = detail ?? string.Empty;
        }
    }

    /// <summary>
    /// One weather front's cascade outcome: the severity the canonical weather
    /// table says it carries, the effects it produced, and what the canonical
    /// owners actually did with them.
    /// </summary>
    public sealed class WeatherCascadeOutcome
    {
        public readonly string EventId;
        public readonly WeatherKind Kind;
        public readonly float Severity;
        public readonly int EffectCount;
        public readonly int Day;
        public readonly List<WeatherCascadeContribution> Contributions = new List<WeatherCascadeContribution>();

        public WeatherCascadeOutcome(string eventId, WeatherKind kind, float severity, int effectCount, int day)
        {
            EventId = eventId ?? string.Empty;
            Kind = kind;
            Severity = severity;
            EffectCount = effectCount;
            Day = day;
        }

        public string Describe() =>
            $"{Kind} cascade day {Day}: severity {Severity:0.#}, {EffectCount} effect(s), "
            + $"{Contributions.Count} canonical contribution(s)";
    }

    /// <summary>
    /// Plan 135 / C2[27] host session — turns weather from a number into a
    /// driver by binding the Core <see cref="WeatherCascadeSystem"/> to the
    /// canonical owners that weather is supposed to pressure.
    /// <para>
    /// Authority boundary (deliberate). This session owns NO weather, shelter,
    /// expedition, market, morale, or accessibility fact. It holds only the
    /// cascade's own event/effect ledger (which is what the
    /// <c>weather_cascade</c> save section persists), plus a set of per-front
    /// transient multipliers it hands to owners and withdraws when the front
    /// passes:
    /// <list type="bullet">
    /// <item>severity is read from <see cref="WeatherEffectsCatalog"/> — the same
    /// table <c>WeatherSystem</c> already uses for rad/visibility/thermal/travel,
    /// so the cascade cannot disagree with the weather the player is in;</item>
    /// <item>shelter stress goes to <see cref="DisasterResponseSystem"/> (the
    /// canonical resilience owner) — never to a local integrity counter;</item>
    /// <item>expedition delay goes to <see cref="ExpeditionSystem"/>'s existing
    /// stamina-drain and encounter-chance multiplier seams;</item>
    /// <item>price shocks go to <see cref="MarketSystem.ApplyShock"/> (clamped,
    /// idempotent per source) — never to a second price table;</item>
    /// <item>morale stress goes to <see cref="NeedsSystem.SetExternalModifier"/>
    /// bounded to the effect's duration;</item>
    /// <item>location accessibility is REPORTED, not stored: route availability
    /// is derived from the gate evaluator at presentation time, so recording a
    /// parallel accessibility ledger would create exactly the second authority
    /// this plan's cascade is meant to route through existing owners.</item>
    /// </list>
    /// </para>
    /// </summary>
    public sealed class WeatherCascadeHostSession : HostSessionBase
    {
        public const string CascadeSystemId = WeatherCascadeSystem.SystemId;

        private readonly List<WeatherCascadeOutcome> _history = new List<WeatherCascadeOutcome>();
        private readonly List<string> _loadErrors = new List<string>();

        /// <summary>Active weather fronts this session installed, keyed by event id.</summary>
        private readonly Dictionary<string, ActiveFront> _fronts =
            new Dictionary<string, ActiveFront>(StringComparer.Ordinal);

        private sealed class ActiveFront
        {
            public string EventId = string.Empty;
            public string Region = string.Empty;
            public int StartDay;
            public int DurationDays;
            public float Severity;
            public float EncounterMultiplier = 1f;
            public bool HasEncounter;
            public bool HasMorale;
            public string MoraleSourceId = string.Empty;
        }

        public WeatherCascadeSystem System { get; }
        public WeatherEffectsCatalog? EffectsCatalog { get; private set; }

        /// <summary>Canonical owners. Bound by the host; null = unbound seam.</summary>
        public DisasterResponseSystem? ShelterResilience { get; set; }
        public ExpeditionSystem? Expeditions { get; set; }
        public MarketSystem? Market { get; set; }
        public NeedsSystem? Needs { get; set; }

        /// <summary>True when the authored cascade table loaded and was bound.</summary>
        public bool UsingAuthoredTemplates { get; private set; }

        /// <summary>Collected load errors; empty when the table is bound.</summary>
        public IReadOnlyList<string> LoadErrors => _loadErrors;

        public IReadOnlyList<WeatherCascadeOutcome> History => _history;

        /// <summary>Day-stamped region accessibility report (report-only, derived).</summary>
        public IReadOnlyList<string> AccessibilityReport => _accessibility;
        private readonly List<string> _accessibility = new List<string>();

        public WeatherCascadeHostSession(WeatherCascadeSystem? system = null)
        {
            System = system ?? new WeatherCascadeSystem();
            System.OnCascadeEvaluated += (_, _) => RequestPresentationRefresh();
        }

        // ── Loading ────────────────────────────────────────────────────

        /// <summary>Bind the authored cascade template table. A missing, empty,
        /// or invalid table is a hard error (never a fall back to generated
        /// effects, which would invent the cascade the plan asks to author).</summary>
        public bool LoadAuthoredTemplates(string dataDirectory, IFileIO files)
        {
            _loadErrors.Clear();
            var load = WeatherCascadeCatalogLoader.Load(dataDirectory, files);
            if (load.HasErrors)
            {
                _loadErrors.AddRange(load.Errors);
                UsingAuthoredTemplates = false;
                return false;
            }
            int bound = System.Engine.BindValidatedTemplates(load.Templates);
            if (bound != load.Templates.Count)
            {
                _loadErrors.Add(
                    $"WeatherCascadeHostSession: bound {bound} of {load.Templates.Count} validated template(s).");
                UsingAuthoredTemplates = false;
                return false;
            }
            UsingAuthoredTemplates = true;
            return true;
        }

        /// <summary>Bind the canonical weather-effects table the severity index reads.</summary>
        public void BindEffectsCatalog(WeatherEffectsCatalog? catalog) => EffectsCatalog = catalog;

        // ── Front evaluation ───────────────────────────────────────────

        /// <summary>
        /// Evaluate one weather front: derive the canonical severity, run the
        /// Core authority, then route each produced effect to its canonical owner.
        /// </summary>
        public WeatherCascadeOutcome TriggerFront(WeatherKind kind, int day, IReadOnlyList<string>? regions = null)
        {
            float severity = WeatherCascadeSeverity.SeverityFor(kind, EffectsCatalog);
            // A mechanically neutral front carries no cascade. Skipping it here
            // (rather than firing a zero-severity event) matters: the engine's
            // own fallback would otherwise generate a morale lift for a clear
            // day, which is a fabricated cascade the authored table never said
            // to produce.
            if (WeatherCascadeSeverity.IsMechanicallyNeutral(kind, EffectsCatalog) || severity <= 0.0001f)
            {
                return new WeatherCascadeOutcome(
                    $"(neutral)_{kind.ToString().ToLowerInvariant()}", kind, 0f, 0, day);
            }

            System.FortificationLevel = ShelterFortificationLevel;
            var ev = System.TriggerCascade(kind, severity, day, regions);
            var outcome = new WeatherCascadeOutcome(ev.id, kind, severity, ev.effects.Count, day);

            string region = ev.affectedRegions.Count > 0 ? ev.affectedRegions[0] : "holdfast_core_sector";
            var front = new ActiveFront
            {
                EventId = ev.id,
                Region = region,
                StartDay = ev.startDay,
                Severity = ev.severity,
                DurationDays = Math.Max(1, ev.durationDays),
                MoraleSourceId = $"weather_stress_{ev.id}"
            };

            foreach (var eff in ev.effects)
                RouteEffect(eff, region, day, front, outcome);

            if (front.HasEncounter && Expeditions != null)
            {
                // Per-location encounter multiplier is the expedition owner's own
                // seam; the front owns the number, the owner owns the roll.
                Expeditions.SetEncounterChanceMultiplier(id =>
                    string.Equals(id, front.Region, StringComparison.OrdinalIgnoreCase)
                        ? front.EncounterMultiplier
                        : 1f);
            }

            _fronts[ev.id] = front;
            _history.Add(outcome);
            RaiseStateChanged();
            return outcome;
        }

        /// <summary>Shelter fortification the cascade reads to mitigate storm
        /// damage (0 = unfortified, 1 = reinforced, 2+ = damage eliminated).
        /// Sourced from the canonical resilience owner when bound.</summary>
        public int ShelterFortificationLevel
        {
            get
            {
                if (ShelterResilience == null) return 0;
                double resilience = ShelterResilience.ResilienceRating;
                if (resilience >= 80.0) return 2;
                if (resilience >= 50.0) return 1;
                return 0;
            }
        }

        private void RouteEffect(
            WeatherEffect eff, string region, int day, ActiveFront front, WeatherCascadeOutcome outcome)
        {
            switch (eff.targetSystem)
            {
                case CascadeTargetSystem.Shelter:
                    RouteShelter(eff, day, outcome);
                    break;

                case CascadeTargetSystem.Expedition:
                    RouteExpedition(eff, front, outcome);
                    break;

                case CascadeTargetSystem.Economy:
                    RouteEconomy(eff, day, outcome);
                    break;

                case CascadeTargetSystem.MentalHealth:
                    RouteMentalHealth(eff, front, day, outcome);
                    break;

                case CascadeTargetSystem.Location:
                    RouteLocation(eff, region, day, outcome);
                    break;

                case CascadeTargetSystem.Faction:
                    // Faction behavior change is reported only: the authoritative
                    // faction-operation owner (FactionWarSystem + territory control)
                    // already gates operations on its own conditions, and this
                    // cascade has no standing-operation ledger to pause.
                    outcome.Contributions.Add(new WeatherCascadeContribution(
                        "faction", "(reported)", eff.description));
                    break;
            }
        }

        private void RouteShelter(WeatherEffect eff, int day, WeatherCascadeOutcome outcome)
        {
            var shelter = ShelterResilience;
            if (shelter == null)
            {
                outcome.Contributions.Add(new WeatherCascadeContribution(
                    "shelter", "(unbound)", eff.description));
                return;
            }

            switch (eff.effectType)
            {
                case CascadeEffectType.Damage:
                    // Storm shear lowers the canonical shelter resilience. The
                    // disaster-response owner owns the number; the cascade only
                    // supplies the magnitude its authority derived.
                    shelter.AdjustResilience(-eff.magnitude);
                    outcome.Contributions.Add(new WeatherCascadeContribution(
                        "shelter", nameof(DisasterResponseSystem),
                        $"resilience {shelter.ResilienceRating:0.#} after -{eff.magnitude:0.#} "
                        + $"{eff.description}"));
                    break;

                case CascadeEffectType.FiltrationStress:
                case CascadeEffectType.ThermalLoad:
                case CascadeEffectType.PowerOutput:
                    // These already reach the shelter through the canonical
                    // weather path (WeatherSystem.TemperaturePenaltyC, the dose
                    // ledger's filtration, and the power grid). The cascade
                    // reports them rather than applying a second copy.
                    outcome.Contributions.Add(new WeatherCascadeContribution(
                        "shelter", nameof(DisasterResponseSystem) + " (via canonical weather path)",
                        $"{eff.effectType} {eff.magnitude:0.#} — {eff.description}"));
                    break;

                case CascadeEffectType.Accessibility:
                    // Black-rain sump flooding: rise the canonical sump/flooding
                    // pressure only through resilience, not a second water level.
                    shelter.AdjustResilience(-(eff.magnitude * 0.05f));
                    outcome.Contributions.Add(new WeatherCascadeContribution(
                        "shelter", nameof(DisasterResponseSystem),
                        $"flooding pressure {eff.magnitude:0.#} -> resilience {shelter.ResilienceRating:0.#}"));
                    break;
            }
        }

        private void RouteExpedition(WeatherEffect eff, ActiveFront front, WeatherCascadeOutcome outcome)
        {
            if (Expeditions == null)
            {
                outcome.Contributions.Add(new WeatherCascadeContribution(
                    "expedition", "(unbound)", eff.description));
                return;
            }

            switch (eff.effectType)
            {
                case CascadeEffectType.Delay:
                case CascadeEffectType.Damage:
                case CascadeEffectType.HazardSurge:
                    // Encounter multiplier, expressed as a rate multiplier from
                    // the front's own magnitude. Clamped to the expedition
                    // owner's own bounded range by the owner when it rolls.
                    float boost = 1f + Math.Max(0f, eff.magnitude) / 100f;
                    if (boost > front.EncounterMultiplier)
                    {
                        front.EncounterMultiplier = boost;
                        front.HasEncounter = true;
                    }
                    outcome.Contributions.Add(new WeatherCascadeContribution(
                        "expedition", nameof(ExpeditionSystem),
                        $"region {front.Region} encounter x{front.EncounterMultiplier:0.00} — {eff.description}"));
                    break;
            }
        }

        private void RouteEconomy(WeatherEffect eff, int day, WeatherCascadeOutcome outcome)
        {
            var market = Market;
            if (market == null)
            {
                outcome.Contributions.Add(new WeatherCascadeContribution(
                    "economy", "(unbound)", eff.description));
                return;
            }

            // The canonical market shock seam is the only way a price change is
            // allowed to reach players. Its accepted unit is basis points with
            // its own clamp, so the front's 0..100 magnitude is projected into
            // that range by the market owner's converter rather than by us.
            float severityBp = MarketSystem.ShockSeverityMinBp
                + (MarketSystem.ShockSeverityMaxBp - MarketSystem.ShockSeverityMinBp)
                  * Math.Clamp(eff.magnitude / 100f, 0f, 1f);

            var shock = market.ApplyShock(
                "fuel_and_supplies", isShortage: true, severityBp, day, Math.Max(1, eff.durationDays), $"weather_{eff.effectId}");
            if (shock == null)
            {
                outcome.Contributions.Add(new WeatherCascadeContribution(
                    "economy", nameof(MarketSystem),
                    $"shock refused for {eff.description} (unknown category or empty market)."));
            }
            else
            {
                outcome.Contributions.Add(new WeatherCascadeContribution(
                    "economy", nameof(MarketSystem),
                    $"shock {shock.shockId} {shock.severityBp:0}bp for {eff.durationDays}d — {eff.description}"));
            }
        }

        private void RouteMentalHealth(WeatherEffect eff, ActiveFront front, int day, WeatherCascadeOutcome outcome)
        {
            var needs = Needs;
            if (needs == null)
            {
                outcome.Contributions.Add(new WeatherCascadeContribution(
                    "mental_health", "(unbound)", eff.description));
                return;
            }

            // Morale is the canonical need the mental-health owner itself reads
            // and restores. The stress is installed as a duration-bound external
            // modifier so the needs system owns the per-hour application and the
            // expiry, and the cascade never writes a morale value directly.
            bool negative = eff.magnitude < 0f;
            float perHour = MathF.Abs(eff.magnitude) / 24f;
            if (perHour <= 0f)
            {
                outcome.Contributions.Add(new WeatherCascadeContribution(
                    "mental_health", nameof(NeedsSystem), $"zero-magnitude stress skipped ({eff.description})."));
                return;
            }

            int applied = 0;
            foreach (var survivor in needs.Registered)
            {
                if (survivor == null || !survivor.IsAliveState) continue;
                needs.SetExternalModifier(
                    survivor.Id, front.MoraleSourceId, NeedKind.Morale,
                    negative ? -perHour : perHour, priority: 10,
                    startDay: day, endDay: day + Math.Max(1, eff.durationDays) - 1);
                applied++;
            }

            front.HasMorale = true;
            outcome.Contributions.Add(new WeatherCascadeContribution(
                "mental_health", nameof(NeedsSystem),
                $"{applied} survivor(s) morale {(negative ? "-" : "+")}{perHour:0.###}/h for "
                + $"{Math.Max(1, eff.durationDays)}d — {eff.description}"));
        }

        private void RouteLocation(WeatherEffect eff, string region, int day, WeatherCascadeOutcome outcome)
        {
            // Report only. Route availability is derived from the gate evaluator
            // at presentation time (RouteAvailabilityKind is never serialized), so
            // recording an accessibility ledger here would fork that authority.
            bool blocked = eff.effectType == CascadeEffectType.Accessibility && eff.magnitude > 0f
                ? false
                : true;
            string note = $"day {day} region {region}: {(blocked ? "closed" : "open")} — {eff.description}";
            lock (_accessibility) _accessibility.Add(note);
            outcome.Contributions.Add(new WeatherCascadeContribution(
                "location", "(reported — derived at presentation)", note));
        }

        // ── Day tick ───────────────────────────────────────────────────

        /// <summary>
        /// Advance the cascade clock: expire fronts whose duration ended and
        /// withdraw their contribution from the canonical owners that accepted
        /// one, then let the Core authority recompute its active-effect set.
        /// </summary>
        public List<WeatherCascadeOutcome> TickDay(int day)
        {
            System.TickDay(day);
            var expired = new List<WeatherCascadeOutcome>();

            var due = new List<string>();
            foreach (var kv in _fronts)
            {
                if (day >= kv.Value.StartDay + kv.Value.DurationDays) due.Add(kv.Key);
            }

            foreach (string eventId in due)
            {
                var front = _fronts[eventId];
                _fronts.Remove(eventId);
                WithdrawFront(front, day);
                expired.Add(new WeatherCascadeOutcome(
                    front.EventId, default, front.Severity, 0, day));
            }

            if (due.Count > 0 && Expeditions != null)
            {
                // With the last region-local modifier withdrawn, the owner's own
                // default (1.0 everywhere) is the truthful state.
                Expeditions.SetEncounterChanceMultiplier(_ => 1f);
            }

            return expired;
        }

        private void WithdrawFront(ActiveFront front, int day)
        {
            if (front.HasMorale && Needs != null)
            {
                foreach (var survivor in Needs.Registered)
                {
                    if (survivor == null || !survivor.IsAliveState) continue;
                    Needs.RemoveExternalModifier(survivor.Id, front.MoraleSourceId, NeedKind.Morale);
                }
            }
            RaiseStateChanged();
        }

        // ── Reporting ──────────────────────────────────────────────────

        public string StatusLine()
        {
            int activeEvents = System.State.activeEvents.Count;
            int activeEffects = System.State.activeEffects.Count;
            return $"weather cascade: {activeEvents} active front(s), {activeEffects} effect(s), "
                + $"{_history.Count} evaluated, {_fronts.Count} installed";
        }
    }

    public static class WeatherCascadeSaveStore
    {
        public const string FileName = "weather_cascade_save.json";
        public const string SectionName = "weather_cascade";

        private static readonly SaveStore<WeatherCascadeState> s_store =
            SaveStoreHub.Checksummed<WeatherCascadeState>(FileName, nameof(WeatherCascadeSaveStore));

        public static bool TrySave(WeatherCascadeState state) => s_store.TrySave(state);
        public static WeatherCascadeState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(WeatherCascadeState state) => s_store.CapturePersisted(state);
        public static WeatherCascadeState? TryRestore(string json) => s_store.RestoreEnvelope(json);
        public static WeatherCascadeState? TryRestoreBare(string json) => s_store.RestoreBare(json);
    }
}
