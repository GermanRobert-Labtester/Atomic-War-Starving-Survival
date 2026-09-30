// SPDX-License-Identifier: MIT
// ============================================================================
// Faction diplomacy host composition. Core FactionDiplomacySystem remains the
// sole treaty/relation/mission authority; this partial composes its catalog,
// save section, and the canonical campaign day tick.
// ============================================================================

using System;
using Ashfall.Core.Diplomacy;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private DiplomacyHostSession? _diplomacy;
        private bool _diplomacyDirty;

        public DiplomacyHostSession? DiplomacySession => _diplomacy;

        public void SetupDiplomacy()
        {
            if (_diplomacy != null) return;
            var saved = DiplomacySaveStore.TryLoad();
            _diplomacy = DiplomacyHostSession.Create(saved);
            _diplomacy.StateChanged += () => _diplomacyDirty = true;
            _diplomacy.LoadCatalog(CatalogPath.ResolveDataDir());
        }

        public (bool Success, string Message, ActiveTreatyRecord? Treaty) ProposeDiplomaticTreaty(
            string factionId, string treatyTypeId, int envoySkill = 50)
        {
            SetupDiplomacy();
            int day = _campaignDay?.Calendar.CurrentDay ?? _simDay;
            var result = _diplomacy!.ProposeTreaty(factionId, treatyTypeId, day, envoySkill);
            if (result.Success) _diplomacyDirty = true;
            return result;
        }

        public DiplomaticMissionRecord DispatchDiplomaticMission(
            string missionType, string targetFactionId, string envoyId,
            int durationDays = 3, int envoySkill = 50)
        {
            SetupDiplomacy();
            int day = _campaignDay?.Calendar.CurrentDay ?? _simDay;
            var mission = _diplomacy!.DispatchMission(missionType, targetFactionId, envoyId, day, durationDays, envoySkill);
            _diplomacyDirty = true;
            return mission;
        }

        public void TickDiplomacy(int day)
        {
            SetupDiplomacy();
            if (_diplomacy == null) return;
            _diplomacy.TickDay(day);
            _diplomacyDirty = true;
        }

        public (int Reputation, int ActiveTreaties, int Violations) GetDiplomacyReadout()
        {
            SetupDiplomacy();
            if (_diplomacy == null) return (0, 0, 0);
            return (_diplomacy.GlobalReputation, _diplomacy.ActiveTreatyCount, _diplomacy.TotalViolationCount);
        }

        public void SaveDiplomacy()
        {
            if (_diplomacy == null) return;
            var state = _diplomacy.CaptureState();
            if (CaptureSection(DiplomacySaveStore.SectionName, DiplomacySaveStore.TryCapturePersisted(state)))
                _diplomacyDirty = false;
        }

        public void FlushDiplomacyIfDirty()
        {
            if (_diplomacyDirty) SaveDiplomacy();
        }

        public void ResetDiplomacy()
        {
            _diplomacy = null;
            _diplomacyDirty = false;
        }
    }
}
