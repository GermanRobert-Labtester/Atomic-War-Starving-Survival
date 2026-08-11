using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Inventory
{
    /// <summary>
    /// The type of AI personality core that can be installed into the AI Core module.
    /// </summary>
    public enum AICoreType
    {
        Aggressive,
        Medical,
        Balanced
    }

    [Serializable]
    public class AICoreDataState
    {
        public string itemId = "item_ai_core_data";
        public AICoreType installedCoreType = AICoreType.Balanced;
        public bool isCoreInstalled = false;
        // Track doors randomly locked by aggressive core
        public List<string> lockedRoomIds = new List<string>();
        // Track survivors lectured by medical core
        public List<string> lecturedSurvivorIds = new List<string>();
    }

    /// <summary>
    /// AI Personality Cores — pluggable data cores that alter the shelter AI's
    /// behaviour. "Aggressive Core" boosts turret damage but randomly locks doors.
    /// "Medical Core" speeds up the Autodoc but lectures survivors (morale penalty).
    /// "Balanced Core" provides no bonuses or penalties.
    /// Prompt #792: Item_AICoreData
    /// </summary>
    public class Item_AICoreData
    {
        // -- Constants --
        public const float AggressiveTurretMultiplier = 1.5f;
        public const float MedicalAutodocMultiplier = 1.5f;
        public const float MedicalMoralePenalty = -0.1f;
        public const float DefaultMultiplier = 1.0f;

        // -- Events --
        public event Action<AICoreType> OnCoreInstalled;       // coreType
        public event Action<string> OnDoorRandomlyLocked;       // roomId — aggressive core
        public event Action<string> OnSurvivorLectured;         // survivorId — medical core

        // -- State --
        private AICoreType _installedCoreType = AICoreType.Balanced;
        private bool _isCoreInstalled = false;
        private readonly List<string> _lockedRoomIds = new List<string>();
        private readonly List<string> _lecturedSurvivorIds = new List<string>();

        // -- Public API --

        /// <summary>
        /// Installs an AI personality core of the given type.
        /// Applies type-specific buffs and debuffs.
        /// </summary>
        public void InstallCore(AICoreType type)
        {
            _installedCoreType = type;
            _isCoreInstalled = true;
            OnCoreInstalled?.Invoke(type);

            // Aggressive core: randomly lock a door
            if (type == AICoreType.Aggressive)
            {
                // The actual room to lock is determined by the caller/system;
                // this is a hook — callers should subscribe to OnDoorRandomlyLocked.
            }

            // Medical core: lecture a survivor
            if (type == AICoreType.Medical)
            {
                // The actual survivor to lecture is determined by the caller/system;
                // this is a hook — callers should subscribe to OnSurvivorLectured.
            }
        }

        /// <summary>
        /// Notifies that the aggressive core has randomly locked a specific room's door.
        /// </summary>
        public void NotifyDoorLocked(string roomId)
        {
            if (string.IsNullOrEmpty(roomId)) return;
            if (!_lockedRoomIds.Contains(roomId))
                _lockedRoomIds.Add(roomId);
            OnDoorRandomlyLocked?.Invoke(roomId);
        }

        /// <summary>
        /// Notifies that the medical core has lectured a specific survivor.
        /// </summary>
        public void NotifySurvivorLectured(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return;
            if (!_lecturedSurvivorIds.Contains(survivorId))
                _lecturedSurvivorIds.Add(survivorId);
            OnSurvivorLectured?.Invoke(survivorId);
        }

        /// <summary>
        /// Returns the turret damage multiplier for the given core type.
        /// Aggressive cores boost turret damage by 50%.
        /// </summary>
        public float GetTurretDamageMultiplier(AICoreType type)
        {
            return type == AICoreType.Aggressive ? AggressiveTurretMultiplier : DefaultMultiplier;
        }

        /// <summary>
        /// Returns the Autodoc speed multiplier for the given core type.
        /// Medical cores speed up surgery by 50%.
        /// </summary>
        public float GetAutodocSpeedMultiplier(AICoreType type)
        {
            return type == AICoreType.Medical ? MedicalAutodocMultiplier : DefaultMultiplier;
        }

        /// <summary>
        /// Returns the morale penalty for the given core type.
        /// Medical cores lecture survivors, causing a -0.1 morale penalty.
        /// </summary>
        public float GetMoralePenalty(AICoreType type)
        {
            return type == AICoreType.Medical ? MedicalMoralePenalty : 0f;
        }

        /// <summary>Returns the currently installed core type.</summary>
        public AICoreType GetInstalledCoreType() => _installedCoreType;

        /// <summary>Returns true if a core is currently installed.</summary>
        public bool IsCoreInstalled() => _isCoreInstalled;

        // -- Save / Load --

        public AICoreDataState CaptureState()
        {
            return new AICoreDataState
            {
                itemId = "item_ai_core_data",
                installedCoreType = _installedCoreType,
                isCoreInstalled = _isCoreInstalled,
                lockedRoomIds = new List<string>(_lockedRoomIds),
                lecturedSurvivorIds = new List<string>(_lecturedSurvivorIds)
            };
        }

        public void RestoreState(AICoreDataState saved)
        {
            _lockedRoomIds.Clear();
            _lecturedSurvivorIds.Clear();
            if (saved == null) return;
            _installedCoreType = saved.installedCoreType;
            _isCoreInstalled = saved.isCoreInstalled;
            _lockedRoomIds.AddRange(saved.lockedRoomIds);
            _lecturedSurvivorIds.AddRange(saved.lecturedSurvivorIds);
        }
    }
}
