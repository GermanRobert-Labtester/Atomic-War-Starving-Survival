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
using AtomicWar._Game.Medical;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Events;

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
        public const int CurrentSaveVersion = 2;

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
        private ExpeditionSystem _expeditionSystem;
        private MedicalSystem _medicalSystem;
        private DynamicEconomySystem _economySystem;
        private WorldPhaseSystem _worldPhaseSystem;
        private PowerNetwork _powerNetwork;
        private WaterStorage _waterStorage;
        private GeneratedMap _generatedMap;
        private HatchDefenseSystem _hatchDefense;
        private FactionRadioInterceptSystem _factionRadioIntercepts;
        private AtomicWar._Game.Survivors.MentalBreakSystem _mentalBreakSystem;
        private JournalSystem _journalSystem;
        private VictoryProjectManager _victoryProject;
        private EventRunner _eventRunner;
        // Choreographer is injected as capture/restore delegates rather than a
        // direct reference so Core stays agnostic of the Flashpoint module.
        private Func<FlashpointChoreographerSave> _captureChoreographer;
        private Action<FlashpointChoreographerSave> _restoreChoreographer;
        /// <summary>Optional hook run just before CaptureSnapshot (e.g. HUD → system sync).</summary>
        private Action _preCaptureHook;

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

        /// <summary>Inject expedition system (optional; safe to skip in tests).</summary>
        public void SetExpeditionSystem(ExpeditionSystem expeditionSystem)
        {
            _expeditionSystem = expeditionSystem;
        }

        /// <summary>Inject medical triage pipeline so afflictions persist across save/load.</summary>
        public void SetMedicalSystem(MedicalSystem medicalSystem)
        {
            _medicalSystem = medicalSystem;
        }

        /// <summary>Inject world phase system so CurrentPhase/HasTriggeredExchange persist across save/load.</summary>
        public void SetWorldPhaseSystem(WorldPhaseSystem worldPhaseSystem)
        {
            _worldPhaseSystem = worldPhaseSystem;
        }

        /// <summary>Inject dynamic economy / faction trust matrix for save/load.</summary>
        public void SetEconomySystem(DynamicEconomySystem economySystem)
        {
            _economySystem = economySystem;
        }

        /// <summary>Inject shelter power grid for save/load.</summary>
        public void SetPowerNetwork(PowerNetwork powerNetwork)
        {
            _powerNetwork = powerNetwork;
        }

        /// <summary>Inject hatch defense / raid state for save/load.</summary>
        public void SetHatchDefense(HatchDefenseSystem hatchDefense)
        {
            _hatchDefense = hatchDefense;
        }

        /// <summary>Inject faction radio intercept log for save/load.</summary>
        public void SetFactionRadioIntercepts(FactionRadioInterceptSystem radioIntercepts)
        {
            _factionRadioIntercepts = radioIntercepts;
        }

        /// <summary>Inject diegetic journal / knowledge base for save/load.</summary>
        public void SetJournalSystem(JournalSystem journalSystem)
        {
            _journalSystem = journalSystem;
        }

        /// <summary>Inject campaign win/loss victory project for save/load.</summary>
        public void SetVictoryProjectManager(VictoryProjectManager victoryProject)
        {
            _victoryProject = victoryProject;
        }

        /// <summary>
        /// Inject EventRunner so the scheduled narrative-chain queue
        /// (Prompt #43) persists across save/load.
        /// </summary>
        public void SetEventRunner(EventRunner eventRunner)
        {
            _eventRunner = eventRunner;
        }

        /// <summary>Inject proc-gen wasteland map (reveal/visit flags + seed).</summary>
        public void SetGeneratedMap(GeneratedMap generatedMap)
        {
            _generatedMap = generatedMap;
        }

        /// <summary>Inject bunker water cisterns (clean/dirty/irradiated) for save/load.</summary>
        public void SetWaterStorage(WaterStorage waterStorage)
        {
            _waterStorage = waterStorage;
        }

        /// <summary>Inject mental-break system so affinity matrix persists across save/load.</summary>
        public void SetMentalBreakSystem(MentalBreakSystem mentalBreakSystem)
        {
            _mentalBreakSystem = mentalBreakSystem;
        }

        /// <summary>Inject Day-30 Flashpoint Choreographer adapter so the
        /// choreography checkpoint (buildup days processed, current step)
        /// persists across save/load. The Capture delegate returns the
        /// current state; the Restore delegate applies a loaded snapshot.
        /// Optional; safe to skip if no choreographer is wired.</summary>
        public void SetFlashpointChoreographer(
            Func<FlashpointChoreographerSave> capture,
            Action<FlashpointChoreographerSave> restore)
        {
            _captureChoreographer = capture;
            _restoreChoreographer = restore;
        }

        /// <summary>
        /// Hook invoked immediately before building a save snapshot (quicksave,
        /// autosave, named slots). Used to flush live HUD presentation into
        /// systems (radio open/unread/tuner).
        /// </summary>
        public void SetPreCaptureHook(Action preCapture)
        {
            _preCaptureHook = preCapture;
        }

        /// <summary>Write the current world state to the given slot.</summary>
        public bool Save(string slotId)
        {
            try
            {
                _preCaptureHook?.Invoke();
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

        /// <summary>V1 -> V2 migration: V1 saves lack the FlashpointChoreographer
        /// snapshot. Default values leave the choreographer in a fresh state
        /// (no buildup days processed, choreography not started). The
        /// WorldPhaseSystem.HasTriggeredExchange flag in the same save
        /// determines whether the choreography restarts on next load.</summary>
        private static void MigrateV1toV2(SaveData data)
        {
            data.FlashpointChoreographer = null;
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

            if (_medicalSystem != null)
                data.Medical = _medicalSystem.CaptureState();

            if (_worldPhaseSystem != null)
                data.WorldPhase = _worldPhaseSystem.CaptureState();

            if (_economySystem != null)
                data.Economy = _economySystem.CaptureState();

            if (_powerNetwork != null)
                data.Power = _powerNetwork.CaptureState();

            if (_hatchDefense != null)
                data.HatchDefense = _hatchDefense.CaptureState();

            if (_factionRadioIntercepts != null)
                data.FactionRadioIntercepts = _factionRadioIntercepts.CaptureState();

            if (_journalSystem != null)
                data.Journal = _journalSystem.CaptureState();

            if (_victoryProject != null)
                data.VictoryProject = _victoryProject.CaptureState();

            if (_eventRunner != null)
                data.ScheduledEvents = _eventRunner.CaptureScheduledState();

            if (_generatedMap != null)
                data.GeneratedMap = _generatedMap.CaptureState();

            if (_waterStorage != null)
                data.Water = _waterStorage.CaptureState();

            if (_mentalBreakSystem != null)
            {
                var affSave = new AffinityMatrixSave();
                affSave.Entries.AddRange(_mentalBreakSystem.Affinity.Snapshot());
                data.Affinity = affSave;
            }

            if (_captureChoreographer != null)
                data.FlashpointChoreographer = _captureChoreographer();

            if (_expeditionSystem != null && _expeditionSystem.ActiveExpeditions != null)
            {
                foreach (var exp in _expeditionSystem.ActiveExpeditions)
                {
                    if (exp == null) continue;
                    var saveExp = new ExpeditionSaveState
                    {
                        ExpeditionId = exp.ExpeditionId,
                        SurvivorId = exp.SurvivorId,
                        TargetLocationId = exp.TargetLocationId,
                        TargetLocationName = exp.TargetLocationName,
                        Stance = exp.Stance,
                        Phase = exp.Phase,
                        CurrentTick = exp.CurrentTick,
                        TotalDistanceTicks = exp.TotalDistanceTicks,
                        TravelTicksCompleted = exp.TravelTicksCompleted,
                        LootingTicksCompleted = exp.LootingTicksCompleted,
                        CarryingCapacity = exp.CarryingCapacity,
                        CurrentWeight = exp.CurrentWeight,
                        Stamina = exp.Stamina,
                        SuitDegradation = exp.SuitDegradation,
                        TrueRadPerHour = exp.TrueRadPerHour,
                        DangerLevel = exp.DangerLevel,
                        IsPushingLuck = exp.IsPushingLuck,
                        IsRetreating = exp.IsRetreating
                    };
                    if (exp.CollectedLootItemIds != null)
                    {
                        saveExp.CollectedLootItemIds.AddRange(exp.CollectedLootItemIds);
                    }
                    data.Expeditions.Add(saveExp);
                }
            }

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
                IsListless    = sv.IsListless,

                MedicalSkill  = sv.MedicalSkill,

                // Mental-break system (Prompt #29)
                CurrentMentalBreakId    = sv.currentMentalBreakId ?? string.Empty,
                LowMoraleHours          = sv.lowMoraleHours,
                MentalBreakCureProgress = sv.mentalBreakCureProgress
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

            if (_medicalSystem != null && data.Medical != null)
            {
                _medicalSystem.RestoreState(data.Medical);
            }

            if (_worldPhaseSystem != null && data.WorldPhase != null)
            {
                _worldPhaseSystem.RestoreState(data.WorldPhase);
            }

            if (_economySystem != null && data.Economy != null)
            {
                _economySystem.RestoreState(data.Economy);
            }

            if (_powerNetwork != null && data.Power != null)
            {
                _powerNetwork.RestoreState(data.Power);
                _powerNetwork.ApplyToShelter(_shelter);
            }

            if (_hatchDefense != null && data.HatchDefense != null)
            {
                _hatchDefense.RestoreState(data.HatchDefense);
            }

            if (_factionRadioIntercepts != null)
            {
                // Null snapshot (pre-feature saves) clears to empty log.
                _factionRadioIntercepts.RestoreState(data.FactionRadioIntercepts);
            }

            if (_journalSystem != null)
            {
                // Null journal on legacy saves resets empty (no re-fire of OnEntryAdded).
                _journalSystem.RestoreState(data.Journal);
            }

            if (_victoryProject != null)
            {
                // Null victory on legacy saves resets to Ongoing.
                _victoryProject.RestoreState(data.VictoryProject);
            }

            if (_eventRunner != null)
            {
                // Null queue on legacy saves clears scheduled narrative chains.
                _eventRunner.RestoreScheduledState(data.ScheduledEvents);
            }

            if (_generatedMap != null && data.GeneratedMap != null)
            {
                // Layout is pure seed; regenerate if seed differs, then re-apply fog flags.
                if (_generatedMap.Seed != data.GeneratedMap.Seed)
                {
                    var rebuilt = MapGenerator.Generate(data.GeneratedMap.Seed);
                    _generatedMap.Seed = rebuilt.Seed;
                    _generatedMap.Nodes = rebuilt.Nodes;
                    _generatedMap.Paths = rebuilt.Paths;
                }
                _generatedMap.RestoreRevealState(data.GeneratedMap);
            }

            if (_waterStorage != null && data.Water != null)
            {
                _waterStorage.RestoreState(data.Water);
            }

            if (_mentalBreakSystem != null && data.Affinity != null)
            {
                _mentalBreakSystem.Affinity.Restore(data.Affinity.Entries);
            }

            if (_restoreChoreographer != null)
            {
                // Choreographer restore is safe even if the snapshot is null
                // (first launch) — it resets the state machine to defaults.
                _restoreChoreographer(data.FlashpointChoreographer);
            }

            if (_expeditionSystem != null && data.Expeditions != null)
            {
                RestoreExpeditions(data.Expeditions);
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

            sv.MedicalSkill  = save.MedicalSkill;

            // Mental-break system (Prompt #29)
            sv.currentMentalBreakId    = save.CurrentMentalBreakId;
            sv.lowMoraleHours          = save.LowMoraleHours;
            sv.mentalBreakCureProgress = save.MentalBreakCureProgress;
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

        private void RestoreExpeditions(List<ExpeditionSaveState> expeditions)
        {
            if (_expeditionSystem == null || expeditions == null) return;

            var existingExpeditions = _expeditionSystem.ActiveExpeditions as List<ExpeditionState>;
            var survivors = _getSurvivors?.Invoke();

            foreach (var saveExp in expeditions)
            {
                if (saveExp == null || string.IsNullOrEmpty(saveExp.SurvivorId)) continue;

                Survivor survivor = null;
                if (survivors != null)
                {
                    for (int i = 0; i < survivors.Count; i++)
                    {
                        if (survivors[i]?.Id == saveExp.SurvivorId)
                        {
                            survivor = survivors[i];
                            break;
                        }
                    }
                }

                var state = _expeditionSystem.GetExpeditionBySurvivor(saveExp.SurvivorId);
                if (state == null)
                {
                    state = new ExpeditionState();
                    if (existingExpeditions != null)
                    {
                        existingExpeditions.Add(state);
                    }
                }

                state.ExpeditionId = saveExp.ExpeditionId;
                state.SurvivorId = saveExp.SurvivorId;
                state.TargetLocationId = saveExp.TargetLocationId;
                state.TargetLocationName = saveExp.TargetLocationName;
                state.Stance = saveExp.Stance;
                state.Phase = saveExp.Phase;
                state.CurrentTick = saveExp.CurrentTick;
                state.TotalDistanceTicks = saveExp.TotalDistanceTicks;
                state.TravelTicksCompleted = saveExp.TravelTicksCompleted;
                state.LootingTicksCompleted = saveExp.LootingTicksCompleted;
                state.CarryingCapacity = saveExp.CarryingCapacity;
                state.CurrentWeight = saveExp.CurrentWeight;
                state.Stamina = saveExp.Stamina;
                state.SuitDegradation = saveExp.SuitDegradation;
                state.TrueRadPerHour = saveExp.TrueRadPerHour;
                state.DangerLevel = saveExp.DangerLevel;
                state.IsPushingLuck = saveExp.IsPushingLuck;
                state.IsRetreating = saveExp.IsRetreating;
                state.isCommsSevered = saveExp.IsCommsSevered;
                state.flashpointBehavior = saveExp.FlashpointBehavior;
                state.originalEtaTicks = saveExp.OriginalEtaTicks;
                state.shelterDelayTicksRemaining = saveExp.ShelterDelayTicksRemaining;
                state.returnSpeedMultiplier = saveExp.ReturnSpeedMultiplier;
                state.returnSpeedDivisor = saveExp.ReturnSpeedDivisor;
                state.Survivor = survivor;

                state.CollectedLootItemIds.Clear();
                if (saveExp.CollectedLootItemIds != null)
                {
                    state.CollectedLootItemIds.AddRange(saveExp.CollectedLootItemIds);
                }

                state.CollectedLoot.Clear();
                if (_itemLookup != null && state.CollectedLootItemIds != null)
                {
                    for (int i = 0; i < state.CollectedLootItemIds.Count; i++)
                    {
                        var itemDef = _itemLookup(state.CollectedLootItemIds[i]);
                        if (itemDef != null)
                        {
                            state.CollectedLoot.Add(itemDef);
                        }
                    }
                }

                state.RecalculateWeight();
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
        public MedicalSystemSave Medical;
        public WorldPhaseSave WorldPhase;
        public DynamicEconomySave Economy;
        public PowerNetworkSave Power;
        public HatchDefenseSave HatchDefense;
        public FactionRadioInterceptSave FactionRadioIntercepts;
        public JournalSave Journal;
        public VictoryProjectSave VictoryProject;
        /// <summary>Deferred narrative chain queue (Prompt #43).</summary>
        public ScheduledEventSave ScheduledEvents;
        public WaterStorageSave Water;
        public GeneratedMapSave GeneratedMap;
        public FlashpointChoreographerSave FlashpointChoreographer;
        public AffinityMatrixSave Affinity = new AffinityMatrixSave();
        public List<ExpeditionSaveState> Expeditions = new List<ExpeditionSaveState>();
    }

    /// <summary>Expedition runtime state snapshot.</summary>
    [Serializable]
    public class ExpeditionSaveState
    {
        public string ExpeditionId;
        public string SurvivorId;
        public string TargetLocationId;
        public string TargetLocationName;
        public ExpeditionStance Stance;
        public ExpeditionPhase Phase;
        public int CurrentTick;
        public int TotalDistanceTicks;
        public int TravelTicksCompleted;
        public int LootingTicksCompleted;
        public float CarryingCapacity;
        public float CurrentWeight;
        public float Stamina;
        public float SuitDegradation;
        public float TrueRadPerHour;
        public float DangerLevel;
        public bool IsPushingLuck;
        public bool IsRetreating;

        // Day-30 Flashpoint intercept state (Prompt #26)
        public bool IsCommsSevered;
        public FlashpointBehavior FlashpointBehavior;
        public float OriginalEtaTicks;
        public int ShelterDelayTicksRemaining;
        public float ReturnSpeedMultiplier = 1f;
        public float ReturnSpeedDivisor = 1f;

        public List<string> CollectedLootItemIds = new List<string>();
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

        // Medical skill (0..1)
        public float MedicalSkill = 0.3f;

        // Mental-break system (Prompt #29)
        public string CurrentMentalBreakId = string.Empty;
        public float LowMoraleHours;
        public float MentalBreakCureProgress;
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
