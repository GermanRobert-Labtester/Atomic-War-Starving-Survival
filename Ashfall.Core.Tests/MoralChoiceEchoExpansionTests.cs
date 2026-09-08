using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.MoralChoice;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Plan 109: Comprehensive test suite for the expanded 60-echo quest catalog
    /// in moral_choice_chains.json (32 baseline + 28 new delayed callback quests).
    /// Covers schema parsing, ID uniqueness, baseline preservation, branch distribution,
    /// trigger reference integrity, choice index bounds, temporal reachability,
    /// branch consistency, and runtime eligibility / arbitration / lockout behavior.
    /// </summary>
    public sealed class MoralChoiceEchoExpansionTests : CatalogTestBase
    {
        private static readonly IFileIO s_files = new FileSystemIO();
        private static readonly IJsonSerializer s_json = new SystemTextJsonSerializer();

        private static readonly string[] s_baselineEchoIds = new[]
        {
            "quest_moral_echo_child_returns",
            "quest_moral_echo_child_steals",
            "quest_moral_echo_family_defends",
            "quest_moral_echo_family_ambush",
            "quest_moral_echo_farmer_harvest",
            "quest_moral_echo_farmer_dead",
            "quest_moral_echo_raider_warning",
            "quest_moral_echo_raider_ambush",
            "quest_moral_echo_peacekeeper_intel",
            "quest_moral_echo_peacekeeper_hunted",
            "quest_moral_echo_widow_gift",
            "quest_moral_echo_prophet_map",
            "quest_moral_echo_prophet_curse",
            "quest_moral_echo_soldier_teaches",
            "quest_moral_echo_soldier_hostile",
            "quest_moral_echo_messenger_packet",
            "quest_moral_echo_messenger_stolen",
            "quest_moral_echo_dead_child_haunt",
            "quest_moral_echo_dead_child_peace",
            "quest_moral_echo_scientist_formula",
            "quest_moral_echo_mercy_recognized",
            "quest_moral_echo_iron_feared",
            "quest_moral_echo_listener_confided",
            "quest_moral_echo_betrayer_hunted",
            "quest_moral_echo_mercy_tested",
            "quest_moral_echo_iron_challenged",
            "quest_moral_echo_listener_secret",
            "quest_moral_echo_betrayer_cornered",
            "quest_moral_echo_mercy_final",
            "quest_moral_echo_iron_final",
            "quest_moral_echo_listener_final",
            "quest_moral_echo_betrayer_final"
        };

        private static readonly string[] s_newEchoIds = new[]
        {
            // Mercy Road (8)
            "quest_moral_echo_raider_repaid_warning",
            "quest_moral_echo_betrayers_child_grown",
            "quest_moral_echo_medicine_shared_recovered",
            "quest_moral_echo_convoy_haven_opened",
            "quest_moral_echo_plague_secret_infection",
            "quest_moral_echo_well_gratitude_refused",
            "quest_moral_echo_patrol_reputation_spread",
            "quest_moral_echo_shelter_vote_strained_rations",
            // Iron Way (8)
            "quest_moral_echo_aldric_blockade_retaliation",
            "quest_moral_echo_old_friend_farewell_note",
            "quest_moral_echo_expulsion_deterrence_held",
            "quest_moral_echo_strike_broken_fear_quota",
            "quest_moral_echo_informant_applies_leverage",
            "quest_moral_echo_lowfield_harvest_dividend",
            "quest_moral_echo_varek_blood_debt_claim",
            "quest_moral_echo_calla_camp_empty_ruin",
            // Listener Thread (7)
            "quest_moral_echo_defector_corroborates_truth",
            "quest_moral_echo_cartographer_water_cache_located",
            "quest_moral_echo_prophet_calendar_discrepancy",
            "quest_moral_echo_trader_ledger_censorship_threat",
            "quest_moral_echo_soldier_second_confession",
            "quest_moral_echo_doctors_notes_reinterpreted",
            "quest_moral_echo_librarian_memorial_preserved",
            // Broken Compact (5)
            "quest_moral_echo_kessler_exile_uncovered",
            "quest_moral_echo_poisoned_gift_reputation_drop",
            "quest_moral_echo_crisis_gambit_warlord_respect",
            "quest_moral_echo_voss_blackmail_exposed",
            "quest_moral_echo_pell_hostage_border_locked"
        };

        private static MoralChoiceChainData LoadChains() =>
            MoralChoiceChainCatalogLoader.Load(DataDirectory, s_files, s_json);

        private static Dictionary<string, MoralChoiceQuestDefinition> LoadAllQuests()
        {
            var dict = new Dictionary<string, MoralChoiceQuestDefinition>(StringComparer.Ordinal);
            var baseQuests = MoralChoiceCatalogLoader.Load(DataDirectory, s_files, s_json);
            foreach (var q in baseQuests) dict[q.Id] = q;
            var branchQuests = MoralChoiceBranchQuestCatalogLoader.Load(DataDirectory, s_files, s_json);
            foreach (var q in branchQuests) dict[q.Id] = q;
            var expQuests = MoralChoiceExpansionQuestCatalogLoader.Load(DataDirectory, s_files, s_json);
            foreach (var q in expQuests) dict[q.Id] = q;
            return dict;
        }

        // ── 1. Catalog Count & Parse Tests ──────────────────────────────

        [Fact]
        public void Catalog_LoadsExactlySixtyEchoQuests()
        {
            var data = LoadChains();
            Assert.Equal(60, data.EchoQuests.Count);
        }

        [Fact]
        public void Catalog_EchoQuestIds_AreUniqueAndCanonical()
        {
            var data = LoadChains();
            var seen = new HashSet<string>(StringComparer.Ordinal);
            foreach (var echo in data.EchoQuests)
            {
                Assert.False(string.IsNullOrWhiteSpace(echo.QuestId));
                Assert.StartsWith("quest_moral_echo_", echo.QuestId);
                Assert.True(seen.Add(echo.QuestId), $"Duplicate echo quest ID: {echo.QuestId}");
            }
            Assert.Equal(60, seen.Count);
        }

        // ── 2. Baseline Preservation Tests ──────────────────────────────

        [Fact]
        public void Catalog_PreservesAllThirtyTwoBaselineEchoes()
        {
            var data = LoadChains();
            var byId = data.EchoQuests.ToDictionary(e => e.QuestId, StringComparer.Ordinal);

            foreach (var baseId in s_baselineEchoIds)
            {
                Assert.True(byId.ContainsKey(baseId), $"Baseline echo missing: {baseId}");
            }
            Assert.Equal(32, s_baselineEchoIds.Length);
        }

        // ── 3. Branch Distribution Tests ────────────────────────────────

        [Fact]
        public void Catalog_TwentyEightNewEchoes_MatchRequiredBranchDistribution()
        {
            var data = LoadChains();
            var byId = data.EchoQuests.ToDictionary(e => e.QuestId, StringComparer.Ordinal);

            Assert.Equal(28, s_newEchoIds.Length);
            foreach (var newId in s_newEchoIds)
            {
                Assert.True(byId.ContainsKey(newId), $"New echo missing: {newId}");
            }

            var newEchoes = s_newEchoIds.Select(id => byId[id]).ToList();

            int mercyCount = newEchoes.Count(e => e.Branch == "branch_mercy_road");
            int ironCount = newEchoes.Count(e => e.Branch == "branch_iron_way");
            int listenerCount = newEchoes.Count(e => e.Branch == "branch_listener_thread");
            int brokenCount = newEchoes.Count(e => e.Branch == "branch_broken_compact");

            Assert.Equal(8, mercyCount);
            Assert.Equal(8, ironCount);
            Assert.Equal(7, listenerCount);
            Assert.Equal(5, brokenCount);

            // Total catalog counts (including 3 baseline per branch + 20 branch-agnostic)
            int totalMercy = data.EchoQuests.Count(e => e.Branch == "branch_mercy_road");
            int totalIron = data.EchoQuests.Count(e => e.Branch == "branch_iron_way");
            int totalListener = data.EchoQuests.Count(e => e.Branch == "branch_listener_thread");
            int totalBroken = data.EchoQuests.Count(e => e.Branch == "branch_broken_compact");
            int totalAgnostic = data.EchoQuests.Count(e => string.IsNullOrEmpty(e.Branch));

            Assert.Equal(11, totalMercy);
            Assert.Equal(11, totalIron);
            Assert.Equal(10, totalListener);
            Assert.Equal(8, totalBroken);
            Assert.Equal(20, totalAgnostic);
        }

        // ── 4. Trigger Reference Integrity Tests ────────────────────────

        [Fact]
        public void ReferenceIntegrity_AllTriggeredByResolveInQuestCatalogs()
        {
            var data = LoadChains();
            var allQuests = LoadAllQuests();

            foreach (var echo in data.EchoQuests)
            {
                Assert.False(string.IsNullOrWhiteSpace(echo.TriggeredBy),
                    $"{echo.QuestId} has empty TriggeredBy");
                Assert.True(allQuests.ContainsKey(echo.TriggeredBy),
                    $"{echo.QuestId} TriggeredBy '{echo.TriggeredBy}' not found in any moral quest catalog");
            }
        }

        [Fact]
        public void ReferenceIntegrity_AllTwentyEightNewTriggeredByResolveInQuestGates()
        {
            var data = LoadChains();
            var gateIds = new HashSet<string>(data.QuestGates.Select(g => g.QuestId), StringComparer.Ordinal);
            var byId = data.EchoQuests.ToDictionary(e => e.QuestId, StringComparer.Ordinal);

            var triggersSeen = new HashSet<string>(StringComparer.Ordinal);
            foreach (var newId in s_newEchoIds)
            {
                var echo = byId[newId];
                Assert.True(gateIds.Contains(echo.TriggeredBy),
                    $"New echo {echo.QuestId} TriggeredBy '{echo.TriggeredBy}' not in quest_gates");
                Assert.True(triggersSeen.Add(echo.TriggeredBy),
                    $"Duplicate source quest across new echoes: {echo.TriggeredBy}");
            }
            Assert.Equal(28, triggersSeen.Count);
        }

        // ── 5. Choice Index & Bounds Integrity Tests ────────────────────

        [Fact]
        public void ChoiceIndexIntegrity_AllChoicesAreValidForSourceQuests()
        {
            var data = LoadChains();
            var allQuests = LoadAllQuests();

            foreach (var echo in data.EchoQuests)
            {
                Assert.True(allQuests.TryGetValue(echo.TriggeredBy, out var sourceQuest),
                    $"Source quest '{echo.TriggeredBy}' not found for {echo.QuestId}");

                Assert.InRange(echo.TriggeredByChoice, 0, sourceQuest!.Choices.Count - 1);
            }
        }

        // ── 6. Temporal Reachability & Branch Consistency ───────────────

        [Fact]
        public void TemporalReachability_AllEchoesReachableWithinCampaignHorizon()
        {
            var data = LoadChains();
            var allQuests = LoadAllQuests();

            foreach (var echo in data.EchoQuests)
            {
                Assert.True(echo.MinDaysAfter > 0, $"{echo.QuestId} MinDaysAfter must be positive");
                Assert.InRange(echo.MinDaysAfter, 5, 80);

                if (allQuests.TryGetValue(echo.TriggeredBy, out var source))
                {
                    int earliestFirableDay = source.MinDay + echo.MinDaysAfter;
                    Assert.True(earliestFirableDay <= 360,
                        $"{echo.QuestId} earliest day {earliestFirableDay} exceeds 360-day campaign horizon");
                }
            }
        }

        [Fact]
        public void BranchConsistency_EchoBranchMatchesGatedQuestBranch()
        {
            var data = LoadChains();
            var gateBranches = data.QuestGates.ToDictionary(g => g.QuestId, g => g.Branch, StringComparer.Ordinal);
            var byId = data.EchoQuests.ToDictionary(e => e.QuestId, StringComparer.Ordinal);

            foreach (var newId in s_newEchoIds)
            {
                var echo = byId[newId];
                Assert.True(gateBranches.TryGetValue(echo.TriggeredBy, out var expectedBranch),
                    $"Source {echo.TriggeredBy} has no gate branch");
                Assert.Equal(expectedBranch, echo.Branch);
            }
        }

        // ── 7. Runtime Simulation & Gating Tests ────────────────────────

        [Fact]
        public void Runtime_MercyEcho_FiresOnlyAfterDelayAndMatchingChoice()
        {
            var sys = new MoralChoiceSystem(new SeededRng(42));
            var chainData = LoadChains();
            sys.InitializeChainData(chainData);

            // Source: quest_moral_chain_mercy_16 ("The Prodigal Raider"), min_day 165, choice 0, delay 25
            var quest = new MoralChoiceQuestDefinition
            {
                Id = "quest_moral_chain_mercy_16",
                DisplayName = "The Prodigal Raider",
                Category = "comfort",
                Choices = new List<MoralChoiceOption>
                {
                    new MoralChoiceOption { Label = "Accept him", MoralDelta = 12, EmpathyDelta = 2, Epitaph = "Accepted" },
                    new MoralChoiceOption { Label = "House outside", MoralDelta = 6, EmpathyDelta = 0, Epitaph = "Outside" },
                    new MoralChoiceOption { Label = "Send away", MoralDelta = 0, EmpathyDelta = 0, Epitaph = "Away" },
                    new MoralChoiceOption { Label = "Refuse", MoralDelta = -4, EmpathyDelta = 0, Epitaph = "Refused" }
                }
            };

            sys.Resolve(quest, 0, "loc_shelter_gate", 165);

            // Day 189: 165 + 24 < 165 + 25 -> Not available
            var preDelay = sys.FindAvailableEchoQuests(189);
            Assert.DoesNotContain(preDelay, e => e.QuestId == "quest_moral_echo_raider_repaid_warning");

            // Day 190: 165 + 25 -> Available
            var exactDay = sys.FindAvailableEchoQuests(190);
            Assert.Contains(exactDay, e => e.QuestId == "quest_moral_echo_raider_repaid_warning");

            // Wrong choice rejection
            var sys2 = new MoralChoiceSystem(new SeededRng(43));
            sys2.InitializeChainData(chainData);
            sys2.Resolve(quest, 3, "loc_shelter_gate", 165);
            var wrongChoice = sys2.FindAvailableEchoQuests(200);
            Assert.DoesNotContain(wrongChoice, e => e.QuestId == "quest_moral_echo_raider_repaid_warning");
        }

        [Fact]
        public void Runtime_IronEcho_FiresOnlyAfterDelayAndMatchingChoice()
        {
            var sys = new MoralChoiceSystem(new SeededRng(42));
            var chainData = LoadChains();
            sys.InitializeChainData(chainData);

            // Source: quest_moral_chain_iron_06 ("The Merchant's Proposition"), min_day 12, choice 0, delay 45
            var quest = new MoralChoiceQuestDefinition
            {
                Id = "quest_moral_chain_iron_06",
                DisplayName = "The Merchant's Proposition",
                Category = "trust",
                Choices = new List<MoralChoiceOption>
                {
                    new MoralChoiceOption { Label = "Accept", MoralDelta = -10, EmpathyDelta = 0, Epitaph = "Accepted" },
                    new MoralChoiceOption { Label = "Counter", MoralDelta = 2, EmpathyDelta = 1, Epitaph = "Countered" },
                    new MoralChoiceOption { Label = "Refuse", MoralDelta = 6, EmpathyDelta = 1, Epitaph = "Refused" }
                }
            };

            sys.Resolve(quest, 0, "loc_aldric", 12);

            // Day 56: 12 + 44 < 12 + 45 -> Not available
            Assert.DoesNotContain(sys.FindAvailableEchoQuests(56), e => e.QuestId == "quest_moral_echo_aldric_blockade_retaliation");

            // Day 57: 12 + 45 -> Available
            Assert.Contains(sys.FindAvailableEchoQuests(57), e => e.QuestId == "quest_moral_echo_aldric_blockade_retaliation");
        }

        [Fact]
        public void Runtime_ListenerEcho_FiresOnlyAfterDelayAndMatchingChoice()
        {
            var sys = new MoralChoiceSystem(new SeededRng(42));
            var chainData = LoadChains();
            sys.InitializeChainData(chainData);

            // Source: quest_moral_chain_listen_11 ("The Defector's Confession"), min_day 22, choice 0, delay 35
            var quest = new MoralChoiceQuestDefinition
            {
                Id = "quest_moral_chain_listen_11",
                DisplayName = "The Defector's Confession",
                Category = "listen",
                Choices = new List<MoralChoiceOption>
                {
                    new MoralChoiceOption { Label = "Listen full", MoralDelta = 14, EmpathyDelta = 4, Epitaph = "Listened" },
                    new MoralChoiceOption { Label = "Advocate", MoralDelta = 8, EmpathyDelta = 2, Epitaph = "Advocated" },
                    new MoralChoiceOption { Label = "No backstory", MoralDelta = -2, EmpathyDelta = 0, Epitaph = "Info" },
                    new MoralChoiceOption { Label = "Distrust", MoralDelta = -5, EmpathyDelta = 0, Epitaph = "Distrusted" }
                }
            };

            sys.Resolve(quest, 0, "loc_relay", 22);

            // Day 56 < 57
            Assert.DoesNotContain(sys.FindAvailableEchoQuests(56), e => e.QuestId == "quest_moral_echo_defector_corroborates_truth");

            // Day 57 >= 57
            Assert.Contains(sys.FindAvailableEchoQuests(57), e => e.QuestId == "quest_moral_echo_defector_corroborates_truth");
        }

        [Fact]
        public void Runtime_BrokenCompactEcho_FiresOnlyAfterDelayAndMatchingChoice()
        {
            var sys = new MoralChoiceSystem(new SeededRng(42));
            var chainData = LoadChains();
            sys.InitializeChainData(chainData);

            // Source: quest_moral_chain_betray_04 ("The Framed Hand"), min_day 35, choice 1, delay 35
            var quest = new MoralChoiceQuestDefinition
            {
                Id = "quest_moral_chain_betray_04",
                DisplayName = "The Framed Hand",
                Category = "trust",
                Choices = new List<MoralChoiceOption>
                {
                    new MoralChoiceOption { Label = "Clear record", MoralDelta = 5, EmpathyDelta = 1, Epitaph = "Cleared" },
                    new MoralChoiceOption { Label = "Plant tools", MoralDelta = -15, EmpathyDelta = 0, Epitaph = "Planted" },
                    new MoralChoiceOption { Label = "Report anon", MoralDelta = -8, EmpathyDelta = 0, Epitaph = "Reported" },
                    new MoralChoiceOption { Label = "Tell privately", MoralDelta = 2, EmpathyDelta = 1, Epitaph = "Told" }
                }
            };

            sys.Resolve(quest, 1, "loc_workshop", 35);

            // Day 69 < 70
            Assert.DoesNotContain(sys.FindAvailableEchoQuests(69), e => e.QuestId == "quest_moral_echo_kessler_exile_uncovered");

            // Day 70 >= 70
            Assert.Contains(sys.FindAvailableEchoQuests(70), e => e.QuestId == "quest_moral_echo_kessler_exile_uncovered");
        }

        [Fact]
        public void Runtime_BranchLockout_SuppressesLockedEchoQuests()
        {
            var sys = new MoralChoiceSystem(new SeededRng(42));
            var chainData = LoadChains();
            sys.InitializeChainData(chainData);

            // Resolve 3 entry quests on Iron Way to lock out Mercy Road & Listener Thread
            for (int i = 1; i <= 3; i++)
            {
                var ironEntry = new MoralChoiceQuestDefinition
                {
                    Id = $"quest_moral_chain_iron_{i:D2}",
                    DisplayName = $"Iron Entry {i}",
                    Category = "trust",
                    Choices = new List<MoralChoiceOption>
                    {
                        new MoralChoiceOption { Label = "Iron Choice", MoralDelta = -10, EmpathyDelta = 0, Epitaph = "Iron" }
                    }
                };
                sys.Resolve(ironEntry, 0, "loc_iron", 10 + i);
            }

            Assert.True(sys.IsBranchLocked("branch_mercy_road"));

            // Also resolve a Mercy quest
            var mercyQuest = new MoralChoiceQuestDefinition
            {
                Id = "quest_moral_chain_mercy_16",
                DisplayName = "The Prodigal Raider",
                Category = "comfort",
                Choices = new List<MoralChoiceOption>
                {
                    new MoralChoiceOption { Label = "Accept him", MoralDelta = 12, EmpathyDelta = 2, Epitaph = "Accepted" }
                }
            };
            sys.Resolve(mercyQuest, 0, "loc_shelter_gate", 50);

            // Even on day 200 (> 50 + 25), the Mercy echo MUST NOT fire because Mercy Road is locked!
            var available = sys.FindAvailableEchoQuests(200);
            Assert.DoesNotContain(available, e => e.QuestId == "quest_moral_echo_raider_repaid_warning");
        }

        [Fact]
        public void Runtime_MarkEchoQuestFired_PreventsRefiring()
        {
            var sys = new MoralChoiceSystem(new SeededRng(42));
            var chainData = LoadChains();
            sys.InitializeChainData(chainData);

            var quest = new MoralChoiceQuestDefinition
            {
                Id = "quest_moral_chain_mercy_16",
                DisplayName = "The Prodigal Raider",
                Category = "comfort",
                Choices = new List<MoralChoiceOption>
                {
                    new MoralChoiceOption { Label = "Accept him", MoralDelta = 12, EmpathyDelta = 2, Epitaph = "Accepted" }
                }
            };
            sys.Resolve(quest, 0, "loc_shelter_gate", 165);

            Assert.Contains(sys.FindAvailableEchoQuests(200), e => e.QuestId == "quest_moral_echo_raider_repaid_warning");

            sys.MarkEchoQuestFired("quest_moral_echo_raider_repaid_warning");

            Assert.DoesNotContain(sys.FindAvailableEchoQuests(200), e => e.QuestId == "quest_moral_echo_raider_repaid_warning");
        }

        [Fact]
        public void Runtime_Arbitration_DeterministicOrdering()
        {
            var sys1 = new MoralChoiceSystem(new SeededRng(100));
            var sys2 = new MoralChoiceSystem(new SeededRng(200));
            var chainData = LoadChains();
            sys1.InitializeChainData(chainData);
            sys2.InitializeChainData(chainData);

            var q1 = new MoralChoiceQuestDefinition
            {
                Id = "quest_moral_chain_mercy_04",
                DisplayName = "Mercy 04",
                Choices = new List<MoralChoiceOption> { new MoralChoiceOption { Label = "C0" } }
            };
            var q2 = new MoralChoiceQuestDefinition
            {
                Id = "quest_moral_chain_mercy_06",
                DisplayName = "Mercy 06",
                Choices = new List<MoralChoiceOption> { new MoralChoiceOption { Label = "C0" } }
            };

            sys1.Resolve(q1, 0, "loc_a", 10);
            sys1.Resolve(q2, 0, "loc_b", 10);

            sys2.Resolve(q1, 0, "loc_a", 10);
            sys2.Resolve(q2, 0, "loc_b", 10);

            var avail1 = sys1.FindAvailableEchoQuests(100).Select(e => e.QuestId).ToList();
            var avail2 = sys2.FindAvailableEchoQuests(100).Select(e => e.QuestId).ToList();

            Assert.NotEmpty(avail1);
            Assert.Equal(avail1, avail2);
        }
    }
}
