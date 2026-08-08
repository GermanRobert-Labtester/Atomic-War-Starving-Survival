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
        /// <summary>Exclusive ammo scavenged from the field after resolution (item ids).</summary>
        public List<string> ScavengedAmmoIds = new List<string>();
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
            if (rng == null) rng = AtomicWar._Game.Utilities.SeededRandom.CreateFixed("skirmishencounter");
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

                // Faction A shoots B (ResolveHit-shaped hit chance vs B armor)
                int shotsA = Math.Min(state.factionAAmmo, state.factionACount * 2);
                state.factionAAmmo -= shotsA;
                ammoUsedA += shotsA;
                float hitA = ComputeSkirmishHitChance(state.factionA, state.factionB);
                if (rng.NextDouble() < hitA && state.factionBCount > 0)
                {
                    state.factionBCount--;
                    casualtiesB++;
                }

                // Faction B shoots A
                int shotsB = Math.Min(state.factionBAmmo, state.factionBCount * 2);
                state.factionBAmmo -= shotsB;
                ammoUsedB += shotsB;
                float hitB = ComputeSkirmishHitChance(state.factionB, state.factionA);
                if (rng.NextDouble() < hitB && state.factionACount > 0)
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

            var scavenged = RollScavengedAmmo(state, rng);
            var outcome = new SkirmishOutcome
            {
                winningFaction = state.winningFaction,
                winnerCountRemaining = state.winnerCountRemaining,
                totalCorpses = state.totalCorpsesGenerated,
                totalAmmoWasted = ammoUsedA + ammoUsedB,
                hoursPassed = hoursToWait,
                ScavengedAmmoIds = scavenged,
                summaryText = $"After {hoursToWait:F0} hours, {state.winningFaction} prevailed. {state.totalCorpsesGenerated} corpses lie scattered among depleted casing."
            };

            OnSkirmishResolved?.Invoke(state, outcome);
            return outcome;
        }

        /// <summary>
        /// Field-scavenge exclusive ammo from military/rebel skirmish winners.
        /// Civilian/bandit winners drop craftable common loads only.
        /// </summary>
        public static List<string> RollScavengedAmmo(SkirmishState state, System.Random rng)
        {
            var list = new List<string>();
            if (state == null) return list;
            rng ??= AtomicWar._Game.Utilities.SeededRandom.CreateFixed("skirmishencounter");

            string winner = state.winningFaction ?? string.Empty;
            if (string.Equals(winner, "Mutual Destruction", StringComparison.Ordinal)
                || string.Equals(winner, "none", StringComparison.Ordinal)
                || string.Equals(winner, "player_involved", StringComparison.Ordinal))
            {
                // Both sides bled — mix of exclusive casings left behind.
                list.AddRange(Item_AmmoTypes.RollFactionAmmoLoot(
                    AmmoFactionSource.MilitaryForces, rng, count: 1, preferApApi: true));
                list.AddRange(Item_AmmoTypes.RollFactionAmmoLoot(
                    AmmoFactionSource.RebelForces, rng, count: 1, preferApApi: true));
                return list;
            }

            var source = Item_AmmoTypes.MapFactionId(winner);
            int count = state.totalCorpsesGenerated >= 4 ? 2 : 1;
            bool preferAp = Item_AmmoTypes.IsMilitaryOrRebelSource(source);
            list.AddRange(Item_AmmoTypes.RollFactionAmmoLoot(source, rng, count, preferApApi: preferAp));
            return list;
        }

        /// <summary>
        /// Field-scavenge world gear / extremely rare loose attachments from skirmish winners.
        /// Attachments are almost never loose — usually already fitted on faction weapons.
        /// </summary>
        public static List<WorldLootRoll> RollScavengedWorldLoot(SkirmishState state, System.Random rng)
        {
            if (state == null) return new List<WorldLootRoll>();
            rng ??= AtomicWar._Game.Utilities.SeededRandom.CreateFixed("skirmishencounter");
            return Item_WorldCatalog.RollScavengedWorldLoot(
                state.winningFaction,
                rng,
                corpseCount: state.totalCorpsesGenerated);
        }

        /// <summary>Ids only — convenience for hosts that ignore stack amounts.</summary>
        public static List<string> RollScavengedWorldLootIds(SkirmishState state, System.Random rng)
        {
            var rolls = RollScavengedWorldLoot(state, rng);
            var ids = new List<string>(rolls.Count);
            for (int i = 0; i < rolls.Count; i++)
            {
                if (!string.IsNullOrEmpty(rolls[i].ItemId))
                    ids.Add(rolls[i].ItemId);
            }
            return ids;
        }

        /// <summary>
        /// Background skirmish fire uses ResolveHit: armored factions (military)
        /// soak soft ammo, so kill chance drops when the shooter is treated as soft-lead
        /// and the target as armored. Returns hit probability 0..1.
        /// </summary>
        public static float ComputeSkirmishHitChance(
            string shooterFactionId,
            string targetFactionId,
            Item_AmmoTypes ammo = null)
        {
            ammo ??= new Item_AmmoTypes();
            float targetArmor = Item_AmmoTypes.GetFactionArmor(targetFactionId);
            // Shooter load: military/rebel use AP baseline; raiders use soft FMJ.
            string loadId = Item_AmmoTypes.IsMilitaryOrRebelSource(Item_AmmoTypes.MapFactionId(shooterFactionId))
                ? "ammo_556x45_ap"
                : "ammo_9x19_fmj";
            float baseDmg = 14f;
            if (Item_AmmoTypes.TryGetLoad(loadId, out var load))
                baseDmg = load.BaseDamage;
            var hit = ammo.ResolveHit(loadId, baseDmg, targetArmor);
            // Map terminal damage into a 0.15–0.55 hit chance band (was flat 0.35).
            float ratio = hit.FinalDamage / Mathf.Max(1f, baseDmg);
            return Mathf.Clamp(0.15f + ratio * 0.30f, 0.12f, 0.55f);
        }

        public SkirmishState GetSkirmish(string locationId)
        {
            if (string.IsNullOrEmpty(locationId)) return null;
            _activeSkirmishes.TryGetValue(locationId, out var state);
            return state;
        }
    }
}
