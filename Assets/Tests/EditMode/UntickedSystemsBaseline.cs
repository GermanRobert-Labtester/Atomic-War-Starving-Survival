using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Accessor for the checked-in list of systems that GameBootstrap constructs and
    /// save-wires but never drives (the "C-1" class of bug).
    ///
    /// This exists because the population was large enough that a plain
    /// <c>Assert.IsEmpty</c> left the suite permanently red — a guard nobody can act
    /// on is indistinguishable from no guard. The baseline turns it into a ratchet:
    /// new offenders fail immediately, and the file is only ever allowed to shrink.
    ///
    /// To regenerate after wiring systems up, run the Explicit test
    /// <c>UntickedSystemsBaselineTests.RegenerateBaselineFile</c>.
    /// </summary>
    public static class UntickedSystemsBaseline
    {
        public const string RelativePath = "Assets/Tests/EditMode/UntickedSystemsBaseline.txt";

        public static string AbsolutePath =>
            Path.Combine(Application.dataPath, "Tests", "EditMode", "UntickedSystemsBaseline.txt");

        /// <summary>
        /// Read the baseline. Blank lines and '#' comments are ignored so the file can
        /// carry an explanation of what it is for future readers.
        /// </summary>
        public static SortedSet<string> Load()
        {
            string path = AbsolutePath;
            if (!File.Exists(path))
                throw new FileNotFoundException(
                    $"C-1 baseline missing at {RelativePath}. Regenerate it with the Explicit test " +
                    "UntickedSystemsBaselineTests.RegenerateBaselineFile.", path);

            var set = new SortedSet<string>(StringComparer.Ordinal);
            foreach (string raw in File.ReadAllLines(path))
            {
                string line = raw.Trim();
                if (line.Length == 0 || line.StartsWith("#", StringComparison.Ordinal)) continue;
                set.Add(line);
            }
            return set;
        }

        /// <summary>Overwrite the baseline with <paramref name="names"/>, sorted, plus a header.</summary>
        public static void Write(IEnumerable<string> names)
        {
            var sorted = new SortedSet<string>(names, StringComparer.Ordinal);
            var lines = new List<string>
            {
                "# C-1 baseline — systems GameBootstrap constructs and save-wires but never ticks.",
                "#",
                "# This file is a RATCHET. RegistryDispatchWiringTests fails if a system not listed",
                "# here goes unticked (a new bug), and also fails if a system listed here has since",
                "# been wired (the entry must then be deleted). It may only ever shrink.",
                "#",
                "# Do not add entries by hand to silence a failure — wire the system instead.",
                "# Regenerate with the Explicit test UntickedSystemsBaselineTests.RegenerateBaselineFile.",
                "#",
                $"# entries: {sorted.Count}",
                "",
            };
            lines.AddRange(sorted);
            File.WriteAllLines(AbsolutePath, lines);
        }
    }
}
