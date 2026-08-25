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

        /// <summary>
        /// Fixed world seed so every host agrees on unseeded rolls; per-save
        /// outcome rolls and propagation days are stored in the ledger DTO.
        /// </summary>
        private const int MoralChoiceSeed = 20260825;

        private void SetupMoralChoice()
        {
            if (_moralChoice != null) return;
            SetupJournal();
            _moralChoice = new MoralChoiceSystem(new SeededRng(MoralChoiceSeed));
            _moralChoiceDefs = MoralChoiceCatalogLoader.Load(
                _dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            _moralChoice.OnQuestResolved += WriteMoralChoiceJournalEntry;
            _moralChoice.OnQuestResolved += _ => _moralChoiceDirty = true;
            _moralChoice.OnThresholdEventFired += _ => _moralChoiceDirty = true;

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
            GD.Print($"[Ashfall Godot] Moral choice ready. {_moralChoiceDefs.Count} quests in the catalog.");
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

        private void SaveMoralChoice()
        {
            if (_moralChoice == null) return;
            MoralChoiceSaveStore.Save(_moralChoice.CaptureState());
            _moralChoiceDirty = false;
        }

        private void FlushMoralChoiceIfDirty()
        {
            if (_moralChoiceDirty) SaveMoralChoice();
        }
    }
}
