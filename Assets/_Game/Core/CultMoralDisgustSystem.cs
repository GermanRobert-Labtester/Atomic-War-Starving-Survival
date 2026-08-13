using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;
using Ashfall.Core.Journal;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Doomsday Cult moral tracking (Prompt #78). "The Cult of the Glow" already
    /// inverts the economy (they pay premium for IrradiatedWater/ContaminatedFood).
    /// This system tracks the moral disgust of rational survivors when the player
    /// enables their mass-suicide by trading. Trading with the Cult lowers Morale
    /// of non-Fatalist/non-Sociopath survivors. Save/load safe. Plain C#.
    /// </summary>
    public class CultMoralDisgustSystem
    {
        /// <summary>Morale penalty per trade with the Cult. Applied to rational survivors.</summary>
        public const float TradeDisgustMoralePenalty = 8f;

        /// <summary>Morale penalty when the player sells them IrradiatedWater (enabling ascension).</summary>
        public const float IrradiatedWaterDisgustPenalty = 12f;

        /// <summary>Morale penalty when the player sells them ContaminatedFood.</summary>
        public const float ContaminatedFoodDisgustPenalty = 10f;

        /// <summary>Traits immune to moral disgust.</summary>
        public static readonly HashSet<string> ImmuneTraits = new HashSet<string>
        {
            "Sociopath", "Fatalist"
        };

        /// <summary>Total trades made with the Cult this campaign.</summary>
        private int _totalCultTrades;

        /// <summary>Total irradiated water units sold to the Cult.</summary>
        private float _totalIrradiatedWaterSold;

        /// <summary>Whether a "Mass Ascension" event has been triggered (50+ irradiated water sold).</summary>
        private bool _massAscensionTriggered;

        // -- Events --
        public event Action<float> OnCultTrade; // morale penalty applied
        public event Action OnMassAscension;

        public int TotalCultTrades => _totalCultTrades;
        public float TotalIrradiatedWaterSold => _totalIrradiatedWaterSold;
        public bool MassAscensionTriggered => _massAscensionTriggered;

        private NeedsSystem _needsSystem;
        public void SetNeedsSystem(NeedsSystem ns) => _needsSystem = ns;

        public CultMoralDisgustSystem() { }

        /// <summary>
        /// Record a trade with the Cult. Applies moral disgust to rational survivors.
        /// </summary>
        public float RecordCultTrade(string itemId, float amount,
            IReadOnlyList<Survivors.Survivor> survivors)
        {
            _totalCultTrades++;
            float penalty = TradeDisgustMoralePenalty;

            if (itemId == "irradiated_water")
            {
                penalty = IrradiatedWaterDisgustPenalty;
                _totalIrradiatedWaterSold += amount;

                if (_totalIrradiatedWaterSold >= 50f && !_massAscensionTriggered)
                {
                    _massAscensionTriggered = true;
                    OnMassAscension?.Invoke();
                }
            }
            else if (itemId == "contaminated_food")
            {
                penalty = ContaminatedFoodDisgustPenalty;
            }

            // Apply moral disgust to rational survivors.
            if (survivors != null)
            {
                for (int i = 0; i < survivors.Count; i++)
                {
                    var sv = survivors[i];
                    if (sv == null || !sv.IsAlive) continue;

                    // Sociopaths (trait_sociopath) and Fatalists don't care.
                    // Fatalist is a genuine RiskBias value (assigned by LaborCampSystem);
                    // Sociopath is a TRAIT, not a RiskBias — the old RiskBias check
                    // never matched (see EmpathSystem for the same correction).
                    if (sv.HasTrait(PersonalQuestSystem.SociopathId)
                        || sv.RiskBias == RiskBiasTrait.Fatalist)
                        continue;

                    if (_needsSystem != null)
                        _needsSystem.Modify(sv, NeedKind.Morale, -penalty);
                    else
                        sv.Needs.Morale = Mathf.Clamp(
                            sv.Needs.Morale - penalty, 0f, 100f);
                }
            }

            OnCultTrade?.Invoke(penalty);
            return penalty;
        }

        /// <summary>
        /// Whether the player has been trading heavily with the Cult (affects faction
        /// relations with rational factions like MilitaryRemnants).
        /// </summary>
        public bool IsCultCollaborator => _totalCultTrades >= 5;

        // -----------------------------------------------------------------
        // Save / Load
        // -----------------------------------------------------------------

        public CultMoralSave CaptureState()
        {
            return new CultMoralSave
            {
                TotalCultTrades = _totalCultTrades,
                TotalIrradiatedWaterSold = _totalIrradiatedWaterSold,
                MassAscensionTriggered = _massAscensionTriggered
            };
        }

        public void RestoreState(CultMoralSave save)
        {
            if (save == null)
            {
                _totalCultTrades = 0;
                _totalIrradiatedWaterSold = 0f;
                _massAscensionTriggered = false;
                return;
            }
            _totalCultTrades = save.TotalCultTrades;
            _totalIrradiatedWaterSold = save.TotalIrradiatedWaterSold;
            _massAscensionTriggered = save.MassAscensionTriggered;
        }
    }

    [Serializable]
    public class CultMoralSave
    {
        public int TotalCultTrades;
        public float TotalIrradiatedWaterSold;
        public bool MassAscensionTriggered;
    }
}
