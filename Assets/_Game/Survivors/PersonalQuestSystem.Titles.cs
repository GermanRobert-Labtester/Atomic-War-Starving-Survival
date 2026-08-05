using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Prompts #277–#283 — Titles that no longer exist (Ex-Con through Exec).
    /// Quest recorders, base-trait quirks, and latent-trait host APIs.
    /// </summary>
    public partial class PersonalQuestSystem
    {
        // ── #277 Ex-Con — Redemption Arc / The Enforcer ──────────────────

        /// <summary>
        /// Distrusted: other survivors lock personal stashes when Ex-Con is in the room.
        /// </summary>
        public bool CausesPersonalStashLock(Survivor exCon, Survivor other)
        {
            if (!HasDistrusted(exCon) || other == null || !other.IsAlive) return false;
            if (ReferenceEquals(exCon, other)) return false;
            if (string.IsNullOrEmpty(exCon.CurrentRoomId)) return false;
            return string.Equals(exCon.CurrentRoomId, other.CurrentRoomId, StringComparison.Ordinal);
        }

        /// <summary>AI quirk: refuses orders from Cop or General.</summary>
        public bool RefusesOrdersFrom(Survivor exCon, Survivor orderGiver)
        {
            if (exCon == null || orderGiver == null) return false;
            if (!string.Equals(exCon.ArchetypeId, ExConId, StringComparison.Ordinal)
                && !HasDistrusted(exCon))
                return false;
            return string.Equals(orderGiver.ArchetypeId, CopId, StringComparison.Ordinal)
                   || string.Equals(orderGiver.ArchetypeId, GeneralId, StringComparison.Ordinal);
        }

        /// <summary>Excels at physical labor (host multiplies action speed).</summary>
        public float GetExConPhysicalLaborMultiplier(Survivor sv) =>
            string.Equals(sv?.ArchetypeId, ExConId, StringComparison.Ordinal)
            || HasTheEnforcer(sv)
                ? 1.25f
                : 1f;

        /// <summary>Enforcer: Intimidate ends bunker conflict / mental break without violence.</summary>
        public bool CanIntimidateEndConflict(Survivor sv) => HasTheEnforcer(sv);

        public bool TryIntimidateEndMentalBreak(Survivor enforcer, Survivor target)
        {
            if (!CanIntimidateEndConflict(enforcer) || target == null || !target.IsAlive) return false;
            if (string.IsNullOrEmpty(target.currentMentalBreakId)) return false;
            target.currentMentalBreakId = null;
            target.Needs.Morale = Mathf.Max(target.Needs.Morale, 40f);
            return true;
        }

        /// <summary>Drag a dying wounded survivor back from an expedition.</summary>
        public void RecordDraggedWoundedHome(
            Survivor exCon,
            Survivor wounded,
            bool fromExpedition,
            bool woundedWasDying,
            bool madeItHome,
            int currentDay = 0)
        {
            if (exCon == null || !exCon.IsAlive) return;
            if (!fromExpedition || !woundedWasDying || !madeItHome || wounded == null) return;
            var state = GetOrCreate(exCon.Id);
            SyncFromSurvivor(exCon, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.RedemptionArc, StringComparison.Ordinal))
                return;

            state.Progress = 1f;
            exCon.QuestProgress = 1f;
            OnQuestProgress?.Invoke(exCon, "dragged_wounded", 1);
            CompleteQuestline(exCon, currentDay);
        }

        // ── #278 Sheriff — Last Ride / Legend of the Wastes ──────────────

        /// <summary>Moral Compass: bunker-wide morale buff.</summary>
        public float GetMoralCompassBunkerMorale(IReadOnlyList<Survivor> survivors)
        {
            if (survivors == null) return 0f;
            for (int i = 0; i < survivors.Count; i++)
            {
                var s = survivors[i];
                if (s == null || !s.IsAlive) continue;
                if (HasMoralCompass(s) || string.Equals(s.ArchetypeId, SheriffId, StringComparison.Ordinal))
                    return MoralCompassBunkerMorale;
            }
            return 0f;
        }

        /// <summary>Massive hit when player chooses Evil event options.</summary>
        public void ApplyMoralCompassEvilChoice(Survivor sheriff, bool evilChoice)
        {
            if (!evilChoice || sheriff == null || !sheriff.IsAlive) return;
            if (!HasMoralCompass(sheriff)
                && !string.Equals(sheriff.ArchetypeId, SheriffId, StringComparison.Ordinal))
                return;
            sheriff.Needs.Morale = Mathf.Max(0f, sheriff.Needs.Morale - MoralCompassEvilHit);
            sheriff.Needs.Health = Mathf.Max(1f, sheriff.Needs.Health - MoralCompassEvilHit * 0.5f);
        }

        /// <summary>Failing Heart: max stamina decays over days.</summary>
        public float GetFailingHeartStaminaMax(Survivor sheriff, int daysProgressed)
        {
            if (!HasFailingHeart(sheriff)
                && !string.Equals(sheriff?.ArchetypeId, SheriffId, StringComparison.Ordinal))
                return 100f;
            var state = GetOrCreate(sheriff.Id);
            if (state.SheriffStaminaMax <= 0f) state.SheriffStaminaMax = 100f;
            float decayed = 100f - daysProgressed * FailingHeartStaminaDecayPerDay;
            state.SheriffStaminaMax = Mathf.Max(20f, decayed);
            return state.SheriffStaminaMax;
        }

        public void TickFailingHeart(Survivor sheriff, int currentDay)
        {
            if (sheriff == null || !sheriff.IsAlive) return;
            if (!HasFailingHeart(sheriff)) return;
            GetFailingHeartStaminaMax(sheriff, currentDay);
        }

        /// <summary>AI quirk: auto-assign to Guard if no one else is.</summary>
        public bool ShouldAutoAssignGuard(Survivor sheriff, bool someoneElseGuarding) =>
            !someoneElseGuarding
            && (HasMoralCompass(sheriff)
                || string.Equals(sheriff?.ArchetypeId, SheriffId, StringComparison.Ordinal));

        /// <summary>Legend: raid frequency ×0.25 globally.</summary>
        public float GetLegendRaidFrequencyMultiplier(IReadOnlyList<Survivor> survivors)
        {
            if (survivors == null) return 1f;
            for (int i = 0; i < survivors.Count; i++)
            {
                if (HasLegendOfTheWastes(survivors[i]))
                    return LegendRaidFrequencyMult;
            }
            return 1f;
        }

        /// <summary>Execute a Raider Boss in a specific map encounter.</summary>
        public void RecordRaiderBossExecuted(
            Survivor sheriff,
            bool wasRaiderBoss,
            bool executed,
            int currentDay = 0)
        {
            if (sheriff == null || !sheriff.IsAlive) return;
            if (!wasRaiderBoss || !executed) return;
            var state = GetOrCreate(sheriff.Id);
            SyncFromSurvivor(sheriff, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheLastRide, StringComparison.Ordinal))
                return;

            state.Progress = 1f;
            sheriff.QuestProgress = 1f;
            OnQuestProgress?.Invoke(sheriff, "raider_boss_executed", 1);
            CompleteQuestline(sheriff, currentDay);
        }

        // ── #279 Former Politician — Real Leader / The Statesman ─────────

        /// <summary>Silver Tongue: charisma maxed; manual labor skills capped at 0.</summary>
        public float GetManualLaborSkillCap(Survivor sv) =>
            HasSilverTongue(sv)
            || string.Equals(sv?.ArchetypeId, FormerPoliticianId, StringComparison.Ordinal)
                ? 0f
                : 100f;

        public float GetSilverTongueCharisma(Survivor sv) =>
            HasSilverTongue(sv)
            || string.Equals(sv?.ArchetypeId, FormerPoliticianId, StringComparison.Ordinal)
                ? 100f
                : -1f; // host: -1 means no override

        /// <summary>AI quirk: tries to Delegate; dirty jobs cause massive morale loss.</summary>
        public bool TriesToDelegateTasks(Survivor sv) =>
            HasSilverTongue(sv)
            || string.Equals(sv?.ArchetypeId, FormerPoliticianId, StringComparison.Ordinal);

        public bool IsDirtyLaborAction(string actionId)
        {
            if (string.IsNullOrEmpty(actionId)) return false;
            return actionId.IndexOf("clean", StringComparison.OrdinalIgnoreCase) >= 0
                   || actionId.IndexOf("waste", StringComparison.OrdinalIgnoreCase) >= 0
                   || actionId.IndexOf("dig", StringComparison.OrdinalIgnoreCase) >= 0
                   || actionId.IndexOf("rubble", StringComparison.OrdinalIgnoreCase) >= 0
                   || actionId.IndexOf("excavat", StringComparison.OrdinalIgnoreCase) >= 0
                   || actionId.IndexOf("septic", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public float ApplyDirtyLaborMorale(Survivor politician, string actionId)
        {
            if (!TriesToDelegateTasks(politician) || !IsDirtyLaborAction(actionId)) return 0f;
            // Quest days still count; morale hit remains until Statesman.
            if (HasTheStatesman(politician)) return 0f;
            politician.Needs.Morale = Mathf.Max(0f, politician.Needs.Morale - SilverTongueLaborMoraleHit);
            return SilverTongueLaborMoraleHit;
        }

        /// <summary>One day of dirty jobs toward A Real Leader (14).</summary>
        public void RecordDirtyLaborDay(
            Survivor politician,
            bool didDirtyJob,
            int currentDay = 0)
        {
            if (politician == null || !politician.IsAlive) return;
            var state = GetOrCreate(politician.Id);
            SyncFromSurvivor(politician, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.ARealLeader, StringComparison.Ordinal))
                return;

            if (!didDirtyJob)
            {
                state.RealLeaderDirtyDays = 0;
                state.Progress = 0f;
                politician.QuestProgress = 0f;
                OnQuestProgress?.Invoke(politician, "real_leader_reset", 0);
                return;
            }

            state.RealLeaderDirtyDays++;
            state.Progress = state.RealLeaderDirtyDays;
            politician.QuestProgress = state.RealLeaderDirtyDays;
            OnQuestProgress?.Invoke(politician, "real_leader_days", state.RealLeaderDirtyDays);
            if (state.RealLeaderDirtyDays >= RealLeaderDirtyDaysRequired)
                CompleteQuestline(politician, currentDay);
        }

        /// <summary>Statesman: merge two warring factions into an Alliance via Radio.</summary>
        public bool CanMergeFactionsViaRadio(Survivor sv) => HasTheStatesman(sv);

        // ── #280 Tech Bro — Hard Reboot / Cybernetics ────────────────────

        /// <summary>AI quirk: wastes power playing offline games if unsupervised.</summary>
        public bool WastesPowerOnTablet(Survivor sv, bool supervised) =>
            !supervised
            && !HasCybernetics(sv)
            && (HasDelusional(sv)
                || string.Equals(sv?.ArchetypeId, TechBroId, StringComparison.Ordinal))
            && !GetOrCreate(sv.Id).TechTabletDead;

        public float GetTechBroPowerWasteWatts(Survivor sv, bool supervised) =>
            WastesPowerOnTablet(sv, supervised) ? TechBroPowerWasteWatts : 0f;

        /// <summary>EMP kills the tablet permanently (quest gate).</summary>
        public void RecordTabletDestroyedByEmp(Survivor tech, bool empHit, int currentDay = 0)
        {
            if (tech == null || !tech.IsAlive || !empHit) return;
            var state = GetOrCreate(tech.Id);
            state.TechTabletDead = true;
            OnQuestProgress?.Invoke(tech, "tablet_dead", 1);
        }

        /// <summary>Build a manual WaterPurifier from scrap after tablet dies.</summary>
        public void RecordManualWaterPurifierBuilt(
            Survivor tech,
            bool builtFromScrap,
            bool isManual,
            int currentDay = 0)
        {
            if (tech == null || !tech.IsAlive) return;
            if (!builtFromScrap || !isManual) return;
            var state = GetOrCreate(tech.Id);
            SyncFromSurvivor(tech, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheHardReboot, StringComparison.Ordinal))
                return;
            if (!state.TechTabletDead) return;

            state.Progress = 1f;
            tech.QuestProgress = 1f;
            OnQuestProgress?.Invoke(tech, "manual_purifier", 1);
            CompleteQuestline(tech, currentDay);
        }

        public bool CanCraftAutoTurrets(Survivor sv) => HasCybernetics(sv);

        // ── #281 News Anchor — Final Broadcast / Beacon of Truth ─────────

        /// <summary>Photogenic: high morale drain from bad hygiene.</summary>
        public float GetPhotogenicHygieneMoraleHit(Survivor sv, float hygiene01) =>
            HasPhotogenic(sv) && hygiene01 < 0.4f ? PhotogenicHygieneMoraleHit : 0f;

        public void ApplyPhotogenicHygieneMorale(Survivor sv, float hygiene01)
        {
            float d = GetPhotogenicHygieneMoraleHit(sv, hygiene01);
            if (d <= 0f || sv == null || !sv.IsAlive) return;
            sv.Needs.Morale = Mathf.Max(0f, sv.Needs.Morale - d);
        }

        /// <summary>AI quirk: constantly writes in JournalSystem (lore spam).</summary>
        public bool SpamsJournalEntries(Survivor sv) =>
            HasPhotogenic(sv)
            || string.Equals(sv?.ArchetypeId, NewsAnchorId, StringComparison.Ordinal)
            || HasBeaconOfTruth(sv);

        public int GetJournalEntriesPerDay(Survivor sv) =>
            SpamsJournalEntries(sv) ? 3 : 0;

        /// <summary>Beacon: global trade prices −30%.</summary>
        public float GetBeaconTradePriceMultiplier(IReadOnlyList<Survivor> survivors)
        {
            if (survivors == null) return 1f;
            for (int i = 0; i < survivors.Count; i++)
            {
                if (HasBeaconOfTruth(survivors[i]))
                    return BeaconTradePriceMult;
            }
            return 1f;
        }

        /// <summary>Broadcast the truth on the Radio Tower node.</summary>
        public void RecordFinalBroadcast(
            Survivor anchor,
            string nodeId,
            bool broadcastTruth,
            int currentDay = 0)
        {
            if (anchor == null || !anchor.IsAlive) return;
            if (!broadcastTruth) return;
            if (!string.Equals(nodeId, "the_radio_tower", StringComparison.Ordinal)
                && !string.IsNullOrEmpty(nodeId)
                && nodeId.IndexOf("radio", StringComparison.OrdinalIgnoreCase) < 0)
                return;
            var state = GetOrCreate(anchor.Id);
            SyncFromSurvivor(anchor, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheFinalBroadcast, StringComparison.Ordinal))
                return;

            state.Progress = 1f;
            anchor.QuestProgress = 1f;
            OnQuestProgress?.Invoke(anchor, "final_broadcast", 1);
            CompleteQuestline(anchor, currentDay);
        }

        // ── #282 Nomad — Putting Down Roots / Master Pathologist ─────────

        /// <summary>Agoraphile: loses morale when inside the bunker.</summary>
        public float GetAgoraphileBunkerMoraleHitPerDay(Survivor nomad) =>
            HasAgoraphile(nomad) ? AgoraphileBunkerMoraleHitPerDay : 0f;

        public void ApplyAgoraphileBunkerDay(Survivor nomad, bool spentDayInside)
        {
            if (!HasAgoraphile(nomad) || nomad == null || !nomad.IsAlive) return;
            if (!spentDayInside) return;
            nomad.Needs.Morale = Mathf.Max(0f, nomad.Needs.Morale - AgoraphileBunkerMoraleHitPerDay);
            var state = GetOrCreate(nomad.Id);
            state.NomadInsideDays++;
        }

        /// <summary>AI quirk: paces at hatch; leaves after 5 days inside.</summary>
        public bool PacesAtHatch(Survivor nomad) =>
            HasAgoraphile(nomad)
            || string.Equals(nomad?.ArchetypeId, NomadId, StringComparison.Ordinal);

        public bool ShouldLeaveBunkerOnOwn(Survivor nomad)
        {
            if (!PacesAtHatch(nomad) || HasMasterPathologist(nomad)) return false;
            return GetOrCreate(nomad.Id).NomadInsideDays >= NomadInsideDaysBeforeFlee;
        }

        public void RecordOutsideDay(Survivor nomad)
        {
            if (nomad == null) return;
            GetOrCreate(nomad.Id).NomadInsideDays = 0;
        }

        /// <summary>Master Pathologist: immune to weather; scavenge in lethal weather free.</summary>
        public bool IsImmuneToWeatherEffects(Survivor sv) => HasMasterPathologist(sv);

        public bool CanScavengeInLethalWeather(Survivor sv) => HasMasterPathologist(sv);

        /// <summary>Build and fully upgrade personal BedModule room.</summary>
        public void RecordPersonalBedModuleFullyUpgraded(
            Survivor nomad,
            bool isPersonalRoom,
            bool fullyUpgraded,
            int currentDay = 0)
        {
            if (nomad == null || !nomad.IsAlive) return;
            if (!isPersonalRoom || !fullyUpgraded) return;
            var state = GetOrCreate(nomad.Id);
            SyncFromSurvivor(nomad, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.PuttingDownRoots, StringComparison.Ordinal))
                return;

            state.Progress = 1f;
            nomad.QuestProgress = 1f;
            OnQuestProgress?.Invoke(nomad, "bed_module_upgraded", 1);
            CompleteQuestline(nomad, currentDay);
        }

        // ── #283 Exec — Golden Parachute / Monopolist ────────────────────

        /// <summary>Ruthless: +20% module efficiency, +35% wear.</summary>
        public float GetRuthlessModuleEfficiencyMultiplier(Survivor sv) =>
            HasRuthless(sv) ? RuthlessModuleEfficiencyMult : 1f;

        public float GetRuthlessModuleWearMultiplier(Survivor sv) =>
            HasRuthless(sv) ? RuthlessModuleWearMult : 1f;

        /// <summary>
        /// AI quirk: during fires, prioritize high-tier items over dying survivors.
        /// </summary>
        public bool PrioritizesLootOverLivesInFire(Survivor exec) =>
            HasRuthless(exec)
            || string.Equals(exec?.ArchetypeId, ExecId, StringComparison.Ordinal);

        /// <summary>Monopolist: buy out faction inventories → vassal tribute.</summary>
        public bool CanBuyOutFactionInventories(Survivor sv) => HasMonopolist(sv);

        /// <summary>Amass trade value toward Golden Parachute.</summary>
        public void RecordBunkerTradeValue(
            Survivor exec,
            float totalTradeValue,
            int currentDay = 0)
        {
            if (exec == null || !exec.IsAlive) return;
            var state = GetOrCreate(exec.Id);
            SyncFromSurvivor(exec, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheGoldenParachute, StringComparison.Ordinal))
                return;

            state.Progress = totalTradeValue;
            exec.QuestProgress = totalTradeValue;
            OnQuestProgress?.Invoke(exec, "trade_value", Mathf.RoundToInt(totalTradeValue));
            if (totalTradeValue >= GoldenParachuteTradeValue)
                CompleteQuestline(exec, currentDay);
        }
    }
}
