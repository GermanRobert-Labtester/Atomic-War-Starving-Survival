// SPDX-License-Identifier: MIT
// Guard for the expedition UI localization keys introduced by the P105–P108
// waves. The full-catalog gate already enforces shape and placeholder parity;
// this pins the expedition keys so a future cleanup cannot silently drop one
// (the runtime falls back to English, which hides the regression).
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Ashfall.Core.Expeditions;
using Xunit;

namespace Ashfall.Core.Tests.Localization
{
    public sealed class ExpeditionLocaleKeysTests
    {
        private static readonly Regex PlaceholderPattern = new(
            @"\{([A-Za-z_][A-Za-z0-9_]*|\d+)(?::[^}]*)?\}", RegexOptions.Compiled);

        /// <summary>A ui.expedition.* catalog key literal embedded in UI source.</summary>
        private static readonly Regex UiExpeditionKeyPattern = new(
            @"\"(ui\.expedition\.[A-Za-z0-9_.]+)\"", RegexOptions.Compiled);

        private static readonly string[] RequiredKeys =
        {
            "ui.expedition.prep_header",
            "ui.expedition.return_header",
            "ui.expedition.failure_header",
            "ui.expedition.prep_survivor",
            "ui.expedition.refuel_tooltip",
            "ui.expedition.track_gear_tooltip",
            "ui.expedition.fitness_title",
            "ui.expedition.no_route",
            "ui.expedition.micro_location",
            "ui.expedition.scout",
            "ui.expedition.stamina",
            "ui.expedition.danger",
            "ui.expedition.dispatch_stealth",
            "ui.expedition.dispatch_speed",
            "ui.expedition.dispatch_blocked",
            "ui.expedition.encounter_title",
            "ui.expedition.encounter_pending",
            "ui.expedition.banner_encounter",
            "ui.expedition.banner_specific",
            "ui.expedition.shell_title",
            "ui.expedition.radar.rail.median_value",
            "ui.expedition.host_unavailable",
            "ui.expedition.section.active",
            "ui.expedition.section.pending",
            "ui.expedition.section.prep",
            "ui.expedition.section.targets",
            "ui.expedition.advance",
            "ui.expedition.return_dashboard",
            "ui.expedition.push_luck",
            "ui.expedition.order_return",
            "ui.expedition.estimate",
            "ui.expedition.estimate_tooltip",
            "ui.expedition.resolve",
            "ui.expedition.dismiss_all",
            "ui.expedition.tactical_header",
            "ui.expedition.decide_later",
            "ui.expedition.fitness_factors",
            "ui.expedition.radar.filter.all",
            "ui.expedition.radar.filter.all.hint",
            "ui.expedition.radar.filter.near",
            "ui.expedition.radar.filter.near.hint",
            "ui.expedition.radar.filter.medium",
            "ui.expedition.radar.filter.medium.hint",
            "ui.expedition.radar.filter.far",
            "ui.expedition.radar.filter.far.hint",
            "ui.expedition.radar.col.survivor",
            "ui.expedition.radar.col.phase",
            "ui.expedition.radar.col.distance",
            "ui.expedition.radar.col.stamina",
            "ui.expedition.radar.col.loot",
            "ui.expedition.radar.col.encounters",
            "ui.expedition.radar.col.location",
            "ui.expedition.radar.col.legs",
            "ui.expedition.radar.col.danger",
            "ui.expedition.radar.col.enc_pct",
            "ui.expedition.radar.col.status",
            "ui.expedition.radar.active_header",
            "ui.expedition.radar.targets_header",
            "ui.expedition.radar.detail_header",
            "ui.expedition.radar.select_hint",
            "ui.expedition.phase.outbound",
            "ui.expedition.phase.looting",
            "ui.expedition.phase.inbound",
            "ui.expedition.phase.completed",
            "ui.expedition.phase.failed",
            "ui.expedition.phase.camp",
            "ui.expedition.radar.title",
            "ui.expedition.radar.range_filter",
            "ui.expedition.radar.rail.active",
            "ui.expedition.radar.rail.queued",
            "ui.expedition.radar.rail.blocked",
            "ui.expedition.radar.rail.median",
            "ui.expedition.radar.rail.max_danger",
            "ui.expedition.radar.rail.enc_pct",
            "ui.expedition.radar.detail.scout",
            "ui.expedition.radar.detail.travel",
            "ui.expedition.radar.detail.travel_value",
            "ui.expedition.radar.detail.cargo",
            "ui.expedition.radar.detail.cargo_value",
            "ui.expedition.radar.detail.encounter_hr",
            "ui.expedition.radar.detail.route",
            "ui.expedition.radar.sortie_title",
            "ui.expedition.radar.target_title",
            "ui.expedition.radar.status.blocked",
            "ui.expedition.radar.status.ready",
            "ui.expedition.radar.status.needs_survivor",
            "ui.expedition.radar.no_targets",
            "ui.expedition.radar.no_sorties",
            "ui.expedition.radar.subsection.loot",
            "ui.expedition.radar.subsection.status",
            "ui.expedition.radar.level_value",
            "ui.expedition.radar.push_suffix",
            "ui.expedition.radar.pushing_luck_suffix",
            "ui.expedition.prep_vehicle",
            "ui.expedition.prep_weapon",
            "ui.expedition.refuel",
            "ui.expedition.fit_track_gear",
            "ui.expedition.radar_console",
            "ui.expedition.camp_console",
            "ui.expedition.rail_console",
            "ui.expedition.progress_line",
            "ui.expedition.transit_outbound",
            "ui.expedition.transit_inbound",
            "ui.expedition.target_line",
            "ui.expedition.world_line",
            "ui.expedition.world_unclaimed",
            "ui.expedition.world_ruined",
            "ui.expedition.world_threats",
            "ui.expedition.badge.req",
            "ui.expedition.badge.cost",
            "ui.expedition.badge.cost_list",
            "ui.expedition.badge.gain",
            "ui.expedition.badge.codex",
            "ui.expedition.badge.map",
            "ui.expedition.badge.unavailable",
            "ui.expedition.badge.cost_unavailable",
            "ui.expedition.radar.rail.median_empty",
            "ui.expedition.summary",
            "ui.expedition.esc_hint",
            "ui.expedition.no_active",
            "ui.expedition.one_time",
            "ui.expedition.fitness.unfit",
            "ui.expedition.fitness.fit",
            "ui.expedition.fitness.impaired",
            "ui.status.expedition_injury",
            "ui.status.expedition_injury.value",
        };

        private static string RepoRoot()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                if (File.Exists(Path.Combine(dir.FullName, "assets", "l10n", "strings.csv")))
                    return dir.FullName;
                dir = dir.Parent;
            }
            throw new DirectoryNotFoundException("Could not locate the ASHFALL localization catalog.");
        }

        private static string[] ParseCsvLine(string line)
        {
            var fields = new List<string>();
            var current = new StringBuilder();
            bool inQuotes = false;
            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                if (c == '"')
                {
                    if (inQuotes && i + 1 < line.Length && line[i + 1] == '"') { current.Append('"'); i++; }
                    else inQuotes = !inQuotes;
                }
                else if (c == ',' && !inQuotes) { fields.Add(current.ToString()); current.Clear(); }
                else current.Append(c);
            }
            fields.Add(current.ToString());
            return fields.ToArray();
        }

        private static HashSet<string> Placeholders(string value)
        {
            var set = new HashSet<string>(StringComparer.Ordinal);
            foreach (Match match in PlaceholderPattern.Matches(value))
                set.Add(match.Groups[1].Value);
            return set;
        }

        [Fact]
        public void RequiredExpeditionKeys_ExistWithGermanAndPlaceholderParity()
        {
            var rows = new Dictionary<string, string[]>(StringComparer.Ordinal);
            foreach (string line in File.ReadAllLines(Path.Combine(RepoRoot(), "assets", "l10n", "strings.csv")))
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#", StringComparison.Ordinal)) continue;
                string[] row = ParseCsvLine(line);
                if (row.Length == 4) rows[row[0]] = row;
            }

            var errors = new List<string>();
            foreach (string key in RequiredKeys)
            {
                if (!rows.TryGetValue(key, out var row))
                {
                    errors.Add($"{key}: missing");
                    continue;
                }
                if (string.IsNullOrWhiteSpace(row[1])) errors.Add($"{key}: missing English");
                if (string.IsNullOrWhiteSpace(row[2])) errors.Add($"{key}: missing German");
                if (row[1] == row[2]) errors.Add($"{key}: German copies English");
                if (!Placeholders(row[1]).SetEquals(Placeholders(row[2])))
                    errors.Add($"{key}: placeholder mismatch");
            }

            Assert.True(errors.Count == 0, "expedition locale key failures: " + string.Join("; ", errors));
        }

        /// <summary>
        /// Every <see cref="ExpeditionPhase"/> value must have a catalog row.
        /// The runtime helper falls back to the raw enum name, which hides a
        /// missing translation; this ties the enum to the catalog so a new
        /// phase cannot ship unlocalized (the Camp phase was the first catch).
        /// </summary>
        [Fact]
        public void EveryExpeditionPhaseValue_HasACatalogKey()
        {
            var keys = new HashSet<string>(StringComparer.Ordinal);
            foreach (string line in File.ReadAllLines(Path.Combine(RepoRoot(), "assets", "l10n", "strings.csv")))
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#", StringComparison.Ordinal)) continue;
                string[] row = ParseCsvLine(line);
                if (row.Length == 4) keys.Add(row[0]);
            }

            var missing = new List<string>();
            foreach (ExpeditionPhase phase in Enum.GetValues(typeof(ExpeditionPhase)))
            {
                string key = "ui.expedition.phase." + phase.ToString().ToLowerInvariant();
                if (!keys.Contains(key)) missing.Add($"{phase}: {key}");
            }

            Assert.True(missing.Count == 0,
                "expedition phase values without a catalog key: " + string.Join("; ", missing));
        }

        /// <summary>
        /// Floor for the expedition catalog surface. The per-key pin above
        /// names the keys a feature currently uses; this catches a wholesale
        /// trim of the ui.expedition.* block (bad merge, overzealous cleanup)
        /// that drops keys no test names explicitly. Ratchet down, never up:
        /// lower the floor only when a key is intentionally retired.
        /// </summary>
        [Fact]
        public void ExpeditionKeySurface_DoesNotRegress()
        {
            const int MinimumExpeditionKeys = 127;
            int count = 0;
            foreach (string line in File.ReadAllLines(Path.Combine(RepoRoot(), "assets", "l10n", "strings.csv")))
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#", StringComparison.Ordinal)) continue;
                string[] row = ParseCsvLine(line);
                if (row.Length == 4 && row[0].StartsWith("ui.expedition.", StringComparison.Ordinal)) count++;
            }

            Assert.True(count >= MinimumExpeditionKeys,
                $"ui.expedition.* catalog keys dropped below the floor: {count} < {MinimumExpeditionKeys}.");
        }

        /// <summary>
        /// Every <c>ui.expedition.*</c> literal referenced by the expedition UI
        /// panels must have a catalog row. The pinned list above names the keys
        /// a feature consumes today; this ties the source to the catalog, so a
        /// new <c>AshfallLocalization.Tr("ui.expedition.new_thing")</c> call
        /// without its row fails instead of silently falling back to the English
        /// literal. Covers the radar and camp panels, which the CI drift gate's
        /// registered-surface list does not.
        /// </summary>
        [Fact]
        public void EveryUiExpeditionKeyLiteral_HasACatalogRow()
        {
            var keys = new HashSet<string>(StringComparer.Ordinal);
            foreach (string line in File.ReadAllLines(Path.Combine(RepoRoot(), "assets", "l10n", "strings.csv")))
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#", StringComparison.Ordinal)) continue;
                string[] row = ParseCsvLine(line);
                if (row.Length == 4) keys.Add(row[0]);
            }

            string uiRoot = Path.Combine(RepoRoot(), "src", "UI");
            var missing = new List<string>();
            foreach (string file in new[]
            {
                "ExpeditionPanel.cs", "ExpeditionRadarPanel.cs",
                "ExpeditionPhaseText.cs", "ExpeditionCampPanel.cs",
            })
            {
                string path = Path.Combine(uiRoot, file);
                if (!File.Exists(path)) continue;
                foreach (Match match in UiExpeditionKeyPattern.Matches(File.ReadAllText(path)))
                {
                    string key = match.Groups[1].Value;
                    if (!keys.Contains(key)) missing.Add($"{file}: {key}");
                }
            }

            Assert.True(missing.Count == 0,
                "expedition UI references catalog keys with no row: " + string.Join("; ", missing));
        }
    }
}
