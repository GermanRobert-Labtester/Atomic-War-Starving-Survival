// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : AntenatalMaternalHealthSaveStore
// Core State : Ashfall.Core.Survivors.AntenatalMaternalCareState
// Host Caller: Main.AntenatalMaternalHealth
// Purpose    : Expansion 37 — Antenatal Care, Maternal Support & Neonatal Health
//              host session and persistence.
// ============================================================================

using System;
using System.Collections.Generic;
using Ashfall.Core.Survivors;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class AntenatalMaternalHealthSaveStore
    {
        public const string FileName = "antenatal_maternal_health_save.json";
        public const string SectionName = "antenatal_maternal_health";

        private static readonly SaveStore<AntenatalMaternalCareState> s_store =
            SaveStoreHub.Checksummed<AntenatalMaternalCareState>(FileName, nameof(AntenatalMaternalHealthSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(AntenatalMaternalCareState state) => s_store.CaptureBare(state);
        public static AntenatalMaternalCareState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(AntenatalMaternalCareState state) => s_store.TrySave(state);
        public static AntenatalMaternalCareState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>
    /// Expansion 37 host session. Wraps the stateful
    /// <see cref="AntenatalMaternalCareLedger"/> over the signed pure
    /// <see cref="AntenatalMaternalHealthEngine"/>.
    /// ChildDevelopmentSystem and GenerationalSystem remain the underlying child and lineage authorities;
    /// this host session manages active maternal care and neonatal delivery outcomes.
    /// </summary>
    public sealed class AntenatalMaternalHealthHostSession : HostSessionBase
    {
        private readonly AntenatalMaternalCareLedger _ledger;

        public AntenatalMaternalCareLedger Ledger => _ledger;
        public AntenatalMaternalCensus Census => _ledger.GetCensus();
        public int ShelterClinicQualityPermille => _ledger.ShelterClinicQualityPermille;
        public int ShelterSanitationQualityPermille => _ledger.ShelterSanitationQualityPermille;
        public string LastEvent { get; private set; } = string.Empty;

        public AntenatalMaternalHealthHostSession(AntenatalMaternalCareState? state = null)
        {
            _ledger = new AntenatalMaternalCareLedger(state);
        }

        public static AntenatalMaternalHealthHostSession Create(AntenatalMaternalCareState? state = null) =>
            new AntenatalMaternalHealthHostSession(state);

        public MaternalPregnancyState RegisterPregnancy(
            string motherSurvivorId,
            int gestationDays = 1,
            int nutritionReservePermille = 800,
            int fatiguePermille = 200,
            int stressPermille = 100)
        {
            var preg = _ledger.RegisterPregnancy(
                motherSurvivorId, gestationDays, nutritionReservePermille, fatiguePermille, stressPermille);
            LastEvent = $"Registered pregnancy for '{motherSurvivorId}' at day {gestationDays} ({ResolveTrimesterName(gestationDays)}).";
            RaiseStateChanged();
            return preg;
        }

        public Dictionary<string, TrimesterProgressionResult> AdvanceDay(
            int currentDay,
            Func<string, int>? getNutritionIntakePermille = null,
            Func<string, int>? getRestHoursProvided = null)
        {
            var results = _ledger.AdvanceDay(currentDay, getNutritionIntakePermille, getRestHoursProvided);
            LastEvent = $"Advanced antenatal health for {results.Count} active pregnancies.";
            RaiseStateChanged();
            return results;
        }

        public BirthResolutionResult ResolveDelivery(
            string motherSurvivorId,
            int birthSeed,
            string? childId = null)
        {
            var birth = _ledger.ResolveDelivery(motherSurvivorId, birthSeed, childId);
            LastEvent = $"Delivery resolved for '{motherSurvivorId}': Outcome {birth.Outcome}, Neonatal Vigor {birth.NeonatalVigorPermille}\u2030.";
            RaiseStateChanged();
            return birth;
        }

        public void SetClinicQuality(int permille)
        {
            _ledger.SetClinicQuality(permille);
            LastEvent = $"Shelter clinic quality updated to {permille}\u2030.";
            RaiseStateChanged();
        }

        public void SetSanitationQuality(int permille)
        {
            _ledger.SetSanitationQuality(permille);
            LastEvent = $"Shelter sanitation quality updated to {permille}\u2030.";
            RaiseStateChanged();
        }

        private static string ResolveTrimesterName(int gestationDays)
        {
            return AntenatalMaternalHealthEngine.ResolveTrimester(gestationDays) switch
            {
                GestationTrimester.FirstTrimester => "1st Trimester",
                GestationTrimester.SecondTrimester => "2nd Trimester",
                GestationTrimester.ThirdTrimester => "3rd Trimester",
                GestationTrimester.FullTerm => "Full Term",
                _ => "Postpartum"
            };
        }
    }
}
