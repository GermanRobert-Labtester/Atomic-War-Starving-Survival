// SPDX-License-Identifier: MIT
// ASHFALL CI Source Gate: Forbidden Core APIs.
// Enforces Core Invariants (Invariant 1: Zero engine coupling; Invariant 4: Zero nondeterminism).
// Scans all C# files under Assets/Ashfall.Core/ with comments stripped to guarantee:
//   1. Zero engine namespaces (UnityEngine, UnityEditor, Godot, GodotSharp).
//   2. Zero nondeterministic RNGs (System.Random, new Random(), UnityEngine.Random).
//   3. Zero nondeterministic GUID creation (Guid.NewGuid()).
//   4. Zero legacy serializer bypasses (JsonUtility, Newtonsoft.Json, BinaryFormatter).
//   5. Zero wall-clock simulation drift (DateTime.Now, DateTime.UtcNow).
//   6. Zero blocking thread sleep hazards (Thread.Sleep).
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class ForbiddenCoreApiGateTests
    {
        private static readonly Regex LineComment = new Regex(@"//.*$", RegexOptions.Multiline | RegexOptions.Compiled);
        private static readonly Regex BlockComment = new Regex(@"/\*.*?\*/", RegexOptions.Singleline | RegexOptions.Compiled);

        private static string CoreDir()
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
                    string probe = Path.Combine(dir.FullName, "Assets", "Ashfall.Core");
                    if (Directory.Exists(probe))
                        return probe;
                    dir = dir.Parent;
                }
            }
            throw new DirectoryNotFoundException("Could not locate Assets/Ashfall.Core from the test run");
        }

        private static string StripComments(string code)
        {
            code = BlockComment.Replace(code, string.Empty);
            return LineComment.Replace(code, string.Empty);
        }

        private static List<(string FilePath, string RelativePath, string StrippedCode)> LoadCoreSourceFiles()
        {
            string coreRoot = CoreDir();
            var files = Directory.EnumerateFiles(coreRoot, "*.cs", SearchOption.AllDirectories)
                .Where(f => !f.Replace('\\', '/').Contains("/obj/") && !f.Replace('\\', '/').Contains("/bin/"))
                .ToList();

            var list = new List<(string FilePath, string RelativePath, string StrippedCode)>(files.Count);
            foreach (var f in files)
            {
                string rel = Path.GetRelativePath(coreRoot, f).Replace('\\', '/');
                string text = File.ReadAllText(f);
                string stripped = StripComments(text);
                list.Add((f, rel, stripped));
            }
            return list;
        }

        [Fact]
        public void CoreSource_ContainsZeroEngineNamespaces()
        {
            var files = LoadCoreSourceFiles();
            var enginePatterns = new[]
            {
                new Regex(@"\busing\s+UnityEngine\b", RegexOptions.Compiled),
                new Regex(@"\busing\s+UnityEditor\b", RegexOptions.Compiled),
                new Regex(@"\busing\s+Godot\b", RegexOptions.Compiled),
                new Regex(@"\busing\s+GodotSharp\b", RegexOptions.Compiled),
                new Regex(@"\bUnityEngine\s*\.", RegexOptions.Compiled),
                new Regex(@"\bUnityEditor\s*\.", RegexOptions.Compiled),
                new Regex(@"\bGodot\s*\.\s*(?:Node|Control|Vector|Transform|Resource|GD|Engine|SceneTree)\b", RegexOptions.Compiled)
            };

            var violations = new List<string>();

            foreach (var (filePath, relPath, code) in files)
            {
                foreach (var pattern in enginePatterns)
                {
                    var matches = pattern.Matches(code);
                    foreach (Match m in matches)
                    {
                        violations.Add($"{relPath}: matched forbidden engine pattern '{m.Value}'");
                    }
                }
            }

            Assert.True(
                violations.Count == 0,
                $"Invariant 1 Violation — Engine namespace coupling found in Assets/Ashfall.Core:\n  " +
                string.Join("\n  ", violations));
        }

        [Fact]
        public void CoreSource_ContainsZeroSystemRandomOrNondeterministicRng()
        {
            var files = LoadCoreSourceFiles();
            var rngPatterns = new[]
            {
                new Regex(@"\bnew\s+System\s*\.\s*Random\s*\(", RegexOptions.Compiled),
                new Regex(@"\bnew\s+Random\s*\(", RegexOptions.Compiled),
                new Regex(@"\bSystem\s*\.\s*Random\b", RegexOptions.Compiled),
                new Regex(@"\bUnityEngine\s*\.\s*Random\b", RegexOptions.Compiled)
            };

            var violations = new List<string>();

            foreach (var (filePath, relPath, code) in files)
            {
                foreach (var pattern in rngPatterns)
                {
                    var matches = pattern.Matches(code);
                    foreach (Match m in matches)
                    {
                        violations.Add($"{relPath}: matched forbidden RNG '{m.Value}'. Use ISeededRng / SeededRng instead.");
                    }
                }
            }

            Assert.True(
                violations.Count == 0,
                $"Invariant 4 Violation — Nondeterministic RNG found in Assets/Ashfall.Core:\n  " +
                string.Join("\n  ", violations));
        }

        [Fact]
        public void CoreSource_ContainsZeroGuidNewGuid()
        {
            var files = LoadCoreSourceFiles();
            var guidPattern = new Regex(@"\bGuid\s*\.\s*NewGuid\s*\(", RegexOptions.Compiled);

            var violations = new List<string>();

            foreach (var (filePath, relPath, code) in files)
            {
                var matches = guidPattern.Matches(code);
                foreach (Match m in matches)
                {
                    violations.Add($"{relPath}: Guid.NewGuid() forbidden for simulation determinism.");
                }
            }

            Assert.True(
                violations.Count == 0,
                $"Invariant 4 Violation — Guid.NewGuid() found in Assets/Ashfall.Core:\n  " +
                string.Join("\n  ", violations));
        }

        [Fact]
        public void CoreSource_ContainsZeroLegacySerializerBypasses()
        {
            var files = LoadCoreSourceFiles();
            var serializerPatterns = new[]
            {
                new Regex(@"\bJsonUtility\b", RegexOptions.Compiled),
                new Regex(@"\bNewtonsoft\b", RegexOptions.Compiled),
                new Regex(@"\bBinaryFormatter\b", RegexOptions.Compiled)
            };

            var violations = new List<string>();

            foreach (var (filePath, relPath, code) in files)
            {
                foreach (var pattern in serializerPatterns)
                {
                    var matches = pattern.Matches(code);
                    foreach (Match m in matches)
                    {
                        violations.Add($"{relPath}: matched forbidden serializer '{m.Value}'. Use IJsonSerializer or System.Text.Json.");
                    }
                }
            }

            Assert.True(
                violations.Count == 0,
                $"Data Contract Violation — Banned serializer references found in Assets/Ashfall.Core:\n  " +
                string.Join("\n  ", violations));
        }

        [Fact]
        public void CoreSource_ContainsZeroWallClockOrThreadSleepHazards()
        {
            var files = LoadCoreSourceFiles();
            var hazardPatterns = new[]
            {
                new Regex(@"\bDateTime\s*\.\s*Now\b", RegexOptions.Compiled),
                new Regex(@"\bDateTime\s*\.\s*UtcNow\b", RegexOptions.Compiled),
                new Regex(@"\bThread\s*\.\s*Sleep\b", RegexOptions.Compiled)
            };

            var violations = new List<string>();

            foreach (var (filePath, relPath, code) in files)
            {
                // IWallClock.cs is the documented single port/adapter for non-simulation wall-clock metadata
                if (relPath.EndsWith("IWallClock.cs", StringComparison.OrdinalIgnoreCase))
                    continue;

                foreach (var pattern in hazardPatterns)
                {
                    var matches = pattern.Matches(code);
                    foreach (Match m in matches)
                    {
                        violations.Add($"{relPath}: matched simulation hazard '{m.Value}'. Use IClock / ISimClock instead.");
                    }
                }
            }

            Assert.True(
                violations.Count == 0,
                $"Determinism & Safety Violation — Wall-clock or sleep hazards found in Assets/Ashfall.Core:\n  " +
                string.Join("\n  ", violations));
        }
    }
}
