// SPDX-License-Identifier: MIT
// Plan 203 — perimeter defense extension: sector topology, false alarms,
// weather wear, alert-device reset lifecycle, attacker counterplay,
// encounter snapshot, bounded intrusion log, save safety, determinism.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Defense;
using Ashfall.Core.Inventory;
using Xunit;

namespace Ashfall.Core.Tests.Defense
{
    public class PerimeterDefensePlan203Tests
    {
        private static List<PerimeterDefenseDefinition> CreateDefs() => new List<PerimeterDefenseDefinition>
        {
            new PerimeterDefenseDefinition
            {
                defense_id = "def_razorwire", display_name = "Razorwire", defense_type = "entanglement",
                max_hp = 100, slow_factor = 0.6f, base_damage = 5f,
                counter_tags = new List<string> { "cutting_tools" },
                weather_wear_per_storm = 12f
            },
            new PerimeterDefenseDefinition
            {
                defense_id = "def_tripwire", display_name = "Flare Tripwire", defense_type = "early_warning",
                max_hp = 50, prevents_stealth_breach = true, night_accuracy_bonus = 0.3f,
                counter_tags = new List<string> { "stealth" },
                weather_wear_per_storm = 25f, false_alarm_rate_bp = 300, alert_device = true
            },
            new PerimeterDefenseDefinition
            {
                defense_id = "def_turret", display_name = "Sentry Turret", defense_type = "automated_turret",
                max_hp = 300, base_damage = 15f, fire_rate_burst = 3,
                required_ammo_type = "ammo_9x19", magazine_capacity = 30,
                counter_tags = new List<string> { "emp" },
                weather_wear_per_storm = 4f
            }
        };

        private static (PerimeterDefenseSystem sys, Inventory.Inventory inv) MakeSystem(int seed)
        {
            var inv = new Inventory.Inventory();
            inv.AddById("scrap_metal", 100);
            inv.AddById("ammo_9x19", 100);
            inv.AddById("sandbags", 50);
            inv.AddById("flare_tripwire", 50);
            var sys = new PerimeterDefenseSystem(CreateDefs(), inv, new SeededRng(seed));
            return (sys, inv);
        }

        // ── Sector topology ─────────────────────────────────────────────

        [Fact]
        public void Construct_AutoAssignsEmplacement_ToCanonicalSector()
        {
            var (sys, _) = MakeSystem(42);
            Assert.True(sys.ConstructEmplacement("def_razorwire").IsSuccess);
            Assert.True(sys.ConstructEmplacement("def_tripwire").IsSuccess);

            var sectors = sys.Sectors;
            Assert.NotEmpty(sectors);
            int assigned = sectors.Sum(s => s.emplacement_ids.Count);
            Assert.Equal(2, assigned);
            Assert.All(sectors, s => Assert.True(PerimeterSector.IsValid(s.sector_id)));
        }

        [Fact]
        public void AssignEmplacement_RejectsUnknownSector_AndMovesBetweenSectors()
        {
            var (sys, _) = MakeSystem(42);
            Assert.True(sys.ConstructEmplacement("def_razorwire").IsSuccess);
            string empId = sys.Emplacements[0].emplacement_id;

            Assert.False(sys.AssignEmplacementToSector(empId, "atlantis").IsSuccess);
            Assert.True(sys.AssignEmplacementToSector(empId, PerimeterSector.Gate).IsSuccess);

            var gate = sys.FindSector(PerimeterSector.Gate);
            Assert.NotNull(gate);
            Assert.Contains(empId, gate!.emplacement_ids);
            // Moved, not copied.
            Assert.Equal(1, sys.Sectors.Sum(s => s.emplacement_ids.Count));
        }

        // ── Weather wear ────────────────────────────────────────────────

        [Fact]
        public void SevereWeather_WearsIntactEmplacements_ButNotDestroyed()
        {
            var (sys, _) = MakeSystem(42);
            Assert.True(sys.ConstructEmplacement("def_razorwire").IsSuccess); // wear 12, hp 100
            var emp = sys.Emplacements[0];
            emp.current_hp = 30;

            // An already-destroyed emplacement must not magically regain HP.
            sys.ConstructEmplacement("def_turret");
            sys.Emplacements[1].is_destroyed = true;
            sys.Emplacements[1].current_hp = 0;

            sys.TickDay(10, severeWeather: true);

            Assert.Equal(18, emp.current_hp); // 30 - 12
            Assert.True(sys.Emplacements[1].is_destroyed);
            Assert.Equal(0, sys.Emplacements[1].current_hp);
        }

        [Fact]
        public void ClearWeather_CausesNoWear()
        {
            var (sys, _) = MakeSystem(42);
            Assert.True(sys.ConstructEmplacement("def_razorwire").IsSuccess);
            int before = sys.Emplacements[0].current_hp;
            sys.TickDay(1, severeWeather: false);
            Assert.Equal(before, sys.Emplacements[0].current_hp);
        }

        // ── Alert devices: spend, reset, false alarms ───────────────────

        [Fact]
        public void FalseAlarm_IsDeterministic_SpendsDevice_AndResetRestores()
        {
            // Find a seed that produces a false alarm within 40 days at 300bp/day.
            int seed = -1;
            for (int s = 1; s <= 64 && seed < 0; s++)
            {
                var (probe, _) = MakeSystem(s);
                Assert.True(probe.ConstructEmplacement("def_tripwire").IsSuccess);
                for (int d = 1; d <= 40; d++) probe.TickDay(d, severeWeather: false);
                if (probe.Sectors.Any(sec => sec.alarm_spent)) seed = s;
            }
            Assert.True(seed > 0, "No deterministic false alarm found in seeds 1..64 (rate too low?)");

            var (sys, _) = MakeSystem(seed);
            Assert.True(sys.ConstructEmplacement("def_tripwire").IsSuccess);
            for (int d = 1; d <= 40; d++) sys.TickDay(d, severeWeather: false);

            var sector = sys.Sectors.First(s => s.alarm_spent || s.false_alarm_count > 0);
            Assert.True(sector.alarm_spent);
            Assert.True(sector.false_alarm_count >= 1);
            Assert.Contains(sys.IntrusionLog, e => e.kind == "false_alarm");

            // Stealth denial is suppressed while spent.
            var snap = sys.GetEncounterSnapshot();
            Assert.False(snap.stealth_denied);

            // Reset restores the device.
            Assert.True(sys.ResetSectorAlarm(sector.sector_id).IsSuccess);
            Assert.False(sector.alarm_spent);
            Assert.True(sys.GetEncounterSnapshot().stealth_denied);

            // Reset again is blocked (nothing spent).
            Assert.False(sys.ResetSectorAlarm(sector.sector_id).IsSuccess);
        }

        [Fact]
        public void HostileApproach_TriggersAndSpendsSectorDevice()
        {
            var (sys, _) = MakeSystem(42);
            Assert.True(sys.ConstructEmplacement("def_tripwire").IsSuccess);
            string sectorId = sys.Sectors[0].sector_id;

            var res = sys.SimulateRaiderAssault(1, isNight: false, currentDay: 5);

            var sector = sys.FindSector(sectorId)!;
            Assert.True(sector.alarm_spent);
            Assert.Equal(5, sector.last_trigger_day);
            Assert.Contains(sys.IntrusionLog, e => e.kind == "hostile_trigger");
        }

        [Fact]
        public void Disarm_SuppressesTriggers_AndRearmingRestores()
        {
            var (sys, _) = MakeSystem(42);
            Assert.True(sys.ConstructEmplacement("def_tripwire").IsSuccess);
            string sectorId = sys.Sectors[0].sector_id;

            Assert.True(sys.DisarmSector(sectorId).IsSuccess);
            sys.SimulateRaiderAssault(1, currentDay: 2);
            var sector = sys.FindSector(sectorId)!;
            Assert.False(sector.alarm_spent); // disarmed devices don't trigger

            Assert.True(sys.DisarmSector(sectorId).IsSuccess); // re-arm
            Assert.True(sector.alarm_armed);
        }

        // ── Counterplay ─────────────────────────────────────────────────

        [Fact]
        public void AttackerCounters_NeutralizeMatchingDefenses()
        {
            // Turret vs emp attackers: the countered turret cannot engage, so fewer
            // raiders fall. Non-turret defenses express counters via the snapshot.
            var (sys, inv) = MakeSystem(42);
            Assert.True(sys.ConstructEmplacement("def_turret").IsSuccess);
            Assert.True(sys.LoadAmmo(sys.Emplacements[0].emplacement_id, 30).IsSuccess);

            var uncountered = sys.SimulateRaiderAssault(10, attackerCounterTags: new[] { "stealth" });
            int killsUncountered = uncountered.AttackersKilled;
            int ammoUncountered = sys.Emplacements[0].loaded_ammo_count;

            // Reload for the countered run.
            sys.Emplacements[0].loaded_ammo_count = 30;
            var countered = sys.SimulateRaiderAssault(10, attackerCounterTags: new[] { "emp" });

            Assert.True(killsUncountered > 0, "Uncountered turret should inflict casualties");
            Assert.Equal(0, countered.AttackersKilled);
            Assert.Equal(30, sys.Emplacements[0].loaded_ammo_count); // countered turret never fired

            var snap = sys.GetEncounterSnapshot(new[] { "emp" });
            Assert.Contains(sys.Emplacements[0].emplacement_id, snap.countered_emplacement_ids);
        }

        [Fact]
        public void StealthDenial_RespectsAttackerStealthCounter()
        {
            var (sys, _) = MakeSystem(42);
            Assert.True(sys.ConstructEmplacement("def_tripwire").IsSuccess);
            string sectorId = sys.Sectors[0].sector_id;

            // Countered stealth raiders slip past the tripwire — and the device
            // does not even trigger (stealth counters both detection and spend).
            var countered = sys.SimulateRaiderAssault(1, attackerCounterTags: new[] { "stealth" });
            Assert.False(countered.StealthInfiltrationNeutralized);
            Assert.False(sys.FindSector(sectorId)!.alarm_spent);

            // Un-countered raiders are detected and the device spends.
            var detected = sys.SimulateRaiderAssault(1);
            Assert.True(detected.StealthInfiltrationNeutralized);
            Assert.True(sys.FindSector(sectorId)!.alarm_spent);
        }

        // ── Encounter snapshot ──────────────────────────────────────────

        [Fact]
        public void EncounterSnapshot_ReportsDelayAndInitiative()
        {
            var (sys, _) = MakeSystem(42);
            Assert.True(sys.ConstructEmplacement("def_razorwire").IsSuccess);  // slow 0.6
            Assert.True(sys.ConstructEmplacement("def_tripwire").IsSuccess);   // early warning
            Assert.True(sys.ConstructEmplacement("def_turret").IsSuccess);

            var snap = sys.GetEncounterSnapshot();
            Assert.Equal(1.6f, snap.movement_delay_multiplier, 2);
            Assert.True(snap.detection_initiative_bonus > 0f);
            Assert.True(snap.stealth_denied);
            Assert.NotEmpty(snap.protected_sectors);
        }

        // ── Intrusion log ───────────────────────────────────────────────

        [Fact]
        public void IntrusionLog_IsBounded()
        {
            var (sys, _) = MakeSystem(42);
            Assert.True(sys.ConstructEmplacement("def_razorwire").IsSuccess);
            for (int d = 1; d <= 60; d++)
                sys.SimulateRaiderAssault(1, currentDay: d);

            Assert.True(sys.IntrusionLog.Count <= PerimeterDefenseSystem.IntrusionLogCapacity,
                $"Log count {sys.IntrusionLog.Count} exceeds capacity");
        }

        // ── Save safety ─────────────────────────────────────────────────

        [Fact]
        public void OldSaveBaseline_NoSectorsNoLog_NoAlarmBehavior()
        {
            string legacyJson = "{\"systemId\":\"perimeter_defense\",\"schema_version\":1,\"last_tick_day\":3,"
                + "\"emplacements\":[{\"emplacement_id\":\"emp_old\",\"defense_id\":\"def_tripwire\","
                + "\"current_hp\":50,\"max_hp\":50,\"is_active\":true,\"is_destroyed\":false,"
                + "\"loaded_ammo_count\":0,\"required_ammo_type\":\"\",\"magazine_capacity\":0,"
                + "\"barrel_wear_percent\":0,\"is_jammed\":false}]}";

            var restored = System.Text.Json.JsonSerializer.Deserialize<PerimeterDefenseSave>(legacyJson);
            Assert.NotNull(restored);
            Assert.Empty(restored!.sectors);
            Assert.Empty(restored.intrusion_log);

            var sys = new PerimeterDefenseSystem(CreateDefs(), new Inventory.Inventory(), new SeededRng(42));
            sys.RestoreState(restored);

            // Legacy save: no sector topology, no alarm lifecycle — stealth denial
            // falls back to the historical always-on behavior (snapshot has no sector
            // record, so SectorAlarmSpentFor returns false).
            Assert.True(sys.GetEncounterSnapshot().stealth_denied);
            Assert.Empty(sys.Sectors);

            // New construction after restore joins the sector grid normally.
            Assert.True(sys.ConstructEmplacement("def_razorwire").IsSuccess);
            Assert.NotEmpty(sys.Sectors);
        }

        [Fact]
        public void SaveRoundTrip_PreservesSectorsAlarmsAndLog()
        {
            var (sys, _) = MakeSystem(42);
            Assert.True(sys.ConstructEmplacement("def_razorwire").IsSuccess);
            Assert.True(sys.ConstructEmplacement("def_tripwire").IsSuccess);
            sys.SimulateRaiderAssault(1, currentDay: 4);

            var json = System.Text.Json.JsonSerializer.Serialize(sys.CaptureState());
            var restored = System.Text.Json.JsonSerializer.Deserialize<PerimeterDefenseSave>(json)!;
            Assert.Equal(2, restored.schema_version);

            var sys2 = new PerimeterDefenseSystem(CreateDefs(), new Inventory.Inventory(), new SeededRng(42));
            sys2.RestoreState(restored);

            Assert.Equal(sys.Sectors.Count, sys2.Sectors.Count);
            for (int i = 0; i < sys.Sectors.Count; i++)
            {
                Assert.Equal(sys.Sectors[i].sector_id, sys2.Sectors[i].sector_id);
                Assert.Equal(sys.Sectors[i].alarm_spent, sys2.Sectors[i].alarm_spent);
                Assert.Equal(sys.Sectors[i].false_alarm_count, sys2.Sectors[i].false_alarm_count);
                Assert.Equal(sys.Sectors[i].emplacement_ids.Count, sys2.Sectors[i].emplacement_ids.Count);
            }
            Assert.Equal(sys.IntrusionLog.Count, sys2.IntrusionLog.Count);
        }

        // ── Deterministic replay ────────────────────────────────────────

        [Fact]
        public void DeterministicReplay_SameSeedSameWeather_SameOutcome()
        {
            PerimeterDefenseSave Run(int seed)
            {
                var (sys, _) = MakeSystem(seed);
                Assert.True(sys.ConstructEmplacement("def_razorwire").IsSuccess);
                Assert.True(sys.ConstructEmplacement("def_tripwire").IsSuccess);
                for (int d = 1; d <= 30; d++)
                {
                    sys.TickDay(d, severeWeather: d % 5 == 0);
                    if (d % 7 == 0) sys.SimulateRaiderAssault(2, isNight: d % 2 == 0, currentDay: d);
                }
                return sys.CaptureState();
            }

            var a = Run(31);
            var b = Run(31);
            Assert.Equal(a.emplacements[0].current_hp, b.emplacements[0].current_hp);
            Assert.Equal(a.emplacements[1].current_hp, b.emplacements[1].current_hp);
            Assert.Equal(a.sectors.Count, b.sectors.Count);
            for (int i = 0; i < Math.Min(a.sectors.Count, b.sectors.Count); i++)
            {
                Assert.Equal(a.sectors[i].alarm_spent, b.sectors[i].alarm_spent);
                Assert.Equal(a.sectors[i].false_alarm_count, b.sectors[i].false_alarm_count);
            }
            Assert.Equal(a.intrusion_log.Count, b.intrusion_log.Count);
            for (int i = 0; i < Math.Min(a.intrusion_log.Count, b.intrusion_log.Count); i++)
            {
                Assert.Equal(a.intrusion_log[i].kind, b.intrusion_log[i].kind);
                Assert.Equal(a.intrusion_log[i].day, b.intrusion_log[i].day);
            }
        }

        // ── Data authority gate ─────────────────────────────────────────

        [Fact]
        public void Catalog_CounterTagsAndAlertFields_PresentOnAllDefenses()
        {
            string start = Directory.GetCurrentDirectory();
            Assert.True(CatalogLocator.TryFindDataDirectory(start, out string dataDir));
            string path = System.IO.Path.Combine(dataDir, "perimeter_defenses.json");
            Assert.True(File.Exists(path));

            using var doc = System.Text.Json.JsonDocument.Parse(File.ReadAllText(path));
            Assert.Equal(1, doc.RootElement.GetProperty("schema_version").GetInt32());
            int count = 0;
            foreach (var def in doc.RootElement.GetProperty("defenses").EnumerateArray())
            {
                count++;
                // Additive Plan 203 fields must exist on every defense (explicit zeros allowed).
                Assert.True(def.TryGetProperty("counter_tags", out _), "missing counter_tags");
                Assert.True(def.TryGetProperty("weather_wear_per_storm", out _), "missing weather_wear_per_storm");
                Assert.True(def.TryGetProperty("false_alarm_rate_bp", out _), "missing false_alarm_rate_bp");
                Assert.True(def.TryGetProperty("alert_device", out _), "missing alert_device");
            }
            Assert.Equal(8, count);
        }
    }
}
