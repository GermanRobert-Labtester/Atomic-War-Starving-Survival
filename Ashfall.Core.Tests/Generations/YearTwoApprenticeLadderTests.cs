// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Generations
{
    public sealed class YearTwoApprenticeLadderTests
    {
        private const string Mentorship = "mentorship_rough_repairs";
        private static ChildProfile Child() => new ChildProfile
        {
            ChildId = "child", BirthDay = 1, Stage = DevelopmentStage.Infant,
            Milestones = new List<string> { "vocational_apprenticeship" }
        };
        private static ApprenticeshipSystem Create(out SkillProgressionSystem skills)
        {
            skills = new SkillProgressionSystem();
            skills.RecordAction(new SimpleSkillActor("mentor"), "skill_rough_repairs", 60f, 1);
            var system = new ApprenticeshipSystem(new SeededRng(42), skills,
                new DutyRosterSystem(), new SurvivorRelationsSystem(new SeededRng(42)));
            system.RegisterMentorship(new MentorshipDef
            {
                mentorship_id = Mentorship, target_skill_id = "skill_rough_repairs",
                legacy_trait_id = "trait_stoic_craftsman", legacy_xp_grant = 140f
            });
            return system;
        }

        [Fact]
        public void OfferUsesCanonicalAgeAndMilestoneRatherThanCachedStage()
        {
            var system = Create(out _);
            var child = Child();
            child.Stage = DevelopmentStage.YoungAdult;
            Assert.False(system.CanOfferVocationalPair(child, 500, 360));
            child.Stage = DevelopmentStage.Infant;
            Assert.True(system.CanOfferVocationalPair(child, 501, 360));
            child.Milestones.Clear();
            Assert.False(system.CanOfferVocationalPair(child, 501, 360));
        }

        [Fact]
        public void DeclineSuppressesOnlyChapterAnchoredQuarter()
        {
            var system = Create(out _);
            var child = Child();
            Assert.Equal(ActionResult.StatusKind.Success,
                system.RespondVocationalPair(child, "", "", false, 501, 470).Status);
            Assert.False(system.CanOfferVocationalPair(child, 560, 470));
            Assert.True(system.CanOfferVocationalPair(child, 561, 470));
            Assert.Empty(system.State.activePairs);
            Assert.False(system.CanOfferVocationalPair(child, 720, 720));
        }

        [Fact]
        public void AcceptanceCreatesCatalogPairOnceAndSuppressesLaterOffers()
        {
            var system = Create(out _);
            var child = Child();
            Assert.Equal(ActionResult.StatusKind.Success,
                system.RespondVocationalPair(child, "mentor", Mentorship, true, 501, 360).Status);
            var pair = Assert.Single(system.State.activePairs);
            Assert.True(pair.isVocationalPair);
            Assert.Equal("skill_rough_repairs", pair.targetSkillId);
            Assert.Equal(501, pair.dayStarted);
            Assert.False(system.CanOfferVocationalPair(child, 720, 360));
            Assert.Equal(ActionResult.StatusKind.Blocked,
                system.RespondVocationalPair(child, "mentor", Mentorship, true, 501, 360).Status);
            Assert.Single(system.State.vocationalResponses);
        }

        [Fact]
        public void InvalidCatalogOrUnqualifiedMentorDoesNotConsumeConsent()
        {
            var system = Create(out _);
            var child = Child();
            Assert.Equal(ActionResult.StatusKind.Failed,
                system.RespondVocationalPair(child, "mentor", "missing", true, 501, 360).Status);
            Assert.Equal(ActionResult.StatusKind.Blocked,
                system.RespondVocationalPair(child, "unqualified", Mentorship, true, 501, 360).Status);
            Assert.True(system.CanOfferVocationalPair(child, 501, 360));
            Assert.Empty(system.State.vocationalResponses);
        }

        [Fact]
        public void MentorDeathMakesVocationalApprenticeEligibleWithoutLegacyGrant()
        {
            var system = Create(out var skills);
            system.RespondVocationalPair(Child(), "mentor", Mentorship, true, 501, 360);
            system.NotifyMentorDeath("mentor");
            system.NotifyMentorDeath("mentor");
            system.TickDay(502);
            var pair = Assert.Single(system.State.activePairs);
            Assert.True(pair.actingEligible);
            Assert.True(pair.isCancelled);
            Assert.False(pair.isLegacyInherited);
            Assert.Empty(system.GetActivePairs());
            Assert.Equal(0f, skills.GetXp("child", "skill_rough_repairs"));
            Assert.Empty(system.State.legacyTraitsGranted);
        }

        [Fact]
        public void AdultMentorDeathPreservesExistingLegacyInheritance()
        {
            var system = Create(out var skills);
            system.StartPair("mentor", "adult", "skill_rough_repairs", mentorshipId: Mentorship);
            system.NotifyMentorDeath("mentor");
            var pair = Assert.Single(system.State.activePairs);
            Assert.True(pair.isLegacyInherited);
            Assert.False(pair.actingEligible);
            Assert.True(skills.GetXp("adult", "skill_rough_repairs") > 0);
            Assert.Contains("trait_stoic_craftsman", system.State.legacyTraitsGranted["adult"]);
        }

        [Fact]
        public void OrdinaryPairCannotBypassKnownChildConsentButStillAcceptsAdults()
        {
            var system = Create(out _);
            var child = Child();
            system.ChildProfileProvider = id => id == child.ChildId ? child : null;
            Assert.Equal("consent_required", system.StartPair("mentor", "child", "skill_rough_repairs").FailureCode);
            Assert.Equal(ActionResult.StatusKind.Success,
                system.RespondVocationalPair(child, "mentor", Mentorship, true, 501, 360).Status);
            Assert.Equal(ActionResult.StatusKind.Success,
                system.StartPair("mentor", "adult", "skill_rough_repairs").Status);
        }

        [Fact]
        public void CaptureRestoreDeepCopiesConsentAndEligibilityAndReadsLegacyState()
        {
            var system = Create(out _);
            system.RespondVocationalPair(Child(), "mentor", Mentorship, true, 501, 360);
            system.NotifyMentorDeath("mentor");
            var saved = system.CaptureState();
            saved.vocationalResponses[0].mentorId = "snapshot";
            Assert.Equal("mentor", system.State.vocationalResponses[0].mentorId);
            var restored = Create(out _);
            restored.RestoreState(saved);
            saved.vocationalResponses.Clear();
            Assert.Single(restored.State.vocationalResponses);
            Assert.True(restored.State.activePairs[0].actingEligible);
            var legacy = new SystemTextJsonSerializer().Deserialize<ApprenticeshipState>("{\"activePairs\":[]}");
            restored.RestoreState(legacy);
            Assert.Empty(restored.State.vocationalResponses);
            Assert.True(restored.CanOfferVocationalPair(Child(), 501, 360));
        }
    }
}
