// SPDX-License-Identifier: MIT
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.MoralChoice;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        // ── Moral choice ("The Weight of Survival") host wiring ──
        // The score is invisible by design: hosts read CurrentBand and the
        // threshold events, never the raw number.
        private MoralChoiceSystem _moralChoice = null!;
        private List<MoralChoiceQuestDefinition> _moralChoiceDefs = new List<MoralChoiceQuestDefinition>();
        private bool _moralChoiceDirty;

        // ── Branching / gossip / faction reactions (Phase 2 data) ──
        private MoralChoiceChainData _moralChainData = new MoralChoiceChainData();
        private MoralChoiceGossipData _moralGossipData = new MoralChoiceGossipData();
        private MoralChoiceFactionReactionsData _moralFactionReactions = new MoralChoiceFactionReactionsData();
        private MoralChoiceFlagDefinitions _moralFlagDefs = new MoralChoiceFlagDefinitions();
        private MoralChoiceGossipRuntime _moralGossipRuntime = null!;

        /// <summary>
        /// Fixed world seed so every host agrees on unseeded rolls; per-save
        /// outcome rolls and propagation days are stored in the ledger DTO.
        /// </summary>
        private const int MoralChoiceSeed = 20260825;

        private void SetupMoralChoice()
        {
            if (_moralChoice != null) return;
            SetupJournal();
            SetupCampaignDay();
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            _moralChoice = new MoralChoiceSystem(_campaignDay.Rng.GetStream(Ashfall.Core.Random.CampaignStreamIds.MoralChoice).Rng, flags: _consequenceLedger);
            _moralChoiceDefs = MoralChoiceCatalogLoader.Load(_dataDir, fileIO, json);

            // Load branching chain quests and merge into the catalog
            var chainQuests = MoralChoiceBranchQuestCatalogLoader.Load(_dataDir, fileIO, json);
            _moralChoiceDefs.AddRange(chainQuests);

            // Load expansion quests and merge into the catalog
            var expansionQuests = MoralChoiceExpansionQuestCatalogLoader.Load(_dataDir, fileIO, json);
            _moralChoiceDefs.AddRange(expansionQuests);

            // Register all definitions into the Core system's authoritative catalog
            _moralChoice.RegisterQuests(_moralChoiceDefs);

            // Load chain architecture (branches, gates, echo quests)
            _moralChainData = MoralChoiceChainCatalogLoader.Load(_dataDir, fileIO, json);
            _moralChoice.InitializeChainData(_moralChainData);

            // Load gossip, faction reactions, and flag definitions
            _moralGossipData = MoralChoiceGossipCatalogLoader.Load(_dataDir, fileIO, json);
            _moralFactionReactions = MoralChoiceFactionReactionsCatalogLoader.Load(_dataDir, fileIO, json);
            _moralFlagDefs = MoralChoiceFlagCatalogLoader.Load(_dataDir, fileIO, json);
            _moralGossipRuntime = new MoralChoiceGossipRuntime(_moralGossipData, _campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.MoralChoice, 0, 1));

            _moralChoice.OnQuestResolved += WriteMoralChoiceJournalEntry;
            _moralChoice.OnQuestResolved += _ => _moralChoiceDirty = true;
            // Plan IV Task 5: a resolved trapping dilemma acks its pending
            // outbox fact so the outbox never re-surfaces it after restore.
            _moralChoice.OnQuestResolved += resolution =>
            {
                if (resolution.questId != null && resolution.questId.StartsWith(TrappingMoralQuestIdPrefix, StringComparison.Ordinal))
                    MarkTrappingMoralEventDelivered(resolution.questId);
            };
            _moralChoice.OnThresholdEventFired += WriteThresholdEventJournalEntry;
            _moralChoice.OnThresholdEventFired += _ => _moralChoiceDirty = true;
            _moralChoice.OnBranchLocked += WriteBranchLockoutJournalEntry;
            _moralChoice.OnBranchLocked += _ => _moralChoiceDirty = true;

            var save = MoralChoiceSaveStore.TryLoad();
            if (save != null)
            {
                try
                {
                    _moralChoice.RestoreState(save);
                    GD.Print($"[Ashfall Godot] Moral choice ledger restored " +
                             $"(day {save.lastReconciledDay}, {_moralChoice.QuestsResolved} resolved).");
                }
                catch (Exception e)
                {
                    GD.PrintErr($"[Ashfall Godot] Moral choice restore rejected: {e.Message}");
                }
            }
            GD.Print($"[Ashfall Godot] Moral choice ready. {_moralChoiceDefs.Count} quests " +
                     $"({_moralChainData.Branches.Count} branches, " +
                     $"{_moralGossipData.CampChatter.Neutral.Count} neutral chatter lines).");
        }

        public MoralChoiceSystem MoralChoice => _moralChoice;
        public IReadOnlyList<MoralChoiceQuestDefinition> MoralChoiceDefs => _moralChoiceDefs;

        public MoralChoiceQuestDefinition? GetMoralChoiceDef(string questId)
        {
            SetupMoralChoice();
            return _moralChoiceDefs.FirstOrDefault(
                d => string.Equals(d.Id, questId, StringComparison.Ordinal));
        }

        public List<MoralChoiceQuestDefinition> GetAvailableMoralChoices()
        {
            SetupMoralChoice();
            var list = new List<MoralChoiceQuestDefinition>();
            foreach (var d in _moralChoiceDefs)
            {
                // Plan IV Task 5: trapping-sourced dilemmas are excluded from
                // the standing offer pass — they surface only while their
                // moral-consequence fact is pending in the trapping outbox.
                if (d.Id != null && d.Id.StartsWith(TrappingMoralQuestIdPrefix, StringComparison.Ordinal))
                    continue;
                if (!_moralChoice.IsResolved(d.Id) &&
                    MoralChoiceSystem.IsAvailableOnDay(d, _simDay) &&
                    _moralChoice.IsChainQuestAccessible(d.Id, _simDay))
                {
                    list.Add(d);
                }
            }

            // Plan IV Task 5: surface pending trapping dilemmas in outbox
            // sequence order. The pending fact is the persistence owner until
            // the player resolves the dilemma in the moral ledger.
            if (_wildlifeTrapping != null)
            {
                var pending = _wildlifeTrapping.System.GetPendingEvents();
                foreach (var ev in pending)
                {
                    if (ev == null || !string.Equals(ev.kind, WildlifeTrappingEventKinds.MoralConsequence, StringComparison.Ordinal))
                        continue;
                    var def = GetMoralChoiceDef(ev.payloadId);
                    if (def == null || _moralChoice.IsResolved(def.Id)) continue;
                    if (!list.Any(l => string.Equals(l.Id, def.Id, StringComparison.Ordinal)))
                        list.Add(def);
                }
            }
            return list;
        }

        /// <summary>Catalog id prefix shared by all trapping-sourced moral dilemmas.</summary>
        public const string TrappingMoralQuestIdPrefix = "quest_moral_trap_prey_";

        /// <summary>
        /// Plan IV Task 5: ack every pending moral-consequence fact that maps
        /// to the given quest id. Called from the resolution event so the
        /// trapping outbox marks the fact delivered only after the moral
        /// ledger committed the resolution.
        /// </summary>
        private void MarkTrappingMoralEventDelivered(string questId)
        {
            if (_wildlifeTrapping == null) return;
            var pending = _wildlifeTrapping.System.GetPendingEvents();
            foreach (var ev in pending)
            {
                if (ev == null || !string.Equals(ev.kind, WildlifeTrappingEventKinds.MoralConsequence, StringComparison.Ordinal))
                    continue;
                if (!string.Equals(ev.payloadId, questId, StringComparison.Ordinal)) continue;
                _wildlifeTrapping.System.MarkEventDelivered(ev.eventId);
            }
        }

        public List<MoralChoiceQuestDefinition> GetResolvedMoralChoices()
        {
            SetupMoralChoice();
            var list = new List<MoralChoiceQuestDefinition>();
            foreach (var d in _moralChoiceDefs)
            {
                if (_moralChoice.IsResolved(d.Id))
                    list.Add(d);
            }
            return list;
        }

        public MoralChoiceResolution? GetMoralChoiceResolution(string questId)
        {
            SetupMoralChoice();
            if (_moralChoice.TryGetResolution(questId, out var res))
                return res;
            return null;
        }

        public IReadOnlyList<MoralChoiceQuestDefinition> GetDailyMoralOffers(int maxOffers = 1)
        {
            SetupMoralChoice();
            return _moralChoice.GetDailyOffers(_simDay, maxOffers);
        }

        /// <summary>
        /// Resolve a catalog quest by id. Returns false when the id is unknown
        /// or the quest is already resolved; the journal line is written by
        /// the event hook, and overnight settlement lands in TickSimDay.
        /// </summary>
        public bool TryResolveMoralChoice(string questId, int choiceIndex)
        {
            SetupMoralChoice();
            var def = _moralChoiceDefs.FirstOrDefault(
                d => string.Equals(d.Id, questId, StringComparison.Ordinal));
            if (def == null) return false;
            if (!_moralChoice.TryResolve(questId, choiceIndex, def.LocationId, _simDay, out _))
                return false;

            _moralChoiceDirty = true;
            AtomicWar.GodotApp.Audio.AudioManager.Instance?.PlayCue(AtomicWar.GodotApp.Audio.AudioCueCatalog.UiConfirm);
            return true;
        }

        /// <summary>Journal integration: one entry per resolution, arrow only — never the number.</summary>
        private void WriteMoralChoiceJournalEntry(MoralChoiceResolution resolution)
        {
            SetupJournal();
            string arrow = resolution.impactMark == "up" ? "🔺"
                : resolution.impactMark == "down" ? "🔻" : "⚪";
            _journal.TryAddRawEntry(resolution.questId, $"{arrow} {resolution.epitaph}", null!, resolution.resolvedDay);
            _journalDirty = true;
        }

        /// <summary>Branch lockout journal entry: a door has closed.</summary>
        private void WriteBranchLockoutJournalEntry(string lockedBranchId)
        {
            if (_moralChainData?.LockoutRules == null) return;
            var branch = _moralChainData.Branches.FirstOrDefault(
                b => string.Equals(b.Id, lockedBranchId, StringComparison.Ordinal));
            string branchName = branch?.DisplayName ?? lockedBranchId;
            string template = _moralChainData.LockoutRules.LockoutJournalTemplate;
            string text = template.Replace("{locked_branch_name}", branchName);

            SetupJournal();
            _journal.TryAddRawEntry($"branch_lockout_{lockedBranchId}", text, null!, _simDay);
            _journalDirty = true;
        }

        /// <summary>
        /// Get the faction reaction dialogue for a threshold event.
        /// Returns null if no reaction data exists for the event.
        /// </summary>
        private MoralThresholdReaction? GetFactionReaction(string eventId)
        {
            SetupMoralChoice();
            if (_moralFactionReactions.ThresholdReactions.TryGetValue(eventId, out var reaction))
                return reaction;
            return null;
        }

        /// <summary>
        /// Journal the authored faction reaction when a moral threshold fires.
        /// Uses the catalog journal line when present; otherwise a restrained fallback.
        /// </summary>
        private void WriteThresholdEventJournalEntry(string eventId)
        {
            var reaction = GetFactionReaction(eventId);
            string text = reaction != null && !string.IsNullOrWhiteSpace(reaction.JournalEntry)
                ? reaction.JournalEntry
                : $"Threshold crossed: {eventId}.";

            SetupJournal();
            _journal.TryAddRawEntry($"moral_threshold_{eventId}", text, null!, _simDay);
            _journalDirty = true;
        }

        /// <summary>
        /// Get the current gossip band (with decay) for NPC interactions.
        /// </summary>
        private MoralPathBand GetCurrentGossipBand()
        {
            SetupMoralChoice();
            return _moralGossipRuntime.GetEffectiveGossipBand(_moralChoice, _simDay);
        }

        private void SaveMoralChoice()
        {
            if (_moralChoice == null) return;
            if (CaptureSection("moral_choice", MoralChoiceSaveStore.TryCapturePersisted(_moralChoice.CaptureState())))
                _moralChoiceDirty = false;
        }

        private void FlushMoralChoiceIfDirty()
        {
            if (_moralChoiceDirty) SaveMoralChoice();
        }
    }
}
