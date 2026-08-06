using System;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    public enum CrawlspaceResult
    {
        Success,
        CaveIn,
        Bitten,
        FatalCaveIn
    }

    [Serializable]
    public class CrawlspaceState
    {
        public string actionId = "action_crawlspace";
        public float caveInChance = 0.20f;
        public float biteChance = 0.15f;
        public int lootMin = 1;
        public int lootMax = 5;
    }

    public class Action_Crawlspace
    {
        public event Action<string, int> OnLootRetrieved;
        public event Action<string> OnCaveIn;
        public event Action<string, string> OnBitten;

        private CrawlspaceState _state;

        private static readonly string[] CreatureTypes = new string[]
        {
            "mutant_rat",
            "irradiated_snake",
            "feral_cat"
        };

        public Action_Crawlspace(CrawlspaceState state = null)
        {
            _state = state ?? new CrawlspaceState();
        }

        public string ActionId => _state.actionId;

        /// <summary>
        /// Sends a child into a crawlspace to retrieve loot adults can't reach.
        /// High risk: cave-ins and creature bites. Child-exclusive action.
        /// </summary>
        public CrawlspaceResult SendChild(string childId, System.Random rng)
        {
            if (string.IsNullOrEmpty(childId))
            {
                Debug.LogWarning("[Action_Crawlspace] SendChild called with null/empty childId.");
                return CrawlspaceResult.CaveIn;
            }

            if (rng == null)
            {
                rng = new System.Random();
            }

            double caveInRoll = rng.NextDouble();
            double biteRoll = rng.NextDouble();

            // Check cave-in first (most dangerous outcome)
            if (caveInRoll < _state.caveInChance)
            {
                // Of cave-ins, 30% are fatal
                bool fatal = rng.NextDouble() < 0.30;
                OnCaveIn?.Invoke(childId);

                return fatal ? CrawlspaceResult.FatalCaveIn : CrawlspaceResult.CaveIn;
            }

            // Check creature bite
            if (biteRoll < _state.biteChance)
            {
                string creature = CreatureTypes[rng.Next(CreatureTypes.Length)];
                OnBitten?.Invoke(childId, creature);
                return CrawlspaceResult.Bitten;
            }

            // Success: retrieve loot
            int lootCount = rng.Next(_state.lootMin, _state.lootMax + 1);
            OnLootRetrieved?.Invoke(childId, lootCount);

            return CrawlspaceResult.Success;
        }

        public CrawlspaceState CaptureState()
        {
            return new CrawlspaceState
            {
                actionId = _state.actionId,
                caveInChance = _state.caveInChance,
                biteChance = _state.biteChance,
                lootMin = _state.lootMin,
                lootMax = _state.lootMax
            };
        }

        public void RestoreState(CrawlspaceState state)
        {
            _state = state ?? new CrawlspaceState();
        }
    }
}
