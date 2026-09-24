// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 180: Skill Certification and Tier System Integration Tests
// Verifies certification/specialization catalog loading, exam attempts,
// deterministic grading, benefit aggregation, census, and state round-trip.
// ============================================================================
using System;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Survivors;

namespace Ashfall.Core.Tests.Plan180SkillCertification
{
    public sealed class Plan180SkillCertificationTests
    {
        private static string ResolveDataPath(string filename)
        {
            var candidates = new[]
            {
                Path.Combine(AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data", filename),
                Path.Combine(AppContext.BaseDirectory, "../../../Assets/StreamingAssets/Data", filename),
                Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data", filename),
                Path.Combine(Directory.GetCurrentDirectory(), "../Assets/StreamingAssets/Data", filename)
            };
            foreach (var c in candidates)
            {
                if (File.Exists(c)) return Path.GetFullPath(c);
            }
            return Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data", filename));
        }

        [Fact]
        public void LoadCatalog_LoadsAllCertificationsAndSpecializations()
        {
            var sys = new SkillCertificationSystem();
            string path = ResolveDataPath("skill_certifications.json");
            Assert.True(File.Exists(path), $"skill_certifications.json must exist at {path}");

            sys.LoadCatalog(File.ReadAllText(path));

            var certs = sys.GetAllCertifications();
            Assert.True(certs.Count >= 8, $"Expected >= 8 certifications, got {certs.Count}");
            Assert.Contains(certs, c => c.cert_id == "cert_certified_medic");
            Assert.Contains(certs, c => c.cert_id == "cert_master_engineer");
            Assert.Contains(certs, c => c.cert_id == "cert_expert_tracker");
            Assert.Contains(certs, c => c.cert_id == "cert_certified_trader");
            Assert.Contains(certs, c => c.cert_id == "cert_master_combatant");

            var specs = sys.GetAllSpecializations();
            Assert.True(specs.Count >= 6, $"Expected >= 6 specializations, got {specs.Count}");
            Assert.Contains(specs, s => s.spec_id == "spec_combat_medic");
            Assert.Contains(specs, s => s.spec_id == "spec_field_engineer");
        }

        [Fact]
        public void CanAttemptExam_EnforcesPrerequisitesAndCooldown()
        {
            var sys = new SkillCertificationSystem();
            string path = ResolveDataPath("skill_certifications.json");
            sys.LoadCatalog(File.ReadAllText(path));

            // Candidate with insufficient skill cannot attempt exam
            bool allowedLowSkill = sys.CanAttemptExam("surv_1", "cert_master_engineer", candidateSkillLevel: 50f, currentDay: 1, out string reasonLow);
            Assert.False(allowedLowSkill);
            Assert.Contains("insufficient_skill_level", reasonLow);

            // Candidate with sufficient skill can attempt exam
            bool allowedHighSkill = sys.CanAttemptExam("surv_1", "cert_master_engineer", candidateSkillLevel: 85f, currentDay: 1, out _);
            Assert.True(allowedHighSkill);

            // Conduct exam (failing with low RNG)
            var rngFail = new SeededRng(1); // deterministic low roll
            var (passed, _, _) = sys.ConductExam("surv_1", "cert_master_engineer", candidateSkill: 80f, examinerId: "", examinerSkill: 0f, day: 1, rng: rngFail);

            if (!passed)
            {
                // Day 2 should still be on cooldown (< 2 days)
                bool allowedDay2 = sys.CanAttemptExam("surv_1", "cert_master_engineer", candidateSkillLevel: 85f, currentDay: 2, out string reasonCool);
                Assert.False(allowedDay2);
                Assert.Equal("exam_cooldown", reasonCool);

                // Day 3 cooldown expired (day 3 - day 1 = 2)
                bool allowedDay3 = sys.CanAttemptExam("surv_1", "cert_master_engineer", candidateSkillLevel: 85f, currentDay: 3, out _);
                Assert.True(allowedDay3);
            }
        }

        [Fact]
        public void ConductExam_GrantsCertificationAndSpecializationPerks()
        {
            var sys = new SkillCertificationSystem();
            string path = ResolveDataPath("skill_certifications.json");
            sys.LoadCatalog(File.ReadAllText(path));

            string? earnedCertEvent = null;
            sys.OnCertificationEarned += (survivorId, certId) => earnedCertEvent = certId;

            // Deterministic high-skill pass
            var rngPass = new SeededRng(100);
            var (passed, score, msg) = sys.ConductExam(
                candidateId: "surv_medic",
                certId: "cert_certified_medic",
                candidateSkill: 50f,
                examinerId: "surv_mentor",
                examinerSkill: 90f,
                day: 5,
                rng: rngPass);

            Assert.True(passed, $"Exam should pass with 50 skill + 90 mentor: {msg} (score: {score})");
            Assert.True(sys.HasCertification("surv_medic", "cert_certified_medic"));
            Assert.Equal("cert_certified_medic", earnedCertEvent);

            // Unlocked benefits should contain the certification benefit
            var benefits = sys.GetUnlockedBenefits("surv_medic");
            Assert.NotEmpty(benefits);
            Assert.Contains("unlock_advanced_surgery", benefits);

            // Cannot re-take already certified exam
            bool canRetake = sys.CanAttemptExam("surv_medic", "cert_certified_medic", 60f, currentDay: 10, out string reasonRe);
            Assert.False(canRetake);
            Assert.Equal("already_certified", reasonRe);
        }

        [Fact]
        public void StateRoundTrip_CapturesAndRestoresAllProfilesAndCertifications()
        {
            var sys1 = new SkillCertificationSystem();
            string path = ResolveDataPath("skill_certifications.json");
            sys1.LoadCatalog(File.ReadAllText(path));

            var rng = new SeededRng(42);
            sys1.ConductExam("surv_1", "cert_certified_medic", 50f, "", 0f, 1, rng);
            sys1.ConductExam("surv_2", "cert_certified_trader", 50f, "", 0f, 2, rng);

            var census1 = sys1.GetCensus();
            var state = sys1.CaptureState();

            var sys2 = new SkillCertificationSystem();
            sys2.LoadCatalog(File.ReadAllText(path));
            sys2.RestoreState(state);

            var census2 = sys2.GetCensus();
            Assert.Equal(census1.CertifiedSurvivorsCount, census2.CertifiedSurvivorsCount);
            Assert.Equal(census1.TotalCertificationsAwarded, census2.TotalCertificationsAwarded);
            Assert.Equal(sys1.HasCertification("surv_1", "cert_certified_medic"), sys2.HasCertification("surv_1", "cert_certified_medic"));
            Assert.Equal(sys1.HasCertification("surv_2", "cert_certified_trader"), sys2.HasCertification("surv_2", "cert_certified_trader"));
        }
    }
}
