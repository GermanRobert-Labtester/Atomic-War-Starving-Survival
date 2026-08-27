using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Ashfall.Core.Tests.Data
{
    public class JsonSchemaPolicyGateTests
    {
        private static readonly string DataDir = Path.Combine("Assets", "StreamingAssets", "Data");

        [Fact]
        public void AllAuthoritativeCatalogs_MustDeclareSchemaVersion()
        {
            // Find repository root
            string current = Directory.GetCurrentDirectory();
            string dataPath = Path.Combine(current, DataDir);
            if (!Directory.Exists(dataPath))
            {
                // Traverse up to find repo root
                var parent = Directory.GetParent(current);
                while (parent != null && !Directory.Exists(Path.Combine(parent.FullName, DataDir)))
                {
                    parent = parent.Parent;
                }
                if (parent != null)
                {
                    dataPath = Path.Combine(parent.FullName, DataDir);
                }
            }

            Assert.True(Directory.Exists(dataPath), $"Authoritative data directory not found at: {dataPath}");

            var jsonFiles = Directory.GetFiles(dataPath, "*.json", SearchOption.AllDirectories);
            Assert.True(jsonFiles.Length >= 400, $"Expected at least 400 JSON files, found {jsonFiles.Length}");

            var violations = new List<string>();
            int validatedCount = 0;

            foreach (var file in jsonFiles)
            {
                string relPath = Path.GetRelativePath(dataPath, file);
                string text = File.ReadAllText(file);

                try
                {
                    using var doc = JsonDocument.Parse(text);
                    if (doc.RootElement.ValueKind != JsonValueKind.Object)
                    {
                        violations.Add($"{relPath}: Root is {doc.RootElement.ValueKind}, expected Object");
                        continue;
                    }

                    if (!doc.RootElement.TryGetProperty("schema_version", out var svProp))
                    {
                        violations.Add($"{relPath}: Missing 'schema_version' property");
                        continue;
                    }

                    if (svProp.ValueKind != JsonValueKind.Number || !svProp.TryGetInt32(out int version) || version < 1)
                    {
                        violations.Add($"{relPath}: 'schema_version' must be an integer >= 1 (was {svProp})");
                        continue;
                    }

                    validatedCount++;
                }
                catch (JsonException ex)
                {
                    violations.Add($"{relPath}: JSON parse error: {ex.Message}");
                }
            }

            Assert.Empty(violations);
            Assert.Equal(jsonFiles.Length, validatedCount);
        }

        [Theory]
        [InlineData("[]", false, "array root")]
        [InlineData("{\"items\": []}", false, "missing schema_version")]
        [InlineData("{\"schema_version\": \"1\"}", false, "string schema_version")]
        [InlineData("{\"schema_version\": 0}", false, "zero schema_version")]
        [InlineData("{\"schema_version\": -1}", false, "negative schema_version")]
        [InlineData("{\"schema_version\": 1, \"items\": []}", true, "valid v1")]
        [InlineData("{\"schema_version\": 2, \"id\": \"item_test\"}", true, "valid v2")]
        public void ValidateJsonPayload_ConformsToSchemaPolicy(string json, bool shouldPass, string description)
        {
            bool isValid = false;
            try
            {
                using var doc = JsonDocument.Parse(json);
                if (doc.RootElement.ValueKind == JsonValueKind.Object &&
                    doc.RootElement.TryGetProperty("schema_version", out var sv) &&
                    sv.ValueKind == JsonValueKind.Number &&
                    sv.TryGetInt32(out int ver) &&
                    ver >= 1)
                {
                    isValid = true;
                }
            }
            catch
            {
                isValid = false;
            }

            Assert.True(isValid == shouldPass, $"Test case failed: {description}");
        }
    }
}
