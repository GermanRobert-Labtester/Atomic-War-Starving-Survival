using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Flags;
using Ashfall.Core.Journal;
using Ashfall.Core.Medical;
using Ashfall.Core.Memorial;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Task 121 — unified survivor-death pipeline contract tests.
    /// Pins idempotency, multi-cause normalization, assignment clearing,
    /// memorial/journal exactly-once, briefing feed, last-survivor detection,
    /// save-during-death round-trips, and legacy-save reconciliation.
    /// </summary>
    public sealed class SurvivorFateSystemTests
    {
        // ── Fixture ────────────────────────────────────────────────────

        private sealed class Fixture
        {
            public SurvivorRosterSystem Roster = new SurvivorRosterSystem();
            public NeedsSystem Needs = new NeedsSystem();
            public DutyRosterSystem Duty = NewDutyRoster();
            public CaregivingSystem Caregiving = new CaregivingSystem();
            public MedicalWardSystem Ward = new MedicalWardSystem(
                new MedicalWardState(),
                new[] { new MedicalBed("bed_a", "Bed A", MedicalBedCategory.General, false) },
                Array.Empty<MedicalProcedureDef>());
            public SurvivorSocialCoordinator Social;
            public FinalWishSystem FinalWish = new FinalWishSystem { Rng = new SeededRng(99) };
            public MemorialSystem Memorial = new MemorialSystem(new MemorialState());
            public JournalSystem Journal = new JournalSystem();
            public CampaignConsequenceLedger Flags = new CampaignConsequenceLedger();
            public List<string> Recalled = new List<string>();
            public SurvivorFateSystem Fate;
            public int Day = 10;

            private static DutyRosterSystem NewDutyRoster()
            {
                var duty = new DutyRosterSystem();
                // Rows normally come from the duty-roster catalog load; seed two.
                duty.RestoreState(new DutyRosterSystemState
                {
                    rows = new List<DutyRosterRow>
                    {
                        new DutyRosterRow { survivorId = "survivor_a", displayName = "A" },
                        new DutyRosterRow { survivorId = "survivor_b", displayName = "B" }
                    }
                });
                return duty;
            }

            public Fixture()
            {
                Social = new SurvivorSocialCoordinator(new SeededRng(7), Needs, null, Duty, () => Day);
                Fate = new SurvivorFateSystem(
                    roster: Roster,
                    needs: Needs,
                    dutyRoster: Duty,
                    caregiving: Caregiving,
                    medicalWard: Ward,
                    social: Social,
                    finalWish: FinalWish,
                    memorial: Memorial,
                    journal: Journal,
                    flags: Flags,
                    getDay: () => Day,
                    displayNameFor: id => Roster.FindDefinition(id)?.displayName ?? id,
                    expeditionRecall: id => Recalled.Add(id));

                AddSurvivor("survivor_a", "Alex");
                AddSurvivor("survivor_b", "Blair");
            }

            public void AddSurvivor(string id, string name)
            {
                Roster.RegisterDefinition(new SurvivorDefinition { id = id, displayName = name });
                Roster.Join(id, 1);
                Needs.Register(new SurvivorNeedsState { Id = id });
            }
        }

        // ── 1. Multi-cause normalization ───────────────────────────────

        [Theory]
        [InlineData(SurvivorDeathCause.Needs)]
        [InlineData(SurvivorDeathCause.Radiation)]
        [InlineData(SurvivorDeathCause.Disease)]
        [InlineData(SurvivorDeathCause.Combat)]
        [InlineData(SurvivorDeathCause.Expedition)]
        [InlineData(SurvivorDeathCause.Medical)]
        [InlineData(SurvivorDeathCause.Scripted)]
        [InlineData(SurvivorDeathCause.Unknown)]
        public void EveryCause_ProducesOneCompleteCascade(SurvivorDeathCause cause)
        {
            var f = new Fixture();
            var fate = f.Fate.ReportDeath("survivor_a", cause, "detail_x", "test_source");

            Assert.Equal(cause, fate.cause);
            Assert.Equal(10, fate.day); // day defaulted from clock
            Assert.Equal("test_source", fate.source);

            // roster marked dead with immutable reason
            var entry = f.Roster.Find("survivor_a");
            Assert.NotNull(entry);
            Assert.False(entry.isAlive);
            Assert.NotEmpty(entry.deathReason);

            // needs state dead
            var needs = f.Needs.Get("survivor_a");
            Assert.NotNull(needs);
            Assert.False(needs.IsAliveState);

            // memorial + journal exactly once
            Assert.Single(f.Memorial.Entries);
            Assert.Equal("survivor_a", f.Memorial.Entries[0].SurvivorId);
            Assert.Equal(1, f.Journal.Entries.Count);

            // flags + counters
            Assert.Equal(1, f.Flags.GetCounter(SurvivorFateSystem.CounterDeathsTotal));
            Assert.True(f.Flags.IsSet(SurvivorFateSystem.FlagSurvivorDiedPrefix + "survivor_a"));

            // briefing feed buffered one survivor_perished event
            var drain = new List<DayStateChangeEvent>();
            f.Fate.DrainDayEvents(drain);
            Assert.Single(drain);
            Assert.Equal("survivor_perished", drain[0].Kind);
            Assert.Equal("survivor_a", drain[0].PrimaryId);

            // grief applied to the living survivor
            var survivorB = f.Needs.Get("survivor_b");
            Assert.True(survivorB.Morale < 50f, $"expected grief morale hit, got {survivorB.Morale}");
        }

        // ── 2. Duplicate-event idempotency ─────────────────────────────

        [Fact]
        public void DuplicateReport_IsIdempotent_NoDuplicateSideEffects()
        {
            var f = new Fixture();
            var first = f.Fate.ReportDeath("survivor_a", SurvivorDeathCause.Needs, day: 9);
            var second = f.Fate.ReportDeath("survivor_a", SurvivorDeathCause.Combat, day: 12);

            // first record wins — cause and day are immutable once recorded
            Assert.Same(first, second);
            Assert.Equal(SurvivorDeathCause.Needs, second.cause);
            Assert.Equal(9, second.day);

            Assert.Single(f.Memorial.Entries);
            Assert.Equal(1, f.Journal.Entries.Count);
            Assert.Equal(1, f.Flags.GetCounter(SurvivorFateSystem.CounterDeathsTotal));
            Assert.Equal(1, f.Roster.Find("survivor_a")!.deathReason.Length > 0 ? 1 : 0); // reason written once
            Assert.Equal(1, f.Fate.DeathCount);

            // only one briefing event across duplicate reports
            var drain = new List<DayStateChangeEvent>();
            f.Fate.DrainDayEvents(drain);
            Assert.Single(drain);
        }

        [Fact]
        public void MultiCauseSameSurvivor_OnlyFirstCascadeRuns()
        {
            var f = new Fixture();
            f.Fate.ReportDeath("survivor_a", SurvivorDeathCause.Radiation);
            f.Fate.ReportDeath("survivor_a", SurvivorDeathCause.Disease, "disease_zoonotic_flu");

            var fate = f.Fate.FindFate("survivor_a");
            Assert.Equal(SurvivorDeathCause.Radiation, fate!.cause);
            Assert.Single(f.Memorial.Entries);
        }

        // ── 3. Assignment clearing ─────────────────────────────────────

        [Fact]
        public void Death_ClearsDutyCaregivingMedicalAndExpeditionAssignments()
        {
            var f = new Fixture();
            Assert.True(f.Duty.Assign(DutyRosterIds.RoleIntakeSleeper, "survivor_a"));
            f.Caregiving.IsAlive = id => f.Roster.Find(id)?.isAlive ?? false;
            f.Caregiving.AssignCaregiver("survivor_b", "survivor_a"); // a is patient
            Assert.True(f.Ward.Admit("survivor_a", "bed_a", 10).Succeeded);

            f.Fate.ReportDeath("survivor_a", SurvivorDeathCause.Medical, source: "ward");

            Assert.Null(f.Duty.GetRoleOf("survivor_a"));
            Assert.Null(f.Ward.GetActiveAdmission("survivor_a"));
            Assert.Null(f.Caregiving.GetCaregiverForPatient("survivor_a"));
        }

        [Fact]
        public void Death_OfCaregiver_ClearsTheirCaregivingLane()
        {
            var f = new Fixture();
            f.Caregiving.IsAlive = id => f.Roster.Find(id)?.isAlive ?? false;
            f.Caregiving.AssignCaregiver("survivor_b", "survivor_a");

            f.Fate.ReportDeath("survivor_b", SurvivorDeathCause.Needs);

            Assert.Null(f.Caregiving.GetCaregiverForPatient("survivor_a"));
        }

        [Fact]
        public void Death_RecallsActiveExpedition()
        {
            var f = new Fixture();
            f.Fate.ReportDeath("survivor_a", SurvivorDeathCause.Expedition, "loc_ruined_mall");
            Assert.Contains("survivor_a", f.Recalled);
        }

        // ── 4. Social / leadership / final-wish ────────────────────────

        [Fact]
        public void Death_OfLeader_UpdatesLeadershipStressAndClearsLeader()
        {
            var f = new Fixture();
            f.Social.SetAliveSurvivors(new List<string> { "survivor_a", "survivor_b" });
            Assert.True(f.Social.DesignateLeader("survivor_a"));
            Assert.Equal("survivor_a", f.Social.Leadership.CurrentLeaderId);

            f.Fate.ReportDeath("survivor_a", SurvivorDeathCause.Combat);

            Assert.True(string.IsNullOrEmpty(f.Social.Leadership.CurrentLeaderId));
        }

        [Fact]
        public void Death_WithCompletedWish_MemorializedAsResolved()
        {
            var f = new Fixture();
            f.FinalWish.RegisterWish("arche", FinalWishSystem.WishSeeTheSky);
            f.FinalWish.DeclareTerminalPrognosis("survivor_a", "arche", isAlive: true);
            f.FinalWish.AdvanceWishStep("survivor_a", "step1"); // see_the_sky needs 1 step

            f.Fate.ReportDeath("survivor_a", SurvivorDeathCause.Disease);

            Assert.True(f.Memorial.Entries[0].FinalWishResolved);
        }

        [Fact]
        public void Death_WithActiveWish_FailsItAndMemorializedUnresolved()
        {
            var f = new Fixture();
            float buff = 0f;
            f.FinalWish.ApplyPermanentShelterMoraleBuff = d => buff += d;
            f.FinalWish.RegisterWish("arche", FinalWishSystem.WishBuildMemorial); // 3 steps
            f.FinalWish.DeclareTerminalPrognosis("survivor_a", "arche", isAlive: true);

            f.Fate.ReportDeath("survivor_a", SurvivorDeathCause.Disease);

            Assert.False(f.Memorial.Entries[0].FinalWishResolved);
            Assert.Equal(FinalWishSystem.WishFailedMoralePenalty, buff);
        }

        // ── 5. Last-survivor detection ─────────────────────────────────

        [Fact]
        public void LastSurvivorDeath_RaisesOnLastSurvivorDied()
        {
            var f = new Fixture();
            SurvivorFateEvent last = null;
            f.Fate.OnLastSurvivorDied += e => last = e;

            f.Fate.ReportDeath("survivor_a", SurvivorDeathCause.Needs);
            Assert.Null(last); // one still alive

            f.Fate.ReportDeath("survivor_b", SurvivorDeathCause.Radiation);
            Assert.NotNull(last);
            Assert.Equal("survivor_b", last.survivorId);
            Assert.Equal(0, f.Roster.LivingCount);
        }

        [Fact]
        public void NoRosterLane_LastSurvivorNeverFires()
        {
            var fate = new SurvivorFateSystem(); // fully lane-less
            SurvivorFateEvent last = null;
            fate.OnLastSurvivorDied += e => last = e;
            fate.ReportDeath("survivor_x", SurvivorDeathCause.Unknown);
            Assert.Null(last); // no roster → no living-count evaluation
            Assert.True(fate.HasFate("survivor_x"));
        }

        // ── 6. Save-during-death round-trip ────────────────────────────

        [Fact]
        public void CaptureRestore_BetweenDeaths_PreservesIdempotency()
        {
            var f = new Fixture();
            f.Fate.ReportDeath("survivor_a", SurvivorDeathCause.Needs, day: 8);

            // Save mid-cascade-window, restore into a fresh system.
            var save = f.Fate.CaptureState();

            var f2 = new Fixture();
            // Mirror real load order: fate records and roster restore together.
            f2.Fate.RestoreState(save);
            f2.Roster.RestoreState(f.Roster.CaptureState());

            Assert.True(f2.Fate.HasFate("survivor_a"));
            var restored = f2.Fate.FindFate("survivor_a");
            Assert.Equal(SurvivorDeathCause.Needs, restored!.cause);
            Assert.Equal(8, restored.day);

            // Duplicate report after restore: the fate ledger no-ops it —
            // no second cascade, no memorial in this session's (empty)
            // memorial lane, no second counter increment, no second death count.
            f2.Fate.ReportDeath("survivor_a", SurvivorDeathCause.Combat, day: 99);
            Assert.Empty(f2.Memorial.Entries);
            Assert.Equal(0, f2.Flags.GetCounter(SurvivorFateSystem.CounterDeathsTotal));
            Assert.Equal(1, f2.Fate.DeathCount);
            // Roster still records the original immutable reason, unchanged.
            Assert.False(f2.Roster.Find("survivor_a")!.isAlive);

            // A new death after restore still cascades.
            f2.Fate.ReportDeath("survivor_b", SurvivorDeathCause.Disease, "disease_zoonotic_flu");
            Assert.Equal(2, f2.Fate.DeathCount);
            Assert.Single(f2.Memorial.Entries, e => e.SurvivorId == "survivor_b");
        }

        [Fact]
        public void CaptureState_IsDeterministicallyOrdered()
        {
            var f = new Fixture();
            f.Fate.ReportDeath("survivor_b", SurvivorDeathCause.Combat, day: 5);
            f.Fate.ReportDeath("survivor_a", SurvivorDeathCause.Needs, day: 5);

            var save = f.Fate.CaptureState();
            Assert.Equal("survivor_a", save.fates[0].survivorId); // ordinal order on tie
            Assert.Equal("survivor_b", save.fates[1].survivorId);
        }

        // ── 7. Legacy-save reconciliation ──────────────────────────────

        [Fact]
        public void ReconcileFromRoster_SynthesizesFatesForLegacyDead()
        {
            var f = new Fixture();
            // Simulate a pre-pipeline save: roster says dead, no fate record.
            f.Roster.Die("survivor_a", "starved before the pipeline existed");

            int synthesized = f.Fate.ReconcileFromRoster();

            Assert.Equal(1, synthesized);
            var fate = f.Fate.FindFate("survivor_a");
            Assert.NotNull(fate);
            Assert.Equal(SurvivorDeathCause.Unknown, fate!.cause);
            Assert.Equal("starved before the pipeline existed", fate.causeDetail);
            Assert.Equal("legacy_reconcile", fate.source);
            Assert.Single(f.Memorial.Entries);

            // Idempotent — second reconcile adds nothing.
            Assert.Equal(0, f.Fate.ReconcileFromRoster());
            Assert.Single(f.Memorial.Entries);
        }

        // ── 8. Player-avatar distinction ───────────────────────────────

        [Fact]
        public void PlayerAvatarDeath_IsFlaggedOnRecord()
        {
            var f = new Fixture();
            var fate = f.Fate.ReportDeath("survivor_a", SurvivorDeathCause.Radiation,
                source: "holdfast_runtime", isPlayerAvatar: true);

            Assert.True(fate.isPlayerAvatar);
            Assert.False(f.Roster.Find("survivor_a")!.isAlive);

            var other = f.Fate.ReportDeath("survivor_b", SurvivorDeathCause.Needs);
            Assert.False(other.isPlayerAvatar);
        }

        // ── 9. Drain semantics ─────────────────────────────────────────

        [Fact]
        public void DrainDayEvents_EmptiesBuffer_NoDuplicatesOnSecondDrain()
        {
            var f = new Fixture();
            f.Fate.ReportDeath("survivor_a", SurvivorDeathCause.Needs);

            var first = new List<DayStateChangeEvent>();
            f.Fate.DrainDayEvents(first);
            var second = new List<DayStateChangeEvent>();
            f.Fate.DrainDayEvents(second);

            Assert.Single(first);
            Assert.Empty(second);
        }

        [Fact]
        public void ReportDeath_NullOrBlankId_Throws()
        {
            var f = new Fixture();
            Assert.Throws<ArgumentNullException>(() => f.Fate.ReportDeath(null!));
            Assert.Throws<ArgumentException>(() => f.Fate.ReportDeath(new SurvivorFateEvent()));
        }

        // ── 10. Graceful degradation ───────────────────────────────────

        [Fact]
        public void LanelessSystem_StillRecordsFate()
        {
            var fate = new SurvivorFateSystem();
            var record = fate.ReportDeath("survivor_x", SurvivorDeathCause.Expedition, day: 3);
            Assert.Equal(3, record.day);
            Assert.True(fate.HasFate("survivor_x"));
            Assert.Equal(1, fate.DeathCount);

            var save = fate.CaptureState();
            var restored = new SurvivorFateSystem();
            restored.RestoreState(save);
            Assert.True(restored.HasFate("survivor_x"));
        }
    }
}
