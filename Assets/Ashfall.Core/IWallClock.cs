using System;
using System.Globalization;

namespace Ashfall.Core
{
    /// <summary>
    /// Host wall-clock interface for non-deterministic metadata, diagnostic logs,
    /// and file timestamps. Wall-clock values must NEVER drive simulation state,
    /// campaign day progression, or deterministic simulation checksums.
    /// </summary>
    public interface IWallClock
    {
        DateTime UtcNow { get; }
        long UtcTicks { get; }
        string FormatIsoUtc();
        string FormatFileTimestamp();
    }

    /// <summary>
    /// Default wall-clock implementation using system UTC with invariant culture.
    /// </summary>
    public sealed class SystemWallClock : IWallClock
    {
        public static readonly SystemWallClock Instance = new SystemWallClock();

        public DateTime UtcNow => DateTime.UtcNow; // DETERMINISM_ALLOWLIST: Port adapter for host wall clock
        public long UtcTicks => DateTime.UtcNow.Ticks; // DETERMINISM_ALLOWLIST: Port adapter for host wall clock
        public string FormatIsoUtc() => DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture); // DETERMINISM_ALLOWLIST: Port adapter for host wall clock
        public string FormatFileTimestamp() => DateTime.UtcNow.ToString("yyyyMMdd-HHmmss", CultureInfo.InvariantCulture); // DETERMINISM_ALLOWLIST: Port adapter for host wall clock
    }

    /// <summary>
    /// Frozen / controllable wall-clock for unit tests, headless simulations,
    /// and reproducible metadata verification.
    /// </summary>
    public sealed class FrozenWallClock : IWallClock
    {
        private DateTime _time;

        public FrozenWallClock(DateTime time)
        {
            _time = time.Kind == DateTimeKind.Utc ? time : time.ToUniversalTime();
        }

        public DateTime UtcNow => _time;
        public long UtcTicks => _time.Ticks;
        public string FormatIsoUtc() => _time.ToString("o", CultureInfo.InvariantCulture);
        public string FormatFileTimestamp() => _time.ToString("yyyyMMdd-HHmmss", CultureInfo.InvariantCulture);

        public void SetTime(DateTime time)
        {
            _time = time.Kind == DateTimeKind.Utc ? time : time.ToUniversalTime();
        }

        public void Advance(TimeSpan delta)
        {
            _time = _time.Add(delta);
        }
    }
}
