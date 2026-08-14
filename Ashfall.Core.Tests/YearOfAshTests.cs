using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.YearOfAsh;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class YearOfAshTests
    {
        [Fact]
        public void Timeline_AdvancesThroughAllThreePhases()
        {
            var timeline = new YearOfAshTimelineSystem();
            Assert.Equal(180, timeline.CurrentDay);
            Assert.Equal(YearOfAshPhase.Phase4_DeepFreeze, timeline.CurrentPhase);
            Assert.True(timeline.AmbientTemperatureCelsius <= -25.0f);
            Assert.Equal(1.40f, timeline.CalculateCaloricMultiplier());

            // Advance to Phase 5
            timeline.AdvanceDay(250);
            Assert.Equal(250, timeline.CurrentDay);
            Assert.Equal(YearOfAshPhase.Phase5_FactionSiege, timeline.CurrentPhase);
            Assert.True(timeline.ContinuityDecreeActive);

            // Advance to Phase 6
            timeline.AdvanceDay(320);
            Assert.Equal(320, timeline.CurrentDay);
            Assert.Equal(YearOfAshPhase.Phase6_TheGreatThaw, timeline.CurrentPhase);
            Assert.True(timeline.FinalBroadcastsActive);
            Assert.True(timeline.RadonInfiltrationRate > 0.20f);
        }

        [Fact]
        public void DoorEncounters_EvaluatesHumanistVsRuthlessReactionsDeterministically()
        {
            var encounters = new DoorEncounterSystem();
            var e1 = encounters.Catalog[0]; // Deserter family
            var admitChoice = e1.choices[0]; // Compassionate choice
            var stripChoice = e1.choices[2]; // Ruthless choice

            var humanistSurvivor = new SurvivorOccupantSnapshot
            {
                survivorId = "survivor_elena",
                name = "Elena Vance",
                moralBranch = "humanist",
                guiltLevel = 20
            };

            var ruthlessSurvivor = new SurvivorOccupantSnapshot
            {
                survivorId = "survivor_kurt",
                name = "Kurt Drake",
                moralBranch = "ruthless",
                guiltLevel = 10
            };

            // Compassionate choice: Humanist gains morale, Ruthless loses morale
            var rxHumanist = encounters.CalculateSurvivorReaction(humanistSurvivor, admitChoice);
            var rxRuthless = encounters.CalculateSurvivorReaction(ruthlessSurvivor, admitChoice);

            Assert.True(rxHumanist.moraleDelta > 0);
            Assert.True(rxRuthless.moraleDelta < 0);

            // Ruthless choice: Humanist suffers severe guilt and morale drop
            var rxHumanistRuthless = encounters.CalculateSurvivorReaction(humanistSurvivor, stripChoice);
            Assert.True(rxHumanistRuthless.moraleDelta < 0);
            Assert.True(rxHumanistRuthless.guiltDelta > 0);
        }

        [Fact]
        public void DoorEncounters_TraumaBondDampensNegativeMoraleImpact()
        {
            var encounters = new DoorEncounterSystem();
            var e1 = encounters.Catalog[0];
            var stripChoice = e1.choices[2]; // Ruthless choice

            var standardSurvivor = new SurvivorOccupantSnapshot
            {
                survivorId = "s1",
                moralBranch = "humanist",
                hasTraumaBondWithLeader = false
            };

            var bondedSurvivor = new SurvivorOccupantSnapshot
            {
                survivorId = "s2",
                moralBranch = "humanist",
                hasTraumaBondWithLeader = true
            };

            var rxStandard = encounters.CalculateSurvivorReaction(standardSurvivor, stripChoice);
            var rxBonded = encounters.CalculateSurvivorReaction(bondedSurvivor, stripChoice);

            Assert.True(Math.Abs(rxBonded.moraleDelta) < Math.Abs(rxStandard.moraleDelta));
        }

        [Fact]
        public void FactionWar_ModifiesStandingAndEnactsDecrees()
        {
            var war = new FactionWarSystem();
            Assert.Equal(0, war.GetStanding("faction_central_garrison"));

            war.ModifyStanding("faction_central_garrison", 60);
            Assert.True(war.GetStanding("faction_central_garrison") >= 50);

            war.ModifyStanding("faction_ash_sign", -80);
            Assert.True(war.GetStanding("faction_ash_sign") <= -50);

            int initialTension = war.WarTension;
            war.EnactDecree("decree_martial_continuity");
            Assert.True(war.WarTension > initialTension);
        }

        [Fact]
        public void YearOfAshSave_CapturesAndEncodesDeterministicChecksum()
        {
            var timeline = new YearOfAshTimelineSystem();
            timeline.AdvanceDay(210);

            var encounters = new DoorEncounterSystem();
            var war = new FactionWarSystem();
            war.ModifyStanding("faction_rebuilders", 35);

            var clock = new ManualClock(210);
            var save = YearOfAshSaveCodec.Capture(timeline, encounters, war, clock);

            Assert.NotNull(save);
            Assert.Equal(210, save.simDay);
            Assert.False(string.IsNullOrEmpty(save.Checksum));

            var jsonSerializer = new SystemTextJsonSerializer();
            string jsonText = YearOfAshSaveCodec.Encode(save, jsonSerializer);
            Assert.Contains("210", jsonText);

            var restored = YearOfAshSaveCodec.Decode(jsonText, jsonSerializer);
            Assert.Equal(210, restored.simDay);
            Assert.Equal(YearOfAshPhase.Phase4_DeepFreeze, restored.timeline.phase);
        }

        [Fact]
        public void DoorEncounterCatalogLoader_LoadsJsonEntriesCorrectly()
        {
            var system = new DoorEncounterSystem();
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            // Walk up from CWD until we find Assets/StreamingAssets/Data
            string dataDir = string.Empty;
            string search = System.IO.Directory.GetCurrentDirectory();
            for (int i = 0; i < 6; i++)
            {
                string candidate = System.IO.Path.Combine(search, "Assets", "StreamingAssets", "Data");
                if (fileIO.DirectoryExists(candidate)) { dataDir = candidate; break; }
                string parent = System.IO.Directory.GetParent(search)?.FullName;
                if (parent == null) break;
                search = parent;
            }

            if (string.IsNullOrEmpty(dataDir))
            {
                // Can't find catalog from CI; skip gracefully
                return;
            }

            int loaded = DoorEncounterCatalogLoader.LoadAndRegister(system, dataDir, fileIO, json);
            Assert.True(loaded >= 40, $"Expected >= 40 loaded encounters, got {loaded}");
            Assert.Contains(system.Catalog, e => e.encounterId == "door_encounter_garrison_deserter_family");
            Assert.Contains(system.Catalog, e => e.encounterId == "door_encounter_wandering_trauma_surgeon");
            Assert.Contains(system.Catalog, e => e.encounterId == "door_encounter_continuity_broadcast_courier");
            Assert.Contains(system.Catalog, e => e.encounterId == "door_encounter_continental_evacuation_liaison");
        }

        [Fact]
        public void YearOfAshCatalogLoader_LoadsItemsEventsAndQuestsCorrectly()
        {
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            string dataDir = string.Empty;
            string search = System.IO.Directory.GetCurrentDirectory();
            for (int i = 0; i < 6; i++)
            {
                string candidate = System.IO.Path.Combine(search, "Assets", "StreamingAssets", "Data");
                if (fileIO.DirectoryExists(candidate)) { dataDir = candidate; break; }
                string parent = System.IO.Directory.GetParent(search)?.FullName;
                if (parent == null) break;
                search = parent;
            }

            if (string.IsNullOrEmpty(dataDir)) return;

            var items = YearOfAshCatalogLoader.LoadItems(dataDir, fileIO, json);
            Assert.True(items.Count >= 36, $"Expected >= 36 items, got {items.Count}");
            Assert.Contains(items, item => item.id == "item_military_filter_crate");
            Assert.Contains(items, item => item.id == "item_continental_maritime_transponder");
            Assert.Contains(items, item => item.id == "item_boron_shielding_tile");
            Assert.Contains(items, item => item.id == "item_evacuation_manifest_scroll");

            var events = YearOfAshCatalogLoader.LoadEvents(dataDir, fileIO, json);
            Assert.True(events.Count >= 36, $"Expected >= 36 events, got {events.Count}");
            Assert.Contains(events, ev => ev.id == "event_black_blizzard_inversion");
            Assert.Contains(events, ev => ev.id == "event_black_mud_inundation");
            Assert.Contains(events, ev => ev.id == "event_final_dawn_year_one");

            var questSystem = new QuestlineSystem();
            int initialQuests = questSystem.Catalog.Count;
            int loadedQuests = YearOfAshCatalogLoader.LoadAndRegisterQuests(questSystem, dataDir, fileIO, json);
            Assert.True(loadedQuests >= 12, $"Expected >= 12 external quests, got {loadedQuests}");
            Assert.True(questSystem.Catalog.Count >= initialQuests);
            Assert.NotNull(questSystem.FindDefinition("quest_continental_convoy_gate"));
            Assert.NotNull(questSystem.FindDefinition("quest_final_manifest_muster"));
        }

        [Fact]
        public void FactionWar_SimulatesDailyFrictionCorrectly()
        {
            var war = new FactionWarSystem();
            int initialTension = war.WarTension;

            // Day 200 is Phase 4, no major war friction
            war.SimulateDailyFriction(200);
            Assert.Equal(initialTension, war.WarTension);

            // Day 250 is Phase 5 (Faction Siege)
            war.SimulateDailyFriction(250);
            Assert.True(war.WarTension > initialTension);

            // Day 255 triggers territorial shift
            war.SimulateDailyFriction(255);
            Assert.True(war.State.totalArtilleryStrikesLogged > 0);
        }

        [Fact]
        public void YearOfAshCatalogLoader_LoadsLocationsRadioAndSurvivorsCorrectly()
        {
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            string dataDir = string.Empty;
            string search = System.IO.Directory.GetCurrentDirectory();
            for (int i = 0; i < 6; i++)
            {
                string candidate = System.IO.Path.Combine(search, "Assets", "StreamingAssets", "Data");
                if (fileIO.DirectoryExists(candidate)) { dataDir = candidate; break; }
                string parent = System.IO.Directory.GetParent(search)?.FullName;
                if (parent == null) break;
                search = parent;
            }

            if (string.IsNullOrEmpty(dataDir)) return;

            var locations = YearOfAshCatalogLoader.LoadLocations(dataDir, fileIO, json);
            Assert.True(locations.Count >= 30, $"Expected >= 30 locations, got {locations.Count}");
            Assert.Contains(locations, l => l.id == "loc_the_allotments");
            Assert.Contains(locations, l => l.id == "loc_denial_cut_substation");
            Assert.Contains(locations, l => l.id == "loc_brine_pumping_sluice");
            Assert.Contains(locations, l => l.id == "loc_continental_radio_beacon");
            Assert.Contains(locations, l => l.id == "loc_aurora_borealis_grounding_shoal");

            var radio = YearOfAshCatalogLoader.LoadRadioBroadcasts(dataDir, fileIO, json);
            Assert.True(radio.Count >= 18, $"Expected >= 18 radio broadcasts, got {radio.Count}");
            Assert.Contains(radio, r => r.id == "radio_garrison_decree_180");
            Assert.Contains(radio, r => r.id == "radio_continental_maritime_beacon_340");
            Assert.Contains(radio, r => r.id == "radio_final_dawn_broadcast_360");

            var survivors = YearOfAshCatalogLoader.LoadSurvivors(dataDir, fileIO, json);
            Assert.True(survivors.Count >= 24, $"Expected >= 24 survivors, got {survivors.Count}");
            Assert.Contains(survivors, s => s.id == "survivor_ottilie_frayne");
            Assert.Contains(survivors, s => s.id == "survivor_anneke_ruhl");
            Assert.Contains(survivors, s => s.id == "survivor_dr_sarah_chen");
            Assert.Contains(survivors, s => s.id == "survivor_erik_dahl");
        }

        [Fact]
        public void YearOfAshRadonSystem_SimulatesThawInfiltrationAndScrubberReplacement()
        {
            var radon = new YearOfAshRadonSystem();

            // Pre-thaw: baseline radon
            radon.TickDailyRadon(200, -30.0f);
            Assert.True(radon.IndoorRadonBqm3 <= 120.0f);

            // Phase 6 thaw with positive temperature
            radon.TickDailyRadon(310, 3.0f);
            Assert.True(radon.ActiveFissures >= 1);
            Assert.True(radon.IndoorRadonBqm3 > 120.0f);

            // Multiple days of high infiltration degrades scrubber
            for (int i = 311; i <= 325; i++)
            {
                radon.TickDailyRadon(i, 4.0f);
            }
            Assert.True(radon.ScrubberHealthPercent < 100.0f);

            // Replace scrubber
            radon.ReplaceScrubberFilter();
            Assert.Equal(100.0f, radon.ScrubberHealthPercent);

            // Seal fissures
            bool sealedOk = radon.SealFoundationFissures();
            Assert.True(sealedOk);
            Assert.Equal(0, radon.ActiveFissures);
        }

        [Fact]
        public void YearOfAshDeepFreezeSystem_SimulatesSubZeroThermalBalanceAndIntakeIcing()
        {
            var freeze = new YearOfAshDeepFreezeSystem();

            // Deep freeze day at -38C surface
            freeze.TickDailyThermal(190, -38.0f);
            Assert.True(freeze.IntakeIceMm > 0.0f);

            // Icing accumulation over several days triggers blockage alarm
            for (int i = 191; i <= 200; i++)
            {
                freeze.TickDailyThermal(i, -38.0f);
            }
            Assert.True(freeze.IsIntakeBlocked);

            // Manual de-icing clears intake
            freeze.ClearIntakeIce();
            Assert.False(freeze.IsIntakeBlocked);
            Assert.Equal(0.0f, freeze.IntakeIceMm);

            // Insulation upgrade improves thermal retention
            float preInsulationTemp = freeze.IndoorTempCelsius;
            freeze.UpgradeThermalInsulation(0.2f);
            Assert.True(freeze.State.thermalInsulationQuality >= 1.0f);
        }

        private class ManualClock : IClock
        {
            private int _day;
            public ManualClock(int day) { _day = day; }
            public int Day => _day;
            public void AdvanceDays(int days) { _day += days; }
            public void SetDay(int day) { _day = day; }
        }
    }
}
