// SPDX-License-Identifier: MIT
// ASHFALL CI Gate: Panel Live Refresh (UI/UX audit 2026-09-25 follow-up).
//
// A panel that binds a live host session and never subscribes to its change
// events (and whose host never refreshes it) freezes at open time — it renders
// but does not reflect play. This gate encodes the sweep method:
//
//   bound session  ->  subscription in the panel
//                  OR host-side <field>.RefreshView(...) call
//                  OR documented open-time snapshot exemption.
//
// The allowlist is deliberately explicit: adding a panel there is a conscious
// decision that the surface shows a record captured at open, not live state.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace Ashfall.Core.Tests.UI
{
    public sealed class PanelLiveRefreshGateTests
    {
        private static readonly Dictionary<string, string> OpenTimeSnapshotExemptions = new(StringComparer.Ordinal)
        {
            ["CargoAirdropPanel"] = "event-log view; the opener selects the record and rows are action-driven",
            ["EventDetailPanel"] = "detail view of one selected event record captured at open",
            ["ExpansionsHubPanel"] = "hub index rendered from expansion registry state at open",
            ["FactionCommuniqueBoardPanel"] = "message board of a selected faction; refreshed by its opener",
            ["FactionCultureCodexPanel"] = "codex page of a selected faction; static authored content",
            ["FactionDetailPanel"] = "detail view of one selected faction record captured at open",
            ["MaritimeAtlasPanel"] = "atlas snapshot of charted sites captured at open",
            ["PlasticPyrolysisPanel"] = "process console; every mutation goes through its own action handlers",
            ["SafeCrackModal"] = "single-attempt modal; state cannot change while it is open",
        };

        private static string FindRepoRoot()
        {
            var dir = new DirectoryInfo(Path.GetFullPath(Directory.GetCurrentDirectory()));
            while (dir != null)
            {
                if (Directory.Exists(Path.Combine(dir.FullName, "src"))
                    && Directory.Exists(Path.Combine(dir.FullName, "Assets")))
                    return dir.FullName;
                dir = dir.Parent!;
            }
            throw new DirectoryNotFoundException(
                "Could not locate repository root from " + Directory.GetCurrentDirectory());
        }

        private static readonly Regex BindSessionParam = new(
            @"\bBind\w*\s*\(([^)]*)\)", RegexOptions.Singleline | RegexOptions.Compiled);
        private static readonly Regex SessionType = new(
            @"([A-Za-z_][\w.]*HostSession)\??", RegexOptions.Compiled);
        private static readonly Regex ChangeSubscription = new(
            @"(\+=\s*\w*(StateChanged|On\w*Changed)|\b(StateChanged|On\w*Changed)\w*\s*\+=)", RegexOptions.Compiled);
        private static readonly Regex NotifyStyleSubscription = new(
            @"\.On\w+\s*\+=", RegexOptions.Compiled);

        [Fact]
        public void EveryPanelBindingALiveSession_RefreshesOrIsDocumented()
        {
            string root = FindRepoRoot();
            string mainSources = string.Join("\n",
                Directory.GetFiles(Path.Combine(root, "src"), "Main*.cs", SearchOption.TopDirectoryOnly)
                    .Select(File.ReadAllText));
            mainSources += string.Join("\n",
                Directory.GetFiles(Path.Combine(root, "src"), "Main*.cs", SearchOption.AllDirectories)
                    .Where(f => !f.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}"))
                    .Select(File.ReadAllText));

            var offenders = new List<string>();
            foreach (string file in Directory.GetFiles(Path.Combine(root, "src", "UI"), "*.cs"))
            {
                string source = File.ReadAllText(file);
                string className = Path.GetFileNameWithoutExtension(file);

                bool bindsSession = BindSessionParam.Matches(source)
                    .SelectMany(m => SessionType.Matches(m.Groups[1].Value).Select(t => t.Value))
                    .Any();
                if (!bindsSession || IsPanelApiOnly(className))
                    continue;

                bool selfRefreshes = ChangeSubscription.IsMatch(source)
                                     || NotifyStyleSubscription.IsMatch(source);
                if (selfRefreshes)
                    continue;

                string coreName = className.Replace("Panel", string.Empty).Replace("Modal", string.Empty);
                string field = coreName.Length > 0 ? "_" + char.ToLowerInvariant(coreName[0]) + coreName[1..] : string.Empty;
                bool hostRefreshes = field.Length > 0
                    && Regex.IsMatch(mainSources, Regex.Escape(field) + @"[?.\w]*\.RefreshView\(");

                if (!hostRefreshes && !OpenTimeSnapshotExemptions.ContainsKey(className))
                    offenders.Add(className);
            }

            Assert.True(offenders.Count == 0,
                "Panels binding a live session without a refresh path (add a subscription, a host "
                + "RefreshView call, or an explicit OpenTimeSnapshotExemptions reason): "
                + string.Join(", ", offenders.OrderBy(x => x)));
        }

        // Abstract panel contracts / scaffold bases declare Bind without being
        // concrete surfaces.
        private static bool IsPanelApiOnly(string className) =>
            className.EndsWith("Base", StringComparison.Ordinal)
            || className.EndsWith("Scaffold", StringComparison.Ordinal)
            || className == "IBindablePanel";

        [Fact]
        public void EveryExemption_NamesAFilenameThatExists()
        {
            string root = FindRepoRoot();
            foreach (string name in OpenTimeSnapshotExemptions.Keys)
                Assert.True(File.Exists(Path.Combine(root, "src", "UI", name + ".cs")),
                    $"stale exemption (no such panel file): {name}");
        }
    }
}
