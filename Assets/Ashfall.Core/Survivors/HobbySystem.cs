// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Survivors
{
    public enum HobbyCategory
    {
        Creative = 0,
        Intellectual = 1,
        Physical = 2,
        Social = 3,
        Crafting = 4,
        Collecting = 5
    }

    public enum HobbyMastery
    {
        Novice = 0,
        Apprentice = 1,
        Journeyman = 2,
        Master = 3
    }

    [Serializable]
    public sealed class HobbyDefinition
    {
        public string HobbyId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public HobbyCategory Category { get; set; } = HobbyCategory.Creative;
        public string RequiredFacility { get; set; } = string.Empty;
        public float BaseMoraleBonus { get; set; } = 5f;
    }

    [Serializable]
    public sealed class SurvivorHobbyProgress
    {
        public string SurvivorId { get; set; } = string.Empty;
        public string HobbyId { get; set; } = string.Empty;
        public float Proficiency { get; set; } = 0f;
        public int SessionsCompleted { get; set; } = 0;
        public int LastSessionDay { get; set; } = 0;
        public HobbyMastery Mastery { get; set; } = HobbyMastery.Novice;
    }

    [Serializable]
    public sealed class HobbySessionResult
    {
        public string SurvivorId { get; set; } = string.Empty;
        public string HobbyId { get; set; } = string.Empty;
        public int Day { get; set; }
        public float MoraleGained { get; set; }
        public float ProficiencyGained { get; set; }
        public HobbyMastery NewMastery { get; set; }
        public List<string> CoParticipants { get; set; } = new List<string>();
    }

    [Serializable]
    public sealed class HobbySystemState
    {
        public int SchemaVersion { get; set; } = 1;
        public int NextSequence { get; set; } = 1;
        public List<SurvivorHobbyProgress> ProgressRecords { get; set; } = new List<SurvivorHobbyProgress>();
        public List<HobbyDefinition> AuthoredHobbies { get; set; } = new List<HobbyDefinition>();
    }

    /// <summary>
    /// Plan 161 — Survivor Hobby & Leisure System.
    /// Tracks survivor personal pastimes, creative pursuits, proficiency mastery,
    /// off-duty morale generation, and shared hobby affinity.
    /// </summary>
    public sealed class HobbySystem
    {
        private readonly HobbySystemState _state;

        public event Action<HobbySessionResult>? OnSessionCompleted;
        public event Action<SurvivorHobbyProgress, HobbyMastery>? OnMasteryAchieved;

        public int ProgressCount => _state.ProgressRecords.Count;

        public HobbySystem(HobbySystemState? state = null)
        {
            _state = state ?? new HobbySystemState();
            EnsureDefaultHobbies();
        }

        private void EnsureDefaultHobbies()
        {
            if (_state.AuthoredHobbies.Count == 0)
            {
                _state.AuthoredHobbies.Add(new HobbyDefinition { HobbyId = "hobby_woodcarving", Name = "Woodcarving", Category = HobbyCategory.Crafting, BaseMoraleBonus = 6f });
                _state.AuthoredHobbies.Add(new HobbyDefinition { HobbyId = "hobby_painting", Name = "Painting", Category = HobbyCategory.Creative, BaseMoraleBonus = 8f });
                _state.AuthoredHobbies.Add(new HobbyDefinition { HobbyId = "hobby_reading", Name = "Reading", Category = HobbyCategory.Intellectual, BaseMoraleBonus = 5f });
                _state.AuthoredHobbies.Add(new HobbyDefinition { HobbyId = "hobby_instrument", Name = "Music & Strings", Category = HobbyCategory.Creative, BaseMoraleBonus = 8f });
                _state.AuthoredHobbies.Add(new HobbyDefinition { HobbyId = "hobby_chess", Name = "Chess & Tactics", Category = HobbyCategory.Social, BaseMoraleBonus = 6f });
                _state.AuthoredHobbies.Add(new HobbyDefinition { HobbyId = "hobby_botany", Name = "Herbarium Collecting", Category = HobbyCategory.Collecting, BaseMoraleBonus = 5f });
            }
        }

        public static HobbyMastery ResolveMastery(float proficiency)
        {
            if (proficiency >= 75f) return HobbyMastery.Master;
            if (proficiency >= 50f) return HobbyMastery.Journeyman;
            if (proficiency >= 25f) return HobbyMastery.Apprentice;
            return HobbyMastery.Novice;
        }

        public SurvivorHobbyProgress GetOrCreateProgress(string survivorId, string hobbyId)
        {
            if (string.IsNullOrEmpty(survivorId)) throw new ArgumentNullException(nameof(survivorId));
            if (string.IsNullOrEmpty(hobbyId)) throw new ArgumentNullException(nameof(hobbyId));

            var record = _state.ProgressRecords.FirstOrDefault(r =>
                string.Equals(r.SurvivorId, survivorId, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(r.HobbyId, hobbyId, StringComparison.OrdinalIgnoreCase));

            if (record == null)
            {
                record = new SurvivorHobbyProgress
                {
                    SurvivorId = survivorId,
                    HobbyId = hobbyId,
                    Proficiency = 0f,
                    SessionsCompleted = 0,
                    LastSessionDay = 0,
                    Mastery = HobbyMastery.Novice
                };
                _state.ProgressRecords.Add(record);
            }

            return record;
        }

        public HobbySessionResult ConductSession(
            string survivorId,
            string hobbyId,
            int currentDay,
            IEnumerable<string>? coParticipantIds = null,
            ISeededRng? rng = null)
        {
            var progress = GetOrCreateProgress(survivorId, hobbyId);
            var hobby = _state.AuthoredHobbies.FirstOrDefault(h => string.Equals(h.HobbyId, hobbyId, StringComparison.OrdinalIgnoreCase))
                        ?? new HobbyDefinition { HobbyId = hobbyId, BaseMoraleBonus = 5f };

            var participants = coParticipantIds != null ? coParticipantIds.ToList() : new List<string>();

            float gain = progress.Proficiency >= 75f ? 2.5f : (progress.Proficiency >= 50f ? 4f : 6f);
            if (rng != null)
            {
                gain *= (float)(0.8 + rng.NextDouble() * 0.4);
            }

            progress.Proficiency = Math.Clamp(progress.Proficiency + gain, 0f, 100f);
            progress.SessionsCompleted++;
            progress.LastSessionDay = currentDay;

            var oldMastery = progress.Mastery;
            progress.Mastery = ResolveMastery(progress.Proficiency);

            float moraleBonus = hobby.BaseMoraleBonus * (progress.Mastery switch
            {
                HobbyMastery.Master => 1.75f,
                HobbyMastery.Journeyman => 1.40f,
                HobbyMastery.Apprentice => 1.15f,
                _ => 1.0f
            });

            if (participants.Count > 0)
            {
                moraleBonus += participants.Count * 1.5f;
            }

            var result = new HobbySessionResult
            {
                SurvivorId = survivorId,
                HobbyId = hobbyId,
                Day = currentDay,
                MoraleGained = moraleBonus,
                ProficiencyGained = gain,
                NewMastery = progress.Mastery,
                CoParticipants = participants
            };

            OnSessionCompleted?.Invoke(result);

            if (progress.Mastery > oldMastery)
            {
                OnMasteryAchieved?.Invoke(progress, progress.Mastery);
            }

            return result;
        }

        public float GetSharedHobbyAffinityBonus(string survivorA, string survivorB)
        {
            if (string.IsNullOrEmpty(survivorA) || string.IsNullOrEmpty(survivorB)) return 0f;

            var hobbiesA = _state.ProgressRecords
                .Where(p => string.Equals(p.SurvivorId, survivorA, StringComparison.OrdinalIgnoreCase) && p.Proficiency >= 20f)
                .Select(p => p.HobbyId)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var hobbiesB = _state.ProgressRecords
                .Where(p => string.Equals(p.SurvivorId, survivorB, StringComparison.OrdinalIgnoreCase) && p.Proficiency >= 20f)
                .Select(p => p.HobbyId);

            int shared = hobbiesB.Count(h => hobbiesA.Contains(h));
            return shared * 5.0f; // +5 relationship affinity per shared hobby
        }

        public IReadOnlyList<SurvivorHobbyProgress> GetSurvivorHobbies(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return Array.Empty<SurvivorHobbyProgress>();
            return _state.ProgressRecords
                .Where(p => string.Equals(p.SurvivorId, survivorId, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public HobbySystemState CaptureState()
        {
            var captured = new HobbySystemState
            {
                SchemaVersion = _state.SchemaVersion,
                NextSequence = _state.NextSequence,
                AuthoredHobbies = new List<HobbyDefinition>(_state.AuthoredHobbies),
                ProgressRecords = new List<SurvivorHobbyProgress>(_state.ProgressRecords.Count)
            };

            for (int i = 0; i < _state.ProgressRecords.Count; i++)
            {
                var p = _state.ProgressRecords[i];
                captured.ProgressRecords.Add(new SurvivorHobbyProgress
                {
                    SurvivorId = p.SurvivorId,
                    HobbyId = p.HobbyId,
                    Proficiency = p.Proficiency,
                    SessionsCompleted = p.SessionsCompleted,
                    LastSessionDay = p.LastSessionDay,
                    Mastery = p.Mastery
                });
            }

            return captured;
        }

        public void RestoreState(HobbySystemState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            _state.SchemaVersion = state.SchemaVersion;
            _state.NextSequence = state.NextSequence;
            _state.AuthoredHobbies = new List<HobbyDefinition>(state.AuthoredHobbies ?? Enumerable.Empty<HobbyDefinition>());
            _state.ProgressRecords.Clear();

            if (state.ProgressRecords != null)
            {
                for (int i = 0; i < state.ProgressRecords.Count; i++)
                {
                    var p = state.ProgressRecords[i];
                    _state.ProgressRecords.Add(new SurvivorHobbyProgress
                    {
                        SurvivorId = p.SurvivorId,
                        HobbyId = p.HobbyId,
                        Proficiency = p.Proficiency,
                        SessionsCompleted = p.SessionsCompleted,
                        LastSessionDay = p.LastSessionDay,
                        Mastery = p.Mastery
                    });
                }
            }
        }
    }
}
