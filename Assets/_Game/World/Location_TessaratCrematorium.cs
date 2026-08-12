using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.World
{
    /// <summary>
    /// Expansion IV — The Tessarat Crematorium. The municipal crematorium ran out
    /// of natural gas on Day 4. Staff switched to burning wood. By Day 12, they
    /// were burning furniture. By Day 18, they stopped keeping names and just kept
    /// the fires fed to stop the plague. The ovens are still warm. The ash in the
    /// hoppers is not wood ash. It is high in calcium and phosphorus.
    /// </summary>
    public class Location_TessaratCrematorium
    {
        public const string LocationId = "location_tessarat_crematorium";
        public const string DisplayName = "The Tessarat Crematorium";
        public const int TravelHours = 2;  // 2.5h round trip
        public const int DangerLevel = 5;
        public const float BaseRads = 18f; // mSv/h

        // ── Unique loot ───────────────────────────────────────────────
        public const string Item_CrematoriumAsh = "crematorium_ash";
        public const string Item_WoodBlock = "wood_block";

        // ── Ash yield per visit ───────────────────────────────────────
        public const int AshYieldPerVisit = 4;
        public const int MaxAshVisits = 10; // After 10 visits, the hopper is empty

        // ── Moral consequences ────────────────────────────────────────
        public const float BotanistRevelationMorale = 10f; // Botanist realizes value
        public const float PriestObjectionMorale = -25f;   // Priest violently objects
        public const float ChefRefusalMorale = -15f;       // Chef refuses to cook
        public const float HydroponicsYieldBonus = 0.25f;  // +25% crop yield

        // ── Events ────────────────────────────────────────────────────
        public event Action<string> OnAshCollected;
        public event Action<string> OnBotanistRevelation;
        public event Action<string> OnPriestObjection;
        public event Action<string> OnChefRefusal;
        public event Action<string, float> OnAshAppliedToHydroponics;

        private readonly System.Random _rng;
        private int _visitsCompleted;
        private int _totalAshCollected;
        private bool _ashAppliedToHydroponics;
        private bool _priestObjected;
        private bool _chefRefused;

        public int VisitsCompleted => _visitsCompleted;
        public int TotalAshCollected => _totalAshCollected;
        public bool IsAshAppliedToHydroponics => _ashAppliedToHydroponics;
        public bool IsHopperExhausted => _visitsCompleted >= MaxAshVisits;

        public Location_TessaratCrematorium(System.Random rng = null)
        {
            _rng = rng ?? new System.Random(2222);
        }

        /// <summary>
        /// Collect ash from the crematorium hoppers. Each visit yields 4 bags
        /// of high-phosphorus bone ash fertilizer. The ash is not wood ash.
        /// </summary>
        public CrematoriumVisitResult VisitCrematorium(string scavengerId, string scavengerArchetype)
        {
            if (IsHopperExhausted)
                return new CrematoriumVisitResult { Success = false, HopperExhausted = true };

            _visitsCompleted++;
            int ashYield = AshYieldPerVisit;
            _totalAshCollected += ashYield;

            var result = new CrematoriumVisitResult
            {
                Success = true,
                AshYield = ashYield,
                Message = "The hoppers are warm. The ash is fine, white, and powdery. " +
                    "It smells like calcium and something sweeter. The the_botanist " +
                    "runs it through their fingers and goes quiet."
            };

            // Botanist recognizes the fertilizer value
            if (scavengerArchetype == "the_botanist")
            {
                result.BotanistPresent = true;
                OnBotanistRevelation?.Invoke(scavengerId);
            }

            OnAshCollected?.Invoke(scavengerId);
            return result;
        }

        /// <summary>
        /// Apply crematorium ash to the hydroponics bay. +25% crop yield.
        /// The the_priest will violently object. The the_chef will refuse.
        /// The math says the tomatoes are the only source of Vitamin C.
        /// </summary>
        public HydroponicsResult ApplyAshToHydroponics(
            string applierId,
            string priestId,
            string chefId)
        {
            _ashAppliedToHydroponics = true;

            var result = new HydroponicsResult
            {
                YieldBonus = HydroponicsYieldBonus,
                Message = "The tomatoes grow huge and red. The the_chef refuses " +
                    "to cook them. The math says they are the only source of Vitamin C. " +
                    "You eat the tomatoes."
            };

            // Priest objections
            if (!string.IsNullOrEmpty(priestId))
            {
                _priestObjected = true;
                result.PriestAffected = true;
                OnPriestObjection?.Invoke(priestId);
            }

            // Chef refusal
            if (!string.IsNullOrEmpty(chefId))
            {
                _chefRefused = true;
                result.ChefAffected = true;
                OnChefRefusal?.Invoke(chefId);
            }

            OnAshAppliedToHydroponics?.Invoke(applierId, HydroponicsYieldBonus);
            return result;
        }

        // ── Save / Load ───────────────────────────────────────────────

        public CrematoriumSave CaptureState()
        {
            return new CrematoriumSave
            {
                VisitsCompleted = _visitsCompleted,
                TotalAshCollected = _totalAshCollected,
                AshAppliedToHydroponics = _ashAppliedToHydroponics,
                PriestObjected = _priestObjected,
                ChefRefused = _chefRefused
            };
        }

        public void RestoreState(CrematoriumSave save)
        {
            _visitsCompleted = 0;
            _totalAshCollected = 0;
            _ashAppliedToHydroponics = false;
            _priestObjected = false;
            _chefRefused = false;
            if (save == null) return;
            _visitsCompleted = save.VisitsCompleted;
            _totalAshCollected = save.TotalAshCollected;
            _ashAppliedToHydroponics = save.AshAppliedToHydroponics;
            _priestObjected = save.PriestObjected;
            _chefRefused = save.ChefRefused;
        }
    }

    [Serializable]
    public class CrematoriumVisitResult
    {
        public bool Success;
        public bool HopperExhausted;
        public int AshYield;
        public bool BotanistPresent;
        public string Message;
    }

    [Serializable]
    public class HydroponicsResult
    {
        public float YieldBonus;
        public bool PriestAffected;
        public bool ChefAffected;
        public string Message;
    }

    [Serializable]
    public class CrematoriumSave
    {
        public int VisitsCompleted;
        public int TotalAshCollected;
        public bool AshAppliedToHydroponics;
        public bool PriestObjected;
        public bool ChefRefused;
    }
}
