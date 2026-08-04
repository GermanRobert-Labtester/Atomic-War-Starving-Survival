using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Survivors
{
    [Serializable]
    public class SurvivorDiaryEntry
    {
        public int DayCreated;
        public float MoraleAtCreation;
        public string TraitId;
        public string Content;

        public SurvivorDiaryEntry() { }

        public SurvivorDiaryEntry(int dayCreated, float morale, string traitId, string content)
        {
            DayCreated = dayCreated;
            MoraleAtCreation = morale;
            TraitId = traitId;
            Content = content ?? string.Empty;
        }
    }

    public class DiaryIntel
    {
        public string SurvivorId;
        public RiskBiasTrait RiskBias;
        public List<string> ActiveAfflictionNames = new List<string>();
        public List<AffinityEntry> Affinities = new List<AffinityEntry>();
        public List<SurvivorDiaryEntry> Entries = new List<SurvivorDiaryEntry>();
        public bool WasCaught;
    }

    public class SurvivorDiariesSystem
    {
        private readonly Dictionary<string, List<SurvivorDiaryEntry>> _diariesBySurvivor =
            new Dictionary<string, List<SurvivorDiaryEntry>>();

        public event Action<Survivor, SurvivorDiaryEntry> OnDiaryEntryAdded;
        public event Action<Survivor, bool> OnDiaryRead; // survivor, wasCaught

        public IReadOnlyList<SurvivorDiaryEntry> GetDiaryEntries(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return Array.Empty<SurvivorDiaryEntry>();
            if (_diariesBySurvivor.TryGetValue(survivorId, out var entries))
            {
                return entries;
            }
            return Array.Empty<SurvivorDiaryEntry>();
        }

        public void Tick(
            float gameHours,
            IReadOnlyList<Survivor> survivors,
            int currentDay,
            System.Random rng)
        {
            if (gameHours <= 0f || survivors == null) return;
            if (rng == null) rng = new System.Random();

            for (int i = 0; i < survivors.Count; i++)
            {
                var sv = survivors[i];
                if (sv == null || !sv.IsAlive) continue;

                if (rng.NextDouble() < 0.05f * (gameHours / 24f))
                {
                    GeneratePassiveEntry(sv, currentDay, rng);
                }
            }
        }

        public void GeneratePassiveEntry(Survivor sv, int currentDay, System.Random rng)
        {
            if (sv == null) return;
            if (!_diariesBySurvivor.TryGetValue(sv.Id, out var entries))
            {
                entries = new List<SurvivorDiaryEntry>();
                _diariesBySurvivor[sv.Id] = entries;
            }

            string trait = sv.Traits != null && sv.Traits.Count > 0 ? sv.Traits[rng.Next(sv.Traits.Count)] : "default";
            string text = BuildEntryText(sv, trait);

            var entry = new SurvivorDiaryEntry(currentDay, sv.Needs != null ? sv.Needs.Morale : 50f, trait, text);
            entries.Add(entry);

            OnDiaryEntryAdded?.Invoke(sv, entry);
        }

        public DiaryIntel ReadDiary(
            Survivor sv,
            Func<Survivor, List<string>> getAfflictions,
            MentalBreakSystem mentalBreak,
            System.Random rng,
            float customCatchRoll = -1f)
        {
            if (sv == null) return null;
            if (rng == null) rng = new System.Random();

            float catchChance = Mathf.Clamp(0.25f + (sv.PerceivedRadRisk * 0.4f), 0.1f, 0.85f);
            double roll = customCatchRoll >= 0f ? customCatchRoll : rng.NextDouble();
            bool caught = roll < catchChance;

            if (caught)
            {
                if (sv.Needs != null)
                {
                    sv.Needs.Morale = Mathf.Clamp(sv.Needs.Morale - 25f, 0f, 100f);
                }
                if (mentalBreak != null && mentalBreak.Affinity != null)
                {
                    mentalBreak.Affinity.Adjust(sv.Id, "bunker_leader", -35f);
                }

                sv.HasHiddenStash = true;
                if (sv.HiddenItemIds == null)
                {
                    sv.HiddenItemIds = new List<string>();
                }
                sv.HiddenItemIds.Add("stash_ration_" + rng.Next(100, 999));
            }

            var intel = new DiaryIntel
            {
                SurvivorId = sv.Id,
                RiskBias = sv.RiskBias,
                WasCaught = caught,
                Entries = new List<SurvivorDiaryEntry>(GetDiaryEntries(sv.Id))
            };

            if (getAfflictions != null)
            {
                var affs = getAfflictions(sv);
                if (affs != null)
                {
                    intel.ActiveAfflictionNames.AddRange(affs);
                }
            }

            if (mentalBreak != null && mentalBreak.Affinity != null)
            {
                intel.Affinities = mentalBreak.Affinity.Snapshot();
            }

            OnDiaryRead?.Invoke(sv, caught);
            return intel;
        }

        private static string BuildEntryText(Survivor sv, string trait)
        {
            float morale = sv.Needs != null ? sv.Needs.Morale : 50f;
            if (morale < 30f)
            {
                return $"Day log ({trait}): The dust won't stop leaking. I don't trust anyone here.";
            }
            if (morale > 70f)
            {
                return $"Day log ({trait}): Still alive today. We might actually survive this.";
            }
            return $"Day log ({trait}): Routine day. Dosimeter ticking constantly.";
        }
    }
}
