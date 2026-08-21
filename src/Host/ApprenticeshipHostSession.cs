using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Host session for ApprenticeshipSystem.
    /// Manages master-apprentice pairings, mentor qualification checks, daily training ticks, and skill graduations.
    /// </summary>
    public sealed class ApprenticeshipHostSession
    {
        public ApprenticeshipSystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;

        public event Action? StateChanged;

        public ApprenticeshipHostSession(ApprenticeshipSystem system)
        {
            if (system == null)
            {
                var skills = new SkillProgressionSystem();
                var roster = new DutyRosterSystem();
                var relations = new SurvivorRelationsSystem(new SeededRng(1986));
                system = new ApprenticeshipSystem(new SeededRng(1986), skills, roster, relations, new GodotLog());
            }
            System = system;

            System.OnApprenticeshipCompleted += pair =>
            {
                LastEvent = $"[Apprenticeship] GRADUATION: {pair.apprenticeId} has mastered {pair.targetSkillId}!";
                StateChanged?.Invoke();
            };

            System.OnApprenticeshipChanged += () =>
            {
                StateChanged?.Invoke();
            };
        }

        public ActionResult StartPair(string mentorId, string apprenticeId, string targetSkillId, float targetXp = 100f)
        {
            var res = System.StartPair(mentorId, apprenticeId, targetSkillId, targetXp);
            if (res.IsSuccess)
            {
                LastEvent = $"Assigned {apprenticeId} under mentor {mentorId} for {targetSkillId}";
                StateChanged?.Invoke();
            }
            return res;
        }

        public ActionResult CancelPair(string pairId)
        {
            var res = System.CancelPair(pairId);
            if (res.IsSuccess)
            {
                LastEvent = $"Cancelled apprenticeship pair {pairId}";
                StateChanged?.Invoke();
            }
            return res;
        }

        public void TickDay(int day)
        {
            System.TickDay(day);
            StateChanged?.Invoke();
        }
    }
}
