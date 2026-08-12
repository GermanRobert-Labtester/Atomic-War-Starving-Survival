using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Factions
{
    /// <summary>
    /// Ash Sign Cult System — an apocalyptic cult worshipping fallout rain
    /// offers specialized radiation-resistant herbal remedies, but demands
    /// dangerous ritual acts inside irradiated hotspots.
    ///
    /// Extends System_CultLeash. Plain C#, save-safe.
    /// </summary>
    public class AshSignCultSystem
    {
        public const string RemedyItemId = "rad_resistant_herbal_remedy";
        public const float RemedyDoseReduction = 15f;
        public const float RitualDurationHours = 24f;
        public const float RitualSurvivalChance = 0.50f;
        public const float RitualRadiationExposure = 80f;
        public const float CultHostilityIncrease = 10f;
        public const float CultRelationshipGain = 15f;
        public const int RitualOfferIntervalDays = 10;
        public const int RitualStartDay = 15;

        public event Action<Survivor> OnRitualOffered;
        public event Action<Survivor, bool> OnRitualCompleted;
        // sv, survived
        public event Action OnRemedyCreated;

        private int _lastRitualOfferDay = -1;
        private bool _ritualActive;
        private string _ritualSurvivorId;

        public bool IsRitualActive => _ritualActive;

        public void Tick(int currentDay, IReadOnlyList<Survivor> survivors,
            System.Random rng)
        {
            if (currentDay < RitualStartDay) return;
            if (_ritualActive) return;
            if (currentDay - _lastRitualOfferDay < RitualOfferIntervalDays) return;

            _lastRitualOfferDay = currentDay;
            if (survivors == null || survivors.Count == 0) return;

            // Pick a random healthy survivor for the ritual offer
            var healthy = new List<Survivor>();
            for (int i = 0; i < survivors.Count; i++)
            {
                if (survivors[i] != null && survivors[i].IsAlive &&
                    survivors[i].State != SurvivorState.Incapacitated)
                    healthy.Add(survivors[i]);
            }
            if (healthy.Count == 0) return;

            var chosen = healthy[rng.Next(healthy.Count)];
            OnRitualOffered?.Invoke(chosen);
        }

        /// <summary>
        /// Accept the ritual: survivor spends 24h in high-rad zone.
        /// Returns true if survivor lives.
        /// </summary>
        public bool AcceptRitual(Survivor sv, System.Random rng,
            Action<Survivor, float> applyRadiationDose,
            Action<string> addItemToInventory)
        {
            if (sv == null || !sv.IsAlive) return false;

            _ritualActive = true;
            _ritualSurvivorId = sv.Id;

            // Apply radiation dose from ritual
            applyRadiationDose?.Invoke(sv, RitualRadiationExposure);

            // Survival roll
            float roll = (float)(rng?.NextDouble() ?? 0.5);
            bool survived = roll < RitualSurvivalChance;

            if (survived)
            {
                addItemToInventory?.Invoke(RemedyItemId);
                OnRemedyCreated?.Invoke();
            }

            _ritualActive = false;
            OnRitualCompleted?.Invoke(sv, survived);
            return survived;
        }

        /// <summary>
        /// Refuse the ritual: cult hostility increases.
        /// </summary>
        public void RefuseRitual()
        {
            _ritualActive = false;
            // Hostility handled by faction relationship system
        }

        /// <summary>
        /// Apply the herbal remedy to reduce radiation dose.
        /// </summary>
        public bool ApplyRemedy(Survivor sv, Action<Survivor, float> reduceRadiationDose,
            Action<string> consumeItem)
        {
            if (sv == null || !sv.IsAlive) return false;
            consumeItem?.Invoke(RemedyItemId);
            reduceRadiationDose?.Invoke(sv, RemedyDoseReduction);
            return true;
        }
    }
}
