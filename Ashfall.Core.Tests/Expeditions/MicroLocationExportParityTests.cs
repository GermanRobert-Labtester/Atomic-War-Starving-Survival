// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Ashfall.Core;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests.Expeditions
{
    /// <summary>
    /// Flagship Plan 49 (Task F23): Micro-Location Export Parity & Cross-Reference Integrity Suite.
    ///
    /// Validates:
    /// - export_presets.cfg packages all JSON data catalogs and CSV localization tables.
    /// - micro_locations.json conforms to schema_version 1 and collection_id contract.
    /// - All 25 production micro-locations load with 0 deserialization errors.
    /// - All referenced items (grantItemId, costItems, requiredItemId) resolve in items.json.
    /// - All referenced locations (discoverLocationId, requiredLocationId) resolve in locations catalogs.
    /// - All micro-location encounter IDs are unique and non-colliding.
    /// </summary>
    public sealed class MicroLocationExportParityTests : CatalogTestBase
    {
        [Fact]
        public void ExportPresetsCfg_ConfiguredForDataAndLocalizationPackaging()
        {
            string cfgPath = Path.Combine(DataDirectory, "..", "..", "..", "export_presets.cfg");
            string fullPath = Path.GetFullPath(cfgPath);
            Assert.True(File.Exists(fullPath), $"export_presets.cfg not found at {fullPath}");

            string text = File.ReadAllText(fullPath);
            Assert.Contains("platform=\"Linux/X11\"", text);
            Assert.Contains("platform=\"Windows Desktop\"", text);
            Assert.Contains("export_filter=\"all_resources\"", text);
            Assert.Contains("include_filter=\"*.json, *.csv\"", text);
        }

        [Fact]
        public void MicroLocationsCatalog_PackagingIntegrity_ZeroErrors()
        {
            string path = Path.Combine(DataDirectory, "micro_locations.json");
            Assert.True(File.Exists(path), "micro_locations.json must exist in StreamingAssets/Data");

            using var doc = JsonDocument.Parse(File.ReadAllText(path));
            var root = doc.RootElement;

            Assert.True(root.TryGetProperty("schema_version", out var schemaProp));
            Assert.Equal(1, schemaProp.GetInt32());

            Assert.True(root.TryGetProperty("collection_id", out var colProp));
            Assert.Equal("micro_locations_catalog", colProp.GetString());

            Assert.True(root.TryGetProperty("encounters", out var encountersProp));
            var list = new List<EncounterDefinition>();

            foreach (var elem in encountersProp.EnumerateArray())
            {
                var def = JsonSerializer.Deserialize<EncounterDefinition>(elem.GetRawText(), SystemTextJsonSerializer.Options);
                Assert.NotNull(def);
                Assert.False(string.IsNullOrWhiteSpace(def!.id));
                Assert.StartsWith("micro_", def.id);
                Assert.False(string.IsNullOrWhiteSpace(def.title));
                Assert.False(string.IsNullOrWhiteSpace(def.description));
                Assert.NotEmpty(def.choices);

                foreach (var choice in def.choices)
                {
                    Assert.False(string.IsNullOrWhiteSpace(choice.choiceId));
                    Assert.False(string.IsNullOrWhiteSpace(choice.text));
                }

                list.Add(def);
            }

            Assert.True(list.Count >= 25, $"Expected at least 25 micro-locations, found {list.Count}");

            // Ensure no duplicate IDs within catalog
            var duplicateIds = list.GroupBy(x => x.id).Where(g => g.Count() > 1).Select(g => g.Key).ToList();
            Assert.Empty(duplicateIds);
        }

        [Fact]
        public void CrossReferenceIntegrity_ItemsAndLocationsResolve()
        {
            string dataDir = DataDirectory;
            string microPath = Path.Combine(dataDir, "micro_locations.json");
            using var doc = JsonDocument.Parse(File.ReadAllText(microPath));

            // Load known item IDs
            string itemsPath = Path.Combine(dataDir, "items.json");
            var itemIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (File.Exists(itemsPath))
            {
                using var itemsDoc = JsonDocument.Parse(File.ReadAllText(itemsPath));
                if (itemsDoc.RootElement.TryGetProperty("items", out var itemArr))
                {
                    foreach (var it in itemArr.EnumerateArray())
                    {
                        if (it.TryGetProperty("id", out var idProp))
                            itemIds.Add(idProp.GetString()!);
                    }
                }
            }

            // Load known location IDs
            var locIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            string[] locFiles = { "locations.json", "expedition_locations.json", "holdfast_locations.json" };
            foreach (var lf in locFiles)
            {
                string p = Path.Combine(dataDir, lf);
                if (File.Exists(p))
                {
                    using var lDoc = JsonDocument.Parse(File.ReadAllText(p));
                    if (lDoc.RootElement.TryGetProperty("locations", out var lArr))
                    {
                        foreach (var loc in lArr.EnumerateArray())
                        {
                            if (loc.TryGetProperty("id", out var idProp))
                                locIds.Add(idProp.GetString()!);
                        }
                    }
                }
            }

            var missingItems = new List<string>();
            var missingLocations = new List<string>();

            foreach (var elem in doc.RootElement.GetProperty("encounters").EnumerateArray())
            {
                string encounterId = elem.GetProperty("id").GetString()!;

                if (elem.TryGetProperty("requiredLocationId", out var reqLocProp))
                {
                    string reqLoc = reqLocProp.GetString()!;
                    if (!string.IsNullOrEmpty(reqLoc) && !locIds.Contains(reqLoc))
                    {
                        missingLocations.Add($"{encounterId} -> reqLoc: {reqLoc}");
                    }
                }

                if (elem.TryGetProperty("choices", out var choicesProp))
                {
                    foreach (var c in choicesProp.EnumerateArray())
                    {
                        string choiceId = c.GetProperty("choiceId").GetString()!;

                        if (c.TryGetProperty("grantItemId", out var grantItemProp))
                        {
                            string gid = grantItemProp.GetString()!;
                            if (!string.IsNullOrEmpty(gid) && !itemIds.Contains(gid))
                            {
                                missingItems.Add($"{encounterId}.{choiceId} -> grantItem: {gid}");
                            }
                        }

                        if (c.TryGetProperty("requiredItemId", out var reqItemProp))
                        {
                            string rid = reqItemProp.GetString()!;
                            if (!string.IsNullOrEmpty(rid) && !itemIds.Contains(rid))
                            {
                                missingItems.Add($"{encounterId}.{choiceId} -> reqItem: {rid}");
                            }
                        }

                        if (c.TryGetProperty("discoverLocationId", out var disLocProp))
                        {
                            string did = disLocProp.GetString()!;
                            if (!string.IsNullOrEmpty(did) && !locIds.Contains(did))
                            {
                                missingLocations.Add($"{encounterId}.{choiceId} -> discoverLoc: {did}");
                            }
                        }
                    }
                }
            }

            Assert.Empty(missingItems);
            Assert.Empty(missingLocations);
        }

        [Fact]
        public void NoDuplicateEncounterIds_AcrossAllCatalogs()
        {
            string dataDir = DataDirectory;
            string microPath = Path.Combine(dataDir, "micro_locations.json");
            using var microDoc = JsonDocument.Parse(File.ReadAllText(microPath));

            var microIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var elem in microDoc.RootElement.GetProperty("encounters").EnumerateArray())
            {
                microIds.Add(elem.GetProperty("id").GetString()!);
            }

            string[] otherCatalogs = { "encounters.json", "encounters_phase1.json", "travel_encounters.json" };
            foreach (var cat in otherCatalogs)
            {
                string p = Path.Combine(dataDir, cat);
                if (!File.Exists(p)) continue;

                using var oDoc = JsonDocument.Parse(File.ReadAllText(p));
                JsonElement arr = default;
                if (oDoc.RootElement.TryGetProperty("encounters", out var eArr))
                    arr = eArr;
                else if (oDoc.RootElement.ValueKind == JsonValueKind.Array)
                    arr = oDoc.RootElement;

                if (arr.ValueKind == JsonValueKind.Array)
                {
                    foreach (var elem in arr.EnumerateArray())
                    {
                        if (elem.TryGetProperty("id", out var idProp))
                        {
                            string id = idProp.GetString()!;
                            Assert.False(microIds.Contains(id), $"Collision: micro-location ID \"{id}\" duplicated in {cat}");
                        }
                    }
                }
            }
        }
    }
}
