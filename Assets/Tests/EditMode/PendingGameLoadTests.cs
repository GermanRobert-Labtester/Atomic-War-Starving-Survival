using NUnit.Framework;
using AtomicWar._Game.Utilities;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Covers the menu -> gameplay handoff. The consume-clears behaviour is
    /// the load-bearing part: GameBootstrap.Awake applies the pending slot, and
    /// if it were not cleared a later scene reload would silently re-load a
    /// stale save over the player's session.
    /// </summary>
    [TestFixture]
    public class PendingGameLoadTests
    {
        // PendingGameLoad is static, so state survives between test cases
        // within a domain. Reset on both sides to keep cases independent and
        // to avoid leaking a pending slot into an unrelated fixture.
        [SetUp]
        public void SetUp() => PendingGameLoad.Clear();

        [TearDown]
        public void TearDown() => PendingGameLoad.Clear();

        [Test]
        public void SlotId_ByDefault_IsNullMeaningNewGame()
        {
            Assert.That(PendingGameLoad.SlotId, Is.Null);
        }

        [Test]
        public void Difficulty_ByDefault_IsOperative()
        {
            Assert.That(PendingGameLoad.Difficulty, Is.EqualTo(ExpeditionDifficulty.Operative));
        }

        [Test]
        public void ConsumeSlotId_WhenNothingPending_ReturnsNull()
        {
            Assert.That(PendingGameLoad.ConsumeSlotId(), Is.Null);
        }

        [Test]
        public void ConsumeSlotId_WhenSlotSet_ReturnsThatSlot()
        {
            PendingGameLoad.SlotId = "quicksave";

            Assert.That(PendingGameLoad.ConsumeSlotId(), Is.EqualTo("quicksave"));
        }

        [Test]
        public void ConsumeSlotId_AfterConsuming_ClearsThePendingSlot()
        {
            PendingGameLoad.SlotId = "autosave";
            PendingGameLoad.ConsumeSlotId();

            Assert.That(PendingGameLoad.SlotId, Is.Null);
        }

        [Test]
        public void ConsumeSlotId_CalledTwice_ReturnsNullOnTheSecondCall()
        {
            PendingGameLoad.SlotId = "autosave";

            Assert.That(PendingGameLoad.ConsumeSlotId(), Is.EqualTo("autosave"));
            Assert.That(PendingGameLoad.ConsumeSlotId(), Is.Null);
        }

        [Test]
        public void ConsumeSlotId_Always_LeavesDifficultyUntouched()
        {
            // Difficulty describes the run, not the load request, so consuming
            // the slot must not reset it.
            PendingGameLoad.SlotId = "quicksave";
            PendingGameLoad.Difficulty = ExpeditionDifficulty.Veteran;

            PendingGameLoad.ConsumeSlotId();

            Assert.That(PendingGameLoad.Difficulty, Is.EqualTo(ExpeditionDifficulty.Veteran));
        }

        [Test]
        public void Clear_AfterSettingBothFields_RestoresNewGameDefaults()
        {
            PendingGameLoad.SlotId = "quicksave";
            PendingGameLoad.Difficulty = ExpeditionDifficulty.Veteran;

            PendingGameLoad.Clear();

            Assert.That(PendingGameLoad.SlotId, Is.Null);
            Assert.That(PendingGameLoad.Difficulty, Is.EqualTo(ExpeditionDifficulty.Operative));
        }
    }
}
