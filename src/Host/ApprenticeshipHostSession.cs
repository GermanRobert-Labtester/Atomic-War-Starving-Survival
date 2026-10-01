// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
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
    : HostSessionBase{
        public ApprenticeshipSystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;
        public Func<IReadOnlyList<ChildProfile>>? ChildrenProvider { get; set; }
        public Func<IReadOnlyList<string>>? MentorsProvider { get; set; }
        public Func<int>? DayProvider { get; set; }
        public Func<int>? ChapterOpenDayProvider { get; set; }
        public Func<bool>? ChapterTwoProvider { get; set; }
        public IReadOnlyList<ChildProfile> VocationalCandidates()
        {
            var result = new List<ChildProfile>();
            if (ChapterTwoProvider?.Invoke() != true) return result;
            int day = DayProvider?.Invoke() ?? 0;
            int opened = ChapterOpenDayProvider?.Invoke() ?? 0;
            foreach (var child in ChildrenProvider?.Invoke() ?? Array.Empty<ChildProfile>())
                if (System.CanOfferVocationalPair(child, day, opened)) result.Add(child);
            return result;
        }

        public ActionResult RespondVocationalPair(string childId, string mentorId, string mentorshipId, bool accept)
        {
            if (ChapterTwoProvider?.Invoke() != true)
            {
                LastEvent = "Pairing unavailable: chapter_not_open";
                RaiseStateChanged();
                return ActionResult.Blocked("chapter_not_open", "apprentice.chapter_not_open");
            }
            ChildProfile? child = null;
            foreach (var candidate in ChildrenProvider?.Invoke() ?? Array.Empty<ChildProfile>())
                if (candidate.ChildId == childId) { child = candidate; break; }
            if (child == null)
            {
                LastEvent = "Pairing unavailable: unknown_child";
                RaiseStateChanged();
                return ActionResult.Blocked("unknown_child", "apprentice.unknown_child");
            }
            bool mentorExists = false;
            foreach (var id in MentorsProvider?.Invoke() ?? Array.Empty<string>())
                if (id == mentorId) { mentorExists = true; break; }
            if (accept && (!mentorExists || mentorId == childId))
            {
                LastEvent = "Pairing unavailable: mentor_unavailable";
                RaiseStateChanged();
                return ActionResult.Blocked("mentor_unavailable", "apprentice.mentor_unavailable");
            }
            var result = System.RespondVocationalPair(child, mentorId, mentorshipId, accept,
                DayProvider?.Invoke() ?? 0, ChapterOpenDayProvider?.Invoke() ?? 0);
            LastEvent = result.IsSuccess
                ? (accept ? $"{child.Name} accepted the mentorship." : $"{child.Name} declined the mentorship this quarter.")
                : $"Pairing unavailable: {result.FailureCode}";
            RaiseStateChanged();
            return result;
        }
        public ApprenticeshipHostSession(ApprenticeshipSystem system)
        {
            // The campaign composer supplies the shared skill progression,
            // roster, and relations authorities. Creating private fallbacks
            // here would make apprenticeship progress invisible to duty,
            // trapping, and the saved campaign ledger.
            System = system ?? throw new ArgumentNullException(nameof(system));

            System.OnApprenticeshipCompleted += pair =>
            {
                LastEvent = $"[Apprenticeship] GRADUATION: {pair.apprenticeId} has mastered {pair.targetSkillId}!";
                RaiseStateChanged();
            };

            System.OnApprenticeshipChanged += () =>
            {
                RaiseStateChanged();
            };
        }

        public ActionResult StartPair(string mentorId, string apprenticeId, string targetSkillId, float targetXp = 100f)
        {
            var res = System.StartPair(mentorId, apprenticeId, targetSkillId, targetXp);
            LastEvent = res.IsSuccess
                ? $"Assigned {apprenticeId} under mentor {mentorId} for {targetSkillId}"
                : $"Pairing unavailable: {res.FailureCode}";
            RaiseStateChanged();
            return res;
        }

        public ActionResult CancelPair(string pairId)
        {
            var res = System.CancelPair(pairId);
            LastEvent = res.IsSuccess
                ? $"Cancelled apprenticeship pair {pairId}"
                : $"Cancel refused: {res.FailureCode}";
            RaiseStateChanged();
            return res;
        }

        /// <summary>
        /// Promotes a retained acting-eligible apprentice (vocational mentor
        /// lost) to the acting designation; the pair leaves the active ledger.
        /// </summary>
        public ActionResult AssignActingDesignation(string pairId)
        {
            var res = System.AssignActingDesignation(pairId);
            LastEvent = res.IsSuccess
                ? $"Assigned the acting designation for {pairId}"
                : $"Designation refused: {res.FailureCode}";
            RaiseStateChanged();
            return res;
        }

        public void TickDay(int day)
        {
            System.TickDay(day);
            RaiseStateChanged();
        }

    }
}
