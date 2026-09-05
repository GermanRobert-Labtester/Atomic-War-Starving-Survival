using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Ashfall.Core;
using Godot;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Plan VIII · Task 23 — packaged-data parity gate.
    /// Proves the EXPORTED Linux layout (exe + .pck + loose Assets/StreamingAssets/Data)
    /// carries the same authoritative catalogs as the repository data authority:
    /// every catalog present, byte-identical (SHA-256), parseable, exact Linux path
    /// casing, no Git-LFS pointers shipped in place of binaries, and a real ELF
    /// executable + PCK payload. Runs both from the repository (target
    /// builds/linux) and from inside an exported build (target = executable dir).
    /// </summary>
    public static partial class HostCli
    {
        /// <summary>Value of a "--name value" CLI option, or null.</summary>
        public static string? GetOptionValue(string[] args, string name)
        {
            if (args == null) return null;
            for (int i = 0; i < args.Length - 1; i++)
                if (string.Equals(args[i], name, StringComparison.Ordinal))
                    return args[i + 1];
            return null;
        }

        public static int RunExportParitySelfTest(string dataDirectory, string? targetDirOverride = null)
        {
            CatalogLocator.UseInvariantCulture();
            int errors = 0;
            int checkedFiles = 0;

            // ── target resolution ───────────────────────────────────────
            string targetDir = ResolveParityTarget(targetDirOverride);
            GD.Print($"[PARITY] target={targetDir}");

            string exportedData = Path.Combine(targetDir, "Assets", "StreamingAssets", "Data");
            string exePath = Path.Combine(targetDir, "ashfall.x86_64");
            string pckPath = Path.Combine(targetDir, "ashfall.pck");

            if (!Directory.Exists(exportedData))
            {
                GD.PrintErr($"[PARITY] exported data deployment missing: {exportedData}");
                return 1;
            }

            // ── binary payload checks (23.7/23.8) ───────────────────────
            errors += CheckElfExecutable(exePath);
            errors += CheckPck(pckPath);
            errors += CheckNoLfsPointers(exportedData, ref checkedFiles);

            // ── source catalog enumeration (authority) ──────────────────
            var sourceFiles = new List<string>();
            try
            {
                foreach (var path in Directory.EnumerateFiles(dataDirectory, "*.json", SearchOption.AllDirectories))
                    sourceFiles.Add(path);
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[PARITY] cannot enumerate source data dir '{dataDirectory}': {ex.Message}");
                return errors + 1;
            }
            sourceFiles.Sort(StringComparer.Ordinal);
            if (sourceFiles.Count == 0)
            {
                GD.PrintErr($"[PARITY] source data dir has zero JSON catalogs: {dataDirectory}");
                return errors + 1;
            }

            // ── casing map of the exported tree (23.6) ──────────────────
            var exportedIndex = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var path in Directory.EnumerateFiles(exportedData, "*.json", SearchOption.AllDirectories))
                exportedIndex[Path.GetRelativePath(exportedData, path)] = path;

            // ── presence + exact-case + byte parity + parse (23.5) ──────
            foreach (var srcPath in sourceFiles)
            {
                string rel = Path.GetRelativePath(dataDirectory, srcPath);
                if (!exportedIndex.TryGetValue(rel, out var dstPath))
                {
                    GD.PrintErr($"[PARITY] missing catalog in package: {rel}");
                    errors++;
                    continue;
                }
                if (!string.Equals(Path.GetFileName(dstPath), Path.GetFileName(rel), StringComparison.Ordinal) ||
                    !File.Exists(Path.Combine(exportedData, rel)))
                {
                    GD.PrintErr($"[PARITY] case mismatch in package: expected '{rel}', found '{Path.GetRelativePath(exportedData, dstPath)}'");
                    errors++;
                    continue;
                }

                checkedFiles++;
                string srcHash = Sha256(srcPath);
                string dstHash = Sha256(dstPath);
                if (srcHash != dstHash)
                {
                    GD.PrintErr($"[PARITY] hash mismatch: {rel}");
                    errors++;
                    continue;
                }
                if (!TryParseJson(dstPath))
                {
                    GD.PrintErr($"[PARITY] packaged catalog does not parse: {rel}");
                    errors++;
                }
            }

            // ── version stamp traceability (23.11) ──────────────────────
            string stampPath = Path.Combine(targetDir, "RELEASE_STAMP.txt");
            if (File.Exists(stampPath))
                foreach (var line in File.ReadAllLines(stampPath))
                    GD.Print("[PARITY] stamp: " + line);
            else
                GD.Print("[PARITY] stamp: (RELEASE_STAMP.txt not present — written by scripts/ci/export-build.sh)");

            if (errors > 0)
            {
                GD.PrintErr($"EXPORT_PARITY_SELFTEST FAIL — {errors} error(s), {checkedFiles} catalogs verified byte-identical");
                return 1;
            }

            GD.Print($"EXPORT_PARITY_SELFTEST PASS — {checkedFiles} catalogs byte-identical + parseable, casing exact, no LFS pointers, exe+PCK present");
            return 0;
        }

        private static string ResolveParityTarget(string? overrideDir)
        {
            if (!string.IsNullOrEmpty(overrideDir))
                return Path.GetFullPath(overrideDir);

            // Inside an exported build the executable sits next to Assets/.
            try
            {
                string exePath = OS.GetExecutablePath();
                if (!string.IsNullOrEmpty(exePath))
                {
                    string exeDir = Path.GetDirectoryName(exePath) ?? string.Empty;
                    if (Directory.Exists(Path.Combine(exeDir, "Assets", "StreamingAssets", "Data")))
                        return exeDir;
                }
            }
            catch { /* headless dev context — fall through */ }

            return Path.Combine(Directory.GetCurrentDirectory(), "builds", "linux");
        }

        private static int CheckElfExecutable(string exePath)
        {
            if (!File.Exists(exePath))
            {
                GD.PrintErr($"[PARITY] exported executable missing: {exePath}");
                return 1;
            }
            using var fs = File.OpenRead(exePath);
            Span<byte> magic = stackalloc byte[4];
            if (fs.Read(magic) != 4 || magic[0] != 0x7f || magic[1] != (byte)'E' || magic[2] != (byte)'L' || magic[3] != (byte)'F')
            {
                GD.PrintErr($"[PARITY] exported executable is not an ELF binary: {exePath}");
                return 1;
            }
            return 0;
        }

        private static int CheckPck(string pckPath)
        {
            if (!File.Exists(pckPath))
            {
                GD.PrintErr($"[PARITY] exported PCK missing: {pckPath}");
                return 1;
            }
            var info = new FileInfo(pckPath);
            if (info.Length < 1024 * 1024)
            {
                GD.PrintErr($"[PARITY] exported PCK suspiciously small ({info.Length} bytes): {pckPath}");
                return 1;
            }
            GD.Print($"[PARITY] pck: {info.Length / (1024 * 1024)} MB");
            return 0;
        }

        private static int CheckNoLfsPointers(string exportedData, ref int checkedFiles)
        {
            int errors = 0;
            const string pointerPrefix = "version https://git-lfs";
            foreach (var path in Directory.EnumerateFiles(exportedData, "*", SearchOption.AllDirectories))
            {
                var info = new FileInfo(path);
                if (info.Length < pointerPrefix.Length) continue;
                using var fs = info.OpenRead();
                var buf = new byte[pointerPrefix.Length];
                if (fs.Read(buf, 0, buf.Length) != buf.Length) continue;
                checkedFiles++;
                if (Encoding.ASCII.GetString(buf).StartsWith(pointerPrefix, StringComparison.Ordinal))
                {
                    GD.PrintErr($"[PARITY] Git-LFS pointer shipped instead of binary: {path}");
                    errors++;
                }
            }
            return errors;
        }

        private static string Sha256(string path)
        {
            using var sha = SHA256.Create();
            using var fs = File.OpenRead(path);
            return Convert.ToHexString(sha.ComputeHash(fs));
        }

        private static bool TryParseJson(string path)
        {
            try
            {
                using var doc = System.Text.Json.JsonDocument.Parse(File.ReadAllText(path));
                return doc.RootElement.ValueKind is System.Text.Json.JsonValueKind.Object
                    or System.Text.Json.JsonValueKind.Array;
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[PARITY] JSON parse error in {path}: {ex.Message}");
                return false;
            }
        }
    }
}
