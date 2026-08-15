using System;
using System.Collections.Generic;
using AtomicWar._Game.Survivors;
using Ashfall.Core.Journal;

namespace AtomicWar._Game.Events
{
    /// <summary>
    /// Unity-side journal. The entire domain — entries, dedupe via
    /// <see cref="KnowledgeBase"/>, tabs, unread tracking, save capture/restore — now lives in
    /// <see cref="Ashfall.Core.Journal.JournalSystem"/> and is shared with the Godot host. This
    /// subclass carries only what genuinely depends on Unity-side gameplay types: the Lorekeeper
    /// morale passive and the News Anchor lore-output perk, both of which need
    /// <see cref="PersonalQuestSystem"/>, <see cref="NeedsSystem"/> and <see cref="Survivor"/>.
    ///
    /// Keeping the class name and namespace means every existing call site compiles unchanged
    /// while inheriting the shared behaviour, so the two copies can no longer drift apart.
    /// </summary>
    public class JournalSystem : Ashfall.Core.Journal.JournalSystem
    {
        private PersonalQuestSystem _personalQuests;
        private Func<IReadOnlyList<Survivor>> _getSurvivors;
        private NeedsSystem _needsSystem;

        /// <summary>Prompt #245 — Lorekeeper passive journal morale.</summary>
        public void BindPersonalQuests(
            PersonalQuestSystem personalQuests,
            Func<IReadOnlyList<Survivor>> getSurvivors = null)
        {
            _personalQuests = personalQuests;
            _getSurvivors = getSurvivors;
        }

        public void SetNeedsSystem(NeedsSystem ns) => _needsSystem = ns;

        /// <summary>Apply Lorekeeper bunker-wide journal morale boost once per day.</summary>
        public void ApplyLorekeeperMoraleTick()
        {
            if (_personalQuests == null || _getSurvivors == null) return;
            var list = _getSurvivors();
            float boost = _personalQuests.GetJournalMoraleBoost(list);
            if (boost <= 0f || list == null) return;
            for (int i = 0; i < list.Count; i++)
            {
                var s = list[i];
                if (s == null || !s.IsAlive) continue;
                if (_needsSystem != null)
                    _needsSystem.Modify(s, NeedKind.Morale, boost);
                else
                    s.Needs.Morale = UnityEngine.Mathf.Clamp(s.Needs.Morale + boost, 0f, 100f);
            }
        }

        /// <summary>#281 News Anchor: dramatically increases lore output (extra journal lines).</summary>
        public int TickNewsAnchorJournalSpam(int day)
        {
            if (_personalQuests == null || _getSurvivors == null) return 0;
            var list = _getSurvivors();
            if (list == null) return 0;
            int written = 0;
            for (int i = 0; i < list.Count; i++)
            {
                var s = list[i];
                if (s == null || !s.IsAlive) continue;
                int n = _personalQuests.GetJournalEntriesPerDay(s);
                for (int k = 0; k < n; k++)
                {
                    string id = "anchor_broadcast_" + s.Id + "_d" + day + "_" + k;
                    if (TryAddRawEntry(id, "The airwaves fill with their voice. Day " + day + ".", s, day) != null)
                        written++;
                }
            }
            return written;
        }
    }
}
