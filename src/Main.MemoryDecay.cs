// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 185 — Memory & Knowledge Decay host wiring.
// The pure domain MemoryDecaySystem is the authority for memory clarity,
// gradual cognitive degradation, reinforcement, certification, and archive preservation.
// ============================================================================

using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core.Cognition;

using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private MemoryDecayHostSession? _memoryDecay;
        private bool _memoryDecayDirty;

        public MemoryDecayHostSession? MemoryDecay => _memoryDecay;

        public void SetupMemoryDecay()
        {
            if (_memoryDecay != null) return;

            var saved = MemoryDecaySaveStore.TryLoad();
            _memoryDecay = MemoryDecayHostSession.Create(saved);
            _memoryDecay.StateChanged += () => _memoryDecayDirty = true;

            // Load authored decay rates catalog
            string catalogPath = CatalogPath.ResolveCatalog("memory_decay_rates.json");
            var catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (catalogIo.FileExists(catalogPath))
            {
                _memoryDecay.LoadCatalog(catalogIo.ReadAllText(catalogPath));
            }
        }

        public MemoryRecord RegisterSurvivorMemory(
            string survivorId,
            MemoryDomain domain,
            string referenceId,
            int day,
            float initialStrength = 100f,
            bool isCertified = false,
            bool isPreserved = false)
        {
            SetupMemoryDecay();
            return _memoryDecay!.RegisterOrUpdate(survivorId, domain, referenceId, day, initialStrength, isCertified, isPreserved);
        }

        public bool ReinforceSurvivorMemory(
            string survivorId,
            MemoryDomain domain,
            string referenceId,
            int day,
            ReinforcementType type = ReinforcementType.Review,
            float boostAmount = 25f)
        {
            SetupMemoryDecay();
            return _memoryDecay!.Reinforce(survivorId, domain, referenceId, day, type, boostAmount);
        }

        public MemoryDecayCensus GetMemoryDecayCensus() =>
            _memoryDecay?.Census ?? default;

        public void TickMemoryDecay(int day)
        {
            SetupMemoryDecay();
            _memoryDecay!.TickDay(day);
        }

        public void SaveMemoryDecay()
        {
            if (_memoryDecay == null) return;
            var state = _memoryDecay.System.CaptureState();
            MemoryDecaySaveStore.TrySave(state);
            if (CaptureSection(
                    MemoryDecaySaveStore.SectionName,
                    MemoryDecaySaveStore.TryCapturePersisted(state)))
            {
                _memoryDecayDirty = false;
            }
        }

        public void FlushMemoryDecayIfDirty()
        {
            if (_memoryDecayDirty)
            {
                SaveMemoryDecay();
            }
        }

        public void ResetMemoryDecay()
        {
            _memoryDecay = null;
            _memoryDecayDirty = false;
        }
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

    }
}
