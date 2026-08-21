using System;
using System.Collections.Generic;
using Ashfall.Core.Survivors;

namespace Ashfall.Core
{
    [Serializable]
    public sealed class ApprenticeshipState
    {
        public string systemId = ApprenticeshipSystem.SystemId;
        public List<Apprenticeship> activePairs = new List<Apprenticeship>();
        public List<string> completedSkillIds = new List<string>();
    }

    [Serializable]
    public sealed class Apprenticeship
    {
        public string pairId = string.Empty;
        public string mentorId = string.Empty;
        public string apprenticeId = string.Empty;
        public string targetSkillId = string.Empty;
        public float progressXp;
        public float targetXp = 100f;
        public int dayStarted = -1;
        public bool isComplete;
        public bool isCancelled;
        public string milestonePerkId = string.Empty;
    }

    public sealed class ApprenticeshipSystem
    {
        public const string SystemId = "apprenticeship";
        private ApprenticeshipState _state = new ApprenticeshipState();
        private readonly ISeededRng _rng;
        private readonly ILog _log;
        private readonly SkillProgressionSystem _skills;
        private readonly DutyRosterSystem _roster;
        private readonly SurvivorRelationsSystem _relations;
        private int _currentDay;

        public ApprenticeshipState State => _state;
        public event Action<Apprenticeship> OnApprenticeshipCompleted;
        public event Action OnApprenticeshipChanged;

        public ApprenticeshipSystem(
            ISeededRng rng,
            SkillProgressionSystem skills,
            DutyRosterSystem roster,
            SurvivorRelationsSystem relations,
            ILog log = null!)
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _skills = skills ?? throw new ArgumentNullException(nameof(skills));
            _roster = roster ?? throw new ArgumentNullException(nameof(roster));
            _relations = relations ?? throw new ArgumentNullException(nameof(relations));
            _log = log ?? NullLog.Instance;
        }

        public ActionResult StartPair(string mentorId, string apprenticeId, string targetSkillId, float targetXp = 100f)
        {
            // Check duty roster availability
            if (_roster.GetAssignment(mentorId) != null)
                return ActionResult.Blocked("mentor_busy", "apprentice.mentor_busy");
            if (_roster.GetAssignment(apprenticeId) != null)
                return ActionResult.Blocked("apprentice_busy", "apprentice.apprentice_busy");

            // Check eligibility
            float mentorSkill = _skills.GetXp(mentorId, targetSkillId);
            if (mentorSkill < 30f)
                return ActionResult.Blocked("mentor_unqualified", "apprentice.mentor_unqualified");

            if (_state.activePairs.Exists(p => p.mentorId == mentorId && p.apprenticeId == apprenticeId))
                return ActionResult.Blocked("pair_exists", "apprentice.pair_exists");

            var pair = new Apprenticeship
            {
                pairId = $"appr_{_currentDay}_{mentorId}_{apprenticeId}",
                mentorId = mentorId, apprenticeId = apprenticeId,
                targetSkillId = targetSkillId, targetXp = targetXp,
                dayStarted = _currentDay
            };
            _state.activePairs.Add(pair);
            OnApprenticeshipChanged?.Invoke();
            return ActionResult.Success("apprentice.pair_started");
        }

        public ActionResult CancelPair(string pairId)
        {
            var pair = _state.activePairs.Find(p => p.pairId == pairId);
            if (pair == null || pair.isComplete || pair.isCancelled)
                return ActionResult.Blocked("no_pair", "apprentice.no_pair");

            _state.activePairs.Remove(pair);
            OnApprenticeshipChanged?.Invoke();
            return ActionResult.Success("apprentice.pair_cancelled");
        }

        public void TickDay(int day)
        {
            _currentDay = day;

            foreach (var pair in _state.activePairs)
            {
                if (pair.isComplete || pair.isCancelled) continue;

                // Deterministic XP gain: 10 XP per day
                float xpGain = 10f;
                pair.progressXp += xpGain;

                if (pair.progressXp >= pair.targetXp)
                {
                    pair.isComplete = true;
                    _state.completedSkillIds.Add(pair.targetSkillId);

                    // Finalize skill
                    _skills.RecordAction(new SimpleSkillActor(pair.apprenticeId), pair.targetSkillId, pair.targetXp, _currentDay);

                    // Relationship bonus
                    _relations.ModifyAffinity(pair.mentorId, pair.apprenticeId, 10f);

                    _log.Info($"[Apprentice] {pair.apprenticeId} completed {pair.targetSkillId} under {pair.mentorId}");
                    OnApprenticeshipCompleted?.Invoke(pair);
                }
            }

            _state.activePairs.RemoveAll(p => p.isCancelled);
            OnApprenticeshipChanged?.Invoke();
        }

        public List<Apprenticeship> GetActivePairs() => _state.activePairs.FindAll(p => !p.isComplete && !p.isCancelled);

        public ApprenticeshipState CaptureState() => _state;
        public void RestoreState(ApprenticeshipState saved)
        {
            if (saved == null) return;
            _state = saved;
            OnApprenticeshipChanged?.Invoke();
        }
    }
}
