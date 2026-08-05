#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AtomicWar._Game.Editor
{
    /// <summary>
    /// Editor script to run tests, validate JSON data referential integrity,
    /// compile standalone Windows and Mac executables, and log output to build_log.txt.
    /// </summary>
    public static class BuildScript
    {
        public const string LogFilePath = "build_log.txt";

        /// <summary>
        /// Command-line / CI batchmode entry point:
        /// -executeMethod AtomicWar._Game.Editor.BuildScript.PerformBuildPipeline
        /// </summary>
        public static void PerformBuildPipeline()
        {
            File.WriteAllText(LogFilePath, $"=== ASHFALL BUILD PIPELINE STARTED AT {DateTime.UtcNow:u} ===\n");
            AppendLog($"Unity Version: {Application.unityVersion}");

            // 1. Data Validation Gate (#8)
            AppendLog("\n--- STEP 1: JSON DATA VALIDATION GATE ---");
            List<string> validationErrors = JsonDataImporter.ValidateAll();

            if (validationErrors != null && validationErrors.Count > 0)
            {
                AppendLog($"[FAIL] Data Validation Gate FAILED with {validationErrors.Count} error(s):");
                foreach (var error in validationErrors)
                {
                    AppendLog($"  ERROR: {error}");
                }
                AppendLog("\n=== BUILD PIPELINE HALTED DUE TO DATA VALIDATION FAILURE ===");
                Debug.LogError($"[BuildPipeline] Data Validation Failed:\n{string.Join("\n", validationErrors)}");
                // Never quit the Editor during EditMode/PlayMode test runs — Exit aborts the suite.
                if (Application.isBatchMode && !IsRunningAutomatedTests())
                {
                    EditorApplication.Exit(1);
                }
                return;
            }

            AppendLog("[PASS] JSON Data Validation passed with 0 errors.");

            // 2. Standalone Builds
            AppendLog("\n--- STEP 2: COMPILING STANDALONE BUILDS ---");

            string[] scenes = GetBuildScenes();
            if (scenes.Length == 0)
            {
                // A player with no scenes is not a build worth shipping, so this
                // is an error rather than a warning.
                AppendLog("[FAIL] No scenes enabled in Build Settings — nothing to build.");
                Halt();
                return;
            }

            var outcomes = new[]
            {
                BuildTargetIfSupported("Windows", BuildTarget.StandaloneWindows64, "Builds/Windows/ASHFALL.exe", scenes),
                BuildTargetIfSupported("Mac", BuildTarget.StandaloneOSX, "Builds/Mac/ASHFALL.app", scenes),
                // Linux was missing entirely, even though it is the platform this
                // project is developed and tested on and the only build support
                // module installed locally.
                BuildTargetIfSupported("Linux", BuildTarget.StandaloneLinux64, "Builds/Linux/ASHFALL.x86_64", scenes),
            };

            int failed = outcomes.Count(o => o == BuildOutcome.Failed);
            int built = outcomes.Count(o => o == BuildOutcome.Succeeded);

            if (failed > 0)
            {
                AppendLog("\n=== BUILD PIPELINE HALTED DUE TO COMPILATION FAILURE ===");
                Halt();
                return;
            }

            // Previously every uninstalled target returned "ok", so on a machine
            // with no build support modules the pipeline printed COMPLETED
            // SUCCESSFULLY and exited 0 having produced zero artifacts -- a green
            // signal for a build that never happened. Verified: a local run
            // skipped both Windows and Mac and still reported success.
            if (built == 0)
            {
                AppendLog("\n=== BUILD PIPELINE PRODUCED NO ARTIFACTS ===");
                AppendLog("Every target was skipped because no build support module is installed.");
                AppendLog("Install a platform module, or this pipeline cannot verify anything.");
                Debug.LogError("[BuildPipeline] No artifacts produced — every build target was skipped.");
                Halt();
                return;
            }

            AppendLog($"\n=== ASHFALL BUILD PIPELINE COMPLETED SUCCESSFULLY ({built} artifact(s)) ===");
            Debug.Log($"[BuildPipeline] Build pipeline completed successfully ({built} artifact(s)).");
        }

        /// <summary>Outcome of a single platform build attempt.</summary>
        private enum BuildOutcome { Skipped, Succeeded, Failed }

        /// <summary>
        /// Exit non-zero in batchmode, but never while the test suite is running
        /// (EditorApplication.Exit would abort the whole run).
        /// </summary>
        private static void Halt()
        {
            if (Application.isBatchMode && !IsRunningAutomatedTests())
            {
                EditorApplication.Exit(1);
            }
        }

        /// <summary>
        /// True when Unity was launched with -runTests (EditMode/PlayMode suite).
        /// PerformBuildPipeline must not call EditorApplication.Exit in that case.
        /// </summary>
        private static bool IsRunningAutomatedTests()
        {
            string[] args = System.Environment.GetCommandLineArgs();
            for (int i = 0; i < args.Length; i++)
            {
                if (string.Equals(args[i], "-runTests", System.StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Build one standalone target, reporting whether it actually produced an
        /// artifact. A missing build support module is Skipped, not Succeeded --
        /// the distinction is the whole point: treating "not installed" as success
        /// is what let the pipeline report green with nothing built.
        /// </summary>
        private static BuildOutcome BuildTargetIfSupported(
            string label, BuildTarget target, string path, string[] scenes)
        {
            if (!BuildPipeline.IsBuildTargetSupported(BuildTargetGroup.Standalone, target))
            {
                AppendLog($"[SKIP] {label} build target ({target}) is not installed on this Editor instance.");
                return BuildOutcome.Skipped;
            }

            EnsureDirectoryForFile(path);

            var report = BuildPipeline.BuildPlayer(scenes, path, target, BuildOptions.None);
            if (report.summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded)
            {
                AppendLog($"[PASS] {label} build succeeded: {path} ({report.summary.totalSize} bytes)");
                return BuildOutcome.Succeeded;
            }

            AppendLog($"[FAIL] {label} build failed: {report.summary.result}");
            return BuildOutcome.Failed;
        }

        private static string[] GetBuildScenes()
        {
            return EditorBuildSettings.scenes
                .Where(s => s.enabled && !string.IsNullOrEmpty(s.path))
                .Select(s => s.path)
                .ToArray();
        }

        private static void EnsureDirectoryForFile(string filePath)
        {
            string dir = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        private static void AppendLog(string message)
        {
            File.AppendAllText(LogFilePath, message + "\n");
        }
    }
}
#endif
