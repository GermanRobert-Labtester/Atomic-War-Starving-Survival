// SPDX-License-Identifier: MIT
// Task #132 P1-A — Defect D1: stale needs registration surviving a restore.
using System.Collections.Generic;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Survivors
{
    /// <summary>
    /// D1 reproduction and regression coverage.
    ///
    /// <para><b>The defect.</b> <c>NeedsSystem.Register</c> de-duplicated with
    /// <c>List.Contains</c>, which is reference equality for a class without an
    /// <c>Equals</c> override. Two different <see cref="SurvivorNeedsState"/>
    /// objects carrying the same <c>Id</c> could therefore both be registered.
    /// <c>Get(id)</c> returns the first match, so the older object won — and the
    /// simulation ticked both.</para>
    ///
    /// <para><b>How it reached production.</b>
    /// <c>SurvivorsHostSession.RestoreSave</c> cleared its own
    /// <c>RosterState</c> list and rebuilt fresh state objects, but never called
    /// <c>Needs.Unregister</c> for the ones it dropped. After a load the previous
    /// campaign's needs states stayed registered as ghosts. The same file's
    /// sibling <c>SevenDayDeterministicSmokeTest.RestoreSurvivorState</c> does
    /// unregister first, which is what showed the omission was an oversight
    /// rather than a design choice.</para>
    ///
    /// <para><b>Why it mattered.</b> A ghost kept decaying, so it could reach 0 HP
    /// and fire <c>OnDied</c> — and the host forwards that to the survivor-fate
    /// cascade, killing a survivor who is alive in the loaded campaign. The
    /// cascade's own <c>_needs.Get(id)</c> also resolved to the ghost, so
    /// <c>ForceDeath</c> and the shelter-wide grief loop mutated objects nothing
    /// was reading.</para>
    /// </summary>
    public class NeedsStaleRegistrationTests
    {
        private const string Mae = "the_surveyor";

        private static SurvivorNeedsState State(string id, float health = 100f, float hunger = 0f)
            => new SurvivorNeedsState { Id = id, Health = health, Hunger = hunger };

        // ── The mechanism ──────────────────────────────────────────────

        /// <summary>
        /// The core invariant: one registered state per survivor id. Registering a
        /// replacement must evict the old object, not shadow it.
        /// </summary>
        [Fact]
        public void Register_SameIdTwice_EvictsTheOlderState()
        {
            var needs = new NeedsSystem();
            var stale = State(Mae, health: 10f);
            var fresh = State(Mae, health: 90f);

            needs.Register(stale);
            needs.Register(fresh);

            Assert.Equal(1, needs.RegisteredCount);
            Assert.Same(fresh, Assert.Single(needs.Registered));
            Assert.Same(fresh, needs.Get(Mae));
        }

        [Fact]
        public void Register_SameInstanceTwice_IsIdempotent()
        {
            var needs = new NeedsSystem();
            var state = State(Mae);

            needs.Register(state);
            needs.Register(state);

            Assert.Equal(1, needs.RegisteredCount);
            Assert.Same(state, needs.Get(Mae));
        }

        [Fact]
        public void Register_DistinctIds_AreBothKept()
        {
            var needs = new NeedsSystem();
            var a = State("a_one");
            var b = State("b_two");

            needs.Register(a);
            needs.Register(b);

            Assert.Equal(2, needs.RegisteredCount);
            Assert.Same(a, needs.Get("a_one"));
            Assert.Same(b, needs.Get("b_two"));
        }

        [Fact]
        public void Register_NullIsIgnored()
        {
            var needs = new NeedsSystem();
            needs.Register(null!);
            Assert.Equal(0, needs.RegisteredCount);
        }

        /// <summary>
        /// States with no id cannot be keyed, so they keep the old reference-only
        /// de-duplication rather than evicting each other.
        /// </summary>
        [Fact]
        public void Register_EmptyIdStatesDoNotEvictEachOther()
        {
            var needs = new NeedsSystem();
            var a = State(string.Empty);
            var b = State(string.Empty);

            needs.Register(a);
            needs.Register(b);

            Assert.Equal(2, needs.RegisteredCount);
        }

        // ── The consequence: ghosts must not be simulated ──────────────

        /// <summary>
        /// The evicted state must stop being ticked. If it still decays it can
        /// still reach 0 HP and still report a death.
        /// </summary>
        [Fact]
        public void Tick_DoesNotAdvanceAnEvictedState()
        {
            var needs = new NeedsSystem();
            var stale = State(Mae, hunger: 50f);
            var fresh = State(Mae, hunger: 10f);

            needs.Register(stale);
            needs.Register(fresh);

            float staleHungerBefore = stale.Hunger;
            needs.Tick(10f);

            Assert.Equal(staleHungerBefore, stale.Hunger);
            Assert.True(fresh.Hunger > 10f, "the live state should have decayed");
        }

        /// <summary>
        /// The scenario that could kill a living survivor: a pre-restore state
        /// sitting one tick from death must not be able to announce it.
        /// </summary>
        [Fact]
        public void EvictedState_CannotReportDeath()
        {
            var needs = new NeedsSystem();

            // Ghost is starving and one tick from death.
            var ghost = State(Mae, health: 0.2f, hunger: 99f);
            // The loaded survivor is healthy and well fed.
            var restored = State(Mae, health: 100f, hunger: 0f);

            needs.Register(ghost);
            needs.Register(restored);

            var deaths = new List<SurvivorNeedsState>();
            needs.OnDied += s => deaths.Add(s);

            needs.Tick(24f);

            Assert.Empty(deaths);
            Assert.False(ghost.IsDead);
            Assert.False(restored.IsDead);
            Assert.True(restored.IsAliveState);
        }

        /// <summary>
        /// The lookup the survivor-fate cascade uses must resolve to the loaded
        /// state, so ForceDeath and the grief loop act on what the game is reading.
        /// </summary>
        [Fact]
        public void Get_ResolvesToTheRestoredState_NotTheGhost()
        {
            var needs = new NeedsSystem();
            var ghost = State(Mae, health: 5f);
            var restored = State(Mae, health: 80f);

            needs.Register(ghost);
            needs.Register(restored);

            var resolved = needs.Get(Mae);
            Assert.Same(restored, resolved);

            needs.ForceDeath(resolved!);

            Assert.True(restored.IsDead);
            Assert.False(ghost.IsDead);
        }

        [Fact]
        public void Modify_ByIdTargetsTheRestoredState()
        {
            var needs = new NeedsSystem();
            var ghost = State(Mae, hunger: 0f);
            var restored = State(Mae, hunger: 0f);

            needs.Register(ghost);
            needs.Register(restored);

            needs.Modify(Mae, NeedKind.Hunger, 25f);

            Assert.Equal(25f, restored.Hunger);
            Assert.Equal(0f, ghost.Hunger);
        }

        // ── Restore-shaped end-to-end reproduction ─────────────────────

        /// <summary>
        /// Reproduces the host restore sequence in Core: build a roster, tick it,
        /// then rebuild fresh state objects from a save without unregistering the
        /// old ones — exactly what SurvivorsHostSession.RestoreSave did.
        ///
        /// <para>Before the fix this left six registrations for three survivors,
        /// with every lookup resolving to a pre-restore ghost.</para>
        /// </summary>
        [Fact]
        public void RestoreShapedSequence_LeavesOneStatePerSurvivor()
        {
            var needs = new NeedsSystem();
            string[] ids = { "a_one", "b_two", "c_three" };

            // Original campaign.
            var original = new List<SurvivorNeedsState>();
            foreach (string id in ids)
            {
                var s = State(id, health: 100f);
                original.Add(s);
                needs.Register(s);
            }
            needs.Tick(48f); // let them get hungry

            // Restore: fresh objects, same ids, host forgets to unregister.
            var restored = new List<SurvivorNeedsState>();
            foreach (string id in ids)
            {
                var s = State(id, health: 100f, hunger: 5f);
                restored.Add(s);
                needs.Register(s);
            }

            Assert.Equal(3, needs.RegisteredCount);
            for (int i = 0; i < ids.Length; i++)
                Assert.Same(restored[i], needs.Get(ids[i]));

            // Only the restored objects advance from here.
            var hungerBefore = new List<float>();
            foreach (var g in original) hungerBefore.Add(g.Hunger);

            needs.Tick(5f);

            for (int i = 0; i < original.Count; i++)
                Assert.Equal(hungerBefore[i], original[i].Hunger);
            for (int i = 0; i < restored.Count; i++)
                Assert.True(restored[i].Hunger > 5f);
        }

        /// <summary>
        /// Registration order still determines tick order, and eviction must not
        /// scramble it — a reordered roster would change simulation results for
        /// the same seed.
        /// </summary>
        [Fact]
        public void Eviction_PreservesRegistrationOrderOfTheOthers()
        {
            var needs = new NeedsSystem();
            var a = State("a_one");
            var b = State("b_two");
            var c = State("c_three");

            needs.Register(a);
            needs.Register(b);
            needs.Register(c);

            var bReplacement = State("b_two");
            needs.Register(bReplacement);

            var order = new List<string>();
            foreach (var s in needs.Registered) order.Add(s.Id);

            // b keeps its slot rather than moving to the end.
            Assert.Equal(new[] { "a_one", "b_two", "c_three" }, order);
            Assert.Same(bReplacement, needs.Registered[1]);
        }

        [Fact]
        public void Unregister_RemovesTheState()
        {
            var needs = new NeedsSystem();
            var state = State(Mae);
            needs.Register(state);
            needs.Unregister(state);

            Assert.Equal(0, needs.RegisteredCount);
            Assert.Null(needs.Get(Mae));
        }
    }
}
