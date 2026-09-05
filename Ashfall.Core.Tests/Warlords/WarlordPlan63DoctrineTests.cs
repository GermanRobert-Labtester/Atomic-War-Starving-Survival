using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Warlords;

namespace Ashfall.Core.Tests.Warlords
{
    /// <summary>
    /// Plan 63 — Warlord Doctrines Expansion (8 → 24 Strategic Profiles).
    /// Asserts full structural, behavioral, deterministic, and relational
    /// validity of all 24 strategic doctrines in warlord_doctrines.json.
    /// </summary>
    public class WarlordPlan63DoctrineTests
    {
        private static string DataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found))
                return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found))
                return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        private static WarlordDoctrineCatalog LoadCatalog()
        {
            var files = new FileSystemIO();
            return WarlordDoctrineCatalogLoader.Load(DataDir(), files, new SystemTextJsonSerializer());
        }

        [Fact]
        public void WarlordPlan63_CatalogContainsExactly24DistinctDoctrines()
        {
            var catalog = LoadCatalog();
            Assert.NotNull(catalog);
            Assert.Equal(24, catalog.Doctrines.Count);

            var ids = catalog.Doctrines.Select(d => d.id).ToList();
            Assert.Equal(24, ids.Distinct(StringComparer.Ordinal).Count());
        }

        [Fact]
        public void WarlordPlan63_PreservesAllOriginal8Doctrines()
        {
            var catalog = LoadCatalog();
            var original = new[]
            {
                "warlord_doctrine_toll",
                "warlord_doctrine_consolidation",
                "warlord_doctrine_annexation",
                "warlord_doctrine_withdrawal",
                "warlord_doctrine_besiege",
                "warlord_doctrine_traffic",
                "warlord_doctrine_ashprophet",
                "warlord_doctrine_procedure"
            };

            foreach (var id in original)
            {
                var d = catalog.GetDoctrine(id);
                Assert.NotNull(d);
                Assert.False(string.IsNullOrWhiteSpace(d!.display_name));
                Assert.False(string.IsNullOrWhiteSpace(d.description));
            }
        }

        [Fact]
        public void WarlordPlan63_IncludesAll16NewStrategicProfiles()
        {
            var catalog = LoadCatalog();
            var expectedNew = new[]
            {
                "warlord_doctrine_lightning_raider",
                "warlord_doctrine_vulture_raider",
                "warlord_doctrine_iron_perimeter",
                "warlord_doctrine_layered_redoubt",
                "warlord_doctrine_pressed_ranks",
                "warlord_doctrine_borrowed_voices",
                "warlord_doctrine_toll_kingdom",
                "warlord_doctrine_sacred_campaign",
                "warlord_doctrine_salvage_supremacy",
                "warlord_doctrine_chains_of_work",
                "warlord_doctrine_many_knives",
                "warlord_doctrine_scorched_earth",
                "warlord_doctrine_convoy_interdiction",
                "warlord_doctrine_silent_garrison",
                "warlord_doctrine_proxy_provocation",
                "warlord_doctrine_resource_stranglehold"
            };

            foreach (var id in expectedNew)
            {
                var d = catalog.GetDoctrine(id);
                Assert.NotNull(d);
                Assert.False(string.IsNullOrWhiteSpace(d!.display_name));
                Assert.False(string.IsNullOrWhiteSpace(d.description));
                Assert.InRange(d.risk_tolerance, 0.05f, 0.95f);
                Assert.NotEmpty(d.eligible_actions);
                Assert.NotEmpty(d.action_weights);
                Assert.NotEmpty(d.resource_priority);
                Assert.NotEmpty(d.transitions);
            }
        }

        [Fact]
        public void WarlordPlan63_AllDoctrinesHaveValidTransitionsAndWeights()
        {
            var catalog = LoadCatalog();
            var validSignals = new HashSet<string>(StringComparer.Ordinal)
            {
                "supply_ratio",
                "failure_streak",
                "success_streak",
                "contested_count",
                "player_tribute_reliability",
                "environment_hazard",
                "rival_pressure"
            };

            var validActions = new HashSet<string>(StringComparer.Ordinal)
            {
                "demand_tribute",
                "raid",
                "defend",
                "contest",
                "annex",
                "withdraw"
            };

            var allIds = new HashSet<string>(catalog.Doctrines.Select(d => d.id), StringComparer.Ordinal);

            foreach (var d in catalog.Doctrines)
            {
                Assert.NotNull(d);
                Assert.NotEmpty(d.id);
                Assert.False(string.IsNullOrWhiteSpace(d.display_name));
                Assert.False(string.IsNullOrWhiteSpace(d.description));
                Assert.InRange(d.risk_tolerance, 0f, 1f);

                // Eligible actions must be known
                foreach (var action in d.eligible_actions)
                {
                    Assert.Contains(action, validActions);
                }

                // Action weights must reference eligible actions
                foreach (var kv in d.action_weights)
                {
                    Assert.Contains(kv.Key, d.eligible_actions);
                    Assert.True(kv.Value > 0, $"Action weight for {kv.Key} in {d.id} must be positive.");
                }

                // Transitions must target defined doctrines and use valid signals
                foreach (var tr in d.transitions)
                {
                    Assert.Contains(tr.to, allIds);
                    Assert.Contains(tr.signal, validSignals);
                    Assert.True(tr.condition == "gte" || tr.condition == "lt",
                        $"Condition in {d.id} transition must be gte or lt, got {tr.condition}");
                }
            }
        }

        [Fact]
        public void WarlordPlan63_WarlordCatalogValidator_ReportsClean()
        {
            var files = new FileSystemIO();
            var catalog = LoadCatalog();
            var report = WarlordCatalogValidator.Validate(catalog, DataDir(), files);

            Assert.True(report.Clean, "WarlordCatalogValidator errors: " + string.Join("; ", report.Errors));
            Assert.True(report.AliasWarnings.Count > 0, "Alias warnings should be retained.");
        }

        [Fact]
        public void WarlordPlan63_SeededSimulationWithNewDoctrines_IsDeterministic()
        {
            var catalog = LoadCatalog();

            var newDoctrines = new[]
            {
                "warlord_doctrine_lightning_raider",
                "warlord_doctrine_vulture_raider",
                "warlord_doctrine_iron_perimeter",
                "warlord_doctrine_layered_redoubt",
                "warlord_doctrine_pressed_ranks",
                "warlord_doctrine_borrowed_voices",
                "warlord_doctrine_toll_kingdom",
                "warlord_doctrine_sacred_campaign",
                "warlord_doctrine_salvage_supremacy",
                "warlord_doctrine_chains_of_work",
                "warlord_doctrine_many_knives",
                "warlord_doctrine_scorched_earth",
                "warlord_doctrine_convoy_interdiction",
                "warlord_doctrine_silent_garrison",
                "warlord_doctrine_proxy_provocation",
                "warlord_doctrine_resource_stranglehold"
            };

            foreach (var doctrineId in newDoctrines)
            {
                var sysA = new WarlordDoctrineSystem(catalog, seedSalt: 4242);
                sysA.State.doctrineId = doctrineId;

                var sysB = new WarlordDoctrineSystem(catalog, seedSalt: 4242);
                sysB.State.doctrineId = doctrineId;

                var actionsA = new List<string>();
                var actionsB = new List<string>();
                sysA.OnActionExecuted += act => actionsA.Add(act.Action + ":" + act.TargetLocationId);
                sysB.OnActionExecuted += act => actionsB.Add(act.Action + ":" + act.TargetLocationId);

                var ctx = new WarlordContext
                {
                    EnvironmentHazard = 0.3f,
                    RivalPressure = 0.4f,
                    PlayerStanding = 10
                };

                for (int day = 1; day <= 15; day++)
                {
                    var rngA = new SeededRng(4242 + day);
                    var rngB = new SeededRng(4242 + day);

                    sysA.TickDaily(day, rngA, ctx);
                    sysB.TickDaily(day, rngB, ctx);

                    Assert.Equal(sysA.DoctrineId, sysB.DoctrineId);
                    Assert.Equal(sysA.State.supply, sysB.State.supply);
                }

                Assert.Equal(actionsA.Count, actionsB.Count);
                for (int i = 0; i < actionsA.Count; i++)
                {
                    Assert.Equal(actionsA[i], actionsB[i]);
                }
            }
        }
    }
}
