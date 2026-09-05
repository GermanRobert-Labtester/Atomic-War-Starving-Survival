// SPDX-License-Identifier: MIT
// Plan 163 — Defense save/load: deep-copy capture, full trap-state round-trip,
// post-restore engagement equivalence.
using System;
using Ashfall.Core;
using Ashfall.Core.Defense;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class DefensePersistenceTests
    {
        private static DefenseTrapDefinition TestTrap(float capture = 0f) => new()
        {
            id = "trap_test_snare",
            display_name = "Test Snare",
            defense_type = "snare",
            base_strength = 2,
            max_hp = 100,
            activation_chance = 1f,
            capture_chance = capture,
            build_costs = { ["scrap_metal"] = 1 },
            reset_costs = { ["scrap_metal"] = 1 },
            repair_costs = { ["scrap_metal"] = 1 }
        };

        private static bool Stock(string _, int __) => true;

        [Fact]
        public void CaptureState_IsDeepCopyNotAlias()
        {
            var sys = new DefenseSystem(new[] { TestTrap() });
            sys.InstallTrap("trap_test_snare", "north", Stock);
            var snapshot = sys.CaptureState();

            sys.ResolvePreCombatRaid(10, 4, false, null, null, new SeededRng(1), new SeededRng(2));
            Assert.True(snapshot.installations[0].armed, "snapshot must not alias live sprung state");
            Assert.False(sys.Installations[0].armed);
        }

        [Fact]
        public void RoundTrip_PreservesAllTrapStatesAndLog()
        {
            var sys = new DefenseSystem(new[] { TestTrap(capture: 1f) });
            sys.InstallTrap("trap_test_snare", "north", Stock);
            sys.AssignManning(sys.Installations[0].installation_id, "survivor_a");
            sys.ResolvePreCombatRaid(12, 4, false, null, null, new SeededRng(1), new SeededRng(2));
            sys.TryResetTrap(sys.Installations[0].installation_id, Stock);

            var restored = new DefenseSystem(new[] { TestTrap(capture: 1f) });
            restored.RestoreState(sys.CaptureState());

            var a = sys.Installations[0];
            var b = restored.Installations[0];
            Assert.Equal(a.installation_id, b.installation_id);
            Assert.Equal(a.current_hp, b.current_hp);
            Assert.Equal(a.armed, b.armed);
            Assert.Equal(a.sprung, b.sprung);
            Assert.Equal(a.broken, b.broken);
            Assert.Equal(a.manned_by, b.manned_by);
            Assert.Equal(a.total_activations, b.total_activations);
            Assert.Equal(a.total_captures, b.total_captures);
            Assert.Equal(sys.RaidLog.Count, restored.RaidLog.Count);
        }

        [Fact]
        public void PostRestore_NextRaidMatchesUninterrupted()
        {
            var run = new Func<(int remaining, int captured, int logCount)>(() =>
            {
                var sys = new DefenseSystem(new[] { TestTrap(capture: 0.5f) });
                sys.InstallTrap("trap_test_snare", "north", Stock);
                // Raid 1, then reset; raid 2 must draw from the same stream ids.
                sys.ResolvePreCombatRaid(20, 5, false, null, null, new SeededRng(500), new SeededRng(501));
                sys.TryResetTrap(sys.Installations[0].installation_id, Stock);
                var r2 = sys.ResolvePreCombatRaid(21, 5, false, null, null, new SeededRng(510), new SeededRng(511));
                return (r2.RemainingRaiders, r2.RaidersCaptured, sys.RaidLog.Count);
            });

            // Run B: save after raid 1, restore into a fresh system, raid 2.
            var sysB = new DefenseSystem(new[] { TestTrap(capture: 0.5f) });
            sysB.InstallTrap("trap_test_snare", "north", Stock);
            sysB.ResolvePreCombatRaid(20, 5, false, null, null, new SeededRng(500), new SeededRng(501));
            sysB.TryResetTrap(sysB.Installations[0].installation_id, Stock);
            var restored = new DefenseSystem(new[] { TestTrap(capture: 0.5f) });
            restored.RestoreState(sysB.CaptureState());
            var b2 = restored.ResolvePreCombatRaid(21, 5, false, null, null, new SeededRng(510), new SeededRng(511));

            var a = run();
            Assert.Equal(a.remaining, b2.RemainingRaiders);
            Assert.Equal(a.captured, b2.RaidersCaptured);
            Assert.Equal(a.logCount, restored.RaidLog.Count);
        }

        [Fact]
        public void RestoreState_OldSaveDefaultsSurvive()
        {
            var sys = new DefenseSystem(null);
            sys.RestoreState(null);
            Assert.Empty(sys.Installations);
            var bare = new DefenseSystemState();
            bare.installations = null;
            bare.raid_log = null;
            sys.RestoreState(bare);
            Assert.NotNull(sys.Installations);
        }

        [Fact]
        public void InstallationCounter_ContinuesPastRestoredIds()
        {
            var sys = new DefenseSystem(new[] { TestTrap() });
            sys.InstallTrap("trap_test_snare", "a", Stock);
            sys.InstallTrap("trap_test_snare", "b", Stock);

            var restored = new DefenseSystem(new[] { TestTrap() });
            restored.RestoreState(sys.CaptureState());
            Assert.True(restored.InstallTrap("trap_test_snare", "c", Stock));
            Assert.Equal(3, restored.Installations.Count);
            Assert.All(restored.Installations, i => Assert.Single(restored.Installations, x => x.installation_id == i.installation_id));
        }
    }
}
