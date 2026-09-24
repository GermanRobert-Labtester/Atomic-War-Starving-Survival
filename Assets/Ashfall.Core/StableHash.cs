// SPDX-License-Identifier: MIT
using System;

namespace Ashfall.Core
{
    /// <summary>
    /// Deterministic, runtime-stable string hashing for simulation keys and
    /// dedup ids. djb2/x33 — deliberately NOT string.GetHashCode(), which is
    /// randomized per process in modern .NET and would break the cross-host
    /// determinism invariant (same seed ⇒ same simulation in both engines).
    /// Engine-agnostic; safe for save-derived keys.
    /// </summary>
    public static class StableHash
    {
        public static int Of(string? value)
        {
            if (string.IsNullOrEmpty(value)) return 0;
            unchecked
            {
                int h = 5381;
                for (int i = 0; i < value.Length; i++)
                    h = ((h << 5) + h) ^ value[i];
                return h;
            }
        }

        /// <summary>
        /// Combines an integer seed with another integer using a fixed,
        /// process-stable mix. Use this instead of <see cref="System.HashCode"/>
        /// for any value that can influence simulation, persistence, or replay.
        /// </summary>
        public static int Combine(int seed, int value)
        {
            unchecked
            {
                // Boosted combine followed by the MurmurHash3 finalizer. The
                // constants and arithmetic are fixed, so the result is stable
                // across processes, runtimes, and Godot/.NET hosts.
                uint mixed = (uint)seed;
                mixed ^= (uint)value + 0x9E3779B9u + (mixed << 6) + (mixed >> 2);
                mixed ^= mixed >> 16;
                mixed *= 0x7FEB352Du;
                mixed ^= mixed >> 15;
                mixed *= 0x846CA68Bu;
                mixed ^= mixed >> 16;
                return (int)mixed;
            }
        }

        /// <summary>Combines a seed with a 64-bit value without boxing.</summary>
        public static int Combine(int seed, long value) =>
            Combine(Combine(seed, unchecked((int)value)), unchecked((int)(value >> 32)));

        /// <summary>Combines a seed with canonical text without platform string hashing.</summary>
        public static int Combine(int seed, string? value) => Combine(seed, Of(value));

        /// <summary>
        /// Returns a zero-based non-negative remainder. Unlike
        /// <c>Math.Abs(value) % modulus</c>, this is safe for
        /// <see cref="int.MinValue"/> because it never negates the value.
        /// </summary>
        public static int NonNegativeRemainder(int value, int modulus)
        {
            if (modulus <= 0)
                throw new ArgumentOutOfRangeException(nameof(modulus), "Modulus must be positive.");

            int remainder = value % modulus;
            return remainder < 0 ? remainder + modulus : remainder;
        }
    }
}
