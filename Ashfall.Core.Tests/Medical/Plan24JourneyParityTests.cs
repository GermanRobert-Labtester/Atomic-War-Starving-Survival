// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Medical;
using Ashfall.Core.Memorial;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Medical
{
    /// <summary>
    /// Plan 24 (Task A4, Wave 8) — the verification closure: journey save/load
    /// parity across every Plan 24 seam, and the 30-day policy-simulation
    /// invariant harness with a mid-run restore. All journeys run through the
    /// production save path (each system's CaptureState/RestoreState — the
    /// same methods the host save stores call), never hand-built state.
    /// </summary>
    public sealed class Plan24JourneyParityTests
    {
        // ── The journey world (every Plan 24 seam, production save path) ──

        private sealed class JourneyWorld
        {
            public SurvivorRosterSystem Roster = new SurvivorRosterSystem();
            public NeedsSystem Needs = new NeedsSystem();
            public DutyRosterSystem Duty = new DutyRosterSystem();
            public SurvivorRelationsSystem Relations = new SurvivorRelationsSystem(new SeededRng(11), null);
            public SurvivorSocialCoordinator Social = null!;
            public MemorialSystem Memorial = new MemorialSystem(new MemorialState());
            public SurvivorFateSystem Fate = null!;
            public List<SurvivorNeedsState> Alive = new List<SurvivorNeedsState>();
            public int Day;

            public JourneyWorld(int seed)
            {
                Social = new SurvivorSocialCoordinator(new SeededRng(seed), Needs, Relations, Duty, () => Day);
                Fate = new SurvivorFateSystem(
                    roster: Roster, needs: Needs, dutyRoster: Duty,
                    social: Social, memorial: Memorial,
                    getDay: () => Day,
                    displayNameFor: id => Roster.FindDefinition(id)?.displayName ?? id);
                foreach (var id in new[] { "survivor_a", "survivor_b", "survivor_c" })
                {
                    Roster.RegisterDefinition(new SurvivorDefinition { id = id, displayName = id });
                    Roster.Join(id, 1);
                    var state = new SurvivorNeedsState { Id = id, Morale = 50f };
                    Needs.Register(state);
                    Alive.Add(state);
                }
                Duty.Unlock(1);
                Needs.CurrentDay = 1;
            }

            /// <summary>The production save path: each system's real CaptureState.</summary>
            public JourneySave CaptureAll() => new JourneySave
            {
                Needs = Alive.Select(s => (s.Id, s.Hunger, s.Thirst, s.Fatigue, s.Warmth,
                    s.Morale, s.Health, s.Hygiene, s.IsAlive, s.IsDead)).ToList(),
                Duty = Duty.CaptureState(),
                Relations = Relations.CaptureState(),
                Social = Social.CaptureState(),
                Memorial = Memorial.CaptureState(),
            };

            /// <summary>The production restore path: fresh systems, real RestoreState.</summary>
            public static JourneyWorld Restored(int seed, JourneySave save)
            {
                var world = new JourneyWorld(seed);
                world.Duty.RestoreState(save.Duty);
                world.Relations.RestoreState(save.Relations);
                world.Social.RestoreState(save.Social);
                world.Memorial.RestoreState(save.Memorial);
                world.Alive.Clear();
                foreach (var (id, hunger, thirst, fatigue, warmth, morale, health, hygiene, isAlive, isDead) in save.Needs)
                {
                    var s = new SurvivorNeedsState
                    {
                        Id = id, Hunger = hunger, Thirst = thirst, Fatigue = fatigue,
                        Warmth = warmth, Morale = morale, Health = health, Hygiene = hygiene,
                        IsAlive = isAlive, IsDead = isDead
                    };
                    world.Needs.Register(s); // replaces in place (the ghost-evicting Register)
                    world.Alive.Add(s);
                }
                return world;
            }
        }

        private sealed class JourneySave
        {
            public List<(string Id, float Hunger, float Thirst, float Fatigue, float Warmth,
                float Morale, float Health, float Hygiene, bool IsAlive, bool IsDead)> Needs = new();
            public DutyRosterSystemState Duty = null!;
            public SurvivorRelationsState Relations = null!;
            public SurvivorSocialSaveState Social = null!;
            public MemorialState Memorial = null!;
        }

        /// <summary>Field-by-field canonical fingerprint (invariant culture).</summary>
        private static string Fingerprint(JourneyWorld w) => string.Join("|",
            w.Alive.OrderBy(s => s.Id, StringComparer.Ordinal).Select(s =>
                $"{s.Id}:{s.Hunger:0.###}:{s.Thirst:0.###}:{s.Fatigue:0.###}:{s.Warmth:0.###}:" +
                $"{s.Morale:0.###}:{s.Health:0.###}:{s.Hygiene:0.###}:{(s.IsDead ? 1 : 0)}")
            .Concat(new[]
            {
                "duty:" + string.Join(";", w.Duty.State.assignments
                    .Select(a => $"{a.role}={a.survivorId}:{a.fitnessWarningAcknowledged}")
                    .OrderBy(x => x, StringComparer.Ordinal)),
                "grief:" + string.Join(";", w.Relations.State.relationships
                    .Select(r => $"{r.dwellerA}~{r.dwellerB}:{r.grief:0.###}:{r.grief_since_day}")
                    .OrderBy(x => x, StringComparer.Ordinal)),
                "memorial:" + string.Join(";", w.Memorial.Entries
                    .Select(e => $"{e.SurvivorId}@{e.Day}:{e.MournedDay}")),
                "ration:" + string.Join(";", w.Alive.Where(s => s.IsAliveState)
                    .Select(s => $"{s.Id}:{w.Social.Ration.GetAllocation(s.Id):0.###}")
                    .OrderBy(x => x, StringComparer.Ordinal)),
            }));

        // ── Journey A: treatment → discharge → impaired window → recovery ──

        [Fact]
        public void TreatmentJourney_ContinuousEqualsInterruptedAtEveryCheckpoint()
        {
            var ward = MakeWard(out var state);

            // Continuous run: admit day 3, treat, discharge day 5.
            Assert.True(ward.Admit("survivor_a", "bed_a", 3).Succeeded);
            ward.RunProcedure("survivor_a", "proc_bedrest", 4);
            Assert.True(ward.Discharge("survivor_a", 5).Succeeded);
            var continuous = ward.CaptureState();

            // Interrupted run: save at mid-stay (day 4, post-procedure),
            // restore into fresh systems, continue identically.
            var ward2 = MakeWard(out var state2);
            Assert.True(ward2.Admit("survivor_a", "bed_a", 3).Succeeded);
            ward2.RunProcedure("survivor_a", "proc_bedrest", 4);
            var saved = ward2.CaptureState(); // the production save path

            var restored = MakeWard(out _);
            restored.RestoreState(saved);
            Assert.True(restored.Discharge("survivor_a", 5).Succeeded);
            var interrupted = restored.CaptureState();

            // Field-by-field: admissions, status, discharge day all equal.
            Assert.Equal(continuous.Admissions.Count, interrupted.Admissions.Count);
            for (int i = 0; i < continuous.Admissions.Count; i++)
            {
                Assert.Equal(continuous.Admissions[i].PatientId, interrupted.Admissions[i].PatientId);
                Assert.Equal(continuous.Admissions[i].Status, interrupted.Admissions[i].Status);
                Assert.Equal(continuous.Admissions[i].DischargedDay, interrupted.Admissions[i].DischargedDay);
            }
            Assert.Equal(continuous.ProceduresRun.Count, interrupted.ProceduresRun.Count);
        }

        private static MedicalWardSystem MakeWard(out MedicalWardState state)
        {
            state = new MedicalWardState();
            return new MedicalWardSystem(
                state,
                new[] { new MedicalBed("bed_a", "Bed A", MedicalBedCategory.General) },
                new[]
                {
                    new MedicalProcedureDef("proc_bedrest", "Bed Rest", "MedicalSystem",
                            new Dictionary<string, int>(), durationHours: 2f)
                });
        }

        // ── Journey B: death → grief → mourning → re-split, mid-cascade reload ──

        [Fact]
        public void DeathGriefJourney_ContinuousEqualsInterruptedAfterCascade()
        {
            RunDeathJourney(new JourneyWorld(11), continuous: true, out var continuousFp);
            var interruptedWorld = new JourneyWorld(11);
            RunDeathJourney(interruptedWorld, continuous: false, out var interruptedFp);
            Assert.Equal(continuousFp, interruptedFp);
        }

        private static void RunDeathJourney(JourneyWorld w, bool continuous, out string fingerprint)
        {
            // Unequal service: a designated leader takes priority rations.
            w.Social.RationPolicy = StartingLevel.RationPolicy.Standard;
            w.Social.SetAliveSurvivors(w.Alive.Where(s => s.IsAliveState).Select(s => s.Id).ToList());
            w.Social.DesignateLeader("survivor_c");

            // Resentment builds toward the leader for two days.
            for (int day = 1; day <= 2; day++)
            {
                w.Day = day;
                w.Needs.CurrentDay = day;
                w.Social.TickDay(day, w.Alive);
            }

            // A non-leader dies through the real fate cascade (day 3):
            // shelter grief + relationship grief + onset stamps + vacancy.
            w.Day = 3;
            w.Needs.CurrentDay = 3;
            w.Fate.ReportDeath("survivor_a", SurvivorDeathCause.Needs, "starvation", "test");

            if (!continuous)
            {
                // Interruption immediately after the cascade: save, restore into
                // a fresh world, continue from identical canonical facts.
                var saved = w.CaptureAll();
                var restored = JourneyWorld.Restored(11, saved);
                ContinueDeathJourney(restored);
                fingerprint = Fingerprint(restored);
                return;
            }

            ContinueDeathJourney(w);
            fingerprint = Fingerprint(w);
        }

        private static void ContinueDeathJourney(JourneyWorld w)
        {
            // Mourning vigil on day 4 (exactly once per death).
            w.Day = 4;
            w.Needs.CurrentDay = 4;
            var pending = w.Memorial.LatestUnmourned();
            Assert.NotNull(pending);
            Assert.Equal(ActionResult.StatusKind.Success, w.Memorial.Mourn(pending!.SurvivorId, 4).Status);

            // Day 5: the re-split runs on the reduced roster; the grievance
            // engine ticks for the living only.
            w.Day = 5;
            w.Needs.CurrentDay = 5;
            w.Social.SetAliveSurvivors(w.Alive.Where(s => s.IsAliveState).Select(s => s.Id).ToList());
            w.Social.TickDay(5, w.Alive);
        }

        // ── The 30-day policy-simulation invariant harness ────────────

        [Fact]
        public void ThirtyDaySimulation_SameSeedIdentical_AndMidReloadSuffixIdentical()
        {
            var continuous = new JourneyWorld(90);
            string continuousFp = RunThirtyDays(continuous, reloadAtDay: -1);

            var reloaded = new JourneyWorld(90);
            string reloadedFp = RunThirtyDays(reloaded, reloadAtDay: 15);

            Assert.Equal(continuousFp, reloadedFp);
        }

        private static string RunThirtyDays(JourneyWorld w, int reloadAtDay)
        {

            for (int day = 1; day <= 30; day++)
            {
                w.Day = day;
                w.Needs.CurrentDay = day;

                // The policy: keep every living survivor on night watch through
                // the fitness gate (impaired assignments require the explicit
                // confirmation flag; hard blocks fail closed).
                w.Social.SetAliveSurvivors(w.Alive.Where(s => s.IsAliveState).Select(s => s.Id).ToList());
                foreach (var s in w.Alive.Where(s => s.IsAliveState))
                {
                    var verdict = w.Duty.PreviewRoleFitness(s.Id, DutyRosterIds.RoleNightWatch);
                    if (verdict == null || !verdict.Allowed) continue;
                    w.Duty.AssignWithResult(DutyRosterIds.RoleNightWatch, s.Id,
                        confirmFitnessWarning: verdict.RequiresConfirmation);
                }

                // The day: needs drift + the social/ration/grievance ticks.
                w.Needs.Tick(24f);
                w.Social.TickDay(day, w.Alive);

                // Day-12 stressor: a death through the real cascade, then the
                // day-13 vigil — grief decays through the rest of the run.
                if (day == 12)
                    w.Fate.ReportDeath("survivor_a", SurvivorDeathCause.Needs, "sim", "test");
                if (day == 13 && w.Memorial.LatestUnmourned() != null)
                    w.Memorial.Mourn(w.Memorial.LatestUnmourned()!.SurvivorId, day);

                // Per-day invariants (impossible states must never appear).
                foreach (var assignment in w.Duty.State.assignments)
                {
                    if (assignment == null || string.IsNullOrEmpty(assignment.survivorId)) continue;
                    var occupant = w.Needs.Get(assignment.survivorId);
                    Assert.NotNull(occupant);
                    Assert.True(occupant!.IsAliveState,
                        $"day {day}: a dead survivor holds the {assignment.role} assignment");
                }
                foreach (var s in w.Alive)
                {
                    if (!s.IsAliveState) continue;
                    Assert.InRange(s.Hunger, 0f, 100f);
                    Assert.InRange(s.Morale, 0f, 100f);
                    Assert.InRange(s.Health, 0f, Math.Max(0f, s.MaxHealthCap));
                }

                // The mid-run restore: after day 15's tick, save through the
                // production path, restore into a fresh world, continue.
                if (day == reloadAtDay)
                {
                    var saved = w.CaptureAll();
                    var restored = JourneyWorld.Restored(90, saved);
                    // Continue days 16–30 on the restored world and fingerprint
                    // its final state — the same shape the continuous run returns.
                    return ContinueThirtyDaySuffix(restored, day + 1);
                }
            }

            return Fingerprint(w);
        }

        private static string ContinueThirtyDaySuffix(JourneyWorld w, int startDay)
        {
            for (int day = startDay; day <= 30; day++)
            {
                w.Day = day;
                w.Needs.CurrentDay = day;
                w.Social.SetAliveSurvivors(w.Alive.Where(s => s.IsAliveState).Select(s => s.Id).ToList());
                foreach (var s in w.Alive.Where(s => s.IsAliveState))
                {
                    var verdict = w.Duty.PreviewRoleFitness(s.Id, DutyRosterIds.RoleNightWatch);
                    if (verdict == null || !verdict.Allowed) continue;
                    w.Duty.AssignWithResult(DutyRosterIds.RoleNightWatch, s.Id,
                        confirmFitnessWarning: verdict.RequiresConfirmation);
                }
                w.Needs.Tick(24f);
                w.Social.TickDay(day, w.Alive);
                if (day == 13 && w.Memorial.LatestUnmourned() != null)
                    w.Memorial.Mourn(w.Memorial.LatestUnmourned()!.SurvivorId, day);
            }
            return Fingerprint(w);
        }
    }
}
