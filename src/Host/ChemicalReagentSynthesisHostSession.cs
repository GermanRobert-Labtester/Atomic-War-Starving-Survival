// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : ChemicalReagentSynthesisSaveStore
// Core State : Ashfall.Core.Shelter.ChemicalReagentSynthesisState
// Host Caller: Main.ChemicalReagentSynthesis
// Purpose    : Expansion 39 — Chemical Synthesis Safety, Catalyst Purity & Reagent Grade
//              host session and persistence.
// ============================================================================

using System;
using Ashfall.Core.Save;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public static class ChemicalReagentSynthesisSaveStore
    {
        public const string FileName = "chemical_reagent_synthesis_save.json";
        public const string SectionName = "chemical_reagent_synthesis";

        private static readonly SaveStore<ChemicalReagentSynthesisState> s_store =
            SaveStoreHub.Checksummed<ChemicalReagentSynthesisState>(FileName, nameof(ChemicalReagentSynthesisSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(ChemicalReagentSynthesisState state) => s_store.CaptureBare(state);
        public static ChemicalReagentSynthesisState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(ChemicalReagentSynthesisState state) => s_store.TrySave(state);
        public static ChemicalReagentSynthesisState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>
    /// Expansion 39 host session. Wraps the stateful <see cref="ChemicalReagentLedger"/>
    /// over the signed pure calculation engine <see cref="ChemicalReagentSynthesisEngine"/>.
    /// Specialised synthesis engines (ChlorAlkali, FischerTropsch, BioFermentation, PharmaLab)
    /// remain process-specific authorities; this host session governs reactor vessel thermal/pressure
    /// envelopes, catalyst wear, stoichiometric mass balances, reagent grading, and hazardous waste neutralization.
    /// </summary>
    public sealed class ChemicalReagentSynthesisHostSession : HostSessionBase
    {
        private readonly ChemicalReagentLedger _ledger;

        public ChemicalReagentLedger Ledger => _ledger;
        public ChemicalReagentCensus Census => _ledger.GetCensus();
        public string LastEvent { get; private set; } = string.Empty;

        public ChemicalReagentSynthesisHostSession(ChemicalReagentSynthesisState? state = null)
        {
            _ledger = new ChemicalReagentLedger(state);
        }

        public static ChemicalReagentSynthesisHostSession Create(ChemicalReagentSynthesisState? state = null) =>
            new ChemicalReagentSynthesisHostSession(state);

        public void RegisterOrUpdateReactor(SynthesisReactorState reactor)
        {
            _ledger.RegisterOrUpdateReactor(reactor);
            LastEvent = $"Registered/updated reactor vessel '{reactor.ReactorId}'.";
            RaiseStateChanged();
        }

        public SynthesisReactionStepResult ExecuteSynthesisCycle(
            string reactorId,
            int targetBatchKg,
            int reactantRatioPermille,
            int operatorSkillPermille,
            long timestampTicks = 0)
        {
            var result = _ledger.ExecuteSynthesisCycle(
                reactorId, targetBatchKg, reactantRatioPermille, operatorSkillPermille, timestampTicks);

            LastEvent = result.IsReactionAborted
                ? $"Synthesis cycle aborted on '{reactorId}': Hazard state {result.HazardState}!"
                : $"Synthesis on '{reactorId}' produced {result.OutputBatchKg}kg (Grade: {result.PurityGrade}, Waste: {result.NeutralizingWasteVolumeLitres}L, Cat Wear: {result.CatalystDegradationPermille}\u2030).";

            RaiseStateChanged();
            return result;
        }

        public MassBalanceResult EvaluateMassBalance(
            int precursorInputKg,
            int stoichiometricRatioPermille,
            int conversionRatePermille)
        {
            return _ledger.EvaluateMassBalance(precursorInputKg, stoichiometricRatioPermille, conversionRatePermille);
        }

        public bool NeutralizeAcidicWaste(int litresToNeutralize, int acidConcentrationPermille)
        {
            bool success = _ledger.NeutralizeAcidicWaste(litresToNeutralize, acidConcentrationPermille);
            if (success)
            {
                LastEvent = $"Neutralized {litresToNeutralize}L of acidic effluent waste.";
                RaiseStateChanged();
            }
            else
            {
                LastEvent = $"Failed to neutralize {litresToNeutralize}L waste: insufficient neutralizing alkali stock.";
            }
            return success;
        }

        public void RestockNeutralizingAlkali(int alkaliKg)
        {
            _ledger.RestockNeutralizingAlkali(alkaliKg);
            LastEvent = $"Restocked neutralizing alkali stock by {alkaliKg}kg.";
            RaiseStateChanged();
        }

        public bool RegenerateCatalystBed(string reactorId, int catalystRestorationPermille)
        {
            bool success = _ledger.RegenerateCatalystBed(reactorId, catalystRestorationPermille);
            if (success)
            {
                LastEvent = $"Regenerated catalyst bed for '{reactorId}' (+{catalystRestorationPermille}\u2030).";
                RaiseStateChanged();
            }
            return success;
        }

        public bool AdjustCoolingCapacity(string reactorId, int coolingCapacityPermille)
        {
            bool success = _ledger.AdjustCoolingCapacity(reactorId, coolingCapacityPermille);
            if (success)
            {
                LastEvent = $"Adjusted cooling capacity for '{reactorId}' to {coolingCapacityPermille}\u2030.";
                RaiseStateChanged();
            }
            return success;
        }

        public void AdvanceDay(int dayNumber)
        {
            _ledger.AdvanceDay(dayNumber);
            LastEvent = $"Advanced chemical synthesis operations for day {dayNumber}.";
            RaiseStateChanged();
        }

        public ChemicalReagentSynthesisState CaptureState() => _ledger.CaptureState();

        public void RestoreState(ChemicalReagentSynthesisState? state)
        {
            _ledger.RestoreState(state);
            LastEvent = "Restored chemical reagent synthesis state.";
            RaiseStateChanged();
        }
    }
}
