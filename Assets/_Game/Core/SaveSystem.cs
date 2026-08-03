using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Serializes/deserializes every save-safe system snapshot to disk (JSON)
    /// under a named slot. Coordinates with each system's serializable state so
    /// a load fully reconstructs a prior session.
    ///
    /// Forward-compatible: SaveData carries a saveVersion int and a migration
    /// stub (MigrateV1toV2) even before V2 exists.
    ///
    /// Corrupt-save detection: each file stores a SHA-256 checksum computed over
    /// the JSON body (with an empty checksum placeholder). A mismatch on load
    /// triggers a graceful fallback message instead of crashing.
    /// </summary>
    public class SaveSystem
    {
        public const int CurrentSaveVersion = 1;

        private readonly GameState _gameState;
        private readonly WeatherSystem _weatherSystem;
        private readonly TemperatureSystem _temperatureSystem;
        private readonly NeedsSystem _needsSystem;
        private readonly RadiationSystem _radiationSystem;
        private readonly Shelter.Shelter _shelter;
        private readonly Func<IReadOnlyList<Survivor>> _getSurvivors;
        private readonly Func<string, ItemDefinition> _itemLookup;
        private readonly Func<string, ShelterModule> _moduleLookup;
        private readonly string _savesDir;

        // Optional — injected after construction to keep constructor signature stable
        private PhotoperiodSystem _photoPeriodSystem;
        private RadiationKnowledgeMap _knowledgeMap;
        private Inventory.Inventory _inventory;

        private readonly Dictionary<string, bool> _worldFlags = new Dictionary<string, bool>();

        public SaveSystem(
            GameState gameState,
            WeatherSystem weatherSystem,
            TemperatureSystem temperatureSystem,
            NeedsSystem needsSystem,
            RadiationSystem radiationSystem,
            Shelter.Shelter shelter,
            Func<IReadOnlyList<Survivor>> getSurvivors,
            Func<string, ItemDefinition> itemLookup,
            Func<string, ShelterModule> moduleLookup,
            string savesDir = null)
        {
            _gameState = gameState ?? throw new ArgumentNullException(nameof(gameState));
            _weatherSystem = weatherSystem;
            _temperatureSystem = temperatureSystem;
            _needsSystem = needsSystem;
            _radiationSystem = radiationSystem;
            _shelter = shelter;
            _getSurvivors = getSurvivors;
            _itemLookup = itemLookup;
            _moduleLookup = moduleLookup;
            _savesDir = savesDir ?? Path.Combine(Application.persistentDataPath, "saves");

            if (_gameState != null)
            {
                _gameState.OnPhaseChanged += OnPhaseChanged;
            }
        }

        /// <summary>Inject a PhotoperiodSystem after construction (optional; safe to skip in tests).</summary>
        public void SetPhotoPeriodSystem(PhotoperiodSystem photoPeriodSystem)
        {
            _photoPeriodSystem = photoPeriodSystem;
        }

        /// <summary>Inject radiation fog-of-war map (optional; safe to skip in tests).</summary>
        public void SetKnowledgeMap(RadiationKnowledgeMap knowledgeMap)
        {
            _knowledgeMap = knowledgeMap;
        }

        /// <summary>Inject inventory so device battery/calibration/broken persist across save/load.</summary>
        public void SetInventory(Inventory.Inventory inventory)
        {
            _inventory = inventory;
        }

        /// <summary>Write the current world state to the given slot.</summary>
        public bool Save(string slotId)
        {
            try
            {
                var snapshot = CaptureSnapshot();
                snapshot.GameState.Phase = _gameState.Phase;
                snapshot.GameState.Day = _gameState.Day;
                snapshot.GameState.IsPaused = _gameState.IsPaused;

                snapshot.Checksum = "";
                string body = JsonUtility.ToJson(snapshot, true);
                snapshot.Checksum = ComputeChecksum(body);
                string finalJson = JsonUtility.ToJson(snapshot, true);

                Directory.CreateDirectory(_savesDir);
                File.WriteAllText(SlotPath(slotId), finalJson);
                Debug.Log($"[SaveSystem] Saved to slot '{slotId}'.");
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[SaveSystem] Save to '{slotId}' failed: {ex.Message}");
                return false;
            }
        }

        /// <summary>Replace the current world state from the given slot.</summary>
        public bool Load(string slotId)
        {
            string path = SlotPath(slotId);
            if (!File.Exists(path))
            {
                Debug.LogWarning($"[SaveSystem] Slot '{slotId}' not found.");
                return false;
            }

            try
            {
                string json = File.ReadAllText(path);
                var data = JsonUtility.FromJson<SaveData>(json);
                if (data == null)
                {
                    Debug.LogError($"[SaveSystem] Slot '{slotId}' is not valid JSON.");
                    return false;
                }

                if (!VerifyChecksum(data, json))
                {
                    Debug.LogError($"[SaveSystem] Slot '{slotId}' is corrupt (checksum mismatch). Load aborted.");
                    return false;
                }

                if (data.SaveVersion < CurrentSaveVersion)
                {
                    Migrate(data);
                }

                RestoreFromSnapshot(data);
                Debug.Log($"[SaveSystem] Loaded slot '{slotId}' (version {data.SaveVersion}).");
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[SaveSystem] Load from '{slotId}' failed: {ex.Message}");
                return false;
            }
        }

        /// <summary>Whether a save exists for the given slot.</summary>
        public bool SlotExists(string slotId) => File.Exists(SlotPath(slotId));

        /// <summary>Delete a save slot.</summary>
        public bool Delete(string slotId)
        {
            string path = SlotPath(slotId);
            if (!File.Exists(path)) return false;
            File.Delete(path);
            return true;
        }

        /// <summary>All slot ids that have save files.</summary>
        public string[] ListSlots()
        {
            if (!Directory.Exists(_savesDir)) return Array.Empty<string>();
            return Directory.GetFiles(_savesDir, "save_*.json")
                .Select(f => Path.GetFileNameWithoutExtension(f).Substring("save_".Length))
                .ToArray();
        }

        /// <summary>Auto-save to the "autosave" slot.</summary>
        public void AutoSave() => Save("autosave");

        /// <summary>Set or clear a world flag (persisted across saves).</summary>
        public void SetWorldFlag(string key, bool value) => _worldFlags[key] = value;

        /// <summary>Get a world flag value (false if not set).</summary>
        public bool GetWorldFlag(string key) => _worldFlags.TryGetValue(key, out var v) && v;

        /// <summary>Read-only snapshot of all world flags.</summary>
        public IReadOnlyDictionary<string, bool> WorldFlags => _worldFlags;

        // -----------------------------------------------------------------
        // Checksum
        // -----------------------------------------------------------------

        private static string ComputeChecksum(string json)
        {
            using var sha = SHA256.Create();
            byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(json));
            var sb = new StringBuilder(hash.Length * 2);
            foreach (byte b in hash) sb.Append(b.ToString("x2"));
            return sb.ToString();
        }

        private static bool VerifyChecksum(SaveData data, string rawJson)
        {
            string saved = data.Checksum;
            data.Checksum = "";
            string body = JsonUtility.ToJson(data, true);
            string computed = ComputeChecksum(body);
            data.Checksum = saved;
            return string.Equals(computed, saved, StringComparison.Ordinal);
        }

        // -----------------------------------------------------------------
        // Migration stubs
        // -----------------------------------------------------------------

        private static void Migrate(SaveData data)
        {
            if (data.SaveVersion < 2) MigrateV1toV2(data);
            // if (data.SaveVersion < 3) MigrateV2toV3(data);
        }

        /// <summary>V1 -> V2 migration stub. Extend when V2 schema is defined.</summary>
        private static void MigrateV1toV2(SaveData data)
        {
            // TODO: transform data from V1 schema to V2 schema
            data.SaveVersion = 2;
        }

        // -----------------------------------------------------------------
        // Capture snapshot
        // -----------------------------------------------------------------

        private SaveData CaptureSnapshot()
        {
            var data = new SaveData
            {
                SaveVersion = CurrentSaveVersion,
                GameState = new GameStateSave
                {
                    Phase = _gameState.Phase,
                    Day = _gameState.Day,
                    IsPaused = _gameState.IsPaused
                }
            };

            if (_weatherSystem != null)
                data.Weather = _weatherSystem.GetState();

            if (_temperatureSystem != null)
                data.ElapsedHours = _temperatureSystem.TotalElapsedHours;

            if (_getSurvivors != null)
            {
                foreach (var sv in _getSurvivors())
                {
                    data.Survivors.Add(CaptureSurvivor(sv));
                }
            }

            if (_shelter != null)
            {
                foreach (var mod in _shelter.Modules)
                {
                    data.ShelterModules.Add(new ShelterModuleSave
                    {
                        ModuleId = mod.ModuleId,
                        Level = mod.Level,
                        IsEnabled = mod.IsEnabled,
                        FilterHealth = mod.FilterHealth,
                        Fuel = mod.Fuel,
                        WaterConversionProgress = mod.WaterConversionProgress
                    });
                }
            }

            if (_worldFlags.Count > 0)
            {
                foreach (var kv in _worldFlags)
                {
                    data.WorldFlagKeys.Add(kv.Key);
                    data.WorldFlagValues.Add(kv.Value);
                }
            }

            if (_photoPeriodSystem != null)
                data.Photoperiod = _photoPeriodSystem.GetState();

            if (_knowledgeMap != null)
                data.RadiationKnowledge = _knowledgeMap.CaptureState();

            if (_inventory != null)
                data.Inventory = _inventory.CaptureState();

            return data;
        }

        private SurvivorSave CaptureSurvivor(Survivor sv)
        {
            var save = new SurvivorSave
            {
                Id = sv.Id,
                DisplayName = sv.DisplayName,
                State = sv.State,

                Hunger = sv.Needs.Hunger,
                Thirst = sv.Needs.Thirst,
                Fatigue = sv.Needs.Fatigue,
                Warmth = sv.Needs.Warmth,
                Morale = sv.Needs.Morale,
                Health = sv.Needs.Health,
                WasHungerCritical = sv.Needs.WasHungerCritical,
                WasThirstCritical = sv.Needs.WasThirstCritical,
                WasWarmthCritical = sv.Needs.WasWarmthCritical,

                RadiationDose = sv.RadiationDose,
                LifetimeRadiationExposure = sv.LifetimeRadiationExposure,
                HasAcuteRadiationSickness = sv.HasAcuteRadiationSickness,
                HasChronicIllness = sv.HasChronicIllness,
                HasRadResistance = sv.HasRadResistance,
                RadResistanceHoursRemaining = sv.RadResistanceHoursRemaining,
                HasFullSuitEquipped = sv.HasFullSuitEquipped,

                // Latent damage / prognosis pipeline
                AcuteDoseWindow = sv.AcuteDoseWindow,
                PrognosisStage = sv.PrognosisStage,
                OnsetTimer = sv.OnsetTimer,
                LatentDamage = sv.LatentDamage,
                IodineProtectionTimer = sv.IodineProtectionTimer,
                HasAcuteRadiationSyndrome = sv.HasAcuteRadiationSyndrome,

                // Light / photoperiod
                LightExposure = sv.LightExposure,
                VitaminDProxy = sv.VitaminDProxy,
                IsListless    = sv.IsListless
            };

            if (_radiationSystem != null && !string.IsNullOrEmpty(sv.Id))
            {
                var dos = _radiationSystem.GetDosimeter(sv.Id);
                if (dos != null)
                {
                    save.DosimeterRate = dos.CurrentRate;
                    save.DosimeterRecent = dos.RecentExposure;
                }
            }

            return save;
        }

        // -----------------------------------------------------------------
        // Restore from snapshot
        // -----------------------------------------------------------------

        private void RestoreFromSnapshot(SaveData data)
        {
            _gameState.Day = data.GameState.Day;
            _gameState.Phase = data.GameState.Phase;
            _gameState.IsPaused = data.GameState.IsPaused;

            if (_weatherSystem != null && data.Weather != null)
            {
                _weatherSystem.RestoreState(data.Weather);
            }

            if (_temperatureSystem != null)
            {
                _temperatureSystem.SetElapsedHours(data.ElapsedHours);
            }

            // Survivors
            var existing = _getSurvivors?.Invoke();
            if (existing != null && data.Survivors != null)
            {
                for (int i = 0; i < data.Survivors.Count; i++)
                {
                    Survivor sv = i < existing.Count ? existing[i] : null;
                    if (sv == null) continue;
                    RestoreSurvivor(sv, data.Survivors[i]);
                }
            }

            // Shelter modules
            if (_shelter != null && data.ShelterModules != null)
            {
                RestoreShelterModules(data.ShelterModules);
            }

            // Dosimeters
            if (_radiationSystem != null && data.Survivors != null)
            {
                RestoreDosimeters(data.Survivors);
            }

            // World flags
            _worldFlags.Clear();
            if (data.WorldFlagKeys != null && data.WorldFlagValues != null)
            {
                int count = Mathf.Min(data.WorldFlagKeys.Count, data.WorldFlagValues.Count);
                for (int i = 0; i < count; i++)
                {
                    _worldFlags[data.WorldFlagKeys[i]] = data.WorldFlagValues[i];
                }
            }

            if (_photoPeriodSystem != null && data.Photoperiod != null)
            {
                _photoPeriodSystem.RestoreState(data.Photoperiod);
            }

            if (_knowledgeMap != null && data.RadiationKnowledge != null)
            {
                _knowledgeMap.RestoreState(data.RadiationKnowledge);
            }

            if (_inventory != null && data.Inventory != null && _itemLookup != null)
            {
                _inventory.RestoreState(data.Inventory, _itemLookup);
            }
        }

        private void RestoreSurvivor(Survivor sv, SurvivorSave save)
        {
            sv.Id = save.Id;
            sv.DisplayName = save.DisplayName;
            sv.State = save.State;

            sv.Needs.Hunger = save.Hunger;
            sv.Needs.Thirst = save.Thirst;
            sv.Needs.Fatigue = save.Fatigue;
            sv.Needs.Warmth = save.Warmth;
            sv.Needs.Morale = save.Morale;
            sv.Needs.Health = save.Health;
            sv.Needs.WasHungerCritical = save.WasHungerCritical;
            sv.Needs.WasThirstCritical = save.WasThirstCritical;
            sv.Needs.WasWarmthCritical = save.WasWarmthCritical;

            sv.RadiationDose = save.RadiationDose;
            sv.LifetimeRadiationExposure = save.LifetimeRadiationExposure;
            sv.HasAcuteRadiationSickness = save.HasAcuteRadiationSickness;
            sv.HasChronicIllness = save.HasChronicIllness;
            sv.HasRadResistance = save.HasRadResistance;
            sv.RadResistanceHoursRemaining = save.RadResistanceHoursRemaining;
            sv.HasFullSuitEquipped = save.HasFullSuitEquipped;

            // Latent damage / prognosis pipeline
            sv.AcuteDoseWindow = save.AcuteDoseWindow;
            sv.PrognosisStage = save.PrognosisStage;
            sv.OnsetTimer = save.OnsetTimer;
            sv.LatentDamage = save.LatentDamage;
            sv.IodineProtectionTimer = save.IodineProtectionTimer;
            sv.HasAcuteRadiationSyndrome = save.HasAcuteRadiationSyndrome;

            // Light / photoperiod
            sv.LightExposure = save.LightExposure;
            sv.VitaminDProxy = save.VitaminDProxy;
            sv.IsListless    = save.IsListless;
        }

        private void RestoreShelterModules(List<ShelterModuleSave> saved)
        {
            foreach (var modSave in saved)
            {
                var instance = _shelter.GetModule(modSave.ModuleId);
                if (instance == null)
                {
                    instance = new ShelterModuleInstance(modSave.ModuleId);
                    _shelter.AddModule(instance);
                }

                instance.Level = modSave.Level;
                instance.IsEnabled = modSave.IsEnabled;
                instance.FilterHealth = modSave.FilterHealth;
                instance.Fuel = modSave.Fuel;
                instance.WaterConversionProgress = modSave.WaterConversionProgress;

                if (_moduleLookup != null)
                {
                    instance.Definition = _moduleLookup(modSave.ModuleId);
                }
            }
        }

        private void RestoreDosimeters(List<SurvivorSave> survivors)
        {
            foreach (var svSave in survivors)
            {
                if (string.IsNullOrEmpty(svSave.Id)) continue;
                var dos = _radiationSystem.GetDosimeter(svSave.Id);
                dos.CurrentRate = svSave.DosimeterRate;
                dos.RecentExposure = svSave.DosimeterRecent;
                dos.LifetimeDose = svSave.LifetimeRadiationExposure;
            }
        }

        // -----------------------------------------------------------------
        // Autosave on phase change
        // -----------------------------------------------------------------

        private void OnPhaseChanged(GamePhase phase)
        {
            if (phase == GamePhase.Running)
            {
                AutoSave();
            }
        }

        // -----------------------------------------------------------------
        // Path helpers
        // -----------------------------------------------------------------

        private string SlotPath(string slotId) => Path.Combine(_savesDir, $"save_{slotId}.json");
    }

    // =====================================================================
    // Save data container
    // =====================================================================

    [Serializable]
    public class SaveData
    {
        public int SaveVersion = SaveSystem.CurrentSaveVersion;
        public string Checksum = "";
        public GameStateSave GameState = new GameStateSave();
        public WeatherState Weather;
        public float ElapsedHours;
        public PhotoperiodState Photoperiod;
        public List<SurvivorSave> Survivors = new List<SurvivorSave>();
        public List<ShelterModuleSave> ShelterModules = new List<ShelterModuleSave>();
        public List<string> WorldFlagKeys = new List<string>();
        public List<bool> WorldFlagValues = new List<bool>();
        public RadiationKnowledgeSave RadiationKnowledge;
        public InventorySaveState Inventory;
    }

    // =====================================================================
    // Sub-snapshots
    // =====================================================================

    /// <summary>DTO for GameState (auto-properties are invisible to JsonUtility).</summary>
    [Serializable]
    public class GameStateSave
    {
        public GamePhase Phase;
        public int Day;
        public bool IsPaused;
    }

    /// <summary>Full survivor snapshot including needs, radiation, and dosimeter.</summary>
    [Serializable]
    public class SurvivorSave
    {
        public string Id;
        public string DisplayName;
        public SurvivorState State;

        // Needs (plain fields for JsonUtility)
        public float Hunger;
        public float Thirst;
        public float Fatigue;
        public float Warmth = 100f;
        public float Morale = 75f;
        public float Health = 100f;
        public bool WasHungerCritical;
        public bool WasThirstCritical;
        public bool WasWarmthCritical;

        // Radiation
        public float RadiationDose;
        public float LifetimeRadiationExposure;
        public bool HasAcuteRadiationSickness;
        public bool HasChronicIllness;
        public bool HasRadResistance;
        public float RadResistanceHoursRemaining;
        public bool HasFullSuitEquipped;

        // Latent damage / prognosis pipeline
        public float AcuteDoseWindow;
        public PrognosisStage PrognosisStage;
        public float OnsetTimer;
        public float LatentDamage;
        public float IodineProtectionTimer;
        public bool HasAcuteRadiationSyndrome;

        // Dosimeter
        public float DosimeterRate;
        public float DosimeterRecent;

        // Light / photoperiod
        public float LightExposure = 100f;
        public float VitaminDProxy = 100f;
        public bool  IsListless;
    }

    /// <summary>Shelter module runtime state snapshot.</summary>
    [Serializable]
    public class ShelterModuleSave
    {
        public string ModuleId;
        public int Level = 1;
        public bool IsEnabled = true;
        public float FilterHealth = 100f;
        public float Fuel;
        public float WaterConversionProgress;
    }
}
