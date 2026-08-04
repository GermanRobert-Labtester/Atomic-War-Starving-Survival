using System.Collections.Generic;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Events;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Survivors;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Prompt #15 — Deserter's Stand: a quiet narrative discovery at a civil-war
    /// checkpoint. No combat. The scavenger finds weapons among the dead and
    /// carries home a trauma that does not wash off.
    /// Distinct from combat toll encounter <c>enc_deserters</c>.
    /// </summary>
    public static class DesertersStandSystem
    {
        public const string EncounterId = "enc_deserters_stand";
        public const string CombatDesertersEncounterId = "enc_deserters";
        public const string TraumaMassacreId = "trauma_massacre";
        public const string ServiceRifleItemId = "service_rifle";

        /// <summary>Morale crash on witnessing the mutual kill over food.</summary>
        public const float MoraleCrash = 35f;

        /// <summary>How many service rifles appear among the dead.</summary>
        public const int WeaponLootCount = 2;

        public const string LogMessage =
            "Checkpoint empty. They shot each other over a tin of meat. The rifles still work.";

        /// <summary>
        /// Factory for the narrative discovery encounter (force-on-arrival via map flag).
        /// </summary>
        public static EncounterSO CreateEncounter()
        {
            var enc = ScriptableObject.CreateInstance<EncounterSO>();
            enc.id = EncounterId;
            enc.title = "Deserter's Stand";
            enc.description =
                "A checkpoint. Sandbags still stacked. Two uniforms, neither side winning. " +
                "They killed each other over a tin of meat. The rifles are clean. " +
                "You take what they no longer need.";
            enc.category = EncounterCategory.Discovery;
            enc.baseWeight = 1f;
            enc.minDangerLevel = 0f;
            enc.requiredLocationId = string.Empty; // matched via MapNode.HasDeserterStand
            enc.forceOnArrival = true;
            enc.enableAutoResolution = false; // no combat engage/flee
            enc.stealthWeightMultiplier = 1f;
            enc.speedWeightMultiplier = 1f;
            enc.choices = new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = "gather_the_weapons",
                    Text = "Gather the rifles. Leave the tin.",
                    MoraleDelta = -MoraleCrash
                },
                new EventChoice
                {
                    ChoiceId = "walk_away",
                    Text = "Walk away. Someone else can have the guns.",
                    MoraleDelta = -MoraleCrash * 0.5f
                }
            };
            return enc;
        }

        /// <summary>High-tier weapon left at the stand.</summary>
        public static ItemDefinition CreateServiceRifleDefinition()
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = ServiceRifleItemId;
            item.displayName = "Service Rifle";
            item.description =
                "Civil-war issue. The stock is scored with a name you do not know.";
            item.type = ItemType.Weapon;
            item.stackMax = 1;
            item.weight = 3.5f;
            item.tradeValue = 40f;
            item.durability = 85f;
            return item;
        }

        public static bool IsDesertersStandEncounter(EncounterSO encounter)
        {
            return encounter != null
                && string.Equals(encounter.id, EncounterId, System.StringComparison.Ordinal);
        }

        /// <summary>
        /// Apply trauma + morale + weapon loot. Call when the narrative beat resolves.
        /// Weapons are always taken on "gather"; walk_away skips weapons but keeps trauma.
        /// </summary>
        public static void Apply(
            ExpeditionState exp,
            Survivor scavenger,
            ItemDefinition serviceRifle,
            string choiceId = "gather_the_weapons")
        {
            if (scavenger == null || !scavenger.IsAlive) return;

            // Trauma: mutual kill over food — permanent moral scar
            if (scavenger.Traumas == null)
                scavenger.Traumas = new List<string>();
            if (!scavenger.HasTrauma(TraumaMassacreId))
                scavenger.Traumas.Add(TraumaMassacreId);

            // Morale already applied via choice MoraleDelta in ResolveEncounter;
            // ensure a floor crash if choice was null / skipped.
            if (scavenger.Needs != null && choiceId == null)
            {
                scavenger.Needs.Morale = Mathf.Clamp(
                    scavenger.Needs.Morale - MoraleCrash, 0f, 100f);
            }

            bool takeWeapons = string.IsNullOrEmpty(choiceId)
                || string.Equals(choiceId, "gather_the_weapons", System.StringComparison.OrdinalIgnoreCase);

            if (takeWeapons && exp != null && serviceRifle != null)
            {
                for (int i = 0; i < WeaponLootCount; i++)
                    exp.TryAddLoot(serviceRifle);
            }
        }

        /// <summary>True when the proc-gen node hosts the narrative stand.</summary>
        public static bool NodeHasStand(GeneratedMap map, string nodeId)
        {
            if (map == null || string.IsNullOrEmpty(nodeId)) return false;
            var n = map.GetNode(nodeId);
            return n != null && n.HasDeserterStand && !n.IsShelter;
        }
    }
}
