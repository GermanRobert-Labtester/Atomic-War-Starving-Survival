// SPDX-License-Identifier: MIT
// DEC-358 / PFGL-RT-W4: live flee extract under fire (shot while running away).
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Combat
{
    public partial class TacticalCombatSystem
    {
        public const float ExtractHoldSeconds = 1.25f;

        /// <summary>
        /// Begin a live extract: living players sprint toward the arena extract
        /// volume while enemies keep acting. Replaces the instant PlayerRetreat
        /// roll while <see cref="CombatState.RealtimeActive"/> is true.
        /// </summary>
        public CombatActionResult RequestFlee()
        {
            var res = new CombatActionResult();
            if (_state == null || _state.Resolved)
            {
                res.Message = "Encounter is over.";
                return res;
            }

            var mods = GetStanceMods(CurrentStance());
            if (!mods.CanFlee)
            {
                res.Message = "You cannot flee from a last stand.";
                return res;
            }

            if (!_state.RealtimeActive || _state.Phase != (int)CombatPhase.ActiveRealtime)
            {
                res.Message = "Realtime clock is not active — use PlayerRetreat.";
                return res;
            }

            var players = LivingPlayers();
            int armed = 0;
            int already = 0;
            for (int i = 0; i < players.Count; i++)
            {
                var c = players[i];
                if (c == null || c.IsDowned) continue;
                if (c.MotionMode == (int)CombatMotionMode.Flee)
                {
                    already++;
                    continue;
                }
                c.MotionMode = (int)CombatMotionMode.Flee;
                c.ExtractProgress01 = 0f;
                if (c.IsPinned) c.IsPinned = false;
                var perks = PerksFor(c.SurvivorId, _state.Seed);
                perks?.RecordFlee(c.SurvivorId);
                armed++;
            }

            if (armed == 0 && already == 0)
            {
                res.Message = "No standing survivors to extract.";
                return res;
            }

            if (armed > 0)
                AddEvent("flee_start", _state.EncounterId, "The squad breaks for the extract corridor under fire.");
            res.Success = true;
            res.Message = already > 0 && armed == 0
                ? "Already fleeing — hold the extract volume."
                : "Fleeing — hold the extract volume to break contact.";
            Notify();
            return res;
        }

        private void TickRealtimeFlee(CombatArenaDefinition arena, CombatInputFrame input, float dt)
        {
            if (input.Flee)
                RequestFlee();

            bool anyFleeing = false;
            var players = LivingPlayers();
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i] != null && players[i].MotionMode == (int)CombatMotionMode.Flee && !players[i].IsDowned)
                {
                    anyFleeing = true;
                    break;
                }
            }
            if (!anyFleeing) return;

            SteerFleeingPlayers(arena, players, dt);
            AdvanceExtractHold(arena, players, dt);
        }

        private void SteerFleeingPlayers(CombatArenaDefinition arena, List<CombatantState> players, float dt)
        {
            CombatArenaCatalog.VolumeCenter(arena.extract_volume, out float cx, out float cy);
            float speed = arena.flee_speed > 0f ? arena.flee_speed : arena.run_speed;

            for (int i = 0; i < players.Count; i++)
            {
                var c = players[i];
                if (c == null || c.IsDowned || c.HasFled) continue;
                if (c.MotionMode != (int)CombatMotionMode.Flee) continue;

                if (CombatArenaCatalog.Contains(arena.extract_volume, c.PosX, c.PosY))
                {
                    c.VelX = 0f;
                    c.VelY = 0f;
                    continue;
                }

                float dx = cx - c.PosX;
                float dy = cy - c.PosY;
                float len = (float)Math.Sqrt(dx * dx + dy * dy);
                if (len < 0.001f) len = 0.001f;
                c.VelX = (dx / len) * speed;
                c.VelY = (dy / len) * speed;
                c.FacingRad = (float)Math.Atan2(dy, dx);
                IntegratePose(c, arena, dt);
                c.Lane = CombatArenaCatalog.NearestLane(arena, c.PosX);
            }
        }

        private void AdvanceExtractHold(CombatArenaDefinition arena, List<CombatantState> players, float dt)
        {
            int living = 0;
            int inside = 0;
            for (int i = 0; i < players.Count; i++)
            {
                var c = players[i];
                if (c == null || c.HasFled) continue;
                if (c.IsDowned) continue; // downed do not block extract
                living++;
                if (c.MotionMode == (int)CombatMotionMode.Flee
                    && CombatArenaCatalog.Contains(arena.extract_volume, c.PosX, c.PosY))
                    inside++;
            }

            if (living == 0)
            {
                // Everyone downed/dead while fleeing — let CheckResolution handle Lost.
                return;
            }

            bool squadInside = inside >= living;
            if (!squadInside) return;

            float step = dt / Math.Max(0.05f, ExtractHoldSeconds);
            bool ready = true;
            for (int i = 0; i < players.Count; i++)
            {
                var c = players[i];
                if (c == null || c.IsDowned || c.HasFled) continue;
                if (c.MotionMode != (int)CombatMotionMode.Flee) continue;
                c.ExtractProgress01 = MathfCompat.Clamp01(c.ExtractProgress01 + step);
                if (c.ExtractProgress01 < 0.999f) ready = false;
            }

            if (ready)
                CompleteRealtimeExtract();
        }

        private void CompleteRealtimeExtract()
        {
            if (_state.Resolved) return;
            var players = LivingPlayers();
            for (int i = 0; i < players.Count; i++)
            {
                var c = players[i];
                if (c == null) continue;
                if (!c.IsDowned)
                {
                    c.HasFled = true;
                    c.IsPinned = false;
                    c.MotionMode = (int)CombatMotionMode.Flee;
                    c.ExtractProgress01 = 1f;
                }
            }

            _state.Phase = (int)CombatPhase.Retreated;
            _state.Resolved = true;
            _state.OutcomeText = "Your people fall back and break contact under fire.";
            AddEvent("retreat", _state.EncounterId, "The squad holds extract and breaks contact.");
            BuildAndApplyAftermath("Retreated", -2f);
            OnEncounterEnded?.Invoke(_state);
            Notify();
        }
    }
}
