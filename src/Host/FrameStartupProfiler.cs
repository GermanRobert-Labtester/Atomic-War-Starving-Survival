// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using Ashfall.Core.Performance;
using Godot;

namespace AtomicWar.GodotApp.Host;

/// <summary>Opt-in startup observations. Inactive in normal play; no gameplay state.</summary>
public partial class FrameStartupProfiler : Node
{
    private string? _output;
    private static bool _measureProcesses;
    private static readonly Dictionary<string, (long calls, long ticks)> ProcessTotals = new();

    public readonly struct ProcessScope : IDisposable
    {
        private readonly string? _name;
        private readonly long _start;
        internal ProcessScope(string? name)
        {
            _name = name;
            _start = name == null ? 0 : Stopwatch.GetTimestamp();
        }
        public void Dispose()
        {
            if (_name == null) return;
            long elapsed = Stopwatch.GetTimestamp() - _start;
            ProcessTotals.TryGetValue(_name, out var previous);
            ProcessTotals[_name] = (previous.calls + 1, previous.ticks + elapsed);
        }
    }

    public static ProcessScope MeasureProcess(string name) => new(_measureProcesses ? name : null);
    public override void _EnterTree()
    {
        _output = System.Environment.GetEnvironmentVariable("ASHFALL_STARTUP_PROFILE_PATH");
        SetProcess(!string.IsNullOrWhiteSpace(_output));
        if (string.IsNullOrWhiteSpace(_output)) return;
        CatalogReadProfiler.Reset();
        CatalogReadProfiler.Enabled = true;
        ProcessTotals.Clear();
        _measureProcesses = true;
    }

    public override void _Process(double delta)
    {
        SetProcess(false);
        // Godot may already have queued one process callback before the tree
        // applies SetProcess(false) from _EnterTree. A normal launch has no
        // output path and must remain a true no-op in that edge case.
        if (string.IsNullOrWhiteSpace(_output)) return;
        CatalogReadProfiler.Enabled = false;
        using var process = System.Diagnostics.Process.GetCurrentProcess();
        var report = new
        {
            engineToFirstProcessMs = Time.GetTicksMsec(),
            processToFirstProcessMs = (DateTime.UtcNow - process.StartTime.ToUniversalTime()).TotalMilliseconds,
            // Godot loads assemblies from bytes; Location can legitimately be empty.
            assemblyBytes = string.IsNullOrEmpty(typeof(Main).Assembly.Location)
                ? (long?)null : new FileInfo(typeof(Main).Assembly.Location).Length,
            catalogReadSamples = CatalogReadProfiler.CaptureSamples(),
            note = "First Main scene process callback; OS file caches were not cleared. Catalog samples measure file reads, not deserialization."
        };
        File.WriteAllText(_output!, JsonSerializer.Serialize(report, new JsonSerializerOptions { WriteIndented = true, IncludeFields = true }));
        CatalogReadProfiler.Reset();
    }

    public override void _ExitTree()
    {
        CatalogReadProfiler.Enabled = false;
        _measureProcesses = false;
        if (string.IsNullOrWhiteSpace(_output)) return;
        var totals = new Dictionary<string, object>();
        foreach (var pair in ProcessTotals)
            totals[pair.Key] = new
            {
                calls = pair.Value.calls,
                totalMs = pair.Value.ticks * 1000.0 / Stopwatch.Frequency,
                meanMs = pair.Value.ticks * 1000.0 / Stopwatch.Frequency / pair.Value.calls
            };
        File.WriteAllText(_output + ".ui.json", JsonSerializer.Serialize(totals, new JsonSerializerOptions { WriteIndented = true }));
    }
}
