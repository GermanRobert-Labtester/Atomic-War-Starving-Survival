using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.AI;
using AtomicWar._Game.AI.Actions;
using AtomicWar._Game.Crafting;
using AtomicWar._Game.Data;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Events;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Flashpoint;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Shelter.Modules;
using AtomicWar._Game.Simulation;
using AtomicWar._Game.UI;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.Core
{
    public partial class GameBootstrap
    {
        /// <summary>
        /// MISC-005: seeded stream for the smuggle comfort-item coin flip.
        /// </summary>
        private static readonly System.Random SmuggleRng =
            SeededRandom.CreateFixed("bunkersocial_smuggle");

        /// <summary>Prompts #469-#478 interpersonal &amp; leadership systems.</summary>
        public BunkerSocialDirector BunkerSocial { get; private set; }

        /// <summary>
        /// Build the social director and wire its host hooks. Called from
        /// InitFoundation once <see cref="Survivors"/>, <see cref="Inventory"/>
        /// and the combat perk stack exist. Registered for saving in
        /// FinishSystemRegistration.
        /// </summary>
        private void InitBunkerSocial()
        {
            // Share MentalBreakSystem's matrix (created earlier in InitFoundation):
            // it is the one EventRunner choices, mental-break drain and GriefKeepsakes
            // write to, and the only one SaveSystem captures. Without this the
            // director's relationship systems run on an orphan matrix that resets
            // to neutral on every load.
            BunkerSocial = new BunkerSocialDirector(MentalBreakSystem?.Affinity);
            BunkerSocial.SetNeedsSystem(NeedsSystem);
            // Prompt #839 — crime gossip chain (affinity rot as rumors spread).
            Gossip = new System_Gossip();
            WireGossipSystem();
            SubscribeBunkerSocialNarrative();

            // ── Host hooks (Inventory / Shelter / faction / AI edges) ──
            // #473 banishing a SerialKiller/Saboteur carries no morale penalty.
            BunkerSocial.IsSevereThreat = sv =>
                sv != null
                && (string.Equals(sv.ArchetypeId, PersonalQuestSystem.SerialKillerId, StringComparison.Ordinal)
                    || string.Equals(sv.ArchetypeId, PersonalQuestSystem.SaboteurId, StringComparison.Ordinal)
                    || sv.HasTrait("serial_killer") || sv.HasTrait("saboteur"));

            // #471 Leadership score = Charisma/Strength proxy.
            BunkerSocial.LeadershipScore = sv => sv == null ? 0f
                : sv.EffectiveScienceSkill * 0.5f
                  + sv.ProgressionCombatBonus
                  + Mathf.Clamp01(sv.Needs.Morale / 100f) * 0.5f;

            // #476 birth safety requires pristine medical supplies.
            BunkerSocial.HasPristineMedicalSupplies = HasPristineMedicalSuppliesCheck;

            // #471 surrendering control pays in rations.
            BunkerSocial.YieldBunkerControl = units => units > 0 && RemoveRationsFromInventory(units);

            // #478 smuggle drain → comfort item return.
            BunkerSocial.SmuggleDrain = resourceId =>
            {
                if (!string.IsNullOrEmpty(resourceId) && Inventory != null && Inventory.RemoveById(resourceId, 1))
                {
                    // MISC-005: seeded so the smuggled comfort item is the same on
                    // every replay of a save, not a wall-clock coin flip.
                    string comfort = (SmuggleRng.NextDouble() < 0.5) ? "comfort_alcohol" : "comfort_drugs";
                    BunkerSocialNarrative.Raise("smuggle", null, resourceId, comfort);
                    return comfort;
                }
                return null;
            };
            BunkerSocial.AvailableSmuggleResources = _ => AvailableInventoryItemIds();

            // #475 sabotage side-effect (meal contamination / tool hiding).
            BunkerSocial.SabotageWorkHandler = (perp, victim, kind) =>
            {
                BunkerSocialNarrative.Raise("feud_sabotage", perp?.Id, victim?.Id, kind);
                return true;
            };

            // #474 returned banished survivor → hatch-breaching Raider Boss.
            Action<BanishedRecord> onBanishedReturned = rec =>
            {
                int day = TimeSystem != null ? TimeSystem.CurrentDay : 0;
                EventBus.Raise(new BanishedRaiderRaidEvent { RaiderId = rec.Id, Day = day });
                BunkerSocialNarrative.Raise("banished_returned", rec.Id, null, null);
            };
            BunkerSocial.Banishment.OnBanishedReturned += onBanishedReturned;
            _subscriptions.Track(() => BunkerSocial.Banishment.OnBanishedReturned -= onBanishedReturned);
        }

        /// <summary>
        /// Prompt #839 — roster sync, affinity decay into InterpersonalAffinity,
        /// and crime-witness hooks from tribunal / feud sabotage.
        /// </summary>
        private void WireGossipSystem()
        {
            if (Gossip == null) return;

            RefreshGossipRoster();

            // Rumor rot → shared affinity matrix (negative).
            Action<string, string, float> onAffinityDecayed = (criminalId, targetId, amount) =>
            {
                if (BunkerSocial?.Affinity == null || amount <= 0f) return;
                BunkerSocial.Affinity.Adjust(criminalId, targetId, -amount);
            };
            Gossip.OnAffinityDecayed += onAffinityDecayed;
            _subscriptions.Track(() => Gossip.OnAffinityDecayed -= onAffinityDecayed);

            Action<string, string, string> onCrimeWitnessed = (witnessId, criminalId, crime) =>
                BunkerSocialNarrative.Raise("gossip_crime", witnessId, criminalId, crime);
            Gossip.OnCrimeWitnessed += onCrimeWitnessed;
            _subscriptions.Track(() => Gossip.OnCrimeWitnessed -= onCrimeWitnessed);

            Action<string, string, string> onRumorSpread = (fromId, toId, criminalId) =>
                BunkerSocialNarrative.Raise("gossip_spread", fromId, toId, criminalId);
            Gossip.OnRumorSpread += onRumorSpread;
            _subscriptions.Track(() => Gossip.OnRumorSpread -= onRumorSpread);

            if (BunkerSocial == null) return;

            // Tribunal crime → a living witness (first other living survivor) starts the chain.
            Action<Survivor, string, BunkerCrimeSeverity> onTribunalStarted = (suspect, crimeId, severity) =>
            {
                if (suspect == null || string.IsNullOrEmpty(suspect.Id)) return;
                string witness = FindGossipWitness(suspect.Id);
                if (string.IsNullOrEmpty(witness)) return;
                int day = TimeSystem != null ? TimeSystem.CurrentDay : 1;
                RefreshGossipRoster();
                Gossip.WitnessCrime(witness, suspect.Id, crimeId ?? "crime", day);
            };
            BunkerSocial.Tribunal.OnTribunalStarted += onTribunalStarted;
            _subscriptions.Track(() => BunkerSocial.Tribunal.OnTribunalStarted -= onTribunalStarted);

            // Feud sabotage is witnessed by the victim.
            Action<Survivor, Survivor, string> onSabotageOccurred = (perp, victim, kind) =>
            {
                if (perp == null || victim == null) return;
                if (string.IsNullOrEmpty(perp.Id) || string.IsNullOrEmpty(victim.Id)) return;
                int day = TimeSystem != null ? TimeSystem.CurrentDay : 1;
                RefreshGossipRoster();
                Gossip.WitnessCrime(victim.Id, perp.Id, kind ?? "sabotage", day);
            };
            BunkerSocial.Feuds.OnSabotageOccurred += onSabotageOccurred;
            _subscriptions.Track(() => BunkerSocial.Feuds.OnSabotageOccurred -= onSabotageOccurred);
        }

        private void RefreshGossipRoster()
        {
            if (Gossip == null || Survivors == null) return;
            var ids = new List<string>();
            for (int i = 0; i < Survivors.Count; i++)
            {
                var sv = Survivors[i];
                if (sv == null || string.IsNullOrEmpty(sv.Id)) continue;
                // Include dead in roster so mid-run deaths don't break rumor identity,
                // but prefer living for spread (TickDay still walks all ids).
                ids.Add(sv.Id);
            }
            Gossip.SetSurvivorRoster(ids);
        }

        private string FindGossipWitness(string criminalId)
        {
            if (Survivors == null) return null;
            for (int i = 0; i < Survivors.Count; i++)
            {
                var sv = Survivors[i];
                if (sv == null || !sv.IsAlive || string.IsNullOrEmpty(sv.Id)) continue;
                if (string.Equals(sv.Id, criminalId, StringComparison.Ordinal)) continue;
                return sv.Id;
            }
            return null;
        }

        /// <summary>
        /// Public API: witness a crime for the gossip system (UI / events / tests).
        /// </summary>
        public void ReportCrimeWitness(string witnessId, string criminalId, string crimeType)
        {
            if (Gossip == null) return;
            RefreshGossipRoster();
            int day = TimeSystem != null ? TimeSystem.CurrentDay : 1;
            Gossip.WitnessCrime(witnessId, criminalId, crimeType, day);
        }

        /// <summary>Surface every major social state-change onto the EventBus for UI/journal.</summary>
        private void SubscribeBunkerSocialNarrative()
        {
            if (BunkerSocial == null) return;

            Action<Survivor, Survivor> onBecomeLovers = (a, b) => BunkerSocialNarrative.Raise("lovers", a?.Id, b?.Id);
            BunkerSocial.Romance.OnBecomeLovers += onBecomeLovers;
            _subscriptions.Track(() => BunkerSocial.Romance.OnBecomeLovers -= onBecomeLovers);

            Action<Survivor, Survivor> onRomanceBreakup = (a, b) => BunkerSocialNarrative.Raise("breakup", a?.Id, b?.Id);
            BunkerSocial.Romance.OnBreakup += onRomanceBreakup;
            _subscriptions.Track(() => BunkerSocial.Romance.OnBreakup -= onRomanceBreakup);

            Action<Survivor, Survivor> onFeudStarted = (a, b) => BunkerSocialNarrative.Raise("feud", a?.Id, b?.Id);
            BunkerSocial.Feuds.OnFeudStarted += onFeudStarted;
            _subscriptions.Track(() => BunkerSocial.Feuds.OnFeudStarted -= onFeudStarted);

            Action<Survivor> onMutinyStartedNarrative = l => BunkerSocialNarrative.Raise("mutiny", l?.Id, null);
            BunkerSocial.Mutiny.OnMutinyStarted += onMutinyStartedNarrative;
            _subscriptions.Track(() => BunkerSocial.Mutiny.OnMutinyStarted -= onMutinyStartedNarrative);

            // Audit H-6g: present the standoff choices so the player can actually resolve it.
            BunkerSocial.Mutiny.OnMutinyStarted += HandleMutinyStarted;
            _subscriptions.Track(() => BunkerSocial.Mutiny.OnMutinyStarted -= HandleMutinyStarted);

            Action<MutinyResolution> onMutinyResolved = r => BunkerSocialNarrative.Raise("mutiny_resolved", r.ToString(), null);
            BunkerSocial.Mutiny.OnMutinyResolved += onMutinyResolved;
            _subscriptions.Track(() => BunkerSocial.Mutiny.OnMutinyResolved -= onMutinyResolved);

            Action<Survivor, bool> onBanish = (sv, penalized) =>
                BunkerSocialNarrative.Raise("banish", sv?.Id, null, penalized.ToString());
            BunkerSocial.Banishment.OnBanish += onBanish;
            _subscriptions.Track(() => BunkerSocial.Banishment.OnBanish -= onBanish);

            Action<Survivor> onImprisoned = sv => BunkerSocialNarrative.Raise("imprisoned", sv?.Id, null);
            BunkerSocial.Brig.OnImprisoned += onImprisoned;
            _subscriptions.Track(() => BunkerSocial.Brig.OnImprisoned -= onImprisoned);

            Action<Survivor> onBrigReleased = sv => BunkerSocialNarrative.Raise("released", sv?.Id, null);
            BunkerSocial.Brig.OnReleased += onBrigReleased;
            _subscriptions.Track(() => BunkerSocial.Brig.OnReleased -= onBrigReleased);

            Action<Survivor, Survivor> onPregnancyStarted = (p, partner) => BunkerSocialNarrative.Raise("pregnancy", p?.Id, partner?.Id);
            BunkerSocial.Pregnancy.OnPregnancyStarted += onPregnancyStarted;
            _subscriptions.Track(() => BunkerSocial.Pregnancy.OnPregnancyStarted -= onPregnancyStarted);

            Action<Survivor> onChildBorn = p => BunkerSocialNarrative.Raise("child_born", p?.Id, null);
            BunkerSocial.Pregnancy.OnChildBorn += onChildBorn;
            _subscriptions.Track(() => BunkerSocial.Pregnancy.OnChildBorn -= onChildBorn);

            Action<Survivor, BunkerPunishment, PunishmentMatch, bool> onVerdict = (sv, pun, match, mismatched) =>
                BunkerSocialNarrative.Raise("verdict", sv?.Id, pun.ToString(), match.ToString());
            BunkerSocial.Tribunal.OnVerdict += onVerdict;
            _subscriptions.Track(() => BunkerSocial.Tribunal.OnVerdict -= onVerdict);

            Action<Survivor, Survivor> onAllianceFormed = (a, b) => BunkerSocialNarrative.Raise("secret_alliance", a?.Id, b?.Id);
            BunkerSocial.BlackMarket.OnAllianceFormed += onAllianceFormed;
            _subscriptions.Track(() => BunkerSocial.BlackMarket.OnAllianceFormed -= onAllianceFormed);

            Action<string, string> onAllianceExposed = (a, b) => BunkerSocialNarrative.Raise("alliance_exposed", a, b);
            BunkerSocial.BlackMarket.OnAllianceExposed += onAllianceExposed;
            _subscriptions.Track(() => BunkerSocial.BlackMarket.OnAllianceExposed -= onAllianceExposed);

            Action<Survivor, string> onGriefMentalBreakApplied = (bereaved, breakId) =>
                BunkerSocialNarrative.Raise("grief_break", bereaved?.Id, null, breakId);
            BunkerSocial.OnGriefMentalBreakApplied += onGriefMentalBreakApplied;
            _subscriptions.Track(() => BunkerSocial.OnGriefMentalBreakApplied -= onGriefMentalBreakApplied);
        }

        private bool HasPristineMedicalSuppliesCheck(Survivor patient)
        {
            if (Inventory == null) return false;
            return Inventory.CountById("pristine_medical_supplies") >= 1
                || Inventory.CountById("medical_supplies") >= 1
                || Inventory.CountById("medical_kit") >= 1;
        }

        private bool RemoveRationsFromInventory(int units)
        {
            if (Inventory == null || units <= 0) return false;
            // Try "ration" first, then any food-tagged item.
            if (Inventory.RemoveById("ration", units)) return true;
            return Inventory.RemoveById("canned_food", units);
        }

        private IReadOnlyList<string> AvailableInventoryItemIds()
        {
            var ids = new List<string>();
            if (Inventory == null || Inventory.Slots == null) return ids;
            for (int i = 0; i < Inventory.Slots.Count && ids.Count < 12; i++)
            {
                var slot = Inventory.Slots[i];
                if (slot?.Item != null && !string.IsNullOrEmpty(slot.Item.id) && slot.Amount > 0)
                    ids.Add(slot.Item.id);
            }
            return ids;
        }
    }

    /// <summary>A Raider Boss (banished survivor) leads a hatch breach bypassing perimeter traps (#474).</summary>
    public sealed class BanishedRaiderRaidEvent
    {
        public string RaiderId;
        public int Day;
    }

    /// <summary>A lightweight social narrative signal for the journal / UI / audio layer.</summary>
    public sealed class BunkerSocialNarrativeEvent
    {
        public string Kind;      // smoke: lovers, breakup, mutiny, banish, etc.
        public string A;
        public string B;
        public string Note;
    }

    /// <summary>Helper for raising social narrative events onto the EventBus.</summary>
    public static class BunkerSocialNarrative
    {
        public static void Raise(string kind, string a, string b, string note = null)
        {
            EventBus.Raise(new BunkerSocialNarrativeEvent { Kind = kind, A = a, B = b, Note = note });
        }
    }
}
