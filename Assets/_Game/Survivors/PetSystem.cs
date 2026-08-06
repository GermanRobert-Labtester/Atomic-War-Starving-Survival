using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Manages animal companions (Pets). Pets boost survivor morale while alive,
    /// consume food/water, generate CO2, provide specialized traits, and radiate
    /// rooms/survivors as mobile contamination sources when dirty.
    /// Save/load safe; raises events on state change.
    /// </summary>
    public class PetSystem
    {
        public const float HungerRatePerHour = 1.5f;
        public const float ThirstRatePerHour = 2.0f;
        public const float MoraleBoostPerHour = 1.5f;
        public const float CatastrophicMoraleDropOnDeath = -40f;
        public const float Co2GenerationPerHour = 2.0f; // Air quality reduction per pet per hour
        public const float FurContaminationDepositRate = 0.05f; // Rate fur contamination transfers to room per hour

        private readonly NeedsSystem _needsSystem;
        private readonly Action<string, float> _depositRoomContamination; // (roomId, amount) -> deposits contamination into room
        private readonly Action<float> _addBunkerContamination;           // (amount) -> adds bunker contamination
        private readonly List<PetState> _pets = new List<PetState>();
        private PersonalQuestSystem _personalQuests;
        private Func<IReadOnlyList<Survivor>> _getSurvivors;

        public IReadOnlyList<PetState> Pets => _pets;

        /// <summary>Prompt #217 — Zoonotic Expert taming.</summary>
        public void BindPersonalQuests(
            PersonalQuestSystem personalQuests,
            Func<IReadOnlyList<Survivor>> getSurvivors = null)
        {
            _personalQuests = personalQuests;
            _getSurvivors = getSurvivors;
        }

        /// <summary>
        /// Prompt #217 — tame a wasteland animal. Requires Zoonotic Expert owner
        /// and fewer than 3 living pets already tamed by them.
        /// </summary>
        public bool TryTameWastelandAnimal(Survivor owner, PetState animal)
        {
            if (owner == null || !owner.IsAlive || animal == null) return false;
            if (_personalQuests == null || !_personalQuests.HasZoonoticExpert(owner))
                return false;
            int owned = CountPetsOwnedBy(owner.Id);
            if (owned >= PersonalQuestSystem.ZoonoticMaxTamedAnimals) return false;
            animal.OwnerSurvivorId = owner.Id;
            animal.EatsSpoiledMeatOnly = true;
            AddPet(animal);
            return true;
        }

        public int CountPetsOwnedBy(string ownerId)
        {
            if (string.IsNullOrEmpty(ownerId)) return 0;
            int n = 0;
            for (int i = 0; i < _pets.Count; i++)
            {
                var p = _pets[i];
                if (p != null && p.IsAlive
                    && string.Equals(p.OwnerSurvivorId, ownerId, System.StringComparison.Ordinal))
                    n++;
            }
            return n;
        }

        public event Action<PetState> OnPetAdded;
        public event Action<PetState> OnPetDied;
        public event Action<PetState, string> OnPetRoomChanged;

        public PetSystem(
            NeedsSystem needsSystem,
            Action<string, float> depositRoomContamination = null,
            Action<float> addBunkerContamination = null)
        {
            _needsSystem = needsSystem ?? throw new ArgumentNullException(nameof(needsSystem));
            _depositRoomContamination = depositRoomContamination;
            _addBunkerContamination = addBunkerContamination;
        }

        public void AddPet(PetState pet)
        {
            if (pet == null || string.IsNullOrEmpty(pet.Id)) return;
            if (!_pets.Contains(pet))
            {
                _pets.Add(pet);
                OnPetAdded?.Invoke(pet);
            }
        }

        public void RemovePet(PetState pet)
        {
            if (pet != null)
            {
                _pets.Remove(pet);
            }
        }

        public PetState GetPet(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            for (int i = 0; i < _pets.Count; i++)
            {
                if (_pets[i] != null && _pets[i].Id == id) return _pets[i];
            }
            return null;
        }

        public void SetPetRoom(PetState pet, string roomId)
        {
            if (pet == null) return;
            pet.CurrentRoomId = roomId;
            OnPetRoomChanged?.Invoke(pet, roomId);
        }

        public void Decontaminate(PetState pet)
        {
            if (pet == null) return;
            pet.FurContamination = 0f;
            pet.Radiation = Mathf.Max(0f, pet.Radiation - 20f);
        }

        public void Tick(IReadOnlyList<Survivor> survivors, float gameHours)
        {
            if (gameHours <= 0f) return;

            for (int i = _pets.Count - 1; i >= 0; i--)
            {
                var pet = _pets[i];
                if (pet == null || !pet.IsAlive) continue;

                // 1. Need progression (Hunger & Thirst)
                pet.Hunger = Mathf.Clamp(pet.Hunger + HungerRatePerHour * gameHours, 0f, 100f);
                pet.Thirst = Mathf.Clamp(pet.Thirst + ThirstRatePerHour * gameHours, 0f, 100f);

                if (pet.Hunger >= 100f || pet.Thirst >= 100f)
                {
                    pet.IsAlive = false;
                    OnPetDied?.Invoke(pet);
                    TriggerCatastrophicMoraleDrop(survivors);
                    continue;
                }

                // 2. Mobile Contamination (Radiates room and adds ambient contamination)
                if (pet.FurContamination > 0f && !string.IsNullOrEmpty(pet.CurrentRoomId))
                {
                    float deposit = pet.FurContamination * FurContaminationDepositRate * gameHours;
                    if (_depositRoomContamination != null)
                    {
                        _depositRoomContamination(pet.CurrentRoomId, deposit);
                    }
                    else if (_addBunkerContamination != null)
                    {
                        _addBunkerContamination(deposit);
                    }

                    // Pet receives rad dose from its own fur
                    pet.Radiation = Mathf.Clamp(pet.Radiation + pet.FurContamination * 0.1f * gameHours, 0f, 100f);
                }

                // 3. Passive Morale Boost to living survivors
                if (survivors != null)
                {
                    for (int s = 0; s < survivors.Count; s++)
                    {
                        var sv = survivors[s];
                        if (sv != null && sv.IsAlive)
                        {
                            _needsSystem.Modify(sv, NeedKind.Morale, MoraleBoostPerHour * gameHours);
                        }
                    }
                }
            }
        }

        private void TriggerCatastrophicMoraleDrop(IReadOnlyList<Survivor> survivors)
        {
            if (survivors == null) return;
            for (int i = 0; i < survivors.Count; i++)
            {
                var sv = survivors[i];
                if (sv != null && sv.IsAlive)
                {
                    _needsSystem.Modify(sv, NeedKind.Morale, CatastrophicMoraleDropOnDeath);
                }
            }
        }

        // -----------------------------------------------------------------
        // Save / Load
        // -----------------------------------------------------------------

        /// <summary>
        /// Snapshot all pets (deep copy). Null-safe restore is a no-op clear.
        /// </summary>
        public PetSystemSave CaptureState()
        {
            var pets = new PetState[_pets.Count];
            for (int i = 0; i < _pets.Count; i++)
                pets[i] = ClonePet(_pets[i]);
            return new PetSystemSave { Pets = pets };
        }

        /// <summary>
        /// Replace pet list from a snapshot. Null/empty clears to empty roster.
        /// </summary>
        public void RestoreState(PetSystemSave save)
        {
            _pets.Clear();
            if (save?.Pets == null) return;
            for (int i = 0; i < save.Pets.Length; i++)
            {
                var p = save.Pets[i];
                if (p == null || string.IsNullOrEmpty(p.Id)) continue;
                _pets.Add(ClonePet(p));
            }
        }

        private static PetState ClonePet(PetState src)
        {
            if (src == null) return null;
            // Normalize null strings to "" so JsonUtility round-trips match
            // (Unity serializes missing string fields as empty, not null).
            return new PetState
            {
                Id = src.Id ?? "",
                DisplayName = src.DisplayName ?? "",
                Traits = src.Traits,
                Hunger = src.Hunger,
                Thirst = src.Thirst,
                Radiation = src.Radiation,
                FurContamination = src.FurContamination,
                CurrentRoomId = src.CurrentRoomId ?? "",
                IsAlive = src.IsAlive,
                OwnerSurvivorId = src.OwnerSurvivorId ?? "",
                EatsSpoiledMeatOnly = src.EatsSpoiledMeatOnly
            };
        }
    }

    /// <summary>Save DTO for <see cref="PetSystem"/> (ISaveable adapter id: "pets").</summary>
    [Serializable]
    public class PetSystemSave
    {
        public PetState[] Pets;
    }
}
