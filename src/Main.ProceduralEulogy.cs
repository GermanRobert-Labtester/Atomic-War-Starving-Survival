// SPDX-License-Identifier: MIT
// ============================================================================
// Procedural eulogy host composition. The Core ProceduralEulogyEngine remains
// the sole composition authority over a dweller's life summary; the host builds
// the life record from the authoritative survivor/memorial owners and archives
// the spoken text through the existing journal owner.
// ============================================================================

using System;
using Ashfall.Core.Journal;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private ProceduralEulogyHostSession? _eulogy;
        private bool _eulogyDirty;

        public ProceduralEulogyHostSession? ProceduralEulogySession => _eulogy;

        public void SetupProceduralEulogy()
        {
            if (_eulogy != null) return;
            var saved = ProceduralEulogySaveStore.TryLoad();
            _eulogy = ProceduralEulogyHostSession.Create(saved);
            _eulogy.StateChanged += () => _eulogyDirty = true;
        }

        /// <summary>
        /// Composes and archives an eulogy for a dweller whose life ended. The
        /// life record is built from the authoritative survivor/memorial state.
        /// </summary>
        public string SpeakEulogy(DwellerLifeRecord life)
        {
            SetupProceduralEulogy();
            string text = _eulogy!.ComposeEulogy(life);
            _eulogyDirty = true;
            return text;
        }

        public DwellerLifeRecord BuildDwellerLifeRecord(
            string dwellerId, string dwellerName, string preWarProfession,
            int daysSurvived, int mealsPrepared, int shiftsCompleted,
            int radDoseAbsorbedMsv, string causeOfDeath, string favoriteRelicName)
        {
            return new DwellerLifeRecord
            {
                dwellerId = dwellerId,
                dwellerName = dwellerName,
                preWarProfession = preWarProfession,
                daysSurvived = daysSurvived,
                mealsPrepared = mealsPrepared,
                shiftsCompleted = shiftsCompleted,
                radDoseAbsorbedMsv = radDoseAbsorbedMsv,
                causeOfDeath = causeOfDeath,
                favoriteRelicName = favoriteRelicName
            };
        }

        public string? GetMostRecentEulogy()
        {
            SetupProceduralEulogy();
            return _eulogy?.GetMostRecentEulogy();
        }

        public int GetEulogyCount()
        {
            SetupProceduralEulogy();
            return _eulogy?.ArchivedCount ?? 0;
        }

        public void SaveProceduralEulogy()
        {
            if (_eulogy == null) return;
            var state = _eulogy.CaptureState();
            ProceduralEulogySaveStore.TrySave(state);
            if (CaptureSection(ProceduralEulogySaveStore.SectionName, ProceduralEulogySaveStore.TryCapturePersisted(state)))
                _eulogyDirty = false;
        }

        public void FlushProceduralEulogyIfDirty()
        {
            if (_eulogyDirty) SaveProceduralEulogy();
        }

        public void ResetProceduralEulogy()
        {
            _eulogy = null;
            _eulogyDirty = false;
        }
    }
}
