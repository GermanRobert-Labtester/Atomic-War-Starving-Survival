using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Shelter;

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
        private readonly Shelter.Shelter _shelter;
        private readonly List<PetState> _pets = new List<PetState>();
        private readonly List<ShelterRoom> _rooms = new List<ShelterRoom>();

        public IReadOnlyList<PetState> Pets => _pets;

        public event Action<PetState> OnPetAdded;
        public event Action<PetState> OnPetDied;
        public event Action<PetState, string> OnPetRoomChanged;

        public PetSystem(NeedsSystem needsSystem, Shelter.Shelter shelter = null)
        {
            _needsSystem = needsSystem ?? throw new ArgumentNullException(nameof(needsSystem));
            _shelter = shelter;
        }

        public void RegisterRoom(ShelterRoom room)
        {
            if (room != null && !_rooms.Contains(room))
            {
                _rooms.Add(room);
            }
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
                    ShelterRoom room = FindRoom(pet.CurrentRoomId);
                    if (room != null)
                    {
                        float deposit = pet.FurContamination * FurContaminationDepositRate * gameHours;
                        room.AmbientContamination = Mathf.Clamp01(room.AmbientContamination + deposit);
                    }
                    else if (_shelter != null)
                    {
                        _shelter.AddBunkerContamination(pet.FurContamination * 0.1f * gameHours);
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

        public ShelterRoom FindRoom(string roomId)
        {
            if (string.IsNullOrEmpty(roomId)) return null;
            for (int i = 0; i < _rooms.Count; i++)
            {
                if (_rooms[i] != null && _rooms[i].RoomId == roomId) return _rooms[i];
            }
            return null;
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
    }
}
