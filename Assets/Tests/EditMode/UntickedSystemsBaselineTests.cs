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
    }
}
