// SPDX-License-Identifier: MIT
// Expansion 41 — The Quiet : SleepAcousticRestEngine focused tests
using Xunit;
using Ashfall.Core.Needs;

namespace Ashfall.Core.Tests.Needs
{
    public sealed class SleepAcousticRestEngineTests
    {
        // ── 1. Soundproofing attenuates source decibels reaching bunk ──
        [Fact]
        public void CalculateAcousticAttenuation_ReducesDecibels_ProportionalToProofing()
        {
            int uninsulatedDb = SleepAcousticRestEngine.CalculateAcousticAttenuation(
                sourceDecibels: 75,
                wallSoundproofingPermille: 100,
                doorSoundproofingPermille: 100);

            int insulatedDb = SleepAcousticRestEngine.CalculateAcousticAttenuation(
                sourceDecibels: 75,
                wallSoundproofingPermille: 900,
                doorSoundproofingPermille: 800);

            Assert.True(insulatedDb < uninsulatedDb,
                $"Insulated room ({insulatedDb} dB) should be quieter than uninsulated ({uninsulatedDb} dB)");
        }

        // ── 2. Crowding penalty triggers when floor area per occupant is below threshold ──
        [Fact]
        public void CalculateCrowdingDensity_PenalizesCrowdedQuarters()
        {
            // 8 people in 12 sq metres = 1.5 sq metres per person -> extreme crowding
            int crowdedPenalty = SleepAcousticRestEngine.CalculateCrowdingDensity(
                occupants: 8,
                roomAreaSqMetres: 12);

            // 2 people in 20 sq metres = 10 sq metres per person -> spacious
            int spaciousPenalty = SleepAcousticRestEngine.CalculateCrowdingDensity(
                occupants: 2,
                roomAreaSqMetres: 20);

            Assert.True(crowdedPenalty >= 700,
                $"Extreme crowding should produce heavy penalty, got {crowdedPenalty}");
            Assert.Equal(0, spaciousPenalty);
        }

        // ── 3. High noise and severe quiet-hours violation yield Unbearable/Disturbed band ──
        [Fact]
        public void EvaluateSleepQuality_DegradesToDisturbedOrUnbearable_OnLoudNoiseAndViolations()
        {
            var quarter = new SleepingQuarterState
            {
                RoomId = "dorm-noisy",
                AmbientNoiseLevelDecibels = 80,
                WallSoundproofingPermille = 100,
                DoorSoundproofingPermille = 100,
                AssignedOccupants = 6,
                RoomAreaSquareMetres = 15
            };

            var quality = SleepAcousticRestEngine.EvaluateSleepQuality(
                quarter: quarter,
                isQuietHours: true,
                compliance: QuietHoursCompliance.ViolatedSevere,
                hoursSlept: 8);

            Assert.True(quality.EnvironmentBand <= SleepEnvironmentBand.Disturbed,
                $"Severe noise and violations should yield Disturbed or Unbearable, got {quality.EnvironmentBand}");
            Assert.True(quality.MoraleRestorationBonus < 0,
                "Disturbed sleep should inflict morale penalty");
        }

        // ── 4. Soundproofed, spacious, compliant dark room achieves DeepSanctuary or Restful ──
        [Fact]
        public void EvaluateSleepQuality_AchievesRestfulOrSanctuary_UnderOptimalConditions()
        {
            var quarter = new SleepingQuarterState
            {
                RoomId = "sanctuary-01",
                AmbientNoiseLevelDecibels = 40,
                WallSoundproofingPermille = 950,
                DoorSoundproofingPermille = 900,
                AssignedOccupants = 1,
                RoomAreaSquareMetres = 16,
                DarknessQualityPermille = 950,
                HasSensoryReliefKit = true
            };

            var quality = SleepAcousticRestEngine.EvaluateSleepQuality(
                quarter: quarter,
                isQuietHours: true,
                compliance: QuietHoursCompliance.Compliant,
                hoursSlept: 8);

            Assert.True(quality.EnvironmentBand >= SleepEnvironmentBand.Restful,
                $"Optimal quiet room should achieve Restful or DeepSanctuary, got {quality.EnvironmentBand}");
            Assert.True(quality.FatigueRestorationMultiplierPermille >= 1000,
                "Restful sleep should boost fatigue clearing multiplier");
            Assert.True(quality.MoraleRestorationBonus > 0,
                "Restful sleep should confer morale bonus");
        }

        // ── 5. Net fatigue recovery correctly applies quality multiplier ──
        [Fact]
        public void ComputeNetFatigueRecovery_AppliesMultiplierCorrectly()
        {
            int baseRecovery = 50;
            int normalRecovery = SleepAcousticRestEngine.ComputeNetFatigueRecovery(baseRecovery, 1000);
            int boostedRecovery = SleepAcousticRestEngine.ComputeNetFatigueRecovery(baseRecovery, 1500);
            int degradedRecovery = SleepAcousticRestEngine.ComputeNetFatigueRecovery(baseRecovery, 500);

            Assert.Equal(50, normalRecovery);
            Assert.Equal(75, boostedRecovery);
            Assert.Equal(25, degradedRecovery);
        }
    }
}
