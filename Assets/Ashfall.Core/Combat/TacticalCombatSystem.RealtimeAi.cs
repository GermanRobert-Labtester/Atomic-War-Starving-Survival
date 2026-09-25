// SPDX-License-Identifier: MIT
// DEC-358 / PFGL-RT-W3: live enemy AI special moves on the realtime clock.
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Combat
{
    public partial class TacticalCombatSystem
    {
        private const float AiThinkIntervalSeconds = 0.35f;
        private const float BurrowHideSeconds = 2.0f;
        private const float BurrowEmergeDebuffSeconds = 0.75f;
        private const float FlankBonusSeconds = 2.0f;
        private const float FlankLateralThreshold = 3.5f;
        private const float SporePeriodSeconds = 3.0f;
        private const float ChargeMeleeRadius = 1.25f;
        private const float ChargeDamage = 18f;
        private const float ChargeSelfStunSeconds = 0.6f;
        private const float ChargeCooldownSeconds = 1.4f;
        private const float EnemyDefaultFireCooldown = 0.55f;
        private const float SuppressFireCooldown = 0.18f;
        private const float PinDecayIntervalSeconds = 1.0f;
        private const float FlankAccuracyBonus = 1.25f;
        private const float BurrowEmergeAccuracyScale = 0.7f;

        private float _pinDecayAccum;

        public static bool IsBurrowHidden(CombatantState c) =>
            c != null && string.Equals(c.AiBehaviorPhase, "burrow_hide", StringComparison.Ordinal);

        private void TickRealtimeAi(CombatArenaDefinition arena, float dt, ISeededRng rng)
        {
            var enemies = LivingEnemies();
            if (enemies.Count == 0) return;

            enemies.Sort((a, b) => string.CompareOrdinal(a.Id, b.Id));
            var players = LivingPlayers();

            for (int i = 0; i < enemies.Count; i++)
            {
                var enemy = enemies[i];
                if (enemy == null || enemy.IsDowned || !enemy.PoseSeeded) continue;

                if (enemy.AiPhaseTimer > 0f)
                {
                    enemy.AiPhaseTimer = Math.Max(0f, enemy.AiPhaseTimer - dt);
                    if (enemy.AiPhaseTimer <= 0f)
                        OnAiPhaseExpired(enemy, arena);
                }

                if (enemy.AiThinkCooldown > 0f)
                    enemy.AiThinkCooldown = Math.Max(0f, enemy.AiThinkCooldown - dt);

                if (enemy.AiThinkCooldown <= 0f)
                {
                    DecideAiBehavior(enemy, arena, players, rng);
                    enemy.AiThinkCooldown = AiThinkIntervalSeconds;
                }

                SteerAiMotion(enemy, arena, players, dt);
                TryEnemyRealtimeFire(enemy, players, rng);
            }
        }

        private void TickRealtimePinDecay(float dt)
        {
            _pinDecayAccum += dt;
            while (_pinDecayAccum >= PinDecayIntervalSeconds)
            {
                _pinDecayAccum -= PinDecayIntervalSeconds;
                for (int i = 0; i < _state.Combatants.Count; i++)
                {
                    var c = _state.Combatants[i];
                    if (c == null || !c.IsPinned) continue;
                    c.PinnedTurnsRemaining--;
                    if (c.PinnedTurnsRemaining <= 0)
                        c.IsPinned = false;
                }
            }
        }

        private void OnAiPhaseExpired(CombatantState enemy, CombatArenaDefinition arena)
        {
            string phase = enemy.AiBehaviorPhase ?? string.Empty;
            if (phase == "burrow_hide")
            {
                // Reappear on a flank spine offset from current lane.
                float offset = enemy.Lane <= 0 ? 2.5f : (enemy.Lane >= 2 ? -2.5f : (enemy.PosX < arena.width * 0.5f ? 2.5f : -2.5f));
                enemy.PosX = MathfCompat.Clamp(enemy.PosX + offset, 0f, arena.width);
                enemy.PosY = MathfCompat.Clamp(enemy.PosY + 0.4f, 0f, arena.height);
                enemy.CoverRating = Math.Min(enemy.CoverRating, 0.35f);
                enemy.AiBehaviorPhase = "burrow_emerge";
                enemy.AiPhaseTimer = BurrowEmergeDebuffSeconds;
                enemy.Lane = CombatArenaCatalog.NearestLane(arena, enemy.PosX);
                AddEvent("ai_burrow_emerge", enemy.Id, enemy.Name + " surfaces off the spine.");
            }
            else if (phase == "burrow_emerge" || phase == "flank_bonus" || phase == "charge_stun")
            {
                enemy.AiBehaviorPhase = string.Empty;
            }
        }

        private void DecideAiBehavior(
            CombatantState enemy,
            CombatArenaDefinition arena,
            List<CombatantState> players,
            ISeededRng rng)
        {
            if (enemy.IsPinned) return;
            if (IsBurrowHidden(enemy)) return;

            string move = string.IsNullOrEmpty(enemy.AiSpecialMove) ? "None" : enemy.AiSpecialMove;
            switch (move)
            {
                case "Burrow":
                    if (string.IsNullOrEmpty(enemy.AiBehaviorPhase))
                    {
                        enemy.AiBehaviorPhase = "burrow_hide";
                        enemy.AiPhaseTimer = BurrowHideSeconds;
                        enemy.CoverRating = 1f;
                        enemy.VelX = 0f;
                        enemy.VelY = 0f;
                        enemy.MotionMode = (int)CombatMotionMode.Cover;
                        AddEvent("ai_burrow", enemy.Id, enemy.Name + " burrows out of sight.");
                    }
                    break;

                case "Flank":
                    // Steering handles approach; think only arms the bonus when lateral.
                    break;

                case "Spore":
                    if (enemy.AiPhaseTimer <= 0f)
                    {
                        // No ChemWarfare ownership inside Core combat — emit a host-visible event.
                        AddEvent(
                            "spore_cloud",
                            enemy.Id,
                            enemy.Name + " releases a spore cloud at lane " + enemy.Lane +
                            " (" + enemy.PosX.ToString("0.0") + "," + enemy.PosY.ToString("0.0") + ").");
                        enemy.AiPhaseTimer = SporePeriodSeconds;
                        enemy.AiBehaviorPhase = "spore_armed";
                    }
                    break;

                case "Charge":
                    enemy.AiBehaviorPhase = "charge";
                    break;

                case "SuppressiveFire":
                    enemy.AiBehaviorPhase = "suppress";
                    break;

                case "TacticalRetreat":
                    if (enemy.FleeThreshold >= 0f
                        && enemy.MaxHealth > 0f
                        && (enemy.Health / enemy.MaxHealth) <= enemy.FleeThreshold)
                    {
                        enemy.AiBehaviorPhase = "retreat";
                    }
                    else if (enemy.AiBehaviorPhase == "retreat")
                    {
                        enemy.AiBehaviorPhase = string.Empty;
                    }
                    break;

                default:
                    if (string.IsNullOrEmpty(enemy.AiBehaviorPhase))
                        enemy.AiBehaviorPhase = "hold";
                    break;
            }

            // Quiet unused rng consumption reserved for future weighted choices —
            // keeps call signature ready without nondeterminism from wall clock.
            _ = rng;
            _ = arena;
            _ = players;
        }

        private void SteerAiMotion(
            CombatantState enemy,
            CombatArenaDefinition arena,
            List<CombatantState> players,
            float dt)
        {
            if (enemy.IsPinned || IsBurrowHidden(enemy))
            {
                enemy.VelX = 0f;
                enemy.VelY = 0f;
                return;
            }

            if (enemy.AiBehaviorPhase == "charge_stun")
            {
                enemy.VelX = 0f;
                enemy.VelY = 0f;
                enemy.MotionMode = (int)CombatMotionMode.Idle;
                return;
            }

            var focus = NearestLivingPlayer(enemy, players);
            if (focus == null)
            {
                enemy.VelX = 0f;
                enemy.VelY = 0f;
                enemy.MotionMode = (int)CombatMotionMode.Idle;
                return;
            }

            string move = string.IsNullOrEmpty(enemy.AiSpecialMove) ? "None" : enemy.AiSpecialMove;
            float speed = arena.walk_speed;

            if (move == "Charge" || enemy.AiBehaviorPhase == "charge")
            {
                speed = arena.run_speed;
                enemy.MotionMode = (int)CombatMotionMode.Run;
                float dx = focus.PosX - enemy.PosX;
                float dy = focus.PosY - enemy.PosY;
                float len = (float)Math.Sqrt(dx * dx + dy * dy);
                if (len < 0.001f) len = 0.001f;
                enemy.VelX = (dx / len) * speed;
                enemy.VelY = (dy / len) * speed;
                enemy.FacingRad = (float)Math.Atan2(dy, dx);
                IntegratePose(enemy, arena, dt);
                enemy.Lane = CombatArenaCatalog.NearestLane(arena, enemy.PosX);
                return;
            }

            if (move == "Flank")
            {
                // Steer toward the outer spine opposite the player's facing/side.
                float targetX = focus.PosX < arena.width * 0.5f
                    ? Math.Min(arena.width - 1.5f, focus.PosX + FlankLateralThreshold + 1f)
                    : Math.Max(1.5f, focus.PosX - FlankLateralThreshold - 1f);
                float targetY = focus.PosY;
                float dx = targetX - enemy.PosX;
                float dy = targetY - enemy.PosY;
                float len = (float)Math.Sqrt(dx * dx + dy * dy);
                if (len > 0.15f)
                {
                    enemy.MotionMode = (int)CombatMotionMode.Walk;
                    enemy.VelX = (dx / len) * speed;
                    enemy.VelY = (dy / len) * speed;
                    enemy.FacingRad = (float)Math.Atan2(dy, dx);
                    IntegratePose(enemy, arena, dt);
                }
                else
                {
                    enemy.VelX = 0f;
                    enemy.VelY = 0f;
                    enemy.MotionMode = (int)CombatMotionMode.Idle;
                }

                float lateral = Math.Abs(enemy.PosX - focus.PosX);
                if (lateral >= FlankLateralThreshold && enemy.AiBehaviorPhase != "flank_bonus")
                {
                    enemy.AiBehaviorPhase = "flank_bonus";
                    enemy.AiPhaseTimer = FlankBonusSeconds;
                    AddEvent("ai_flank", enemy.Id, enemy.Name + " takes a flanking angle.");
                }

                enemy.Lane = CombatArenaCatalog.NearestLane(arena, enemy.PosX);
                return;
            }

            if (enemy.AiBehaviorPhase == "retreat")
            {
                // Push toward the far edge (enemy spawn side).
                float targetX = arena.width - 1.5f;
                float dx = targetX - enemy.PosX;
                enemy.MotionMode = (int)CombatMotionMode.Walk;
                enemy.VelX = Math.Sign(dx) * speed;
                enemy.VelY = 0f;
                if (Math.Abs(dx) > 0.05f)
                    enemy.FacingRad = dx > 0f ? 0f : (float)Math.PI;
                IntegratePose(enemy, arena, dt);
                enemy.Lane = CombatArenaCatalog.NearestLane(arena, enemy.PosX);
                return;
            }

            // Default / suppress / spore / hold: slow advance toward player lane spine.
            float homeX = focus.PosX + (focus.PosX < enemy.PosX ? 2.5f : -2.5f);
            float hdx = homeX - enemy.PosX;
            if (Math.Abs(hdx) > 0.4f)
            {
                enemy.MotionMode = (int)CombatMotionMode.Walk;
                enemy.VelX = Math.Sign(hdx) * speed * 0.6f;
                enemy.VelY = 0f;
                IntegratePose(enemy, arena, dt);
            }
            else
            {
                enemy.VelX = 0f;
                enemy.VelY = 0f;
                enemy.MotionMode = (int)CombatMotionMode.Idle;
            }
            enemy.Lane = CombatArenaCatalog.NearestLane(arena, enemy.PosX);
        }

        private void TryEnemyRealtimeFire(
            CombatantState enemy,
            List<CombatantState> players,
            ISeededRng rng)
        {
            if (enemy.IsPinned || IsBurrowHidden(enemy)) return;
            if (enemy.AiBehaviorPhase == "charge_stun") return;
            if (enemy.FireCooldown > 0f) return;
            if (players == null || players.Count == 0) return;

            string move = string.IsNullOrEmpty(enemy.AiSpecialMove) ? "None" : enemy.AiSpecialMove;

            // Charge resolves melee on contact instead of ranged fire.
            if (move == "Charge" || enemy.AiBehaviorPhase == "charge")
            {
                var focus = NearestLivingPlayer(enemy, players);
                if (focus == null) return;
                float dx = focus.PosX - enemy.PosX;
                float dy = focus.PosY - enemy.PosY;
                float dist = (float)Math.Sqrt(dx * dx + dy * dy);
                if (dist <= ChargeMeleeRadius)
                {
                    float dmg = ChargeDamage * (enemy.AiDamageMod > 0f ? enemy.AiDamageMod : 1f);
                    ApplyDamage(focus, dmg, enemy, false, rng);
                    AddEvent("ai_charge_hit", focus.Id, enemy.Name + " charges into " + focus.Name + ".");
                    enemy.AiBehaviorPhase = "charge_stun";
                    enemy.AiPhaseTimer = ChargeSelfStunSeconds;
                    enemy.FireCooldown = ChargeCooldownSeconds;
                    enemy.VelX = 0f;
                    enemy.VelY = 0f;
                }
                return;
            }

            var target = players[rng.Next(0, players.Count)];
            if (target == null || target.IsDowned || target.HasFled) return;

            float baseAcc = 0.50f;
            float accMod = enemy.AiAccuracyMod > 0f ? enemy.AiAccuracyMod : 1f;
            if (enemy.AiBehaviorPhase == "burrow_emerge")
                accMod *= BurrowEmergeAccuracyScale;
            if (enemy.AiBehaviorPhase == "flank_bonus")
                accMod *= FlankAccuracyBonus;
            if (move == "SuppressiveFire" || enemy.AiBehaviorPhase == "suppress")
                accMod *= 0.55f;
            if (enemy.AiBehaviorPhase == "retreat")
                accMod *= 0.65f;

            var stance = CurrentStance();
            var mods = GetStanceMods(stance);
            float acc = baseAcc * accMod * (1f - mods.Defense);
            if (acc < 0.05f) acc = 0.05f;
            if (acc > 0.95f) acc = 0.95f;

            enemy.AimRad = (float)Math.Atan2(target.PosY - enemy.PosY, target.PosX - enemy.PosX);
            enemy.FacingRad = enemy.AimRad;

            bool fleeing = target.MotionMode == (int)CombatMotionMode.Flee;
            if (rng.NextDouble() < acc)
            {
                float laneDmg = 6f + (enemy.Lane == target.Lane ? 4f : 0f);
                float dmgMod = enemy.AiDamageMod > 0f ? enemy.AiDamageMod : 1f;
                float dmg = laneDmg * dmgMod;
                if (move == "SuppressiveFire" || enemy.AiBehaviorPhase == "suppress")
                    dmg *= 0.65f;
                ApplyDamage(target, dmg, enemy, false, rng);
                string fireKind = fleeing ? "flee_hit" : "enemy_fire";
                AddEvent(fireKind, target.Id, enemy.Name + " hits " + target.Name + (fleeing ? " mid-escape." : "."));

                if (move == "SuppressiveFire" || enemy.AiBehaviorPhase == "suppress")
                {
                    target.IsPinned = true;
                    target.PinnedTurnsRemaining = Math.Max(target.PinnedTurnsRemaining, 2);
                    AddEvent("ai_suppress_pin", target.Id, enemy.Name + " pins " + target.Name + ".");
                }
            }
            else
            {
                AddEvent(fleeing ? "flee_miss" : "enemy_fire", target.Id,
                    enemy.Name + " misses " + target.Name + (fleeing ? " as they run." : "."));
            }

            enemy.FireCooldown = (move == "SuppressiveFire" || enemy.AiBehaviorPhase == "suppress")
                ? SuppressFireCooldown
                : EnemyDefaultFireCooldown;
        }

        private static CombatantState? NearestLivingPlayer(CombatantState enemy, List<CombatantState> players)
        {
            CombatantState? best = null;
            float bestDist = float.MaxValue;
            for (int i = 0; i < players.Count; i++)
            {
                var p = players[i];
                if (p == null || p.IsDowned || p.HasFled || !p.PoseSeeded) continue;
                float dx = p.PosX - enemy.PosX;
                float dy = p.PosY - enemy.PosY;
                float d2 = dx * dx + dy * dy;
                if (d2 < bestDist)
                {
                    bestDist = d2;
                    best = p;
                }
            }
            return best;
        }
    }
}
