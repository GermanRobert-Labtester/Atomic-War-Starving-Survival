// SPDX-License-Identifier: MIT
// P013 — the day-1 acute-radiation state is flagged, teachable and recoverable.
//
// The tutorial survivor is seeded with HasAcuteRadiationSickness = true at a
// dose below the acute threshold. These pin the two facts that matter:
//   * a sub-threshold dose never applies the acute health loss (so the seed is
//     not a silent death sentence), and
//   * anti-rad treatment (or an explicit dose set) that brings the dose back
//     below the threshold clears the acute status instead of leaving the
//     survivor permanently "acute".
using Xunit;
using Ashfall.Core.Radiation;

namespace Ashfall.Core.Tests.AcuteRadiationTests
{
    public sealed class AcuteSicknessResolutionTests
    {
        private static SurvivorRadState Make(float dose, bool acute)
            => new SurvivorRadState
            {
                Id = "survivor_gunner_mikhail",
                RadiationDose = dose,
                HasAcuteRadiationSickness = acute,
                IsAlive = true
            };

        [Fact]
        public void SeededSubthresholdAcuteState_AppliesNoAcuteHealthLoss()
        {
            int healthCalls = 0;
            var sys = new RadiationSystem(
                applyNeed: (_, need, _) => { if (need == "health") healthCalls++; });
            var s = Make(dose: 15f, acute: true); // the authored day-1 start
            sys.Register(s);

            sys.Expose(s, 1f, 1f); // dose 16 — still below AcuteThreshold (80)
            Assert.Equal(0, healthCalls);
        }

        [Fact]
        public void AntiRad_BelowThreshold_ClearsAcuteSickness()
        {
            var sys = new RadiationSystem();
            var s = Make(dose: 15f, acute: true);
            sys.Register(s);
            Assert.True(s.HasAcuteRadiationSickness);

            sys.AdministerAntiRad(s, 40f); // 15 -> 0
            Assert.Equal(0f, s.RadiationDose);
            Assert.False(s.HasAcuteRadiationSickness);
        }

        [Fact]
        public void AntiRad_StillAboveThreshold_KeepsAcuteSickness()
        {
            var sys = new RadiationSystem();
            var s = Make(dose: 95f, acute: true);
            sys.Register(s);

            sys.AdministerAntiRad(s, 10f); // 85 >= 80
            Assert.True(s.HasAcuteRadiationSickness);
        }

        [Fact]
        public void SetDoseBelowThreshold_Clears_AndExposureRegrants()
        {
            var sys = new RadiationSystem();
            var s = Make(dose: 90f, acute: true);
            sys.Register(s);

            sys.SetDose(s, 0f);
            Assert.False(s.HasAcuteRadiationSickness);

            sys.Expose(s, 90f, 1f); // dose 90 >= 80 re-grants through GrantStatus
            Assert.True(s.HasAcuteRadiationSickness);
        }
    }
}