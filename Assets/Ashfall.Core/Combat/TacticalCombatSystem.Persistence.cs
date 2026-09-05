using System;
using System.Collections.Generic;

namespace Ashfall.Core.Combat
{
    public partial class TacticalCombatSystem
    {
        // ══ Snapshot for the host / UI ════════════════════════════════════

        public CombatSnapshot BuildSnapshot()
        {
            var snap = new CombatSnapshot
            {
                EncounterId = _state.EncounterId,
                ResolutionId = _state.ResolutionId,
                LocationName = _state.LocationName,
                Day = _state.Day,
                Turn = _state.Turn,
                Phase = ((CombatPhase)_state.Phase).ToString(),
                StanceId = _state.PlayerStance,
                Resolved = _state.Resolved,
                OutcomeText = _state.OutcomeText,
                IsActive = !string.IsNullOrEmpty(_state.EncounterId) && !_state.Resolved,
                Aftermath = CloneAftermath(_state.Aftermath)
            };

            var combatants = new List<CombatantState>(_state.Combatants);
            combatants.Sort((a, b) => string.CompareOrdinal(a.Id, b.Id));
            for (int i = 0; i < combatants.Count; i++)
            {
                var c = combatants[i];
                var w = WeaponOf(c);
                var csnap = new CombatantSnapshot
                {
                    Id = c.Id,
                    Name = c.Name,
                    IsPlayer = c.IsPlayer,
                    FactionId = c.FactionId,
                    Lane = ((CombatLane)MathfCompat.Clamp(c.Lane, 0, 2)).ToString(),
                    Health = (int)Math.Round(c.Health),
                    MaxHealth = (int)Math.Round(c.MaxHealth),
                    ArmorRating = (int)Math.Round(c.ArmorRating * 100f),
                    CoverRating = (int)Math.Round(c.CoverRating * 100f),
                    IsDowned = c.IsDowned,
                    IsPinned = c.IsPinned,
                    IsLastStand = c.IsLastStand,
                    WeaponName = w != null ? (CombatCatalog.GetWeapon(w.WeaponId)?.displayName ?? w.WeaponId) : "—",
                    WeaponConditionPct = w != null ? (int)Math.Round(w.ConditionPct * 100f) : 0,
                    WeaponJammed = w != null && w.IsJammed,
                    WeaponAmmo = w != null ? w.AmmoId : string.Empty
                };
                if (c.IsDowned) csnap.Status = "DOWNED (" + c.BleedTurnsRemaining + ")";
                else if (c.IsPinned) csnap.Status = "PINNED";
                else if (c.IsLastStand) csnap.Status = "LAST STAND";
                else csnap.Status = "OK";
                snap.Combatants.Add(csnap);
            }

            var weapons = new List<WeaponInstanceState>(_state.Weapons);
            weapons.Sort((a, b) => string.CompareOrdinal(a.InstanceId, b.InstanceId));
            for (int i = 0; i < weapons.Count; i++)
            {
                var w = weapons[i];
                var def = CombatCatalog.GetWeapon(w.WeaponId);
                snap.Weapons.Add(new WeaponSnapshot
                {
                    InstanceId = w.InstanceId,
                    WeaponId = w.WeaponId,
                    WeaponName = def?.displayName ?? w.WeaponId,
                    ConditionPct = (int)Math.Round(w.ConditionPct * 100f),
                    JamChancePct = (int)Math.Round(WeaponConditionSystem.ComputeJamChance(w) * 100f),
                    IsJammed = w.IsJammed,
                    ScrapRepairCost = WeaponConditionSystem.GetScrapRepairCost(w),
                    AmmoRemaining = w.AmmoRemaining,
                    OwnerSurvivorId = w.OwnerSurvivorId
                });
            }

            snap.Events.AddRange(_state.Events);
            snap.Loot.AddRange(_state.Loot);
            return snap;
        }

        // ══ Save / Load (deep-copy, deterministic ordering) ═══════════════

        public CombatState CaptureState()
        {
            var copy = new CombatState
            {
                SystemId = _state.SystemId,
                SaveVersion = CombatState.CurrentSaveVersion,
                EncounterId = _state.EncounterId,
                ExpeditionId = _state.ExpeditionId,
                LocationId = _state.LocationId,
                LocationName = _state.LocationName,
                Day = _state.Day,
                Seed = _state.Seed,
                Turn = _state.Turn,
                Phase = _state.Phase,
                PlayerStance = _state.PlayerStance,
                RoundNumber = _state.RoundNumber,
                Resolved = _state.Resolved,
                OutcomeText = _state.OutcomeText,
                ResolutionId = _state.ResolutionId,
                Aftermath = CloneAftermath(_state.Aftermath)
            };
            copy.BoundWeaponConditions = CloneBoundWeaponConditions(_state.BoundWeaponConditions);
            copy.Combatants = CloneCombatants(_state.Combatants);
            copy.Weapons = CloneWeapons(_state.Weapons);
            copy.Barriers = CloneBarriers(_state.Barriers);
            copy.Events = CloneEvents(_state.Events);
            copy.Loot.AddRange(_state.Loot);
            return copy;
        }

        /// <summary>
        /// Restore from a save. Migrates legacy/foreign fields with backward-
        /// compatible defaults (null-safe, clamps phase/lane).
        /// </summary>
        public void RestoreState(CombatState saved)
        {
            if (saved == null) return;
            var migrated = Migrate(saved);
            _state = migrated;
            Notify();
        }

        /// <summary>Migrate an older/foreign CombatState to the current version (null-safe).</summary>
        public static CombatState Migrate(CombatState s)
        {
            var m = new CombatState
            {
                SystemId = string.IsNullOrEmpty(s.SystemId) ? SystemId : s.SystemId,
                SaveVersion = CombatState.CurrentSaveVersion,
                EncounterId = s.EncounterId ?? string.Empty,
                ExpeditionId = s.ExpeditionId ?? string.Empty,
                LocationId = s.LocationId ?? string.Empty,
                LocationName = s.LocationName ?? string.Empty,
                Day = s.Day,
                Seed = s.Seed,
                Turn = Math.Max(1, s.Turn),
                Phase = MathfCompat.Clamp(s.Phase, (int)CombatPhase.Setup, (int)CombatPhase.Retreated),
                PlayerStance = string.IsNullOrEmpty(s.PlayerStance) ? StanceId(TacticalStance.HoldPosition) : s.PlayerStance,
                RoundNumber = s.RoundNumber,
                Resolved = s.Resolved,
                OutcomeText = s.OutcomeText ?? string.Empty,
                ResolutionId = s.ResolutionId ?? string.Empty,
                Aftermath = CloneAftermath(s.Aftermath)
            };
            m.BoundWeaponConditions = CloneBoundWeaponConditions(s.BoundWeaponConditions);
            m.Combatants = CloneCombatants(s.Combatants);
            m.Weapons = CloneWeapons(s.Weapons);
            m.Barriers = CloneBarriers(s.Barriers);
            m.Events = CloneEvents(s.Events);
            if (s.Loot != null) m.Loot.AddRange(s.Loot);
            return m;
        }

        public static CombatAftermath? CloneAftermath(CombatAftermath? src)
        {
            if (src == null) return null;
            var copy = new CombatAftermath
            {
                ResolutionId = src.ResolutionId ?? string.Empty,
                EncounterId = src.EncounterId ?? string.Empty,
                Outcome = src.Outcome ?? string.Empty,
                MoraleConsequences = src.MoraleConsequences,
                IsApplied = src.IsApplied
            };
            if (src.SurvivorInjuries != null) copy.SurvivorInjuries.AddRange(src.SurvivorInjuries);
            if (src.SurvivorDeaths != null) copy.SurvivorDeaths.AddRange(src.SurvivorDeaths);
            if (src.WeaponWear != null)
            {
                foreach (var w in src.WeaponWear)
                {
                    if (w == null) continue;
                    copy.WeaponWear.Add(new CombatWeaponWearRecord
                    {
                        InstanceId = w.InstanceId ?? string.Empty,
                        WeaponId = w.WeaponId ?? string.Empty,
                        OwnerSurvivorId = w.OwnerSurvivorId ?? string.Empty,
                        StartConditionPct = w.StartConditionPct,
                        FinalConditionPct = w.FinalConditionPct,
                        WearDeltaPct = w.WearDeltaPct
                    });
                }
            }
            if (src.AmmoSpent != null)
            {
                foreach (var a in src.AmmoSpent)
                {
                    if (a == null) continue;
                    copy.AmmoSpent.Add(new CombatAmmoSpentRecord
                    {
                        AmmoId = a.AmmoId ?? string.Empty,
                        RoundsSpent = a.RoundsSpent
                    });
                }
            }
            if (src.LootConsequences != null)
            {
                foreach (var l in src.LootConsequences)
                {
                    if (l == null) continue;
                    copy.LootConsequences.Add(new CombatLootEntry
                    {
                        itemId = l.itemId ?? string.Empty,
                        quantity = l.quantity,
                        weightKg = l.weightKg
                    });
                }
            }
            return copy;
        }

        public static List<BoundWeaponConditionEntry> CloneBoundWeaponConditions(List<BoundWeaponConditionEntry>? src)
        {
            var copy = new List<BoundWeaponConditionEntry>();
            if (src != null)
            {
                for (int i = 0; i < src.Count; i++)
                {
                    var b = src[i];
                    if (b == null) continue;
                    copy.Add(new BoundWeaponConditionEntry
                    {
                        instanceId = b.instanceId ?? string.Empty,
                        conditionPct = b.conditionPct
                    });
                }
            }
            return copy;
        }

        private static List<CombatantState> CloneCombatants(List<CombatantState> src)
        {
            var ordered = src ?? new List<CombatantState>();
            var copy = new List<CombatantState>();
            for (int i = 0; i < ordered.Count; i++)
            {
                var c = ordered[i];
                if (c == null) continue;
                copy.Add(CloneCombatant(c));
            }
            return copy;
        }

        private static CombatantState CloneCombatant(CombatantState c)
        {
            return new CombatantState
            {
                Id = c.Id ?? string.Empty,
                Name = c.Name ?? string.Empty,
                SurvivorId = c.SurvivorId ?? string.Empty,
                IsPlayer = c.IsPlayer,
                FactionId = c.FactionId ?? string.Empty,
                Lane = MathfCompat.Clamp(c.Lane, 0, 2),
                Health = c.Health,
                MaxHealth = MathfCompat.Max(1f, c.MaxHealth),
                ArmorRating = MathfCompat.Clamp01(c.ArmorRating),
                CoverRating = MathfCompat.Clamp01(c.CoverRating),
                IsDowned = c.IsDowned,
                BleedTurnsRemaining = c.BleedTurnsRemaining,
                IsPinned = c.IsPinned,
                PinnedTurnsRemaining = c.PinnedTurnsRemaining,
                IsLastStand = c.IsLastStand,
                WeaponInstanceId = c.WeaponInstanceId ?? string.Empty,
                HasFled = c.HasFled,
                AiStancePreference = c.AiStancePreference ?? "HoldPosition",
                AiSpecialMove = c.AiSpecialMove ?? "None",
                AiAccuracyMod = c.AiAccuracyMod <= 0f ? 1f : c.AiAccuracyMod,
                AiDamageMod = c.AiDamageMod <= 0f ? 1f : c.AiDamageMod,
                SurrenderThreshold = c.SurrenderThreshold,
                FleeThreshold = c.FleeThreshold,
                CatalogId = c.CatalogId ?? string.Empty
            };
        }

        private static List<WeaponInstanceState> CloneWeapons(List<WeaponInstanceState> src)
        {
            var copy = new List<WeaponInstanceState>();
            var ordered = src != null ? new List<WeaponInstanceState>(src) : new List<WeaponInstanceState>();
            ordered.Sort((a, b) => string.CompareOrdinal(a.InstanceId, b.InstanceId));
            for (int i = 0; i < ordered.Count; i++)
            {
                var w = ordered[i];
                if (w == null) continue;
                copy.Add(CloneWeapon(w));
            }
            return copy;
        }

        private static WeaponInstanceState CloneWeapon(WeaponInstanceState w)
        {
            return new WeaponInstanceState
            {
                InstanceId = w.InstanceId ?? string.Empty,
                WeaponId = w.WeaponId ?? string.Empty,
                OwnerSurvivorId = w.OwnerSurvivorId ?? string.Empty,
                OwnerCombatantId = w.OwnerCombatantId ?? string.Empty,
                ConditionPct = MathfCompat.Clamp01(w.ConditionPct),
                IsJammed = w.IsJammed,
                JamClearTicksRemaining = w.JamClearTicksRemaining,
                JamsSurvived = w.JamsSurvived,
                ShotsFired = w.ShotsFired,
                BurstCount = w.BurstCount,
                CachedJamChance = MathfCompat.Clamp01(w.CachedJamChance),
                AshFoul = MathfCompat.Clamp01(w.AshFoul),
                AmmoId = w.AmmoId ?? string.Empty,
                AmmoRemaining = w.AmmoRemaining,
                ScrapRepairCost = w.ScrapRepairCost
            };
        }

        private static List<BarrierState> CloneBarriers(List<BarrierState> src)
        {
            var copy = new List<BarrierState>();
            var ordered = src != null ? new List<BarrierState>(src) : new List<BarrierState>();
            ordered.Sort((a, b) => string.CompareOrdinal(a.Id, b.Id));
            for (int i = 0; i < ordered.Count; i++)
            {
                var b = ordered[i];
                if (b == null) continue;
                copy.Add(new BarrierState
                {
                    Id = b.Id ?? string.Empty,
                    Lane = MathfCompat.Clamp(b.Lane, 0, 2),
                    IsPlayer = b.IsPlayer,
                    MaterialId = b.MaterialId ?? string.Empty,
                    IntegrityPct = MathfCompat.Clamp01(b.IntegrityPct * 0.01f) * 100f,
                    ArmorRating = b.ArmorRating
                });
            }
            return copy;
        }

        private static List<CombatEvent> CloneEvents(List<CombatEvent> src)
        {
            var copy = new List<CombatEvent>();
            var ordered = src != null ? new List<CombatEvent>(src) : new List<CombatEvent>();
            for (int i = 0; i < ordered.Count; i++)
            {
                var e = ordered[i];
                if (e == null) continue;
                copy.Add(new CombatEvent
                {
                    Kind = e.Kind ?? string.Empty,
                    Day = e.Day,
                    Turn = e.Turn,
                    SubjectId = e.SubjectId ?? string.Empty,
                    TargetId = e.TargetId ?? string.Empty,
                    Detail = e.Detail ?? string.Empty,
                    Value = e.Value
                });
            }
            return copy;
        }
    }
}
