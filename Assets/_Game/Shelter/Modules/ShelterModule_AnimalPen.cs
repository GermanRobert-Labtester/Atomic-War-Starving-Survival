using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Shelter.Modules
{
    public enum LivestockSpecies
    {
        AshGoat,
        ScrapHen,
        BlindRabbit
    }

    [Serializable]
    public class LivestockAnimal
    {
        public string animalId;
        public LivestockSpecies species;
        public string nickname;
        public float health = 100f;         // 0..100
        public float hunger = 0f;           // 0..100 (100 = starving)
        public float maturationDays = 0f;
        public bool isMature = true;
        public bool isSick = false;
    }

    [Serializable]
    public class AnimalPenModuleState
    {
        public string moduleId = "shelter_module_animal_pen";
        public string displayName = "Livestock Pen";
        public bool isBuilt = false;
        public int penCapacity = 6;
        public float storedFeedKg = 15f;
        public float storedWaterLiters = 20f;
        public List<LivestockAnimal> animals = new List<LivestockAnimal>();
        public int totalEggsGathered = 0;
        public float totalMilkGatheredLiters = 0f;
        public int totalMeatYieldedKg = 0;
    }

    /// <summary>
    /// Expansion V / Spec §3.3: Livestock & Mutated-Animal Husbandry System.
    /// Manages breeding, feeding, daily yields (milk, eggs, fertilizer), and slaughtering of shelter livestock.
    /// </summary>
    public class ShelterModule_AnimalPen
    {
        private AnimalPenModuleState _state = new AnimalPenModuleState();

        public event Action<LivestockAnimal> OnAnimalBorn;
        public event Action<LivestockSpecies, float> OnYieldGathered;
        public event Action<LivestockAnimal, int> OnAnimalSlaughtered;

        public AnimalPenModuleState State => _state;
        public bool IsBuilt => _state.isBuilt;
        public int AnimalCount => _state.animals?.Count ?? 0;

        public void BuildPen()
        {
            _state.isBuilt = true;
            if (_state.animals.Count == 0)
            {
                // Initial breeding pair
                AddAnimal(LivestockSpecies.ScrapHen, "Hen-1");
                AddAnimal(LivestockSpecies.ScrapHen, "Hen-2");
            }
        }

        public bool AddAnimal(LivestockSpecies species, string nickname = "")
        {
            if (_state.animals.Count >= _state.penCapacity) return false;

            var animal = new LivestockAnimal
            {
                animalId = $"animal_{species.ToString().ToLower()}_{Guid.NewGuid().ToString().Substring(0, 5)}",
                species = species,
                nickname = string.IsNullOrEmpty(nickname) ? species.ToString() : nickname,
                health = 100f,
                hunger = 0f,
                isMature = true
            };

            _state.animals.Add(animal);
            OnAnimalBorn?.Invoke(animal);
            return true;
        }

        public void AddFeed(float feedKg) => _state.storedFeedKg += Mathf.Max(0f, feedKg);
        public void AddWater(float waterLiters) => _state.storedWaterLiters += Mathf.Max(0f, waterLiters);

        /// <summary>
        /// Daily tick: processes food consumption, growth, and daily produce yields.
        /// </summary>
        public void DailyTick()
        {
            if (!_state.isBuilt || _state.animals.Count == 0) return;

            for (int i = _state.animals.Count - 1; i >= 0; i--)
            {
                var animal = _state.animals[i];
                float feedNeeded = animal.species == LivestockSpecies.AshGoat ? 2.0f : 0.5f;
                float waterNeeded = animal.species == LivestockSpecies.AshGoat ? 3.0f : 0.8f;

                if (_state.storedFeedKg >= feedNeeded && _state.storedWaterLiters >= waterNeeded)
                {
                    _state.storedFeedKg -= feedNeeded;
                    _state.storedWaterLiters -= waterNeeded;
                    animal.hunger = Mathf.Max(0f, animal.hunger - 20f);
                    animal.health = Mathf.Min(100f, animal.health + 5f);

                    // Daily yields
                    if (animal.isMature && !animal.isSick)
                    {
                        if (animal.species == LivestockSpecies.ScrapHen)
                        {
                            _state.totalEggsGathered += 1;
                            OnYieldGathered?.Invoke(animal.species, 1f);
                        }
                        else if (animal.species == LivestockSpecies.AshGoat)
                        {
                            _state.totalMilkGatheredLiters += 1.5f;
                            OnYieldGathered?.Invoke(animal.species, 1.5f);
                        }
                    }
                }
                else
                {
                    animal.hunger = Mathf.Min(100f, animal.hunger + 30f);
                    if (animal.hunger >= 80f)
                    {
                        animal.health -= 25f;
                        if (animal.health <= 0f)
                        {
                            _state.animals.RemoveAt(i);
                        }
                    }
                }
            }
        }

        public int SlaughterAnimal(string animalId)
        {
            int idx = _state.animals.FindIndex(a => a.animalId == animalId);
            if (idx < 0) return 0;

            var animal = _state.animals[idx];
            _state.animals.RemoveAt(idx);

            int meatKg = animal.species switch
            {
                LivestockSpecies.AshGoat => 18,
                LivestockSpecies.BlindRabbit => 3,
                _ => 2
            };

            _state.totalMeatYieldedKg += meatKg;
            OnAnimalSlaughtered?.Invoke(animal, meatKg);
            return meatKg;
        }

        public AnimalPenModuleState CaptureState() => _state;

        public void RestoreState(AnimalPenModuleState state)
        {
            _state = state ?? new AnimalPenModuleState();
            if (_state.animals == null)
                _state.animals = new List<LivestockAnimal>();
        }
    }
}
