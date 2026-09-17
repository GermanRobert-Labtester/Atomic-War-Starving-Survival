// SPDX-License-Identifier: MIT
using System;
using Godot;
using Ashfall.Core;

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
            if (res.IsSuccess)
            {
                LastEvent = $"Assigned {apprenticeId} under mentor {mentorId} for {targetSkillId}";
            }
            return res;
        }

        public ActionResult CancelPair(string pairId)
        {
            var res = System.CancelPair(pairId);
            if (res.IsSuccess)
            {
                LastEvent = $"Cancelled apprenticeship pair {pairId}";
            }
            return res;
        }

        public void TickDay(int day)
        {
            System.TickDay(day);
            RaiseStateChanged();
        }

        public override void Save()
        {
            if (!IsDirty) return;
            ApprenticeshipSaveStore.TrySave(System.CaptureState());
            base.Save();
        }
    }
}
