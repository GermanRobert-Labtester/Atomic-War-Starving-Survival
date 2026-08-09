using System;
using System.IO;
using UnityEngine;
using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// A-11: Log rotation for Player.log / Editor.log.
    ///
    /// Unity appends to the same Player.log file across sessions. In long
    /// play sessions or after many launches, the file can grow to hundreds
    /// of megabytes. This component:
    ///
    ///  1. On startup, checks the active log file size.
    ///  2. If it exceeds MaxLogSizeBytes, archives the previous content
    ///     to a timestamped .bak file and truncates the current log.
    ///  3. Deletes archive files older than MaxArchiveAgeDays.
    ///
    /// Truncation uses a FileStream with FileShare.ReadWrite so it works
    /// even while Unity has the log open for writing. If truncation fails
    /// (e.g., file locked on some platforms), it falls back to logging a
    /// warning and leaving the file as-is.
    ///
    /// This component should execute early. Place it on the same
    /// GameObject as GameBootstrap or set ScriptExecutionOrder to -100.
    /// </summary>
    [DefaultExecutionOrder(-100)]
    public class LogRotationManager : MonoBehaviour
    {
        [Header("Log Rotation")]
        [Tooltip("Maximum log file size in megabytes before archiving.")]
        [Min(1)]
        public int MaxLogSizeMB = 50;

        [Tooltip("Archive files older than this (in days) are deleted on startup.")]
        [Min(1)]
        public int MaxArchiveAgeDays = 7;

        [Tooltip("Maximum number of archive files to keep. Oldest are deleted first.")]
        [Min(1)]
        public int MaxArchiveCount = 5;

        private void Awake()
        {
            try
            {
                string logPath = GetLogPath();
                if (string.IsNullOrEmpty(logPath) || !File.Exists(logPath))
                    return;

                long sizeBytes = new FileInfo(logPath).Length;
                long maxSizeBytes = (long)MaxLogSizeMB * 1024 * 1024;

                if (sizeBytes > maxSizeBytes)
                {
                    ArchiveLog(logPath, sizeBytes);
                    TruncateLog(logPath);
                }

                string baseName = Path.GetFileNameWithoutExtension(logPath);
                CleanOldArchives(Path.GetDirectoryName(logPath), MaxArchiveAgeDays, MaxArchiveCount, baseName);
            }
            catch (Exception ex)
            {
                // Log rotation must never crash the game.
                Debug.LogWarning($"[LogRotation] Failed to rotate log: {ex.Message}");
            }
        }

        /// <summary>
        /// Get the Player.log / Editor.log path for the current platform.
        /// </summary>
        private static string GetLogPath()
        {
            // Unity's log file location depends on platform and whether
            // we're in the editor or a standalone build. Single return keeps
            // complexity metrics stable across #if branches.
            string path;
#if UNITY_EDITOR
#if UNITY_EDITOR_WIN
            path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData),
                "Unity", "Editor", "Editor.log");
#elif UNITY_EDITOR_OSX
            path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),
                "Library", "Logs", "Unity", "Editor.log");
#else
            path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),
                ".config", "unity3d", "Editor.log");
#endif
#else
            string companyName = Application.companyName;
            string productName = Application.productName;
#if UNITY_STANDALONE_WIN
            path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData),
                "Low", companyName, productName, "Player.log");
#elif UNITY_STANDALONE_OSX
            path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),
                "Library", "Logs", companyName, productName, "Player.log");
#else
            path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),
                ".config", "unity3d", companyName, productName, "Player.log");
#endif
#endif
            return path;
        }

        /// <summary>
        /// Copy the current log to a timestamped archive file.
        /// </summary>
        public static void ArchiveLog(string logPath, long sizeBytes)
        {
            string dir = Path.GetDirectoryName(logPath);
            string baseName = Path.GetFileNameWithoutExtension(logPath);
            string ext = Path.GetExtension(logPath);
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string archivePath = Path.Combine(dir, $"{baseName}_archive_{timestamp}{ext}");

            File.Copy(logPath, archivePath, overwrite: true);
            GameLog.Log($"[LogRotation] Archived log ({sizeBytes / 1024 / 1024.0:F1} MB) to: {archivePath}");
        }

        /// <summary>
        /// Truncate the log file to zero bytes. Uses FileShare.ReadWrite
        /// so it works even while Unity has the file open for writing.
        /// </summary>
        public static void TruncateLog(string logPath)
        {
            try
            {
                // Open with ReadWrite sharing so we don't conflict with Unity's writer.
                using (var fs = new FileStream(logPath, FileMode.Truncate,
                    FileAccess.Write, FileShare.ReadWrite))
                {
                    fs.SetLength(0);
                    fs.Flush();
                }
                GameLog.Log("[LogRotation] Truncated active log file.");
            }
            catch (IOException)
            {
                // File is locked — can't truncate. The archive copy is still valid.
                Debug.LogWarning("[LogRotation] Could not truncate log (file locked). Archive copy saved.");
            }
            catch (UnauthorizedAccessException)
            {
                Debug.LogWarning("[LogRotation] Could not truncate log (permission denied). Archive copy saved.");
            }
        }

        /// <summary>
        /// Delete archive files older than maxAgeDays, and keep at
        /// most maxArchives archives.
        /// </summary>
        public static void CleanOldArchives(string dir, int maxAgeDays, int maxArchives, string baseName)
        {
            if (!Directory.Exists(dir)) return;

            string ext = ".log";
            string searchPattern = $"{baseName}_archive_*{ext}";

            var files = Directory.GetFiles(dir, searchPattern);
            var archiveFiles = new System.Collections.Generic.List<(string path, DateTime time)>();

            for (int i = 0; i < files.Length; i++)
            {
                try
                {
                    var info = new FileInfo(files[i]);
                    archiveFiles.Add((files[i], info.LastWriteTime));
                }
                catch
                {
                    // Skip unreadable files.
                }
            }

            // Sort oldest-first.
            archiveFiles.Sort((a, b) => a.time.CompareTo(b.time));

            DateTime cutoff = DateTime.Now.AddDays(-maxAgeDays);
            int deleted = 0;

            for (int i = 0; i < archiveFiles.Count; i++)
            {
                bool tooOld = archiveFiles[i].time < cutoff;
                bool tooMany = i < archiveFiles.Count - maxArchives;

                if (tooOld || tooMany)
                {
                    try
                    {
                        File.Delete(archiveFiles[i].path);
                        deleted++;
                    }
                    catch
                    {
                        // Skip undeletable files.
                    }
                }
            }

            if (deleted > 0)
                GameLog.Log($"[LogRotation] Deleted {deleted} old archive file(s).");
        }
    }
}
