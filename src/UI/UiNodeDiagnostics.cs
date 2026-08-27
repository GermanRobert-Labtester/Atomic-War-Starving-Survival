using System;
using Godot;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Opt-in headless diagnostic that reports live UI node counts before and
    /// after each panel test. Enabled with the ASHFALL_UI_NODE_DIAGNOSTICS
    /// environment variable (1/true/yes); silent and near-free when unset.
    ///
    /// Usage inside a uitest (Main is a Node, so the tree is reachable):
    /// <code>
    ///     UiNodeDiagnostics.Mark(this, "survivors");
    ///     _survivorsOverlay.Bind(_survivors);
    ///     _survivorsOverlay.Open();
    ///     ...
    ///     CloseAllOverlayPanels();
    ///     UiNodeDiagnostics.Report(this, "survivors");
    /// </code>
    ///
    /// Reports, per point in time:
    ///   - tree nodes   — every node in the live scene tree (from the root)
    ///   - ui controls  — Control-derived nodes in that tree
    ///   - live objects — Godot ObjectCount performance monitor (includes
    ///                    tree nodes plus not-yet-freed instances; a rising
    ///                    baseline across open→close cycles signals leaks)
    ///
    /// An open→close panel cycle should return to its pre-open counts. A
    /// persistent positive delta after Close is reported as NODE LEAK so a
    /// reviewer sees it without reading raw numbers.
    ///
    /// Diagnostic only: never changes any uitest pass/fail verdict, and
    /// prints nothing unless explicitly enabled.
    /// </summary>
    public static class UiNodeDiagnostics
    {
        /// <summary>Environment variable that opts this diagnostic in.</summary>
        public const string EnvVarName = "ASHFALL_UI_NODE_DIAGNOSTICS";

        private static bool? _enabled;

        /// <summary>True when the diagnostic was opted into via environment.</summary>
        public static bool Enabled =>
            _enabled ??= IsTruthy(System.Environment.GetEnvironmentVariable(EnvVarName));

        /// <summary>Point-in-time node counts captured by <see cref="Mark"/> / <see cref="Report"/>.</summary>
        public readonly struct Counts
        {
            public readonly int TreeNodes;
            public readonly int UiControls;
            public readonly long LiveObjects;

            public Counts(int treeNodes, int uiControls, long liveObjects)
            {
                TreeNodes = treeNodes;
                UiControls = uiControls;
                LiveObjects = liveObjects;
            }
        }

        private static Counts _last;
        private static bool _hasLast;
        private static string _lastLabel = string.Empty;

        private static bool IsTruthy(string? value)
        {
            if (string.IsNullOrEmpty(value)) return false;
            return value == "1" || value.Equals("true", System.StringComparison.OrdinalIgnoreCase)
                             || value.Equals("yes",  System.StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Walks the live scene tree from its root and counts all nodes,
        /// Control-derived nodes, and reads the engine's live Object monitor.
        /// </summary>
        public static Counts Capture(Node scope)
        {
            var root = scope.GetTree()?.Root;
            if (root == null)
                return new Counts(0, 0, 0);
            int nodes = 0, controls = 0;
            Walk(root, ref nodes, ref controls);
            // Do not count the root itself? Count it — it is a real node.
            long objects = (long)Performance.GetMonitor(Performance.Monitor.ObjectCount);
            return new Counts(nodes, controls, objects);
        }

        private static void Walk(Node node, ref int nodes, ref int controls)
        {
            nodes++;
            if (node is Control) controls++;
            foreach (var child in node.GetChildren())
            {
                if (child is Node n)
                    Walk(n, ref nodes, ref controls);
            }
        }

        /// <summary>
        /// Captures and prints the "before" counts for a named panel test.
        /// Establishes the baseline that the paired <see cref="Report"/> diffs against.
        /// </summary>
        public static void Mark(Node scope, string label)
        {
            if (!Enabled) return;
            _last = Capture(scope);
            _hasLast = true;
            _lastLabel = label;
            GD.Print($"[UiNodeDiag] before {label,-18} nodes={_last.TreeNodes} controls={_last.UiControls} objects={_last.LiveObjects}");
        }

        /// <summary>
        /// Captures and prints the "after" counts for a named panel test plus
        /// the delta against the last <see cref="Mark"/>. A positive node delta
        /// after a panel close (cycle should return to baseline) is flagged as
        /// a suspected node leak.
        /// </summary>
        public static void Report(Node scope, string label)
        {
            if (!Enabled) return;
            var now = Capture(scope);
            int dNodes = now.TreeNodes  - _last.TreeNodes;
            int dCtrls = now.UiControls - _last.UiControls;
            long dObjs = now.LiveObjects - _last.LiveObjects;
            string baselineNote = _hasLast && _lastLabel == label
                ? $" (vs before: {dNodes:+0;-0;+0} nodes, {dCtrls:+0;-0;+0} controls, {dObjs:+0;-0;+0} objects)"
                : " (no matching Mark — showing absolute counts)";
            GD.Print($"[UiNodeDiag] after  {label,-18} nodes={now.TreeNodes} controls={now.UiControls} objects={now.LiveObjects}{baselineNote}");
            if (_hasLast && _lastLabel == label && dNodes > 0)
                GD.Print($"[UiNodeDiag] NODE LEAK SUSPECT: '{label}' left {dNodes} node(s) in the tree after its test block closed");
            _last = now;
            _hasLast = true;
            _lastLabel = label;
        }

        /// <summary>Prints a section banner so long diagnostic logs stay readable.</summary>
        public static void Section(string title)
        {
            if (!Enabled) return;
            GD.Print($"[UiNodeDiag] ── {title} ──");
        }
    }
}
