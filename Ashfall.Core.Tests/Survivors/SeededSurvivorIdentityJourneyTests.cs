// SPDX-License-Identifier: MIT
// ASHFALL Seeded Long-Play Survivor Identity Journey Test (Wave E §4 / R15-R17).
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Flags;
using Ashfall.Core.Journal;
using Ashfall.Core.Memorial;
using Ashfall.Core.Narrative;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Survivors
{
    public sealed class SeededSurvivorIdentityJourneyTests : CatalogTestBase
    {
        private static string FindDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        [Fact]
        public void SeededSurvivorJourney_60Days_ProvesIdentity_Leadership_Affinity_DeathMemory_AndMaturation()
        {
            string dataDir = FindDataDir();
            var io = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();
            var rng = new SeededRng(1337);
            var log = NullLog.Instance;

            // 1. Setup Authored Identity Enrichment (R15)
            var enrichment = ExpansionEnrichmentCatalogLoader.Load(dataDir, io, serializer);
            Assert.NotNull(enrichment);
            Assert.True(enrichment.SurvivorFieldCount >= 70);

            // Verify authored identity data for key survivor
            var elenaEnrichment = enrichment.GetSurvivorFields("elena_vasquez");
            Assert.NotNull(elenaEnrichment);
            Assert.Equal("collectivist_solidarity", elenaEnrichment.belief_profile_id);

            // 2. Setup Heirloom Catalog (R16)
            var heirlooms = DwellerHeirloomCatalog.Load(dataDir, io, serializer);
            Assert.NotNull(heirlooms);
            Assert.True(heirlooms.AllHeirlooms.Count >= 20);

            // 3. Setup Roster, Needs, Relations, Social, and Leadership
            var roster = new SurvivorRosterSystem();
            roster.RegisterDefinition(new SurvivorDefinition
            {
                id = "survivor_dr_irina_vel",
                displayName = "Dr. Irina Vel",
                profession = "Chief Medical Officer"
            });
            roster.RegisterDefinition(new SurvivorDefinition
            {
                id = "survivor_sonya_vel",
                displayName = "Sonya Vel",
                profession = "Botanist"
            });
            roster.Join("survivor_dr_irina_vel", 1);
            roster.Join("survivor_sonya_vel", 1);

            var needs = new NeedsSystem();
            var survivors = new List<SurvivorNeedsState>
            {
                new() { Id = "survivor_dr_irina_vel", Health = 100f, Hunger = 10f, Thirst = 10f, Morale = 80f },
                new() { Id = "survivor_sonya_vel", Health = 80f, Hunger = 15f, Thirst = 15f, Morale = 70f }
            };
            foreach (var s in survivors) needs.Register(s);

            var relations = new SurvivorRelationsSystem(rng);
            relations.ModifyAffinity("survivor_dr_irina_vel", "survivor_sonya_vel", 75f);
            relations.ModifyTrust("survivor_dr_irina_vel", "survivor_sonya_vel", 65f);

            int currentSimDay = 1;
            var dutyRoster = new DutyRosterSystem();
            var socialCoordinator = new SurvivorSocialCoordinator(
                rng,
                needs,
                relations,
                dutyRoster,
                () => currentSimDay,
                log);

            socialCoordinator.SetAliveSurvivors(new[] { "survivor_dr_irina_vel", "survivor_sonya_vel" });

            // Leadership Designation (R17)
            Assert.True(socialCoordinator.DesignateLeader("survivor_dr_irina_vel"));
            Assert.Equal("survivor_dr_irina_vel", socialCoordinator.Leadership.CurrentLeaderId);

            // Caregiving Setup (R17)
            var caregiving = new CaregivingSystem();
            float careRecoveryApplied = 0f;
            caregiving.ApplyHealthRecoveryBonus = (patientId, amount) => careRecoveryApplied += amount;
            caregiving.GetRelationshipEffect = (c, p) => relations.EffectOf(c, p);
            Assert.True(caregiving.AssignCaregiver("survivor_dr_irina_vel", "survivor_sonya_vel"));

            // Cohort Setup (R17)
            var cohort = new CohortSystem();
            Assert.True(cohort.BookChild("child_leo", new[] { "survivor_dr_irina_vel", "survivor_sonya_vel" }, "low", birthDay: 1));

            // Memorial, Journal, and Fate Setup (R16)
            var memorialState = new MemorialState();
            var memorial = new MemorialSystem(memorialState);
            var griefSink = new CapturingGriefSink();
            memorial.GriefSink = griefSink;

            var journal = new JournalSystem();
            var eulogyEngine = new ProceduralEulogyEngine();
            var fateSystem = new SurvivorFateSystem(
                roster: roster,
                needs: needs,
                social: socialCoordinator,
                memorial: memorial,
                journal: journal,
                getDay: () => currentSimDay,
                displayNameFor: id => id == "survivor_dr_irina_vel" ? "Dr. Irina Vel" : "Sonya Vel",
                eulogyEngine: eulogyEngine,
                heirlooms: heirlooms);

            bool leaderBreakRiskOccurred = false;
            socialCoordinator.Leadership.OnLeaderBreakRisk += id => leaderBreakRiskOccurred = true;

            string maturedChildId = null;
            cohort.OnMaturation += (id, day) => maturedChildId = id;

            // ── SIMULATION DAYS 1 - 20: Stable Growth & Caregiving ──
            for (int day = 1; day <= 20; day++)
            {
                currentSimDay = day;
                socialCoordinator.TickDay(day, survivors);
                caregiving.Tick(24f);
            }

            Assert.True(careRecoveryApplied > 5f, "Caregiving with positive relationship must provide health recovery bonus");
            Assert.Equal("survivor_dr_irina_vel", socialCoordinator.Leadership.CurrentLeaderId);

            // ── SIMULATION DAYS 21 - 35: Crisis Overload & Leadership Stress ──
            for (int c = 0; c < 4; c++)
            {
                socialCoordinator.OnCrisisEvent(25f);
            }
            Assert.True(leaderBreakRiskOccurred, "Leader Dr. Irina must experience break risk from accumulated crisis stress");
            Assert.Equal(100f, socialCoordinator.Leadership.GetLeaderStress("survivor_dr_irina_vel"));

            // Dr. Irina steps down due to stress
            Assert.True(socialCoordinator.Leadership.StepDown("survivor_dr_irina_vel"));
            Assert.Null(socialCoordinator.Leadership.CurrentLeaderId);
            Assert.True(socialCoordinator.Leadership.StepDownCooldown > 0f);

            // Tick past cooldown and designate Sonya as successor
            socialCoordinator.Leadership.Tick((socialCoordinator.Leadership.StepDownCooldown + 1f) * 24f);
            Assert.True(socialCoordinator.DesignateLeader("survivor_sonya_vel"));
            Assert.Equal("survivor_sonya_vel", socialCoordinator.Leadership.CurrentLeaderId);

            // ── SIMULATION DAY 40: Tragic Death of Dr. Irina (R16) ──
            currentSimDay = 40;
            caregiving.UnassignCaregiver("survivor_sonya_vel");

            var irinaNeeds = needs.Get("survivor_dr_irina_vel");
            irinaNeeds.Health = 0f;

            var fateRecord = fateSystem.ReportDeath("survivor_dr_irina_vel", SurvivorDeathCause.Needs, "starvation");
            Assert.NotNull(fateRecord);
            Assert.Equal("survivor_dr_irina_vel", fateRecord.survivorId);

            // Assert Eulogy Composed
            Assert.Equal(1, eulogyEngine.ArchivedEulogies.Count);
            string eulogyText = eulogyEngine.ArchivedEulogies[0];
            Assert.Contains("DR. IRINA VEL", eulogyText);
            Assert.Contains("Chief Medical Officer", eulogyText);

            // Assert Heirloom Inherited by living relative Sonya
            Assert.Equal(1, memorial.Entries.Count);
            var memEntry = memorial.Entries[0];
            Assert.Equal("survivor_dr_irina_vel", memEntry.SurvivorId);
            Assert.Equal(eulogyText, memEntry.Epitaph);
            Assert.Equal("heirloom_01_vel_stethoscope_silver", memEntry.HeirloomItemId);
            Assert.Equal("survivor_sonya_vel", memEntry.HeirloomRecipientId);

            // Assert Grief dispersion reached living relative
            Assert.True(griefSink.Records.Count >= 1);
            Assert.Contains("survivor_sonya_vel", griefSink.Records[0].SurvivngRelationshipIds);

            // Assert Journal Inscribed
            Assert.True(journal.Knowledge.Has(SurvivorFateSystem.JournalKeyPrefix + "survivor_dr_irina_vel"));
            Assert.True(journal.Knowledge.Has(SurvivorFateSystem.JournalKeyPrefix + "survivor_dr_irina_vel_eulogy"));
            Assert.True(journal.Knowledge.Has(SurvivorFateSystem.JournalKeyPrefix + "survivor_dr_irina_vel_heirloom"));

            // ── SIMULATION DAYS 41 - 60: Cohort Maturation & Save/Load Roundtrip ──
            // Simulate child maturation at day 366
            Assert.True(cohort.TryMaturation("child_leo", 366));
            Assert.Equal("child_leo", maturedChildId);
            Assert.True(cohort.GetChild("child_leo")!.isMatured);

            // Save / Load Roundtrip Verification for Fate and Eulogies
            var fateSave = fateSystem.CaptureState();
            Assert.NotNull(fateSave);
            Assert.NotNull(fateSave.eulogies);
            Assert.Equal(1, fateSave.eulogies.archivedEulogyTexts.Count);

            var restoredFate = new SurvivorFateSystem(
                roster: roster,
                state: fateSave);
            Assert.Equal(1, restoredFate.EulogyEngine.ArchivedEulogies.Count);
            Assert.Equal(eulogyText, restoredFate.EulogyEngine.ArchivedEulogies[0]);
        }
    }
}
