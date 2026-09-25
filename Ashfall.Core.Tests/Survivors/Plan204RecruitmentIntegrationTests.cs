// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 204: Survivor Recruitment & Defection System — Integration Tests
// Verifies recruitment campaign & candidate templates catalog loading,
// active recruitment campaigns, defection offers & discovery risk,
// candidate discovery, and save/restore persistence.
// ============================================================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core.Survivors;

namespace Ashfall.Core.Tests.Survivors
{
    public sealed class Plan204RecruitmentIntegrationTests : CatalogTestBase
    {
        [Fact]
        public void LoadCatalog_LoadsCampaignAndCandidateTemplates()
        {
            var system = new RecruitmentSystem();
            string path = Path.Combine(DataDirectory, "recruitment_templates.json");
            Assert.True(File.Exists(path), $"recruitment_templates.json must exist at {path}");

            system.LoadCatalog(File.ReadAllText(path));

            var campaigns = system.GetAllCampaignDefs();
            Assert.Equal(5, campaigns.Count);

            var candidates = system.GetAllCandidateDefs();
            Assert.Equal(6, candidates.Count);

            var activeRec = system.GetCampaignDef("active_recruitment");
            Assert.NotNull(activeRec);
            Assert.Equal(5, activeRec.base_duration_days);
            Assert.Equal(15, activeRec.cost_food);

            var defector = system.GetCandidateDef("candidate_faction_deserter");
            Assert.NotNull(defector);
            Assert.Equal("defector", defector.candidate_type);
            Assert.Equal(75, defector.base_willingness);
        }

        [Fact]
        public void DiscoverCandidate_RegistersNewCandidateInState()
        {
            var system = new RecruitmentSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "recruitment_templates.json")));

            RecruitmentCandidateRecord? discovered = null;
            system.OnCandidateDiscovered += c => discovered = c;

            var cand = system.DiscoverCandidate(
                candidateTemplateId: "candidate_displaced_farmer",
                locationId: "farm_ruins_01",
                day: 3,
                currentFaction: "refugee_camp");

            Assert.NotNull(cand);
            Assert.NotNull(discovered);
            Assert.Equal(1, system.KnownCandidateCount);
            Assert.Equal("refugee", cand.CandidateType);
            Assert.Equal(85, cand.Willingness);
            Assert.False(cand.IsRecruited);
        }

        [Fact]
        public void StartCampaign_EnforcesMaxActiveCampaignsAndSkillBonuses()
        {
            var system = new RecruitmentSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "recruitment_templates.json")));

            var (success1, _, camp1) = system.StartCampaign("active_recruitment", "surv_scout1", "wasteland", day: 1, recruiterSkill: 80);
            var (success2, _, _) = system.StartCampaign("asylum_offer", "surv_scout2", "refugees", day: 1);
            var (success3, _, _) = system.StartCampaign("trade_for_survivor", "surv_scout3", "faction_iron", day: 1);

            Assert.True(success1);
            Assert.True(success2);
            Assert.True(success3);
            Assert.Equal(3, system.ActiveCampaignCount);

            // High recruiter skill boosts success chance
            Assert.NotNull(camp1);
            Assert.True(camp1.SuccessChance > 55);

            // 4th campaign rejected due to capacity
            var (success4, message4, _) = system.StartCampaign("active_recruitment", "surv_scout4", "wasteland", day: 1);
            Assert.False(success4);
            Assert.Contains("Maximum active campaigns", message4);
        }

        [Fact]
        public void MakeDefectionOffer_RegistersPendingOfferWithRiskCalculations()
        {
            var system = new RecruitmentSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "recruitment_templates.json")));

            var candidate = system.DiscoverCandidate("candidate_faction_deserter", "faction_base", day: 1, currentFaction: "faction_militia");

            var (success, _, offer) = system.MakeDefectionOffer(candidate.CandidateId, "asylum", offerValue: 60f, day: 1, diplomacySkill: 70);

            Assert.True(success);
            Assert.NotNull(offer);
            Assert.Equal("pending", offer.Status);
            Assert.True(offer.SuccessChance >= 50);
            Assert.True(offer.DiscoveredRisk <= 30);
        }

        [Fact]
        public void TickDay_CompletesCampaignsAndResolvesDefections()
        {
            var system = new RecruitmentSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "recruitment_templates.json")));

            RecruitmentCampaignRecord? completedCamp = null;
            system.OnCampaignCompleted += c => completedCamp = c;

            // Start campaign on day 1 (duration is 5 days -> finishes day 6)
            system.StartCampaign("active_recruitment", "recruiter_01", "wasteland", day: 1, recruiterSkill: 60);

            // Defection offer
            var candidate = system.DiscoverCandidate("candidate_scavenger", "plains", day: 1);
            system.MakeDefectionOffer(candidate.CandidateId, "asylum", offerValue: 80f, day: 1, diplomacySkill: 80);

            // Tick to day 6
            system.TickDay(6);

            Assert.NotNull(completedCamp);
            Assert.Equal("succeeded", completedCamp.Status);
            Assert.Equal(6, completedCamp.CompletedDay);
            Assert.True(system.TotalRecruitedCount >= 1);
        }

        [Fact]
        public void SaveRestoreState_PreservesCampaignsCandidatesAndOffers()
        {
            var system = new RecruitmentSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "recruitment_templates.json")));

            system.StartCampaign("asylum_offer", "recruiter_02", "refugees", day: 1);
            var cand = system.DiscoverCandidate("candidate_mercenary", "bar_zone", day: 1, "mercs");
            system.MakeDefectionOffer(cand.CandidateId, "money", 50f, day: 1);

            var captured = system.CaptureState();
            Assert.NotNull(captured);
            Assert.Single(captured.Campaigns);
            Assert.Single(captured.Candidates);
            Assert.Single(captured.Offers);

            var restored = new RecruitmentSystem();
            restored.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "recruitment_templates.json")));
            restored.RestoreState(captured);

            Assert.Equal(1, restored.ActiveCampaignCount);
            Assert.Equal(1, restored.KnownCandidateCount);
            Assert.Equal(1, restored.GetDefectionOffers().Count);
        }

        [Fact]
        public void GetCensusAndTryAdmitCandidate_UpdatesCensusAndRosterStatus()
        {
            var system = new RecruitmentSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "recruitment_templates.json")));

            var census0 = system.GetCensus();
            Assert.Equal(0, census0.ActiveCampaigns);
            Assert.Equal(0, census0.KnownCandidates);
            Assert.Equal(0, census0.TotalRecruited);
            Assert.Equal(0, census0.PendingOffers);

            var cand = system.DiscoverCandidate("candidate_scavenger", "outskirts", day: 1);
            Assert.Equal(1, system.GetCensus().KnownCandidates);

            bool admitted = system.TryAdmitCandidate(cand.CandidateId, out var admittedCand);
            Assert.True(admitted);
            Assert.NotNull(admittedCand);
            Assert.Equal("recruited", admittedCand.Status);
            Assert.Equal(1, system.GetCensus().TotalRecruited);
        }
    }
}
