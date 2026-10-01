// SPDX-License-Identifier: MIT
// ============================================================================
// Radiation economy bridge host composition. The Core bridge owns contaminated
// trade multipliers, block rules, and the evaluation ledger; this partial
// composes its catalog and save section. Base item pricing stays with the
// economy owner.
// ============================================================================

using System;
using Ashfall.Core.Radiation;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private RadiationEconomyHostSession? _radiationEconomy;
        private bool _radiationEconomyDirty;

        public RadiationEconomyHostSession? RadiationEconomySession => _radiationEconomy;

        public void SetupRadiationEconomy()
        {
            if (_radiationEconomy != null) return;
            var saved = RadiationEconomySaveStore.TryLoad();
            _radiationEconomy = RadiationEconomyHostSession.Create(saved);
            _radiationEconomy.StateChanged += () => _radiationEconomyDirty = true;
            _radiationEconomy.LoadCatalog(CatalogPath.ResolveDataDir());
        }

        public TradeEvaluation EvaluateContaminatedTrade(
            string itemId, string category, int contaminationLevel, float basePrice, string? targetFactionId = null)
        {
            SetupRadiationEconomy();
            var evaluation = _radiationEconomy!.EvaluateTrade(itemId, category, contaminationLevel, basePrice, targetFactionId);
            _radiationEconomyDirty = true;
            return evaluation;
        }

        public (int Evaluations, int Blocked) GetRadiationEconomyReadout()
        {
            SetupRadiationEconomy();
            if (_radiationEconomy == null) return (0, 0);
            return (_radiationEconomy.Bridge.TotalEvaluations, _radiationEconomy.Bridge.TotalBlockedTrades);
        }

        public void SaveRadiationEconomy()
        {
            if (_radiationEconomy == null) return;
            var state = _radiationEconomy.CaptureState();
            if (CaptureSection(RadiationEconomySaveStore.SectionName, RadiationEconomySaveStore.TryCapturePersisted(state)))
                _radiationEconomyDirty = false;
        }

        public void ResetRadiationEconomy()
        {
            _radiationEconomy = null;
            _radiationEconomyDirty = false;
        }
    }
}
