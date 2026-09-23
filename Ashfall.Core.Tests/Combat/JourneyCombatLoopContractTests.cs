// SPDX-License-Identifier: MIT
// ============================================================================
// Journey combat-loop contract — what a host driving loop MUST model.
//
// The --real-campaign-journey-selftest auto-spawned encounter was red because
// its driving loop made three assumptions the authority does not honour:
//
//   1. "There is no in-combat revive action" — FALSE. PlayerBandage clears
//      IsDowned, zeroes BleedTurnsRemaining and heals 15 HP. Without it the
//      loop stands still while an ally bleeds out over DefaultBleedTurns turns,
//      TickBleedOut kills them, and a roster death takes the campaign terminal
//      and seals the Iron Man slot.
//   2. "Retreat ends the fight" — PlayerRetreat rolls against stance mobility
//      and on failure applies injury and re-checks resolution, so a driving loop
//      must not stop just because it called retreat.
//   3. "The caller's chosen shooter is who fires" — FALSE. PlayerFire ignores
//      the caller and calls PickActiveShooter() over LivingPlayers(), which is
//      the living player roster SORTED BY ORDINAL COMBATANT ID. A loop that
//      walks raw Combatants order picks a different survivor than the engine
//      does, and therefore resupplies the wrong caliber.
//
// These tests pin the authority's real behaviour so a host probe can be written
// against evidence instead of assumption. They run in the Core test project,
// so they hold even when the host build is blocked by unrelated in-flight work.
// ============================================================================
using System;
using System.Collections.Generic;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Combat;

namespace Ashfall.Core.Tests.Combat
{
    public sealed class JourneyCombatLoopContractTests
    {
        public JourneyCombatLoopContractTests() => CombatCatalog.SeedDefaults();

        private static List<CombatantState> Roster()
        {
            // Deliberately NOT in ordinal order, so a loop that walks raw list
            // order and an engine that sorts disagree — the exact trap that
            // starved the real shooter of ammunition.
            return new List<CombatantState>
            {
                new CombatantState
                {
                    Id = "p_elena_vasquez", Name = "ELENA VASQUEZ", SurvivorId = "elena_vasquez",
                    IsPlayer = true, Health = 100, MaxHealth = 100, ArmorRating = 0.4f, CoverRating = 0.3f
                },
                new CombatantState
                {
                    Id = "p_dr_sarah_chen", Name = "DR SARAH CHEN", SurvivorId = "survivor_dr_sarah_chen",
                    IsPlayer = true, Health = 100, MaxHealth = 100, ArmorRating = 0.4f, CoverRating = 0.3f
                },
                new CombatantState
                {
                    Id = "p_gunner_mikhail", Name = "GUNNER MIKHAIL", SurvivorId = "survivor_gunner_mikhail",
                    IsPlayer = true, Health = 100, MaxHealth = 100, ArmorRating = 0.5f, CoverRating = 0.2f
                }
            };
        }

        private static List<WeaponInstanceState> Weapons()
            => new List<WeaponInstanceState>
            {
                new WeaponInstanceState
                {
                    InstanceId = "eq_journey_test_rifle", WeaponId = "weapon_assault_rifle",
                    OwnerSurvivorId = "elena_vasquez", ConditionPct = 0.9f,
                    AmmoId = "ammo_556", AmmoRemaining = 50
                },
                new WeaponInstanceState
                {
                    InstanceId = "w_p_dr_sarah_chen", WeaponId = "weapon_pipe_rifle",
                    OwnerSurvivorId = "survivor_dr_sarah_chen", ConditionPct = 0.9f,
                    AmmoId = "ammo_357", AmmoRemaining = 50
                },
                new WeaponInstanceState
                {
                    InstanceId = "w_p_gunner_mikhail", WeaponId = "weapon_pipe_rifle",
                    OwnerSurvivorId = "survivor_gunner_mikhail", ConditionPct = 0.9f,
                    AmmoId = "ammo_357", AmmoRemaining = 50
                }
            };

        /// <summary>The journey's catalog-driven spawn shape (3 enemies, catalog health).</summary>
        /// <summary>
        /// Simulated carried stock plus the host's ConsumeAmmo/ConsumeItem ports,
        /// mirroring CombatHostSession.WireRealState: rounds are drawn from stock
        /// and a driving loop tops the stock up from carried reserve.
        /// </summary>
        private sealed class CarriedStock
        {
            public readonly Dictionary<string, int> Ammo = new Dictionary<string, int>();
            public int Bandages = 3;
            public int Deaths;
            public int Heals;

            /// <summary>Mirrors the host's survivor health record, which
            /// CombatHostSession.WireRealState maintains additively
            /// (min(maxHealth, health + delta)) and returns as the NEW health.</summary>
            public readonly Dictionary<string, float> SurvivorHealth = new Dictionary<string, float>();

            public float AddHealth(string survivorId, float delta)
            {
                if (!SurvivorHealth.TryGetValue(survivorId, out float cur)) cur = 100f;
                float next = MathF.Min(100f, MathF.Max(0f, cur + delta));
                SurvivorHealth[survivorId] = next;
                return next;
            }

            public CombatHostPorts ToPorts() => new CombatHostPorts(
                damageSurvivor: (id, dmg) => { Deaths++; AddHealth(id, -dmg); return SurvivorHealth.TryGetValue(id, out float h) ? h : 0f; },
                healSurvivor: (id, heal) => { Heals++; return AddHealth(id, heal); },
                applyMoraleDelta: (_, _) => { },
                consumeAmmo: (ammoId, n) =>
                {
                    if (!Ammo.TryGetValue(ammoId, out int have)) have = 0;
                    if (have < n) return -1;
                    Ammo[ammoId] = have - n;
                    return Ammo[ammoId];
                },
                consumeItem: (itemId, n) =>
                {
                    if (!string.Equals(itemId, "bandage", StringComparison.OrdinalIgnoreCase)) return true;
                    if (Bandages < n) return false;
                    Bandages -= n;
                    return true;
                },
                grantLoot: _ => { },
                markCombatSurvived: _ => { });
        }

        private static TacticalCombatSystem BeginJourneyEncounter(int seed = 4242, CarriedStock? stock = null)
        {
            var system = new TacticalCombatSystem();
            bool ok = system.BeginEncounter(
                "enc_loc_journey_encounter_test_1",
                "exp_loc_journey_encounter_test",
                "loc_journey_encounter_test",
                "Journey Encounter Test",
                day: 1,
                seed,
                Roster(),
                Weapons(),
                enemyCount: 3,
                enemyHealth: 0,
                enemyCombatantIds: new[]
                {
                    "combatant_desperate_scavenger",
                    "combatant_desperate_scavenger",
                    "combatant_feral_mutt"
                });
            Assert.True(ok, "the journey's encounter shape must begin");
            if (stock != null) system.Ports = stock.ToPorts();
            return system;
        }

        // ── 1 — the authority's shooter selection rule is ordinal, not list order ──

        [Fact]
        public void EngineShooter_IsTheLowestOrdinalLivingArmedPlayer_NotTheFirstInList()
        {
            var system = BeginJourneyEncounter();

            // The authority's rule: LivingPlayers() sorts by ordinal Id, so
            // p_dr_sarah_chen precedes p_elena_vasquez.
            string engineShooter = EngineActiveShooterId(system);
            string rawListFirst = RawCombatantsOrderShooterId(system);

            Assert.Equal("p_dr_sarah_chen", engineShooter);
            Assert.True(rawListFirst != engineShooter,
                "a loop walking raw Combatants order must NOT pick the same survivor as the engine — "
                + "if it did, this test would no longer prove the trap exists");
            Assert.Equal("p_elena_vasquez", rawListFirst);
        }

        [Fact]
        public void EngineShooterCarriesA357_WhileTheRawListFirstCarriesA556()
        {
            var system = BeginJourneyEncounter();

            Assert.Equal("ammo_357", EngineActiveShooterAmmoId(system));
            Assert.Equal("ammo_556", RawCombatantsOrderShooterAmmoId(system));
        }

        // ── 2 — the revive action the journey comment claimed does not exist ──

        [Fact]
        public void PlayerBandage_StabilizesADownedPlayer_AndStopsTheBleedOutClock()
        {
            var stock = new CarriedStock();
            var system = BeginJourneyEncounter(4242, stock);
            var victim = system.State.Combatants.Find(c => c.Id == "p_dr_sarah_chen")!;
            var rescuer = system.State.Combatants.Find(c => c.Id == "p_gunner_mikhail")!;

            victim.IsDowned = true;
            victim.BleedTurnsRemaining = TacticalCombatSystem.DefaultBleedTurns;
            float healthBefore = victim.Health;

            var result = system.PlayerBandage(rescuer.SurvivorId, victim.SurvivorId, new SeededRng(4242));

            Assert.True(result.Success, result.Message);
            Assert.False(victim.IsDowned, "a bandaged survivor must be back on their feet");
            Assert.Equal(0, victim.BleedTurnsRemaining);
            Assert.True(victim.Health >= 15f,
                "a bandaged survivor must be stabilized back onto their feet (the authority sets them to 15 HP)");

            // And critically: the bleed-out clock that killed them is stopped.
            // Enemy fire may still hurt them, so assert on the DEATH CAUSE —
            // no bleed-out death event may name this survivor.
            for (int turn = 0; turn < TacticalCombatSystem.DefaultBleedTurns + 2; turn++)
            {
                var outcome = system.EndTurn(new SeededRng(4242 + turn));
                foreach (var evt in outcome.AddedEvents)
                    if (string.Equals(evt.Kind, "death", StringComparison.Ordinal)
                        && evt.Detail != null
                        && evt.Detail.Contains(victim.Name, StringComparison.Ordinal))
                        Assert.Fail($"a bandaged survivor bled out anyway: {evt.Detail}");
            }
        }

        [Fact]
        public void ADownedUnbandagedPlayer_BleedsOutWithinTheDefaultWindow()
        {
            // The failure the journey hit: no bandage, no intervention, dead survivor.
            var stock = new CarriedStock();
            var system = BeginJourneyEncounter(4242, stock);
            var victim = system.State.Combatants.Find(c => c.Id == "p_gunner_mikhail")!;
            victim.IsDowned = true;
            victim.BleedTurnsRemaining = TacticalCombatSystem.DefaultBleedTurns;

            for (int turn = 0; turn < TacticalCombatSystem.DefaultBleedTurns + 1; turn++)
                if (!system.State.Resolved) system.EndTurn(new SeededRng(4242 + turn));

            Assert.True(victim.Health <= 0f,
                "this is the behaviour the journey loop failed to model — an unbandaged downed player dies");
            Assert.True(victim.HasFled, "a bled-out player is removed from the living pools");
        }

        // ── 3 — retreat may fail, so a loop must not stop on the call ──

        [Fact]
        public void PlayerRetreat_ResolvesOnSuccess_AndLeavesTheFightOpenOnFailure()
        {
            var system = BeginJourneyEncounter();
            system.PlayerLastStand("survivor_dr_sarah_chen", new SeededRng(4242));
            var lastStand = system.PlayerRetreat(new SeededRng(4242));
            Assert.False(lastStand.Success);
            Assert.False(system.State.Resolved);
            Assert.Equal("You cannot flee from a last stand.", lastStand.Message);
        }

        [Fact]
        public void PlayerRetreat_CanBeRetriedWithoutEndingTheEncounterPrematurely()
        {
            // Across seeds, retreat either resolves or applies injury and leaves
            // the encounter running — never a silent no-op that a loop would
            // mistake for "the fight is over".
            int resolvedCount = 0, openCount = 0;
            for (int seed = 0; seed < 40; seed++)
            {
                var system = BeginJourneyEncounter(seed);
                var result = system.PlayerRetreat(new SeededRng(seed));
                if (system.State.Resolved) resolvedCount++;
                else if (result.Success) openCount++;
            }

            Assert.True(resolvedCount > 0, "retreat must sometimes extract cleanly");
            Assert.True(openCount + resolvedCount == 40,
                "every retreat attempt must either resolve or report a disrupted withdrawal");
        }

        // ── 4 — the corrected driving loop actually finishes the journey fight ──

        [Fact]
        public void CorrectedDrivingLoop_ResolvesTheJourneyEncounter_WithoutKillingAPlayer()
        {
            // Exactly the pattern now in Main.UiTests.RealCampaignJourney.cs:
            //   * resupply the caliber the ENGINE will consume (PickActiveShooter
            //     over ordinal-sorted LivingPlayers),
            //   * bandage a downed ally with carried reserve,
            //   * withdraw when no standing armed survivor remains, retrying
            //     while the encounter stays open.
            var stock = new CarriedStock();
            var system = BeginJourneyEncounter(4242, stock);
            var rng = new SeededRng(4242);
            int guard = 0, retreats = 0, bandages = 0, shots = 0, jamClears = 0;

            while (!system.State.Resolved && guard++ < 400)
            {
                var enemy = system.State.Combatants.Find(c => !c.IsPlayer && !c.HasFled);
                if (enemy == null) break;

                string? engineShooterId = EngineActiveShooterId(system);
                if (engineShooterId == null)
                {
                    system.PlayerRetreat(rng);
                    if (system.State.Resolved) break;
                    system.EndTurn(rng);
                    if (++retreats > 8) break;
                    continue;
                }

                var downed = system.State.Combatants.Find(
                    c => c.IsPlayer && c.IsDowned && !c.HasFled && c.BleedTurnsRemaining > 0
                         && !string.Equals(c.Id, engineShooterId, StringComparison.Ordinal));
                if (downed != null)
                {
                    if (stock.Bandages < 1) stock.Bandages += 3;
                    var before = downed.IsDowned;
                    system.PlayerBandage(engineShooterId, downed.SurvivorId, rng);
                    if (before && !downed.IsDowned) bandages++;
                    if (!system.State.Resolved) system.EndTurn(rng);
                    continue;
                }

                var shooterWeapon = system.State.Weapons.Find(w => w.InstanceId == EngineShooterWeaponInstanceId(system));
                if (shooterWeapon != null && shooterWeapon.IsJammed)
                {
                    system.PlayerClearJam(engineShooterId, rng);
                    jamClears++;
                }
                else
                {
                    string ammoId = shooterWeapon?.AmmoId ?? "ammo_357";
                    if (!stock.Ammo.TryGetValue(ammoId, out int have)) have = 0;
                    if (have < 5) stock.Ammo[ammoId] = have + 20;
                    system.PlayerFire(enemy.Id, rng);
                    shots++;
                }

                if (!system.State.Resolved) system.EndTurn(rng);
            }

            Assert.True(system.State.Resolved,
                $"combat must resolve — guard={guard} shots={shots} bandages={bandages} "
                + $"jamClears={jamClears} retreats={retreats}");
            Assert.True(guard < 400, "the loop must not spin to its guard limit");
            Assert.True(shots > 0, "the loop must actually have shot at something");
            Assert.True(system.State.Phase == (int)Ashfall.Core.Combat.CombatPhase.Won,
                $"the bandaging, resupplying loop must WIN the journey encounter, not draw it — "
                + $"phase={(Ashfall.Core.Combat.CombatPhase)system.State.Phase}: {system.State.OutcomeText}");

            foreach (var c in system.State.Combatants)
                if (c.IsPlayer && !c.HasFled)
                    Assert.True(c.Health > 0f,
                        $"player '{c.Id}' died — the journey loop must model bandage/retreat, "
                        + "otherwise a routable encounter goes terminal and seals the Iron Man slot");
        }

        [Fact]
        public void HardcodedAmmo556Loop_StarvesTheEngineShooter_AndNeverResolves()
        {
            // The literal bug: supply only the raw-list-first survivor's caliber.
            // The engine consumes the ordinal-first survivor's caliber, which is
            // different, so every shot fails and the fight never ends.
            // Stock only the caliber the RAW-LIST-FIRST survivor carries, exactly
            // as the hardcoded loop did. The engine's shooter needs .357.
            var stock = new CarriedStock();
            stock.Ammo["ammo_556"] = 400;
            var system = BeginJourneyEncounter(4242, stock);
            var rng = new SeededRng(4242);
            int failedForAmmo = 0, firedOk = 0;

            int guard = 0;
            while (!system.State.Resolved && guard++ < 12)
            {
                var enemy = system.State.Combatants.Find(c => !c.IsPlayer && !c.HasFled);
                if (enemy == null) break;
                var result = system.PlayerFire(enemy.Id, rng);
                if (result.Message != null
                    && result.Message.Contains("ammunition", StringComparison.OrdinalIgnoreCase))
                    failedForAmmo++;
                else if (result.Success)
                    firedOk++;
                if (!system.State.Resolved) system.EndTurn(rng);
            }

            Assert.True(failedForAmmo > 0,
                "a shooter whose caliber is not stocked must fail to fire — this is the starvation the "
                + "hardcoded-caliber loop caused for the whole 401-turn guard");
            Assert.Equal(0, firedOk);
        }

        [Fact]
        public void CorrectedDrivingLoop_IsDeterministic_AcrossTwoIdenticalRuns()
        {
            string RunOnce()
            {
                var stock = new CarriedStock();
                var system = BeginJourneyEncounter(4242, stock);
                var rng = new SeededRng(4242);
                var fingerprint = new System.Text.StringBuilder();

                int guard = 0, retreats = 0;
                while (!system.State.Resolved && guard++ < 400)
                {
                    var enemy = system.State.Combatants.Find(c => !c.IsPlayer && !c.HasFled);
                    if (enemy == null) break;

                    string? shooterId = EngineActiveShooterId(system);
                    if (shooterId == null)
                    {
                        system.PlayerRetreat(rng);
                        if (system.State.Resolved) break;
                        system.EndTurn(rng);
                        if (++retreats > 8) break;
                        continue;
                    }

                    var downed = system.State.Combatants.Find(
                        c => c.IsPlayer && c.IsDowned && !c.HasFled && c.BleedTurnsRemaining > 0
                             && !string.Equals(c.Id, shooterId, StringComparison.Ordinal));
                    if (downed != null)
                    {
                        if (stock.Bandages < 1) stock.Bandages += 3;
                        system.PlayerBandage(shooterId, downed.SurvivorId, rng);
                        if (!system.State.Resolved) system.EndTurn(rng);
                        continue;
                    }

                    var weapon = system.State.Weapons.Find(w => w.InstanceId == EngineShooterWeaponInstanceId(system));
                    if (weapon != null && weapon.IsJammed) system.PlayerClearJam(shooterId, rng);
                    else
                    {
                        string ammoId = weapon?.AmmoId ?? "ammo_357";
                        if (!stock.Ammo.TryGetValue(ammoId, out int have)) have = 0;
                        if (have < 5) stock.Ammo[ammoId] = have + 20;
                        system.PlayerFire(enemy.Id, rng);
                    }

                    if (!system.State.Resolved) system.EndTurn(rng);
                }

                foreach (var c in system.State.Combatants)
                    fingerprint.Append($"{c.Id}:{c.Health:0.##}:{c.IsDowned}:{c.HasFled};");
                fingerprint.Append($"|R={system.State.Resolved}:P={system.State.Phase}:T={system.State.Turn}"
                    + $":D={stock.Deaths}:O=\"{system.State.OutcomeText}\"");
                return fingerprint.ToString();
            }

            Assert.Equal(RunOnce(), RunOnce());
        }

        // ── helpers mirroring the authority, for the assertions above ──

        private static string? EngineActiveShooterId(TacticalCombatSystem system)
        {
            var players = system.State.Combatants.FindAll(c => c.IsPlayer && !c.HasFled);
            players.Sort((a, b) => string.CompareOrdinal(a.Id, b.Id));
            foreach (var c in players)
                if (!c.IsDowned && !string.IsNullOrEmpty(c.WeaponInstanceId)) return c.Id;
            return null;
        }

        private static string? EngineShooterWeaponInstanceId(TacticalCombatSystem system)
        {
            string? id = EngineActiveShooterId(system);
            if (id == null) return null;
            var c = system.State.Combatants.Find(x => x.Id == id);
            return c?.WeaponInstanceId;
        }

        private static string EngineActiveShooterAmmoId(TacticalCombatSystem system)
        {
            var instanceId = EngineShooterWeaponInstanceId(system);
            var weapon = system.State.Weapons.Find(w => w.InstanceId == instanceId);
            return weapon?.AmmoId ?? "(none)";
        }

        private static string? RawCombatantsOrderShooterId(TacticalCombatSystem system)
        {
            var shooter = system.State.Combatants.Find(
                c => c.IsPlayer && !c.IsDowned && !c.HasFled && !string.IsNullOrEmpty(c.WeaponInstanceId));
            return shooter?.Id;
        }

        private static string RawCombatantsOrderShooterAmmoId(TacticalCombatSystem system)
        {
            var shooter = system.State.Combatants.Find(
                c => c.IsPlayer && !c.IsDowned && !c.HasFled && !string.IsNullOrEmpty(c.WeaponInstanceId));
            var weapon = system.State.Weapons.Find(w => w.InstanceId == shooter?.WeaponInstanceId);
            return weapon?.AmmoId ?? "(none)";
        }
    }
}
