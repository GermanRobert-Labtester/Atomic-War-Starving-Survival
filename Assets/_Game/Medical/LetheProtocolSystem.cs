using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Medical
{
    public struct WakingSicknessEvent
    {
        public float ReservoirLevel;
        public int AffectedCount;

        public WakingSicknessEvent(float reservoirLevel, int affectedCount)
        {
            ReservoirLevel = reservoirLevel;
            AffectedCount = affectedCount;
        }
    }

    /// <summary>
    /// Expansion IV — Chapter 39 & Chapter 42.1 The Lethe Protocol & The Waking Sickness.
    /// Tracks the secondary chemical reservoir hooked to the water purifier.
    /// When the pre-war amnestic reservoir depletes below 20%, survivors lose chemical pacification
    /// and experience Affliction_Hyperthymesia (violent return of Day 0 trauma).
    /// </summary>
    public class LetheProtocolSystem
    {
        public const float MaxReservoirLevel = 100f;
        public const float CriticalRedLineLevel = 20f;

        public const string Affliction_Hyperthymesia = "affliction_hyperthymesia";
        public const string Affliction_LiverFailure = "affliction_liver_failure";
        public const string Affliction_Dementia = "affliction_dementia";
        public const string Trait_HardenedSoul = "trait_hardened_soul";
        public const string Trait_Lobotomized = "trait_lobotomized";

        private float _reservoirLevel = 100f;
        private bool _wakingSicknessActive;
        private bool _isEmbraced;
        private bool _isSynthesizedLieActive;

        private NeedsSystem _needsSystem;
        private MentalBreakSystem _mentalBreakSystem;
        private readonly System.Random _rng;

        public float ReservoirLevel => _reservoirLevel;
        public bool IsWakingSicknessActive => _wakingSicknessActive;
        public bool IsEmbraced => _isEmbraced;
        public bool IsSynthesizedLieActive => _isSynthesizedLieActive;

        public event Action<float> OnReservoirLevelChanged;
        public event Action OnWakingSicknessStarted;
        public event Action<WakingSicknessEvent> OnWakingSicknessEventBus;
        public event Action<Survivor> OnLobotomyPerformed;

        public LetheProtocolSystem(System.Random rng = null)
        {
            _rng = rng ?? new System.Random(4000);
        }

        public void BindDependencies(NeedsSystem needsSystem, MentalBreakSystem mentalBreakSystem)
        {
            _needsSystem = needsSystem;
            _mentalBreakSystem = mentalBreakSystem;
        }

        /// <summary>
        /// Depletes chemical reservoir as water purifier runs.
        /// </summary>
        public void ConsumeAmnesticDose(float waterVolumePurified, IReadOnlyList<Survivor> survivors)
        {
            if (_isEmbraced) return;

            float depletion = waterVolumePurified * 0.15f;
            _reservoirLevel = Mathf.Max(0f, _reservoirLevel - depletion);
            OnReservoirLevelChanged?.Invoke(_reservoirLevel);

            if (_reservoirLevel <= CriticalRedLineLevel && !_wakingSicknessActive)
            {
                TriggerWakingSickness(survivors);
            }

            if (_isSynthesizedLieActive && survivors != null)
            {
                // Slow liver/dementia toxicity over time from synthetic lie substitute
                for (int i = 0; i < survivors.Count; i++)
                {
                    var sv = survivors[i];
                    if (sv == null || !sv.IsAlive) continue;

                    if (_rng.NextDouble() < 0.05)
                    {
                        if (!sv.HasTrait(Affliction_LiverFailure))
                            sv.Traits.Add(Affliction_LiverFailure);
                    }
                }
            }
        }

        /// <summary>
        /// Triggers the sudden violent return of repressed Day 0 trauma across the shelter.
        /// </summary>
        public void TriggerWakingSickness(IReadOnlyList<Survivor> survivors)
        {
            _wakingSicknessActive = true;
            OnWakingSicknessStarted?.Invoke();

            int affected = 0;
            if (survivors != null)
            {
                for (int i = 0; i < survivors.Count; i++)
                {
                    var sv = survivors[i];
                    if (sv == null || !sv.IsAlive) continue;

                    if (!sv.HasTrait(Trait_HardenedSoul) && !sv.HasTrait(Trait_Lobotomized))
                    {
                        if (!sv.HasTrait(Affliction_Hyperthymesia))
                            sv.Traits.Add(Affliction_Hyperthymesia);

                        if (_needsSystem != null)
                        {
                            _needsSystem.Modify(sv, NeedKind.Morale, -40f);
                        }

                        affected++;
                    }
                }
            }

            OnWakingSicknessEventBus?.Invoke(new WakingSicknessEvent(_reservoirLevel, affected));
        }

        /// <summary>
        /// Choice 1: Synthesize the Lie using item_scopolamine_root and item_lithium_salts.
        /// </summary>
        public void SynthesizeLie(float syrupDosesAdded)
        {
            _isSynthesizedLieActive = true;
            _wakingSicknessActive = false;
            _reservoirLevel = Mathf.Min(MaxReservoirLevel, _reservoirLevel + syrupDosesAdded * 25f);
            OnReservoirLevelChanged?.Invoke(_reservoirLevel);
        }

        /// <summary>
        /// Choice 2: Embrace the Waking. Stop dosing. Survivors who survive gain trait_hardened_soul.
        /// </summary>
        public void EmbraceTheWaking(IReadOnlyList<Survivor> survivors)
        {
            _isEmbraced = true;
            _wakingSicknessActive = false;
            _reservoirLevel = 0f;
            OnReservoirLevelChanged?.Invoke(_reservoirLevel);

            if (survivors != null)
            {
                for (int i = 0; i < survivors.Count; i++)
                {
                    var sv = survivors[i];
                    if (sv == null || !sv.IsAlive) continue;

                    if (!sv.HasTrait(Trait_HardenedSoul))
                    {
                        sv.Traits.Add(Trait_HardenedSoul);
                    }
                }
            }
        }

        /// <summary>
        /// Choice 3: The Lobotomy Option. Autodoc prefrontal lobotomy turning volatile survivor into docile drone.
        /// </summary>
        public bool PerformLobotomy(Survivor target)
        {
            if (target == null || !target.IsAlive) return false;

            if (!target.HasTrait(Trait_Lobotomized))
            {
                target.Traits.Add(Trait_Lobotomized);
            }

            // Remove emotional / social / belief traits and breaks
            target.currentMentalBreakId = null;
            target.Traits.Remove(Affliction_Hyperthymesia);
            target.Traits.Remove("trait_artifact_reverence");
            target.Traits.Remove("trait_agoraphobia_severe");

            // High stat boost for docility & labor
            target.BaseMaxStamina = 150f;
            target.ProgressionSurvivalBonus = 0.5f;

            OnLobotomyPerformed?.Invoke(target);
            return true;
        }

        public LetheSave GetState()
        {
            return new LetheSave
            {
                ReservoirLevel = _reservoirLevel,
                WakingSicknessActive = _wakingSicknessActive,
                IsEmbraced = _isEmbraced,
                IsSynthesizedLieActive = _isSynthesizedLieActive
            };
        }

        public void RestoreState(LetheSave save)
        {
            if (save == null) return;
            _reservoirLevel = save.ReservoirLevel;
            _wakingSicknessActive = save.WakingSicknessActive;
            _isEmbraced = save.IsEmbraced;
            _isSynthesizedLieActive = save.IsSynthesizedLieActive;
            OnReservoirLevelChanged?.Invoke(_reservoirLevel);
        }
    }

    [Serializable]
    public class LetheSave
    {
        public float ReservoirLevel;
        public bool WakingSicknessActive;
        public bool IsEmbraced;
        public bool IsSynthesizedLieActive;
    }
}
