// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Archaeology;
using Ashfall.Core.Disease;
using Ashfall.Core.Economy;
using Ashfall.Core.Inventory;
using Ashfall.Core.Survivors;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.Integration
{
    public class Plans186_189_CampaignContinuityTests
    {
        [Fact]
        public void Plans186_189_FullContinuityPipeline_IntegratesAcrossAllFourSystems()
        {
            var rng = new SeededRng(186189);
            var inv = new Inventory.Inventory();
            var needs = new NeedsSystem();
            var diseaseState = new DiseaseSystemState();
            var disease = new DiseaseSystem(diseaseState);
            var researchState = new ResearchState();
            var research = new ResearchSystem(null, researchState);

            // 1. Plan 186: Radioactive fallout plume approaches
            var fallout = new FalloutSystem();
            var cloud = fallout.SpawnCloud("fallout_pattern_strontium_plume", -10f, 0f, "zone_epicenter");
            cloud.radius = 25f;
            cloud.toxicity = 200f;

            // Fallout creates high hazard over shelter zone
            float radExposure = fallout.GetZoneRadiationRate("loc_holdfast", 0f, 0f);
            Assert.True(radExposure > 50f, $"Expected high radiation, got {radExposure}");

            // Commander seals the shelter against fallout
            fallout.SealShelter(48f, 0.90f);
            float attenuatedExposure = fallout.GetZoneRadiationRate("loc_holdfast", 0f, 0f);
            Assert.True(attenuatedExposure < radExposure * 0.2f);

            // 2. Plan 187: Trapped in shelter, starvation escalates to crisis
            var dweller1 = new SurvivorNeedsState { Id = "dweller_alec", Hunger = 95f, Morale = 70f, Health = 100f };
            var dweller2 = new SurvivorNeedsState { Id = "dweller_bren", Hunger = 92f, Morale = 65f, Health = 100f };
            needs.Register(dweller1);
            needs.Register(dweller2);

            var desperation = new DesperationSystem(rng, inv, needs, disease);
            desperation.RegisterCorpse("corpse_dweller_claire");

            // Desperation act: dweller_alec harvests deceased dweller_claire
            var desperationResult = desperation.HarvestCorpse("dweller_alec", "corpse_dweller_claire", "desperation_consume_corpse", 12);
            Assert.True(desperationResult.IsSuccess);

            // Meat gained, taboo broken, morale plummeted
            Assert.True(inv.CountById("raw_meat") > 0);
            Assert.True(dweller2.Morale < 65f);
            Assert.Contains("dweller_alec", desperation.State.cannibalSurvivorIds);

            // 3. Plan 189: Expedition surveys distant pre-war ruins
            var archaeology = new ArchaeologySystem(rng, inv, research);
            var site = archaeology.SurveyRuins("loc_deep_silo", 4.0f);
            Assert.NotNull(site);

            var archive = archaeology.ProgressExcavation(site.siteId, 10f);
            Assert.NotNull(archive);

            // Shelter restores power and decrypts archive
            inv.AddById("item_decryption_keycard_prewar", 1);
            var decryptResult = archaeology.ProgressDecryption(archive.archiveId, 10f, 4.0f, hasPower: true, hasKeycard: true);
            Assert.True(decryptResult.IsSuccess);
            Assert.True(archive.unlocked);
            Assert.Contains(archive.archiveId, researchState.unlockedIds);

            // 4. Plan 188: Decrypted intelligence unlocks high-value mercenary bounty
            var mercenary = new MercenarySystem(rng, inv);
            mercenary.GenerateBoard(12, new List<string> { "target_warlord_grendel" });
            var contract = mercenary.ActiveContracts[0];

            var acceptResult = mercenary.AcceptContract(contract.contractId, 12);
            Assert.True(acceptResult.IsSuccess);

            // Slay target, gather proof item, claim bounty reward
            inv.AddById(contract.requiredProofItemId, 1);
            var claimResult = mercenary.ClaimReward(contract.contractId);
            Assert.True(claimResult.IsSuccess);
            Assert.True(inv.CountById("scrap_metal") >= (int)contract.rewardAmount);
            Assert.Equal(BountyContractStatus.Claimed, contract.status);

            // 5. Multi-System Persistence Validation
            var fState = fallout.State;
            var dState = desperation.State;
            var aState = archaeology.State;
            var mState = mercenary.State;

            var falloutRestored = new FalloutSystem();
            falloutRestored.RestoreState(fState);
            Assert.Equal(fallout.IsShelterSealed, falloutRestored.IsShelterSealed);

            var desperationRestored = new DesperationSystem(rng, inv, needs, disease);
            desperationRestored.RestoreState(dState);
            Assert.Equal(desperation.MutinyPressure, desperationRestored.MutinyPressure);
            Assert.Equal(dState.actsHistory.Count, desperationRestored.State.actsHistory.Count);

            var archaeologyRestored = new ArchaeologySystem(rng, inv, research);
            archaeologyRestored.RestoreState(aState);
            Assert.Equal(aState.unlockedLoreIds.Count, archaeologyRestored.State.unlockedLoreIds.Count);

            var mercenaryRestored = new MercenarySystem(rng, inv);
            mercenaryRestored.RestoreState(mState);
            Assert.Equal(mState.contracts.Count, mercenaryRestored.State.contracts.Count);
        }
    }
}
