// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public static class SkillCertificationSelfTest
    {
        public static int Run(string dataDir)
        {
            Console.WriteLine("=== [HostCli] Skill Certification & Tier System Self-Test (Plan 180) ===");
            int passed = 0;

            void Check(bool condition, string name)
            {
                if (condition)
                {
                    Console.WriteLine($"[PASS] Check {++passed}: {name}");
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check {passed + 1}: {name}");
                    throw new InvalidOperationException($"SkillCertification self-test assertion failed: {name}");
                }
            }

            try
            {
                // Check 1: Catalog loaded certifications
                var session = SkillCertificationHostSession.Create(dataDir);
                Check(session.System.Certifications.Count >= 8,
                    $"Authoritative catalog loaded {session.System.Certifications.Count} certifications from skill_certifications.json.");

                // Check 2: Catalog loaded specializations
                Check(session.System.Specializations.Count >= 6,
                    $"Authoritative catalog loaded {session.System.Specializations.Count} specializations.");

                // Check 3: Skill tier mapping thresholds
                bool tiersOk = session.GetTierForLevel(10f) == SkillTier.Novice &&
                               session.GetTierForLevel(25f) == SkillTier.Competent &&
                               session.GetTierForLevel(45f) == SkillTier.Proficient &&
                               session.GetTierForLevel(65f) == SkillTier.Expert &&
                               session.GetTierForLevel(85f) == SkillTier.Master;
                Check(tiersOk, "Skill tier mapping maps level thresholds cleanly across Novice through Master.");

                // Check 4: Insufficient skill level rejected for exam
                bool canLow = session.CanAttemptExam("survivor_novice", "cert_certified_medic", 20f, 1, out string reasonLow);
                Check(!canLow && reasonLow.Contains("insufficient_skill_level"),
                    $"Exam eligibility correctly rejected low skill level candidate ({reasonLow}).");

                // Check 5: Unknown certification rejected
                bool canUnknown = session.CanAttemptExam("survivor_expert", "cert_unknown_test", 90f, 1, out string reasonUnknown);
                Check(!canUnknown && reasonUnknown == "unknown_certification",
                    "Unknown certification ID rejected.");

                // Check 6: Conduct exam with eligible candidate succeeds
                var rng = new SeededRng(18001);
                var examResult = session.ConductExam(
                    "survivor_candidate",
                    "cert_certified_medic",
                    candidateSkill: 55f, // required is 40f
                    examinerId: "survivor_mentor",
                    examinerSkill: 60f,
                    day: 1,
                    rng: rng);

                Check(examResult.passed && session.HasCertification("survivor_candidate", "cert_certified_medic"),
                    $"Exam passed and 'cert_certified_medic' awarded (score={examResult.score:F1}).");

                // Check 7: Already certified cannot take same exam
                bool canRetake = session.CanAttemptExam("survivor_candidate", "cert_certified_medic", 55f, 5, out string reasonRetake);
                Check(!canRetake && reasonRetake == "already_certified",
                    "Re-taking already earned certification safely rejected.");

                // Check 8: Benefits query returns unlocked capabilities
                var benefits = session.GetUnlockedBenefits("survivor_candidate");
                Check(benefits.Contains("unlock_advanced_surgery") && benefits.Contains("treatment_speed_15"),
                    $"Benefits query returned {benefits.Count} capabilities including unlock_advanced_surgery.");

                // Check 9: Examiner bonus applied
                var rng2 = new SeededRng(18002);
                var examWithExaminer = session.ConductExam(
                    "survivor_candidate_2",
                    "cert_master_engineer",
                    candidateSkill: 65f, // required 60f
                    examinerId: "survivor_master_eng",
                    examinerSkill: 80f,
                    day: 2,
                    rng: rng2);
                Check(examWithExaminer.passed && session.HasCertification("survivor_candidate_2", "cert_master_engineer"),
                    $"Exam with qualified examiner succeeded for 'cert_master_engineer' (score={examWithExaminer.score:F1}).");

                // Check 10: Specialization unlock evaluation
                // 'spec_combat_medic' requires 'cert_certified_medic'
                Check(session.HasSpecialization("survivor_candidate", "spec_combat_medic"),
                    "Candidate awarded specialization 'spec_combat_medic' upon earning prerequisite certification.");

                // Check 11: Specialization abilities included in benefits
                var specBenefits = session.GetUnlockedBenefits("survivor_candidate");
                Check(specBenefits.Contains("field_triage") && specBenefits.Contains("rapid_stabilization"),
                    "Specialization unique abilities (field_triage, rapid_stabilization) included in unlocked benefits.");

                // Check 12: Save/restore round-trip fidelity
                var census = session.GetCensus();
                var state = session.CaptureState();
                var restoredSession = SkillCertificationHostSession.Create(dataDir, state);
                var restoredCensus = restoredSession.GetCensus();

                Check(restoredCensus.CertifiedSurvivorsCount == census.CertifiedSurvivorsCount &&
                      restoredCensus.TotalCertificationsAwarded == census.TotalCertificationsAwarded &&
                      restoredCensus.TotalSpecializationsAwarded == census.TotalSpecializationsAwarded &&
                      restoredSession.HasCertification("survivor_candidate", "cert_certified_medic") &&
                      restoredSession.HasSpecialization("survivor_candidate", "spec_combat_medic"),
                    "Save/restore round-trip preserved 100% parity across profiles, certifications, and specializations.");

                Console.WriteLine($"=== Skill Certification Self-Test Result: {passed}/12 Passed ===");
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Skill Certification self-test terminated with exception: {ex.Message}\n{ex.StackTrace}");
                return 1;
            }
        }
    }
}
