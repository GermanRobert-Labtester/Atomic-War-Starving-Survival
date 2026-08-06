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
            BunkerSocial = new BunkerSocialDirector();
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
                    string comfort = (UnityEngine.Random.value < 0.5f) ? "comfort_alcohol" : "comfort_drugs";
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

        /// <summary>Surface every major social state-change onto the EventBus for UI/journal.</summary>
        private void SubscribeBunkerSocialNarrative()
        {
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
