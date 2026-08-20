using System;
using System.Collections.Generic;
using Ashfall.Core.Shelter;

namespace Ashfall.Core
{
    [Serializable]
    public sealed class OrbitalTelemetryState
    {
        public string systemId = OrbitalHarrowTelemetrySystem.SystemId;
        public bool telemetryActive;
        public int lastImpactDay = -1;
        public int nextImpactDay = -1;
        public int warningLeadDays = 3;
        public int targetGridX = -1;
        public float impactEnergyMj = 10f;
        public bool isBraced;
        public bool braceUsed;
        public List<int> impactHistory = new List<int>();
        public List<OrbitalWarningEntry> warnings = new List<OrbitalWarningEntry>();
    }

    [Serializable]
    public sealed class OrbitalWarningEntry
    {
        public int day;
        public int targetGridX;
        public float energyMj;
        public string telemetryText = string.Empty;
    }

    public sealed class OrbitalHarrowTelemetrySystem
    {
        public const string SystemId = "orbital_harrow_telemetry";

        private OrbitalTelemetryState _state = new OrbitalTelemetryState();
        private readonly SkyLayerArmorSystem _armor;
        private readonly ISeededRng _rng;
        private readonly ILog _log;
        private int _currentDay;

        public OrbitalTelemetryState State => _state;
        public bool HasPendingImpact => _state.nextImpactDay > _currentDay;
        public event Action<OrbitalWarningEntry> OnImpactWarning;
        public event Action<int, float> OnImpactResolved; // day, energy
        public event Action OnTelemetryChanged;

        public OrbitalHarrowTelemetrySystem(SkyLayerArmorSystem armor, ISeededRng rng, ILog log = null)
        {
            _armor = armor ?? throw new ArgumentNullException(nameof(armor));
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _log = log ?? NullLog.Instance;
        }

        public void ActivateTelemetry(int day)
        {
            _state.telemetryActive = true;
            _log.Info($"[OrbitalHarrow] telemetry activated on day {day}");
            OnTelemetryChanged?.Invoke();
        }

        public void ScheduleImpact(int day, int gridX, float energyMj)
        {
            _state.nextImpactDay = day;
            _state.targetGridX = gridX;
            _state.impactEnergyMj = energyMj;
            _state.isBraced = false;
            _state.braceUsed = false;

            var warning = new OrbitalWarningEntry
            {
                day = day,
                targetGridX = gridX,
                energyMj = energyMj,
                telemetryText = $"IMPACT WARNING: Day {day}, Grid {gridX}, Energy {energyMj:F1} MJ"
            };
            _state.warnings.Add(warning);
            OnImpactWarning?.Invoke(warning);
            OnTelemetryChanged?.Invoke();
        }

        public ActionResult Brace(string materialId, int amount)
        {
            if (!HasPendingImpact)
                return ActionResult.Blocked("no_impact", "orbital.no_impact");
            if (_state.braceUsed)
                return ActionResult.Blocked("already_braced", "orbital.already_braced");

            _state.isBraced = true;
            _state.braceUsed = true;
            _log.Info($"[OrbitalHarrow] braced with {amount}x {materialId}");
            OnTelemetryChanged?.Invoke();
            return ActionResult.Success("orbital.braced",
                new Dictionary<string, double> { { "mitigation", _state.isBraced ? 0.5 : 0.0 } });
        }

        public void TickDay(int day)
        {
            _currentDay = day;
            if (_state.nextImpactDay == day)
            {
                ResolveImpact();
            }
        }

        private void ResolveImpact()
        {
            float energy = _state.isBraced ? _state.impactEnergyMj * 0.5f : _state.impactEnergyMj;
            bool breached = _armor.EvaluateKineticImpact(_state.targetGridX, energy, out float damage);

            _state.impactHistory.Add(_state.nextImpactDay);
            _state.lastImpactDay = _state.nextImpactDay;
            _state.nextImpactDay = -1;
            _state.isBraced = false;

            _log.Info($"[OrbitalHarrow] impact resolved: breached={breached}, damage={damage:F1}");
            OnImpactResolved?.Invoke(_state.lastImpactDay, energy);
            OnTelemetryChanged?.Invoke();
        }

        public OrbitalTelemetryState CaptureState() => _state;
        public void RestoreState(OrbitalTelemetryState saved)
        {
            if (saved == null) return;
            _state = saved;
            OnTelemetryChanged?.Invoke();
        }
    }
}
