using Godot;
using Ashfall.Core;
using System;
using System.IO;

namespace AtomicWar.GodotApp
{
    public sealed class GodotLog : ILog
    {
        private static string? s_logDirectory;
        private static string? s_logFilePath;
        private static readonly object s_lock = new object();

        public static string? LogDirectory => s_logDirectory;
        public static string? LogFilePath => s_logFilePath;

        public static void ConfigureLogDirectory(string? logDir)
        {
            if (string.IsNullOrWhiteSpace(logDir))
            {
                s_logDirectory = null;
                s_logFilePath = null;
                return;
            }

            try
            {
                Directory.CreateDirectory(logDir);
                s_logDirectory = logDir;
                s_logFilePath = Path.Combine(logDir, "ashfall_headless.log");
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[GodotLog] Failed to initialize log directory '{logDir}': {ex.Message}");
                s_logDirectory = null;
                s_logFilePath = null;
            }
        }

        private static void WriteToFile(string level, string message)
        {
            if (string.IsNullOrEmpty(s_logFilePath)) return;

            try
            {
                lock (s_lock)
                {
                    string entry = $"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] [{level}] {message}{System.Environment.NewLine}"; // DETERMINISM_ALLOWLIST: Host diagnostic log timestamp
                    File.AppendAllText(s_logFilePath, entry);
                }
            }
            catch
            {
                // Fallback: file write failure must never crash the host process
            }
        }

        public void Info(string message)
        {
            GD.Print(message);
            WriteToFile("INFO", message);
        }

        public void Warn(string message)
        {
            GD.PushWarning(message);
            WriteToFile("WARN", message);
        }

        public void Error(string message)
        {
            GD.PrintErr(message);
            WriteToFile("ERROR", message);
        }
    }
}
