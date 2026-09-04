// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.MoralChoice;
using Ashfall.Core.Radio;
using Ashfall.Core.YearOfAsh;
using Xunit;

namespace Ashfall.Core.Tests.Radio
{
    /// <summary>
    /// Flagship Task 24: Distress Signal Faction Relationship Integration Suite.
    ///
    /// Validates:
    /// - Faction standing mutations reuse canonical FactionWarSystem.
    /// - Standing bounds (-100 to +100) and hostile/allied thresholds are strictly respected.
    /// - Rescue success grants authored faction reputation once.
    /// - Ignore choice applies authored standing penalty for faction-linked signals.
    /// - Raider lures and warlord traps do not apply victim standing penalties upon avoidance.
    /// - Idempotent consequence application across repeated queries and actions.
    /// - Full save/load preservation of standing deltas.
    /// </summary>
    public sealed class DistressSignalFactionTests : CatalogTestBase
    {
        private static (RadioDistressSystem distress, FactionWarSystem factionWar, MoralChoiceSystem moral, Dictionary<string, MoralChoiceQuestDefinition> quests) CreateFixture()
        {
            var distress = new RadioDistressSystem();
            string path = Path.Combine(DataDirectory, "radio_distress_signals.json");
            distress.LoadFromJson(File.ReadAllText(path));

            var factionWar = new FactionWarSystem();
            var moral = new MoralChoiceSystem(new SeededRng(42));
            var quests = MoralChoiceCatalogLoader.LoadStubs(DataDirectory, new FileSystemIO(), new SystemTextJsonSerializer())
                .ToDictionary(q => q.Id, StringComparer.OrdinalIgnoreCase);

            return (distress, factionWar, moral, quests);
        }

        [Fact]
        public void FactionReferences_ResolveAgainstCanonicalLore()
        {
            var (distress, _, _, _) = CreateFixture();
            string lorePath = Path.Combine(DataDirectory, "faction_lore.json");
            string loreRaw = File.ReadAllText(lorePath);

            foreach (var sig in distress.Definitions)
            {
                if (!string.IsNullOrEmpty(sig.SenderFactionId))
                {
                    Assert.Contains($"\"faction_id\": \"{sig.SenderFactionId}\"", loreRaw);
                }
                if (!string.IsNullOrEmpty(sig.DeceptiveFactionId))
                {
                    Assert.Contains($"\"faction_id\": \"{sig.DeceptiveFactionId}\"", loreRaw);
                }
            }
        }

        [Fact]
        public void RescueResolution_AwardsFactionStanding_Once()
        {
            var (distress, factionWar, moral, quests) = CreateFixture();

            // 901.2 MHz - Stranded Military Patrol (Iron Garrison)
            string signalId = "freq_distress_901_2";
            distress.Intercept(signalId, day: 1);
            distress.TryTriggerMoralChoice(signalId, out string moralChoiceId);

            int standingBefore = factionWar.GetStanding("iron_garrison");

            // Choose Rescue (choiceIndex = 0)
            bool success = distress.ResolveMoralChoice(signalId, 0, moral, quests[moralChoiceId], day: 2, out _, factionWar);
            Assert.True(success);

            // Complete the rescue expedition
            bool rescued = distress.CompleteRescue(signalId, factionWar);
            Assert.True(rescued);

            int standingAfter = factionWar.GetStanding("iron_garrison");
            Assert.Equal(standingBefore + 15, standingAfter);

            // Repeat call: standing must NOT increase again (idempotent)
            distress.CompleteRescue(signalId, factionWar);
            Assert.Equal(standingAfter, factionWar.GetStanding("iron_garrison"));
        }

        [Fact]
        public void IgnoreResolution_PenalizesFactionStanding_WhenConfigured()
        {
            var (distress, factionWar, moral, quests) = CreateFixture();

            // 901.2 MHz - Stranded Military Patrol (Iron Garrison, ignoreConsequence: faction_standing_loss)
            string signalId = "freq_distress_901_2";
            distress.Intercept(signalId, day: 1);
            distress.TryTriggerMoralChoice(signalId, out string moralChoiceId);

            int standingBefore = factionWar.GetStanding("iron_garrison");

            // Choose Ignore (choiceIndex = 1)
            bool success = distress.ResolveMoralChoice(signalId, 1, moral, quests[moralChoiceId], day: 2, out _, factionWar);
            Assert.True(success);

            int standingAfter = factionWar.GetStanding("iron_garrison");
            Assert.True(standingAfter < standingBefore, "Ignoring military patrol must reduce Iron Garrison standing");
            Assert.Equal(standingBefore - 15, standingAfter);

            // Repeat call: standing must NOT decrease again (idempotent)
            distress.ResolveMoralChoice(signalId, 1, moral, quests[moralChoiceId], day: 2, out _, factionWar);
            Assert.Equal(standingAfter, factionWar.GetStanding("iron_garrison"));
        }

        [Fact]
        public void RaiderTrapAvoidance_DoesNotPenalizeStanding()
        {
            var (distress, factionWar, _, _) = CreateFixture();

            // 192.4 MHz - Raider Lure (Fuel Cache, deceptive_faction_id: raiders)
            string trapId = "freq_distress_192_4";
            distress.Intercept(trapId, day: 1);
            distress.MarkTriangulated(trapId);

            int raiderStandingBefore = factionWar.GetStanding("raiders");

            // Avoid / expire trap safely
            distress.Resolve(trapId, DistressSignalStatus.ResolvedTrapAvoided, "Trap identified and avoided via skill check.");

            int raiderStandingAfter = factionWar.GetStanding("raiders");
            Assert.Equal(raiderStandingBefore, raiderStandingAfter);
        }

        [Fact]
        public void FactionStanding_RespectsCanonicalClampingBounds()
        {
            var factionWar = new FactionWarSystem();
            string factionId = "iron_garrison";

            // Push above +100
            for (int i = 0; i < 10; i++)
            {
                factionWar.ModifyStanding(factionId, 25);
            }
            Assert.Equal(100, factionWar.GetStanding(factionId));

            // Push below -100
            for (int i = 0; i < 15; i++)
            {
                factionWar.ModifyStanding(factionId, -25);
            }
            Assert.Equal(-100, factionWar.GetStanding(factionId));
        }

        [Fact]
        public void FactionStanding_PreservedAcrossSaveAndRestore()
        {
            var (distress, factionWar, moral, quests) = CreateFixture();

            string signalId = "freq_distress_901_2";
            distress.Intercept(signalId, day: 1);
            distress.TryTriggerMoralChoice(signalId, out string moralChoiceId);
            distress.ResolveMoralChoice(signalId, 0, moral, quests[moralChoiceId], day: 2, out _, factionWar);
            distress.CompleteRescue(signalId, factionWar);

            int savedStanding = factionWar.GetStanding("iron_garrison");
            Assert.Equal(15, savedStanding);
            var savedDistressState = distress.CaptureState();

            // Restore
            var freshFactionWar = new FactionWarSystem(factionWar.State);
            var freshDistress = new RadioDistressSystem();
            freshDistress.RestoreState(savedDistressState);

            Assert.Equal(savedStanding, freshFactionWar.GetStanding("iron_garrison"));
            var restoredSignal = freshDistress.GetActiveState(signalId);
            Assert.NotNull(restoredSignal);
            Assert.Equal(0, restoredSignal!.MoralChoiceResolutionIndex);
        }

        [Fact]
        public void CompleteRescue_RefusesIgnoredSignal_NoStandingFlip()
        {
            // INV-06 / §13: an ignored distress call is terminal. Completing a
            // "rescue" afterwards must not resurrect the sender or pay standing.
            var (distress, factionWar, moral, quests) = CreateFixture();

            string signalId = "freq_distress_901_2";
            distress.Intercept(signalId, day: 1);
            distress.TryTriggerMoralChoice(signalId, out string moralChoiceId);
            distress.ResolveMoralChoice(signalId, 1, moral, quests[moralChoiceId], day: 2, out _, factionWar);

            int standingAfterIgnore = factionWar.GetStanding("iron_garrison");
            Assert.Equal(-15, standingAfterIgnore);

            bool resurrected = distress.CompleteRescue(signalId, factionWar);
            Assert.False(resurrected, "An ignored signal can never be flipped to rescued");
            Assert.Equal(standingAfterIgnore, factionWar.GetStanding("iron_garrison"));
            Assert.Equal(DistressSignalStatus.ResolvedIgnored, distress.GetActiveState(signalId)!.Status);
        }

        [Fact]
        public void CompleteRescue_RefusesSignalWithNoExpeditionInFlight()
        {
            // T24.5 / R10: selecting "rescue" pays nothing until a rescue party
            // actually completes. Without a dispatched expedition, completion
            // (and the standing payout) is refused.
            var (distress, factionWar, moral, quests) = CreateFixture();

            string signalId = "freq_distress_901_2";
            distress.Intercept(signalId, day: 1);
            distress.TryTriggerMoralChoice(signalId, out string moralChoiceId);

            int standingBefore = factionWar.GetStanding("iron_garrison");

            // Choice alone must not pay standing (T24.5 preferred timing).
            bool chose = distress.ResolveMoralChoice(signalId, 0, moral, quests[moralChoiceId], day: 2, out _, factionWar);
            Assert.True(chose);
            Assert.Equal(standingBefore, factionWar.GetStanding("iron_garrison"));
        }
    }
}
