// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Godot;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Requirement level for a host subsystem collaborator (Plan 36B.2).
    /// </summary>
    public enum CollaboratorRequirement
    {
        /// <summary>Mandatory for correct production simulation behavior.</summary>
        Required,

        /// <summary>Optional capability / progressive enhancement.</summary>
        Optional,

        /// <summary>Fallback implementation active in specific environments.</summary>
        Fallback,

        /// <summary>Testing or diagnostic mock only.</summary>
        TestOnly
    }

    /// <summary>
    /// Execution environment where a fallback is legally permitted (Plan 36B.9).
    /// </summary>
    public enum AllowedEnvironment
    {
        All,
        Production,
        DevOnly,
        TestOnly,
        HeadlessOnly
    }

    /// <summary>
    /// Formal declaration of an active fallback adapter (Plan 36B.9).
    /// </summary>
    public sealed class NamedFallback
    {
        public string FallbackId { get; set; } = string.Empty;
        public string Owner { get; set; } = string.Empty;
        public AllowedEnvironment Environment { get; set; } = AllowedEnvironment.Production;
        public string Reason { get; set; } = string.Empty;

        public NamedFallback() { }

        public NamedFallback(string fallbackId, string owner, AllowedEnvironment env, string reason)
        {
            FallbackId = fallbackId;
            Owner = owner;
            Environment = env;
            Reason = reason;
        }
    }

    /// <summary>
    /// Standardized runtime wiring report emitted by host sessions (Plan 36B.4).
    /// </summary>
    public sealed class WiringReport
    {
        public string SessionId { get; set; } = string.Empty;
        public IReadOnlyList<string> RequiredCollaborators { get; set; } = Array.Empty<string>();
        public IReadOnlyList<string> BoundCollaborators { get; set; } = Array.Empty<string>();
        public IReadOnlyList<string> MissingCollaborators { get; set; } = Array.Empty<string>();
        public IReadOnlyList<string> RequiredPorts { get; set; } = Array.Empty<string>();
        public IReadOnlyList<string> BoundPorts { get; set; } = Array.Empty<string>();
        public IReadOnlyList<string> MissingPorts { get; set; } = Array.Empty<string>();
        public IReadOnlyList<NamedFallback> ActiveFallbacks { get; set; } = Array.Empty<NamedFallback>();

        public bool IsValid => MissingCollaborators.Count == 0 && MissingPorts.Count == 0;

        public int TotalRequired => RequiredCollaborators.Count + RequiredPorts.Count;
        public int TotalBound => BoundCollaborators.Count + BoundPorts.Count;
        public int TotalMissing => MissingCollaborators.Count + MissingPorts.Count;
    }

    /// <summary>
    /// Interface for host sessions that participate in boot and session-swap wiring validation.
    /// </summary>
    public interface IWiringReporter
    {
        WiringReport GetWiringReport();
    }

    /// <summary>
    /// Aggregated summary of all host subsystem wiring reports.
    /// </summary>
    public sealed class HostWiringSummary
    {
        public int TotalSessions { get; set; }
        public int TotalWiringRequired { get; set; }
        public int TotalWiringBound { get; set; }
        public int TotalWiringMissing { get; set; }
        public int TotalFallbacksActive { get; set; }
        public IReadOnlyList<WiringReport> Reports { get; set; } = Array.Empty<WiringReport>();

        public bool IsValid => TotalWiringMissing == 0;
    }

    /// <summary>
    /// Central runtime validator for host subsystem wiring and collaborator validation (Plan 36B.5 - 36B.7).
    /// </summary>
    public static class HostWiringValidator
    {
        private static readonly List<IWiringReporter> _reporters = new();

        public static void RegisterReporter(IWiringReporter reporter)
        {
            if (reporter != null && !_reporters.Contains(reporter))
            {
                _reporters.Add(reporter);
            }
        }

        public static void UnregisterReporter(IWiringReporter reporter)
        {
            if (reporter != null)
            {
                _reporters.Remove(reporter);
            }
        }

        public static void ClearReporters()
        {
            _reporters.Clear();
        }

        public static IReadOnlyList<IWiringReporter> GetRegisteredReporters() => _reporters.AsReadOnly();

        /// <summary>
        /// Validates all registered or supplied wiring reporters, prints a structured table and machine summary.
        /// </summary>
        public static HostWiringSummary ValidateAll(IEnumerable<IWiringReporter>? reporters = null)
        {
            var targetList = (reporters ?? _reporters).ToList();
            var reports = new List<WiringReport>();

            int totalRequired = 0;
            int totalBound = 0;
            int totalMissing = 0;
            int totalFallbacks = 0;

            foreach (var reporter in targetList)
            {
                try
                {
                    var report = reporter.GetWiringReport();
                    if (report != null)
                    {
                        reports.Add(report);
                        totalRequired += report.TotalRequired;
                        totalBound += report.TotalBound;
                        totalMissing += report.TotalMissing;
                        totalFallbacks += report.ActiveFallbacks.Count;
                    }
                }
                catch (Exception ex)
                {
                    GD.PrintErr($"[HOST_WIRING] Exception obtaining report from {reporter.GetType().Name}: {ex.Message}");
                    var errReport = new WiringReport
                    {
                        SessionId = reporter.GetType().Name,
                        MissingCollaborators = new[] { $"Exception: {ex.Message}" }
                    };
                    reports.Add(errReport);
                    totalMissing++;
                }
            }

            var summary = new HostWiringSummary
            {
                TotalSessions = reports.Count,
                TotalWiringRequired = totalRequired,
                TotalWiringBound = totalBound,
                TotalWiringMissing = totalMissing,
                TotalFallbacksActive = totalFallbacks,
                Reports = reports
            };

            PrintWiringTable(summary);
            return summary;
        }

        /// <summary>
        /// Emits formatted table and machine-readable metrics (Plan 36B.6).
        /// </summary>
        public static void PrintWiringTable(HostWiringSummary summary)
        {
            GD.Print("── HOST SUBSYSTEM WIRING REPORT (Plan 36B) ──");
            GD.Print(string.Format("{0,-28} | {1,9} | {2,11} | {3,9} | {4,10} | {5,7} | {6,9}",
                "Subsystem", "ReqCollab", "BoundCollab", "ReqPorts", "BoundPorts", "Missing", "Fallbacks"));
            GD.Print(new string('-', 96));

            foreach (var r in summary.Reports)
            {
                string line = string.Format("{0,-28} | {1,9} | {2,11} | {3,9} | {4,10} | {5,7} | {6,9}",
                    r.SessionId.Length > 28 ? r.SessionId.Substring(0, 25) + "..." : r.SessionId,
                    r.RequiredCollaborators.Count,
                    r.BoundCollaborators.Count,
                    r.RequiredPorts.Count,
                    r.BoundPorts.Count,
                    r.TotalMissing,
                    r.ActiveFallbacks.Count);
                GD.Print(line);

                if (r.TotalMissing > 0)
                {
                    if (r.MissingCollaborators.Count > 0)
                        GD.PrintErr($"  [MISSING COLLABORATORS] {string.Join(", ", r.MissingCollaborators)}");
                    if (r.MissingPorts.Count > 0)
                        GD.PrintErr($"  [MISSING PORTS] {string.Join(", ", r.MissingPorts)}");
                }
            }
            GD.Print(new string('-', 96));
            GD.Print($"HOST_SESSIONS={summary.TotalSessions}");
            GD.Print($"HOST_WIRING_REQUIRED={summary.TotalWiringRequired}");
            GD.Print($"HOST_WIRING_BOUND={summary.TotalWiringBound}");
            GD.Print($"HOST_WIRING_MISSING={summary.TotalWiringMissing}");
            GD.Print($"HOST_FALLBACKS_ACTIVE={summary.TotalFallbacksActive}");
        }

        /// <summary>
        /// Validates session wiring during boot and session replacement (Plan 36B.7).
        /// Returns true if all required collaborators and ports are satisfied.
        /// </summary>
        public static bool ValidateSessionWiring(IEnumerable<IWiringReporter>? reporters = null)
        {
            var summary = ValidateAll(reporters);
            if (summary.TotalWiringMissing > 0)
            {
                GD.PrintErr($"[FAIL] Host session wiring validation failed with {summary.TotalWiringMissing} missing port(s)/collaborator(s).");
                return false;
            }
            GD.Print($"[PASS] Host session wiring validated cleanly across {summary.TotalSessions} subsystem(s).");
            return true;
        }
    }
}
