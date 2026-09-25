// SPDX-License-Identifier: MIT
using System;
using System.IO;
using Godot;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Local JSONL sink for the playable-metrics recorder.
    ///
    /// The recorder (Ashfall.Core.Telemetry.PlaySessionRecorder) is a bounded,
    /// in-memory audit buffer. This sink subscribes to its per-event seam and
    /// appends one JSON row per event to <c>user://play_metrics.jsonl</c> so a
    /// real playtest can be analysed by
    /// <c>scripts/tools/first_hour_funnel.py --discover</c>.
    ///
    /// Audit-only: never feeds gameplay, never blocks the caller. Every
    /// operation is wrapped so a disk problem cannot break a session; the sink
    /// does not drain the recorder buffer, so the registered
    /// <c>playable_metrics</c> save section and end-of-campaign report keep
    /// their data.
    /// </summary>
    public static class PlayMetricJsonlSink
    {
        /// <summary>Canonical sink path, discovered by the funnel tool.</summary>
        public const string UserPath = "user://play_metrics.jsonl";

        /// <summary>
        /// Test/CI override. When set, rows go to this path instead of
        /// <see cref="UserPath"/> so selftests never pollute real playtest data.
        /// </summary>
        public static string? PathOverride { get; set; }

        private static string SinkPath => PathOverride ?? UserPath;

        private static readonly object _lock = new object();

        /// <summary>Absolute path for diagnostics/tools.</summary>
        public static string GlobalizedPath => ProjectSettings.GlobalizePath(SinkPath);

        /// <summary>
        /// Append one JSONL row. Creates the file on first use and appends
        /// thereafter; a malformed/empty row is ignored.
        /// </summary>
        public static void AppendLine(string? jsonl)
        {
            if (string.IsNullOrWhiteSpace(jsonl)) return;
            lock (_lock)
            {
                try
                {
                    if (!Godot.FileAccess.FileExists(SinkPath))
                    {
                        using var created = Godot.FileAccess.Open(SinkPath, Godot.FileAccess.ModeFlags.Write);
                        created?.StoreLine(jsonl);
                        return;
                    }

                    using var file = Godot.FileAccess.Open(SinkPath, Godot.FileAccess.ModeFlags.ReadWrite);
                    if (file == null) return;
                    file.SeekEnd();
                    file.StoreLine(jsonl);
                }
                catch (Exception)
                {
                    // Local audit only — a failed append must never surface in play.
                }
            }
        }

        /// <summary>Delete the sink file (used by selftests for a clean run).</summary>
        public static void Reset()
        {
            lock (_lock)
            {
                try
                {
                    if (Godot.FileAccess.FileExists(SinkPath))
                        DirAccess.RemoveAbsolute(ProjectSettings.GlobalizePath(SinkPath));
                }
                catch (Exception)
                {
                    // best effort only
                }
            }
        }

        /// <summary>Number of rows currently in the sink (0 when absent).</summary>
        public static int RowCount()
        {
            lock (_lock)
            {
                try
                {
                    string path = ProjectSettings.GlobalizePath(SinkPath);
                    if (!File.Exists(path)) return 0;
                    int rows = 0;
                    foreach (string line in File.ReadLines(path))
                    {
                        if (!string.IsNullOrWhiteSpace(line)) rows++;
                    }
                    return rows;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }
    }
}
