using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core
{
    [Serializable]
    public sealed class RegionalTreatyState
    {
        public string systemId = RegionalTreatySystem.SystemId;
        public List<TreatyInstance> treaties = new List<TreatyInstance>();
    }

    [Serializable]
    public sealed class TreatyDefinition
    {
        public string treaty_id = string.Empty;
        public string display_name = string.Empty;
        public string faction_id = string.Empty;
        public string description = string.Empty;
        public float ratification_cost_scrap;
        public int ratification_cost_day;
        public List<string> prerequisites = new List<string>();
        public List<TreatyEffect> effects = new List<TreatyEffect>();
        /// <summary>Task 21 — all signatory factions; effect consumers match against
        /// the full list (first entry is kept in faction_id for compatibility).</summary>
        public List<string> signatory_factions = new List<string>();
        public float compliance_check_interval_days = 30f;
        public float violation_penalty_affinity = -20f;
        /// <summary>Task 21.9 — treaty term length in days; 0 = indefinite.
        /// Expiry uses the same effect-removal path as breach (no orphan modifiers).</summary>
        public float term_days;
    }

    [Serializable]
    public sealed class TreatyEffect
    {
        public string effect_type = string.Empty; // "economy_discount", "raid_pressure_relief", "water_quota", "power"
        public string target_id = string.Empty;
        public float value;
    }

    [Serializable]
    public sealed class TreatyInstance
    {
        public string treatyId = string.Empty;
        public TreatyStatus status;
        public int proposedDay = -1;
        public int ratifiedDay = -1;
        public int violatedDay = -1;
        public float complianceScore = 1f;
        public int lastComplianceCheckDay = -1;
    }

    public enum TreatyStatus { Proposed, Ratified, Active, Violated, Suspended, Expired }

    public sealed class RegionalTreatySystem
    {
        public const string SystemId = "regional_treaty";
        private RegionalTreatyState _state = new RegionalTreatyState();
        private readonly Dictionary<string, TreatyDefinition> _catalog = new Dictionary<string, TreatyDefinition>(StringComparer.Ordinal);
        private readonly ILog _log;
        private int _currentDay;

        public RegionalTreatyState State => _state;
        public event Action<TreatyInstance> OnTreatyStatusChanged;

        /// <summary>Task 21.2 — typed transition emitted AFTER the state mutation,
        /// for every status change except the initial Propose (which starts no
        /// world effects). Restoring a save never emits this — consumers may
        /// treat transitions as exactly-once per lifecycle change.</summary>
        public event Action<TreatyTransition>? OnTreatyTransition;

        public RegionalTreatySystem(ILog? log = null)
        {
            _log = log ?? NullLog.Instance;
        }

        public void LoadCatalog(List<TreatyDefinition> treaties)
        {
            if (treaties == null) return;
            _catalog.Clear();
            foreach (var t in treaties)
                if (!string.IsNullOrEmpty(t.treaty_id))
                    _catalog[t.treaty_id] = t;
        }

        public TreatyDefinition? GetDefinition(string id)
        {
            _catalog.TryGetValue(id, out var def);
            return def;
        }

        public ActionResult Propose(string treatyId)
        {
            if (_state.treaties.Exists(t => t.treatyId == treatyId))
                return ActionResult.Blocked("already_proposed", "treaty.already_proposed");
            if (!_catalog.TryGetValue(treatyId, out var def))
                return ActionResult.Failed("unknown_treaty", "treaty.unknown");

            _state.treaties.Add(new TreatyInstance
            {
                treatyId = treatyId, status = TreatyStatus.Proposed, proposedDay = _currentDay
            });
            _log.Info($"[Treaty] proposed {treatyId}");
            OnTreatyStatusChanged?.Invoke(_state.treaties[_state.treaties.Count - 1]);
            return ActionResult.Success("treaty.proposed");
        }

        public ActionResult Ratify(string treatyId, int scrapCost)
        {
            var treaty = _state.treaties.Find(t => t.treatyId == treatyId);
            if (treaty == null) return ActionResult.Failed("unknown_treaty", "treaty.unknown");
            if (treaty.status != TreatyStatus.Proposed)
                return ActionResult.Blocked("not_proposed", "treaty.not_proposed");

            if (!_catalog.TryGetValue(treatyId, out var def))
                return ActionResult.Failed("missing_def", "treaty.missing_def");

            if (scrapCost < def.ratification_cost_scrap)
                return ActionResult.Blocked("insufficient_scrap", "treaty.insufficient_scrap");

            treaty.status = TreatyStatus.Ratified;
            treaty.ratifiedDay = _currentDay;
            _log.Info($"[Treaty] ratified {treatyId}");
            OnTreatyStatusChanged?.Invoke(treaty);
            OnTreatyTransition?.Invoke(BuildTransition(treaty, TreatyStatus.Proposed,
                TreatyViolationCause.None, endedEffects: null, startedEffects: ActiveDescriptorsFor(treaty)));
            return ActionResult.Success("treaty.ratified",
                new Dictionary<string, double> { { "scrap_cost", def.ratification_cost_scrap } });
        }

        /// <summary>Task 21.5/21.7 — player-initiated breach. Removes all ratified
        /// benefits and flags the treaty Violated with cause Betrayal; consumers
        /// apply breach consequences (standing penalty, raid pressure) from the
        /// typed transition or derived state — never by poking their own stats.</summary>
        public ActionResult BreakTreaty(string treatyId)
        {
            var treaty = _state.treaties.Find(t => t.treatyId == treatyId);
            if (treaty == null) return ActionResult.Failed("unknown_treaty", "treaty.unknown");
            if (treaty.status != TreatyStatus.Ratified && treaty.status != TreatyStatus.Active)
                return ActionResult.Blocked("not_active", "treaty.not_active");

            var ended = ActiveDescriptorsFor(treaty);
            var from = treaty.status;
            treaty.status = TreatyStatus.Violated;
            treaty.violatedDay = _currentDay;
            _log.Warn($"[Treaty] {treatyId} BROKEN by signatory breach");
            OnTreatyStatusChanged?.Invoke(treaty);
            OnTreatyTransition?.Invoke(BuildTransition(treaty, from, TreatyViolationCause.Betrayal, ended, null));
            return ActionResult.Success("treaty.broken");
        }

        public bool IsActive(string treatyId)
        {
            var t = _state.treaties.Find(x => x.treatyId == treatyId);
            return t != null && (t.status == TreatyStatus.Ratified || t.status == TreatyStatus.Active);
        }

        public List<TreatyEffect> GetActiveEffects()
        {
            var effects = new List<TreatyEffect>();
            foreach (var t in _state.treaties)
            {
                if (t.status != TreatyStatus.Ratified && t.status != TreatyStatus.Active) continue;
                if (_catalog.TryGetValue(t.treatyId, out var def))
                    effects.AddRange(def.effects);
            }
            return effects;
        }

        /// <summary>Task 21.2/21.11 — typed active-effect descriptors. Deterministic
        /// order: ordinal by treaty id, then effect kind, then target id. Unknown
        /// data effect_type strings are skipped rather than guessed at.</summary>
        public List<TreatyActiveEffect> GetActiveEffectDescriptors()
        {
            var result = new List<TreatyActiveEffect>();
            foreach (var t in _state.treaties)
            {
                if (t.status != TreatyStatus.Ratified && t.status != TreatyStatus.Active) continue;
                result.AddRange(ActiveDescriptorsFor(t));
            }
            result.Sort(CompareDescriptors);
            return result;
        }

        /// <summary>Task 21.5 — aggregate raid-pressure modifier for the canonical
        /// authority (Muster.IronRaidersSystem.EvaluateRaidChance):
        /// +TreatyEffectTable.BreachRaidPressure per Violated treaty,
        /// −relief per active RaidPressureRelief descriptor, symmetrically clamped.
        /// Derived from state on every read — nothing to persist, nothing to double-apply.</summary>
        public float GetRaidPressureModifier()
        {
            float total = 0f;
            foreach (var t in _state.treaties)
            {
                if (t.status == TreatyStatus.Violated)
                {
                    total += TreatyEffectTable.BreachRaidPressure;
                    continue;
                }
                if (t.status != TreatyStatus.Ratified && t.status != TreatyStatus.Active) continue;
                if (_catalog.TryGetValue(t.treatyId, out var def))
                {
                    foreach (var d in BuildDescriptors(t, def))
                        if (d.Kind == TreatyEffectKind.RaidPressureRelief)
                            total -= Math.Max(0f, d.Value);
                }
            }
            return Math.Max(-TreatyEffectTable.RaidPressureModifierClamp,
                   Math.Min(TreatyEffectTable.RaidPressureModifierClamp, total));
        }

        /// <summary>Task 21.4 — best active TradeDiscount available to a faction
        /// (any signatory of the pact; empty signatory list = applies to all partners).
        /// Fraction, e.g. 0.10 = −10%.</summary>
        public float GetTradeDiscount(string factionId)
        {
            float best = 0f;
            foreach (var t in _state.treaties)
            {
                if (t.status != TreatyStatus.Ratified && t.status != TreatyStatus.Active) continue;
                if (!_catalog.TryGetValue(t.treatyId, out var def)) continue;
                if (!DefIncludesFaction(def, factionId)) continue;
                foreach (var d in BuildDescriptors(t, def))
                    if (d.Kind == TreatyEffectKind.TradeDiscount)
                        best = Math.Max(best, Math.Max(0f, d.Value));
            }
            return best;
        }

        /// <summary>Best active SupplyPriceRelief for a faction, same matching rule.</summary>
        public float GetSupplyPriceRelief(string factionId)
        {
            float best = 0f;
            foreach (var t in _state.treaties)
            {
                if (t.status != TreatyStatus.Ratified && t.status != TreatyStatus.Active) continue;
                if (!_catalog.TryGetValue(t.treatyId, out var def)) continue;
                if (!DefIncludesFaction(def, factionId)) continue;
                foreach (var d in BuildDescriptors(t, def))
                    if (d.Kind == TreatyEffectKind.SupplyPriceRelief)
                        best = Math.Max(best, Math.Max(0f, d.Value));
            }
            return best;
        }

        /// <summary>Count of treaties in a given status — feeds the Plan 25C
        /// escalation spine's MusterPathInput.ActiveTreatyCount/ViolatedTreatyCount.</summary>
        public int CountByStatus(TreatyStatus status)
        {
            int n = 0;
            for (int i = 0; i < _state.treaties.Count; i++)
                if (_state.treaties[i].status == status) n++;
            return n;
        }

        private bool DefIncludesFaction(TreatyDefinition def, string factionId)
        {
            if (string.IsNullOrEmpty(factionId)) return true;
            if (def.signatory_factions != null && def.signatory_factions.Count > 0)
            {
                for (int i = 0; i < def.signatory_factions.Count; i++)
                    if (string.Equals(def.signatory_factions[i], factionId, StringComparison.Ordinal))
                        return true;
                return false;
            }
            return string.IsNullOrEmpty(def.faction_id) ||
                   string.Equals(def.faction_id, factionId, StringComparison.Ordinal);
        }

        private List<TreatyActiveEffect> ActiveDescriptorsFor(TreatyInstance t)
        {
            return _catalog.TryGetValue(t.treatyId, out var def)
                ? BuildDescriptors(t, def)
                : new List<TreatyActiveEffect>();
        }

        private List<TreatyActiveEffect> BuildDescriptors(TreatyInstance t, TreatyDefinition def)
        {
            var list = new List<TreatyActiveEffect>();
            if (def.effects == null) return list;
            foreach (var e in def.effects)
            {
                if (e == null) continue;
                if (!TreatyEffectTable.TryMapKind(e.effect_type, out var kind, out var fallback))
                    continue;
                float value = e.value != 0f ? e.value : fallback;
                list.Add(new TreatyActiveEffect
                {
                    TreatyId = t.treatyId,
                    FactionId = def.faction_id,
                    Kind = kind,
                    TargetId = e.target_id,
                    Value = value,
                    SourceId = TreatyActiveEffect.MakeSourceId(t.treatyId, kind)
                });
            }
            list.Sort(CompareDescriptors);
            return list;
        }

        private static int CompareDescriptors(TreatyActiveEffect a, TreatyActiveEffect b)
        {
            int c = string.CompareOrdinal(a.TreatyId, b.TreatyId);
            if (c != 0) return c;
            c = ((int)a.Kind).CompareTo((int)b.Kind);
            if (c != 0) return c;
            return string.CompareOrdinal(a.TargetId, b.TargetId);
        }

        private TreatyTransition BuildTransition(
            TreatyInstance t, TreatyStatus from, TreatyViolationCause cause,
            List<TreatyActiveEffect>? endedEffects, List<TreatyActiveEffect>? startedEffects)
        {
            var transition = new TreatyTransition
            {
                TreatyId = t.treatyId,
                FactionId = _catalog.TryGetValue(t.treatyId, out var def) ? def.faction_id : string.Empty,
                From = from,
                To = t.status,
                Day = _currentDay,
                Cause = cause,
                EndedEffects = endedEffects ?? new List<TreatyActiveEffect>(),
                StartedEffects = startedEffects ?? new List<TreatyActiveEffect>()
            };
            transition.EndedEffects.Sort(CompareDescriptors);
            transition.StartedEffects.Sort(CompareDescriptors);
            return transition;
        }

        public void TickDay(int day)
        {
            _currentDay = day;
            foreach (var t in _state.treaties)
            {
                if (t.status != TreatyStatus.Ratified && t.status != TreatyStatus.Active) continue;
                if (!_catalog.TryGetValue(t.treatyId, out var def)) continue;

                // Task 21.9 — term expiry. Same removal path as breach: the active
                // effects end (EndedEffects on the transition), nothing is orphaned.
                if (def.term_days > 0 && day - t.ratifiedDay >= def.term_days)
                {
                    var expiredEffects = ActiveDescriptorsFor(t);
                    var fromStatus = t.status;
                    t.status = TreatyStatus.Expired;
                    _log.Info($"[Treaty] {t.treatyId} EXPIRED after {def.term_days:0} day term");
                    OnTreatyStatusChanged?.Invoke(t);
                    OnTreatyTransition?.Invoke(BuildTransition(t, fromStatus, TreatyViolationCause.None, expiredEffects, null));
                    continue;
                }

                if (day - t.lastComplianceCheckDay >= def.compliance_check_interval_days)
                {
                    t.lastComplianceCheckDay = day;
                    t.complianceScore = Math.Max(0, t.complianceScore - 0.1f);
                    if (t.complianceScore <= 0)
                    {
                        var endedEffects = ActiveDescriptorsFor(t);
                        var fromStatus = t.status;
                        t.status = TreatyStatus.Violated;
                        t.violatedDay = day;
                        _log.Warn($"[Treaty] {t.treatyId} VIOLATED");
                        OnTreatyStatusChanged?.Invoke(t);
                        OnTreatyTransition?.Invoke(BuildTransition(t, fromStatus, TreatyViolationCause.ComplianceFailure, endedEffects, null));
                    }
                }
            }
        }

        public RegionalTreatyState CaptureState() => CloneState(_state);

        public void RestoreState(RegionalTreatyState saved)
        {
            if (saved == null) return;
            _state = CloneState(saved);
        }

        private static RegionalTreatyState CloneState(RegionalTreatyState src)
        {
            if (src == null) return new RegionalTreatyState();
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(src);
            return s.Deserialize<RegionalTreatyState>(json) ?? new RegionalTreatyState();
        }
    }
}
