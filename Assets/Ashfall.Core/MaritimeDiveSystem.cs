using System;
using System.Collections.Generic;

namespace Ashfall.Core
{
    [Serializable]
    public sealed class MaritimeDiveState
    {
        public string systemId = MaritimeDiveSystem.SystemId;
        public List<DiveSite> sites = new List<DiveSite>();
        public List<DiveOutcome> outcomes = new List<DiveOutcome>();
    }

    [Serializable]
    public sealed class DiveSite
    {
        public string siteId = string.Empty;
        public string displayName = string.Empty;
        public float depthMeters;
        public float hazardLevel;     // 0-1
        public bool isExplored;
        public bool isHazardous;
        public float radiationLevel;
    }

    [Serializable]
    public sealed class DiveOutcome
    {
        public string siteId = string.Empty;
        public int day;
        public DiveResult result;
        public string recoveredItemId = string.Empty;
        public float radiationDose;
        public string notes = string.Empty;
    }

    public enum DiveResult { Success, Partial, Contaminated, Failed, CrewLost }

    public sealed class MaritimeDiveSystem
    {
        public const string SystemId = "maritime_dive";
        private MaritimeDiveState _state = new MaritimeDiveState();
        private readonly ISeededRng _rng;
        private readonly ILog _log;
        private int _currentDay;

        public MaritimeDiveState State => _state;
        public event Action<DiveOutcome> OnDiveCompleted;
        public event Action OnSitesChanged;

        public MaritimeDiveSystem(ISeededRng rng, ILog log = null)
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _log = log ?? NullLog.Instance;
        }

        public ActionResult RegisterSite(string siteId, string displayName, float depthMeters, float hazardLevel)
        {
            if (_state.sites.Exists(s => s.siteId == siteId))
                return ActionResult.Blocked("site_exists", "dive.site_exists");
            _state.sites.Add(new DiveSite
            {
                siteId = siteId, displayName = displayName,
                depthMeters = depthMeters,
                hazardLevel = Math.Clamp(hazardLevel, 0f, 1f),
                radiationLevel = Math.Clamp(hazardLevel, 0f, 1f) * 100f
            });
            OnSitesChanged?.Invoke();
            return ActionResult.Success("dive.site_registered");
        }

        public ActionResult ConductDive(string siteId, string diverId, float equipmentQuality)
        {
            var site = _state.sites.Find(s => s.siteId == siteId);
            if (site == null) return ActionResult.Failed("unknown_site", "dive.unknown_site");

            float successChance = (1f - site.hazardLevel) * equipmentQuality;
            var roll = (float)_rng.NextDouble();
            DiveResult result;
            string recoveredItem = string.Empty;
            float dose = 0;

            if (roll < successChance * 0.7f)
            {
                result = DiveResult.Success;
                recoveredItem = RollRecovery(site);
                dose = site.radiationLevel * 0.1f;
            }
            else if (roll < successChance)
            {
                result = DiveResult.Partial;
                recoveredItem = RollRecovery(site);
                dose = site.radiationLevel * 0.3f;
            }
            else if (roll < successChance + 0.15f)
            {
                result = DiveResult.Contaminated;
                recoveredItem = string.Empty;
                dose = site.radiationLevel * 0.8f;
            }
            else if (roll < successChance + 0.1f)
            {
                result = DiveResult.CrewLost;
                dose = site.radiationLevel * 2f;
            }
            else
            {
                result = DiveResult.Failed;
                dose = site.radiationLevel * 0.2f;
            }

            site.isExplored = result != DiveResult.Failed;
            site.isHazardous = result == DiveResult.Contaminated || result == DiveResult.CrewLost;

            var outcome = new DiveOutcome
            {
                siteId = siteId, day = _currentDay, result = result,
                recoveredItemId = recoveredItem, radiationDose = dose,
                notes = $"diver={diverId}, quality={equipmentQuality:F1}, roll={roll:F2}"
            };
            _state.outcomes.Add(outcome);
            _log.Info($"[Dive] {site.displayName}: {result} (dose={dose:F1} mSv)");
            OnDiveCompleted?.Invoke(outcome);
            return ActionResult.Success($"dive.{result.ToString().ToLowerInvariant()}",
                new Dictionary<string, double>
                {
                    { "dose", dose },
                    { "result", (int)result }
                });
        }

        private string RollRecovery(DiveSite site)
        {
            var items = new[] { "salvage_metal", "salvage_electronics", "salvage_fuel", "artifact_fragments" };
            return items[_rng.Next(0, items.Length)];
        }

        public void TickDay(int day)
        {
            _currentDay = day;
        }

        public MaritimeDiveState CaptureState() => _state;
        public void RestoreState(MaritimeDiveState saved)
        {
            if (saved == null) return;
            _state = saved;
            OnSitesChanged?.Invoke();
        }
    }
}
