using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Maintenance helper for the C-1 ratchet baseline. The regeneration test is
    /// [Explicit] so it never runs in CI — regenerating automatically would defeat the
    /// entire point of a ratchet (it would happily record new breakage as "expected").
    /// Run it by hand from the Test Runner after deliberately wiring systems up.
    /// </summary>
    [TestFixture]
    public class UntickedSystemsBaselineTests
    {
        private GameObject _go;
        private GameBootstrap _bootstrap;

        [SetUp]
        public void SetUp()
        {
            _go = new GameObject("UntickedSystemsBaselineTests");
            _go.SetActive(false);
            _bootstrap = _go.AddComponent<GameBootstrap>();

            // Mirror RegistryDispatchWiringTests: profiles must be injected before the
            // systems are built, and InitializeSystems is invoked directly because Awake
            // is not reliably dispatched under the EditMode runner.
            RegistryDispatchWiringTests.InjectBootstrapFields(_bootstrap);
            var init = typeof(GameBootstrap).GetMethod(
                "InitializeSystems", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.IsNotNull(init, "InitializeSystems must exist.");
            init.Invoke(_bootstrap, null);
            _go.SetActive(true);
        }

        [TearDown]
        public void TearDown()
        {
            if (_go != null) Object.DestroyImmediate(_go);
            _bootstrap = null;
        }

        [Test]
        public void Baseline_IsLoadable_AndMatchesFileOnDisk()
        {
            var baseline = UntickedSystemsBaseline.Load();
            Assert.IsNotEmpty(baseline,
                "Baseline file parsed to zero entries — if the debt really is cleared, " +
                "replace the ratchet with a plain IsEmpty assertion.");
        }

        [Test]
        [Explicit("Rewrites the checked-in baseline. Run deliberately, then review the diff.")]
        public void RegenerateBaselineFile()
        {
            var names = new List<string>(_bootstrap.GetUntickedSystemNames());
            UntickedSystemsBaseline.Write(names);
            Debug.Log($"[C-1] Wrote {names.Count} entries to {UntickedSystemsBaseline.RelativePath}");
        }

        [Test]
        public void GetUntickedSystemNames_TreatsDestroyedUnityObjectSystemAsMissing()
        {
            // L-3: `prop.GetValue(this) == null` boxes the value to `object`, which is
            // plain reference equality -- it never reaches UnityEngine.Object's
            // overridden `==` that also treats a destroyed ("fake null") object as
            // null. Reflection erases the static type, so a destroyed
            // ScriptableObject/MonoBehaviour system property (e.g. ShelterLayout)
            // used to read as still "constructed" instead of being skipped like every
            // other torn-down reference in this codebase.
            var method = typeof(GameBootstrap).GetMethod(
                "IsSystemPropertyValueMissing", BindingFlags.Static | BindingFlags.NonPublic);
            Assert.IsNotNull(method, "IsSystemPropertyValueMissing must exist.");

            Assert.IsTrue((bool)method.Invoke(null, new object[] { null }),
                "A plain C# null must read as missing.");
            Assert.IsFalse((bool)method.Invoke(null, new object[] { new object() }),
                "A live plain C# system reference must not be treated as missing.");

            var live = ScriptableObject.CreateInstance<ScriptableObject>();
            try
            {
                Assert.IsFalse((bool)method.Invoke(null, new object[] { live }),
                    "A live UnityEngine.Object system reference must not be treated as missing.");

                Object.DestroyImmediate(live);
                Assert.IsTrue((bool)method.Invoke(null, new object[] { live }),
                    "A destroyed UnityEngine.Object system reference must read as missing, " +
                    "matching every other != null check on a destroyed object in this codebase.");
            }
            finally
            {
                if (live != null) Object.DestroyImmediate(live);
            }
        }
    }
}
