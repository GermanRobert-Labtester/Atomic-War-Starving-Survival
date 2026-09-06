// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.Narrative;
using Ashfall.Core.YearOfAsh;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class NarrativeAndFactionWarIntegrationTests
    {
        private static readonly string DataDir = FindDataDir();

        private static string FindDataDir()
        {
            string? dir = new DirectoryInfo(AppContext.BaseDirectory).FullName;
            while (dir != null)
            {
                string probe = Path.Combine(dir, "Assets", "StreamingAssets", "Data", "narrative", "survivor_letters_lost_kin.json");
                if (File.Exists(probe)) return Path.Combine(dir, "Assets", "StreamingAssets", "Data");
                dir = Path.GetDirectoryName(dir.TrimEnd(Path.DirectorySeparatorChar));
            }
            throw new DirectoryNotFoundException("data authority not found");
        }

        [Fact]
        public void SurvivorLetterCatalog_LoadsAll25AuthoredLetters()
        {
            string path = Path.Combine(DataDir, "narrative", "survivor_letters_lost_kin.json");
            Assert.True(File.Exists(path), $"Letter catalog missing at {path}");

            string json = File.ReadAllText(path);
            var catalog = new SurvivorLetterCatalog();
            catalog.Load(json, new SystemTextJsonSerializer());

            Assert.Equal(25, catalog.AllLetters.Count);
            foreach (var letter in catalog.AllLetters)
            {
                Assert.False(string.IsNullOrEmpty(letter.letter_id));
                Assert.False(string.IsNullOrEmpty(letter.author_dweller));
                Assert.False(string.IsNullOrEmpty(letter.intended_recipient));
                Assert.False(string.IsNullOrEmpty(letter.letter_text));
            }
        }

        [Fact]
        public void SurvivorLetterDeliverySystem_FollowsFullDeliveryLifecycle()
        {
            string path = Path.Combine(DataDir, "narrative", "survivor_letters_lost_kin.json");
            string json = File.ReadAllText(path);
            var catalog = new SurvivorLetterCatalog();
            catalog.Load(json, new SystemTextJsonSerializer());

            var deliverySystem = new SurvivorLetterDeliverySystem(catalog);
            string letterId = "letter_01_dmitri_to_mother_in_leningrad";

            // 1. Initial state is NotFound
            Assert.Equal(LetterDeliveryStates.NotFound, deliverySystem.GetState(letterId));

            // 2. Mark found on day 15
            bool found = deliverySystem.MarkFound(letterId, 15);
            Assert.True(found);
            Assert.Equal(LetterDeliveryStates.Found, deliverySystem.GetState(letterId));

            // 3. Address matching to living dweller
            var dwellers = new List<DwellerAddressCandidate>
            {
                new DwellerAddressCandidate { SurvivorId = "survivor_dmitri", Name = "Dmitri Morozov", IsAlive = true },
                new DwellerAddressCandidate { SurvivorId = "survivor_anna", Name = "Anna Ivanova", IsAlive = true }
            };

            bool addressed = deliverySystem.TryAddressToSurvivor(letterId, dwellers);
            Assert.True(addressed);
            Assert.Equal(LetterDeliveryStates.Addressed, deliverySystem.GetState(letterId));
            var rec = deliverySystem.GetOrCreateRecord(letterId);
            Assert.Equal("survivor_dmitri", rec.matched_survivor_id);

            // 4. Deliver letter
            float appliedMorale = 0;
            string targetSurvivor = string.Empty;
            deliverySystem.Deliver(letterId, day: 16, applyMorale: (id, amount) =>
            {
                targetSurvivor = id;
                appliedMorale = amount;
            });

            Assert.Equal(LetterDeliveryStates.Delivered, deliverySystem.GetState(letterId));
            Assert.Equal("survivor_dmitri", targetSurvivor);
            Assert.Equal(SurvivorLetterDeliverySystem.DefaultDeliveryMoraleBonus, appliedMorale);
        }

        [Fact]
        public void SurvivorLetterDeliverySystem_WithholdAndUnansweredFlows()
        {
            var deliverySystem = new SurvivorLetterDeliverySystem();
            string letterWithheld = "letter_02_baker_anna_to_sister_in_odessa";
            string letterUnanswered = "letter_03_little_sonya_to_father_in_kiev";

            deliverySystem.MarkFound(letterWithheld, 20);
            deliverySystem.AssignRecipientExplicit(letterWithheld, "survivor_anna");

            float withheldPenalty = 0;
            deliverySystem.Withhold(letterWithheld, day: 21, applyMorale: (id, amount) => withheldPenalty = amount);
            Assert.Equal(LetterDeliveryStates.Withheld, deliverySystem.GetState(letterWithheld));
            Assert.Equal(SurvivorLetterDeliverySystem.DefaultWithholdMoralePenalty, withheldPenalty);

            deliverySystem.MarkFound(letterUnanswered, 25);
            deliverySystem.MarkUnanswered(letterUnanswered, 26);
            Assert.Equal(LetterDeliveryStates.Unanswered, deliverySystem.GetState(letterUnanswered));
        }

        [Fact]
        public void SurvivorLetterDeliverySystem_SaveRestoreRoundTrip()
        {
            var sys1 = new SurvivorLetterDeliverySystem();
            sys1.MarkFound("letter_01", 10);
            sys1.AssignRecipientExplicit("letter_01", "survivor_1");
            sys1.Deliver("letter_01", 12);

            sys1.MarkFound("letter_02", 15);
            sys1.AssignRecipientExplicit("letter_02", "survivor_2");
            sys1.Withhold("letter_02", 16);

            var save = sys1.CaptureState();

            var sys2 = new SurvivorLetterDeliverySystem();
            sys2.RestoreState(save);

            Assert.Equal(LetterDeliveryStates.Delivered, sys2.GetState("letter_01"));
            Assert.Equal(LetterDeliveryStates.Withheld, sys2.GetState("letter_02"));
            Assert.Equal("survivor_1", sys2.GetOrCreateRecord("letter_01").matched_survivor_id);
            Assert.Equal(12, sys2.GetOrCreateRecord("letter_01").resolved_day);
        }

        [Fact]
        public void FactionWarRunner_AppliesStandingDeltasToFactionWarSystem()
        {
            var factionWar = new FactionWarSystem();
            var catalog = new FactionWarContentCatalog();

            // Create a chain with a choice that adjusts standing
            var chain = new FactionWarEventChain
            {
                chainId = "test_chain_host_wiring",
                title = "Host Wiring Test Chain"
            };
            var stage = new FactionWarEventStage
            {
                stageId = "test_stage_1",
                triggerCondition = "immediate",
                bodyText = "Choose path:"
            };
            stage.choices.Add(new FactionWarEventChoice
            {
                choiceId = "choice_support_faction_a",
                text = "Send supplies",
                standingFactionId = "faction_water_barons",
                standingDelta = 15,
                leadsToStageId = string.Empty // terminal
            });
            chain.stages.Add(stage);
            catalog.AddEventChain(chain);

            var runner = new FactionWarChainRunner(catalog);

            // Wire standing delta applier to FactionWarSystem
            runner.StandingDeltaApplier = (factionId, delta) =>
            {
                factionWar.ModifyStanding(factionId, delta);
            };

            int initialStanding = factionWar.GetStanding("faction_water_barons");

            runner.ResolveChoice("test_chain_host_wiring", "test_stage_1", "choice_support_faction_a", currentDay: 5);

            int updatedStanding = factionWar.GetStanding("faction_water_barons");
            Assert.Equal(initialStanding + 15, updatedStanding);
        }
    }
}
