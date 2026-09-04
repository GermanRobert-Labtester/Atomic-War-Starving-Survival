using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Disease;
using Ashfall.Core.Radio;
using Ashfall.Core.Subterranean;
using Ashfall.Core.Survivors;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.Flagship11;

/// <summary>
/// Shared fixture for Flagship XI tests: locates Assets/StreamingAssets/Data by
/// walking up from the test bin / CWD (the CatalogTestBase pattern, inlined so
/// the companion verification project compiles only this folder).
/// </summary>
public abstract class Flagship11TestBase
{
    protected static string FindDataDirectory()
    {
        foreach (string start in new[] { Directory.GetCurrentDirectory(), AppContext.BaseDirectory })
        {
            if (string.IsNullOrEmpty(start)) continue;
            DirectoryInfo? dir = new DirectoryInfo(start);
            while (dir != null)
            {
                string candidate = Path.Combine(dir.FullName, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(candidate)) return candidate;
                dir = dir.Parent;
            }
        }
        throw new InvalidOperationException("Could not locate Assets/StreamingAssets/Data from test run directory.");
    }
}

public class Flagship11CatalogTests : Flagship11TestBase
{
    private static ContagionEventsCatalogContainer LoadContagion()
    {
        string dir = FindDataDirectory();
        return ContagionEventCatalogLoader.Load(dir, new FileSystemIO(), new SystemTextJsonSerializer());
    }

    private static PathogenStrainCatalogContainer LoadPathogens()
    {
        string dir = FindDataDirectory();
        return PathogenStrainCatalogLoader.Load(dir, new FileSystemIO(), new SystemTextJsonSerializer());
    }

    private static SubterraneanZoneCatalogContainer LoadZones()
    {
        string dir = FindDataDirectory();
        return SubterraneanZoneCatalogLoader.Load(dir, new FileSystemIO(), new SystemTextJsonSerializer());
    }

    private static PsyOpsCatalogContainer LoadPsyOps()
    {
        string dir = FindDataDirectory();
        return PsyOpsCatalogLoader.Load(dir, new FileSystemIO(), new SystemTextJsonSerializer());
    }

    private static readonly string[] KnownEmotions = { "hope", "despair", "panic" };
    private static readonly string[] KnownThemes =
        { "hope", "defectionappeal", "aidpromise", "fear", "unity", "counterrumor" };
    private static readonly string[] KnownZoneTypes =
        { "cave", "metro", "utilitytunnel", "bunker", "mine", "collapsedfacility" };
    private static readonly string[] KnownOxygenClasses = { "stale", "thin", "foul" };

    // ---------- Plan 154: contagion_events.json ----------

    [Fact]
    public void ContagionCatalog_LoadsSixAuthoredEvents()
    {
        var catalog = LoadContagion();
        Assert.Equal(1, catalog.schema_version);
        Assert.True(catalog.contagion_events.Count >= 6, "expected the six authored contagion events");
        Assert.All(catalog.contagion_events, e => Assert.Equal("contagion_", e.id.Substring(0, 10)));
    }

    [Fact]
    public void ContagionCatalog_IdsAreUniqueAndFieldsValid()
    {
        var catalog = LoadContagion();
        var ids = catalog.contagion_events.Select(e => e.id).ToList();
        Assert.Equal(ids.Count, ids.Distinct(StringComparer.Ordinal).Count());

        foreach (var e in catalog.contagion_events)
        {
            Assert.False(string.IsNullOrWhiteSpace(e.display_name));
            Assert.Contains(e.emotion_type.ToLowerInvariant(), KnownEmotions);
            Assert.InRange(e.base_intensity, 0f, 1f);
            Assert.InRange(e.recovery_per_day, 0f, 1f);
            Assert.True(e.duration_days >= 1, $"{e.id} duration_days must be >= 1");
            Assert.True(e.bond_multiplier > 0f, $"{e.id} bond_multiplier must be positive");
            Assert.True(e.proximity_multiplier > 0f, $"{e.id} proximity_multiplier must be positive");
        }
    }

    [Fact]
    public void ContagionCatalog_HasHopeDespairAndPanicChannels()
    {
        var catalog = LoadContagion();
        var emotions = catalog.contagion_events.Select(e => e.emotion_type.ToLowerInvariant()).Distinct().ToList();
        Assert.Contains("hope", emotions);
        Assert.Contains("despair", emotions);
        Assert.Contains("panic", emotions);
    }

    // ---------- Plan 155: pathogens.json ----------

    [Fact]
    public void PathogenCatalog_LoadsFourFictionalStrains()
    {
        var catalog = LoadPathogens();
        Assert.Equal(1, catalog.schema_version);
        Assert.True(catalog.pathogen_strains.Count >= 4, "expected the four authored strains");
        Assert.Contains(catalog.pathogen_strains, s => s.id == "pathogen_ash_fever");
        Assert.Contains(catalog.pathogen_strains, s => s.id == "pathogen_red_lung");
        Assert.Contains(catalog.pathogen_strains, s => s.id == "pathogen_frost_rot");
        Assert.Contains(catalog.pathogen_strains, s => s.id == "pathogen_glass_cough");
    }

    [Fact]
    public void PathogenCatalog_StrainsReferenceRealParentDiseases()
    {
        string dir = FindDataDirectory();
        var strains = LoadPathogens().pathogen_strains;

        var diseaseLoader = typeof(DiseaseCatalogLoader);
        Assert.NotNull(diseaseLoader); // the Disease authority ships its own loader
        var diseaseCatalog = Ashfall.Core.Disease.DiseaseCatalogLoader.Load(
            dir, new FileSystemIO(), new SystemTextJsonSerializer());
        var diseaseIds = diseaseCatalog.Diseases.Select(d => d.id).ToHashSet(StringComparer.Ordinal);

        foreach (var s in strains)
        {
            Assert.True(diseaseIds.Contains(s.strain_of),
                $"{s.id} names unknown parent disease '{s.strain_of}'");
        }
    }

    [Fact]
    public void PathogenCatalog_MutationGraphStaysInsideStrainIdsAndValuesAreBounded()
    {
        var catalog = LoadPathogens();
        var strainIds = catalog.pathogen_strains.Select(s => s.id).ToHashSet(StringComparer.Ordinal);

        foreach (var s in catalog.pathogen_strains)
        {
            Assert.InRange(s.lethality, 0f, 1f);
            Assert.InRange(s.infectivity, 0f, 1f);
            Assert.InRange(s.radiation_severity_gain, 0f, 1f);
            Assert.InRange(s.mutation_chance_per_day, 0f, 1f);
            Assert.True(s.incubation_days >= 0);
            Assert.True(s.illness_days >= 1);
            foreach (string target in s.mutation_targets)
                Assert.True(strainIds.Contains(target),
                    $"{s.id} mutates into unknown strain '{target}'");
        }
    }

    // ---------- Plan 156: subterranean_zones.json ----------

    [Fact]
    public void ZoneCatalog_LoadsTenZonesAcrossThreeDepthTiers()
    {
        var catalog = LoadZones();
        Assert.Equal(1, catalog.schema_version);
        Assert.True(catalog.subterranean_zones.Count >= 10);
        var tiers = catalog.subterranean_zones.Select(z => z.depth_tier).Distinct().OrderBy(t => t).ToList();
        Assert.Equal(new[] { 1, 2, 3 }, tiers);
    }

    [Fact]
    public void ZoneCatalog_AnchorsTablesAndConnectionsResolve()
    {
        string dir = FindDataDirectory();
        var zones = LoadZones().subterranean_zones;
        var zoneIds = zones.Select(z => z.id).ToHashSet(StringComparer.Ordinal);

        // Surface anchors must be real wasteland map nodes.
        var mapRaw = new FileSystemIO().ReadAllText(Path.Combine(dir, "wasteland_map_v1.json"));
        Assert.False(string.IsNullOrWhiteSpace(mapRaw));
        var mapIds = System.Text.Json.JsonDocument.Parse(mapRaw)
            .RootElement.GetProperty("nodes").EnumerateArray()
            .Select(n => n.GetProperty("id").GetString()!)
            .ToHashSet(StringComparer.Ordinal);

        // Loot tables must be real scavenging tables.
        var tablesRaw = new FileSystemIO().ReadAllText(Path.Combine(dir, "scavenging_tables.json"));
        var tableIds = System.Text.Json.JsonDocument.Parse(tablesRaw)
            .RootElement.GetProperty("tables").EnumerateArray()
            .Select(t => t.GetProperty("id").GetString()!)
            .ToHashSet(StringComparer.Ordinal);

        foreach (var z in zones)
        {
            Assert.False(string.IsNullOrWhiteSpace(z.display_name));
            Assert.Contains(z.zone_type.ToLowerInvariant(), KnownZoneTypes);
            Assert.Contains(z.oxygen_class.ToLowerInvariant(), KnownOxygenClasses);
            Assert.InRange(z.base_structural_risk, 0f, 1f);
            Assert.InRange(z.flood_susceptibility, 0f, 1f);
            Assert.True(mapIds.Contains(z.surface_anchor_id),
                $"{z.id} anchored at unknown surface node '{z.surface_anchor_id}'");
            Assert.True(tableIds.Contains(z.scavenging_table_id),
                $"{z.id} references unknown loot table '{z.scavenging_table_id}'");

            foreach (var c in z.connection_rules)
            {
                Assert.True(zoneIds.Contains(c.to),
                    $"{z.id} connects to unknown zone '{c.to}'");
                if (!string.IsNullOrEmpty(c.from))
                    Assert.True(zoneIds.Contains(c.from),
                        $"{z.id} declares unknown connection origin '{c.from}'");
            }
        }
    }

    // ---------- Plan 157: propaganda_campaigns.json ----------

    [Fact]
    public void PsyOpsCatalog_LoadsEightCampaignsOnRealFactions()
    {
        var catalog = LoadPsyOps();
        Assert.Equal(1, catalog.schema_version);
        Assert.True(catalog.propaganda_campaigns.Count >= 8);

        // Only faction ids verifiably present in the live data corpus.
        var knownFactions = new HashSet<string>(StringComparer.Ordinal)
        {
            "faction_the_office", "faction_the_cutters", "faction_the_fleet",
            "faction_black_flotilla", "faction_supply_corps", "faction_railway_guild",
            "faction_hydro_barons", "faction_ordnance_foundry", "faction_scavengers",
            "faction_central_garrison"
        };

        foreach (var c in catalog.propaganda_campaigns)
        {
            Assert.StartsWith("psyops_", c.id);
            Assert.False(string.IsNullOrWhiteSpace(c.display_name));
            Assert.Contains(c.message_theme.ToLowerInvariant(), KnownThemes);
            Assert.Contains(c.countered_by.ToLowerInvariant(), KnownThemes);
            Assert.True(knownFactions.Contains(c.target_faction_id),
                $"{c.id} targets unknown faction '{c.target_faction_id}'");
            Assert.InRange(c.base_reach, 0f, 100f);
            Assert.InRange(c.receptiveness, 0f, 1f);
            Assert.True(c.duration_days >= 1);
            Assert.True(c.power_demand_watts > 0f, $"{c.id} must declare a power demand");
            Assert.True(c.loyalty_pressure_per_day > 0f);
        }
    }

    [Fact]
    public void PsyOpsCatalog_CampaignIdsAreUniqueAndTargetMultipleFactions()
    {
        var catalog = LoadPsyOps();
        var ids = catalog.propaganda_campaigns.Select(c => c.id).ToList();
        Assert.Equal(ids.Count, ids.Distinct(StringComparer.Ordinal).Count());
        Assert.True(catalog.propaganda_campaigns.Select(c => c.target_faction_id).Distinct().Count() >= 6,
            "campaigns should span the faction landscape, not gang up on one target");
    }
}
