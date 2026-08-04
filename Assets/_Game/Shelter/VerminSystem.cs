using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Shelter
{
    /// <summary>
    /// Vermin Infestations (Prompt #51). High food storage + low hygiene spawn pests.
    /// Pests consume 1-5% of stored food daily and drag contamination across rooms.
    /// The AI can HuntRats. Adopting a cat (PetSystem) suppresses pest growth.
    /// Save/load safe. Plain C#.
    /// </summary>
    public class VerminSystem
    {
        /// <summary>PestLevel at/above which food theft begins.</summary>
        public const float PestActiveThreshold = 10f;

        /// <summary>Max pest level (saturation).</summary>
        public const float MaxPestLevel = 100f;

        /// <summary>Pest growth per day per unit of stored food above the threshold.</summary>
        public const float PestGrowthPerFoodPerDay = 0.02f;

        /// <summary>Pest growth multiplier when hygiene is low.</summary>
        public const float LowHygieneGrowthMultiplier = 3f;

        /// <summary>Hygiene below which pest growth accelerates.</summary>
        public const float PestHygieneThreshold = 40f;

        /// <summary>Fraction of food stores eaten per day at max pest level.</summary>
        public const float MaxFoodTheftFraction = 0.05f;

        /// <summary>Contamination dragged per pest-level point per hour.</summary>
        public const float ContaminationPerPestLevelPerHour = 0.001f;

        /// <summary>Pest reduction per HuntRats action.</summary>
        public const float HuntRatsReduction = 25f;

        /// <summary>Fatigue cost of hunting rats for an hour.</summary>
        public const float HuntRatsFatigueCost = 8f;

        /// <summary>Pest suppression per cat (PetSystem).</summary>
        public const float CatSuppressionPerDay = 15f;

        private float _pestLevel;
        private readonly System.Random _rng;
        private Func<Shelter> _getShelter;
        private Func<Survivors.PetSystem> _getPetSystem;
        private Func<float> _getHygiene;     // from WasteSystem

        // -- Public state --
        public float PestLevel => _pestLevel;
        public bool IsInfested => _pestLevel >= PestActiveThreshold;

        /// <summary>Fraction of food that pests will consume today (0..MaxFoodTheftFraction).</summary>
        public float DailyFoodTheftFraction
        {
            get
            {
                if (!IsInfested) return 0f;
                return MaxFoodTheftFraction * (_pestLevel / MaxPestLevel);
            }
        }

        // -- Events --
        public event Action<float, float> OnPestLevelChanged;  // (old, new)
        public event Action OnInfestationStarted;
        public event Action OnInfestationEnded;
        public event Action<int> OnFoodStolen;                  // units eaten
        public event Action<float> OnContaminationDragged;     // amount added

        public VerminSystem(System.Random rng = null)
        {
            _rng = rng ?? new System.Random(51);
        }

        public void Bind(
            Func<Shelter> getShelter,
            Func<Survivors.PetSystem> getPetSystem = null,
            Func<float> getHygiene = null)
        {
            _getShelter = getShelter;
            _getPetSystem = getPetSystem;
            _getHygiene = getHygiene;
        }

        // -----------------------------------------------------------------
        // Tick
        // -----------------------------------------------------------------

        /// <summary>Grow pest population, steal food, drag contamination.</summary>
        public void Tick(float gameHours, Inventory.Inventory inventory)
        {
            if (gameHours <= 0f) return;
            float gameDays = gameHours / 24f;

            // 1. Pest growth from stored food + low hygiene.
            if (inventory != null)
            {
                int foodCount = inventory.CountByType(Inventory.ItemType.Food);
                float hygiene = _getHygiene?.Invoke() ?? 100f;
                float growthMultiplier = hygiene < PestHygieneThreshold
                    ? LowHygieneGrowthMultiplier
                    : 1f;
                float growth = foodCount * PestGrowthPerFoodPerDay * gameDays * growthMultiplier;

                // Cat suppression.
                var petSys = _getPetSystem?.Invoke();
                if (petSys != null && petSys.Pets != null)
                {
                    int catCount = 0;
                    for (int i = 0; i < petSys.Pets.Count; i++)
                    {
                        if (petSys.Pets[i] != null && petSys.Pets[i].IsAlive)
                            catCount++;
                    }
                    growth -= catCount * CatSuppressionPerDay * gameDays;
                }

                SetPestLevel(_pestLevel + growth);
            }

            // 2. Natural decay when no food.
            if (inventory == null || inventory.CountByType(Inventory.ItemType.Food) == 0)
            {
                SetPestLevel(_pestLevel - 3f * gameDays); // starve out
            }

            // 3. Food theft.
            if (IsInfested && inventory != null)
            {
                int totalFood = inventory.CountByType(Inventory.ItemType.Food);
                int stolen = Mathf.Max(1, Mathf.RoundToInt(totalFood * DailyFoodTheftFraction * gameDays));
                int actuallyStolen = inventory.RemoveByType(Inventory.ItemType.Food, stolen);
                if (actuallyStolen > 0)
                {
                    _pestLevel = Mathf.Min(MaxPestLevel, _pestLevel + actuallyStolen * 0.5f);
                    OnFoodStolen?.Invoke(actuallyStolen);
                }
            }

            // 4. Contamination dragging.
            if (IsInfested)
            {
                float contamination = ContaminationPerPestLevelPerHour * _pestLevel * gameHours;
                var shelter = _getShelter?.Invoke();
                if (shelter != null && shelter.Rooms != null)
                {
                    for (int i = 0; i < shelter.Rooms.Count; i++)
                    {
                        var room = shelter.Rooms[i];
                        if (room == null) continue;
                        room.AmbientContamination = Mathf.Clamp01(
                            room.AmbientContamination + contamination);
                    }
                }
                OnContaminationDragged?.Invoke(contamination);
            }
        }

        // -----------------------------------------------------------------
        // Actions
        // -----------------------------------------------------------------

        /// <summary>Hunt rats. Reduces pest level at fatigue cost.</summary>
        public float HuntRats(Survivors.Survivor hunter)
        {
            if (hunter == null || !hunter.IsAlive) return 0f;
            if (_pestLevel <= 0f) return 0f;

            float reduction = HuntRatsReduction;
            // Crafting skill represents trap-making ability.
            reduction *= (1f + hunter.EffectiveCraftingSkill * 0.5f);

            float old = _pestLevel;
            SetPestLevel(_pestLevel - reduction);

            hunter.Needs.Fatigue = Mathf.Clamp(
                hunter.Needs.Fatigue + HuntRatsFatigueCost, 0f, 100f);

            return old - _pestLevel;
        }

        /// <summary>Force pest level for tests/scripted events.</summary>
        public void SetPestLevelOverride(float level)
        {
            SetPestLevel(level);
        }

        // -----------------------------------------------------------------
        // Save / Load
        // -----------------------------------------------------------------

        private void SetPestLevel(float value)
        {
            float old = _pestLevel;
            _pestLevel = Mathf.Clamp(value, 0f, MaxPestLevel);

            bool wasInfested = old >= PestActiveThreshold;
            bool isNowInfested = _pestLevel >= PestActiveThreshold;

            if (!wasInfested && isNowInfested)
                OnInfestationStarted?.Invoke();
            else if (wasInfested && !isNowInfested)
                OnInfestationEnded?.Invoke();

            if (Mathf.Abs(_pestLevel - old) > 0.001f)
                OnPestLevelChanged?.Invoke(old, _pestLevel);
        }

        public VerminSave CaptureState()
        {
            return new VerminSave { PestLevel = _pestLevel };
        }

        public void RestoreState(VerminSave save)
        {
            if (save == null) { _pestLevel = 0f; return; }
            _pestLevel = Mathf.Clamp(save.PestLevel, 0f, MaxPestLevel);
        }
    }

    [Serializable]
    public class VerminSave
    {
        public float PestLevel;
    }
}
