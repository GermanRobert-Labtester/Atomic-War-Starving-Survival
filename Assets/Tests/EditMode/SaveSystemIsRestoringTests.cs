using NUnit.Framework;
using AtomicWar._Game.Core;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// M-8: SaveSystem.IsRestoring is true for the duration of a Load() and false
    /// otherwise, so event-firing systems can check it and skip notifying listeners
    /// that haven't been restored yet.
    /// </summary>
    [TestFixture]
    public class SaveSystemIsRestoringTests
    {
        private class FakeSystem
        {
            public int Value;
        }

        private class FakeSystemSave
        {
            public int Value;
        }

        private string _dir;

        [SetUp]
        public void SetUp() => _dir = SaveSystemTestFactory.TempDir("is_restoring");

        [Test]
        public void IsRestoring_IsFalse_BeforeAnyLoad()
        {
            var saveSystem = SaveSystemTestFactory.MakeSave(_dir);
            Assert.IsFalse(saveSystem.IsRestoring);
        }

        [Test]
        public void IsRestoring_IsTrue_DuringRestoreState_AndFalseAfterLoadReturns()
        {
            var system = new FakeSystem { Value = 7 };
            var saveSystem = SaveSystemTestFactory.MakeSave(_dir, ss =>
                ss.RegisterSaveable(system, "fake_system",
                    s => new FakeSystemSave { Value = s.Value },
                    (s, o) => s.Value = ((FakeSystemSave)o).Value));
            Assert.IsTrue(saveSystem.Save("test_is_restoring"));

            bool? observedDuringRestore = null;
            var restored = new FakeSystem { Value = -1 };
            var saveSystem2 = SaveSystemTestFactory.MakeSave(_dir, ss =>
                ss.RegisterSaveable(restored, "fake_system",
                    s => new FakeSystemSave { Value = s.Value },
                    (s, o) =>
                    {
                        observedDuringRestore = ss.IsRestoring;
                        s.Value = ((FakeSystemSave)o).Value;
                    }));

            Assert.IsTrue(saveSystem2.Load("test_is_restoring"));

            Assert.IsTrue(observedDuringRestore.HasValue, "restore callback never ran");
            Assert.IsTrue(observedDuringRestore.Value, "IsRestoring must be true while a subsystem is being restored");
            Assert.IsFalse(saveSystem2.IsRestoring, "IsRestoring must be false again once Load() returns");
        }

        [Test]
        public void IsRestoring_IsFalse_AfterFailedLoad()
        {
            var saveSystem = SaveSystemTestFactory.MakeSave(_dir);
            Assert.IsFalse(saveSystem.Load("no_such_slot"));
            Assert.IsFalse(saveSystem.IsRestoring);
        }
    }
}
