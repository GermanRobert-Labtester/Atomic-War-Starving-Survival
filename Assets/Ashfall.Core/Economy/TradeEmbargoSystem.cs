// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Economy
{
    // ── Domain rules (loaded from trade_embargoes.json or registered) ──────

    /// <summary>
    /// Plan 14A — one weather-driven embargo rule. A rule is ACTIVE while the
    /// current world weather equals its weather kind; while active it can
    /// block or slow caravan routes in its affected regions and surcharge its
    /// affected goods. When the weather clears, the price shock decays to
    /// neutral over <see cref="DecayDays"/> (routes recover instantly — a
    /// blocked road does not stay muddy after the storm).
    /// </summary>
    public sealed class EmbargoRule
    {
        public string RuleId = string.Empty;
        public WeatherKind Weather;
        /// <summary>Regions the rule covers. "*" covers every known region; otherwise a RegionalSupplyRouter supply tag.</summary>
        public IReadOnlyList<string> AffectedRegions { get; }
        /// <summary>GoodCategories ids surcharged by the rule (may be empty).</summary>
        public IReadOnlyList<string> AffectedCategories { get; }
        /// <summary>Goods-catalog item ids surcharged by the rule (may be empty).</summary>
        public IReadOnlyList<string> AffectedItemIds { get; }
        /// <summary>True when the rule surcharges every good (category-agnostic shocks such as RadHail).</summary>
        public bool AffectsAllGoods { get; }
        /// <summary>Price multiplier while active, permille (1500 = +50%). Always &gt; 0; decay approaches 1000 from this side without crossing.</summary>
        public int PriceMultiplierPermille { get; }
        /// <summary>When true, caravans with a covered origin region do not advance while the weather lasts.</summary>
        public bool CaravanBlocked { get; }
        /// <summary>Route progress multiplier while active, permille (500 = half travel progress). 1000 = no slowdown.</summary>
        public int RouteSlowPermille { get; }
        /// <summary>Days for the price shock to decay to exactly neutral after the weather clears (0 = instant).</summary>
        public int DecayDays { get; }

        public EmbargoRule(
            string ruleId, WeatherKind weather,
            IEnumerable<string>? affectedRegions,
            IEnumerable<string>? affectedCategories,
            IEnumerable<string>? affectedItemIds,
            bool affectsAllGoods,
            int priceMultiplierPermille,
            bool caravanBlocked,
            int routeSlowPermille,
            int decayDays)
        {
            RuleId = ruleId ?? string.Empty;
            Weather = weather;
            AffectedRegions = affectedRegions != null ? new List<string>(affectedRegions) : new List<string>();
            AffectedCategories = affectedCategories != null ? new List<string>(affectedCategories) : new List<string>();
            AffectedItemIds = affectedItemIds != null ? new List<string>(affectedItemIds) : new List<string>();
            AffectsAllGoods = affectsAllGoods;
            PriceMultiplierPermille = priceMultiplierPermille;
            CaravanBlocked = caravanBlocked;
            RouteSlowPermille = routeSlowPermille;
            DecayDays = decayDays;
        }
    }

    /// <summary>Load outcome: rules plus validation errors (domain result, no exceptions).</summary>
    public sealed class TradeEmbargoLoadResult
    {
        public List<EmbargoRule> Rules { get; } = new List<EmbargoRule>();
        public List<string> Errors { get; } = new List<string>();
        public bool HasErrors => Errors.Count > 0;
    }

    /// <summary>Immutable in-memory embargo rule registry.</summary>
    public sealed class TradeEmbargoCatalog
    {
        private readonly List<EmbargoRule> _rules = new List<EmbargoRule>();
        public IReadOnlyList<EmbargoRule> Rules => _rules;
        public int Count => _rules.Count;
        internal void Add(EmbargoRule rule) => _rules.Add(rule);
    }

    // ── Persisted runtime state (decay tracking only) ──────────────────────

    /// <summary>One in-flight embargo price shock: the captured peak, the remaining decay steps, and the current decayed multiplier.</summary>
    [Serializable]
    public sealed class EmbargoShockRuntime
    {
        public string ruleId = string.Empty;
        public int weatherKind = 0;
        public int peakPermille = 1000;
        public int currentPermille = 1000;
        public int remainingDecayDays = 0;
        /// <summary>Day the shock last advanced (active refresh or decay step) — guards against double decay on same-day re-notify.</summary>
        public int lastAdvancedDay = -1;
    }

    /// <summary>
    /// Versioned embargo runtime state. Persisted through the market economy
    /// save path; an older save without this state restores as neutral (no
    /// in-flight shocks — rules themselves are data, never saved).
    /// </summary>
    [Serializable]
    public sealed class TradeEmbargoState
    {
        public const int Version = 1;
        public int version = Version;
        public int lastNotifiedDay = -1;
        public List<EmbargoShockRuntime> shocks = new List<EmbargoShockRuntime>();
    }

    // ── System ─────────────────────────────────────────────────────────────

    /// <summary>
    /// Plan 14A — commodity embargo authority. Core owns the rules and the
    /// decay arithmetic; the host bridge feeds the current weather once per
    /// day and the market applies <see cref="GetCurrentPriceMultiplier"/> as
    /// ONE embargo factor inside the canonical price equation (regional
    /// baseline from 14B composes separately; the two never double-apply).
    /// Deterministic: no RNG anywhere; identical state + identical notify
    /// sequence produce identical multipliers.
    /// </summary>
    public class TradeEmbargoSystem
    {
        /// <summary>Combined embargo product clamp — mirrors the market shock-product band so no embargo stack escapes the bounded price path.</summary>
        public const float CombinedProductFloor = 0.4f;
        public const float CombinedProductCeiling = 2.5f;

        /// <summary>Load-time multiplier bound (permille). Data outside it is an integrity error, not a tuning hint.</summary>
        public const int MinMultiplierPermille = 500;
        public const int MaxMultiplierPermille = 5000;
        public const int MinRouteSlowPermille = 1;     // below this, author caravan_blocked instead
        public const int MaxRouteSlowPermille = 1000;  // slowdowns never speed travel up
        public const int MaxDecayDays = 30;

        private readonly Dictionary<string, EmbargoRule> _rules = new Dictionary<string, EmbargoRule>(StringComparer.Ordinal);
        private readonly TradeEmbargoState _state = new TradeEmbargoState();

        public TradeEmbargoState State => _state;
        public int RuleCount => _rules.Count;

        public TradeEmbargoSystem(TradeEmbargoCatalog? catalog = null)
        {
            if (catalog != null)
            {
                foreach (var rule in catalog.Rules)
                    RegisterRule(rule);
            }
        }

        // ── Rule registration ──────────────────────────────────────────

        /// <summary>
        /// Register one rule. Duplicate semantics are explicit: a rule whose
        /// id already exists, or that duplicates another rule's
        /// (weather, regions, targets) signature, is REJECTED (returns false)
        /// rather than silently combined.
        /// </summary>
        public bool RegisterRule(EmbargoRule rule)
        {
            if (rule == null || !IsRuleValid(rule, out _)) return false;
            if (_rules.ContainsKey(rule.RuleId)) return false;
            foreach (var existing in _rules.Values)
            {
                if (existing.Weather == rule.Weather
                    && SameRegions(existing.AffectedRegions, rule.AffectedRegions)
                    && SameTargets(existing, rule))
                    return false;
            }
            _rules.Add(rule.RuleId, rule);
            return true;
        }

        /// <summary>Rule lookup by id (null when absent).</summary>
        public EmbargoRule? FindRule(string ruleId) =>
            !string.IsNullOrEmpty(ruleId) && _rules.TryGetValue(ruleId, out var rule) ? rule : null;

        // ── Pure rule evaluation (current weather) ─────────────────────

        /// <summary>True when any rule matches the current weather.</summary>
        public bool IsEmbargoActive(WeatherKind current)
        {
            foreach (var rule in _rules.Values)
                if (rule.Weather == current) return true;
            return false;
        }

        /// <summary>Rules matching the current weather, ordinal by rule id (deterministic).</summary>
        public List<EmbargoRule> GetActiveRules(WeatherKind current)
        {
            var result = new List<EmbargoRule>();
            foreach (var rule in _rules.Values)
                if (rule.Weather == current) result.Add(rule);
            result.Sort((a, b) => string.CompareOrdinal(a.RuleId, b.RuleId));
            return result;
        }

        /// <summary>Distinct regions covered by rules matching the current weather ("*" stays "*"); ordinal order.</summary>
        public List<string> GetAffectedRegions(WeatherKind current)
        {
            var set = new HashSet<string>(StringComparer.Ordinal);
            foreach (var rule in GetActiveRules(current))
                foreach (var region in rule.AffectedRegions)
                    set.Add(region);
            var result = new List<string>(set);
            result.Sort(StringComparer.Ordinal);
            return result;
        }

        /// <summary>
        /// True when a rule matching the current weather blocks routes in the
        /// region. Null/empty/unknown regions never crash and never match
        /// ("*" covers only non-empty region names).
        /// </summary>
        public bool IsRouteBlocked(string? region, WeatherKind current)
        {
            if (string.IsNullOrEmpty(region)) return false;
            foreach (var rule in GetActiveRules(current))
            {
                if (!rule.CaravanBlocked || !RegionCovered(rule, region)) continue;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Route progress multiplier for the region under the current weather:
        /// 0 when blocked, else the most restrictive slow multiplier among
        /// matching rules (1.0 = neutral). Deterministic.
        /// </summary>
        public float GetRouteProgressMultiplier(string? region, WeatherKind current)
        {
            if (string.IsNullOrEmpty(region)) return 1f;
            float result = 1f;
            foreach (var rule in GetActiveRules(current))
            {
                if (!RegionCovered(rule, region)) continue;
                if (rule.CaravanBlocked) return 0f;
                float slow = rule.RouteSlowPermille / 1000f;
                if (slow < result) result = slow;
            }
            return result;
        }

        /// <summary>
        /// Pure active-weather price multiplier for one good in one region
        /// (permille): the PRODUCT of each matching rule's multiplier, each
        /// rule applied at most once, clamped to the market shock-product
        /// band. This is the rule-math view; the market path uses
        /// <see cref="GetCurrentPriceMultiplier"/> (decay-aware).
        /// </summary>
        public int GetWeatherPriceMultiplierPermille(string? region, WeatherKind current, string itemId, string itemCategory)
        {
            float product = 1f;
            foreach (var rule in GetActiveRules(current))
            {
                if (!RegionCovered(rule, region)) continue;
                if (!RuleTargets(rule, itemId, itemCategory)) continue;
                product *= rule.PriceMultiplierPermille / 1000f;
            }
            product = Math.Clamp(product, CombinedProductFloor, CombinedProductCeiling);
            return (int)Math.Round(product * 1000f, System.MidpointRounding.AwayFromZero);
        }

        // ── Decay-aware state path (market integration) ────────────────

        /// <summary>
        /// Advance the embargo state to the given day's weather. Idempotent
        /// per (day, rule): a rule active today refreshes its shock exactly
        /// once; a rule whose weather cleared decays at most one step per
        /// day, reaching exactly neutral (removed) at the end of its window.
        /// Deterministic; no RNG.
        /// </summary>
        public void NotifyWeather(int day, WeatherKind current)
        {
            _state.lastNotifiedDay = Math.Max(_state.lastNotifiedDay, day);

            // 1. Refresh shocks whose rule matches today's weather.
            var matchedRuleIds = new HashSet<string>(StringComparer.Ordinal);
            foreach (var rule in GetActiveRules(current))
            {
                matchedRuleIds.Add(rule.RuleId);
                var shock = FindOrCreateShock(rule.RuleId, rule.Weather);
                shock.peakPermille = ClampMultiplier(rule.PriceMultiplierPermille);
                shock.currentPermille = shock.peakPermille;
                shock.remainingDecayDays = Math.Clamp(rule.DecayDays, 0, MaxDecayDays);
                shock.lastAdvancedDay = day;
            }

            // 2. Decay every non-matching shock at most one step today.
            for (int i = _state.shocks.Count - 1; i >= 0; i--)
            {
                var shock = _state.shocks[i];
                if (shock == null || string.IsNullOrEmpty(shock.ruleId)) { _state.shocks.RemoveAt(i); continue; }
                if (matchedRuleIds.Contains(shock.ruleId)) continue;
                if (shock.lastAdvancedDay >= day) continue; // already advanced today

                shock.lastAdvancedDay = day;
                shock.remainingDecayDays = Math.Max(0, shock.remainingDecayDays - 1);
                if (shock.remainingDecayDays <= 0)
                {
                    shock.currentPermille = 1000; // decay lands exactly on neutral
                    _state.shocks.RemoveAt(i);
                    continue;
                }
                shock.currentPermille = InterpolateDecay(shock.peakPermille, shock.remainingDecayDays, DecayDaysFor(shock.ruleId));
            }
        }

        /// <summary>
        /// Current decay-aware embargo multiplier (permille) for one good in
        /// one region: the product of every in-flight shock whose rule covers
        /// the region and targets the good, clamped to the market
        /// shock-product band. 1000 = neutral.
        /// </summary>
        public int GetCurrentPriceMultiplierPermille(string? region, string itemId, string itemCategory)
        {
            float product = 1f;
            foreach (var shock in _state.shocks)
            {
                if (shock == null) continue;
                var rule = FindRule(shock.ruleId);
                if (rule == null) continue;
                if (!RegionCovered(rule, region)) continue;
                if (!RuleTargets(rule, itemId, itemCategory)) continue;
                product *= shock.currentPermille / 1000f;
            }
            product = Math.Clamp(product, CombinedProductFloor, CombinedProductCeiling);
            return (int)Math.Round(product * 1000f, System.MidpointRounding.AwayFromZero);
        }

        /// <summary>In-flight shocks (read-only view of the persisted state).</summary>
        public IReadOnlyList<EmbargoShockRuntime> ActiveShocks => _state.shocks;

        // ── Summary / validation ───────────────────────────────────────

        /// <summary>One-line structured summary of the embargo situation under the given weather (read-model seed for panels).</summary>
        public EmbargoSummary GetEmbargoSummary(WeatherKind current)
        {
            var summary = new EmbargoSummary { weatherKind = current.ToString() };
            var rules = GetActiveRules(current);
            summary.embargoActive = rules.Count > 0;
            summary.affectedRegions = GetAffectedRegions(current);
            var categories = new HashSet<string>(StringComparer.Ordinal);
            var items = new HashSet<string>(StringComparer.Ordinal);
            foreach (var rule in rules)
            {
                foreach (var c in rule.AffectedCategories) categories.Add(c);
                foreach (var it in rule.AffectedItemIds) items.Add(it);
                if (rule.AffectsAllGoods) summary.allGoodsAffected = true;
                if (rule.CaravanBlocked) summary.anyRouteBlocked = true;
            }
            summary.affectedCategories = categories.OrderBy(x => x, StringComparer.Ordinal).ToList();
            summary.affectedItemIds = items.OrderBy(x => x, StringComparer.Ordinal).ToList();
            summary.activeRuleCount = rules.Count;
            return summary;
        }

        /// <summary>
        /// Rule validity check. Errors name the rule, the field, and the
        /// violated expectation so the integrity gate can fail loudly.
        /// </summary>
        public static bool IsRuleValid(EmbargoRule? rule, out List<string> errors)
        {
            errors = new List<string>();
            if (rule == null) { errors.Add("rule is null"); return false; }
            if (string.IsNullOrWhiteSpace(rule.RuleId) || !IsSnakeCase(rule.RuleId))
                errors.Add($"'{rule.RuleId}': rule_id must be non-empty snake_case");
            if (rule.PriceMultiplierPermille < MinMultiplierPermille || rule.PriceMultiplierPermille > MaxMultiplierPermille)
                errors.Add($"'{rule.RuleId}': price_multiplier_permille {rule.PriceMultiplierPermille} outside [{MinMultiplierPermille},{MaxMultiplierPermille}]");
            if (rule.RouteSlowPermille < MinRouteSlowPermille || rule.RouteSlowPermille > MaxRouteSlowPermille)
                errors.Add($"'{rule.RuleId}': route_slow_permille {rule.RouteSlowPermille} outside [{MinRouteSlowPermille},{MaxRouteSlowPermille}]");
            if (rule.CaravanBlocked && rule.RouteSlowPermille < 1000)
                errors.Add($"'{rule.RuleId}': a blocked route cannot also carry a slowdown (choose one mechanism)");
            if (rule.DecayDays < 0 || rule.DecayDays > MaxDecayDays)
                errors.Add($"'{rule.RuleId}': decay_days {rule.DecayDays} outside [0,{MaxDecayDays}]");
            if (rule.AffectedRegions.Count == 0)
                errors.Add($"'{rule.RuleId}': affected_regions must not be empty (use [\"*\"] for all regions)");
            foreach (var region in rule.AffectedRegions)
            {
                if (region == "*") continue;
                if (!RegionalSupplyRouter.IsAcceptedSupplyTag(region))
                    errors.Add($"'{rule.RuleId}': affected region '{region}' is not in the accepted supply-tag vocabulary");
            }
            foreach (var category in rule.AffectedCategories)
                if (!GoodCategories.IsKnown(category))
                    errors.Add($"'{rule.RuleId}': affected category '{category}' is not a known goods category");
            foreach (var item in rule.AffectedItemIds)
                if (string.IsNullOrWhiteSpace(item))
                    errors.Add($"'{rule.RuleId}': affected_item_ids must not contain empty entries");
            if (!rule.AffectsAllGoods && rule.AffectedCategories.Count == 0 && rule.AffectedItemIds.Count == 0)
                errors.Add($"'{rule.RuleId}': rule must target categories, item ids, or affects_all_goods");
            if (rule.AffectsAllGoods && (rule.AffectedCategories.Count > 0 || rule.AffectedItemIds.Count > 0))
                errors.Add($"'{rule.RuleId}': a rule with affects_all_goods must not also name explicit targets");
            if (!rule.AffectsAllGoods && rule.AffectedCategories.Count > 0 && rule.AffectedItemIds.Count > 0)
                errors.Add($"'{rule.RuleId}': rule must not mix affected_categories and affected_item_ids (one target kind per rule)");
            return errors.Count == 0;
        }

        // ── Save / Load ────────────────────────────────────────────────

        public TradeEmbargoState CaptureState()
        {
            var copy = new TradeEmbargoState
            {
                version = TradeEmbargoState.Version,
                lastNotifiedDay = Math.Max(-1, _state.lastNotifiedDay)
            };
            var shocks = new List<EmbargoShockRuntime>(_state.shocks);
            shocks.Sort((a, b) => string.CompareOrdinal(a?.ruleId, b?.ruleId));
            var seen = new HashSet<string>(StringComparer.Ordinal);
            foreach (var s in shocks)
            {
                if (s == null || string.IsNullOrEmpty(s.ruleId) || !seen.Add(s.ruleId)) continue;
                copy.shocks.Add(new EmbargoShockRuntime
                {
                    ruleId = s.ruleId,
                    weatherKind = s.weatherKind,
                    peakPermille = ClampMultiplier(s.peakPermille),
                    currentPermille = ClampMultiplier(s.currentPermille),
                    remainingDecayDays = Math.Clamp(s.remainingDecayDays, 0, MaxDecayDays),
                    lastAdvancedDay = Math.Max(-1, s.lastAdvancedDay)
                });
            }
            return copy;
        }

        /// <summary>
        /// Restore runtime decay state. Newer versions throw (a save from the
        /// future must not silently corrupt); older/missing state restores as
        /// neutral. Rules themselves are never restored from saves — they are
        /// data.
        /// </summary>
        public void RestoreState(TradeEmbargoState? saved)
        {
            if (saved == null) return;
            if (saved.version > TradeEmbargoState.Version)
                throw new InvalidOperationException(
                    $"trade embargo save version {saved.version} is newer than supported ({TradeEmbargoState.Version})");
            _state.lastNotifiedDay = Math.Max(-1, saved.lastNotifiedDay);
            _state.shocks.Clear();
            if (saved.shocks == null) return;
            var seen = new HashSet<string>(StringComparer.Ordinal);
            foreach (var s in saved.shocks)
            {
                if (s == null || string.IsNullOrEmpty(s.ruleId) || !seen.Add(s.ruleId)) continue;
                _state.shocks.Add(new EmbargoShockRuntime
                {
                    ruleId = s.ruleId,
                    weatherKind = s.weatherKind,
                    peakPermille = ClampMultiplier(s.peakPermille),
                    currentPermille = ClampMultiplier(s.currentPermille),
                    remainingDecayDays = Math.Clamp(s.remainingDecayDays, 0, MaxDecayDays),
                    lastAdvancedDay = Math.Max(-1, s.lastAdvancedDay)
                });
            }
        }

        // ── Internals ──────────────────────────────────────────────────

        private EmbargoShockRuntime FindOrCreateShock(string ruleId, WeatherKind weather)
        {
            foreach (var shock in _state.shocks)
                if (shock != null && shock.ruleId == ruleId) return shock;
            var created = new EmbargoShockRuntime { ruleId = ruleId, weatherKind = (int)weather };
            _state.shocks.Add(created);
            return created;
        }

        private int DecayDaysFor(string ruleId)
        {
            var rule = FindRule(ruleId);
            return rule != null ? Math.Clamp(rule.DecayDays, 0, MaxDecayDays) : 1;
        }

        /// <summary>Linear decay from peak toward exactly 1000 across the remaining window; never crosses neutral.</summary>
        private static int InterpolateDecay(int peakPermille, int remainingDecayDays, int decayDays)
        {
            if (decayDays <= 0 || remainingDecayDays <= 0) return 1000;
            int span = peakPermille - 1000;
            if (span == 0) return 1000;
            double fraction = (double)remainingDecayDays / decayDays;
            int offset = (int)Math.Round(span * fraction, System.MidpointRounding.AwayFromZero);
            int result = 1000 + offset;
            // Never cross neutral from the shock side.
            if (span > 0) return Math.Clamp(result, 1000, peakPermille);
            return Math.Clamp(result, peakPermille, 1000);
        }

        private static int ClampMultiplier(int permille) =>
            Math.Clamp(permille, 1, MaxMultiplierPermille < 1 ? 1 : MaxMultiplierPermille);

        private static bool RegionCovered(EmbargoRule rule, string? region)
        {
            if (rule == null || string.IsNullOrEmpty(region)) return false;
            foreach (var r in rule.AffectedRegions)
            {
                if (r == "*")
                {
                    // "*" covers every KNOWN region; an unknown region name is
                    // not a caravan origin and must not match a wildcard rule.
                    if (RegionalSupplyRouter.IsAcceptedSupplyTag(region)) return true;
                    continue;
                }
                if (string.Equals(r, region, StringComparison.Ordinal)) return true;
            }
            return false;
        }

        private static bool RuleTargets(EmbargoRule rule, string itemId, string itemCategory)
        {
            if (rule.AffectsAllGoods) return true;
            foreach (var id in rule.AffectedItemIds)
                if (string.Equals(id, itemId, StringComparison.Ordinal)) return true;
            foreach (var category in rule.AffectedCategories)
                if (string.Equals(category, itemCategory, StringComparison.Ordinal)) return true;
            return false;
        }

        private static bool SameRegions(IReadOnlyList<string> a, IReadOnlyList<string> b)
        {
            if (a.Count != b.Count) return false;
            var set = new HashSet<string>(a, StringComparer.Ordinal);
            foreach (var region in b)
                if (!set.Contains(region)) return false;
            return true;
        }

        private static bool SameTargets(EmbargoRule a, EmbargoRule b)
        {
            if (a.AffectsAllGoods != b.AffectsAllGoods) return false;
            if (!SameRegions(a.AffectedCategories, b.AffectedCategories)) return false;
            return SameRegions(a.AffectedItemIds, b.AffectedItemIds);
        }

        private static bool IsSnakeCase(string id)
        {
            if (string.IsNullOrEmpty(id)) return false;
            for (int i = 0; i < id.Length; i++)
            {
                char c = id[i];
                bool ok = (c >= 'a' && c <= 'z') || (c >= '0' && c <= '9') || c == '_';
                if (!ok) return false;
            }
            return id[0] != '_' && id[id.Length - 1] != '_';
        }
    }

    /// <summary>Structured embargo snapshot for read models (no business logic).</summary>
    [Serializable]
    public sealed class EmbargoSummary
    {
        public string weatherKind = string.Empty;
        public bool embargoActive;
        public bool allGoodsAffected;
        public bool anyRouteBlocked;
        public int activeRuleCount;
        public List<string> affectedRegions = new List<string>();
        public List<string> affectedCategories = new List<string>();
        public List<string> affectedItemIds = new List<string>();
    }

    // ── Loader (strict, GoodsCatalogLoader-pattern) ────────────────────────

    /// <summary>
    /// Engine-agnostic loader for trade_embargoes.json with load-time
    /// validation: schema envelope, snake_case rule ids, real WeatherKind
    /// values, accepted region vocabulary, known goods categories, multiplier
    /// and slowdown bounds, duplicate rules. Errors are collected, never
    /// thrown.
    /// </summary>
    public static class TradeEmbargoCatalogLoader
    {
        public const string FileName = "trade_embargoes.json";
        public const int CurrentSchemaVersion = 1;

        public static TradeEmbargoLoadResult Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            var result = new TradeEmbargoLoadResult();
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
            {
                result.Errors.Add("loader requires dataDir, IFileIO and IJsonSerializer");
                return result;
            }

            string path = fileIO.Combine(dataDir, FileName);
            if (!fileIO.FileExists(path))
            {
                result.Errors.Add("catalog file missing: " + FileName);
                return result;
            }

            string raw = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw))
            {
                result.Errors.Add("catalog file empty: " + FileName);
                return result;
            }

            TradeEmbargoRoot root;
            try
            {
                root = json.Deserialize<TradeEmbargoRoot>(raw);
            }
            catch (Exception e)
            {
                result.Errors.Add("catalog malformed JSON: " + e.Message);
                return result;
            }
            if (root == null)
            {
                result.Errors.Add("catalog parsed to null");
                return result;
            }
            if (root.schema_version > CurrentSchemaVersion)
            {
                result.Errors.Add($"catalog schema {root.schema_version} is newer than supported {CurrentSchemaVersion}");
                return result;
            }

            var rawRules = root.rules;
            if (rawRules == null)
            {
                result.Errors.Add("catalog rules array is null");
                return result;
            }

            var seen = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < rawRules.Count; i++)
            {
                var rawRule = rawRules[i];
                if (rawRule == null) { result.Errors.Add($"entry [{i}] is null"); continue; }

                string id = rawRule.rule_id ?? string.Empty;
                if (string.IsNullOrWhiteSpace(id))
                {
                    result.Errors.Add($"entry [{i}] missing rule_id");
                    continue;
                }
                if (!seen.Add(id))
                {
                    result.Errors.Add($"duplicate rule_id '{id}' at entry [{i}]");
                    continue;
                }

                if (string.IsNullOrWhiteSpace(rawRule.weather_kind)
                    || !Enum.TryParse(rawRule.weather_kind.Trim(), out WeatherKind weather))
                {
                    result.Errors.Add($"'{id}' weather_kind '{rawRule.weather_kind}' does not resolve to a WeatherKind value");
                    continue;
                }

                var rule = new EmbargoRule(
                    id,
                    weather,
                    rawRule.affected_regions ?? new List<string>(),
                    rawRule.affected_categories ?? new List<string>(),
                    rawRule.affected_item_ids ?? new List<string>(),
                    rawRule.affects_all_goods,
                    rawRule.price_multiplier_permille ?? 1000,
                    rawRule.caravan_blocked,
                    rawRule.route_slow_permille ?? 1000,
                    rawRule.decay_days ?? 2);

                if (!TradeEmbargoSystem.IsRuleValid(rule, out var ruleErrors))
                {
                    foreach (var err in ruleErrors)
                        result.Errors.Add(err);
                    continue;
                }

                result.Rules.Add(rule);
            }
            return result;
        }

        public static TradeEmbargoCatalog ToCatalog(TradeEmbargoLoadResult load)
        {
            var catalog = new TradeEmbargoCatalog();
            if (load != null)
            {
                foreach (var rule in load.Rules)
                    catalog.Add(rule);
            }
            return catalog;
        }

        private class TradeEmbargoRoot
        {
            public int schema_version = 1;
            public string collection_id = string.Empty;
            public string description = string.Empty;
            public List<RawEmbargoRule> rules = new List<RawEmbargoRule>();
        }

        /// <summary>Strict DTO: null fields mean ABSENT, not defaulted.</summary>
        private class RawEmbargoRule
        {
            public string rule_id;
            public string weather_kind;
            public List<string> affected_regions;
            public List<string> affected_categories;
            public List<string> affected_item_ids;
            public bool affects_all_goods;
            public int? price_multiplier_permille;
            public bool caravan_blocked;
            public int? route_slow_permille;
            public int? decay_days;
        }
    }
}
