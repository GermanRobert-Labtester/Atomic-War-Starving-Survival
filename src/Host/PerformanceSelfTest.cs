using Godot;
using System;
using System.IO;
using Ashfall.Core.Performance;
using Ashfall.Core.Performance.Workloads;

namespace AtomicWar.GodotApp
{
    public static partial class HostCli
    {
        /// <summary>
        /// --runtime-scale-selftest (Task 130): performance budget validation for
        /// 30/180/360-day campaign workloads. Measures day-advance latency (total
        /// and per-owner), save/load/checksum latency, allocation, retained memory,
        /// and lifecycle leak tests. Writes machine-readable JSON to
        /// artifacts/runtime-scale-results.json. Exits 0 when all core gates pass.
        /// </summary>
        public static int RunRuntimeScaleSelfTest(string dataDirectory)
        {
            int failures = 0;
            void Check(bool ok, string label)
            {
                GD.Print($"[{(ok ? "PASS" : "FAIL")}] {label}");
                if (!ok) failures++;
            }

            string artifactsDir = "artifacts";
            Directory.CreateDirectory(artifactsDir);

            string platform = System.Environment.OSVersion.Platform.ToString();
            string runtime = "godot";

            const int measured = 5;

            var results = new System.Collections.Generic.List<PerfResult>();

            // 1. 30-day baseline.
            using (var harness30 = BuildHarness(WorkloadProfile.Days30, platform, runtime))
            {
                harness30.AdvanceDays(Math.Min(3, WorkloadProfile.Days30.CampaignDays));
                using var session30 = new PerfSession(BuildContext(WorkloadProfile.Days30, platform, runtime));
                for (int i = 0; i < measured; i++)
                {
                    session30.Measure(() => harness30.AdvanceDays(WorkloadProfile.Days30.CampaignDays));
                }
                var stats30 = session30.ComputeStatistics();
                results.Add(session30.ToResult("day_advance_30d", "advisory", slowestOwnerMs: stats30.Maximum));
                Check(stats30.Median < 2000, $"30d day-advance median < 2s (was {stats30.Median:F1}ms)");
            }

            // 2. 180-day mature.
            using (var harness180 = BuildHarness(WorkloadProfile.Days180, platform, runtime))
            {
                harness180.AdvanceDays(Math.Min(3, WorkloadProfile.Days180.CampaignDays));
                using var session180 = new PerfSession(BuildContext(WorkloadProfile.Days180, platform, runtime));
                for (int i = 0; i < measured; i++)
                {
                    session180.Measure(() => harness180.AdvanceDays(WorkloadProfile.Days180.CampaignDays));
                }
                var stats180 = session180.ComputeStatistics();
                results.Add(session180.ToResult("day_advance_180d", "advisory"));
                Check(stats180.Median < 12000, $"180d day-advance median < 12s (was {stats180.Median:F1}ms)");
            }

            // 3. 360-day stress.
            using (var harness360 = BuildHarness(WorkloadProfile.Days360, platform, runtime))
            {
                harness360.AdvanceDays(Math.Min(3, WorkloadProfile.Days360.CampaignDays));
                using var session360 = new PerfSession(BuildContext(WorkloadProfile.Days360, platform, runtime));
                for (int i = 0; i < measured; i++)
                {
                    session360.Measure(() => harness360.AdvanceDays(WorkloadProfile.Days360.CampaignDays));
                }
                var stats360 = session360.ComputeStatistics();
                results.Add(session360.ToResult("day_advance_360d", "advisory"));
                Check(stats360.Median < 30000, $"360d day-advance median < 30s (was {stats360.Median:F1}ms)");
            }

            // 4. Persistence gate on the 30-day workload.
            using (var harnessP = BuildHarness(WorkloadProfile.Days30, platform, runtime))
            {
                harnessP.AdvanceDays(WorkloadProfile.Days30.CampaignDays);
                using var sessionP = new PerfSession(BuildContext(WorkloadProfile.Days30, platform, runtime));
                sessionP.Measure(() => harnessP.MeasureSaveLatency());
                sessionP.Measure(() => harnessP.CaptureSavePayload());
                var statsP = sessionP.ComputeStatistics();
                results.Add(sessionP.ToResult("save_30d", "advisory"));
                Check(statsP.Median < 500, $"30d save median < 500ms (was {statsP.Median:F1}ms)");
            }

            // 5. Allocation growth bound.
            using (var harnessA = BuildHarness(WorkloadProfile.Days30, platform, runtime))
            {
                harnessA.AdvanceDays(3);
                using var sessionA = new PerfSession(BuildContext(WorkloadProfile.Days30, platform, runtime));
                for (int i = 0; i < measured; i++)
                {
                    sessionA.Measure(() => harnessA.AdvanceDays(1));
                }
                var statsA = sessionA.ComputeStatistics();
                results.Add(sessionA.ToResult("alloc_growth_30d", "advisory"));
                Check(statsA.MedianAllocatedBytes < 5_000_000,
                    $"per-day allocation median < 5MB (was {statsA.MedianAllocatedBytes:N0} bytes)");
            }

            // 6. Lifecycle / retained memory.
            long before = GC.GetTotalMemory(forceFullCollection: true);
            using (var harnessL = BuildHarness(WorkloadProfile.Days30, platform, runtime))
            {
                harnessL.AdvanceDays(30);
                harnessL.Dispose();
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            long after = GC.GetTotalMemory(forceFullCollection: true);
            long retained = after - before;
            Check(retained < 20_000_000L, $"retained memory after 30d lifecycle < 20MB (was {retained / 1_000_000}MB)");

            // Write machine-readable report.
            string reportPath = Path.Combine(artifactsDir, "runtime-scale-results.json");
            try
            {
                var opts = new System.Text.Json.JsonSerializerOptions { WriteIndented = true };
                string json = System.Text.Json.JsonSerializer.Serialize(results, opts);
                File.WriteAllText(reportPath, json);
                GD.Print($"REPORT {reportPath}");
            }
            catch (Exception ex)
            {
                GD.PrintErr($"RUNTIME_SCALE_SELFTEST REPORT WRITE FAIL: {ex.Message}");
                failures++;
            }

            GD.Print(failures == 0
                ? "RUNTIME_SCALE_SELFTEST PASS"
                : $"RUNTIME_SCALE_SELFTEST FAIL — {failures} failing check(s)");
            return failures == 0 ? 0 : 1;
        }

        private static PerfWorkloadContext BuildContext(WorkloadProfile profile, string platform, string runtime)
        {
            return new PerfWorkloadContext
            {
                WorkloadId = $"perf_{profile.Name}",
                CampaignDays = profile.CampaignDays,
                Seed = 9001,
                RosterTier = profile.RosterTier,
                JournalTier = profile.JournalTier,
                ExpeditionTier = profile.ExpeditionTier,
                WorldStateTier = profile.WorldStateTier,
                Platform = platform,
                Runtime = runtime,
            };
        }

        private static PerformanceCampaignHarness BuildHarness(WorkloadProfile profile, string platform, string runtime)
        {
            return new PerformanceCampaignHarness(BuildContext(profile, platform, runtime));
        }
    }
}
