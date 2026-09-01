namespace Ashfall.Core
{
    /// <summary>
    /// Engine-agnostic JSON port. Unity JsonUtility is banned from this assembly.
    /// A save written through this port must load in both hosts.
    /// </summary>
    public interface IJsonSerializer
    {
        string Serialize<T>(T value);
        T? Deserialize<T>(string json) where T : class;
    }

    /// <summary>
    /// Text/file access for catalogs and saves. Hosts implement this against
    /// StreamingAssets, res://, or a debug filesystem path.
    /// </summary>
    public interface IFileIO
    {
        bool DirectoryExists(string path);
        bool FileExists(string path);
        string ReadAllText(string path);
        void WriteAllText(string path, string contents);
        string Combine(params string[] parts);

        void CreateDirectory(string path)
        {
            if (string.IsNullOrEmpty(path)) return;
            try { System.IO.Directory.CreateDirectory(path); } catch { /* cleanup: best-effort directory creation */ }
        }

        void DeleteFile(string path)
        {
            if (string.IsNullOrEmpty(path)) return;
            try { if (System.IO.File.Exists(path)) System.IO.File.Delete(path); } catch { /* cleanup: best-effort file deletion */ }
        }

        /// <summary>
        /// Enumerate files matching a search pattern. Default fallback uses System.IO.
        /// Hosts that virtualize paths (e.g., Godot res:// PCK) should override.
        /// </summary>
        string[] EnumerateFiles(string directory, string searchPattern, System.IO.SearchOption searchOption)
        {
            if (string.IsNullOrEmpty(directory)) return new string[0];
            if (!DirectoryExists(directory)) return new string[0];
            try
            {
                return System.IO.Directory.GetFiles(directory, searchPattern, searchOption);
            }
            catch
            {
                return new string[0];
            }
        }
    }

    public interface ILog
    {
        void Info(string message);
        void Warn(string message);
        void Error(string message);
    }

    /// <summary>Simulation calendar. Never DateTime.Now.</summary>
    public interface IClock
    {
        int Day { get; }
        void AdvanceDays(int days);
        void SetDay(int day);
    }

    /// <summary>Seeded RNG. Same seed must yield the same sequence in both hosts.</summary>
    public interface ISeededRng
    {
        int Seed { get; }
        int Next(int minInclusive, int maxExclusive);
        float NextFloat();
        double NextDouble();
    }
}
