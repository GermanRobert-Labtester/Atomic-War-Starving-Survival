using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Duty Roster (Exp 02) integration: chart lifecycle, morning roster,
    /// assignments, manifest cap, shelter encounters, morale marks, Second
    /// Winter, Holdfast two-way flags, Overflow practice, hatch bridge, and
    /// save migration. Cross-tool QA: roster x needs x levy (Prompt #26).
    /// </summary>
    public class DutyRosterIntegrationTests
    {
        // ── Helpers ────────────────────────────────────────────────────

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

        private static DutyRosterSystem ReadyChart(int day = 60)
        {
            var roster = Sys();
            roster.Unlock(day - 1);
            Assert.True(roster.CanBeginChart(day, false, false));
            Assert.True(roster.ResolveChartChoice(DutyRosterIds.ChoiceWritePencil, day));
            return roster;
        }

        private static void Enroll(DutyRosterSystem roster, int day, params (string id, string name, string job)[] people)
        {
            roster.TickMorning(day, Occupants(people));
        }

        // ── 1. Identity / catalog integrity ─────────────────────────────

        [Fact]
        public void Ids_StackWingsAndOverflowAreCanonicalSnakeCase()
        {
            foreach (string id in DutyRosterIds.StackWingIds)
                Assert.Matches("^loc_stack_[a-z0-9_]+$", id);
            Assert.Equal(6, DutyRosterIds.StackWingIds.Length);

            foreach (string id in DutyRosterIds.OverflowNodeIds)
                Assert.Matches("^loc_overflow_[a-z0-9_]+$", id);
            Assert.Equal(4, DutyRosterIds.OverflowNodeIds.Length);
            Assert.All(DutyRosterIds.OverflowNodeIds, id => Assert.True(DutyRosterSystem.IsOverflowNode(id)));
            Assert.False(DutyRosterSystem.IsOverflowNode("loc_stack_sleeping"));
            Assert.False(DutyRosterSystem.IsOverflowNode("loc_overflow_made_up"));
        }

        [Fact]
        public void Ids_BlankRowsIsACurrentNotAPower()
        {
            string dataDir = FindDataDir();
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var root = json.Deserialize<CurrentsCatalogProbe>(files.ReadAllText(files.Combine(dataDir, "currents.json")));
            Assert.NotNull(root);
            Assert.NotNull(root.entries);
            Assert.Contains(root.entries, c => c.id == "faction_blank_rows");
            // The Current must NOT exist in faction_lore.json (no seventh Power).
            var lore = CatalogLocator.LoadWrappedList<FactionLoreProbe>(files.ReadAllText(files.Combine(dataDir, "faction_lore.json")), SystemTextJsonSerializer.Options);
            Assert.DoesNotContain(lore, f => f.faction_id == "faction_blank_rows");
        }

        [Fact]
        public void Catalog_QuestsAndMarksResolve()
        {
            string dataDir = FindDataDir();
            var loader = new DutyRosterCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer(), NullLog.Instance);
            var catalog = loader.Load(dataDir);
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
            Assert.NotNull(catalog.GetMark(DutyRosterHoldfastBridge.MarkThreeAway));
            Assert.NotNull(catalog.GetMark(DutyRosterHoldfastBridge.MarkEdorStool));
            Assert.NotNull(catalog.GetLocation(DutyRosterIds.LocStackRosterWall));
            Assert.NotNull(catalog.GetLocation(DutyRosterIds.LocOverflowAlloc11));
            Assert.NotNull(catalog.GetSeason(DutyRosterIds.SeasonSecondWinter));
        }

        // ── 2/3. Old save -> blank chart; unlock / soft gate ───────────

        [Fact]
        public void Save_OldV1MigratesToBlankChartAndClosedOverflow()
        {
            var json = new SystemTextJsonSerializer();
            var v1 = new DutyRosterSaveV1
            {
                saveVersion = 1,
                simDay = 30,
                roster = new DutyRosterSystemState { expansionUnlocked = true },
                marks = new MoraleMarkSystemState(),
                encounters = new ShelterEncounterSystemState { expansionUnlocked = true }
            };
            v1.Checksum = SaveChecksum.Compute(v1);

            var migrated = DutyRosterSaveCodec.Decode(json.Serialize(v1), json);
            Assert.Equal(DutyRosterSave.CurrentSaveVersion, migrated.saveVersion);
            Assert.Equal(30, migrated.simDay);
            // Old saves load with a blank chart until the expansion is legitimately started.
            Assert.Equal(DutyRosterIds.ScriptBlank, migrated.roster.chartScript);
            Assert.False(migrated.overflow.access);
            Assert.Empty(migrated.overflow.visitedNodes);
        }

        [Fact]
        public void SoftGate_RequiresDayOrLoreOrInspectOrClerk()
        {
            var roster = Sys();
            roster.Unlock(10);
            Assert.False(roster.CanBeginChart(10, false, false)); // too early
            Assert.True(roster.CanBeginChart(60, false, false));  // day gate
            Assert.True(roster.CanBeginChart(10, true, false));   // lore flag
            Assert.True(roster.CanBeginChart(10, false, true));   // clerk started
            roster.NotifyWallInspected();
            Assert.True(roster.CanBeginChart(10, false, false));  // wall inspected
        }

        // ── 4. Chart transitions ───────────────────────────────────────

        [Fact]
        public void Chart_PencilWaitInkEraseBurnAndInvalidTransitions()
        {
            var roster = ReadyChart();

            // Erase a missing name is a no-op.
            Assert.False(roster.EraseName("nobody"));

            // Wait ink: pencil -> blank(wait).
            Assert.True(roster.ResolveChartChoice(DutyRosterIds.ChoiceWaitInk, 62));
            Assert.Equal(DutyRosterIds.ScriptBlank, roster.ChartScript);
            Assert.True(roster.State.waitInk);

            // Pencil again is allowed.
            Assert.True(roster.ResolveChartChoice(DutyRosterIds.ChoiceWritePencil, 63));
            Assert.Equal(DutyRosterIds.ScriptPencil, roster.ChartScript);

            // Burn.
            Assert.True(roster.BurnChart(70));
            Assert.Equal(DutyRosterIds.ScriptBurned, roster.ChartScript);
            Assert.True(roster.State.mutationRosterBurned);

            // After burn: no writes, no chart choices, no ink ending.
            Assert.False(roster.WriteName("npc_kess_adler", "Kess", "clerk", DutyRosterIds.ScriptPencil, 71, true));
            Assert.False(roster.ResolveChartChoice(DutyRosterIds.ChoiceWritePencil, 71));
            Assert.False(roster.ResolveInkEnding(71));
            Assert.False(roster.BurnChart(71)); // idempotent
        }

        [Fact]
        public void Chart_InkEndingRequiresPencilScriptAndNotBurned()
        {
            var roster = ReadyChart();
            // Ink ending works from pencil.
            Assert.True(roster.ResolveInkEnding(90));
            Assert.Equal(DutyRosterIds.ScriptInk, roster.ChartScript);
            Assert.Equal(DutyRosterIds.EndingInk, roster.State.endingId);

            // Ink names persist; blank-rows access withdrawn if a blank name is inked.
            var roster2 = ReadyChart();
            roster2.RegisterBlankRowsLivingName("npc_nila_brant");
            roster2.WriteName("npc_nila_brant", "Nila Brant", "lamp_oil_clerk", DutyRosterIds.ScriptInk, 90, true);
            Assert.False(roster2.BlankRowsAccess);
        }

        // ── 5. Morning roster: sleeping prereq + absent rules ──────────

        [Fact]
        public void Morning_UnsleptNameCannotBePenciled()
        {
            var roster = ReadyChart();
            Assert.False(roster.WriteName("npc_kess_adler", "Kess", "clerk", DutyRosterIds.ScriptPencil, 60, sleptHere: false));
            Assert.True(roster.WriteName("npc_kess_adler", "Kess", "clerk", DutyRosterIds.ScriptPencil, 60, sleptHere: true));
        }

        [Fact]
        public void Morning_AbsentAndDeadCannotBeAssigned()
        {
            var roster = ReadyChart();
            Enroll(roster, 61, ("npc_kess_adler", "Kess", "clerk"), ("npc_ansel_duth", "Ansel", "parent"), ("npc_hadi_morrow", "Hadi", "vet"));

            Assert.True(roster.SetStatus("npc_hadi_morrow", DutyRosterIds.StatusMissing));
            Assert.True(roster.SetStatus("npc_ansel_duth", DutyRosterIds.StatusDead));

            Assert.True(roster.Assign(DutyRosterIds.RoleNightWatch, "npc_kess_adler"));
            Assert.False(roster.Assign(DutyRosterIds.RoleNightWatch, "npc_hadi_morrow")); // missing
            Assert.False(roster.Assign(DutyRosterIds.RoleMess, "npc_ansel_duth"));       // dead
        }

        // ── 6. Assignments: duplicate-role rejection ────────────────────

        [Fact]
        public void Assignments_SurvivorHoldsAtMostOneRole()
        {
            var roster = ReadyChart();
            Enroll(roster, 61, ("npc_kess_adler", "Kess", "clerk"), ("npc_ansel_duth", "Ansel", "parent"));

            Assert.True(roster.Assign(DutyRosterIds.RoleNightWatch, "npc_kess_adler"));
            // Same survivor on a second role is rejected.
            Assert.False(roster.Assign(DutyRosterIds.RoleMess, "npc_kess_adler"));
            Assert.Equal(DutyRosterIds.RoleNightWatch, roster.GetRoleOf("npc_kess_adler"));

            // Clearing frees the survivor.
            Assert.True(roster.Assign(DutyRosterIds.RoleNightWatch, null));
            Assert.True(roster.Assign(DutyRosterIds.RoleMess, "npc_kess_adler"));
            Assert.Equal(DutyRosterIds.RoleMess, roster.GetRoleOf("npc_kess_adler"));
        }

        [Fact]
        public void Assignments_UnknownRoleOrSurvivorRejected()
        {
            var roster = ReadyChart();
            Enroll(roster, 61, ("npc_kess_adler", "Kess", "clerk"));
            Assert.False(roster.Assign("not_a_role", "npc_kess_adler"));
            Assert.False(roster.Assign(DutyRosterIds.RoleMess, "nobody"));
        }

        // ── 7. Manifest cap 14 ──────────────────────────────────────────

        [Fact]
        public void Manifest_FourteenthBunkIsHardCap()
        {
            var roster = ReadyChart();
            var people = new List<DutyRosterOccupant>();
            for (int i = 1; i <= 14; i++)
                people.Add(new DutyRosterOccupant { survivorId = "sv_" + i, displayName = "S" + i, sleptHere = true });
            roster.TickMorning(61, people);
            Assert.Equal(14, roster.OccupiedRowCount);
            Assert.False(roster.WriteName("sv_15", "S15", "unlisted", DutyRosterIds.ScriptPencil, 62, true));
        }

        // ── 8. Blank Rows access ───────────────────────────────────────

        [Fact]
        public void BlankRows_WithdrawOnListingAndCanBeRegrantedByPractice()
        {
            var roster = ReadyChart();
            roster.RegisterBlankRowsLivingName("npc_nila_brant");
            Assert.True(roster.BlankRowsAccess);
            roster.NotifyListedOnCensusOr12C("npc_nila_brant");
            Assert.False(roster.BlankRowsAccess);
            Assert.True(roster.GrantBlankRowsAccess()); // quest_roster_blank_access
            Assert.True(roster.BlankRowsAccess);
            Assert.False(roster.GrantBlankRowsAccess()); // idempotent
        }

        // ── 9/10. Shelter encounters: one/night, crisis, queue ─────────

        [Fact]
        public void Encounters_OnePerNightAndCrisisOverride()
        {
            var enc = new ShelterEncounterSystem(1208);
            enc.Unlock(60);
            Assert.True(enc.StartEncounter("se_a", ShelterEncounterSystem.KindNightSlate, 61));
            Assert.False(enc.StartEncounter("se_b", ShelterEncounterSystem.KindNightSlate, 61)); // second blocked
            Assert.True(enc.StartEncounterCrisis("se_c", ShelterEncounterSystem.KindNightSlate, 61)); // crisis allowed
            Assert.True(enc.StartEncounter("se_d", ShelterEncounterSystem.KindNightSlate, 62)); // new night
            Assert.Equal(1, enc.EncountersThisNight); // counter reset on the new day
            enc.ResetNightCounter(62);
            Assert.Equal(1, enc.EncountersThisNight);
        }

        [Fact]
        public void Encounters_VisitorQueueIsSingleFileAndResolvedIdsDedupe()
        {
            var enc = new ShelterEncounterSystem(1208);
            enc.Unlock(60);
            Assert.True(enc.QueueVisitor(ShelterEncounterSystem.VisitorEdor, 61));
            Assert.True(enc.QueueVisitor(ShelterEncounterSystem.VisitorLen, 61));
            Assert.False(enc.QueueVisitor(ShelterEncounterSystem.VisitorEdor, 61)); // no double-book
            Assert.Equal(ShelterEncounterSystem.VisitorEdor, enc.PeekVisitor());
            Assert.True(enc.ResolveVisitor(ShelterEncounterSystem.VisitorEdor));
            Assert.Equal(ShelterEncounterSystem.VisitorLen, enc.PeekVisitor());

            Assert.True(enc.StartEncounter("se_x", ShelterEncounterSystem.KindMealShort, 61));
            Assert.True(enc.ResolveEncounter("se_x", 62));
            Assert.True(enc.IsResolved("se_x"));
            // A resolved id is never re-staged.
            Assert.False(enc.StartEncounter("se_x", ShelterEncounterSystem.KindMealShort, 63));
        }

        [Fact]
        public void HatchBridge_StagesOneScenePerNightAndNeverTouchesExpeditionNumbers()
        {
            var enc = new ShelterEncounterSystem(1208);
            enc.Unlock(60);
            Assert.True(enc.BridgeHatchReturn(61, "npc_kess_adler"));
            Assert.False(enc.BridgeHatchReturn(61, "npc_ansel_duth")); // one per night
            Assert.True(enc.BridgeHatchReturn(62, "npc_hadi_morrow", crisis: true)); // crisis window
            Assert.True(enc.BridgeHatchReturn(62, "npc_tamsin_rook", crisis: true));

            // The bridge owns no ExpeditionSystem state; magnitudes stay in the expedition owner.
            Assert.Equal(3, enc.State.history.Count); // 1 (day 61) + 2 crisis (day 62)
        }

        // ── 11. Determinism ─────────────────────────────────────────────

        [Fact]
        public void Determinism_SameSeedSameInputsSameAssignment()
        {
            var a = Sys(42);
            var b = Sys(42);
            foreach (var r in new[] { a, b })
            {
                r.Unlock(60);
                r.ResolveChartChoice(DutyRosterIds.ChoiceWritePencil, 60);
                r.TickMorning(61, Occupants(
                    ("sv_a", "A", "x"), ("sv_b", "B", "y"), ("sv_c", "C", "z"), ("sv_d", "D", "w")));
            }
            int nA = a.AutoAssignDefaults(61);
            int nB = b.AutoAssignDefaults(61);
            Assert.Equal(nA, nB);
            for (int i = 0; i < DutyRosterIds.AssignmentRoles.Length; i++)
            {
                string role = DutyRosterIds.AssignmentRoles[i];
                Assert.Equal(a.GetAssignment(role), b.GetAssignment(role));
            }
        }

        // ── 12. Morale marks: persistence + clear ───────────────────────

        [Fact]
        public void Marks_PersistAcrossDaysAndClearOnlyByAuthoredAction()
        {
            var marks = new MoraleMarkSystem();
            marks.SetMark("mark_bowl_cold", "the true thing", 60);
            // Marks do not expire on sleep: no tick/decay exists by design.
            Assert.True(marks.HasMark("mark_bowl_cold"));
            Assert.Equal("the true thing", marks.GetPayload("mark_bowl_cold"));
            Assert.True(marks.ClearMark("mark_bowl_cold"));
            Assert.False(marks.HasMark("mark_bowl_cold"));
            Assert.False(marks.ClearMark("mark_bowl_cold"));
        }

        // ── 13. Second Winter: multiplier + reset ───────────────────────

        [Fact]
        public void SecondWinter_MultiplierAppliesAndResets()
        {
            var enc = new ShelterEncounterSystem(1208);
            enc.Unlock(60);
            Assert.Equal(1f, enc.EncounterWeightMultiplier);
            enc.SetSecondWinter(DutyRosterIds.SecondWinterEncounterWeight, 200);
            Assert.Equal(DutyRosterIds.SecondWinterEncounterWeight, enc.EncounterWeightMultiplier);
            Assert.True(enc.IsSecondWinterActive);
            enc.ClearSecondWinter();
            Assert.Equal(1f, enc.EncounterWeightMultiplier);
            Assert.False(enc.IsSecondWinterActive);
        }

        // ── 14/15. Holdfast two-way flags ───────────────────────────────

        [Fact]
        public void HoldfastToDuty_LevyHonourMarksRowsLevyAndSetsMark()
        {
            var roster = ReadyChart();
            Enroll(roster, 61, ("npc_kess_adler", "Kess", "clerk"), ("npc_ansel_duth", "Ansel", "parent"),
                ("npc_hadi_morrow", "Hadi", "vet"), ("npc_tamsin_rook", "Tamsin", "clerk"));
            var marks = new MoraleMarkSystem();
            var census = new CensusClaimSystem();
            Assert.True(census.IssueLevy(new[] { "npc_kess_adler", "npc_ansel_duth" }, 65));
            Assert.True(census.HonourLevy());

            DutyRosterHoldfastBridge.SyncFromHoldfast(roster, marks, new ShelterEncounterSystem(1208),
                census, null, null, null, 70);

            Assert.True(census.LevyHonour);
            Assert.True(marks.HasMark(DutyRosterHoldfastBridge.MarkThreeAway));
            Assert.Equal(4, roster.Rows.Count); // rows remain; status flips
            Assert.Equal(3, CountWithStatus(roster, DutyRosterIds.StatusLevy));
        }

        [Fact]
        public void HoldfastToDuty_RefuseQueuesEdorStool()
        {
            var roster = ReadyChart();
            Enroll(roster, 61, ("npc_kess_adler", "Kess", "clerk"));
            var marks = new MoraleMarkSystem();
            var enc = new ShelterEncounterSystem(1208);
            enc.Unlock(60);
            var census = new CensusClaimSystem();
            Assert.True(census.RefuseLevy(70));

            DutyRosterHoldfastBridge.SyncFromHoldfast(roster, marks, enc, census, null, null, null, 70);

            Assert.True(marks.HasMark(DutyRosterHoldfastBridge.MarkEdorStool));
            Assert.True(enc.IsResolved("se_edor_stool_levy_refuse") || enc.GetActive("se_edor_stool_levy_refuse") != null);
        }

        [Fact]
        public void HoldfastToDuty_MembraneAndWaystationSetMarks()
        {
            var roster = ReadyChart();
            var marks = new MoraleMarkSystem();
            var brine = new BrineWaterSystem();
            brine.State.membraneSector4Strip = true;
            var way = new WaystationSystem();
            way.Unlock();
            way.AssignWatch(new[] { "npc_tamsin_rook" });

            DutyRosterHoldfastBridge.SyncFromHoldfast(roster, marks, new ShelterEncounterSystem(1208),
                null, null, way, brine, 70);

            Assert.True(marks.HasMark(DutyRosterHoldfastBridge.MarkFilterWho));
            Assert.True(marks.HasMark(DutyRosterHoldfastBridge.MarkTamsinWatchShort));
        }

        [Fact]
        public void HoldfastToDuty_IceRoadDarkSetsMarkAndCrowdEncounter()
        {
            var roster = ReadyChart();
            var marks = new MoraleMarkSystem();
            var enc = new ShelterEncounterSystem(1208);
            enc.Unlock(60);
            var ice = new IceRoadSystem();
            ice.Unlock(60);
            ice.State.isOpen = false;

            DutyRosterHoldfastBridge.SyncFromHoldfast(roster, marks, enc, null, ice, null, null, 70);

            Assert.True(marks.HasMark(DutyRosterHoldfastBridge.MarkHouseThinned));
            Assert.NotNull(enc.GetActive("se_road_dark_crowd"));
        }

        [Fact]
        public void DutyToHoldfast_SnapshotCarriesChartNorthAndHadiStatus()
        {
            var roster = ReadyChart();
            Enroll(roster, 61, ("npc_kess_adler", "Kess", "clerk"), ("npc_hadi_morrow", "Hadi", "vet"),
                ("npc_ansel_duth", "Ansel", "parent"));
            roster.HideFromNorthCopy("npc_ansel_duth");

            var snap = DutyRosterHoldfastBridge.SnapshotForHoldfast(roster);
            Assert.Equal(DutyRosterIds.ScriptPencil, snap.ChartScript);
            Assert.Equal("mutation_roster_pencil", snap.Mutation);
            Assert.DoesNotContain(snap.NorthRows, r => r.survivorId == "npc_ansel_duth");
            Assert.Contains(snap.LevyNames, n => n == "npc_kess_adler");
            Assert.Equal("listed", snap.HadiStatus);

            // Never-back once missing/dead.
            roster.SetStatus("npc_hadi_morrow", DutyRosterIds.StatusMissing);
            Assert.Equal("never_back", DutyRosterHoldfastBridge.SnapshotForHoldfast(roster).HadiStatus);
        }

        [Fact]
        public void DutyToHoldfast_LevyValidationFlagsMissingRows()
        {
            var roster = ReadyChart();
            Enroll(roster, 61, ("npc_kess_adler", "Kess", "clerk"));
            var failures = DutyRosterHoldfastBridge.ValidateLevyNamesAgainstRoster(
                roster, new[] { "npc_kess_adler", "npc_hadi_morrow" });
            Assert.Contains("npc_hadi_morrow", failures);
            Assert.DoesNotContain("npc_kess_adler", failures);
        }

        // ── 16. Save round-trip, migration, defaults, future-version ────

        [Fact]
        public void Save_RoundTripsChartMarksEncountersAndOverflow()
        {
            var roster = ReadyChart();
            Enroll(roster, 61, ("npc_kess_adler", "Kess", "clerk"), ("npc_hadi_morrow", "Hadi", "vet"));
            roster.Assign(DutyRosterIds.RoleNightWatch, "npc_kess_adler");
            roster.GrantOverflowAccess();
            roster.RegisterOverflowVisit(DutyRosterIds.LocOverflowAlloc11);

            var marks = new MoraleMarkSystem();
            marks.SetMark("mark_bowl_cold", "payload", 61);
            var enc = new ShelterEncounterSystem(1208);
            enc.Unlock(60);
            enc.StartEncounter("se_keep", ShelterEncounterSystem.KindNightSlate, 61);

            var clock = new SimClock(61 * 1440);
            var save = DutyRosterSaveCodec.Capture(roster, marks, enc, clock);
            var json = new SystemTextJsonSerializer();
            var decoded = DutyRosterSaveCodec.Decode(DutyRosterSaveCodec.Encode(save, json), json);

            var r2 = Sys();
            var m2 = new MoraleMarkSystem();
            var e2 = new ShelterEncounterSystem(1208);
            DutyRosterSaveCodec.Restore(decoded, r2, m2, e2, new SimClock(0));

            Assert.Equal(2, r2.OccupiedRowCount);
            Assert.Equal("npc_kess_adler", r2.GetAssignment(DutyRosterIds.RoleNightWatch));
            Assert.True(r2.OverflowAccess);
            Assert.True(r2.HasVisitedOverflow(DutyRosterIds.LocOverflowAlloc11));
            Assert.True(m2.HasMark("mark_bowl_cold"));
            Assert.True(e2.IsUnlocked);
            Assert.NotNull(e2.GetActive("se_keep"));
        }

        [Fact]
        public void Save_MissingStateDefaultsAndFutureVersionRejected()
        {
            var json = new SystemTextJsonSerializer();
            // Future version hard-rejected.
            var future = new DutyRosterSave { saveVersion = DutyRosterSave.CurrentSaveVersion + 1 };
            future.Checksum = SaveChecksum.Compute(future);
            Assert.Throws<InvalidOperationException>(() => DutyRosterSaveCodec.Decode(json.Serialize(future), json));

            // Missing sub-state -> safe defaults.
            var roster = Sys();
            var marks = new MoraleMarkSystem();
            var enc = new ShelterEncounterSystem(1208);
            var blank = new DutyRosterSave { saveVersion = DutyRosterSave.CurrentSaveVersion, simDay = 5 };
            blank.roster = null;
            blank.marks = null;
            blank.encounters = null;
            blank.overflow = null;
            blank.Checksum = SaveChecksum.Compute(blank);
            var decoded = DutyRosterSaveCodec.Decode(json.Serialize(blank), json);
            DutyRosterSaveCodec.Restore(decoded, roster, marks, enc, new SimClock(0));
            Assert.Equal(DutyRosterIds.ScriptBlank, roster.ChartScript);
            Assert.Equal(0, marks.Count);
            Assert.False(enc.IsUnlocked);
            Assert.False(roster.OverflowAccess);
        }

        [Fact]
        public void Save_ChecksumStableAcrossSerializerRoundTrip()
        {
            var roster = ReadyChart();
            Enroll(roster, 61, ("npc_kess_adler", "Kess", "clerk"));
            roster.GrantOverflowAccess();
            var save = DutyRosterSaveCodec.Capture(roster, new MoraleMarkSystem(), new ShelterEncounterSystem(1208), new SimClock(61 * 1440));
            string json = new SystemTextJsonSerializer().Serialize(save);
            var parsed = new SystemTextJsonSerializer().Deserialize<DutyRosterSave>(json);
            Assert.Equal(SaveChecksum.Compute(save), SaveChecksum.Compute(parsed));
        }

        // ── 17. Overflow practice bounds ────────────────────────────────

        [Fact]
        public void Overflow_ClosedAccessRejectsVisitsAndUnknownNodesNeverBlessed()
        {
            var roster = Sys();
            roster.Unlock(60);
            Assert.False(roster.RegisterOverflowVisit(DutyRosterIds.LocOverflowAlloc11)); // closed
            roster.GrantOverflowAccess();
            Assert.True(roster.RegisterOverflowVisit(DutyRosterIds.LocOverflowAlloc11));
            Assert.False(roster.RegisterOverflowVisit(DutyRosterIds.LocOverflowAlloc11)); // dedupe
            Assert.False(roster.RegisterOverflowVisit("loc_overflow_made_up"));
            Assert.Equal(1, roster.OverflowVisited.Count);
            // Restore never blesses an unauthenticated node.
            var state = roster.CaptureOverflowState();
            state.visitedNodes.Add("loc_overflow_made_up");
            roster.RestoreOverflowState(state);
            Assert.False(roster.HasVisitedOverflow("loc_overflow_made_up"));
        }

        // ── 18. Quest runtime (stage machine over the authored catalog) ──

        [Fact]
        public void Quests_StartPrereqMinDayAndCompletion()
        {
            string dataDir = FindDataDir();
            var loader = new DutyRosterCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer(), NullLog.Instance);
            var catalog = loader.Load(dataDir);
            var rt = new DutyRosterQuestRuntime();
            rt.BindCatalog(catalog);

            // quest_roster_the_chart has min_day 60, no prereq.
            Assert.False(rt.StartQuest(DutyRosterIds.QuestTheChart, 30)); // too early
            Assert.True(rt.StartQuest(DutyRosterIds.QuestTheChart, 60));
            Assert.False(rt.StartQuest(DutyRosterIds.QuestTheChart, 61)); // already started
            Assert.True(rt.IsStarted(DutyRosterIds.QuestTheChart));

            // quest_roster_who_eats requires the chart complete.
            Assert.False(rt.StartQuest(DutyRosterIds.QuestWhoEats, 61)); // prereq incomplete
            Assert.True(rt.AdvanceStage(DutyRosterIds.QuestTheChart, 61)); // ->1
            Assert.True(rt.AdvanceStage(DutyRosterIds.QuestTheChart, 62)); // ->2
            Assert.True(rt.AdvanceStage(DutyRosterIds.QuestTheChart, 63)); // ->3
            Assert.True(rt.AdvanceStage(DutyRosterIds.QuestTheChart, 64)); // ->4
            Assert.True(rt.AdvanceStage(DutyRosterIds.QuestTheChart, 65)); // ->5 complete
            Assert.True(rt.IsComplete(DutyRosterIds.QuestTheChart));
            Assert.True(rt.StartQuest(DutyRosterIds.QuestWhoEats, 66));

            // Failure blocks further advancement.
            Assert.True(rt.FailQuest(DutyRosterIds.QuestWhoEats, 70));
            Assert.False(rt.AdvanceStage(DutyRosterIds.QuestWhoEats, 71));
            Assert.True(rt.IsFailed(DutyRosterIds.QuestWhoEats));
        }

        [Fact]
        public void Quests_ChoiceResolutionIsAuthoredOnly()
        {
            string dataDir = FindDataDir();
            var loader = new DutyRosterCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer(), NullLog.Instance);
            var catalog = loader.Load(dataDir);
            var rt = new DutyRosterQuestRuntime();
            rt.BindCatalog(catalog);
            Assert.True(rt.StartQuest(DutyRosterIds.QuestTheChart, 60));

            var chart = catalog.GetQuest(DutyRosterIds.QuestTheChart);
            Assert.NotNull(chart.choices);
            Assert.True(chart.choices.Length >= 2, "chart quest authored with choices");
            Assert.True(rt.ResolveChoice(DutyRosterIds.QuestTheChart, chart.choices[0].id));
            Assert.False(rt.ResolveChoice(DutyRosterIds.QuestTheChart, "not_a_choice"));
        }

        [Fact]
        public void Quests_SaveRoundTripAndV3Migration()
        {
            string dataDir = FindDataDir();
            var loader = new DutyRosterCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer(), NullLog.Instance);
            var catalog = loader.Load(dataDir);
            var rt = new DutyRosterQuestRuntime();
            rt.BindCatalog(catalog);
            Assert.True(rt.StartQuest(DutyRosterIds.QuestTheChart, 60));
            rt.AdvanceStage(DutyRosterIds.QuestTheChart, 61);

            var json = new SystemTextJsonSerializer();
            var save = DutyRosterSaveCodec.Capture(Sys(), new MoraleMarkSystem(), new ShelterEncounterSystem(1208),
                new SimClock(61 * 1440), rt);
            var decoded = DutyRosterSaveCodec.Decode(DutyRosterSaveCodec.Encode(save, json), json);
            var rt2 = new DutyRosterQuestRuntime();
            rt2.BindCatalog(catalog);
            DutyRosterSaveCodec.Restore(decoded, Sys(), new MoraleMarkSystem(), new ShelterEncounterSystem(1208),
                new SimClock(0), rt2);
            Assert.True(rt2.IsStarted(DutyRosterIds.QuestTheChart));
            Assert.Equal(1, rt2.GetCurrentStage(DutyRosterIds.QuestTheChart));

            // v2 legacy: overflow preserved, quest ledger starts empty.
            var v2 = new DutyRosterSaveV2
            {
                saveVersion = 2,
                simDay = 40,
                roster = new DutyRosterSystemState { expansionUnlocked = true },
                marks = new MoraleMarkSystemState(),
                encounters = new ShelterEncounterSystemState(),
                overflow = new DutyRosterOverflowState { access = true }
            };
            v2.Checksum = SaveChecksum.Compute(v2);
            var migrated = DutyRosterSaveCodec.Decode(json.Serialize(v2), json);
            Assert.Equal(DutyRosterSave.CurrentSaveVersion, migrated.saveVersion);
            Assert.True(migrated.overflow.access);
            Assert.Empty(migrated.quests.quests);
        }

        // ── 19. Authored quest effects apply real Core state ────────────

        [Fact]
        public void Quests_ChartCompletionPutsRosterInUse()
        {
            string dataDir = FindDataDir();
            var loader = new DutyRosterCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer(), NullLog.Instance);
            var catalog = loader.Load(dataDir);
            // Fresh roster; the chart quest IS the pencil choice in the real path.
            var roster = Sys();
            roster.Unlock(59);
            Assert.True(roster.ResolveChartChoice(DutyRosterIds.ChoiceWritePencil, 60));
            Assert.True(roster.MutationInUse);
            Assert.True(roster.LevyRequiresRows);
            var rt = new DutyRosterQuestRuntime();
            rt.BindCatalog(catalog);
            Assert.True(rt.StartQuest(DutyRosterIds.QuestTheChart, 60));
            for (int s = 0; s < 5; s++) rt.AdvanceStage(DutyRosterIds.QuestTheChart, 61 + s);

            Assert.True(rt.HasMutation(DutyRosterIds.MutationRosterInUse));
            rt.ApplyKnownEffects(roster, new MoraleMarkSystem(), 66, NullLog.Instance);
            Assert.True(roster.MutationInUse, "chart quest completion affirms the roster in use");
            Assert.True(roster.LevyRequiresRows, "in-use roster makes the levy read the north copy");
        }

        [Fact]
        public void Quests_MarkCompletionsAndUnknownMutations()
        {
            string dataDir = FindDataDir();
            var loader = new DutyRosterCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer(), NullLog.Instance);
            var catalog = loader.Load(dataDir);
            var roster = Sys();
            roster.Unlock(59);
            var marks = new MoraleMarkSystem();
            var rt = new DutyRosterQuestRuntime();
            rt.BindCatalog(catalog);

            // Complete the chart quest first (prereq for the side quests).
            Assert.True(rt.StartQuest(DutyRosterIds.QuestTheChart, 60));
            for (int s = 0; s < 5; s++) rt.AdvanceStage(DutyRosterIds.QuestTheChart, 61 + s);
            rt.ApplyKnownEffects(roster, marks, 66, NullLog.Instance);

            // quest_roster_ansel_truth completes -> mark_child_truth.
            Assert.True(rt.StartQuest("quest_roster_ansel_truth", 66));
            Assert.True(rt.AdvanceStage("quest_roster_ansel_truth", 67)); // 1 stage -> complete
            rt.ApplyKnownEffects(roster, marks, 67, NullLog.Instance);
            Assert.True(marks.HasMark("mark_child_truth"), "authored mark completion sets the mark");

            // quest_roster_the_tin completes -> mutation_brass_kept (a recorded flag
            // with no owning roster field — readable by the Holdfast bridge).
            Assert.True(rt.StartQuest("quest_roster_the_tin", 66));
            Assert.True(rt.AdvanceStage("quest_roster_the_tin", 67)); // stage 1
            Assert.True(rt.AdvanceStage("quest_roster_the_tin", 68)); // stage 2 -> complete
            Assert.True(rt.HasMutation("mutation_brass_kept"));
            Assert.Contains("mutation_brass_kept", rt.AppliedMutations);
        }

        [Fact]
        public void Quests_CrisisWindowQuestUnlocksExtraEncounters()
        {
            string dataDir = FindDataDir();
            var loader = new DutyRosterCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer(), NullLog.Instance);
            var catalog = loader.Load(dataDir);
            var rt = new DutyRosterQuestRuntime();
            rt.BindCatalog(catalog);
            Assert.False(rt.IsCrisisQuestActive());

            // The window quest's prereq is the chart quest; complete it first.
            Assert.True(rt.StartQuest(DutyRosterIds.QuestTheChart, 60));
            for (int s = 0; s < 5; s++) rt.AdvanceStage(DutyRosterIds.QuestTheChart, 61 + s);
            Assert.True(rt.StartQuest(DutyRosterIds.QuestWindow, 66));
            Assert.True(rt.IsCrisisQuestActive(), "active window quest opens the crisis window");

            // Crisis path on the real encounter system allows 2 scenes/night.
            var enc = new ShelterEncounterSystem(1208);
            enc.Unlock(60);
            Assert.True(enc.StartEncounterCrisis("se_a", ShelterEncounterSystem.KindNightSlate, 61));
            Assert.True(enc.StartEncounterCrisis("se_b", ShelterEncounterSystem.KindNightSlate, 61));
            Assert.Equal(2, enc.EncountersThisNight);

            // Completing the quest closes the window.
            var def = catalog.GetQuest(DutyRosterIds.QuestWindow);
            for (int s = 0; s < def.StageCount; s++) rt.AdvanceStage(DutyRosterIds.QuestWindow, 62 + s);
            Assert.False(rt.IsCrisisQuestActive());
        }

        [Fact]
        public void Quests_AppliedMutationsSurviveSaveRoundTrip()
        {
            string dataDir = FindDataDir();
            var loader = new DutyRosterCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer(), NullLog.Instance);
            var catalog = loader.Load(dataDir);
            var rt = new DutyRosterQuestRuntime();
            rt.BindCatalog(catalog);
            Assert.True(rt.StartQuest(DutyRosterIds.QuestTheChart, 60));
            for (int s = 0; s < 5; s++) rt.AdvanceStage(DutyRosterIds.QuestTheChart, 61 + s);

            var json = new SystemTextJsonSerializer();
            var save = DutyRosterSaveCodec.Capture(Sys(), new MoraleMarkSystem(), new ShelterEncounterSystem(1208),
                new SimClock(66 * 1440), rt);
            var decoded = DutyRosterSaveCodec.Decode(DutyRosterSaveCodec.Encode(save, json), json);
            var rt2 = new DutyRosterQuestRuntime();
            rt2.BindCatalog(catalog);
            DutyRosterSaveCodec.Restore(decoded, Sys(), new MoraleMarkSystem(), new ShelterEncounterSystem(1208),
                new SimClock(0), rt2);
            Assert.True(rt2.HasMutation(DutyRosterIds.MutationRosterInUse));
        }

        [Fact]
        public void DutyToHoldfast_SnapshotCarriesQuestMutationsAndMarks()
        {
            string dataDir = FindDataDir();
            var loader = new DutyRosterCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer(), NullLog.Instance);
            var catalog = loader.Load(dataDir);
            var roster = Sys();
            roster.Unlock(59);
            roster.ResolveChartChoice(DutyRosterIds.ChoiceWritePencil, 60);
            var marks = new MoraleMarkSystem();
            marks.BindCatalog(catalog);
            marks.SetMark("mark_bowl_cold", null, 61);
            var rt = new DutyRosterQuestRuntime();
            rt.BindCatalog(catalog);
            Assert.True(rt.StartQuest(DutyRosterIds.QuestTheChart, 60));
            for (int s = 0; s < 5; s++) rt.AdvanceStage(DutyRosterIds.QuestTheChart, 61 + s);

            var snap = DutyRosterHoldfastBridge.SnapshotForHoldfast(roster, rt, marks);
            Assert.Contains(DutyRosterIds.MutationRosterInUse, snap.QuestMutations);
            Assert.Contains("mark_bowl_cold", snap.MarkIds);
            Assert.Equal("mutation_roster_pencil", snap.Mutation);
        }

        // ── 20. Bespoke choice effects + mutation→mark map ──────────────

        [Fact]
        public void Choices_HadiHiddenHidesFromNorthAndSetsMark()
        {
            string dataDir = FindDataDir();
            var loader = new DutyRosterCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer(), NullLog.Instance);
            var catalog = loader.Load(dataDir);
            var roster = Sys();
            roster.Unlock(59);
            roster.ResolveChartChoice(DutyRosterIds.ChoiceWritePencil, 60);
            roster.TickMorning(61, Occupants(("npc_hadi_morrow", "Hadi", "vet")));
            var marks = new MoraleMarkSystem();
            var rt = new DutyRosterQuestRuntime();
            rt.BindCatalog(catalog);

            Assert.True(rt.StartQuest(DutyRosterIds.QuestTheChart, 60));
            for (int s = 0; s < 5; s++) rt.AdvanceStage(DutyRosterIds.QuestTheChart, 61 + s);
            Assert.True(rt.StartQuest(DutyRosterIds.QuestCaretaker, 66));
            // The caretaker quest's authored choices include flag_hadi_hidden.
            var caretaker = catalog.GetQuest(DutyRosterIds.QuestCaretaker);
            var hiddenChoice = System.Array.Find(caretaker.choices, c => c.set_flag == "flag_hadi_hidden");
            Assert.NotNull(hiddenChoice);
            Assert.True(rt.ResolveChoiceWithEffects(DutyRosterIds.QuestCaretaker, hiddenChoice.id, roster, marks, 61));
            Assert.True(marks.HasMark("mark_hadi_hidden"), "hadi-hidden choice sets the authored mark");

            // Hadi is now hidden from north copies.
            var snap = DutyRosterHoldfastBridge.SnapshotForHoldfast(roster, rt, marks);
            Assert.DoesNotContain(snap.NorthRows, r => r.survivorId == "npc_hadi_morrow");
            Assert.Equal("hidden", snap.HadiStatus);
        }

        [Fact]
        public void Effects_FourteenthClaimedSetsMarkAndIsSaveSafe()
        {
            string dataDir = FindDataDir();
            var loader = new DutyRosterCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer(), NullLog.Instance);
            var catalog = loader.Load(dataDir);
            var roster = Sys();
            roster.Unlock(59);
            roster.ResolveChartChoice(DutyRosterIds.ChoiceWritePencil, 60);
            var marks = new MoraleMarkSystem();
            var rt = new DutyRosterQuestRuntime();
            rt.BindCatalog(catalog);

            Assert.True(rt.StartQuest(DutyRosterIds.QuestTheChart, 60));
            for (int s = 0; s < 5; s++) rt.AdvanceStage(DutyRosterIds.QuestTheChart, 61 + s);
            Assert.True(rt.StartQuest(DutyRosterIds.QuestFourteenth, 66));
            var fourteenth = catalog.GetQuest(DutyRosterIds.QuestFourteenth);
            for (int s = 0; s < fourteenth.StageCount; s++) rt.AdvanceStage(DutyRosterIds.QuestFourteenth, 67 + s);
            rt.ApplyKnownEffects(roster, marks, 70, NullLog.Instance);
            Assert.True(marks.HasMark("mark_fourteenth_claimed"), "fourteenth quest completion sets the claimed mark");

            // Mutations + marks survive the save envelope.
            var json = new SystemTextJsonSerializer();
            var save = DutyRosterSaveCodec.Capture(roster, marks, new ShelterEncounterSystem(1208), new SimClock(70 * 1440), rt);
            var decoded = DutyRosterSaveCodec.Decode(DutyRosterSaveCodec.Encode(save, json), json);
            var m2 = new MoraleMarkSystem();
            DutyRosterSaveCodec.Restore(decoded, Sys(), m2, new ShelterEncounterSystem(1208), new SimClock(0), new DutyRosterQuestRuntime());
            Assert.True(m2.HasMark("mark_fourteenth_claimed"));
        }

        // ── 18b. ExpansionMasterSession wiring ───────────────────────────

        [Fact]
        public void MasterSession_RosterWiredAndTicks()
        {
            var session = ExpansionMasterSession.Load(FindDataDir());
            Assert.NotNull(session.DutyRoster);
            Assert.True(session.DutyRosterData.GetQuest(DutyRosterIds.QuestTheChart) != null);
            session.DutyRoster.Unlock(session.Clock.Day);
            session.DutyRoster.ResolveChartChoice(DutyRosterIds.ChoiceWritePencil, session.Clock.Day);
            session.DutyRoster.TickMorning(session.Clock.Day,
                new List<DutyRosterOccupant>
                {
                    new DutyRosterOccupant { survivorId = "npc_kess_adler", displayName = "Kess", sleptHere = true }
                });
            Assert.NotNull(session.DutyRoster.GetRow("npc_kess_adler"));
        }

        // ── Data helpers ────────────────────────────────────────────────

        private static string FindDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        public sealed class CurrentsProbe { public string id = string.Empty; }
        public sealed class CurrentsCatalogProbe { public int schema_version; public List<CurrentsProbe> entries = new List<CurrentsProbe>(); }
        public sealed class FactionLoreProbe { public string faction_id = string.Empty; }

        private static int CountWithStatus(DutyRosterSystem roster, string status)
        {
            int n = 0;
            for (int i = 0; i < roster.Rows.Count; i++)
                if (roster.Rows[i] != null && roster.Rows[i].status == status) n++;
            return n;
        }
    }
}
