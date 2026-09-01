// SPDX-License-Identifier: MIT
// Task #132 — Canonical store, lifecycle transactions, determinism, persistence.
using System;
using System.Collections.Generic;
using System.Text.Json;
using Ashfall.Core;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Survivors
{
    public class SurvivorEntityStoreTests
    {
        private static SurvivorId Id(string raw) => new SurvivorId(raw);

        /// <summary>
        /// The options the real save path uses. The state DTOs expose public fields
        /// in the house style, and those only serialize with
        /// <c>IncludeFields = true</c> — so a round-trip test using default options
        /// would compare two empty documents and pass without proving anything.
        /// </summary>
        private static readonly JsonSerializerOptions SaveOptions = SystemTextJsonSerializer.Options;

        private const string Mae = "the_surveyor";
        private const string Iora = "elena_vasquez";
        private const string Expedition = "expedition_ashen_yard";

        private static SurvivorEntityStore StoreWith(params string[] ids)
        {
            var store = new SurvivorEntityStore();
            foreach (string id in ids)
                Assert.True(store.TryJoin(Id(id), id, day: 1).IsCommitted);
            return store;
        }

        // ── Join ───────────────────────────────────────────────────────

        [Fact]
        public void Join_CreatesResidentAtRevisionOne()
        {
            var store = new SurvivorEntityStore();
            var result = store.TryJoin(Id(Mae), definitionId: Mae, day: 40);

            Assert.True(result.IsCommitted);
            Assert.Equal(SurvivorLifecycleState.Unknown, result.From);
            Assert.Equal(SurvivorLifecycleState.Resident, result.To);
            Assert.Equal(1L, result.Revision);

            var survivor = store.GetRequired(Id(Mae));
            Assert.Equal(SurvivorLifecycleState.Resident, survivor.Lifecycle);
            Assert.Equal(40, survivor.JoinedDay);
            Assert.Equal(40, survivor.LifecycleDay);
            Assert.Equal(Mae, survivor.DefinitionId);
            Assert.Equal(string.Empty, survivor.ActiveExpeditionId);
            Assert.Equal(1, store.Count);
            Assert.Equal(1, store.LivingCount);
        }

        [Fact]
        public void Join_DefaultsDefinitionIdToTheSurvivorId()
        {
            var store = new SurvivorEntityStore();
            store.TryJoin(Id(Mae), definitionId: null, day: 1);
            Assert.Equal(Mae, store.GetRequired(Id(Mae)).DefinitionId);
        }

        [Fact]
        public void Join_DuplicateIsRefused_AndChangesNothing()
        {
            var store = StoreWith(Mae);
            long before = store.GetRequired(Id(Mae)).Revision;

            var result = store.TryJoin(Id(Mae), Mae, day: 9);

            Assert.True(result.IsBlocked);
            Assert.Equal(SurvivorLifecycleFailure.AlreadyExists, result.FailureCode);
            Assert.Equal(1, store.Count);
            Assert.Equal(before, store.GetRequired(Id(Mae)).Revision);
            Assert.Equal(1, store.GetRequired(Id(Mae)).JoinedDay);
        }

        [Fact]
        public void Join_EmptyIdIsRefused()
        {
            var store = new SurvivorEntityStore();
            var result = store.TryJoin(SurvivorId.None, "x", day: 1);

            Assert.True(result.IsBlocked);
            Assert.Equal(SurvivorLifecycleFailure.IdInvalid, result.FailureCode);
            Assert.Equal(0, store.Count);
        }

        [Fact]
        public void Join_RaisesJoinedAndLifecycleEventsAfterCommit()
        {
            var store = new SurvivorEntityStore();
            SurvivorAggregate? joined = null;
            var transitions = new List<SurvivorLifecycleTransition>();
            int changed = 0;

            // Events must observe committed state, so the store is already
            // queryable by the time a handler runs.
            store.OnJoined += a =>
            {
                joined = a;
                Assert.True(store.Contains(a.Id));
            };
            store.OnLifecycleChanged += t => transitions.Add(t);
            store.OnChanged += () => changed++;

            store.TryJoin(Id(Mae), Mae, day: 3);

            Assert.NotNull(joined);
            Assert.Equal(Id(Mae), joined!.Id);
            Assert.Single(transitions);
            Assert.Equal(SurvivorTransition.Join, transitions[0].Transition);
            Assert.Equal(SurvivorLifecycleState.Resident, transitions[0].To);
            Assert.Equal(1, changed);
        }

        // ── Deploy / Return ────────────────────────────────────────────

        [Fact]
        public void Deploy_MovesResidentToAwayAndRecordsExpedition()
        {
            var store = StoreWith(Mae);
            var result = store.TryDeploy(Id(Mae), Expedition, day: 5);

            Assert.True(result.IsCommitted);
            Assert.Equal(SurvivorLifecycleState.Resident, result.From);
            Assert.Equal(SurvivorLifecycleState.Away, result.To);
            Assert.Equal(2L, result.Revision);

            var survivor = store.GetRequired(Id(Mae));
            Assert.Equal(SurvivorLifecycleState.Away, survivor.Lifecycle);
            Assert.Equal(Expedition, survivor.ActiveExpeditionId);
            Assert.Equal(5, survivor.LifecycleDay);
            Assert.Equal(1, survivor.JoinedDay);
            Assert.Equal(1, store.DeployedCount);
            Assert.Equal(0, store.ResidentCount);
            Assert.Equal(1, store.LivingCount);
        }

        [Fact]
        public void Deploy_WithoutExpeditionIdIsRefused()
        {
            var store = StoreWith(Mae);
            var result = store.TryDeploy(Id(Mae), string.Empty, day: 5);

            Assert.True(result.IsBlocked);
            Assert.Equal(SurvivorLifecycleFailure.ExpeditionIdRequired, result.FailureCode);
            Assert.Equal(SurvivorLifecycleState.Resident, store.GetRequired(Id(Mae)).Lifecycle);
        }

        [Fact]
        public void Deploy_SameExpeditionTwiceIsIdempotent()
        {
            var store = StoreWith(Mae);
            store.TryDeploy(Id(Mae), Expedition, day: 5);
            long revision = store.GetRequired(Id(Mae)).Revision;

            var again = store.TryDeploy(Id(Mae), Expedition, day: 6);

            Assert.Equal(SurvivorLifecycleResult.StatusKind.AlreadyInState, again.Status);
            Assert.True(again.IsSatisfied);
            Assert.False(again.IsCommitted);
            Assert.Equal(revision, store.GetRequired(Id(Mae)).Revision);
            Assert.Equal(5, store.GetRequired(Id(Mae)).LifecycleDay);
        }

        /// <summary>
        /// A survivor cannot be on two expeditions at once. This is the invariant
        /// that a plain expedition dictionary could not express.
        /// </summary>
        [Fact]
        public void Deploy_ToASecondExpeditionWhileAwayIsRefused()
        {
            var store = StoreWith(Mae);
            store.TryDeploy(Id(Mae), Expedition, day: 5);

            var result = store.TryDeploy(Id(Mae), "expedition_other", day: 6);

            Assert.True(result.IsBlocked);
            Assert.Equal(SurvivorLifecycleFailure.TransitionIllegal, result.FailureCode);
            Assert.Contains("already deployed", result.Message);
            Assert.Equal(Expedition, store.GetRequired(Id(Mae)).ActiveExpeditionId);
        }

        [Fact]
        public void Deploy_UnknownSurvivorIsRefused()
        {
            var store = new SurvivorEntityStore();
            var result = store.TryDeploy(Id(Mae), Expedition, day: 1);

            Assert.True(result.IsBlocked);
            Assert.Equal(SurvivorLifecycleFailure.Unknown, result.FailureCode);
            Assert.Contains("does not exist", result.Message);
        }

        [Fact]
        public void Deploy_DeadSurvivorIsRefused()
        {
            var store = StoreWith(Mae);
            store.TryDie(Id(Mae), day: 4);

            var result = store.TryDeploy(Id(Mae), Expedition, day: 5);

            Assert.True(result.IsBlocked);
            Assert.Contains("the dead cannot be deployed", result.Message);
            Assert.Equal(SurvivorLifecycleState.Dead, store.GetRequired(Id(Mae)).Lifecycle);
        }

        [Fact]
        public void Return_BringsAwaySurvivorHomeAndClearsExpedition()
        {
            var store = StoreWith(Mae);
            store.TryDeploy(Id(Mae), Expedition, day: 5);

            var result = store.TryReturn(Id(Mae), day: 8);

            Assert.True(result.IsCommitted);
            Assert.Equal(SurvivorLifecycleState.Resident, result.To);
            Assert.Equal(3L, result.Revision);

            var survivor = store.GetRequired(Id(Mae));
            Assert.Equal(SurvivorLifecycleState.Resident, survivor.Lifecycle);
            Assert.Equal(string.Empty, survivor.ActiveExpeditionId);
            Assert.Equal(8, survivor.LifecycleDay);
        }

        [Fact]
        public void Return_WhenAlreadyHomeIsIdempotent()
        {
            var store = StoreWith(Mae);
            long revision = store.GetRequired(Id(Mae)).Revision;

            var result = store.TryReturn(Id(Mae), day: 8);

            Assert.Equal(SurvivorLifecycleResult.StatusKind.AlreadyInState, result.Status);
            Assert.Equal(revision, store.GetRequired(Id(Mae)).Revision);
        }

        [Fact]
        public void Return_DeadSurvivorIsRefused()
        {
            var store = StoreWith(Mae);
            store.TryDeploy(Id(Mae), Expedition, day: 5);
            store.TryDie(Id(Mae), day: 6);

            var result = store.TryReturn(Id(Mae), day: 7);

            Assert.True(result.IsBlocked);
            Assert.Contains("the dead do not return", result.Message);
        }

        // ── Death ──────────────────────────────────────────────────────

        [Fact]
        public void Die_FromResident()
        {
            var store = StoreWith(Mae, Iora);
            var result = store.TryDie(Id(Mae), day: 12);

            Assert.True(result.IsCommitted);
            Assert.Equal(SurvivorLifecycleState.Dead, result.To);
            Assert.Equal(SurvivorLifecycleState.Dead, store.GetRequired(Id(Mae)).Lifecycle);
            Assert.Equal(1, store.LivingCount);
            Assert.Equal(1, store.DeceasedCount);
            Assert.Equal(2, store.Count);
        }

        /// <summary>
        /// Dying in the field must clear deployment in the same commit, so a corpse
        /// can never remain an active expedition participant.
        /// </summary>
        [Fact]
        public void Die_WhileAway_ClearsTheExpeditionAtomically()
        {
            var store = StoreWith(Mae);
            store.TryDeploy(Id(Mae), Expedition, day: 5);

            var result = store.TryDie(Id(Mae), day: 7);

            Assert.True(result.IsCommitted);
            Assert.Equal(SurvivorLifecycleState.Away, result.From);

            var survivor = store.GetRequired(Id(Mae));
            Assert.Equal(SurvivorLifecycleState.Dead, survivor.Lifecycle);
            Assert.Equal(string.Empty, survivor.ActiveExpeditionId);
            Assert.False(survivor.IsDeployed);
            Assert.Equal(0, store.DeployedCount);
        }

        /// <summary>
        /// Several systems detect death independently. A second report must be a
        /// no-op so the fate cascade cannot run twice.
        /// </summary>
        [Fact]
        public void Die_IsIdempotent()
        {
            var store = StoreWith(Mae);
            var first = store.TryDie(Id(Mae), day: 12);
            long revision = store.GetRequired(Id(Mae)).Revision;

            var second = store.TryDie(Id(Mae), day: 13);

            Assert.True(first.IsCommitted);
            Assert.Equal(SurvivorLifecycleResult.StatusKind.AlreadyInState, second.Status);
            Assert.True(second.IsSatisfied);
            Assert.Equal(revision, store.GetRequired(Id(Mae)).Revision);
            Assert.Equal(12, store.GetRequired(Id(Mae)).LifecycleDay);
        }

        [Fact]
        public void Die_UnknownSurvivorIsRefused()
        {
            var store = new SurvivorEntityStore();
            var result = store.TryDie(Id(Mae), day: 1);
            Assert.True(result.IsBlocked);
            Assert.Equal(SurvivorLifecycleFailure.Unknown, result.FailureCode);
        }

        // ── Memorial ───────────────────────────────────────────────────

        [Fact]
        public void Memorialize_RequiresDead()
        {
            var store = StoreWith(Mae);

            var living = store.TryMemorialize(Id(Mae), day: 12);
            Assert.True(living.IsBlocked);
            Assert.Contains("cannot memorialize a living survivor", living.Message);

            store.TryDie(Id(Mae), day: 12);
            var dead = store.TryMemorialize(Id(Mae), day: 13);

            Assert.True(dead.IsCommitted);
            Assert.Equal(SurvivorLifecycleState.Memorialized, store.GetRequired(Id(Mae)).Lifecycle);
            Assert.Equal(1, store.DeceasedCount);
            Assert.Equal(0, store.LivingCount);
        }

        [Fact]
        public void Memorialize_IsIdempotent()
        {
            var store = StoreWith(Mae);
            store.TryDie(Id(Mae), day: 12);
            store.TryMemorialize(Id(Mae), day: 13);
            long revision = store.GetRequired(Id(Mae)).Revision;

            var again = store.TryMemorialize(Id(Mae), day: 14);

            Assert.Equal(SurvivorLifecycleResult.StatusKind.AlreadyInState, again.Status);
            Assert.Equal(revision, store.GetRequired(Id(Mae)).Revision);
        }

        [Fact]
        public void Memorialized_IsTerminal()
        {
            var store = StoreWith(Mae);
            store.TryDie(Id(Mae), day: 12);
            store.TryMemorialize(Id(Mae), day: 13);

            Assert.True(store.TryDeploy(Id(Mae), Expedition, day: 14).IsBlocked);
            Assert.True(store.TryReturn(Id(Mae), day: 14).IsBlocked);
            Assert.True(store.TryLeave(Id(Mae), day: 14).IsBlocked);
            Assert.Equal(
                SurvivorLifecycleResult.StatusKind.AlreadyInState,
                store.TryDie(Id(Mae), day: 14).Status);

            Assert.Equal(SurvivorLifecycleState.Memorialized, store.GetRequired(Id(Mae)).Lifecycle);
            Assert.Empty(SurvivorLifecycle.LegalTransitionsFrom(SurvivorLifecycleState.Memorialized));
        }

        // ── Leave ──────────────────────────────────────────────────────

        [Fact]
        public void Leave_RemovesResidentAndReleasesActiveComponents()
        {
            var store = StoreWith(Mae, Iora);
            var needs = new FakeSurvivorComponentStore("needs");
            var memorial = new FakeSurvivorComponentStore("memorial", retainsHistoryAfterDeath: true);
            store.RegisterComponentStore(needs);
            store.RegisterComponentStore(memorial);
            needs.Attach(Id(Mae));
            memorial.Attach(Id(Mae));

            SurvivorAggregate? left = null;
            store.OnLeft += a => left = a;

            var result = store.TryLeave(Id(Mae), day: 20);

            Assert.True(result.IsCommitted);
            Assert.False(store.Contains(Id(Mae)));
            Assert.Equal(1, store.Count);
            Assert.NotNull(left);
            Assert.Equal(Id(Mae), left!.Id);

            Assert.False(needs.Contains(Id(Mae)));       // active state released
            Assert.True(memorial.Contains(Id(Mae)));      // history retained
            Assert.Equal(0, memorial.ReleaseCallCount);   // history store never asked
        }

        [Fact]
        public void Leave_WhileDeployedIsRefused()
        {
            var store = StoreWith(Mae);
            store.TryDeploy(Id(Mae), Expedition, day: 5);

            var result = store.TryLeave(Id(Mae), day: 6);

            Assert.True(result.IsBlocked);
            Assert.Contains("cannot leave the campaign while deployed", result.Message);
            Assert.True(store.Contains(Id(Mae)));
        }

        [Fact]
        public void Leave_DeadSurvivorIsRefused()
        {
            var store = StoreWith(Mae);
            store.TryDie(Id(Mae), day: 5);

            var result = store.TryLeave(Id(Mae), day: 6);

            Assert.True(result.IsBlocked);
            Assert.True(store.Contains(Id(Mae)));
        }

        [Fact]
        public void Leave_DoesNotTouchComponentsWhenRefused()
        {
            var store = StoreWith(Mae);
            store.TryDeploy(Id(Mae), Expedition, day: 5);
            var needs = new FakeSurvivorComponentStore("needs");
            store.RegisterComponentStore(needs);
            needs.Attach(Id(Mae));

            Assert.True(store.TryLeave(Id(Mae), day: 6).IsBlocked);

            Assert.Equal(0, needs.ReleaseCallCount);
            Assert.True(needs.Contains(Id(Mae)));
        }

        // ── Atomicity ──────────────────────────────────────────────────

        /// <summary>
        /// The core atomicity guarantee: a refused transaction leaves the campaign
        /// bit-for-bit unchanged and fires no events.
        /// </summary>
        [Fact]
        public void BlockedTransaction_ChangesNothingAndRaisesNoEvents()
        {
            var store = StoreWith(Mae, Iora);
            string before = JsonSerializer.Serialize(store.CaptureState(), SaveOptions);

            int events = 0;
            store.OnLifecycleChanged += _ => events++;
            store.OnChanged += () => events++;
            store.OnJoined += _ => events++;
            store.OnLeft += _ => events++;

            Assert.True(store.TryJoin(Id(Mae), Mae, day: 2).IsBlocked);
            Assert.True(store.TryDeploy(Id(Mae), string.Empty, day: 2).IsBlocked);
            Assert.True(store.TryMemorialize(Id(Mae), day: 2).IsBlocked);
            Assert.True(store.TryDeploy(Id("the_unknown_one"), Expedition, day: 2).IsBlocked);
            Assert.True(store.TryLeave(Id("the_unknown_one"), day: 2).IsBlocked);

            Assert.Equal(0, events);
            Assert.Equal(before, JsonSerializer.Serialize(store.CaptureState(), SaveOptions));
        }

        /// <summary>
        /// A reference taken before a transition keeps showing the pre-transition
        /// state, because a transition swaps in a new immutable aggregate rather
        /// than mutating the old one. That makes a half-applied read impossible.
        /// </summary>
        [Fact]
        public void CommittedTransition_DoesNotMutateAPreviouslyReadAggregate()
        {
            var store = StoreWith(Mae);
            var snapshot = store.GetRequired(Id(Mae));

            store.TryDeploy(Id(Mae), Expedition, day: 5);

            Assert.Equal(SurvivorLifecycleState.Resident, snapshot.Lifecycle);
            Assert.Equal(string.Empty, snapshot.ActiveExpeditionId);
            Assert.Equal(1L, snapshot.Revision);

            var current = store.GetRequired(Id(Mae));
            Assert.Equal(SurvivorLifecycleState.Away, current.Lifecycle);
            Assert.Equal(2L, current.Revision);
            Assert.NotSame(snapshot, current);
        }

        [Fact]
        public void Revision_IncrementsOncePerCommittedTransition()
        {
            var store = StoreWith(Mae);
            Assert.Equal(1L, store.GetRequired(Id(Mae)).Revision);

            store.TryDeploy(Id(Mae), Expedition, day: 2);
            Assert.Equal(2L, store.GetRequired(Id(Mae)).Revision);

            store.TryReturn(Id(Mae), day: 3);
            Assert.Equal(3L, store.GetRequired(Id(Mae)).Revision);

            store.TryReturn(Id(Mae), day: 4); // already home — no increment
            Assert.Equal(3L, store.GetRequired(Id(Mae)).Revision);

            store.TryDie(Id(Mae), day: 5);
            Assert.Equal(4L, store.GetRequired(Id(Mae)).Revision);

            store.TryMemorialize(Id(Mae), day: 6);
            Assert.Equal(5L, store.GetRequired(Id(Mae)).Revision);
        }

        // ── Lookup ─────────────────────────────────────────────────────

        [Fact]
        public void TryGet_ReturnsFalseRatherThanFabricating()
        {
            var store = StoreWith(Mae);

            Assert.False(store.TryGet(Id("the_absent"), out var missing));
            Assert.Null(missing);
            Assert.False(store.TryGet(SurvivorId.None, out _));
            Assert.True(store.TryGet(Id(Mae), out var found));
            Assert.Equal(Id(Mae), found.Id);
        }

        [Fact]
        public void GetRequired_ThrowsWithTheIdAndCampaignSize()
        {
            var store = StoreWith(Mae);
            var ex = Assert.Throws<KeyNotFoundException>(() => store.GetRequired(Id("the_absent")));
            Assert.Contains("the_absent", ex.Message);
            Assert.Contains("1 survivor", ex.Message);
        }

        [Fact]
        public void TryResolve_IsTheRawStringBoundary()
        {
            var store = StoreWith(Mae);

            Assert.True(store.TryResolve(Mae, out var found));
            Assert.Equal(Id(Mae), found.Id);

            // Unknown, unparseable and null all fail without throwing and without
            // normalizing anything.
            Assert.False(store.TryResolve("the_absent", out _));
            Assert.False(store.TryResolve("The_Surveyor", out _));
            Assert.False(store.TryResolve(null, out _));
            Assert.False(store.TryResolve("", out _));
        }

        [Fact]
        public void RegisterComponentStore_RejectsDuplicateNames()
        {
            var store = new SurvivorEntityStore();
            store.RegisterComponentStore(new FakeSurvivorComponentStore("needs"));

            Assert.Throws<ArgumentException>(
                () => store.RegisterComponentStore(new FakeSurvivorComponentStore("needs")));
            Assert.Throws<ArgumentNullException>(() => store.RegisterComponentStore(null!));
            Assert.Single(store.ComponentStores);
        }

        // ── Determinism ────────────────────────────────────────────────

        /// <summary>
        /// Iteration must be ordinal by id, not join order — otherwise the same seed
        /// produces a different simulation depending on how the roster was built.
        /// </summary>
        [Fact]
        public void Iteration_IsOrdinalRegardlessOfJoinOrder()
        {
            var forward = StoreWith("a_first", "m_middle", "z_last");
            var reverse = StoreWith("z_last", "m_middle", "a_first");

            var forwardIds = new List<string>();
            foreach (var s in forward.Survivors) forwardIds.Add(s.Id.Value);

            var reverseIds = new List<string>();
            foreach (var s in reverse.Survivors) reverseIds.Add(s.Id.Value);

            Assert.Equal(new[] { "a_first", "m_middle", "z_last" }, forwardIds);
            Assert.Equal(forwardIds, reverseIds);
        }

        [Fact]
        public void Iteration_StaysOrderedAfterTransitionsAndDepartures()
        {
            var store = StoreWith("z_last", "a_first", "m_middle");
            store.TryDeploy(Id("m_middle"), Expedition, day: 2);
            store.TryDie(Id("z_last"), day: 3);
            store.TryLeave(Id("a_first"), day: 4);
            store.TryJoin(Id("b_second"), "b_second", day: 5);

            var ids = new List<string>();
            foreach (var s in store.Survivors) ids.Add(s.Id.Value);

            Assert.Equal(new[] { "b_second", "m_middle", "z_last" }, ids);
        }

        [Fact]
        public void Ids_MatchSurvivorOrder()
        {
            var store = StoreWith("z_last", "a_first");
            var ids = store.Ids;
            var survivors = store.Survivors;

            Assert.Equal(survivors.Count, ids.Count);
            for (int i = 0; i < ids.Count; i++)
                Assert.Equal(survivors[i].Id, ids[i]);
        }

        // ── Save / Load ────────────────────────────────────────────────

        [Fact]
        public void Capture_IsOrderedAndDetached()
        {
            var store = StoreWith("z_last", "a_first");
            var captured = store.CaptureState();

            Assert.Equal(SurvivorEntityStore.SchemaVersion, captured.schema_version);
            Assert.Equal(SurvivorEntityStore.SystemId, captured.system_id);
            Assert.Equal("a_first", captured.survivors[0].survivor_id);
            Assert.Equal("z_last", captured.survivors[1].survivor_id);

            // Mutating the snapshot must not reach live state.
            captured.survivors[0].lifecycle = (int)SurvivorLifecycleState.Dead;
            Assert.Equal(SurvivorLifecycleState.Resident, store.GetRequired(Id("a_first")).Lifecycle);
        }

        [Fact]
        public void RoundTrip_PreservesEveryLifecycleState()
        {
            var store = StoreWith("a_resident", "b_away", "c_dead", "d_memorial");
            store.TryDeploy(Id("b_away"), Expedition, day: 4);
            store.TryDie(Id("c_dead"), day: 5);
            store.TryDie(Id("d_memorial"), day: 6);
            store.TryMemorialize(Id("d_memorial"), day: 7);

            string json = JsonSerializer.Serialize(store.CaptureState(), SaveOptions);
            var restoredState = JsonSerializer.Deserialize<SurvivorEntityStoreState>(json, SaveOptions);

            var reloaded = new SurvivorEntityStore();
            var report = reloaded.RestoreState(restoredState);

            Assert.True(report.IsClean, report.ToString());
            Assert.Equal(4, report.Accepted);

            Assert.Equal(SurvivorLifecycleState.Resident, reloaded.GetRequired(Id("a_resident")).Lifecycle);

            var away = reloaded.GetRequired(Id("b_away"));
            Assert.Equal(SurvivorLifecycleState.Away, away.Lifecycle);
            Assert.Equal(Expedition, away.ActiveExpeditionId);

            Assert.Equal(SurvivorLifecycleState.Dead, reloaded.GetRequired(Id("c_dead")).Lifecycle);
            Assert.Equal(SurvivorLifecycleState.Memorialized, reloaded.GetRequired(Id("d_memorial")).Lifecycle);

            // Byte-identical re-capture: the round trip is lossless.
            Assert.Equal(json, JsonSerializer.Serialize(reloaded.CaptureState(), SaveOptions));
        }

        [Fact]
        public void RoundTrip_PreservesRevisionAndDays()
        {
            var store = StoreWith(Mae);
            store.TryDeploy(Id(Mae), Expedition, day: 5);
            var before = store.GetRequired(Id(Mae));

            var reloaded = new SurvivorEntityStore();
            reloaded.RestoreState(store.CaptureState());
            var after = reloaded.GetRequired(Id(Mae));

            Assert.Equal(before.Revision, after.Revision);
            Assert.Equal(before.JoinedDay, after.JoinedDay);
            Assert.Equal(before.LifecycleDay, after.LifecycleDay);
            Assert.Equal(before.DefinitionId, after.DefinitionId);
        }

        [Fact]
        public void Restore_RefusesANewerSchema()
        {
            var store = StoreWith(Mae);
            var state = store.CaptureState();
            state.schema_version = SurvivorEntityStore.SchemaVersion + 1;

            var target = StoreWith(Iora);
            var report = target.RestoreState(state);

            Assert.True(report.IsFatal);
            Assert.Contains("newer than this build", report.FatalReason);
            // A refused load must not have half-replaced the existing campaign.
            Assert.True(target.Contains(Id(Iora)));
            Assert.Equal(1, target.Count);
        }

        [Fact]
        public void Restore_NullClearsTheStore()
        {
            var store = StoreWith(Mae);
            var report = store.RestoreState(null);

            Assert.False(report.IsFatal);
            Assert.Equal(0, store.Count);
        }

        [Fact]
        public void Restore_RejectsUnparseableAndDuplicateIds()
        {
            var state = new SurvivorEntityStoreState();
            state.survivors.Add(new SurvivorAggregateState { survivor_id = "the_good", lifecycle = 1, revision = 1 });
            state.survivors.Add(new SurvivorAggregateState { survivor_id = "The_Bad", lifecycle = 1, revision = 1 });
            state.survivors.Add(new SurvivorAggregateState { survivor_id = "", lifecycle = 1, revision = 1 });
            state.survivors.Add(new SurvivorAggregateState { survivor_id = "the_good", lifecycle = 3, revision = 9 });
            state.survivors.Add(null!);

            var store = new SurvivorEntityStore();
            var report = store.RestoreState(state);

            Assert.Equal(1, report.Accepted);
            Assert.Equal(4, report.Rejected.Count);
            Assert.Contains(report.Rejected, r => r.Contains("uppercase", StringComparison.OrdinalIgnoreCase));
            Assert.Contains(report.Rejected, r => r.Contains("duplicate"));
            Assert.Contains(report.Rejected, r => r.Contains("null entry"));

            // First row wins: the duplicate did not overwrite it.
            Assert.Equal(SurvivorLifecycleState.Resident, store.GetRequired(Id("the_good")).Lifecycle);
            Assert.Equal(1L, store.GetRequired(Id("the_good")).Revision);
        }

        [Fact]
        public void Restore_RejectsIllegalLifecycleRatherThanGuessing()
        {
            var state = new SurvivorEntityStoreState();
            state.survivors.Add(new SurvivorAggregateState { survivor_id = "the_unknown_state", lifecycle = 0, revision = 1 });
            state.survivors.Add(new SurvivorAggregateState { survivor_id = "the_future_state", lifecycle = 99, revision = 1 });

            var store = new SurvivorEntityStore();
            var report = store.RestoreState(state);

            Assert.Equal(0, report.Accepted);
            Assert.Equal(2, report.Rejected.Count);
            Assert.All(report.Rejected, r => Assert.Contains("not a legal state", r));
            Assert.Equal(0, store.Count);
        }

        [Fact]
        public void Restore_RepairsAwayWithoutExpedition()
        {
            var state = new SurvivorEntityStoreState();
            state.survivors.Add(new SurvivorAggregateState
            {
                survivor_id = "the_stranded",
                lifecycle = (int)SurvivorLifecycleState.Away,
                active_expedition_id = "",
                revision = 3
            });

            var store = new SurvivorEntityStore();
            var report = store.RestoreState(state);

            Assert.Equal(1, report.Accepted);
            Assert.Single(report.Repaired);
            Assert.Contains("restored as Resident", report.Repaired[0]);
            Assert.Equal(SurvivorLifecycleState.Resident, store.GetRequired(Id("the_stranded")).Lifecycle);
        }

        [Fact]
        public void Restore_ClearsExpeditionOnNonAwaySurvivor()
        {
            var state = new SurvivorEntityStoreState();
            state.survivors.Add(new SurvivorAggregateState
            {
                survivor_id = "the_dead_walker",
                lifecycle = (int)SurvivorLifecycleState.Dead,
                active_expedition_id = Expedition,
                revision = 4
            });

            var store = new SurvivorEntityStore();
            var report = store.RestoreState(state);

            Assert.Equal(1, report.Accepted);
            Assert.Single(report.Repaired);
            Assert.Contains("cleared", report.Repaired[0]);
            Assert.Equal(string.Empty, store.GetRequired(Id("the_dead_walker")).ActiveExpeditionId);
        }

        [Fact]
        public void Restore_RaisesSubOneRevision()
        {
            var state = new SurvivorEntityStoreState();
            state.survivors.Add(new SurvivorAggregateState
            {
                survivor_id = "the_unversioned",
                lifecycle = (int)SurvivorLifecycleState.Resident,
                revision = 0
            });

            var store = new SurvivorEntityStore();
            var report = store.RestoreState(state);

            Assert.Single(report.Repaired);
            Assert.Equal(1L, store.GetRequired(Id("the_unversioned")).Revision);
        }

        [Fact]
        public void Restore_ReplacesRatherThanMergesPreviousCampaign()
        {
            var first = StoreWith("a_first", "b_second");
            var second = StoreWith("c_third");

            first.RestoreState(second.CaptureState());

            Assert.Equal(1, first.Count);
            Assert.True(first.Contains(Id("c_third")));
            Assert.False(first.Contains(Id("a_first")));
        }

        [Fact]
        public void Reset_EmptiesTheStore()
        {
            var store = StoreWith(Mae, Iora);
            int changed = 0;
            store.OnChanged += () => changed++;

            store.Reset();

            Assert.Equal(0, store.Count);
            Assert.Empty(store.Survivors);
            Assert.Equal(1, changed);
        }
    }
}
