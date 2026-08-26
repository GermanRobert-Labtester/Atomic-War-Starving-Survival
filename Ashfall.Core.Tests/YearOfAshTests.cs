using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Verdict;
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
        public void YearOfAshSave_Restore_RebuildsTimelineEncountersAndFactionWar()
        {
            // Build live state worth saving: deep-freeze day, two resolved door
            // encounters, faction standing and a decree.
            var timeline = new YearOfAshTimelineSystem();
            timeline.AdvanceDay(255);

            var encounters = new DoorEncounterSystem();
            var roster = new List<SurvivorOccupantSnapshot>
            {
                new SurvivorOccupantSnapshot { survivorId = "s1", moralBranch = "humanist" }
            };
            var entry = encounters.Catalog[0];
            encounters.ResolveChoice(entry, entry.choices[0], roster);

            var war = new FactionWarSystem();
            war.ModifyStanding("faction_rebuilders", 35);
            war.EnactDecree("decree_martial_continuity");
            war.SimulateDailyFriction(255);

            var save = YearOfAshSaveCodec.Capture(timeline, encounters, war, null);
            var jsonSerializer = new SystemTextJsonSerializer();
            var loaded = YearOfAshSaveCodec.Decode(
                YearOfAshSaveCodec.Encode(save, jsonSerializer), jsonSerializer);

            // Restore into FRESH systems — a new campaign must rebuild, not re-run.
            var timelineB = new YearOfAshTimelineSystem();
            var encountersB = new DoorEncounterSystem();
            var warB = new FactionWarSystem();
            YearOfAshSaveCodec.Restore(loaded, timelineB, encountersB, warB);

            Assert.Equal(255, timelineB.CurrentDay);
            Assert.Equal(YearOfAshPhase.Phase5_FactionSiege, timelineB.CurrentPhase);
            Assert.True(timelineB.ContinuityDecreeActive);

            Assert.Equal(1, encountersB.State.totalEncountersResolved);
            Assert.Contains(entry.encounterId, encountersB.State.resolvedEncounterIds);
            Assert.Equal(encounters.State.cumulativeMoraleDelta, encountersB.State.cumulativeMoraleDelta);

            Assert.Equal(35, warB.GetStanding("faction_rebuilders"));
            Assert.Contains("decree_martial_continuity", warB.State.enactedDecrees);
            Assert.Equal(war.State.totalArtilleryStrikesLogged, warB.State.totalArtilleryStrikesLogged);
            Assert.Equal(war.WarTension, warB.WarTension);

            // The restored faction roster must still contain every default faction
            // (RestoreState merges, never shrinks the known-faction set).
            Assert.Contains(warB.State.factions, f => f.factionId == "faction_central_garrison");
            Assert.Contains(warB.State.factions, f => f.factionId == "faction_black_ops");
        }

        [Fact]
        public void DefaultFactionRoster_IncludesForwardRoster()
        {
            // NARRATIVE_NEEDS.md §5: the Rebuilders splinter introduced in
            // evt_d552_rebuilders_fracture has a real faction_id
            // (faction_forward_roster) and needs a FactionWarSystem roster
            // slot to participate in simulated daily friction, not just live
            // in the event-chain content.
            var war = new FactionWarSystem();
            Assert.Contains(war.State.factions, f => f.factionId == "faction_forward_roster");
            Assert.Equal(0, war.GetStanding("faction_forward_roster"));
        }

        [Fact]
        public void YearOfAshSave_V4_CapturesAndRestoresChainRunnerProgress()
        {
            string start = System.IO.Directory.GetCurrentDirectory();
            string dir;
            if (!CatalogLocator.TryFindDataDirectory(start, out dir))
                CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out dir);

            var loader = new FactionWarContentCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer());
            var catalog = loader.Load(dir);

            var runnerA = new FactionWarChainRunner(catalog);
            runnerA.RecordLocationVisited("loc_grain_silo");
            runnerA.ResolveChoice("evt_d480_grain_tally_dispute", "evt_d480_grain_tally_dispute_s1",
                "evt_d480_grain_tally_dispute_s1_c1", 480);

            var timeline = new YearOfAshTimelineSystem();
            var encounters = new DoorEncounterSystem();
            var war = new FactionWarSystem();

            var save = YearOfAshSaveCodec.Capture(timeline, encounters, war, null, factionWarChainRunner: runnerA);
            Assert.Equal(YearOfAshSave.CurrentSaveVersion, save.saveVersion);

            var jsonSerializer = new SystemTextJsonSerializer();
            string jsonText = YearOfAshSaveCodec.Encode(save, jsonSerializer);
            var loaded = YearOfAshSaveCodec.Decode(jsonText, jsonSerializer);

            var runnerB = new FactionWarChainRunner(catalog);
            var timelineB = new YearOfAshTimelineSystem();
            var encountersB = new DoorEncounterSystem();
            var warB = new FactionWarSystem();
            YearOfAshSaveCodec.Restore(loaded, timelineB, encountersB, warB, factionWarChainRunner: runnerB);

            Assert.True(runnerB.HasVisited("loc_grain_silo"));
            Assert.Equal(2, runnerB.CumulativeMoraleDelta);
            Assert.False(runnerB.IsChainResolved("evt_d480_grain_tally_dispute"));
        }

        [Fact]
        public void YearOfAshSave_V3Envelope_MigratesWithFreshChainRunner()
        {
            // Build a v3 envelope by hand (the pre-chain-runner shape) and
            // confirm it upgrades cleanly instead of failing the checksum
            // check against the v4 shape.
            var v3 = new YearOfAshSaveV3();
            v3.simDay = 250;
            v3.factionWar.factions.Add(new FactionStandingRecord { factionId = "faction_rebuilders", standing = 20 });
            v3.Checksum = SaveChecksum.Compute(v3);

            var jsonSerializer = new SystemTextJsonSerializer();
            string jsonText = jsonSerializer.Serialize(v3);

            var migrated = YearOfAshSaveCodec.Decode(jsonText, jsonSerializer);

            Assert.Equal(YearOfAshSave.CurrentSaveVersion, migrated.saveVersion);
            Assert.Equal(250, migrated.simDay);
            Assert.Contains(migrated.factionWar.factions, f => f.factionId == "faction_rebuilders" && f.standing == 20);
            Assert.NotNull(migrated.factionWarChainRunner);
            Assert.Empty(migrated.factionWarChainRunner.chains);
        }

        [Fact]
        public void RestoreState_WithNullSections_IsANoOp()
        {
            // v1 saves always carry the three sections, but defensive null tolerance
            // must not throw or corrupt fresh systems.
            var timeline = new YearOfAshTimelineSystem();
            var encounters = new DoorEncounterSystem();
            var war = new FactionWarSystem();

            timeline.RestoreState(null);
            encounters.RestoreState(null);
            war.RestoreState(null);

            Assert.Equal(180, timeline.CurrentDay);
            Assert.Equal(0, encounters.State.totalEncountersResolved);
            Assert.Equal(50, war.WarTension);
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
            Assert.True(loaded >= 60, $"Expected >= 60 loaded encounters, got {loaded}");
            Assert.Contains(system.Catalog, e => e.encounterId == "door_encounter_garrison_deserter_family");
            Assert.Contains(system.Catalog, e => e.encounterId == "door_encounter_wandering_trauma_surgeon");
            Assert.Contains(system.Catalog, e => e.encounterId == "door_encounter_arsenal_brass_trader");
            Assert.Contains(system.Catalog, e => e.encounterId == "door_encounter_day_360_dawn_witnesses");
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
            Assert.True(items.Count >= 48, $"Expected >= 48 items, got {items.Count}");
            Assert.Contains(items, item => item.id == "item_military_filter_crate");
            Assert.Contains(items, item => item.id == "item_continental_maritime_transponder");
            Assert.Contains(items, item => item.id == "item_boron_shielding_tile");
            Assert.Contains(items, item => item.id == "item_brass_stamping_die");
            Assert.Contains(items, item => item.id == "item_evacuation_manifest_scroll");
            // Binding parity: the file's schema must actually reach the DTO fields
            // (Unity's JsonUtility binds these; the Godot serializer must too).
            foreach (var item in items)
            {
                Assert.False(string.IsNullOrEmpty(item.name), item.id + " name unbound");
                Assert.False(string.IsNullOrEmpty(item.category), item.id + " category unbound");
            }

            var events = YearOfAshCatalogLoader.LoadEvents(dataDir, fileIO, json);
            Assert.True(events.Count >= 48, $"Expected >= 48 events, got {events.Count}");
            Assert.Contains(events, ev => ev.id == "event_deep_freeze_onset");
            Assert.Contains(events, ev => ev.id == "event_black_mud_thaw_inundation");
            Assert.Contains(events, ev => ev.id == "event_granite_arsenal_foundry_explosion");
            Assert.Contains(events, ev => ev.id == "event_final_dawn_year_one");
            foreach (var ev in events)
            {
                Assert.False(string.IsNullOrEmpty(ev.description), ev.id + " description unbound");
                Assert.True(ev.day >= 180, ev.id + " day unbound");
            }

            var questSystem = new QuestlineSystem();
            int initialQuests = questSystem.Catalog.Count;
            int loadedQuests = YearOfAshCatalogLoader.LoadAndRegisterQuests(questSystem, dataDir, fileIO, json);
            Assert.True(loadedQuests >= 24, $"Expected >= 24 external quests, got {loadedQuests}");
            Assert.True(questSystem.Catalog.Count >= initialQuests);
            Assert.NotNull(questSystem.FindDefinition("quest_continental_convoy_gate"));
            Assert.NotNull(questSystem.FindDefinition("quest_granite_foundry_brass_smuggling"));
            Assert.NotNull(questSystem.FindDefinition("quest_day_360_final_reckoning"));
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
            Assert.True(locations.Count >= 60, $"Expected >= 60 locations, got {locations.Count}");
            Assert.Contains(locations, l => l.id == "loc_the_allotments");
            Assert.Contains(locations, l => l.id == "loc_denial_cut_substation");
            Assert.Contains(locations, l => l.id == "loc_brine_pumping_sluice");
            Assert.Contains(locations, l => l.id == "loc_granite_arsenal_foundry");
            Assert.Contains(locations, l => l.id == "loc_the_final_dawn_outlook");
            foreach (var loc in locations)
            {
                Assert.False(string.IsNullOrEmpty(loc.displayName), loc.id + " displayName unbound");
                Assert.False(string.IsNullOrEmpty(loc.sector), loc.id + " sector unbound");
                Assert.True(loc.riskLevel >= 1, loc.id + " riskLevel unbound");
            }

            var radio = YearOfAshCatalogLoader.LoadRadioBroadcasts(dataDir, fileIO, json);
            Assert.True(radio.Count >= 36, $"Expected >= 36 radio broadcasts, got {radio.Count}");
            Assert.Contains(radio, r => r.id == "radio_142_carrier_discovery");
            Assert.Contains(radio, r => r.id == "radio_garrison_martial_edict");
            Assert.Contains(radio, r => r.id == "radio_granite_arsenal_mobilization_broadcast");
            Assert.Contains(radio, r => r.id == "radio_day_360_beacon_silence");
            foreach (var r in radio)
            {
                Assert.False(string.IsNullOrEmpty(r.message), r.id + " message unbound");
                Assert.False(string.IsNullOrEmpty(r.source), r.id + " source unbound");
                Assert.False(string.IsNullOrEmpty(r.frequency), r.id + " frequency unbound");
                Assert.True(r.dayTrigger > 0, r.id + " dayTrigger unbound");
            }

            var survivors = YearOfAshCatalogLoader.LoadSurvivors(dataDir, fileIO, json);
            Assert.True(survivors.Count >= 36, $"Expected >= 36 survivors, got {survivors.Count}");
            Assert.Contains(survivors, s => s.id == "survivor_ottilie_frayne");
            Assert.Contains(survivors, s => s.id == "survivor_anneke_ruhl");
            Assert.Contains(survivors, s => s.id == "survivor_markov_arsenal_assayer");
            Assert.Contains(survivors, s => s.id == "survivor_talia_upland_commander");
            foreach (var s in survivors)
            {
                Assert.False(string.IsNullOrEmpty(s.name), s.id + " name unbound");
                Assert.False(string.IsNullOrEmpty(s.moralAlignment), s.id + " moralAlignment unbound");
            }
            Assert.Contains(survivors, s => s.id == "survivor_anton_salt_trader");
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

        [Fact]
        public void YearOfAshSave_Roundtrip_PreservesDeepFreezeAndRadon()
        {
            // Both systems accumulate irreversible state: intake ice and frozen-pipeline
            // days on the thermal side, a degrading scrubber filter and a cumulative alpha
            // dose on the radon side. A reload that resets them is a save-scum exploit —
            // quit during a radon crisis, come back to a pristine filter and zero dose.
            var timeline = new YearOfAshTimelineSystem();
            timeline.AdvanceDay(340);

            // The two systems own different phases: deep freeze runs to day 240 and
            // de-ices after, radon only wakes at day 300. Tick each in its own window
            // so both carry real accumulated state at the moment of the save.
            var freeze = new YearOfAshDeepFreezeSystem();
            for (int day = 190; day <= 240; day++)
                freeze.TickDailyThermal(day, -38.0f);

            var radon = new YearOfAshRadonSystem();
            for (int day = 300; day <= 340; day++)
                radon.TickDailyRadon(day, -38.0f);

            Assert.True(radon.State.totalAlphaDoseLogged > 0.0f, "precondition: dose accumulated");
            Assert.True(radon.State.scrubberFilterHealthPercent < 100.0f, "precondition: filter degraded");
            Assert.True(freeze.State.intakeIceThicknessMm > 0.0f, "precondition: intake iced");

            var save = YearOfAshSaveCodec.Capture(
                timeline, new DoorEncounterSystem(), new FactionWarSystem(), null,
                freeze, radon, new QuestlineSystem());

            var json = new SystemTextJsonSerializer();
            var loaded = YearOfAshSaveCodec.Decode(YearOfAshSaveCodec.Encode(save, json), json);

            var freezeB = new YearOfAshDeepFreezeSystem();
            var radonB = new YearOfAshRadonSystem();
            YearOfAshSaveCodec.Restore(
                loaded, new YearOfAshTimelineSystem(), new DoorEncounterSystem(),
                new FactionWarSystem(), freezeB, radonB, new QuestlineSystem());

            Assert.Equal(freeze.State.indoorTemperatureCelsius, freezeB.State.indoorTemperatureCelsius);
            Assert.Equal(freeze.State.intakeIceThicknessMm, freezeB.State.intakeIceThicknessMm);
            Assert.Equal(freeze.State.thermalInsulationQuality, freezeB.State.thermalInsulationQuality);
            Assert.Equal(freeze.State.daysFrozenPipelinesExperienced, freezeB.State.daysFrozenPipelinesExperienced);
            Assert.Equal(freeze.IsIntakeBlocked, freezeB.IsIntakeBlocked);

            Assert.Equal(radon.State.indoorRadonBqm3, radonB.State.indoorRadonBqm3);
            Assert.Equal(radon.State.scrubberFilterHealthPercent, radonB.State.scrubberFilterHealthPercent);
            Assert.Equal(radon.State.totalAlphaDoseLogged, radonB.State.totalAlphaDoseLogged);
            Assert.Equal(radon.State.activeFoundationFissures, radonB.State.activeFoundationFissures);
            Assert.Equal(radon.State.isScrubberAlarmActive, radonB.State.isScrubberAlarmActive);
        }

        [Fact]
        public void YearOfAshSave_Roundtrip_PreservesQuestlineProgress()
        {
            var quests = new QuestlineSystem();
            var captured = quests.CaptureState();
            captured.completedQuestlineIds.Add("quest_year_of_ash_thaw");
            captured.failedQuestlineIds.Add("quest_year_of_ash_convoy");
            captured.totalMoraleDeltaFromQuests = -12;
            captured.totalGuiltDeltaFromQuests = 7;

            var questsB = new QuestlineSystem();
            questsB.RestoreState(captured);

            Assert.Contains("quest_year_of_ash_thaw", questsB.State.completedQuestlineIds);
            Assert.Contains("quest_year_of_ash_convoy", questsB.State.failedQuestlineIds);
            Assert.Equal(-12, questsB.State.totalMoraleDeltaFromQuests);
            Assert.Equal(7, questsB.State.totalGuiltDeltaFromQuests);
        }

        [Fact]
        public void YearOfAshSave_V1File_MigratesToCurrentWithFreshSections()
        {
            // A save written before the thermal/radon/quest sections existed must still
            // load — validated against its FROZEN v1 shape, exactly like HoldfastSave.
            var v1 = new YearOfAshSaveV1
            {
                simDay = 255,
                timeline = new YearOfAshTimelineSystem().CaptureState(),
                encounters = new DoorEncounterSystem().CaptureState(),
                factionWar = new FactionWarSystem().CaptureState()
            };
            v1.timeline.currentDay = 255;
            v1.Checksum = SaveChecksum.Compute(v1);

            var json = new SystemTextJsonSerializer();
            var migrated = YearOfAshSaveCodec.Decode(json.Serialize(v1), json);

            Assert.Equal(YearOfAshSave.CurrentSaveVersion, migrated.saveVersion);
            Assert.Equal(255, migrated.simDay);
            Assert.NotNull(migrated.deepFreeze);
            Assert.NotNull(migrated.radon);
            Assert.NotNull(migrated.quests);
            // Fresh sections carry system defaults, not zeroes.
            Assert.Equal(120.0f, migrated.radon.indoorRadonBqm3);
            Assert.Equal(100.0f, migrated.radon.scrubberFilterHealthPercent);
        }

        [Fact]
        public void GetPlayableQuestlines_NeverOffersAnUnadvanceableQuestline()
        {
            // The JSON catalog carries objectives, not choices, so questlines loaded from
            // it can be started and then never advanced. A host that offers them strands
            // the player. GetPlayableQuestlines is the filter; this pins that contract.
            string dataDir;
            if (!CatalogLocator.TryFindDataDirectory(System.IO.Directory.GetCurrentDirectory(), out dataDir))
                CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out dataDir);

            var quests = new QuestlineSystem();
            YearOfAshCatalogLoader.LoadAndRegisterQuests(
                quests, dataDir, new FileSystemIO(), new SystemTextJsonSerializer());

            foreach (int day in new[] { 185, 220, 255, 300, 360 })
            {
                foreach (var def in quests.GetPlayableQuestlines(day))
                {
                    var first = def.FindStage(def.firstStageId);
                    Assert.NotNull(first);
                    Assert.True(first!.choices.Count > 0,
                        def.questlineId + " was offered on day " + day + " with no choices");

                    // And it must genuinely advance, not just look advanceable.
                    var probe = new QuestlineSystem();
                    probe.RegisterQuestline(def);
                    Assert.True(probe.StartQuestline(def.questlineId, day));
                    Assert.NotNull(probe.TakeChoice(def.questlineId, first.choices[0].choiceId, day));
                }
            }
        }

        [Fact]
        public void WithheldQuestlineCount_ReportsTheUnauthoredContentGap()
        {
            // Deliberately asserts the gap is *visible*, not that it is zero. When the
            // missing choices get authored this count drops and the host diagnostic
            // shrinks with it — that is the intended signal, not a test failure.
            string dataDir;
            if (!CatalogLocator.TryFindDataDirectory(System.IO.Directory.GetCurrentDirectory(), out dataDir))
                CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out dataDir);

            var quests = new QuestlineSystem();
            YearOfAshCatalogLoader.LoadAndRegisterQuests(
                quests, dataDir, new FileSystemIO(), new SystemTextJsonSerializer());

            int playable = quests.GetPlayableQuestlines(360).Count;
            int withheld = quests.WithheldQuestlineCount(360);

            Assert.Equal(quests.GetAvailableQuestlines(360).Count, playable + withheld);
            Assert.True(playable > 0, "the hand-authored questlines must remain offerable");
        }

        private class ManualClock : IClock
        {
            private int _day;
            public ManualClock(int day) { _day = day; }
            public int Day => _day;
            public void AdvanceDays(int days) { _day += days; }
            public void SetDay(int day) { _day = day; }
        }

        [Fact]
        public void LegacyQuestConverter_ProducesPlayableQuestlines()
        {
            string dataDir;
            if (!CatalogLocator.TryFindDataDirectory(System.IO.Directory.GetCurrentDirectory(), out dataDir))
                CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out dataDir);
            if (string.IsNullOrEmpty(dataDir)) return;

            var quests = new QuestlineSystem();
            int registered = YearOfAshCatalogLoader.LoadAndRegisterQuests(
                quests, dataDir, new FileSystemIO(), new SystemTextJsonSerializer());

            Assert.True(quests.Catalog.Count >= registered);

            foreach (var def in quests.Catalog)
            {
                Assert.True(quests.IsPlayable(def), $"quest '{def.questlineId}' registered but not playable");
                var first = def.FindStage(def.firstStageId);
                Assert.NotNull(first);
                Assert.True(first.choices.Count > 0, $"quest '{def.questlineId}' first stage has no choices");

                var last = def.stages[def.stages.Count - 1];
                Assert.True(last.isTerminal, $"quest '{def.questlineId}' last stage not terminal");
            }
        }

        [Fact]
        public void LegacyQuestConverter_LinearTraversal_ReachesTerminal()
        {
            string dataDir;
            if (!CatalogLocator.TryFindDataDirectory(System.IO.Directory.GetCurrentDirectory(), out dataDir))
                CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out dataDir);
            if (string.IsNullOrEmpty(dataDir)) return;

            var quests = new QuestlineSystem();
            YearOfAshCatalogLoader.LoadAndRegisterQuests(
                quests, dataDir, new FileSystemIO(), new SystemTextJsonSerializer());

            var def = quests.FindDefinition("quest_garrison_blood_debt");
            Assert.NotNull(def);
            Assert.True(quests.IsPlayable(def));

            Assert.True(quests.StartQuestline(def.questlineId, 200));
            var record = quests.State.active.Find(a => a.questlineId == def.questlineId);
            Assert.NotNull(record);
            Assert.Equal(def.firstStageId, record.currentStageId);

            var stage = def.FindStage(record.currentStageId);
            while (!stage.isTerminal && stage.choices.Count > 0)
            {
                var choice = stage.choices[0];
                var result = quests.TakeChoice(def.questlineId, choice.choiceId, 200);
                Assert.NotNull(result);
                if (string.IsNullOrEmpty(choice.nextStageId)) break;
                stage = def.FindStage(record.currentStageId);
            }
            Assert.Equal(QuestlineStatus.Completed, record.status);
        }

        [Fact]
        public void VerdictLocations_LoadAndAreQueryable()
        {
            string dataDir;
            if (!CatalogLocator.TryFindDataDirectory(System.IO.Directory.GetCurrentDirectory(), out dataDir))
                CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out dataDir);
            if (string.IsNullOrEmpty(dataDir)) return;

            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var locations = VerdictCatalogLoader.LoadLocations(dataDir, io, json);

            Assert.True(locations.Count >= 4, $"expected >=4 verdict locations, got {locations.Count}");

            var geophone = locations.Find(l => l.id == "loc_geophone_pit_1");
            Assert.NotNull(geophone);
            Assert.Equal("The First Geophone Pit", geophone.displayName);
            Assert.True(geophone.dangerLevel > 0);
            Assert.True(geophone.travelHours > 0f);

            var fuseWorld = locations.Find(l => l.id == "loc_network_fuse_bunker");
            Assert.NotNull(fuseWorld);
            Assert.Equal("The Fuse World", fuseWorld.displayName);

            var tapeSilo = locations.Find(l => l.id == "loc_archive_tape_silo");
            Assert.NotNull(tapeSilo);
            Assert.Equal("The Archive Tape-Silo", tapeSilo.displayName);

            var array = locations.Find(l => l.id == "loc_twelve_gauge_array");
            Assert.NotNull(array);
            Assert.Equal("The Twelve-Gauge Array", array.displayName);
        }
    }
}
