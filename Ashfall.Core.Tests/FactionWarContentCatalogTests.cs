using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.YearOfAsh;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class FactionWarContentCatalogTests
    : CatalogTestBase{
        private static string FindDataDir() => DataDirectory;

        private static FactionWarContentCatalog LoadReal()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var loader = new FactionWarContentCatalogLoader(files, json);
            return loader.Load(FindDataDir());
        }

        [Fact]
        public void Loads_All_Five_Files()
        {
            var catalog = LoadReal();
            Assert.True(catalog.EventChainCount > 0, "event chains loaded");
            Assert.True(catalog.JournalEntryCount > 0, "journal entries loaded");
            Assert.True(catalog.BroadcastCount > 0, "broadcasts loaded");
            Assert.True(catalog.DialogueSnippetCount > 0, "dialogue snippets loaded");
            Assert.True(catalog.CommuniqueCount > 0, "communiques loaded");
            Assert.True(catalog.LocationOverrideCount > 0, "location overrides loaded");
        }

        [Fact]
        public void Event_Chains_Have_Required_Fields()
        {
            var catalog = LoadReal();
            foreach (var chain in catalog.EventChains)
            {
                Assert.False(string.IsNullOrEmpty(chain.chainId), "chainId present");
                Assert.False(string.IsNullOrEmpty(chain.title), "title present");
                Assert.NotNull(chain.stages);
                Assert.NotEmpty(chain.stages);
            }
        }

        /// <summary>
        /// Regression guard: FactionWarEventChoice previously mapped to a
        /// wrong field shape (label/outcomeText/setWorldFlag/
        /// factionStandingDelta) that didn't exist in the real JSON, which
        /// System.Text.Json silently no-ops on — every one of the 93 choices
        /// was loading with an empty label and a zeroed, unusable delta/
        /// destination. This pins the REAL shape (text/moraleDelta/
        /// leadsToStageId) and asserts it isn't just present but non-default,
        /// for a specific known choice.
        /// </summary>
        [Fact]
        public void EventChoices_DeserializeRealFieldShape_NotTheStaleOne()
        {
            var catalog = LoadReal();
            var chain = catalog.EventChains.FirstOrDefault(c => c.chainId == "evt_d480_grain_tally_dispute");
            Assert.NotNull(chain);

            var stage = chain!.stages.FirstOrDefault(s => s.stageId == "evt_d480_grain_tally_dispute_s1");
            Assert.NotNull(stage);
            Assert.Equal("A Thumb on the Scale", stage!.title);

            var choice = stage.choices.FirstOrDefault(c => c.choiceId == "evt_d480_grain_tally_dispute_s1_c1");
            Assert.NotNull(choice);
            Assert.Equal("Back the weigher publicly — the scale is the scale", choice!.text);
            Assert.Equal(2, choice.moraleDelta);
            Assert.Equal("evt_d480_grain_tally_dispute_s2", choice.leadsToStageId);
        }

        /// <summary>Every stage's every choice must have real, non-empty text and a
        /// resolvable leadsToStageId (either empty/terminal, or matching a real
        /// stageId somewhere in the same chain's stages).</summary>
        [Fact]
        public void EventChoices_AllHaveNonEmptyTextAndResolvableDestination()
        {
            var catalog = LoadReal();
            foreach (var chain in catalog.EventChains)
            {
                var stageIds = new HashSet<string>();
                foreach (var s in chain.stages) stageIds.Add(s.stageId);

                foreach (var stage in chain.stages)
                {
                    foreach (var choice in stage.choices)
                    {
                        Assert.False(string.IsNullOrEmpty(choice.text),
                            $"{chain.chainId}/{stage.stageId}/{choice.choiceId} has empty text");
                        if (!string.IsNullOrEmpty(choice.leadsToStageId))
                        {
                            Assert.Contains(choice.leadsToStageId, stageIds);
                        }
                    }
                }
            }
        }

        [Fact]
        public void Journal_Entries_Have_Required_Fields()
        {
            var catalog = LoadReal();
            foreach (var entry in catalog.JournalEntries)
            {
                Assert.False(string.IsNullOrEmpty(entry.id), "id present");
                Assert.False(string.IsNullOrEmpty(entry.body), "body present");
                Assert.True(entry.day > 0, $"day positive for {entry.id}");
            }
        }

        [Fact]
        public void Broadcasts_Have_Required_Fields()
        {
            var catalog = LoadReal();
            foreach (var b in catalog.Broadcasts)
            {
                Assert.False(string.IsNullOrEmpty(b.id), "id present");
                Assert.False(string.IsNullOrEmpty(b.message), "message present");
                Assert.True(b.dayTrigger > 0, $"dayTrigger positive for {b.id}");
            }
        }

        [Fact]
        public void GetEligibleChains_Filters_By_Day()
        {
            var catalog = LoadReal();
            var early = catalog.GetEligibleChains(1);
            var late = catalog.GetEligibleChains(500);
            Assert.True(late.Count >= early.Count,
                $"day 500 should have >= chains than day 1 ({late.Count} vs {early.Count})");
        }

        [Fact]
        public void GetJournalForDay_Returns_Day_Specific_Entries()
        {
            var catalog = LoadReal();
            // Day 482 has at least one entry (journal_d482_mira_queue_count)
            var day482 = catalog.GetJournalForDay(482);
            Assert.NotEmpty(day482);
            Assert.All(day482, e => Assert.Equal(482, e.day));
        }

        [Fact]
        public void GetBroadcastsForDay_Filters_By_Trigger()
        {
            var catalog = LoadReal();
            var day0 = catalog.GetBroadcastsForDay(0);
            Assert.Empty(day0);
            var day500 = catalog.GetBroadcastsForDay(500);
            Assert.NotEmpty(day500);
        }

        [Fact]
        public void GetDialogueForLocation_Filters_By_Location_And_Day()
        {
            var catalog = LoadReal();
            // Find a known location from the data
            var all = catalog.DialogueSnippets;
            Assert.NotEmpty(all);
            string knownLoc = all[0].locationId;
            int knownDay = all[0].minDay;

            var result = catalog.GetDialogueForLocation(knownLoc, knownDay);
            Assert.NotEmpty(result);

            var tooEarly = catalog.GetDialogueForLocation(knownLoc, knownDay - 1);
            Assert.Empty(tooEarly);
        }

        [Fact]
        public void GetCommuniquesForFaction_Filters_By_Faction()
        {
            var catalog = LoadReal();
            var garrison = catalog.GetCommuniquesForFaction("faction_central_garrison", 999);
            Assert.NotEmpty(garrison);
            Assert.All(garrison, c => Assert.Equal("faction_central_garrison", c.factionId));

            var empty = catalog.GetCommuniquesForFaction("faction_nonexistent", 999);
            Assert.Empty(empty);
        }

        [Fact]
        public void LocationOverrides_HaveRequiredFieldsAndConsistentDayWindowRule()
        {
            var catalog = LoadReal();
            Assert.NotEmpty(catalog.LocationOverrides);
            foreach (var o in catalog.LocationOverrides)
            {
                Assert.False(string.IsNullOrEmpty(o.id), "override id present");
                Assert.False(string.IsNullOrEmpty(o.locationId), "override locationId present");
                Assert.Contains(o.overrideType, new[] { "pre_strike", "post_strike", "ambient_addendum" });
                Assert.False(string.IsNullOrEmpty(o.description), "override description present");

                // Documented rule (NARRATIVE_NEEDS.md §3): activeUntilDay is
                // bounded only for pre_strike; post_strike/ambient_addendum
                // are open-ended (activeUntilDay left at its 0 default).
                if (o.overrideType == "pre_strike")
                    Assert.True(o.activeUntilDay > 0, $"{o.id} (pre_strike) must have a bounded activeUntilDay");
                else
                    Assert.Equal(0, o.activeUntilDay);
            }
        }

        [Fact]
        public void GetActiveLocationOverride_ResolvesPreStrikeThenPostStrikeAcrossTime()
        {
            var catalog = LoadReal();

            // St Brigid's Almshouse: pre_strike active 515-516, post_strike from 517 onward.
            Assert.Null(catalog.GetActiveLocationOverride("loc_st_brigids_almshouse", 514));

            var pre = catalog.GetActiveLocationOverride("loc_st_brigids_almshouse", 515);
            Assert.NotNull(pre);
            Assert.Equal("pre_strike", pre!.overrideType);

            var post = catalog.GetActiveLocationOverride("loc_st_brigids_almshouse", 517);
            Assert.NotNull(post);
            Assert.Equal("post_strike", post!.overrideType);

            // Open-ended: still active far in the future.
            var farFuture = catalog.GetActiveLocationOverride("loc_st_brigids_almshouse", 5000);
            Assert.NotNull(farFuture);
            Assert.Equal("post_strike", farFuture!.overrideType);
        }

        [Fact]
        public void GetActiveLocationOverride_UnknownLocation_ReturnsNull()
        {
            var catalog = LoadReal();
            Assert.Null(catalog.GetActiveLocationOverride("loc_this_does_not_exist", 600));
        }

        [Fact]
        public void Loads_From_Missing_Directory_Without_Crash()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var loader = new FactionWarContentCatalogLoader(files, json);
            var catalog = loader.Load("nonexistent/path");
            Assert.Equal(0, catalog.EventChainCount);
            Assert.Equal(0, catalog.JournalEntryCount);
        }

        [Fact]
        public void All_Faction_References_Resolve()
        {
            var catalog = LoadReal();
            var knownFactions = new[]
            {
                "faction_central_garrison", "faction_rebuilders", "faction_black_ops",
                "faction_ash_sign", "faction_forward_roster", "warlords_sector_4",
                "faction_hydro_barons", "faction_railway_guild", "faction_supply_corps"
            };

            foreach (var chain in catalog.EventChains)
            {
                foreach (var fid in chain.factionsInvolved)
                {
                    Assert.Contains(fid, knownFactions);
                }
            }

            foreach (var c in catalog.Communiques)
            {
                Assert.Contains(c.factionId, knownFactions);
            }
        }
    }
}
