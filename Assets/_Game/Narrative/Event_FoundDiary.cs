using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.Narrative
{
    [Serializable]
    public class FoundDiaryState
    {
        public string eventId = "event_found_diary";
        public bool containsConfession;
        public bool blackmailActive;
        public string finderId;
        public string ownerOfDiaryId;
        public string blackmailerId;
        public string targetId;
        public bool resolved;
    }

    public class Event_FoundDiary
    {
        public event Action<string, string> OnDiaryFound;
        public event Action<string, string> OnBlackmailStarted;
        public event Action<string> OnBlackmailResolved;

        private readonly FoundDiaryState _state;

        public Event_FoundDiary()
        {
            _state = new FoundDiaryState();
        }

        public Event_FoundDiary(FoundDiaryState state)
        {
            _state = state ?? new FoundDiaryState();
        }

        /// <summary>
        /// A finder discovers another survivor's diary. If it contains a confession,
        /// blackmail becomes possible.
        /// </summary>
        public void DiscoverDiary(string finderId, string ownerOfDiaryId, bool hasConfession)
        {
            var state = _state;
            state.finderId = finderId;
            state.ownerOfDiaryId = ownerOfDiaryId;
            state.containsConfession = hasConfession;
            state.resolved = false;

            var handler = OnDiaryFound;
            if (handler != null)
            {
                handler(finderId, ownerOfDiaryId);
            }
        }

        /// <summary>
        /// The finder (or another survivor) starts blackmailing the diary owner,
        /// demanding extra rations.
        /// </summary>
        public void StartBlackmail(string blackmailerId, string targetId)
        {
            var state = _state;
            if (!state.containsConfession) return;

            state.blackmailerId = blackmailerId;
            state.targetId = targetId;
            state.blackmailActive = true;

            var handler = OnBlackmailStarted;
            if (handler != null)
            {
                handler(blackmailerId, targetId);
            }
        }

        /// <summary>
        /// Resolve the blackmail. If the player intervened, it ends cleanly.
        /// If not, extra rations are consumed by the blackmailer.
        /// </summary>
        public void ResolveBlackmail(string targetId, bool playerIntervened)
        {
            var state = _state;
            if (!state.blackmailActive) return;

            // If player did not intervene, the blackmailer consumed extra rations
            // (the caller is responsible for deducting rations from the supply system)
            if (!playerIntervened)
            {
                GameLog.Log($"[FoundDiary] Blackmail unresolved by player. Extra rations consumed by {state.blackmailerId}.");
            }

            state.blackmailActive = false;
            state.resolved = true;

            var handler = OnBlackmailResolved;
            if (handler != null)
            {
                handler(targetId);
            }
        }

        public FoundDiaryState CaptureState()
        {
            return _state;
        }

        public void RestoreState(FoundDiaryState state)
        {
            if (state == null) return;
            var s = _state;
            s.containsConfession = state.containsConfession;
            s.blackmailActive = state.blackmailActive;
            s.finderId = state.finderId;
            s.ownerOfDiaryId = state.ownerOfDiaryId;
            s.blackmailerId = state.blackmailerId;
            s.targetId = state.targetId;
            s.resolved = state.resolved;
        }
    }
}
