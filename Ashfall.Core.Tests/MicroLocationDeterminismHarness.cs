using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.IO;
using Ashfall.Core.Narrative;
using Ashfall.Core.Random;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// F10 — deterministic micro-location encounter harness. Rebuilds the
    /// production Core wiring (full narrative catalog, registered expedition
    /// destinations, location-typed scavenging authority) on ONE shared
    /// ISeededRng stream — the same contract ExpeditionHostSession uses — and
    /// records a passive per-tick trace. The harness never draws from the
    /// simulation RNG beyond what the simulation itself draws, and never
    /// mutates simulation state (INV-07).
    ///
    /// Depletion snapshots are taken through CaptureState() (the live
    /// authoritative set), not the runtime DTO mirror, which refreshes only
    /// on restore.
    ///
    /// RNG continuation: SeededRng exposes no state getter on the current
    /// trunk (removed upstream mid-wave), so a save-boundary checkpoint is a
    /// draw COUNT from the counting wrapper; a fresh world replays that many
    /// NextDouble() draws from the same seed to land on the identical stream
    /// position (every public draw consumes exactly one NextRaw()).
    /// </summary>
    public sealed class MicroLocationDeterminismHarness
    {
        public const string SurvivorId = "surv_harness";

        public sealed record TraceEntry(
            int Tick,
            string? EncounterId,
            bool IsMicroLocation,
            string DepletedSnapshot,
            int RngDrawsAfter,
            int TotalResolvedAfter);

        public sealed class RunResult
        {
            public int Seed;
            public string ExpeditionId;
            public List<TraceEntry> Trace = new List<TraceEntry>();
            public int FinalPhase;
            public int TicksRun;

            public string Canonical()
            {
                var sb = new StringBuilder();
                sb.Append("seed=").Append(Seed).Append("|exp=").Append(ExpeditionId);
                foreach (var t in Trace)
                {
                    sb.Append('\n').Append("tick=").Append(t.Tick)
                      .Append("|enc=").Append(t.EncounterId ?? "none")
                      .Append("|micro=").Append(t.IsMicroLocation ? "1" : "0")
                      .Append("|dep=[").Append(t.DepletedSnapshot).Append(']')
                      .Append("|resolved=").Append(t.TotalResolvedAfter)
                      .Append("|draws=").Append(t.RngDrawsAfter);
                }
                sb.Append('\n').Append("final_phase=").Append(FinalPhase);
                return sb.ToString();
            }
        }

        /// <summary>One deterministic simulation world: engine + narrative +
        /// bridge on a single stream. Ticks accumulate into Trace.</summary>
        public sealed class Fixture
        {
            public int Seed;
            public ExpeditionSystem Engine = null!;
            public NarrativeEncounterSystem Narrative = null!;
            public ExpeditionEncounterBridge Bridge = null!;
            public CountingRng Rng = null!;
            public List<TraceEntry> Trace = new List<TraceEntry>();
            public int TicksRun;

            /// <summary>Surfaced encounter ids for the most recent Tick call
            /// (multiple only when several expeditions are active).</summary>
            public List<string?> LastTickSurfaced = new List<string?>();

            public void StartExpedition(string expeditionId, int day = 1)
            {
                var def = ExpeditionDefinitionRegistry.Get(expeditionId);
                Assert.NotNull(def);
                // Harness contexts dispatch to authored destinations regardless
                // of discovery gating; discovering is deterministic bookkeeping
                // and consumes no RNG.
                Engine.DiscoverLocation(expeditionId);
                bool ok = Engine.Start(def!, SurvivorId, day, ExpeditionStance.Stealth);
                Assert.True(ok, $"could not start expedition '{expeditionId}'");
            }

            public void Tick(int hours = 1)
            {
                for (int h = 0; h < hours; h++)
                {
                    TicksRun++;
                    int tickNumber = TicksRun;
                    LastTickSurfaced.Clear();
                    Bridge.OnSurfaced += OnSurfacedCapture;
                    try
                    {
                        Engine.TickHours(1f, Rng);
                    }
                    finally
                    {
                        Bridge.OnSurfaced -= OnSurfacedCapture;
                    }

                    string dep = string.Join(",", CaptureDepletedSorted());
                    Trace.Add(new TraceEntry(
                        tickNumber,
                        LastTickSurfaced.Count > 0 ? LastTickSurfaced[^1] : null,
                        LastTickSurfaced.Count > 0 && LastTickSurfaced[^1]?.StartsWith("micro_", StringComparison.Ordinal) == true,
                        dep,
                        Rng.Draws,
                        Narrative.TotalResolved));
                }
            }

            private void OnSurfacedCapture(ExpeditionEncounterBridge.EncounterSurfaced dto)
                => LastTickSurfaced.Add(string.IsNullOrEmpty(dto.encounter_id) ? null : dto.encounter_id);

            public List<string> CaptureDepletedSorted()
            {
                var ids = Narrative.CaptureState().depletedEncounterIds ?? new List<string>();
                ids.Sort(string.CompareOrdinal);
                return ids;
            }

            public RunResult Snapshot(string expeditionId)
            {
                int phase = 0;
                if (Engine.Active.TryGetValue(SurvivorId, out var exp)) phase = exp.phase;
                return new RunResult
                {
                    Seed = Seed,
                    ExpeditionId = expeditionId,
                    Trace = new List<TraceEntry>(Trace),
                    FinalPhase = phase,
                    TicksRun = TicksRun
                };
            }
        }

        // ── Production-catalog fixture construction ─────────────────────

        public static string DataDir()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null && !File.Exists(Path.Combine(dir.FullName, "Ashfall.csproj")))
                dir = dir.Parent!;
            return Path.Combine(dir!.FullName, "Assets", "StreamingAssets", "Data");
        }

        public static Fixture CreateFixture(int seed)
        {
            string dataDir = DataDir();
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var narrative = new NarrativeEncounterSystem();
            narrative.RegisterRange(NarrativeEncounterCatalogLoader.Load(dataDir, fileIO, json));

            var engine = new ExpeditionSystem();
            var scavenging = ScavengingTableCatalog.LoadFromDirectory(dataDir, fileIO, json);
            if (scavenging != null) engine.ScavengingCatalog = scavenging;

            var loaded = ExpeditionCatalogLoader.Load(dataDir, fileIO, json);
            Assert.NotNull(loaded);
            Assert.NotEmpty(loaded!);
            foreach (var def in loaded!)
            {
                if (def != null && !string.IsNullOrEmpty(def.id))
                    ExpeditionDefinitionRegistry.Register(def);
            }

            var rng = new CountingRng(new SeededRng(seed));
            var fixture = new Fixture
            {
                Seed = seed,
                Engine = engine,
                Narrative = narrative,
                Rng = rng
            };
            fixture.Bridge = new ExpeditionEncounterBridge(narrative, rng);
            engine.OnEncounterTriggered += fixture.Bridge.Surface;
            return fixture;
        }

        /// <summary>Run a fresh fixture for the given seed/ticks (F10.5–F10.7).</summary>
        public static RunResult Run(int seed, string expeditionId, int ticks)
        {
            var f = CreateFixture(seed);
            f.StartExpedition(expeditionId);
            f.Tick(ticks);
            return f.Snapshot(expeditionId);
        }

        public static void AssertTracesEqual(RunResult expected, RunResult actual, string context)
        {
            if (expected.Canonical() == actual.Canonical()) return;
            var sb = new StringBuilder();
            sb.Append("MICRO-LOCATION DETERMINISM FAILURE (").Append(context).Append(")\n");
            sb.Append("Seed: ").Append(expected.Seed).Append("  Expedition: ").Append(expected.ExpeditionId).Append('\n');
            var a = expected.Trace;
            var b = actual.Trace;
            int n = Math.Max(a.Count, b.Count);
            for (int i = 0; i < n; i++)
            {
                var ta = i < a.Count ? a[i] : null;
                var tb = i < b.Count ? b[i] : null;
                if (ta == null || tb == null || ta != tb)
                {
                    sb.Append("First divergent tick: ").Append(ta?.Tick ?? tb?.Tick).Append('\n');
                    sb.Append("Expected: ").Append(ta != null ? FormatEntry(ta) : "<none>").Append('\n');
                    sb.Append("Actual:   ").Append(tb != null ? FormatEntry(tb) : "<none>").Append('\n');
                    break;
                }
            }
            sb.Append("--- expected ---\n").Append(expected.Canonical()).Append('\n');
            sb.Append("--- actual ---\n").Append(actual.Canonical());
            Assert.True(false, sb.ToString());
        }

        private static string FormatEntry(TraceEntry t)
            => $"tick={t.Tick} enc={t.EncounterId ?? "none"} micro={(t.IsMicroLocation ? 1 : 0)} dep=[{t.DepletedSnapshot}] resolved={t.TotalResolvedAfter} draws={t.RngDrawsAfter}";
    }

    /// <summary>
    /// F10.8 — diagnostic counting wrapper. Test-only; never installed in
    /// production wiring. Counts draws so tests can pin the documented RNG
    /// consumption contract and checkpoint the stream position (draw count)
    /// across a save boundary without changing randomness itself.
    /// </summary>
    public sealed class CountingRng : ISeededRng
    {
        private readonly ISeededRng _inner;
        public int Draws;

        public CountingRng(ISeededRng inner) => _inner = inner;

        public int Seed => _inner.Seed;
        public int Next(int minInclusive, int maxExclusive) { Draws++; return _inner.Next(minInclusive, maxExclusive); }
        public float NextFloat() { Draws++; return _inner.NextFloat(); }
        public double NextDouble() { Draws++; return _inner.NextDouble(); }

        /// <summary>Land a fresh same-seed stream on the checkpointed draw
        /// position: replay n uncounted draws, then report the checkpoint.
        /// Deterministic — every public draw consumes exactly one NextRaw().</summary>
        public void ReplayDraws(int n)
        {
            for (int i = 0; i < n; i++) _inner.NextDouble();
            Draws = n;
        }
    }
}
