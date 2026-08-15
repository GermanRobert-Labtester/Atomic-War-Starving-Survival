using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Utilities;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// EditMode verification for the Resources/Art pipeline:
    ///
    /// 1. Fallback resolution — GameAssetService.GetSprite(path, fallback) must return
    ///    real Resources/Art art when authored and the caller's legacy reference when not.
    /// 2. Key hygiene — every data id resolved through GameAssetKeys must be well-formed.
    /// 3. Uniqueness — no two files may share a base name with different extensions
    ///    (Resources.Load without an extension is ambiguous and errors at runtime).
    /// 4. Coverage — logs the per-category authoring gap (x of y ids resolve) so the
    ///    art pipeline has an actionable work list. Counts are logged, not asserted,
    ///    because art lands incrementally; only "items resolve at all" is asserted.
    /// </summary>
    [TestFixture]
    public class ArtResourceCoverageTests
    {
        private static readonly string[] KnownFactionIds =
        {
            "faction_central_garrison",
            "faction_upland_militia",
            "faction_cult_of_the_glow",
            "faction_garrison",
            "faction_archivists",
            "faction_osteophages",
            "faction_sun_seekers",
        };

        private static readonly Regex IdField = new Regex(@"^\s*id:\s*(\S+)\s*$", RegexOptions.Compiled);
        private static readonly Regex ModuleIdField =
            new Regex(@"moduleId\s*=\s*""([^""]+)""", RegexOptions.Compiled);

        // ---------------------------------------------------------------- ids

        private static List<string> GatherIdsFromAssets(string dir, Regex field)
        {
            var ids = new List<string>();
            if (!Directory.Exists(dir)) return ids;
            foreach (var file in Directory.GetFiles(dir, "*.asset"))
            {
                foreach (var raw in File.ReadAllLines(file))
                {
                    var m = field.Match(raw);
                    if (m.Success) ids.Add(m.Groups[1].Value);
                }
            }
            return ids;
        }

        private static List<string> GatherModuleIds()
        {
            var ids = new List<string>();
            const string dir = "Assets/_Game/Shelter/Modules";
            if (!Directory.Exists(dir)) return ids;
            foreach (var file in Directory.GetFiles(dir, "*.cs"))
            {
                foreach (var raw in File.ReadAllLines(file))
                {
                    var m = ModuleIdField.Match(raw);
                    if (m.Success) ids.Add(m.Groups[1].Value);
                }
            }
            return ids;
        }

        // ---------------------------------------------------------------- tests

        [Test]
        public void GetSprite_WithFallback_ReturnsFallbackWhenPathMissing()
        {
            var service = new GameAssetService(new ResourcesAssetProvider());
            var fallback = Sprite.Create(Texture2D.whiteTexture,
                new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));

            var result = service.GetSprite("Art/Items/no_such_item_ever", fallback);

            Assert.That(result, Is.SameAs(fallback),
                "Missing Resources path must resolve to the caller's fallback.");
        }

        [Test]
        public void GetSprite_WithFallback_ReturnsRealArtWhenPathExists()
        {
            var service = new GameAssetService(new ResourcesAssetProvider());
            var fallback = Sprite.Create(Texture2D.whiteTexture,
                new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));

            // Authored ammo art (png, post-dedupe) must win over the fallback.
            var result = service.GetSprite(GameAssetKeys.ItemIcon("ammo_300blk_jhp_ap"), fallback);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.SameAs(fallback),
                "Authored Resources/Art sprite must take precedence over the fallback.");
        }

        [Test]
        public void GetSprite_WithFallback_WorksWithoutPlaceholderConfigured()
        {
            var service = new GameAssetService(new ResourcesAssetProvider())
            {
                PlaceholderSprite = Sprite.Create(Texture2D.whiteTexture,
                    new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f)),
            };
            var fallback = Sprite.Create(Texture2D.whiteTexture,
                new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));

            // Even with the global placeholder set, the per-entry fallback must be
            // honoured for a missing path (documented contract of the overload).
            var result = service.GetSprite("Art/Items/no_such_item_ever", fallback);

            Assert.That(result, Is.SameAs(fallback),
                "Per-entry fallback must win over the global placeholder for misses.");
        }

        [Test]
        public void ResourcesArt_HasNoAmbiguousDuplicateBasenames()
        {
            const string root = "Assets/Resources/Art";
            Assert.That(Directory.Exists(root), Is.True, "Resources/Art must exist.");

            var seen = new Dictionary<string, string>(StringComparer.Ordinal);
            var dupes = new List<string>();
            foreach (var file in Directory.GetFiles(root, "*.*", SearchOption.AllDirectories))
            {
                if (file.EndsWith(".meta")) continue;
                string baseName = Path.GetFileNameWithoutExtension(file);
                if (seen.TryGetValue(baseName, out var other))
                {
                    dupes.Add($"{baseName}: {Path.GetFileName(other)} vs {Path.GetFileName(file)}");
                }
                else
                {
                    seen[baseName] = file;
                }
            }

            Assert.That(dupes, Is.Empty,
                "Duplicate base names make extensionless Resources.Load ambiguous:\n" +
                string.Join("\n", dupes));
        }

        [Test]
        public void AllDataIds_AreValidAssetKeys()
        {
            var ids = new List<string>();
            ids.AddRange(GatherIdsFromAssets("Assets/_Game/Data/Generated/Items", IdField));
            ids.AddRange(GatherModuleIds());
            ids.AddRange(GatherIdsFromAssets("Assets/_Game/Data/Generated/Survivors", IdField));
            ids.AddRange(KnownFactionIds);

            var invalid = new List<string>();
            foreach (var id in ids)
            {
                if (!GameAssetKeys.IsValidId(id)) invalid.Add(id);
            }

            Assert.That(invalid, Is.Empty,
                "These ids would silently never resolve as asset keys:\n" +
                string.Join("\n", invalid));
        }

        [Test]
        public void CoverageReport_LogsPerCategoryResolutionCounts()
        {
            var service = new GameAssetService(new ResourcesAssetProvider());
            int CountResolved(IEnumerable<string> ids, Func<string, string> pathFor)
            {
                int hit = 0;
                foreach (var id in ids)
                {
                    var path = pathFor(id);
                    if (path == null) continue;
                    if (service.GetSprite(path) != null) hit++;
                }
                return hit;
            }

            var itemIds = GatherIdsFromAssets("Assets/_Game/Data/Generated/Items", IdField);
            var moduleIds = GatherModuleIds();
            var survivorIds = GatherIdsFromAssets("Assets/_Game/Data/Generated/Survivors", IdField);

            int itemsHit = CountResolved(itemIds, GameAssetKeys.ItemIcon);
            int modulesHit = CountResolved(moduleIds, GameAssetKeys.ShelterModuleIcon);
            int portraitsHit = CountResolved(survivorIds, GameAssetKeys.SurvivorPortrait);
            int factionsHit = CountResolved(KnownFactionIds, GameAssetKeys.FactionEmblem);

            Debug.Log($"[ArtCoverage] items: {itemsHit}/{itemIds.Count} resolve via Resources/Art");
            Debug.Log($"[ArtCoverage] shelter modules: {modulesHit}/{moduleIds.Count} resolve via Resources/Art");
            Debug.Log($"[ArtCoverage] survivor portraits: {portraitsHit}/{survivorIds.Count} resolve via Resources/Art");
            Debug.Log($"[ArtCoverage] factions: {factionsHit}/{KnownFactionIds.Length} resolve via Resources/Art");
            Debug.Log($"[ArtCoverage] service missing-path record: {service.MissingPaths.Count} distinct misses");

            Assert.That(itemIds.Count, Is.GreaterThan(0), "Item data must be present.");
            Assert.That(itemsHit, Is.GreaterThan(0),
                "At least one item id must resolve — the pipeline is wired to real art.");
        }
    }
}
