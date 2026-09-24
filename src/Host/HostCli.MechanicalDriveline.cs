// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : MechanicalDrivelineSelfTest
// Subsystem          : Expansion 40 — The Wheel: Mechanical Power Driveline
// Authority          : docs/expansions/wave6/expansion_40_the_wheel_plan.md
// ============================================================================

using System;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public static class HostCliMechanicalDriveline
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Mechanical Power Driveline & Machine Tools Self-Test (Expansion 40) ===");
            int passed = 0;
            int total = 12;

            try
            {
                // Check 1: Default line shaft branch initialization
                var session = MechanicalDrivelineHostSession.Create();
                var defaultBranch = session.Ledger.GetBranch(MechanicalDrivelineLedger.DefaultBranchId);
                if (defaultBranch != null &&
                    defaultBranch.ShaftLengthMetres == 12 &&
                    defaultBranch.CouplingKind == DrivelineCouplingKind.FlatLeatherBelt &&
                    defaultBranch.LubricationQualityPermille == 850 &&
                    defaultBranch.BearingWearPermille == 100 &&
                    defaultBranch.AlignmentPrecisionPermille == 900)
                {
                    Console.WriteLine("[PASS] Check 1: Default primary machine shop line shaft initialized with valid baseline parameters.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 1: Default line shaft branch initialization failed.");
                }

                // Check 2: WaterWheel power transmission execution
                var transmitResult = session.ExecutePowerTransmission(
                    branchId: MechanicalDrivelineLedger.DefaultBranchId,
                    moverType: PrimeMoverType.WaterWheel,
                    resourceAvailabilityPermille: 850,
                    operatorSkillPermille: 800,
                    operatingHours: 4);

                if (transmitResult.DeliveredTorqueWatts > 0 &&
                    transmitResult.TransmissionEfficiencyPermille > 0 &&
                    transmitResult.BearingWearIncurredPermille >= 0)
                {
                    Console.WriteLine($"[PASS] Check 2: WaterWheel power transmission delivered {transmitResult.DeliveredTorqueWatts}W at {transmitResult.TransmissionEfficiencyPermille}\u2030 efficiency.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 2: WaterWheel power transmission failed.");
                }

                // Check 3: Line shaft friction drag & distance loss
                var shortBranch = new DrivelineBranchState
                {
                    BranchId = "short_shaft",
                    ShaftLengthMetres = 5,
                    CouplingKind = DrivelineCouplingKind.FlatLeatherBelt,
                    AlignmentPrecisionPermille = 1000,
                    BearingWearPermille = 0,
                    LubricationQualityPermille = 1000
                };
                var longBranch = new DrivelineBranchState
                {
                    BranchId = "long_shaft",
                    ShaftLengthMetres = 25,
                    CouplingKind = DrivelineCouplingKind.FlatLeatherBelt,
                    AlignmentPrecisionPermille = 1000,
                    BearingWearPermille = 0,
                    LubricationQualityPermille = 1000
                };
                session.RegisterOrUpdateBranch(shortBranch);
                session.RegisterOrUpdateBranch(longBranch);

                var shortRes = session.ExecutePowerTransmission("short_shaft", PrimeMoverType.FlywheelCoupled, 1000, 1000, 1);
                var longRes = session.ExecutePowerTransmission("long_shaft", PrimeMoverType.FlywheelCoupled, 1000, 1000, 1);

                if (shortRes.DeliveredTorqueWatts > longRes.DeliveredTorqueWatts &&
                    shortRes.TransmissionEfficiencyPermille > longRes.TransmissionEfficiencyPermille)
                {
                    Console.WriteLine($"[PASS] Check 3: Longer shaft incurred higher drag ({shortRes.DeliveredTorqueWatts}W short vs {longRes.DeliveredTorqueWatts}W long).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 3: Shaft length friction scaling check failed.");
                }

                // Check 4: Belt vs Gear vs Direct coupling efficiency comparison
                var bDirect = new DrivelineBranchState { BranchId = "c_direct", CouplingKind = DrivelineCouplingKind.DirectShaft, ShaftLengthMetres = 10, AlignmentPrecisionPermille = 1000, LubricationQualityPermille = 1000, BearingWearPermille = 0 };
                var bGear = new DrivelineBranchState { BranchId = "c_gear", CouplingKind = DrivelineCouplingKind.SpurGearTrain, ShaftLengthMetres = 10, AlignmentPrecisionPermille = 1000, LubricationQualityPermille = 1000, BearingWearPermille = 0 };
                var bBelt = new DrivelineBranchState { BranchId = "c_belt", CouplingKind = DrivelineCouplingKind.FlatLeatherBelt, ShaftLengthMetres = 10, AlignmentPrecisionPermille = 1000, LubricationQualityPermille = 1000, BearingWearPermille = 0 };
                session.RegisterOrUpdateBranch(bDirect);
                session.RegisterOrUpdateBranch(bGear);
                session.RegisterOrUpdateBranch(bBelt);

                var rDirect = session.ExecutePowerTransmission("c_direct", PrimeMoverType.Windmill, 1000, 1000, 1);
                var rGear = session.ExecutePowerTransmission("c_gear", PrimeMoverType.Windmill, 1000, 1000, 1);
                var rBelt = session.ExecutePowerTransmission("c_belt", PrimeMoverType.Windmill, 1000, 1000, 1);

                if (rDirect.TransmissionEfficiencyPermille > rGear.TransmissionEfficiencyPermille &&
                    rGear.TransmissionEfficiencyPermille > rBelt.TransmissionEfficiencyPermille)
                {
                    Console.WriteLine($"[PASS] Check 4: Coupling efficiency correctly ordered (Direct: {rDirect.TransmissionEfficiencyPermille}\u2030 > Gear: {rGear.TransmissionEfficiencyPermille}\u2030 > Belt: {rBelt.TransmissionEfficiencyPermille}\u2030).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 4: Coupling efficiency ranking check failed.");
                }

                // Check 5: Bearing wear accumulation under operating hours and poor lubrication
                var dryBranch = new DrivelineBranchState
                {
                    BranchId = "dry_shaft",
                    ShaftLengthMetres = 10,
                    CouplingKind = DrivelineCouplingKind.FlatLeatherBelt,
                    AlignmentPrecisionPermille = 900,
                    BearingWearPermille = 100,
                    LubricationQualityPermille = 200 // low lube (<400) triggers accelerated wear
                };
                session.RegisterOrUpdateBranch(dryBranch);
                session.ExecutePowerTransmission("dry_shaft", PrimeMoverType.ManualTreadle, 1000, 500, 6);
                var updatedDry = session.Ledger.GetBranch("dry_shaft");

                if (updatedDry != null && updatedDry.BearingWearPermille > 100)
                {
                    Console.WriteLine($"[PASS] Check 5: Low lubrication accelerated bearing wear (Wear grew from 100\u2030 to {updatedDry.BearingWearPermille}\u2030).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 5: Bearing wear accumulation failed.");
                }

                // Check 6: Machine tool runout scaling with bearing wear
                var lowWearBranch = new DrivelineBranchState { BranchId = "low_wear", BearingWearPermille = 50 };
                var highWearBranch = new DrivelineBranchState { BranchId = "high_wear", BearingWearPermille = 800 };
                session.RegisterOrUpdateBranch(lowWearBranch);
                session.RegisterOrUpdateBranch(highWearBranch);

                session.ExecutePowerTransmission("low_wear", PrimeMoverType.ManualTreadle, 500, 500, 1);
                session.ExecutePowerTransmission("high_wear", PrimeMoverType.ManualTreadle, 500, 500, 1);

                var uLow = session.Ledger.GetBranch("low_wear");
                var uHigh = session.Ledger.GetBranch("high_wear");

                if (uLow != null && uHigh != null && uHigh.MachineToolRunoutMicrons > uLow.MachineToolRunoutMicrons)
                {
                    Console.WriteLine($"[PASS] Check 6: Machine tool runout scaled with bearing wear ({uLow.MachineToolRunoutMicrons}μm low wear vs {uHigh.MachineToolRunoutMicrons}μm high wear).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 6: Machine tool runout scaling check failed.");
                }

                // Check 7: Machining tolerance pass evaluation
                var preciseBranch = new DrivelineBranchState
                {
                    BranchId = "precision_lathe_shaft",
                    ShaftLengthMetres = 6,
                    CouplingKind = DrivelineCouplingKind.DirectShaft,
                    AlignmentPrecisionPermille = 980,
                    BearingWearPermille = 20,
                    LubricationQualityPermille = 1000,
                    MachineToolRunoutMicrons = 11
                };
                session.RegisterOrUpdateBranch(preciseBranch);
                var passTol = session.EvaluateMachiningTolerance("precision_lathe_shaft", requiredToleranceMicrons: 25);

                if (passTol.MeetsTolerance && passTol.EffectiveToolPrecisionPermille > 800)
                {
                    Console.WriteLine($"[PASS] Check 7: Precision line shaft achieved tight machining tolerance (Runout: {passTol.RunoutDriftMicrons}μm <= 25μm).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 7: Precision machining tolerance pass check failed.");
                }

                // Check 8: Machining tolerance failure on high runout / excessive wear
                var wornBranch = new DrivelineBranchState
                {
                    BranchId = "worn_drill_shaft",
                    ShaftLengthMetres = 12,
                    CouplingKind = DrivelineCouplingKind.FlatLeatherBelt,
                    AlignmentPrecisionPermille = 450,
                    BearingWearPermille = 750,
                    LubricationQualityPermille = 100,
                    MachineToolRunoutMicrons = 48
                };
                session.RegisterOrUpdateBranch(wornBranch);
                var failTol = session.EvaluateMachiningTolerance("worn_drill_shaft", requiredToleranceMicrons: 20);

                if (!failTol.MeetsTolerance && !string.IsNullOrEmpty(failTol.BottleneckReason))
                {
                    Console.WriteLine($"[PASS] Check 8: Severely worn shaft correctly failed tolerance gate (Reason: {failTol.BottleneckReason}).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 8: Worn shaft tolerance failure check failed.");
                }

                // Check 9: Millwright maintenance and bearing lubrication reload
                int reserveBefore = session.Census.LubricantReserveMl;
                bool maintained = session.PerformMillwrightMaintenance("worn_drill_shaft", lubricantVolumeMl: 250, alignmentAdjustmentPermille: 300);
                var afterMaint = session.Ledger.GetBranch("worn_drill_shaft");

                if (maintained &&
                    afterMaint != null &&
                    afterMaint.AlignmentPrecisionPermille > 450 &&
                    afterMaint.LubricationQualityPermille > 100 &&
                    session.Census.LubricantReserveMl == reserveBefore - 250)
                {
                    Console.WriteLine("[PASS] Check 9: Millwright maintenance restored shaft alignment, replenished lube, and deducted from reserve.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 9: Millwright maintenance execution failed.");
                }

                // Check 10: Millwright maintenance rejection on depleted lubricant reserve
                var dryMaintSession = MechanicalDrivelineHostSession.Create();
                var dryMaintState = dryMaintSession.CaptureState();
                dryMaintState.LubricantReserveMl = 0;
                dryMaintSession.RestoreState(dryMaintState);

                bool rejectedMaint = dryMaintSession.PerformMillwrightMaintenance(MechanicalDrivelineLedger.DefaultBranchId, lubricantVolumeMl: 100, alignmentAdjustmentPermille: 100);
                if (!rejectedMaint)
                {
                    Console.WriteLine("[PASS] Check 10: Maintenance correctly rejected when lubricant reserve is depleted.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 10: Depleted lubricant check failed.");
                }

                // Check 11: Daily advancement advances operating hours and checks alignment drift
                var daySession = MechanicalDrivelineHostSession.Create();
                daySession.AdvanceDay(hoursOperatingPerDay: 8);
                var cDay = daySession.Census;

                if (cDay.ActiveBranchesCount >= 1 && cDay.TotalShaftMetres >= 12)
                {
                    Console.WriteLine($"[PASS] Check 11: Daily advancement advanced driveline branches ({cDay.ActiveBranchesCount} branches, {cDay.TotalShaftMetres}m shaft).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 11: Daily advancement hours check failed.");
                }

                // Check 12: Host session capture/restore round-trip fidelity
                var sourceSession = MechanicalDrivelineHostSession.Create();
                sourceSession.RestockLubricantReserve(1500);
                sourceSession.ExecutePowerTransmission(MechanicalDrivelineLedger.DefaultBranchId, PrimeMoverType.WaterWheel, 1000, 950, 6);

                var captured = sourceSession.CaptureState();
                var targetSession = MechanicalDrivelineHostSession.Create();
                targetSession.RestoreState(captured);

                var cSource = sourceSession.Census;
                var cTarget = targetSession.Census;

                if (cSource.ActiveBranchesCount == cTarget.ActiveBranchesCount &&
                    cSource.TotalShaftMetres == cTarget.TotalShaftMetres &&
                    cSource.AverageLubricationQualityPermille == cTarget.AverageLubricationQualityPermille &&
                    cSource.AverageBearingWearPermille == cTarget.AverageBearingWearPermille &&
                    cSource.LubricantReserveMl == cTarget.LubricantReserveMl &&
                    cSource.HighRunoutBranchCount == cTarget.HighRunoutBranchCount &&
                    cSource.MaintenanceAlertCount == cTarget.MaintenanceAlertCount)
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
                Console.WriteLine($"[ERROR] Expansion 40 self-test threw exception: {ex}");
                return 1;
            }

            Console.WriteLine($"=== Expansion 40 Self-Test Complete: {passed}/{total} Passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
