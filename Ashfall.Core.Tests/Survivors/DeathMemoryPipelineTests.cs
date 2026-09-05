using System;
using System.IO;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Flags;
using Ashfall.Core.Journal;
using Ashfall.Core.Memorial;
using Ashfall.Core.Narrative;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Survivors
{
    public sealed class DeathMemoryPipelineTests : CatalogTestBase
    {
        private static string FindDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        [Fact]
        public void SurvivorDeath_TriggersEulogy_HeirloomInheritance_AndMemorialEpitaph()
        {
            string dataDir = FindDataDir();
            var io = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();

            // Setup real catalogs
            var heirlooms = DwellerHeirloomCatalog.Load(dataDir, io, serializer);
            var eulogyEngine = new ProceduralEulogyEngine();

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
            needs.Register(new SurvivorNeedsState { Id = "survivor_dr_irina_vel", Health = 0f, Morale = 50f });
            needs.Register(new SurvivorNeedsState { Id = "survivor_sonya_vel", Health = 100f, Morale = 60f });

            var relations = new SurvivorRelationsSystem(new SeededRng(12345));
            relations.ModifyAffinity("survivor_dr_irina_vel", "survivor_sonya_vel", 80f);

            var social = new SurvivorSocialCoordinator(
                new SeededRng(12345),
                needs,
                relations,
                new DutyRosterSystem(),
                () => 10);
            social.SetAliveSurvivors(new[] { "survivor_dr_irina_vel", "survivor_sonya_vel" });

            var memorialState = new MemorialState();
            var memorial = new MemorialSystem(memorialState);
            var griefSink = new CapturingGriefSink();
            memorial.GriefSink = griefSink;

            var journal = new JournalSystem();

            var fate = new SurvivorFateSystem(
                roster: roster,
                needs: needs,
                social: social,
                memorial: memorial,
                journal: journal,
                getDay: () => 10,
                displayNameFor: id => id == "survivor_dr_irina_vel" ? "Dr. Irina Vel" : "Sonya Vel",
                eulogyEngine: eulogyEngine,
                heirlooms: heirlooms);

            // Report Irina's death
            var fateRecord = fate.ReportDeath("survivor_dr_irina_vel", SurvivorDeathCause.Needs, "starvation");

            // 1. Verify eulogy was composed and archived
            Assert.Equal(1, eulogyEngine.ArchivedEulogies.Count);
            string eulogyText = eulogyEngine.ArchivedEulogies[0];
            Assert.Contains("DR. IRINA VEL", eulogyText);
            Assert.Contains("Chief Medical Officer", eulogyText);
            Assert.Contains("Father's Silver Acoustic Stethoscope", eulogyText);

            // 2. Verify memorial has epitaph and heirloom inheritance
            Assert.Equal(1, memorial.Entries.Count);
            var memEntry = memorial.Entries[0];
            Assert.Equal("survivor_dr_irina_vel", memEntry.SurvivorId);
            Assert.Equal(eulogyText, memEntry.Epitaph);
            Assert.Equal("heirloom_01_vel_stethoscope_silver", memEntry.HeirloomItemId);
            Assert.Equal("survivor_sonya_vel", memEntry.HeirloomRecipientId);

            // 3. Verify grief dispersion reached living relative
            Assert.True(griefSink.Records.Count >= 1);
            Assert.Contains("survivor_sonya_vel", griefSink.Records[0].SurvivngRelationshipIds);

            // 4. Verify living survivor took shelter-wide grief
            var livingNeeds = needs.Get("survivor_sonya_vel");
            Assert.Equal(52f, livingNeeds.Morale); // 60 - 8 = 52

            // 5. Verify journal captured eulogy and heirloom inheritance entries
            Assert.True(journal.Knowledge.Has(SurvivorFateSystem.JournalKeyPrefix + "survivor_dr_irina_vel"));
            Assert.True(journal.Knowledge.Has(SurvivorFateSystem.JournalKeyPrefix + "survivor_dr_irina_vel_eulogy"));
            Assert.True(journal.Knowledge.Has(SurvivorFateSystem.JournalKeyPrefix + "survivor_dr_irina_vel_heirloom"));

            // 6. Idempotency: reporting again produces no duplicate eulogy or memorial
            fate.ReportDeath("survivor_dr_irina_vel", SurvivorDeathCause.Needs, "starvation");
            Assert.Equal(1, eulogyEngine.ArchivedEulogies.Count);
            Assert.Equal(1, memorial.Entries.Count);

            // 7. Save/restore roundtrip preserves eulogies
            var saveState = fate.CaptureState();
            Assert.Equal(1, saveState.eulogies.archivedEulogyTexts.Count);

            var restoredFate = new SurvivorFateSystem(
                roster: roster,
                state: saveState);
            Assert.Equal(1, restoredFate.EulogyEngine.ArchivedEulogies.Count);
            Assert.Equal(eulogyText, restoredFate.EulogyEngine.ArchivedEulogies[0]);
        }
    }
}
