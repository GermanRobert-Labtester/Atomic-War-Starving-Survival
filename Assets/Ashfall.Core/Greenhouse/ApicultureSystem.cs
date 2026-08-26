using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Greenhouse
{
    // ── Hive state ──────────────────────────────────────────────────

    /// <summary>State of one beehive in the greenhouse.</summary>
    [Serializable]
    public class HiveState
    {
        public string hiveId = string.Empty;
        public string greenhouseBayId = string.Empty;
        public float queenVitality = 1.0f;     // 0..1
        public float colonyPopulation = 1.0f;  // 0..1, normalized
        public float temperatureC = 20f;       // ambient temperature
        public float humidityPct = 50f;        // 0..100
        public float contamination = 0f;       // 0..1
        public float radiationStress = 0f;     // 0..1
        public float feedLevel = 1.0f;         // 0..1, sugar water
        public float waterLevel = 1.0f;        // 0..1
        public float honeyBuffer = 0f;         // kg accumulated
        public float waxBuffer = 0f;           // kg accumulated
        public bool isSwarming = false;
        public bool isDead = false;
        public int lastInspectionDay = -1;
        public int installedDay = -1;
        public List<string> linkedPlotIds = new List<string>();
    }

    /// <summary>System-wide apiculture state (save DTO).</summary>
    [Serializable]
    public class ApicultureState
    {
        public string systemId = ApicultureSystem.SystemId;
        public List<HiveState> hives = new List<HiveState>();
        public float totalHoneyProduced = 0f;
        public float totalWaxProduced = 0f;
    }

    // ── System ──────────────────────────────────────────────────────

    /// <summary>
    /// ASHFALL — Greenhouse Apiculture and Radiation-Hardy Bee Aviary.
    /// A maintained hive pollinates configured greenhouse crops and yields
    /// honey and beeswax over time. Temperature, feed, contamination,
    /// disease, radiation, water/power, and queen health constrain output.
    ///
    /// Key invariant: pollination modifies only configured crop types/plots
    /// and uses a bounded multiplier that is data-defined. It does NOT
    /// grant a global 40% harvest increase regardless of conditions.
    /// </summary>
    public class ApicultureSystem
    {
        public const string SystemId = "apiculture_system";

        // Hive constants
        public const float OptimalTemperatureMin = 15f;
        public const float OptimalTemperatureMax = 30f;
        public const float OptimalHumidityMin = 40f;
        public const float OptimalHumidityMax = 70f;
        public const float FeedConsumptionPerDay = 0.02f;
        public const float WaterConsumptionPerDay = 0.03f;
        public const float QueenAgingRate = 0.001f;      // per day
        public const float PopulationGrowthRate = 0.01f;  // per day when healthy
        public const float PopulationDeclineRate = 0.03f; // per day when stressed
        public const float SwarmThreshold = 0.9f;         // population above this triggers swarm risk
        public const float SwarmChance = 0.05f;           // per day when above threshold
        public const float DeathThreshold = 0.05f;        // population below this = colony death

        // Production constants
        public const float HoneyPerDayPerPop = 0.01f;     // kg per population unit per day
        public const float WaxPerDayPerPop = 0.005f;      // kg per population unit per day
        public const float MaxHoneyBuffer = 5f;            // kg
        public const float MaxWaxBuffer = 2f;              // kg

        // Pollination constants
        public const float MaxPollinationBonus = 0.25f;    // 25% max yield increase
        public const float PollinationPerPopulation = 0.3f; // pollination strength per population unit

        // Contamination/radiation thresholds
        public const float ContaminationStressThreshold = 0.2f;
        public const float RadiationStressThreshold = 0.3f;

        private readonly ApicultureState _state = new ApicultureState();
        private readonly Dictionary<string, HiveState> _hives = new Dictionary<string, HiveState>();

        // Events
        public event Action<string> OnHiveInstalled;           // hiveId
        public event Action<string> OnInspectionCompleted;     // hiveId
        public event Action<string, float> OnPollinationChanged; // hiveId, strength
        public event Action<string, float, float> OnProductionTick; // hiveId, honey, wax
        public event Action<string> OnColonyStressed;          // hiveId
        public event Action<string> OnColonySwarming;          // hiveId
        public event Action<string> OnColonyDied;              // hiveId
        public event Action<string> OnMedicalProcessingCompleted; // hiveId
        public event Action<ApicultureState> OnStateChanged;

        public ApicultureState State => _state;
        public IReadOnlyDictionary<string, HiveState> Hives => _hives;

        public ApicultureSystem()
        {
        }

        // ── Hive management ──────────────────────────────────────────

        /// <summary>Install a new hive in a greenhouse bay.</summary>
        public bool InstallHive(string hiveId, string bayId, int day)
        {
            if (string.IsNullOrEmpty(hiveId) || string.IsNullOrEmpty(bayId)) return false;
            if (_hives.ContainsKey(hiveId)) return false;

            var hive = new HiveState
            {
                hiveId = hiveId,
                greenhouseBayId = bayId,
                queenVitality = 1.0f,
                colonyPopulation = 0.5f, // starting colony
                temperatureC = 20f,
                humidityPct = 50f,
                contamination = 0f,
                radiationStress = 0f,
                feedLevel = 1.0f,
                waterLevel = 1.0f,
                honeyBuffer = 0f,
                waxBuffer = 0f,
                isSwarming = false,
                isDead = false,
                lastInspectionDay = -1,
                installedDay = day,
                linkedPlotIds = new List<string>()
            };

            _hives[hiveId] = hive;
            _state.hives.Add(hive);
            OnHiveInstalled?.Invoke(hiveId);
            RaiseChanged();
            return true;
        }

        /// <summary>Link a hive to specific greenhouse plots for pollination.</summary>
        public bool LinkPlots(string hiveId, List<string> plotIds)
        {
            if (!_hives.TryGetValue(hiveId, out var hive)) return false;
            hive.linkedPlotIds.Clear();
            if (plotIds != null) hive.linkedPlotIds.AddRange(plotIds);
            RaiseChanged();
            return true;
        }

        // ── Daily tick ───────────────────────────────────────────────

        /// <summary>
        /// Advance one day for all hives. Updates population, queen vitality,
        /// feed/water consumption, honey/wax production, and pollination strength.
        /// </summary>
        public void TickDaily(int day, float greenhouseTemperatureC, float greenhouseContamination, float radiationLevel, ISeededRng rng)
        {
            foreach (var hive in _hives.Values)
            {
                if (hive.isDead) continue;

                // Update environment
                hive.temperatureC = greenhouseTemperatureC;
                hive.contamination = greenhouseContamination;
                hive.radiationStress = Math.Clamp(radiationLevel / 100f, 0f, 1f);

                // Feed and water consumption
                hive.feedLevel = Math.Max(0f, hive.feedLevel - FeedConsumptionPerDay);
                hive.waterLevel = Math.Max(0f, hive.waterLevel - WaterConsumptionPerDay);

                // Queen aging
                hive.queenVitality = Math.Max(0f, hive.queenVitality - QueenAgingRate);

                // Population dynamics
                float stressFactor = CalculateStressFactor(hive);
                if (stressFactor < 0.3f)
                {
                    // Healthy: population grows
                    hive.colonyPopulation = Math.Min(1f, hive.colonyPopulation + PopulationGrowthRate * (1f - stressFactor));
                }
                else
                {
                    // Stressed: population declines
                    hive.colonyPopulation = Math.Max(0f, hive.colonyPopulation - PopulationDeclineRate * stressFactor);
                    if (stressFactor > 0.5f)
                        OnColonyStressed?.Invoke(hive.hiveId);
                }

                // Colony death check
                if (hive.colonyPopulation < DeathThreshold)
                {
                    hive.isDead = true;
                    OnColonyDied?.Invoke(hive.hiveId);
                    continue;
                }

                // Swarm check
                if (hive.colonyPopulation > SwarmThreshold && !hive.isSwarming)
                {
                    if (rng != null && rng.NextDouble() < SwarmChance)
                    {
                        hive.isSwarming = true;
                        hive.colonyPopulation *= 0.5f; // half the colony leaves
                        OnColonySwarming?.Invoke(hive.hiveId);
                    }
                }
                else if (hive.isSwarming && hive.colonyPopulation < 0.7f)
                {
                    hive.isSwarming = false; // swarm resolved
                }

                // Honey and wax production
                float productionRate = hive.colonyPopulation * (1f - stressFactor);
                float honey = HoneyPerDayPerPop * productionRate;
                float wax = WaxPerDayPerPop * productionRate;

                hive.honeyBuffer = Math.Min(MaxHoneyBuffer, hive.honeyBuffer + honey);
                hive.waxBuffer = Math.Min(MaxWaxBuffer, hive.waxBuffer + wax);

                _state.totalHoneyProduced += honey;
                _state.totalWaxProduced += wax;

                OnProductionTick?.Invoke(hive.hiveId, honey, wax);
            }

            if (_hives.Count > 0)
                RaiseChanged();
        }

        // ── Pollination ──────────────────────────────────────────────

        /// <summary>
        /// Get the pollination bonus for a specific plot. Returns 0..MaxPollinationBonus.
        /// Only applies to plots linked to healthy hives.
        /// </summary>
        public float GetPollinationBonus(string plotId)
        {
            float totalBonus = 0f;
            foreach (var hive in _hives.Values)
            {
                if (hive.isDead || !hive.linkedPlotIds.Contains(plotId)) continue;

                float strength = hive.colonyPopulation * PollinationPerPopulation;
                strength *= (1f - CalculateStressFactor(hive));
                totalBonus += strength;
            }
            return Math.Min(MaxPollinationBonus, totalBonus);
        }

        /// <summary>Get pollination strength for a hive (0..1).</summary>
        public float GetHivePollinationStrength(string hiveId)
        {
            if (!_hives.TryGetValue(hiveId, out var hive)) return 0f;
            if (hive.isDead) return 0f;
            return hive.colonyPopulation * (1f - CalculateStressFactor(hive));
        }

        // ── Inspection ───────────────────────────────────────────────

        /// <summary>Inspect a hive (reveals condition, resets inspection timer).</summary>
        public HiveState? InspectHive(string hiveId, int day)
        {
            if (!_hives.TryGetValue(hiveId, out var hive)) return null;
            hive.lastInspectionDay = day;
            OnInspectionCompleted?.Invoke(hiveId);
            RaiseChanged();
            return hive;
        }

        // ── Maintenance ──────────────────────────────────────────────

        /// <summary>Refill feed for a hive.</summary>
        public bool RefillFeed(string hiveId, float amount = 1f)
        {
            if (!_hives.TryGetValue(hiveId, out var hive)) return false;
            hive.feedLevel = Math.Min(1f, hive.feedLevel + Math.Max(0f, amount));
            RaiseChanged();
            return true;
        }

        /// <summary>Refill water for a hive.</summary>
        public bool RefillWater(string hiveId, float amount = 1f)
        {
            if (!_hives.TryGetValue(hiveId, out var hive)) return false;
            hive.waterLevel = Math.Min(1f, hive.waterLevel + Math.Max(0f, amount));
            RaiseChanged();
            return true;
        }

        /// <summary>Replace queen in a hive.</summary>
        public bool ReplaceQueen(string hiveId)
        {
            if (!_hives.TryGetValue(hiveId, out var hive)) return false;
            hive.queenVitality = 1.0f;
            RaiseChanged();
            return true;
        }

        // ── Harvest ──────────────────────────────────────────────────

        /// <summary>Harvest accumulated honey and wax from a hive.</summary>
        public (float honey, float wax) Harvest(string hiveId)
        {
            if (!_hives.TryGetValue(hiveId, out var hive)) return (0f, 0f);
            float honey = hive.honeyBuffer;
            float wax = hive.waxBuffer;
            hive.honeyBuffer = 0f;
            hive.waxBuffer = 0f;
            RaiseChanged();
            return (honey, wax);
        }

        // ── Queries ──────────────────────────────────────────────────

        public HiveState? GetHive(string hiveId)
        {
            return _hives.TryGetValue(hiveId, out var hive) ? hive : null;
        }

        public int GetAliveHiveCount()
        {
            int count = 0;
            foreach (var h in _hives.Values) if (!h.isDead) count++;
            return count;
        }

        // ── Stress calculation ───────────────────────────────────────

        private float CalculateStressFactor(HiveState hive)
        {
            float stress = 0f;

            // Temperature stress
            if (hive.temperatureC < OptimalTemperatureMin)
                stress += (OptimalTemperatureMin - hive.temperatureC) / 20f;
            else if (hive.temperatureC > OptimalTemperatureMax)
                stress += (hive.temperatureC - OptimalTemperatureMax) / 20f;

            // Humidity stress
            if (hive.humidityPct < OptimalHumidityMin)
                stress += (OptimalHumidityMin - hive.humidityPct) / 50f;
            else if (hive.humidityPct > OptimalHumidityMax)
                stress += (hive.humidityPct - OptimalHumidityMax) / 50f;

            // Feed/water deprivation
            if (hive.feedLevel <= 0f) stress += 0.3f;
            else if (hive.feedLevel < 0.2f) stress += 0.1f;
            if (hive.waterLevel <= 0f) stress += 0.3f;
            else if (hive.waterLevel < 0.2f) stress += 0.1f;

            // Contamination stress
            if (hive.contamination > ContaminationStressThreshold)
                stress += (hive.contamination - ContaminationStressThreshold) * 0.5f;

            // Radiation stress
            if (hive.radiationStress > RadiationStressThreshold)
                stress += (hive.radiationStress - RadiationStressThreshold) * 0.5f;

            // Queen vitality
            stress += (1f - hive.queenVitality) * 0.2f;

            return Math.Clamp(stress, 0f, 1f);
        }

        // ── Save / Load ──────────────────────────────────────────────

        public ApicultureState CaptureState()
        {
            var copy = new ApicultureState
            {
                systemId = _state.systemId,
                totalHoneyProduced = _state.totalHoneyProduced,
                totalWaxProduced = _state.totalWaxProduced
            };
            var sorted = new List<HiveState>(_state.hives);
            sorted.Sort((a, b) => string.CompareOrdinal(a.hiveId, b.hiveId));
            foreach (var h in sorted)
            {
                copy.hives.Add(new HiveState
                {
                    hiveId = h.hiveId,
                    greenhouseBayId = h.greenhouseBayId,
                    queenVitality = h.queenVitality,
                    colonyPopulation = h.colonyPopulation,
                    temperatureC = h.temperatureC,
                    humidityPct = h.humidityPct,
                    contamination = h.contamination,
                    radiationStress = h.radiationStress,
                    feedLevel = h.feedLevel,
                    waterLevel = h.waterLevel,
                    honeyBuffer = h.honeyBuffer,
                    waxBuffer = h.waxBuffer,
                    isSwarming = h.isSwarming,
                    isDead = h.isDead,
                    lastInspectionDay = h.lastInspectionDay,
                    installedDay = h.installedDay,
                    linkedPlotIds = new List<string>(h.linkedPlotIds)
                });
            }
            return copy;
        }

        public void RestoreState(ApicultureState saved)
        {
            if (saved == null) return;
            _state.systemId = SystemId;
            _hives.Clear();
            _state.hives.Clear();
            _state.totalHoneyProduced = saved.totalHoneyProduced;
            _state.totalWaxProduced = saved.totalWaxProduced;
            if (saved.hives != null)
            {
                foreach (var h in saved.hives)
                {
                    if (h == null || string.IsNullOrEmpty(h.hiveId)) continue;
                    var copy = new HiveState
                    {
                        hiveId = h.hiveId,
                        greenhouseBayId = h.greenhouseBayId,
                        queenVitality = Math.Clamp(h.queenVitality, 0f, 1f),
                        colonyPopulation = Math.Clamp(h.colonyPopulation, 0f, 1f),
                        temperatureC = h.temperatureC,
                        humidityPct = Math.Clamp(h.humidityPct, 0f, 100f),
                        contamination = Math.Clamp(h.contamination, 0f, 1f),
                        radiationStress = Math.Clamp(h.radiationStress, 0f, 1f),
                        feedLevel = Math.Clamp(h.feedLevel, 0f, 1f),
                        waterLevel = Math.Clamp(h.waterLevel, 0f, 1f),
                        honeyBuffer = Math.Max(0f, h.honeyBuffer),
                        waxBuffer = Math.Max(0f, h.waxBuffer),
                        isSwarming = h.isSwarming,
                        isDead = h.isDead,
                        lastInspectionDay = h.lastInspectionDay,
                        installedDay = h.installedDay,
                        linkedPlotIds = h.linkedPlotIds != null ? new List<string>(h.linkedPlotIds) : new List<string>()
                    };
                    _hives[copy.hiveId] = copy;
                    _state.hives.Add(copy);
                }
            }
            RaiseChanged();
        }

        private void RaiseChanged() => OnStateChanged?.Invoke(_state);
    }
}
