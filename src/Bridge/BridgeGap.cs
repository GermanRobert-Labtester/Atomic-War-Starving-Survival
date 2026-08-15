using System;
using System.Collections.Generic;

namespace Ashfall.Bridge
{
    /// <summary>
    /// Failure policy for unimplemented members of the UnityEngine compatibility shim.
    ///
    /// The shim's structural hazard is that a member returning a plausible default converts a
    /// <em>compile error</em> (loud, cheap) into a <em>behavioural bug</em> (silent, expensive).
    /// Every hollow member is therefore classified into exactly one of three buckets:
    ///
    /// <list type="bullet">
    /// <item><see cref="Semantic"/> — silence would make game logic wrong (a coroutine that never
    /// runs, an "instantiated" object that is really the original, a save that never persists).
    /// These throw.</item>
    /// <item><see cref="Cosmetic"/> — the effect is audio/visual only and its absence is expected
    /// in a headless host. These log once and continue; throwing here would make the host unusable
    /// for the thing it exists to do.</item>
    /// <item>Genuine no-ops — members whose correct headless behaviour <em>is</em> to do nothing
    /// (no key is pressed, no button was clicked, nothing is destroyed on a scene load that never
    /// happens). These are left alone and carry a comment saying why.</item>
    /// </list>
    /// </summary>
    public static class BridgeGap
    {
        /// <summary>
        /// When true (the default) semantic gaps throw on first use. Set false to sweep a newly
        /// wired-up system and collect every gap it hits in one run via <see cref="Reported"/>,
        /// rather than fixing them one crash at a time.
        /// </summary>
        public static bool ThrowOnSemanticGap { get; set; } = true;

        private static readonly object s_lock = new object();
        private static readonly HashSet<string> s_reported = new HashSet<string>(StringComparer.Ordinal);

        /// <summary>Members that have been hit at least once, in first-hit order.</summary>
        public static IReadOnlyCollection<string> Reported
        {
            get { lock (s_lock) { return new List<string>(s_reported); } }
        }

        public static void ResetReported()
        {
            lock (s_lock) { s_reported.Clear(); }
        }

        /// <summary>Unimplemented and load-bearing: throws unless explicitly suppressed.</summary>
        public static void Semantic(string member, string consequence)
        {
            string message =
                $"UnityEngine shim: '{member}' is not implemented. {consequence} " +
                "Implement it in src/Bridge/ before relying on this call path.";

            if (ThrowOnSemanticGap)
            {
                throw new NotImplementedException(message);
            }

            ReportOnce(member, () => Godot.GD.PushError($"[bridge/semantic] {message}"));
        }

        /// <summary>Semantic gap in a value-returning member. Returns default if suppressed.</summary>
        public static T Semantic<T>(string member, string consequence)
        {
            Semantic(member, consequence);
            return default!;
        }

        /// <summary>Audio/visual only. Logged once per member, then silent.</summary>
        public static void Cosmetic(string member)
        {
            ReportOnce(member, () => Godot.GD.PushWarning(
                $"[bridge/cosmetic] UnityEngine shim: '{member}' does nothing in the headless " +
                "Godot host. Game logic is unaffected; this is reported once."));
        }

        private static void ReportOnce(string member, Action report)
        {
            lock (s_lock)
            {
                if (!s_reported.Add(member)) return;
            }
            report();
        }
    }
}
