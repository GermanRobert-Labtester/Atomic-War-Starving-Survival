using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core
{
    [Serializable]
    public sealed class LocationEvolutionSaveState
    {
        public int schema_version = 1;
        public string systemId = LocationEvolutionSystem.SystemId;
        public int lastEvolutionDay = -1;
        public List<LocationMutationRecord> mutations = new List<LocationMutationRecord>();
    }

    [Serializable]
    public sealed class LocationMutationRecord
    {
        public string locationId = string.Empty;
        public string currentOwner = "none";
        public float contaminationLevel;
        public float lootDepletionFactor;
        public bool isCleared;
        public bool isRuined;
        public int lastVisitedDay = -1;
        public List<string> activeThreats = new List<string>();
        public List<string> discoveredCaches = new List<string>();
    }

    public sealed partial class LocationEvolutionSystem
    {
        public const string SystemId = "location_evolution";
        private LocationEvolutionSaveState _state = new LocationEvolutionSaveState();
        private readonly ISeededRng _rng;
        private readonly ILog _log;

        public LocationEvolutionSaveState State => _state;
        public event Action<string, string> OnLocationOwnerChanged;
        public event Action<string> OnLocationMutated;

        public LocationEvolutionSystem(ISeededRng? rng = null, ILog? log = null)
        {
            _rng = rng ?? new SeededRng(42);
            _log = log ?? NullLog.Instance;
        }

        public LocationMutationRecord? GetOrCreateRecord(string locationId)
        {
            if (string.IsNullOrEmpty(locationId)) return null;
            var record = _state.mutations.Find(m => string.Equals(m.locationId, locationId, StringComparison.Ordinal));
            if (record == null)
            {
                record = new LocationMutationRecord { locationId = locationId };
                _state.mutations.Add(record);
            }
            return record;
        }

        public ActionResult SetLocationOwner(string locationId, string newOwner)
        {
            var record = GetOrCreateRecord(locationId);
            if (record == null) return ActionResult.Failed("invalid_location", "location.invalid");

            record.currentOwner = newOwner ?? "none";
            OnLocationOwnerChanged?.Invoke(locationId, record.currentOwner);
            return ActionResult.Success("location.owner_updated");
        }

        public ActionResult MarkCleared(string locationId, int day)
        {
            var record = GetOrCreateRecord(locationId);
            if (record == null) return ActionResult.Failed("invalid_location", "location.invalid");

            record.isCleared = true;
            record.lastVisitedDay = day;
            record.currentOwner = "none";
            record.lootDepletionFactor = Math.Min(1f, record.lootDepletionFactor + 0.3f);
            OnLocationMutated?.Invoke(locationId);
            return ActionResult.Success("location.cleared");
        }

        public void TickDay(int day)
        {
            _state.lastEvolutionDay = day;
            foreach (var rec in _state.mutations)
            {
                // Dynamic recovery or degradation over time
                if (rec.isCleared && day - rec.lastVisitedDay > 20)
                {
                    rec.lootDepletionFactor = Math.Max(0f, rec.lootDepletionFactor - 0.05f);
                }
            }
        }

        public LocationEvolutionSaveState CaptureState() => CloneState(_state);

        public void RestoreState(LocationEvolutionSaveState saved)
        {
            if (saved == null) return;
            _state = CloneState(saved);
        }

        private static LocationEvolutionSaveState CloneState(LocationEvolutionSaveState src)
        {
            if (src == null) return new LocationEvolutionSaveState();
            var clone = new LocationEvolutionSaveState
            {
                schema_version = src.schema_version,
                systemId = src.systemId,
                lastEvolutionDay = src.lastEvolutionDay,
                mutations = new List<LocationMutationRecord>(src.mutations.Count)
            };
            for (int i = 0; i < src.mutations.Count; i++)
            {
                var m = src.mutations[i];
                if (m == null) continue;
                clone.mutations.Add(new LocationMutationRecord
                {
                    locationId = m.locationId,
                    currentOwner = m.currentOwner,
                    contaminationLevel = m.contaminationLevel,
                    lootDepletionFactor = m.lootDepletionFactor,
                    isCleared = m.isCleared,
                    isRuined = m.isRuined,
                    lastVisitedDay = m.lastVisitedDay,
                    activeThreats = new List<string>(m.activeThreats ?? new List<string>()),
                    discoveredCaches = new List<string>(m.discoveredCaches ?? new List<string>())
                });
            }
            return clone;
        }
    }
}
