// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Difficulty;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private DifficultyPresetCatalog? _difficultyCatalog;
        private DifficultyDirector? _difficultyDirector;
        private DifficultyScalarsProvider _difficultyScalars = DifficultyScalarsProvider.Legacy;
        private string _difficultyPresetId = string.Empty;

        /// <summary>
        /// The only live difficulty scalar source. Consumers are added only
        /// after their individual calculation sites have been premise-checked.
        /// </summary>
        internal DifficultyScalarsProvider DifficultyScalars => _difficultyScalars;

        internal string DifficultyPresetId => _difficultyPresetId;

        private DifficultyPresetCatalog EnsureDifficultyCatalog()
        {
            if (_difficultyCatalog != null) return _difficultyCatalog;

            _difficultyCatalog = DifficultyPresetCatalogLoader.Load(
                _dataDir,
                new FileSystemIO());
            _difficultyDirector = new DifficultyDirector(_difficultyCatalog);
            return _difficultyCatalog;
        }

        private DifficultyDirector EnsureDifficultyDirector()
        {
            EnsureDifficultyCatalog();
            return _difficultyDirector
                ?? throw new InvalidOperationException("Difficulty director did not initialize.");
        }

        private string DefaultDifficultyPresetId() =>
            EnsureDifficultyDirector().ResolvePreset(null).id;

        private string ResolveDifficultyPresetId(string? presetId) =>
            EnsureDifficultyDirector().ResolvePreset(presetId).id;

        /// <summary>
        /// Called exclusively by the fresh-campaign transaction after the
        /// player commits the menu selection. The resolved ID is later stored
        /// in the checksummed campaign header; there is no in-run edit path.
        /// </summary>
        private void SelectDifficultyForNewCampaign(string? presetId)
        {
            var director = EnsureDifficultyDirector();
            var preset = director.ResolvePreset(presetId);
            _difficultyPresetId = preset.id;
            _difficultyScalars = DifficultyScalarsProvider.FromPreset(preset);
        }

        /// <summary>
        /// Restores a header-bound selection. An absent v1 field uses the
        /// catalog default; an explicit unknown ID fails closed via the
        /// director rather than silently changing a campaign's difficulty.
        /// </summary>
        private void RestoreDifficultyFromCampaignHeader(CampaignDaySave? save)
        {
            SelectDifficultyForNewCampaign(save?.difficulty_preset_id);
        }

        /// <summary>
        /// Returns the selected preset's authored starter-item IDs. The
        /// inventory owner performs the actual grants during fresh setup.
        /// </summary>
        private IReadOnlyList<string> DifficultyStartingBonusItemIds()
        {
            var preset = EnsureDifficultyDirector().ResolvePreset(_difficultyPresetId);
            return preset.starting_bonus_item_ids.ToArray();
        }

        private void ResetDifficultyForCampaign()
        {
            _difficultyPresetId = string.Empty;
            _difficultyScalars = DifficultyScalarsProvider.Legacy;
        }
    }
}
