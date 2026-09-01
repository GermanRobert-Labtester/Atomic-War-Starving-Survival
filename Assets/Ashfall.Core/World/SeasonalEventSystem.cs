using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.World
{
    [Serializable]
    public sealed class ActiveSeasonalEvent
    {
        public string eventId = string.Empty;
        public string name = string.Empty;
        public string seasonId = string.Empty;
        public string category = "Shelter";
        public string severity = "Medium";
        public int triggeredDay;
        public int expiresDay;
        public bool isMitigated;
        public string impactSummary = string.Empty;
        public string mitigationItemId = string.Empty;
        public int mitigationCost;
    }

    [Serializable]
    public sealed class SeasonalEventSaveState
    {
        public string systemId = SeasonalEventSystem.SystemId;
        public List<ActiveSeasonalEvent> activeEvents = new List<ActiveSeasonalEvent>();
        public List<string> cooldownKeys = new List<string>();
        public List<int> cooldownDays = new List<int>();
        public List<string> resolvedEvents = new List<string>();
    }

    public sealed class SeasonalEventSystem
    {
        public const string SystemId = "seasonal_event_system";

        private readonly List<SeasonalEventDef> _definitions = new List<SeasonalEventDef>();
        private readonly List<ActiveSeasonalEvent> _activeEvents = new List<ActiveSeasonalEvent>();
        private readonly Dictionary<string, int> _cooldowns = new Dictionary<string, int>();
        private readonly List<string> _resolvedEvents = new List<string>();
        private readonly ILog _log;

        public IReadOnlyList<ActiveSeasonalEvent> ActiveEvents => _activeEvents.AsReadOnly();
        public IReadOnlyList<SeasonalEventDef> Definitions => _definitions.AsReadOnly();

        public event Action<ActiveSeasonalEvent>? OnEventTriggered;
        public event Action<ActiveSeasonalEvent>? OnEventMitigated;
        public event Action? OnStateChanged;

        public SeasonalEventSystem(ILog? log = null)
        {
            _log = log ?? NullLog.Instance;
        }

        public void BindDefinitions(IEnumerable<SeasonalEventDef> defs)
        {
            _definitions.Clear();
            if (defs != null)
            {
                _definitions.AddRange(defs);
            }
        }

        public void TickDay(int day, string currentSeasonId, ISeededRng rng)
        {
            // 1. Expire old active events
            _activeEvents.RemoveAll(e => e.expiresDay <= day);

            if (_definitions.Count == 0 || string.IsNullOrEmpty(currentSeasonId) || rng == null)
                return;

            // 2. Anti-spam event budget: at most 1 new seasonal event per day
            int triggeredToday = 0;

            foreach (var def in _definitions)
            {
                if (triggeredToday >= 1)
                    break;

                if (!string.Equals(def.season_id, currentSeasonId, StringComparison.OrdinalIgnoreCase))
                    continue;

                // Check if already active
                if (_activeEvents.Exists(e => e.eventId == def.id))
                    continue;

                // Check cooldown
                if (_cooldowns.TryGetValue(def.id, out int lastDay) && (day - lastDay) < def.cooldown_days)
                    continue;

                // Roll deterministic trigger
                double roll = rng.NextDouble();
                if (roll < def.trigger_chance)
                {
                    var active = new ActiveSeasonalEvent
                    {
                        eventId = def.id,
                        name = def.name,
                        seasonId = def.season_id,
                        category = def.category,
                        severity = def.severity,
                        triggeredDay = day,
                        expiresDay = day + 4,
                        isMitigated = false,
                        impactSummary = def.impact_summary,
                        mitigationItemId = def.mitigation_item_id,
                        mitigationCost = def.mitigation_cost
                    };

                    _activeEvents.Add(active);
                    _cooldowns[def.id] = day;
                    triggeredToday++;

                    _log.Info($"[SeasonalEvent] Triggered {def.id} ({def.name}) on day {day} in season {currentSeasonId}");
                    OnEventTriggered?.Invoke(active);
                }
            }

            OnStateChanged?.Invoke();
        }

        public ActionResult Mitigate(string eventId)
        {
            var target = _activeEvents.Find(e => e.eventId == eventId && !e.isMitigated);
            if (target == null)
                return ActionResult.Blocked("not_found", "season.event_not_found");

            target.isMitigated = true;
            if (!_resolvedEvents.Contains(eventId))
                _resolvedEvents.Add(eventId);

            _log.Info($"[SeasonalEvent] Mitigated {eventId}");
            OnEventMitigated?.Invoke(target);
            OnStateChanged?.Invoke();

            return ActionResult.Success("season.event_mitigated");
        }

        public SeasonalEventSaveState CaptureState()
        {
            var save = new SeasonalEventSaveState
            {
                systemId = SystemId,
                resolvedEvents = new List<string>(_resolvedEvents)
            };

            foreach (var a in _activeEvents)
            {
                save.activeEvents.Add(new ActiveSeasonalEvent
                {
                    eventId = a.eventId,
                    name = a.name,
                    seasonId = a.seasonId,
                    category = a.category,
                    severity = a.severity,
                    triggeredDay = a.triggeredDay,
                    expiresDay = a.expiresDay,
                    isMitigated = a.isMitigated,
                    impactSummary = a.impactSummary,
                    mitigationItemId = a.mitigationItemId,
                    mitigationCost = a.mitigationCost
                });
            }

            foreach (var kvp in _cooldowns)
            {
                save.cooldownKeys.Add(kvp.Key);
                save.cooldownDays.Add(kvp.Value);
            }

            return save;
        }

        public void RestoreState(SeasonalEventSaveState? state)
        {
            if (state == null) return;
            _activeEvents.Clear();
            _cooldowns.Clear();
            _resolvedEvents.Clear();

            if (state.activeEvents != null)
                _activeEvents.AddRange(state.activeEvents);

            if (state.resolvedEvents != null)
                _resolvedEvents.AddRange(state.resolvedEvents);

            if (state.cooldownKeys != null && state.cooldownDays != null)
            {
                for (int i = 0; i < state.cooldownKeys.Count && i < state.cooldownDays.Count; i++)
                {
                    _cooldowns[state.cooldownKeys[i]] = state.cooldownDays[i];
                }
            }

            OnStateChanged?.Invoke();
        }
    }
}
