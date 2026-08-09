using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Utilities
{
    /// <summary>
    /// Deterministic RNG helpers so call sites never fall back to wall-clock
    /// <see cref="Random"/> (MISC-005). Prefer salting the campaign world seed
    /// with a stable context string so each system has an independent stream.
    /// </summary>
    public static class SeededRandom
    {
        /// <summary>Seed used when no campaign seed has been injected yet.</summary>
        private const int LegacySeed = 0xA51FA11;

        private static int _worldSeed = -1;

        /// <summary>
        /// Global campaign seed. Set by GameBootstrap at startup so every
        /// <see cref="CreateFixed"/> fallback produces a campaign-specific
        /// stream instead of the same sequence every run.
        /// When -1 (default), falls back to the legacy fixed seed for backward
        /// compatibility with tests and partial hosts.
        ///
        /// Assigning always rewinds the <see cref="Stream"/> registry, since a
        /// seed assignment marks the start of a run.
        /// </summary>
        public static int WorldSeed
        {
            get => _worldSeed;
            set
            {
                _worldSeed = value;
                ResetStreams();
            }
        }

        /// <summary>
        /// Long-lived per-context streams. These outlive a single campaign
        /// because they are process-static, so they must be rewound whenever a
        /// run begins — see <see cref="ResetStreams"/>.
        /// </summary>
        private static readonly Dictionary<string, Random> Streams =
            new Dictionary<string, Random>(StringComparer.Ordinal);

        /// <summary>
        /// The persistent fallback stream for <paramref name="context"/>: the
        /// same <see cref="Random"/> instance every call, so successive rolls
        /// advance it.
        ///
        /// Prefer this over <see cref="CreateFixed"/> for a system's
        /// "no rng was injected" path. CreateFixed returns a *fresh* stream, so
        /// calling it per roll re-seeds identically every time and every roll
        /// comes back with the same value. CreateFixed is still correct where
        /// the context itself is the key (one reproducible stream per map node,
        /// say), which is why both exist.
        /// </summary>
        public static Random Stream(string context)
        {
            string key = context ?? "default";
            lock (Streams)
            {
                if (!Streams.TryGetValue(key, out var rng))
                {
                    rng = Create(_worldSeed >= 0 ? _worldSeed : LegacySeed, key);
                    Streams[key] = rng;
                }
                return rng;
            }
        }

        /// <summary>
        /// Drop every cached <see cref="Stream"/> so the next access re-seeds
        /// from the current <see cref="WorldSeed"/> at position zero. Called on
        /// seed assignment and on save restore: without it, loading the same
        /// slot twice in one process resumes mid-stream and rolls differently
        /// the second time.
        /// </summary>
        public static void ResetStreams()
        {
            lock (Streams) { Streams.Clear(); }
        }

        /// <summary>
        /// Combine a world seed with a context label into a stable 32-bit seed.
        /// Same inputs always produce the same stream; different labels diverge.
        /// </summary>
        public static int Mix(int worldSeed, string context)
        {
            unchecked
            {
                int h = worldSeed;
                if (!string.IsNullOrEmpty(context))
                {
                    for (int i = 0; i < context.Length; i++)
                        h = (h * 397) ^ context[i];
                }
                // Avoid 0 which some Random implementations treat specially.
                if (h == 0) h = 1;
                return h;
            }
        }

        /// <summary>New Random from world seed + context salt.</summary>
        public static Random Create(int worldSeed, string context) =>
            new Random(Mix(worldSeed, context));

        /// <summary>
        /// Return <paramref name="rng"/> if non-null; otherwise a salted stream.
        /// Prefer passing a seeded rng from the host; this is the last-resort path
        /// that used to be <c>new Random()</c>.
        /// </summary>
        public static Random OrCreate(Random rng, int worldSeed, string context) =>
            rng ?? Create(worldSeed, context);

        /// <summary>
        /// Last resort when no world seed is injected. Uses
        /// <see cref="WorldSeed"/> if set (campaign-specific), otherwise
        /// falls back to a fixed legacy seed (tests/partial hosts).
        /// </summary>
        public static Random CreateFixed(string context)
        {
            int seed = _worldSeed >= 0 ? _worldSeed : LegacySeed;
            return Create(seed, context ?? "default");
        }
    }
}
