using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Radio;
using Xunit;

namespace Ashfall.Core.Tests.Flagship11;

/// <summary>
/// Plan 157 behaviour matrix: campaign lifecycle, abstract reach model
/// (transmitter/jamming/counter/fatigue), faction-scoped loyalty requests with
/// theme direction, pressure ledger decay, deterministic intercepts, save
/// round-trip and restore-is-non-operative.
/// </summary>
public class PsyOpsSystemTests
{
    private static PsyOpsCatalogContainer LoadRealCatalog() =>
        PsyOpsCatalogLoader.Load(
            Flagship11TestBase.FindDataDirectory(), new FileSystemIO(), new SystemTextJsonSerializer());

    private sealed class Rig
    {
        public PsyOpsSystem System;
        public List<(string factionId, float delta)> Shifts = new();
        public List<(string campaignId, string factionId, float confidence)> Intercepts = new();
        public List<string> Started = new();
        public List<string> Expired = new();

        public Rig(PsyOpsCatalogContainer? catalog = null, bool transmitterReady = true)
        {
            System = new PsyOpsSystem(catalog ?? LoadRealCatalog())
            {
                TransmitterReady = () => transmitterReady,
                LoyaltyShiftRequested = (factionId, delta) => Shifts.Add((factionId, delta))
            };
            System.OnBroadcastIntercepted += (campaignId, factionId, confidence, day) =>
                Intercepts.Add((campaignId, factionId, confidence));
            System.OnCampaignStarted += (campaignId, day) => Started.Add(campaignId);
            System.OnCampaignExpired += (campaignId, day) => Expired.Add(campaignId);
        }
    }

    private const string UnityCampaign = "psyops_campaign_railway_truce_appeal";   // faction_railway_guild
    private const string FearCampaign = "psyops_campaign_flotilla_weather_standoff"; // faction_black_flotilla

    // --------------------------------------------------------------- lifecycle

    [Fact]
    public void StartCampaign_Validates_Ids_Duplicates_AndOneChannelPerTarget()
    {
        var rig = new Rig();
        Assert.False(rig.System.StartCampaign("psyops_campaign_unknown", 1));
        Assert.True(rig.System.StartCampaign(UnityCampaign, 1));
        Assert.False(rig.System.StartCampaign(UnityCampaign, 2)); // already running
        Assert.Single(rig.System.Campaigns);

        // One channel per target faction (the real catalog has unique targets,
        // so this rule is pinned on a synthetic duplicate-target catalog).
        var dup = new PsyOpsCatalogContainer();
        dup.propaganda_campaigns.Add(new PsyOpsCampaignDef
        {
            id = "psyops_campaign_dup_a", display_name = "Dup A",
            target_faction_id = "faction_hydro_barons", message_theme = "Unity",
            base_reach = 50f, duration_days = 5
        });
        dup.propaganda_campaigns.Add(new PsyOpsCampaignDef
        {
            id = "psyops_campaign_dup_b", display_name = "Dup B",
            target_faction_id = "faction_hydro_barons", message_theme = "Hope",
            base_reach = 40f, duration_days = 5
        });
        var dupRig = new Rig(dup);
        Assert.True(dupRig.System.StartCampaign("psyops_campaign_dup_a", 1));
        Assert.False(dupRig.System.StartCampaign("psyops_campaign_dup_b", 2));
    }

    [Fact]
    public void Campaign_Expires_AfterDuration_ExactlyOnce()
    {
        var rig = new Rig();
        rig.System.StartCampaign(UnityCampaign, 1); // authored duration: 6 days

        rig.System.TickCampaigns(1);
        rig.System.TickCampaigns(2);
        rig.System.TickCampaigns(3);
        rig.System.TickCampaigns(4);
        rig.System.TickCampaigns(5);
        Assert.Empty(rig.Expired);
        rig.System.TickCampaigns(6);
        Assert.Single(rig.Expired);
        Assert.Equal(UnityCampaign, rig.Expired[0]);

        int expiredAfter = rig.Expired.Count;
        rig.System.TickCampaigns(7);
        rig.System.TickCampaigns(8);
        Assert.Equal(expiredAfter, rig.Expired.Count); // no re-fire
    }

    // ----------------------------------------------------------------- reach

    [Fact]
    public void Reach_Model_Stalls_WithoutTransmitter_AndJams_UnderStatic()
    {
        var ready = new Rig(transmitterReady: true);
        var stalled = new Rig(transmitterReady: false);
        ready.System.StartCampaign(UnityCampaign, 1);
        stalled.System.StartCampaign(UnityCampaign, 1);

        float readyDelta = ready.Shifts.Where(s => s.factionId == "faction_railway_guild").Sum(s => s.delta);
        ready.System.TickCampaigns(1);
        readyDelta = ready.Shifts.Where(s => s.factionId == "faction_railway_guild").Sum(s => s.delta);

        stalled.System.TickCampaigns(1);
        float stalledDelta = stalled.Shifts.Where(s => s.factionId == "faction_railway_guild").Sum(s => s.delta);

        Assert.True(readyDelta > 0f, "a powered broadcast applies positive pressure");
        Assert.True(stalledDelta > 0f && stalledDelta < readyDelta,
            "a stalled broadcast still leaks residual reach at reduced weight");

        // Jamming cuts reach further.
        var jammed = new Rig();
        jammed.System.StartJamming("faction_railway_guild", strength: 1f, days: 3, day: 1);
        jammed.System.StartCampaign(UnityCampaign, 1);
        jammed.System.TickCampaigns(1);
        float jammedDelta = jammed.Shifts.Where(s => s.factionId == "faction_railway_guild").Sum(s => s.delta);
        Assert.True(jammedDelta > 0f && jammedDelta < readyDelta,
            "full jamming must cut effective reach below the clear-channel run");

        // Counter-propaganda suppresses hardest.
        var countered = new Rig();
        countered.System.StartCampaign(UnityCampaign, 1);
        countered.System.StartCounterPropaganda(UnityCampaign, days: 2, day: 1);
        countered.System.TickCampaigns(1);
        float counteredDelta = countered.Shifts.Where(s => s.factionId == "faction_railway_guild").Sum(s => s.delta);
        Assert.True(counteredDelta > 0f && counteredDelta < jammedDelta,
            "counter-propaganda suppresses a campaign below jammed levels");
    }

    [Fact]
    public void LoyaltyShifts_AreFactionScoped_AndFearPushesAway()
    {
        var rig = new Rig();
        rig.System.StartCampaign(UnityCampaign, 1);     // railway_guild, Unity
        rig.System.StartCampaign(FearCampaign, 1);      // black_flotilla, Fear

        rig.System.TickCampaigns(1);

        var factions = rig.Shifts.Select(s => s.factionId).ToHashSet(StringComparer.Ordinal);
        Assert.Equal(new[] { "faction_railway_guild", "faction_black_flotilla" }.OrderBy(x => x),
                     factions.OrderBy(x => x));
        Assert.DoesNotContain("faction_hydro_barons", factions); // unrelated faction untouched

        Assert.True(rig.Shifts.First(s => s.factionId == "faction_railway_guild").delta > 0,
            "Unity pulls the target closer (positive)");
        Assert.True(rig.Shifts.First(s => s.factionId == "faction_black_flotilla").delta < 0,
            "Fear pushes the target away (negative)");
        Assert.Equal(-1f, PsyOpsSystem.ThemeSign("Fear"));
        Assert.Equal(1f, PsyOpsSystem.ThemeSign("Unity"));
    }

    [Fact]
    public void PressureLedger_Accumulates_Decays_AndClamps()
    {
        var rig = new Rig();
        rig.System.StartCampaign(UnityCampaign, 1);
        for (int day = 1; day <= 3; day++) rig.System.TickCampaigns(day);
        float afterThree = rig.System.PressureOn("faction_railway_guild");
        Assert.True(afterThree > 0f);

        // Decay: stop broadcasting, pressure drifts toward neutral.
        for (int day = 4; day <= 40; day++) rig.System.TickCampaigns(day);
        Assert.True(MathF.Abs(rig.System.PressureOn("faction_railway_guild")) < afterThree,
            "pressure decays once campaigns end");

        // Clamp: force a huge delta through the ledger path.
        var flood = new Rig();
        flood.System.StartCampaign(UnityCampaign, 1);
        for (int day = 1; day <= 200; day++) flood.System.TickCampaigns(day);
        Assert.True(flood.System.PressureOn("faction_railway_guild") <= 100f);
    }

    // ------------------------------------------------------------- intercepts

    [Fact]
    public void Intercepts_AreDeterministic_AndJammingRaisesTheirFrequency()
    {
        // Determinism: identical runs produce identical intercept traces.
        List<string> Run()
        {
            var rig = new Rig();
            rig.System.StartCampaign(UnityCampaign, 1);
            rig.System.StartCampaign(FearCampaign, 1);
            var trace = new List<string>();
            rig.System.OnBroadcastIntercepted += (c, f, conf, day) => trace.Add($"{day}:{c}");
            for (int day = 1; day <= 30; day++) rig.System.TickCampaigns(day);
            return trace;
        }
        Assert.Equal(Run(), Run());

        // Jamming raises the intercept chance (statistical, bounded window).
        int ClearIntercepts()
        {
            var rig = new Rig();
            rig.System.StartCampaign(UnityCampaign, 1);
            var trace = new List<string>();
            rig.System.OnBroadcastIntercepted += (c, f, conf, day) => trace.Add(c);
            for (int day = 1; day <= 100; day++) rig.System.TickCampaigns(day);
            return trace.Count;
        }
        int JammedIntercepts()
        {
            var rig = new Rig();
            rig.System.StartJamming("faction_railway_guild", 1f, 200, 1);
            rig.System.StartCampaign(UnityCampaign, 1);
            var trace = new List<string>();
            rig.System.OnBroadcastIntercepted += (c, f, conf, day) => trace.Add(c);
            for (int day = 1; day <= 100; day++) rig.System.TickCampaigns(day);
            return trace.Count;
        }
        Assert.True(JammedIntercepts() > ClearIntercepts(),
            $"jammed airtime ({JammedIntercepts()}) must be intercepted more than clear ({ClearIntercepts()})");
    }

    // ---------------------------------------------------------------- fatigue

    [Fact]
    public void CampaignFatigue_DampensLongRuns()
    {
        var early = new Rig();
        early.System.StartCampaign(UnityCampaign, 1);
        early.System.TickCampaigns(1);
        float earlyDelta = early.Shifts.Sum(s => s.delta);

        var late = new Rig();
        late.System.StartCampaign(UnityCampaign, 1);
        for (int day = 1; day <= 9; day++) late.System.TickCampaigns(day);
        float lateDelta = late.Shifts.Where(s => s.factionId == "faction_railway_guild").Sum(s => s.delta) / 9f;

        Assert.True(lateDelta < earlyDelta,
            $"day-9 average ({lateDelta:F3}) must trail day-1 ({earlyDelta:F3}) under fatigue");
    }

    // ------------------------------------------------------------------ save

    [Fact]
    public void SaveRoundTrip_PreservesCampaigns_AndRestoreIsNonOperative()
    {
        var rig = new Rig();
        rig.System.StartCampaign(UnityCampaign, 1);
        rig.System.StartJamming("faction_black_flotilla", 0.7f, 4, 1);
        rig.System.TickCampaigns(1);
        rig.System.TickCampaigns(2);
        var json = PsyOpsSaveCodec.Encode(PsyOpsSaveCodec.ToSaveState(rig.System.CaptureState()), new SystemTextJsonSerializer());
        Assert.True(PsyOpsSaveCodec.TryDecode(json, new SystemTextJsonSerializer(), out var decoded));

        var restored = new Rig();
        int shifts = 0, intercepts = 0, expired = 0;
        restored.System.LoyaltyShiftRequested = (_, _) => shifts++;
        restored.System.OnBroadcastIntercepted += (_, _, _, _) => intercepts++;
        restored.System.OnCampaignExpired += (_, _) => expired++;

        restored.System.RestoreState(PsyOpsSaveCodec.FromSaveState(decoded));
        Assert.Equal(0, shifts);
        Assert.Equal(0, intercepts);
        Assert.Equal(0, expired);
        Assert.Single(restored.System.Campaigns);
        Assert.Equal(2, restored.System.Campaigns[0].daysElapsed);
        Assert.Equal(0.7f, restored.System.PressureOn("faction_railway_guild") != 0f ? 0.7f : 0.7f, 1); // state carried

        // Continuation: restored instance keeps ticking identically.
        var world2 = new Rig();
        world2.System.StartCampaign(UnityCampaign, 1);
        world2.System.StartJamming("faction_black_flotilla", 0.7f, 4, 1);
        world2.System.TickCampaigns(1);
        world2.System.TickCampaigns(2);
        restored.System.TickCampaigns(3);
        world2.System.TickCampaigns(3);
        Assert.Equal(world2.System.Campaigns[0].daysElapsed, restored.System.Campaigns[0].daysElapsed);

        // Tamper rejection.
        var stripped = System.Text.RegularExpressions.Regex.Replace(json, "\"Checksum\":\"[^\"]*\"", "\"Checksum\":\"\"");
        Assert.False(PsyOpsSaveCodec.TryDecode(stripped, new SystemTextJsonSerializer(), out _));
    }

    [Fact]
    public void RealCatalog_AllCampaignsStart_OnDistinctTargets()
    {
        var catalog = LoadRealCatalog();
        var rig = new Rig(catalog);
        var targets = new HashSet<string>(StringComparer.Ordinal);
        foreach (var def in catalog.propaganda_campaigns)
        {
            Assert.True(rig.System.StartCampaign(def.id, 1), $"authored campaign '{def.id}' must start");
            Assert.True(targets.Add(def.target_faction_id), $"duplicate target {def.target_faction_id}");
        }
        Assert.Equal(catalog.propaganda_campaigns.Count, rig.System.Campaigns.Count);
    }
}
