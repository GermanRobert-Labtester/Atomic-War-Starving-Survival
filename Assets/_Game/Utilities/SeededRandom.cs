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
        /// Last resort when no world seed is available (tests, partial hosts).
        /// Still better than unseeded wall-clock if a fixed salt is reused.
        /// </summary>
        public static Random CreateFixed(string context) => Create(0xA51FA11, context ?? "default");
    }
}
