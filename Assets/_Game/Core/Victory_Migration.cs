using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class MigrationState
    {
        public string victoryId = "victory_migration";
        public bool requiresArmoredTruck = true;
        public bool bunkerAbandoned = false;
    }

    /// <summary>
    /// Prompt #767: Trail of Tears (Migration).
    /// Local map stripped of resources. Pack ArmoredTruck with all survivors and supplies.
    /// Abandon bunker, drive cross-country. Ambiguous hopeful ending.
    /// </summary>
    public class Victory_Migration
    {
        private MigrationState _state = new MigrationState();

        public event Action OnEndingTriggered;
        public event Action OnBunkerAbandoned;

        public MigrationState State => _state;

        /// <summary>
        /// Check whether the Migration victory conditions are met:
        /// armored truck available, local map depleted, at least one survivor alive.
        /// Returns true if the Migration ending can trigger.
        /// </summary>
        public bool CheckVictory(bool hasArmoredTruck, bool mapDepleted, int survivorCount)
        {
            if (_state.requiresArmoredTruck && !hasArmoredTruck)
                return false;
            if (!mapDepleted)
                return false;
            if (survivorCount < 1)
                return false;

            _state.bunkerAbandoned = true;

            OnBunkerAbandoned?.Invoke();
            OnEndingTriggered?.Invoke();

            return true;
        }

        /// <summary>
        /// Returns the ending narration text for the Migration scenario.
        /// </summary>
        public string GetEndingText(int survivorCount, int supplyCount)
        {
            string survivorWord = survivorCount == 1 ? "survivor" : "survivors";
            string supplyWord = supplyCount == 1 ? "crate" : "crates";

            return $"The bunker doors opened for the last time. "
                 + $"{survivorCount} {survivorWord} climbed into the armored truck "
                 + $"with {supplyCount} {supplyWord} of whatever was left. "
                 + "The engine turned over. The road ahead was ash and silence, "
                 + "but the wheels were moving. "
                 + "Maybe there is something out there. Maybe there isn't. "
                 + "They drive anyway.";
        }

        public bool IsVictoryAchieved()
        {
            return _state.bunkerAbandoned;
        }

        // ── Save / Load ────────────────────────────────────────────────

        public MigrationState CaptureState()
        {
            return new MigrationState
            {
                victoryId = _state.victoryId,
                requiresArmoredTruck = _state.requiresArmoredTruck,
                bunkerAbandoned = _state.bunkerAbandoned,
            };
        }

        public void RestoreState(MigrationState state)
        {
            if (state == null)
            {
                _state = new MigrationState();
                return;
            }
            _state = new MigrationState
            {
                victoryId = state.victoryId,
                requiresArmoredTruck = state.requiresArmoredTruck,
                bunkerAbandoned = state.bunkerAbandoned,
            };
        }
    }
}
