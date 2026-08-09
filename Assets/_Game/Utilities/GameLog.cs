using UnityEngine;
using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.Utilities
{
    /// <summary>
    /// A-4: Conditional logging helper. Methods are marked with
    /// [Conditional("ASHFALL_DEBUG")] so the compiler strips all call sites
    /// in release builds (no string allocation, no method call overhead).
    ///
    /// To enable verbose logging in development:
    ///   1. Add "ASHFALL_DEBUG" to Project Settings → Player → Scripting Define Symbols
    ///   2. Or add to Editor/ProjectSettings/ProjectSettings.asset
    ///
    /// In release builds, all Log() calls are compiled away.
    /// LogError and LogWarning are NOT conditional — they always execute
    /// because failures must be visible in production.
    /// </summary>
    public static class GameLog
    {
        [System.Diagnostics.Conditional("ASHFALL_DEBUG")]
        public static void Log(string message)
        {
            GameLog.Log(message);
        }

        [System.Diagnostics.Conditional("ASHFALL_DEBUG")]
        public static void Log(string tag, string message)
        {
            GameLog.Log($"[{tag}] {message}");
        }

        public static void LogError(string message)
        {
            Debug.LogError(message);
        }

        public static void LogWarning(string message)
        {
            Debug.LogWarning(message);
        }

        public static void LogError(string tag, string message)
        {
            Debug.LogError($"[{tag}] {message}");
        }

        public static void LogWarning(string tag, string message)
        {
            Debug.LogWarning($"[{tag}] {message}");
        }
    }
}
