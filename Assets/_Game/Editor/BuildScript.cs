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

            // 2. Standalone Builds (Windows & Mac)
            AppendLog("\n--- STEP 2: COMPILING STANDALONE BUILDS ---");

            string[] scenes = GetBuildScenes();
            if (scenes.Length == 0)
            {
                AppendLog("[WARN] No scenes enabled in Build Settings.");
            }

            // Build Windows Standalone
            bool winOk = BuildWindows(scenes);

            // Build Mac Standalone
            bool macOk = BuildMac(scenes);

            if (winOk && macOk)
            {
                AppendLog("\n=== ASHFALL BUILD PIPELINE COMPLETED SUCCESSFULLY ===");
                Debug.Log("[BuildPipeline] Build pipeline completed successfully.");
            }
            else
            {
                AppendLog("\n=== BUILD PIPELINE HALTED DUE TO COMPILATION FAILURE ===");
                if (Application.isBatchMode && !IsRunningAutomatedTests())
                {
                    EditorApplication.Exit(1);
                }
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

        public static bool BuildWindows(string[] scenes)
        {
            if (!BuildPipeline.IsBuildTargetSupported(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64))
            {
                AppendLog("[SKIP] Windows build target (StandaloneWindows64) is not installed on this Editor instance.");
                return true;
            }

            string path = "Builds/Windows/ASHFALL.exe";
            EnsureDirectoryForFile(path);

            var report = BuildPipeline.BuildPlayer(scenes, path, BuildTarget.StandaloneWindows64, BuildOptions.None);
            if (report.summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded)
            {
                AppendLog($"[PASS] Windows build succeeded: {path} ({report.summary.totalSize} bytes)");
                return true;
            }
            else
            {
                AppendLog($"[FAIL] Windows build failed: {report.summary.result}");
                return false;
            }
        }

        public static bool BuildMac(string[] scenes)
        {
            if (!BuildPipeline.IsBuildTargetSupported(BuildTargetGroup.Standalone, BuildTarget.StandaloneOSX))
            {
                AppendLog("[SKIP] Mac build target (StandaloneOSX) is not installed on this Editor instance.");
                return true;
            }

            string path = "Builds/Mac/ASHFALL.app";
            EnsureDirectoryForFile(path);

            var report = BuildPipeline.BuildPlayer(scenes, path, BuildTarget.StandaloneOSX, BuildOptions.None);
            if (report.summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded)
            {
                AppendLog($"[PASS] Mac build succeeded: {path} ({report.summary.totalSize} bytes)");
                return true;
            }
            else
            {
                AppendLog($"[FAIL] Mac build failed: {report.summary.result}");
                return false;
            }
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
