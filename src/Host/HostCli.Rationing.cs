// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : RationingSelfTest
// Subsystem          : Plan 215 — Crisis Rationing Overlay Completion
// ============================================================================

using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.Economy;

namespace AtomicWar.GodotApp
{
    public static class HostCliRationing
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Crisis Rationing Overlay Self-Test (Plan 215) ===");
            int passed = 0;
            const int total = 12;

            try
            {
                string dataRoot = (!string.IsNullOrEmpty(dataDir) && Directory.Exists(dataDir) ? dataDir : CatalogPath.ResolveDataDir());

                // Check 1: the authored protocol catalog loads through the
                // strict loader with all four authored rows.
                var load = RationingProtocolCatalogLoader.Load(dataRoot, CatalogPath.CreateFileIOForDataDir(dataRoot));
                if (!load.HasErrors && load.Protocols.Count == 4)
                {
                    Console.WriteLine($"[PASS] Check 1: Authored protocol catalog loaded ({load.Protocols.Count} protocols).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 1: protocol catalog load failed ({load.Protocols.Count} rows, errors: {string.Join("; ", load.Errors)}).");
                }

                // Check 2: the canonical system accepts the validated rows and
                // resolves the default protocol.
                var rationing = new ResourceRationingSystem();
                rationing.LoadCatalog(new RationingProtocolCatalogData { SchemaVersion = 1, Protocols = load.Protocols });
                if (rationing.GetProtocol("protocol_standard_distribution") != null
                    && rationing.GetProtocol("protocol_triage_survival") != null)
                {
                    Console.WriteLine("[PASS] Check 2: Canonical system resolves authored protocol definitions.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 2: authored protocol definitions not resolvable via GetProtocol.");
                }

                // Check 3: the strict loader rejects an unknown default tier.
                var badDoc = new RationingProtocolDocument
                {
                    schema_version = 1,
                    protocols = new List<RationingProtocolRow>
                    {
                        new() { id = "protocol_bad", name = "Bad", default_tier = "eleventy", morale_penalty_scale = 1f }
                    }
                };
                var badLoad = RationingProtocolCatalogLoader.Validate(badDoc);
                if (badLoad.HasErrors && badLoad.Protocols.Count == 0)
                {
                    Console.WriteLine("[PASS] Check 3: Strict loader rejects an unknown default tier row.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 3: strict loader accepted an invalid tier row.");
                }

                // Check 4: unknown protocol refused once the catalog is loaded.
                bool unknownRefused = !rationing.ApplyProtocol("protocol_nonexistent", Array.Empty<string>(), 1);
                if (unknownRefused && rationing.ActiveProtocolId != "protocol_nonexistent")
                {
                    Console.WriteLine("[PASS] Check 4: Unknown protocol id refused without mutation.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 4: unknown protocol id was not refused.");
                }

                // Check 5: lawful protocol application sets the default tier
                // on a targeted resource and emits exactly one tier event.
                rationing.BindResourceValidator(id => id == "item_canned_beans");
                var tier = rationing.SetRationTier("item_canned_beans", RationingTier.Full, 1);
                int eventsBefore = rationing.EventCount;
                bool applied = rationing.ApplyProtocol("protocol_emergency_crisis", new[] { "item_canned_beans" }, 2);
                int eventsAfter = rationing.EventCount;
                if (applied && rationing.ActiveProtocolId == "protocol_emergency_crisis"
                    && rationing.GetRationTier("item_canned_beans") == RationingTier.Half
                    && eventsAfter == eventsBefore + 1)
                {
                    Console.WriteLine("[PASS] Check 5: Protocol applied the default tier and emitted one tier event.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 5: protocol application wrong (applied={applied}, tier={rationing.GetRationTier("item_canned_beans")}, events {eventsBefore}->{eventsAfter}).");
                }

                // Check 6: authorization restricts allocations under rationing.
                var decision = rationing.AuthorizeAllocation(new ResourceAllocationRequest
                {
                    ResourceId = "item_canned_beans",
                    ConsumerId = "survivor_a",
                    DemandUnits = 2,
                    AvailableUnits = 10,
                    CurrentDay = 3
                });
                if (decision.Authorized && decision.AllocatedUnits < 2 && decision.AllocatedUnits >= 0)
                {
                    Console.WriteLine($"[PASS] Check 6: Authorization restricts demand under the active protocol (allocated {decision.AllocatedUnits}/2).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 6: authorization decision unexpected (authorized={decision.Authorized}, allocated={decision.AllocatedUnits}).");
                }

                // Check 7: restore does NOT re-fire tier events (no replay).
                var snapshot = rationing.CaptureState();
                int eventsAtCapture = rationing.EventCount;
                var restored = new ResourceRationingSystem();
                restored.LoadCatalog(new RationingProtocolCatalogData { SchemaVersion = 1, Protocols = load.Protocols });
                restored.BindResourceValidator(id => id == "item_canned_beans");
                restored.RestoreState(snapshot);
                int eventsAfterRestore = restored.EventCount;
                if (restored.ActiveProtocolId == "protocol_emergency_crisis"
                    && restored.GetRationTier("item_canned_beans") == RationingTier.Half
                    && eventsAfterRestore == eventsAtCapture)
                {
                    Console.WriteLine("[PASS] Check 7: Restore round-trips policy without re-firing tier events.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 7: restore parity broken (protocol={restored.ActiveProtocolId}, events {eventsAtCapture}->{eventsAfterRestore}).");
                }

                // Check 8: restore filters rows that fail the bound validator.
                var tampered = new ResourceRationingState
                {
                    ActiveProtocolId = "protocol_standard_distribution",
                    Targets = new List<RationTarget>
                    {
                        new() { ResourceId = "item_canned_beans", Tier = RationingTier.Half, BaseMultiplier = 0.5f },
                        new() { ResourceId = "item_not_in_catalog", Tier = RationingTier.None, BaseMultiplier = 0f }
                    }
                };
                var filtered = new ResourceRationingSystem();
                filtered.BindResourceValidator(id => id == "item_canned_beans");
                filtered.RestoreState(tampered);
                if (filtered.TargetCount == 1)
                {
                    Console.WriteLine("[PASS] Check 8: Restore ignores saved rows that fail the canonical validator.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 8: restore accepted invalid rows (targets={filtered.TargetCount}).");
                }

                // Check 9: the nested save custody stays inside the economy
                // section (MarketState.rationing) — no second ration store.
                bool nestedCustody = typeof(MarketState).GetField("rationing", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance) != null;
                if (nestedCustody)
                {
                    Console.WriteLine("[PASS] Check 9: Rationing snapshot is nested in the canonical economy save (MarketState.rationing).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 9: nested economy save custody missing.");
                }

                // Check 10: host forwarder — EconomyHostSession.Create loads the
                // authored protocols before restoring the saved policy id.
                var session = EconomyHostSession.Create(dataRoot);
                bool hostLoaded = false;
                foreach (var proto in session.RationingProtocols)
                {
                    if (proto?.Id == "protocol_triage_survival") { hostLoaded = true; break; }
                }
                if (hostLoaded)
                {
                    Console.WriteLine("[PASS] Check 10: Host session loads the authored protocol catalog at Create.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 10: host session did not load the authored protocol catalog.");
                }

                // Check 11: host forwarder refuses unknown protocol and applies
                // a lawful one through the one player route.
                bool hostRefused = !session.ApplyRationingProtocol("protocol_nonexistent", 1);
                bool hostApplied = session.ApplyRationingProtocol("protocol_tightened_reserves", 2)
                    && session.Rationing.ActiveProtocolId == "protocol_tightened_reserves";
                if (hostRefused && hostApplied)
                {
                    Console.WriteLine("[PASS] Check 11: Host protocol command refuses unknown ids and applies lawful ones.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 11: host protocol command broken (refused={hostRefused}, applied={hostApplied}).");
                }

                // Check 12: production wiring gate — the player route and the
                // duplicate-restore reconciliation are present in source.
                string? mainEconomy = null;
                for (var dir = new DirectoryInfo(AppContext.BaseDirectory); dir != null; dir = dir.Parent)
                {
                    string candidate = Path.Combine(dir.FullName, "src", "Main.Economy.cs");
                    if (File.Exists(candidate)) { mainEconomy = File.ReadAllText(candidate); break; }
                }
                bool wiring = mainEconomy != null
                    && mainEconomy.Contains("ApplyRationingProtocolCommand")
                    && mainEconomy.Contains("_economy.ApplyRationingProtocol(protocolId, day)")
                    && mainEconomy.Contains("EconomyHostSession.Create already")
                    && !mainEconomy.Contains("_economy.Market.RestoreState(save);");
                if (wiring)
                {
                    Console.WriteLine("[PASS] Check 12: Player protocol route bound; duplicate market restore removed.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 12: production wiring gate failed (route or reconcile missing).");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FAIL] Unexpected probe exception: {ex.Message}");
            }

            Console.WriteLine($"=== Crisis Rationing Self-Test: {passed}/{total} passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
