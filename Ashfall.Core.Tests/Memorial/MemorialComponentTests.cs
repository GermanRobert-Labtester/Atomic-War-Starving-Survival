// SPDX-License-Identifier: MIT
// Task #132 — Typed Memorial component, adapter, parity, and wire-contract coverage.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Ashfall.Core;
using Ashfall.Core.Memorial;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Memorial
{
    public class MemorialComponentTests
    {
        private static SurvivorId Id(string raw) => new SurvivorId(raw);

        private static MemorialRecord Record(
            string id,
            string cause = "radiation",
            int day = 40,
            int survivedDays = 39,
            bool finalWishResolved = true,
            string epitaph = "She walked into the grey.",
            string heirloomItemId = "wedding_ring",
            string heirloomRecipientId = "the_keeper",
            float moraleDelta = -8f)
            => new MemorialRecord(
                Id(id),
                cause,
                day,
                survivedDays,
                finalWishResolved,
                epitaph,
                heirloomItemId,
                heirloomRecipientId,
                moraleDelta);

        private static MemorialEntry Legacy(
            string id,
            string cause = "radiation",
            int day = 40,
            int survivedDays = 39,
            bool finalWishResolved = true,
            string epitaph = "She walked into the grey.",
            string heirloomItemId = "wedding_ring",
            string heirloomRecipientId = "the_keeper",
            float moraleDelta = -8f)
            => new MemorialEntry
            {
                SurvivorId = id,
                Cause = cause,
                Day = day,
                SurvivedDays = survivedDays,
                FinalWishResolved = finalWishResolved,
                Epitaph = epitaph,
                HeirloomItemId = heirloomItemId,
                HeirloomRecipientId = heirloomRecipientId,
                MoraleDelta = moraleDelta
            };

        private static void AssertRecordEqual(MemorialRecord expected, MemorialRecord actual)
        {
            Assert.Equal(expected.SurvivorId, actual.SurvivorId);
            Assert.Equal(expected.Cause, actual.Cause);
            Assert.Equal(expected.Day, actual.Day);
            Assert.Equal(expected.SurvivedDays, actual.SurvivedDays);
            Assert.Equal(expected.FinalWishResolved, actual.FinalWishResolved);
            Assert.Equal(expected.Epitaph, actual.Epitaph);
            Assert.Equal(expected.HeirloomItemId, actual.HeirloomItemId);
            Assert.Equal(expected.HeirloomRecipientId, actual.HeirloomRecipientId);
            Assert.Equal(expected.MoraleDelta, actual.MoraleDelta);
        }

        [Fact]
        public void Store_UsesExpectedMetadataAndEmptyHistoryLedger()
        {
            var store = new MemorialComponentStore();
            ISurvivorComponentStore component = store;

            Assert.Equal("memorial", component.ComponentName);
            Assert.Equal(SurvivorComponentCardinality.ZeroOrOne, component.Cardinality);
            Assert.True(component.RetainsHistoryAfterDeath);
            Assert.Equal(0, store.Count);
            Assert.Empty(store.OwnerIds);
            Assert.False(store.Contains(Id("the_absent")));
            Assert.False(store.Release(Id("the_absent")));
        }

        [Fact]
        public void Record_IsIdempotent_FirstRecordWinsWithoutEventsOrLifecycleAuthority()
        {
            var store = new MemorialComponentStore();
            var first = Record("a_first", cause: "combat");
            var later = Record("a_first", cause: "radiation", day: 99);

            Assert.Same(first, store.Record(first));
            Assert.Same(first, store.Record(later));
            Assert.False(store.TryRecord(later));
            Assert.Equal(1, store.Count);
            Assert.Same(first, store.TryGet(Id("a_first"), out var found) ? found : null);
        }

        [Fact]
        public void Store_CaptureIsOrdinalDetachedAndContainsEveryHistoricalField()
        {
            var store = new MemorialComponentStore();
            var source = Record(
                "z_last",
                cause: "industrial_fire",
                day: 73,
                survivedDays: -4,
                finalWishResolved: false,
                epitaph: "The lamp went out.",
                heirloomItemId: "field_radio",
                heirloomRecipientId: "a_first",
                moraleDelta: -12.5f);
            store.Record(source);
            store.Record(Record("a_first"));

            var captured = store.CaptureState();

            Assert.Equal(MemorialComponentStore.SchemaVersion, captured.schema_version);
            Assert.Equal(MemorialComponentStore.SystemId, captured.system_id);
            Assert.Equal(new[] { "a_first", "z_last" },
                captured.records.Select(row => row.survivor_id).ToArray());

            var row = captured.records[1];
            Assert.Equal("industrial_fire", row.cause);
            Assert.Equal(73, row.day);
            Assert.Equal(-4, row.survived_days);
            Assert.False(row.final_wish_resolved);
            Assert.Equal("The lamp went out.", row.epitaph);
            Assert.Equal("field_radio", row.heirloom_item_id);
            Assert.Equal("a_first", row.heirloom_recipient_id);
            Assert.Equal(-12.5f, row.morale_delta);

            row.cause = "mutated_after_capture";
            Assert.Equal("industrial_fire", source.Cause);
        }

        [Fact]
        public void DetachedState_RoundTripsAndPreservesWireShape()
        {
            var original = new MemorialComponentStore();
            var source = Record("the_surveyor", survivedDays: 123);
            original.Record(source);

            string json = JsonSerializer.Serialize(
                original.CaptureState(), SystemTextJsonSerializer.Options);
            var restoredState = JsonSerializer.Deserialize<MemorialComponentStoreState>(
                json, SystemTextJsonSerializer.Options);
            var restored = new MemorialComponentStore();
            var report = restored.RestoreState(restoredState);

            Assert.True(report.IsClean, report.ToString());
            Assert.Equal(1, report.Accepted);
            Assert.True(restored.TryGet(Id("the_surveyor"), out var state));
            Assert.NotNull(state);
            AssertRecordEqual(source, state!);
            Assert.Contains("schema_version", json);
            Assert.Contains("system_id", json);
            Assert.Contains("survivor_id", json);
            Assert.Contains("survived_days", json);
            Assert.Equal(
                json,
                JsonSerializer.Serialize(restored.CaptureState(), SystemTextJsonSerializer.Options));
        }

        [Fact]
        public void Restore_RejectsNullInvalidAndDuplicateRows_FirstRowWins()
        {
            var state = new MemorialComponentStoreState();
            state.records.Add(new MemorialRecordState
            {
                survivor_id = "the_good",
                cause = "first",
                survived_days = 12
            });
            state.records.Add(new MemorialRecordState { survivor_id = "The_Bad" });
            state.records.Add(new MemorialRecordState { survivor_id = string.Empty });
            state.records.Add(new MemorialRecordState
            {
                survivor_id = "the_good",
                cause = "second",
                survived_days = 99
            });
            state.records.Add(null!);

            var store = new MemorialComponentStore();
            var report = store.RestoreState(state);

            Assert.Equal(1, report.Accepted);
            Assert.Equal(4, report.Rejected.Count);
            Assert.True(store.TryGet(Id("the_good"), out var restored));
            Assert.Equal("first", restored!.Cause);
            Assert.Equal(12, restored.SurvivedDays);
            Assert.Contains(report.Rejected, row => row.Contains("uppercase", StringComparison.OrdinalIgnoreCase));
            Assert.Contains(report.Rejected, row => row.Contains("duplicate", StringComparison.Ordinal));
            Assert.Contains(report.Rejected, row => row.Contains("null entry", StringComparison.Ordinal));
        }

        [Fact]
        public void Restore_FutureSchemaAndWrongSystemPreserveCurrentState()
        {
            var store = new MemorialComponentStore();
            var current = Record("a_current", cause: "current");
            store.Record(current);

            var future = new MemorialComponentStoreState
            {
                schema_version = MemorialComponentStore.SchemaVersion + 1,
                system_id = MemorialComponentStore.SystemId
            };
            future.records.Add(new MemorialRecordState { survivor_id = "z_future" });
            var futureReport = store.RestoreState(future);

            Assert.True(futureReport.IsFatal);
            Assert.Contains("newer than this build", futureReport.FatalReason);
            Assert.Same(current, store.TryGet(Id("a_current"), out var afterFuture) ? afterFuture : null);
            Assert.False(store.Contains(Id("z_future")));

            var foreign = new MemorialComponentStoreState
            {
                schema_version = MemorialComponentStore.SchemaVersion,
                system_id = "not_memorial"
            };
            foreign.records.Add(new MemorialRecordState { survivor_id = "z_foreign" });
            var foreignReport = store.RestoreState(foreign);

            Assert.True(foreignReport.IsFatal);
            Assert.Contains("does not match", foreignReport.FatalReason);
            Assert.Same(current, store.TryGet(Id("a_current"), out var afterForeign) ? afterForeign : null);
            Assert.False(store.Contains(Id("z_foreign")));
        }

        [Fact]
        public void Restore_NullStateIsTheExplicitEmptyResetForm()
        {
            var store = new MemorialComponentStore();
            store.Record(Record("a_current"));

            var report = store.RestoreState(null);

            Assert.True(report.IsClean, report.ToString());
            Assert.Equal(0, store.Count);
            Assert.Empty(store.OwnerIds);
        }

        [Fact]
        public void ReleaseDoesNotEraseHistory_ButResetDoes()
        {
            var store = new MemorialComponentStore();
            store.Record(Record("a_one"));
            store.Record(Record("b_two"));

            Assert.False(store.Release(Id("a_one")));
            Assert.True(store.Contains(Id("a_one")));
            Assert.Equal(2, store.Count);

            store.Reset();
            Assert.Equal(0, store.Count);
            Assert.Empty(store.OwnerIds);
        }

        [Fact]
        public void Adapter_ImportsAllFieldsPreservesSurvivedDaysAndOrdersOwners()
        {
            var store = new MemorialComponentStore();
            var entries = new List<MemorialEntry>
            {
                Legacy(
                    "z_last",
                    cause: "industrial_fire",
                    day: 73,
                    survivedDays: -4,
                    finalWishResolved: false,
                    epitaph: "The lamp went out.",
                    heirloomItemId: "field_radio",
                    heirloomRecipientId: "a_first",
                    moraleDelta: -12.5f),
                Legacy("a_first")
            };

            var report = MemorialComponentAdapter.ImportLegacy(entries, store);

            Assert.True(report.IsClean, string.Join("\n", report.Rejected));
            Assert.Equal(2, report.Accepted);
            Assert.Equal(new[] { "a_first", "z_last" },
                store.OwnerIds.Select(id => id.Value).ToArray());
            Assert.True(store.TryGet(Id("z_last"), out var imported));
            Assert.NotNull(imported);
            Assert.Equal("industrial_fire", imported!.Cause);
            Assert.Equal(73, imported.Day);
            Assert.Equal(-4, imported.SurvivedDays);
            Assert.False(imported.FinalWishResolved);
            Assert.Equal("The lamp went out.", imported.Epitaph);
            Assert.Equal("field_radio", imported.HeirloomItemId);
            Assert.Equal("a_first", imported.HeirloomRecipientId);
            Assert.Equal(-12.5f, imported.MoraleDelta);
        }

        [Fact]
        public void Adapter_ReportsNullInvalidDuplicateUnknownAndLivingRows()
        {
            var entities = new SurvivorEntityStore();
            entities.TryJoin(Id("a_dead"), "a_dead", 1);
            entities.TryDie(Id("a_dead"), 4);
            entities.TryJoin(Id("b_living"), "b_living", 1);

            var entries = new List<MemorialEntry>
            {
                Legacy("a_dead", survivedDays: 11),
                Legacy("a_dead", survivedDays: 99),
                Legacy("b_living"),
                Legacy("z_unknown"),
                Legacy("The_Bad"),
                null!
            };
            var store = new MemorialComponentStore();

            var report = MemorialComponentAdapter.ImportLegacy(entries, store, entities);

            Assert.Equal(1, report.Accepted);
            Assert.Equal(5, report.Rejected.Count);
            Assert.Contains(report.Rejected, row => row.StartsWith(MemorialImportCode.DuplicateId, StringComparison.Ordinal));
            Assert.Contains(report.Rejected, row => row.StartsWith(MemorialImportCode.OwnerLiving, StringComparison.Ordinal));
            Assert.Contains(report.Rejected, row => row.StartsWith(MemorialImportCode.OwnerUnknown, StringComparison.Ordinal));
            Assert.Contains(report.Rejected, row => row.StartsWith(MemorialImportCode.LegacyIdInvalid, StringComparison.Ordinal));
            Assert.Contains(report.Rejected, row => row.StartsWith(MemorialImportCode.LegacyRowNull, StringComparison.Ordinal));
            Assert.True(store.Contains(Id("a_dead")));
            Assert.Equal(11, store.TryGet(Id("a_dead"), out var imported) ? imported!.SurvivedDays : -1);
        }

        [Fact]
        public void Adapter_MapsNullableLegacyStringsToTypedDefaults()
        {
            var entry = Legacy("a_null");
            entry.Cause = null!;
            entry.Epitaph = null!;
            entry.HeirloomItemId = null!;
            entry.HeirloomRecipientId = null!;
            var store = new MemorialComponentStore();

            var report = MemorialComponentAdapter.ImportLegacy(
                new[] { entry }, store);

            Assert.True(report.IsClean, string.Join("\n", report.Rejected));
            Assert.True(store.TryGet(Id("a_null"), out var imported));
            Assert.NotNull(imported);
            Assert.Equal(string.Empty, imported!.Cause);
            Assert.Equal(string.Empty, imported.Epitaph);
            Assert.Equal(string.Empty, imported.HeirloomItemId);
            Assert.Equal(string.Empty, imported.HeirloomRecipientId);
            Assert.Equal(4, MemorialComponentParity.Compare(new[] { entry }, store)
                .Findings.Count(finding => finding.Code == MemorialParityCode.LegacyFieldNull));
        }

        [Fact]
        public void Adapter_DoesNotMutateEntityLifecycleOrRevision()
        {
            var entities = new SurvivorEntityStore();
            entities.TryJoin(Id("a_dead"), "a_dead", 2);
            entities.TryDie(Id("a_dead"), 8);
            entities.TryMemorialize(Id("a_dead"), 9);
            Assert.True(entities.TryGet(Id("a_dead"), out var before));
            long revision = before!.Revision;
            var transitions = 0;
            entities.OnLifecycleChanged += _ => transitions++;

            var report = MemorialComponentAdapter.ImportLegacy(
                new[] { Legacy("a_dead") },
                new MemorialComponentStore(),
                entities);

            Assert.True(report.IsClean, string.Join("\n", report.Rejected));
            Assert.True(entities.TryGet(Id("a_dead"), out var after));
            Assert.Equal(SurvivorLifecycleState.Memorialized, after!.Lifecycle);
            Assert.Equal(revision, after.Revision);
            Assert.Equal(0, transitions);
        }

        [Fact]
        public void Parity_IsCleanForMatchingLegacyAndTypedRows()
        {
            var legacy = new List<MemorialEntry> { Legacy("a_one") };
            var typed = new MemorialComponentStore();
            typed.Record(Record("a_one"));

            var report = MemorialComponentParity.Compare(legacy, typed);

            Assert.True(report.IsMatch, report.Describe());
            Assert.Equal(1, report.LegacyRows);
            Assert.Equal(1, report.TypedRows);
        }

        [Fact]
        public void Parity_ReportsDuplicateMissingExtraAndStableOrdering()
        {
            var legacy = new List<MemorialEntry>
            {
                Legacy("z_duplicate", cause: "first"),
                Legacy("a_missing"),
                Legacy("z_duplicate", cause: "second")
            };
            var typed = new MemorialComponentStore();
            typed.Record(Record("a_missing"));
            typed.Record(Record("b_extra"));

            var report = MemorialComponentParity.Compare(legacy, typed);

            Assert.False(report.IsMatch);
            Assert.Contains(report.Findings, finding => finding.Code == MemorialParityCode.LegacyDuplicateId);
            Assert.Contains(report.Findings, finding => finding.Code == MemorialParityCode.TypedRecordExtra && finding.SurvivorId == Id("b_extra"));
            Assert.Contains(report.Findings, finding => finding.Code == MemorialParityCode.TypedRecordMissing && finding.SurvivorId == Id("z_duplicate"));

            var ordered = report.Findings
                .Select(finding => (finding.SurvivorId.Value, finding.RawId, finding.Code, finding.Field, finding.Message))
                .ToArray();
            var expected = ordered
                .OrderBy(item => item.Value, StringComparer.Ordinal)
                .ThenBy(item => item.RawId, StringComparer.Ordinal)
                .ThenBy(item => item.Code, StringComparer.Ordinal)
                .ThenBy(item => item.Field, StringComparer.Ordinal)
                .ThenBy(item => item.Message, StringComparer.Ordinal)
                .ToArray();
            Assert.Equal(expected, ordered);
        }

        [Fact]
        public void Parity_UsesLegacyFirstDuplicateForFieldComparison()
        {
            var first = Legacy("a_duplicate", cause: "first");
            var later = Legacy("a_duplicate", cause: "later");
            var typed = new MemorialComponentStore();
            typed.Record(Record("a_duplicate", cause: "first"));

            var report = MemorialComponentParity.Compare(
                new[] { first, later }, typed);

            Assert.Contains(report.Findings,
                finding => finding.Code == MemorialParityCode.LegacyDuplicateId);
            Assert.DoesNotContain(report.Findings,
                finding => finding.Code == MemorialParityCode.FieldMismatch);
        }

        [Fact]
        public void Parity_ReportsEveryHistoricalFieldMismatch()
        {
            var legacy = new List<MemorialEntry>
            {
                Legacy(
                    "a_one",
                    cause: "radiation",
                    day: 40,
                    survivedDays: 39,
                    finalWishResolved: true,
                    epitaph: "old",
                    heirloomItemId: "ring",
                    heirloomRecipientId: "recipient",
                    moraleDelta: -8f)
            };
            var typed = new MemorialComponentStore();
            typed.Record(Record(
                "a_one",
                cause: "combat",
                day: 41,
                survivedDays: 38,
                finalWishResolved: false,
                epitaph: "new",
                heirloomItemId: "radio",
                heirloomRecipientId: "other",
                moraleDelta: -7f));

            var fields = MemorialComponentParity.Compare(legacy, typed)
                .Findings
                .Where(finding => finding.Code == MemorialParityCode.FieldMismatch)
                .Select(finding => finding.Field)
                .ToHashSet(StringComparer.Ordinal);

            Assert.Equal(
                new[]
                {
                    "cause", "day", "epitaph", "final_wish_resolved",
                    "heirloom_item_id", "heirloom_recipient_id", "morale_delta", "survived_days"
                },
                fields.OrderBy(field => field, StringComparer.Ordinal).ToArray());
        }

        [Fact]
        public void Parity_ReportsNullLegacyFieldsAndMalformedIds()
        {
            var malformed = new MemorialEntry
            {
                SurvivorId = "The_Bad",
                Cause = null!,
                Epitaph = null!,
                HeirloomItemId = null!,
                HeirloomRecipientId = null!
            };
            var nullFields = Legacy("a_null");
            nullFields.Cause = null!;
            nullFields.Epitaph = null!;
            nullFields.HeirloomItemId = null!;
            nullFields.HeirloomRecipientId = null!;

            var typed = new MemorialComponentStore();
            typed.Record(new MemorialRecord(
                Id("a_null"), null, 40, 39, true, null, null, null, -8f));

            var report = MemorialComponentParity.Compare(
                new List<MemorialEntry> { malformed, nullFields }, typed);

            Assert.Contains(report.Findings, finding => finding.Code == MemorialParityCode.LegacyIdInvalid);
            var nullFindings = report.Findings
                .Where(finding => finding.Code == MemorialParityCode.LegacyFieldNull)
                .Select(finding => finding.Field)
                .ToHashSet(StringComparer.Ordinal);
            Assert.Equal(
                new[] { "cause", "epitaph", "heirloom_item_id", "heirloom_recipient_id" },
                nullFindings);
            Assert.DoesNotContain(report.Findings,
                finding => finding.SurvivorId == Id("a_null") &&
                           finding.Code == MemorialParityCode.FieldMismatch);
        }

        [Fact]
        public void Parity_IsDeterministicRegardlessOfLegacyRegistrationOrder()
        {
            var firstOrder = new List<MemorialEntry>
            {
                Legacy("z_legacy"),
                Legacy("a_legacy")
            };
            var secondOrder = new List<MemorialEntry>
            {
                Legacy("a_legacy"),
                Legacy("z_legacy")
            };
            var typed = new MemorialComponentStore();
            typed.Record(Record("b_extra"));

            var first = MemorialComponentParity.Compare(firstOrder, typed);
            var second = MemorialComponentParity.Compare(secondOrder, typed);

            Assert.Equal(
                first.Findings.Select(finding => finding.ToString()).ToArray(),
                second.Findings.Select(finding => finding.ToString()).ToArray());
        }

        [Fact]
        public void TypedStore_IntegratesWithReferentialIntegrityWithoutRejectingHistory()
        {
            var entities = new SurvivorEntityStore();
            entities.TryJoin(Id("a_dead"), "a_dead", 1);
            entities.TryDie(Id("a_dead"), 2);
            entities.TryMemorialize(Id("a_dead"), 3);

            var memorial = new MemorialComponentStore();
            memorial.Record(Record("a_dead"));
            memorial.Record(Record("z_unknown"));
            entities.RegisterComponentStore(memorial);

            var report = SurvivorIntegrityValidator.Validate(entities);

            var ownerUnknown = Assert.Single(
                report.Findings,
                finding => finding.Code == SurvivorIntegrityCode.ComponentOwnerUnknown);
            Assert.Equal(Id("z_unknown"), ownerUnknown.SurvivorId);
            Assert.DoesNotContain(report.Findings,
                finding => finding.Code == SurvivorIntegrityCode.ComponentOnDeceased);
        }

        [Fact]
        public void RetainedHistorySurvivesLivingOwnerRemoval()
        {
            var entities = new SurvivorEntityStore();
            entities.TryJoin(Id("a_departing"), "a_departing", 1);
            var memorial = new MemorialComponentStore();
            memorial.Record(Record("a_departing"));
            entities.RegisterComponentStore(memorial);

            var result = entities.TryLeave(Id("a_departing"), 5);

            Assert.True(result.IsCommitted);
            Assert.False(entities.Contains(Id("a_departing")));
            Assert.True(memorial.Contains(Id("a_departing")));
        }

        [Fact]
        public void MemorialSave_CoreWireFieldsAndChecksumRemainDirectV1()
        {
            var save = new MemorialSave
            {
                saveVersion = MemorialSave.CurrentSaveVersion,
                simDay = 44,
                State = new MemorialState
                {
                    Entries = new List<MemorialEntry> { Legacy("a_one", survivedDays: 43) }
                }
            };
            save.Checksum = SaveChecksum.Compute(save);
            var json = new SystemTextJsonSerializer();
            string raw = json.Serialize(save);
            var restored = json.Deserialize<MemorialSave>(raw);

            Assert.NotNull(restored);
            Assert.Equal(1, restored!.saveVersion);
            Assert.Equal(44, restored.simDay);
            Assert.Equal(save.Checksum, restored.Checksum);
            Assert.Equal(save.Checksum, SaveChecksum.Compute(restored));
            Assert.Contains("saveVersion", raw);
            Assert.Contains("simDay", raw);
            Assert.Contains("State", raw);
            Assert.Contains("Entries", raw);
            Assert.Contains("SurvivorId", raw);
            Assert.Contains("SurvivedDays", raw);
            Assert.Contains("Checksum", raw);
        }
    }
}
