using System.Collections.Generic;
using System.IO;
using Xunit;
using Ashfall.Core;

namespace Ashfall.Core.Tests
{
    public class DutyRosterSystemTests
    {
        private static DutyRosterSystem Sys(int seed = 1208) => new DutyRosterSystem(seed);

        private static List<DutyRosterOccupant> Occupants(params (string id, string name, string job)[] people)
        {
            var list = new List<DutyRosterOccupant>();
            for (int i = 0; i < people.Length; i++)
            {
                list.Add(new DutyRosterOccupant
                {
                    survivorId = people[i].id,
                    displayName = people[i].name,
                    occupationObserved = people[i].job,
                    sleptHere = true
                });
            }
            return list;
        }

        [Fact]
        public void BlankUntilUnlock()
        {
            var roster = Sys();
            Assert.False(roster.IsUnlocked);
            Assert.Equal(DutyRosterIds.ScriptBlank, roster.ChartScript);
            Assert.False(roster.WriteName(DutyRosterIds.NpcKessAdler, "Kess Adler", "records_clerk", DutyRosterIds.ScriptPencil, 60, true));
            Assert.Equal(0, roster.OccupiedRowCount);
        }

        [Fact]
        public void ChartGateDayOrInspect()
        {
            var roster = Sys();
            roster.Unlock(10);
            Assert.False(roster.CanBeginChart(10, false, false));
            Assert.True(roster.CanBeginChart(DutyRosterIds.SoftGateDay, false, false));
            roster.NotifyWallInspected();
            Assert.True(roster.CanBeginChart(10, false, false));
        }

        [Fact]
        public void PencilMorningFill()
        {
            var roster = Sys();
            roster.Unlock(60);
            Assert.True(roster.ResolveChartChoice(DutyRosterIds.ChoiceWritePencil, 60));
            Assert.True(roster.MutationInUse);
            var home = Occupants(
                (DutyRosterIds.NpcKessAdler, "Kess Adler", "records_clerk"),
                (DutyRosterIds.NpcAnselDuth, "Ansel Duth", "parent"));
            roster.TickMorning(61, home);
            Assert.Equal(2, roster.OccupiedRowCount);
            Assert.Equal(DutyRosterIds.ScriptPencil, roster.GetRow(DutyRosterIds.NpcKessAdler).script);
            Assert.Equal(61, roster.GetRow(DutyRosterIds.NpcAnselDuth).lastSleptDay);
        }

        [Fact]
        public void InkNeverAutoFills()
        {
            var roster = Sys();
            roster.Unlock(60);
            roster.ResolveChartChoice(DutyRosterIds.ChoiceWritePencil, 60);
            var two = Occupants(
                (DutyRosterIds.NpcKessAdler, "Kess Adler", "records_clerk"),
                (DutyRosterIds.NpcAnselDuth, "Ansel Duth", "parent"));
            roster.TickMorning(61, two);
            Assert.True(roster.SetRowScript(DutyRosterIds.NpcKessAdler, DutyRosterIds.ScriptInk));
            Assert.Equal(DutyRosterIds.ScriptInk, roster.ChartScript);
            var three = Occupants(
                (DutyRosterIds.NpcKessAdler, "Kess Adler", "records_clerk"),
                (DutyRosterIds.NpcAnselDuth, "Ansel Duth", "parent"),
                ("npc_hadi_morrow", "Hadi Morrow", "veterinary_assistant"));
            roster.TickMorning(62, three);
            Assert.Equal(2, roster.OccupiedRowCount);
            Assert.Null(roster.GetRow("npc_hadi_morrow"));
        }

        [Fact]
        public void FourteenCap()
        {
            var roster = Sys();
            roster.Unlock(60);
            roster.ResolveChartChoice(DutyRosterIds.ChoiceWritePencil, 60);
            for (int i = 0; i < DutyRosterIds.ManifestCap; i++)
            {
                string id = "survivor_" + i.ToString("00");
                Assert.True(roster.WriteName(id, id, "unlisted", DutyRosterIds.ScriptPencil, 60, true));
            }

            Assert.Equal(14, roster.OccupiedRowCount);
            Assert.False(roster.WriteName("survivor_14", "extra", "unlisted", DutyRosterIds.ScriptPencil, 60, true));
        }

        [Fact]
        public void CannotAssignDead()
        {
            var roster = Sys();
            roster.Unlock(60);
            roster.ResolveChartChoice(DutyRosterIds.ChoiceWritePencil, 60);
            Assert.True(roster.WriteName(DutyRosterIds.NpcAnselDuth, "Ansel Duth", "parent", DutyRosterIds.ScriptPencil, 60, true));
            Assert.True(roster.SetStatus(DutyRosterIds.NpcAnselDuth, DutyRosterIds.StatusDead));
            Assert.False(roster.Assign(DutyRosterIds.RoleNightWatch, DutyRosterIds.NpcAnselDuth));
        }

        [Fact]
        public void BlankRowsInkWithdrawsAccess()
        {
            var roster = Sys();
            roster.Unlock(60);
            roster.ResolveChartChoice(DutyRosterIds.ChoiceWritePencil, 60);
            roster.RegisterBlankRowsLivingName("npc_nila_brant");
            Assert.True(roster.BlankRowsAccess);
            Assert.False(roster.WriteName("npc_nila_brant", "Nila Brant", "lamp_oil_clerk", DutyRosterIds.ScriptPencil, 60, true));
            Assert.True(roster.WriteName("npc_nila_brant", "Nila Brant", "lamp_oil_clerk", DutyRosterIds.ScriptInk, 60, true));
            Assert.False(roster.BlankRowsAccess);
        }

        [Fact]
        public void LeaveBlankFortyDays()
        {
            var roster = Sys();
            roster.Unlock(60);
            Assert.True(roster.ResolveChartChoice(DutyRosterIds.ChoiceLeaveBlank, 60));
            var home = Occupants((DutyRosterIds.NpcKessAdler, "Kess Adler", "records_clerk"));
            for (int d = 1; d <= DutyRosterIds.StillBlankDays; d++)
                roster.TickMorning(60 + d, home);
            Assert.True(roster.State.mutationRosterStillBlank);
            Assert.Equal(0, roster.OccupiedRowCount);
        }

        [Fact]
        public void HiddenOmittedFromNorthCopy()
        {
            var roster = Sys();
            roster.Unlock(60);
            roster.ResolveChartChoice(DutyRosterIds.ChoiceWritePencil, 60);
            roster.WriteName("npc_hadi_morrow", "Hadi Morrow", "veterinary_assistant", DutyRosterIds.ScriptPencil, 60, true);
            roster.WriteName(DutyRosterIds.NpcKessAdler, "Kess Adler", "records_clerk", DutyRosterIds.ScriptPencil, 60, true);
            roster.HideFromNorthCopy("npc_hadi_morrow");
            var north = roster.CopyForNorth();
            Assert.Single(north);
            Assert.Equal(DutyRosterIds.NpcKessAdler, north[0].survivorId);
            Assert.True(roster.LevyRequiresRows);
            Assert.False(roster.IsValidLevyName("npc_hadi_morrow"));
            Assert.True(roster.IsValidLevyName(DutyRosterIds.NpcKessAdler));
        }

        [Fact]
        public void SameSeedSameAssignment()
        {
            var home = Occupants(
                (DutyRosterIds.NpcKessAdler, "Kess Adler", "records_clerk"),
                (DutyRosterIds.NpcAnselDuth, "Ansel Duth", "parent"),
                ("npc_hadi_morrow", "Hadi Morrow", "veterinary_assistant"));
            var a = Sys(1208);
            var b = Sys(1208);
            a.Unlock(60);
            b.Unlock(60);
            a.ResolveChartChoice(DutyRosterIds.ChoiceWritePencil, 60);
            b.ResolveChartChoice(DutyRosterIds.ChoiceWritePencil, 60);
            a.TickMorning(61, home);
            b.TickMorning(61, home);
            a.AutoAssignDefaults(61);
            b.AutoAssignDefaults(61);
            Assert.Equal(a.GetAssignment(DutyRosterIds.RoleNightWatch), b.GetAssignment(DutyRosterIds.RoleNightWatch));
            Assert.Equal(a.GetAssignment(DutyRosterIds.RoleMess), b.GetAssignment(DutyRosterIds.RoleMess));
            Assert.Equal(a.GetAssignment(DutyRosterIds.RoleHatchOpener), b.GetAssignment(DutyRosterIds.RoleHatchOpener));
        }

        [Fact]
        public void LadleProtocolAndBurn()
        {
            var roster = Sys();
            roster.Unlock(60);
            roster.ResolveChartChoice(DutyRosterIds.ChoiceWritePencil, 60);
            roster.WriteName(DutyRosterIds.NpcKessAdler, "Kess Adler", "records_clerk", DutyRosterIds.ScriptPencil, 60, true);
            Assert.True(roster.ResolveLadleChoice(DutyRosterIds.ChoiceLadleProtocol, 61));
            Assert.True(roster.State.mutationRationProtocol);
            Assert.True(roster.BurnChart(80));
            Assert.Equal(DutyRosterIds.ScriptBurned, roster.ChartScript);
            Assert.Equal(0, roster.OccupiedRowCount);
            Assert.False(roster.WriteName(DutyRosterIds.NpcKessAdler, "Kess Adler", "records_clerk", DutyRosterIds.ScriptPencil, 81, true));
        }

        [Fact]
        public void SaveRoundtrip()
        {
            var json = new SystemTextJsonSerializer();
            var roster = Sys(1208);
            roster.Unlock(60);
            roster.NotifyWallInspected();
            roster.ResolveChartChoice(DutyRosterIds.ChoiceWritePencil, 60);
            roster.WriteName(DutyRosterIds.NpcKessAdler, "Kess Adler", "records_clerk", DutyRosterIds.ScriptPencil, 60, true);
            roster.Assign(DutyRosterIds.RoleNightWatch, DutyRosterIds.NpcKessAdler);
            roster.HideFromNorthCopy(DutyRosterIds.NpcKessAdler);
            string blob = json.Serialize(roster.CaptureState());
            var restored = new DutyRosterSystem(1);
            restored.RestoreState(json.Deserialize<DutyRosterSystemState>(blob));
            Assert.True(restored.IsUnlocked);
            Assert.True(restored.MutationInUse);
            Assert.Equal(DutyRosterIds.NpcKessAdler, restored.GetAssignment(DutyRosterIds.RoleNightWatch));
            Assert.NotNull(restored.GetRow(DutyRosterIds.NpcKessAdler));
            Assert.False(restored.IsValidLevyName(DutyRosterIds.NpcKessAdler));
            Assert.Equal(1208, restored.State.seedSalt);
        }

        [Fact]
        public void PencilRefusesUnsleptName()
        {
            var roster = Sys();
            roster.Unlock(60);
            roster.ResolveChartChoice(DutyRosterIds.ChoiceWritePencil, 60);
            Assert.False(roster.WriteName("npc_on_stool", "Edor Vale", "census_clerk", DutyRosterIds.ScriptPencil, 60, false));
        }

        [Fact]
        public void InkEndingResolves()
        {
            var roster = Sys();
            roster.Unlock(60);
            roster.ResolveChartChoice(DutyRosterIds.ChoiceWritePencil, 60);
            roster.WriteName(DutyRosterIds.NpcKessAdler, "Kess Adler", "records_clerk", DutyRosterIds.ScriptPencil, 60, true);
            Assert.True(roster.ResolveInkEnding(80));
            Assert.Equal(DutyRosterIds.ScriptInk, roster.ChartScript);
            Assert.Equal(DutyRosterIds.EndingInk, roster.State.endingId);
            Assert.False(roster.State.kessPencilAllowed);
        }

        [Fact]
        public void SecondWinterFlagSurvivesSave()
        {
            var json = new SystemTextJsonSerializer();
            var roster = Sys();
            roster.Unlock(60);
            roster.SetSecondWinterActive(true);
            Assert.True(roster.IsSecondWinterActive);
            string blob = json.Serialize(roster.CaptureState());
            var restored = new DutyRosterSystem(1);
            restored.RestoreState(json.Deserialize<DutyRosterSystemState>(blob));
            Assert.True(restored.IsSecondWinterActive);
        }

        [Fact]
        public void BurnBlocksInkEnding()
        {
            var roster = Sys();
            roster.Unlock(60);
            roster.BurnChart(80);
            Assert.False(roster.ResolveInkEnding(81));
            Assert.Equal(DutyRosterIds.ScriptBurned, roster.ChartScript);
        }
    }

    public class MoraleMarkSystemTests
    {
        [Fact]
        public void FlagAndLaterProse()
        {
            var catalog = LoadCatalog();
            var marks = new MoraleMarkSystem();
            marks.BindCatalog(catalog);
            Assert.False(marks.HasMark("mark_bowl_cold"));
            marks.SetMark("mark_bowl_cold", payload: "left until cold", day: 62);
            Assert.True(marks.HasMark("mark_bowl_cold"));
            Assert.Equal("left until cold", marks.GetPayload("mark_bowl_cold"));
            Assert.Contains("enamel", marks.GetLaterProse("mark_bowl_cold"));
        }

        [Fact]
        public void DoesNotClearOnNewDay_OnlyAuthoredClear()
        {
            var marks = new MoraleMarkSystem();
            marks.SetMark("mark_ladle_default", day: 70);
            marks.SetMark("mark_bowl_cold", day: 71);
            Assert.Equal(2, marks.Count);
            Assert.True(marks.ClearMark("mark_ladle_default"));
            Assert.False(marks.HasMark("mark_ladle_default"));
            Assert.True(marks.HasMark("mark_bowl_cold"));
        }

        [Fact]
        public void SaveRoundtrip()
        {
            var json = new SystemTextJsonSerializer();
            var marks = new MoraleMarkSystem();
            marks.SetMark("mark_child_levy_story", payload: "north, forms, thirty days", day: 90);
            string blob = json.Serialize(marks.CaptureState());
            var restored = new MoraleMarkSystem();
            restored.RestoreState(json.Deserialize<MoraleMarkSystemState>(blob));
            Assert.True(restored.HasMark("mark_child_levy_story"));
            Assert.Equal("north, forms, thirty days", restored.GetPayload("mark_child_levy_story"));
        }

        private static DutyRosterCatalog LoadCatalog()
        {
            var loader = new DutyRosterCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer());
            return loader.Load(DutyRosterCatalogTests.DataDir());
        }
    }

    public class DutyRosterCatalogTests
    {
        internal static string DataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found))
                return found;
            if (CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out found))
                return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        [Fact]
        public void StackWingIdsUniqueSnakeCase()
        {
            var loader = new DutyRosterCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer());
            var catalog = loader.Load(DataDir());
            Assert.True(catalog.Locations.Count >= 8);
            var set = new HashSet<string>();
            int overflowCount = 0;
            for (int i = 0; i < catalog.Locations.Count; i++)
            {
                var e = catalog.Locations[i];
                Assert.False(string.IsNullOrEmpty(e.id));
                Assert.True(set.Add(e.id), "duplicate " + e.id);
                Assert.Equal(e.id, e.id.ToLowerInvariant());
                if (e.region == "the_stack" || e.region == "the_approach")
                {
                    Assert.Equal(0f, e.travelHours);
                }
                else
                {
                    Assert.True(e.travelHours >= 1.5f, e.id + " overflow should cost travel");
                    Assert.Equal("the_overflow", e.region);
                    overflowCount++;
                }
                Assert.True(e.region == "the_stack" || e.region == "the_approach" || e.region == "the_overflow", "region " + e.region);
            }
            Assert.Equal(4, overflowCount);

            var wall = catalog.GetLocation(DutyRosterIds.LocStackRosterWall);
            Assert.NotNull(wall);
            Assert.Contains("Fourteen rows", wall.inspect);
            Assert.Contains("ALLOCATION 12", wall.description);
            Assert.NotNull(catalog.GetLocation(DutyRosterIds.LocStackMess));
            Assert.NotNull(catalog.GetLocation(DutyRosterIds.LocStackSleeping));
            Assert.NotNull(catalog.GetLocation(DutyRosterIds.LocStackFiltration));
            Assert.NotNull(catalog.GetLocation("loc_stack_clinic_alcove"));
            Assert.NotNull(catalog.GetLocation("loc_approach_hatch"));
            Assert.NotNull(catalog.GetLocation("loc_approach_apron"));
            Assert.NotNull(catalog.GetLocation("loc_approach_stool"));
            Assert.NotNull(catalog.GetLocation("loc_approach_decon"));
            Assert.NotNull(catalog.GetLocation("loc_overflow_alloc_11"));
            Assert.NotNull(catalog.GetLocation("loc_overflow_alloc_13"));
            Assert.NotNull(catalog.GetLocation("loc_overflow_pump_hatch"));
            Assert.NotNull(catalog.GetLocation("loc_overflow_blank_cellar"));
        }

        [Fact]
        public void ChartAndLadleQuestsRegistered()
        {
            var loader = new DutyRosterCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer());
            var catalog = loader.Load(DataDir());
            Assert.True(catalog.Quests.Count >= 2);
            var chart = catalog.GetQuest(DutyRosterIds.QuestTheChart);
            Assert.NotNull(chart);
            Assert.Equal("shelter", chart.type);
            Assert.Equal(DutyRosterIds.LocStackRosterWall, chart.target_location_id);
            Assert.Equal("lore_dr_chart", chart.knowledge_key);
            Assert.Equal("mutation_roster_in_use", chart.complete_mutation);
            Assert.Contains(chart.choices, c => c.id == DutyRosterIds.ChoiceWritePencil);
            var ladle = catalog.GetQuest(DutyRosterIds.QuestWhoEats);
            Assert.NotNull(ladle);
            Assert.Equal(DutyRosterIds.QuestTheChart, ladle.prereq_quest_id);
            Assert.Equal(DutyRosterIds.LocStackMess, ladle.target_location_id);
            Assert.Contains(ladle.choices, c => c.id == DutyRosterIds.ChoiceLadleProtocol);

            var fourteenth = catalog.GetQuest("quest_roster_fourteenth");
            Assert.NotNull(fourteenth);
            Assert.Equal("shelter", fourteenth.type);
            Assert.Equal("loc_approach_hatch", fourteenth.target_location_id);
            Assert.Contains(fourteenth.choices, c => c.id == "roster_fourteenth_let_in");
            Assert.Contains(fourteenth.choices, c => c.id == "roster_fourteenth_deny");

            var caretaker = catalog.GetQuest("quest_roster_caretaker");
            Assert.NotNull(caretaker);
            Assert.Equal("faction", caretaker.type);
            Assert.Equal("loc_stack_clinic_alcove", caretaker.target_location_id);
            Assert.Contains(caretaker.choices, c => c.id == "roster_hadi_list");
            Assert.Contains(caretaker.choices, c => c.id == "roster_hadi_hide");
            Assert.Contains(caretaker.choices, c => c.id == "roster_hadi_send");

            var column = catalog.GetQuest("quest_roster_the_column");
            Assert.NotNull(column);
            Assert.Contains(column.choices, c => c.id == "roster_column_honour");
            Assert.Contains(column.choices, c => c.id == "roster_column_hide");

            var tin = catalog.GetQuest("quest_roster_the_tin");
            Assert.NotNull(tin);
            Assert.Equal("loc_stack_filtration", tin.target_location_id);
            Assert.Contains(tin.choices, c => c.id == "roster_tin_plate");

            var sole = catalog.GetQuest("quest_roster_sole");
            Assert.NotNull(sole);
            Assert.Contains(sole.choices, c => c.id == "roster_sole_two_witnesses");
            Assert.Contains(sole.choices, c => c.id == "roster_sole_one_witness");

            var window = catalog.GetQuest("quest_roster_window");
            Assert.NotNull(window);
            Assert.Equal("shelter", window.type);
            Assert.Contains(window.choices, c => c.id == "roster_window_held");

            var ink = catalog.GetQuest("quest_roster_ink");
            Assert.NotNull(ink);
            Assert.Equal("shelter", ink.type);
            Assert.Contains(ink.choices, c => c.id == "roster_ink");
            Assert.Contains(ink.choices, c => c.id == "roster_burn_chart");
            Assert.Contains(ink.choices, c => c.id == "roster_erase_all");
            Assert.Contains(ink.choices, c => c.id == "roster_keep_pencil");
        }

        [Fact]
        public void SecondWinterSeasonLoaded()
        {
            var loader = new DutyRosterCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer());
            var catalog = loader.Load(DataDir());
            var season = catalog.GetSeason(DutyRosterIds.SeasonSecondWinter);
            Assert.NotNull(season);
            Assert.Equal(DutyRosterIds.SecondWinterWindowMinDays, season.windowMinDays);
            Assert.Equal(DutyRosterIds.SecondWinterWindowMaxDays, season.windowMaxDays);
            Assert.Equal(DutyRosterIds.SecondWinterEncounterWeight, season.encounterWeight, 3);
        }

        [Fact]
        public void FullExpansionCatalogComplete()
        {
            var loader = new DutyRosterCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer());
            var catalog = loader.Load(DataDir());

            // All 10 main quests registered (plan §4.1).
            Assert.NotNull(catalog.GetQuest(DutyRosterIds.QuestTheChart));
            Assert.NotNull(catalog.GetQuest(DutyRosterIds.QuestWhoEats));
            Assert.NotNull(catalog.GetQuest(DutyRosterIds.QuestFourteenth));
            Assert.NotNull(catalog.GetQuest(DutyRosterIds.QuestCaretaker));
            Assert.NotNull(catalog.GetQuest(DutyRosterIds.QuestTheColumn));
            Assert.NotNull(catalog.GetQuest(DutyRosterIds.QuestTheTin));
            Assert.NotNull(catalog.GetQuest(DutyRosterIds.QuestQuiet));
            Assert.NotNull(catalog.GetQuest(DutyRosterIds.QuestSole));
            Assert.NotNull(catalog.GetQuest(DutyRosterIds.QuestWindow));
            Assert.NotNull(catalog.GetQuest(DutyRosterIds.QuestInk));

            // Side quests registered (plan §4.2) — spot-check each family.
            Assert.NotNull(catalog.GetQuest("quest_roster_pell_numbers"));
            Assert.NotNull(catalog.GetQuest("quest_roster_frayne_minutes"));
            Assert.NotNull(catalog.GetQuest("quest_roster_grange_vote"));
            Assert.NotNull(catalog.GetQuest("quest_roster_ivy_oil"));
            Assert.NotNull(catalog.GetQuest("quest_roster_blank_access"));
            Assert.NotNull(catalog.GetQuest("quest_roster_missing_strip"));
            Assert.NotNull(catalog.GetQuest("quest_roster_kess_pencil"));
            Assert.NotNull(catalog.GetQuest("quest_roster_hadi_shift"));
            Assert.NotNull(catalog.GetQuest("quest_roster_tamsin_watch"));
            Assert.NotNull(catalog.GetQuest("quest_roster_ansel_truth"));
            Assert.NotNull(catalog.GetQuest("quest_roster_len_tag"));
            Assert.NotNull(catalog.GetQuest("quest_roster_nila_eleven"));
            Assert.NotNull(catalog.GetQuest("quest_roster_chair"));
            Assert.NotNull(catalog.GetQuest("quest_roster_12b_kit"));
            Assert.NotNull(catalog.GetQuest("quest_roster_brigid"));
            Assert.NotNull(catalog.GetQuest("quest_roster_boot_crate"));
            Assert.NotNull(catalog.GetQuest("quest_rep_night_slate"));
            Assert.NotNull(catalog.GetQuest("quest_rep_meal_row"));
            Assert.True(catalog.Quests.Count >= 28);

            // All Stack wings incl. airlock + clinic (plan §2.1).
            Assert.NotNull(catalog.GetLocation(DutyRosterIds.LocStackAirlock));
            Assert.NotNull(catalog.GetLocation(DutyRosterIds.LocStackClinicAlcove));
            Assert.True(catalog.Locations.Count >= 14);
        }

        [Fact]
        public void RosterNpcsPresentInCharactersCatalog()
        {
            string path = System.IO.Path.Combine(DataDir(), "characters.json");
            Assert.True(System.IO.File.Exists(path));
            var json = new SystemTextJsonSerializer();
            var chars = CatalogLocator.LoadWrappedList<DutyRosterCharacterProbe>(
                System.IO.File.ReadAllText(path), SystemTextJsonSerializer.Options);
            var ids = new System.Collections.Generic.HashSet<string>();
            for (int i = 0; i < chars.Count; i++)
                ids.Add(chars[i].id);
            Assert.Contains(DutyRosterIds.NpcKessAdler, ids);
            Assert.Contains(DutyRosterIds.NpcAnselDuth, ids);
            Assert.Contains("npc_tamsin_rook", ids);
            Assert.Contains("npc_len_quill", ids);
            Assert.Contains("npc_hadi_morrow", ids);
            Assert.Contains("npc_nila_brant", ids);
        }

        private sealed class DutyRosterCharacterProbe
        {
            public string id = string.Empty;
        }

        [Fact]
        public void MarksHaveLaterProse()
        {
            var loader = new DutyRosterCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer());
            var catalog = loader.Load(DataDir());
            Assert.NotNull(catalog.GetMark("mark_bowl_cold"));
            Assert.Contains("enamel", catalog.GetMark("mark_bowl_cold").later);
            Assert.NotNull(catalog.GetMark("mark_ladle_default"));
        }
    }

    public class ShelterEncounterSystemTests
    {
        private static ShelterEncounterSystem Sys(int seed = 1208) => new ShelterEncounterSystem(seed);

        [Fact]
        public void LockedUntilUnlock()
        {
            var enc = Sys();
            Assert.False(enc.IsUnlocked);
            Assert.False(enc.StartEncounter("se_night_slate", ShelterEncounterSystem.KindNightSlate, 60));
            Assert.False(enc.QueueVisitor(ShelterEncounterSystem.VisitorEdor, 60));
        }

        [Fact]
        public void OneEncounterPerNight()
        {
            var enc = Sys();
            enc.Unlock(60);
            Assert.True(enc.StartEncounter("se_night_slate", ShelterEncounterSystem.KindNightSlate, 60));
            Assert.False(enc.StartEncounter("se_meal_short", ShelterEncounterSystem.KindMealShort, 60));
            Assert.True(enc.StartEncounter("se_meal_short", ShelterEncounterSystem.KindMealShort, 61));
            Assert.Equal(1, enc.EncountersThisNight);
        }

        [Fact]
        public void CrisisAllowsMultiple()
        {
            var enc = Sys();
            enc.Unlock(60);
            Assert.True(enc.StartEncounterCrisis("se_night_slate", ShelterEncounterSystem.KindNightSlate, 60));
            Assert.True(enc.StartEncounterCrisis("se_meal_short", ShelterEncounterSystem.KindMealShort, 60));
            Assert.Equal(2, enc.EncountersThisNight);
        }

        [Fact]
        public void VisitorQueueOneAtATime()
        {
            var enc = Sys();
            enc.Unlock(60);
            Assert.True(enc.QueueVisitor(ShelterEncounterSystem.VisitorEdor, 60));
            Assert.False(enc.QueueVisitor(ShelterEncounterSystem.VisitorEdor, 60));
            Assert.True(enc.QueueVisitor(ShelterEncounterSystem.VisitorLen, 60));
            Assert.Equal(ShelterEncounterSystem.VisitorEdor, enc.PeekVisitor());
            Assert.True(enc.ResolveVisitor(ShelterEncounterSystem.VisitorEdor));
            Assert.Equal(ShelterEncounterSystem.VisitorLen, enc.PeekVisitor());
        }

        [Fact]
        public void ResolveOnceOnly()
        {
            var enc = Sys();
            enc.Unlock(60);
            Assert.True(enc.StartEncounter("se_hatch_return", ShelterEncounterSystem.KindHatchReturn, 60));
            Assert.True(enc.ResolveEncounter("se_hatch_return", 60));
            Assert.False(enc.ResolveEncounter("se_hatch_return", 61));
            Assert.True(enc.IsResolved("se_hatch_return"));
        }

        [Fact]
        public void SaveRoundtrip()
        {
            var json = new SystemTextJsonSerializer();
            var enc = Sys(1208);
            enc.Unlock(60);
            enc.QueueVisitor(ShelterEncounterSystem.VisitorLen, 60);
            enc.StartEncounter("se_edor_stool", ShelterEncounterSystem.KindEdorStool, 60, ShelterEncounterSystem.VisitorEdor);
            enc.ResolveEncounter("se_edor_stool", 60);
            enc.SetSecondWinter(1.6f, 400);
            string blob = json.Serialize(enc.CaptureState());
            var restored = new ShelterEncounterSystem(1);
            restored.RestoreState(json.Deserialize<ShelterEncounterSystemState>(blob));
            Assert.True(restored.IsUnlocked);
            Assert.Equal(ShelterEncounterSystem.VisitorLen, restored.PeekVisitor());
            Assert.True(restored.IsResolved("se_edor_stool"));
            Assert.Equal(1.6f, restored.EncounterWeightMultiplier, 3);
            Assert.True(restored.IsSecondWinterActive);
            Assert.Equal(1208, restored.State.seedSalt);
        }

        [Fact]
        public void SecondWinterMultiplierApplies()
        {
            var enc = Sys();
            enc.Unlock(60);
            Assert.False(enc.IsSecondWinterActive);
            Assert.Equal(1f, enc.EncounterWeightMultiplier, 3);
            enc.SetSecondWinter(1.6f, 400);
            Assert.True(enc.IsSecondWinterActive);
            Assert.Equal(1.6f, enc.EncounterWeightMultiplier, 3);
            enc.ClearSecondWinter();
            Assert.False(enc.IsSecondWinterActive);
            Assert.Equal(1f, enc.EncounterWeightMultiplier, 3);
        }
    }
}
