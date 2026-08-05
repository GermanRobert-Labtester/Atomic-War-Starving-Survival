using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Survivors;
using InventoryClass = AtomicWar._Game.Inventory.Inventory;

namespace AtomicWar._Game.Shelter
{
    /// <summary>
    /// Structural Integrity & Cave-ins (Prompt #49). The bunker ceiling degrades
    /// from nearby mortar strikes (Pre-Day 30) and severe FalloutStorms (Post-Day 30).
    /// Integrity below 50% causes DustLeaks (ambient contamination rises). Below 10%
    /// a random room caves in, permanently destroying modules and crushing occupants.
    /// Players craft ShoringStruts to reinforce.
    /// Save/load safe. Plain C#.
    /// </summary>
    public class StructuralIntegritySystem
    {
        public const string ShoringStrutModuleId = "shoring_strut";
        public const float MaxIntegrity = 100f;
        public const float DustLeakThreshold = 50f;
        public const float CaveInThreshold = 10f;
        public const float DustLeakContaminationPerHour = 0.015f;

        /// <summary>Integrity damage from one nearby mortar strike.</summary>
        public const float MortarStrikeDamage = 12f;

        /// <summary>Integrity damage per hour of severe FalloutStorm.</summary>
        public const float FalloutStormDamagePerHour = 3f;

        /// <summary>Integrity restored per installed ShoringStrut level (per hour of work).</summary>
        public const float StrutRepairPerLevelPerHour = 2f;

        /// <summary>Morale hit when a cave-in kills a survivor.</summary>
        public const float CaveInDeathMoralePenalty = 25f;

        /// <summary>Max shoring strut levels installable.</summary>
        public const int MaxStrutLevels = 5;

        /// <summary>Base wood units consumed to build one shoring strut level.</summary>
        public const int WoodPerStrut = 4;

        public const string WoodItemId = "wood";

        private float _integrity = MaxIntegrity;
        private bool _hasDustLeaks;
        private readonly HashSet<string> _cavedInRoomIds = new HashSet<string>();
        private readonly System.Random _rng;

        /// <summary>Delegate: retrieves the shelter for module lookups.</summary>
        private Func<Shelter> _getShelter;

        /// <summary>Delegate: retrieves living survivors for occupant crush.</summary>
        private Func<IReadOnlyList<Survivors.Survivor>> _getSurvivors;

        /// <summary>Delegate: inflict trauma on a survivor (crushed limb).</summary>
        private Action<Survivors.Survivor, string> _inflictTrauma;

        private ShelterPerkSystem _shelterPerks;
        private Func<int> _getDay;
        private CeilingCollapseSystem _ceilingCollapse;
        private PersonalQuestSystem _personalQuests;

        // -- Public state --
        public float Integrity => _integrity;
        public bool HasDustLeaks => _hasDustLeaks;
        public IReadOnlyCollection<string> CavedInRoomIds => _cavedInRoomIds;

        public int StrutLevel
        {
            get
            {
                var shelter = _getShelter?.Invoke();
                if (shelter == null) return 0;
                var mod = shelter.GetModule(ShoringStrutModuleId);
                return mod != null && mod.IsOperational ? mod.Level : 0;
            }
        }

        public float DamageResistance
        {
            get
            {
                int level = StrutLevel;
                // Each strut level reduces incoming damage by 25%, capped at 80%.
                return Mathf.Min(0.8f, level * 0.25f);
            }
        }

        // -- Events --
        public event Action<float, float> OnIntegrityChanged;          // (old, new)
        public event Action OnDustLeaksStarted;
        public event Action OnDustLeaksStopped;
        public event Action<string, List<string>> OnCaveIn;             // (roomId, destroyedModuleIds)
        public event Action<Survivors.Survivor> OnOccupantCrushed;

        public StructuralIntegritySystem(System.Random rng = null)
        {
            _rng = rng ?? new System.Random(43);
        }

        public void Bind(
            Func<Shelter> getShelter,
            Func<IReadOnlyList<Survivors.Survivor>> getSurvivors = null,
            Action<Survivors.Survivor, string> inflictTrauma = null)
        {
            _getShelter = getShelter;
            _getSurvivors = getSurvivors;
            _inflictTrauma = inflictTrauma;
        }

        /// <summary>Prompt #196 — Structural Engineer strut cost / ceiling load.</summary>
        public void BindShelterPerks(
            ShelterPerkSystem perks,
            Func<int> getDay = null,
            CeilingCollapseSystem ceiling = null)
        {
            _shelterPerks = perks;
            _getDay = getDay ?? (() => 0);
            _ceilingCollapse = ceiling;
        }

        /// <summary>Prompt #227 — Vault Builder locks integrity at 100%.</summary>
        public void BindPersonalQuests(PersonalQuestSystem personalQuests) =>
            _personalQuests = personalQuests;

        /// <summary>
        /// Build one shoring strut level. Consumes wood (50% less with Structural Engineer).
        /// When <paramref name="roomId"/> is set and builder has the perk, that room
        /// gains 2× ceiling-load capacity.
        /// </summary>
        public bool TryInstallShoringStrut(
            Survivor builder,
            InventoryClass inventory,
            ItemDefinition woodItem,
            string roomId = null)
        {
            if (builder == null || !builder.IsAlive || inventory == null || woodItem == null)
                return false;
            if (StrutLevel >= MaxStrutLevels) return false;

            float woodMult = _shelterPerks != null
                ? _shelterPerks.GetShoringWoodCostMultiplier(builder)
                : 1f;
            int woodCost = Mathf.Max(1, Mathf.RoundToInt(WoodPerStrut * woodMult));
            if (inventory.Count(woodItem) < woodCost) return false;
            if (!inventory.Remove(woodItem, woodCost)) return false;

            var shelter = _getShelter?.Invoke();
            if (shelter == null) return false;

            var mod = shelter.GetModule(ShoringStrutModuleId);
            if (mod == null)
            {
                mod = new ShelterModuleInstance(ShoringStrutModuleId, 1)
                {
                    IsEnabled = true,
                    FilterHealth = 100f,
                    RoomId = roomId ?? "plant"
                };
                shelter.AddModule(mod);
            }
            else
            {
                mod.Level = Mathf.Min(MaxStrutLevels, mod.Level + 1);
                mod.IsEnabled = true;
                if (!string.IsNullOrEmpty(roomId))
                    mod.RoomId = roomId;
            }

            // Structural Engineer: reinforced rooms gain 2× ceiling load capacity.
            if (_shelterPerks != null && _shelterPerks.IsStructuralEngineer(builder)
                && !string.IsNullOrEmpty(roomId) && _ceilingCollapse != null)
            {
                _ceilingCollapse.ReinforceRoom(
                    roomId,
                    _shelterPerks.GetCeilingLoadMultiplier(builder));
            }

            _shelterPerks?.RecordShoringStrutBuilt(builder, 1, _getDay?.Invoke() ?? 0);
            return true;
        }

        /// <summary>Wood cost for next strut given optional builder perk.</summary>
        public int GetShoringWoodCost(Survivor builder)
        {
            float mult = _shelterPerks != null
                ? _shelterPerks.GetShoringWoodCostMultiplier(builder)
                : 1f;
            return Mathf.Max(1, Mathf.RoundToInt(WoodPerStrut * mult));
        }

        // -----------------------------------------------------------------
        // Damage & Repair
        // -----------------------------------------------------------------

        /// <summary>Apply integrity damage from an external event.
        /// Damage is reduced by installed shoring struts.</summary>
        public void ApplyDamage(float rawDamage, string cause = null)
        {
            // Prompt #227 — Vault Builder: structural integrity permanently locked at 100%.
            if (_personalQuests != null
                && _personalQuests.LocksStructuralIntegrityAtMax(_getSurvivors?.Invoke()))
            {
                if (_integrity < MaxIntegrity)
                {
                    float old = _integrity;
                    _integrity = MaxIntegrity;
                    _hasDustLeaks = false;
                    OnIntegrityChanged?.Invoke(old, _integrity);
                }
                return;
            }
            if (rawDamage <= 0f) return;
            float resist = DamageResistance;
            float actual = rawDamage * (1f - resist);
            SetIntegrity(_integrity - actual);
        }

        /// <summary>Repair integrity by spending work-hours. Rate scales with strut level.
        /// #250 Pillar of Atlas death applies a permanent 20% repair speed debuff.</summary>
        public float Repair(float workHours)
        {
            if (workHours <= 0f) return 0f;
            int level = StrutLevel;
            float repairRate = StrutRepairPerLevelPerHour * Mathf.Max(1, level);
            if (_personalQuests != null)
                repairRate *= _personalQuests.GetShelterRepairSpeedMultiplier();
            float repaired = Mathf.Min(workHours * repairRate, MaxIntegrity - _integrity);
            if (repaired > 0f)
                SetIntegrity(_integrity + repaired);
            return repaired;
        }

        // -----------------------------------------------------------------
        // Tick — dust leaks, cave-in checks
        // -----------------------------------------------------------------

        /// <summary>Apply dust-leak contamination and check for cave-ins.</summary>
        public void Tick(float gameHours, Shelter shelter)
        {
            if (_personalQuests != null
                && _personalQuests.LocksStructuralIntegrityAtMax(_getSurvivors?.Invoke()))
            {
                if (_integrity < MaxIntegrity)
                {
                    float old = _integrity;
                    _integrity = MaxIntegrity;
                    _hasDustLeaks = false;
                    OnIntegrityChanged?.Invoke(old, _integrity);
                }
                return;
            }

            if (gameHours <= 0f || shelter == null) return;

            UpdateDustLeaks(gameHours, shelter);
            CheckCaveIn(shelter);
        }

        private void UpdateDustLeaks(float gameHours, Shelter shelter)
        {
            bool shouldLeak = _integrity < DustLeakThreshold;

            if (shouldLeak && !_hasDustLeaks)
            {
                _hasDustLeaks = true;
                OnDustLeaksStarted?.Invoke();
            }
            else if (!shouldLeak && _hasDustLeaks)
            {
                _hasDustLeaks = false;
                OnDustLeaksStopped?.Invoke();
            }

            if (_hasDustLeaks && shelter.Rooms != null)
            {
                float dust = DustLeakContaminationPerHour * gameHours
                    * (1f - _integrity / DustLeakThreshold);
                for (int i = 0; i < shelter.Rooms.Count; i++)
                {
                    var room = shelter.Rooms[i];
                    if (room == null || room.RoomId == "deep_vault") continue;
                    room.AmbientContamination = Mathf.Clamp01(
                        room.AmbientContamination + dust);
                }
            }
        }

        private void CheckCaveIn(Shelter shelter)
        {
            if (_integrity >= CaveInThreshold) return;
            if (shelter.Rooms == null || shelter.Rooms.Count == 0) return;

            // Cave-in chance scales with how far below the threshold we are.
            float danger = 1f - (_integrity / CaveInThreshold);
            float chancePerHour = 0.02f * danger; // 2% per hour at 0 integrity

            if (_rng.NextDouble() >= chancePerHour) return;

            // Pick a random room that isn't already caved in.
            var candidates = new List<ShelterRoom>();
            for (int i = 0; i < shelter.Rooms.Count; i++)
            {
                var r = shelter.Rooms[i];
                if (r != null && !_cavedInRoomIds.Contains(r.RoomId)
                    && r.RoomId != "deep_vault")
                    candidates.Add(r);
            }
            if (candidates.Count == 0) return;

            var target = candidates[_rng.Next(candidates.Count)];
            ExecuteCaveIn(target, shelter);
        }

        private void ExecuteCaveIn(ShelterRoom room, Shelter shelter)
        {
            _cavedInRoomIds.Add(room.RoomId);
            var destroyedModules = new List<string>();

            // Destroy all modules installed in this room.
            if (shelter.Modules != null)
            {
                for (int i = shelter.Modules.Count - 1; i >= 0; i--)
                {
                    var mod = shelter.Modules[i];
                    if (mod == null) continue;
                    if (string.Equals(mod.RoomId, room.RoomId, StringComparison.Ordinal))
                    {
                        destroyedModules.Add(mod.ModuleId);
                        mod.IsEnabled = false;
                        mod.FilterHealth = 0f;
                    }
                }
            }

            // Mark the room as sealed/caved in.
            room.UnlockState = RoomUnlockState.Sealed;
            room.RubbleClearHoursRemaining = 48f; // 2 days to clear
            room.RubbleClearHoursTotal = 48f;

            // Crush occupants.
            var survivors = _getSurvivors?.Invoke();
            if (survivors != null)
            {
                for (int i = 0; i < survivors.Count; i++)
                {
                    var sv = survivors[i];
                    if (sv == null || !sv.IsAlive) continue;
                    if (string.Equals(sv.CurrentRoomId, room.RoomId, StringComparison.Ordinal))
                    {
                        // 50% instant death, 50% severe injury.
                        if (_rng.NextDouble() < 0.5f)
                        {
                            sv.State = Survivors.SurvivorState.Dead;
                            OnOccupantCrushed?.Invoke(sv);
                        }
                        else
                        {
                            sv.Needs.Health = Mathf.Clamp(sv.Needs.Health - 60f, 0f, 100f);
                            _inflictTrauma?.Invoke(sv, "crushed_limb");
                        }
                    }
                }
            }

            OnCaveIn?.Invoke(room.RoomId, destroyedModules);
        }

        // -----------------------------------------------------------------
        // Save / Load
        // -----------------------------------------------------------------

        private void SetIntegrity(float value)
        {
            float old = _integrity;
            _integrity = Mathf.Clamp(value, 0f, MaxIntegrity);
            if (Mathf.Abs(_integrity - old) > 0.001f)
                OnIntegrityChanged?.Invoke(old, _integrity);
        }

        public StructuralIntegritySave CaptureState()
        {
            var ids = new string[_cavedInRoomIds.Count];
            _cavedInRoomIds.CopyTo(ids);
            return new StructuralIntegritySave
            {
                Integrity = _integrity,
                HasDustLeaks = _hasDustLeaks,
                CavedInRoomIds = ids
            };
        }

        public void RestoreState(StructuralIntegritySave save)
        {
            if (save == null)
            {
                _integrity = MaxIntegrity;
                _hasDustLeaks = false;
                _cavedInRoomIds.Clear();
                return;
            }
            _integrity = Mathf.Clamp(save.Integrity, 0f, MaxIntegrity);
            _hasDustLeaks = save.HasDustLeaks;
            _cavedInRoomIds.Clear();
            if (save.CavedInRoomIds != null)
            {
                for (int i = 0; i < save.CavedInRoomIds.Length; i++)
                {
                    if (!string.IsNullOrEmpty(save.CavedInRoomIds[i]))
                        _cavedInRoomIds.Add(save.CavedInRoomIds[i]);
                }
            }
        }
    }

    [Serializable]
    public class StructuralIntegritySave
    {
        public float Integrity = StructuralIntegritySystem.MaxIntegrity;
        public bool HasDustLeaks;
        public string[] CavedInRoomIds;
    }
}
