using System;
using System.IO;
using System.Text;

namespace AtomicWar._Game.Utilities
{
    /// <summary>Session debug ingest for Cursor debug mode. Remove after the audit.</summary>
    public static class AgentDebugLog
    {
        const string LogPath = "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/.cursor/debug-06085b.log";

        public static void Write(string hypothesisId, string location, string message, string dataJsonObject)
        {
            try
            {
                string dir = Path.GetDirectoryName(LogPath);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                long ts = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                var sb = new StringBuilder(256);
                sb.Append("{\"sessionId\":\"06085b\",\"hypothesisId\":\"");
                sb.Append(hypothesisId);
                sb.Append("\",\"location\":\"");
                sb.Append(location);
                sb.Append("\",\"message\":\"");
                sb.Append(Escape(message));
                sb.Append("\",\"data\":");
                sb.Append(string.IsNullOrEmpty(dataJsonObject) ? "{}" : dataJsonObject);
                sb.Append(",\"timestamp\":");
                sb.Append(ts);
                sb.Append("}\n");
                File.AppendAllText(LogPath, sb.ToString());
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogWarning("[AgentDebugLog] write failed: " + ex.Message);
            }
        }

        static string Escape(string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            return s.Replace("\\", "\\\\").Replace("\"", "\\\"");
        }
    }
}
