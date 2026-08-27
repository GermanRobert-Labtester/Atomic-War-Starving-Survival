using System;
using System.Collections.Generic;

namespace Ashfall.Core.Combat
{
    public partial class TacticalCombatSystem
    {
        // ══ Targeting / query helpers ═════════════════════════════════════

        private CombatantState? FindPlayerCombatant(string survivorIdOrCombatantId)
        {
            for (int i = 0; i < _state.Combatants.Count; i++)
            {
                var c = _state.Combatants[i];
                if (!c.IsPlayer) continue;
                if (c.Id == survivorIdOrCombatantId || c.SurvivorId == survivorIdOrCombatantId)
                    return c;
            }
            return null;
        }

        private CombatantState? FindCombatant(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            for (int i = 0; i < _state.Combatants.Count; i++)
                if (_state.Combatants[i].Id == id) return _state.Combatants[i];
            return null;
        }

        private List<CombatantState> LivingPlayers()
        {
            var list = new List<CombatantState>();
            for (int i = 0; i < _state.Combatants.Count; i++)
            {
                var c = _state.Combatants[i];
                if (!c.IsPlayer || c.HasFled) continue;
                list.Add(c);
            }
            list.Sort((a, b) => string.CompareOrdinal(a.Id, b.Id));
            return list;
        }

        private List<CombatantState> LivingEnemies()
        {
            var list = new List<CombatantState>();
            for (int i = 0; i < _state.Combatants.Count; i++)
            {
                var c = _state.Combatants[i];
                if (c.IsPlayer || c.HasFled) continue;
                list.Add(c);
            }
            list.Sort((a, b) => string.CompareOrdinal(a.Id, b.Id));
            return list;
        }

        private CombatantState? PickActiveShooter()
        {
            var players = LivingPlayers();
            if (players.Count == 0) return null;
            for (int i = 0; i < players.Count; i++)
            {
                var c = players[i];
                if (!c.IsDowned && !string.IsNullOrEmpty(c.WeaponInstanceId)) return c;
            }
            return null;
        }

        private WeaponInstanceState? WeaponOf(CombatantState c)
        {
            if (c == null || string.IsNullOrEmpty(c.WeaponInstanceId)) return null;
            for (int i = 0; i < _state.Weapons.Count; i++)
                if (_state.Weapons[i].InstanceId == c.WeaponInstanceId) return _state.Weapons[i];
            return null;
        }

        private bool IsLaneContestedForPlayer(CombatantState player)
        {
            var enemies = LivingEnemies();
            for (int i = 0; i < enemies.Count; i++)
                if (enemies[i].Lane == player.Lane && !enemies[i].IsDowned) return true;
            return false;
        }

        private float FlankMultiplier(CombatantState player)
        {
            return IsLaneContestedForPlayer(player) ? 1f : 1.5f;
        }

        private string? GetArmorMaterialId(CombatantState c)
        {
            if (c.ArmorRating >= 0.6f) return "armor_plate";
            if (c.ArmorRating >= 0.4f) return "armor_kevlar";
            if (c.ArmorRating >= 0.2f) return "armor_cloth";
            return null;
        }

        private float GetCloseQuartersBonus(CombatantState shooter)
        {
            var p = PerksFor(shooter.SurvivorId, _state.Seed);
            return p != null ? p.GetCloseQuartersDamageMultiplier(shooter.SurvivorId, false) : 1f;
        }

        private BarrierState? FindPlayerLaneBarrier(int lane)
        {
            for (int i = 0; i < _state.Barriers.Count; i++)
                if (_state.Barriers[i].IsPlayer && _state.Barriers[i].Lane == lane) return _state.Barriers[i];
            return null;
        }

        private float GetMaxTrapDamageMultiplier()
        {
            float m = 1f;
            var players = LivingPlayers();
            for (int i = 0; i < players.Count; i++)
            {
                var p = PerksFor(players[i].SurvivorId, _state.Seed);
                if (p != null) m = Math.Max(m, p.GetTrapDamageMultiplier(players[i].SurvivorId));
            }
            return m;
        }

        /// <summary>Jam chance a given survivor's weapon would show in the UI — same value as resolution.</summary>
        public float UIJamChance(CombatantState c)
        {
            var w = WeaponOf(c);
            return w == null ? 0f : WeaponConditionSystem.ComputeJamChance(w);
        }
    }
}
