// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 179: Unified Psychology & Phobia Integration Tests
// Verifies phobia catalog loading, trauma event phobia development,
// phobia exposure triggers, coping mechanism teaching, therapy sessions,
// and state persistence.
// ============================================================================
using System;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core.Psychology;
using Ashfall.Core.Random;

namespace Ashfall.Core.Tests.Plan179UnifiedPsychology
{
    public sealed class Plan179UnifiedPsychologyIntegrationTests
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
        public void LoadCatalog_LoadsAllPhobiasAndCopingMechanisms()
        {
            var sys = new PsychologicalProfileSystem();
            string path = ResolveDataPath("psychology_profiles.json");
            Assert.True(File.Exists(path), $"psychology_profiles.json must exist at {path}");

            sys.LoadCatalog(File.ReadAllText(path));

            var phobias = sys.GetAllPhobias();
            Assert.True(phobias.Count >= 6, $"Expected >= 6 phobia definitions, got {phobias.Count}");
            Assert.Contains(phobias, p => p.phobia_id == "phobia_claustrophobia");
            Assert.Contains(phobias, p => p.phobia_id == "phobia_blood_phobia");
            Assert.Contains(phobias, p => p.phobia_id == "phobia_radiation_phobia");

            var mechanisms = sys.GetAllCopingMechanisms();
            Assert.True(mechanisms.Count >= 6, $"Expected >= 6 coping mechanisms, got {mechanisms.Count}");
            Assert.Contains(mechanisms, c => c.mechanism_id == "cope_meditation");
            Assert.Contains(mechanisms, c => c.mechanism_id == "cope_creative_work");
        }

        [Fact]
        public void RecordTraumaEvent_DevelopsPhobiaAboveThreshold()
        {
            var sys = new PsychologicalProfileSystem();
            string path = ResolveDataPath("psychology_profiles.json");
            sys.LoadCatalog(File.ReadAllText(path));

            string survivorId = "survivor_hana";
            string phobiaDevelopedId = string.Empty;
            sys.OnPhobiaDeveloped += (sId, phobiaId, severity) =>
            {
                if (sId == survivorId) phobiaDevelopedId = phobiaId;
            };

            // Low resilience (20), high trauma (70) → high chance of phobia
            var profile = sys.EnsureProfile(survivorId);
            profile.resilience = 20.0f;

            var rng = new SeededRng(101);
            bool developed = sys.RecordTraumaEvent(survivorId, "combat_trauma", 70f, 5, rng);

            Assert.True(developed, "High severity trauma should develop a phobia at low resilience");
            Assert.NotEmpty(phobiaDevelopedId);
            Assert.Single(profile.phobias);
        }

        [Fact]
        public void EvaluatePhobiaExposure_TriggersActivePhobiaInContext()
        {
            var sys = new PsychologicalProfileSystem();
            sys.LoadCatalog(File.ReadAllText(ResolveDataPath("psychology_profiles.json")));

            string survivorId = "survivor_lex";
            var profile = sys.EnsureProfile(survivorId);
            profile.resilience = 15.0f;

            string triggeredPhobiaId = string.Empty;
            sys.OnPhobiaTriggered += (sId, phobiaId) =>
            {
                if (sId == survivorId) triggeredPhobiaId = phobiaId;
            };

            // Manually inject a blood phobia with high severity
            profile.phobias.Add(new ActivePhobia
            {
                phobia_id = "phobia_blood_phobia",
                severity = 65.0f,
                developed_on_day = 3,
                developed_from_trauma = "combat_trauma",
                is_managed = false
            });

            var result = sys.EvaluatePhobiaExposure(survivorId, "blood_or_injury");

            Assert.True(result.Triggered);
            Assert.Equal("phobia_blood_phobia", result.PhobiaId);
            Assert.Equal("phobia_blood_phobia", triggeredPhobiaId);
            Assert.NotEmpty(result.ActiveEffects);
        }

        [Fact]
        public void TeachCopingMechanism_RaisesResilienceAndPreventsRelearn()
        {
            var sys = new PsychologicalProfileSystem();
            sys.LoadCatalog(File.ReadAllText(ResolveDataPath("psychology_profiles.json")));

            string survivorId = "survivor_mika";
            var profile = sys.EnsureProfile(survivorId);
            float resilienceBefore = profile.resilience;

            bool learned = sys.TeachCopingMechanism(survivorId, "cope_meditation", "therapy", 8);
            Assert.True(learned);
            Assert.Single(profile.coping_mechanisms);
            Assert.True(profile.resilience > resilienceBefore, "Learning a coping mechanism should increase resilience");

            // Teaching the same mechanism again should fail
            bool learnedAgain = sys.TeachCopingMechanism(survivorId, "cope_meditation", "therapy", 9);
            Assert.False(learnedAgain);
            Assert.Single(profile.coping_mechanisms);
        }

        [Fact]
        public void ConductTherapy_ReducesPhobiaSeverityAndManagesAtLowSeverity()
        {
            var sys = new PsychologicalProfileSystem();
            sys.LoadCatalog(File.ReadAllText(ResolveDataPath("psychology_profiles.json")));

            string survivorId = "survivor_crow";
            var profile = sys.EnsureProfile(survivorId);
            profile.phobias.Add(new ActivePhobia
            {
                phobia_id = "phobia_nyctophobia",
                severity = 12.0f,   // Low enough that good therapy will manage it
                developed_on_day = 1,
                developed_from_trauma = "nighttime_attack",
                is_managed = false
            });

            float resilienceBefore = profile.resilience;

            // High therapist skill of 90 should reduce by 13.5 → severity drops to < 5 → managed
            bool success = sys.ConductTherapySession(survivorId, "phobia_nyctophobia", therapistSkill: 90f);
            Assert.True(success);
            Assert.True(profile.phobias[0].is_managed, "Phobia should be marked managed when severity drops <= 5");
            Assert.True(profile.resilience > resilienceBefore, "Managing a phobia should boost resilience");
        }

        [Fact]
        public void SaveRestoreState_PreservesProfilesPhobiasAndCounters()
        {
            var sys = new PsychologicalProfileSystem();
            sys.LoadCatalog(File.ReadAllText(ResolveDataPath("psychology_profiles.json")));

            var rng = new SeededRng(777);
            var profileA = sys.EnsureProfile("survivor_alpha");
            profileA.resilience = 30.0f;
            sys.RecordTraumaEvent("survivor_alpha", "combat_trauma", 75f, 4, rng);

            sys.TeachCopingMechanism("survivor_alpha", "cope_exercise", "experience", 5);
            sys.ConductTherapySession("survivor_alpha", profileA.phobias.FirstOrDefault()?.phobia_id ?? "none", 60f);

            var captured = sys.CaptureState();
            var restored = new PsychologicalProfileSystem();
            restored.RestoreState(captured);

            var restoredProfile = restored.GetProfile("survivor_alpha");
            Assert.NotNull(restoredProfile);
            Assert.Equal(profileA.trauma_event_count, restoredProfile.trauma_event_count);
            Assert.Equal(profileA.coping_mechanisms.Count, restoredProfile.coping_mechanisms.Count);
        }
    }
}
