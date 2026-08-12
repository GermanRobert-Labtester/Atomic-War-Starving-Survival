using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Factions
{
    /// <summary>
    /// Scavenger Refuge System — desperate civilian survivor clusters seek
    /// temporary shelter, medical aid, or clean water, forcing hard choices
    /// regarding limited bunker capacity.
    ///
    /// Plain C#, save-safe. Works with Shelter bed capacity.
    /// </summary>
    public class ScavengerRefugeSystem
    {
        public const int MinRefugees = 2;
        public const int MaxRefugees = 5;
        public const int RefugeEventIntervalDays = 20;
        public const int RefugeStartDay = 10;
        public const float AcceptMoraleBonus = 20f;
        public const float RefuseMoralePenalty = -10f;
        public const float PartialAcceptMoraleBonus = 10f;
        public const int FoodCostPerRefugeePerDay = 2;
        public const int WaterCostPerRefugeePerDay = 1;

        public event Action<int> OnRefugeesArrived;
        // count
        public event Action<int> OnRefugeesAccepted;
        public event Action OnRefugeesRefused;
        public event Action<int, int> OnPartialAcceptance;
        // accepted, total

        private int _lastRefugeDay = -1;
        private int _totalRefugeesAccepted;

        public int TotalRefugeesAccepted => _totalRefugeesAccepted;

        public void Tick(int currentDay, System.Random rng,
            Func<int> getAvailableBeds)
        {
            if (currentDay < RefugeStartDay) return;
            if (currentDay - _lastRefugeDay < RefugeEventIntervalDays) return;

            _lastRefugeDay = currentDay;
            int count = MinRefugees + rng.Next(MaxRefugees - MinRefugees + 1);
            OnRefugeesArrived?.Invoke(count);
        }

        /// <summary>
        /// Accept all refugees (up to bed capacity).
        /// </summary>
        public int AcceptAll(int count, int availableBeds)
        {
            int accepted = Math.Min(count, availableBeds);
            _totalRefugeesAccepted += accepted;
            OnRefugeesAccepted?.Invoke(accepted);
            return accepted;
        }

        /// <summary>
        /// Refuse all refugees.
        /// </summary>
        public void RefuseAll()
        {
            OnRefugeesRefused?.Invoke();
        }

        /// <summary>
        /// Accept partial: take some, give supplies to rest.
        /// </summary>
        public int AcceptPartial(int count, int acceptCount)
        {
            int accepted = Math.Min(acceptCount, count);
            _totalRefugeesAccepted += accepted;
            OnPartialAcceptance?.Invoke(accepted, count);
            return accepted;
        }
    }
}
