// SPDX-License-Identifier: MIT
// ============================================================================
// Ashfall Core : Expansion 37 — The Quickening
// Subsystem    : Antenatal Care, Maternal Support & Neonatal Health Ledger
// Authority    : docs/expansions/wave6/expansion_37_the_quickening_plan.md
//                WAVE6_INDEX.md, DEC-332
// ============================================================================
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Survivors
{
    /// <summary>
    /// Persisted record of a completed delivery.
    /// </summary>
    [Serializable]
    public sealed class DeliveryRecord
    {
        public string MotherSurvivorId { get; set; } = string.Empty;
        public string ChildId { get; set; } = string.Empty;
        public int DeliveryDay { get; set; } = 1;
        public BirthOutcomeClassification Outcome { get; set; } = BirthOutcomeClassification.Healthy;
        public int NeonatalVigorPermille { get; set; } = 800;
        public int MaternalExhaustionPermille { get; set; } = 500;
        public int PostpartumRecoveryDaysNeeded { get; set; } = 14;

        public DeliveryRecord Clone() => new DeliveryRecord
        {
            MotherSurvivorId = MotherSurvivorId,
            ChildId = ChildId,
            DeliveryDay = DeliveryDay,
            Outcome = Outcome,
            NeonatalVigorPermille = NeonatalVigorPermille,
            MaternalExhaustionPermille = MaternalExhaustionPermille,
            PostpartumRecoveryDaysNeeded = PostpartumRecoveryDaysNeeded
        };
    }

    /// <summary>
    /// Persisted state for antenatal care, active pregnancies, clinical readiness, and delivery history.
    /// </summary>
    [Serializable]
    public sealed class AntenatalMaternalCareState
    {
        public int SchemaVersion { get; set; } = 1;
        public int ShelterClinicQualityPermille { get; set; } = 600;
        public int ShelterSanitationQualityPermille { get; set; } = 700;
        public Dictionary<string, MaternalPregnancyState> ActivePregnancies { get; set; } =
            new Dictionary<string, MaternalPregnancyState>(StringComparer.Ordinal);
        public List<DeliveryRecord> DeliveryHistory { get; set; } = new List<DeliveryRecord>();

        public AntenatalMaternalCareState Clone()
        {
            var clone = new AntenatalMaternalCareState
            {
                SchemaVersion = SchemaVersion,
                ShelterClinicQualityPermille = ShelterClinicQualityPermille,
                ShelterSanitationQualityPermille = ShelterSanitationQualityPermille,
                ActivePregnancies = new Dictionary<string, MaternalPregnancyState>(ActivePregnancies.Count, StringComparer.Ordinal),
                DeliveryHistory = new List<DeliveryRecord>(DeliveryHistory.Count)
            };

            foreach (var kvp in ActivePregnancies)
            {
                clone.ActivePregnancies[kvp.Key] = kvp.Value.Clone();
            }

            foreach (var record in DeliveryHistory)
            {
                clone.DeliveryHistory.Add(record.Clone());
            }

            return clone;
        }
    }

    /// <summary>
    /// Bounded read model for the antenatal and maternal health subsystem.
    /// </summary>
    public struct AntenatalMaternalCensus
    {
        public int ActivePregnanciesCount { get; }
        public int StableCount { get; }
        public int StrainedCount { get; }
        public int AtRiskCount { get; }
        public int CriticalCount { get; }
        public int LaborReadyCount { get; }
        public int TotalDeliveriesCount { get; }
        public int ShelterClinicQualityPermille { get; }
        public int ShelterSanitationQualityPermille { get; }

        public AntenatalMaternalCensus(
            int activePregnanciesCount,
            int stableCount,
            int strainedCount,
            int atRiskCount,
            int criticalCount,
            int laborReadyCount,
            int totalDeliveriesCount,
            int shelterClinicQualityPermille,
            int shelterSanitationQualityPermille)
        {
            ActivePregnanciesCount = activePregnanciesCount;
            StableCount = stableCount;
            StrainedCount = strainedCount;
            AtRiskCount = atRiskCount;
            CriticalCount = criticalCount;
            LaborReadyCount = laborReadyCount;
            TotalDeliveriesCount = totalDeliveriesCount;
            ShelterClinicQualityPermille = shelterClinicQualityPermille;
            ShelterSanitationQualityPermille = shelterSanitationQualityPermille;
        }
    }

    /// <summary>
    /// Owns mutable state for antenatal maternal health, trimester progressions, and deliveries,
    /// delegating calculations to <see cref="AntenatalMaternalHealthEngine"/>.
    /// ChildDevelopmentSystem and GenerationalSystem remain the owners of registered children and lineage.
    /// </summary>
    public sealed class AntenatalMaternalCareLedger
    {
        private readonly AntenatalMaternalCareState _state;

        public AntenatalMaternalCareLedger(AntenatalMaternalCareState? state = null)
        {
            _state = state ?? new AntenatalMaternalCareState();
        }

        public IReadOnlyDictionary<string, MaternalPregnancyState> ActivePregnancies => _state.ActivePregnancies;
        public IReadOnlyList<DeliveryRecord> DeliveryHistory => _state.DeliveryHistory;
        public int ShelterClinicQualityPermille => _state.ShelterClinicQualityPermille;
        public int ShelterSanitationQualityPermille => _state.ShelterSanitationQualityPermille;

        public AntenatalMaternalCareState CaptureState() => _state.Clone();

        public void RestoreState(AntenatalMaternalCareState? saved)
        {
            if (saved == null) return;
            if (saved.SchemaVersion > _state.SchemaVersion)
            {
                throw new InvalidOperationException(
                    $"Antenatal maternal care schema {saved.SchemaVersion} is newer than supported {_state.SchemaVersion}.");
            }

            _state.SchemaVersion = saved.SchemaVersion <= 0 ? _state.SchemaVersion : saved.SchemaVersion;
            _state.ShelterClinicQualityPermille = Math.Clamp(saved.ShelterClinicQualityPermille, 0, 1000);
            _state.ShelterSanitationQualityPermille = Math.Clamp(saved.ShelterSanitationQualityPermille, 0, 1000);

            _state.ActivePregnancies.Clear();
            if (saved.ActivePregnancies != null)
            {
                foreach (var kvp in saved.ActivePregnancies)
                {
                    if (kvp.Value != null)
                    {
                        _state.ActivePregnancies[kvp.Key] = kvp.Value.Clone();
                    }
                }
            }

            _state.DeliveryHistory.Clear();
            if (saved.DeliveryHistory != null)
            {
                foreach (var item in saved.DeliveryHistory)
                {
                    if (item != null)
                    {
                        _state.DeliveryHistory.Add(item.Clone());
                    }
                }
            }
        }

        public void Clear()
        {
            _state.ActivePregnancies.Clear();
            _state.DeliveryHistory.Clear();
            _state.ShelterClinicQualityPermille = 600;
            _state.ShelterSanitationQualityPermille = 700;
        }

        public void SetClinicQuality(int permille)
        {
            _state.ShelterClinicQualityPermille = Math.Clamp(permille, 0, 1000);
            foreach (var preg in _state.ActivePregnancies.Values)
            {
                preg.MedicalSupervisionQualityPermille = _state.ShelterClinicQualityPermille;
            }
        }

        public void SetSanitationQuality(int permille)
        {
            _state.ShelterSanitationQualityPermille = Math.Clamp(permille, 0, 1000);
            foreach (var preg in _state.ActivePregnancies.Values)
            {
                preg.ShelterSanitationQualityPermille = _state.ShelterSanitationQualityPermille;
            }
        }

        public MaternalPregnancyState RegisterPregnancy(
            string motherSurvivorId,
            int gestationDays = 1,
            int nutritionReservePermille = 800,
            int fatiguePermille = 200,
            int stressPermille = 100)
        {
            if (string.IsNullOrWhiteSpace(motherSurvivorId))
            {
                throw new ArgumentException("Mother survivor ID cannot be empty.", nameof(motherSurvivorId));
            }

            var pregnancy = new MaternalPregnancyState
            {
                MotherSurvivorId = motherSurvivorId,
                GestationDays = Math.Max(1, gestationDays),
                MaternalNutritionReservePermille = Math.Clamp(nutritionReservePermille, 0, 1000),
                MaternalFatiguePermille = Math.Clamp(fatiguePermille, 0, 1000),
                AccumulatedStressPermille = Math.Clamp(stressPermille, 0, 1000),
                MedicalSupervisionQualityPermille = _state.ShelterClinicQualityPermille,
                ShelterSanitationQualityPermille = _state.ShelterSanitationQualityPermille,
                IsPostpartum = false
            };

            _state.ActivePregnancies[motherSurvivorId] = pregnancy;
            return pregnancy;
        }

        public bool IsPregnant(string motherSurvivorId)
        {
            if (string.IsNullOrWhiteSpace(motherSurvivorId)) return false;
            return _state.ActivePregnancies.TryGetValue(motherSurvivorId, out var state) && !state.IsPostpartum;
        }

        public bool IsLaborReady(string motherSurvivorId)
        {
            if (string.IsNullOrWhiteSpace(motherSurvivorId)) return false;
            return _state.ActivePregnancies.TryGetValue(motherSurvivorId, out var state) &&
                   state.GestationDays >= AntenatalMaternalHealthEngine.FullTermGestationDaysThreshold &&
                   !state.IsPostpartum;
        }

        public MaternalPregnancyState? GetPregnancy(string motherSurvivorId)
        {
            if (string.IsNullOrWhiteSpace(motherSurvivorId)) return null;
            return _state.ActivePregnancies.TryGetValue(motherSurvivorId, out var state) ? state : null;
        }

        public Dictionary<string, TrimesterProgressionResult> AdvanceDay(
            int currentDay,
            Func<string, int>? getNutritionIntakePermille = null,
            Func<string, int>? getRestHoursProvided = null)
        {
            var results = new Dictionary<string, TrimesterProgressionResult>(StringComparer.Ordinal);
            var completedPostpartum = new List<string>();

            foreach (var kvp in _state.ActivePregnancies)
            {
                string motherId = kvp.Key;
                var state = kvp.Value;

                state.MedicalSupervisionQualityPermille = _state.ShelterClinicQualityPermille;
                state.ShelterSanitationQualityPermille = _state.ShelterSanitationQualityPermille;

                int nutrition = getNutritionIntakePermille?.Invoke(motherId) ?? 1000;
                int restHours = getRestHoursProvided?.Invoke(motherId) ?? 8;

                if (state.IsPostpartum)
                {
                    // Advance postpartum recovery
                    int recoveryRate = AntenatalMaternalHealthEngine.ComputePostpartumRecoveryRate(
                        Math.Max(1, currentDay),
                        state.MedicalSupervisionQualityPermille,
                        nutrition);

                    state.MaternalFatiguePermille = Math.Max(0, state.MaternalFatiguePermille - recoveryRate * 5);
                    state.MaternalNutritionReservePermille = Math.Min(1000, state.MaternalNutritionReservePermille + recoveryRate * 3);

                    if (state.MaternalFatiguePermille <= 100 && state.MaternalNutritionReservePermille >= 800)
                    {
                        completedPostpartum.Add(motherId);
                    }
                }
                else
                {
                    var prog = AntenatalMaternalHealthEngine.AdvancePregnancyDay(state, nutrition, restHours);
                    results[motherId] = prog;
                }
            }

            foreach (var finishedMotherId in completedPostpartum)
            {
                _state.ActivePregnancies.Remove(finishedMotherId);
            }

            return results;
        }

        public BirthResolutionResult ResolveDelivery(
            string motherSurvivorId,
            int birthSeed,
            string? childId = null)
        {
            if (string.IsNullOrWhiteSpace(motherSurvivorId))
            {
                throw new ArgumentException("Mother survivor ID cannot be empty.", nameof(motherSurvivorId));
            }

            if (!_state.ActivePregnancies.TryGetValue(motherSurvivorId, out var state))
            {
                throw new InvalidOperationException($"No active pregnancy found for survivor '{motherSurvivorId}'.");
            }

            state.MedicalSupervisionQualityPermille = _state.ShelterClinicQualityPermille;
            state.ShelterSanitationQualityPermille = _state.ShelterSanitationQualityPermille;

            var resolution = AntenatalMaternalHealthEngine.ResolveBirthDelivery(state, birthSeed);

            var record = new DeliveryRecord
            {
                MotherSurvivorId = motherSurvivorId,
                ChildId = childId ?? $"child_{motherSurvivorId}_{state.GestationDays}",
                DeliveryDay = state.GestationDays,
                Outcome = resolution.Outcome,
                NeonatalVigorPermille = resolution.NeonatalVigorPermille,
                MaternalExhaustionPermille = resolution.MaternalExhaustionPermille,
                PostpartumRecoveryDaysNeeded = resolution.PostpartumRecoveryDaysNeeded
            };

            _state.DeliveryHistory.Add(record);
            return resolution;
        }

        public AntenatalMaternalCensus GetCensus()
        {
            int active = 0;
            int stable = 0;
            int strained = 0;
            int atRisk = 0;
            int critical = 0;
            int laborReady = 0;

            foreach (var preg in _state.ActivePregnancies.Values)
            {
                if (preg.IsPostpartum) continue;

                active++;
                if (preg.GestationDays >= AntenatalMaternalHealthEngine.FullTermGestationDaysThreshold)
                {
                    laborReady++;
                }

                // Check status using current snapshot
                int baseRisk = AntenatalMaternalHealthEngine.ResolveTrimester(preg.GestationDays) switch
                {
                    GestationTrimester.FirstTrimester => 120,
                    GestationTrimester.SecondTrimester => 80,
                    GestationTrimester.ThirdTrimester => 160,
                    _ => 220
                };
                int netRisk = Math.Clamp(baseRisk +
                    (1000 - preg.MaternalNutritionReservePermille) * 200 / 1000 +
                    preg.MaternalFatiguePermille * 150 / 1000 -
                    preg.MedicalSupervisionQualityPermille * 180 / 1000 -
                    preg.ShelterSanitationQualityPermille * 120 / 1000, 10, 950);

                if (preg.MaternalNutritionReservePermille < 250 || preg.MaternalFatiguePermille > 800 || netRisk > 600)
                    critical++;
                else if (preg.MaternalNutritionReservePermille < 500 || preg.MaternalFatiguePermille > 600 || netRisk > 350)
                    atRisk++;
                else if (preg.MaternalNutritionReservePermille < 750 || preg.MaternalFatiguePermille > 400 || netRisk > 200)
                    strained++;
                else
                    stable++;
            }

            return new AntenatalMaternalCensus(
                activePregnanciesCount: active,
                stableCount: stable,
                strainedCount: strained,
                atRiskCount: atRisk,
                criticalCount: critical,
                laborReadyCount: laborReady,
                totalDeliveriesCount: _state.DeliveryHistory.Count,
                shelterClinicQualityPermille: _state.ShelterClinicQualityPermille,
                shelterSanitationQualityPermille: _state.ShelterSanitationQualityPermille);
        }
    }
}
