using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Recreation;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Recreation
{
    public sealed class SurvivorDowntimeSystemTests
    {
        private static SurvivorDowntimeSystem CreateSystem(out Inventory.Inventory inv, out NeedsSystem needs)
        {
            inv = new Inventory.Inventory();
            needs = new NeedsSystem();
            var rng = new SeededRng(1999);
            return new SurvivorDowntimeSystem(rng, inv, needs);
        }

        [Fact]
        public void StartSession_BlocksIfMissingRequiredItem()
        {
            var sys = CreateSystem(out var inv, out _);
            // hobby_guitar requires item_acoustic_guitar
            var res = sys.StartSession("hobby_guitar", "room_mess_hall", new List<string> { "survivor_01" });

            Assert.Equal(ActionResult.StatusKind.Blocked, res.Status);
            Assert.Equal("missing_hobby_item", res.FailureCode);
        }

        [Fact]
        public void StartSession_SucceedsWhenRequiredItemPresent()
        {
            var sys = CreateSystem(out var inv, out _);
            inv.AddById("item_acoustic_guitar", 1);

            var res = sys.StartSession("hobby_guitar", "room_mess_hall", new List<string> { "survivor_01" });
            Assert.Equal(ActionResult.StatusKind.Success, res.Status);
            Assert.Single(sys.State.activeSessions);
        }

        [Fact]
        public void Whittling_ProducesCraftedArtifact_AndRelievesStress()
        {
            var sys = CreateSystem(out var inv, out var needs);
            inv.AddById("scrap_wood", 2);

            var res = sys.StartSession("hobby_whittling", "room_workshop", new List<string> { "survivor_01" });
            Assert.Equal(ActionResult.StatusKind.Success, res.Status);

            sys.TickDay(1); // Completes pending sessions

            Assert.Empty(sys.State.activeSessions);
            Assert.True(inv.CountById("item_carved_figurine") >= 1);

            var prof = sys.GetOrCreateProfile("survivor_01");
            Assert.True(prof.stressRelievedTotal > 0f);
            Assert.Equal(1, prof.totalSessionsCompleted);
        }

        [Fact]
        public void SkillProgression_BoostsStressReliefEffectiveness()
        {
            var sys = CreateSystem(out var inv, out _);
            inv.AddById("item_playing_cards", 1);

            var prof = sys.GetOrCreateProfile("survivor_01");
            prof.skillLevel = 3; // +20% bonus

            sys.StartSession("hobby_card_games", "room_mess_hall", new List<string> { "survivor_01", "survivor_02" });
            sys.TickDay(1);

            Assert.True(prof.stressRelievedTotal > 20f);
        }

        [Fact]
        public void SaveAndRestore_PreservesProfilesAndHistory()
        {
            var sys = CreateSystem(out var inv, out _);
            inv.AddById("scrap_wood", 1);
            sys.StartSession("hobby_whittling", "room_workshop", new List<string> { "survivor_01" });
            sys.TickDay(1);

            var saved = sys.CaptureState();
            var restored = CreateSystem(out _, out _);
            restored.RestoreState(saved);

            var prof = restored.GetOrCreateProfile("survivor_01");
            Assert.Equal(1, prof.totalSessionsCompleted);
            Assert.Single(restored.State.sessionHistory);
        }
    }
}
