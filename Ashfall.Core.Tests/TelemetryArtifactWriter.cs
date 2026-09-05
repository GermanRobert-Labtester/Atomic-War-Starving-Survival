// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;

namespace Ashfall.Core.Tests
{
    public static class TelemetryArtifactWriter
    {
        public static bool TryWriteLines(string directory, string filename, IEnumerable<string> lines)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(directory)) return false;
                Directory.CreateDirectory(directory);
                string path = Path.Combine(directory, filename);
                File.WriteAllLines(path, lines);
                return true;
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                Console.Error.WriteLine($"[TelemetryArtifactWriter] Failed to write '{filename}' in '{directory}': {ex.Message}");
                return false;
            }
        }
    }
}
