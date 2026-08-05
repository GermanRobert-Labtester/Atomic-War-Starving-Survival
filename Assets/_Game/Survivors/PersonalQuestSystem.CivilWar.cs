using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Prompts #257–#266 — Civil War + interpersonal archetypes.
    /// Quest recorders, base-trait quirks, and latent-trait host APIs.
    /// </summary>
    public partial class PersonalQuestSystem
    {
        // ── #257 Disgraced General — Court Martial / Art of War ──────────

        /// <summary>Hated: all military factions start at -100 trust (shot on sight).</summary>
        public float GetMilitaryFactionTrustOffset(Survivor sv) =>
            HasHated(sv) ? HatedMilitaryTrust : 0f;

        public bool IsShotOnSightByMilitary(Survivor sv) => HasHated(sv);

        /// <summary>AI quirk: only sleeps in a BedModule; floor sleep is refused.</summary>
        public bool RequiresBedModuleToSleep(Survivor sv) =>
            HasTactician(sv) || string.Equals(sv?.ArchetypeId, GeneralId, StringComparison.Ordinal);

        public bool WillRefuseFloorSleep(Survivor sv) => RequiresBedModuleToSleep(sv);

        /// <summary>Massive fatigue penalty when forced to sleep without a bed.</summary>
        public float GetFloorSleepFatiguePenaltyPerHour(Survivor sv, bool hasBedModule) =>
            RequiresBedModuleToSleep(sv) && !hasBedModule ? FloorSleepFatiguePenaltyPerHour : 0f;

        /// <summary>Art of War: +25% ShelterSecurity while this survivor is in the bunker.</summary>
        public float GetArtOfWarShelterSecurityMultiplier(IReadOnlyList<Survivor> survivors)
        {
            if (survivors == null) return 1f;
            for (int i = 0; i < survivors.Count; i++)
            {
                var s = survivors[i];
                if (s == null || !s.IsAlive) continue;
                if (!HasArtOfWar(s)) continue;
                // In bunker if not on expedition / outside.
                if (s.IsOnExpedition) continue;
                return ArtOfWarShelterSecurityMult;
            }
            return 1f;
        }

        public float ApplyArtOfWarShelterSecurity(float baseSecurity, IReadOnlyList<Survivor> survivors) =>
            baseSecurity * GetArtOfWarShelterSecurityMultiplier(survivors);

        /// <summary>Wipe a hit-squad sent specifically for the General.</summary>
        public void RecordHitSquadWiped(
            Survivor general,
            bool targetedAtGeneral,
            bool squadWiped,
            int currentDay = 0)
        {
            if (general == null || !general.IsAlive) return;
            if (!targetedAtGeneral || !squadWiped) return;
            var state = GetOrCreate(general.Id);
            SyncFromSurvivor(general, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.CourtMartial, StringComparison.Ordinal))
                return;

            state.Progress = 1f;
            general.QuestProgress = 1f;
            OnQuestProgress?.Invoke(general, "hit_squad_wiped", 1);
            CompleteQuestline(general, currentDay);
        }

        // ── #258 Rebel Saboteur — Final Payload / Demolitions Expert ─────

        public bool LosesMoraleFromAuthorityOrder(Survivor saboteur, Survivor orderGiver)
        {
            if (!HasAntiAuthority(saboteur) || orderGiver == null) return false;
            return string.Equals(orderGiver.ArchetypeId, QuartermasterId, StringComparison.Ordinal)
                   || string.Equals(orderGiver.ArchetypeId, GeneralId, StringComparison.Ordinal)
                   || HasStrict(orderGiver)
                   || HasTactician(orderGiver);
        }

        public float ApplyAntiAuthorityOrderMorale(Survivor saboteur, Survivor orderGiver)
        {
            if (!LosesMoraleFromAuthorityOrder(saboteur, orderGiver)) return 0f;
            saboteur.Needs.Morale = Mathf.Max(0f, saboteur.Needs.Morale - AntiAuthorityOrderMoraleHit);
            return AntiAuthorityOrderMoraleHit;
        }

        /// <summary>AI quirk: auto-disarm traps on expedition nodes (no prompt).</summary>
        public bool AutoDisarmsTraps(Survivor sv) =>
            HasAntiAuthority(sv)
            || string.Equals(sv?.ArchetypeId, SaboteurId, StringComparison.Ordinal)
            || HasDemolitionsExpert(sv);

        public bool CanBreachVaultsInstantly(Survivor sv) => HasDemolitionsExpert(sv);

        public bool CanBreachBlockedHatchesInstantly(Survivor sv) => HasDemolitionsExpert(sv);

        public float GetExplosiveDamageMultiplier(Survivor sv) =>
            HasDemolitionsExpert(sv) ? DemolitionsExplosiveDamageMult : 1f;

        /// <summary>Craft an IED and destroy the Military Checkpoint node.</summary>
        public void RecordMilitaryCheckpointDestroyed(
            Survivor saboteur,
            string nodeId,
            bool usedIed,
            int currentDay = 0)
        {
            if (saboteur == null || !saboteur.IsAlive) return;
            if (!usedIed) return;
            if (!string.Equals(nodeId, MilitaryCheckpointNodeId, StringComparison.Ordinal))
                return;
            var state = GetOrCreate(saboteur.Id);
            SyncFromSurvivor(saboteur, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheFinalPayload, StringComparison.Ordinal))
                return;

            state.Progress = 1f;
            saboteur.QuestProgress = 1f;
            OnQuestProgress?.Invoke(saboteur, "checkpoint_destroyed", 1);
            CompleteQuestline(saboteur, currentDay);
        }

        // ── #259 Deserter Sniper — Holding the Line / Ghost Shooter ──────

        public bool ShouldAutoFleeCombat(Survivor sv) =>
            HasCoward(sv) && sv != null && sv.IsAlive
            && sv.Needs.Health < (sv.MaxHealthCap > 0f ? sv.MaxHealthCap : 100f) * CowardFleeHealthFrac;

        /// <summary>AI quirk: refuses loud labor (building, generator maintenance).</summary>
        public bool RefusesLoudLabor(Survivor sv) =>
            HasCoward(sv) || string.Equals(sv?.ArchetypeId, DeserterId, StringComparison.Ordinal);

        public bool IsLoudLaborAction(string actionId)
        {
            if (string.IsNullOrEmpty(actionId)) return false;
            return actionId.IndexOf("build", StringComparison.OrdinalIgnoreCase) >= 0
                   || actionId.IndexOf("generator", StringComparison.OrdinalIgnoreCase) >= 0
                   || actionId.IndexOf("maintain", StringComparison.OrdinalIgnoreCase) >= 0
                   || actionId.IndexOf("repair_plant", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        /// <summary>Ghost Shooter: ranged map-layer kills without Hostile Encounter UI.</summary>
        public bool SuppressesHostileEncounterUi(Survivor sv) => HasGhostShooter(sv);

        public bool CanMapLayerRangedEngage(Survivor sv) => HasGhostShooter(sv);

        public float GetGhostShooterCombatMultiplier(Survivor sv) =>
            HasGhostShooter(sv) ? GhostShooterCombatBonus : 1f;

        /// <summary>Defend hatch from a Raid without fleeing.</summary>
        public void RecordRaidDefenseWithoutFleeing(
            Survivor deserter,
            bool raidSurvived,
            bool fled,
            bool defendedHatch,
            int currentDay = 0)
        {
            if (deserter == null || !deserter.IsAlive) return;
            if (!raidSurvived || fled || !defendedHatch) return;
            var state = GetOrCreate(deserter.Id);
            SyncFromSurvivor(deserter, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.HoldingTheLine, StringComparison.Ordinal))
                return;

            state.Progress = 1f;
            deserter.QuestProgress = 1f;
            OnQuestProgress?.Invoke(deserter, "raid_held", 1);
            CompleteQuestline(deserter, currentDay);
        }

        // ── #260 Quartermaster — Inventory Audit / Supply Chain Master ───

        /// <summary>
        /// Strict: passive morale when inventory is neatly sorted/full;
        /// morale loss when any tracked resource dips below 20%.
        /// </summary>
        public float GetStrictInventoryMoraleDelta(
            Survivor qm,
            bool inventoryNeatlySorted,
            bool inventoryFull,
            float lowestResourceFrac)
        {
            if (!HasStrict(qm)) return 0f;
            if (lowestResourceFrac < StrictResourceLowFrac)
                return -StrictInventoryLowMoraleHit;
            if (inventoryNeatlySorted && inventoryFull)
                return StrictInventoryFullMorale;
            return 0f;
        }

        public void ApplyStrictInventoryMorale(
            Survivor qm,
            bool inventoryNeatlySorted,
            bool inventoryFull,
            float lowestResourceFrac)
        {
            float d = GetStrictInventoryMoraleDelta(qm, inventoryNeatlySorted, inventoryFull, lowestResourceFrac);
            if (Mathf.Abs(d) < 0.001f) return;
            qm.Needs.Morale = Mathf.Clamp(qm.Needs.Morale + d, 0f, 100f);
        }

        /// <summary>AI quirk: actively re-sort inventory, overriding player placements.</summary>
        public bool ShouldAutoResortInventory(Survivor qm) =>
            HasStrict(qm) || string.Equals(qm?.ArchetypeId, QuartermasterId, StringComparison.Ordinal);

        public float GetCraftMaterialCostMultiplier(Survivor sv) =>
            HasSupplyChainMaster(sv) ? SupplyChainCraftCostMult : 1f;

        /// <summary>Bunker-wide fuel burn mult when any living Supply Chain Master is present.</summary>
        public float GetBunkerFuelBurnMultiplier(IReadOnlyList<Survivor> survivors)
        {
            if (survivors == null) return 1f;
            for (int i = 0; i < survivors.Count; i++)
            {
                if (survivors[i] != null && survivors[i].IsAlive && HasSupplyChainMaster(survivors[i]))
                    return SupplyChainFuelBurnMult;
            }
            return 1f;
        }

        /// <summary>
        /// Report current stock of each base scrap. Completes when all three
        /// reach InventoryAuditScrapEach (100).
        /// </summary>
        public void RecordScrapStockpile(
            Survivor qm,
            int mechanicalParts,
            int electronicScrap,
            int chemicals,
            int currentDay = 0)
        {
            if (qm == null || !qm.IsAlive) return;
            var state = GetOrCreate(qm.Id);
            SyncFromSurvivor(qm, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.InventoryAudit, StringComparison.Ordinal))
                return;

            state.ScrapMechanicalParts = Mathf.Max(state.ScrapMechanicalParts, mechanicalParts);
            state.ScrapElectronicScrap = Mathf.Max(state.ScrapElectronicScrap, electronicScrap);
            state.ScrapChemicals = Mathf.Max(state.ScrapChemicals, chemicals);
            int min = Mathf.Min(
                state.ScrapMechanicalParts,
                Mathf.Min(state.ScrapElectronicScrap, state.ScrapChemicals));
            state.Progress = min;
            qm.QuestProgress = min;
            OnQuestProgress?.Invoke(qm, "scrap_min", min);

            if (state.ScrapMechanicalParts >= InventoryAuditScrapEach
                && state.ScrapElectronicScrap >= InventoryAuditScrapEach
                && state.ScrapChemicals >= InventoryAuditScrapEach)
            {
                CompleteQuestline(qm, currentDay);
            }
        }

        // ── #261 Child Soldier — Dropping the Rifle / Reclaimed Youth ────

        public bool CanLearnScienceSkill(Survivor sv) => !HasStunted(sv);

        public bool CanLearnMedicalSkill(Survivor sv) => !HasStunted(sv);

        /// <summary>Base weapon damage equals a fully trained adult (no child penalty).</summary>
        public float GetChildSoldierWeaponDamageMultiplier(Survivor sv)
        {
            if (sv == null) return 1f;
            if (string.Equals(sv.ArchetypeId, ChildSoldierId, StringComparison.Ordinal)
                || HasBaseTrait(sv, StuntedId)
                || HasReclaimedYouth(sv))
                return 1f; // adult parity
            return 1f;
        }

        /// <summary>AI quirk: Night Terrors disrupt sleep of anyone in the same room.</summary>
        public bool CausesNightTerrors(Survivor sv) =>
            !HasReclaimedYouth(sv)
            && (HasBaseTrait(sv, StuntedId)
                || string.Equals(sv?.ArchetypeId, ChildSoldierId, StringComparison.Ordinal));

        public bool DisruptsRoomSleep(Survivor source, Survivor sleeper)
        {
            if (!CausesNightTerrors(source) || sleeper == null || !sleeper.IsAlive) return false;
            if (string.IsNullOrEmpty(source.CurrentRoomId)) return false;
            return string.Equals(source.CurrentRoomId, sleeper.CurrentRoomId, StringComparison.Ordinal);
        }

        public float GetNightTerrorSleepDisruption(Survivor source, Survivor sleeper) =>
            DisruptsRoomSleep(source, sleeper) ? ChildSoldierAnxietyDebuff : 0f;

        public float GetUnequippedWeaponAnxietyDebuff(Survivor child) =>
            CausesNightTerrors(child) || (HasBaseTrait(child, StuntedId) && !HasReclaimedYouth(child))
                ? ChildSoldierAnxietyDebuff
                : 0f;

        public bool HasHopeAura(Survivor sv) => HasReclaimedYouth(sv);

        public float GetReclaimedYouthHopeAura(Survivor sv) =>
            HasReclaimedYouth(sv) ? ReclaimedYouthHopeAura : 0f;

        /// <summary>
        /// Force unequipped weapon for a consecutive day. Resets if they re-equip.
        /// Completes after DroppingRifleDaysRequired (30) consecutive days.
        /// </summary>
        public void RecordUnequippedWeaponDay(
            Survivor child,
            bool weaponUnequipped,
            int currentDay = 0)
        {
            if (child == null || !child.IsAlive) return;
            var state = GetOrCreate(child.Id);
            SyncFromSurvivor(child, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.DroppingTheRifle, StringComparison.Ordinal))
                return;

            if (!weaponUnequipped)
            {
                state.UnequippedWeaponDays = 0;
                state.Progress = 0f;
                child.QuestProgress = 0f;
                OnQuestProgress?.Invoke(child, "unequipped_days", 0);
                return;
            }

            state.UnequippedWeaponDays++;
            state.Progress = state.UnequippedWeaponDays;
            child.QuestProgress = state.UnequippedWeaponDays;
            OnQuestProgress?.Invoke(child, "unequipped_days", state.UnequippedWeaponDays);

            // Anxiety while unequipped.
            child.Needs.Morale = Mathf.Max(0f, child.Needs.Morale - ChildSoldierAnxietyDebuff);

            if (state.UnequippedWeaponDays >= DroppingRifleDaysRequired)
                CompleteQuestline(child, currentDay);
        }

        // ── #262 Pure Empath — The Sponge / Soul Weaver ──────────────────

        /// <summary>Hyper-Empathetic: morale drifts toward bunker average.</summary>
        public void ApplyHyperEmpatheticMorale(
            Survivor empath,
            float bunkerAverageMorale,
            float gameHours = 1f)
        {
            if (!HasHyperEmpathetic(empath) || empath?.Needs == null) return;
            float diff = bunkerAverageMorale - empath.Needs.Morale;
            float delta = Mathf.Clamp(diff * 0.3f * gameHours, -1.5f * gameHours, 1.5f * gameHours);
            empath.Needs.Morale = Mathf.Clamp(empath.Needs.Morale + delta, 0f, 100f);
        }

        /// <summary>AI quirk: prioritizes Comfort/Talk over own survival needs.</summary>
        public float GetComfortTalkUtilityBias(Survivor sv) =>
            HasHyperEmpathetic(sv)
            || string.Equals(sv?.ArchetypeId, EmpathId, StringComparison.Ordinal)
                ? ComfortTalkUtilityBias
                : 1f;

        public bool PrioritizesComfortOverSurvival(Survivor sv) => GetComfortTalkUtilityBias(sv) > 1f;

        /// <summary>
        /// Cure a MentalBreak, absorbing trauma (Empath HP → 1). Completes after 3.
        /// </summary>
        public void RecordMentalBreakCured(
            Survivor empath,
            Survivor patient,
            bool curedSuccessfully,
            int currentDay = 0)
        {
            if (empath == null || !empath.IsAlive || !curedSuccessfully) return;
            var state = GetOrCreate(empath.Id);
            SyncFromSurvivor(empath, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheSponge, StringComparison.Ordinal))
                return;

            state.MentalBreaksCured++;
            // Absorb trauma — drop to 1 HP.
            empath.Needs.Health = SpongeAbsorbHealthFloor;
            state.Progress = state.MentalBreaksCured;
            empath.QuestProgress = state.MentalBreaksCured;
            OnQuestProgress?.Invoke(empath, "mental_breaks_cured", state.MentalBreaksCured);

            if (state.MentalBreaksCured >= SpongeCuresRequired)
                CompleteQuestline(empath, currentDay);
        }

        /// <summary>Soul Weaver: transfer own Health and Morale to a dying survivor.</summary>
        public bool TrySoulWeaverTransfer(
            Survivor empath,
            Survivor target,
            float healthAmount,
            float moraleAmount)
        {
            if (!HasSoulWeaver(empath) || target == null || !target.IsAlive) return false;
            if (empath == null || !empath.IsAlive) return false;
            if (healthAmount <= 0f && moraleAmount <= 0f) return false;

            float giveHp = Mathf.Min(healthAmount, Mathf.Max(0f, empath.Needs.Health - 1f));
            float giveMorale = Mathf.Min(moraleAmount, empath.Needs.Morale);

            empath.Needs.Health = Mathf.Max(1f, empath.Needs.Health - giveHp);
            empath.Needs.Morale = Mathf.Max(0f, empath.Needs.Morale - giveMorale);
            target.Needs.Health = Mathf.Min(
                target.MaxHealthCap > 0f ? target.MaxHealthCap : 100f,
                target.Needs.Health + giveHp);
            target.Needs.Morale = Mathf.Min(100f, target.Needs.Morale + giveMorale);
            return giveHp > 0f || giveMorale > 0f;
        }

        // ── #263 Bitter Misanthrope — Hell is Other People / Lone Wolf ───

        public float GetRudeAffinityDrainPerHour(Survivor sv) =>
            HasRude(sv) ? RudeAffinityDrainPerHour : 0f;

        /// <summary>AI quirk: +25% action speed when alone in a room.</summary>
        public float GetSoloRoomActionSpeedMultiplier(Survivor sv, int othersInRoom)
        {
            if (sv == null) return 1f;
            bool aloneQuirk = HasRude(sv)
                              || string.Equals(sv.ArchetypeId, MisanthropeId, StringComparison.Ordinal)
                              || HasLoneWolf(sv);
            if (!aloneQuirk) return 1f;
            return othersInRoom <= 0 ? SoloRoomActionSpeedMult : 1f;
        }

        public float GetLoneWolfNeedsDecayMultiplier(Survivor sv, bool outsideBunker) =>
            HasLoneWolf(sv) && outsideBunker ? LoneWolfNeedsDecayMult : 1f;

        public float GetLoneWolfCombatMultiplier(Survivor sv, bool outsideBunker) =>
            HasLoneWolf(sv) && outsideBunker ? LoneWolfCombatMult : 1f;

        /// <summary>
        /// Solo expedition day (no other party members, no bunker return that day).
        /// Completes after HellIsOtherPeopleDaysRequired (15).
        /// </summary>
        public void RecordSoloExpeditionDay(
            Survivor misanthrope,
            bool entirelyAlone,
            bool returnedToBunker,
            int currentDay = 0)
        {
            if (misanthrope == null || !misanthrope.IsAlive) return;
            var state = GetOrCreate(misanthrope.Id);
            SyncFromSurvivor(misanthrope, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.HellIsOtherPeople, StringComparison.Ordinal))
                return;

            if (!entirelyAlone || returnedToBunker)
            {
                // Quest requires continuous solo expedition — reset on breach.
                state.SoloExpeditionDays = 0;
                state.Progress = 0f;
                misanthrope.QuestProgress = 0f;
                OnQuestProgress?.Invoke(misanthrope, "solo_days", 0);
                return;
            }

            state.SoloExpeditionDays++;
            state.Progress = state.SoloExpeditionDays;
            misanthrope.QuestProgress = state.SoloExpeditionDays;
            OnQuestProgress?.Invoke(misanthrope, "solo_days", state.SoloExpeditionDays);

            if (state.SoloExpeditionDays >= HellIsOtherPeopleDaysRequired)
                CompleteQuestline(misanthrope, currentDay);
        }

        // ── #264 Pollyanna Denialist — Shattered Glass / Grounded Optimist ─

        /// <summary>Denialist: UI always shows RadiationAnxiety as 0.</summary>
        public float GetDisplayedRadiationAnxiety(Survivor sv, float realAnxiety) =>
            HasDenialist(sv) ? 0f : realAnxiety;

        /// <summary>AI quirk: walk outside during FalloutStorms to "enjoy the rain."</summary>
        public bool WantsToWalkOutsideInFalloutStorm(Survivor sv) =>
            HasDenialist(sv)
            || string.Equals(sv?.ArchetypeId, ThePollyannaId, StringComparison.Ordinal);

        /// <summary>
        /// Contract ARS and survive — permanently breaks denial, unlocks Grounded Optimist.
        /// </summary>
        public void RecordSurvivedAcuteRadiationSyndrome(
            Survivor polly,
            bool contractedArs,
            bool survived,
            int currentDay = 0)
        {
            if (polly == null || !polly.IsAlive) return;
            if (!contractedArs || !survived) return;
            var state = GetOrCreate(polly.Id);
            SyncFromSurvivor(polly, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.ShatteredGlass, StringComparison.Ordinal))
                return;

            state.Progress = 1f;
            polly.QuestProgress = 1f;
            OnQuestProgress?.Invoke(polly, "ars_survived", 1);
            CompleteQuestline(polly, currentDay);
        }

        /// <summary>
        /// Grounded Optimist: bunker-wide morale buff that scales UP as conditions worsen.
        /// hardship01: 0 = fine, 1 = catastrophic (weather/food).
        /// </summary>
        public float GetGroundedOptimistMoraleBuff(Survivor polly, float hardship01)
        {
            if (!HasGroundedOptimist(polly)) return 0f;
            float h = Mathf.Clamp01(hardship01);
            return GroundedOptimistBaseMorale + h * 100f * GroundedOptimistHardshipScale;
        }

        public float GetBunkerGroundedOptimistMoraleBuff(
            IReadOnlyList<Survivor> survivors,
            float hardship01)
        {
            if (survivors == null) return 0f;
            float best = 0f;
            for (int i = 0; i < survivors.Count; i++)
            {
                float b = GetGroundedOptimistMoraleBuff(survivors[i], hardship01);
                if (b > best) best = b;
            }
            return best;
        }

        // ── #265 Selfless Martyr — Ultimate Price / Living Saint ─────────

        /// <summary>Sacrificial: intercept damage meant for others during hatch breaches.</summary>
        public bool ShouldInterceptHatchBreachDamage(Survivor martyr, Survivor intendedTarget)
        {
            if (!HasSacrificial(martyr) || martyr == null || !martyr.IsAlive) return false;
            if (intendedTarget == null || ReferenceEquals(martyr, intendedTarget)) return false;
            return true;
        }

        public float InterceptHatchBreachDamage(Survivor martyr, Survivor intendedTarget, float damage)
        {
            if (!ShouldInterceptHatchBreachDamage(martyr, intendedTarget) || damage <= 0f) return damage;
            martyr.Needs.Health = Mathf.Max(0f, martyr.Needs.Health - damage);
            return 0f; // target takes none
        }

        /// <summary>AI quirk: secretly give food to starving survivors (own Hunger spikes).</summary>
        public bool TrySecretlyGiveFoodRation(Survivor martyr, Survivor starving)
        {
            if (martyr == null || !martyr.IsAlive || starving == null || !starving.IsAlive) return false;
            if (!HasSacrificial(martyr)
                && !string.Equals(martyr.ArchetypeId, MartyrId, StringComparison.Ordinal))
                return false;
            if (starving.Needs.Hunger > 30f) return false; // only when truly starving

            martyr.Needs.Hunger = Mathf.Min(100f, martyr.Needs.Hunger + MartyrSecretFoodHungerSpike);
            starving.Needs.Hunger = Mathf.Max(0f, starving.Needs.Hunger - 20f);
            return true;
        }

        /// <summary>
        /// Contract a lethal Phase-2 illness in place of another (event choice).
        /// Unlocks Living Saint.
        /// </summary>
        public void RecordTookLethalPhase2ForOther(
            Survivor martyr,
            bool viaEventChoice,
            bool isPhase2Lethal,
            int currentDay = 0)
        {
            if (martyr == null || !martyr.IsAlive) return;
            if (!viaEventChoice || !isPhase2Lethal) return;
            var state = GetOrCreate(martyr.Id);
            SyncFromSurvivor(martyr, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheUltimatePrice, StringComparison.Ordinal))
                return;

            state.Progress = 1f;
            martyr.QuestProgress = 1f;
            OnQuestProgress?.Invoke(martyr, "took_phase2", 1);
            CompleteQuestline(martyr, currentDay);
        }

        /// <summary>Living Saint death: permanent Inspired floor (Morale min 50).</summary>
        public float GetLivingSaintMoraleFloor() =>
            LivingSaintInspiredActive ? LivingSaintMoraleFloor : 0f;

        public void ApplyLivingSaintMoraleFloor(Survivor sv)
        {
            if (sv == null || !sv.IsAlive || !LivingSaintInspiredActive) return;
            if (sv.Needs.Morale < LivingSaintMoraleFloor)
                sv.Needs.Morale = LivingSaintMoraleFloor;
        }

        // ── #266 Arrogant Surgeon — Botched Job / Humbled Healer ─────────

        /// <summary>God Complex: refuses menial labor (cleaning, digging).</summary>
        public bool RefusesMenialLabor(Survivor sv) => HasGodComplex(sv);

        public bool IsMenialLaborAction(string actionId)
        {
            if (string.IsNullOrEmpty(actionId)) return false;
            return actionId.IndexOf("clean", StringComparison.OrdinalIgnoreCase) >= 0
                   || actionId.IndexOf("dig", StringComparison.OrdinalIgnoreCase) >= 0
                   || actionId.IndexOf("excavat", StringComparison.OrdinalIgnoreCase) >= 0
                   || actionId.IndexOf("sweep", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        /// <summary>Medical skill starts maxed for the Arrogant Surgeon archetype.</summary>
        public float GetStartingMedicalSkill(Survivor sv)
        {
            if (sv != null && string.Equals(sv.ArchetypeId, ArrogantSurgeonId, StringComparison.Ordinal))
                return ArrogantSurgeonMedicalSkill;
            return 0f;
        }

        /// <summary>AI quirk: verbally abuse patients after heal (unless Humbled).</summary>
        public float GetPatientMoraleAfterHealDelta(Survivor medic) =>
            HasGodComplex(medic) ? -GodComplexPatientMoraleHit : 0f;

        public void ApplyPatientMoraleAfterHeal(Survivor medic, Survivor patient)
        {
            float d = GetPatientMoraleAfterHealDelta(medic);
            if (Mathf.Abs(d) < 0.001f || patient == null || !patient.IsAlive) return;
            patient.Needs.Morale = Mathf.Clamp(patient.Needs.Morale + d, 0f, 100f);
        }

        public bool CanCureChronicDisabilities(Survivor medic) => HasHumbledHealer(medic);

        /// <summary>Fail a critical surgery — starts Depression clock for Botched Job.</summary>
        public void RecordCriticalSurgeryFailed(Survivor surgeon, bool wasCritical, int currentDay = 0)
        {
            if (surgeon == null || !surgeon.IsAlive || !wasCritical) return;
            var state = GetOrCreate(surgeon.Id);
            SyncFromSurvivor(surgeon, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheBotchedJob, StringComparison.Ordinal))
                return;

            state.CriticalSurgeryFailed = true;
            state.DepressionDays = 0;
            surgeon.currentMentalBreakId = DepressionBreakId;
            OnQuestProgress?.Invoke(surgeon, "surgery_failed", 1);
        }

        /// <summary>
        /// Tick one day of Depression after a botched critical surgery.
        /// Completes after BotchedJobDepressionDays (10).
        /// </summary>
        public void RecordDepressionDay(Survivor surgeon, int currentDay = 0)
        {
            if (surgeon == null || !surgeon.IsAlive) return;
            var state = GetOrCreate(surgeon.Id);
            SyncFromSurvivor(surgeon, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheBotchedJob, StringComparison.Ordinal))
                return;
            if (!state.CriticalSurgeryFailed) return;

            state.DepressionDays++;
            state.Progress = state.DepressionDays;
            surgeon.QuestProgress = state.DepressionDays;
            surgeon.currentMentalBreakId = DepressionBreakId;
            OnQuestProgress?.Invoke(surgeon, "depression_days", state.DepressionDays);

            if (state.DepressionDays >= BotchedJobDepressionDays)
            {
                if (string.Equals(surgeon.currentMentalBreakId, DepressionBreakId, StringComparison.OrdinalIgnoreCase))
                    surgeon.currentMentalBreakId = null;
                CompleteQuestline(surgeon, currentDay);
            }
        }
    }
}
