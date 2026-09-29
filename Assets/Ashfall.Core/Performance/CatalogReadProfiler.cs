// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;

namespace Ashfall.Core.Performance;

/// <summary>Opt-in read measurements; never caches catalog content or changes load order.</summary>
public static class CatalogReadProfiler
{
    private static readonly Dictionary<string, PerfSession> Sessions = new(StringComparer.Ordinal);
    public static bool Enabled { get; set; }

    public static string ReadAllText(string path)
    {
        if (!Enabled) return File.ReadAllText(path);
        lock (Sessions)
        {
            if (!Sessions.TryGetValue(path, out var session))
            {
                session = new PerfSession(new PerfWorkloadContext { WorkloadId = path });
                Sessions.Add(path, session);
            }
            string text = string.Empty;
            session.Measure(() => text = File.ReadAllText(path), ownerId: path);
            return text;
        }
    }

    public static IReadOnlyList<PerfSample> CaptureSamples()
    {
        lock (Sessions)
        {
            var samples = new List<PerfSample>();
            foreach (var session in Sessions.Values) samples.AddRange(session.MeasuredSamples);
            return samples;
        }
    }

    public static void Reset()
    {
        lock (Sessions)
        {
            foreach (var session in Sessions.Values) session.Dispose();
            Sessions.Clear();
        }
    }
}
