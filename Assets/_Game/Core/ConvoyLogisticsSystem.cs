using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Protocol Zero — Convoy Logistics System. Manages multi-day expeditions
    /// to reach the Silent Cities beyond the standard 12-hour scavenge radius.
    ///
    /// Requires a Snow-Crawler (modified armored snowplow) consuming fuel,
    /// engine oil, and caterpillar tracks. Scavengers on foot must drag a
    /// hand-crank sled — agonizingly slow and lethal if caught in a blizzard.
    ///
    /// Save/load safe. Plain C#.
    /// </summary>
    [Serializable]
    public class ConvoyLogisticsSave
    {
        public string systemId = "convoy_logistics";
        public bool hasSnowCrawler;
        public bool crawlerOperational;
        public float crawlerFuelLiters;
        public float crawlerOilCondition;
        public float crawlerTrackCondition;
        public bool hasHandCrankSled;
        public List<ConvoyDestinationState> knownDestinations = new List<ConvoyDestinationState>();
        public List<ConvoyMissionSave> activeMissions = new List<ConvoyMissionSave>();
    }

    [Serializable]
    public class ConvoyDestinationState
    {
        public string nodeId;
        public string displayName;
        public float distanceKm;
        public float fuelRequiredLiters;
        public float travelHours;
        public bool discovered;
        public bool cleared;
    }

    [Serializable]
    public class ConvoyMissionSave
    {
        public string missionId;
        public string destinationNodeId;
        public List<string> assignedSurvivorIds = new List<string>();
        public int departureDay;
        public float travelProgressHours;
        public float totalTravelHours;
        public bool hasSled;
        public bool sledAbandoned;
        public bool caughtInBlizzard;
    }

    /// <summary>
    /// Events raised by the convoy system.
    /// </summary>
    public struct ConvoyBlizzardEvent
    {
        public string MissionId;
        public List<string> SurvivorIds;
        public bool HasSled;
    }

    public class ConvoyLogisticsSystem
    {
        /// <summary>Fuel consumption per km for the Snow-Crawler.</summary>
        public const float FuelPerKm = 2.5f;

        /// <summary>Oil degradation per hour of travel.</summary>
        public const float OilDegradePerHour = 0.8f;

        /// <summary>Track degradation per hour of travel.</summary>
        public const float TrackDegradePerHour = 1.2f;

        /// <summary>Multiplier for sled carry weight vs normal capacity.</summary>
        public const float SledWeightMultiplier = 3f;

        /// <summary>Fatigue per hour pulling a sled.</summary>
        public const float SledFatiguePerHour = 8f;

        /// <summary>Chance of death if caught in blizzard without sled abandoned.</summary>
        public const float BlizzardDeathChanceWithSled = 0.75f;

        /// <summary>Chance of death if caught in blizzard after abandoning sled.</summary>
        public const float BlizzardDeathChanceWithoutSled = 0.15f;

        // ── Events ────────────────────────────────────────────────────
        public event Action<ConvoyBlizzardEvent> OnBlizzardCaught;
        public event Action<string> OnSledAbandoned; // missionId
        public event Action<string, List<ItemDefinition>> OnConvoyReturned;
        public event Action<string> OnCrawlerDisabled;

        // ── State ─────────────────────────────────────────────────────
        private bool _hasSnowCrawler;
        private bool _crawlerOperational;
        private float _crawlerFuelLiters;
        private float _crawlerOilCondition = 100f;
        private float _crawlerTrackCondition = 100f;
        private bool _hasHandCrankSled;
        private readonly Dictionary<string, ConvoyDestinationState> _destinations = new Dictionary<string, ConvoyDestinationState>();
        private readonly Dictionary<string, ConvoyMissionSave> _activeMissions = new Dictionary<string, ConvoyMissionSave>();
        private int _missionSeq;

        public bool HasSnowCrawler => _hasSnowCrawler;
        public bool IsCrawlerOperational => _crawlerOperational;
        public float CrawlerFuelLiters => _crawlerFuelLiters;
        public float CrawlerOilCondition => _crawlerOilCondition;
        public float CrawlerTrackCondition => _crawlerTrackCondition;
        public bool HasHandCrankSled => _hasHandCrankSled;
        public IReadOnlyDictionary<string, ConvoyDestinationState> Destinations => _destinations;
        public int ActiveMissionCount => _activeMissions.Count;

        // ── Configuration ─────────────────────────────────────────────
        public void RegisterDestination(string nodeId, string displayName, float distanceKm,
            float travelHours, float fuelRequired = -1f)
        {
            if (string.IsNullOrEmpty(nodeId)) return;
            if (_destinations.ContainsKey(nodeId)) return;

            _destinations[nodeId] = new ConvoyDestinationState
            {
                nodeId = nodeId,
                displayName = displayName,
                distanceKm = distanceKm,
                travelHours = travelHours,
                fuelRequiredLiters = fuelRequired > 0f ? fuelRequired : distanceKm * FuelPerKm,
                discovered = true
            };
        }

        /// <summary>Build/repair the Snow-Crawler.</summary>
        public void BuildSnowCrawler()
        {
            _hasSnowCrawler = true;
            _crawlerOperational = true;
            _crawlerFuelLiters = 0f;
            _crawlerOilCondition = 100f;
            _crawlerTrackCondition = 100f;
        }

        /// <summary>Refuel the crawler.</summary>
        public void AddFuel(float liters)
        {
            _crawlerFuelLiters = Mathf.Max(0f, _crawlerFuelLiters + liters);
        }

        /// <summary>Acquire a hand-crank sled.</summary>
        public void AcquireSled()
        {
            _hasHandCrankSled = true;
        }

        // ── Mission Management ─────────────────────────────────────────
        /// <summary>
        /// Launch a convoy mission to a destination. Requires operational
        /// crawler + sufficient fuel, OR a sled for foot travel.
        /// </summary>
        public string LaunchMission(string destinationNodeId, List<string> survivorIds,
            int currentDay, bool useSled = false)
        {
            if (!_destinations.TryGetValue(destinationNodeId, out var dest))
                return null;

            bool useCrawler = _hasSnowCrawler && _crawlerOperational && !useSled;

            if (useCrawler)
            {
                float fuelNeeded = dest.fuelRequiredLiters;
                if (_crawlerFuelLiters < fuelNeeded) return null;
                _crawlerFuelLiters -= fuelNeeded;
            }

            string missionId = $"convoy_{++_missionSeq}";
            var mission = new ConvoyMissionSave
            {
                missionId = missionId,
                destinationNodeId = destinationNodeId,
                assignedSurvivorIds = new List<string>(survivorIds ?? new List<string>()),
                departureDay = currentDay,
                totalTravelHours = dest.travelHours,
                hasSled = useSled,
                sledAbandoned = false,
                caughtInBlizzard = false
            };

            _activeMissions[missionId] = mission;
            return missionId;
        }

        // ── Tick ──────────────────────────────────────────────────────
        /// <summary>
        /// Advance all active convoy missions. Called per game-hour.
        /// </summary>
        public void Tick(float gameHours, int currentDay, bool isBlizzard,
            NeedsSystem needsSystem = null,
            Func<string, Survivor> getSurvivor = null)
        {
            if (gameHours <= 0f) return;

            var completed = new List<string>();
            bool crawlerTravelingThisTick = false;

            foreach (var kv in _activeMissions)
            {
                var mission = kv.Value;

                // Blizzard catch check
                if (isBlizzard && !mission.caughtInBlizzard)
                {
                    // Foot travelers with sled face the choice
                    if (mission.hasSled)
                    {
                        mission.caughtInBlizzard = true;
                        OnBlizzardCaught?.Invoke(new ConvoyBlizzardEvent
                        {
                            MissionId = mission.missionId,
                            SurvivorIds = new List<string>(mission.assignedSurvivorIds),
                            HasSled = true
                        });
                        continue;
                    }
                }

                // Track whether a crawler mission is actively traveling
                if (!mission.hasSled) crawlerTravelingThisTick = true;

                // Advance travel
                float travelSpeed = mission.hasSled ? 0.6f : 1f;
                mission.travelProgressHours += gameHours * travelSpeed;

                // Fatigue from sled pulling
                if (mission.hasSled && !mission.sledAbandoned && needsSystem != null)
                {
                    for (int i = 0; i < mission.assignedSurvivorIds.Count; i++)
                    {
                        var sv = getSurvivor?.Invoke(mission.assignedSurvivorIds[i]);
                        if (sv != null && sv.IsAlive)
                            needsSystem.Modify(sv, NeedKind.Fatigue, SledFatiguePerHour * gameHours);
                    }
                }

                // Mission complete
                if (mission.travelProgressHours >= mission.totalTravelHours)
                    completed.Add(mission.missionId);
            }

            // Crawler degradation — only when actively traveling.
            if (crawlerTravelingThisTick && _hasSnowCrawler && _crawlerOperational)
            {
                _crawlerOilCondition = Mathf.Max(0f, _crawlerOilCondition - OilDegradePerHour * gameHours);
                _crawlerTrackCondition = Mathf.Max(0f, _crawlerTrackCondition - TrackDegradePerHour * gameHours);

                if (_crawlerOilCondition <= 0f || _crawlerTrackCondition <= 0f)
                {
                    _crawlerOperational = false;
                    OnCrawlerDisabled?.Invoke("crawler_breakdown");
                }
            }

            // Process completed missions
            foreach (var id in completed)
            {
                if (_activeMissions.TryGetValue(id, out var mission))
                {
                    _activeMissions.Remove(id);
                    OnConvoyReturned?.Invoke(id, null);
                }
            }
        }

        /// <summary>
        /// Resolve the blizzard choice: abandon sled (lose 80% loot, survive)
        /// or keep pulling (75% death chance).
        /// </summary>
        public bool ResolveBlizzardChoice(string missionId, bool abandonSled, System.Random rng)
        {
            if (!_activeMissions.TryGetValue(missionId, out var mission))
                return false;

            if (abandonSled)
            {
                mission.sledAbandoned = true;
                OnSledAbandoned?.Invoke(missionId);

                // 15% death chance even after abandoning
                if (rng.NextDouble() < BlizzardDeathChanceWithoutSled)
                {
                    _activeMissions.Remove(missionId);
                    return false; // mission failed, survivors died
                }
                mission.caughtInBlizzard = false;
                return true;
            }
            else
            {
                // 75% death chance keeping the sled
                if (rng.NextDouble() < BlizzardDeathChanceWithSled)
                {
                    _activeMissions.Remove(missionId);
                    return false; // mission failed, survivors died
                }
                mission.caughtInBlizzard = false;
                return true;
            }
        }

        // ── Save / Load ────────────────────────────────────────────────
        public ConvoyLogisticsSave CaptureState()
        {
            return new ConvoyLogisticsSave
            {
                hasSnowCrawler = _hasSnowCrawler,
                crawlerOperational = _crawlerOperational,
                crawlerFuelLiters = _crawlerFuelLiters,
                crawlerOilCondition = _crawlerOilCondition,
                crawlerTrackCondition = _crawlerTrackCondition,
                hasHandCrankSled = _hasHandCrankSled,
                knownDestinations = new List<ConvoyDestinationState>(_destinations.Values),
                activeMissions = new List<ConvoyMissionSave>(_activeMissions.Values)
            };
        }

        public void RestoreState(ConvoyLogisticsSave save)
        {
            _destinations.Clear();
            _activeMissions.Clear();
            _missionSeq = 0;

            if (save == null) return;

            _hasSnowCrawler = save.hasSnowCrawler;
            _crawlerOperational = save.crawlerOperational;
            _crawlerFuelLiters = save.crawlerFuelLiters;
            _crawlerOilCondition = save.crawlerOilCondition;
            _crawlerTrackCondition = save.crawlerTrackCondition;
            _hasHandCrankSled = save.hasHandCrankSled;

            if (save.knownDestinations != null)
                for (int i = 0; i < save.knownDestinations.Count; i++)
                    if (save.knownDestinations[i] != null)
                        _destinations[save.knownDestinations[i].nodeId] = save.knownDestinations[i];

            if (save.activeMissions != null)
            {
                for (int i = 0; i < save.activeMissions.Count; i++)
                {
                    var m = save.activeMissions[i];
                    if (m == null) continue;
                    _activeMissions[m.missionId] = m;
                    // Recover mission seq to avoid ID collisions after load.
                    int usIdx = m.missionId.LastIndexOf('_');
                    if (usIdx >= 0 && int.TryParse(m.missionId.Substring(usIdx + 1), out int seq))
                        _missionSeq = Mathf.Max(_missionSeq, seq);
                }
            }
        }
    }
}
