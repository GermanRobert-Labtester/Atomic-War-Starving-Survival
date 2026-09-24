// SPDX-License-Identifier: MIT
// ASHFALL Plan 150 — Romance & Family Dynamics Host Wiring.

using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core.Random;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private RomanceFamilyHostSession? _romanceFamily;
        private bool _romanceFamilyDirty;

        public RomanceFamilyHostSession? RomanceFamily => _romanceFamily;

        public void SetupRomanceFamily()
        {
            if (_romanceFamily != null) return;

            _romanceFamily = RomanceFamilyHostSession.Create(_dataDir);

            var saved = RomanceFamilySaveStore.TryLoad();
            if (saved != null && !string.IsNullOrEmpty(saved.core_state))
            {
                _romanceFamily.RestoreCoreState(saved.core_state);
            }

            _romanceFamily.StateChanged += () => _romanceFamilyDirty = true;

            if (_survivorDetailPanel != null)
            {
                _survivorDetailPanel.RomanceProvider = id =>
                {
                    if (_romanceFamily == null) return null;
                    var rel = _romanceFamily.System.GetRomanticPartner(id);
                    if (rel == null) return null;
                    string other = rel.GetOther(id);
                    return (other, rel.Stage.ToString(), rel.IsSoulmate);
                };
                _survivorDetailPanel.FamilyProvider = id =>
                {
                    if (_romanceFamily == null) return null;
                    var fam = _romanceFamily.System.GetFamilyForSurvivor(id);
                    return fam != null ? $"{fam.FamilyName} ({fam.ParentIds.Count + fam.ChildIds.Count} members)" : null;
                };
            }
        }

        public void SaveRomanceFamily()
        {
            if (_romanceFamily == null) return;
            var state = _romanceFamily.CapturePersistedState();
            RomanceFamilySaveStore.TrySave(state);
            if (CaptureSection("romance_family", RomanceFamilySaveStore.TryCapturePersisted(state)))
            {
                _romanceFamilyDirty = false;
            }
        }

        public void TickRomanceFamily(int day)
        {
            if (_romanceFamily == null) SetupRomanceFamily();
            if (_romanceFamily == null) return;

            // Bonded tenure advances once per day on the Core authority.
            _romanceFamily.AdvanceDay(day);

            TryFormAttractions(day);
        }

        /// <summary>
        /// Drives new attraction from the canonical affinity the relationship
        /// owner already tracks. No parallel affinity is computed here: a pair
        /// only becomes eligible because <see cref="SurvivorRelationsSystem"/>
        /// says their affinity is high, and the roll is taken from the
        /// campaign's own <c>social</c> stream so a replay is identical.
        /// </summary>
        private void TryFormAttractions(int day)
        {
            if (_romanceFamily == null) return;
            if (_survivors?.RosterState == null || _survivorRelationsCore == null) return;

            var living = new List<string>();
            for (int i = 0; i < _survivors.RosterState.Count; i++)
            {
                var entry = _survivors.RosterState[i];
                if (entry != null && entry.IsAlive) living.Add(entry.Id);
            }

            if (living.Count < 2) return;

            int ordinal = 0;
            for (int i = 0; i < living.Count; i++)
            {
                for (int j = i + 1; j < living.Count; j++)
                {
                    string a = living[i];
                    string b = living[j];

                    if (_romanceFamily.GetRelationship(a, b) != null) continue;
                    // A survivor already in a committed partnership is not eligible.
                    if (_romanceFamily.GetRomanticPartner(a) != null) continue;
                    if (_romanceFamily.GetRomanticPartner(b) != null) continue;

                    if (!_survivorRelationsCore.TryGetRelationship(a, b, out var entry) || entry == null) continue;
                    if (entry.affinity < 50f) continue;

                    var rng = _campaignDay.Rng.Fork(CampaignStreamIds.Social, day, ordinal++);
                    _romanceFamily.TryInitiateAttraction(
                        a,
                        b,
                        entry.affinity,
                        ageA: 0,
                        ageB: 0,
                        beliefA: BeliefOf(a),
                        beliefB: BeliefOf(b),
                        sharedTrauma: false,
                        rng: rng,
                        currentDay: day);
                }
            }
        }

        /// <summary>Belief profile from the canonical enrichment owner, or empty when unbound.</summary>
        private string BeliefOf(string survivorId)
        {
            if (_enrichmentService == null) return string.Empty;
            try
            {
                return _enrichmentService.GetView(survivorId)?.BeliefProfileId ?? string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public void FlushRomanceFamilyIfDirty()
        {
            if (_romanceFamilyDirty)
            {
                SaveRomanceFamily();
            }
        }

        public void ResetRomanceFamily()
        {
            _romanceFamily = null;
            _romanceFamilyDirty = false;
        }
    }
}
