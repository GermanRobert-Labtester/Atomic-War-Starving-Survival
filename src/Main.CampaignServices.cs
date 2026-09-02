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
            }
            finally
            {
                _isComposing = false;
            }
        }

        private Ashfall.Core.Survivors.SkillProgressionSystem? _sharedSkillProgression;

        public Ashfall.Core.Survivors.SkillProgressionSystem EnsureSharedSkillProgression()
        {
            if (_sharedSkillProgression != null) return _sharedSkillProgression;

            var fileIO = new Ashfall.Core.FileSystemIO();
            var serializer = new Ashfall.Core.SystemTextJsonSerializer();
            var catalog = Ashfall.Core.Survivors.SkillCatalogLoader.Load(_dataDir, fileIO, serializer);

            _sharedSkillProgression = new Ashfall.Core.Survivors.SkillProgressionSystem();
            if (catalog != null)
            {
                for (int i = 0; i < catalog.Count; i++)
                {
                    _sharedSkillProgression.RegisterSkill(catalog[i]);
                }
            }
            return _sharedSkillProgression;
        }
    }
}
