// SPDX-License-Identifier: MIT
// ============================================================================
// ASHFALL Expansion 39 — The Reagent: Chemical Synthesis Safety, Catalyst Purity
// & Reagent Grade host wiring.
// The signed pure ChemicalReagentSynthesisEngine is the calculation authority.
// Existing specialized synthesis and lab systems remain their own authorities;
// this host owns active reactor vessel thermal/pressure envelopes, catalyst health,
// stoichiometric mass balances, and hazardous waste neutralization.
// ============================================================================

using System;
using Godot;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private ChemicalReagentSynthesisHostSession? _chemicalReagentSynthesis;
        private bool _chemicalReagentSynthesisDirty;

        public ChemicalReagentSynthesisHostSession? ChemicalReagentSynthesis => _chemicalReagentSynthesis;

        public void SetupChemicalReagentSynthesis()
        {
            if (_chemicalReagentSynthesis != null) return;

            var saved = ChemicalReagentSynthesisSaveStore.TryLoad();
            _chemicalReagentSynthesis = ChemicalReagentSynthesisHostSession.Create(saved);
            _chemicalReagentSynthesis.StateChanged += () => _chemicalReagentSynthesisDirty = true;
        }

        public SynthesisReactionStepResult ExecuteChemicalSynthesisCycle(
            string reactorId,
            int targetBatchKg,
            int reactantRatioPermille,
            int operatorSkillPermille,
            long timestampTicks = 0)
        {
            SetupChemicalReagentSynthesis();
            return _chemicalReagentSynthesis!.ExecuteSynthesisCycle(
                reactorId, targetBatchKg, reactantRatioPermille, operatorSkillPermille, timestampTicks);
        }

        public MassBalanceResult EvaluateChemicalMassBalance(
            int precursorInputKg,
            int stoichiometricRatioPermille,
            int conversionRatePermille)
        {
            SetupChemicalReagentSynthesis();
            return _chemicalReagentSynthesis!.EvaluateMassBalance(
                precursorInputKg, stoichiometricRatioPermille, conversionRatePermille);
        }

        public bool NeutralizeChemicalAcidicWaste(int litresToNeutralize, int acidConcentrationPermille)
        {
            SetupChemicalReagentSynthesis();
            return _chemicalReagentSynthesis!.NeutralizeAcidicWaste(litresToNeutralize, acidConcentrationPermille);
        }

        public void RestockChemicalNeutralizingAlkali(int alkaliKg)
        {
            SetupChemicalReagentSynthesis();
            _chemicalReagentSynthesis!.RestockNeutralizingAlkali(alkaliKg);
        }

        public bool RegenerateChemicalCatalystBed(string reactorId, int catalystRestorationPermille)
        {
            SetupChemicalReagentSynthesis();
            return _chemicalReagentSynthesis!.RegenerateCatalystBed(reactorId, catalystRestorationPermille);
        }

        public bool AdjustChemicalCoolingCapacity(string reactorId, int coolingCapacityPermille)
        {
            SetupChemicalReagentSynthesis();
            return _chemicalReagentSynthesis!.AdjustCoolingCapacity(reactorId, coolingCapacityPermille);
        }

        public void AdvanceChemicalReagentSynthesisDay(int dayNumber)
        {
            SetupChemicalReagentSynthesis();
            _chemicalReagentSynthesis!.AdvanceDay(dayNumber);
        }

        public ChemicalReagentCensus GetChemicalReagentCensus() =>
            _chemicalReagentSynthesis?.Census ?? default;

        public void SaveChemicalReagentSynthesis()
        {
            if (_chemicalReagentSynthesis == null) return;
            var state = _chemicalReagentSynthesis.Ledger.CaptureState();
            ChemicalReagentSynthesisSaveStore.TrySave(state);
            if (CaptureSection(
                    ChemicalReagentSynthesisSaveStore.SectionName,
                    ChemicalReagentSynthesisSaveStore.TryCapturePersisted(state)))
            {
                _chemicalReagentSynthesisDirty = false;
            }
        }

        public void FlushChemicalReagentSynthesisIfDirty()
        {
            if (_chemicalReagentSynthesisDirty)
            {
                SaveChemicalReagentSynthesis();
            }
        }

        public void ResetChemicalReagentSynthesis()
        {
            _chemicalReagentSynthesis = null;
            _chemicalReagentSynthesisDirty = false;
        }
    }
}
