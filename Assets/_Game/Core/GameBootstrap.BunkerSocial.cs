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
            BunkerSocial.Banishment.OnBanishedReturned += rec =>
            {
                int day = TimeSystem != null ? TimeSystem.CurrentDay : 0;
                EventBus.Raise(new BanishedRaiderRaidEvent { RaiderId = rec.Id, Day = day });
                BunkerSocialNarrative.Raise("banished_returned", rec.Id, null, null);
            };
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
            Gossip.OnAffinityDecayed += (criminalId, targetId, amount) =>
            {
                if (BunkerSocial?.Affinity == null || amount <= 0f) return;
                BunkerSocial.Affinity.Adjust(criminalId, targetId, -amount);
            };

            Gossip.OnCrimeWitnessed += (witnessId, criminalId, crime) =>
                BunkerSocialNarrative.Raise("gossip_crime", witnessId, criminalId, crime);
            Gossip.OnRumorSpread += (fromId, toId, criminalId) =>
                BunkerSocialNarrative.Raise("gossip_spread", fromId, toId, criminalId);

            if (BunkerSocial == null) return;

            // Tribunal crime → a living witness (first other living survivor) starts the chain.
            BunkerSocial.Tribunal.OnTribunalStarted += (suspect, crimeId, severity) =>
            {
                if (suspect == null || string.IsNullOrEmpty(suspect.Id)) return;
                string witness = FindGossipWitness(suspect.Id);
                if (string.IsNullOrEmpty(witness)) return;
                int day = TimeSystem != null ? TimeSystem.CurrentDay : 1;
                RefreshGossipRoster();
                Gossip.WitnessCrime(witness, suspect.Id, crimeId ?? "crime", day);
            };

            // Feud sabotage is witnessed by the victim.
            BunkerSocial.Feuds.OnSabotageOccurred += (perp, victim, kind) =>
            {
                if (perp == null || victim == null) return;
                if (string.IsNullOrEmpty(perp.Id) || string.IsNullOrEmpty(victim.Id)) return;
                int day = TimeSystem != null ? TimeSystem.CurrentDay : 1;
                RefreshGossipRoster();
                Gossip.WitnessCrime(victim.Id, perp.Id, kind ?? "sabotage", day);
            };
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
            BunkerSocial.Romance.OnBecomeLovers += (a, b) => BunkerSocialNarrative.Raise("lovers", a?.Id, b?.Id);
            BunkerSocial.Romance.OnBreakup += (a, b) => BunkerSocialNarrative.Raise("breakup", a?.Id, b?.Id);
            BunkerSocial.Feuds.OnFeudStarted += (a, b) => BunkerSocialNarrative.Raise("feud", a?.Id, b?.Id);
            BunkerSocial.Mutiny.OnMutinyStarted += l => BunkerSocialNarrative.Raise("mutiny", l?.Id, null);
            BunkerSocial.Mutiny.OnMutinyResolved += r => BunkerSocialNarrative.Raise("mutiny_resolved", r.ToString(), null);
            BunkerSocial.Banishment.OnBanish += (sv, penalized) =>
                BunkerSocialNarrative.Raise("banish", sv?.Id, null, penalized.ToString());
            BunkerSocial.Brig.OnImprisoned += sv => BunkerSocialNarrative.Raise("imprisoned", sv?.Id, null);
            BunkerSocial.Brig.OnReleased += sv => BunkerSocialNarrative.Raise("released", sv?.Id, null);
            BunkerSocial.Pregnancy.OnPregnancyStarted += (p, partner) => BunkerSocialNarrative.Raise("pregnancy", p?.Id, partner?.Id);
            BunkerSocial.Pregnancy.OnChildBorn += p => BunkerSocialNarrative.Raise("child_born", p?.Id, null);
            BunkerSocial.Tribunal.OnVerdict += (sv, pun, match, mismatched) =>
                BunkerSocialNarrative.Raise("verdict", sv?.Id, pun.ToString(), match.ToString());
            BunkerSocial.BlackMarket.OnAllianceFormed += (a, b) => BunkerSocialNarrative.Raise("secret_alliance", a?.Id, b?.Id);
            BunkerSocial.BlackMarket.OnAllianceExposed += (a, b) => BunkerSocialNarrative.Raise("alliance_exposed", a, b);
            BunkerSocial.OnGriefMentalBreakApplied += (bereaved, breakId) =>
                BunkerSocialNarrative.Raise("grief_break", bereaved?.Id, null, breakId);
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
