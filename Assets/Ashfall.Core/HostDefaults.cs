using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Ashfall.Core
{
    /// <summary>
    /// Default host adapters using the BCL. Godot and Unity may wrap these
    /// or replace them (e.g. GD.Print, user://, Application.streamingAssetsPath).
    /// </summary>
    public sealed class FileSystemIO : IFileIO
    {
        public bool DirectoryExists(string path) => Directory.Exists(path);

        public bool FileExists(string path) => File.Exists(path);

        public string ReadAllText(string path) => File.ReadAllText(path, Encoding.UTF8);

        public void WriteAllText(string path, string contents)
        {
            string dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            File.WriteAllText(path, contents ?? string.Empty, Encoding.UTF8);
        }

        public string Combine(params string[] parts) => Path.Combine(parts);
    }

    public sealed class SystemTextJsonSerializer : IJsonSerializer
    {
        public static readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            IncludeFields = true,
            PropertyNameCaseInsensitive = true,
            WriteIndented = false
        };

        public string Serialize<T>(T value) =>
            JsonSerializer.Serialize(value, Options);

        public T Deserialize<T>(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return default;
            return JsonSerializer.Deserialize<T>(json, Options);
        }
    }

    public sealed class ConsoleLog : ILog
    {
        public void Info(string message) => Console.WriteLine(message);
        public void Warn(string message) => Console.WriteLine("WARN " + message);
        public void Error(string message) => Console.Error.WriteLine("ERROR " + message);
    }

    public sealed class NullLog : ILog
    {
        public static readonly NullLog Instance = new NullLog();
        public void Info(string message) { }
        public void Warn(string message) { }
        public void Error(string message) { }
    }

    public sealed class SimClock : IClock
    {
        public int Day { get; private set; }

        public SimClock(int day = 1)
        {
            Day = day;
        }

        public void AdvanceDays(int days)
        {
            if (days < 0)
                throw new ArgumentOutOfRangeException(nameof(days));
            Day += days;
        }

        public void SetDay(int day)
        {
            if (day < 0)
                throw new ArgumentOutOfRangeException(nameof(day));
            Day = day;
        }
    }

    /// <summary>
    /// Integer-seeded System.Random. IceRoad window length does not use this;
    /// it uses a salt modulo so it stays identical across runtimes.
    /// </summary>
    public sealed class SeededRng : ISeededRng
    {
        private readonly Random _rng;

        public int Seed { get; }

        public SeededRng(int seed)
        {
            Seed = seed;
            _rng = new Random(seed);
        }

        public int Next(int minInclusive, int maxExclusive) =>
            _rng.Next(minInclusive, maxExclusive);

        public float NextFloat() => (float)_rng.NextDouble();

        public double NextDouble() => _rng.NextDouble();
    }

    /// <summary>
    /// Walks parents of <paramref name="startDirectory"/> looking for
    /// Assets/StreamingAssets/Data — the shared catalog authority.
    /// </summary>
    public static class CatalogLocator
    {
        public const string RelativeDataPath = "Assets/StreamingAssets/Data";

        public static bool TryFindDataDirectory(string startDirectory, out string dataDirectory)
        {
            dataDirectory = null;
            if (string.IsNullOrEmpty(startDirectory))
                return false;

            DirectoryInfo dir;
            try
            {
                dir = new DirectoryInfo(Path.GetFullPath(startDirectory));
            }
            catch (Exception)
            {
                return false;
            }

            while (dir != null)
            {
                string candidate = Path.Combine(dir.FullName, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(candidate))
                {
                    dataDirectory = candidate;
                    return true;
                }
                dir = dir.Parent;
            }

            return false;
        }

        public static void UseInvariantCulture()
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
        }
    }
}
