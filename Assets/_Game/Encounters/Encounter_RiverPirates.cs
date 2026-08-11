using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Encounters
{
    [Serializable]
    public class RiverPiratesState
    {
        public string id = "encounter_river_pirates";
        public string displayName = "The River Pirates";
        public float pirateBoatHealth = 200f;
        public float lootValue = 500f;
        public bool lootRequiresDivingGear = true;
        public bool playerCanFlee = true;
    }

    /// <summary>
    /// Prompt #572: Encounter: The River Pirates.
    /// Triggers on aquatic expeditions.
    /// They use fast motorboats (loud, uses fuel).
    /// If player is in a rowboat, cannot flee.
    /// Combat is ship-to-ship.
    /// Sinking pirate boat yields massive loot but it sinks, requiring DivingGear to retrieve.
    /// </summary>
    /// <summary>DEMOTE-Encounter-batch — dormant ghost; SO expedition encounters remain live. Re-promote with Boot+Save+host.</summary>
    public class Encounter_RiverPirates
    {
        private RiverPiratesState _state = new RiverPiratesState();

        public event Action<RiverPiratesState> OnPiratesEngaged;
        public event Action<RiverPiratesState> OnPiratesDefeated;
        public event Action<RiverPiratesState> OnLootRetrieved;
        public event Action<RiverPiratesState> OnLootSunk;
        public event Action<RiverPiratesState> OnFleeFailed;

        public RiverPiratesState State => _state;

        public bool EngageCombat(string playerVesselType, float playerCombatPower, System.Random rng)
        {
            // Rowboats cannot flee
            if (playerVesselType == "rowboat")
            {
                _state.playerCanFlee = false;
            }
            else
            {
                _state.playerCanFlee = true;
            }

            OnPiratesEngaged?.Invoke(_state);

            // Ship-to-ship combat resolution
            float pirateCombatPower = 100f; // Base pirate combat power
            float playerRoll = (float)(rng.NextDouble() * playerCombatPower);
            float pirateRoll = (float)(rng.NextDouble() * pirateCombatPower);

            if (playerRoll > pirateRoll)
            {
                _state.pirateBoatHealth = 0f;
                OnPiratesDefeated?.Invoke(_state);
                return true;
            }
            else
            {
                float damageToPirates = playerCombatPower * 0.5f;
                _state.pirateBoatHealth = Mathf.Max(0f, _state.pirateBoatHealth - damageToPirates);

                if (_state.pirateBoatHealth <= 0f)
                {
                    OnPiratesDefeated?.Invoke(_state);
                    return true;
                }

                return false;
            }
        }

        public bool TryFlee(string playerVesselType, System.Random rng)
        {
            if (playerVesselType == "rowboat")
            {
                _state.playerCanFlee = false;
                OnFleeFailed?.Invoke(_state);
                return false;
            }

            // Motorized vessels can flee
            float fleeChance = 0.70f;
            double roll = rng.NextDouble();
            bool success = roll < fleeChance;

            if (!success)
            {
                OnFleeFailed?.Invoke(_state);
            }

            return success;
        }

        public bool SinkPirateBoat(bool hasDivingGear)
        {
            if (_state.pirateBoatHealth > 0f)
            {
                return false; // Cannot sink if not defeated
            }

            if (hasDivingGear)
            {
                OnLootRetrieved?.Invoke(_state);
                return true;
            }
            else
            {
                OnLootSunk?.Invoke(_state);
                return false;
            }
        }

        public RiverPiratesState CaptureState() => _state;

        public void RestoreState(RiverPiratesState saved)
        {
            _state = saved ?? new RiverPiratesState();
        }
    }
}
