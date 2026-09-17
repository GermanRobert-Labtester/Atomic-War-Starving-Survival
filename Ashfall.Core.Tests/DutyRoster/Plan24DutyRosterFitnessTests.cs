// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.DutyRoster
{
    public sealed class Plan24DutyRosterFitnessTests
    {
        private static DutyRosterSystem ReadyRoster()
        {
            var roster = new DutyRosterSystem(1208);
            roster.Unlock(59);
            Assert.True(roster.ResolveChartChoice(DutyRosterIds.ChoiceWritePencil, 60));
            roster.TickMorning(61, new List<DutyRosterOccupant>
            {
                new DutyRosterOccupant
                {
                    survivorId = "survivor_a",
                    displayName = "Survivor A",
                    occupationObserved = "watch",
                    sleptHere = true
                },
                new DutyRosterOccupant
                {
                    survivorId = "survivor_b",
                    displayName = "Survivor B",
                    occupationObserved = "watch",
                    sleptHere = true
                }
            });
            return roster;
        }

        private static RoleFitnessVerdict Verdict(
            string survivorId,
            bool allowed,
            string reason = "quarantined")
        {
            var baseVerdict = new FitnessVerdict(
                survivorId,
                allowed ? FitnessLevel.Impaired : FitnessLevel.Incapacitated,
                allowed ? new string[0] : new[] { reason },
                allowed ? new[] { "severe_fatigue" } : new string[0],
                new[] { NeedKind.Fatigue });
            return new RoleFitnessVerdict(
                survivorId,
                DutyRosterIds.RoleNightWatch,
                baseVerdict,
                allowed,
                allowed,
                baseVerdict.BlockingReasons,
                baseVerdict.DegradedFactors,
                allowed ? 8f : 0f);
        }

        [Fact]
        public void AssignmentCommitUsesHealthAwareVerdict()
        {
            var roster = ReadyRoster();
            roster.EvaluateRoleFitness = (id, _) =>
                id == "survivor_a" ? Verdict(id, false) : Verdict(id, true);

            var result = roster.AssignWithResult(DutyRosterIds.RoleNightWatch, "survivor_a");

            Assert.Equal(ActionResult.StatusKind.Blocked, result.Status);
            Assert.Equal("fitness_blocked", result.FailureCode);
            Assert.Null(roster.GetAssignment(DutyRosterIds.RoleNightWatch));
        }

        [Fact]
        public void PreviewAndCommitShareTheSameFitnessGate()
        {
            var roster = ReadyRoster();
            roster.EvaluateRoleFitness = (id, _) => Verdict(id, false);

            var preview = roster.PreviewAssign(DutyRosterIds.RoleNightWatch, "survivor_a");
            var commit = roster.AssignWithResult(DutyRosterIds.RoleNightWatch, "survivor_a");

            Assert.False(preview.IsAvailable);
            Assert.Equal("fitness_blocked", preview.FailureCode);
            Assert.Equal(ActionResult.StatusKind.Blocked, commit.Status);
            Assert.Null(roster.GetAssignment(DutyRosterIds.RoleNightWatch));
        }

        [Fact]
        public void AutoAssignmentSkipsBlockedCandidateDeterministically()
        {
            var roster = ReadyRoster();
            roster.EvaluateRoleFitness = (id, _) => id == "survivor_a"
                ? Verdict(id, false)
                : FitVerdict(id);

            Assert.Equal(1, roster.AutoAssignDefaults(61));
            Assert.Equal("survivor_b", roster.GetAssignment(DutyRosterIds.RoleNightWatch));
        }

        [Fact]
        public void AutoAssignmentDoesNotSilentlyConfirmImpairedCandidate()
        {
            var roster = ReadyRoster();
            roster.EvaluateRoleFitness = (id, _) => id == "survivor_a"
                ? Verdict(id, true)
                : FitVerdict(id);

            Assert.Equal(1, roster.AutoAssignDefaults(61));
            Assert.Equal("survivor_b", roster.GetAssignment(DutyRosterIds.RoleNightWatch));
        }

        [Fact]
        public void ReassignmentAndRemovalEmitDutyVacancy()
        {
            var roster = ReadyRoster();
            roster.EvaluateRoleFitness = (id, _) => FitVerdict(id);
            var vacated = new List<string>();
            roster.OnDutyVacated += (role, survivorId) => vacated.Add(role + ":" + survivorId);

            Assert.True(roster.Assign(DutyRosterIds.RoleNightWatch, "survivor_a"));
            Assert.True(roster.Assign(DutyRosterIds.RoleNightWatch, "survivor_b"));
            roster.RemoveAssignmentsFor("survivor_b");

            Assert.Equal(
                new[]
                {
                    DutyRosterIds.RoleNightWatch + ":survivor_a",
                    DutyRosterIds.RoleNightWatch + ":survivor_b"
                },
                vacated);
            Assert.Null(roster.GetAssignment(DutyRosterIds.RoleNightWatch));
        }

        [Fact]
        public void ImpairedWarningCanBeAcknowledgedOnExistingAssignment()
        {
            var roster = ReadyRoster();
            roster.EvaluateRoleFitness = (id, _) => Verdict(id, true);
            Assert.Equal(ActionResult.StatusKind.Blocked,
                roster.AssignWithResult(DutyRosterIds.RoleNightWatch, "survivor_a").Status);
            Assert.True(roster.AssignWithResult(
                DutyRosterIds.RoleNightWatch, "survivor_a", confirmFitnessWarning: true).IsSuccess);

            Assert.True(roster.AcknowledgeFitnessWarning(
                DutyRosterIds.RoleNightWatch, "survivor_a", day: 61));

            var entry = roster.State.assignments.Find(a => a.role == DutyRosterIds.RoleNightWatch);
            Assert.NotNull(entry);
            Assert.True(entry!.fitnessWarningAcknowledged);
            Assert.Equal(61, entry.fitnessWarningDay);
            Assert.Contains("severe_fatigue", entry.fitnessWarningReasons);
        }

        [Fact]
        public void WarningOverrideMustBeExplicitAtAssignmentCommit()
        {
            var roster = ReadyRoster();
            roster.EvaluateRoleFitness = (id, _) => Verdict(id, true);

            var unconfirmed = roster.ExecuteAssign(
                DutyRosterIds.RoleNightWatch, "survivor_a", 0, 0);
            var confirmed = roster.ExecuteAssign(
                DutyRosterIds.RoleNightWatch, "survivor_a", 0, 0,
                confirmFitnessWarning: true);

            Assert.Equal("fitness_warning_confirmation_required", unconfirmed.FailureCode);
            Assert.True(confirmed.IsSuccess);
            Assert.Equal("survivor_a", roster.GetAssignment(DutyRosterIds.RoleNightWatch));
        }

        private static RoleFitnessVerdict FitVerdict(string survivorId)
        {
            var baseVerdict = new FitnessVerdict(
                survivorId,
                FitnessLevel.Fit,
                new string[0],
                new string[0],
                new NeedKind[0]);
            return new RoleFitnessVerdict(
                survivorId,
                DutyRosterIds.RoleNightWatch,
                baseVerdict,
                true,
                false,
                new string[0],
                new string[0],
                12f);
        }
    }
}
