using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Endgame
{
    [Serializable]
    public class UnifierState
    {
        public string victoryId = "victory_unifier";
        public float trustRequired = 1.0f;
    }

    /// <summary>
    /// Prompt #765: Faction Unifier.
    /// Through trade, diplomacy, and Embassy, achieve 100% Trust with all non-terrorist factions.
    /// Warlords sign peace treaty. Civil War ends.
    /// </summary>
    public class Victory_Unifier
    {
        private static readonly HashSet<string> TerroristFactionIds = new HashSet<string>
        {
            "faction_terrorist",
            "faction_radical"
        };

        private UnifierState _state = new UnifierState();

        public event Action OnEndingTriggered;
        public event Action<string> OnFactionTrustMaxed;

        public UnifierState State => _state;

        /// <summary>
        /// Check whether all non-terrorist factions have reached maximum trust.
        /// Faction trust levels are keyed by factionId with values in [0, 1].
        /// Returns true if the Unifier victory condition is met.
        /// </summary>
        public bool CheckVictory(Dictionary<string, float> factionTrustLevels)
        {
            if (factionTrustLevels == null || factionTrustLevels.Count == 0)
                return false;

            bool anyNonTerrorist = false;

            foreach (var kvp in factionTrustLevels)
            {
                if (IsTerroristFaction(kvp.Key)) continue;

                anyNonTerrorist = true;

                if (kvp.Value >= _state.trustRequired)
                {
                    OnFactionTrustMaxed?.Invoke(kvp.Key);
                }
                else
                {
                    return false;
                }
            }

            if (!anyNonTerrorist) return false;

            OnEndingTriggered?.Invoke();
            return true;
        }

        /// <summary>
        /// Returns the ending narration text for the Unifier scenario.
        /// </summary>
        public string GetEndingText()
        {
            return "The last radio crackled with a word no one expected: peace. "
                 + "Trade routes opened. The Warlords laid down their arms and signed. "
                 + "The Civil War ended not with a bang, but with a handshake across a table of ash.";
        }

        /// <summary>
        /// Override or extend the set of faction IDs treated as terrorist/excluded factions.
        /// </summary>
        public static bool IsTerroristFaction(string factionId)
        {
            if (string.IsNullOrEmpty(factionId)) return false;
            return TerroristFactionIds.Contains(factionId);
        }

        public bool IsVictoryAchieved(Dictionary<string, float> factionTrustLevels)
        {
            return CheckVictory(factionTrustLevels);
        }

        // ── Save / Load ────────────────────────────────────────────────

        public UnifierState CaptureState()
        {
            return new UnifierState
            {
                victoryId = _state.victoryId,
                trustRequired = _state.trustRequired,
            };
        }

        public void RestoreState(UnifierState state)
        {
            if (state == null)
            {
                _state = new UnifierState();
                return;
            }
            _state = new UnifierState
            {
                victoryId = state.victoryId,
                trustRequired = state.trustRequired,
            };
        }
    }
}
