// SPDX-License-Identifier: MIT
// ASHFALL CI Gate: Panel Route Reachability (R1, 2026-10-01).
// Generalizes the T14 finding (expedition_radar/expedition_camp were
// registered, player-navigable, and unreachable): every player-navigable
// PanelRegistry route must have an emitter surface, be reachable through a
// direct-call funnel, or sit on the pinned pending-foreman list with a
// written reason. New unreachable routes fail this gate; the pending list
// shrinks only by explicit edit.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace Ashfall.Core.Tests.UI
{
    public sealed class PanelRouteReachabilityGateTests
    {
        // Panels opened through direct-call funnels rather than their route
        // id (verified 2026-10-01): the route id is a redundant alias, the
        // panel itself is reachable.
        private static readonly string[] DirectCallReachable =
        {
            "map_detail",       // OpenMapDetailPanel — MapPanel + MapAtlasPanel inspect funnel
            "faction_detail",   // OpenFactionDetailPanel — FactionsPanel inspect funnel
            "quest_detail",     // OpenQuestDetailPanel — QuestsPanel inspect funnel
            "fire_incident",    // OpenFireIncidentPanel — shelter-fire event funnel
            "combat_hud",       // auto-opened and bound with the combat session
        };

        // Verified 2026-10-01 as registered-but-unreachable with blocked or
        // ambiguous hosts. Each needs a foreman decision (claimed host,
        // claimed launcher, or actively-foreign seam) before wiring.
        private static readonly (string Id, string Reason)[] PendingForeman =
        {
            ("verdict_dashboard", "host unclear; verdict surface is event-driven (tribunal reckoning unwired per debt ledger)"),
            ("skill_matrix", "natural host SurvivorDetailPanel is ACTIVE-claimed (claim-c1-plan24-survivor-ledger)"),
            ("brine_extraction", "natural launcher GameDashboardPanel is ACTIVE-claimed (claim-pfgl-codex-luna6-octet)"),
            ("aquifer_treaty_concession", "natural launcher GameDashboardPanel is ACTIVE-claimed (claim-pfgl-codex-luna6-octet)"),
            ("slurry_dewatering_sump", "natural launcher GameDashboardPanel is ACTIVE-claimed (claim-pfgl-codex-luna6-octet)"),
            ("emergency_response", "crisis HUD; auto-open trigger not yet wired — needs event-owner decision"),
            // Found by this gate on first run (2026-10-01) — ids previously
            // quoted only in dev/test surfaces, not player emitters:
            ("chem_warfare_defense", "toxic hazard monitor; host surface ambiguous (shelter vs. medical) — foreman pick"),
            ("ceremony_ritual", "festival/ceremony surface; seasonal event owner decision needed"),
            ("robotics_assembly", "workshop family surface; WorkshopPanel deep link needs foreman sign-off (workshop seam recently repaired)"),
            ("survivor_downtime", "hobbies/downtime; survivors surface family is C1-claimed territory"),
            ("winter_freeze", "deep freeze watch; weather vs. shelter host ambiguous — foreman pick"),
            ("amputation_surgery", "surgical triage; medical seam is ACTIVE-claimed (Main.Medical.cs) and tied to unwired verdict/medical debt"),
            ("justice_tribunal", "tribunal surface; Verdict tribunal reckoning is a known unwired debt item — wire with that debt, not before"),
            // Found by this gate on second run (2026-10-01) — previously
            // quoted only in dev surfaces that have since churned:
            ("archaeology_excavation", "archaeology surface; no player emitter — follow-up wiring batch"),
            ("desperation_crisis", "desperation/taboo monitor; host ambiguous (medical vs. moral) — foreman pick"),
        };

        private static string RepoRoot()
        {
            var dir = new DirectoryInfo(Path.GetFullPath(Directory.GetCurrentDirectory()));
            while (dir != null)
            {
                if (File.Exists(Path.Combine(dir.FullName, "project.godot")))
                    return dir.FullName;
                dir = dir.Parent;
            }
            throw new DirectoryNotFoundException("repository root not found");
        }

        private static string ScanBlob()
        {
            var sb = new StringBuilder();
            string root = RepoRoot();

            // Player emission surfaces: game UI/host code and authored data.
            // Excluded: the registry itself (registration), Main.PlayerSurfaces
            // (ConfigureActions wiring is not an emitter), Host/ (dev probes),
            // uitest harnesses, and this gate.
            foreach (string file in Directory.EnumerateFiles(Path.Combine(root, "src"), "*.cs", SearchOption.AllDirectories))
            {
                string rel = file.Substring(root.Length + 1).Replace('\\', '/');
                if (rel.EndsWith("PanelRegistryBootstrap.cs")) continue;
                if (rel == "src/Main.PlayerSurfaces.cs") continue;
                if (rel.StartsWith("src/Host/")) continue;
                if (rel.Contains("/Main.UiTests.")) continue;
                if (rel.EndsWith("PanelRouteReachabilityGateTests.cs")) continue;
                sb.Append(File.ReadAllText(file));
            }
            string dataDir = Path.Combine(root, "Assets", "StreamingAssets", "Data");
            foreach (string file in Directory.EnumerateFiles(dataDir, "*.json", SearchOption.AllDirectories))
                sb.Append(File.ReadAllText(file));
            return sb.ToString();
        }

        [Fact]
        public void EveryPlayerNavigableRoute_IsReachableOrExplicitlyPending()
        {
            Ashfall.Core.UI.PanelRegistryBootstrap.RegisterAll();
            string blob = ScanBlob();

            var direct = new HashSet<string>(DirectCallReachable, StringComparer.Ordinal);
            var pending = PendingForeman.Select(p => p.Id).ToHashSet(StringComparer.Ordinal);

            var unreachable = new List<string>();
            foreach (string id in Ashfall.Core.UI.PanelRegistry.AllIds)
            {
                var descriptor = Ashfall.Core.UI.PanelRegistry.Get(id);
                if (descriptor == null || !descriptor.IsPlayerNavigable) continue;
                if (direct.Contains(id) || pending.Contains(id)) continue;
                if (!blob.Contains("\"" + id + "\"", StringComparison.Ordinal))
                    unreachable.Add(id);
            }

            Assert.True(unreachable.Count == 0,
                "Registered player-navigable routes with no emitter surface (T14 gap class):\n  " +
                string.Join("\n  ", unreachable) +
                "\nWire a host-panel deep link (see Main.UiPanels R1 block) or add a written pending-foreman entry.");
        }

        [Fact]
        public void PendingForemanList_ContainsNoAlreadyReachableRoutes()
        {
            // The pending list must shrink by explicit edit: an entry whose
            // route gained an emitter or a direct funnel is stale and must be
            // removed so the route is protected by the main fact instead.
            Ashfall.Core.UI.PanelRegistryBootstrap.RegisterAll();
            string blob = ScanBlob();
            var direct = new HashSet<string>(DirectCallReachable, StringComparer.Ordinal);

            var stale = new List<string>();
            foreach (string id in PendingForeman.Select(p => p.Id))
            {
                if (direct.Contains(id) || blob.Contains("\"" + id + "\"", StringComparison.Ordinal))
                    stale.Add(id);
            }

            Assert.True(stale.Count == 0,
                "Stale pending-foreman entries (route is now reachable — remove from the list):\n  " +
                string.Join("\n  ", stale));
        }
    }
}
