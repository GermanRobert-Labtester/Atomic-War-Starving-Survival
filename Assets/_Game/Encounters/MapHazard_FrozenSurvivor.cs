using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.Encounters
{
    [Serializable]
    public class FrozenSurvivorState
    {
        public string hazard_id = "map_hazard_frozen_survivor";
        public string node_id = "";
        public bool is_alive = true;
        public bool was_entered = false;
        public bool was_rescued = false;
        public bool was_looted = false;
        public bool was_abandoned = false;
        public string survivor_name = "";
        public float body_temperature = 27f; // hypothermic but possibly alive
    }

    /// <summary>
    /// Prompt #902: Frozen Survivor — a body in the ash that might not be a body.
    /// Appears on Snowfield and FrozenLake nodes. At first glance, it's just
    /// another frozen corpse. A Perception check reveals the chest is still
    /// rising — barely. The survivor can attempt a rescue (costs Warmth, low
    /// success chance) or walk away (morale hit). If they succeed, the rescued
    /// person gives a small reward and vanishes into the ash; if they fail,
    /// the person dies in their arms.
    ///
    /// Tone: cold, exhausted, human. No heroism here — just the math of
    /// whether you can afford to lose the body heat.
    /// </summary>
    public class MapHazard_FrozenSurvivor
    {
        private FrozenSurvivorState _state;

        private const float PerceptionThreshold = 0.5f;
        private const float RescueBaseChance = 0.35f;
        private const float WarmthCostToAttemptRescue = 20f;
        private const float RescueFailMoraleHit = 15f;
        private const float RescueSuccessMoraleBoost = 25f;
        private const float WalkAwayMoraleHit = 5f;
        private const float LootCorpseMoraleHit = 8f;

        private static readonly string[] RandomNames =
        {
            "Anya", "Branko", "Catarina", "Dmitri", "Elena",
            "Florian", "Greta", "Henrik", "Irina", "Josef",
            "Katja", "Leon", "Mira", "Nikolai", "Olga",
            "Pavel", "Renata", "Sergei", "Tanya", "Viktor"
        };

        public event Action<string> OnCorpseSpotted;           // node_id
        public event Action<string> OnSurvivorFound;           // survivor_name
        public event Action<string> OnRescueSucceeded;          // survivor_name
        public event Action<string, string> OnRescueFailed;    // survivor_name, last_words
        public event Action<string> OnWalkedAway;              // survivor_name
        public event Action<string> OnCorpseLooted;            // node_id

        public string HazardId => _state.hazard_id;
        public string NodeId => _state.node_id;
        public bool WasRescued => _state.was_rescued;
        public string SurvivorName => _state.survivor_name;

        public MapHazard_FrozenSurvivor()
        {
            _state = new FrozenSurvivorState();
        }

        /// <summary>
        /// Initialise the hazard for a specific node.
        /// </summary>
        /// <remarks>
        /// Idempotent per node, matching <see cref="MapHazard_VenusTrap.EnterNode"/>.
        /// The expedition host dispatches this on every looting arrival, and both this
        /// hazard's rolls are seeded fixed per node — so an unconditional re-arm would
        /// clear <c>was_rescued</c> and re-offer the identical, identically-successful
        /// rescue on every revisit, farming the morale reward without bound.
        /// </remarks>
        /// <returns>
        /// True when there is a live, unresolved encounter for the caller to present.
        /// False once the person has been rescued or the body looted.
        /// </returns>
        public bool EnterNode(string nodeId, System.Random rng = null)
        {
            string id = nodeId ?? string.Empty;
            bool sameNode = !string.IsNullOrEmpty(id)
                            && string.Equals(_state.node_id, id, StringComparison.Ordinal);

            if (sameNode && _state.was_entered)
            {
                // Already armed for this node; spent once rescued or looted.
                return !_state.was_rescued && !_state.was_looted;
            }

            rng ??= SeededRandom.CreateFixed("frozen_survivor:" + (nodeId ?? "node"));
            _state.node_id = id;
            _state.was_entered = true;
            _state.is_alive = rng.NextDouble() < 0.6f; // 60% chance they're still alive
            _state.was_rescued = false;
            _state.was_looted = false;
            _state.was_abandoned = false;
            _state.survivor_name = RandomNames[rng.Next(RandomNames.Length)];
            _state.body_temperature = 25f + (float)rng.NextDouble() * 10f; // 25-35 C

            OnCorpseSpotted?.Invoke(_state.node_id);
            return true;
        }

        /// <summary>
        /// Perception check to notice the chest is still rising.
        /// Returns true if the survivor is alive AND the player notices.
        /// </summary>
        public bool CheckForSignsOfLife(float perception)
        {
            if (!_state.is_alive || _state.was_rescued) return false;
            if (perception >= PerceptionThreshold)
            {
                OnSurvivorFound?.Invoke(_state.survivor_name);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Attempt to warm and revive the frozen person.
        /// Costs the rescuer body heat. Success is not guaranteed.
        /// </summary>
        /// <returns>True if the rescue succeeded.</returns>
        public bool AttemptRescue(float medicalSkill, System.Random rng = null)
        {
            if (!_state.is_alive || _state.was_rescued) return false;

            rng ??= SeededRandom.CreateFixed("frozen_survivor_rescue:" + _state.node_id);
            float chance = RescueBaseChance + (medicalSkill / 200f);
            chance = Mathf.Clamp01(chance);

            if (rng.NextDouble() < chance)
            {
                _state.was_rescued = true;
                _state.is_alive = false;
                OnRescueSucceeded?.Invoke(_state.survivor_name);
                return true;
            }
            else
            {
                _state.is_alive = false;
                string lastWords = GetRandomLastWords(rng);
                OnRescueFailed?.Invoke(_state.survivor_name, lastWords);
                return false;
            }
        }

        /// <summary>
        /// Choose to walk away without trying to help.
        /// Returns the morale penalty to apply.
        /// </summary>
        public float WalkAway()
        {
            // Charged once per node: leaving them behind is the same choice each pass,
            // and the host re-presents the encounter on every arrival until it resolves.
            if (_state.was_rescued || _state.was_abandoned) return 0f;
            _state.was_abandoned = true;
            OnWalkedAway?.Invoke(_state.survivor_name);
            return WalkAwayMoraleHit;
        }

        /// <summary>
        /// Loot the body (if dead). Returns the morale penalty.
        /// </summary>
        public float LootCorpse()
        {
            if (_state.was_looted) return 0f;
            _state.was_looted = true;
            OnCorpseLooted?.Invoke(_state.node_id);
            return _state.is_alive ? RescueFailMoraleHit : LootCorpseMoraleHit;
        }

        /// <summary>Warmth cost for attempting a rescue.</summary>
        public float GetRescueWarmthCost() => WarmthCostToAttemptRescue;

        /// <summary>Morale boost if rescue succeeds.</summary>
        public float GetRescueSuccessMoraleBoost() => RescueSuccessMoraleBoost;

        /// <summary>Morale penalty if rescue fails.</summary>
        public float GetRescueFailMoraleHit() => RescueFailMoraleHit;

        /// <summary>Morale penalty for walking away from a living person.</summary>
        public float GetWalkAwayMoraleHit() => WalkAwayMoraleHit;

        /// <summary>Whether this person is still alive.</summary>
        public bool IsAlive() => _state.is_alive && !_state.was_rescued;

        // ── Last words ──────────────────────────────────────────────────

        private static string GetRandomLastWords(System.Random rng)
        {
            string[] words =
            {
                "\"Tell them I made it as far as the snow line.\"",
                "\"There's a cache in the blue silo. South ridge. Don't let it go to waste.\"",
                "\"I can see the river. Is that the river? It looks like glass.\"",
                "\"My daughter is still out there. Her name is Lena. She's seven.\"",
                "\"Thank you for stopping. Most people don't.\"",
                "\"The cold isn't so bad, once you stop fighting it.\"",
                "\"I had a dog. He ran off three days ago. I hope he found somewhere warm.\"",
                "\"Burn my boots. They're still good. No sense wasting them.\"",
            };
            return words[rng.Next(words.Length)];
        }

        // ── Save / Load ─────────────────────────────────────────────────

        public FrozenSurvivorState CaptureState() => _state;

        public void RestoreState(FrozenSurvivorState save)
        {
            if (save != null) _state = save;
        }
    }
}
