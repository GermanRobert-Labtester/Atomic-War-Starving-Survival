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
                if (_inventory != null && _survivors != null)
                {
                    _inventory.Survivors = _survivors;
                    _survivors.Inventory = _inventory;
                }
                if (_holdfastRuntime != null)
                {
                    if (_inventory != null)
                    {
                        _holdfastRuntime.InventorySession = _inventory;
                        _holdfastRuntime.Inventory = _inventory.Inventory;
                    }
                    if (_survivors != null)
                    {
                        _holdfastRuntime.Survivors = _survivors;
                    }
                }

                // Plans 178-201: expansion systems must also exist in a NEW game
                // (RestoreAllSubsystemsFromDisk covers only the load/continue
                // path). Without this, the null-guarded tick blocks never come
                // alive and the sections never persist for fresh campaigns.
                SetupGenerational();
                SetupPrisoners();
                SetupMutations();
                SetupStealth();
                SetupAviation();
                SetupForcedLabor();
                SetupNarcotics();
                SetupPolitics();
                SetupFallout();
                SetupDesperation();
                SetupMercenary();
                SetupArchaeology();
                SetupAmputation();
                SetupRailway();
                SetupFungi();
                SetupJustice();
                SetupRecreation();
                SetupChemWarfare();
                SetupCommsArray();
                SetupCeremony();
                SetupRobotics();

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

        private Ashfall.Core.Economy.FactionStanceEngine? _sharedFactionStance;

        public Ashfall.Core.Economy.FactionStanceEngine EnsureSharedFactionStance()
        {
            if (_sharedFactionStance != null) return _sharedFactionStance;
            SetupSilentFoundry();
            if (_silentFoundry != null)
            {
                _sharedFactionStance = _silentFoundry.GuildStanceEngine;
            }
            else
            {
                _sharedFactionStance = new Ashfall.Core.Economy.FactionStanceEngine();
            }
            return _sharedFactionStance;
        }
    }
}
