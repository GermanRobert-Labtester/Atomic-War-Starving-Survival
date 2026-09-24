// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 177 — Survivor Dream & Sleep Event System host wiring.
// Pure domain DreamSystem governs nocturnal dream generation, nightmare compounding,
// rest quality impact, and psychological dream interpretations.
// ============================================================================

using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Random;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private DreamHostSession? _dreamSystem;
        private bool _dreamSystemDirty;

        public DreamHostSession? DreamSystemSession => _dreamSystem;

        public void SetupSurvivorDreams()
        {
            if (_dreamSystem != null) return;

            var saved = DreamSaveStore.TryLoad();
            _dreamSystem = DreamHostSession.Create(saved);
            _dreamSystem.StateChanged += () => _dreamSystemDirty = true;

            // Load authored dream templates catalog
            string catalogPath = CatalogPath.ResolveCatalog("dream_templates.json");
            var catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (catalogIo.FileExists(catalogPath))
            {
                _dreamSystem.LoadCatalog(catalogIo.ReadAllText(catalogPath));
            }
        }

        public DreamSleepResult ProcessSurvivorDreamCycle(string survivorId, float trauma, float morale, int currentDay, ISeededRng rng, bool forceDream = false)
        {
            SetupSurvivorDreams();
            return _dreamSystem!.ProcessSleepCycle(survivorId, trauma, morale, currentDay, rng, forceDream);
        }

        public bool InterpretSurvivorDream(string survivorId, string recordId, string interpretationChoice)
        {
            SetupSurvivorDreams();
            return _dreamSystem!.InterpretDream(survivorId, recordId, interpretationChoice);
        }

        public List<DreamRecord> GetSurvivorDreamHistory(string survivorId)
        {
            SetupSurvivorDreams();
            return _dreamSystem!.GetDreamHistory(survivorId);
        }

        public int GetSurvivorConsecutiveNightmares(string survivorId)
        {
            SetupSurvivorDreams();
            return _dreamSystem!.GetConsecutiveNightmares(survivorId);
        }

        public DreamCensus GetDreamCensus() =>
            _dreamSystem?.Census ?? default;

        public void TickSurvivorDreams(int day)
        {
            SetupSurvivorDreams();
            SetupSurvivors();
            var roster = _survivors?.RosterState;
            if (roster == null) return;

            var rng = _campaignDay != null
                ? _campaignDay.Rng.Fork(CampaignStreamIds.Psychology, day, 77)
                : null;
            if (rng == null) return;

            var mental = EnsureSurvivorMentalHealth();

            for (int i = 0; i < roster.Count; i++)
            {
                var s = roster[i];
                if (s == null || !s.IsAliveState) continue;

                // Dream inputs stay on canonical owners: mental-health stress
                // is the trauma proxy, while morale comes from NeedsSystem.
                float trauma = mental != null && mental.HasRecord(s.Id)
                    ? mental.GetOrCreateRecord(s.Id).stressPermille / 10f
                    : 0f;
                float morale = _survivors?.Needs.Get(s.Id)?.Morale ?? 50f;

                var res = _dreamSystem!.ProcessSleepCycle(s.Id, trauma, morale, day, rng);
                if (res.HadDream && res.Record != null)
                {
                    if (Math.Abs(res.TraumaDelta) > 0.01f && mental != null)
                    {
                        int stressDelta = (int)Math.Round(res.TraumaDelta * 10f);
                        if (stressDelta > 0) mental.AddStress(s.Id, stressDelta, "dream.trauma");
                        else mental.ReduceStress(s.Id, -stressDelta);
                    }
                    if (Math.Abs(res.MoraleDelta) > 0.01f)
                    {
                        _survivors?.Needs.Modify(s.Id, NeedKind.Morale, res.MoraleDelta);
                    }
                }
            }
        }

        public void SaveSurvivorDreams()
        {
            if (_dreamSystem == null) return;
            var state = _dreamSystem.System.CaptureState();
            DreamSaveStore.TrySave(state);
            if (CaptureSection(
                    DreamSaveStore.SectionName,
                    DreamSaveStore.TryCapturePersisted(state)))
            {
                _dreamSystemDirty = false;
            }
        }

        public void FlushSurvivorDreamsIfDirty()
        {
            if (_dreamSystemDirty)
            {
                SaveSurvivorDreams();
            }
        }

        public void ResetSurvivorDreams()
        {
            _dreamSystem = null;
            _dreamSystemDirty = false;
        }
    }
}
