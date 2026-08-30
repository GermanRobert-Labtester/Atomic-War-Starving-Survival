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

        /// <summary>
        /// Resolve a catalog quest by id. Returns false when the id is unknown
        /// or the quest is already resolved; the journal line is written by
        /// the event hook, and overnight settlement lands in TickSimDay.
        /// </summary>
        private bool TryResolveMoralChoice(string questId, int choiceIndex)
        {
            SetupMoralChoice();
            var def = _moralChoiceDefs.FirstOrDefault(
                d => string.Equals(d.Id, questId, StringComparison.Ordinal));
            if (def == null || _moralChoice.IsResolved(questId)) return false;
            _moralChoice.Resolve(def, choiceIndex, def.LocationId, _simDay);
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
