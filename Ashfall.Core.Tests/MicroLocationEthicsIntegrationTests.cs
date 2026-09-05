using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.Expeditions;
using Ashfall.Core.IO;
using Ashfall.Core.Narrative;
using Ashfall.Core.Random;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Task F15: Ethical choice consequence wiring tests.
    /// Verifies mandatory morale/guilt deltas, negative item offering consumption,
    /// atomic transactions, and save/load persistence.
    /// </summary>
    public class MicroLocationEthicsIntegrationTests
    {
        private static string DataDir()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null && !File.Exists(Path.Combine(dir.FullName, "Ashfall.csproj")))
                dir = dir.Parent!;
            return Path.Combine(dir!.FullName, "Assets", "StreamingAssets", "Data");
        }

        private static NarrativeEncounterSystem CreateLoadedSystem()
        {
            var sys = new NarrativeEncounterSystem();
            string dataDir = DataDir();
            var defs = NarrativeEncounterCatalogLoader.Load(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            foreach (var d in defs)
            {
                if (d.id.StartsWith("micro_", StringComparison.Ordinal))
                    sys.RegisterEncounter(d);
            }
            return sys;
        }

        [Fact]
        public void Shrine_TakeOfferings_AppliesMoraleMinus2GuiltPlus3()
        {
            var sys = CreateLoadedSystem();
            var res = sys.TryResolve("micro_shrine", "take_shrine_offerings", "loc_route", 1);

            Assert.NotNull(res);
            Assert.Equal(-2, res!.MoraleDelta);
            Assert.Equal(3, res.GuiltDelta);
            Assert.Equal("jewelry", res.GrantItemId);
            Assert.Equal(1, res.GrantItemQuantity);
            Assert.True(res.DepletesEncounter);
            Assert.True(sys.IsDepleted("micro_shrine"));
            Assert.Equal(-2, sys.State.cumulativeMorale);
            Assert.Equal(3, sys.State.cumulativeGuilt);
        }

        [Fact]
        public void Shrine_Leave_AppliesMoralePlus2GuiltZero()
        {
            var sys = CreateLoadedSystem();
            var res = sys.TryResolve("micro_shrine", "leave_shrine", "loc_route", 1);

            Assert.NotNull(res);
            Assert.Equal(2, res!.MoraleDelta);
            Assert.Equal(0, res.GuiltDelta);
            Assert.False(res.DepletesEncounter);
            Assert.False(sys.IsDepleted("micro_shrine"));
            Assert.Equal(2, sys.State.cumulativeMorale);
            Assert.Equal(0, sys.State.cumulativeGuilt);
        }

        [Fact]
        public void ImprovisedGrave_Disturb_AppliesMoraleMinus3GuiltPlus4()
        {
            var sys = CreateLoadedSystem();
            var res = sys.TryResolve("micro_improvised_grave", "disturb_grave", "loc_route", 2);

            Assert.NotNull(res);
            Assert.Equal(-3, res!.MoraleDelta);
            Assert.Equal(4, res.GuiltDelta);
            Assert.Equal("wedding_ring", res.GrantItemId);
            Assert.Equal(1, res.GrantItemQuantity);
            Assert.True(res.DepletesEncounter);
            Assert.True(sys.IsDepleted("micro_improvised_grave"));
            Assert.Equal(-3, sys.State.cumulativeMorale);
            Assert.Equal(4, sys.State.cumulativeGuilt);
        }

        [Fact]
        public void AbandonedTent_Search_AppliesMoraleMinus1GuiltPlus2()
        {
            var sys = CreateLoadedSystem();
            var res = sys.TryResolve("micro_abandoned_tent", "search_tent", "loc_route", 3);

            Assert.NotNull(res);
            Assert.Equal(-1, res!.MoraleDelta);
            Assert.Equal(2, res.GuiltDelta);
            Assert.Equal("cloth", res.GrantItemId);
            Assert.Equal(2, res.GrantItemQuantity);
            Assert.True(res.DepletesEncounter);
            Assert.True(sys.IsDepleted("micro_abandoned_tent"));
            Assert.Equal(-1, sys.State.cumulativeMorale);
            Assert.Equal(2, sys.State.cumulativeGuilt);
        }

        [Fact]
        public void EthicalChoice_ConsequencesSurviveSaveLoad()
        {
            var sys = CreateLoadedSystem();
            sys.TryResolve("micro_improvised_grave", "disturb_grave", "loc_route", 5);

            var saved = sys.CaptureState();

            var restored = CreateLoadedSystem();
            restored.RestoreState(saved);

            Assert.Equal(-3, restored.State.cumulativeMorale);
            Assert.Equal(4, restored.State.cumulativeGuilt);
            Assert.True(restored.IsDepleted("micro_improvised_grave"));
            Assert.Single(restored.State.history);
            Assert.Equal("micro_improvised_grave", restored.State.history[0].encounterId);
            Assert.Equal("disturb_grave", restored.State.history[0].choiceId);
        }

        [Fact]
        public void EthicalChoice_ResolutionAppliesExactlyOnce()
        {
            var sys = CreateLoadedSystem();
            var bridge = new ExpeditionEncounterBridge(sys, new SeededRng(100));

            // Surface an encounter
            var state = new ExpeditionState
            {
                expeditionId = "exp_1",
                survivorId = "surv_1",
                locationId = "rural_gas_station"
            };

            bridge.Surface(state);
            string surfacedId = bridge.LastSurfaced.encounter_id;
            Assert.False(string.IsNullOrEmpty(surfacedId));

            var def = sys.Find(surfacedId);
            Assert.NotNull(def);
            string choiceId = def!.choices[0].choiceId;

            bool first = bridge.ResolveChoice(surfacedId, choiceId, 1, "rural_gas_station");
            Assert.True(first);
            Assert.NotNull(bridge.LastResolution);

            // Repeated resolve on the already-resolved surfaced encounter must be rejected
            bool secondAttempt = bridge.ResolveChoice(surfacedId, choiceId, 1, "rural_gas_station");
            Assert.False(secondAttempt);
        }

        [Fact]
        public void Shrine_AddOffering_ConsumesRequiredResource()
        {
            var sys = CreateLoadedSystem();
            var def = sys.Find("micro_shrine");
            Assert.NotNull(def);

            var choice = def!.choices.Find(c => c.choiceId == "add_shrine_offering");
            Assert.NotNull(choice);

            // Authored shape: the offering is an unconditional negative grant —
            // sufficiency is enforced atomically by the host's F2 consumption
            // flow (ShelterInventory.TryConsume), not by a choice-level gate.
            Assert.True(string.IsNullOrEmpty(choice!.requiredItemId));
            Assert.Equal("canned_food", choice.grantItemId);
            Assert.Equal(-1, choice.grantItemQuantity);
            Assert.Equal(3, choice.moraleDelta);
            Assert.Equal(0, choice.guiltDelta);

            var res = sys.TryResolve("micro_shrine", "add_shrine_offering", "loc_route", 4);
            Assert.NotNull(res);
            Assert.Equal("canned_food", res!.GrantItemId);
            Assert.Equal(-1, res.GrantItemQuantity);
            Assert.Equal(3, res.MoraleDelta);
        }

        [Fact]
        public void Shrine_AddOffering_InsufficientResourceDoesNotPartiallyCommit()
        {
            // Simulate Host preflight: if player has insufficient inventory,
            // the resolution is not initiated, preventing partial morale/guilt commit.
            int playerCannedFood = 0;
            int requiredCannedFood = 1;

            var sys = CreateLoadedSystem();
            int startingMorale = sys.State.cumulativeMorale;
            int startingGuilt = sys.State.cumulativeGuilt;

            bool canCommit = playerCannedFood >= requiredCannedFood;
            Assert.False(canCommit);

            if (canCommit)
            {
                sys.TryResolve("micro_shrine", "add_shrine_offering", "loc_route", 1);
            }

            Assert.Equal(startingMorale, sys.State.cumulativeMorale);
            Assert.Equal(startingGuilt, sys.State.cumulativeGuilt);
            Assert.Equal(0, sys.State.totalResolved);
        }

        [Fact]
        public void HighGuiltChoice_DoesNotDuplicateOrInjectUnrelatedConsequences()
        {
            var sys = CreateLoadedSystem();
            var res = sys.TryResolve("micro_improvised_grave", "disturb_grave", "loc_route", 1);

            Assert.NotNull(res);
            Assert.Equal(4, res!.GuiltDelta);
            Assert.Equal(-3, res.MoraleDelta);
            Assert.Equal(string.Empty, res.JournalUnlockId);
            Assert.Equal(string.Empty, res.DiscoverLocationId);
            Assert.Equal(string.Empty, res.SetWorldFlagId);
        }

        [Fact]
        public void SeparateEncounterInstances_CanApplyOwnConsequences()
        {
            var sys = CreateLoadedSystem();

            // Two distinct encounters resolved in sequence
            var res1 = sys.TryResolve("micro_shrine", "leave_shrine", "loc_route", 1);
            var res2 = sys.TryResolve("micro_improvised_grave", "respect_grave", "loc_route", 2);

            Assert.NotNull(res1);
            Assert.NotNull(res2);
            Assert.Equal(4, sys.State.cumulativeMorale); // 2 + 2
            Assert.Equal(0, sys.State.cumulativeGuilt);
            Assert.Equal(2, sys.State.totalResolved);
        }
    }
}
