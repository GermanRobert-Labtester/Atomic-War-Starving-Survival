using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Static hermeticity gates over the engine-agnostic Core ring.
    ///
    /// Invariant 1 — Core must have ZERO engine coupling
    /// (no UnityEngine/*, no Godot/*, no JsonUtility).
    ///
    /// Invariant 4 — Core must be DETERMINISTIC
    /// (no System.Random, no Guid.NewGuid, no DateTime.Now, no GetHashCode()).
    ///
    /// These are compile-proofed by the asmdef (noEngineReferences) for the
    /// engine side but NOT for the nondeterminism side, so we scan the source
    /// directly. If a future edit reintroduces either class of violation, this
    /// test fails loudly instead of silently forking per host.
    /// </summary>
    public class CoreInvariantSourceTests
    {
        private static readonly Regex LineComment = new Regex("//.*", RegexOptions.Compiled);
        private static readonly Regex BlockComment =
            new Regex("/\\*.*?\\*/", RegexOptions.Compiled | RegexOptions.Singleline);

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

        private static IEnumerable<string> AllCoreStatements()
        {
            foreach (string file in Directory.EnumerateFiles(CoreDir(), "*.cs", SearchOption.AllDirectories))
            {
                if (file.Replace('\\', '/').Contains("/obj/") || file.Replace('\\', '/').Contains("/bin/"))
                    continue;
                if (file.EndsWith("IWallClock.cs", StringComparison.OrdinalIgnoreCase))
                    continue;
                string text = File.ReadAllText(file);
                text = BlockComment.Replace(text, string.Empty);
                var lines = text.Split('\n');
                for (int i = 0; i < lines.Length; i++)
                {
                    string line = LineComment.Replace(lines[i], string.Empty);
                    yield return $"{file}:{i + 1} :: {line}";
                }
            }
        }

        [Fact]
        public void Core_HasZeroEngineCoupling()
        {
            string[] banned =
            {
                "UnityEngine.", "using UnityEngine", "using UnityEditor",
                "Godot.", "GodotSharp", "using Godot;",
                "JsonUtility", "UnityEditor"
            };
            var offenders = AllCoreStatements()
                .Where(l => banned.Any(b => l.Contains(b, StringComparison.Ordinal)))
                .ToList();
            Assert.True(offenders.Count == 0,
                "Ashfall.Core is engine-agnostic (Invariant 1) but references engine types:\n"
                + string.Join("\n", offenders.Take(20)));
        }

        [Fact]
        public void Core_HasZeroNondeterminismSources()
        {
            string[] banned =
            {
                "System.Random", "new Random(", "Guid.NewGuid(",
                "DateTime.Now", "DateTime.UtcNow", ".GetHashCode()"
            };
            var offenders = AllCoreStatements()
                .Where(l => banned.Any(b => l.Contains(b, StringComparison.Ordinal)))
                .ToList();
            Assert.True(offenders.Count == 0,
                "Ashfall.Core must be deterministic (Invariant 4) but uses a nondeterministic source:\n"
                + string.Join("\n", offenders.Take(20)));
        }

        [Fact]
        public void Core_SeededRng_ReproducesAcrossInstances()
        {
            // Concept: same seed must yield identical streams in any host/instance.
            var a = new SeededRng(0x5EED_1234);
            var b = new SeededRng(0x5EED_1234);
            var seqA = new List<int>();
            var seqB = new List<int>();
            for (int i = 0; i < 64; i++)
            {
                seqA.Add(a.Next(0, 10_000));
                seqB.Add(b.Next(0, 10_000));
            }
            Assert.Equal(string.Join(",", seqA), string.Join(",", seqB));

            // Different seed diverges.
            var c = new SeededRng(0x5EED_9999);
            Assert.NotEqual(string.Join(",", seqA), string.Join(",", Enumerable.Range(0, 64).Select(_ => c.Next(0, 10_000))));
        }
    }
}
