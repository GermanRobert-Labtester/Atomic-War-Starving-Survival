using System;
using System.Collections.Generic;

namespace Ashfall.Core.Combat
{
    public partial class TacticalCombatSystem
    {
        // ══ Damage, Status, Turn Management & Resolution ═════════════════

        private void ApplyDamage(CombatantState victim, float amount, CombatantState attacker, bool critical, ISeededRng rng)
        {
            if (victim == null || amount <= 0f) return;

            victim.Health = MathfCompat.Max(0f, victim.Health - amount);

            if (victim.Health <= 0f)
            {
                if (victim.IsPlayer && victim.IsLastStand)
                {
                    // Last stand: instant death + mutual kill.
                    Kill(victim);
                    var enemies = LivingEnemies();
                    if (enemies.Count > 0)
                    {
                        var taken = enemies[0];
                        Kill(taken);
                        AddEvent("mutual_kill", victim.Id, victim.Name + " falls; a hostile is dragged down too.");
                    }
                    if (_ports.RaiseTrauma != null && !string.IsNullOrEmpty(victim.SurvivorId))
                        _ports.RaiseTrauma(victim.SurvivorId, "combat_trauma", 1f);
                    return;
                }

                if (!victim.IsDowned)
                {
                    // Downed + bleed-out.
                    victim.IsDowned = true;
                    victim.BleedTurnsRemaining = DefaultBleedTurns;
                    AddEvent("downed", victim.Id, victim.Name + " is downed and bleeding out.");
                    if (_ports.RaiseTrauma != null && !string.IsNullOrEmpty(victim.SurvivorId))
                        _ports.RaiseTrauma(victim.SurvivorId, "combat_injury", 1f);
                }
                else
                {
                    // Already downed and hit again → dead.
                    Kill(victim);
                }
            }
        }

        private void Kill(CombatantState c)
        {
            c.Health = 0f;
            c.HasFled = true; // removed from living pools (deterministic)
            c.IsDowned = false;
            c.BleedTurnsRemaining = 0;
            AddEvent("death", c.Id, c.Name + " is dead.");
            if (c.IsPlayer && !string.IsNullOrEmpty(c.SurvivorId) && _ports.DamageSurvivor != null)
                _ports.DamageSurvivor(c.SurvivorId, 99999f);
            if (!c.IsPlayer)
            {
                // A human kill has consequences.
                var players = LivingPlayers();
                for (int i = 0; i < players.Count; i++)
                {
                    var p = PerksFor(players[i].SurvivorId, _state.Seed);
                    p?.RecordHumanKill(players[i].SurvivorId);
                }
                if (_ports.ApplyMoraleDelta != null)
                    _ports.ApplyMoraleDelta(_state.EncounterId, -CombatPerks.HumanKillMoralePenalty * 0.25f);
            }
        }

        /// <summary>End the player's turn: enemies act, bleed-out ticks, pins decay.</summary>
        public CombatActionResult EndTurn(ISeededRng rng)
        {
            var res = new CombatActionResult();
            if (_state.Resolved) { res.Message = "Encounter is over."; return res; }

            _state.Phase = (int)CombatPhase.EnemyTurn;

            // Bleed-out ticks first (only units downed in EARLIER turns burn turns,
            // so a fresh wound gets a full bleed-out window).
            TickBleedOut();

            // Enemies fire at player targets.
            var enemies = LivingEnemies();
            var players = LivingPlayers();
            var stance = CurrentStance();
            var mods = GetStanceMods(stance);

            for (int i = 0; i < enemies.Count; i++)
            {
                var enemy = enemies[i];
                if (enemy.IsPinned || enemy.IsDowned) continue; // suppressed: accuracy → 0, no fire
                if (players.Count == 0) break;

                var target = players[rng.Next(0, players.Count)];
                if (target.IsDowned) continue;

                // Enemy accuracy: base 0.5, defence reduces it. The catalog-
                // derived AiAccuracyMod multiplies the result so a Burrower
                // Mite (0.95) is fractionally less accurate than a Conscript
                // Levy (0.85) or a Warlord Veteran (1.05).
                float baseAcc = 0.50f;
                float accMod = enemy.AiAccuracyMod > 0f ? enemy.AiAccuracyMod : 1f;
                float acc = baseAcc * accMod * (1f - mods.Defense);
                if (rng.NextDouble() < acc)
                {
                    // Damage scales by lane match (existing rule) and the
                    // catalog-derived AiDamageMod for consistent per-archetype
                    // behaviour across save/load. Mutants get a small extra
                    // kicker when their archetype is registered.
                    float laneDmg = 6f + (float)(enemy.Lane == target.Lane ? 4f : 0f);
                    float dmgMod = enemy.AiDamageMod > 0f ? enemy.AiDamageMod : 1f;
                    float dmg = laneDmg * dmgMod;
                    ApplyDamage(target, dmg, enemy, false, rng);
                    AddEvent("enemy_fire", target.Id, enemy.Name + " hits " + target.Name + ".");
                }
                else
                {
                    AddEvent("enemy_fire", target.Id, enemy.Name + " misses " + target.Name + ".");
                }
            }

            // Pins decay.
            for (int i = 0; i < _state.Combatants.Count; i++)
            {
                var c = _state.Combatants[i];
                if (c.IsPinned && !c.IsPlayer)
                {
                    c.PinnedTurnsRemaining--;
                    if (c.PinnedTurnsRemaining <= 0) c.IsPinned = false;
                }
            }

            _state.Turn++;
            _state.Phase = (int)CombatPhase.PlayerTurn;
            res.Success = true;
            res.Message = "Enemy turn complete.";
            Notify();
            CheckResolution();
            return res;
        }

        private void TickBleedOut()
        {
            var players = LivingPlayers();
            for (int i = 0; i < players.Count; i++)
            {
                var c = players[i];
                if (!c.IsDowned) continue;
                c.BleedTurnsRemaining--;
                AddEvent("bleed", c.Id, c.Name + " bleeding out (" + Math.Max(0, c.BleedTurnsRemaining) + " turns).");
                if (c.BleedTurnsRemaining <= 0)
                    Kill(c);
            }
        }

        /// <summary>Ash Dunes / environmental exposure jams every equipped player firearm.</summary>
        public CombatActionResult TickEnvironmental(float severity, ISeededRng rng)
        {
            var res = new CombatActionResult();
            int affected = 0;
            for (int i = 0; i < _state.Weapons.Count; i++)
            {
                var w = _state.Weapons[i];
                if (w.OwnerSurvivorId == null) continue;
                WeaponConditionSystem.ExposeToAsh(w, severity);
                affected++;
            }
            AddEvent("ash_dunes", _state.EncounterId, "Ash drifts clog " + affected + " firearm action(s).");
            res.Success = affected > 0;
            res.Message = affected + " weapon(s) fouled by ash.";
            Notify();
            return res;
        }

        /// <summary>Record a combat survived for every still-standing player (trauma hook).</summary>
        public void RecordSurvivorsSurvived()
        {
            var players = LivingPlayers();
            for (int i = 0; i < players.Count; i++)
            {
                var c = players[i];
                if (!c.IsDowned && !string.IsNullOrEmpty(c.SurvivorId))
                {
                    var p = PerksFor(c.SurvivorId, _state.Seed);
                    p?.RecordConfinedEncounterSurvived(c.SurvivorId);
                    if (_ports.MarkCombatSurvived != null) _ports.MarkCombatSurvived(c.SurvivorId);
                }
            }
        }

        private void CheckResolution()
        {
            if (_state.Resolved) return;

            var enemies = LivingEnemies();
            var players = LivingPlayers();

            if (enemies.Count == 0)
            {
                _state.Phase = (int)CombatPhase.Won;
                _state.Resolved = true;
                _state.OutcomeText = "Hostiles neutralized.";
                if (_ports.ApplyMoraleDelta != null)
                    _ports.ApplyMoraleDelta(_state.EncounterId, +5f);
                GrantVictoryLoot();
                RecordSurvivorsSurvived();
                AddEvent("victory", _state.EncounterId, "Victory — " + _state.Loot.Count + " loot captured.");
                OnEncounterEnded?.Invoke(_state);
                Notify();
                return;
            }

            bool anyStanding = false;
            for (int i = 0; i < players.Count; i++)
            {
                if (!players[i].IsDowned) { anyStanding = true; break; }
            }
            if (players.Count == 0 || !anyStanding)
            {
                _state.Phase = (int)CombatPhase.Lost;
                _state.Resolved = true;
                _state.OutcomeText = "The squad is wiped out.";
                if (_ports.ApplyMoraleDelta != null)
                    _ports.ApplyMoraleDelta(_state.EncounterId, -12f);
                AddEvent("defeat", _state.EncounterId, "Defeat — no survivors standing.");
                OnEncounterEnded?.Invoke(_state);
                Notify();
            }
        }

        private void GrantVictoryLoot()
        {
            var loot = new CombatLootEntry { itemId = "scrap_metal", quantity = 3, weightKg = 1.4f };
            _state.Loot.Add(loot);
            if (_ports.GrantLoot != null) _ports.GrantLoot(loot);
            var loot2 = new CombatLootEntry { itemId = "ammo_556", quantity = 6, weightKg = 0.6f };
            _state.Loot.Add(loot2);
            if (_ports.GrantLoot != null) _ports.GrantLoot(loot2);
        }

        /// <summary>Run deterministic turns until the encounter resolves (headless/demo).</summary>
        public List<CombatEvent> ResolveToEnd(ISeededRng rng, int maxTurns = 40)
        {
            var done = new List<CombatEvent>();
            int guard = 0;
            while (!_state.Resolved && guard++ < maxTurns)
            {
                var shooter = PickActiveShooter();
                var enemies = LivingEnemies();
                if (shooter != null && enemies.Count > 0)
                {
                    var target = enemies[rng.Next(0, enemies.Count)];
                    PlayerFire(target.Id, rng);
                }
                if (!_state.Resolved)
                    EndTurn(rng);
            }
            done.AddRange(_state.Events);
            return done;
        }
    }
}
