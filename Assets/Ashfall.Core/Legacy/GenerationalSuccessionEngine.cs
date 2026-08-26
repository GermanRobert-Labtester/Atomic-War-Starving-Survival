using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Legacy
{
    [Serializable]
    public sealed class DwellerGenerationRecord
    {
        public string dwellerId;
        public int generationIndex; // 0 = founder, 1 = firstborn/apprentice, 2 = grandchild
        public int inGameAgeYears;
        public bool isRetired;
        public bool isDeceased;
        public string mentorDwellerId;
        public List<string> inheritedTraitIds = new List<string>();
    }

    [Serializable]
    public sealed class GenerationalSuccessionSaveState
    {
        public int currentChapterIndex;
        public int daysElapsedInChapter;
        public int totalYearsElapsed;
        public List<DwellerGenerationRecord> generationRecords = new List<DwellerGenerationRecord>();
    }

    /// <summary>
    /// ASHFALL: THE CENTURY SEED (Expansion 12) — Generational Succession Engine.
    /// Simulates 10x chapter timescale compression, elder retirement, mentor-apprentice skill transfer,
    /// and generational succession across the 10-year horizon.
    /// </summary>
    public sealed class GenerationalSuccessionEngine
    {
        public const int DaysPerChapter = 365; // 1 year per chapter

        public int CurrentChapterIndex { get; private set; } = 1;
        public int DaysElapsedInChapter { get; private set; }
        public int TotalYearsElapsed { get; private set; }

        private readonly Dictionary<string, DwellerGenerationRecord> _records = new Dictionary<string, DwellerGenerationRecord>();

        public event Action<string, int> OnDwellerRetired;
        public event Action<string, string, string> OnTraitInherited;
        public event Action<int> OnChapterAdvanced;

        public void RegisterDweller(string dwellerId, int initialAgeYears, int generation = 0)
        {
            if (string.IsNullOrEmpty(dwellerId)) return;
            if (!_records.ContainsKey(dwellerId))
            {
                _records[dwellerId] = new DwellerGenerationRecord
                {
                    dwellerId = dwellerId,
                    generationIndex = generation,
                    inGameAgeYears = initialAgeYears,
                    isRetired = false,
                    isDeceased = false
                };
            }
        }

        public void AdvanceTime(int days)
        {
            DaysElapsedInChapter += Math.Max(0, days);
            while (DaysElapsedInChapter >= DaysPerChapter)
            {
                DaysElapsedInChapter -= DaysPerChapter;
                CurrentChapterIndex++;
                TotalYearsElapsed++;

                // Age all living dwellers by 1 year
                foreach (var r in _records.Values)
                {
                    if (!r.isDeceased)
                    {
                        r.inGameAgeYears++;
                        if (r.inGameAgeYears >= 65 && !r.isRetired)
                        {
                            r.isRetired = true;
                            OnDwellerRetired?.Invoke(r.dwellerId, r.inGameAgeYears);
                        }
                    }
                }

                OnChapterAdvanced?.Invoke(CurrentChapterIndex);
            }
        }

        public bool FormMentorship(string mentorId, string apprenticeId, string traitId)
        {
            if (!_records.TryGetValue(mentorId, out var mentor) || !_records.TryGetValue(apprenticeId, out var apprentice))
                return false;

            if (mentor.isDeceased || apprentice.isDeceased)
                return false;

            apprentice.mentorDwellerId = mentorId;
            if (!string.IsNullOrEmpty(traitId) && !apprentice.inheritedTraitIds.Contains(traitId))
            {
                apprentice.inheritedTraitIds.Add(traitId);
                OnTraitInherited?.Invoke(mentorId, apprenticeId, traitId);
            }
            return true;
        }

        public DwellerGenerationRecord? GetRecord(string dwellerId)
        {
            _records.TryGetValue(dwellerId, out var rec);
            return rec;
        }

        public GenerationalSuccessionSaveState CaptureState()
        {
            var save = new GenerationalSuccessionSaveState
            {
                currentChapterIndex = CurrentChapterIndex,
                daysElapsedInChapter = DaysElapsedInChapter,
                totalYearsElapsed = TotalYearsElapsed
            };
            foreach (var r in _records.Values)
            {
                save.generationRecords.Add(new DwellerGenerationRecord
                {
                    dwellerId = r.dwellerId,
                    generationIndex = r.generationIndex,
                    inGameAgeYears = r.inGameAgeYears,
                    isRetired = r.isRetired,
                    isDeceased = r.isDeceased,
                    mentorDwellerId = r.mentorDwellerId,
                    inheritedTraitIds = new List<string>(r.inheritedTraitIds)
                });
            }
            return save;
        }

        public void RestoreState(GenerationalSuccessionSaveState state)
        {
            _records.Clear();
            if (state == null) return;

            CurrentChapterIndex = state.currentChapterIndex > 0 ? state.currentChapterIndex : 1;
            DaysElapsedInChapter = state.daysElapsedInChapter;
            TotalYearsElapsed = state.totalYearsElapsed;

            if (state.generationRecords != null)
            {
                foreach (var r in state.generationRecords)
                {
                    _records[r.dwellerId] = new DwellerGenerationRecord
                    {
                        dwellerId = r.dwellerId,
                        generationIndex = r.generationIndex,
                        inGameAgeYears = r.inGameAgeYears,
                        isRetired = r.isRetired,
                        isDeceased = r.isDeceased,
                        mentorDwellerId = r.mentorDwellerId,
                        inheritedTraitIds = new List<string>(r.inheritedTraitIds)
                    };
                }
            }
        }
    }
}
