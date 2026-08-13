namespace Ashfall.Core
{
    /// <summary>
    /// Engine-agnostic JSON port. Unity JsonUtility is banned from this assembly.
    /// A save written through this port must load in both hosts.
    /// </summary>
    public interface IJsonSerializer
    {
        string Serialize<T>(T value);
        T Deserialize<T>(string json);
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
    }
}
