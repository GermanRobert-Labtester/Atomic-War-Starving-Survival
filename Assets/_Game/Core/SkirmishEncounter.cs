using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    public enum SkirmishPlayerAction
    {
        Intervene,
        Wait,
        Flee
    }

    [Serializable]
    public class SkirmishState
    {
        public string skirmishId;
        public string locationId;
        public string factionA;
        public string factionB;
        public int factionACount;
        public int factionBCount;
        public int factionAAmmo;
        public int factionBAmmo;
        public bool isResolved;
        public string winningFaction;
        public int winnerCountRemaining;
        public int winnerAmmoRemaining;
        public int totalCorpsesGenerated;
        public string uiMessage = "Gunfire Echoes.";
    }

    public class SkirmishOutcome
    {
        public string winningFaction;
        public int winnerCountRemaining;
        public int totalCorpses;
        public int totalAmmoWasted;
        public float hoursPassed;
        public string summaryText;
    }

    /// <summary>
    /// Prompt #321: System: Active Skirmish Engine (Multi-Faction Combat).
    /// Spawns two hostile groups at a single location and resolves background combat,
    /// generating corpses and consuming ammo if the player waits.
    /// </summary>
    public class SkirmishEncounter
    {
        private readonly Dictionary<string, SkirmishState> _activeSkirmishes = new Dictionary<string, SkirmishState>();

        public event Action<SkirmishState> OnSkirmishStarted;
        public event Action<SkirmishState, SkirmishOutcome> OnSkirmishResolved;
        public event Action<SkirmishState> OnPlayerIntervened;
        public event Action<SkirmishState> OnPlayerFleed;

        public IReadOnlyDictionary<string, SkirmishState> ActiveSkirmishes => _activeSkirmishes;

        public SkirmishState CreateSkirmish(string locationId, string factionA, string factionB, int countA = 4, int countB = 4)
        {
            var state = new SkirmishState
            {
                skirmishId = "skirmish_" + Guid.NewGuid().ToString("N").Substring(0, 8),
                locationId = locationId,
                factionA = factionA,
                factionB = factionB,
                factionACount = countA,
                factionBCount = countB,
                factionAAmmo = countA * 20,
                factionBAmmo = countB * 20,
                isResolved = false,
                uiMessage = "Gunfire Echoes."
            };

            _activeSkirmishes[locationId] = state;
            OnSkirmishStarted?.Invoke(state);
            return state;
        }

        public SkirmishOutcome ExecuteAction(string locationId, SkirmishPlayerAction action, System.Random rng = null)
        {
            if (rng == null) rng = new System.Random();
            if (!_activeSkirmishes.TryGetValue(locationId, out var state) || state.isResolved)
            {
                return null;
            }

            if (action == SkirmishPlayerAction.Flee)
            {
                OnPlayerFleed?.Invoke(state);
                return new SkirmishOutcome
                {
                    winningFaction = "none",
                    hoursPassed = 0f,
                    summaryText = "The player fled from the gunfire echoes."
                };
            }

            if (action == SkirmishPlayerAction.Intervene)
            {
                OnPlayerIntervened?.Invoke(state);
                return new SkirmishOutcome
                {
                    winningFaction = "player_involved",
                    hoursPassed = 0f,
                    summaryText = "Player stepped into the crossfire to engage both factions."
                };
            }

            // Action == Wait (takes 4 hours)
            return SimulateWait(state, 4.0f, rng);
        }

        public SkirmishOutcome SimulateWait(SkirmishState state, float hoursToWait, System.Random rng)
        {
            int casualtiesA = 0;
            int casualtiesB = 0;
            int ammoUsedA = 0;
            int ammoUsedB = 0;

            int rounds = Mathf.RoundToInt(hoursToWait * 3); // 12 simulation rounds for 4 hours
            for (int r = 0; r < rounds; r++)
            {
                if (state.factionACount <= 0 || state.factionBCount <= 0) break;

                // Faction A shoots B
                int shotsA = Math.Min(state.factionAAmmo, state.factionACount * 2);
                state.factionAAmmo -= shotsA;
                ammoUsedA += shotsA;
                if (rng.NextDouble() < 0.35 && state.factionBCount > 0)
                {
                    state.factionBCount--;
                    casualtiesB++;
                }

                // Faction B shoots A
                int shotsB = Math.Min(state.factionBAmmo, state.factionBCount * 2);
                state.factionBAmmo -= shotsB;
                ammoUsedB += shotsB;
                if (rng.NextDouble() < 0.35 && state.factionACount > 0)
                {
                    state.factionACount--;
                    casualtiesA++;
                }
            }

            state.totalCorpsesGenerated = casualtiesA + casualtiesB;
            state.isResolved = true;

            if (state.factionACount > state.factionBCount)
            {
                state.winningFaction = state.factionA;
                state.winnerCountRemaining = state.factionACount;
                state.winnerAmmoRemaining = state.factionAAmmo;
            }
            else if (state.factionBCount > state.factionACount)
            {
                state.winningFaction = state.factionB;
                state.winnerCountRemaining = state.factionBCount;
                state.winnerAmmoRemaining = state.factionBAmmo;
            }
            else
            {
                state.winningFaction = "Mutual Destruction";
                state.winnerCountRemaining = 0;
                state.winnerAmmoRemaining = 0;
            }

            var outcome = new SkirmishOutcome
            {
                winningFaction = state.winningFaction,
                winnerCountRemaining = state.winnerCountRemaining,
                totalCorpses = state.totalCorpsesGenerated,
                totalAmmoWasted = ammoUsedA + ammoUsedB,
                hoursPassed = hoursToWait,
                summaryText = $"After {hoursToWait:F0} hours, {state.winningFaction} prevailed. {state.totalCorpsesGenerated} corpses lie scattered among depleted casing."
            };

            OnSkirmishResolved?.Invoke(state, outcome);
            return outcome;
        }

        public SkirmishState GetSkirmish(string locationId)
        {
            if (string.IsNullOrEmpty(locationId)) return null;
            _activeSkirmishes.TryGetValue(locationId, out var state);
            return state;
        }
    }
}
