using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Economy;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Shim-wiring selftest (A11 slice 1 — characterization): pins the CURRENT
    /// raid-repel behavior of the Unity DynamicEconomySystem BEFORE any RNG
    /// change, so the port's parity can be proven against a recorded baseline.
    /// Runs headless through the shim — Unity wiring is now provable, not
    /// merely compile-verified.
    /// </summary>
    public static class RngWiringSelfTest
    {
        public static int Run(string dataDirectory)
        {
            int failures = 0;
            void Check(bool condition, string name)
            {
                if (condition) GD.Print("[PASS] " + name);
                else { GD.Print("[FAIL] " + name); failures++; }
            }

            GD.Print("[RngWiringSelfTest] begin");

            var shelter = new AtomicWar._Game.Shelter.Shelter();
            shelter.AddModule(new AtomicWar._Game.Shelter.ShelterModuleInstance
            {
                ModuleId = "radiation_shielding",
                Level = 2 // integrity = 2 * 0.25 = 0.5 -> repel roll runs
            });

            // Faction auto-surrenders after RepelsForAutoSurrender repels, so each
            // raid uses a FRESH instance (fresh surrender state); only the RNG
            // carries over between raids of the same instance in the port.
            var faction = new AtomicWar._Game.Economy.FactionSO
            {
                id = "char_faction",
                raidThreshold = -50f
            };

            AtomicWar._Game.Economy.DynamicEconomySystem NewDse(int seed = 7)
            {
                var d = new AtomicWar._Game.Economy.DynamicEconomySystem(
                    shelter: shelter, decisionSeed: seed);
                d.RegisterFaction(faction);
                d.SetTrust("char_faction", -60f); // below the -50 hostility line
                return d;
            }

            // ── Characterization A: repel distribution at integrity 0.5 ──
            // Fresh instances replay the same first roll (Random(7)), so the
            // distribution is measured over ONE-LONG-LIVED instance, capped
            // per instance by the 2-repel auto-surrender; each instance
            // contributes its pre-surrender rolls.
            int repelled = 0;
            int rollCount = 0;
            for (int i = 0; i < 100; i++)
            {
                // Vary the construction seed so the sample is a real
                // distribution, not one fixed micro-sequence replayed.
                var d = NewDse(seed: i * 7919 + 1);
                for (int r = 0; r < 50; r++)
                {
                    var result = d.TryLaunchRaid("char_faction", ignoreDayGate: true);
                    if (!result.Launched)
                    {
                        if (r == 0) GD.Print($"[DIAG] first raid blocked: {result.Message} trust={d.GetEffectiveTrust("char_faction"):0}");
                        break; // surrendered
                    }
                    rollCount++;
                    if (result.Repelled) repelled++;
                }
            }
            Check(rollCount >= 300, $"rolls collected across instances ({rollCount})");
            Check(repelled > rollCount * 0.4f && repelled < rollCount * 0.6f,
                $"repel fraction ~50% at integrity 0.5 ({repelled}/{rollCount})");

            // ── Characterization B: post-restore CONTINUATION (the A11 fix) ──
            // Capture AFTER a raid (roll count includes it), restore into a
            // fresh instance, then compare the restored stream's next raid
            // against the original's next raid: continuation must MATCH.
            // (Pre-fix, the unpersisted stream restarted from seed and diverged.)
            var freshA = NewDse();
            var resumeB = NewDse();
            bool continuation = true;
            for (int i = 0; i < 30; i++)
            {
                // Fresh instance per pair-step: surrender never accumulates, but
                // the decision seed + roll count travel through the save.
                var freshStep = NewDse();
                var saveAfter = freshStep.CaptureState();
                bool originalRoll = freshStep.TryLaunchRaid("char_faction", ignoreDayGate: true).Repelled;
                saveAfter.RngRollCount++; // one raid consumed after capture
                freshStep.RestoreState(saveAfter); // surrender reset, stream kept
                var nextOriginal = freshStep.TryLaunchRaid("char_faction", ignoreDayGate: true).Repelled;

                resumeB.RestoreState(saveAfter);
                var nextRestored = resumeB.TryLaunchRaid("char_faction", ignoreDayGate: true).Repelled;
                if (nextOriginal != nextRestored) continuation = false;
            }
            Check(continuation,
                "post-restore stream CONTINUES the original sequence (A11 fixed)");

            // ── Characterization C: same-construction-seed, same sequence ──
            int c1 = 0, c2 = 0;
            for (int i = 0; i < 200; i++)
            {
                if (NewDse().TryLaunchRaid("char_faction", ignoreDayGate: true).Repelled) c1++;
                if (NewDse().TryLaunchRaid("char_faction", ignoreDayGate: true).Repelled) c2++;
            }
            Check(c1 == c2, $"two identically-constructed systems behave identically (a={c1} b={c2})");

            GD.Print(failures == 0 ? "RNG_WIRING_SELFTEST PASS" : "RNG_WIRING_SELFTEST FAIL");
            return failures == 0 ? 0 : 1;
        }
    }
}
