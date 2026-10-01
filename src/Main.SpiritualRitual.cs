// SPDX-License-Identifier: MIT
// ASHFALL Expansion 13 (The Faithful & The Fractured) — ritual calendar host wiring.
//
// Binds the sealed SpiritualRitualCalendarEngine to the authored ritual corpus
// the live SpiritualCatalog already loaded, tracks the bounded cooldown ledger,
// and routes the engine's morale verdict into the canonical morale owner.
// No piety meter, no second ritual registry.

using System;
using Ashfall.Core.Spiritual;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private SpiritualRitualHostSession? _spiritualRitual;
        private bool _spiritualRitualDirty;

        public SpiritualRitualHostSession? SpiritualRitual => _spiritualRitual;

        public void SetupSpiritualRitual()
        {
            if (_spiritualRitual == null)
            {
                _spiritualRitual = new SpiritualRitualHostSession();
                var saved = SpiritualRitualSaveStore.TryLoad();
                if (saved != null) _spiritualRitual.RestoreState(saved);
                _spiritualRitual.StateChanged += () => _spiritualRitualDirty = true;
            }

            // Bind the already-loaded authored catalog — never reloaded here.
            SetupSpiritual();
            _spiritualRitual.BindCatalog(_spiritualCatalog);
        }

        /// <summary>
        /// Performs one authored ritual through the engine and routes the verdict
        /// into the canonical morale owner. Refused rituals leave the ledger and
        /// morale untouched.
        /// </summary>
        public SpiritualRitualProjection? PerformSpiritualRitual(string ritualId, int day)
        {
            SetupSpiritualRitual();
            if (_spiritualRitual == null) return null;

            int shelterMorale = GetShelterAverageMoralePermille();
            var projection = _spiritualRitual.TryPerformRitual(ritualId, day, shelterMorale);
            if (projection == null) return null;

            ApplySpiritualRitualMorale(projection.MoraleDeltaPermille);
            return projection;
        }

        private void ApplySpiritualRitualMorale(int moraleDeltaPermille)
        {
            if (moraleDeltaPermille == 0 || _survivors == null) return;
            float delta = moraleDeltaPermille / 1000f;
            foreach (var survivor in _survivors.Needs.Registered)
            {
                if (survivor == null || string.IsNullOrEmpty(survivor.Id)) continue;
                _survivors.Needs.Modify(survivor, NeedKind.Morale, delta);
            }
        }

        private int GetShelterAverageMoralePermille()
        {
            if (_survivors == null) return 500;
            float total = 0f;
            int count = 0;
            foreach (var survivor in _survivors.Needs.Registered)
            {
                if (survivor == null) continue;
                total += survivor.Morale;
                count++;
            }
            if (count == 0) return 500;
            return (int)Math.Round((total / count) * 10f, MidpointRounding.AwayFromZero);
        }

        /// <summary>Holy-day observance scheduled for a belief movement on a campaign day.</summary>
        public HolyDayObservance? GetSpiritualHolyDay(string movementId, int campaignDay)
        {
            SetupSpiritualRitual();
            return _spiritualRitual?.GetScheduledObservance(movementId, campaignDay);
        }

        public void SaveSpiritualRitual()
        {
            if (_spiritualRitual == null) return;
            var state = _spiritualRitual.CaptureState();
            if (CaptureSection("spiritual_ritual", SpiritualRitualSaveStore.TryCapturePersisted(state)))
                _spiritualRitualDirty = false;
        }

        public void ResetSpiritualRitual()
        {
            _spiritualRitual = null;
            _spiritualRitualDirty = false;
        }
    }
}
