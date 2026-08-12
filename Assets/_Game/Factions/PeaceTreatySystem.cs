using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Factions
{
    /// <summary>
    /// Peace Treaty System (#78) — brokering a historic ceasefire between
    /// the Garrison military remnants and the local Militia factions
    /// through diplomatic trade and strategic concessions.
    ///
    /// Plain C#, save-safe.
    /// </summary>
    public class PeaceTreatySystem
    {
        public const float MinGarrisonStandingForTreaty = 60f;
        public const float MinMilitiaStandingForTreaty = 60f;
        public const int FoodConcessionAmount = 20;
        public const int AmmoConcessionAmount = 50;
        public const int MedicalConcessionAmount = 5;
        public const float TreatyMoraleBonus = 25f;
        public const float TreatyDailyMoraleBonus = 2f;

        public event Action OnTreatyNegotiationsStarted;
        public event Action<string> OnConcessionDemanded;
        // concession type: food/ammo/medical
        public event Action OnTreatySigned;
        public event Action OnTreatyBroken;
        public event Action<float> OnTreatyMoraleBonusApplied;

        private bool _negotiationsActive;
        private bool _treatySigned;
        private int _concessionsMade;
        private int _concessionsRequired = 3;

        public bool IsTreatySigned => _treatySigned;
        public bool IsNegotiating => _negotiationsActive;

        public bool StartNegotiations(float garrisonStanding, float militiaStanding)
        {
            if (_treatySigned || _negotiationsActive) return false;
            if (garrisonStanding < MinGarrisonStandingForTreaty) return false;
            if (militiaStanding < MinMilitiaStandingForTreaty) return false;

            _negotiationsActive = true;
            _concessionsMade = 0;
            OnTreatyNegotiationsStarted?.Invoke();
            return true;
        }

        public string GetNextConcessionDemand()
        {
            string[] demands = { "food", "ammo", "medical" };
            int index = _concessionsMade % demands.Length;
            string demand = demands[index];
            OnConcessionDemanded?.Invoke(demand);
            return demand;
        }

        public bool MakeConcession(string concessionType,
            ref int foodAvailable, ref int ammoAvailable, ref int medicalAvailable)
        {
            if (!_negotiationsActive) return false;

            switch (concessionType)
            {
                case "food":
                    if (foodAvailable < FoodConcessionAmount) return false;
                    foodAvailable -= FoodConcessionAmount;
                    break;
                case "ammo":
                    if (ammoAvailable < AmmoConcessionAmount) return false;
                    ammoAvailable -= AmmoConcessionAmount;
                    break;
                case "medical":
                    if (medicalAvailable < MedicalConcessionAmount) return false;
                    medicalAvailable -= MedicalConcessionAmount;
                    break;
                default:
                    return false;
            }

            _concessionsMade++;
            if (_concessionsMade >= _concessionsRequired)
            {
                _treatySigned = true;
                _negotiationsActive = false;
                OnTreatySigned?.Invoke();
                OnTreatyMoraleBonusApplied?.Invoke(TreatyMoraleBonus);
            }

            return true;
        }

        public float GetDailyMoraleBonus()
        {
            return _treatySigned ? TreatyDailyMoraleBonus : 0f;
        }
    }
}
