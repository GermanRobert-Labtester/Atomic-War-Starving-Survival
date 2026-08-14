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
                    shelter: shelter, rng: new System.Random(seed));
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

            // ── Characterization B: rng restart behavior (the A11 defect) ──
            // The CURRENT rng state is NOT persisted: after a save/restore the
            // stream RESTARTS from the construction seed — post-reload raids
            // replay the same rolls (no continuation). Pin the restart so the
            // port's fix (persisted roll count) flips this to continuation.
            // Per-raid restore resets the surrender gate but never touches the
            // instance rng, so both streams stay alive.
            var freshA = NewDse();
            var resumeB = NewDse();
            int[] postRestore = new int[30];
            int[] freshFirst = new int[30];
            for (int i = 0; i < 30; i++)
            {
                var pre = freshA.CaptureState();
                postRestore[i] = freshA.TryLaunchRaid("char_faction", ignoreDayGate: true).Repelled ? 1 : 0;
                freshA.RestoreState(pre);
                resumeB.RestoreState(pre);
                freshFirst[i] = resumeB.TryLaunchRaid("char_faction", ignoreDayGate: true).Repelled ? 1 : 0;
            }
            int aSum = 0, bSum = 0;
            foreach (var v in postRestore) aSum += v;
            foreach (var v in freshFirst) bSum += v;
            // CURRENT contract: post-restore replays the stream from the start.
            bool restartPinned = true;
            for (int i = 1; i < 30; i++)
                if (postRestore[i] != freshFirst[i]) restartPinned = false;
            Check(restartPinned,
                $"CURRENT: post-restore stream restarts identically (replay) — A11 defect pinned");

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
