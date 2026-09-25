// SPDX-License-Identifier: MIT
// Alpha feature G5 — quiet-hours tradeoff projection: the published floor,
// violation detection, and the observed breach count all come from the one
// noise authority (no duplicated formula in the panel).

using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.QuietHours
{
    public sealed class QuietHoursTradeoffTests
    {
        [Fact]
        public void WouldViolateQuietHours_Only_When_Active_In_Window_And_Above_Floor()
        {
            var noise = new ShelterNoiseSystem();
            noise.RegisterRoom("workshop", 0f, 0f);
            noise.AddNoiseSource(NoiseSourceType.Machinery, "workshop", 80f, NoiseFrequency.Medium);
            noise.TickDay(1, 12); // compute overall noise at midday

            // Quiet hours disabled → never breaching.
            Assert.False(noise.WouldViolateQuietHours(23));

            noise.SetQuietHours(true, 22, 6);
            Assert.True(noise.OverallNoiseLevel > ShelterNoiseSystem.QuietHoursNoiseFloorDb);
            Assert.True(noise.WouldViolateQuietHours(23));   // inside the window
            Assert.False(noise.WouldViolateQuietHours(12));  // outside the window
        }

        [Fact]
        public void TickDay_Records_Breach_And_Counts_It()
        {
            var noise = new ShelterNoiseSystem();
            noise.RegisterRoom("workshop", 0f, 0f);
            noise.AddNoiseSource(NoiseSourceType.Machinery, "workshop", 80f, NoiseFrequency.Medium);
            noise.SetQuietHours(true, 22, 6);

            Assert.Equal(0, noise.QuietHoursViolationCount);
            Assert.Null(noise.LatestQuietHoursViolation);

            noise.TickDay(3, 23);

            Assert.Equal(1, noise.QuietHoursViolationCount);
            var last = noise.LatestQuietHoursViolation;
            Assert.NotNull(last);
            Assert.Equal(3, last!.Day);
            Assert.Equal(ShelterNoiseSystem.QuietHoursViolationRisk, last.DetectionRiskAdded, 3);
        }

        [Fact]
        public void A_Silent_Shelter_Does_Not_Breach_The_Published_Floor()
        {
            var noise = new ShelterNoiseSystem();
            noise.RegisterRoom("dorm", 0f, 0f);
            noise.SetQuietHours(true, 22, 6);
            noise.TickDay(5, 23);

            Assert.False(noise.WouldViolateQuietHours(23));
            Assert.Equal(0, noise.QuietHoursViolationCount);
        }
    }
}
