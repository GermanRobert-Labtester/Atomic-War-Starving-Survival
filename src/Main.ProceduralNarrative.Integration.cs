// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Factions;
using Ashfall.Core.Narrative;
using Ashfall.Core.Quests;
using Ashfall.Core.Random;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private ProceduralNarrativeHostSession? _proceduralNarrative169;
        private bool _proceduralNarrative169Dirty;

        /// <summary>
        /// CORE-MECH W7 — the reopen grammar. A failed or abandoned thread can be
        /// revived when the authored prerequisites and the campaign day say so. The
        /// quest runtime owns the state machine and the exactly-once guard; the
        /// eligibility is supplied here because the authored prerequisites and the
        /// flag ledger live in the host layer. Flag ids are namespaced
        /// <c>quest.reopened.*</c>, the namespace W11's one-shot primitive later
        /// formalizes.
        /// </summary>
        internal void EvaluateQuestReopenOpportunities(int day)
        {
            var runtime = _proceduralNarrative169?.QuestRuntime;
            if (runtime == null) return;

            var candidates = runtime.GetReopenCandidates(quest => IsQuestReopenEligible(quest, day));
            foreach (var quest in candidates)
            {
                if (!runtime.Reopen(quest.instanceId, day, q => IsQuestReopenEligible(q, day)))
                    continue;

                SetupJournal();
                _journal?.TryAddRawEntry(
                    $"quest_reopened_{quest.instanceId}_{day}",
                    $"\"{quest.titleKey}\" reopens — its prerequisites are satisfied again.",
                    null!, day);
            }
        }

        /// <summary>
        /// Data-driven eligibility: an authored prerequisite flag (if the quest binds
        /// one) and the authored minimum day. With no authored prerequisite the quest
        /// reopens as soon as it has failed or been abandoned — the grammar still
        /// gives the player their thread back.
        /// </summary>
        private bool IsQuestReopenEligible(Ashfall.Core.Quests.QuestInstanceState quest, int day)
        {
            if (quest == null) return false;
            if (quest.status != Ashfall.Core.Quests.QuestLifecycleState.Failed &&
                quest.status != Ashfall.Core.Quests.QuestLifecycleState.Abandoned) return false;

            string prerequisite = quest.actorBindings.TryGetValue("prereq_quest_id", out var p) ? p : string.Empty;
            if (!string.IsNullOrEmpty(prerequisite) && !_consequenceLedger.IsSet("quest." + prerequisite))
                return false;

            if (quest.actorBindings.TryGetValue("min_day", out var minDayText)
                && int.TryParse(minDayText, out int minDay) && day < minDay)
                return false;

            return true;
        }

        private void SaveProceduralNarrative()
        {
            if (_proceduralNarrative169 == null) return;
            var payload = new ProceduralNarrativeSaveState
            {
                narrative = _proceduralNarrative169.CaptureNarrativeState(),
                quests = _proceduralNarrative169.CaptureQuestState()
            };
            CaptureSection(ProceduralNarrativeSaveStore.SectionName,
                ProceduralNarrativeSaveStore.TryCapturePersisted(payload));
        }

        private void TickPlan169Narrative(int day)
        {
            SetupPlans166To169();
            _proceduralNarrative169?.AdvanceDay(day);
            // CORE-MECH W7: after the day's expiry pass, evaluate the reopen
            // grammar so a thread lost yesterday can come back today.
            EvaluateQuestReopenOpportunities(day);
        }

        /// <summary>
        /// Plan 171 — player-facing generation command. The snapshot is
        /// assembled from the existing survivor, expedition/map, inventory,
        /// and canonical quest-runtime owners; the procedural narrative host
        /// then performs the deterministic JSON-template selection and
        /// registers the accepted instance in QuestRuntimeCoordinator.
        /// </summary>
        public bool TryGenerateProceduralQuest()
        {
            SetupPlans166To169();
            SetupExpeditions();
            SetupSurvivors();
            SetupInventory();
            if (_proceduralNarrative169 == null || _campaignDay == null)
                return false;

            var snapshot = new NarrativeWorldSnapshot
            {
                day = Math.Max(1, _simDay),
                aliveSurvivorIds = (_survivors?.RosterState ?? new List<SurvivorNeedsState>())
                    .Where(s => s != null && s.IsAliveState)
                    .Select(s => s.Id)
                    .Where(id => !string.IsNullOrWhiteSpace(id))
                    .OrderBy(id => id, StringComparer.Ordinal)
                    .ToList(),
                knownLocationIds = (_expeditions?.Engine.CaptureKnownLocations() ?? new List<string>())
                    .Where(id => !string.IsNullOrWhiteSpace(id))
                    .Distinct(StringComparer.Ordinal)
                    .OrderBy(id => id, StringComparer.Ordinal)
                    .ToList(),
                obtainableItemIds = (_inventory?.Catalog.Ids ?? Array.Empty<string>())
                    .Where(id => !string.IsNullOrWhiteSpace(id))
                    .OrderBy(id => id, StringComparer.Ordinal)
                    .ToList(),
                activeQuests = _proceduralNarrative169.QuestRuntime.BuildReadModel().ToList()
            };

            // Fork by day and a fixed command lane so UI retries do not consume
            // the campaign stream or alter unrelated deterministic systems.
            var rng = _campaignDay.Rng.Fork(CampaignStreamIds.Narrative, snapshot.day, actionIndex: 171);
            bool accepted = _proceduralNarrative169.GenerateAndRegister(snapshot, rng);
            if (accepted) _proceduralNarrative169Dirty = true;
            return accepted;
        }

        private sealed class Plan169NarrativeDayOwner : Ashfall.Core.Campaign.IDayAdvanceOwner
        {
            private readonly Main _main;
            public Plan169NarrativeDayOwner(Main main) => _main = main;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _main.TickPlan169Narrative(day);
                events.Add(new DayStateChangeEvent("procedural_narrative_ticked", "plan_169", null, null, day));
            }
        }

    }
}
