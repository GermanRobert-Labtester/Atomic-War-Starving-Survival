using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Shelter;

namespace Ashfall.Core.World
{
    /// <summary>
    /// Cryo-ash weather hardening and thermal insulation maintenance.
    /// Owns installed upgrades, intake ice, pipe freeze progression,
    /// insulation wear, and auxiliary heat deployment.
    /// Consumes existing domain authorities; never duplicates them.
    /// </summary>
    public sealed class WeatherHardeningSystem
    {
        public const string SystemId = "weather_hardening";
        public const float IntakeIceBlockageThreshold = 100f;
        public const float PipeFreezeBurstThreshold = 100f;
        public const float InsulationCriticalThreshold = 25f;

        private WeatherHardeningState _state;
        private readonly ISeededRng _rng;
        private readonly ILog _log;
        private readonly WeatherSystem? _weather;
        private readonly ShelterThermalSystem? _thermal;
        private readonly PowerGridSystem? _powerGrid;
        private readonly WaterTreatmentSystem? _waterTreatment;
        private readonly Ashfall.Core.Inventory.Inventory? _inventory;
        private readonly Dictionary<string, WeatherHardeningUpgradeDef> _upgrades = new(StringComparer.Ordinal);
        private int _currentDay;

        public WeatherHardeningState State => _state;
        public float GlobalIntakeIce => _state.globalIntakeIce;
        public bool ManifoldFrozen => _state.manifoldFrozen;
        public IReadOnlyDictionary<string, WeatherHardeningUpgradeDef> Upgrades => _upgrades;

        public event Action? OnIntakeBlocked;
        public event Action? OnIntakeCleared;
        public event Action<string, int>? OnPipeFrozen;       // zoneId, day
        public event Action<string, int, float>? OnPipeBurst; // pipeId, day, severity
        public event Action<string>? OnInsulationCritical;   // zoneId
        public event Action<string>? OnAuxiliaryHeatDepleted;// zoneId
        public event Action<string, float>? OnFrostHeaveDetected; // zoneId, stress

        public WeatherHardeningSystem(
            WeatherHardeningState? state,
            ISeededRng rng,
            ILog? log = null,
            WeatherSystem? weather = null,
            ShelterThermalSystem? thermal = null,
            PowerGridSystem? powerGrid = null,
            WaterTreatmentSystem? waterTreatment = null,
            Ashfall.Core.Inventory.Inventory? inventory = null)
        {
            _state = state ?? new WeatherHardeningState();
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _log = log ?? NullLog.Instance;
            _weather = weather;
            _thermal = thermal;
            _powerGrid = powerGrid;
            _waterTreatment = waterTreatment;
            _inventory = inventory;
        }

        // ── Catalog ──────────────────────────────────────────────────

        public void RegisterUpgrade(WeatherHardeningUpgradeDef def)
        {
            if (def != null && !string.IsNullOrEmpty(def.UpgradeId))
                _upgrades[def.UpgradeId] = def;
        }

        public void LoadCatalog(WeatherHardeningCatalog? catalog)
        {
            if (catalog?.Upgrades == null) return;
            foreach (var def in catalog.Upgrades)
                RegisterUpgrade(def);
        }

        // ── Queries ──────────────────────────────────────────────────

        public bool IsUpgradeInstalled(string upgradeId)
        {
            return _state.installedUpgrades.Exists(u => u.upgradeId == upgradeId);
        }

        public float GetInstalledFreezeProtection(string zoneId)
        {
            float total = 0f;
            foreach (var up in _state.installedUpgrades.Where(u => u.zoneId == zoneId))
            {
                if (_upgrades.TryGetValue(up.upgradeId, out var def))
                    total += def.FreezeRiskReduction;
            }
            return Math.Clamp(total, 0f, 0.95f);
        }

        public float GetInstalledThermalRetention(string zoneId)
        {
            float total = 0f;
            foreach (var up in _state.installedUpgrades.Where(u => u.zoneId == zoneId))
            {
                if (_upgrades.TryGetValue(up.upgradeId, out var def))
                    total += def.ThermalRetention;
            }
            return Math.Clamp(total, 0f, 0.95f);
        }

        public float GetUpgradeElectricalDraw(string zoneId)
        {
            float total = 0f;
            foreach (var up in _state.installedUpgrades.Where(u => u.zoneId == zoneId))
            {
                if (_upgrades.TryGetValue(up.upgradeId, out var def))
                    total += def.ElectricalWatts;
            }
            return Math.Max(0f, total);
        }

        // ── Actions ──────────────────────────────────────────────────

        public ActionResult InstallUpgrade(string upgradeId, string zoneId)
        {
            if (string.IsNullOrEmpty(upgradeId)) return ActionResult.Failed("invalid_upgrade", "hardening.invalid_upgrade");
            if (string.IsNullOrEmpty(zoneId)) return ActionResult.Failed("invalid_zone", "hardening.invalid_zone");
            if (!_upgrades.TryGetValue(upgradeId, out var def)) return ActionResult.Failed("unknown_upgrade", "hardening.unknown_upgrade");
            if (_state.installedUpgrades.Exists(u => u.upgradeId == upgradeId && u.zoneId == zoneId))
                return ActionResult.Blocked("already_installed", "hardening.already_installed");

            if (_inventory == null)
                return ActionResult.Failed("no_inventory", "hardening.no_inventory");

            foreach (var cost in def.MaterialCosts)
            {
                if (!_inventory.HasSufficient(cost.ItemId, cost.Amount))
                    return ActionResult.Blocked("insufficient_materials", "hardening.insufficient_materials",
                        new Dictionary<string, double> { { "needed", cost.Amount } });
            }

            foreach (var cost in def.MaterialCosts)
            {
                _inventory.TryConsume(cost.ItemId, cost.Amount);
            }

            _state.installedUpgrades.Add(new InstalledUpgrade
            {
                upgradeId = upgradeId,
                zoneId = zoneId,
                installDay = _currentDay,
                condition = 100f
            });

            _log.Info($"[WeatherHardening] Installed {def.DisplayName} in {zoneId}");
            return ActionResult.Success("hardening.upgrade_installed");
        }

        public ActionResult ToggleAuxiliaryHeat(string zoneId, bool active)
        {
            if (string.IsNullOrEmpty(zoneId)) return ActionResult.Failed("invalid_zone", "hardening.invalid_zone");
            var zone = GetOrCreateZone(zoneId);
            if (zone.auxiliaryHeatActive == active) return ActionResult.Success("hardening.aux_heat_noop");

            if (active)
            {
                // Find an installed auxiliary heat upgrade
                var auxUpgrade = _state.installedUpgrades
                    .Where(u => u.zoneId == zoneId && _upgrades.TryGetValue(u.upgradeId, out var d) && d.HeatOutputKw > 0f)
                    .FirstOrDefault();
                if (auxUpgrade == null)
                    return ActionResult.Blocked("no_aux_heater", "hardening.no_aux_heater");

                if (_upgrades.TryGetValue(auxUpgrade.upgradeId, out var def) && !string.IsNullOrEmpty(def.FuelItemId))
                {
                    if (_inventory == null || !_inventory.HasSufficient(def.FuelItemId, 1))
                        return ActionResult.Blocked("no_fuel", "hardening.no_fuel");
                }
            }

            zone.auxiliaryHeatActive = active;
            _log.Info($"[WeatherHardening] Aux heat in {zoneId} set to {active}");
            return ActionResult.Success("hardening.aux_heat_toggled");
        }

        // ── Daily Tick ───────────────────────────────────────────────

        public void TickDay(int day)
        {
            _currentDay = day;
            if (day <= _state.lastProcessedDay) return;
            _state.lastProcessedDay = day;

            // 1. Weather snapshot
            var weatherKind = _weather?.Current ?? WeatherKind.Clear;
            bool isExtremeCold = weatherKind == WeatherKind.Blizzard || weatherKind == WeatherKind.FalloutStorm;
            float outdoorTemp = _weather?.GetTemperaturePenaltyCelsius() ?? 0f;

            // 2. Power availability snapshot
            bool powerGranted(string zoneId) => _powerGrid?.IsRoomPowered(zoneId) ?? false;

            // 3. Apply active protection + update intake ice
            float accumulationRate = isExtremeCold ? 2.5f : 0.5f;
            float passiveThaw = Math.Max(0f, outdoorTemp) * 0.1f;

            foreach (var up in _state.installedUpgrades)
            {
                if (!_upgrades.TryGetValue(up.upgradeId, out var def)) continue;
                if (def.TargetType == "ventilation-intake" && def.ElectricalWatts > 0f)
                {
                    if (powerGranted(up.zoneId))
                        accumulationRate -= def.FreezeRiskReduction * 1.5f;
                }
            }

            _state.globalIntakeIce = Math.Clamp(_state.globalIntakeIce + accumulationRate - passiveThaw, 0f, 100f);

            bool wasBlocked = _state.manifoldFrozen;
            _state.manifoldFrozen = _state.globalIntakeIce >= IntakeIceBlockageThreshold;
            if (!wasBlocked && _state.manifoldFrozen)
                OnIntakeBlocked?.Invoke();
            else if (wasBlocked && !_state.manifoldFrozen)
                OnIntakeCleared?.Invoke();

            // 4. Update zone hardening states
            foreach (var zone in _state.zones)
            {
                // Insulation wear from condensation cycles (simplified)
                zone.insulationHealth = Math.Max(0f, zone.insulationHealth - 0.05f);

                // Auxiliary heat fuel consumption
                if (zone.auxiliaryHeatActive)
                {
                    var auxUpgrade = _state.installedUpgrades
                        .Where(u => u.zoneId == zone.zoneId && _upgrades.TryGetValue(u.upgradeId, out var d) && d.HeatOutputKw > 0f)
                        .FirstOrDefault();
                    if (auxUpgrade != null && _upgrades.TryGetValue(auxUpgrade.upgradeId, out var auxDef) && !string.IsNullOrEmpty(auxDef.FuelItemId))
                    {
                        float needed = auxDef.FuelConsumptionPerDay;
                        if (_inventory != null && _inventory.HasSufficient(auxDef.FuelItemId, (int)Math.Ceiling(needed)))
                        {
                            _inventory.TryConsume(auxDef.FuelItemId, (int)Math.Ceiling(needed));
                        }
                        else
                        {
                            zone.auxiliaryHeatActive = false;
                            OnAuxiliaryHeatDepleted?.Invoke(zone.zoneId);
                        }
                    }
                }

                // Critical insulation warning
                if (zone.insulationHealth < InsulationCriticalThreshold)
                    OnInsulationCritical?.Invoke(zone.zoneId);
            }

            // 5. Pipe freeze progression (integrated with thermal)
            if (_thermal != null)
            {
                foreach (var room in _thermal.State.rooms)
                {
                    var zone = GetOrCreateZone(room.roomId);
                    float temp = room.currentTempC;
                    float protection = GetInstalledFreezeProtection(room.roomId);

                    if (temp < 0f)
                    {
                        float freezeRate = 0.1f * (1f - protection);
                        zone.pipeFreezeProgress = Math.Min(PipeFreezeBurstThreshold, zone.pipeFreezeProgress + freezeRate);
                    }
                    else if (temp > 5f)
                    {
                        zone.pipeFreezeProgress = Math.Max(0f, zone.pipeFreezeProgress - 0.2f);
                    }

                    if (zone.pipeFreezeProgress >= PipeFreezeBurstThreshold && !room.isFrozen)
                    {
                        zone.pipeFreezeProgress = 0f;
                        zone.lastFreezeDay = day;
                        OnPipeFrozen?.Invoke(room.roomId, day);
                    }
                }
            }

            // 6. Frost heave (simplified stress contribution)
            if (isExtremeCold)
            {
                foreach (var zone in _state.zones)
                {
                    float stress = outdoorTemp * 0.01f * (1f - GetInstalledThermalRetention(zone.zoneId));
                    if (stress > 0.01f)
                        OnFrostHeaveDetected?.Invoke(zone.zoneId, stress);
                }
            }
        }

        // ── Persistence ──────────────────────────────────────────────

        public WeatherHardeningState CaptureState() => CloneState(_state);

        public void RestoreState(WeatherHardeningState saved)
        {
            if (saved == null) return;
            _state = CloneState(saved);
            // Do not fire events on restore
        }

        private static WeatherHardeningState CloneState(WeatherHardeningState src)
        {
            if (src == null) return new WeatherHardeningState();
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(src);
            return s.Deserialize<WeatherHardeningState>(json) ?? new WeatherHardeningState();
        }

        private ZoneHardeningState GetOrCreateZone(string zoneId)
        {
            var zone = _state.zones.Find(z => z.zoneId == zoneId);
            if (zone == null)
            {
                zone = new ZoneHardeningState { zoneId = zoneId };
                _state.zones.Add(zone);
            }
            return zone;
        }
    }
}
