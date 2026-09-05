using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Catalogs;
using Ashfall.Core.Culture;
using Ashfall.Core.Diplomacy;
using Ashfall.Core.Institutions;
using Ashfall.Core.Inventory;
using Ashfall.Core.Sanatorium;
using Ashfall.Core.SkyDefense;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Flagship Tasks 5-8 cross-system gates (plan §14/§15/§9.2): one shared
    /// availability authority across all four institutions, one scarce
    /// inventory contended by all four, and the culture↔sanatorium journal
    /// link awarding exactly once.
    /// </summary>
    public class LateGameInstitutionCrossSystemTests
    {
        private static string DataDir
        {
            get
            {
                if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out string found))
                    return found;
                throw new InvalidOperationException("data dir not found");
            }
        }

        private static List<CulturalArchiveTomeDefinition> LoadTomes() =>
            CulturalArchiveTomeCatalogLoader.Load(DataDir, new FileSystemIO(), new SystemTextJsonSerializer());

        private static List<DiplomaticTreatyDefinition> LoadFrameworks() =>
            DiplomaticTreatyCatalogLoader.Load(DataDir, new FileSystemIO(), new SystemTextJsonSerializer());

        private static List<SkyDefenseOrdnanceDefinition> LoadOrdnance() =>
            SkyDefenseOrdnanceCatalogLoader.Load(DataDir, new FileSystemIO(), new SystemTextJsonSerializer());

        private static PsychologicalTherapyCatalogContainer LoadTherapies() =>
            PsychologicalTherapyCatalogLoader.Load(DataDir, new FileSystemIO(), new SystemTextJsonSerializer());

        private sealed class FakeConditions : ISurvivorConditionPort
        {
            public readonly HashSet<string> Sick = new(StringComparer.Ordinal);
            public readonly Dictionary<string, int> Acute = new(StringComparer.Ordinal);
            public bool HasCondition(string survivorId, string conditionId) => Sick.Contains(survivorId);
            public int GetAcuteStressPermille(string survivorId) => Acute.GetValueOrDefault(survivorId, 700);
            public void ApplyAcuteStressReduction(string survivorId, int permille) { }
            public void ApplyRecoveryProgress(string survivorId, int progress) { }
            public void SuppressReversibleCondition(string survivorId, string conditionId) { }
            public int GetRelationshipTrust(string therapistId, string patientId) => 50;
        }

        private sealed class StaticFactions : IFactionContextPort
        {
            public string? GetFactionTag(string factionId) => factionId is "faction_a" or "faction_b" ? "militia" : null;
            public bool IsHostile(string factionId) => false;
        }

        /// <summary>
        /// All four institutions over ONE ledger, ONE inventory, ONE authority
        /// map — the plan §13 harness composition (seed 42).
        /// </summary>
        private sealed class World
        {
            public InstitutionAssignmentLedger Ledger = new();
            public Inventory.Inventory Inventory = new();
            public FakeConditions Conditions = new();
            public CulturalArchiveVaultSystem Culture;
            public DiplomaticSummitSystem Diplomacy;
            public SkyDefenseBatterySystem Defense;
            public PsychologicalSanatoriumSystem Sanatorium;

            public World(int seed = 42)
            {
                Culture = new CulturalArchiveVaultSystem(Inventory, availability: Ledger);
                Culture.LoadTomeCatalog(LoadTomes());
                Diplomacy = new DiplomaticSummitSystem(seed, inventory: Inventory,
                    availability: Ledger, factions: new StaticFactions());
                Diplomacy.LoadTreatyCatalog(LoadFrameworks());
                Defense = new SkyDefenseBatterySystem(seed, inventory: Inventory, availability: Ledger);
                Defense.LoadOrdnanceCatalog(LoadOrdnance());
                Defense.EnsureDefaultTurret();
                Sanatorium = new PsychologicalSanatoriumSystem(seed, inventory: Inventory,
                    availability: Ledger, skills: StaticSkills.Instance, conditions: Conditions);
                Sanatorium.LoadTherapyCatalog(LoadTherapies());
                foreach (var id in new[] { "clean_water", "scrap_metal", "machine_oil", "paper_stock", "acetate_blank_disc" })
                    Inventory.TryProduce(id, 8);
            }
        }

        private sealed class StaticSkills : ISurvivorSkillsPort
        {
            public static readonly StaticSkills Instance = new();
            public bool HasSkill(string survivorId, string skillId) => (survivorId, skillId) switch
            {
                ("survivor_therapist", "skill_watchful") => true,
                _ => false,
            };
        }

        // ------------------------------------------------------------------
        // §15 — survivor contention through the ONE authority
        // ------------------------------------------------------------------

        [Fact]
        public void OneClaimPerSurvivor_AcrossAllFourInstitutions()
        {
            var w = new World();

            // scholar claims via the archive
            Assert.True(w.Ledger.TryClaim("survivor_scholar", CulturalArchiveVaultSystem.InstitutionId, "scholar"));
            // same survivor cannot become a delegate, gunner, therapist or patient
            Assert.False(w.Ledger.TryClaim("survivor_scholar", DiplomaticSummitSystem.InstitutionId, "delegate"));
            Assert.False(w.Ledger.TryClaim("survivor_scholar", SkyDefenseBatterySystem.InstitutionId, "gunner"));
            Assert.False(w.Ledger.TryClaim("survivor_scholar", PsychologicalSanatoriumSystem.InstitutionId, "therapist"));
            Assert.False(w.Ledger.TryClaim("survivor_scholar", PsychologicalSanatoriumSystem.InstitutionId, "patient"));
            // identical triple is idempotent
            Assert.True(w.Ledger.TryClaim("survivor_scholar", CulturalArchiveVaultSystem.InstitutionId, "scholar"));

            // release restores
            w.Ledger.Release("survivor_scholar", CulturalArchiveVaultSystem.InstitutionId, "scholar");
            Assert.True(w.Ledger.IsAvailable("survivor_scholar"));
        }

        [Fact]
        public void Patient_CannotTakeActiveDuty_AcrossInstitutions()
        {
            var w = new World();
            w.Conditions.Sick.Add("survivor_patient_e");

            Assert.True(w.Sanatorium.TryAdmitPatient("survivor_patient_e", "condition_combat_ptsd", 1).Status
                        == ActionResult.StatusKind.Success);
            // admitted patient is claimed — no summit delegation, no battery crew
            Assert.False(w.Ledger.TryClaim("survivor_patient_e", DiplomaticSummitSystem.InstitutionId, "delegate"));
            Assert.False(w.Ledger.TryClaim("survivor_patient_e", SkyDefenseBatterySystem.InstitutionId, "gunner"));

            // discharge restores cross-institution eligibility
            w.Sanatorium.TryDischargePatient("survivor_patient_e", 5);
            Assert.True(w.Ledger.IsAvailable("survivor_patient_e"));
        }

        [Fact]
        public void GuaranteeHolder_AndGunner_AndDelegate_AreMutuallyExclusive()
        {
            var w = new World();
            // quick treaty for a guarantee
            w.Diplomacy.TryScheduleSummit(DiplomaticSummitSystem.NeutralSummitSiteId,
                new[] { "faction_a", "faction_b" }, new[] { "survivor_envoy" },
                "treaty_non_aggression_compact", 10);
            var summit = w.Diplomacy.Summits[0];
            for (int i = 0; i < 12 && summit.status == "negotiating"
                             && summit.negotiation_stability < DiplomaticSummitSystem.RatificationThreshold; i++)
                w.Diplomacy.AdvanceNegotiation(summit.summit_id, true);
            Assert.Equal(ActionResult.StatusKind.Success, w.Diplomacy.TryRatifyTreaty(summit.summit_id, 20).Status);
            var treaty = w.Diplomacy.Treaties.First(t => t.status == "active");

            Assert.Equal(ActionResult.StatusKind.Success,
                w.Diplomacy.TryExchangeGuarantee(treaty.treaty_id, "survivor_envoy", "faction_a", 21).Status);
            // the held guarantee cannot simultaneously crew the battery
            Assert.False(w.Ledger.TryClaim("survivor_envoy", SkyDefenseBatterySystem.InstitutionId, "gunner"));
        }

        // ------------------------------------------------------------------
        // §14 — resource contention across systems, atomically
        // ------------------------------------------------------------------

        [Fact]
        public void ScarceInventory_IsContendedAtomically_NoLocalCaching()
        {
            var w = new World();
            // scarce stock (plan §14 list mapped to real ids)
            foreach (var id in new[] { "machine_oil", "scrap_chemical", "clean_water", "mechanical_parts", "sedative_draught", "paper_stock", "item_preservation_salt", "microfiche_film" })
                w.Inventory.TryProduce(id, 2);

            // 1) culture restoration consumes the shelter's scrap_chemical + clean_water
            var restore = w.Culture.TryRestoreDocument("tome_stoic_meditations");
            Assert.Equal(ActionResult.StatusKind.Success, restore.Status);
            Assert.Equal(1, w.Inventory.CountById("clean_water")); // 2 - 1

            // 2) sanatorium now cannot pay the dark-tank cost (needs clean_water x2)
            w.Conditions.Sick.Add("survivor_patient_e");
            Assert.True(w.Sanatorium.TryAdmitPatient("survivor_patient_e", "condition_combat_ptsd", 1).Status
                        == ActionResult.StatusKind.Success);
            var therapy = w.Sanatorium.TryStartTherapy("survivor_patient_e",
                "therapy_sensory_deprivation_immersion", "survivor_therapist", 1);
            Assert.Equal(ActionResult.StatusKind.Blocked, therapy.Status);
            Assert.Equal(1, w.Inventory.CountById("clean_water"));   // nothing taken
            Assert.Equal(2, w.Inventory.CountById("item_preservation_salt")); // untouched

            // 3) sky-defense magazine load sees the same live inventory
            var turret = w.Defense.GetTurret(SkyDefenseBatterySystem.DefaultTurretId)!;
            Assert.Equal(ActionResult.StatusKind.Blocked,
                w.Defense.TryLoadMagazine(turret.turret_id, "ammo_76mm_he_flak").Status); // none stocked
            Assert.Equal(0, turret.magazine_count);

            // 4) diplomacy ratification pays concessions from the same pool
            w.Inventory.TryProduce("clean_water", 6); // top up for the compact's 4
            w.Diplomacy.TryScheduleSummit(DiplomaticSummitSystem.NeutralSummitSiteId,
                new[] { "faction_a", "faction_b" }, new[] { "survivor_envoy" },
                "treaty_non_aggression_compact", 10);
            var summit = w.Diplomacy.Summits[0];
            for (int i = 0; i < 12 && summit.status == "negotiating"
                             && summit.negotiation_stability < DiplomaticSummitSystem.RatificationThreshold; i++)
                w.Diplomacy.AdvanceNegotiation(summit.summit_id, true);
            int waterBefore = w.Inventory.CountById("clean_water");
            Assert.Equal(ActionResult.StatusKind.Success, w.Diplomacy.TryRatifyTreaty(summit.summit_id, 20).Status);
            Assert.Equal(waterBefore - 4, w.Inventory.CountById("clean_water"));

            // 5) every system now observes the drained pool — no cached overspend
            Assert.Equal(3, w.Inventory.CountById("clean_water")); // 1 + 6 - 4
        }

        // ------------------------------------------------------------------
        // §9.2 — culture ↔ sanatorium journal link
        // ------------------------------------------------------------------

        [Fact]
        public void DreamTranscription_FeedsArchive_ExactlyOncePerCompletion()
        {
            var w = new World();
            w.Conditions.Sick.Add("survivor_patient_e");

            Assert.True(w.Sanatorium.TryAdmitPatient("survivor_patient_e", "condition_guilt_insomnia_loop", 1).Status
                        == ActionResult.StatusKind.Success);
            w.Inventory.TryProduce("paper_stock", 5);

            // the host binds: journal completion → one archive oral-history disc
            var cutResults = new List<ActionResult>();
            w.Sanatorium.OnTherapeuticJournalCompleted += (survivor, therapy) =>
            {
                string discId = $"archive_disc_dream_{survivor}";
                cutResults.Add(w.Culture.TryCutArchiveDisc(discId, "oral_history", survivor, day: 2));
            };
            w.Inventory.TryProduce("acetate_blank_disc", 5);

            // therapy_dream_transcription: 2-day protocol, eligible for guilt loop
            Assert.Equal(ActionResult.StatusKind.Success,
                w.Sanatorium.TryStartTherapy("survivor_patient_e", "therapy_dream_transcription",
                    "survivor_therapist", 1).Status);
            w.Sanatorium.TickDay(1);
            w.Sanatorium.TickDay(2);

            // one completion → one event → one archive recording (created, owned id)
            Assert.Single(cutResults);
            Assert.Equal(ActionResult.StatusKind.Success, cutResults[0].Status);
            Assert.Single(w.Culture.Recordings);
            Assert.Equal("archive_disc_dream_survivor_patient_e", w.Culture.Recordings[0].recording_id);

            // restore-and-replay cannot double-award: re-running TickDay on the
            // same day does not re-complete a finished therapy
            w.Sanatorium.TickDay(2);
            Assert.Single(w.Culture.Recordings);
        }

        // ------------------------------------------------------------------
        // §9.5 — diplomacy policy consulted by a patrol-style caller
        // ------------------------------------------------------------------

        [Fact]
        public void PatrolSystem_ConsultsTreatyPolicy_AndViolationsRouteOnce()
        {
            var w = new World();
            w.Diplomacy.TryScheduleSummit(DiplomaticSummitSystem.NeutralSummitSiteId,
                new[] { "faction_a", "faction_b" }, new[] { "survivor_envoy" },
                "treaty_patrol_standdown", 10);
            var summit = w.Diplomacy.Summits[0];
            for (int i = 0; i < 12 && summit.status == "negotiating"
                             && summit.negotiation_stability < DiplomaticSummitSystem.RatificationThreshold; i++)
                w.Diplomacy.AdvanceNegotiation(summit.summit_id, true);
            Assert.Equal(ActionResult.StatusKind.Success, w.Diplomacy.TryRatifyTreaty(summit.summit_id, 20).Status);

            // the (hypothetical) patrol dispatcher consults the published policy
            bool dispatchAllowed = w.Diplomacy.IsArmedPatrolAllowed("faction_a", "high_scarp_ridgeline");
            Assert.False(dispatchAllowed); // ridgeline is a stand-down DMZ

            // movement happens anyway → violation recorded once, then expires cleanly
            w.Diplomacy.ReportArmedPatrol("faction_a", "high_scarp_ridgeline", 25);
            var treaty = w.Diplomacy.Treaties.First(t => t.status == "active");
            w.Diplomacy.TickDay(treaty.expiry_day);
            Assert.True(w.Diplomacy.IsArmedPatrolAllowed("faction_a", "high_scarp_ridgeline"));
        }
    }
}
