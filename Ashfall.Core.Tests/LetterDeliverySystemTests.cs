using System;
using System.IO;
using System.Text.Json;
using Xunit;
using Ashfall.Core.Narrative;

namespace Ashfall.Core.Tests
{
    public class LetterDeliverySystemTests
    {
        [Fact]
        public void DiscoverAndDeliverLetter_AppliesMoraleAndTransitionsState()
        {
            var sys = new LetterDeliverySystem();
            string letterId = "letter_01_dmitri_to_mother_in_leningrad";

            var discovered = sys.DiscoverLetter(letterId, 42, "survivor_dmitri");
            Assert.Equal(LetterDeliveryState.Addressed, discovered.state);
            Assert.Equal(42, discovered.foundDay);

            float moraleGiven = 0;
            sys.OnLetterDelivered += (rec, delta) => moraleGiven = delta;

            bool ok = sys.DeliverLetter(letterId, 45, "Delivered by expedition scout", 7.5f);
            Assert.True(ok);

            var finalRec = sys.GetRecord(letterId);
            Assert.NotNull(finalRec);
            Assert.Equal(LetterDeliveryState.Delivered, finalRec.state);
            Assert.Equal(45, finalRec.resolvedDay);
            Assert.Equal(7.5f, moraleGiven);
        }

        [Fact]
        public void WithholdAndUnanswered_TransitionsStateCorrectly()
        {
            var sys = new LetterDeliverySystem();
            string letterA = "letter_02_baker_anna_to_sister_in_odessa";
            string letterB = "letter_03_little_sonya_to_father_in_kiev";

            sys.WithholdLetter(letterA, 88, "Spared the dweller bad news from the coast");
            var recA = sys.GetRecord(letterA);
            Assert.NotNull(recA);
            Assert.Equal(LetterDeliveryState.Withheld, recA.state);

            sys.MarkUnanswered(letterB, 120, "Postbox rusted shut");
            var recB = sys.GetRecord(letterB);
            Assert.NotNull(recB);
            Assert.Equal(LetterDeliveryState.Unanswered, recB.state);
        }

        [Fact]
        public void StateSaveAndRestore_PreservesAllDeliveryRecords()
        {
            var sys1 = new LetterDeliverySystem();
            sys1.DiscoverLetter("letter_01_dmitri_to_mother_in_leningrad", 10, "survivor_dmitri");
            sys1.DeliverLetter("letter_01_dmitri_to_mother_in_leningrad", 12, "Delivered", 6.0f);
            sys1.WithholdLetter("letter_02_baker_anna_to_sister_in_odessa", 15, "Withheld");

            var state = sys1.CaptureState();
            Assert.Equal(2, state.records.Count);

            var sys2 = new LetterDeliverySystem();
            sys2.RestoreState(state);

            var rec1 = sys2.GetRecord("letter_01_dmitri_to_mother_in_leningrad");
            Assert.NotNull(rec1);
            Assert.Equal(LetterDeliveryState.Delivered, rec1.state);
            Assert.Equal(12, rec1.resolvedDay);

            var rec2 = sys2.GetRecord("letter_02_baker_anna_to_sister_in_odessa");
            Assert.NotNull(rec2);
            Assert.Equal(LetterDeliveryState.Withheld, rec2.state);
        }
    }
}
