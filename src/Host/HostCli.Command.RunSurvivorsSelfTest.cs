// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Clock;
using Ashfall.Core.Crafting;
using Ashfall.Core.Economy;
using Ashfall.Core.Endgame;
using Ashfall.Core.Events;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Flags;
using Ashfall.Core.Legacy;
using Ashfall.Core.Medical;
using Ashfall.Core.Muster;
using Ashfall.Core.Narrative;
using Ashfall.Core.Save;
using Ashfall.Core.Settings;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;
using Ashfall.Core.UtilityAI;
using Ashfall.Core.Verdict;
using Ashfall.Core.Warlords;
using Ashfall.Core.World;
using Ashfall.Core.YearOfAsh;
using AtomicWar.GodotApp.Narrative;
using AtomicWar.GodotApp.Settings;
using AtomicWar.GodotApp.UI;
using AtomicWar.GodotApp.YearOfAsh;
using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AtomicWar.GodotApp
{
    public static partial class HostCli
    {

        public static int RunSurvivorsSelfTest()
        {
            var report = SurvivorsHeadlessDemo.Run(new GodotLog());
            GD.Print(report.Summary);
            bool pass = report.Passed;
            void Check(bool condition, string name)
            {
                GD.Print(condition ? "[PASS] " + name : "[FAIL] " + name);
                pass &= condition;
            }

            try
            {
                // Host bridge gate (Loop 9 gap): equipped inventory gear must flow
                // into ExposureContext.WornGear and cut Mikhail's outside-zone dose.
                var invSession = InventoryHostSession.Create(seedWhenNoSave: true);
                invSession.Equip("hazmat_suit");
                invSession.Equip("gas_mask");

                var gearedSession = new SurvivorsHostSession();
                gearedSession.SeedDemoRoster();
                gearedSession.Inventory = invSession;

                var bareSession = new SurvivorsHostSession();
                bareSession.SeedDemoRoster();

                var mikhailGeared = gearedSession.RadStateFor("survivor_gunner_mikhail");
                var mikhailBare = bareSession.RadStateFor("survivor_gunner_mikhail");
                float gearedBefore = mikhailGeared?.RadiationDose ?? 0f;
                float bareBefore = mikhailBare?.RadiationDose ?? 0f;
                gearedSession.TickHour(2f);
                bareSession.TickHour(2f);
                float gearedDelta = (mikhailGeared?.RadiationDose ?? 0f) - gearedBefore;
                float bareDelta = (mikhailBare?.RadiationDose ?? 0f) - bareBefore;

                bool gearWorks = gearedDelta < bareDelta;
                GD.Print(gearWorks
                    ? $"[PASS] equipped gear cuts outside-zone dose (geared +{gearedDelta:F1} mSv vs bare +{bareDelta:F1} mSv)"
                    : $"[FAIL] equipped gear did not cut dose (geared +{gearedDelta:F1} vs bare +{bareDelta:F1})");
                pass &= gearWorks;
            }
            catch (Exception e)
            {
                GD.Print("[FAIL] gear-bridge probe threw: " + e.Message);
                pass = false;
            }
            try
            {
                // Save/load round-trip of the wired session: equipped gear must survive
                // a full save → restore cycle and keep protecting (Loop 1 + Invariant 3).
                var invSave = InventoryHostSession.Create(seedWhenNoSave: true);
                invSave.Equip("hazmat_suit");
                var geared = new SurvivorsHostSession();
                geared.SeedDemoRoster();
                geared.Inventory = invSave;

                var survSave = geared.CaptureSave();
                var invState = invSave.CaptureSave();

                var restoredInv = InventoryHostSession.Create(seedWhenNoSave: false);
                restoredInv.RestoreSave(invState);
                var restored = new SurvivorsHostSession();
                restored.RestoreSave(survSave);
                restored.Inventory = restoredInv;

                var mikhail = restored.RadStateFor("survivor_gunner_mikhail");
                bool stateSurvived = mikhail != null && mikhail.RadiationDose == (survSave.survivors.Find(s => s.id == "survivor_gunner_mikhail")?.radiationDose ?? -1f);
                GD.Print(stateSurvived
                    ? "[PASS] survivors state survives restore (dose, roster)"
                    : "[FAIL] survivors state lost on restore");
                pass &= stateSurvived;

                float before = mikhail?.RadiationDose ?? 0f;
                restored.TickHour(2f);
                float after = mikhail?.RadiationDose ?? 0f;
                bool gearAfterRestore = (after - before) < 1f;
                GD.Print(gearAfterRestore
                    ? $"[PASS] gear protection survives save/load (dose +{after - before:F1} mSv over 2h)"
                    : $"[FAIL] gear protection lost after save/load (dose +{after - before:F1} mSv)");
                pass &= gearAfterRestore;
            }
            catch (Exception e)
            {
                GD.Print("[FAIL] save/load round-trip probe threw: " + e.Message);
                pass = false;
            }
            try
            {
                // Defect D1 (Task #132 P1-A): RestoreSave must not leave the previous
                // campaign's needs states registered in the simulation. A leaked
                // ghost keeps decaying and can reach 0 HP, raising OnDied and
                // reporting the death of a survivor who is alive in the loaded save.
                // Core covers the mechanism; this covers the real host path, which
                // Ashfall.Core.Tests cannot reach.
                var session = new SurvivorsHostSession();
                session.SeedDemoRoster();
                int rosterSize = session.RosterState.Count;

                const string probeId = "survivor_dr_sarah_chen";
                var preRestore = session.Find(probeId);
                var saved = session.CaptureSave();

                // Push the soon-to-be-stale object to the edge of death.
                if (preRestore != null)
                {
                    preRestore.Health = 0.2f;
                    preRestore.Hunger = 99f;
                    preRestore.Thirst = 99f;
                }

                int deathsForProbe = 0;
                var allDeaths = new System.Collections.Generic.List<string>();
                session.OnSurvivorDied += (id, cause, detail) =>
                {
                    allDeaths.Add($"{id}({cause})");
                    if (string.Equals(id, probeId, StringComparison.Ordinal)) deathsForProbe++;
                };

                session.RestoreSave(saved);

                bool noGhosts = session.Needs.RegisteredCount == rosterSize;
                GD.Print(noGhosts
                    ? $"[PASS] D1: restore leaves one needs state per survivor ({session.Needs.RegisteredCount} for {rosterSize})"
                    : $"[FAIL] D1: restore leaked needs registrations ({session.Needs.RegisteredCount} registered for {rosterSize} survivors)");
                pass &= noGhosts;

                var afterRestore = session.Needs.Get(probeId);
                bool ghostEvicted = afterRestore != null && !ReferenceEquals(afterRestore, preRestore);
                GD.Print(ghostEvicted
                    ? "[PASS] D1: needs lookup resolves to the restored state, not the pre-restore object"
                    : "[FAIL] D1: needs lookup still resolves to the pre-restore object");
                pass &= ghostEvicted;

                // Advance well past the point the stale object would have died.
                //
                // Scoped to the probed survivor deliberately. Other demo survivors
                // may legitimately die in this window — survivor_gunner_mikhail is
                // seeded acuteRad at 80 HP and BuildExposure places him outside in a
                // 40 mSv/hr zone with no shielding, so his dose crosses the 80
                // AcuteThreshold within ~2h and the -5 HP/hr drain kills him around
                // hour 18. That is gameplay, not a ghost. D1's invariant is only
                // that a PRE-RESTORE object cannot announce a death for a survivor
                // whose restored state is alive.
                session.TickHour(24f);

                bool probeAlive = session.Find(probeId)?.IsAliveState == true;
                bool noGhostDeath = deathsForProbe == 0 && probeAlive;
                GD.Print(noGhostDeath
                    ? $"[PASS] D1: no stale-object death for '{probeId}' (restored state alive; unrelated deaths: {(allDeaths.Count == 0 ? "none" : string.Join(", ", allDeaths))})"
                    : $"[FAIL] D1: {deathsForProbe} death(s) reported for '{probeId}' after restore (alive={probeAlive}; all deaths: {string.Join(", ", allDeaths)})");
                pass &= noGhostDeath;
            }
            catch (Exception e)
            {
                GD.Print("[FAIL] D1 stale-restore probe threw: " + e.Message);
                pass = false;
            }
            try
            {
                // H10 contract: exercise the actual host projection, the
                // checksummed survivors section bytes, and multi-survivor identity.
                void PopulatePersistenceState(SurvivorsHostSession session, bool reverse)
                {
                    if (reverse)
                    {
                        session.AddSurvivor("survivor_beta", "Beta");
                        session.AddSurvivor("survivor_alpha", "Alpha");
                    }
                    else
                    {
                        session.AddSurvivor("survivor_alpha", "Alpha");
                        session.AddSurvivor("survivor_beta", "Beta");
                    }

                    var alpha = session.Find("survivor_alpha")!;
                    alpha.Hunger = 91.25f;
                    alpha.Thirst = 2.5f;
                    alpha.Fatigue = 77f;
                    alpha.Warmth = 18f;
                    alpha.Morale = 3f;
                    alpha.Health = 44.5f;
                    alpha.Hygiene = 7.5f;
                    alpha.WasHungerCritical = true;
                    alpha.WasThirstCritical = false;
                    alpha.WasWarmthCritical = true;
                    alpha.MaxHealthCap = 88f;
                    var alphaRad = session.RadStateFor("survivor_alpha")!;
                    alphaRad.RadiationDose = 99f;
                    alphaRad.LifetimeRadiationExposure = 512.5f;
                    alphaRad.HasRadResistance = true;
                    alphaRad.RadResistanceHoursRemaining = 2.25f;
                    alphaRad.IodineProtectionTimer = 6.5f;
                    alphaRad.HasAcuteRadiationSickness = true;
                    alphaRad.HasChronicIllness = true;
                    alphaRad.HasAcuteRadiationSyndrome = true;

                    var beta = session.Find("survivor_beta")!;
                    beta.Hunger = 0f;
                    beta.Thirst = 100f;
                    beta.Fatigue = 0f;
                    beta.Warmth = 100f;
                    beta.Morale = 100f;
                    beta.Health = 100f;
                    beta.Hygiene = 0f;
                    beta.MaxHealthCap = 100f;
                }

                var source = new SurvivorsHostSession();
                PopulatePersistenceState(source, reverse: false);
                var captured = source.CaptureSave();
                string persisted = SurvivorsSaveStore.TryCapturePersisted(captured);
                Check(!string.IsNullOrEmpty(persisted), "H10 survivors capture emits persisted checksum envelope");

                var json = new SystemTextJsonSerializer();
                var restoredEnvelope = SaveEnvelopeHelper.RestoreEnvelope<SurvivorsSaveState>(persisted, json);
                Check(restoredEnvelope.Success && restoredEnvelope.State != null,
                    "H10 survivors checksum envelope restores successfully");
                if (restoredEnvelope.Success && restoredEnvelope.State != null)
                {
                    var clean = new SurvivorsHostSession();
                    clean.RestoreSave(restoredEnvelope.State);
                    var alpha = clean.Find("survivor_alpha")!;
                    var alphaRad = clean.RadStateFor("survivor_alpha")!;
                    var beta = clean.Find("survivor_beta")!;
                    var betaRad = clean.RadStateFor("survivor_beta")!;
                    bool alphaNeeds = alpha.Hunger == 91.25f && alpha.Thirst == 2.5f
                        && alpha.Fatigue == 77f && alpha.Warmth == 18f
                        && alpha.Morale == 3f && alpha.Health == 44.5f
                        && alpha.Hygiene == 7.5f && alpha.WasHungerCritical
                        && !alpha.WasThirstCritical && alpha.WasWarmthCritical
                        && alpha.MaxHealthCap == 88f;
                    bool alphaRadiation = alphaRad.RadiationDose == 99f
                        && alphaRad.LifetimeRadiationExposure == 512.5f
                        && alphaRad.HasRadResistance
                        && alphaRad.RadResistanceHoursRemaining == 2.25f
                        && alphaRad.IodineProtectionTimer == 6.5f
                        && alphaRad.HasAcuteRadiationSickness
                        && alphaRad.HasChronicIllness
                        && alphaRad.HasAcuteRadiationSyndrome;
                    bool betaBoundary = beta.Hunger == 0f && beta.Thirst == 100f
                        && beta.Fatigue == 0f && beta.Warmth == 100f
                        && beta.Morale == 100f && beta.Health == 100f
                        && beta.Hygiene == 0f && betaRad.RadiationDose == 0f
                        && betaRad.LifetimeRadiationExposure == 0f;
                    bool identity = clean.Roster.Find("survivor_alpha")?.survivorId == "survivor_alpha"
                        && clean.Roster.Find("survivor_beta")?.survivorId == "survivor_beta"
                        && clean.Needs.Get("survivor_alpha")?.Id == "survivor_alpha"
                        && clean.RadStateFor("survivor_alpha")?.Id == "survivor_alpha";
                    Check(alphaNeeds, "H10 needs capture/restore preserves mutated fields");
                    Check(alphaRadiation, "H10 radiation capture/restore preserves full state");
                    Check(betaBoundary, "H10 zero and boundary values survive capture/restore");
                    Check(identity, "H10 restored needs/radiation remain attached to canonical survivor ids");
                    Check(clean.Needs.RegisteredCount == 2 && clean.Radiation.RegisteredCount == 2,
                        "H10 restore registers exactly one needs and radiation component per survivor");
                }

                var reordered = new SurvivorsHostSession();
                PopulatePersistenceState(reordered, reverse: true);
                string reorderedPersisted = SurvivorsSaveStore.TryCapturePersisted(reordered.CaptureSave());
                Check(persisted == reorderedPersisted,
                    "H10 insertion-order differences produce identical persisted bytes and checksum");

                var changed = source.CaptureSave();
                changed.survivors[0].health += 1f;
                string changedPersisted = SurvivorsSaveStore.TryCapturePersisted(changed);
                Check(persisted != changedPersisted,
                    "H10 meaningful persisted-field changes alter the checksum envelope");

                var mismatch = new SurvivorsSaveState
                {
                    survivors = new List<SurvivorSliceState>
                    {
                        new SurvivorSliceState { id = "survivor_alpha" }
                    },
                    roster = new SurvivorRosterState
                    {
                        entries = new List<SurvivorRosterEntry>
                        {
                            new SurvivorRosterEntry { survivorId = "survivor_beta", definitionId = "survivor_beta" }
                        }
                    }
                };
                bool mismatchRejected = false;
                try
                {
                    new SurvivorsHostSession().RestoreSave(mismatch);
                }
                catch (InvalidOperationException ex)
                {
                    mismatchRejected = ex.Message.Contains("identity", StringComparison.OrdinalIgnoreCase);
                }
                Check(mismatchRejected, "H10 mismatched roster and slice identities are rejected");
            }
            catch (Exception e)
            {
                GD.Print("[FAIL] H10 persistence contract probe threw: " + e.Message);
                pass = false;
            }

            return EmitSummary("survivors_selftest", pass, pass ? 0 : 1);
        }

    }
}
