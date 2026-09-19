// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using AtomicWar.GodotApp.UI;
using AtomicWar.GodotApp.YearOfAsh;

namespace AtomicWar.GodotApp
{
    public static partial class HostCli
    {
        /// <summary>
        /// Plan 133 / #135 — Faction War Communiqué Board headless verification.
        /// Verifies:
        ///  - Unbound truthful state ("District wire not connected.")
        ///  - Day-gated early campaign state (Day 100: "No communiqués have been issued yet as of day 100.")
        ///  - Day-gated war arc state (Day 500: rendered communiqué cards, titles, word-wrapped bodies, lore attribution)
        ///  - Idempotent refresh (multiple RefreshView calls produce stable card counts)
        ///  - Clean unbind transition
        /// </summary>
        public static int RunFactionCommuniqueBoardSelfTest(string dataDirectory)
        {
            int failures = 0;
            void Check(bool condition, string name)
            {
                if (condition) GD.Print("[COMMUNIQUE_BOARD] PASS " + name);
                else
                {
                    GD.Print("[COMMUNIQUE_BOARD] FAIL " + name);
                    failures++;
                }
            }

            try
            {
                var panel = new FactionCommuniqueBoardPanel();
                panel._Ready();

                // 1. Unbound state
                Check(!panel.IsBound, "unbound_initially");
                List<string> unboundLabels = CollectLabels(panel);
                Check(unboundLabels.Exists(t => t.Contains("District wire not connected.", StringComparison.Ordinal)), "unbound_truthful_empty_state");

                // 2. Bound at day 100 (prior to war arc, first communique is day 489)
                var session = YearOfAshHostSession.Create(dataDirectory, loadExistingSave: false);
                panel.Bind(session, 100);
                Check(panel.IsBound, "bound_at_day_100");

                List<string> day100Labels = CollectLabels(panel);
                Check(day100Labels.Exists(t => t.Contains("No communiqués have been issued yet as of day 100.", StringComparison.Ordinal)), "day_100_truthful_empty_state");

                // 3. Bound at day 500 (inside war arc)
                panel.Bind(session, 500);
                List<string> day500Labels = CollectLabels(panel);
                Check(day500Labels.Count > 4, "day_500_rendered_cards_exist");

                // Check for day-gated communiques up to day 500 (e.g. Day 489)
                bool hasKnownTitle = day500Labels.Exists(t => t.Contains("Manifest Procedure at Checkpoint Gamma", StringComparison.Ordinal));
                Check(hasKnownTitle, "day_500_has_known_communique_title");

                bool hasFactionAttribution = day500Labels.Exists(t => t.Contains("CENTRAL GARRISON", StringComparison.OrdinalIgnoreCase) || t.Contains("REBUILDERS", StringComparison.OrdinalIgnoreCase));
                Check(hasFactionAttribution, "day_500_has_faction_attribution");

                // Check later communique at day 550 (e.g. Day 537)
                panel.Bind(session, 550);
                List<string> day550Labels = CollectLabels(panel);
                bool hasLaterTitle = day550Labels.Exists(t => t.Contains("Continuity of Distribution, Formally Secured", StringComparison.Ordinal));
                Check(hasLaterTitle, "day_550_has_later_communique_title");

                // 4. Idempotent refresh
                int countBefore = day550Labels.Count;
                panel.RefreshView();
                List<string> refreshedLabels = CollectLabels(panel);
                Check(refreshedLabels.Count == countBefore, "refresh_idempotent_card_count_stable");

                // 5. Unbind transition
                panel.Unbind();
                Check(!panel.IsBound, "unbound_after_unbind_call");
                List<string> postUnbindLabels = CollectLabels(panel);
                Check(postUnbindLabels.Exists(t => t.Contains("District wire not connected.", StringComparison.Ordinal)), "post_unbind_empty_state");

                panel.QueueFree();
            }
            catch (Exception ex)
            {
                Check(false, "exception: " + ex.Message);
            }

            return EmitSummary(
                "faction_communique_board_selftest",
                failures == 0,
                failures == 0 ? 0 : 1,
                details: failures == 0 ? "PASS" : $"FAIL ({failures} failures)");
        }

        private static List<string> CollectLabels(Node root)
        {
            var results = new List<string>();
            void Walk(Node node)
            {
                if (node is Label lbl && !string.IsNullOrEmpty(lbl.Text))
                    results.Add(lbl.Text);
                foreach (Node child in node.GetChildren())
                    Walk(child);
            }
            Walk(root);
            return results;
        }
    }
}
