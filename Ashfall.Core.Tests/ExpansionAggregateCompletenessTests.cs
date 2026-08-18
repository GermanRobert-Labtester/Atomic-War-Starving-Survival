using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Maritime;
using Ashfall.Core.Verdict;
using Ashfall.Core.Warlords;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Pins the canonical expansion contract for the <c>--expansions-selftest</c>
    /// aggregate. If a canonical expansion (01–10) is missing from
    /// <see cref="ExpansionSuite.Canonical"/>, or a canonical expansion's Core
    /// authority no longer passes, this regression test fails — matching the
    /// runtime completeness check in the host aggregate.
    /// </summary>
    public class ExpansionAggregateCompletenessTests
    {
        private static string DataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found))
                return found;
            if (CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out found))
                return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        [Fact]
        public void CanonicalRegistry_ContainsExactlyTenUniqueExpansions()
        {
            var defs = ExpansionSuite.Canonical;
            Assert.Equal(10, defs.Length);
            Assert.Equal(ExpansionSuite.CanonicalCount, defs.Length);

            var ids = new HashSet<string>(StringComparer.Ordinal);
            var names = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < defs.Length; i++)
            {
                Assert.True(ids.Add(defs[i].Id), "duplicate expansion id " + defs[i].Id);
                Assert.True(names.Add(defs[i].Name), "duplicate expansion name " + defs[i].Name);
                Assert.StartsWith("expansion_", defs[i].Id);
                Assert.True(defs[i].Number >= 1 && defs[i].Number <= 10,
                    "expansion number out of 01-10 range: " + defs[i].Number);
            }

            string[] expected = {
                "expansion_01_holdfast", "expansion_02_duty_roster",
                "expansion_03_standing_record", "expansion_04_nobodys_charter",
                "expansion_05_year_of_ash", "expansion_06_muster",
                "expansion_07_the_dose", "expansion_08_the_verdict",
                "expansion_09_black_flotilla", "expansion_10_silent_foundry"
            };
            foreach (string id in expected)
                Assert.True(ExpansionSuite.IsCanonical(id), "missing canonical expansion id " + id);
        }

        [Fact]
        public void CanonicalExpansions_WithCoreDemo_RunGreen()
        {
            string dataDir = DataDir();
            Assert.True(HoldfastHeadlessDemo.Run(dataDir).Passed, "Exp 01 — The Holdfast");
            Assert.True(DutyRosterHeadlessDemo.Run(dataDir).Passed, "Exp 02 — The Duty Roster");
            Assert.True(StandingRecordHeadlessDemo.Run(dataDir).Passed, "Exp 03 — The Standing Record");
            Assert.True(CrossingHeadlessDemo.Run(dataDir).Passed, "Exp 04 — Nobody's Charter");
            Assert.True(WarlordHeadlessDemo.Run(dataDir).Passed, "Exp 05 — The Year of Ash (warlord authority)");
            Assert.True(Ashfall.Core.Muster.MusterHeadlessDemo.Run().Passed, "Exp 06 — The Muster");
            Assert.True(SilentFoundryHeadlessDemo.Run(dataDir).Passed, "Exp 10 — The Silent Foundry");
        }

        [Fact]
        public void HostOnlyExpansions_DataAuthorityIsReachable()
        {
            string dataDir = DataDir();
            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            // Exp 07 — The Dose / The Vigil.
            var dose = Ashfall.Core.DoseContentCatalogLoader.Load(dataDir, io, json);
            Assert.NotNull(dose);
            Assert.True(dose.quests.Count > 0, "dose content catalog must load register quest lines");

            // Exp 08 — The Verdict.
            var verdictItems = VerdictCatalogLoader.LoadItems(dataDir, io, json);
            Assert.True(verdictItems.Count >= 12, "verdict item catalog must load the evidence items");
            var verdictQuests = new Ashfall.Core.YearOfAsh.QuestlineSystem();
            int verdictQuestCount = VerdictQuestCatalogLoader.LoadAndRegister(verdictQuests, dataDir, io, json);
            Assert.True(verdictQuestCount > 0, "verdict questline catalog must remain reachable");

            // Exp 09 — The Black Flotilla / Maritime.
            var locations = DeepLoreLocationCatalogLoader.Load(dataDir, io, json);
            Assert.True(locations.Count >= 10, "deep-lore location catalog must load");
            var diveSites = DiveSiteCatalogLoader.Load(dataDir, io, json);
            Assert.True(diveSites.dive_sites.Count >= 4, "dive-site catalog must define 4+ sites");
        }
    }
}
