using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
using AtomicWar._Game.Data;
using AtomicWar._Game.Events;
using AtomicWar._Game.Core; // GameBootstrap (lives in AtomicWar._Game.Core)

namespace AtomicWar._Game.Editor
{
    /// <summary>
    /// H-3: Static analyzer for GameEvent id collisions and naming-convention
    /// violations. Every public event source in the codebase — the
    /// EncounterEventFactory methods, the Ensure* helpers in GameBootstrap,
    /// the chain factories in SuspicionTracker / EventRunner, and any
    /// user-authored catalog SO under Assets/StreamingAssets/Data/ — must
    /// produce events with non-empty snake_case ids. Any collision
    /// shadows the second event silently (EventRunner.FindInPool returns
    /// the first match), so this validator runs at design time and as
    /// a CI gate.
    ///
    /// Run from the Unity menu: Tools/ASHFALL/Validate Event Ids.
    /// Or invoke from CI: <c>unity -batchmode -executeMethod
    /// AtomicWar._Game.Editor.EventIdValidator.RunFromCommandLine</c>.
    /// </summary>
    public static partial class EventIdValidator
    {
        /// <summary>Snake-case regex: lowercase letters, digits, underscores. Must start with a letter.</summary>
        public static readonly Regex SnakeCasePattern = new Regex(@"^[a-z][a-z0-9_]*$");

        /// <summary>
        /// Run the full validation pass over every event source. Returns
        /// a sorted list of human-readable diagnostic strings. An empty
        /// list means everything is valid.
        /// </summary>
        public static List<string> Validate()
        {
            var allEvents = CollectAllEvents();
            var diagnostics = new List<string>();
            CheckDuplicates(allEvents, diagnostics);
            CheckEmptyIds(allEvents, diagnostics);
            CheckNamingConvention(allEvents, diagnostics);
            return diagnostics;
        }

        /// <summary>
        /// Run the validation pass and exit the Editor with code 0 (no
        /// diagnostics) or 1 (diagnostics found). For use in CI.
        /// </summary>
        public static void RunFromCommandLine()
        {
            var diagnostics = Validate();
            if (diagnostics.Count == 0)
            {
                Debug.Log("[EventIdValidator] OK — 0 diagnostics across " +
                    CountAllEvents() + " events.");
                UnityEditor.EditorApplication.Exit(0);
            }
            else
            {
                Debug.LogError("[EventIdValidator] FAILED — " + diagnostics.Count + " diagnostics:");
                for (int i = 0; i < diagnostics.Count; i++)
                {
                    Debug.LogError("  " + (i + 1) + ". " + diagnostics[i]);
                }
                UnityEditor.EditorApplication.Exit(1);
            }
        }

        /// <summary>
        /// Add a Unity menu item under Tools/ASHFALL so designers can
        /// trigger the validation from the editor.
        /// </summary>
        [UnityEditor.MenuItem("Tools/ASHFALL/Validate Event Ids")]
        public static void RunFromMenu()
        {
            var diagnostics = Validate();
            if (diagnostics.Count == 0)
            {
                Debug.Log("[EventIdValidator] OK — " + CountAllEvents() + " events, 0 diagnostics.");
                UnityEditor.EditorUtility.DisplayDialog(
                    "Event ID Validation",
                    "OK — " + CountAllEvents() + " events validated, 0 diagnostics.",
                    "OK");
            }
            else
            {
                var report = "[EventIdValidator] FAILED — " + diagnostics.Count + " diagnostics:\n\n";
                for (int i = 0; i < diagnostics.Count; i++)
                {
                    report += (i + 1) + ". " + diagnostics[i] + "\n";
                }
                Debug.LogError(report);
                UnityEditor.EditorUtility.DisplayDialog(
                    "Event ID Validation",
                    "FAILED — " + diagnostics.Count + " diagnostics. See Console for details.",
                    "OK");
            }
        }

        // ── Collection ────────────────────────────────────────────────

        /// <summary>
        /// Invoke every public static method on EncounterEventFactory that
        /// returns GameEvent (or List&lt;GameEvent&gt;), and every factory
        /// method referenced by the Ensure* helpers in GameBootstrap.
        /// The result is a flat list of (id, sourceLabel) tuples.
        /// </summary>
        public static List<(string id, string source)> CollectAllEvents()
        {
            var result = new List<(string, string)>();
            CollectFromEncounterFactory(result);
            CollectFromChainFactories(result);
            CollectFromEnsureHelpers(result);
            CollectFromGameBootstrapDirect(result);
            CollectFromStreamingAssetsCatalog(result);
            return result;
        }

        /// <summary>
        /// H-6: Collect event ids from EventPoolBuilder.Build(), which is the
        /// canonical event pool source after the H-6 refactor. This captures
        /// all factory events (emissary, safe haven, blood for water, buried
        /// alive, child found) plus EncounterEventFactory.CreateAll().
        /// </summary>

        // ── Checks ───────────────────────────────────────────────────

    }
}
