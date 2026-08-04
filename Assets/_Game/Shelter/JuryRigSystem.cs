using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Shelter
{
    /// <summary>
    /// Module Jury-Rigging / Overclocking (Prompt #52). A desperate player can
    /// jury-rig a broken shelter module to run at 150% speed without ElectronicScrap.
    /// The rigged module has a 10% daily chance to catch fire or permanently break.
    /// High-risk, high-reward UI toggle per module.
    /// Save/load safe. Plain C#.
    /// </summary>
    public class JuryRigSystem
    {
        /// <summary>Modules that are currently jury-rigged (ModuleId -> rig state).</summary>
        private readonly Dictionary<string, JuryRigState> _riggedModules = new Dictionary<string, JuryRigState>();

        /// <summary>Daily chance (0..1) of catastrophic failure per jury-rigged module.</summary>
        public const float CatastrophicFailureChancePerDay = 0.10f;

        /// <summary>Speed multiplier while jury-rigged.</summary>
        public const float OverclockSpeedMultiplier = 1.5f;

        /// <summary>Delegate for fire ignition in a room.</summary>
        public Action<string, float> StartFireInRoom;

        /// <summary>Delegate: get shelter for module lookup.</summary>
        private Func<Shelter> _getShelter;

        private readonly System.Random _rng;
        private float _dailyFailureAccumulator;

        // -- Events --
        public event Action<string> OnModuleJuryRigged;       // moduleId
        public event Action<string> OnModuleUnrigged;         // moduleId
        public event Action<string, string> OnCatastrophicFailure; // (moduleId, reason: "fire"/"destroyed")
        public event Action OnStateChanged;

        public JuryRigSystem(System.Random rng = null)
        {
            _rng = rng ?? new System.Random(52);
        }

        public void Bind(Func<Shelter> getShelter)
        {
            _getShelter = getShelter;
        }

        /// <summary>Whether a module is currently jury-rigged.</summary>
        public bool IsJuryRigged(string moduleId)
        {
            return !string.IsNullOrEmpty(moduleId) && _riggedModules.ContainsKey(moduleId);
        }

        /// <summary>Get the speed multiplier for a module (1.5 if rigged, 1.0 otherwise).</summary>
        public float GetSpeedMultiplier(string moduleId)
        {
            return IsJuryRigged(moduleId) ? OverclockSpeedMultiplier : 1f;
        }

        /// <summary>Jury-rig a broken module. Returns false if already rigged or missing.</summary>
        public bool JuryRig(string moduleId)
        {
            if (string.IsNullOrEmpty(moduleId)) return false;
            if (_riggedModules.ContainsKey(moduleId)) return false;

            var shelter = _getShelter?.Invoke();
            if (shelter == null) return false;
            var mod = shelter.GetModule(moduleId);
            if (mod == null) return false;

            // Module must be broken (disabled or 0 health) to be jury-rigged.
            if (mod.IsOperational && mod.FilterHealth > 0f) return false;

            mod.IsEnabled = true;
            mod.FilterHealth = Mathf.Max(mod.FilterHealth, 10f); // Barely functional.

            _riggedModules[moduleId] = new JuryRigState
            {
                ModuleId = moduleId,
                RigDay = 0, // resolved per tick
                HoursRigged = 0f
            };

            OnModuleJuryRigged?.Invoke(moduleId);
            OnStateChanged?.Invoke();
            return true;
        }

        /// <summary>Remove jury-rig from a module, returning it to broken state.</summary>
        public bool Unrig(string moduleId)
        {
            if (string.IsNullOrEmpty(moduleId)) return false;
            if (!_riggedModules.Remove(moduleId)) return false;

            var shelter = _getShelter?.Invoke();
            if (shelter != null)
            {
                var mod = shelter.GetModule(moduleId);
                if (mod != null)
                {
                    mod.IsEnabled = false;
                    mod.FilterHealth = 0f;
                }
            }

            OnModuleUnrigged?.Invoke(moduleId);
            OnStateChanged?.Invoke();
            return true;
        }

        /// <summary>List of currently rigged module ids.</summary>
        public IReadOnlyCollection<string> RiggedModuleIds => _riggedModules.Keys;

        // -----------------------------------------------------------------
        // Tick — daily catastrophic failure roll
        // -----------------------------------------------------------------

        /// <summary>Roll for catastrophic failure on each jury-rigged module daily.</summary>
        public void Tick(float gameHours, int currentDay)
        {
            if (gameHours <= 0f || _riggedModules.Count == 0) return;

            _dailyFailureAccumulator += gameHours / 24f;
            if (_dailyFailureAccumulator < 1f) return;
            _dailyFailureAccumulator -= 1f;

            var toFail = new List<string>();
            foreach (var kv in _riggedModules)
            {
                if (_rng.NextDouble() < CatastrophicFailureChancePerDay)
                    toFail.Add(kv.Key);
            }

            var shelter = _getShelter?.Invoke();
            foreach (var id in toFail)
            {
                // 50% fire, 50% permanent destruction.
                if (_rng.NextDouble() < 0.5f)
                {
                    // Fire!
                    var mod = shelter?.GetModule(id);
                    string roomId = mod != null && !string.IsNullOrEmpty(mod.RoomId)
                        ? mod.RoomId : "plant";
                    StartFireInRoom?.Invoke(roomId, 0.4f);
                    OnCatastrophicFailure?.Invoke(id, "fire");
                }
                else
                {
                    // Permanently destroyed beyond repair.
                    _riggedModules.Remove(id);
                    if (shelter != null)
                    {
                        var mod = shelter.GetModule(id);
                        if (mod != null)
                        {
                            mod.IsEnabled = false;
                            mod.FilterHealth = -1f; // Sentry: destroyed beyond repair.
                            mod.Level = 0;
                        }
                    }
                    OnCatastrophicFailure?.Invoke(id, "destroyed");
                }
            }

            if (toFail.Count > 0)
                OnStateChanged?.Invoke();
        }

        // -----------------------------------------------------------------
        // Save / Load
        // -----------------------------------------------------------------

        public JuryRigSave CaptureState()
        {
            var ids = new string[_riggedModules.Count];
            var hours = new float[_riggedModules.Count];
            int i = 0;
            foreach (var kv in _riggedModules)
            {
                ids[i] = kv.Key;
                hours[i] = kv.Value.HoursRigged;
                i++;
            }
            return new JuryRigSave
            {
                RiggedModuleIds = ids,
                RiggedHours = hours,
                DailyFailureAccumulator = _dailyFailureAccumulator
            };
        }

        public void RestoreState(JuryRigSave save)
        {
            _riggedModules.Clear();
            _dailyFailureAccumulator = 0f;
            if (save == null) return;

            _dailyFailureAccumulator = save.DailyFailureAccumulator;
            if (save.RiggedModuleIds != null)
            {
                for (int i = 0; i < save.RiggedModuleIds.Length; i++)
                {
                    string id = save.RiggedModuleIds[i];
                    if (string.IsNullOrEmpty(id)) continue;
                    float h = save.RiggedHours != null && i < save.RiggedHours.Length
                        ? save.RiggedHours[i] : 0f;
                    _riggedModules[id] = new JuryRigState
                    {
                        ModuleId = id,
                        HoursRigged = h
                    };
                }
            }
        }
    }

    [Serializable]
    public class JuryRigState
    {
        public string ModuleId;
        public int RigDay;
        public float HoursRigged;
    }

    [Serializable]
    public class JuryRigSave
    {
        public string[] RiggedModuleIds;
        public float[] RiggedHours;
        public float DailyFailureAccumulator;
    }
}
