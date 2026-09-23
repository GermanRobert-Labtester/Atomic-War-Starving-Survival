// SPDX-License-Identifier: MIT
using System.Linq;
using Ashfall.Core.Records;
using Xunit;

namespace Ashfall.Core.Tests.Records
{
    /// <summary>
    /// Plan 55 / Task 55A — strict loader contract for the authored retention
    /// policy table. The data authority decides what a campaign may remember,
    /// so a malformed row must be rejected rather than silently defaulted.
    /// </summary>
    public class RetentionPolicyCatalogLoaderTests
    {
        private const string ValidTable = @"{
              ""schema_version"": 1,
              ""policies"": [
                { ""collection_key"": ""kitchen_serving_log"", ""max_capacity"": 200,
                  ""action"": ""keep_newest_k"", ""protected_obligation"": false },
                { ""collection_key"": ""machine_log"", ""max_capacity"": 300,
                  ""action"": ""keep_newest_k"", ""protected_obligation"": false },
                { ""collection_key"": ""faction_war_decrees"", ""max_capacity"": 100,
                  ""action"": ""drop_prose_keep_ids"", ""protected_obligation"": false },
                { ""collection_key"": ""dose_ledger"", ""max_capacity"": 500,
                  ""action"": ""rollup_summary"", ""protected_obligation"": false },
                { ""collection_key"": ""survivor_wills_and_legacies"", ""max_capacity"": 2147483647,
                  ""action"": ""keep_newest_k"", ""protected_obligation"": true }
              ]
            }";

        [Fact]
        public void LoadsTheRealAuthoredTable()
        {
            var result = LoadRealTable();
            Assert.False(result.HasErrors, string.Join("; ", result.Errors));
            Assert.NotEmpty(result.Policies);
            Assert.Contains(result.Policies, p => p.CollectionKey == "kitchen_serving_log" && p.MaxCapacity == 200);
            Assert.Contains(result.Policies, p => p.IsProtectedObligation);
        }

        [Fact]
        public void OverlayWinsForNamedKeysAndKeepsUnlistedDefaults()
        {
            var catalog = new RetentionPolicyCatalog();
            int before = catalog.ActivePolicies.Count;

            int applied = catalog.ApplyOverlay(new[]
            {
                new RetentionPolicyDefinition("kitchen_serving_log", 25, RetentionAction.KeepNewestK)
            });

            Assert.Equal(1, applied);
            Assert.True(catalog.TryGetPolicy("kitchen_serving_log", out var kitchen));
            Assert.Equal(25, kitchen!.MaxCapacity);
            // Every unlisted default bound survives, so a partial authored table
            // cannot silently remove a protective obligation policy.
            Assert.True(catalog.ActivePolicies.Count >= before);
            Assert.True(catalog.HasPolicy("survivor_wills_and_legacies"));
        }

        [Fact]
        public void ProtectedObligationsCanNeverBePruned()
        {
            var catalog = new RetentionPolicyCatalog();
            var obligations = Enumerable.Range(0, 500).Select(i => $"will_{i:D3}").ToList();

            bool applied = catalog.ApplyRetention("survivor_wills_and_legacies", obligations, out int pruned);

            Assert.True(applied);
            Assert.Equal(0, pruned);
            Assert.Equal(500, obligations.Count);
        }

        [Fact]
        public void BoundedCollectionTrimsOldestEntriesAndReportsTheSeam()
        {
            var catalog = new RetentionPolicyCatalog();
            string? seamKey = null;
            int seamAfter = -1;
            catalog.OnEntriesPrunedSeam = (key, _, after) => { seamKey = key; seamAfter = after; };

            var log = Enumerable.Range(0, 260).Select(i => (object)i).ToList();
            bool applied = catalog.ApplyRetention("kitchen_serving_log", log, out int pruned);

            Assert.True(applied);
            Assert.Equal(60, pruned);
            Assert.Equal(200, log.Count);
            Assert.Equal(60, log[0]);
            Assert.Equal("kitchen_serving_log", seamKey);
            Assert.Equal(200, seamAfter);
        }

        [Fact]
        public void LoaderRejectsUnknownAction()
        {
            var result = LoadFrom(ValidTable.Replace("\"keep_newest_k\"", "\"make_it_up\""));
            Assert.True(result.HasErrors);
            Assert.Contains(result.Errors, e => e.Contains("unknown action"));
            Assert.DoesNotContain(result.Policies, p => p.CollectionKey == "survivor_wills_and_legacies");
        }

        [Fact]
        public void LoaderRejectsProtectedObligationWithFiniteCap()
        {
            var result = LoadFrom(ValidTable.Replace(
                "\"max_capacity\": 2147483647,\n                  \"action\": \"keep_newest_k\", \"protected_obligation\": true",
                "\"max_capacity\": 12,\n                  \"action\": \"keep_newest_k\", \"protected_obligation\": true"));
            Assert.True(result.HasErrors);
            Assert.Contains(result.Errors, e => e.Contains("protected obligation"));
            // The bad row is rejected; the unaffected rows stay valid.
            Assert.DoesNotContain(result.Policies, p => p.CollectionKey == "survivor_wills_and_legacies");
        }

        [Fact]
        public void LoaderRejectsDuplicateKeysAndNonPositiveCapacity()
        {
            var dup = ValidTable.Replace(
                "{ \"collection_key\": \"machine_log\"",
                "{ \"collection_key\": \"kitchen_serving_log\"");
            var dupResult = LoadFrom(dup);
            Assert.True(dupResult.HasErrors);
            Assert.Contains(dupResult.Errors, e => e.Contains("duplicate collection_key"));

            var zero = ValidTable.Replace("\"max_capacity\": 200", "\"max_capacity\": 0");
            var zeroResult = LoadFrom(zero);
            Assert.True(zeroResult.HasErrors);
            Assert.Contains(zeroResult.Errors, e => e.Contains("max_capacity must be >= 1"));
        }

        [Fact]
        public void LoaderRejectsNonSnakeCaseKeys()
        {
            var result = LoadFrom(ValidTable.Replace("kitchen_serving_log", "KitchenServingLog"));
            Assert.True(result.HasErrors);
            Assert.Contains(result.Errors, e => e.Contains("snake_case"));
            Assert.DoesNotContain(result.Policies, p => p.CollectionKey == "KitchenServingLog");
        }

        private static RetentionPolicyCatalogLoadResult LoadFrom(string json)
        {
            var files = new InMemoryFiles(json);
            return RetentionPolicyCatalogLoader.Load("data", files);
        }

        private static RetentionPolicyCatalogLoadResult LoadRealTable()
        {
            // Walk up to the repository data authority so this test validates the
            // shipped file, not a copy.
            var dir = System.IO.Directory.GetCurrentDirectory();
            while (!string.IsNullOrEmpty(dir)
                && !System.IO.File.Exists(System.IO.Path.Combine(dir, "Assets", "StreamingAssets", "Data", "retention_policies.json")))
            {
                var parent = System.IO.Directory.GetParent(dir);
                dir = parent?.FullName;
            }

            if (string.IsNullOrEmpty(dir)) return LoadFrom(ValidTable);
            string dataDir = System.IO.Path.Combine(dir, "Assets", "StreamingAssets", "Data");
            return RetentionPolicyCatalogLoader.Load(dataDir, new Ashfall.Core.FileSystemIO());
        }

        private sealed class InMemoryFiles : Ashfall.Core.IFileIO
        {
            private readonly string _json;
            public InMemoryFiles(string json) => _json = json;

            public bool DirectoryExists(string path) => true;
            public bool FileExists(string path) => true;
            public string ReadAllText(string path) => _json;
            public void WriteAllText(string path, string contents) { }
            public string Combine(params string[] parts) => string.Join("/", parts);
        }
    }
}
