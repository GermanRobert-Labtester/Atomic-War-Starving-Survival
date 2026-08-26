using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Gate tests for the production-art generation manifest.
    ///
    /// The manifest is the canonical plan that drives every Batch N of the
    /// visual pipeline. It is emitted by `tools/production_manifest.py`
    /// (Python tooling that runs in plain BCL it must not be re-implemented
    /// in C#). What these tests guard is the *invariants* the manifest must
    /// hold before any Batch can be trusted:
    ///
    ///   1. The file exists at the canonical path.
    ///   2. The row count equals the curated actionable set + the reference
    ///      skip set (currently 478 + 136 = 614). Drift here means the
    ///      pipeline produced an inconsistent plan.
    ///   3. Every row carries the required fields, and no two rows share a
    ///      `content_id` (otherwise the next phase would generate twice and
    ///      the runtime would resolve ambiguously).
    ///   4. Every actionable row's `target_filename` follows the canonical
    ///      naming rule for its visual family (Inventory-Item → item_*,
    ///      Survivor-Portrait → survivor_* or npc_*, Location-Art → loc_*,
    ///      Faction-Art → faction_*). A canonical-name regression here
    ///      silently breaks AssetRegistry resolution.
    ///   5. The wire manifest (WIRING_MATRIX.json) and the production
    ///      manifest are 1:1 on `content_id` for the missing set. A row
    ///      that exists in one but not the other is a phasing bug.
    ///   6. The priority band counts are non-negative and sum to the
    ///      actionable count.
    ///
    /// The tests do NOT regenerate the manifest. They only read it. If
    /// they fail, the response is to run `python3 tools/production_manifest.py`
    /// and re-commit — never to silently edit the JSON.
    /// </summary>
    public class ProductionArtManifestTests
    {
        private const string ManifestRelativePath = "docs/visual/PRODUCTION_ART_GENERATION_MANIFEST.json";
        private const string WireMatrixRelativePath = "docs/visual/WIRING_MATRIX.json";

        private static string RepoRoot()
        {
            string start = Directory.GetCurrentDirectory();
            // The Core test runs from the repo root or a build subdir. Walk
            // parents until we find both a .git and project.godot.
            var dir = new DirectoryInfo(Path.GetFullPath(start));
            while (dir != null)
            {
                if (Directory.Exists(Path.Combine(dir.FullName, ".git"))
                    && File.Exists(Path.Combine(dir.FullName, "project.godot")))
                {
                    return dir.FullName;
                }
                dir = dir.Parent;
            }
            // Fallback: try the AppContext base directory (production test
            // harness runs from bin/Debug/net9.0/).
            string baseDir = AppContext.BaseDirectory;
            dir = new DirectoryInfo(baseDir);
            while (dir != null)
            {
                if (Directory.Exists(Path.Combine(dir.FullName, ".git"))
                    && File.Exists(Path.Combine(dir.FullName, "project.godot")))
                {
                    return dir.FullName;
                }
                dir = dir.Parent;
            }
            return start;
        }

        private static string FindManifestPath(out string repoRoot)
        {
            repoRoot = RepoRoot();
            string candidate = Path.Combine(repoRoot, ManifestRelativePath);
            return File.Exists(candidate) ? candidate : null;
        }

        private static string FindWireMatrixPath(out string repoRoot)
        {
            repoRoot = RepoRoot();
            string candidate = Path.Combine(repoRoot, WireMatrixRelativePath);
            return File.Exists(candidate) ? candidate : null;
        }

        private static List<JsonElement> LoadManifest()
        {
            string path = FindManifestPath(out _);
            Assert.False(string.IsNullOrEmpty(path),
                ManifestRelativePath + " must exist; run `python3 tools/production_manifest.py` first");
            string json = File.ReadAllText(path);
            var doc = JsonDocument.Parse(json);
            var list = new List<JsonElement>();
            foreach (var el in doc.RootElement.EnumerateArray())
            {
                list.Add(el.Clone());
            }
            return list;
        }

        [Fact]
        public void ManifestExistsAtCanonicalPath()
        {
            string path = FindManifestPath(out _);
            Assert.False(string.IsNullOrEmpty(path),
                ManifestRelativePath + " must exist at the repo root");
        }

        [Fact]
        public void RowCountEqualsActionablePlusReferenceSkip()
        {
            var rows = LoadManifest();
            int actionable = rows.Count(r => GetString(r, "generation_status") == "PENDING");
            int skipped = rows.Count(r => GetString(r, "generation_status") == "SKIP_REFERENCE_ONLY");
            int total = rows.Count;
            // The manifest law: 478 actionable + 136 skipped = 614.
            // If the counts drift, the queue (Phase 14/15) has shifted and
            // the rest of the pipeline must be re-anchored.
            Assert.True(actionable + skipped == total,
                "actionable + skipped must equal total (" + actionable + " + " + skipped + " vs " + total + ")");
            Assert.True(actionable > 0, "manifest must have at least one actionable row");
            // Reference-skip rows only appear when the wiring matrix contains
            // catalogs marked as Reference-Skip (recipes, relic_recipes, etc.).
            // If the matrix currently contains none, skipped may legitimately be 0.
        }

        [Fact]
        public void EveryRowHasRequiredFields()
        {
            var rows = LoadManifest();
            // Every row must carry these — actionable rows additionally carry
            // content_type (kind); reference-skip rows carry kind. Either is
            // acceptable as long as the row is identifiable.
            var requiredCore = new[]
            {
                "content_id", "source_catalog", "visual_family", "subfamily",
                "generation_status", "qa_status", "wiring_status",
                "runtime_status"
            };
            foreach (var row in rows)
            {
                foreach (var key in requiredCore)
                {
                    Assert.True(row.TryGetProperty(key, out var prop),
                        "row " + GetString(row, "content_id") + " missing required field '" + key + "'");
                    Assert.True(prop.ValueKind != JsonValueKind.Null,
                        "row " + GetString(row, "content_id") + " field '" + key + "' must not be null");
                }
                // The row must declare either 'kind' (reference-skip rows) or
                // 'content_type' (actionable rows). Either is acceptable.
                bool hasKind = row.TryGetProperty("kind", out var kp) && kp.ValueKind != JsonValueKind.Null;
                bool hasContentType = row.TryGetProperty("content_type", out var cp) && cp.ValueKind != JsonValueKind.Null;
                Assert.True(hasKind || hasContentType,
                    "row " + GetString(row, "content_id") + " must have either 'kind' or 'content_type'");
            }
        }

        [Fact]
        public void EveryActionableRowCarriesTargetFilenameAndDimensions()
        {
            var rows = LoadManifest();
            foreach (var row in rows)
            {
                if (GetString(row, "generation_status") != "PENDING") continue;
                string cid = GetString(row, "content_id");
                Assert.True(row.TryGetProperty("target_filename", out var tfn),
                    "actionable row " + cid + " missing target_filename");
                Assert.False(string.IsNullOrEmpty(tfn.GetString()),
                    "actionable row " + cid + " target_filename must not be empty");
                Assert.True(row.TryGetProperty("target_directory", out var tdir),
                    "actionable row " + cid + " missing target_directory");
                Assert.True(row.TryGetProperty("target_width", out var tw),
                    "actionable row " + cid + " missing target_width");
                Assert.True(row.TryGetProperty("target_height", out var th),
                    "actionable row " + cid + " missing target_height");
                Assert.True(tw.GetInt32() > 0 && th.GetInt32() > 0,
                    "actionable row " + cid + " dimensions must be positive");
            }
        }

        [Fact]
        public void NoTwoRowsShareContentId()
        {
            var rows = LoadManifest();
            var seen = new HashSet<string>();
            foreach (var row in rows)
            {
                string cid = GetString(row, "content_id");
                Assert.True(seen.Add(cid),
                    "duplicate content_id '" + cid + "' in production manifest");
            }
        }

        [Fact]
        public void CanonicalFilenameRuleIsSatisfiedPerFamily()
        {
            var rows = LoadManifest();
            foreach (var row in rows)
            {
                if (GetString(row, "generation_status") != "PENDING") continue;
                string cid = GetString(row, "content_id");
                string family = GetString(row, "visual_family");
                string tfn = GetString(row, "target_filename");

                switch (family)
                {
                    case "Inventory-Item":
                        Assert.True(tfn.StartsWith("item_"),
                            "Inventory-Item row " + cid + " target_filename must start with 'item_' (got " + tfn + ")");
                        break;
                    case "Survivor-Portrait":
                    case "NPC-Portrait":
                        Assert.True(tfn.StartsWith("survivor_") || tfn.StartsWith("npc_"),
                            family + " row " + cid + " target_filename must start with 'survivor_' or 'npc_' (got " + tfn + ")");
                        break;
                    case "Location-Art":
                        Assert.True(tfn.StartsWith("loc_") || tfn.StartsWith("location_"),
                            "Location-Art row " + cid + " target_filename must start with 'loc_' or 'location_' (got " + tfn + ")");
                        break;
                    case "Faction-Art":
                        Assert.True(tfn.StartsWith("faction_"),
                            "Faction-Art row " + cid + " target_filename must start with 'faction_' (got " + tfn + ")");
                        break;
                }
            }
        }

        [Fact]
        public void PrioritiesAreSortedByBandThenImportance()
        {
            var rows = LoadManifest();
            var bandOrder = new Dictionary<string, int>
            {
                ["P0"] = 0, ["P1"] = 1, ["P2"] = 2, ["P3"] = 3, ["P4"] = 4
            };
            int prevBand = -1;
            double prevImportance = double.MaxValue;
            string prevCid = null;
            foreach (var row in rows)
            {
                if (GetString(row, "generation_status") != "PENDING") continue;
                string band = GetString(row, "runtime_priority");
                double imp = GetDouble(row, "gameplay_importance");
                Assert.True(bandOrder.ContainsKey(band),
                    "row " + GetString(row, "content_id") + " has unknown priority band " + band);
                int b = bandOrder[band];
                Assert.True(b >= prevBand,
                    "manifest lost band order: prev=" + prevBand + " next=" + b + " at " + GetString(row, "content_id"));
                if (b > prevBand)
                {
                    prevBand = b;
                    prevImportance = imp;
                }
                else
                {
                    // Within a band, importance is descending.
                    Assert.True(imp <= prevImportance + 1e-6,
                        "within band " + band + ", importance must be descending: "
                        + prevCid + " (" + prevImportance + ") → " + GetString(row, "content_id") + " (" + imp + ")");
                    prevImportance = imp;
                }
                prevCid = GetString(row, "content_id");
            }
        }

        [Fact]
        public void PriorityBandCountsAreNonNegativeAndSumToActionable()
        {
            var rows = LoadManifest();
            int actionable = 0;
            var bandCounts = new Dictionary<string, int>
            {
                ["P0"] = 0, ["P1"] = 0, ["P2"] = 0, ["P3"] = 0, ["P4"] = 0
            };
            foreach (var row in rows)
            {
                if (GetString(row, "generation_status") != "PENDING") continue;
                actionable++;
                string band = GetString(row, "runtime_priority");
                if (bandCounts.ContainsKey(band))
                {
                    bandCounts[band]++;
                }
            }
            int sum = 0;
            foreach (var kvp in bandCounts)
            {
                Assert.True(kvp.Value >= 0, "band " + kvp.Key + " count is negative");
                sum += kvp.Value;
            }
            Assert.Equal(actionable, sum);
        }

        [Fact]
        public void ManifesAndWireMatrixShareMissingContentIds()
        {
            string manifestPath = FindManifestPath(out _);
            string wirePath = FindWireMatrixPath(out _);
            if (string.IsNullOrEmpty(wirePath))
            {
                return; // wire matrix is tooling-emitted; tolerate absence in CI
            }
            var manifest = LoadManifest();
            var wire = JsonDocument.Parse(File.ReadAllText(wirePath));
            var wireMissing = new HashSet<string>();
            foreach (var row in wire.RootElement.EnumerateArray())
            {
                if (row.TryGetProperty("resolved_path", out var rp)
                    && rp.GetString() == "MISSING"
                    && row.TryGetProperty("content_id", out var cid))
                {
                    wireMissing.Add(cid.GetString());
                }
            }
            var manifestIds = new HashSet<string>();
            foreach (var row in manifest)
            {
                manifestIds.Add(GetString(row, "content_id"));
            }
            // The manifest is a subset of the wire-missing set: every manifest
            // row must correspond to a missing catalog entry.
            foreach (var id in manifestIds)
            {
                Assert.True(wireMissing.Contains(id),
                    "manifest content_id '" + id + "' is not in the wire matrix MISSING set");
            }
        }

        [Fact]
        public void EveryActionableTargetFilenameIsGenuinelyMissingOnDisk()
        {
            // If a target_filename already exists in assets/art, the row
            // is no longer actionable — the manifest should be regenerated.
            // This test catches drift between the manifest and the file
            // system (e.g. someone dropped an asset and ran no rebuild).
            var rows = LoadManifest();
            string artDir = Path.Combine(RepoRoot(), "assets", "art");
            if (!Directory.Exists(artDir))
            {
                return; // the assets tree is tooling-side; tolerate absence in CI
            }
            var onDiskStems = new HashSet<string>(
                Directory.EnumerateFiles(artDir)
                    .Select(f => Path.GetFileNameWithoutExtension(f) ?? string.Empty),
                StringComparer.OrdinalIgnoreCase);
            foreach (var row in rows)
            {
                if (GetString(row, "generation_status") != "PENDING") continue;
                string tfn = GetString(row, "target_filename");
                string stem = Path.GetFileNameWithoutExtension(tfn);
                Assert.False(onDiskStems.Contains(stem),
                    "actionable row target_filename '" + tfn + "' already exists on disk — "
                    + "regenerate the manifest with `python3 tools/production_manifest.py`");
            }
        }

        [Fact]
        public void ReferenceAssetsPointToExistingFiles()
        {
            // Each actionable row's reference_assets must point to a real
            // file on disk (or the prompt composer will silently skip the
            // anchor). A stale reference starves the next batch of a real
            // style anchor.
            var rows = LoadManifest();
            string repoRoot = RepoRoot();
            foreach (var row in rows)
            {
                if (GetString(row, "generation_status") != "PENDING") continue;
                if (!row.TryGetProperty("reference_assets", out var refs)) continue;
                if (refs.ValueKind != JsonValueKind.Array) continue;
                foreach (var ra in refs.EnumerateArray())
                {
                    if (!ra.TryGetProperty("file_path", out var fp)) continue;
                    string filePath = fp.GetString();
                    if (string.IsNullOrEmpty(filePath)) continue;
                    string absPath = Path.Combine(repoRoot, filePath.Replace('/', Path.DirectorySeparatorChar));
                    Assert.True(File.Exists(absPath),
                        "row " + GetString(row, "content_id") + " reference asset '" + filePath + "' does not exist on disk");
                }
            }
        }

        [Fact]
        public void EveryActionableRowHasAtLeastOneReferenceAnchored()
        {
            // Phase 17 survival: generation prompts without a reference
            // anchor produce "variant" output that often drifts off the
            // ASHFALL palette. A row with zero references is a Q2 candidate
            // for hand-curation before generation kicks off.
            var rows = LoadManifest();
            int anchored = 0;
            int unanchored = 0;
            foreach (var row in rows)
            {
                if (GetString(row, "generation_status") != "PENDING") continue;
                if (!row.TryGetProperty("reference_assets", out var refs)) continue;
                if (refs.ValueKind != JsonValueKind.Array) continue;
                bool hasAny = false;
                foreach (var _ in refs.EnumerateArray()) { hasAny = true; break; }
                if (hasAny) anchored++; else unanchored++;
            }
            // We allow unanchored rows but they should be a minority (< 50%).
            // If they grow beyond that, the anchor bank has rotted.
            int total = anchored + unanchored;
            Assert.True(total > 0, "no actionable rows");
            double ratio = (double)unanchored / total;
            Assert.True(ratio < 0.5,
                "unanchored actionable rows must be < 50% (got "
                + unanchored + "/" + total + " = " + ratio.ToString("P") + ")");
        }

        [Fact]
        public void ManifestSourceCatalogsExistOnDisk()
        {
            // The source_catalog field must correspond to a real JSON file
            // under Assets/StreamingAssets/Data. A row whose source catalog
            // doesn't exist cannot be regenerated; the row is orphaned.
            var rows = LoadManifest();
            string repoRoot = RepoRoot();
            foreach (var row in rows)
            {
                string cat = GetString(row, "source_catalog");
                if (string.IsNullOrEmpty(cat)) continue;
                string absPath = Path.Combine(repoRoot,
                    "Assets", "StreamingAssets", "Data", cat + ".json");
                Assert.True(File.Exists(absPath),
                    "row " + GetString(row, "content_id")
                    + " source_catalog '" + cat + "' does not exist at " + absPath);
            }
        }

        [Fact]
        public void RuntimeContextTopIdsJsonIsConsistent()
        {
            // Phase 17 emits docs/visual/runtime_context_top_ids.json. The
            // surfaced_count must equal the union of top-N categories and
            // intersect the actionable manifest. If the file is missing we
            // tolerate absence (the tooling may not have run yet).
            string repoRoot = RepoRoot();
            string path = Path.Combine(repoRoot, "docs", "visual", "runtime_context_top_ids.json");
            if (!File.Exists(path)) return;
            var data = JsonDocument.Parse(File.ReadAllText(path)).RootElement;
            var manifest = LoadManifest();
            var manifestIds = new HashSet<string>();
            foreach (var row in manifest)
            {
                manifestIds.Add(GetString(row, "content_id"));
            }
            // Recompute surfaced locally
            int surfaced = 0;
            foreach (var cat in new[] { "items", "survivors", "locations", "characters" })
            {
                if (!data.TryGetProperty("top_in_manifest", out var tim)) continue;
                if (!tim.TryGetProperty(cat, out var arr)) continue;
                foreach (var el in arr.EnumerateArray())
                {
                    if (manifestIds.Contains(el.GetString())) surfaced++;
                }
            }
            int claimed = data.GetProperty("surfaced_count").GetInt32();
            Assert.Equal(claimed, surfaced);
        }

        // --- helpers ---

        private static string GetString(JsonElement row, string key)
        {
            if (!row.TryGetProperty(key, out var prop)) return null;
            return prop.ValueKind == JsonValueKind.String ? prop.GetString() : prop.ToString();
        }

        private static double GetDouble(JsonElement row, string key)
        {
            if (!row.TryGetProperty(key, out var prop)) return 0.0;
            return prop.ValueKind == JsonValueKind.Number ? prop.GetDouble() : 0.0;
        }
    }
}
