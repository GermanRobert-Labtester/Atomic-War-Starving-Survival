// SPDX-License-Identifier: MIT
// ASHFALL Core Tests: Persistent Filename Uniqueness & SaveSectionRegistry Gate.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Ashfall.Core.Save;
using Xunit;

namespace Ashfall.Core.Tests.Save
{
    public class PersistentFilenameRegistryGateTests
    {
        private static readonly Dictionary<string, string> SectionAliases = new(StringComparer.Ordinal)
        {
            ["holdfast_s1"] = "holdfast",
            ["holdfast_trade"] = "holdfast_trade",
            ["weather"] = "world"
        };

        private static readonly HashSet<string> NonGameplayFiles = new(StringComparer.OrdinalIgnoreCase)
        {
            "settings.json",
            "audio_settings.json"
        };

        private static string RepoRoot()
        {
            string[] candidates =
            {
                Directory.GetCurrentDirectory(),
                AppContext.BaseDirectory
            };
            foreach (string start in candidates)
            {
                var dir = new DirectoryInfo(Path.GetFullPath(start));
                while (dir != null)
                {
                    string probeProps = Path.Combine(dir.FullName, "Directory.Packages.props");
                    if (File.Exists(probeProps))
                        return dir.FullName;
                    dir = dir.Parent;
                }
            }
            throw new DirectoryNotFoundException("Could not locate repository root from test execution context.");
        }

        private class DiscoveredFileStore
        {
            public string ClassName { get; set; } = string.Empty;
            public string SourceFile { get; set; } = string.Empty;
            public string FileName { get; set; } = string.Empty;
            public string? SectionName { get; set; }
        }

        private static List<DiscoveredFileStore> DiscoverAllStores(string root)
        {
            var results = new List<DiscoveredFileStore>();
            string[] scanDirs = { Path.Combine(root, "src"), Path.Combine(root, "Assets", "Ashfall.Core") };

            var filePattern = new Regex(@"public\s+const\s+string\s+(?:FileName|SaveFileName)\s*=\s*""([^""]+)""", RegexOptions.Compiled);
            var secPattern = new Regex(@"public\s+const\s+string\s+SectionName\s*=\s*""([^""]+)""", RegexOptions.Compiled);
            var classPattern = new Regex(@"public\s+(?:static\s+|sealed\s+)?class\s+([A-Za-z0-9_]+)", RegexOptions.Compiled);

            foreach (var sdir in scanDirs)
            {
                if (!Directory.Exists(sdir)) continue;

                foreach (var file in Directory.EnumerateFiles(sdir, "*.cs", SearchOption.AllDirectories))
                {
                    string normPath = file.Replace('\\', '/');
                    if (normPath.Contains("/bin/") || normPath.Contains("/obj/")) continue;
                    if (normPath.EndsWith("Test.cs") || normPath.EndsWith("Tests.cs") || normPath.EndsWith("SelfTest.cs")) continue;

                    string content = File.ReadAllText(file);
                    var classMatches = classPattern.Matches(content);

                    for (int i = 0; i < classMatches.Count; i++)
                    {
                        var cm = classMatches[i];
                        string cname = cm.Groups[1].Value;
                        int cstart = cm.Index;
                        int cend = (i + 1 < classMatches.Count) ? classMatches[i + 1].Index : content.Length;
                        string cbody = content.Substring(cstart, cend - cstart);

                        var fmatch = filePattern.Match(cbody);
                        if (fmatch.Success)
                        {
                            string fn = fmatch.Groups[1].Value;
                            var smatch = secPattern.Match(cbody);
                            string? sec = smatch.Success ? smatch.Groups[1].Value : null;

                            results.Add(new DiscoveredFileStore
                            {
                                ClassName = cname,
                                SourceFile = Path.GetRelativePath(root, file).Replace('\\', '/'),
                                FileName = fn,
                                SectionName = sec
                            });
                        }
                    }
                }
            }

            return results;
        }

        [Fact]
        public void PersistentFilenames_AreStrictlyUniqueAcrossAllStores()
        {
            string root = RepoRoot();
            var stores = DiscoverAllStores(root);

            Assert.True(stores.Count >= 60, $"Expected at least 60 persistent stores, found {stores.Count}.");

            var grouped = stores.GroupBy(s => s.FileName, StringComparer.OrdinalIgnoreCase);
            var collisions = new List<string>();

            foreach (var g in grouped)
            {
                if (g.Count() > 1)
                {
                    string storesList = string.Join(", ", g.Select(s => $"{s.ClassName} ({s.SourceFile})"));
                    collisions.Add($"Filename '{g.Key}' is shared by multiple stores: {storesList}");
                }
            }

            Assert.True(collisions.Count == 0,
                $"Discovered {collisions.Count} filename collisions across persistent stores:\n  " +
                string.Join("\n  ", collisions));
        }

        [Fact]
        public void PersistentSaveFilenames_FollowSnakeCaseJsonConvention()
        {
            string root = RepoRoot();
            var stores = DiscoverAllStores(root);
            var snakeJsonRegex = new Regex(@"^[a-z0-9_]+\.json$", RegexOptions.Compiled);

            var invalidNames = new List<string>();
            foreach (var s in stores)
            {
                if (!snakeJsonRegex.IsMatch(s.FileName))
                {
                    invalidNames.Add($"{s.ClassName} in {s.SourceFile} defines non-snake_case filename '{s.FileName}'");
                }
            }

            Assert.True(invalidNames.Count == 0,
                $"Discovered {invalidNames.Count} non-conforming persistent filenames:\n  " +
                string.Join("\n  ", invalidNames));
        }

        [Fact]
        public void EverySaveStoreSectionName_IsRepresentedInSaveSectionRegistry()
        {
            // Formerly incomplete SaveStores are now enrolled in SaveSectionRegistry
            // and wired through Setup/Save/SaveAll. Keep this set empty unless a
            // new store lands ahead of registry enrollment.
            var incompleteSectionAllowlist = new HashSet<string>(StringComparer.Ordinal);

            string root = RepoRoot();
            var stores = DiscoverAllStores(root);
            var registeredKeys = new HashSet<string>(SaveSectionRegistry.All.Select(s => s.SectionKey), StringComparer.Ordinal);

            var unmappedSections = new List<string>();

            foreach (var s in stores)
            {
                if (s.SectionName != null)
                {
                    string canonical = SectionAliases.TryGetValue(s.SectionName, out var mapped) ? mapped : s.SectionName;
                    if (!registeredKeys.Contains(canonical) && !incompleteSectionAllowlist.Contains(canonical))
                    {
                        unmappedSections.Add($"{s.ClassName} in {s.SourceFile} declares unmapped SectionName '{s.SectionName}'");
                    }
                }
            }

            Assert.True(unmappedSections.Count == 0,
                $"Discovered {unmappedSections.Count} unmapped SaveStore SectionNames:\n  " +
                string.Join("\n  ", unmappedSections));
        }

        [Fact]
        public void EveryRegisteredSaveSection_HasAnAssociatedSaveStoreFile()
        {
            string root = RepoRoot();
            var stores = DiscoverAllStores(root);
            var storeSections = new HashSet<string>(StringComparer.Ordinal);

            foreach (var s in stores)
            {
                if (s.SectionName != null)
                {
                    string canonical = SectionAliases.TryGetValue(s.SectionName, out var mapped) ? mapped : s.SectionName;
                    storeSections.Add(canonical);
                }
            }

            var missingFromRegistry = new List<string>();
            foreach (var meta in SaveSectionRegistry.All)
            {
                if (!storeSections.Contains(meta.SectionKey))
                {
                    missingFromRegistry.Add($"Registered section '{meta.SectionKey}' ({meta.SaveMethod}) has no matching SaveStore with SectionName");
                }
            }

            Assert.True(missingFromRegistry.Count == 0,
                $"Discovered {missingFromRegistry.Count} registered save sections without associated SaveStores:\n  " +
                string.Join("\n  ", missingFromRegistry));
        }
    }
}
