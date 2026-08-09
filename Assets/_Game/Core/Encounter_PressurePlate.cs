using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.Core
{
    public enum TrapResponse
    {
        Dive,
        Freeze,
        Defuse
    }

    [Serializable]
    public class PressurePlateState
    {
        public string encounter_id = "encounter_pressure_plate";
        public List<string> survived_ids = new List<string>();
        public List<string> killed_ids = new List<string>();
    }

    /// <summary>DEMOTE-Encounter-batch — dormant ghost; SO expedition encounters remain live. Re-promote with Boot+Save+host.</summary>
    public sealed class Encounter_PressurePlate
    {
        private PressurePlateState _state;

        public event Action<string, TrapResponse> OnTrapResponseChosen;
        public event Action<string> OnTrapSurvived;
        public event Action<string> OnTrapKilled;

        public string EncounterId => _state.encounter_id;

        // Skill thresholds for each response
        public const float DiveAgilityThreshold = 0.6f;
        public const float FreezeStaminaThreshold = 0.5f;
        public const float DefuseEngineeringThreshold = 0.7f;

        // Probability of surviving if below threshold
        public const float BelowThresholdSurvivalChance = 0.5f;

        public Encounter_PressurePlate()
        {
            _state = new PressurePlateState();
        }

        /// <summary>
        /// Resolves a pressure-plate trap encounter.
        /// Dive requires Agility > 0.6, Freeze requires Stamina > 0.5,
        /// Defuse requires Engineering > 0.7. Below threshold: 50% lethal.
        /// Returns true if the survivor survived.
        /// </summary>
        public bool ResolveTrap(
            string survivor_id,
            TrapResponse response,
            float agility,
            float stamina,
            float engineering,
            System.Random rng)
        {
            if (string.IsNullOrEmpty(survivor_id))
            {
                Debug.LogError("[Encounter_PressurePlate] survivor_id is null or empty.");
                return false;
            }

            if (rng == null)
            {
                Debug.LogError("[Encounter_PressurePlate] rng is null.");
                return false;
            }

            OnTrapResponseChosen?.Invoke(survivor_id, response);

            bool meets_threshold;

            switch (response)
            {
                case TrapResponse.Dive:
                    meets_threshold = agility > DiveAgilityThreshold;
                    break;

                case TrapResponse.Freeze:
                    meets_threshold = stamina > FreezeStaminaThreshold;
                    break;

                case TrapResponse.Defuse:
                    meets_threshold = engineering > DefuseEngineeringThreshold;
                    break;

                default:
                    Debug.LogError($"[Encounter_PressurePlate] Unknown TrapResponse: {response}");
                    return false;
            }

            bool survived;

            if (meets_threshold)
            {
                // Above threshold — guaranteed survival
                survived = true;
            }
            else
            {
                // Below threshold — 50% lethal
                double roll = rng.NextDouble();
                survived = roll < BelowThresholdSurvivalChance;
            }

            if (survived)
            {
                if (!_state.survived_ids.Contains(survivor_id))
                {
                    _state.survived_ids.Add(survivor_id);
                }

                OnTrapSurvived?.Invoke(survivor_id);
                GameLog.Log($"[Encounter_PressurePlate] Survivor '{survivor_id}' survived " +
                          $"({response}, threshold_met={meets_threshold}).");
            }
            else
            {
                if (!_state.killed_ids.Contains(survivor_id))
                {
                    _state.killed_ids.Add(survivor_id);
                }

                OnTrapKilled?.Invoke(survivor_id);
                GameLog.Log($"[Encounter_PressurePlate] Survivor '{survivor_id}' killed by trap " +
                          $"({response}, threshold_met={meets_threshold}).");
            }

            return survived;
        }

        public PressurePlateState CaptureState()
        {
            return new PressurePlateState
            {
                encounter_id = _state.encounter_id,
                survived_ids = new List<string>(_state.survived_ids),
                killed_ids = new List<string>(_state.killed_ids)
            };
        }

        public void RestoreState(PressurePlateState saved)
        {
            _state = saved ?? new PressurePlateState();
        }
    }
}
