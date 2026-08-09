using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.Core
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
            _state.finderId = finderId;
            _state.ownerOfDiaryId = ownerOfDiaryId;
            _state.containsConfession = hasConfession;
            _state.resolved = false;

            OnDiaryFound?.Invoke(finderId, ownerOfDiaryId);
        }

        /// <summary>
        /// The finder (or another survivor) starts blackmailing the diary owner,
        /// demanding extra rations.
        /// </summary>
        public void StartBlackmail(string blackmailerId, string targetId)
        {
            if (!_state.containsConfession) return;

            _state.blackmailerId = blackmailerId;
            _state.targetId = targetId;
            _state.blackmailActive = true;

            OnBlackmailStarted?.Invoke(blackmailerId, targetId);
        }

        /// <summary>
        /// Resolve the blackmail. If the player intervened, it ends cleanly.
        /// If not, extra rations are consumed by the blackmailer.
        /// </summary>
        public void ResolveBlackmail(string targetId, bool playerIntervened)
        {
            if (!_state.blackmailActive) return;

            // If player did not intervene, the blackmailer consumed extra rations
            // (the caller is responsible for deducting rations from the supply system)
            if (!playerIntervened)
            {
                GameLog.Log($"[FoundDiary] Blackmail unresolved by player. Extra rations consumed by {_state.blackmailerId}.");
            }

            _state.blackmailActive = false;
            _state.resolved = true;

            OnBlackmailResolved?.Invoke(targetId);
        }

        public FoundDiaryState CaptureState()
        {
            return _state;
        }

        public void RestoreState(FoundDiaryState state)
        {
            if (state == null) return;
            _state.containsConfession = state.containsConfession;
            _state.blackmailActive = state.blackmailActive;
            _state.finderId = state.finderId;
            _state.ownerOfDiaryId = state.ownerOfDiaryId;
            _state.blackmailerId = state.blackmailerId;
            _state.targetId = state.targetId;
            _state.resolved = state.resolved;
        }
    }
}
