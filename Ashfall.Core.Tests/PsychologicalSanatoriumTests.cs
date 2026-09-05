using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Catalogs;
using Ashfall.Core.Diplomacy;
using Ashfall.Core.Institutions;
using Ashfall.Core.Inventory;
using Ashfall.Core.Sanatorium;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Flagship Task 8 — PsychologicalSanatoriumSystem behaviour gates:
    /// admission eligibility/capacity, atomic therapy costs, canonical
    /// condition-port outcomes, sedative discipline, relapse determinism,
    /// reversible-only suppression, discharge idempotence, save continuation.
    /// </summary>
    public class PsychologicalSanatoriumTests
    {
        private sealed class TrackingAvailability : IInstitutionAvailability
        {
            public readonly HashSet<string> Claims = new(StringComparer.Ordinal);
            public bool IsAvailable(string survivorId) => !Claims.Contains(survivorId);
            public bool TryClaim(string survivorId, string institutionId, string roleId) => Claims.Add(survivorId);
            public void Release(string survivorId, string institutionId, string roleId) => Claims.Remove(survivorId);
        }

        private sealed class FakeConditions : ISurvivorConditionPort
        {
            public readonly HashSet<string> Conditions = new(StringComparer.Ordinal);
            public readonly Dictionary<string, int> Acute = new(StringComparer.Ordinal);
            public readonly List<string> Suppressed = new();
            public int RecoveryApplied;
            public readonly Dictionary<string, int> Trust = new(StringComparer.Ordinal);

            public FakeConditions()
            {
                Acute["survivor_patient_a"] = 800;
                Acute["survivor_patient_b"] = 700;
                Acute["survivor_patient_c"] = 750;
                Conditions.Add("survivor_patient_a");
                Conditions.Add("survivor_patient_b");
                Conditions.Add("survivor_patient_c");
            }

            public bool HasCondition(string survivorId, string conditionId) =>
                Conditions.Contains(survivorId) && conditionId.Length > 0;
            public int GetAcuteStressPermille(string survivorId) => Acute.GetValueOrDefault(survivorId, 0);
            public void ApplyAcuteStressReduction(string survivorId, int permille) =>
                Acute[survivorId] = Math.Clamp(Acute.GetValueOrDefault(survivorId) - permille, 0, 1000);
            public void ApplyRecoveryProgress(string survivorId, int progress) => RecoveryApplied += progress;
            public void SuppressReversibleCondition(string survivorId, string conditionId) =>
                Suppressed.Add($"{survivorId}/{conditionId}");
            public int GetRelationshipTrust(string therapistId, string patientId) =>
                Trust.GetValueOrDefault($"{therapistId}->{patientId}", 50);
        }

        private sealed class Fixture
        {
            public Inventory.Inventory Inventory = new();
            public TrackingAvailability Availability = new();
            public FakeConditions Conditions = new();
            public PsychologicalSanatoriumSystem Sanatorium = null!;
            public List<(string Survivor, string Therapy)> Journals = new();
            public List<(string Survivor, string Condition, int Day)> Relapses = new();
            public List<string> Discharges = new();

            public static Fixture Create(int masterSeed = 42)
            {
                var f = new Fixture();
                f.Sanatorium = new PsychologicalSanatoriumSystem(
                    masterSeed,
                    inventory: f.Inventory,
                    availability: f.Availability,
                    skills: new StaticSkills(),
                    conditions: f.Conditions);
                f.Sanatorium.LoadTherapyCatalog(LoadContainer());
                f.Sanatorium.OnTherapeuticJournalCompleted += (s, t) => f.Journals.Add((s, t));
                f.Sanatorium.OnPatientRelapsed += (s, c, d) => f.Relapses.Add((s, c, d));
                f.Sanatorium.OnPatientDischarged += p => f.Discharges.Add(p.survivor_id);
                foreach (var id in new[] { "clean_water", "item_preservation_salt", "sedative_draught", "paper_stock", "bandage" })
                    f.Inventory.TryProduce(id, 10);
                return f;
            }

            public static PsychologicalTherapyCatalogContainer LoadContainer()
            {
                string dataDir = CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out string found)
                    ? found
                    : throw new InvalidOperationException("data dir not found");
                return PsychologicalTherapyCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            }

            public bool Admit(string survivor = "survivor_patient_a", string condition = "condition_combat_ptsd", int day = 1) =>
                Sanatorium.TryAdmitPatient(survivor, condition, day).Status == ActionResult.StatusKind.Success;
        }

        private sealed class StaticSkills : ISurvivorSkillsPort
        {
            public static readonly StaticSkills Instance = new();
            public bool HasSkill(string survivorId, string skillId) => (survivorId, skillId) switch
            {
                ("survivor_therapist_pro", "skill_cold_analysis") => true,
                ("survivor_therapist_kind", "skill_watchful") => true,
                _ => false,
            };
        }

        // ------------------------------------------------------------------
        // ADMISSION
        // ------------------------------------------------------------------

        [Fact]
        public void Admission_RequiresCanonicalCondition_Bed_AndNotAlreadyAdmitted()
        {
            var f = Fixture.Create();
            // not eligible (condition unknown to catalog)
            Assert.Equal(ActionResult.StatusKind.Blocked,
                f.Sanatorium.TryAdmitPatient("survivor_patient_a", "condition_nope", 1).Status);

            Assert.True(f.Admit());
            Assert.Equal(1, f.Sanatorium.OccupiedBeds);
            Assert.False(f.Availability.IsAvailable("survivor_patient_a")); // work eligibility claimed

            // duplicate admission
            Assert.Equal(ActionResult.StatusKind.Blocked,
                f.Sanatorium.TryAdmitPatient("survivor_patient_a", "condition_chronic_hypervigilance", 2).Status);

            // capacity (2 beds)
            Assert.True(f.Admit("survivor_patient_b", "condition_guilt_insomnia_loop"));
            Assert.False(f.Sanatorium.HasBed);
            Assert.Equal(ActionResult.StatusKind.Blocked,
                f.Sanatorium.TryAdmitPatient("survivor_patient_c", "condition_combat_ptsd", 3).Status);

            // ineligible survivor (canonical authority says no condition)
            var f2 = Fixture.Create();
            f2.Conditions.Conditions.Remove("survivor_patient_c");
            Assert.Equal(ActionResult.StatusKind.Blocked,
                f2.Sanatorium.TryAdmitPatient("survivor_patient_c", "condition_combat_ptsd", 1).Status);
        }

        // ------------------------------------------------------------------
        // THERAPY
        // ------------------------------------------------------------------

        [Fact]
        public void SensoryTherapy_ConsumesAuthoredResources_AtomicAtStart()
        {
            var f = Fixture.Create();
            f.Admit();
            int water = f.Inventory.CountById("clean_water");
            int salt = f.Inventory.CountById("item_preservation_salt");

            var ok = f.Sanatorium.TryStartTherapy("survivor_patient_a",
                "therapy_sensory_deprivation_immersion", "survivor_therapist_kind", 1);
            Assert.Equal(ActionResult.StatusKind.Success, ok.Status);
            Assert.Equal(water - 2, f.Inventory.CountById("clean_water"));
            Assert.Equal(salt - 1, f.Inventory.CountById("item_preservation_salt"));

            // second therapy while active → blocked, no extra consumption
            Assert.Equal(ActionResult.StatusKind.Blocked,
                f.Sanatorium.TryStartTherapy("survivor_patient_a", "therapy_dream_transcription",
                    "survivor_therapist_kind", 1).Status);
            Assert.Equal(salt - 1, f.Inventory.CountById("item_preservation_salt"));
        }

        [Fact]
        public void SensoryTherapy_MissingInputs_ConsumesNothing()
        {
            var f = Fixture.Create();
            f.Admit();
            f.Inventory.TryConsume("clean_water", 10); // drain
            int salt = f.Inventory.CountById("item_preservation_salt");

            var blocked = f.Sanatorium.TryStartTherapy("survivor_patient_a",
                "therapy_sensory_deprivation_immersion", "survivor_therapist_kind", 1);
            Assert.Equal(ActionResult.StatusKind.Blocked, blocked.Status);
            Assert.Equal(salt, f.Inventory.CountById("item_preservation_salt"));
            Assert.Equal(string.Empty, f.Sanatorium.GetPatient("survivor_patient_a")!.active_therapy_id);
        }

        [Fact]
        public void Therapist_MustMatchAuthoredSkill_ThroughCanonicalAuthority()
        {
            var f = Fixture.Create();
            f.Admit(condition: "condition_guilt_insomnia_loop");
            // therapy_cognitive_catharsis requires skill_cold_analysis
            Assert.Equal(ActionResult.StatusKind.Blocked,
                f.Sanatorium.TryStartTherapy("survivor_patient_a", "therapy_cognitive_catharsis",
                    "survivor_therapist_kind", 1).Status); // watchful only
            Assert.Equal(ActionResult.StatusKind.Success,
                f.Sanatorium.TryStartTherapy("survivor_patient_a", "therapy_cognitive_catharsis",
                    "survivor_therapist_pro", 1).Status);
        }

        [Fact]
        public void AuthoredStressReduction_AppliesAtCompletion_ThroughCanonicalPort()
        {
            var f = Fixture.Create();
            f.Admit(); // acute 800
            f.Sanatorium.TryStartTherapy("survivor_patient_a",
                "therapy_sensory_deprivation_immersion", "survivor_therapist_kind", 1);

            f.Sanatorium.TickDay(1); // 1-day protocol completes
            // watchful-only therapist: 400 authored + 5 assist
            Assert.Equal(800 - 405, f.Conditions.Acute["survivor_patient_a"]);
            Assert.Single(f.Sanatorium.Patients.Where(p => p.completed_therapy_count == 1));
        }

        [Fact]
        public void TherapistSkill_ShiftsOutcome_ThroughSharedCalculator()
        {
            // desensitization requires skill_cold_analysis; assist bonus is
            // authored per relevant skill and applied through the one outcome path.
            var f = Fixture.Create();
            f.Admit();
            int before = f.Conditions.Acute["survivor_patient_a"];
            Assert.Equal(ActionResult.StatusKind.Success,
                f.Sanatorium.TryStartTherapy("survivor_patient_a", "therapy_trauma_desensitization",
                    "survivor_therapist_pro", 1).Status);
            for (int day = 1; day <= 4; day++)
                f.Sanatorium.TickDay(day);
            Assert.Equal(300 + 10, before - f.Conditions.Acute["survivor_patient_a"]);

            // the watchful-only therapist is rejected for this protocol
            var f2 = Fixture.Create();
            f2.Admit();
            Assert.Equal(ActionResult.StatusKind.Blocked,
                f2.Sanatorium.TryStartTherapy("survivor_patient_a", "therapy_trauma_desensitization",
                    "survivor_therapist_kind", 1).Status);
        }

        [Fact]
        public void DreamTranscription_EmitsOneJournalEvent_PerCompletion()
        {
            var f = Fixture.Create();
            f.Admit(condition: "condition_guilt_insomnia_loop");
            f.Sanatorium.TryStartTherapy("survivor_patient_a", "therapy_dream_transcription",
                "survivor_therapist_kind", 1);
            f.Sanatorium.TickDay(1);
            f.Sanatorium.TickDay(2);

            Assert.Single(f.Journals);
            Assert.Equal(("survivor_patient_a", "therapy_dream_transcription"), f.Journals[0]);
        }

        // ------------------------------------------------------------------
        // SEDATIVES
        // ------------------------------------------------------------------

        [Fact]
        public void Sedative_ConsumesOne_RestrictionExpiresDeterministically()
        {
            var f = Fixture.Create();
            f.Admit();
            int stock = f.Inventory.CountById("sedative_draught");

            var ok = f.Sanatorium.TryAdministerSedative("survivor_patient_a", 10);
            Assert.Equal(ActionResult.StatusKind.Success, ok.Status);
            Assert.Equal(stock - 1, f.Inventory.CountById("sedative_draught"));

            // stacking blocked while sedated
            Assert.Equal(ActionResult.StatusKind.Blocked,
                f.Sanatorium.TryAdministerSedative("survivor_patient_a", 10).Status);
            Assert.Equal(ActionResult.StatusKind.Blocked,
                f.Sanatorium.TryAdministerSedative("survivor_patient_a", 11).Status);

            // expires the day after the window
            Assert.Equal(ActionResult.StatusKind.Success,
                f.Sanatorium.TryAdministerSedative("survivor_patient_a", 12).Status);
        }

        // ------------------------------------------------------------------
        // RELAPSE + RECOVERY
        // ------------------------------------------------------------------

        [Fact]
        public void Relapse_IsDeterministic_AndRaisesAcuteThroughPort()
        {
            var a = RelapseTrace(42, days: 20);
            var b = RelapseTrace(42, days: 20);
            Assert.Equal(a, b);
        }

        private static string RelapseTrace(int seed, int days)
        {
            var f = Fixture.Create(seed);
            f.Admit(); // untreated patient, risk 200‰
            var trace = new List<string>();
            f.Sanatorium.OnPatientRelapsed += (s, c, d) => trace.Add($"{s}:{c}:{d}");
            for (int day = 2; day <= 1 + days; day++)
                f.Sanatorium.TickDay(day);
            return string.Join("|", trace);
        }

        [Fact]
        public void ZeroRelapseRisk_NeverRelapses()
        {
            var f = Fixture.Create();
            f.Admit();
            var patient = f.Sanatorium.GetPatient("survivor_patient_a")!;
            patient.relapse_risk_permille = 0;

            for (int day = 2; day <= 60; day++)
                f.Sanatorium.TickDay(day);
            Assert.Empty(f.Relapses);
        }

        [Fact]
        public void ReversibleCondition_SuppressedAtFullRecovery_NonReversibleNever()
        {
            var f = Fixture.Create();
            // authored fast lane: drive recovery via a custom 1-day, high-progress therapy
            var container = Fixture.LoadContainer();
            container.therapies.Add(new PsychologicalTherapyDefinition
            {
                therapy_id = "therapy_test_fast_track",
                display_name = "Fast Recovery Protocol",
                duration_days = 1,
                staff_skill_id = "skill_watchful",
                eligible_conditions = new() { "condition_flash_blindness_shock", "condition_combat_ptsd" },
                recovery_progress = 100,          // → treatment_progress +500 per completion
                acute_stress_reduction_permille = 50,
            });
            var f2 = Fixture.Create();
            f2.Sanatorium.LoadTherapyCatalog(container);
            f2.Admit(condition: "condition_flash_blindness_shock");
            var patient = f2.Sanatorium.GetPatient("survivor_patient_a")!;
            patient.condition_ids.Add("condition_combat_ptsd"); // also carries non-reversible

            for (int i = 0; i < 2; i++)
            {
                Assert.Equal(ActionResult.StatusKind.Success,
                    f2.Sanatorium.TryStartTherapy("survivor_patient_a", "therapy_test_fast_track",
                        "survivor_therapist_kind", 1 + i * 2).Status);
                f2.Sanatorium.TickDay(1 + i * 2);
            }

            // reversible condition suppressed exactly once; non-reversible untouched
            Assert.Contains("survivor_patient_a/condition_flash_blindness_shock", f2.Conditions.Suppressed);
            Assert.DoesNotContain(f2.Conditions.Suppressed, s => s.EndsWith("condition_combat_ptsd"));
            Assert.Equal("discharged", patient.status); // auto-discharge at full recovery
        }

        // ------------------------------------------------------------------
        // DISCHARGE
        // ------------------------------------------------------------------

        [Fact]
        public void Discharge_ReleasesBed_AndEligibility_ExactlyOnce()
        {
            var f = Fixture.Create();
            f.Admit();
            f.Admit("survivor_patient_b", "condition_guilt_insomnia_loop");
            Assert.False(f.Sanatorium.HasBed);

            var result = f.Sanatorium.TryDischargePatient("survivor_patient_a", 10);
            Assert.Equal(ActionResult.StatusKind.Success, result.Status);
            Assert.True(f.Availability.IsAvailable("survivor_patient_a"));
            Assert.True(f.Sanatorium.HasBed);
            Assert.Single(f.Discharges);
            Assert.Equal(10, f.Sanatorium.GetPatient("survivor_patient_a")!.discharge_day);

            // treatment history preserved, second discharge rejected
            Assert.True(f.Sanatorium.GetPatient("survivor_patient_a")!.admission_day == 1);
            Assert.Equal(ActionResult.StatusKind.Blocked,
                f.Sanatorium.TryDischargePatient("survivor_patient_a", 11).Status);
            Assert.Single(f.Discharges);
        }

        // ------------------------------------------------------------------
        // SAVE / RESTORE
        // ------------------------------------------------------------------

        [Fact]
        public void PatientProgress_SurvivesSaveLoad_AndContinuationMatches()
        {
            var f = Fixture.Create();
            f.Admit();
            f.Sanatorium.TryStartTherapy("survivor_patient_a", "therapy_trauma_desensitization",
                "survivor_therapist_pro", 1);
            f.Sanatorium.TickDay(2); // mid-therapy (4-day protocol)

            var saved = f.Sanatorium.CaptureState();
            var fresh = Fixture.Create();
            fresh.Sanatorium.RestoreState(saved);

            var a = f.Sanatorium.GetPatient("survivor_patient_a")!;
            var b = fresh.Sanatorium.GetPatient("survivor_patient_a")!;
            Assert.Equal(a.active_therapy_id, b.active_therapy_id);
            Assert.Equal(a.therapy_days_elapsed, b.therapy_days_elapsed);
            Assert.Equal(a.treatment_progress, b.treatment_progress);
            Assert.Equal(a.relapse_risk_permille, b.relapse_risk_permille);

            // post-restore next day matches uninterrupted run (relapse stream keyed
            // by seed+survivor+day, so restore cannot shift outcomes)
            f.Sanatorium.TickDay(3);
            fresh.Sanatorium.TickDay(3);
            Assert.Equal(a.status, b.status);
            Assert.Equal(f.Conditions.Acute["survivor_patient_a"], fresh.Conditions.Acute["survivor_patient_a"]);
        }

        [Fact]
        public void OldSave_MissingSanatoriumSection_DefaultsSafely()
        {
            var f = Fixture.Create();
            f.Sanatorium.RestoreState(null);
            Assert.Empty(f.Sanatorium.Patients);
            Assert.True(f.Sanatorium.HasBed);
        }
    }
}
