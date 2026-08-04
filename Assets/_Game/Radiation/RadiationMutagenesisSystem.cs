using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Radiation
{
    /// <summary>
    /// Radiation Mutagenesis — The Slow Burn (Prompt #60). Expands ChronicIllness
    /// into a multi-stage cascade based on lifetime radiation exposure.
    /// Stage 1: Hair Loss (morale drop).
    /// Stage 2: Cataracts (scavenging fog of war increases, combat/survey stats drop).
    /// Stage 3: Cellular Breakdown (immune system fails, all minor wounds infect).
    /// Feels like creeping, irreversible decay. Save/load safe. Plain C#.
    /// </summary>
    public class RadiationMutagenesisSystem
    {
        /// <summary>Lifetime rads at which Stage 1 (Hair Loss) triggers.</summary>
        public const float Stage1HairLossThreshold = 200f;

        /// <summary>Lifetime rads at which Stage 2 (Cataracts) triggers.</summary>
        public const float Stage2CataractsThreshold = 500f;

        /// <summary>Lifetime rads at which Stage 3 (Cellular Breakdown) triggers.</summary>
        public const float Stage3CellularThreshold = 900f;

        /// <summary>Morale penalty when hair loss first occurs (one-time).</summary>
        public const float HairLossMoralePenalty = 15f;

        /// <summary>Scavenging fog-of-war multiplier for Cataracts (higher = less visible).</summary>
        public const float CataractsFogMultiplier = 2f;

        /// <summary>Survey/combat effectiveness multiplier for Cataracts.</summary>
        public const float CataractsSurveyMultiplier = 0.6f;

        /// <summary>Chance per wound that it becomes infected with Cellular Breakdown.</summary>
        public const float CellularBreakdownInfectionChance = 0.8f;

        /// <summary>Health drain per hour from Cellular Breakdown.</summary>
        public const float CellularBreakdownHealthDrainPerHour = 0.3f;

        /// <summary>Per-survivor current mutagenesis stage (0=none, 1=hairloss, 2=cataracts, 3=cellular).</summary>
        private readonly Dictionary<string, int> _stages = new Dictionary<string, int>();

        /// <summary>Survivors who have already suffered the one-time morale hit from hair loss.</summary>
        private readonly HashSet<string> _hairLossApplied = new HashSet<string>();

        // Affliction IDs used by this system (kept as strings to avoid Medical assembly ref).
        public const string HairLossAfflictionId = "hair_loss";
        public const string CellularBreakdownAfflictionId = "cellular_breakdown";

        private Func<float> _getPartyAverageRadiation;
        private Action<Survivors.Survivor, string> _inflictAffliction;

        // -- Events --
        public event Action<Survivors.Survivor, int> OnStageAdvanced; // survivor, newStage
        public event Action<Survivors.Survivor> OnHairLoss;
        public event Action<Survivors.Survivor> OnCataracts;
        public event Action<Survivors.Survivor> OnCellularBreakdown;

        public RadiationMutagenesisSystem() { }

        public void Bind(
            Func<float> getPartyAverageRadiation = null,
            Action<Survivors.Survivor, string> inflictAffliction = null)
        {
            _getPartyAverageRadiation = getPartyAverageRadiation;
            _inflictAffliction = inflictAffliction;
        }

        /// <summary>Current mutagenesis stage for a survivor (0-3).</summary>
        public int GetStage(string survivorId)
        {
            return !string.IsNullOrEmpty(survivorId) && _stages.TryGetValue(survivorId, out int s) ? s : 0;
        }

        public bool HasHairLoss(string survivorId) => GetStage(survivorId) >= 1;
        public bool HasCataracts(string survivorId) => GetStage(survivorId) >= 2;
        public bool HasCellularBreakdown(string survivorId) => GetStage(survivorId) >= 3;

        /// <summary>
        /// Evaluate and advance mutagenesis stage based on lifetime radiation exposure.
        /// Called daily from the game loop.
        /// </summary>
        public void Evaluate(IReadOnlyList<Survivors.Survivor> survivors)
        {
            if (survivors == null) return;

            for (int i = 0; i < survivors.Count; i++)
            {
                var sv = survivors[i];
                if (sv == null || !sv.IsAlive) continue;

                float lifetimeRads = sv.LifetimeRadiationExposure;
                int currentStage = GetStage(sv.Id);
                int newStage = currentStage;

                if (lifetimeRads >= Stage3CellularThreshold) newStage = 3;
                else if (lifetimeRads >= Stage2CataractsThreshold) newStage = 2;
                else if (lifetimeRads >= Stage1HairLossThreshold) newStage = 1;

                if (newStage <= currentStage) continue;

                // Advance to all intermediate stages.
                for (int stage = currentStage + 1; stage <= newStage; stage++)
                {
                    ApplyStage(sv, stage);
                }
                _stages[sv.Id] = newStage;
                OnStageAdvanced?.Invoke(sv, newStage);
            }
        }

        /// <summary>
        /// Tick: apply ongoing effects from active mutagenesis stages.
        /// </summary>
        public void Tick(float gameHours, IReadOnlyList<Survivors.Survivor> survivors)
        {
            if (gameHours <= 0f || survivors == null) return;

            for (int i = 0; i < survivors.Count; i++)
            {
                var sv = survivors[i];
                if (sv == null || !sv.IsAlive) continue;

                // Stage 3: continuous health drain from cellular breakdown.
                if (HasCellularBreakdown(sv.Id))
                {
                    sv.Needs.Health = Mathf.Clamp(
                        sv.Needs.Health - CellularBreakdownHealthDrainPerHour * gameHours,
                        0f, sv.MaxHealthCap);
                }
            }
        }

        /// <summary>
        /// Called when a survivor gets a new wound. With Cellular Breakdown,
        /// wounds auto-infect.
        /// </summary>
        public bool ShouldAutoInfect(Survivors.Survivor sv, System.Random rng = null)
        {
            if (sv == null || !HasCellularBreakdown(sv.Id)) return false;
            rng = rng ?? new System.Random();
            return rng.NextDouble() < CellularBreakdownInfectionChance;
        }

        private void ApplyStage(Survivors.Survivor sv, int stage)
        {
            switch (stage)
            {
                case 1:
                    if (_hairLossApplied.Add(sv.Id))
                    {
                        sv.Needs.Morale = Mathf.Clamp(
                            sv.Needs.Morale - HairLossMoralePenalty, 0f, 100f);
                        _inflictAffliction?.Invoke(sv, HairLossAfflictionId);
                        OnHairLoss?.Invoke(sv);
                    }
                    break;
                case 2:
                    // Cataracts reduce scavenging/survey yield.
                    if (!sv.ActiveChronicIllness.HasValue
                        || sv.ActiveChronicIllness.Value != Survivors.ChronicIllnessKind.RadiationCataracts)
                    {
                        sv.ActiveChronicIllness = Survivors.ChronicIllnessKind.RadiationCataracts;
                    }
                    OnCataracts?.Invoke(sv);
                    break;
                case 3:
                    // Cellular breakdown: immune collapse.
                    _inflictAffliction?.Invoke(sv, CellularBreakdownAfflictionId);
                    if (!sv.ActiveChronicIllness.HasValue
                        || sv.ActiveChronicIllness.Value != Survivors.ChronicIllnessKind.OrganFailure)
                    {
                        sv.ActiveChronicIllness = Survivors.ChronicIllnessKind.OrganFailure;
                    }
                    OnCellularBreakdown?.Invoke(sv);
                    break;
            }
        }

        // -----------------------------------------------------------------
        // Save / Load
        // -----------------------------------------------------------------

        public MutagenesisSave CaptureState()
        {
            var keys = new string[_stages.Count];
            var values = new int[_stages.Count];
            int i = 0;
            foreach (var kv in _stages) { keys[i] = kv.Key; values[i] = kv.Value; i++; }
            var hairLoss = new string[_hairLossApplied.Count];
            _hairLossApplied.CopyTo(hairLoss);
            return new MutagenesisSave
            {
                StageKeys = keys,
                StageValues = values,
                HairLossAppliedIds = hairLoss
            };
        }

        public void RestoreState(MutagenesisSave save)
        {
            _stages.Clear();
            _hairLossApplied.Clear();
            if (save == null) return;
            if (save.StageKeys != null)
            {
                for (int i = 0; i < save.StageKeys.Length; i++)
                {
                    if (string.IsNullOrEmpty(save.StageKeys[i])) continue;
                    int v = save.StageValues != null && i < save.StageValues.Length ? save.StageValues[i] : 0;
                    _stages[save.StageKeys[i]] = Mathf.Clamp(v, 0, 3);
                }
            }
            if (save.HairLossAppliedIds != null)
            {
                for (int i = 0; i < save.HairLossAppliedIds.Length; i++)
                {
                    if (!string.IsNullOrEmpty(save.HairLossAppliedIds[i]))
                        _hairLossApplied.Add(save.HairLossAppliedIds[i]);
                }
            }
        }
    }

    [Serializable]
    public class MutagenesisSave
    {
        public string[] StageKeys;
        public int[] StageValues;
        public string[] HairLossAppliedIds;
    }
}
