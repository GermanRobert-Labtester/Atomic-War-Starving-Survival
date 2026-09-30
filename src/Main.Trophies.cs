// SPDX-License-Identifier: MIT
// ============================================================================
// Trophy mount pipeline host composition. The Core TrophySystem remains the
// exactly-once award authority; this partial composes its authored trophies.json
// catalog, forwards quarry-preservation facts, and persists the award ledger.
// ============================================================================

using System;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private TrophyHostSession? _trophies;
        private bool _trophiesDirty;

        public TrophyHostSession? TrophiesSession => _trophies;

        public void SetupTrophies()
        {
            if (_trophies != null) return;
            var saved = TrophySaveStore.TryLoad();
            _trophies = TrophyHostSession.Create(saved);
            _trophies.StateChanged += () => _trophiesDirty = true;
            _trophies.LoadCatalog(CatalogPath.ResolveDataDir());
        }

        /// <summary>
        /// Forwards a quarry-preservation fact from a hunting/trapping owner.
        /// The trophy system awards the mount opportunity exactly once.
        /// </summary>
        public TrophyAwardRecord? PreserveQuarryTrophy(string speciesId)
        {
            SetupTrophies();
            int day = _campaignDay?.Calendar.CurrentDay ?? _simDay;
            var record = _trophies!.RecordQuarryPreserved(speciesId, day);
            if (record != null) _trophiesDirty = true;
            return record;
        }

        public (int Catalog, int Awarded, int UnlockedRecipes) GetTrophyReadout()
        {
            SetupTrophies();
            if (_trophies == null) return (0, 0, 0);
            return (_trophies.System.Catalog.Count, _trophies.System.AwardedTrophyIds.Count, _trophies.System.UnlockedRecipeIds.Count);
        }

        public void SaveTrophies()
        {
            if (_trophies == null) return;
            var state = _trophies.CaptureState();
            if (CaptureSection(TrophySaveStore.SectionName, TrophySaveStore.TryCapturePersisted(state)))
                _trophiesDirty = false;
        }

        public void FlushTrophiesIfDirty()
        {
            if (_trophiesDirty) SaveTrophies();
        }

        public void ResetTrophies()
        {
            _trophies = null;
            _trophiesDirty = false;
        }
    }
}
