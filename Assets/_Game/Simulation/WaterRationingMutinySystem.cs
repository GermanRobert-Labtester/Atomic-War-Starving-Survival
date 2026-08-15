using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Simulation
{
    /// <summary>
    /// Expansion VI — Water Rationing & Mutiny. You can set the water slider:
    /// 2L (Healthy), 1L (Thirsty, -5% work speed), 0.5L (Dehydrated, Health decay).
    /// If survivors catch the player cheating (drinking from reserves while others
    /// are dehydrated), mutiny triggers. The bunker locks the RationLock module.
    /// Save/load safe. Plain C#.
    /// </summary>
    public class WaterRationingMutinySystem
    {
        public enum RationLevel
        {
            Healthy = 2,    // 2L per person per day
            Thirsty = 1,    // 1L, -5% work speed
            Dehydrated = 0  // 0.5L, Health decay
        }

        public const float HealthyLiters = 2f;
        public const float ThirstyLiters = 1f;
        public const float DehydratedLiters = 0.5f;
        public const float ThirstyWorkSpeedPenalty = 0.05f;
        public const float DehydratedHealthDecayPerHour = 0.5f;
        public const float DehydratedMoraleDecayPerHour = 1f;
        public const int MutinyTriggerDays = 3; // 3 days dehydrated before mutiny risk
        public const float MutinyChancePerDay = 0.15f;
        public const string Module_RationLock = "ration_lock";

        public event Action<RationLevel> OnRationLevelChanged;
        public event Action<string> OnCheatingDetected;
        public event Action OnMutinyTriggered;
        public event Action<string> OnMutinyResolved;

        private RationLevel _currentLevel = RationLevel.Healthy;
        private float _dehydratedDays;
        private bool _mutinyTriggered;
        private bool _cheatingDetected;
        private readonly System.Random _rng;

        public RationLevel CurrentLevel => _currentLevel;
        public float DehydratedDays => _dehydratedDays;
        public bool IsMutinyTriggered => _mutinyTriggered;
        public bool IsCheatingDetected => _cheatingDetected;

        public WaterRationingMutinySystem(System.Random rng = null)
        {
            _rng = rng ?? new System.Random(4444);
        }

        /// <summary>Set the water ration level for the bunker.</summary>
        public void SetRationLevel(RationLevel level)
        {
            if (_currentLevel == level) return;
            _currentLevel = level;
            if (level != RationLevel.Dehydrated)
                _dehydratedDays = 0;
            OnRationLevelChanged?.Invoke(level);
        }

        /// <summary>
        /// Report cheating: a favored survivor was caught drinking from clean_water
        /// reserves while others are dehydrated.
        /// </summary>
        public void ReportCheating(string cheaterId)
        {
            if (_currentLevel != RationLevel.Dehydrated) return;
            _cheatingDetected = true;
            OnCheatingDetected?.Invoke(cheaterId);
        }

        /// <summary>
        /// Per-tick update. Applies dehydration effects and checks for mutiny.
        /// </summary>
        public void Tick(float gameHours, IReadOnlyList<AtomicWar._Game.Survivors.Survivor> survivors,
            Action<string, AtomicWar._Game.Survivors.NeedKind, float> modifyNeed)
        {
            if (_currentLevel == RationLevel.Healthy) return;

            float gameDays = gameHours / 24f;

            if (_currentLevel == RationLevel.Dehydrated)
            {
                _dehydratedDays += gameDays;

                // Health decay
                if (survivors != null)
                {
                    for (int i = 0; i < survivors.Count; i++)
                    {
                        var sv = survivors[i];
                        if (sv == null || !sv.IsAlive) continue;
                        modifyNeed?.Invoke(sv.Id, AtomicWar._Game.Survivors.NeedKind.Health,
                            -DehydratedHealthDecayPerHour * gameHours);
                        modifyNeed?.Invoke(sv.Id, AtomicWar._Game.Survivors.NeedKind.Morale,
                            -DehydratedMoraleDecayPerHour * gameHours);
                    }
                }

                // Mutiny check after 3 days dehydrated
                if (!_mutinyTriggered && _dehydratedDays >= MutinyTriggerDays)
                {
                    if (_cheatingDetected || _rng.NextDouble() < MutinyChancePerDay * gameDays)
                    {
                        _mutinyTriggered = true;
                        OnMutinyTriggered?.Invoke();
                    }
                }
            }
        }

        /// <summary>
        /// Resolve the mutiny. Player must negotiate, surrender weapons, or brawl.
        /// </summary>
        public bool ResolveMutiny(string resolution)
        {
            if (!_mutinyTriggered) return false;
            _mutinyTriggered = false;
            OnMutinyResolved?.Invoke(resolution);
            return true;
        }

        /// <summary>Get work speed multiplier based on current ration level.</summary>
        public float GetWorkSpeedMultiplier()
        {
            return _currentLevel switch
            {
                RationLevel.Healthy => 1f,
                RationLevel.Thirsty => 1f - ThirstyWorkSpeedPenalty,
                RationLevel.Dehydrated => 0.80f, // -20% work speed
                _ => 1f
            };
        }

        // ── Save / Load ───────────────────────────────────────────────

        public WaterRationSave CaptureState()
        {
            return new WaterRationSave
            {
                CurrentLevel = _currentLevel,
                DehydratedDays = _dehydratedDays,
                MutinyTriggered = _mutinyTriggered,
                CheatingDetected = _cheatingDetected
            };
        }

        public void RestoreState(WaterRationSave save)
        {
            _currentLevel = RationLevel.Healthy;
            _dehydratedDays = 0;
            _mutinyTriggered = false;
            _cheatingDetected = false;
            if (save == null) return;
            _currentLevel = save.CurrentLevel;
            _dehydratedDays = save.DehydratedDays;
            _mutinyTriggered = save.MutinyTriggered;
            _cheatingDetected = save.CheatingDetected;
        }
    }

    [Serializable]
    public class WaterRationSave
    {
        public WaterRationingMutinySystem.RationLevel CurrentLevel;
        public float DehydratedDays;
        public bool MutinyTriggered;
        public bool CheatingDetected;
    }
}
