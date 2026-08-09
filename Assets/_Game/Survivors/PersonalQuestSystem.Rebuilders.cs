using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Prompts #284–#298 — Rebuilders, scholars, and outlaws.
    /// Quest recorders, base-trait quirks, and latent-trait host APIs.
    /// </summary>
    public partial class PersonalQuestSystem
    {
        // ── #284 Coal Miner — The Canary / Deep Delver ───────────────────

        /// <summary>Black Lung: max stamina permanently −20%.</summary>
        public float GetBlackLungStaminaMaxMultiplier(Survivor sv) =>
            HasBlackLung(sv) ? BlackLungStaminaMaxMult : 1f;

        /// <summary>Claustrophilic: morale gain in small underground rooms.</summary>
        public float GetClaustrophilicMoralePerHour(Survivor sv, bool inSmallUndergroundRoom) =>
            HasClaustrophilic(sv) && inSmallUndergroundRoom
                ? ClaustrophilicMoralePerHour
                : 0f;

        public void ApplyClaustrophilicMorale(
            Survivor sv, bool inSmallUndergroundRoom, float gameHours = 1f)
        {
            float d = GetClaustrophilicMoralePerHour(sv, inSmallUndergroundRoom) * gameHours;
            if (Mathf.Abs(d) < 0.001f || sv == null || !sv.IsAlive) return;
            // QUEST-001: route through ApplyMoraleDelta so Traumatized 50% cap holds.
            ApplyMoraleDelta(sv, d);
        }

        /// <summary>AI quirk: prefer deepest/lowest room for sleep over comfort.</summary>
        public bool PrefersDeepestRoomToSleep(Survivor sv) =>
            string.Equals(sv?.ArchetypeId, CoalMinerId, StringComparison.Ordinal)
            || HasBlackLung(sv)
            || HasDeepDelver(sv);

        /// <summary>Deep Delver: immune to O2/CO2 atmospheric penalties.</summary>
        public bool IsImmuneToAtmosphereGasPenalties(Survivor sv) => HasDeepDelver(sv);

        /// <summary>Deep Delver: excavating rubble takes 75% less time (0.25× duration).</summary>
        public float GetDeepDelverExcavationDurationMultiplier(Survivor sv) =>
            HasDeepDelver(sv) ? DeepDelverExcavationDurationMult : 1f;

        /// <summary>
        /// Survive a CO leak shelter event with zero health damage while saving another.
        /// </summary>
        public void RecordCanaryCoLeakSurvived(
            Survivor miner,
            bool coLeakEvent,
            float healthDamageTaken,
            bool savedAnother,
            int currentDay = 0)
        {
            if (miner == null || !miner.IsAlive) return;
            if (!coLeakEvent || healthDamageTaken > 0.001f || !savedAnother) return;
            var state = GetOrCreate(miner.Id);
            SyncFromSurvivor(miner, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheCanary, StringComparison.Ordinal))
                return;

            state.Progress = 1f;
            miner.QuestProgress = 1f;
            OnQuestProgress?.Invoke(miner, "canary_co_leak", 1);
            CompleteQuestline(miner, currentDay);
        }

        // ── #285 Truck Driver — The Long Haul / Logistics Master ─────────

        /// <summary>Caffeinated: sleep restores half as much fatigue.</summary>
        public float GetCaffeinatedSleepRestoreMultiplier(Survivor sv) =>
            HasCaffeinated(sv) ? CaffeinatedSleepRestoreMult : 1f;

        /// <summary>Needs clean water regularly or suffers fatigue crash.</summary>
        public bool NeedsConstantCleanWater(Survivor sv) =>
            HasCaffeinated(sv)
            || string.Equals(sv?.ArchetypeId, TruckDriverId, StringComparison.Ordinal);

        public void ApplyCaffeinatedWaterCrash(Survivor sv, bool drankCleanWaterToday)
        {
            if (!NeedsConstantCleanWater(sv) || sv == null || !sv.IsAlive) return;
            if (drankCleanWaterToday) return;
            if (_needsSystem != null)
                _needsSystem.Modify(sv, NeedKind.Fatigue, CaffeinatedFatigueCrash);
            else
                sv.Needs.Fatigue = Mathf.Min(100f, sv.Needs.Fatigue + CaffeinatedFatigueCrash);
        }

        /// <summary>Host: mark that this survivor drank clean water today (#285).</summary>
        public void NotifyCleanWaterConsumed(Survivor sv)
        {
            if (sv == null || string.IsNullOrEmpty(sv.Id)) return;
            GetOrCreate(sv.Id).DrankCleanWaterThisDay = true;
        }

        /// <summary>Cabin fever builds twice as fast; needs expeditions.</summary>
        public float GetCabinFeverRateMultiplier(Survivor sv) =>
            string.Equals(sv?.ArchetypeId, TruckDriverId, StringComparison.Ordinal)
            || HasCaffeinated(sv)
                ? TruckDriverCabinFeverMult
                : 1f;

        /// <summary>Logistics Master: carrying capacity ×3.</summary>
        public float GetLogisticsMasterCarryCapacityMultiplier(Survivor sv) =>
            HasLogisticsMaster(sv) ? LogisticsMasterCarryCapacityMult : 1f;

        /// <summary>Logistics Master: internal hauling moves stacks instantly.</summary>
        public bool HaulsStacksInstantly(Survivor sv) => HasLogisticsMaster(sv);

        /// <summary>Complete a scavenging run at ≥200% max encumbrance.</summary>
        public void RecordLongHaulOverencumberedRun(
            Survivor driver,
            float encumbranceRatio,
            bool runCompleted,
            int currentDay = 0)
        {
            if (driver == null || !driver.IsAlive) return;
            if (!runCompleted || encumbranceRatio + 0.001f < LongHaulEncumbranceRatio) return;
            var state = GetOrCreate(driver.Id);
            SyncFromSurvivor(driver, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheLongHaul, StringComparison.Ordinal))
                return;

            state.Progress = 1f;
            driver.QuestProgress = 1f;
            OnQuestProgress?.Invoke(driver, "long_haul", 1);
            CompleteQuestline(driver, currentDay);
        }

        // ── #286 Welder — The Iron Gate / Forge Master ───────────────────

        /// <summary>Calloused: immune to fire and temperature damage.</summary>
        public bool IsImmuneToFireAndTemperatureDamage(Survivor sv) => HasCalloused(sv);

        /// <summary>Deaf in one ear: 50% stealth check fail on expeditions.</summary>
        public float GetDeafStealthFailChance(Survivor sv) =>
            HasDeafInOneEar(sv) ? DeafStealthFailChance : 0f;

        public bool RollDeafStealthFail(Survivor sv, System.Random rng = null)
        {
            float p = GetDeafStealthFailChance(sv);
            if (p <= 0f) return false;
            rng ??= AtomicWar._Game.Utilities.SeededRandom.Stream("personalquestsystem_rebuilders");
            return rng.NextDouble() < p;
        }

        /// <summary>Over-engineers repairs: 2× scrap cost, 150% max durability.</summary>
        public float GetWelderRepairScrapMultiplier(Survivor sv) =>
            string.Equals(sv?.ArchetypeId, WelderId, StringComparison.Ordinal)
            || HasCalloused(sv)
                ? WelderRepairScrapMult
                : 1f;

        public float GetWelderRepairMaxDurabilityMultiplier(Survivor sv) =>
            string.Equals(sv?.ArchetypeId, WelderId, StringComparison.Ordinal)
            || HasCalloused(sv)
                ? WelderRepairMaxDurabilityMult
                : 1f;

        /// <summary>Forge Master: craft military armor from scrap; repairs never degrade.</summary>
        public bool CanCraftMilitaryArmorFromScrap(Survivor sv) => HasForgeMaster(sv);

        public bool RepairsNeverDegradePassively(Survivor sv) => HasForgeMaster(sv);

        /// <summary>Build and fully upgrade the hatch to absolute maximum level.</summary>
        public void RecordIronGateHatchMaxed(
            Survivor welder,
            int hatchLevel,
            int maxLevel,
            int currentDay = 0)
        {
            if (welder == null || !welder.IsAlive) return;
            if (maxLevel <= 0 || hatchLevel < maxLevel) return;
            var state = GetOrCreate(welder.Id);
            SyncFromSurvivor(welder, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheIronGate, StringComparison.Ordinal))
                return;

            state.Progress = hatchLevel;
            welder.QuestProgress = hatchLevel;
            OnQuestProgress?.Invoke(welder, "iron_gate", hatchLevel);
            CompleteQuestline(welder, currentDay);
        }

        // ── #287 Custodian — The Mess / Sanitization Expert ──────────────

        /// <summary>Invisible: no interpersonal affinity gains or losses.</summary>
        public bool BlocksInterpersonalAffinity(Survivor sv) => HasInvisible(sv);

        /// <summary>Neat Freak: morale loss when hygiene &lt; 80%.</summary>
        public float GetNeatFreakHygieneMoraleHit(Survivor sv, float hygiene01) =>
            HasNeatFreak(sv) && hygiene01 < NeatFreakHygieneThreshold
                ? NeatFreakMoraleHit
                : 0f;

        public void ApplyNeatFreakHygienePressure(Survivor sv, float hygiene01)
        {
            float hit = GetNeatFreakHygieneMoraleHit(sv, hygiene01);
            if (hit <= 0f || sv == null || !sv.IsAlive) return;
            // QUEST-001: route through ApplyMoraleDelta so Traumatized 50% cap holds.
            ApplyMoraleDelta(sv, -hit);
        }

        /// <summary>AI quirk: clean waste/mold before thirst or hunger.</summary>
        public bool PrioritizesCleaningOverNeeds(Survivor sv) =>
            string.Equals(sv?.ArchetypeId, CustodianId, StringComparison.Ordinal)
            || HasNeatFreak(sv)
            || HasInvisible(sv);

        /// <summary>Sanitization Expert: bunker immune to pests and disease spread.</summary>
        public bool GrantsBunkerPestAndDiseaseImmunity(Survivor sv) => HasSanitizationExpert(sv);

        public bool BunkerImmuneToPestsAndDisease(IReadOnlyList<Survivor> survivors)
        {
            if (survivors == null) return false;
            for (int i = 0; i < survivors.Count; i++)
            {
                if (GrantsBunkerPestAndDiseaseImmunity(survivors[i])) return true;
            }
            return false;
        }

        /// <summary>Personally clean up one human corpse toward The Mess (5).</summary>
        public void RecordCorpseCleaned(Survivor custodian, bool humanCorpse, int currentDay = 0)
        {
            if (custodian == null || !custodian.IsAlive || !humanCorpse) return;
            var state = GetOrCreate(custodian.Id);
            SyncFromSurvivor(custodian, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheMess, StringComparison.Ordinal))
                return;

            state.CorpsesCleaned++;
            state.Progress = state.CorpsesCleaned;
            custodian.QuestProgress = state.CorpsesCleaned;
            OnQuestProgress?.Invoke(custodian, "corpses_cleaned", state.CorpsesCleaned);
            if (state.CorpsesCleaned >= MessCorpsesRequired)
                CompleteQuestline(custodian, currentDay);
        }

        // ── #288 Lumberjack — The Clearcut / Deforester ──────────────────

        /// <summary>Brawn: melee damage ×2, firearms accuracy −50%.</summary>
        public float GetBrawnMeleeDamageMultiplier(Survivor sv) =>
            HasBrawn(sv) ? BrawnMeleeDamageMult : 1f;

        public float GetBrawnFirearmsAccuracyMultiplier(Survivor sv) =>
            HasBrawn(sv) ? BrawnFirearmsAccuracyMult : 1f;

        /// <summary>AI quirk: destroy broken wood modules for scrap when morale is low.</summary>
        public bool ShouldSalvageBrokenWoodWhenMoraleLow(Survivor sv, float morale) =>
            (string.Equals(sv?.ArchetypeId, LumberjackId, StringComparison.Ordinal) || HasBrawn(sv))
            && morale < LumberjackSalvageMoraleThreshold;

        /// <summary>Deforester: forest wood yield ×5; melee bleeds / dismembers raiders.</summary>
        public float GetDeforesterWoodYieldMultiplier(Survivor sv) =>
            HasDeforester(sv) ? DeforesterWoodYieldMult : 1f;

        public bool MeleeCausesBleedingAndDismember(Survivor sv) => HasDeforester(sv);

        /// <summary>Kill a mutated bear using only an axe.</summary>
        public void RecordClearcutBearKill(
            Survivor lumberjack,
            string enemyId,
            string weaponId,
            bool killed,
            int currentDay = 0)
        {
            if (lumberjack == null || !lumberjack.IsAlive || !killed) return;
            if (!string.Equals(enemyId, MutatedBearEnemyId, StringComparison.Ordinal)
                && !string.Equals(enemyId, "mutated_bear", StringComparison.OrdinalIgnoreCase))
                return;
            if (!string.Equals(weaponId, AxeWeaponId, StringComparison.Ordinal)
                && (weaponId == null
                    || weaponId.IndexOf("axe", StringComparison.OrdinalIgnoreCase) < 0))
                return;

            var state = GetOrCreate(lumberjack.Id);
            SyncFromSurvivor(lumberjack, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheClearcut, StringComparison.Ordinal))
                return;

            state.Progress = 1f;
            lumberjack.QuestProgress = 1f;
            OnQuestProgress?.Invoke(lumberjack, "clearcut_bear", 1);
            CompleteQuestline(lumberjack, currentDay);
        }

        // ── #289 Microbiologist — The Strain / Epidemiologist ────────────

        /// <summary>Germaphobe: refuses medical triage without hazmat in bunker.</summary>
        public bool RequiresHazmatForTriage(Survivor sv) =>
            HasGermaphobe(sv)
            || string.Equals(sv?.ArchetypeId, MicrobiologistId, StringComparison.Ordinal);

        public bool CanPerformTriage(Survivor sv, bool hazmatEquipped, bool inBunker)
        {
            if (!RequiresHazmatForTriage(sv)) return true;
            if (!inBunker) return true;
            return hazmatEquipped;
        }

        /// <summary>AI quirk: 20% chance to refuse rations as tainted.</summary>
        public bool RollRefuseRationsAsTainted(Survivor sv, System.Random rng = null)
        {
            if (sv == null || !sv.IsAlive) return false;
            if (!string.Equals(sv.ArchetypeId, MicrobiologistId, StringComparison.Ordinal)
                && !HasGermaphobe(sv))
                return false;
            rng ??= AtomicWar._Game.Utilities.SeededRandom.Stream("personalquestsystem_rebuilders");
            return rng.NextDouble() < MicrobiologistRefuseRationChance;
        }

        /// <summary>Epidemiologist: craft vaccines granting Phase-1 immunity bunker-wide.</summary>
        public bool CanCraftPhase1Vaccines(Survivor sv) => HasEpidemiologist(sv);

        /// <summary>
        /// Cultivate sepsis for a sample then cure the patient.
        /// </summary>
        public void RecordStrainSepsisSample(
            Survivor micro,
            bool cultivatedSepsis,
            bool extractedSample,
            bool curedPatient,
            int currentDay = 0)
        {
            if (micro == null || !micro.IsAlive) return;
            if (!cultivatedSepsis || !extractedSample || !curedPatient) return;
            var state = GetOrCreate(micro.Id);
            SyncFromSurvivor(micro, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheStrain, StringComparison.Ordinal))
                return;

            state.Progress = 1f;
            micro.QuestProgress = 1f;
            OnQuestProgress?.Invoke(micro, "strain_sample", 1);
            CompleteQuestline(micro, currentDay);
        }

        // ── #290 Astronomer — The Dead Stars / Celestial Navigator ───────

        /// <summary>Night Owl: 0.5× day speed, 1.5× night speed.</summary>
        public float GetNightOwlActionSpeedMultiplier(Survivor sv, bool isNight)
        {
            if (!HasNightOwl(sv)
                && !string.Equals(sv?.ArchetypeId, AstronomerId, StringComparison.Ordinal))
                return 1f;
            return isNight ? NightOwlNightSpeedMult : NightOwlDaySpeedMult;
        }

        /// <summary>AI quirk: climbs to surface hatch at night (sniper risk if unsupervised).</summary>
        public bool SeeksSurfaceSkyAtNight(Survivor sv, bool isNight) =>
            isNight
            && (string.Equals(sv?.ArchetypeId, AstronomerId, StringComparison.Ordinal)
                || HasNightOwl(sv));

        /// <summary>Celestial Navigator: night pathing halves travel; zero night ambushes.</summary>
        public float GetCelestialNavigatorNightTravelMultiplier(Survivor sv) =>
            HasCelestialNavigator(sv) ? CelestialNavigatorNightTravelMult : 1f;

        public bool GuaranteesZeroNightAmbushes(Survivor sv) => HasCelestialNavigator(sv);

        /// <summary>Survive one fallout storm exposed on the surface (need 3).</summary>
        public void RecordDeadStarsStormExposed(
            Survivor astronomer,
            bool falloutStorm,
            bool surfaceExposed,
            bool survived,
            int currentDay = 0)
        {
            if (astronomer == null || !astronomer.IsAlive) return;
            if (!falloutStorm || !surfaceExposed || !survived) return;
            var state = GetOrCreate(astronomer.Id);
            SyncFromSurvivor(astronomer, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheDeadStars, StringComparison.Ordinal))
                return;

            state.FalloutStormsSurvivedExposed++;
            state.Progress = state.FalloutStormsSurvivedExposed;
            astronomer.QuestProgress = state.FalloutStormsSurvivedExposed;
            OnQuestProgress?.Invoke(astronomer, "dead_stars", state.FalloutStormsSurvivedExposed);
            if (state.FalloutStormsSurvivedExposed >= DeadStarsStormsRequired)
                CompleteQuestline(astronomer, currentDay);
        }

        // ── #291 Librarian — The Archive / Archivist ─────────────────────

        /// <summary>Quiet: zero noise pollution from any action.</summary>
        public bool GeneratesZeroNoise(Survivor sv) => HasQuiet(sv);

        /// <summary>Frail: max health capped at 60.</summary>
        public float GetFrailMaxHealthCap(Survivor sv) =>
            HasFrail(sv) ? FrailMaxHealthCap : 100f;

        /// <summary>AI quirk: hoard books in personal stash.</summary>
        public bool HoardsBooksInPersonalStash(Survivor sv) =>
            string.Equals(sv?.ArchetypeId, LibrarianId, StringComparison.Ordinal)
            || HasQuiet(sv)
            || HasFrail(sv);

        /// <summary>Books burned for fuel trigger mental break risk.</summary>
        public bool SuffersMentalBreakIfBooksBurned(Survivor sv) =>
            HoardsBooksInPersonalStash(sv);

        /// <summary>Archivist: bunker-wide skill decay stops.</summary>
        public bool StopsBunkerSkillDecay(Survivor sv) => HasArchivist(sv);

        public bool BunkerSkillDecayStopped(IReadOnlyList<Survivor> survivors)
        {
            if (survivors == null) return false;
            for (int i = 0; i < survivors.Count; i++)
            {
                if (StopsBunkerSkillDecay(survivors[i])) return true;
            }
            return false;
        }

        /// <summary>Collect one unique intel/lore item toward The Archive (50).</summary>
        public void RecordArchiveIntelCollected(
            Survivor librarian,
            string intelId,
            int currentDay = 0)
        {
            if (librarian == null || !librarian.IsAlive || string.IsNullOrEmpty(intelId)) return;
            var state = GetOrCreate(librarian.Id);
            SyncFromSurvivor(librarian, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheArchive, StringComparison.Ordinal))
                return;

            if (state.UniqueIntelIds == null)
                state.UniqueIntelIds = new List<string>();
            for (int i = 0; i < state.UniqueIntelIds.Count; i++)
            {
                if (string.Equals(state.UniqueIntelIds[i], intelId, StringComparison.Ordinal))
                    return;
            }
            state.UniqueIntelIds.Add(intelId);
            state.Progress = state.UniqueIntelIds.Count;
            librarian.QuestProgress = state.UniqueIntelIds.Count;
            OnQuestProgress?.Invoke(librarian, "archive_intel", state.UniqueIntelIds.Count);
            if (state.UniqueIntelIds.Count >= ArchiveIntelRequired)
                CompleteQuestline(librarian, currentDay);
        }

        // ── #292 Accountant — In the Black / Auditor ─────────────────────

        /// <summary>Penny Pincher: only eats when hunger ≥ 95% (at 5% remaining food need).</summary>
        public bool OnlyEatsWhenStarving(Survivor sv) => HasPennyPincher(sv);

        public bool ShouldRefuseFoodUntilCritical(Survivor sv, float hunger01) =>
            OnlyEatsWhenStarving(sv) && hunger01 < PennyPincherHungerEatThreshold;

        /// <summary>
        /// AI quirk: lock storage after 3 deficit days (consumption &gt; production).
        /// </summary>
        public void RecordResourceDeficitDay(Survivor accountant, bool deficit, int currentDay = 0)
        {
            if (accountant == null || !accountant.IsAlive) return;
            var state = GetOrCreate(accountant.Id);
            if (deficit)
            {
                state.ResourceDeficitDays++;
                if (state.ResourceDeficitDays >= AccountantDeficitDaysToLock)
                    state.StorageLockedByAccountant = true;
            }
            else
            {
                state.ResourceDeficitDays = 0;
            }
        }

        public bool IsStorageLockedByAccountant(Survivor accountant) =>
            accountant != null && GetOrCreate(accountant.Id).StorageLockedByAccountant;

        public void OverrideAccountantStorageLock(Survivor accountant)
        {
            if (accountant == null) return;
            var state = GetOrCreate(accountant.Id);
            state.StorageLockedByAccountant = false;
            state.ResourceDeficitDays = 0;
        }

        public bool AnyAccountantStorageLock(IReadOnlyList<Survivor> survivors)
        {
            if (survivors == null) return false;
            for (int i = 0; i < survivors.Count; i++)
            {
                if (IsStorageLockedByAccountant(survivors[i])) return true;
            }
            return false;
        }

        /// <summary>Auditor: favorable trade; scavenging reveals remaining item counts.</summary>
        public bool HasFavorableTradeValues(Survivor sv) => HasAuditor(sv);

        public bool RevealsScavengeRemainingCounts(Survivor sv) => HasAuditor(sv);

        /// <summary>Bunker at 100% capacity in food, water, and fuel simultaneously.</summary>
        public void RecordInTheBlackCapacities(
            Survivor accountant,
            float foodFill01,
            float waterFill01,
            float fuelFill01,
            int currentDay = 0)
        {
            if (accountant == null || !accountant.IsAlive) return;
            if (foodFill01 + 0.001f < 1f || waterFill01 + 0.001f < 1f || fuelFill01 + 0.001f < 1f)
                return;
            var state = GetOrCreate(accountant.Id);
            SyncFromSurvivor(accountant, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.InTheBlack, StringComparison.Ordinal))
                return;

            state.Progress = 1f;
            accountant.QuestProgress = 1f;
            OnQuestProgress?.Invoke(accountant, "in_the_black", 1);
            CompleteQuestline(accountant, currentDay);
        }

        // ── #293 Musician — The Masterpiece / Maestro ────────────────────

        /// <summary>Fragile Ego: morale drops twice as fast after craft/repair failure.</summary>
        public float GetFragileEgoFailureMoraleMultiplier(Survivor sv) =>
            HasFragileEgo(sv) ? FragileEgoFailureMoraleMult : 1f;

        /// <summary>Play Instrument: AoE morale for adjacent rooms (no resource cost).</summary>
        public bool CanPlayInstrument(Survivor sv) =>
            string.Equals(sv?.ArchetypeId, MusicianId, StringComparison.Ordinal)
            || HasFragileEgo(sv)
            || HasMaestro(sv);

        public float GetPlayInstrumentMoraleAura(Survivor sv) =>
            CanPlayInstrument(sv)
                ? (HasMaestro(sv) ? MaestroPlayMoraleAura : PlayInstrumentMoraleAura)
                : 0f;

        /// <summary>Maestro: play cures mental breaks and suppresses rad anxiety 24h.</summary>
        public bool PlayInstrumentCuresMentalBreaks(Survivor sv) => HasMaestro(sv);

        public bool PlayInstrumentSuppressesRadAnxiety(Survivor sv) => HasMaestro(sv);

        public void ApplyPlayInstrumentAura(
            Survivor musician,
            IReadOnlyList<Survivor> inAdjacentRooms,
            int currentDay = 0)
        {
            if (musician == null || !musician.IsAlive || !CanPlayInstrument(musician)) return;
            float aura = GetPlayInstrumentMoraleAura(musician);
            if (inAdjacentRooms != null && aura > 0f)
            {
                for (int i = 0; i < inAdjacentRooms.Count; i++)
                {
                    var s = inAdjacentRooms[i];
                    if (s == null || !s.IsAlive) continue;
                    if (_needsSystem != null)
                        _needsSystem.Modify(s, NeedKind.Morale, aura);
                    else
                        s.Needs.Morale = Mathf.Clamp(s.Needs.Morale + aura, 0f, 100f);
                    if (PlayInstrumentCuresMentalBreaks(musician)
                        && !string.IsNullOrEmpty(s.currentMentalBreakId))
                        s.currentMentalBreakId = null;
                }
            }
            if (PlayInstrumentSuppressesRadAnxiety(musician))
            {
                var state = GetOrCreate(musician.Id);
                state.MaestroRadAnxietySuppressUntilDay = currentDay + MaestroSuppressDays;
            }
        }

        public bool IsRadAnxietySuppressedByMaestro(
            IReadOnlyList<Survivor> survivors, int currentDay)
        {
            if (survivors == null) return false;
            for (int i = 0; i < survivors.Count; i++)
            {
                var s = survivors[i];
                if (s == null || !s.IsAlive) continue;
                var st = GetOrCreate(s.Id);
                if (st.MaestroRadAnxietySuppressUntilDay > currentDay) return true;
            }
            return false;
        }

        /// <summary>Broadcast a live musical performance from the radio tower node.</summary>
        public void RecordMasterpieceBroadcast(
            Survivor musician,
            string nodeId,
            bool livePerformance,
            int currentDay = 0)
        {
            if (musician == null || !musician.IsAlive || !livePerformance) return;
            if (!string.Equals(nodeId, RadioTowerNodeId, StringComparison.Ordinal)
                && (nodeId == null
                    || nodeId.IndexOf("radio_tower", StringComparison.OrdinalIgnoreCase) < 0))
                return;
            var state = GetOrCreate(musician.Id);
            SyncFromSurvivor(musician, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheMasterpiece, StringComparison.Ordinal))
                return;

            state.Progress = 1f;
            musician.QuestProgress = 1f;
            OnQuestProgress?.Invoke(musician, "masterpiece", 1);
            CompleteQuestline(musician, currentDay);
        }

        // ── #294 Smuggler — The Stash / Blockade Runner ──────────────────

        /// <summary>Shady: suspicion events fire more often.</summary>
        public float GetShadySuspicionMultiplier(Survivor sv) =>
            HasShady(sv) ? ShadySuspicionMult : 1f;

        /// <summary>AI quirk: bring back contraband not on the node loot table.</summary>
        public bool GeneratesContrabandLoot(Survivor sv) =>
            string.Equals(sv?.ArchetypeId, SmugglerId, StringComparison.Ordinal)
            || HasShady(sv);

        public float GetContrabandLootChance(Survivor sv) =>
            GeneratesContrabandLoot(sv) ? SmugglerContrabandChance : 0f;

        /// <summary>Blockade Runner: bypass faction blockades; dead drops always succeed.</summary>
        public bool CanBypassFactionBlockades(Survivor sv) => HasBlockadeRunner(sv);

        public bool GuaranteesDeadDropSuccess(Survivor sv) => HasBlockadeRunner(sv);

        /// <summary>
        /// Retrieve pre-war stash from a guarded faction vault without killing guards.
        /// </summary>
        public void RecordStashRetrieved(
            Survivor smuggler,
            bool guardedVault,
            bool retrieved,
            bool killedGuards,
            int currentDay = 0)
        {
            if (smuggler == null || !smuggler.IsAlive) return;
            if (!guardedVault || !retrieved || killedGuards) return;
            var state = GetOrCreate(smuggler.Id);
            SyncFromSurvivor(smuggler, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheStash, StringComparison.Ordinal))
                return;

            state.Progress = 1f;
            smuggler.QuestProgress = 1f;
            OnQuestProgress?.Invoke(smuggler, "stash_retrieved", 1);
            CompleteQuestline(smuggler, currentDay);
        }

        // ── #295 Hitman — The Last Contract / Executioner ────────────────

        /// <summary>Professional: no combat morale loss; refuses medical/farming.</summary>
        public bool IgnoresCombatMoraleLoss(Survivor sv) => HasProfessional(sv);

        public bool RefusesMedicalAndFarming(Survivor sv) =>
            HasProfessional(sv)
            || string.Equals(sv?.ArchetypeId, HitmanId, StringComparison.Ordinal);

        /// <summary>AI quirk: sleeping with loaded weapon — abrupt wake may discharge.</summary>
        public float GetAccidentalDischargeChance(Survivor sv) =>
            string.Equals(sv?.ArchetypeId, HitmanId, StringComparison.Ordinal)
            || HasProfessional(sv)
                ? HitmanAccidentalDischargeChance
                : 0f;

        public bool RollAccidentalDischargeOnWake(Survivor hitman, System.Random rng = null)
        {
            float p = GetAccidentalDischargeChance(hitman);
            if (p <= 0f) return false;
            rng ??= AtomicWar._Game.Utilities.SeededRandom.Stream("personalquestsystem_rebuilders");
            return rng.NextDouble() < p;
        }

        /// <summary>Executioner: 300% crit vs humans; instant raider execute for security.</summary>
        public float GetExecutionerHumanCritMultiplier(Survivor sv) =>
            HasExecutioner(sv) ? ExecutionerHumanCritMult : 1f;

        public bool CanExecuteCapturedRaiders(Survivor sv) => HasExecutioner(sv);

        /// <summary>Assassinate a marked faction leader.</summary>
        public void RecordLastContractAssassination(
            Survivor hitman,
            bool wasFactionLeader,
            bool assassinated,
            int currentDay = 0)
        {
            if (hitman == null || !hitman.IsAlive) return;
            if (!wasFactionLeader || !assassinated) return;
            var state = GetOrCreate(hitman.Id);
            SyncFromSurvivor(hitman, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheLastContract, StringComparison.Ordinal))
                return;

            state.Progress = 1f;
            hitman.QuestProgress = 1f;
            OnQuestProgress?.Invoke(hitman, "last_contract", 1);
            CompleteQuestline(hitman, currentDay);
        }

        // ── #296 Pickpocket — The Big Score / Shadow ─────────────────────

        /// <summary>Slight of Hand: steal from traders (risk trust hit if caught).</summary>
        public bool CanStealFromFactionTraders(Survivor sv) =>
            HasSlightOfHand(sv)
            || string.Equals(sv?.ArchetypeId, PickpocketId, StringComparison.Ordinal);

        public float GetPickpocketCatchChance(Survivor sv) =>
            CanStealFromFactionTraders(sv) ? PickpocketCatchChance : 0f;

        /// <summary>AI quirk: shuffle items between bunker containers.</summary>
        public bool ShufflesStorageContainers(Survivor sv) =>
            string.Equals(sv?.ArchetypeId, PickpocketId, StringComparison.Ordinal)
            || HasSlightOfHand(sv);

        /// <summary>Shadow: 100% stealth; steal equipped weapons in hatch breach.</summary>
        public float GetShadowStealth(Survivor sv) => HasShadow(sv) ? 1f : 0f;

        public bool CanStealRaiderEquippedWeapons(Survivor sv) => HasShadow(sv);

        /// <summary>Steal a unique keycard from a sleeping faction boss without waking them.</summary>
        public void RecordBigScoreKeycard(
            Survivor pickpocket,
            bool sleepingBoss,
            bool stoleKeycard,
            bool wokeBoss,
            int currentDay = 0)
        {
            if (pickpocket == null || !pickpocket.IsAlive) return;
            if (!sleepingBoss || !stoleKeycard || wokeBoss) return;
            var state = GetOrCreate(pickpocket.Id);
            SyncFromSurvivor(pickpocket, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheBigScore, StringComparison.Ordinal))
                return;

            state.Progress = 1f;
            pickpocket.QuestProgress = 1f;
            OnQuestProgress?.Invoke(pickpocket, "big_score", 1);
            CompleteQuestline(pickpocket, currentDay);
        }

        // ── #297 Forger — The Perfect Fake / Master of Disguise ──────────

        /// <summary>Counterfeiter: craft fake pre-war money and jewelry from scrap.</summary>
        public bool CanCraftFakeCurrencyAndJewelry(Survivor sv) =>
            HasCounterfeiter(sv)
            || string.Equals(sv?.ArchetypeId, ForgerId, StringComparison.Ordinal);

        /// <summary>AI quirk: high chance of fake journal lore entries.</summary>
        public float GetFakeJournalLoreChance(Survivor sv) =>
            string.Equals(sv?.ArchetypeId, ForgerId, StringComparison.Ordinal)
            || HasCounterfeiter(sv)
                ? ForgerFakeJournalChance
                : 0f;

        /// <summary>Master of Disguise: equip faction uniforms for Trust 100 treatment.</summary>
        public bool CanWearFactionDisguise(Survivor sv) => HasMasterOfDisguise(sv);

        public float GetDisguiseFactionTrust(Survivor sv, bool wearingMatchingUniform) =>
            HasMasterOfDisguise(sv) && wearingMatchingUniform ? 100f : float.NaN;

        /// <summary>Forge a military pass and walk a level-5 checkpoint unharmed.</summary>
        public void RecordPerfectFakeCheckpoint(
            Survivor forger,
            bool forgedMilitaryPass,
            int checkpointLevel,
            bool passedUnharmed,
            int currentDay = 0)
        {
            if (forger == null || !forger.IsAlive) return;
            if (!forgedMilitaryPass || !passedUnharmed) return;
            if (checkpointLevel < PerfectFakeCheckpointLevel) return;
            var state = GetOrCreate(forger.Id);
            SyncFromSurvivor(forger, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.ThePerfectFake, StringComparison.Ordinal))
                return;

            state.Progress = 1f;
            forger.QuestProgress = 1f;
            OnQuestProgress?.Invoke(forger, "perfect_fake", 1);
            CompleteQuestline(forger, currentDay);
        }

        // ── #298 Getaway Driver — The Escape / Mechanic Prodigy ──────────

        /// <summary>Antsy: morale drops after more than 3 days in the bunker.</summary>
        public void TickAntsyBunkerDay(Survivor driver, bool spentDayInside, int currentDay = 0)
        {
            if (driver == null || !driver.IsAlive) return;
            if (!HasAntsy(driver)
                && !string.Equals(driver.ArchetypeId, GetawayDriverId, StringComparison.Ordinal))
                return;
            var state = GetOrCreate(driver.Id);
            if (spentDayInside)
            {
                state.GetawayInsideDays++;
                if (state.GetawayInsideDays > GetawayAntsyDays)
                {
                    driver.Needs.Morale = Mathf.Max(
                        0f, driver.Needs.Morale - GetawayAntsyMoraleHitPerDay);
                }
            }
            else
            {
                state.GetawayInsideDays = 0;
            }
        }

        /// <summary>AI quirk: on bicycle/vehicle expedition, ignore fatigue penalties.</summary>
        public bool IgnoresVehicleExpeditionFatigue(Survivor sv) =>
            string.Equals(sv?.ArchetypeId, GetawayDriverId, StringComparison.Ordinal)
            || HasAntsy(sv)
            || HasMechanicProdigy(sv);

        /// <summary>Mechanic Prodigy: −50% fuel, never break down, safe escape.</summary>
        public float GetMechanicProdigyFuelMultiplier(Survivor sv) =>
            HasMechanicProdigy(sv) ? MechanicProdigyFuelMult : 1f;

        public bool VehiclesNeverBreakDown(Survivor sv) => HasMechanicProdigy(sv);

        public bool GuaranteesSafeMapEscape(Survivor sv) => HasMechanicProdigy(sv);

        /// <summary>
        /// Repair a ruined wasteland vehicle and outrun a radioactive storm on the map.
        /// </summary>
        public void RecordEscapeVehicleStorm(
            Survivor driver,
            bool repairedVehicle,
            bool outranStorm,
            int currentDay = 0)
        {
            if (driver == null || !driver.IsAlive) return;
            if (!repairedVehicle || !outranStorm) return;
            var state = GetOrCreate(driver.Id);
            SyncFromSurvivor(driver, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheEscape, StringComparison.Ordinal))
                return;

            state.Progress = 1f;
            driver.QuestProgress = 1f;
            OnQuestProgress?.Invoke(driver, "escape_storm", 1);
            CompleteQuestline(driver, currentDay);
        }

        // ── Host action-id helpers (#284–#298) ───────────────────────────

        public bool IsCleaningAction(string actionId)
        {
            if (string.IsNullOrEmpty(actionId)) return false;
            return actionId.IndexOf("clean", StringComparison.OrdinalIgnoreCase) >= 0
                   || actionId.IndexOf("waste", StringComparison.OrdinalIgnoreCase) >= 0
                   || actionId.IndexOf("mold", StringComparison.OrdinalIgnoreCase) >= 0
                   || actionId.IndexOf("sanit", StringComparison.OrdinalIgnoreCase) >= 0
                   || actionId.IndexOf("sweep", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public bool IsFarmingAction(string actionId)
        {
            if (string.IsNullOrEmpty(actionId)) return false;
            return actionId.IndexOf("farm", StringComparison.OrdinalIgnoreCase) >= 0
                   || actionId.IndexOf("plant", StringComparison.OrdinalIgnoreCase) >= 0
                   || actionId.IndexOf("harvest", StringComparison.OrdinalIgnoreCase) >= 0
                   || actionId.IndexOf("hydropon", StringComparison.OrdinalIgnoreCase) >= 0
                   || actionId.IndexOf("tend_crop", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public bool IsMedicalTriageAction(string actionId)
        {
            if (string.IsNullOrEmpty(actionId)) return false;
            return actionId.IndexOf("triage", StringComparison.OrdinalIgnoreCase) >= 0
                   || actionId.IndexOf("treat", StringComparison.OrdinalIgnoreCase) >= 0
                   || actionId.IndexOf("heal", StringComparison.OrdinalIgnoreCase) >= 0
                   || actionId.IndexOf("surgery", StringComparison.OrdinalIgnoreCase) >= 0
                   || actionId.IndexOf("bandage", StringComparison.OrdinalIgnoreCase) >= 0
                   || actionId.IndexOf("medical", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public bool IsPlayInstrumentAction(string actionId)
        {
            if (string.IsNullOrEmpty(actionId)) return false;
            return actionId.IndexOf("play_instrument", StringComparison.OrdinalIgnoreCase) >= 0
                   || actionId.IndexOf("instrument", StringComparison.OrdinalIgnoreCase) >= 0
                   || actionId.IndexOf("music", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public bool IsSalvageWoodAction(string actionId)
        {
            if (string.IsNullOrEmpty(actionId)) return false;
            return actionId.IndexOf("salvage", StringComparison.OrdinalIgnoreCase) >= 0
                   || actionId.IndexOf("dismantle", StringComparison.OrdinalIgnoreCase) >= 0
                   || actionId.IndexOf("scrap_wood", StringComparison.OrdinalIgnoreCase) >= 0
                   || actionId.IndexOf("wood_scrap", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        /// <summary>
        /// Atmosphere O2/CO2 health penalty multiplier (0 when Deep Delver immune).
        /// </summary>
        public float GetAtmosphereGasPenaltyMultiplier(Survivor sv) =>
            IsImmuneToAtmosphereGasPenalties(sv) ? 0f : 1f;

        /// <summary>
        /// Apply hypoxia/CO2 health damage unless Deep Delver. Returns damage applied.
        /// </summary>
        public float ApplyAtmosphereGasPenalty(
            Survivor sv, float rawHealthDamage, bool isO2OrCo2Penalty)
        {
            if (sv == null || !sv.IsAlive || !isO2OrCo2Penalty || rawHealthDamage <= 0f)
                return 0f;
            float mult = GetAtmosphereGasPenaltyMultiplier(sv);
            float dmg = rawHealthDamage * mult;
            if (dmg <= 0f) return 0f;
            SurvivorNeedWrite.AdjustHealth(sv, -dmg);
            return dmg;
        }

        /// <summary>
        /// Host CO-leak shelter event resolution for The Canary.
        /// Zero health damage + saved another → quest complete.
        /// </summary>
        public void ResolveCoLeakShelterEvent(
            Survivor miner,
            float healthDamageTaken,
            bool savedAnother,
            int currentDay = 0)
        {
            RecordCanaryCoLeakSurvived(
                miner,
                coLeakEvent: true,
                healthDamageTaken: healthDamageTaken,
                savedAnother: savedAnother,
                currentDay: currentDay);
        }

        /// <summary>Host corpse dispose → The Mess progress.</summary>
        public void NotifyHumanCorpseCleaned(Survivor cleaner, int currentDay = 0) =>
            RecordCorpseCleaned(cleaner, humanCorpse: true, currentDay);

        /// <summary>
        /// Host: when hatch module reaches absolute max level, credit Iron Gate.
        /// </summary>
        public void NotifyHatchUpgradeInstalled(
            Survivor welder, int hatchLevel, int maxLevel, int currentDay = 0) =>
            RecordIronGateHatchMaxed(welder, hatchLevel, maxLevel, currentDay);

        /// <summary>
        /// Host: inventory fill ratios at 100% food/water/fuel → In the Black.
        /// </summary>
        public void NotifyResourceCapacities(
            Survivor accountant,
            float foodFill01,
            float waterFill01,
            float fuelFill01,
            int currentDay = 0) =>
            RecordInTheBlackCapacities(accountant, foodFill01, waterFill01, fuelFill01, currentDay);

        /// <summary>
        /// Fragile Ego: scale morale loss after a failed craft/repair.
        /// </summary>
        public void ApplyFragileEgoCraftFailure(Survivor sv, float baseMoraleLoss)
        {
            if (sv == null || !sv.IsAlive || baseMoraleLoss <= 0f) return;
            float mult = GetFragileEgoFailureMoraleMultiplier(sv);
            float hit = baseMoraleLoss * mult;
            // QUEST-001: route through ApplyMoraleDelta so Traumatized 50% cap holds.
            ApplyMoraleDelta(sv, -hit);
        }

        /// <summary>
        /// Professional: combat morale loss is ignored (returns 0 when active).
        /// </summary>
        public float GetCombatMoraleLoss(Survivor sv, float baseLoss) =>
            IgnoresCombatMoraleLoss(sv) ? 0f : baseLoss;
    }
}
