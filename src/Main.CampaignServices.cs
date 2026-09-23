// SPDX-License-Identifier: MIT
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
                SetupDifficulty();
                SetupWorld();
                SetupMedical();
                SetupMedicalWard();
                SetupPhase0();
                SetupCrafting();
                SetupExpeditions();
                SetupReconTelemetry();
                SetupEconomy();
                SetupJournal();
                SetupRadio();
                SetupPowerGrid();
                SetupGreenhouse();
                ComposePlans74To77();
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
                SetupEchoes();
                SetupSpiritual();
                SetupUtilityAi();
                SetupCaravans();
                SetupExpansions();

                // CF-P28-ONE-BOOTSTRAP-PATH: the fresh lifecycle runs the same
                // declarative manifest bootstrap as RestoreAllSubsystemsFromDisk.
                // All 18 delegates are idempotent; this constructs any manifest
                // subsystem the direct calls above did not.
                ExecuteSubsystemManifestBootstrap();

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
                SetupAnomalyHazard();
                SetupCompanionAnimals();
                SetupBionics();
                SetupZealotry();
                SetupFallout();
                SetupDesperation();
                SetupMercenary();
                SetupArchaeology();
                SetupAmputation();
                SetupRailway();
                SetupFungi();
                SetupContrabandStash();
                SetupShelterBarter();
                SetupBlackProjectsArchive();
                SetupTechnicalMaterialArchive();
                SetupOralLore();
                SetupHydroGeologyDiscovery();
                SetupGrainMillingArchive();
                SetupLeatherworkArchive();
                SetupPlasticPyrolysis();
                SetupCargoAirdrop();
                SetupJustice();
                SetupRecreation();
                SetupChemWarfare();
                SetupCommsArray();
                SetupCeremony();
                SetupRobotics();
                SetupBioFermentation();

                // Expanded shelter systems (last — depends on World/PowerGrid/Inventory/Survivors/MedicalWard/Phase0/Crafting/Journal/Expeditions)
                SetupExpandedShelterSystems();
                // Plan 49: the excavation hazard authority must exist on the fresh
                // path too, so the subterranean flood bridge and day events are live
                // from day 1 (the restore path already constructs it).
                SetupExcavationHazards();
                SetupPlans166To169();
                BindDifficultyConsumers();
            }
            finally
            {
                _isComposing = false;
            }
        }

        private Ashfall.Core.Survivors.SkillProgressionSystem? _sharedSkillProgression;

        /// <summary>
        /// Plan 180/185/195 — daily dormancy tick for the shared progression
        /// authority (DEBT-185-SKILL-DORMANCY-TICK). Skills unused for the
        /// catalog's dormant window leave the active set; practice reactivates
        /// them. State already persists inside the `apprenticeship` section, so
        /// this adds no save section. Host-shaped actors: one per living
        /// survivor, matching what the practice systems record against.
        /// </summary>
        public void TickSharedSkillProgression(int day)
        {
            var skills = EnsureSharedSkillProgression();
            var roster = _survivors?.RosterState;
            if (roster == null || roster.Count == 0) return;

            var actors = new System.Collections.Generic.List<Ashfall.Core.Survivors.SimpleSkillActor>(roster.Count);
            for (int i = 0; i < roster.Count; i++)
            {
                var survivor = roster[i];
                if (survivor == null || !survivor.IsAliveState) continue;
                actors.Add(new Ashfall.Core.Survivors.SimpleSkillActor(survivor.Id));
            }

            skills.TickDaily(day, actors);
        }

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
                throw new InvalidOperationException(
                    "No campaign-owned faction stance authority is available; faction surfaces must not create a local default.");
            }
            return _sharedFactionStance;
        }
    }
}
