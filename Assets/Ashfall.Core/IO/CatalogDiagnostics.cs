// SPDX-License-Identifier: MIT
// ASHFALL Core: shared JSON-catalog loader diagnostics.
//
// Most catalog loaders follow the same fallback pattern: try to parse a JSON
// file as shape A; if that fails, try shape B; if both fail, return empty.
// Historically these branches used bare "catch { }" (silent swallow), which
// hides every malformed catalog from the host log.
//
// Centralising the warning sink here lets every loader share one diagnostics
// surface so a single log sink captures every malformed catalog at host boot.
// The default sink is ConsoleLog, which keeps Core tests quiet and writes to
// the Godot output stream in the headless demo path. The host can register
// an explicit sink via CatalogDiagnostics.RegisterLog(...).
//
// Mirrors the audit follow-up (PR-1 Mechanical): no swallow-without-visibility
// anywhere in catalog loaders.

using System;
#pragma warning disable CS8618

namespace Ashfall.Core.IO
{
    /// <summary>
    /// Process-wide diagnostics sink for catalog JSON parse failures.
    /// Reentrant; safe to call from any thread.
    /// </summary>
    public static class CatalogDiagnostics
    {
        private static readonly object s_lock = new object();
        private static ILog s_log;

        /// <summary>Register a sink. Pass null to detach.</summary>
        public static void RegisterLog(ILog sink)
        {
            lock (s_lock) { s_log = sink; }
        }

        private static ILog EffectiveSink
        {
            get
            {
                lock (s_lock)
                {
                    if (s_log == null) s_log = new ConsoleLog();
                    return s_log;
                }
            }
        }

        /// <summary>
        /// Emit a structured warning describing a catalog parse failure.
        /// </summary>
        public static void Warn(string path, string shape, Exception ex)
        {
            try
            {
                EffectiveSink.Warn(
                    "CatalogDiagnostics(" + (path ?? "<unknown>") + "): failed to parse as " + shape
                    + "; falling back to next shape. Reason: "
                    + (ex == null ? "<null exception>" : ex.Message));
            }
            catch
            {
                // Last-resort swallow: if the sink itself throws, we
                // preserve the previous silent-fallback behavior, but
                // strictly narrower than the original unconditional bare catch.
            }
        }
    }
}
