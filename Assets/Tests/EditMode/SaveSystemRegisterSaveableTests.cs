using NUnit.Framework;
using AtomicWar._Game.Core;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// H-9: SaveSystem.RegisterSaveable&lt;T&gt; lets a new system self-wire its
    /// persistence in one call instead of adding a dedicated SetXxx method +
    /// backing field to SaveSystem.Wiring.cs. It is the public counterpart of
    /// the private RegisterSystem&lt;T&gt; helper that backs the existing 558
    /// Set* methods, minus the ref-field assignment those need for internal
    /// CapIf-style special-casing (see SaveSystem.Capture.World.cs).
    /// </summary>
    [TestFixture]
    public class SaveSystemRegisterSaveableTests
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
        public void SetUp() => _dir = SaveSystemTestFactory.TempDir("register_saveable");

        [Test]
        public void RegisteredSystem_RoundTripsThroughSaveAndLoad()
        {
            var system = new FakeSystem { Value = 42 };
            var saveSystem = SaveSystemTestFactory.MakeSave(_dir, ss =>
                ss.RegisterSaveable(system, "fake_system",
                    s => new FakeSystemSave { Value = s.Value },
                    (s, o) => s.Value = ((FakeSystemSave)o).Value));

            Assert.IsTrue(saveSystem.Save("test_register_saveable"));

            var restoredSystem = new FakeSystem { Value = -1 };
            var saveSystem2 = SaveSystemTestFactory.MakeSave(_dir, ss =>
                ss.RegisterSaveable(restoredSystem, "fake_system",
                    s => new FakeSystemSave { Value = s.Value },
                    (s, o) => s.Value = ((FakeSystemSave)o).Value));

            Assert.IsTrue(saveSystem2.Load("test_register_saveable"));
            Assert.AreEqual(42, restoredSystem.Value);
        }

        [Test]
        public void NullSystem_DoesNotRegister_SaveDoesNotThrow()
        {
            FakeSystem system = null;
            var saveSystem = SaveSystemTestFactory.MakeSave(_dir, ss =>
                ss.RegisterSaveable(system, "fake_system",
                    s => new FakeSystemSave { Value = s.Value },
                    (s, o) => s.Value = ((FakeSystemSave)o).Value));

            Assert.DoesNotThrow(() => saveSystem.Save("test_null_system"));
        }

        [Test]
        public void SecondRegistration_SameSaveId_ReplacesFirst()
        {
            var first = new FakeSystem { Value = 1 };
            var second = new FakeSystem { Value = 2 };
            var saveSystem = SaveSystemTestFactory.MakeSave(_dir);
            saveSystem.RegisterSaveable(first, "fake_system",
                s => new FakeSystemSave { Value = s.Value },
                (s, o) => s.Value = ((FakeSystemSave)o).Value);

            saveSystem.RegisterSaveable(second, "fake_system",
                s => new FakeSystemSave { Value = s.Value },
                (s, o) => s.Value = ((FakeSystemSave)o).Value);

            Assert.IsTrue(saveSystem.Save("test_replace"));

            var restored = new FakeSystem { Value = -1 };
            var saveSystem2 = SaveSystemTestFactory.MakeSave(_dir, ss =>
                ss.RegisterSaveable(restored, "fake_system",
                    s => new FakeSystemSave { Value = s.Value },
                    (s, o) => s.Value = ((FakeSystemSave)o).Value));
            Assert.IsTrue(saveSystem2.Load("test_replace"));

            Assert.AreEqual(2, restored.Value, "The later registration for the same SaveId must win.");
        }
    }
}
