using NUnit.Framework;
using AtomicWar._Game.Core;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// H-7: SystemRegistry.Register/Get&lt;T&gt; is the service-locator alternative
    /// to adding new GameBootstrap properties (C-3). These pin the instance-lookup
    /// half of the registry, kept separate from the tick-dispatch lists it also holds.
    /// </summary>
    [TestFixture]
    public class SystemRegistryInstanceLookupTests
    {
        private class FakeSystemA { public int Value; }
        private class FakeSystemB { }

        private SystemRegistry _registry;

        [SetUp]
        public void SetUp() => _registry = new SystemRegistry();

        [Test]
        public void Get_ReturnsRegisteredInstance()
        {
            var system = new FakeSystemA { Value = 42 };
            _registry.Register(system);

            var found = _registry.Get<FakeSystemA>();

            Assert.AreSame(system, found);
        }

        [Test]
        public void Get_UnregisteredType_ReturnsNull()
        {
            Assert.IsNull(_registry.Get<FakeSystemA>());
        }

        [Test]
        public void Register_Null_DoesNotOverwriteExistingInstance()
        {
            var system = new FakeSystemA();
            _registry.Register(system);

            _registry.Register<FakeSystemA>(null);

            Assert.AreSame(system, _registry.Get<FakeSystemA>());
        }

        [Test]
        public void Register_SameTypeTwice_ReplacesEarlierInstance()
        {
            var first = new FakeSystemA();
            var second = new FakeSystemA();
            _registry.Register(first);

            _registry.Register(second);

            Assert.AreSame(second, _registry.Get<FakeSystemA>());
        }

        [Test]
        public void DifferentTypes_AreLookedUpIndependently()
        {
            var a = new FakeSystemA();
            var b = new FakeSystemB();
            _registry.Register(a);
            _registry.Register(b);

            Assert.AreSame(a, _registry.Get<FakeSystemA>());
            Assert.AreSame(b, _registry.Get<FakeSystemB>());
        }

        [Test]
        public void TryGet_RegisteredType_ReturnsTrueAndInstance()
        {
            var system = new FakeSystemA();
            _registry.Register(system);

            bool found = _registry.TryGet<FakeSystemA>(out var result);

            Assert.IsTrue(found);
            Assert.AreSame(system, result);
        }

        [Test]
        public void TryGet_UnregisteredType_ReturnsFalseAndNull()
        {
            bool found = _registry.TryGet<FakeSystemA>(out var result);

            Assert.IsFalse(found);
            Assert.IsNull(result);
        }

        [Test]
        public void Clear_RemovesInstanceRegistrations()
        {
            _registry.Register(new FakeSystemA());

            _registry.Clear();

            Assert.IsNull(_registry.Get<FakeSystemA>());
        }

        [Test]
        public void InstanceLookup_IsIndependentOfTickRegistration()
        {
            // A system can be instance-registered (for lookup) without being
            // tick-registered (e.g. a save-only or purely event-driven system).
            var system = new FakeSystemA();
            _registry.Register(system);

            Assert.IsFalse(_registry.IsSystemTicked(nameof(FakeSystemA)));
            Assert.AreSame(system, _registry.Get<FakeSystemA>());
        }
    }
}
