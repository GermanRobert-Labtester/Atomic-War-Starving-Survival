using System;

namespace AtomicWar._Game.Utilities
{
    /// <summary>
    /// Deterministic RNG helpers so call sites never fall back to wall-clock
    /// <see cref="Random"/> (MISC-005). Prefer salting the campaign world seed
    /// with a stable context string so each system has an independent stream.
    /// </summary>
    public static class SeededRandom
    {
        /// <summary>
        /// Global campaign seed. Set by GameBootstrap at startup so every
        /// <see cref="CreateFixed"/> fallback produces a campaign-specific
        /// stream instead of the same sequence every run.
        /// When -1 (default), falls back to the legacy fixed seed for backward
        /// compatibility with tests and partial hosts.
        /// </summary>
        public static int WorldSeed { get; set; } = -1;

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
            int seed = WorldSeed >= 0 ? WorldSeed : 0xA51FA11;
            return Create(seed, context ?? "default");
        }
    }
}
