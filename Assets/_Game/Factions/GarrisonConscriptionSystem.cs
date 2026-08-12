using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Factions
{
    /// <summary>
    /// Garrison Conscription System — a military remnant faction demands food
    /// tributes and young survivors for military service in exchange for
    /// defensive artillery support against encroaching raiders.
    ///
    /// Extends System_GarrisonComplianceLedger. Plain C#, save-safe.
    /// </summary>
    public class GarrisonConscriptionSystem
    {
        public const int ConscriptionIntervalDays = 15;
        public const int ConscriptionStartDay = 20;
        public const int MaxSurvivorAge = 30;
        public const int FoodTributeAmount = 5;
        public const int DoubleFoodTributeAmount = 10;
        public const float RefuseRelationshipPenalty = -50f;
        public const int RefusePunitiveRaidDays = 3;
        public const string ArtillerySupportBuffId = "garrison_artillery_support";

        public event Action<int, int> OnConscriptionDemand;
        // foodAmount, survivorsRequested
        public event Action<string, int> OnConscriptionResolved;
        // outcome, survivorsSent
        public event Action OnPunitiveRaidTriggered;

        private int _lastConscriptionDay = -1;
        private int _survivorsContributed;
        private bool _hasArtillerySupport;

        public bool HasArtillerySupport => _hasArtillerySupport;

        /// <summary>
        /// Tick — check if conscription demand is due.
        /// </summary>
        public void Tick(int currentDay, IReadOnlyList<Survivor> survivors,
            System.Random rng)
        {
            if (currentDay < ConscriptionStartDay) return;
            if (currentDay - _lastConscriptionDay < ConscriptionIntervalDays) return;
            _lastConscriptionDay = currentDay;

            int youngCount = CountYoungSurvivors(survivors);
            int requested = Math.Min(1, youngCount);
            OnConscriptionDemand?.Invoke(FoodTributeAmount, requested);
        }

        /// <summary>
        /// Accept conscription: lose 1 young survivor + food tribute, gain artillery support.
        /// </summary>
        public bool AcceptConscription(IReadOnlyList<Survivor> survivors,
            Action<Survivor> removeSurvivor, Action<string, int> removeFood)
        {
            var young = FindYoungSurvivor(survivors);
            if (young == null) return false;

            removeSurvivor?.Invoke(young);
            removeFood?.Invoke("food_ration", FoodTributeAmount);
            _survivorsContributed++;
            _hasArtillerySupport = true;
            OnConscriptionResolved?.Invoke("accepted", 1);
            return true;
        }

        /// <summary>
        /// Negotiate: double food tribute, keep survivor.
        /// </summary>
        public bool NegotiateConscription(Action<string, int> removeFood)
        {
            removeFood?.Invoke("food_ration", DoubleFoodTributeAmount);
            _hasArtillerySupport = true;
            OnConscriptionResolved?.Invoke("negotiated", 0);
            return true;
        }

        /// <summary>
        /// Refuse: lose relationship, risk punitive raid in 3 days.
        /// </summary>
        public void RefuseConscription()
        {
            _hasArtillerySupport = false;
            OnConscriptionResolved?.Invoke("refused", 0);
            OnPunitiveRaidTriggered?.Invoke();
        }

        /// <summary>
        /// Auto-repel one raid if artillery support is active.
        /// Returns true if a raid was auto-repelled.
        /// </summary>
        public bool TryAutoRepelRaid()
        {
            if (!_hasArtillerySupport) return false;
            _hasArtillerySupport = false; // one use
            return true;
        }

        private int CountYoungSurvivors(IReadOnlyList<Survivor> survivors)
        {
            int count = 0;
            if (survivors == null) return count;
            for (int i = 0; i < survivors.Count; i++)
            {
                if (survivors[i] != null && survivors[i].IsAlive &&
                    survivors[i].Age <= MaxSurvivorAge)
                    count++;
            }
            return count;
        }

        private Survivor FindYoungSurvivor(IReadOnlyList<Survivor> survivors)
        {
            if (survivors == null) return null;
            for (int i = 0; i < survivors.Count; i++)
            {
                if (survivors[i] != null && survivors[i].IsAlive &&
                    survivors[i].Age <= MaxSurvivorAge)
                    return survivors[i];
            }
            return null;
        }
    }
}
