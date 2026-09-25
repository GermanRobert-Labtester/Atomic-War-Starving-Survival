// SPDX-License-Identifier: MIT
// DEC-358 / PFGL-RT-W1: fixed-tick realtime combat clock + locomotion.
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Combat
{
    public partial class TacticalCombatSystem
    {
        public const float RealtimeSimDt = 1f / 20f;
        private const float ClimbAttachRadius = 0.85f;
        private const float StaminaRunDrainPerSecond = 0.12f;
        private const float StaminaClimbDrainPerSecond = 0.18f;
        private const float StaminaRegenPerSecond = 0.08f;

        private CombatArenaDefinition? _activeArena;

        /// <summary>
        /// Enter the DEC-358 realtime fight clock. Seeds poses from the arena catalog
        /// (or procedural default) and sets <see cref="CombatPhase.ActiveRealtime"/>.
        /// Legacy EndTurn remains available while <see cref="CombatState.RealtimeActive"/> is false.
        /// </summary>
        public bool EnableRealtime(string? arenaId = null, ISeededRng? rng = null)
        {
            if (_state == null || string.IsNullOrEmpty(_state.EncounterId) || _state.Resolved)
                return false;

            var arena = CombatArenaCatalog.GetOrDefault(arenaId);
            _activeArena = arena;
            _state.ArenaId = arena.id;
            _state.RealtimeActive = true;
            _state.Phase = (int)CombatPhase.ActiveRealtime;
            _state.SimTime = 0f;
            _state.SimTick = 0;
            SeedPoses(arena, rng);
            AddEvent("realtime_enabled", _state.EncounterId, "Realtime combat clock armed on " + arena.id + ".");
            Notify();
            return true;
        }

        public CombatArenaDefinition ActiveArena =>
            _activeArena ?? CombatArenaCatalog.GetOrDefault(_state?.ArenaId);

        /// <summary>
        /// One deterministic realtime simulation step. Host pumps fixed dt via accumulator.
        /// W1 locomotion + W2 aim/fire/reload. AI/flee land in later waves.
        /// </summary>
        public CombatActionResult TickRealtime(float dt, CombatInputFrame? input, ISeededRng? rng)
        {
            var res = new CombatActionResult();
            if (_state == null || _state.Resolved)
            {
                res.Message = "Encounter is over.";
                return res;
            }

            if (!_state.RealtimeActive || _state.Phase != (int)CombatPhase.ActiveRealtime)
            {
                res.Message = "Realtime clock is not active.";
                return res;
            }

            if (dt <= 0f)
            {
                res.Message = "Non-positive dt.";
                return res;
            }

            // Cap a single call to avoid spiral-of-death from huge hitch dt.
            if (dt > RealtimeSimDt * 5f)
                dt = RealtimeSimDt * 5f;

            var arena = ActiveArena;
            input ??= CombatInputFrame.Empty;

            TickRealtimeFlee(arena, input, dt);
            ApplyPlayerMotion(arena, input, dt);
            ApplyPlayerAim(input);
            TickFireCooldowns(dt);
            IntegrateIdleStamina(dt);
            DeriveLanesFromPoses(arena);

            if (input.Reload)
                TryRealtimeReload(input);

            if (rng != null && !_state.Resolved)
            {
                TryRealtimeFire(input, rng);
                TickRealtimeAi(arena, dt, rng);
            }

            if (!_state.Resolved)
                TickRealtimePinDecay(dt);

            _state.SimTime += dt;
            _state.SimTick++;
            res.Success = true;
            res.Message = "tick " + _state.SimTick;
            Notify();
            CheckResolution();
            return res;
        }

        /// <summary>
        /// Effective rounds-per-minute for cadence. Authored ROF wins; otherwise
        /// derive from burst so assault (~600) fires faster than a pipe rifle (~200).
        /// </summary>
        public static float EffectiveRoundsPerMinute(CombatWeaponDefinition? def)
        {
            if (def == null) return 200f;
            if (def.roundsPerMinute > 0f) return def.roundsPerMinute;
            int burst = Math.Max(1, def.burst);
            return Math.Max(120f, burst * 200f);
        }

        public static float FireCooldownSeconds(CombatWeaponDefinition? def)
        {
            float rpm = EffectiveRoundsPerMinute(def);
            if (rpm < 1f) rpm = 1f;
            return 60f / rpm;
        }

        private void SeedPoses(CombatArenaDefinition arena, ISeededRng? rng)
        {
            int playerIndex = 0;
            int enemyIndex = 0;
            for (int i = 0; i < _state.Combatants.Count; i++)
            {
                var c = _state.Combatants[i];
                if (c == null) continue;
                if (c.IsDowned)
                {
                    c.MotionMode = (int)CombatMotionMode.Downed;
                    c.PoseSeeded = true;
                    continue;
                }

                CombatArenaSpawn spawn;
                if (c.IsPlayer)
                {
                    spawn = PickSpawn(arena.player_spawns, playerIndex, c.Lane, playerSide: true, arena);
                    playerIndex++;
                }
                else
                {
                    spawn = PickSpawn(arena.enemy_spawns, enemyIndex, c.Lane, playerSide: false, arena);
                    enemyIndex++;
                }

                float jx = 0f;
                float jy = 0f;
                if (rng != null)
                {
                    jx = (float)(rng.NextDouble() * 0.6 - 0.3);
                    jy = (float)(rng.NextDouble() * 0.4 - 0.2);
                }

                c.PosX = spawn.x + jx;
                c.PosY = spawn.y + jy;
                c.VelX = 0f;
                c.VelY = 0f;
                c.FacingRad = c.IsPlayer ? 0f : (float)Math.PI;
                c.AimRad = c.FacingRad;
                c.MotionMode = (int)CombatMotionMode.Idle;
                c.Stamina01 = c.Stamina01 <= 0f ? 1f : MathfCompat.Clamp01(c.Stamina01);
                c.FireCooldown = 0f;
                c.AiThinkCooldown = 0f;
                c.AiBehaviorPhase = string.Empty;
                c.AiPhaseTimer = 0f;
                c.ExtractProgress01 = 0f;
                c.Lane = MathfCompat.Clamp(spawn.lane, 0, 2);
                c.PoseSeeded = true;
            }
        }

        private static CombatArenaSpawn PickSpawn(
            List<CombatArenaSpawn>? spawns,
            int index,
            int laneFallback,
            bool playerSide,
            CombatArenaDefinition arena)
        {
            if (spawns != null && spawns.Count > 0)
            {
                int i = index % spawns.Count;
                var s = spawns[i];
                if (s != null) return s;
            }

            float spineX = 12f;
            if (arena.lane_spines != null)
            {
                for (int i = 0; i < arena.lane_spines.Count; i++)
                {
                    if (arena.lane_spines[i] != null && arena.lane_spines[i].lane == laneFallback)
                    {
                        spineX = arena.lane_spines[i].x;
                        break;
                    }
                }
            }

            return new CombatArenaSpawn
            {
                lane = MathfCompat.Clamp(laneFallback, 0, 2),
                x = playerSide ? Math.Max(2f, spineX - 6f) : Math.Min(arena.width - 2f, spineX + 6f),
                y = 1f + index * 0.8f
            };
        }

        private void ApplyPlayerMotion(CombatArenaDefinition arena, CombatInputFrame input, float dt)
        {
            var subject = ResolveRealtimeSubject(input.SubjectId);
            if (subject == null || subject.IsDowned || subject.HasFled) return;
            // Flee steering owns motion while extract is active.
            if (subject.MotionMode == (int)CombatMotionMode.Flee) return;
            if (subject.IsPinned)
            {
                subject.VelX = 0f;
                subject.VelY = 0f;
                subject.MotionMode = (int)CombatMotionMode.Idle;
                return;
            }

            // Climb request: stick to nearest climb segment and move along it.
            if (input.Climb
                && CombatArenaCatalog.NearClimbSegment(arena, subject.PosX, subject.PosY, ClimbAttachRadius, out var segment)
                && segment != null
                && subject.Stamina01 > 0.05f)
            {
                subject.MotionMode = (int)CombatMotionMode.Climb;
                float lenX = segment.x1 - segment.x0;
                float lenY = segment.y1 - segment.y0;
                float len = (float)Math.Sqrt(lenX * lenX + lenY * lenY);
                if (len < 0.001f) len = 0.001f;
                // Project onto the segment so horizontal drift cannot break attach.
                float t = ((subject.PosX - segment.x0) * lenX + (subject.PosY - segment.y0) * lenY) / (len * len);
                if (t < 0f) t = 0f;
                else if (t > 1f) t = 1f;
                subject.PosX = segment.x0 + t * lenX;
                subject.PosY = segment.y0 + t * lenY;
                float dir = input.MoveY >= 0f ? 1f : -1f;
                // Prefer MoveY; if caller only sets Climb, still ascend.
                if (Math.Abs(input.MoveY) < 0.05f && Math.Abs(input.MoveX) < 0.05f)
                    dir = 1f;
                float speed = arena.climb_speed * dir;
                subject.VelX = (lenX / len) * speed;
                subject.VelY = (lenY / len) * speed;
                IntegratePose(subject, arena, dt);
                subject.Stamina01 = MathfCompat.Clamp01(subject.Stamina01 - StaminaClimbDrainPerSecond * dt);
                subject.Lane = CombatArenaCatalog.NearestLane(arena, subject.PosX);
                return;
            }

            // Climb held but no segment in range: do not convert the climb stick into a walk.
            if (input.Climb)
            {
                subject.VelX = 0f;
                subject.VelY = 0f;
                subject.MotionMode = (int)CombatMotionMode.Idle;
                return;
            }

            float mx = input.MoveX;
            float my = input.MoveY;
            float mag = (float)Math.Sqrt(mx * mx + my * my);
            if (mag > 1f)
            {
                mx /= mag;
                my /= mag;
                mag = 1f;
            }

            if (mag < 0.05f)
            {
                subject.VelX = 0f;
                subject.VelY = 0f;
                subject.MotionMode = (int)CombatMotionMode.Idle;
                return;
            }

            bool wantRun = input.Sprint && subject.Stamina01 > 0.05f;
            float speedCap = wantRun ? arena.run_speed : arena.walk_speed;
            subject.MotionMode = wantRun ? (int)CombatMotionMode.Run : (int)CombatMotionMode.Walk;
            subject.VelX = mx * speedCap;
            subject.VelY = my * speedCap;
            if (Math.Abs(mx) > 0.01f || Math.Abs(my) > 0.01f)
                subject.FacingRad = (float)Math.Atan2(my, mx);

            IntegratePose(subject, arena, dt);
            if (wantRun)
                subject.Stamina01 = MathfCompat.Clamp01(subject.Stamina01 - StaminaRunDrainPerSecond * dt);
            subject.Lane = CombatArenaCatalog.NearestLane(arena, subject.PosX);
        }

        private void IntegrateIdleStamina(float dt)
        {
            for (int i = 0; i < _state.Combatants.Count; i++)
            {
                var c = _state.Combatants[i];
                if (c == null || c.IsDowned) continue;
                if (c.MotionMode == (int)CombatMotionMode.Idle || c.MotionMode == (int)CombatMotionMode.Cover)
                    c.Stamina01 = MathfCompat.Clamp01(c.Stamina01 + StaminaRegenPerSecond * dt);
            }
        }

        private static void IntegratePose(CombatantState c, CombatArenaDefinition arena, float dt)
        {
            c.PosX += c.VelX * dt;
            c.PosY += c.VelY * dt;
            if (c.PosX < 0f) c.PosX = 0f;
            if (c.PosY < 0f) c.PosY = 0f;
            if (c.PosX > arena.width) c.PosX = arena.width;
            if (c.PosY > arena.height) c.PosY = arena.height;
        }

        private void DeriveLanesFromPoses(CombatArenaDefinition arena)
        {
            for (int i = 0; i < _state.Combatants.Count; i++)
            {
                var c = _state.Combatants[i];
                if (c == null || !c.PoseSeeded) continue;
                c.Lane = CombatArenaCatalog.NearestLane(arena, c.PosX);
            }
        }

        private CombatantState? ResolveRealtimeSubject(string subjectId)
        {
            if (!string.IsNullOrEmpty(subjectId))
            {
                for (int i = 0; i < _state.Combatants.Count; i++)
                {
                    var c = _state.Combatants[i];
                    if (c == null) continue;
                    if (string.Equals(c.Id, subjectId, StringComparison.Ordinal)
                        || string.Equals(c.SurvivorId, subjectId, StringComparison.Ordinal))
                        return c;
                }
            }

            for (int i = 0; i < _state.Combatants.Count; i++)
            {
                var c = _state.Combatants[i];
                if (c != null && c.IsPlayer && !c.IsDowned && !c.HasFled)
                    return c;
            }

            return null;
        }

        private void ApplyPlayerAim(CombatInputFrame input)
        {
            var subject = ResolveRealtimeSubject(input.SubjectId);
            if (subject == null || subject.IsDowned || subject.HasFled) return;

            if (!string.IsNullOrEmpty(input.AimTargetId))
            {
                var target = FindCombatant(input.AimTargetId);
                if (target != null && target.PoseSeeded)
                {
                    float dx = target.PosX - subject.PosX;
                    float dy = target.PosY - subject.PosY;
                    if (Math.Abs(dx) > 0.001f || Math.Abs(dy) > 0.001f)
                        subject.AimRad = (float)Math.Atan2(dy, dx);
                    return;
                }
            }

            // Host may stream an absolute aim angle every tick while aiming/firing/bracing.
            if (input.Brace || input.FireHeld || input.FirePressed)
                subject.AimRad = input.AimRad;
        }

        private void TickFireCooldowns(float dt)
        {
            for (int i = 0; i < _state.Combatants.Count; i++)
            {
                var c = _state.Combatants[i];
                if (c == null) continue;
                if (c.FireCooldown > 0f)
                    c.FireCooldown = Math.Max(0f, c.FireCooldown - dt);
            }
        }

        private void TryRealtimeReload(CombatInputFrame input)
        {
            var subject = ResolveRealtimeSubject(input.SubjectId);
            if (subject == null) return;
            PlayerReload(subject.Id);
        }

        private void TryRealtimeFire(CombatInputFrame input, ISeededRng rng)
        {
            var subject = ResolveRealtimeSubject(input.SubjectId);
            if (subject == null || subject.IsDowned || subject.HasFled || subject.IsPinned)
                return;
            if (subject.MotionMode == (int)CombatMotionMode.Climb)
                return;
            if (subject.FireCooldown > 0f)
                return;

            var weapon = WeaponOf(subject);
            if (weapon == null || weapon.IsJammed) return;
            var def = CombatCatalog.GetWeapon(weapon.WeaponId);
            if (def == null) return;

            bool automatic = Math.Max(1, def.burst) > 1 || def.isSuppressionCapable;
            bool wantFire = automatic ? input.FireHeld : (input.FirePressed || input.FireHeld);
            if (!wantFire) return;

            string targetId = ResolveRealtimeFireTarget(subject, input);
            if (string.IsNullOrEmpty(targetId)) return;

            float scale = ComputeMotionAccuracyScale(subject, input, def);
            PlayerFire(targetId, rng, subject.Id, scale);
            // Always gate the trigger after a pull so empty/jammed weapons do not spam.
            subject.FireCooldown = FireCooldownSeconds(def);
        }

        private string ResolveRealtimeFireTarget(CombatantState subject, CombatInputFrame input)
        {
            if (!string.IsNullOrEmpty(input.AimTargetId))
            {
                var named = FindCombatant(input.AimTargetId);
                if (named != null && !named.IsPlayer && !named.IsDowned && !named.HasFled)
                    return named.Id;
            }

            // Soft-lock: nearest visible living hostile by pose distance.
            CombatantState? best = null;
            float bestDistSq = float.MaxValue;
            var enemies = LivingEnemies();
            for (int i = 0; i < enemies.Count; i++)
            {
                var e = enemies[i];
                if (e == null || !e.PoseSeeded || IsBurrowHidden(e)) continue;
                float dx = e.PosX - subject.PosX;
                float dy = e.PosY - subject.PosY;
                float d2 = dx * dx + dy * dy;
                if (d2 < bestDistSq)
                {
                    bestDistSq = d2;
                    best = e;
                }
            }

            return best != null ? best.Id : string.Empty;
        }

        private static float ComputeMotionAccuracyScale(
            CombatantState subject,
            CombatInputFrame input,
            CombatWeaponDefinition def)
        {
            float scale = 1f;
            int mode = subject.MotionMode;
            if (mode == (int)CombatMotionMode.Run)
                scale = 0.55f;
            else if (mode == (int)CombatMotionMode.Walk)
                scale = 0.85f;
            else if (mode == (int)CombatMotionMode.Flee)
                scale = 0.50f; // hip-fire only while extracting

            if (mode != (int)CombatMotionMode.Flee
                && input.Brace
                && (mode == (int)CombatMotionMode.Idle || mode == (int)CombatMotionMode.Walk || mode == (int)CombatMotionMode.Cover))
            {
                float brace = def.aimBraceBonus > 0f ? def.aimBraceBonus : 0.20f;
                scale += brace;
            }

            if (scale < 0.2f) scale = 0.2f;
            if (scale > 1.4f) scale = 1.4f;
            return scale;
        }
    }
}
