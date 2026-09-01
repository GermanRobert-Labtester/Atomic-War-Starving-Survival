using Ashfall.Core.Muster;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class MusterPathEvaluatorTests
    {
        private static MusterPathInput Input(
            string dominant = "",
            int tension = 0,
            int hostile = 0,
            int allied = 0,
            int majors = 4,
            int treaties = 0,
            int violated = 0,
            bool grievance = false,
            bool peace = false,
            bool campFormed = true,
            int members = 9)
        {
            return new MusterPathInput
            {
                DominantFactionId = dominant,
                WarTension = tension,
                HostileFactionCount = hostile,
                AlliedFactionCount = allied,
                SurvivingMajorFactions = majors,
                ActiveTreatyCount = treaties,
                ViolatedTreatyCount = violated,
                GrievanceUnresolved = grievance,
                PeacePressure = peace,
                CampFormed = campFormed,
                CampMembers = members
            };
        }

        // ── victor's path ──────────────────────────────────────────────

        [Fact]
        public void Victors_WhenDominantWithBroadHostility()
        {
            Assert.Equal(MusterPaths.Victors, MusterPathEvaluator.Evaluate(
                Input(dominant: "faction_central_garrison", hostile: 2)));
        }

        [Fact]
        public void Victors_WhenDominantUnderHighWarTension()
        {
            Assert.Equal(MusterPaths.Victors, MusterPathEvaluator.Evaluate(
                Input(dominant: "faction_rebuilders", tension: 60)));
        }

        [Fact]
        public void DominanceAloneWithoutPressureIsNotVictors()
        {
            // A dominant faction with low tension and little hostility has not
            // imposed a victor's gathering; other rules decide.
            Assert.Equal(MusterPaths.Unsettled, MusterPathEvaluator.Evaluate(
                Input(dominant: "faction_ash_sign", tension: 30, hostile: 1, campFormed: false)));
        }

        // ── negotiated path ────────────────────────────────────────────

        [Fact]
        public void Negotiated_WithSurvivorsTreatyAndCamp()
        {
            Assert.Equal(MusterPaths.Negotiated, MusterPathEvaluator.Evaluate(
                Input(treaties: 1)));
        }

        [Fact]
        public void Negotiated_PeacePressureSubstitutesForTreaties()
        {
            Assert.Equal(MusterPaths.Negotiated, MusterPathEvaluator.Evaluate(
                Input(peace: true, grievance: true)));
        }

        [Fact]
        public void Negotiated_RequiresTheCamp()
        {
            Assert.Equal(MusterPaths.Unsettled, MusterPathEvaluator.Evaluate(
                Input(treaties: 1, campFormed: false)));
        }

        [Fact]
        public void Negotiated_RequiresMultipleMajorFactions()
        {
            Assert.Equal(MusterPaths.Unsettled, MusterPathEvaluator.Evaluate(
                Input(treaties: 1, majors: 1)));
        }

        [Fact]
        public void Negotiated_RequiresAWorkingChannel()
        {
            Assert.Equal(MusterPaths.Unsettled, MusterPathEvaluator.Evaluate(
                Input(campFormed: true)));
        }

        // ── precedence and fallback ────────────────────────────────────

        [Fact]
        public void VictorsOverridesNegotiatedConditions()
        {
            // Dominance + tension wins even when treaties, peace and the camp
            // would otherwise describe a negotiated gathering.
            Assert.Equal(MusterPaths.Victors, MusterPathEvaluator.Evaluate(
                Input(dominant: "faction_hydro_barons", tension: 80, treaties: 2, peace: true)));
        }

        [Fact]
        public void Unsettled_IsTheAmbiguousFallback()
        {
            Assert.Equal(MusterPaths.Unsettled, MusterPathEvaluator.Evaluate(
                Input(campFormed: false, majors: 1)));
        }

        [Fact]
        public void NullInput_YieldsUnsettled()
        {
            Assert.Equal(MusterPaths.Unsettled, MusterPathEvaluator.Evaluate(null));
        }

        [Fact]
        public void Evaluate_IsDeterministicAndIdempotent()
        {
            var input = Input(dominant: "faction_central_garrison", hostile: 3);
            string first = MusterPathEvaluator.Evaluate(input);
            for (int i = 0; i < 5; i++)
                Assert.Equal(first, MusterPathEvaluator.Evaluate(input));
        }

        // ── MusterSystem persistence of the path ──────────────────────

        [Fact]
        public void MusterSystem_StoresAndRoundTripsThePath()
        {
            var sys = new MusterSystem();
            Assert.Equal(string.Empty, sys.MusterPath);
            Assert.False(sys.SetMusterPath("not_a_path"));
            Assert.True(sys.SetMusterPath(MusterPaths.Negotiated));
            Assert.False(sys.SetMusterPath(MusterPaths.Negotiated)); // idempotent
            Assert.True(sys.SetMusterPath(MusterPaths.Victors));

            var restored = new MusterSystem();
            restored.RestoreState(sys.CaptureState());
            Assert.Equal(MusterPaths.Victors, restored.MusterPath);
        }

        [Fact]
        public void MusterSystem_OldSavesWithoutPathRestoreToEmpty()
        {
            var legacy = new MusterState { escalationDay = 300, musterTriggered = true };
            legacy.musterPath = null; // simulate a pre-Plan-25 payload after deserialization
            var sys = new MusterSystem();
            sys.RestoreState(legacy);
            Assert.Equal(string.Empty, sys.MusterPath);
        }
    }
}
