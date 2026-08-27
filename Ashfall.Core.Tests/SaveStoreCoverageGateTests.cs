// SPDX-License-Identifier: MIT
// ASHFALL gate: every Godot save store must be checksum-protected.
//
// A save store in src/ is compliant when its non-comment source either
//   (a) stamps/validates a SaveChecksum envelope itself, or
//   (b) delegates disk serialization to a Core save codec
//       (XxxSaveCodec.Encode / .Decode / .TryDecode), which owns the
//       checksum internally.
//
// Bare-state stores (serialize state directly, no integrity check) silently
// accept corrupt or foreign saves. Weather, HostEvent and ChemicalDependency
// shipped bare until sealed — this gate makes a recurrence fail CI loudly.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class SaveStoreCoverageGateTests
    {
        private static readonly Regex LineComment = new Regex("//.*", RegexOptions.Compiled);
        private static readonly Regex BlockComment =
            new Regex("/\\*.*?\\*/", RegexOptions.Compiled | RegexOptions.Singleline);

        // Codec delegation, e.g. HoldfastSaveCodec.Encode(...) / .TryDecode(...)
        private static readonly Regex CodecDelegation =
            new Regex(@"\w*Codec\s*\.\s*(Encode|Decode|TryDecode)", RegexOptions.Compiled);

        private static string SrcDir()
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
                    string probe = Path.Combine(dir.FullName, "src");
                    if (Directory.Exists(probe))
                        return probe;
                    dir = dir.Parent;
                }
            }
            throw new DirectoryNotFoundException("Could not locate src/ from the test run");
        }

        private static string StripComments(string text)
        {
            text = BlockComment.Replace(text, string.Empty);
            return LineComment.Replace(text, string.Empty);
        }

        [Fact]
        public void EverySaveStore_IsChecksumProtected()
        {
            string srcRoot = SrcDir();
            var storeFiles = Directory
                .EnumerateFiles(srcRoot, "*SaveStore*.cs", SearchOption.AllDirectories)
                .Where(f => !f.Replace('\\', '/').Contains("/obj/") &&
                            !f.Replace('\\', '/').Contains("/bin/") &&
                            !f.EndsWith("SelfTest.cs", StringComparison.OrdinalIgnoreCase) &&
                            !f.EndsWith("Tests.cs", StringComparison.OrdinalIgnoreCase) &&
                            !f.EndsWith("Test.cs", StringComparison.OrdinalIgnoreCase))
                .OrderBy(f => f, StringComparer.Ordinal)
                .ToList();

            // Sanity: the store population is large and stable (~60 files).
            // If this drops drastically the discovery broke — fail loudly.
            Assert.True(storeFiles.Count >= 40,
                $"Expected to discover the full save-store population under src/, found only {storeFiles.Count}.");

            var bare = new List<string>();
            foreach (string file in storeFiles)
            {
                string code = StripComments(File.ReadAllText(file));
                bool hasChecksum = code.Contains("Checksum");
                bool delegatesToCodec = CodecDelegation.IsMatch(code);
                if (!hasChecksum && !delegatesToCodec)
                    bare.Add(Path.GetFileName(file));
            }

            Assert.True(bare.Count == 0,
                "Bare save stores (no checksum envelope, no codec delegation) found — " +
                "corruption in these files is undetectable on load. Seal each one with the " +
                "ExpeditionSaveStore envelope pattern ({ State, Checksum } + legacy bare-state " +
                "fallback) or delegate to a Core save codec:\n  " +
                string.Join("\n  ", bare));
        }

        [Fact]
        public void EverySaveStore_IsSlotRootIsolated()
        {
            string srcRoot = SrcDir();
            var storeFiles = Directory
                .EnumerateFiles(srcRoot, "*SaveStore*.cs", SearchOption.AllDirectories)
                .Where(f => !f.Replace('\\', '/').Contains("/obj/") &&
                            !f.Replace('\\', '/').Contains("/bin/") &&
                            !f.EndsWith("SelfTest.cs", StringComparison.OrdinalIgnoreCase) &&
                            !f.EndsWith("Tests.cs", StringComparison.OrdinalIgnoreCase) &&
                            !f.EndsWith("Test.cs", StringComparison.OrdinalIgnoreCase))
                .OrderBy(f => f, StringComparer.Ordinal)
                .ToList();

            var unisolated = new List<string>();
            foreach (string file in storeFiles)
            {
                string code = StripComments(File.ReadAllText(file));
                bool hasSlotRoot = code.Contains("SaveSlotRoot") || code.Contains("ResolveSlotFile") || code.Contains("ResolveSlotPath");
                if (!hasSlotRoot)
                    unisolated.Add(Path.GetFileName(file));
            }

            Assert.True(unisolated.Count == 0,
                "Save stores missing SaveSlotRoot isolation found — saves in these files risk " +
                "polluting the global user:// directory during headless tests or profile switching:\n  " +
                string.Join("\n  ", unisolated));
        }
    }
}
