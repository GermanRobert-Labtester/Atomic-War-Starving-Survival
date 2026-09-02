using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Ashfall.Core.Disease;
using Ashfall.Core.Inventory;

namespace Ashfall.Core.Survivors
{
    [Serializable]
    public sealed class DesperationEventDef
    {
        public string desperation_id { get; set; } = string.Empty;
        public string event_id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public float required_starvation { get; set; } = 90.0f;
        public List<string> required_conditions { get; set; } = new List<string>();
        public List<string> forbidden_conditions { get; set; } = new List<string>();
        public string resource_yield_item_id { get; set; } = "raw_meat";
        public int resource_yield_count { get; set; } = 6;
        public string taboo_level { get; set; } = "Broken";
        public float morale_shock { get; set; } = 35.0f;
        public float prion_risk { get; set; } = 0.15f;
        public float mutiny_pressure { get; set; } = 25.0f;
        public string trait_granted { get; set; } = "trait_cannibal";
        public string trait_awarded { get; set; } = "trait_cannibal";

        public string Id => !string.IsNullOrEmpty(desperation_id) ? desperation_id : event_id;
    }

    [Serializable]
    public sealed class DesperationCatalogContainer
    {
        public int schema_version { get; set; } = 1;
        public List<DesperationEventDef> events { get; set; } = new List<DesperationEventDef>();
    }

    [Serializable]
    public sealed class DesperationActRecord
    {
        public string actId { get; set; } = string.Empty;
        public string eventId { get; set; } = string.Empty;
        public string actorId { get; set; } = string.Empty;
        public string corpseId { get; set; } = string.Empty;
        public int day { get; set; }
        public string tabooLevel { get; set; } = "Broken";
        public int meatYield { get; set; }
        public bool prionContracted { get; set; }
    }

    [Serializable]
    public sealed class DesperationState
    {
        public string systemId = DesperationSystem.SystemId;
        public float mutinyPressure;
        public List<string> harvestedCorpseIds = new List<string>();
        public List<string> cannibalSurvivorIds = new List<string>();
        public List<DesperationActRecord> actsHistory = new List<DesperationActRecord>();
        public List<string> unburiedCorpseIds = new List<string>();
        public List<string> oneShotShockIds = new List<string>();
    }

    public sealed class DesperationSystem
    {
        public const string SystemId = "desperation";
        public const float CrisisStarvationThreshold = 90.0f;

        private DesperationState _state = new DesperationState();
        private readonly Dictionary<string, DesperationEventDef> _catalog = new Dictionary<string, DesperationEventDef>(StringComparer.Ordinal);
        private readonly ISeededRng _rng;
        private readonly Inventory.Inventory _inventory;
        private readonly NeedsSystem _needs;
        private readonly DiseaseSystem? _diseaseSystem;
        private readonly ILog _log;
        private int _actCounter;

        public DesperationState State => _state;
        public float MutinyPressure => _state.mutinyPressure;

        public event Action<DesperationActRecord>? OnTabooBroken;
        public event Action<float>? OnMutinyPressureChanged;

        public DesperationSystem(
            ISeededRng rng,
            Inventory.Inventory inventory,
            NeedsSystem needs,
            DiseaseSystem? diseaseSystem = null,
            ILog? log = null,
            string dataPath = "")
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _needs = needs ?? throw new ArgumentNullException(nameof(needs));
            _diseaseSystem = diseaseSystem;
            _log = log ?? NullLog.Instance;

            LoadCatalog(dataPath);
        }

        public void LoadCatalog(string dataPath)
        {
            string path = string.IsNullOrEmpty(dataPath)
                ? Path.Combine(AppContext.BaseDirectory, "Assets", "StreamingAssets", "Data", "desperation_events.json")
                : Path.Combine(dataPath, "desperation_events.json");

            if (!File.Exists(path))
            {
                RegisterEvent(new DesperationEventDef
                {
                    event_id = "desperation_consume_corpse",
                    display_name = "Harvest Fallen Dweller",
                    required_starvation = 90.0f,
                    resource_yield_item_id = "raw_meat",
                    resource_yield_count = 8,
                    taboo_level = "Broken",
                    morale_shock = 35.0f,
                    prion_risk = 0.15f,
                    mutiny_pressure = 25.0f,
                    trait_awarded = "trait_cannibal"
                });
                return;
            }

            try
            {
                string json = File.ReadAllText(path);
                var container = JsonSerializer.Deserialize<DesperationCatalogContainer>(json);
                if (container?.events != null)
                {
                    foreach (var ev in container.events)
                        RegisterEvent(ev);
                }
            }
            catch (Exception ex)
            {
                _log.Warn($"[DesperationSystem] Failed to load catalog from {path}: {ex.Message}");
            }
        }

        public void RegisterEvent(DesperationEventDef def)
        {
            if (def == null || string.IsNullOrEmpty(def.Id)) return;
            _catalog[def.Id] = def;
        }

        public void RegisterCorpse(string corpseId)
        {
            if (string.IsNullOrEmpty(corpseId)) return;
            if (!_state.unburiedCorpseIds.Contains(corpseId) && !_state.harvestedCorpseIds.Contains(corpseId))
            {
                _state.unburiedCorpseIds.Add(corpseId);
            }
        }

        public bool IsActionEligible(string survivorId, string eventId, string corpseId)
        {
            var survivor = _needs.Get(survivorId);
            if (survivor == null || !survivor.IsAliveState) return false;

            if (!_catalog.TryGetValue(eventId, out var def)) return false;

            // Strict Starvation Gate: actor or settlement must be in crisis
            if (survivor.Hunger < def.required_starvation) return false;

            // Target corpse validation
            if (string.IsNullOrEmpty(corpseId)) return false;
            if (_state.harvestedCorpseIds.Contains(corpseId)) return false;
            if (!_state.unburiedCorpseIds.Contains(corpseId)) return false;

            return true;
        }

        public ActionResult HarvestCorpse(string actorId, string corpseId, string eventId, int currentDay, bool hasCynicalTrait = false)
        {
            if (!IsActionEligible(actorId, eventId, corpseId))
            {
                return ActionResult.Blocked("crisis_not_reached", "desperation.crisis_not_reached");
            }

            _catalog.TryGetValue(eventId, out var def);
            int yieldCount = def?.resource_yield_count ?? 6;
            string yieldItem = def?.resource_yield_item_id ?? "raw_meat";
            float shock = def?.morale_shock ?? 35.0f;
            float pRisk = def?.prion_risk ?? 0.15f;
            float mPressure = def?.mutiny_pressure ?? 25.0f;

            // 1. Mark corpse harvested
            _state.unburiedCorpseIds.Remove(corpseId);
            _state.harvestedCorpseIds.Add(corpseId);

            // 2. Grant meat resource yield atomically
            _inventory.AddById(yieldItem, yieldCount);

            // 3. Mark actor as cannibal
            if (!_state.cannibalSurvivorIds.Contains(actorId))
            {
                _state.cannibalSurvivorIds.Add(actorId);
            }

            // 4. Resolve deterministic prion disease risk
            bool prionInfected = false;
            if (_rng.NextDouble() < pRisk && _diseaseSystem != null)
            {
                prionInfected = true;
                _diseaseSystem.Infect(actorId, "disease_prion_tremor", currentDay);
            }

            // 5. Apply collective morale shock across non-participating survivors
            string shockKey = $"shock_{++_actCounter}_{corpseId}";
            if (!_state.oneShotShockIds.Contains(shockKey))
            {
                _state.oneShotShockIds.Add(shockKey);
                for (int i = 0; i < _needs.Registered.Count; i++)
                {
                    var s = _needs.Registered[i];
                    if (s == null || !s.IsAliveState) continue;

                    if (s.Id == actorId)
                    {
                        // Actor suffers direct guilt, offset slightly by cynical trait if present
                        float actorGuilt = hasCynicalTrait ? (shock * 0.5f) : (shock * 0.8f);
                        _needs.Modify(s, NeedKind.Morale, -actorGuilt);
                    }
                    else
                    {
                        // Innocent dwellers suffer full broken taboo shock
                        _needs.Modify(s, NeedKind.Morale, -shock);
                    }
                }
            }

            // 6. Mutiny pressure increases
            _state.mutinyPressure += mPressure;
            OnMutinyPressureChanged?.Invoke(_state.mutinyPressure);

            // 7. Record act
            var record = new DesperationActRecord
            {
                actId = $"act_{_actCounter}_{currentDay}",
                eventId = eventId,
                actorId = actorId,
                corpseId = corpseId,
                day = currentDay,
                tabooLevel = def?.taboo_level ?? "Broken",
                meatYield = yieldCount,
                prionContracted = prionInfected
            };

            _state.actsHistory.Add(record);
            OnTabooBroken?.Invoke(record);

            return ActionResult.Success("desperation.corpse_harvested");
        }

        public void RestoreState(DesperationState state)
        {
            if (state == null) return;
            _state = state;
            _actCounter = _state.actsHistory.Count;
        }
    }
}
