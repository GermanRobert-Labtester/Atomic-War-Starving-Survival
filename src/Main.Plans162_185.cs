// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Cognition;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        /// <summary>
        /// Plan 185 bounded integration: compose a deterministic memory read
        /// model from the shared skill progression and journal owners. The
        /// projection has no save section and never mutates either source.
        /// </summary>
        public IReadOnlyList<MemoryRecord> BuildMemoryDecayProjection(
            string survivorId = "",
            int currentDay = -1)
        {
            SetupSurvivors();
            SetupJournal();
            var skills = EnsureSharedSkillProgression();
            int day = currentDay > 0 ? currentDay : _simDay;
            var facts = new List<CanonicalMemoryFact>();
            var roster = _survivors?.RosterState;

            if (roster != null)
            {
                for (int i = 0; i < roster.Count; i++)
                {
                    var survivor = roster[i];
                    if (survivor == null || !survivor.IsAliveState) continue;
                    if (!string.IsNullOrEmpty(survivorId)
                        && !string.Equals(survivor.Id, survivorId, StringComparison.OrdinalIgnoreCase))
                        continue;

                    for (int d = 0; d < Ashfall.Core.Survivors.SkillProgressionSystem.Disciplines.Length; d++)
                    {
                        string discipline = Ashfall.Core.Survivors.SkillProgressionSystem.Disciplines[d];
                        float progress = skills.GetDisciplineProgress01(survivor.Id, discipline);
                        int daysSincePractice = skills.DaysSinceLastPractice(survivor.Id, discipline, day);
                        if (progress <= 0f && daysSincePractice < 0) continue;

                        int lastDay = daysSincePractice < 0
                            ? day
                            : Math.Max(1, day - daysSincePractice);
                        bool isCertified = false;
                        var activeSkillIds = skills.GetActiveSkillIds(survivor.Id);
                        for (int s = 0; s < activeSkillIds.Count; s++)
                        {
                            var definition = skills.GetSkill(activeSkillIds[s]);
                            if (definition != null
                                && string.Equals(definition.disciplineId, discipline, StringComparison.Ordinal))
                            {
                                isCertified = true;
                                break;
                            }
                        }

                        facts.Add(new CanonicalMemoryFact
                        {
                            SurvivorId = survivor.Id,
                            Domain = MemoryDomain.Skill,
                            SourceId = $"skill:{survivor.Id}:{discipline}",
                            Strength = progress * 100f,
                            LastReinforcedDay = lastDay,
                            IsCertified = isCertified,
                            IsPreserved = false
                        });
                    }
                }
            }

            if (_journal != null)
            {
                foreach (var entry in _journal.Entries)
                {
                    if (entry == null || string.IsNullOrEmpty(entry.Id)) continue;
                    if (!string.IsNullOrEmpty(survivorId)
                        && !string.Equals(entry.AuthorId, survivorId, StringComparison.OrdinalIgnoreCase))
                        continue;

                    facts.Add(new CanonicalMemoryFact
                    {
                        SurvivorId = string.IsNullOrEmpty(entry.AuthorId) ? "shelter" : entry.AuthorId,
                        Domain = MemoryDomain.EventMemory,
                        SourceId = $"journal:{entry.Id}",
                        Strength = 100f,
                        LastReinforcedDay = Math.Max(1, entry.Day),
                        IsCertified = false,
                        IsPreserved = false
                    });
                }
            }

            return MemoryDecaySystem.ProjectCanonicalSources(facts, day);
        }

        /// <summary>
        /// Plan 162 bounded integration: expose a searchable archive index
        /// over the persisted journal and memorial authorities. This does not
        /// create a parallel archive ledger or persistence path.
        /// </summary>
        public IReadOnlyList<ArchiveEntry> BuildShelterArchiveProjection()
        {
            SetupJournal();
            SetupMemorial();
            return ShelterArchiveSystem.ProjectCanonicalSources(_journal, _memorial);
        }
    }
}
