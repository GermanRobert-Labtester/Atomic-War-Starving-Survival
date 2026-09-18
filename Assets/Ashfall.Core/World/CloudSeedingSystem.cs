// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Inventory;
using Ashfall.Core.Random;

namespace Ashfall.Core.World
{
    [Serializable]
    public sealed class CloudSeedingSaveState
    {
        public bool isInstalled;
        public int installDay;
        public int cooldownRemaining;
        public int lastDeployedDay;
        public WeatherKind lastTargetWeather;
        public bool partialProtectionActive;
        public int partialProtectionDay;
    }

    public sealed class CloudSeedingResult
    {
        public bool Success { get; }
        public string Status { get; }
        public string? FailureReason { get; }
        public IReadOnlyDictionary<string, int> MaterialsConsumed { get; }
        public float SuccessChance { get; }
        public WeatherKind TargetWeather { get; }
        public int TargetDay { get; }
        public int CooldownApplied { get; }
        public bool PartialProtectionApplied { get; }

        public CloudSeedingResult(
            bool success,
            string status,
            string? failureReason,
            IReadOnlyDictionary<string, int> materialsConsumed,
            float successChance,
            WeatherKind targetWeather,
            int targetDay,
            int cooldownApplied,
            bool partialProtectionApplied)
        {
            Success = success;
            Status = status;
            FailureReason = failureReason;
            MaterialsConsumed = materialsConsumed ?? new Dictionary<string, int>();
            SuccessChance = Math.Clamp(successChance, 0f, 1f);
            TargetWeather = targetWeather;
            TargetDay = targetDay;
            CooldownApplied = cooldownApplied;
            PartialProtectionApplied = partialProtectionApplied;
        }

        public static CloudSeedingResult Failed(
            string failureReason,
            WeatherKind targetWeather,
            int targetDay,
            float successChance = 0f)
        {
            return new CloudSeedingResult(
                success: false,
                status: "failed",
                failureReason: failureReason,
                materialsConsumed: new Dictionary<string, int>(),
                successChance: successChance,
                targetWeather: targetWeather,
                targetDay: targetDay,
                cooldownApplied: 0,
                partialProtectionApplied: false);
        }
    }

    /// <summary>
    /// ASHFALL — Cloud Seeding Countermeasure System (Plan 14D / C1.5).
    /// Strategic weather counterplay system that cancels predicted severe weather events
    /// using silver-iodide canisters or chemical reagents.
    /// Engine-agnostic (Core). Deterministic RNG, atomic consumption, 7-day cooldown.
    /// </summary>
    public sealed class CloudSeedingSystem
    {
        public const string RequiredKnowledgeId = "knowledge_atmospheric_cloud_seeding";
        public const string PrimaryCanisterId = "item_cloud_seeding_canister";
        public const int StandardCooldownDays = 7;

        private readonly WeatherSystem _weather;
        private readonly WeatherStationSystem? _station;
        private readonly Inventory.Inventory? _inventory;
        private readonly ISeededRng _rng;
        private readonly ILog _log;

        private CloudSeedingSaveState _state = new CloudSeedingSaveState();
        private int _currentDay = 1;

        public CloudSeedingSaveState State => _state;
        public bool IsInstalled => _state.isInstalled;
        public bool IsOnCooldown => _state.cooldownRemaining > 0;
        public int CooldownRemaining => _state.cooldownRemaining;
        public bool PartialProtectionActive => _state.partialProtectionActive && _state.partialProtectionDay == _currentDay;

        /// <summary>
        /// Optional research gate provider. Returns true if the research is unlocked.
        /// </summary>
        public Func<string, bool>? ResearchUnlockedProvider { get; set; }

        public event Action<CloudSeedingResult>? OnCloudSeedingDeployed;
        public event Action? OnStateChanged;

        public CloudSeedingSystem(
            WeatherSystem weather,
            WeatherStationSystem? station,
            Inventory.Inventory? inventory,
            ISeededRng rng,
            ILog? log = null)
        {
            _weather = weather ?? throw new ArgumentNullException(nameof(weather));
            _station = station;
            _inventory = inventory;
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _log = log ?? NullLog.Instance;
        }

        public ActionResult Install(int day)
        {
            if (_state.isInstalled)
                return ActionResult.Blocked("already_installed", "cloud_seeding.already_installed");

            _state.isInstalled = true;
            _state.installDay = day;
            _log.Info($"[CloudSeeding] Installed on day {day}");
            OnStateChanged?.Invoke();
            return ActionResult.Success("cloud_seeding.installed");
        }

        public (bool CanDeploy, string? Reason, float SuccessChance, Dictionary<string, int> Cost) PreflightDeploy(
            int day,
            WeatherKind targetWeather,
            int targetDay = 0)
        {
            if (!_state.isInstalled)
                return (false, "cloud_seeding.not_installed", 0f, new Dictionary<string, int>());

            if (IsOnCooldown)
                return (false, "cloud_seeding.cooldown_active", 0f, new Dictionary<string, int>());

            if (ResearchUnlockedProvider != null && !ResearchUnlockedProvider(RequiredKnowledgeId))
                return (false, "cloud_seeding.research_locked", 0f, new Dictionary<string, int>());

            if (!IsEligibleSevereWeather(targetWeather))
                return (false, "cloud_seeding.illegal_weather_target", 0f, new Dictionary<string, int>());

            float chance = ComputeSuccessChance(day, targetWeather, targetDay);
            var cost = ResolveDeploymentCost();

            if (_inventory != null && !HasMaterials(cost))
                return (false, "cloud_seeding.insufficient_materials", chance, cost);

            return (true, null, chance, cost);
        }

        public CloudSeedingResult Deploy(int day, WeatherKind targetWeather, int targetDay = 0)
        {
            _currentDay = day;
            var preflight = PreflightDeploy(day, targetWeather, targetDay);
            if (!preflight.CanDeploy)
            {
                return CloudSeedingResult.Failed(
                    preflight.Reason ?? "preflight_failed",
                    targetWeather,
                    targetDay,
                    preflight.SuccessChance);
            }

            // Atomic material consumption
            if (_inventory != null)
            {
                foreach (var kvp in preflight.Cost)
                {
                    _inventory.Remove(kvp.Key, kvp.Value);
                }
            }

            float roll = (float)_rng.NextDouble();
            bool success = roll <= preflight.SuccessChance;

            bool partialProtection = false;
            if (success)
            {
                // Successful weather cancellation
                if (targetDay <= day || targetDay == 0)
                {
                    _weather.ForceWeather(WeatherKind.Clear);
                }

                if (_station != null)
                {
                    int matchDay = targetDay > 0 ? targetDay : day;
                    foreach (var f in _station.State.cachedForecast)
                    {
                        if (f.day == matchDay)
                        {
                            f.weather = WeatherKind.Clear;
                            f.isRouteSafe = true;
                        }
                    }
                }
                _log.Info($"[CloudSeeding] Successfully seeded atmosphere for {targetWeather} on day {targetDay} (roll={roll:F2} <= chance={preflight.SuccessChance:F2})");
            }
            else
            {
                // Failure: partial protection if kinetic / hail
                if (targetWeather == WeatherKind.GlassStorm || targetWeather == WeatherKind.RadHail)
                {
                    partialProtection = true;
                    _state.partialProtectionActive = true;
                    _state.partialProtectionDay = targetDay > 0 ? targetDay : day;
                }
                _log.Info($"[CloudSeeding] Deployment failed for {targetWeather} on day {targetDay} (roll={roll:F2} > chance={preflight.SuccessChance:F2})");
            }

            // Apply 7-day cooldown regardless of success/failure
            _state.cooldownRemaining = StandardCooldownDays;
            _state.lastDeployedDay = day;
            _state.lastTargetWeather = targetWeather;

            var result = new CloudSeedingResult(
                success: success,
                status: success ? "success" : "failed",
                failureReason: success ? null : "deployment_roll_failed",
                materialsConsumed: preflight.Cost,
                successChance: preflight.SuccessChance,
                targetWeather: targetWeather,
                targetDay: targetDay,
                cooldownApplied: StandardCooldownDays,
                partialProtectionApplied: partialProtection);

            OnCloudSeedingDeployed?.Invoke(result);
            OnStateChanged?.Invoke();
            return result;
        }

        public void TickDay(int day)
        {
            _currentDay = day;
            if (_state.cooldownRemaining > 0)
            {
                _state.cooldownRemaining--;
            }

            if (_state.partialProtectionActive && _state.partialProtectionDay < day)
            {
                _state.partialProtectionActive = false;
            }
            OnStateChanged?.Invoke();
        }

        public CloudSeedingSaveState CaptureState()
        {
            return new CloudSeedingSaveState
            {
                isInstalled = _state.isInstalled,
                installDay = _state.installDay,
                cooldownRemaining = _state.cooldownRemaining,
                lastDeployedDay = _state.lastDeployedDay,
                lastTargetWeather = _state.lastTargetWeather,
                partialProtectionActive = _state.partialProtectionActive,
                partialProtectionDay = _state.partialProtectionDay
            };
        }

        public void RestoreState(CloudSeedingSaveState? saved)
        {
            if (saved == null)
            {
                _state = new CloudSeedingSaveState();
                return;
            }

            _state = new CloudSeedingSaveState
            {
                isInstalled = saved.isInstalled,
                installDay = saved.installDay,
                cooldownRemaining = Math.Max(0, saved.cooldownRemaining),
                lastDeployedDay = saved.lastDeployedDay,
                lastTargetWeather = saved.lastTargetWeather,
                partialProtectionActive = saved.partialProtectionActive,
                partialProtectionDay = saved.partialProtectionDay
            };
            OnStateChanged?.Invoke();
        }

        private float ComputeSuccessChance(int day, WeatherKind targetWeather, int targetDay)
        {
            float baseChance = 0.70f;
            if (_station != null && _station.IsOperational && _station.State.isCalibrated)
            {
                baseChance += 0.10f;
            }

            // -20% penalty if deployed during the active crisis itself
            if (targetDay <= day && _weather.Current == targetWeather)
            {
                baseChance -= 0.20f;
            }

            return Math.Clamp(baseChance, 0.10f, 0.95f);
        }

        private Dictionary<string, int> ResolveDeploymentCost()
        {
            var cost = new Dictionary<string, int>(StringComparer.Ordinal);
            if (_inventory != null && _inventory.CountById(PrimaryCanisterId) > 0)
            {
                cost[PrimaryCanisterId] = 1;
            }
            else
            {
                // Secondary reagent cost
                cost["chemicals"] = 3;
                cost["fuel"] = 2;
            }
            return cost;
        }

        private bool HasMaterials(Dictionary<string, int> cost)
        {
            if (_inventory == null) return true;
            foreach (var kvp in cost)
            {
                if (_inventory.CountById(kvp.Key) < kvp.Value)
                    return false;
            }
            return true;
        }

        private static bool IsEligibleSevereWeather(WeatherKind kind)
        {
            return kind switch
            {
                WeatherKind.FalloutStorm or
                WeatherKind.Blizzard or
                WeatherKind.GlassStorm or
                WeatherKind.RadHail or
                WeatherKind.EMPStorm or
                WeatherKind.AcidSnow or
                WeatherKind.BlackRain or
                WeatherKind.BioFog or
                WeatherKind.IceStorm => true,
                _ => false
            };
        }
    }
}
