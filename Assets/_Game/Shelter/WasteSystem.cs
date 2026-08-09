using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Shelter
{
    /// <summary>
    /// Waste Management & Hygiene (Prompt #50). Survivors generate waste daily.
    /// Without a functioning latrine, ambient Hygiene drops, drastically multiplying
    /// Phase-1 affliction chances (Dysentery, Cholera). Disposing outside requires an
    /// expedition tick and exposes the carrier to rads.
    /// Save/load safe. Plain C#.
    /// </summary>
    public class WasteSystem
    {
        public const string LatrineModuleId = "latrine";
        public const string SoapItemId = "soap";
        public const string BleachItemId = "bleach";

        /// <summary>Waste units generated per survivor per day.</summary>
        public const float WastePerSurvivorPerDay = 1f;

        /// <summary>Hygiene decay per hour per unmanaged waste unit.</summary>
        public const float HygieneDecayPerWastePerHour = 0.3f;

        /// <summary>Hygiene threshold below which affliction chances multiply.</summary>
        public const float LowHygieneThreshold = 40f;

        /// <summary>Disease chance multiplier when hygiene is at 0.</summary>
        public const float MaxDiseaseMultiplier = 4f;

        /// <summary>Latrine capacity in waste units before requiring disposal.</summary>
        public const float LatrineCapacity = 20f;

        /// <summary>Hygiene restored per soap use (bathing).</summary>
        public const float SoapHygieneRestore = 25f;

        /// <summary>Hygiene restored per bleach use (cleaning latrine).</summary>
        public const float BleachHygieneRestore = 15f;

        /// <summary>Waste removed by one bleach cleaning of the latrine.</summary>
        public const float BleachWasteRemoval = 10f;

        /// <summary>Radiation dose applied to the survivor dumping waste outside.</summary>
        public const float OutsideDisposalRadDose = 5f;

        /// <summary>Fatigue cost of dumping waste outside (hours of labor).</summary>
        public const float OutsideDisposalFatigue = 15f;

        /// <summary>Hours required for an outside disposal trip.</summary>
        public const float OutsideDisposalHours = 2f;

        private float _accumulatedWaste;
        private float _hygiene = 100f;
        private readonly System.Random _rng;
        private Func<Shelter> _getShelter;
        private Func<IReadOnlyList<Survivors.Survivor>> _getSurvivors;

        private NeedsSystem _needsSystem;
        public void SetNeedsSystem(NeedsSystem ns) => _needsSystem = ns;

        // -- Public state --
        public float AccumulatedWaste => _accumulatedWaste;
        public float Hygiene => _hygiene;
        public bool IsHygieneLow => _hygiene < LowHygieneThreshold;

        public float DiseaseChanceMultiplier
        {
            get
            {
                if (!IsHygieneLow) return 1f;
                float t = 1f - (_hygiene / LowHygieneThreshold);
                return 1f + t * (MaxDiseaseMultiplier - 1f);
            }
        }

        /// <summary>True when a latrine is installed and operational.</summary>
        public bool HasLatrine
        {
            get
            {
                var shelter = _getShelter?.Invoke();
                if (shelter == null) return false;
                var mod = shelter.GetModule(LatrineModuleId);
                return mod != null && mod.IsOperational;
            }
        }

        /// <summary>Current waste stored in the latrine (0 when no latrine).</summary>
        public float LatrineFill => HasLatrine ? _accumulatedWaste : 0f;

        /// <summary>True when the latrine is full and needs emptying.</summary>
        public bool IsLatrineFull => HasLatrine && _accumulatedWaste >= LatrineCapacity;

        // -- Events --
        public event Action<float, float> OnHygieneChanged;
        public event Action OnLatrineFull;
        public event Action<float> OnWasteDisposed;  // amount removed
        /// <summary>Fired when a survivor carries waste outside (rad exposure hook for GameBootstrap).</summary>
        public event Action<Survivors.Survivor> OnOutsideDisposal;

        public WasteSystem(System.Random rng = null)
        {
            _rng = rng ?? new System.Random(52);
        }

        public void Bind(
            Func<Shelter> getShelter,
            Func<IReadOnlyList<Survivors.Survivor>> getSurvivors = null)
        {
            _getShelter = getShelter;
            _getSurvivors = getSurvivors;
        }

        // -----------------------------------------------------------------
        // Tick
        // -----------------------------------------------------------------

        /// <summary>Generate waste from survivors, decay hygiene.</summary>
        public void Tick(float gameHours, int currentDay)
        {
            if (gameHours <= 0f) return;

            var survivors = _getSurvivors?.Invoke();
            if (survivors == null || survivors.Count == 0) return;

            // Waste generation: each living survivor produces waste daily.
            int livingCount = 0;
            for (int i = 0; i < survivors.Count; i++)
            {
                if (survivors[i] != null && survivors[i].IsAlive)
                    livingCount++;
            }

            float dailyWaste = livingCount * WastePerSurvivorPerDay * (gameHours / 24f);
            _accumulatedWaste += dailyWaste;

            // If no latrine, waste piles up faster and hygiene drops faster.
            if (!HasLatrine)
            {
                _accumulatedWaste += dailyWaste * 0.5f; // extra mess
                float decay = HygieneDecayPerWastePerHour * _accumulatedWaste * gameHours * 2f;
                SetHygiene(_hygiene - decay);
            }
            else
            {
                // Latrine contains waste but still decays hygiene slowly.
                float decay = HygieneDecayPerWastePerHour * _accumulatedWaste * gameHours * 0.3f;
                SetHygiene(_hygiene - decay);

                // Clamp latrine waste to capacity (overflow = hygiene crash).
                if (_accumulatedWaste > LatrineCapacity)
                {
                    float overflow = _accumulatedWaste - LatrineCapacity;
                    _accumulatedWaste = LatrineCapacity;
                    SetHygiene(_hygiene - overflow * 2f);
                    OnLatrineFull?.Invoke();
                }
            }

            // Minimum hygiene floor: 0.
            SetHygiene(Mathf.Max(0f, _hygiene));
        }

        // -----------------------------------------------------------------
        // Actions
        // -----------------------------------------------------------------

        /// <summary>Dump waste outside. Takes fatigue cost. Clears half the latrine.
        /// The caller (GameBootstrap) applies the radiation dose separately via
        /// <see cref="OnOutsideDisposal"/> so this system stays free of Core refs.</summary>
        public float DumpWasteOutside(Survivors.Survivor carrier)
        {
            if (carrier == null || !carrier.IsAlive) return 0f;
            if (_accumulatedWaste <= 0f) return 0f;

            float removed = Mathf.Min(_accumulatedWaste, LatrineCapacity * 0.5f);
            _accumulatedWaste -= removed;

            if (_needsSystem != null)
                _needsSystem.Modify(carrier, NeedKind.Fatigue, OutsideDisposalFatigue);
            else
                carrier.Needs.Fatigue = Mathf.Clamp(
                    carrier.Needs.Fatigue + OutsideDisposalFatigue, 0f, 100f);

            // Radiation exposure is applied by the caller (GameBootstrap) via
            // OnOutsideDisposal to keep WasteSystem free of Core assembly refs.
            OnOutsideDisposal?.Invoke(carrier);

            // Small hygiene recovery from disposing.
            SetHygiene(Mathf.Min(100f, _hygiene + 3f));

            OnWasteDisposed?.Invoke(removed);
            return removed;
        }

        /// <summary>Use soap to bathe, restoring personal hygiene.</summary>
        public float UseSoap(Survivors.Survivor survivor)
        {
            if (survivor == null || !survivor.IsAlive) return 0f;
            float old = _hygiene;
            SetHygiene(Mathf.Min(100f, _hygiene + SoapHygieneRestore));
            if (_needsSystem != null)
                _needsSystem.Modify(survivor, NeedKind.Morale, 5f);
            else
                survivor.Needs.Morale = Mathf.Clamp(survivor.Needs.Morale + 5f, 0f, 100f);
            return _hygiene - old;
        }

        /// <summary>Use bleach to clean the latrine, removing waste and restoring hygiene.</summary>
        public float UseBleach()
        {
            float removed = Mathf.Min(_accumulatedWaste, BleachWasteRemoval);
            _accumulatedWaste -= removed;
            float old = _hygiene;
            SetHygiene(Mathf.Min(100f, _hygiene + BleachHygieneRestore));
            return _hygiene - old;
        }

        // -----------------------------------------------------------------
        // Save / Load
        // -----------------------------------------------------------------

        private void SetHygiene(float value)
        {
            float old = _hygiene;
            _hygiene = Mathf.Clamp(value, 0f, 100f);
            if (Mathf.Abs(_hygiene - old) > 0.001f)
                OnHygieneChanged?.Invoke(old, _hygiene);
        }

        public WasteSystemSave CaptureState()
        {
            return new WasteSystemSave
            {
                AccumulatedWaste = _accumulatedWaste,
                Hygiene = _hygiene
            };
        }

        public void RestoreState(WasteSystemSave save)
        {
            if (save == null)
            {
                _accumulatedWaste = 0f;
                _hygiene = 100f;
                return;
            }
            _accumulatedWaste = Mathf.Max(0f, save.AccumulatedWaste);
            _hygiene = Mathf.Clamp(save.Hygiene, 0f, 100f);
        }
    }

    [Serializable]
    public class WasteSystemSave
    {
        public float AccumulatedWaste;
        public float Hygiene = 100f;
    }
}
