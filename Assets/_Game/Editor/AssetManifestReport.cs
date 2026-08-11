using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.Editor
{
    /// <summary>
    /// Generates the art/audio work list: every game-data id that needs a sprite, the
    /// exact path that sprite must live at, and whether it exists yet.
    ///
    /// Written for the hand-off to asset production. Without it, "which icons are
    /// still missing?" is answered by playing the game and watching the console, which
    /// only surfaces the ones you happened to look at.
    ///
    /// Menu: ASHFALL → Assets → Generate Asset Manifest.
    /// </summary>
    public static class AssetManifestReport
    {
        private const string OutputPath = "audit/ASSET_MANIFEST.md";

        [MenuItem("ASHFALL/Assets/Generate Asset Manifest")]
        public static void Generate()
        {
            string report = Build(out int total, out int present);
            string full = Path.GetFullPath(Path.Combine(Application.dataPath, "..", OutputPath));
            Directory.CreateDirectory(Path.GetDirectoryName(full));
            File.WriteAllText(full, report);
            AssetDatabase.Refresh();
            Debug.Log($"[AssetManifest] {present}/{total} assets present. Wrote {OutputPath}");
        }

        /// <summary>
        /// Build the manifest. Split from <see cref="Generate"/> so tests can assert on
        /// the content without touching the filesystem or the menu.
        /// </summary>
        public static string Build(out int total, out int present)
        {
            var rows = new List<(string Kind, string Id, string Path, bool Exists, bool ValidId)>();

            foreach (string id in CollectItemIds())
            {
                string path = GameAssetKeys.ItemIcon(id);
                rows.Add(("item icon", id, path, ResourceExists<Sprite>(path), GameAssetKeys.IsValidId(id)));
            }

            total = rows.Count;
            present = 0;
            foreach (var r in rows) if (r.Exists) present++;

            var sb = new StringBuilder();
            sb.AppendLine("# ASHFALL — asset manifest");
            sb.AppendLine();
            sb.AppendLine($"Generated: {DateTime.UtcNow:yyyy-MM-dd HH:mm}Z");
            sb.AppendLine();
            sb.AppendLine("Every id below needs an asset at the given path, relative to a");
            sb.AppendLine("`Resources` folder and with no file extension. Drop a sprite at that");
            sb.AppendLine("path and it is picked up automatically — no code or data change needed.");
            sb.AppendLine();
            sb.AppendLine($"- **{present} / {total}** item icons present");
            sb.AppendLine($"- **{total - present}** still to author");
            sb.AppendLine();

            var badIds = new List<string>();
            foreach (var r in rows) if (!r.ValidId) badIds.Add(r.Id);
            if (badIds.Count > 0)
            {
                sb.AppendLine("## Malformed ids");
                sb.AppendLine();
                sb.AppendLine("These are not lowercase snake_case, so their asset path can never");
                sb.AppendLine("resolve. Fix the id in the catalog, not the filename.");
                sb.AppendLine();
                foreach (string b in badIds) sb.AppendLine($"- `{b}`");
                sb.AppendLine();
            }

            sb.AppendLine("## Item icons");
            sb.AppendLine();
            sb.AppendLine("| status | id | expected path |");
            sb.AppendLine("|---|---|---|");
            rows.Sort((a, b) => string.CompareOrdinal(a.Id, b.Id));
            foreach (var r in rows)
                sb.AppendLine($"| {(r.Exists ? "ok" : "MISSING")} | `{r.Id}` | `{r.Path}` |");

            return sb.ToString();
        }

        /// <summary>
        /// Union of the C# world catalog and the JSON item table. Both feed real items
        /// into the game, and either one alone under-reports what art is needed.
        /// </summary>
        private static SortedSet<string> CollectItemIds()
        {
            var ids = new SortedSet<string>(StringComparer.Ordinal);

            foreach (string id in Item_WorldCatalog.AllIds())
                if (!string.IsNullOrWhiteSpace(id)) ids.Add(id);

            string jsonPath = Path.Combine(Application.streamingAssetsPath, "Data", "items.json");
            if (File.Exists(jsonPath))
            {
                foreach (System.Text.RegularExpressions.Match m in
                         System.Text.RegularExpressions.Regex.Matches(
                             File.ReadAllText(jsonPath), "\"id\"\\s*:\\s*\"([^\"]+)\""))
                {
                    string id = m.Groups[1].Value;
                    if (!string.IsNullOrWhiteSpace(id)) ids.Add(id);
                }
            }

            return ids;
        }

        private static bool ResourceExists<T>(string path) where T : UnityEngine.Object =>
            !string.IsNullOrEmpty(path) && Resources.Load<T>(path) != null;
    }
}
