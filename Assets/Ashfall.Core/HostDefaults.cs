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
            string? dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            File.WriteAllText(path, contents ?? string.Empty, Encoding.UTF8);
        }

        public string Combine(params string[] parts) => Path.Combine(parts);

        /// <summary>
        /// BCL implementation of JSON catalog enumeration.
        /// </summary>
        public string[] EnumerateJsonFiles(string dataDirectory, SearchOption searchOption)
        {
            return Directory.GetFiles(dataDirectory, "*.json", searchOption);
        }
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

        public T? Deserialize<T>(string json) where T : class
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
    /// Deterministic PRNG using xorshift64*. Same seed produces identical
    /// sequences on every .NET runtime/version, satisfying the cross-engine
    /// determinism invariant. Do NOT replace with System.Random.
    /// </summary>
    public sealed class SeededRng : ISeededRng
    {
        private ulong _state;

        public int Seed { get; }

        public SeededRng(int seed)
        {
            Seed = seed;
            // SplitMix64 initializer: spreads a small seed across all 64 bits.
            _state = (ulong)(seed ^ (long)((uint)seed >> 30)) * 0xbf58476d1ce4e5b9UL;
            _state = (ulong)((long)_state ^ (long)((uint)_state >> 27)) * 0x94d049bb133111ebUL;
            _state = (ulong)((long)_state ^ (long)((uint)_state >> 31));
        }

        public int Next(int minInclusive, int maxExclusive)
        {
            if (minInclusive >= maxExclusive)
                throw new ArgumentOutOfRangeException(nameof(maxExclusive), "maxExclusive must be greater than minInclusive");
            long range = (long)maxExclusive - minInclusive;
            ulong v = NextRaw();
            // Modulo bias is negligible for game-use ranges and avoids int overflow.
            return minInclusive + (int)(v % (ulong)range);
        }

        public float NextFloat() => (float)NextDouble();

        public double NextDouble() => NextRaw() / (double)ulong.MaxValue;

        private ulong NextRaw()
        {
            // xorshift64* — deterministic, fast, single-state.
            _state ^= _state >> 12;
            _state ^= _state << 25;
            _state ^= _state >> 27;
            return _state * 0x2545F4914F6CDD1DUL;
        }
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
            dataDirectory = null!;
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
