// SPDX-License-Identifier: MIT
// ============================================================================
// ASHFALL Expansion 37 — The Quickening: Antenatal Care, Maternal Support
// & Neonatal Health host wiring.
// The signed pure AntenatalMaternalHealthEngine is the trimester progression,
// caloric demand, and delivery resolution calculation authority.
// ChildDevelopmentSystem and GenerationalSystem remain the underlying child and lineage authorities;
// this host owns the active maternal care, trimester tracking, and delivery records.
// ============================================================================

using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private AntenatalMaternalHealthHostSession? _antenatalMaternalHealth;
        private bool _antenatalMaternalHealthDirty;

        public AntenatalMaternalHealthHostSession? AntenatalMaternalHealth => _antenatalMaternalHealth;

        public void SetupAntenatalMaternalHealth()
        {
            if (_antenatalMaternalHealth != null) return;

            var saved = AntenatalMaternalHealthSaveStore.TryLoad();
            _antenatalMaternalHealth = AntenatalMaternalHealthHostSession.Create(saved);
            _antenatalMaternalHealth.StateChanged += () => _antenatalMaternalHealthDirty = true;
        }

        public MaternalPregnancyState RegisterPregnancy(
            string motherSurvivorId,
            int gestationDays = 1,
            int nutritionReservePermille = 800,
            int fatiguePermille = 200,
            int stressPermille = 100)
        {
            SetupAntenatalMaternalHealth();
            return _antenatalMaternalHealth!.RegisterPregnancy(
                motherSurvivorId, gestationDays, nutritionReservePermille, fatiguePermille, stressPermille);
        }

        public Dictionary<string, TrimesterProgressionResult> AdvanceAntenatalDay(
            int day,
            Func<string, int>? getNutritionIntake = null,
            Func<string, int>? getRestHours = null)
        {
            SetupAntenatalMaternalHealth();
            return _antenatalMaternalHealth!.AdvanceDay(day, getNutritionIntake, getRestHours);
        }

        public BirthResolutionResult ResolveMaternalDelivery(
            string motherSurvivorId,
            int birthSeed,
            string? childId = null)
        {
            SetupAntenatalMaternalHealth();
            return _antenatalMaternalHealth!.ResolveDelivery(motherSurvivorId, birthSeed, childId);
        }

        public void SetMaternalClinicQuality(int permille)
        {
            SetupAntenatalMaternalHealth();
            _antenatalMaternalHealth!.SetClinicQuality(permille);
        }

        public void SetMaternalSanitationQuality(int permille)
        {
            SetupAntenatalMaternalHealth();
            _antenatalMaternalHealth!.SetSanitationQuality(permille);
        }

        public AntenatalMaternalCensus GetAntenatalMaternalCensus() =>
            _antenatalMaternalHealth?.Census ?? default;

        public void SaveAntenatalMaternalHealth()
        {
            if (_antenatalMaternalHealth == null) return;
            var state = _antenatalMaternalHealth.Ledger.CaptureState();
            AntenatalMaternalHealthSaveStore.TrySave(state);
            if (CaptureSection(AntenatalMaternalHealthSaveStore.SectionName, AntenatalMaternalHealthSaveStore.TryCapturePersisted(state)))
                _antenatalMaternalHealthDirty = false;
        }

        public void FlushAntenatalMaternalHealthIfDirty()
        {
            if (_antenatalMaternalHealthDirty)
                SaveAntenatalMaternalHealth();
        }

        public void ResetAntenatalMaternalHealth()
        {
            _antenatalMaternalHealth = null;
            _antenatalMaternalHealthDirty = false;
        }
    }
}
