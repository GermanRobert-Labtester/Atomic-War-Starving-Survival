using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Prompts #267–#276 — Internal chemistry + adapted-to-the-end archetypes.
    /// Quest recorders, base-trait quirks, and latent-trait host APIs.
    /// </summary>
    public partial class PersonalQuestSystem
    {
        // ── #267 Relapsing Addict — Cold Turkey / Clean & Sober ──────────

        /// <summary>
        /// AI quirk: morale below threshold → bypass locks and auto-consume medical chems.
        /// Clean &amp; Sober disables this.
        /// </summary>
        public bool ShouldForceConsumeMedicalChems(Survivor sv) =>
            sv != null && sv.IsAlive
            && string.Equals(sv.ArchetypeId, RelapsingAddictId, StringComparison.Ordinal)
            && !HasCleanAndSober(sv)
            && sv.Needs.Morale < ForcedChemMoraleThreshold;

        public bool IsImmuneToChemicalAddiction(Survivor sv) =>
            HasCleanAndSober(sv) || HasChemResistant(sv);

        public float GetCleanAndSoberStaminaMultiplier(Survivor sv) =>
            HasCleanAndSober(sv) ? CleanAndSoberStaminaMult : 1f;

        /// <summary>
        /// One clean day with no chem use. Relapse resets the streak.
        /// Completes after ColdTurkeyDaysRequired (21).
        /// </summary>
        public void RecordColdTurkeyCleanDay(Survivor addict, bool usedAnyChem, int currentDay = 0)
        {
            if (addict == null || !addict.IsAlive) return;
            var state = GetOrCreate(addict.Id);
            SyncFromSurvivor(addict, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.ColdTurkey, StringComparison.Ordinal))
                return;

            if (usedAnyChem)
            {
                state.ColdTurkeyCleanDays = 0;
                state.Progress = 0f;
                addict.QuestProgress = 0f;
                OnQuestProgress?.Invoke(addict, "cold_turkey_relapse", 0);
                return;
            }

            state.ColdTurkeyCleanDays++;
            state.Progress = state.ColdTurkeyCleanDays;
            addict.QuestProgress = state.ColdTurkeyCleanDays;
            OnQuestProgress?.Invoke(addict, "cold_turkey_days", state.ColdTurkeyCleanDays);
            if (state.ColdTurkeyCleanDays >= ColdTurkeyDaysRequired)
                CompleteQuestline(addict, currentDay);
        }

        // ── #268 Insomniac — Long Night / The Watcher ────────────────────

        /// <summary>Restless: max fatigue permanently capped at 80%.</summary>
        public float GetMaxFatigueCap(Survivor sv)
        {
            if (HasRestless(sv)) return RestlessMaxFatigueCap;
            return 100f;
        }

        /// <summary>AI quirk: paces at night generating noise (cleared by The Watcher).</summary>
        public bool GeneratesNightPaceNoise(Survivor sv) =>
            HasRestless(sv)
            || (string.Equals(sv?.ArchetypeId, InsomniacId, StringComparison.Ordinal)
                && !HasTheWatcher(sv));

        public float GetNightPaceNoisePerHour(Survivor sv) =>
            GeneratesNightPaceNoise(sv) ? InsomniacNightNoisePerHour : 0f;

        /// <summary>The Watcher: never suffers fatigue penalties to combat.</summary>
        public bool IgnoresFatigueCombatPenalties(Survivor sv) => HasTheWatcher(sv);

        /// <summary>One night alone on hatch guard without sleeping.</summary>
        public void RecordLongNightGuardNight(
            Survivor insomniac,
            bool guardedAlone,
            bool slept,
            int currentDay = 0)
        {
            if (insomniac == null || !insomniac.IsAlive) return;
            var state = GetOrCreate(insomniac.Id);
            SyncFromSurvivor(insomniac, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheLongNight, StringComparison.Ordinal))
                return;

            if (!guardedAlone || slept)
            {
                state.LongNightGuardNights = 0;
                state.Progress = 0f;
                insomniac.QuestProgress = 0f;
                OnQuestProgress?.Invoke(insomniac, "long_night_reset", 0);
                return;
            }

            state.LongNightGuardNights++;
            state.Progress = state.LongNightGuardNights;
            insomniac.QuestProgress = state.LongNightGuardNights;
            OnQuestProgress?.Invoke(insomniac, "long_night_nights", state.LongNightGuardNights);
            if (state.LongNightGuardNights >= LongNightGuardNightsRequired)
                CompleteQuestline(insomniac, currentDay);
        }

        // ── #269 Hypochondriac — Real Illness / Hyper-Aware ──────────────

        /// <summary>AI quirk: generate fake affliction UI alerts (stops when Hyper-Aware).</summary>
        public bool ShouldGenerateFakeAfflictionAlert(Survivor sv) =>
            HasParanoidHealth(sv)
            || (string.Equals(sv?.ArchetypeId, HypochondriacId, StringComparison.Ordinal)
                && !HasHyperAware(sv));

        /// <summary>
        /// Apply placebo or withhold. Without placebo: real morale + fatigue hits.
        /// </summary>
        public void ApplyHypochondriacPlaceboTick(Survivor sv, bool givenPlacebo)
        {
            if (sv == null || !sv.IsAlive) return;
            if (!ShouldGenerateFakeAfflictionAlert(sv)) return;
            if (givenPlacebo)
            {
                sv.Needs.Morale = Mathf.Min(100f, sv.Needs.Morale + PlaceboMoraleRestore);
                return;
            }
            sv.Needs.Morale = Mathf.Max(0f, sv.Needs.Morale - FakeIllnessMoraleHit);
            sv.Needs.Fatigue = Mathf.Min(GetMaxFatigueCap(sv), sv.Needs.Fatigue + FakeIllnessFatigueHit);
        }

        /// <summary>Hyper-Aware: immune to contamination spread.</summary>
        public bool IsImmuneToContaminationSpread(Survivor sv) => HasHyperAware(sv);

        /// <summary>Contract and survive actual Sepsis → unlock Hyper-Aware.</summary>
        public void RecordSepsisSurvived(
            Survivor hypo,
            bool contractedSepsis,
            bool survived,
            int currentDay = 0)
        {
            if (hypo == null || !hypo.IsAlive) return;
            if (!contractedSepsis || !survived) return;
            var state = GetOrCreate(hypo.Id);
            SyncFromSurvivor(hypo, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheRealIllness, StringComparison.Ordinal))
                return;

            state.Progress = 1f;
            hypo.QuestProgress = 1f;
            OnQuestProgress?.Invoke(hypo, "sepsis_survived", 1);
            CompleteQuestline(hypo, currentDay);
        }

        // ── #270 Pyromaniac — Trial by Fire / Fire-Breather ──────────────

        /// <summary>Fascination: morale gain near running heaters/generators.</summary>
        public float GetFascinationHeaterMoralePerHour(Survivor sv, bool nearRunningHeatOrPower) =>
            HasFascination(sv) && nearRunningHeatOrPower ? FascinationHeaterMoralePerHour : 0f;

        public void ApplyFascinationHeaterMorale(Survivor sv, bool nearRunningHeatOrPower, float gameHours = 1f)
        {
            float d = GetFascinationHeaterMoralePerHour(sv, nearRunningHeatOrPower) * gameHours;
            if (Mathf.Abs(d) < 0.001f || sv == null || !sv.IsAlive) return;
            sv.Needs.Morale = Mathf.Clamp(sv.Needs.Morale + d, 0f, 100f);
        }

        /// <summary>
        /// AI quirk: 5% daily chance to deliberately start a fire when morale &lt; 30.
        /// Fire-Breather stops this.
        /// </summary>
        public bool ShouldDeliberatelyStartFire(Survivor sv, System.Random rng = null)
        {
            if (sv == null || !sv.IsAlive) return false;
            if (HasFireBreather(sv)) return false;
            if (!string.Equals(sv.ArchetypeId, PyromaniacId, StringComparison.Ordinal)
                && !HasFascination(sv))
                return false;
            if (sv.Needs.Morale >= PyromaniacFireMoraleThreshold) return false;
            rng ??= AtomicWar._Game.Utilities.SeededRandom.CreateFixed("personalquestsystem_chemistry");
            return rng.NextDouble() < PyromaniacDailyFireChance;
        }

        public bool CanCraftIncendiaryWeapons(Survivor sv) => HasFireBreather(sv);

        /// <summary>Extinguish one bunker fire toward Trial by Fire (5).</summary>
        public void RecordBunkerFireExtinguished(Survivor pyro, int currentDay = 0)
        {
            if (pyro == null || !pyro.IsAlive) return;
            var state = GetOrCreate(pyro.Id);
            SyncFromSurvivor(pyro, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TrialByFire, StringComparison.Ordinal))
                return;

            state.FiresExtinguished++;
            state.Progress = state.FiresExtinguished;
            pyro.QuestProgress = state.FiresExtinguished;
            OnQuestProgress?.Invoke(pyro, "fires_extinguished", state.FiresExtinguished);
            if (state.FiresExtinguished >= TrialByFireExtinguishRequired)
                CompleteQuestline(pyro, currentDay);
        }

        // ── #271 Blind Preacher — Voice in the Dark / Sonar ──────────────

        /// <summary>Blind: cannot go on expeditions or fire guns.</summary>
        public bool CanGoOnExpedition(Survivor sv)
        {
            if (sv == null) return false;
            if (HasBlind(sv) || string.Equals(sv.ArchetypeId, BlindPreacherId, StringComparison.Ordinal))
                return false;
            return true;
        }

        public bool CanFireGuns(Survivor sv) =>
            !HasBlind(sv)
            && !string.Equals(sv?.ArchetypeId, BlindPreacherId, StringComparison.Ordinal)
            && !(sv != null && sv.CannotFight);

        /// <summary>AI quirk: navigate bunker by sound (AudioMixer subscription).</summary>
        public bool NavigatesBySoundOnly(Survivor sv) =>
            HasBlind(sv)
            || string.Equals(sv?.ArchetypeId, BlindPreacherId, StringComparison.Ordinal);

        /// <summary>Sonar: 12h advanced warning of raids / fallout storms.</summary>
        public bool ProvidesRaidStormEarlyWarning(Survivor sv) => HasSonar(sv);

        public float GetSonarWarningHours(Survivor sv) =>
            HasSonar(sv) ? SonarRaidWarningHours : 0f;

        public bool AnySonarEarlyWarning(IReadOnlyList<Survivor> survivors)
        {
            if (survivors == null) return false;
            for (int i = 0; i < survivors.Count; i++)
            {
                if (ProvidesRaidStormEarlyWarning(survivors[i])) return true;
            }
            return false;
        }

        /// <summary>
        /// #271: Blind Preacher (or Sonar latent) may convert Despair via dialogue.
        /// </summary>
        public bool CanConvertDespairViaDialogue(Survivor speaker) =>
            speaker != null && speaker.IsAlive
            && (HasBlind(speaker)
                || HasSonar(speaker)
                || string.Equals(speaker.ArchetypeId, BlindPreacherId, StringComparison.Ordinal));

        /// <summary>Convert one survivor Despair → Hope via dialogue.</summary>
        public void RecordDespairToHopeConversion(
            Survivor preacher,
            Survivor target,
            bool viaDialogue,
            int currentDay = 0)
        {
            if (preacher == null || !preacher.IsAlive) return;
            if (!viaDialogue || target == null) return;
            var state = GetOrCreate(preacher.Id);
            SyncFromSurvivor(preacher, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.AVoiceInTheDark, StringComparison.Ordinal))
                return;

            state.DespairToHopeConverts++;
            state.Progress = state.DespairToHopeConverts;
            preacher.QuestProgress = state.DespairToHopeConverts;
            // Target emerges from despair.
            if (string.Equals(target.currentMentalBreakId, DespairBreakId, StringComparison.OrdinalIgnoreCase))
                target.currentMentalBreakId = null;
            target.Needs.Morale = Mathf.Max(target.Needs.Morale, 50f);
            OnQuestProgress?.Invoke(preacher, "despair_converts", state.DespairToHopeConverts);
            if (state.DespairToHopeConverts >= VoiceInDarkConvertsRequired)
                CompleteQuestline(preacher, currentDay);
        }

        // ── #272 Prepper — Bunker Breached / Improvised Engineering ──────

        /// <summary>AI quirk: only eats own pre-war MREs until stash runs out.</summary>
        public bool RefusesSharedRations(Survivor sv) =>
            HasParanoid(sv)
            || string.Equals(sv?.ArchetypeId, PrepperId, StringComparison.Ordinal);

        public bool WillOnlyEatOwnMres(Survivor sv)
        {
            if (!RefusesSharedRations(sv)) return false;
            var state = GetOrCreate(sv.Id);
            // Default stash count if never initialized.
            if (state.PrepperMreRemaining <= 0f && !state.TraitUnlocked
                && string.Equals(sv.ArchetypeId, PrepperId, StringComparison.Ordinal))
            {
                // Count mre_prewar in hidden stash as remaining.
                int n = 0;
                if (sv.HiddenItemIds != null)
                {
                    for (int i = 0; i < sv.HiddenItemIds.Count; i++)
                        if (string.Equals(sv.HiddenItemIds[i], "mre_prewar", StringComparison.Ordinal))
                            n++;
                }
                state.PrepperMreRemaining = n > 0 ? n : 2f;
            }
            return state.PrepperMreRemaining > 0f;
        }

        public bool TryConsumePrepperMre(Survivor sv)
        {
            if (!WillOnlyEatOwnMres(sv)) return false;
            var state = GetOrCreate(sv.Id);
            if (state.PrepperMreRemaining <= 0f) return false;
            state.PrepperMreRemaining -= 1f;
            if (sv.HiddenItemIds != null)
                sv.HiddenItemIds.Remove("mre_prewar");
            sv.Needs.Hunger = Mathf.Max(0f, sv.Needs.Hunger - 30f);
            return true;
        }

        /// <summary>Improvised Engineering: build modules from 100% junk.</summary>
        public bool CanBuildModulesFromJunkOnly(Survivor sv) => HasImprovisedEngineering(sv);

        /// <summary>Survive a raid where the hatch is completely destroyed.</summary>
        public void RecordHatchDestroyedRaidSurvived(
            Survivor prepper,
            bool hatchDestroyed,
            bool survived,
            int currentDay = 0)
        {
            if (prepper == null || !prepper.IsAlive) return;
            if (!hatchDestroyed || !survived) return;
            var state = GetOrCreate(prepper.Id);
            SyncFromSurvivor(prepper, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheBunkerBreached, StringComparison.Ordinal))
                return;

            state.Progress = 1f;
            prepper.QuestProgress = 1f;
            OnQuestProgress?.Invoke(prepper, "hatch_destroyed_survived", 1);
            CompleteQuestline(prepper, currentDay);
        }

        // ── #273 Mutated Outcast — Embracing the Glow / Radiotrophic ────

        /// <summary>AI quirk: others lose morale eating in the same room.</summary>
        public float GetOutcastRoomMealMoraleHit(Survivor outcast, Survivor diner)
        {
            if (outcast == null || diner == null || !outcast.IsAlive || !diner.IsAlive) return 0f;
            if (ReferenceEquals(outcast, diner)) return 0f;
            if (!string.Equals(outcast.ArchetypeId, OutcastId, StringComparison.Ordinal)
                && !HasRadiotrophic(outcast))
                return 0f;
            // Radiotrophic still looks wrong — keep social hit unless we want otherwise.
            if (string.IsNullOrEmpty(outcast.CurrentRoomId)
                || !string.Equals(outcast.CurrentRoomId, diner.CurrentRoomId, StringComparison.Ordinal))
                return 0f;
            return OutcastRoomMealMoraleHit;
        }

        public void ApplyOutcastRoomMealMorale(Survivor outcast, Survivor diner)
        {
            float d = GetOutcastRoomMealMoraleHit(outcast, diner);
            if (d <= 0f) return;
            diner.Needs.Morale = Mathf.Max(0f, diner.Needs.Morale - d);
        }

        /// <summary>Radiotrophic: radiation heals instead of damages in high-rad zones.</summary>
        public bool IsRadiotrophic(Survivor sv) => HasRadiotrophic(sv);

        public float GetRadiotrophicHealPerHour(Survivor sv, float zoneRadPerHour) =>
            HasRadiotrophic(sv) && zoneRadPerHour >= 50f ? RadiotrophicHealPerHour : 0f;

        public void ApplyRadiotrophicTick(Survivor sv, float zoneRadPerHour, float gameHours = 1f)
        {
            if (!HasRadiotrophic(sv) || sv == null || !sv.IsAlive) return;
            if (zoneRadPerHour < 50f) return;
            float heal = RadiotrophicHealPerHour * gameHours;
            SurvivorNeedWrite.SetHealth(
                sv,
                Mathf.Min(sv.MaxHealthCap > 0f ? sv.MaxHealthCap : 100f, sv.Needs.Health + heal));
            sv.Needs.Fatigue = Mathf.Max(0f, sv.Needs.Fatigue - heal);
        }

        /// <summary>Reach 1000 mSv lifetime without dying.</summary>
        public void RecordLifetimeRadsMilestone(
            Survivor outcast,
            float lifetimeMsv,
            bool isAlive,
            int currentDay = 0)
        {
            if (outcast == null || !isAlive) return;
            if (lifetimeMsv < EmbracingGlowLifetimeRads) return;
            var state = GetOrCreate(outcast.Id);
            SyncFromSurvivor(outcast, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.EmbracingTheGlow, StringComparison.Ordinal))
                return;

            state.Progress = 1f;
            outcast.QuestProgress = 1f;
            OnQuestProgress?.Invoke(outcast, "embraced_glow", 1);
            CompleteQuestline(outcast, currentDay);
        }

        // ── #274 Feral Orphan — The Pack / Apex Scavenger ────────────────

        /// <summary>Animalistic: lowers adult affinity; eats only raw meat.</summary>
        public float GetAnimalisticAffinityDrain(Survivor orphan) =>
            HasAnimalistic(orphan) ? AnimalisticAffinityDrain : 0f;

        public bool EatsOnlyRawMeat(Survivor orphan) => HasAnimalistic(orphan);

        /// <summary>AI quirk: sleeps on floor even if beds available.</summary>
        public bool PrefersFloorSleep(Survivor orphan) =>
            HasAnimalistic(orphan)
            || string.Equals(orphan?.ArchetypeId, FeralOrphanId, StringComparison.Ordinal);

        /// <summary>AI quirk: bites if healed by strangers (not Vet / Fierce Mother).</summary>
        public bool BitesWhenHealedByStranger(Survivor orphan, Survivor healer)
        {
            if (!HasAnimalistic(orphan) && !string.Equals(orphan?.ArchetypeId, FeralOrphanId, StringComparison.Ordinal))
                return false;
            if (healer == null) return true;
            return !string.Equals(healer.ArchetypeId, VetId, StringComparison.Ordinal)
                   && !string.Equals(healer.ArchetypeId, FierceMotherId, StringComparison.Ordinal);
        }

        /// <summary>Apex Scavenger: can use tools; inherits Zoonotic Expert.</summary>
        public bool CanUseTools(Survivor orphan) =>
            HasApexScavenger(orphan) || !HasAnimalistic(orphan);

        public bool HasZoonoticExpertInherited(Survivor orphan) =>
            HasApexScavenger(orphan) || HasZoonoticExpert(orphan);

        /// <summary>One training day under Vet or Fierce Mother.</summary>
        public void RecordPackTrainingDay(
            Survivor orphan,
            Survivor trainer,
            bool trainedToday,
            int currentDay = 0)
        {
            if (orphan == null || !orphan.IsAlive) return;
            var state = GetOrCreate(orphan.Id);
            SyncFromSurvivor(orphan, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.ThePack, StringComparison.Ordinal))
                return;

            bool validTrainer = trainer != null
                && (string.Equals(trainer.ArchetypeId, VetId, StringComparison.Ordinal)
                    || string.Equals(trainer.ArchetypeId, FierceMotherId, StringComparison.Ordinal)
                    || HasZoonoticExpert(trainer)
                    || HasMatriarch(trainer));

            if (!trainedToday || !validTrainer)
            {
                // Streak not required — only cumulative valid days.
                return;
            }

            state.PackTrainingDays++;
            state.Progress = state.PackTrainingDays;
            orphan.QuestProgress = state.PackTrainingDays;
            OnQuestProgress?.Invoke(orphan, "pack_training_days", state.PackTrainingDays);
            if (state.PackTrainingDays >= PackTrainingDaysRequired)
                CompleteQuestline(orphan, currentDay);
        }

        // ── #275 Pacifist — Ultimate Test / Zen State ────────────────────

        public bool CannotEquipWeapons(Survivor sv) =>
            HasVowOfNonviolence(sv)
            || string.Equals(sv?.ArchetypeId, PacifistId, StringComparison.Ordinal);

        public bool AutoFleesAllEncounters(Survivor sv) => CannotEquipWeapons(sv);

        /// <summary>AI quirk: hunger strike if another executes/kills needlessly.</summary>
        public void NotifyNeedlessKill(Survivor monk, Survivor killer, bool wasNeedless)
        {
            if (monk == null || !monk.IsAlive || !wasNeedless) return;
            if (!CannotEquipWeapons(monk)) return;
            if (ReferenceEquals(monk, killer)) return;
            var state = GetOrCreate(monk.Id);
            state.HungerStrikeActive = true;
        }

        public bool IsOnHungerStrike(Survivor monk)
        {
            if (monk == null) return false;
            return GetOrCreate(monk.Id).HungerStrikeActive;
        }

        public void TickHungerStrike(Survivor monk)
        {
            if (!IsOnHungerStrike(monk) || monk == null || !monk.IsAlive) return;
            // Ends when morale improves above 50.
            if (monk.Needs.Morale >= 50f)
            {
                GetOrCreate(monk.Id).HungerStrikeActive = false;
                return;
            }
            // Refuse food: hunger rises.
            monk.Needs.Hunger = Mathf.Min(100f, monk.Needs.Hunger + 5f);
        }

        public bool RefusesToEat(Survivor monk) => IsOnHungerStrike(monk);

        public float GetZenNeedsDecayMultiplier(Survivor sv) =>
            HasZenState(sv) ? ZenNeedsDecayMult : 1f;

        /// <summary>Navigate Level 5 danger node with zero damage dealt.</summary>
        public void RecordPacifistDangerNode(
            Survivor monk,
            int dangerLevel,
            float damageDealt,
            bool completedNode,
            int currentDay = 0)
        {
            if (monk == null || !monk.IsAlive) return;
            if (!completedNode || dangerLevel < 5 || damageDealt > 0.001f) return;
            var state = GetOrCreate(monk.Id);
            SyncFromSurvivor(monk, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheUltimateTest, StringComparison.Ordinal))
                return;

            state.Progress = 1f;
            monk.QuestProgress = 1f;
            OnQuestProgress?.Invoke(monk, "pacifist_node", 1);
            CompleteQuestline(monk, currentDay);
        }

        // ── #276 Widow — Last Seed / Master Geneticist ───────────────────

        /// <summary>Grieving: randomly stops work (action efficiency drain).</summary>
        public float GetGrievingActionEfficiencyMultiplier(Survivor widow) =>
            HasGrieving(widow) ? GrievingActionEfficiencyMult : 1f;

        /// <summary>AI quirk: prioritizes hydroponics over sleep.</summary>
        public bool PrioritizesHydroponicsOverSleep(Survivor widow) =>
            HasGrieving(widow)
            || string.Equals(widow?.ArchetypeId, WidowId, StringComparison.Ordinal);

        public bool IsHydroponicsAction(string actionId)
        {
            if (string.IsNullOrEmpty(actionId)) return false;
            return actionId.IndexOf("hydro", StringComparison.OrdinalIgnoreCase) >= 0
                   || actionId.IndexOf("plant", StringComparison.OrdinalIgnoreCase) >= 0
                   || actionId.IndexOf("harvest", StringComparison.OrdinalIgnoreCase) >= 0
                   || actionId.IndexOf("water_crop", StringComparison.OrdinalIgnoreCase) >= 0
                   || actionId.IndexOf("greenhouse", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        /// <summary>Master Geneticist: cross-breed MedicinalFood that heals Phase 1.</summary>
        public bool CanCrossBreedMedicinalFood(Survivor widow) => HasMasterGeneticist(widow);

        public bool MedicinalFoodHealsPhase1(Survivor eater, bool ateMedicinalFood) =>
            ateMedicinalFood; // host checks crop flag; latent enables crafting

        /// <summary>Successfully grow PreWarRose in the greenhouse.</summary>
        public void RecordPreWarRoseGrown(Survivor widow, string cropId, bool harvested, int currentDay = 0)
        {
            if (widow == null || !widow.IsAlive) return;
            if (!harvested) return;
            if (!string.Equals(cropId, PreWarRoseItemId, StringComparison.Ordinal)) return;
            var state = GetOrCreate(widow.Id);
            SyncFromSurvivor(widow, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheLastSeed, StringComparison.Ordinal))
                return;

            // Grief lifts with the bloom.
            if (widow.Traits != null)
                widow.Traits.Remove(GrievingId);

            state.Progress = 1f;
            widow.QuestProgress = 1f;
            OnQuestProgress?.Invoke(widow, "pre_war_rose", 1);
            CompleteQuestline(widow, currentDay);
        }
    }
}
