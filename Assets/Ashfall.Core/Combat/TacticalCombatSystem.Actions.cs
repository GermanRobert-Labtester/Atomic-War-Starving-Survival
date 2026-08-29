using System;
using System.Collections.Generic;
using Ashfall.Core.PlayerCommand;

namespace Ashfall.Core.Combat
{
    public partial class TacticalCombatSystem
    {
        // ══ Player actions & Hit Resolution ══════════════════════════════

        public CombatActionResult SetStance(TacticalStance stance, string? subjectSurvivorId = null)
        {
            var res = new CombatActionResult();
            if (_state.Resolved)
            {
                res.Message = "Encounter is over.";
                return res;
            }
            if (!GetStanceMods(stance).CanFlee && stance == TacticalStance.Retreat)
            {
                // handled below; retreat is its own action, keep stance allowed
            }

            _state.PlayerStance = StanceId(stance);
            var mods = GetStanceMods(stance);
            if (subjectSurvivorId != null && mods.MoraleDelta != 0f && _ports.ApplyMoraleDelta != null)
                _ports.ApplyMoraleDelta(subjectSurvivorId, mods.MoraleDelta);

            AddEvent("stance", subjectSurvivorId ?? _state.EncounterId, "Stance set to " + stance);
            res.Success = true;
            res.Message = "Stance: " + stance;
            Notify();
            return res;
        }

        public CombatActionResult PlayerFire(string targetId, ISeededRng rng)
        {
            var res = new CombatActionResult();
            if (_state.Resolved) { res.Message = "Encounter is over."; return res; }

            var shooter = PickActiveShooter();
            if (shooter == null) { res.Message = "No armed standing survivor to fire."; return res; }

            var weapon = WeaponOf(shooter);
            if (weapon == null) { res.Message = shooter.Name + " has no weapon."; return res; }

            var target = FindCombatant(targetId);
            if (target == null || target.IsPlayer || target.HasFled)
            {
                res.Message = "Invalid target: " + targetId;
                return res;
            }

            var def = CombatCatalog.GetWeapon(weapon.WeaponId);
            if (def == null) { res.Message = "Unknown weapon: " + weapon.WeaponId; return res; }

            var stance = CurrentStance();
            var mods = GetStanceMods(stance);

            // ── Burst / jam / burst-failure ──
            if (weapon.IsJammed)
            {
                AddEvent("jam_persist", shooter.Id, weapon.WeaponId + " is jammed; clear it first.");
                res.Message = weapon.WeaponId + " is jammed — clear the jam first.";
                Notify();
                return res;
            }

            // Ammo: consume burst rounds through host or the weapon token.
            int burst = Math.Max(1, def.burst);
            int rounds = (int)Math.Max(1, Math.Round(burst * mods.AmmoUse, MidpointRounding.AwayFromZero));
            int remaining;
            if (_ports.ConsumeAmmo != null)
            {
                remaining = _ports.ConsumeAmmo(weapon.AmmoId, rounds);
                if (remaining < 0)
                {
                    res.Message = "No " + (CombatCatalog.GetAmmo(weapon.AmmoId)?.displayName ?? weapon.AmmoId) + " ammunition.";
                    Notify();
                    return res;
                }
            }
            else
            {
                if (weapon.AmmoRemaining < rounds)
                {
                    res.Message = "Weapon out of ammunition.";
                    Notify();
                    return res;
                }
                weapon.AmmoRemaining -= rounds;
            }

            weapon.ShotsFired += burst;
            var perShooter = PerksFor(shooter.SurvivorId, _state.Seed);
            perShooter?.RecordAmmoExpended(shooter.SurvivorId, rounds);

            // ── Degradation ──
            float degrade = WeaponConditionSystem.ComputeDegradePerBurst(weapon) * mods.Degrade;
            WeaponConditionSystem.Degrade(weapon, degrade);

            // ── Jam / burst failure ──
            bool jammed = WeaponConditionSystem.TryJammed(weapon, rng);
            if (jammed)
            {
                AddEvent("weapon_jam", shooter.Id, weapon.WeaponId + " jammed mid-action.");
                res.Success = true;
                res.Message = weapon.WeaponId + " jammed — clear it.";
                Notify();
                return res;
            }

            bool burstFailure = WeaponConditionSystem.TryWeaponBurst(weapon, rng);
            if (burstFailure)
            {
                AddEvent("weapon_burst", shooter.Id, weapon.WeaponId + " burst in the hand.");
                if (_ports.RaiseTrauma != null) _ports.RaiseTrauma(shooter.SurvivorId, "combat_injury", 1f);
                res.Success = true;
                res.Message = weapon.WeaponId + " burst — the action is wrecked.";
                Notify();
                return res;
            }

            // ── Ballistics ──
            var ammo = CombatCatalog.GetAmmo(weapon.AmmoId);
            var coverMaterial = CombatCatalog.GetMaterial("material_concrete"); // default rubble cover
            var armorMaterial = CombatCatalog.GetMaterial(GetArmorMaterialId(shooter)!);
            var barrier = FindPlayerLaneBarrier(target.Lane); // enemy behind a player barrier? use enemy barrier

            var ctx = new BallisticContext
            {
                ShooterId = shooter.Id,
                ShooterName = shooter.Name,
                IsPlayerShooter = true,
                WeaponId = weapon.WeaponId,
                WeaponName = def.displayName,
                WeaponAccuracy = def.accuracy,
                WeaponDamage = def.damage,
                WeaponRangeMod = def.range,
                AmmoId = weapon.AmmoId,
                AmmoName = ammo?.displayName ?? weapon.AmmoId,
                AmmoDamageMod = ammo?.damageMod ?? 1f,
                AmmoRangeMod = ammo?.rangeMod ?? 1f,
                StanceAccuracyMod = mods.Accuracy,
                StanceDamageMod = mods.Damage,
                ExternalAccuracyMod = 1f - (weapon.IsJammed ? 1f : 0f),
                ExternalDamageMod = FlankMultiplier(shooter) * GetCloseQuartersBonus(shooter),
                IsFirstShotCritBonus = false,
                ExtraCritChance = 0f,
                IntendedTarget = target,
                CoverMaterial = coverMaterial!,
                ArmorMaterial = armorMaterial!,
                BarrierMaterial = null!,
                RicochetTargets = LivingEnemies()
            };

            var outcome = BallisticsSystem.Resolve(ctx, rng);

            var ev = new CombatEvent
            {
                Kind = "fire",
                Day = _state.Day,
                Turn = _state.Turn,
                SubjectId = shooter.Id,
                TargetId = target.Id,
                Detail = shooter.Name + " fires " + weapon.WeaponId + " → " + Describe(outcome),
                Value = outcome.DamageDealt
            };
            _state.Events.Add(ev);
            OnCombatEvent?.Invoke(_state, ev);

            // Apply damage to the resolved target.
            if (outcome.DamageDealt > 0f && !string.IsNullOrEmpty(outcome.ResolvedTargetId))
            {
                var victim = FindCombatant(outcome.ResolvedTargetId);
                if (victim != null)
                    ApplyDamage(victim, outcome.DamageDealt, shooter, outcome.IsCritical, rng);
            }

            res.Success = true;
            res.Message = Describe(outcome);
            Notify();
            CheckResolution();
            return res;
        }

        public CombatActionResult PlayerSuppress(ISeededRng rng)
        {
            var res = new CombatActionResult();
            if (_state.Resolved) { res.Message = "Encounter is over."; return res; }

            var shooter = PickActiveShooter();
            if (shooter == null) { res.Message = "No standing armed survivor."; return res; }
            var weapon = WeaponOf(shooter);
            if (weapon == null) { res.Message = shooter.Name + " has no weapon."; return res; }
            var def = CombatCatalog.GetWeapon(weapon.WeaponId);
            if (def == null || !def.isSuppressionCapable)
            {
                res.Message = "Weapon cannot lay suppressive fire (needs a rifle or LMG).";
                return res;
            }

            var stance = CurrentStance();
            var mods = GetStanceMods(stance);
            int rounds = (int)Math.Max(2, Math.Round(CombatPerks.SuppressingFireAmmoCost * mods.AmmoUse, MidpointRounding.AwayFromZero));

            int rem = _ports.ConsumeAmmo != null ? _ports.ConsumeAmmo(weapon.AmmoId, rounds) : (weapon.AmmoRemaining -= rounds);
            if (_ports.ConsumeAmmo != null && rem < 0)
            {
                res.Message = "Not enough ammo for suppressive fire.";
                Notify();
                return res;
            }
            if (_ports.ConsumeAmmo == null && weapon.AmmoRemaining < 0)
            {
                weapon.AmmoRemaining += rounds;
                res.Message = "Not enough ammo for suppressive fire.";
                Notify();
                return res;
            }

            weapon.ShotsFired += rounds;
            var perks = PerksFor(shooter.SurvivorId, _state.Seed);
            perks?.RecordAmmoExpended(shooter.SurvivorId, rounds);

            // Pinned accuracy drops to 0 for the duration.
            var enemies = LivingEnemies();
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].IsPinned = true;
                enemies[i].PinnedTurnsRemaining = DefaultSuppressDuration;
            }

            AddEvent("suppress", _state.EncounterId, shooter.Name + " lays suppressive fire, pinning " + enemies.Count + " hostiles.");
            res.Success = true;
            res.Message = "Suppressive fire — " + enemies.Count + " enemies pinned.";
            Notify();
            return res;
        }

        public CombatActionResult PlayerClearJam(string survivorIdOrCombatantId, ISeededRng rng)
        {
            var res = new CombatActionResult();
            var c = FindPlayerCombatant(survivorIdOrCombatantId);
            if (c == null) { res.Message = "Unknown survivor."; return res; }
            var weapon = WeaponOf(c);
            if (weapon == null) { res.Message = "No weapon to clear."; return res; }
            if (!weapon.IsJammed) { res.Message = weapon.WeaponId + " is not jammed."; return res; }

            var perks = PerksFor(c.SurvivorId, _state.Seed);
            int ticks = perks != null ? perks.GetJamClearTicks(c.SurvivorId) : WeaponConditionSystem.DefaultJamClearTicks;
            bool cleared = WeaponConditionSystem.TickJamClear(weapon, ticks);
            perks?.RecordWeaponJamSurvived(c.SurvivorId);

            AddEvent("clear_jam", c.Id, weapon.WeaponId + " jam cleared (" + ticks + " ticks).");
            res.Success = true;
            res.Message = cleared ? weapon.WeaponId + " cleared." : weapon.WeaponId + " jam being worked (" + weapon.JamClearTicksRemaining + " ticks left).";
            Notify();
            return res;
        }

        /// <summary>Item 6: explicit Reload action. Refills ammo for the survivor's
        /// weapon if the inventory adapter can supply the magazine.</summary>
        public CombatActionResult PlayerReload(string survivorIdOrCombatantId)
        {
            var res = new CombatActionResult();
            var c = FindPlayerCombatant(survivorIdOrCombatantId);
            if (c == null) { res.Message = "Unknown survivor."; return res; }
            var weapon = WeaponOf(c);
            if (weapon == null) { res.Message = "No weapon to reload."; return res; }
            string ammoId = weapon.AmmoId;
            if (string.IsNullOrEmpty(ammoId))
            {
                res.Message = weapon.WeaponId + " has no ammo type assigned.";
                return res;
            }
            int maxLoad = weapon.MagazineCapacity > 0 ? weapon.MagazineCapacity : 30;
            int needed = Math.Max(0, maxLoad - weapon.AmmoRemaining);
            if (needed == 0)
            {
                res.Message = weapon.WeaponId + " is already fully loaded.";
                return res;
            }
            int granted = _ports?.ConsumeAmmo != null ? _ports.ConsumeAmmo(ammoId, needed) : 0;
            if (granted <= 0)
            {
                res.Message = "No " + ammoId + " available to reload.";
                return res;
            }
            weapon.AmmoRemaining += granted;
            AddEvent("reload", c.Id,
                weapon.WeaponId + " reloaded +" + granted + " " + ammoId + " (" +
                weapon.AmmoRemaining + "/" + maxLoad + ").");
            res.Success = true;
            res.Message = weapon.WeaponId + " +" + granted + " " + ammoId + ".";
            Notify();
            return res;
        }

        /// <summary>
        /// Side-effect-free preview of a field repair command.
        /// Shares the same validation path as <see cref="PlayerFieldRepair"/>.
        /// </summary>
        public CommandPreview PreviewPlayerFieldRepair(string survivorIdOrCombatantId, long stateVersion = 0)
        {
            var c = FindPlayerCombatant(survivorIdOrCombatantId);
            if (c == null)
                return CommandPreview.Unavailable(PlayerCommandCode.RepairWeapon, "unknown_survivor", "combat.unknown_survivor", stateVersion);

            var weapon = WeaponOf(c);
            if (weapon == null)
                return CommandPreview.Unavailable(PlayerCommandCode.RepairWeapon, "no_weapon", "combat.no_weapon", stateVersion);

            int cost = WeaponConditionSystem.GetScrapRepairCost(weapon);
            if (cost <= 0)
                return CommandPreview.Unavailable(PlayerCommandCode.RepairWeapon, "no_scrap_needed", "combat.no_scrap_needed", stateVersion);

            return CommandPreview.Available(
                PlayerCommandCode.RepairWeapon,
                stateVersion,
                new Dictionary<string, double> { { "scrap_cost", cost }, { "condition_restored", 1 } },
                isIrreversible: false,
                messageKey: "combat.preview_field_repair");
        }

        /// <summary>
        /// Execute a field repair using the same validation path as <see cref="PreviewPlayerFieldRepair"/>.
        /// Stale previews are rejected without mutation.
        /// </summary>
        public CommandResult ExecutePlayerFieldRepair(string survivorIdOrCombatantId, long expectedStateVersion = 0, long currentStateVersion = 0)
        {
            var preview = PreviewPlayerFieldRepair(survivorIdOrCombatantId, expectedStateVersion);
            if (!preview.IsAvailable)
                return CommandResult.FromPreview(preview);

            if (preview.StateVersion != currentStateVersion)
                return CommandResult.StalePreview(PlayerCommandCode.RepairWeapon, preview.StateVersion, currentStateVersion);

            var result = PlayerFieldRepair(survivorIdOrCombatantId, new SeededRng(0));
            if (!result.Success)
                return new CommandResult(
                    PlayerCommandCode.RepairWeapon,
                    ActionResult.Failed("execute_failed", result.Message),
                    expectedStateVersion,
                    currentStateVersion);

            return CommandResult.FromSuccess(
                PlayerCommandCode.RepairWeapon,
                ActionResult.Success("combat.field_repaired", result.AddedEvents?.Count > 0
                    ? new Dictionary<string, double> { { "events", result.AddedEvents.Count } }
                    : null),
                expectedStateVersion,
                currentStateVersion + 1);
        }


        public CombatActionResult PlayerFieldRepair(string survivorIdOrCombatantId, ISeededRng rng)
        {
            var res = new CombatActionResult();
            var c = FindPlayerCombatant(survivorIdOrCombatantId);
            if (c == null) { res.Message = "Unknown survivor."; return res; }
            var weapon = WeaponOf(c);
            if (weapon == null) { res.Message = "No weapon to repair."; return res; }

            int cost = WeaponConditionSystem.GetScrapRepairCost(weapon);
            bool ok = _condition.TryFieldRepair(weapon, _ports);
            if (!ok)
            {
                res.Message = "Cannot repair — need " + cost + " " + WeaponConditionSystem.ScrapMaterialId + ".";
                Notify();
                return res;
            }
            AddEvent("repair", c.Id, weapon.WeaponId + " field-repaired (" + cost + " scrap).");
            res.Success = true;
            res.Message = weapon.WeaponId + " repaired to full condition.";
            Notify();
            return res;
        }

        public CombatActionResult PlayerMoveLane(string survivorIdOrCombatantId, CombatLane lane, ISeededRng rng)
        {
            var res = new CombatActionResult();
            var c = FindPlayerCombatant(survivorIdOrCombatantId);
            if (c == null) { res.Message = "Unknown survivor."; return res; }
            if (c.IsDowned) { res.Message = c.Name + " is downed and cannot reposition."; return res; }

            c.Lane = (int)lane;
            AddEvent("lane", c.Id, c.Name + " moved to " + lane + " lane.");
            res.Success = true;
            res.Message = c.Name + " is now " + lane + ".";
            Notify();
            return res;
        }

        public CombatActionResult PlayerDeployTrap(ISeededRng rng)
        {
            var res = new CombatActionResult();
            if (_state.Resolved) { res.Message = "Encounter is over."; return res; }
            var enemies = LivingEnemies();
            if (enemies.Count == 0) { res.Message = "No hostiles to trap."; return res; }

            // A trap wounds the leading enemy in the most contested lane.
            var leading = enemies[0];
            var perkMultiplier = GetMaxTrapDamageMultiplier();
            float dmg = 14f * perkMultiplier;
            ApplyDamage(leading, dmg, null!, false, rng);
            AddEvent("trap", leading.Id, "A jury-rigged trap tears into " + leading.Name + ".");
            res.Success = true;
            res.Message = "Trap wounds " + leading.Name + " (" + dmg.ToString("0") + ").";
            Notify();
            CheckResolution();
            return res;
        }

        /// <summary>Clean ash/contamination from all player weapons (decon flush / maintenance).</summary>
        public CombatActionResult PlayerDecontaminate(ISeededRng rng)
        {
            var res = new CombatActionResult();
            var players = LivingPlayers();
            int cleaned = 0;
            for (int i = 0; i < _state.Weapons.Count; i++)
            {
                var w = _state.Weapons[i];
                if (w.OwnerSurvivorId == null) continue;
                if (w.CachedJamChance > 0.5f || w.IsJammed)
                {
                    w.IsJammed = false;
                    w.JamClearTicksRemaining = 0;
                    w.CachedJamChance = WeaponConditionSystem.ComputeJamChance(w);
                    cleaned++;
                }
            }
            AddEvent("decon", _state.EncounterId, "Decontamination flush cleaned " + cleaned + " weapon action(s).");
            res.Success = true;
            res.Message = cleaned + " weapon(s) flushed clean.";
            Notify();
            return res;
        }

        public CombatActionResult PlayerBandage(string rescuerId, string downedId, ISeededRng rng)
        {
            var res = new CombatActionResult();
            var rescuer = FindPlayerCombatant(rescuerId);
            if (rescuer == null) { res.Message = "Unknown rescuer."; return res; }
            var downed = FindPlayerCombatant(downedId);
            if (downed == null || !downed.IsDowned) { res.Message = "That survivor is not downed."; return res; }

            bool consumed = _ports.ConsumeItem != null && _ports.ConsumeItem("bandage", 1);
            if (_ports.ConsumeItem != null && !consumed)
            {
                res.Message = "No bandage available.";
                Notify();
                return res;
            }

            downed.IsDowned = false;
            downed.BleedTurnsRemaining = 0;
            downed.Health = MathfCompat.Max(downed.Health, 15f);
            if (_ports.HealSurvivor != null)
                downed.Health = _ports.HealSurvivor(downed.SurvivorId, 15f);

            AddEvent("bandage", downed.Id, rescuer.Name + " bandages " + downed.Name + ".");
            res.Success = true;
            res.Message = downed.Name + " bandaged and stabilized.";
            Notify();
            return res;
        }

        public CombatActionResult PlayerRetreat(ISeededRng rng)
        {
            var res = new CombatActionResult();
            if (_state.Resolved) { res.Message = "Encounter is over."; return res; }
            var mods = GetStanceMods(CurrentStance());
            if (!mods.CanFlee)
            {
                res.Message = "You cannot flee from a last stand.";
                return res;
            }

            float success = mods.Mobility;
            foreach (var c in LivingPlayers())
            {
                var p = PerksFor(c.SurvivorId, _state.Seed);
                p?.RecordFlee(c.SurvivorId);
            }

            if (rng.NextDouble() < success)
            {
                // Clean retreat.
                foreach (var c in LivingPlayers()) { c.HasFled = true; if (c.IsPinned) c.IsPinned = false; }
                _state.Phase = (int)CombatPhase.Retreated;
                _state.Resolved = true;
                _state.OutcomeText = "Your people fall back and break contact.";
                AddEvent("retreat", _state.EncounterId, "The squad disengages and retreats.");
                OnEncounterEnded?.Invoke(_state);
                Notify();
                res.Success = true;
                res.Message = "Squad extracts successfully.";
                return res;
            }

            // Failed retreat: take injury on the way out.
            var players = LivingPlayers();
            if (players.Count > 0)
            {
                var victim = players[0];
                float inj = MathfCompat.Max(8f, victim.Health * 0.4f);
                ApplyDamage(victim, inj, null!, false, rng);
                AddEvent("retreat_fail", victim.Id, "The retreat collapses; " + victim.Name + " is hit.");
            }
            res.Success = true;
            res.Message = "Retreat disrupted — someone is hit.";
            Notify();
            CheckResolution();
            return res;
        }

        public CombatActionResult PlayerLastStand(string survivorIdOrCombatantId, ISeededRng rng)
        {
            var res = new CombatActionResult();
            var c = FindPlayerCombatant(survivorIdOrCombatantId);
            if (c == null) { res.Message = "Unknown survivor."; return res; }
            c.IsLastStand = true;
            SetStance(TacticalStance.LastStand, c.SurvivorId);
            AddEvent("last_stand", c.Id, c.Name + " declares a last stand — no retreat, doubled accuracy.");
            res.Success = true;
            res.Message = c.Name + " is fighting to the death.";
            Notify();
            return res;
        }

        /// <summary>Human-readable one-line outcome for logging/labels.</summary>
        public static string Describe(BallisticOutcome o)
        {
            switch (o.Result)
            {
                case BallisticResult.DirectHit:
                    return "direct hit (" + o.DamageDealt.ToString("0") + " dmg)" + (o.IsCritical ? " CRIT" : "");
                case BallisticResult.Missed:
                    return "miss";
                case BallisticResult.Blocked:
                    return "blocked by cover/barrier";
                case BallisticResult.Penetrated:
                    return "penetrated cover, " + o.DamageDealt.ToString("0") + " throughput";
                case BallisticResult.Ricocheted:
                    return "ricochet → " + o.ResolvedTargetId + " (" + o.DamageDealt.ToString("0") + " dmg)";
                case BallisticResult.Stopped:
                    return "stopped by armor";
                default:
                    return o.Result.ToString();
            }
        }
    }
}
