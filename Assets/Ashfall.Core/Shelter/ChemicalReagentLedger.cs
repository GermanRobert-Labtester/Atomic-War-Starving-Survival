// SPDX-License-Identifier: MIT
// ============================================================================
// Ashfall Core : Expansion 39 — The Reagent
// Subsystem    : Chemical Synthesis Safety, Catalyst Purity & Reagent Grade Ledger
// Authority    : docs/expansions/wave6/expansion_39_the_reagent_plan.md
// ============================================================================
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Shelter
{
    /// <summary>
    /// Historic record of a completed or aborted chemical synthesis reaction batch.
    /// </summary>
    public sealed class SynthesisReactionBatchRecord
    {
        public string BatchId { get; set; } = string.Empty;
        public string ReactorId { get; set; } = string.Empty;
        public int TargetBatchKg { get; set; }
        public int ProducedBatchKg { get; set; }
        public ReagentPurityGrade PurityGrade { get; set; } = ReagentPurityGrade.Crude;
        public SynthesisReactorHazardState HazardState { get; set; } = SynthesisReactorHazardState.Stable;
        public int CatalystDegradedPermille { get; set; }
        public int NeutralizingWasteLitres { get; set; }
        public bool IsAborted { get; set; }
        public long TimestampTicks { get; set; }
    }

    /// <summary>
    /// Persistent state payload for Expansion 39 Chemical Reagent Synthesis.
    /// </summary>
    public sealed class ChemicalReagentSynthesisState
    {
        public int SchemaVersion { get; set; } = 1;
        public List<SynthesisReactorState> Reactors { get; set; } = new List<SynthesisReactorState>();
        public List<SynthesisReactionBatchRecord> CompletedBatches { get; set; } = new List<SynthesisReactionBatchRecord>();
        public int NeutralizingAlkaliStockKg { get; set; } = 250;
        public int AccumulatedAcidicWasteLitres { get; set; } = 0;
        public int TotalNeutralizedWasteLitres { get; set; } = 0;
        public Dictionary<string, int> ReagentPurityInventoryKg { get; set; } = new Dictionary<string, int>(StringComparer.Ordinal);
        public int SafetyAlertCount { get; set; } = 0;
        public int ThermalRunawayCount { get; set; } = 0;
        public int CumulativeProducedKg { get; set; } = 0;

        public ChemicalReagentSynthesisState Clone()
        {
            var clone = new ChemicalReagentSynthesisState
            {
                SchemaVersion = SchemaVersion,
                NeutralizingAlkaliStockKg = NeutralizingAlkaliStockKg,
                AccumulatedAcidicWasteLitres = AccumulatedAcidicWasteLitres,
                TotalNeutralizedWasteLitres = TotalNeutralizedWasteLitres,
                SafetyAlertCount = SafetyAlertCount,
                ThermalRunawayCount = ThermalRunawayCount,
                CumulativeProducedKg = CumulativeProducedKg,
                Reactors = new List<SynthesisReactorState>(Reactors.Count),
                CompletedBatches = new List<SynthesisReactionBatchRecord>(CompletedBatches.Count),
                ReagentPurityInventoryKg = new Dictionary<string, int>(ReagentPurityInventoryKg, StringComparer.Ordinal)
            };

            foreach (var r in Reactors)
            {
                clone.Reactors.Add(r.Clone());
            }

            foreach (var b in CompletedBatches)
            {
                clone.CompletedBatches.Add(new SynthesisReactionBatchRecord
                {
                    BatchId = b.BatchId,
                    ReactorId = b.ReactorId,
                    TargetBatchKg = b.TargetBatchKg,
                    ProducedBatchKg = b.ProducedBatchKg,
                    PurityGrade = b.PurityGrade,
                    HazardState = b.HazardState,
                    CatalystDegradedPermille = b.CatalystDegradedPermille,
                    NeutralizingWasteLitres = b.NeutralizingWasteLitres,
                    IsAborted = b.IsAborted,
                    TimestampTicks = b.TimestampTicks
                });
            }

            return clone;
        }
    }

    /// <summary>
    /// Read-only snapshot census of chemical synthesis operations.
    /// </summary>
    public struct ChemicalReagentCensus
    {
        public int ActiveReactorsCount { get; }
        public int HazardousReactorsCount { get; }
        public int AccumulatedAcidicWasteLitres { get; }
        public int NeutralizingAlkaliStockKg { get; }
        public int TotalReagentStockKg { get; }
        public int AnalyticalPharmaStockKg { get; }
        public int SafetyAlertCount { get; }
        public int ThermalRunawayCount { get; }

        public ChemicalReagentCensus(
            int activeReactorsCount,
            int hazardousReactorsCount,
            int accumulatedAcidicWasteLitres,
            int neutralizingAlkaliStockKg,
            int totalReagentStockKg,
            int analyticalPharmaStockKg,
            int safetyAlertCount,
            int thermalRunawayCount)
        {
            ActiveReactorsCount = activeReactorsCount;
            HazardousReactorsCount = hazardousReactorsCount;
            AccumulatedAcidicWasteLitres = accumulatedAcidicWasteLitres;
            NeutralizingAlkaliStockKg = neutralizingAlkaliStockKg;
            TotalReagentStockKg = totalReagentStockKg;
            AnalyticalPharmaStockKg = analyticalPharmaStockKg;
            SafetyAlertCount = safetyAlertCount;
            ThermalRunawayCount = thermalRunawayCount;
        }
    }

    /// <summary>
    /// Stateful Core domain ledger for Expansion 39 Chemical Reagent Synthesis.
    /// Manages reactor vessels, synthesis batches, catalyst health, reagent grades,
    /// and acidic effluent neutralization.
    /// </summary>
    public sealed class ChemicalReagentLedger
    {
        public const int MaxCompletedBatchesRetained = 200;
        public const int AcidicWasteAlertThresholdLitres = 500;

        private ChemicalReagentSynthesisState _state = new ChemicalReagentSynthesisState();

        public ChemicalReagentLedger(ChemicalReagentSynthesisState? state = null)
        {
            RestoreState(state);
        }

        private void EnsureDefaultReactor()
        {
            if (_state.Reactors.Count == 0)
            {
                _state.Reactors.Add(new SynthesisReactorState
                {
                    ReactorId = "primary_synthesis_vessel",
                    OperatingTemperaturePermille = 350,
                    OperatingPressurePermille = 300,
                    CatalystActivityPermille = 950,
                    FeedstockPurityPermille = 900,
                    CoolingCapacityPermille = 850,
                    HazardState = SynthesisReactorHazardState.Stable
                });
            }
        }

        public void RegisterOrUpdateReactor(SynthesisReactorState reactor)
        {
            if (reactor == null || string.IsNullOrWhiteSpace(reactor.ReactorId)) return;

            int idx = _state.Reactors.FindIndex(r => string.Equals(r.ReactorId, reactor.ReactorId, StringComparison.OrdinalIgnoreCase));
            if (idx >= 0)
            {
                _state.Reactors[idx] = reactor.Clone();
            }
            else
            {
                _state.Reactors.Add(reactor.Clone());
            }
        }

        public SynthesisReactorState? GetReactor(string reactorId)
        {
            if (string.IsNullOrWhiteSpace(reactorId)) return null;
            return _state.Reactors.Find(r => string.Equals(r.ReactorId, reactorId, StringComparison.OrdinalIgnoreCase));
        }

        public SynthesisReactionStepResult ExecuteSynthesisCycle(
            string reactorId,
            int targetBatchKg,
            int reactantRatioPermille,
            int operatorSkillPermille,
            long timestampTicks = 0)
        {
            var reactor = GetReactor(reactorId);
            if (reactor == null)
            {
                return new SynthesisReactionStepResult(
                    0, ReagentPurityGrade.Crude, SynthesisReactorHazardState.Stable, 0, 0, true);
            }

            var result = ChemicalReagentSynthesisEngine.EvaluateReactionStep(
                reactor, targetBatchKg, reactantRatioPermille, operatorSkillPermille);

            if (result.IsReactionAborted || result.HazardState == SynthesisReactorHazardState.ThermalExcursionRunaway)
            {
                _state.ThermalRunawayCount++;
                _state.SafetyAlertCount++;
            }
            else
            {
                string gradeKey = result.PurityGrade.ToString();
                if (!_state.ReagentPurityInventoryKg.TryGetValue(gradeKey, out int currentKg))
                {
                    currentKg = 0;
                }
                _state.ReagentPurityInventoryKg[gradeKey] = currentKg + result.OutputBatchKg;
                _state.CumulativeProducedKg += result.OutputBatchKg;

                if (result.HazardState != SynthesisReactorHazardState.Stable)
                {
                    _state.SafetyAlertCount++;
                }
            }

            _state.AccumulatedAcidicWasteLitres += result.NeutralizingWasteVolumeLitres;

            string batchId = $"batch_syn_{timestampTicks}_{_state.CompletedBatches.Count + 1}";
            _state.CompletedBatches.Add(new SynthesisReactionBatchRecord
            {
                BatchId = batchId,
                ReactorId = reactorId,
                TargetBatchKg = targetBatchKg,
                ProducedBatchKg = result.OutputBatchKg,
                PurityGrade = result.PurityGrade,
                HazardState = result.HazardState,
                CatalystDegradedPermille = result.CatalystDegradationPermille,
                NeutralizingWasteLitres = result.NeutralizingWasteVolumeLitres,
                IsAborted = result.IsReactionAborted,
                TimestampTicks = timestampTicks
            });

            if (_state.CompletedBatches.Count > MaxCompletedBatchesRetained)
            {
                _state.CompletedBatches.RemoveAt(0);
            }

            return result;
        }

        public MassBalanceResult EvaluateMassBalance(
            int precursorInputKg,
            int stoichiometricRatioPermille,
            int conversionRatePermille)
        {
            return ChemicalReagentSynthesisEngine.CalculateMassBalance(
                precursorInputKg, stoichiometricRatioPermille, conversionRatePermille);
        }

        public bool NeutralizeAcidicWaste(int litresToNeutralize, int acidConcentrationPermille)
        {
            if (litresToNeutralize <= 0 || _state.AccumulatedAcidicWasteLitres <= 0) return false;

            int effectiveLitres = Math.Min(litresToNeutralize, _state.AccumulatedAcidicWasteLitres);
            int alkaliRequiredKg = ChemicalReagentSynthesisEngine.CalculateWasteNeutralizationDemand(
                effectiveLitres, acidConcentrationPermille);

            if (_state.NeutralizingAlkaliStockKg < alkaliRequiredKg)
            {
                return false;
            }

            _state.NeutralizingAlkaliStockKg -= alkaliRequiredKg;
            _state.AccumulatedAcidicWasteLitres -= effectiveLitres;
            _state.TotalNeutralizedWasteLitres += effectiveLitres;
            return true;
        }

        public void RestockNeutralizingAlkali(int alkaliKg)
        {
            if (alkaliKg <= 0) return;
            _state.NeutralizingAlkaliStockKg += alkaliKg;
        }

        public bool RegenerateCatalystBed(string reactorId, int catalystRestorationPermille)
        {
            var reactor = GetReactor(reactorId);
            if (reactor == null || catalystRestorationPermille <= 0) return false;

            reactor.CatalystActivityPermille = Math.Clamp(
                reactor.CatalystActivityPermille + catalystRestorationPermille, 0, 1000);
            return true;
        }

        public bool AdjustCoolingCapacity(string reactorId, int coolingCapacityPermille)
        {
            var reactor = GetReactor(reactorId);
            if (reactor == null) return false;

            reactor.CoolingCapacityPermille = Math.Clamp(coolingCapacityPermille, 0, 1000);
            return true;
        }

        public void AdvanceDay(int dayNumber)
        {
            // Passive ambient cooling and pressure venting towards equilibrium
            foreach (var reactor in _state.Reactors)
            {
                if (reactor.OperatingTemperaturePermille > 350)
                {
                    reactor.OperatingTemperaturePermille = Math.Max(350, reactor.OperatingTemperaturePermille - 80);
                }
                if (reactor.OperatingPressurePermille > 300)
                {
                    reactor.OperatingPressurePermille = Math.Max(300, reactor.OperatingPressurePermille - 60);
                }

                if (reactor.OperatingTemperaturePermille < 750 &&
                    reactor.OperatingPressurePermille < 700 &&
                    reactor.HazardState != SynthesisReactorHazardState.Stable)
                {
                    reactor.HazardState = SynthesisReactorHazardState.Stable;
                }
            }

            // Hazard evaluation for excessive unneutralized acidic waste
            if (_state.AccumulatedAcidicWasteLitres >= AcidicWasteAlertThresholdLitres)
            {
                _state.SafetyAlertCount++;
            }
        }

        public ChemicalReagentCensus GetCensus()
        {
            int activeReactors = _state.Reactors.Count;
            int hazardousReactors = 0;
            foreach (var r in _state.Reactors)
            {
                if (r.HazardState != SynthesisReactorHazardState.Stable ||
                    r.OperatingTemperaturePermille >= 750 ||
                    r.OperatingPressurePermille >= 700)
                {
                    hazardousReactors++;
                }
            }

            int totalStockKg = 0;
            foreach (var kvp in _state.ReagentPurityInventoryKg)
            {
                totalStockKg += kvp.Value;
            }

            _state.ReagentPurityInventoryKg.TryGetValue(ReagentPurityGrade.AnalyticalPharma.ToString(), out int pharmaKg);

            return new ChemicalReagentCensus(
                activeReactors,
                hazardousReactors,
                _state.AccumulatedAcidicWasteLitres,
                _state.NeutralizingAlkaliStockKg,
                totalStockKg,
                pharmaKg,
                _state.SafetyAlertCount,
                _state.ThermalRunawayCount);
        }

        public ChemicalReagentSynthesisState CaptureState() => _state.Clone();

        public void RestoreState(ChemicalReagentSynthesisState? state)
        {
            if (state == null)
            {
                _state = new ChemicalReagentSynthesisState();
                EnsureDefaultReactor();
                return;
            }

            _state = state.Clone();
            EnsureDefaultReactor();
        }
    }
}
