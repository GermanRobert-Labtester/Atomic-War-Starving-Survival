using System;
using Godot;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        private bool _isComposing;
        private int _composeCampaignCallCount;

        public int ComposeCampaignCallCount => _composeCampaignCallCount;
        public void ResetComposeCampaignCallCount() => _composeCampaignCallCount = 0;

        private void RequireComposed(string caller)
        {
            if (!_isComposing)
                throw new InvalidOperationException($"{caller} called before ComposeCampaign()");
        }

        /// <summary>
        /// Single authoritative composition root. Called once before any gameplay panel opens.
        /// </summary>
        public void ComposeCampaign()
        {
            _composeCampaignCallCount++;
            _isComposing = true;
            try
            {
                SetupCampaignDay();
                SetupHoldfastRuntime();
                SetupStartingLevel();
                SetupEventsHost();
                SetupExpansionQuests();
                SetupThirdonary();
                SetupInventory();
                SetupSurvivors();
                SetupWorld();
                SetupMedical();
                SetupMedicalWard();
                SetupPhase0();
                SetupCrafting();
                SetupExpeditions();
                SetupEconomy();
                SetupJournal();
                SetupRadio();
                SetupPowerGrid();
                SetupGreenhouse();
                SetupMaritime();
                SetupYearOfAsh();
                SetupVerdict();
                SetupDutyRoster();
                SetupMuster();
                SetupFactionBranch();
                SetupMoralChoice();
                SetupDeepCoast();
                SetupSilentFoundry();
                SetupPhantom();
                SetupDoseLedger();
                SetupCombat();
                SetupNarrative();
                SetupUtilityAi();
                SetupCaravans();
                SetupExpansions();

                // Wiring that needs all services up
                SetupExpeditionCombatHandoff(_combat);

                // Expanded shelter systems (last — depends on World/PowerGrid/Inventory/Survivors/MedicalWard/Phase0/Crafting/Journal/Expeditions)
                SetupExpandedShelterSystems();

                // Domain-specific builders for optional/expansion-gated services
                ConstructSettlementBuilder();
                ConstructScavengerBuilder();
            }
            finally
            {
                _isComposing = false;
            }
        }

        // -----------------------------------------------------------------
        // Optional / expansion-gated domain builders
        // -----------------------------------------------------------------

        private void ConstructSettlementBuilder()
        {
            // Placeholder for optional settlement-building composition.
            // Wire here when the expansion catalog is present.
        }

        private void ConstructScavengerBuilder()
        {
            // Placeholder for optional scavenger-run composition.
            // Wire here when the expansion catalog is present.
        }
    }
}
