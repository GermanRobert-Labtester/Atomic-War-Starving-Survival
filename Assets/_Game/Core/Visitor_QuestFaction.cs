using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class QuestFactionState
    {
        public string cardId = "visitor_quest_faction";
        public string displayName = "Quest-Specific Target Faction";
        public string activeQuestId;
        public string targetLocationId;
        public bool isSpawned = false;
    }

    /// <summary>
    /// Prompt #365: Faction: Quest-Specific Factions.
    /// Injected into VisitorRNGSystem when a active Personal Quest exists.
    /// Ensures narrative target characters spawn at their designated locations.
    /// </summary>
    public class Visitor_QuestFaction
    {
        private QuestFactionState _state = new QuestFactionState();

        public event Action<QuestFactionState, string, string> OnQuestFactionInjected;

        public QuestFactionState State => _state;

        public bool TryInjectQuestVisitor(string questId, string targetLocationId, VisitorRNGSystem visitorSystem)
        {
            if (string.IsNullOrEmpty(questId) || string.IsNullOrEmpty(targetLocationId) || visitorSystem == null)
                return false;

            _state.activeQuestId = questId;
            _state.targetLocationId = targetLocationId;
            _state.isSpawned = true;

            var nodeState = visitorSystem.GetNodeState(targetLocationId);
            if (nodeState != null)
            {
                nodeState.assignedVisitorCardId = _state.cardId;
                nodeState.assignedVisitorTitle = $"Quest Target ({questId})";
                nodeState.primaryFactionId = "quest_target";
            }

            OnQuestFactionInjected?.Invoke(_state, questId, targetLocationId);
            return true;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public QuestFactionState CaptureState() => _state;

        public void RestoreState(QuestFactionState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
