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
        public float compliance_check_interval_days = 30f;
        public float violation_penalty_affinity = -20f;
    }

    [Serializable]
    public sealed class TreatyEffect
    {
        public string effect_type = string.Empty; // "economy_discount", "route_access", "water_quota", "labor", "power"
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

        public RegionalTreatySystem(ILog log = null!)
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
            return ActionResult.Success("treaty.ratified",
                new Dictionary<string, double> { { "scrap_cost", def.ratification_cost_scrap } });
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

        public void TickDay(int day)
        {
            _currentDay = day;
            foreach (var t in _state.treaties)
            {
                if (t.status != TreatyStatus.Ratified && t.status != TreatyStatus.Active) continue;
                if (!_catalog.TryGetValue(t.treatyId, out var def)) continue;

                if (day - t.lastComplianceCheckDay >= def.compliance_check_interval_days)
                {
                    t.lastComplianceCheckDay = day;
                    t.complianceScore = Math.Max(0, t.complianceScore - 0.1f);
                    if (t.complianceScore <= 0)
                    {
                        t.status = TreatyStatus.Violated;
                        t.violatedDay = day;
                        _log.Warn($"[Treaty] {t.treatyId} VIOLATED");
                        OnTreatyStatusChanged?.Invoke(t);
                    }
                }
            }
        }

        public RegionalTreatyState CaptureState() => _state;
        public void RestoreState(RegionalTreatyState saved)
        {
            if (saved == null) return;
            _state = saved;
        }
    }
}
