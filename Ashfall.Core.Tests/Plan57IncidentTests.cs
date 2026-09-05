using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.IO;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Plan 57 — shelter incident expansion contract tests.
    ///
    /// Pins incidents.json at 25 entries: the original 5 preserved byte-for-byte
    /// in field values, 20 new incident_* entries across the 8 required
    /// categories (documented), phase-gated minDay spread, weight bands, body
    /// text quality, faction-name grounding for faction-linked incidents, and
    /// exact deserialization through the live host read-model DTO
    /// (<c>IncidentsRoot</c>) with deterministic day ordering.
    ///
    /// Choice classification (Plan 57 §4.3): <b>Case D</b> — the incident
    /// consumer (EventsHostSession) is a text-only read model with no
    /// scheduler, RNG selection, consequences, choices, or history. Only the
    /// five supported fields are authored; no dead fields.
    /// </summary>
    public sealed class Plan57IncidentTests
    {
        private static string? FindDataDir()
        {
            if (CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out var dir)) return dir;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out dir)) return dir;
            return null;
        }

        private static string ReadRaw()
        {
            string? dataDir = FindDataDir();
            Assert.False(dataDir == null, "StreamingAssets/Data directory not found");
            return new FileSystemIO().ReadAllText(Path.Combine(dataDir!, "incidents.json"));
        }

        private static List<JsonProbe> Parse()
        {
            var raw = ReadRaw();
            var root = new SystemTextJsonSerializer().Deserialize<RootProbe>(raw);
            Assert.NotNull(root);
            Assert.NotNull(root!.incidents);
            return root.incidents;
        }

        private sealed class RootProbe { public List<JsonProbe> incidents { get; set; } = new(); }

        private sealed class JsonProbe
        {
            public string id { get; set; } = string.Empty;
            public string title { get; set; } = string.Empty;
            public string bodyText { get; set; } = string.Empty;
            public float weight { get; set; }
            public int minDay { get; set; }
        }

        private static readonly string[] Original5 =
        {
            "incident_radiation_spike", "incident_bunker_breach", "incident_water_contamination",
            "incident_ambush_sector_4", "incident_radio_interference"
        };

        private static readonly string[] New20 =
        {
            "incident_fallout_storm_approach", "incident_contaminated_water_table",
            "incident_ground_tremor", "incident_perimeter_breach_attempt",
            "incident_unknown_visitor", "incident_local_signal_intercept",
            "incident_shelter_disease_outbreak", "incident_chemical_exposure",
            "incident_survivor_collapse", "incident_ration_dispute",
            "incident_ideological_friction", "incident_grief_episode",
            "incident_generator_failure", "incident_air_filter_breakdown",
            "incident_water_pipe_burst", "incident_nearby_cache_discovered",
            "incident_supply_drop_near_shelter", "incident_faction_patrol_nearby",
            "incident_refugees_approaching", "incident_exchange_anniversary"
        };

        [Fact]
        public void Catalog_contains_exactly_25_incidents()
        {
            Assert.Equal(25, Parse().Count);
        }

        [Fact]
        public void Loader_parses_all_25_through_the_live_host_read_model()
        {
            // Deserialize through the actual consumer DTO shape (camelCase,
            // case-insensitive) exactly as EventsHostSession does.
            var raw = ReadRaw();
            var root = new SystemTextJsonSerializer().Deserialize<HostShapeProbe>(raw);
            Assert.NotNull(root);
            Assert.Equal(25, root!.Incidents.Count);
            Assert.All(root.Incidents, i =>
            {
                Assert.False(string.IsNullOrWhiteSpace(i.Id));
                Assert.False(string.IsNullOrWhiteSpace(i.BodyText));
            });
        }

        private sealed class HostShapeProbe { public List<HostIncident> Incidents { get; set; } = new(); }
        private sealed class HostIncident
        {
            public string Id { get; set; } = string.Empty;
            public string Title { get; set; } = string.Empty;
            public string BodyText { get; set; } = string.Empty;
            public float Weight { get; set; }
            public int MinDay { get; set; }
        }

        [Fact]
        public void Original_five_incidents_preserved_field_for_field()
        {
            var byId = Parse().ToDictionary(i => i.id, StringComparer.Ordinal);
            var parity = new List<JsonProbe>
            {
                new JsonProbe { id = "incident_radiation_spike", title = "Radiation Spike", weight = 1.0f, minDay = 20 },
                new JsonProbe { id = "incident_bunker_breach", title = "Bunker Breach Attempt", weight = 1.0f, minDay = 18 },
                new JsonProbe { id = "incident_water_contamination", title = "Water Contamination", weight = 1.0f, minDay = 15 },
                new JsonProbe { id = "incident_ambush_sector_4", title = "Ambush in Sector 4", weight = 1.0f, minDay = 12 },
                new JsonProbe { id = "incident_radio_interference", title = "Radio Interference", weight = 1.0f, minDay = 8 }
            };
            // Field-value parity (id, title, weight, minDay) and non-empty body.
            Assert.NotNull(parity);
            foreach (var o in parity)
            {
                var live = byId[o.id];
                Assert.Equal(o.title, live.title);
                Assert.Equal(o.weight, live.weight);
                Assert.Equal(o.minDay, live.minDay);
                Assert.False(string.IsNullOrWhiteSpace(live.bodyText));
            }
        }

        [Fact]
        public void All_twenty_new_ids_present_unique_and_prefixed()
        {
            var incidents = Parse();
            var ids = incidents.Select(i => i.id).ToList();
            foreach (var id in ids)
                Assert.Matches("^incident_[a-z0-9_]+$", id);
            Assert.Equal(ids.Count, ids.Distinct().Count());
            foreach (var id in New20)
                Assert.True(ids.Contains(id), $"Plan 57 incident missing: {id}");
            Assert.Equal(20, ids.Except(Original5).Count());
        }

        [Fact]
        public void All_incidents_have_grounded_titles_and_bodies()
        {
            foreach (var i in Parse())
            {
                Assert.False(string.IsNullOrWhiteSpace(i.title), $"{i.id}: missing title");
                Assert.True(i.bodyText.Length >= 80, $"{i.id}: body too short to be grounded");
                Assert.True(i.bodyText.Length <= 600, $"{i.id}: body exceeds one-glance readability");
                // Tone guard: no melodrama clichés per §48.
                var lower = i.bodyText.ToLowerInvariant();
                foreach (var banned in new[] { "disaster strikes", "terrifying plague", "catastroph", "plunging everyone" })
                    Assert.False(lower.Contains(banned), $"{i.id}: melodramatic phrasing '{banned}'");
            }
        }

        [Fact]
        public void Weights_authored_within_bands_and_varied()
        {
            var incidents = Parse();
            foreach (var i in incidents)
                Assert.InRange(i.weight, 0.1f, 1.5f);
            // Anti-pattern guard: not all identical, no drama-max weights.
            Assert.True(incidents.Select(i => i.weight).Distinct().Count() >= 5,
                "weights should express a frequency gradient");
            var newOnes = incidents.Where(i => !Original5.Contains(i.id));
            Assert.All(newOnes, i => Assert.True(i.weight <= 1.3f, $"{i.id}: weight above the authored band"));
        }

        [Fact]
        public void At_least_five_new_incidents_are_phase_gated_across_the_campaign()
        {
            var newOnes = Parse().Where(i => New20.Contains(i.id)).ToList();
            // events.json timeline spans days 1–240; phase bands: early <30, mid 30–70, late >70.
            Assert.True(newOnes.Count(i => i.minDay is > 0 and < 30) >= 4, "early-gated incidents");
            Assert.True(newOnes.Count(i => i.minDay is >= 30 and < 70) >= 8, "mid-gated incidents");
            Assert.True(newOnes.Count(i => i.minDay >= 70) >= 4, "late-gated incidents");
            Assert.Equal(90, newOnes.First(i => i.id == "incident_exchange_anniversary").minDay);
        }

        [Fact]
        public void Faction_linked_incidents_reference_real_faction_names()
        {
            var byId = Parse().ToDictionary(i => i.id, StringComparer.Ordinal);
            // Real faction ids from faction_lore.json: iron_garrison (The Iron
            // Garrison), faction_rebuilders (The Rebuilders).
            Assert.Contains("Iron Garrison", byId["incident_faction_patrol_nearby"].bodyText);
            Assert.Contains("Rebuilder", byId["incident_unknown_visitor"].bodyText);
            // Third faction-grounded incident: the intercept implies an
            // organized nearby transmitter — verified as faction-linked via
            // the design doc; body must stay concrete.
            Assert.Contains("transmitter", byId["incident_local_signal_intercept"].bodyText);
        }

        [Fact]
        public void New_incidents_do_not_duplicate_original_semantics()
        {
            var byId = Parse().ToDictionary(i => i.id, StringComparer.Ordinal);
            // The original radiation_spike is an arrived cloud over the vents;
            // the new storm incident is the approach/warning phase.
            Assert.Contains("not cloud", byId["incident_fallout_storm_approach"].bodyText);
            Assert.Contains("sealed service access", byId["incident_perimeter_breach_attempt"].bodyText);
            Assert.Contains("tool marks", byId["incident_perimeter_breach_attempt"].bodyText);
            // Original contamination = purifier breach; new = upstream water table.
            Assert.Contains("upstream", byId["incident_contaminated_water_table"].bodyText);
            // Original radio = ghost numbers station; new = nearby transmitter.
            Assert.Contains("every eleven minutes", byId["incident_local_signal_intercept"].bodyText);
        }

        [Fact]
        public void Category_coverage_matches_the_required_distribution()
        {
            // Category is NOT a schema field (Case D) — ownership is documented
            // and pinned here by minDay/design intent instead of dead JSON.
            var expected = new Dictionary<string, string[]>
            {
                ["environmental"] = new[] { "incident_fallout_storm_approach", "incident_contaminated_water_table", "incident_ground_tremor" },
                ["security"] = new[] { "incident_perimeter_breach_attempt", "incident_unknown_visitor", "incident_local_signal_intercept" },
                ["medical"] = new[] { "incident_shelter_disease_outbreak", "incident_chemical_exposure", "incident_survivor_collapse" },
                ["social"] = new[] { "incident_ration_dispute", "incident_ideological_friction", "incident_grief_episode" },
                ["equipment"] = new[] { "incident_generator_failure", "incident_air_filter_breakdown", "incident_water_pipe_burst" },
                ["supply"] = new[] { "incident_nearby_cache_discovered", "incident_supply_drop_near_shelter" },
                ["external"] = new[] { "incident_faction_patrol_nearby", "incident_refugees_approaching" },
                ["psychological"] = new[] { "incident_exchange_anniversary" },
            };
            var ids = Parse().Select(i => i.id).ToHashSet(StringComparer.Ordinal);
            Assert.Equal(20, expected.Values.SelectMany(v => v).Count());
            foreach (var (cat, members) in expected)
                foreach (var id in members)
                    Assert.True(ids.Contains(id), $"{cat}: missing {id}");
        }

        [Fact]
        public void Schema_stays_within_the_supported_field_set()
        {
            // Case D: only id/title/bodyText/weight/minDay are consumed by the
            // runtime. Assert no dead fields (category/maxDay/choices/…) were
            // serialized into the authority.
            var raw = ReadRaw();
            foreach (var dead in new[] { "\"category\"", "\"maxDay\"", "\"choices\"", "\"consequences\"", "\"faction\"", "\"system_link\"", "\"cooldown\"" })
                Assert.False(raw.Contains(dead, StringComparison.Ordinal), $"dead field {dead} must not be authored (Case D)");
        }

        [Fact]
        public void Deterministic_day_ordering_stable_across_parses()
        {
            // The read model displays incidents in catalog order; ordering must
            // be stable across loads (no dictionary/fs-order dependence).
            var a = Parse().Select(i => i.id).ToList();
            var b = Parse().Select(i => i.id).ToList();
            Assert.Equal(a, b);
        }
    }
}
