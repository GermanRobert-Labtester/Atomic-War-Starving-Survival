using Ashfall.Core;
using Ashfall.Core.NpcArcs;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Plan 52 — recurring-NPC arcs. Thin wiring only: the arc system is a
    /// stateless projection over the expansion-quest ledger and the survivor
    /// roster, so there is no arc save section (a save that round-trips
    /// quests + roster resolves identical arc states). Host duties here:
    /// construct the resolver, suppress stale distress signals for
    /// dead/recruited/terminal NPCs, and advance an NPC's authored arc quest
    /// when one of their distress signals resolves.
    /// </summary>
    public partial class Main
    {
        private NpcArcSystem? _npcArcs;

        /// <summary>Resolved-NPC-state read model (UI/selftests). Constructed lazily.</summary>
        public NpcArcSystem? NpcArcs => _npcArcs;

        private void SetupNpcArcs()
        {
            if (_npcArcs != null) return;

            SetupExpansionQuests();
            SetupSurvivors();
            if (_expansionQuests == null || _survivors == null) return;

            _npcArcs = new NpcArcSystem(
                NpcArcCatalog.Load(_dataDir),
                () => _simDay,
                _expansionQuests.System,
                _survivors.Roster);

            // Dead / recruited / arc-terminal NPCs stop emitting fresh distress
            // signals — the radio never re-begs for someone already resolved.
            SetupRadio();
            if (_radio != null)
                _radio.DistressSystem.NpcSignalSuppressionFilter = _npcArcs.IsSignalSuppressed;

            // Resolving a distress signal advances the NPC's authored arc quest.
            _radio?.DistressSystem.OnSignalResolved += (def, state, resolution) =>
            {
                if (_npcArcs == null || def == null || string.IsNullOrEmpty(def.ResolveQuestId)) return;
                AdvanceArcQuestFromSignal(def.ResolveQuestId, resolution);
            };
        }

        private void AdvanceArcQuestFromSignal(string questId, string resolution)
        {
            if (_expansionQuests == null) return;
            var quests = _expansionQuests.System;
            int day = _simDay;

            if (!quests.IsStarted(questId))
                quests.StartQuest(questId, day);
            quests.MakeChoice(questId, string.Equals(resolution, "ResolvedRescued", System.StringComparison.Ordinal)
                ? "signal_rescued"
                : "signal_resolved", day);
            if (!quests.IsCompleted(questId))
                quests.CompleteQuest(questId, day);
        }
    }
}
