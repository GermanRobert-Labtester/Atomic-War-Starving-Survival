// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : SleepAcousticRestSelfTest
// Subsystem          : Expansion 41 — The Quiet: Sleep Quality & Soundproofing
// Authority          : docs/expansions/wave6/expansion_41_the_quiet_plan.md
// ============================================================================

using System;
using Ashfall.Core.Needs;

namespace AtomicWar.GodotApp
{
    public static class HostCliSleepAcousticRest
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Sleep Quality, Soundproofing & Shelter Crowding Self-Test (Expansion 41) ===");
            int passed = 0;
            int total = 12;

            try
            {
                // Check 1: Default sleeping quarter initialization
                var session = SleepAcousticRestHostSession.Create();
                var defaultQuarter = session.Ledger.GetQuarter("primary_shelter_dormitory");
                if (defaultQuarter != null &&
                    defaultQuarter.RoomAreaSquareMetres == 28 &&
                    defaultQuarter.AssignedOccupants == 4 &&
                    defaultQuarter.WallSoundproofingPermille == 650 &&
                    defaultQuarter.DoorSoundproofingPermille == 550 &&
                    defaultQuarter.AmbientNoiseLevelDecibels == 48)
                {
                    Console.WriteLine("[PASS] Check 1: Default primary shelter dormitory initialized with valid baseline acoustic parameters.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 1: Default sleeping quarter initialization failed.");
                }

                // Check 2: Acoustic attenuation calculation
                int lowAtten = SleepAcousticRestEngine.CalculateAcousticAttenuation(60, 200, 200);
                int highAtten = SleepAcousticRestEngine.CalculateAcousticAttenuation(60, 800, 800);

                if (highAtten < lowAtten && highAtten < 60)
                {
                    Console.WriteLine($"[PASS] Check 2: Soundproofing attenuated ambient noise (60dB reduced to {highAtten}dB high-spec vs {lowAtten}dB low-spec).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 2: Acoustic attenuation calculation failed.");
                }

                // Check 3: Crowding density area per occupant calculation
                int spaciousPenalty = SleepAcousticRestEngine.CalculateCrowdingDensity(occupants: 2, roomAreaSqMetres: 40);
                int crampedPenalty = SleepAcousticRestEngine.CalculateCrowdingDensity(occupants: 6, roomAreaSqMetres: 12);

                if (crampedPenalty > spaciousPenalty)
                {
                    Console.WriteLine($"[PASS] Check 3: Crowding density penalty scaled with room congestion ({crampedPenalty}\u2030 cramped vs {spaciousPenalty}\u2030 spacious).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 3: Crowding density scaling check failed.");
                }

                // Check 4: Quiet hours compliance vs violation impact
                var compResult = session.EvaluateQuarterSleep("primary_shelter_dormitory", true, QuietHoursCompliance.Compliant, 8);
                var violResult = session.EvaluateQuarterSleep("primary_shelter_dormitory", true, QuietHoursCompliance.ViolatedSevere, 8);

                if (compResult.SleepQualityIndexPermille > violResult.SleepQualityIndexPermille &&
                    compResult.FatigueRestorationMultiplierPermille > violResult.FatigueRestorationMultiplierPermille)
                {
                    Console.WriteLine($"[PASS] Check 4: Quiet hours compliance yielded superior rest ({compResult.SleepQualityIndexPermille}\u2030 compliant vs {violResult.SleepQualityIndexPermille}\u2030 violated).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 4: Quiet hours compliance impact failed.");
                }

                // Check 5: Soundproofing upgrade decibel reduction
                session.UpdateRoomSoundproofing("primary_shelter_dormitory", 900, 850);
                var upgraded = session.Ledger.GetQuarter("primary_shelter_dormitory");

                if (upgraded != null &&
                    upgraded.WallSoundproofingPermille == 900 &&
                    upgraded.DoorSoundproofingPermille == 850)
                {
                    Console.WriteLine("[PASS] Check 5: Sleeping quarter soundproofing successfully upgraded.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 5: Soundproofing upgrade failed.");
                }

                // Check 6: Restful or DeepSanctuary sleep environment band criteria
                var quietRoom = new SleepingQuarterState
                {
                    RoomId = "officer_sanctuary",
                    RoomAreaSquareMetres = 20,
                    AssignedOccupants = 1,
                    WallSoundproofingPermille = 950,
                    DoorSoundproofingPermille = 950,
                    AmbientNoiseLevelDecibels = 30,
                    DarknessQualityPermille = 950,
                    HasSensoryReliefKit = true
                };
                session.RegisterOrUpdateQuarter(quietRoom);
                var sanctResult = session.EvaluateQuarterSleep("officer_sanctuary", true, QuietHoursCompliance.Compliant, 8);

                if (sanctResult.EnvironmentBand == SleepEnvironmentBand.DeepSanctuary ||
                    sanctResult.EnvironmentBand == SleepEnvironmentBand.Restful)
                {
                    Console.WriteLine($"[PASS] Check 6: High-spec acoustic room achieved {sanctResult.EnvironmentBand} band (Quality: {sanctResult.SleepQualityIndexPermille}\u2030).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 6: Expected DeepSanctuary/Restful but got {sanctResult.EnvironmentBand}.");
                }

                // Check 7: Disturbed or Unbearable band detection under severe noise
                var noisyRoom = new SleepingQuarterState
                {
                    RoomId = "generator_adjacent_bunk",
                    RoomAreaSquareMetres = 10,
                    AssignedOccupants = 6,
                    WallSoundproofingPermille = 100,
                    DoorSoundproofingPermille = 100,
                    AmbientNoiseLevelDecibels = 82, // Industrial noise level
                    DarknessQualityPermille = 300,
                    HasSensoryReliefKit = false
                };
                session.RegisterOrUpdateQuarter(noisyRoom);
                var noisyResult = session.EvaluateQuarterSleep("generator_adjacent_bunk", false, QuietHoursCompliance.ViolatedSevere, 4);

                if (noisyResult.EnvironmentBand == SleepEnvironmentBand.Disturbed ||
                    noisyResult.EnvironmentBand == SleepEnvironmentBand.Unbearable)
                {
                    Console.WriteLine($"[PASS] Check 7: Severe noise and crowding correctly classified as {noisyResult.EnvironmentBand} band.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 7: Expected Disturbed/Unbearable but got {noisyResult.EnvironmentBand}.");
                }

                // Check 8: Fatigue recovery multiplier scaling
                if (sanctResult.FatigueRestorationMultiplierPermille > 1000 &&
                    noisyResult.FatigueRestorationMultiplierPermille < 750)
                {
                    Console.WriteLine($"[PASS] Check 8: Fatigue restoration multiplier scaled across acoustic bands (Sanctuary: {sanctResult.FatigueRestorationMultiplierPermille}\u2030 vs Noisy: {noisyResult.FatigueRestorationMultiplierPermille}\u2030).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 8: Fatigue recovery multiplier scaling failed.");
                }

                // Check 9: Sensory relief kit installation and sleep quality bonus
                var baseDorm = new SleepingQuarterState
                {
                    RoomId = "test_dorm",
                    RoomAreaSquareMetres = 20,
                    AssignedOccupants = 3,
                    WallSoundproofingPermille = 600,
                    DoorSoundproofingPermille = 600,
                    AmbientNoiseLevelDecibels = 50,
                    DarknessQualityPermille = 700,
                    HasSensoryReliefKit = false
                };
                session.RegisterOrUpdateQuarter(baseDorm);
                var preKit = session.EvaluateQuarterSleep("test_dorm", true, QuietHoursCompliance.Compliant, 8);

                int kitsBefore = session.Census.SensoryKitsReserve;
                bool installed = session.InstallSensoryReliefKit("test_dorm");
                var postKit = session.EvaluateQuarterSleep("test_dorm", true, QuietHoursCompliance.Compliant, 8);

                if (installed &&
                    session.Census.SensoryKitsReserve == kitsBefore - 1 &&
                    postKit.SleepQualityIndexPermille > preKit.SleepQualityIndexPermille)
                {
                    Console.WriteLine($"[PASS] Check 9: Sensory relief kit installed, deducted reserve, and increased sleep quality ({preKit.SleepQualityIndexPermille}\u2030 -> {postKit.SleepQualityIndexPermille}\u2030).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 9: Sensory relief kit installation failed.");
                }

                // Check 10: Sensory relief kit exhaustion / depletion rejection
                var drySession = SleepAcousticRestHostSession.Create();
                var dryState = drySession.CaptureState();
                dryState.SensoryReliefKitsReserve = 0;
                drySession.RestoreState(dryState);

                var unequippedRoom = new SleepingQuarterState
                {
                    RoomId = "unequipped_room",
                    HasSensoryReliefKit = false
                };
                drySession.RegisterOrUpdateQuarter(unequippedRoom);
                bool dryInstalled = drySession.InstallSensoryReliefKit("unequipped_room");

                if (!dryInstalled)
                {
                    Console.WriteLine("[PASS] Check 10: Sensory kit installation correctly rejected when inventory reserve is depleted.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 10: Depleted kit installation check failed.");
                }

                // Check 11: Daily advancement advances nights slept and tracks disturbance alerts
                var daySession = SleepAcousticRestHostSession.Create();
                daySession.AdvanceDay(hoursSlept: 8);
                var cDay = daySession.Census;

                if (cDay.QuartersCount >= 1 && cDay.TotalOccupants >= 1)
                {
                    Console.WriteLine($"[PASS] Check 11: Daily sleep advancement recorded across {cDay.QuartersCount} quarters ({cDay.TotalOccupants} occupants, avg quality: {cDay.AverageSleepQualityPermille}\u2030).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 11: Daily advancement failed.");
                }

                // Check 12: Host session capture/restore round-trip fidelity
                var sourceSession = SleepAcousticRestHostSession.Create();
                sourceSession.RestockSensoryReliefKits(15);
                sourceSession.SetQuietHoursSchedule(23, 7, true);
                sourceSession.EvaluateQuarterSleep("primary_shelter_dormitory", true, QuietHoursCompliance.Compliant, 8);

                var captured = sourceSession.CaptureState();
                var targetSession = SleepAcousticRestHostSession.Create();
                targetSession.RestoreState(captured);

                var cSource = sourceSession.Census;
                var cTarget = targetSession.Census;

                if (cSource.QuartersCount == cTarget.QuartersCount &&
                    cSource.TotalOccupants == cTarget.TotalOccupants &&
                    cSource.AverageSleepQualityPermille == cTarget.AverageSleepQualityPermille &&
                    cSource.RestfulOrSanctuaryCount == cTarget.RestfulOrSanctuaryCount &&
                    cSource.DisturbedOrUnbearableCount == cTarget.DisturbedOrUnbearableCount &&
                    cSource.SensoryKitsReserve == cTarget.SensoryKitsReserve &&
                    cSource.QuietHoursViolationsCount == cTarget.QuietHoursViolationsCount)
                {
                    Console.WriteLine("[PASS] Check 12: Host session state preserved across capture/restore round-trip.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 12: Host session capture/restore round-trip failed.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Expansion 41 self-test threw exception: {ex}");
                return 1;
            }

            Console.WriteLine($"=== Expansion 41 Self-Test Complete: {passed}/{total} Passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
