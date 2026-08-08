using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Prompts #299–#318 — youth of the ashes, improbable oddities, final flawed humans.
    /// Quest recorders, base-trait quirks, and latent-trait host APIs.
    /// </summary>
    public partial class PersonalQuestSystem
    {
        // ── #299 Prom Queen — The Real World / Diplomat ──────────────────

        public bool RefusesJunkAndSpoiledFood(Survivor sv) => HasSpoiled(sv);

        public bool IsJunkOrSpoiledFoodId(string itemId)
        {
            if (string.IsNullOrEmpty(itemId)) return false;
            return itemId.IndexOf("junk", StringComparison.OrdinalIgnoreCase) >= 0
                   || itemId.IndexOf("spoiled", StringComparison.OrdinalIgnoreCase) >= 0
                   || string.Equals(itemId, SpoiledMeatItemId, StringComparison.Ordinal)
                   || string.Equals(itemId, "junk_food", StringComparison.Ordinal);
        }

        public bool ShouldRefuseFoodItem(Survivor sv, string itemId) =>
            RefusesJunkAndSpoiledFood(sv) && IsJunkOrSpoiledFoodId(itemId);

        /// <summary>Spoiled: Charisma is maxed (1.0 utility).</summary>
        public float GetCharisma01(Survivor sv) =>
            HasSpoiled(sv) || HasDiplomat(sv) ? 1f : 0.5f;

        /// <summary>AI quirk: 2h/day makeup/hygiene burns clean water.</summary>
        public float GetMakeupHygieneHoursPerDay(Survivor sv) =>
            string.Equals(sv?.ArchetypeId, PromQueenId, StringComparison.Ordinal)
            || HasSpoiled(sv)
                ? PromQueenMakeupHoursPerDay
                : 0f;

        public float GetMakeupCleanWaterBurn(Survivor sv) =>
            GetMakeupHygieneHoursPerDay(sv) > 0f ? PromQueenMakeupWaterUnits : 0f;

        public bool CanTalkDownRaiderRaid(Survivor sv) => HasDiplomat(sv);

        public bool TryConvertRaidToTrade(Survivor sv, bool raidAtHatch) =>
            HasDiplomat(sv) && raidAtHatch;

        public void RecordRealWorldSoloRaidDefense(
            Survivor queen,
            bool leftAloneToDefend,
            bool killedRaider,
            int currentDay = 0)
        {
            if (queen == null || !queen.IsAlive) return;
            if (!leftAloneToDefend || !killedRaider) return;
            var state = GetOrCreate(queen.Id);
            SyncFromSurvivor(queen, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheRealWorld, StringComparison.Ordinal))
                return;
            state.Progress = 1f;
            queen.QuestProgress = 1f;
            OnQuestProgress?.Invoke(queen, "real_world_kill", 1);
            CompleteQuestline(queen, currentDay);
        }

        // ── #300 Jock — The Gladiator / Wasteland Gladiator ──────────────

        public float GetHighMetabolismRationMultiplier(Survivor sv) =>
            HasHighMetabolism(sv) ? HighMetabolismRationMult : 1f;

        public float GetStrength01(Survivor sv) =>
            HasHighMetabolism(sv) || HasWastelandGladiator(sv) ? 1f : 0.5f;

        /// <summary>AI quirk: punch walls when morale low → structural damage.</summary>
        public bool ShouldPunchWallsWhenMoraleLow(Survivor sv, float morale) =>
            (string.Equals(sv?.ArchetypeId, JockId, StringComparison.Ordinal)
             || HasHighMetabolism(sv))
            && morale < JockPunchMoraleThreshold
            && !HasWastelandGladiator(sv);

        public float GetWallPunchStructuralDamage(Survivor sv) =>
            ShouldPunchWallsWhenMoraleLow(sv, sv?.Needs?.Morale ?? 100f)
                ? JockWallPunchIntegrityDamage
                : 0f;

        public float GetUnarmedDamageMultiplier(Survivor sv) =>
            HasWastelandGladiator(sv) ? WastelandGladiatorUnarmedMult : 1f;

        public bool CanThrowRaidersFromAirlock(Survivor sv) => HasWastelandGladiator(sv);

        public bool UnarmedExceedsFirearms(Survivor sv) => HasWastelandGladiator(sv);

        public void RecordGladiatorBossDefeat(
            Survivor jock,
            bool pureHandToHand,
            bool weaponsEquipped,
            bool bossDefeated,
            int currentDay = 0)
        {
            if (jock == null || !jock.IsAlive) return;
            if (!pureHandToHand || weaponsEquipped || !bossDefeated) return;
            var state = GetOrCreate(jock.Id);
            SyncFromSurvivor(jock, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheGladiator, StringComparison.Ordinal))
                return;
            state.Progress = 1f;
            jock.QuestProgress = 1f;
            OnQuestProgress?.Invoke(jock, "gladiator_boss", 1);
            CompleteQuestline(jock, currentDay);
        }

        // ── #301 Med Student — The First Save / Chief of Medicine ────────

        public float GetTextbookHealingDurationMultiplier(Survivor sv) =>
            HasTextbookKnowledge(sv) ? TextbookHealingDurationMult : 1f;

        public float GetMedicalSupplyCostMultiplier(Survivor sv) =>
            HasChiefOfMedicine(sv) ? ChiefOfMedicineSupplyMult : 1f;

        public bool CanInstantlyDiagnosePhase2(Survivor sv) => HasChiefOfMedicine(sv);

        /// <summary>AI quirk: first trauma treatment → puke (hygiene + fatigue hit).</summary>
        public bool TryMedStudentFirstTraumaPuke(Survivor student, string afflictionId)
        {
            if (student == null || !student.IsAlive) return false;
            if (!HasTextbookKnowledge(student)
                && !string.Equals(student.ArchetypeId, MedStudentId, StringComparison.Ordinal))
                return false;
            if (!IsTraumaAfflictionId(afflictionId)) return false;
            var state = GetOrCreate(student.Id);
            if (state.MedStudentHasPukedOnTrauma) return false;
            state.MedStudentHasPukedOnTrauma = true;
            student.Needs.Hygiene = Mathf.Max(0f, student.Needs.Hygiene - MedStudentPukeHygieneHit);
            student.Needs.Fatigue = Mathf.Min(100f, student.Needs.Fatigue + MedStudentPukeFatigueHit);
            return true;
        }

        public static bool IsTraumaAfflictionId(string afflictionId)
        {
            if (string.IsNullOrEmpty(afflictionId)) return false;
            return afflictionId.IndexOf("gunshot", StringComparison.OrdinalIgnoreCase) >= 0
                   || afflictionId.IndexOf("amputat", StringComparison.OrdinalIgnoreCase) >= 0
                   || afflictionId.IndexOf("lacerat", StringComparison.OrdinalIgnoreCase) >= 0
                   || afflictionId.IndexOf("broken", StringComparison.OrdinalIgnoreCase) >= 0
                   || afflictionId.IndexOf("trauma", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public void RecordFirstSaveComaRevive(
            Survivor student,
            bool usedAdrenaline,
            bool patientRevivedFromComa,
            int currentDay = 0)
        {
            if (student == null || !student.IsAlive) return;
            if (!usedAdrenaline || !patientRevivedFromComa) return;
            var state = GetOrCreate(student.Id);
            SyncFromSurvivor(student, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheFirstSave, StringComparison.Ordinal))
                return;
            state.Progress = 1f;
            student.QuestProgress = 1f;
            OnQuestProgress?.Invoke(student, "first_save", 1);
            CompleteQuestline(student, currentDay);
        }

        // ── #302 Gamer — Player One / Drone Operator ─────────────────────

        public bool SuffersAgoraphobicExpeditionBreak(Survivor sv) => HasAgoraphobic(sv);

        public bool TryTriggerAgoraphobicExpeditionBreak(Survivor sv)
        {
            if (!SuffersAgoraphobicExpeditionBreak(sv) || sv == null) return false;
            if (string.IsNullOrEmpty(sv.currentMentalBreakId))
                sv.currentMentalBreakId = "panic";
            return true;
        }

        /// <summary>AI quirk: works night / sleeps day (Night Owl-style inverted social).</summary>
        public bool WorksNightsSleepsDays(Survivor sv) =>
            string.Equals(sv?.ArchetypeId, GamerId, StringComparison.Ordinal)
            || HasAgoraphobic(sv)
            || HasDroneOperator(sv);

        public float GetGamerCoopTaskSpeedMultiplier(Survivor sv, bool isNight) =>
            WorksNightsSleepsDays(sv)
                ? (isNight ? 1.25f : 0.35f)
                : 1f;

        public bool CanRemoteScavengeWithDrone(Survivor sv) => HasDroneOperator(sv);

        public void RecordPlayerOneDroneMap(
            Survivor gamer,
            string nodeId,
            bool droneRepaired,
            int currentDay = 0)
        {
            if (gamer == null || !gamer.IsAlive || string.IsNullOrEmpty(nodeId)) return;
            if (!droneRepaired) return;
            var state = GetOrCreate(gamer.Id);
            SyncFromSurvivor(gamer, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.PlayerOne, StringComparison.Ordinal))
                return;
            if (state.DroneMappedNodeIds == null)
                state.DroneMappedNodeIds = new List<string>();
            for (int i = 0; i < state.DroneMappedNodeIds.Count; i++)
            {
                if (string.Equals(state.DroneMappedNodeIds[i], nodeId, StringComparison.Ordinal))
                    return;
            }
            state.DroneMappedNodeIds.Add(nodeId);
            state.Progress = state.DroneMappedNodeIds.Count;
            gamer.QuestProgress = state.DroneMappedNodeIds.Count;
            OnQuestProgress?.Invoke(gamer, "drone_map", state.DroneMappedNodeIds.Count);
            if (state.DroneMappedNodeIds.Count >= GamerDroneNodesRequired)
                CompleteQuestline(gamer, currentDay);
        }

        // ── #303 Choir Boy — Loss of Faith / Choir of One ────────────────

        public bool GoesIntoShockOnWitnessedMurder(Survivor sv) => HasInnocent(sv);

        public void ApplyInnocentMurderShock(Survivor sv)
        {
            if (!GoesIntoShockOnWitnessedMurder(sv) || sv == null || !sv.IsAlive) return;
            sv.currentMentalBreakId = "shock";
            sv.Needs.Morale = Mathf.Max(0f, sv.Needs.Morale - InnocentMurderShockMoraleHit);
        }

        /// <summary>AI quirk: pray over dead reduces corpse ambient morale penalty.</summary>
        public float GetCorpseMoralePenaltyMultiplier(IReadOnlyList<Survivor> survivors)
        {
            if (survivors == null) return 1f;
            for (int i = 0; i < survivors.Count; i++)
            {
                var s = survivors[i];
                if (s == null || !s.IsAlive) continue;
                if (string.Equals(s.ArchetypeId, ChoirBoyId, StringComparison.Ordinal)
                    || HasInnocent(s)
                    || HasChoirOfOne(s))
                    return ChoirBoyCorpseMoralePenaltyMult;
            }
            return 1f;
        }

        public bool NegatesBunkerRadiationAnxiety(IReadOnlyList<Survivor> survivors) =>
            AnyLivingWithTrait(survivors, ChoirOfOneId);

        public void RecordLossOfFaithStarvation(
            Survivor choir,
            bool bunkerOutOfFood,
            int consecutiveFoodlessDays,
            int currentDay = 0)
        {
            if (choir == null || !choir.IsAlive) return;
            if (!bunkerOutOfFood) return;
            var state = GetOrCreate(choir.Id);
            SyncFromSurvivor(choir, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.LossOfFaith, StringComparison.Ordinal))
                return;
            state.StarvationFoodlessDays = Mathf.Max(state.StarvationFoodlessDays, consecutiveFoodlessDays);
            state.Progress = state.StarvationFoodlessDays;
            choir.QuestProgress = state.StarvationFoodlessDays;
            OnQuestProgress?.Invoke(choir, "loss_of_faith", state.StarvationFoodlessDays);
            if (state.StarvationFoodlessDays >= ChoirBoyStarvationDaysRequired)
                CompleteQuestline(choir, currentDay);
        }

        // ── #304 Twin Alpha — Separation Anxiety / Hive Tactics ──────────

        public bool HasSymbioticBond(Survivor sv) => HasBaseTrait(sv, SymbioticBondId);

        public void LinkTwins(Survivor alpha, Survivor beta)
        {
            if (alpha == null || beta == null) return;
            GetOrCreate(alpha.Id).TwinPartnerId = beta.Id;
            GetOrCreate(beta.Id).TwinPartnerId = alpha.Id;
        }

        public string GetTwinPartnerId(Survivor sv) =>
            sv == null ? null : GetOrCreate(sv.Id).TwinPartnerId;

        public void NotifyTwinDeath(Survivor deceased, IReadOnlyList<Survivor> survivors)
        {
            if (deceased == null || survivors == null) return;
            string partnerId = GetOrCreate(deceased.Id).TwinPartnerId;
            if (string.IsNullOrEmpty(partnerId))
            {
                // Fall back: archetype pair.
                for (int i = 0; i < survivors.Count; i++)
                {
                    var s = survivors[i];
                    if (s == null || !s.IsAlive || s.Id == deceased.Id) continue;
                    if (IsTwinPair(deceased, s))
                    {
                        ApplySymbioticBondDeath(deceased, s);
                        return;
                    }
                }
                return;
            }
            for (int i = 0; i < survivors.Count; i++)
            {
                var s = survivors[i];
                if (s != null && string.Equals(s.Id, partnerId, StringComparison.Ordinal))
                {
                    ApplySymbioticBondDeath(deceased, s);
                    return;
                }
            }
        }

        private static bool IsTwinPair(Survivor a, Survivor b)
        {
            if (a == null || b == null) return false;
            return (string.Equals(a.ArchetypeId, TwinAlphaId, StringComparison.Ordinal)
                    && string.Equals(b.ArchetypeId, TwinBetaId, StringComparison.Ordinal))
                   || (string.Equals(a.ArchetypeId, TwinBetaId, StringComparison.Ordinal)
                       && string.Equals(b.ArchetypeId, TwinAlphaId, StringComparison.Ordinal));
        }

        private void ApplySymbioticBondDeath(Survivor deceased, Survivor partner)
        {
            if (partner == null || !partner.IsAlive || !HasSymbioticBond(partner)) return;
            if (string.Equals(partner.ArchetypeId, TwinAlphaId, StringComparison.Ordinal)
                || string.Equals(deceased.ArchetypeId, TwinBetaId, StringComparison.Ordinal)
                   && string.Equals(partner.ArchetypeId, TwinAlphaId, StringComparison.Ordinal))
            {
                // Alpha: permanent catatonic break.
                partner.currentMentalBreakId = "catatonic";
                partner.State = SurvivorState.Incapacitated;
                return;
            }
            if (string.Equals(partner.ArchetypeId, TwinBetaId, StringComparison.Ordinal))
            {
                // Beta: suicide event.
                partner.State = SurvivorState.Dead;
            }
        }

        /// <summary>AI quirk: Alpha only acts if Beta is in the same room.</summary>
        public bool TwinAlphaRequiresBetaSameRoom(Survivor alpha, string betaRoomId) =>
            string.Equals(alpha?.ArchetypeId, TwinAlphaId, StringComparison.Ordinal)
            && HasSymbioticBond(alpha)
            && !HasHiveTactics(alpha)
            && (string.IsNullOrEmpty(alpha.CurrentRoomId)
                || !string.Equals(alpha.CurrentRoomId, betaRoomId, StringComparison.Ordinal));

        public bool CanExecuteUtilityAction(Survivor alpha, Survivor betaInRoom)
        {
            if (!string.Equals(alpha?.ArchetypeId, TwinAlphaId, StringComparison.Ordinal))
                return true;
            if (HasHiveTactics(alpha)) return true;
            if (betaInRoom == null || !betaInRoom.IsAlive) return false;
            return string.Equals(alpha.CurrentRoomId, betaInRoom.CurrentRoomId, StringComparison.Ordinal);
        }

        public float GetHiveTacticsCombatMultiplier(Survivor alpha, Survivor beta) =>
            HasHiveTactics(alpha)
            && beta != null
            && beta.IsAlive
            && string.Equals(alpha?.CurrentRoomId, beta.CurrentRoomId, StringComparison.Ordinal)
                ? HiveTacticsCombatMult
                : 1f;

        public void RecordSeparationAnxietyExpedition(
            Survivor alpha,
            bool alphaOnExpedition,
            bool betaStayedInBunker,
            bool expeditionCompleted,
            int currentDay = 0)
        {
            if (alpha == null || !alpha.IsAlive) return;
            if (!alphaOnExpedition || !betaStayedInBunker || !expeditionCompleted) return;
            var state = GetOrCreate(alpha.Id);
            SyncFromSurvivor(alpha, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.SeparationAnxiety, StringComparison.Ordinal))
                return;
            state.Progress = 1f;
            alpha.QuestProgress = 1f;
            OnQuestProgress?.Invoke(alpha, "separation_anxiety", 1);
            CompleteQuestline(alpha, currentDay);
        }

        // ── #305 Twin Beta — The Independent / Hive Healing ──────────────

        /// <summary>AI quirk: mirror Alpha's needs (hunger/thirst/fatigue).</summary>
        public void MirrorTwinAlphaNeeds(Survivor beta, Survivor alpha)
        {
            if (beta == null || alpha == null || !beta.IsAlive || !alpha.IsAlive) return;
            if (!string.Equals(beta.ArchetypeId, TwinBetaId, StringComparison.Ordinal)
                && !HasSymbioticBond(beta))
                return;
            if (HasHiveHealing(beta)) return; // independent after latent
            beta.Needs.Hunger = alpha.Needs.Hunger;
            beta.Needs.Thirst = alpha.Needs.Thirst;
            beta.Needs.Fatigue = alpha.Needs.Fatigue;
        }

        public bool DuplicatesHealingToTwinPartner(Survivor beta) => HasHiveHealing(beta);

        public float ApplyHiveHealingDuplicate(
            Survivor beta,
            Survivor alpha,
            float healthDelta)
        {
            if (!DuplicatesHealingToTwinPartner(beta) || alpha == null || !alpha.IsAlive)
                return 0f;
            if (healthDelta <= 0f) return 0f;
            float before = alpha.Needs.Health;
            SurvivorNeedWrite.SetHealth(alpha, Mathf.Min(alpha.MaxHealthCap, alpha.Needs.Health + healthDelta));
            return alpha.Needs.Health - before;
        }

        public void RecordIndependentHealAlpha(
            Survivor beta,
            Survivor alpha,
            bool alphaHadPhase2Lethal,
            bool alphaHealed,
            int currentDay = 0)
        {
            if (beta == null || !beta.IsAlive) return;
            if (!alphaHadPhase2Lethal || !alphaHealed) return;
            var state = GetOrCreate(beta.Id);
            SyncFromSurvivor(beta, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheIndependent, StringComparison.Ordinal))
                return;
            state.Progress = 1f;
            beta.QuestProgress = 1f;
            OnQuestProgress?.Invoke(beta, "the_independent", 1);
            CompleteQuestline(beta, currentDay);
        }

        // ── #306 Theorist — Vindicated / Truth Seeker ────────────────────

        public bool CannotEquipHelmets(Survivor sv) => HasTinfoilHat(sv);

        public bool RefusesAntibiotics(Survivor sv) => HasTinfoilHat(sv);

        /// <summary>AI quirk: randomly disable radio (host rolls chance).</summary>
        public bool ShouldSabotageRadio(Survivor sv, System.Random rng = null)
        {
            if (sv == null || !sv.IsAlive) return false;
            if (!string.Equals(sv.ArchetypeId, TheoristId, StringComparison.Ordinal)
                && !HasTinfoilHat(sv))
                return false;
            if (HasTruthSeeker(sv)) return false;
            rng ??= AtomicWar._Game.Utilities.SeededRandom.CreateFixed("personalquestsystem_ashes");
            return rng.NextDouble() < TheoristRadioSabotageChance;
        }

        public float GetTruthSeekerIntelMultiplier(Survivor sv) =>
            HasTruthSeeker(sv) ? TruthSeekerIntelMult : 1f;

        public bool IntelAlwaysVerifiedNeverTrap(Survivor sv) => HasTruthSeeker(sv);

        public void RecordVindicatedNumbersStation(
            Survivor theorist,
            bool decodedNumbersStation,
            bool governmentStillActive,
            int currentDay = 0)
        {
            if (theorist == null || !theorist.IsAlive) return;
            if (!decodedNumbersStation || !governmentStillActive) return;
            var state = GetOrCreate(theorist.Id);
            SyncFromSurvivor(theorist, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.Vindicated, StringComparison.Ordinal))
                return;
            state.Progress = 1f;
            theorist.QuestProgress = 1f;
            OnQuestProgress?.Invoke(theorist, "vindicated", 1);
            CompleteQuestline(theorist, currentDay);
        }

        // ── #307 Hermit — The Old Woods / Wildman ────────────────────────

        public bool CannotUseStovePurifierOrMedicalBed(Survivor sv) => HasUncivilized(sv);

        public bool RefusesIndoorSleepWhenCrowded(Survivor sv, int bunkerPopulation) =>
            (string.Equals(sv?.ArchetypeId, HermitId, StringComparison.Ordinal)
             || HasUncivilized(sv))
            && bunkerPopulation > HermitIndoorSleepMaxPop
            && !HasWildman(sv);

        public string GetPreferredSleepRoomWhenCrowded(Survivor sv, int bunkerPopulation) =>
            RefusesIndoorSleepWhenCrowded(sv, bunkerPopulation) ? AirlockRoomId : null;

        public bool ImmuneToForestSwampTrapsAndEncounters(Survivor sv) => HasWildman(sv);

        public bool CanEatRawIrradiatedFloraSafely(Survivor sv) => HasWildman(sv);

        public void RecordOldWoodsCleared(
            Survivor hermit,
            string nodeId,
            bool usedOnlyKnife,
            bool allHostilesCleared,
            int currentDay = 0)
        {
            if (hermit == null || !hermit.IsAlive) return;
            if (!usedOnlyKnife || !allHostilesCleared) return;
            if (!string.IsNullOrEmpty(nodeId)
                && nodeId.IndexOf("mutated_forest", StringComparison.OrdinalIgnoreCase) < 0
                && !string.Equals(nodeId, MutatedForestNodeId, StringComparison.Ordinal))
                return;
            var state = GetOrCreate(hermit.Id);
            SyncFromSurvivor(hermit, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheOldWoods, StringComparison.Ordinal))
                return;
            state.Progress = 1f;
            hermit.QuestProgress = 1f;
            OnQuestProgress?.Invoke(hermit, "old_woods", 1);
            CompleteQuestline(hermit, currentDay);
        }

        // ── #308 Patient — The Awakening / Second Life ───────────────────

        public bool IsComatoseBurden(Survivor sv) =>
            HasComatoseBurden(sv)
            || (string.Equals(sv?.ArchetypeId, PatientId, StringComparison.Ordinal)
                && !HasSecondLife(sv));

        public bool HasZeroAiUntilAwakening(Survivor sv) => IsComatoseBurden(sv);

        public void TickAwakeningDay(
            Survivor patient,
            bool keptAliveAndHydrated,
            int currentDay = 0)
        {
            if (patient == null || !patient.IsAlive) return;
            if (!IsComatoseBurden(patient)) return;
            var state = GetOrCreate(patient.Id);
            SyncFromSurvivor(patient, state);
            // Patient quest is day-0: arm immediately so 15 hydrated days can accumulate.
            if (!state.QuestActive && !state.TraitUnlocked)
                TryStartQuestline(patient, "awakening_start", currentDay);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheAwakening, StringComparison.Ordinal))
                return;

            if (keptAliveAndHydrated)
                state.PatientHydratedDays++;
            else
                state.PatientHydratedDays = 0;

            state.Progress = state.PatientHydratedDays;
            patient.QuestProgress = state.PatientHydratedDays;
            OnQuestProgress?.Invoke(patient, "awakening", state.PatientHydratedDays);

            if (state.PatientHydratedDays >= PatientAwakeningDaysRequired)
            {
                patient.State = SurvivorState.Idle;
                CompleteQuestline(patient, currentDay);
                ApplySecondLifeRoll(patient);
            }
        }

        public void ApplySecondLifeRoll(Survivor patient, System.Random rng = null)
        {
            if (patient == null || !HasSecondLife(patient)) return;
            rng ??= new System.Random(patient.Id?.GetHashCode() ?? 308);
            // Stats rolled at 150% of normal max (1.0 base → up to 1.5 effective).
            patient.MedicalSkill = Mathf.Clamp01(0.5f + (float)rng.NextDouble()) * SecondLifeStatMaxMult / 1.5f;
            patient.CraftingSkill = Mathf.Clamp01(0.5f + (float)rng.NextDouble()) * SecondLifeStatMaxMult / 1.5f;
            patient.ScienceSkill = Mathf.Clamp01(0.5f + (float)rng.NextDouble()) * SecondLifeStatMaxMult / 1.5f;
            // Store excess in progression bonuses so effective > 1.0 is possible.
            patient.ProgressionMedicalBonus = SecondLifeStatMaxMult - 1f;
            patient.ProgressionCraftingBonus = SecondLifeStatMaxMult - 1f;
            patient.ProgressionCombatBonus = SecondLifeStatMaxMult - 1f;
            patient.BaseMaxHealth = 100f * SecondLifeStatMaxMult;
            patient.BaseMaxStamina = 100f * SecondLifeStatMaxMult;
            SurvivorNeedWrite.SetHealth(patient, patient.MaxHealthCap);
        }

        // ── #309 Escapee — Breaking Chains / Iron Will ───────────────────

        public float GetCultOfTheGlowDamageMultiplier(Survivor sv) =>
            HasBranded(sv) ? BrandedCultDamageMult : 1f;

        public bool CannotTradeWithCultOfTheGlow(Survivor sv) => HasBranded(sv);

        /// <summary>AI quirk: destroy religious items brought into bunker.</summary>
        public bool DestroysReligiousItems(Survivor sv) =>
            string.Equals(sv?.ArchetypeId, EscapeeId, StringComparison.Ordinal)
            || HasBranded(sv);

        public bool IsReligiousItemId(string itemId)
        {
            if (string.IsNullOrEmpty(itemId)) return false;
            return itemId.IndexOf("reli", StringComparison.OrdinalIgnoreCase) >= 0
                   || itemId.IndexOf("bible", StringComparison.OrdinalIgnoreCase) >= 0
                   || itemId.IndexOf("prayer", StringComparison.OrdinalIgnoreCase) >= 0
                   || itemId.IndexOf("cult", StringComparison.OrdinalIgnoreCase) >= 0
                   || itemId.IndexOf("scripture", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public bool IsImmuneToAllMentalBreaks(Survivor sv) => HasIronWill(sv);

        public bool IsImmuneToInterpersonalAffinityDrain(Survivor sv) => HasIronWill(sv);

        public void RecordBreakingChainsEmissaryKill(
            Survivor escapee,
            bool assassinatedCultEmissaryAtHatch,
            int currentDay = 0)
        {
            if (escapee == null || !escapee.IsAlive) return;
            if (!assassinatedCultEmissaryAtHatch) return;
            var state = GetOrCreate(escapee.Id);
            SyncFromSurvivor(escapee, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.BreakingChains, StringComparison.Ordinal))
                return;
            state.Progress = 1f;
            escapee.QuestProgress = 1f;
            OnQuestProgress?.Invoke(escapee, "breaking_chains", 1);
            CompleteQuestline(escapee, currentDay);
        }

        // ── #310 Stowaway — Earning Keep / Unseen Listener ───────────────

        public float GetUndocumentedRationMultiplier(Survivor sv) =>
            HasUndocumented(sv) ? UndocumentedRationMult : 1f;

        /// <summary>AI quirk: hide in dark corners; sneak food while others sleep.</summary>
        public bool HidesInDarkCorners(Survivor sv) =>
            string.Equals(sv?.ArchetypeId, StowawayId, StringComparison.Ordinal)
            || HasUndocumented(sv)
            || (HasBlind(sv) && string.Equals(sv?.ArchetypeId, StowawayId, StringComparison.Ordinal));

        public bool SneaksFoodWhileOthersSleep(Survivor sv) => HidesInDarkCorners(sv);

        public bool ProvidesExactRaiderPlanLogs(Survivor sv) => HasUnseenListener(sv);

        /// <summary>Host raid-warning gate: any living Unseen Listener overhears raider plans.</summary>
        public bool AnyUnseenListenerWarning(IReadOnlyList<Survivor> survivors)
        {
            if (survivors == null) return false;
            for (int i = 0; i < survivors.Count; i++)
            {
                if (ProvidesExactRaiderPlanLogs(survivors[i])) return true;
            }
            return false;
        }

        public void RecordEarningKeepHiddenWater(
            Survivor stowaway,
            bool discoveredHiddenWaterSource,
            bool savedGroupFromDehydration,
            int currentDay = 0)
        {
            if (stowaway == null || !stowaway.IsAlive) return;
            if (!discoveredHiddenWaterSource || !savedGroupFromDehydration) return;
            var state = GetOrCreate(stowaway.Id);
            SyncFromSurvivor(stowaway, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.EarningKeep, StringComparison.Ordinal))
                return;
            state.Progress = 1f;
            stowaway.QuestProgress = 1f;
            OnQuestProgress?.Invoke(stowaway, "earning_keep", 1);
            CompleteQuestline(stowaway, currentDay);
        }

        // ── #311 Billionaire — Worthless Paper / Ruthless Capitalist ─────

        public float GetEntitledLaborMoraleHitPerDay(Survivor sv) =>
            HasEntitled(sv) ? EntitledLaborMoraleHitPerDay : 0f;

        public void ApplyEntitledLaborDay(Survivor sv, bool didPhysicalLabor)
        {
            if (!HasEntitled(sv) || sv == null || !sv.IsAlive || !didPhysicalLabor) return;
            sv.Needs.Morale = Mathf.Max(0f, sv.Needs.Morale - EntitledLaborMoraleHitPerDay);
        }

        public float GetStartingPreWarMoney(Survivor sv) =>
            string.Equals(sv?.ArchetypeId, BillionaireId, StringComparison.Ordinal)
                ? BillionaireStartingMoney
                : 0f;

        /// <summary>AI quirk: bribe others → bunker morale hit.</summary>
        public float GetBribeBunkerMoraleHit(Survivor sv) =>
            string.Equals(sv?.ArchetypeId, BillionaireId, StringComparison.Ordinal)
            || HasEntitled(sv)
                ? BillionaireBribeMoraleHit
                : 0f;

        public float GetRuthlessCapitalistFactionSellMult(Survivor sv) =>
            HasRuthlessCapitalist(sv) ? RuthlessCapitalistSellMult : 1f;

        public void RecordWorthlessPaperBurn(
            Survivor billionaire,
            float moneyBurned,
            bool heaterUsedToPreventFreeze,
            int currentDay = 0)
        {
            if (billionaire == null || !billionaire.IsAlive) return;
            if (!heaterUsedToPreventFreeze || moneyBurned + 0.001f < BillionaireStartingMoney) return;
            var state = GetOrCreate(billionaire.Id);
            SyncFromSurvivor(billionaire, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.WorthlessPaper, StringComparison.Ordinal))
                return;
            state.Progress = 1f;
            billionaire.QuestProgress = 1f;
            OnQuestProgress?.Invoke(billionaire, "worthless_paper", 1);
            CompleteQuestline(billionaire, currentDay);
        }

        // ── #312 Apprentice — The Master's Fate / Prodigy ────────────────

        public float GetClumsyToolBreakChance(Survivor sv) =>
            HasClumsy(sv) ? ClumsyToolBreakChance : 0f;

        public bool RollClumsyToolBreak(Survivor sv, System.Random rng = null)
        {
            float p = GetClumsyToolBreakChance(sv);
            if (p <= 0f) return false;
            rng ??= AtomicWar._Game.Utilities.SeededRandom.CreateFixed("personalquestsystem_ashes");
            return rng.NextDouble() < p;
        }

        /// <summary>AI quirk: questions slow nearby action speed.</summary>
        public float GetApprenticeQuestionSpeedMultOnTarget(Survivor apprentice) =>
            string.Equals(apprentice?.ArchetypeId, ApprenticeId, StringComparison.Ordinal)
            || HasClumsy(apprentice)
                ? ApprenticeQuestionSpeedMult
                : 1f;

        public bool InstantlyInheritsMaxSkillOfRoommate(Survivor sv) => HasProdigy(sv);

        public void ApplyProdigySkillInherit(Survivor apprentice, Survivor mentor)
        {
            if (!InstantlyInheritsMaxSkillOfRoommate(apprentice) || mentor == null) return;
            apprentice.MedicalSkill = Mathf.Max(apprentice.MedicalSkill, mentor.MedicalSkill);
            apprentice.CraftingSkill = Mathf.Max(apprentice.CraftingSkill, mentor.CraftingSkill);
            apprentice.ScienceSkill = Mathf.Max(apprentice.ScienceSkill, mentor.ScienceSkill);
            apprentice.ProgressionMedicalBonus = Mathf.Max(
                apprentice.ProgressionMedicalBonus, mentor.ProgressionMedicalBonus);
            apprentice.ProgressionCraftingBonus = Mathf.Max(
                apprentice.ProgressionCraftingBonus, mentor.ProgressionCraftingBonus);
            apprentice.ProgressionCombatBonus = Mathf.Max(
                apprentice.ProgressionCombatBonus, mentor.ProgressionCombatBonus);
        }

        public void RecordMastersFateBurial(
            Survivor apprentice,
            bool retrievedHistorianBody,
            bool buried,
            int currentDay = 0)
        {
            if (apprentice == null || !apprentice.IsAlive) return;
            if (!retrievedHistorianBody || !buried) return;
            var state = GetOrCreate(apprentice.Id);
            SyncFromSurvivor(apprentice, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheMastersFate, StringComparison.Ordinal))
                return;
            state.Progress = 1f;
            apprentice.QuestProgress = 1f;
            OnQuestProgress?.Invoke(apprentice, "masters_fate", 1);
            CompleteQuestline(apprentice, currentDay);
        }

        // ── #313 Fire Chief — The Backdraft / Commander ──────────────────

        public float GetBurnScarsCharismaMultiplier(Survivor sv) =>
            HasBurnScars(sv) ? BurnScarsCharismaMult : 1f;

        public bool IsImmuneToSuperficialFireDamage(Survivor sv) =>
            HasBurnScars(sv) || HasCommander(sv);

        /// <summary>AI quirk: aggressively prioritize AirFilter upgrades.</summary>
        public bool PrioritizesAirFilterUpgrades(Survivor sv) =>
            string.Equals(sv?.ArchetypeId, FireChiefId, StringComparison.Ordinal)
            || HasBurnScars(sv)
            || HasCommander(sv);

        public bool GrantsEmergencyCommanderAura(Survivor sv) => HasCommander(sv);

        public bool IsEmergencyAiPerfectlyEfficient(
            IReadOnlyList<Survivor> survivors,
            bool fireOrRaidEvent)
        {
            if (!fireOrRaidEvent || survivors == null) return false;
            for (int i = 0; i < survivors.Count; i++)
            {
                if (GrantsEmergencyCommanderAura(survivors[i])) return true;
            }
            return false;
        }

        public void RecordBackdraftSaves(
            Survivor chief,
            int trappedSurvivorsSaved,
            int currentDay = 0)
        {
            if (chief == null || !chief.IsAlive) return;
            var state = GetOrCreate(chief.Id);
            SyncFromSurvivor(chief, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheBackdraft, StringComparison.Ordinal))
                return;
            state.BackdraftSaves += Mathf.Max(0, trappedSurvivorsSaved);
            state.Progress = state.BackdraftSaves;
            chief.QuestProgress = state.BackdraftSaves;
            OnQuestProgress?.Invoke(chief, "backdraft", state.BackdraftSaves);
            if (state.BackdraftSaves >= FireChiefSavesRequired)
                CompleteQuestline(chief, currentDay);
        }

        // ── #314 Amputee — The Prosthetic / Cyber-Arm ────────────────────

        public float GetMissingArmSpeedMultiplier(Survivor sv) =>
            HasMissingArm(sv) ? MissingArmSpeedMult : 1f;

        /// <summary>AI quirk: tinker scrap → small noise.</summary>
        public float GetAmputeeTinkerNoise(Survivor sv) =>
            string.Equals(sv?.ArchetypeId, AmputeeId, StringComparison.Ordinal)
            || HasMissingArm(sv)
                ? AmputeeTinkerNoise
                : 0f;

        public float GetCyberArmCraftSpeedMultiplier(Survivor sv) =>
            HasCyberArm(sv) ? CyberArmCraftMeleeMult : 1f;

        public float GetCyberArmMeleeMultiplier(Survivor sv) =>
            HasCyberArm(sv) ? CyberArmCraftMeleeMult : 1f;

        public bool CanPunchThroughBlockedDoors(Survivor sv) => HasCyberArm(sv);

        public void RecordProstheticParts(
            Survivor amputee,
            int mechanicalPartsTotal,
            int electronicScrapTotal,
            int currentDay = 0)
        {
            if (amputee == null || !amputee.IsAlive) return;
            var state = GetOrCreate(amputee.Id);
            SyncFromSurvivor(amputee, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheProsthetic, StringComparison.Ordinal))
                return;
            state.ScrapMechanicalParts = Mathf.Max(state.ScrapMechanicalParts, mechanicalPartsTotal);
            state.ScrapElectronicScrap = Mathf.Max(state.ScrapElectronicScrap, electronicScrapTotal);
            state.Progress = state.ScrapMechanicalParts + state.ScrapElectronicScrap;
            amputee.QuestProgress = state.Progress;
            OnQuestProgress?.Invoke(amputee, "prosthetic", (int)state.Progress);
            if (state.ScrapMechanicalParts >= ProstheticMechanicalPartsRequired
                && state.ScrapElectronicScrap >= ProstheticElectronicScrapRequired)
            {
                CompleteQuestline(amputee, currentDay);
                // Remove missing arm disability once cyber arm unlocks.
                if (amputee.DisabilityIds != null)
                    amputee.DisabilityIds.RemoveAll(id =>
                        string.Equals(id, MissingArmDisabilityId, StringComparison.OrdinalIgnoreCase));
            }
        }

        // ── #315 Inmate — A Good Death / Redemption ──────────────────────

        public float GetLethalInstantKillChance(Survivor sv) =>
            HasLethal(sv) ? LethalInstantKillChance : 0f;

        public bool RollLethalInstantKill(Survivor sv, System.Random rng = null)
        {
            float p = GetLethalInstantKillChance(sv);
            if (p <= 0f) return false;
            rng ??= AtomicWar._Game.Utilities.SeededRandom.CreateFixed("personalquestsystem_ashes");
            return rng.NextDouble() < p;
        }

        public bool IsUntrustedByAllFactions(Survivor sv) => HasUntrusted(sv);

        public float GetFactionTrustWithUntrusted(Survivor sv, float baseTrust) =>
            IsUntrustedByAllFactions(sv) ? 0f : baseTrust;

        /// <summary>AI quirk: craft and hide shivs.</summary>
        public bool CraftsAndHidesShivs(Survivor sv) =>
            string.Equals(sv?.ArchetypeId, InmateId, StringComparison.Ordinal)
            || HasLethal(sv)
            || HasUntrusted(sv);

        public bool MeleeCausesFearAndRout(Survivor sv) => HasRedemption(sv);

        public void RecordGoodDeathAirlockHold(
            Survivor inmate,
            bool heldAirlockAlone,
            bool bunkerSurvived,
            bool inmateSurvived,
            int currentDay = 0)
        {
            if (inmate == null) return;
            if (!heldAirlockAlone || !bunkerSurvived) return;
            var state = GetOrCreate(inmate.Id);
            SyncFromSurvivor(inmate, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.AGoodDeath, StringComparison.Ordinal))
                return;
            state.Progress = 1f;
            inmate.QuestProgress = 1f;
            OnQuestProgress?.Invoke(inmate, "good_death", 1);
            if (inmateSurvived && inmate.IsAlive)
            {
                CompleteQuestline(inmate, currentDay);
            }
            else
            {
                // Sacrificial completion without latent if dead — still mark quest done.
                state.QuestActive = false;
                inmate.QuestlineActive = false;
                state.TraitUnlocked = false;
            }
        }

        // ── #316 Synth — Tears in Rain / Overclocked ─────────────────────

        public bool IsAndroid(Survivor sv) => HasAndroid(sv) || HasOverclocked(sv);

        public bool NeedsFoodOrWater(Survivor sv) => !IsAndroid(sv);

        public bool HealsWithPowerAndMechanicalParts(Survivor sv) => IsAndroid(sv);

        /// <summary>AI quirk: white fluid discovery → bunker paranoia.</summary>
        public bool BleedsWhiteFluid(Survivor sv) => IsAndroid(sv);

        public void ApplySynthDiscoveryParanoia(IReadOnlyList<Survivor> survivors, Survivor synth)
        {
            if (survivors == null || !BleedsWhiteFluid(synth)) return;
            for (int i = 0; i < survivors.Count; i++)
            {
                var s = survivors[i];
                if (s == null || !s.IsAlive || s.Id == synth.Id) continue;
                if (string.IsNullOrEmpty(s.currentMentalBreakId))
                    s.currentMentalBreakId = "paranoia";
                s.Needs.Morale = Mathf.Max(0f, s.Needs.Morale - 15f);
            }
        }

        public bool NeedsSleep(Survivor sv) => !HasOverclocked(sv);

        public bool IsImmuneToRadiationAsAndroid(Survivor sv) => HasOverclocked(sv);

        public bool CannotBeRevivedIfBroken(Survivor sv) => HasOverclocked(sv);

        public void RecordTearsInRainEmp(
            Survivor synth,
            bool empEventOccurred,
            bool memoryCoreIntact,
            int currentDay = 0)
        {
            if (synth == null || !synth.IsAlive) return;
            if (!empEventOccurred || !memoryCoreIntact) return;
            var state = GetOrCreate(synth.Id);
            SyncFromSurvivor(synth, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TearsInRain, StringComparison.Ordinal))
                return;
            // Day 30 EMP event timing is host-enforced; we accept the record.
            state.Progress = 1f;
            synth.QuestProgress = 1f;
            OnQuestProgress?.Invoke(synth, "tears_in_rain", 1);
            CompleteQuestline(synth, currentDay);
        }

        // ── #317 Dog — Man's Best Friend / Wasteland Guardian ────────────

        public bool ProvidesGoodBoyRoomMorale(Survivor sv) => HasGoodBoy(sv);

        public float GetGoodBoyRoomMoraleAura(Survivor sv) =>
            HasGoodBoy(sv) ? GoodBoyRoomMoraleAura : 0f;

        /// <summary>Host daily tick: room-mates sharing the dog's room gain a small morale aura.</summary>
        public void ApplyGoodBoyMoraleAura(Survivor dog, IReadOnlyList<Survivor> survivors)
        {
            float aura = GetGoodBoyRoomMoraleAura(dog);
            if (aura <= 0f || dog == null || survivors == null) return;
            for (int i = 0; i < survivors.Count; i++)
            {
                var s = survivors[i];
                if (s == null || !s.IsAlive || s.Id == dog.Id) continue;
                if (string.IsNullOrEmpty(dog.CurrentRoomId)
                    || !string.Equals(s.CurrentRoomId, dog.CurrentRoomId, StringComparison.Ordinal))
                    continue;
                s.Needs.Morale = Mathf.Clamp(s.Needs.Morale + aura, 0f, 100f);
            }
        }

        public bool CannotCraftOrBuild(Survivor sv) =>
            HasGoodBoy(sv) || string.Equals(sv?.ArchetypeId, DogId, StringComparison.Ordinal);

        /// <summary>AI quirk: dig up bones/scrap from dirt floor.</summary>
        public bool DigsUpBonesOrScrap(Survivor sv) =>
            string.Equals(sv?.ArchetypeId, DogId, StringComparison.Ordinal)
            || HasGoodBoy(sv);

        public bool AutoHuntsVermin(Survivor sv) => HasWastelandGuardian(sv);

        public bool TracksHumanScentsPerfectly(Survivor sv) => HasWastelandGuardian(sv);

        public void RecordMansBestFriendIntercept(
            Survivor dog,
            bool interceptedLethalAttackOnOwner,
            int currentDay = 0)
        {
            if (dog == null || !dog.IsAlive) return;
            if (!interceptedLethalAttackOnOwner) return;
            var state = GetOrCreate(dog.Id);
            SyncFromSurvivor(dog, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.MansBestFriend, StringComparison.Ordinal))
                return;
            state.Progress = 1f;
            dog.QuestProgress = 1f;
            OnQuestProgress?.Invoke(dog, "mans_best_friend", 1);
            CompleteQuestline(dog, currentDay);
        }

        // ── #318 Core — The Turing Test / Omniscience ────────────────────

        public bool IsBunkerCore(Survivor sv) =>
            HasBunkerCore(sv)
            || string.Equals(sv?.ArchetypeId, CoreId, StringComparison.Ordinal);

        public bool DiesWhenPowerNetworkFails(Survivor sv) => IsBunkerCore(sv);

        public void NotifyPowerNetworkFailure(Survivor core)
        {
            if (!DiesWhenPowerNetworkFails(core) || core == null) return;
            if (!HasOmniscience(core))
                core.State = SurvivorState.Dead;
        }

        public bool CanProgramAutomationMacros(Survivor sv) => HasOmniscience(sv);

        public void RecordTuringTestDecryption(
            Survivor core,
            bool decryptedMilitaryRadioNetwork,
            int currentDay = 0)
        {
            if (core == null || !core.IsAlive) return;
            if (!decryptedMilitaryRadioNetwork) return;
            if (currentDay > TuringTestDeadlineDay) return;
            var state = GetOrCreate(core.Id);
            SyncFromSurvivor(core, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheTuringTest, StringComparison.Ordinal))
                return;
            state.Progress = 1f;
            core.QuestProgress = 1f;
            OnQuestProgress?.Invoke(core, "turing_test", 1);
            CompleteQuestline(core, currentDay);
        }
    }
}
