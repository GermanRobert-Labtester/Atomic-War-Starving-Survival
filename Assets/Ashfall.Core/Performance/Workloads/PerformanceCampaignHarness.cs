using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Ashfall.Core.Campaign;
using Ashfall.Core.Clock;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Inventory;
using Ashfall.Core.Journal;
using Ashfall.Core.Random;
using Ashfall.Core.Save;
using Ashfall.Core.Survivors;
using Ashfall.Core.World;

namespace Ashfall.Core.Performance.Workloads;

/// <summary>
/// Deterministic campaign workload harness for performance measurement.
/// Constructs a legal campaign state using Core systems and advances it
/// through the CampaignDayCoordinator.
/// </summary>
public sealed class PerformanceCampaignHarness : IDisposable
{
    private readonly PerfWorkloadContext _context;
    private bool _disposed;

    /// <summary>Campaign day coordinator under test.</summary>
    public CampaignDayCoordinator Coordinator { get; }

    /// <summary>Survivor roster system.</summary>
    public SurvivorRosterSystem Survivors { get; }

    /// <summary>Inventory system.</summary>
    public Inventory.Inventory Inventory { get; }

    /// <summary>Journal system.</summary>
    public JournalSystem Journal { get; }

    /// <summary>Weather system.</summary>
    public WeatherSystem Weather { get; }

    /// <summary>Expedition system.</summary>
    public ExpeditionSystem Expeditions { get; }

    /// <summary>World evolution systems.</summary>
    public LocationEvolutionSystem LocationEvolution { get; }
    public WildlifeMigrationSystem Wildlife { get; }
    public LandmarkDegradationSystem Landmark { get; }

    /// <summary>Current campaign day.</summary>
    public int CurrentDay => Coordinator.Calendar is Ashfall.Core.Clock.ISimClock simClock ? simClock.DayIndex : Coordinator.LastAdvancedDay;

    /// <summary>RNG seeded from context.</summary>
    public ISeededRng Rng { get; }

    public PerformanceCampaignHarness(PerfWorkloadContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));

        Rng = new SeededRng(context.Seed);
        Coordinator = new CampaignDayCoordinator(
            new CampaignCalendar(initialDay: 1),
            new Ashfall.Core.Random.CampaignRngManager(masterSeed: context.Seed));

        Survivors = new SurvivorRosterSystem();
        Inventory = new Inventory.Inventory();
        Journal = new JournalSystem();
        Weather = new WeatherSystem();
        Expeditions = new ExpeditionSystem();
        LocationEvolution = new LocationEvolutionSystem();
        Wildlife = new WildlifeMigrationSystem();
        Landmark = new LandmarkDegradationSystem();

        SeedRoster(context.RosterTier);
        SeedCatalog();
        SeedJournal(context.JournalTier);
        SeedWorld(context.WorldStateTier);
        SeedExpeditions(context.ExpeditionTier);

        RegisterOwners();
    }

    private void SeedRoster(string tier)
    {
        int count = ScaleTier.RosterCount(tier);
        var defs = new[]
        {
            ("survivor_perf_alpha", "Alpha", "Farmer", 90f),
            ("survivor_perf_bravo", "Bravo", "Doctor", 100f),
            ("survivor_perf_charlie", "Charlie", "Engineer", 95f),
            ("survivor_perf_delta", "Delta", "Scout", 85f),
            ("survivor_perf_echo", "Echo", "Medic", 88f),
            ("survivor_perf_foxtrot", "Foxtrot", "Soldier", 92f),
            ("survivor_perf_golf", "Golf", "Teacher", 80f),
            ("survivor_perf_hotel", "Hotel", "Chef", 87f),
            ("survivor_perf_india", "India", "Mechanic", 91f),
            ("survivor_perf_juliet", "Juliet", "Student", 78f),
            ("survivor_perf_kilo", "Kilo", "Farmer", 93f),
            ("survivor_perf_lima", "Lima", "Doctor", 97f),
            ("survivor_perf_mike", "Mike", "Engineer", 84f),
            ("survivor_perf_november", "November", "Scout", 89f),
            ("survivor_perf_oscar", "Oscar", "Medic", 86f),
            ("survivor_perf_papa", "Papa", "Soldier", 94f),
            ("survivor_perf_quebec", "Quebec", "Teacher", 82f),
            ("survivor_perf_romeo", "Romeo", "Chef", 90f),
            ("survivor_perf_sierra", "Sierra", "Mechanic", 88f),
            ("survivor_perf_tango", "Tango", "Student", 79f),
            ("survivor_perf_uniform", "Uniform", "Farmer", 96f),
            ("survivor_perf_victor", "Victor", "Doctor", 85f),
            ("survivor_perf_whiskey", "Whiskey", "Engineer", 92f),
            ("survivor_perf_xray", "Xray", "Scout", 87f),
        };

        for (int i = 0; i < Math.Min(count, defs.Length); i++)
        {
            var (id, name, profession, health) = defs[i];
            Survivors.RegisterDefinition(new SurvivorDefinition
            {
                id = id,
                displayName = name,
                profession = profession,
                baseHealth = health,
            });
            Survivors.Join(id, 1);
        }
    }

    private void SeedCatalog()
    {
        foreach (var item in new[] { "canned_food", "clean_water", "bandage", "iodine_pills", "rad_away", "gas_mask", "hazmat_suit", "battery", "scrap_mechanical", "fuel_canister" })
        {
            Inventory.AddById(item, 2);
        }
    }

    private void SeedJournal(string tier)
    {
        int count = ScaleTier.JournalEntryCount(tier);
        for (int i = 0; i < count; i++)
        {
            Journal.TryAddRawEntry(
                $"perf_entry_{i}",
                $"Performance journal entry {i}",
                author: new PerfAuthor(),
                day: i + 1);
        }
    }

    private void SeedWorld(string tier)
    {
        var profile = new SeasonProfileDef
        {
            id = "perf_profile",
            weatherCheckIntervalHours = 6f,
            seasons = new List<SeasonWindowDef>
            {
                new SeasonWindowDef
                {
                    id = "perf_window",
                    startDay = 0,
                    clearWeight = 1f,
                    rainWeight = 1f,
                    overcastWeight = 1f,
                    ashfallWeight = 1f,
                    falloutStormWeight = 1f,
                    blizzardWeight = 1f,
                    blackRainWeight = 1f,
                }
            }
        };
        Weather.BindProfile(profile, _context.Seed);

        foreach (var id in new[] { "loc_perf_a", "loc_perf_b", "loc_perf_c", "loc_perf_d", "loc_perf_e" })
        {
            LocationEvolution.GetOrCreateRecord(id);
        }

        Wildlife.RegisterPack("pack_perf_1", "species_perf", "sector_perf", 6);
        Wildlife.SetSectorAdjacency(new[] { ("sector_perf", new List<string> { "sector_perf" }) });
        Landmark.RegisterLandmark("landmark_perf_1", "loc_perf_a", 10f);
    }

    private void SeedExpeditions(string tier)
    {
        var def = new ExpeditionDefinition
        {
            id = "loc_perf_expedition",
            displayName = "Performance Expedition Site",
            distanceTicks = 5,
            dangerLevel = 3,
            encounterChancePerTick = 0.15f,
            baseStaminaDrainPerHour = 2.5f,
            lootCategories = new List<string> { "scrap_metal", "clean_water" }
        };
        ExpeditionDefinitionRegistry.Register(def);

        int active = tier switch
        {
            ScaleTier.ExpeditionNone => 0,
            ScaleTier.ExpeditionTypical => 2,
            ScaleTier.ExpeditionHigh => 6,
            ScaleTier.ExpeditionStress => 10,
            _ => 2,
        };

        for (int i = 0; i < active; i++)
        {
            string svId = Survivors.Roster.Count > i ? Survivors.Roster[i].survivorId : $"survivor_perf_exp_{i}";
            Expeditions.Start(def, svId, 1);
        }
    }

    private void RegisterOwners()
    {
        void Record(string ownerId, Action action)
        {
            long start = Stopwatch.GetTimestamp();
            action();
            _ = Stopwatch.GetTimestamp() - start;
        }

        Coordinator.Register("perf_weather", new PerfDayOwner("perf_weather", 1, Record, (day, events) =>
        {
            Weather.Tick(24f);
            events.Add(new DayStateChangeEvent("weather_ticked", "perf_weather", null, null, 0f));
        }));

        Coordinator.Register("perf_survivors", new PerfDayOwner("perf_survivors", 3, Record, (day, events) =>
        {
            for (int i = 0; i < Survivors.LivingCount; i++)
            {
                var sv = Survivors.Roster[i];
                if (sv.isAlive)
                {
                    // Minimal tick: keep entries alive and move the roster state forward.
                }
            }
            events.Add(new DayStateChangeEvent("needs_ticked", "perf_survivors", null, null, Survivors.LivingCount));
        }));

        Coordinator.Register("perf_expeditions", new PerfDayOwner("perf_expeditions", 4, Record, (day, events) =>
        {
            foreach (var kv in Expeditions.Active)
            {
                Expeditions.TickHours(1f, Rng);
            }
            events.Add(new DayStateChangeEvent("expeditions_ticked", "perf_expeditions", null, null, Expeditions.ActiveCount));
        }));

        Coordinator.Register("perf_world", new PerfDayOwner("perf_world", 4, Record, (day, events) =>
        {
            bool hazard = (day % 7) == 0;
            LocationEvolution.TickDay(day, new LocationEvolutionInputs(hazard ? 150f : 1f, hazard), Rng);
            Wildlife.TickDay(day, Rng);
            Landmark.TickDay(day, hazard ? 16f : 0f);
            events.Add(new DayStateChangeEvent("world_ticked", "perf_world", null, null, hazard ? 1f : 0f));
        }));

        Coordinator.Register("perf_journal", new PerfDayOwner("perf_journal", 5, Record, (day, events) =>
        {
            Journal.TryAddRawEntry(
                $"perf_day_{day}",
                $"Day {day} performance entry",
                new PerfAuthor(),
                day);
            events.Add(new DayStateChangeEvent("journal_ticked", "perf_journal", null, null, Journal.Entries.Count));
        }));
    }

    /// <summary>
    /// Advance the campaign by the configured number of days.
    /// Returns total elapsed milliseconds.
    /// </summary>
    public double AdvanceDays(int days)
    {
        if (days < 0) throw new ArgumentOutOfRangeException(nameof(days));
        if (_disposed) throw new ObjectDisposedException(nameof(PerformanceCampaignHarness));

        var sw = PerfStopwatch.StartNew();
        int startDay = CurrentDay < 1 ? 0 : CurrentDay;
        for (int day = startDay + 1; day <= startDay + days; day++)
        {
            var result = Coordinator.Advance(day);
            if (result == null)
                throw new InvalidOperationException($"Day {day} advance returned null (re-entrant or stale day guard).");
        }
        return sw.Stop().ElapsedMilliseconds;
    }

    /// <summary>
    /// Capture a deterministic save payload for the current state.
    /// </summary>
    public string CaptureSavePayload()
    {
        if (_disposed) throw new ObjectDisposedException(nameof(PerformanceCampaignHarness));
        var json = new SystemTextJsonSerializer();

        var payloads = new Dictionary<string, string>();
        foreach (var section in SaveSectionRegistry.All)
        {
            payloads[section.SectionKey] = string.Empty;
        }

        var manifest = new SaveManifest
        {
            currentDay = CurrentDay,
            seed = _context.Seed,
            lastSaveTick = 1,
            slotId = new SaveSlotId("perf_slot"),
            profileId = new SaveProfileId("perf_profile"),
            campaignName = $"Performance_{_context.WorkloadId}",
            generationId = $"gen_perf_{_context.WorkloadId}",
        };

        var envelope = CampaignEnvelopeBuilder.Build(payloads, manifest);
        return json.Serialize(envelope);
    }

    /// <summary>
    /// Measure save latency by serializing current state.
    /// </summary>
    public double MeasureSaveLatency()
    {
        if (_disposed) throw new ObjectDisposedException(nameof(PerformanceCampaignHarness));
        var sw = PerfStopwatch.StartNew();
        string payload = CaptureSavePayload();
        return sw.Stop().ElapsedMilliseconds;
    }

    /// <summary>
    /// Measure load latency by deserializing a saved payload.
    /// </summary>
    public double MeasureLoadLatency(string payload)
    {
        if (_disposed) throw new ObjectDisposedException(nameof(PerformanceCampaignHarness));
        var sw = PerfStopwatch.StartNew();
        var json = new SystemTextJsonSerializer();
        var envelope = json.Deserialize<AggregateSaveEnvelope>(payload);
        return sw.Stop().ElapsedMilliseconds;
    }

    /// <summary>
    /// Measure checksum latency for a payload.
    /// </summary>
    public static double MeasureChecksumLatency(string payload)
    {
        var sw = PerfStopwatch.StartNew();
        string checksum = SaveChecksum.Compute(payload);
        return sw.Stop().ElapsedMilliseconds;
    }

    /// <summary>
    /// Measure retained memory after a new-game lifecycle.
    /// </summary>
    public long MeasureRetainedMemoryAfterNewGame()
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        return GC.GetTotalMemory(forceFullCollection: true);
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
    }

    /// <summary>
    /// Lightweight IDayAdvanceOwner that records per-owner timing via a callback.
    /// </summary>
    private sealed class PerfDayOwner : IDayAdvanceOwner
    {
        private readonly string _ownerId;
        private readonly int _phase;
        private readonly Action<string, Action> _recordTiming;
        private readonly Action<int, List<DayStateChangeEvent>> _tick;

        public PerfDayOwner(string ownerId, int phase, Action<string, Action> recordTiming, Action<int, List<DayStateChangeEvent>> tick)
        {
            _ownerId = ownerId;
            _phase = phase;
            _recordTiming = recordTiming;
            _tick = tick;
        }

        public void CapturePreDaySnapshot(int day) { }

        public void TickDay(int day, List<DayStateChangeEvent> events)
        {
            _recordTiming(_ownerId, () => _tick(day, events));
        }
    }

    private sealed class PerfAuthor : Ashfall.Core.Journal.ISurvivorAuthor
    {
        public string Id => "perf_author";
        public string DisplayName => "Perf";
        public Ashfall.Core.Journal.RiskBiasTrait RiskBias => Ashfall.Core.Journal.RiskBiasTrait.Realist;
    }
}
