// SPDX-License-Identifier: MIT
// Task #132 — Typed Needs component and parity coverage.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Ashfall.Core;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Survivors
{
    public class NeedsComponentStoreTests
    {
        private static SurvivorId Id(string raw) => new SurvivorId(raw);

        private static SurvivorNeedsState State(string id, float hunger = 0f)
            => new SurvivorNeedsState
            {
                Id = id,
                Hunger = hunger,
                Thirst = 12f,
                Fatigue = 23f,
                Warmth = 84f,
                Morale = 61f,
                Health = 73f,
                Hygiene = 48f,
                WasHungerCritical = true,
                WasThirstCritical = false,
                WasWarmthCritical = true,
                MaxHealthCap = 87f,
                IsAlive = true,
                IsDead = false
            };

        private static void AssertStateEqual(SurvivorNeedsState expected, SurvivorNeedsState actual)
        {
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Hunger, actual.Hunger);
            Assert.Equal(expected.Thirst, actual.Thirst);
            Assert.Equal(expected.Fatigue, actual.Fatigue);
            Assert.Equal(expected.Warmth, actual.Warmth);
            Assert.Equal(expected.Morale, actual.Morale);
            Assert.Equal(expected.Health, actual.Health);
            Assert.Equal(expected.Hygiene, actual.Hygiene);
            Assert.Equal(expected.WasHungerCritical, actual.WasHungerCritical);
            Assert.Equal(expected.WasThirstCritical, actual.WasThirstCritical);
            Assert.Equal(expected.WasWarmthCritical, actual.WasWarmthCritical);
            Assert.Equal(expected.MaxHealthCap, actual.MaxHealthCap);
            Assert.Equal(expected.IsAlive, actual.IsAlive);
            Assert.Equal(expected.IsDead, actual.IsDead);
            Assert.Equal(expected.IsAliveState, actual.IsAliveState);
        }

        [Fact]
        public void Store_UsesTypedOwnerAndRejectsRawIdMismatch()
        {
            var store = new NeedsComponentStore();
            var first = State("a_one", hunger: 11f);

            Assert.True(store.TryUpsert(Id("a_one"), first, out string firstError), firstError);
            Assert.True(store.Contains(Id("a_one")));
            Assert.Same(first, store.TryGet(Id("a_one"), out var found) ? found : null);

            var mismatch = State("b_two", hunger: 99f);
            Assert.False(store.TryUpsert(Id("a_one"), mismatch, out string error));
            Assert.Contains("does not match", error);
            Assert.Same(first, store.TryGet(Id("a_one"), out found) ? found : null);
            Assert.Equal(1, store.Count);
        }

        [Fact]
        public void Store_UpsertReplacesById_AndOwnerIdsAreOrdinal()
        {
            var store = new NeedsComponentStore();
            Assert.True(store.TryRegister(Id("z_last"), State("z_last"), out _));
            Assert.True(store.TryRegister(Id("a_first"), State("a_first"), out _));

            var replacement = State("z_last", hunger: 66f);
            Assert.True(store.TryUpsert(Id("z_last"), replacement, out _));

            Assert.Equal(new[] { "a_first", "z_last" },
                store.OwnerIds.Select(id => id.Value).ToArray());
            Assert.Equal(2, store.Count);
            Assert.Same(replacement, store.TryGet(Id("z_last"), out var current) ? current : null);
            Assert.Equal(66f, current!.Hunger);
        }

        [Fact]
        public void Store_ImplementsComponentContract()
        {
            ISurvivorComponentStore component = new NeedsComponentStore();

            Assert.Equal("needs", component.ComponentName);
            Assert.Equal(SurvivorComponentCardinality.ZeroOrOne, component.Cardinality);
            Assert.False(component.RetainsHistoryAfterDeath);
            Assert.False(component.Contains(Id("the_absent")));
            Assert.False(component.Release(Id("the_absent")));
        }

        [Fact]
        public void Capture_IsSortedAndDetached_AndIncludesEveryPersistedField()
        {
            var store = new NeedsComponentStore();
            var source = State("z_last", hunger: 37.25f);
            store.TryUpsert(Id("z_last"), source, out _);
            store.TryUpsert(Id("a_first"), State("a_first"), out _);

            var captured = store.CaptureState();

            Assert.Equal(NeedsComponentStore.SchemaVersion, captured.schema_version);
            Assert.Equal(NeedsComponentStore.SystemId, captured.system_id);
            Assert.Equal(new[] { "a_first", "z_last" },
                captured.survivors.Select(row => row.survivor_id).ToArray());

            var row = captured.survivors[1];
            Assert.Equal(37.25f, row.hunger);
            Assert.Equal(12f, row.thirst);
            Assert.Equal(23f, row.fatigue);
            Assert.Equal(84f, row.warmth);
            Assert.Equal(61f, row.morale);
            Assert.Equal(73f, row.health);
            Assert.Equal(48f, row.hygiene);
            Assert.True(row.was_hunger_critical);
            Assert.False(row.was_thirst_critical);
            Assert.True(row.was_warmth_critical);
            Assert.Equal(87f, row.max_health_cap);
            Assert.True(row.is_alive);
            Assert.False(row.is_dead);

            row.hunger = 0f;
            Assert.Equal(37.25f, source.Hunger);
        }

        [Fact]
        public void RoundTrip_PreservesRowsAndWireShape()
        {
            var original = new NeedsComponentStore();
            var source = State("the_surveyor", hunger: 41.5f);
            original.TryUpsert(Id("the_surveyor"), source, out _);

            string json = JsonSerializer.Serialize(original.CaptureState(), SystemTextJsonSerializer.Options);
            var restoredState = JsonSerializer.Deserialize<NeedsComponentStoreState>(
                json, SystemTextJsonSerializer.Options);
            var restored = new NeedsComponentStore();
            var report = restored.RestoreState(restoredState);

            Assert.True(report.IsClean, report.ToString());
            Assert.Equal(1, report.Accepted);
            Assert.True(restored.TryGet(Id("the_surveyor"), out var state));
            Assert.NotNull(state);
            AssertStateEqual(source, state!);
            Assert.Contains("schema_version", json);
            Assert.Contains("survivor_id", json);
            Assert.Contains("max_health_cap", json);
            Assert.Equal(json,
                JsonSerializer.Serialize(restored.CaptureState(), SystemTextJsonSerializer.Options));
        }

        [Fact]
        public void Restore_RejectsFutureSchemaWithoutReplacingCurrentState()
        {
            var store = new NeedsComponentStore();
            store.TryUpsert(Id("a_current"), State("a_current"), out _);
            var future = new NeedsComponentStoreState
            {
                schema_version = NeedsComponentStore.SchemaVersion + 1,
                system_id = NeedsComponentStore.SystemId
            };
            future.survivors.Add(new NeedsComponentState { survivor_id = "z_future" });

            var report = store.RestoreState(future);

            Assert.True(report.IsFatal);
            Assert.Contains("newer than this build", report.FatalReason);
            Assert.True(store.Contains(Id("a_current")));
            Assert.False(store.Contains(Id("z_future")));
        }

        [Fact]
        public void Restore_UsesFirstRowForDuplicateAndRejectsInvalidRows()
        {
            var state = new NeedsComponentStoreState();
            state.survivors.Add(new NeedsComponentState { survivor_id = "the_good", hunger = 12f });
            state.survivors.Add(new NeedsComponentState { survivor_id = "The_Bad" });
            state.survivors.Add(new NeedsComponentState { survivor_id = string.Empty });
            state.survivors.Add(new NeedsComponentState { survivor_id = "the_good", hunger = 99f });
            state.survivors.Add(null!);

            var store = new NeedsComponentStore();
            var report = store.RestoreState(state);

            Assert.Equal(1, report.Accepted);
            Assert.Equal(4, report.Rejected.Count);
            Assert.True(store.TryGet(Id("the_good"), out var restored));
            Assert.Equal(12f, restored!.Hunger);
            Assert.Contains(report.Rejected, row => row.Contains("uppercase", StringComparison.OrdinalIgnoreCase));
            Assert.Contains(report.Rejected, row => row.Contains("duplicate", StringComparison.Ordinal));
            Assert.Contains(report.Rejected, row => row.Contains("null entry", StringComparison.Ordinal));
        }

        [Fact]
        public void ReleaseAndReset_RemoveOnlyActiveRecords()
        {
            var store = new NeedsComponentStore();
            store.TryUpsert(Id("a_one"), State("a_one"), out _);
            store.TryUpsert(Id("b_two"), State("b_two"), out _);

            Assert.True(store.Release(Id("a_one")));
            Assert.False(store.Contains(Id("a_one")));
            Assert.False(store.Release(Id("a_one")));
            Assert.Equal(1, store.Count);

            store.Reset();
            Assert.Equal(0, store.Count);
            Assert.Empty(store.OwnerIds);
        }

        [Fact]
        public void Parity_CleanWhenLegacyAndTypedRowsMatch()
        {
            var legacy = new NeedsSystem();
            var state = State("a_one", hunger: 22f);
            legacy.Register(state);

            var typed = new NeedsComponentStore();
            typed.TryUpsert(Id("a_one"), state, out _);

            var report = NeedsComponentParity.Compare(legacy, typed);

            Assert.True(report.IsMatch, report.Describe());
            Assert.Equal(1, report.LegacyRows);
            Assert.Equal(1, report.TypedRows);
        }

        [Fact]
        public void Parity_ReportsDuplicateMissingExtraAndFieldMismatchInStableOrder()
        {
            var legacy = new NeedsSystem();
            legacy.Register(State("a_one", hunger: 10f));
            legacy.Register(State("b_two", hunger: 20f));
            var duplicate = State("c_three", hunger: 30f);
            legacy.Register(duplicate);
            duplicate.Id = "b_two"; // simulate a legacy list corrupted after registration

            var typed = new NeedsComponentStore();
            typed.TryUpsert(Id("a_one"), State("a_one", hunger: 11f), out _);
            typed.TryUpsert(Id("z_extra"), State("z_extra"), out _);

            var report = NeedsComponentParity.Compare(legacy, typed);

            Assert.False(report.IsMatch);
            Assert.Contains(report.Findings, finding => finding.Code == NeedsParityCode.LegacyDuplicateId);
            Assert.Contains(report.Findings, finding => finding.Code == NeedsParityCode.TypedRecordMissing && finding.SurvivorId == Id("b_two"));
            Assert.Contains(report.Findings, finding => finding.Code == NeedsParityCode.TypedRecordExtra && finding.SurvivorId == Id("z_extra"));
            Assert.Contains(report.Findings, finding => finding.Code == NeedsParityCode.FieldMismatch && finding.Field == "hunger");

            var order = report.Findings.Select(finding => finding.SurvivorId.Value).ToArray();
            Assert.Equal(order.OrderBy(id => id, StringComparer.Ordinal).ToArray(), order);
        }

        [Fact]
        public void Parity_ReportsInvalidLegacyIdsAndDuplicateMalformedRawIds()
        {
            var legacy = new NeedsSystem();
            var first = State("a_one");
            var second = State("b_two");
            legacy.Register(first);
            legacy.Register(second);
            first.Id = "The_Bad";
            second.Id = "The_Bad";
            legacy.Register(State(string.Empty));

            var report = NeedsComponentParity.Compare(legacy, new NeedsComponentStore());

            var invalid = report.Findings
                .Where(finding => finding.Code == NeedsParityCode.LegacyIdInvalid)
                .ToList();
            Assert.Equal(3, invalid.Count);
            var duplicate = Assert.Single(
                report.Findings,
                finding => finding.Code == NeedsParityCode.LegacyDuplicateId);
            Assert.Equal(SurvivorId.None, duplicate.SurvivorId);
            Assert.Equal("The_Bad", duplicate.RawId);
            Assert.Equal("2", duplicate.Actual);

            var rawIds = report.Findings.Select(finding => finding.RawId).ToArray();
            Assert.Equal(rawIds.OrderBy(rawId => rawId, StringComparer.Ordinal).ToArray(), rawIds);
        }

        [Fact]
        public void Parity_ReportsTypedKeyWhenStateRawIdDrifts()
        {
            var legacy = new NeedsSystem();
            legacy.Register(State("a_one"));

            var typed = new NeedsComponentStore();
            var typedState = State("a_one");
            typed.TryUpsert(Id("a_one"), typedState, out _);
            typedState.Id = "The_Bad";

            var report = NeedsComponentParity.Compare(legacy, typed);
            var finding = Assert.Single(
                report.Findings,
                item => item.Code == NeedsParityCode.TypedIdMismatch);

            Assert.Equal(Id("a_one"), finding.SurvivorId);
            Assert.Equal("The_Bad", finding.RawId);
            Assert.Equal("a_one", finding.Expected);
            Assert.Equal("The_Bad", finding.Actual);
        }

        [Fact]
        public void Parity_ReportsAllPersistedFields()
        {
            var legacyState = State("a_one");
            var typedState = State("a_one");
            typedState.Hunger += 1f;
            typedState.Thirst += 1f;
            typedState.Fatigue += 1f;
            typedState.Warmth -= 1f;
            typedState.Morale -= 1f;
            typedState.Health -= 1f;
            typedState.Hygiene -= 1f;
            typedState.WasHungerCritical = false;
            typedState.WasThirstCritical = true;
            typedState.WasWarmthCritical = false;
            typedState.MaxHealthCap = 80f;
            typedState.IsAlive = false;
            typedState.IsDead = true;

            var legacy = new NeedsSystem();
            legacy.Register(legacyState);
            var typed = new NeedsComponentStore();
            typed.TryUpsert(Id("a_one"), typedState, out _);

            var fields = NeedsComponentParity.Compare(legacy, typed)
                .Findings
                .Where(finding => finding.Code == NeedsParityCode.FieldMismatch)
                .Select(finding => finding.Field)
                .ToHashSet(StringComparer.Ordinal);

            Assert.Equal(
                new[]
                {
                    "fatigue", "health", "hunger", "hygiene", "is_alive", "is_alive_state",
                    "is_dead", "max_health_cap", "morale", "thirst", "warmth",
                    "was_hunger_critical", "was_thirst_critical", "was_warmth_critical"
                },
                fields.OrderBy(field => field, StringComparer.Ordinal).ToArray());
        }

        [Fact]
        public void Parity_DoesNotMutateEitherSide()
        {
            var legacy = new NeedsSystem();
            var state = State("a_one", hunger: 15f);
            legacy.Register(state);
            var typed = new NeedsComponentStore();
            typed.TryUpsert(Id("a_one"), state, out _);

            NeedsComponentParity.Compare(legacy, typed);

            Assert.Equal(15f, state.Hunger);
            Assert.Same(state, legacy.Get("a_one"));
            Assert.Same(state, typed.TryGet(Id("a_one"), out var found) ? found : null);
        }
    }
}
